using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Catalog.Domain.Helpers
{
    public class ExpressionBinding : IExpressionBinding
    {
        public Expression<Func<T, bool>> GenericExpressionBindingList<T>(List<Expression<Func<T, bool>>> expression, ExpressionJoint joint = ExpressionJoint.And) where T : class
        {
            Expression<Func<T, bool>> finalExpression = null;

            foreach (var filter in expression)
            {
                var value = filter;

                if (finalExpression == null)
                    finalExpression = filter;
                else if (joint == ExpressionJoint.And)
                    finalExpression = ExpressionExtensions.And(finalExpression, filter);
                else
                    finalExpression = ExpressionExtensions.Or(finalExpression, filter);
            }
            return finalExpression;
        }

        public Expression<Func<T, bool>> GenericExpressionBindingListProduct<T>(List<Expression<Func<T, bool>>> expression, ExpressionJoint joint = ExpressionJoint.And) where T : class
        {
            Expression<Func<T, bool>> finalExpression = null;
            Expression<Func<T, bool>> nullExpression = null;
            nullExpression = CreatePredicate<T>("IsActive", true, FilterOperatorEnum.IsEqualTo);
            expression.Add(nullExpression);
            foreach (var filter in expression)
            {
                var value = filter;

                if (finalExpression == null)
                    finalExpression = filter;
                else if (joint == ExpressionJoint.And)
                    finalExpression = ExpressionExtensions.And(finalExpression, filter);
                else
                    finalExpression = ExpressionExtensions.Or(finalExpression, filter);
            }
            return finalExpression;
        }
        public Expression<Func<T, bool>> GenericExpressionBindingListProductSeller<T>(List<Expression<Func<T, bool>>> expression, ExpressionJoint joint = ExpressionJoint.And) where T : class
        {
            Expression<Func<T, bool>> finalExpression = null;
            Expression<Func<T, bool>> nullExpression = null;
            Expression<Func<T, bool>> nullExpressionforStock = null;
            Expression<Func<T, bool>> nullExpressionforSalePrice = null;

            nullExpression = CreatePredicate<T>("IsActive", true, FilterOperatorEnum.IsEqualTo);
            nullExpressionforStock = CreatePredicate<T>("StockCount", 0, FilterOperatorEnum.GreaterThan);
            nullExpressionforSalePrice = CreatePredicate<T>("SalePrice", Convert.ToDecimal(1), FilterOperatorEnum.GreaterThanOrEqual);


            expression.Add(nullExpression);
            expression.Add(nullExpressionforStock);
            expression.Add(nullExpressionforSalePrice);

            foreach (var filter in expression)
            {
                var value = filter;

                if (finalExpression == null)
                    finalExpression = filter;
                else if (joint == ExpressionJoint.And)
                    finalExpression = ExpressionExtensions.And(finalExpression, filter);
                else
                    finalExpression = ExpressionExtensions.Or(finalExpression, filter);
            }
            return finalExpression;
        }
        public Expression<Func<T, bool>> GenericExpressionBinding<T>(List<FilterModel> filterModel, ExpressionJoint joint = ExpressionJoint.And, FilterOperatorEnum filterOperatorEnum = FilterOperatorEnum.IsEqualTo) where T : class
        {
            Expression<Func<T, bool>> finalExpression = null;

            var type = typeof(T);
            var innerList = new List<object>();
            //foreach (var prop in type.GetProperties())
            //{

            //    if (typeof(Entity).IsAssignableFrom(prop.PropertyType))
            //        innerList.Add(prop);
            //}

            foreach (var filter in filterModel)
            {
                var value = filter.Id;

                Expression<Func<T, bool>> predicate;

                if (filter.FilterField == "Search")
                {
                    var list = CreatePredicateSearch<T>(filter.FilterField, value, filterOperatorEnum);

                    for (int i = 0; i < list.Count - 1; i++)
                    {
                        if (finalExpression == null)
                            finalExpression = list[0];
                        finalExpression = ExpressionExtensions.Or(finalExpression, list[i + 1]);
                    }
                    return finalExpression;
                }
                else
                {
                    if (filter.FilterField.Split('-')[0] == ProductFilterEnum.Attribute.ToString())
                    {
                        filter.FilterField = ProductFilterEnum.AttributeValueId.ToString();
                    }
                    predicate = CreatePredicate<T>(filter.FilterField, value, filterOperatorEnum);
                }

                if (finalExpression == null)
                    finalExpression = predicate;
                else if (joint == ExpressionJoint.And)
                    finalExpression = ExpressionExtensions.And(finalExpression, predicate);
                else
                    finalExpression = ExpressionExtensions.Or(finalExpression, predicate);
            }

            return finalExpression;
        }

        private static Expression<Func<T, bool>> CreatePredicate<T>(string field, object searchValue, FilterOperatorEnum filterOperator) where T : class
        {
            var xType = typeof(T);
            var x = Expression.Parameter(xType, "type");

            var column = xType.GetProperties().FirstOrDefault(p => p.Name.ToLowerInvariant() == field.ToLowerInvariant());

            Expression body = null;

            switch (filterOperator)
            {
                case FilterOperatorEnum.IsEqualTo:
                    body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.Equal(
                    Expression.PropertyOrField(x, field),
                    Expression.Constant(searchValue));
                    break;
                case FilterOperatorEnum.GreaterThan:
                    body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.GreaterThan(
                    Expression.PropertyOrField(x, field),
                    Expression.Constant(searchValue));
                    break;
                case FilterOperatorEnum.GreaterThanOrEqual:
                    body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.GreaterThanOrEqual(
                    Expression.PropertyOrField(x, field),
                    Expression.Constant(searchValue));
                    break;
                case FilterOperatorEnum.IsNotEqualTo:
                    body = column == null
                 ? (Expression)Expression.Constant(true)
                 : Expression.NotEqual(
                     Expression.PropertyOrField(x, field),
                     Expression.Constant(searchValue));
                    break;
                case FilterOperatorEnum.StartsWith:
                    body = column == null
                 ? (Expression)Expression.Constant(true)
                 : Expression.Call(Expression.Property(x, column.Name), typeof(string).GetMethod("StartsWith", new[] { typeof(string) }), Expression.Constant(searchValue, typeof(string)));
                    break;
                case FilterOperatorEnum.Contains:
                    body = column == null
                 ? (Expression)Expression.Constant(true)
                 : Expression.Call(Expression.Property(x, column.Name), typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(searchValue, typeof(string)), Expression.Constant(StringComparison.InvariantCultureIgnoreCase));
                    break;
                case FilterOperatorEnum.DoesNotContain:
                    body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.Not(Expression.Constant(searchValue, typeof(string)), typeof(string).GetMethod("Contains", new[] { typeof(string) }));
                    break;
                case FilterOperatorEnum.EndsWith:
                    body = column == null
                 ? (Expression)Expression.Constant(true)
                 : Expression.Call(Expression.Property(x, column.Name), typeof(string).GetMethod("EndsWith", new[] { typeof(string) }), Expression.Constant(searchValue, typeof(string)));
                    break;
                case FilterOperatorEnum.IsNull:
                    body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.Equal(
                    Expression.PropertyOrField(x, field),
                    Expression.Constant(null, typeof(object)));
                    break;
                case FilterOperatorEnum.IsNotNull:
                    body = column == null
                ? (Expression)Expression.Constant(true)
                : Expression.NotEqual(
                    Expression.PropertyOrField(x, field),
                    Expression.Constant(null, typeof(object)));
                    break;
                case FilterOperatorEnum.IsEmpty:
                    body = column == null
                 ? (Expression)Expression.Constant(true)
                 : Expression.Equal(
                     Expression.PropertyOrField(x, field),
                     Expression.Constant(string.Empty, typeof(object)));
                    break;
                case FilterOperatorEnum.IsNotEmpty:
                    body = column == null
              ? (Expression)Expression.Constant(true)
              : Expression.NotEqual(
                  Expression.PropertyOrField(x, field),
                  Expression.Constant(string.Empty, typeof(object)));
                    break;
                case FilterOperatorEnum.HasNoValue:
                    body = column == null
               ? (Expression)Expression.Constant(true)
               : Expression.Equal(
                   Expression.PropertyOrField(x, field),
                   Expression.Constant(null, typeof(object)));
                    break;
                case FilterOperatorEnum.HasValue:
                    body = column == null
              ? (Expression)Expression.Constant(true)
              : Expression.Equal(
                  Expression.PropertyOrField(x, field),
                  Expression.Constant(null, typeof(object)));
                    break;

                case FilterOperatorEnum.Between:
                    body = column == null
               ? (Expression)Expression.Constant(true)
                : Expression.And(Expression.LessThanOrEqual(Expression.PropertyOrField(x, field), Expression.Constant(Convert.ToDecimal(searchValue.ToString().Split(",")[1]), typeof(decimal))),
                                 Expression.GreaterThanOrEqual(Expression.PropertyOrField(x, field), Expression.Constant(Convert.ToDecimal(searchValue.ToString().Split(",")[0]), typeof(decimal))));
                    break;

                case FilterOperatorEnum.IsEqualToNotNullGuid:
                    body = column == null
              ? (Expression)Expression.Constant(true)
              : Expression.Equal(Expression.PropertyOrField(x, field), Expression.Constant(new Guid(searchValue.ToString()), typeof(Guid)));
                    break;
                case FilterOperatorEnum.IsEqualToGuid:
                    body = column == null
              ? (Expression)Expression.Constant(true)
              : Expression.Equal(Expression.PropertyOrField(x, field), Expression.Constant(new Guid(searchValue.ToString()), typeof(Guid?)));
                    break;
                default:
                    break;
            }

            return Expression.Lambda<Func<T, bool>>(body, x);
        }

        private List<Expression<Func<T, bool>>> CreatePredicateSearch<T>(string field, object searchValue, FilterOperatorEnum filterOperator) where T : class
        {
            var xType = typeof(T);
            var x = Expression.Parameter(xType, "type");

            List<Expression<Func<T, bool>>> bodyList = new List<Expression<Func<T, bool>>>();

            Expression<Func<T, bool>> bodyName = Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Property(x, "Name"), typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(searchValue, typeof(string))), x);
            bodyList.Add(bodyName);

            Expression<Func<T, bool>> bodyDesc = Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Property(x, "Description"), typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(searchValue, typeof(string))), x);
            bodyList.Add(bodyDesc);

            Expression<Func<T, bool>> bodyDis = Expression.Lambda<Func<T, bool>>(Expression.Call(Expression.Property(x, "DisplayName"), typeof(string).GetMethod("Contains", new[] { typeof(string) }), Expression.Constant(searchValue, typeof(string))), x);
            bodyList.Add(bodyDis);

            return bodyList;
        }
    }
}