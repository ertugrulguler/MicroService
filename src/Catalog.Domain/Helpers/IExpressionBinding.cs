using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Catalog.Domain.Helpers
{
    public interface IExpressionBinding
    {
        Expression<Func<T, bool>> GenericExpressionBinding<T>(List<FilterModel> filterModel, ExpressionJoint joint = ExpressionJoint.And, FilterOperatorEnum filterOperatorEnum = FilterOperatorEnum.IsEqualTo) where T : class;
        Expression<Func<T, bool>> GenericExpressionBindingList<T>(List<Expression<Func<T, bool>>> expression, ExpressionJoint joint = ExpressionJoint.And) where T : class;
        Expression<Func<T, bool>> GenericExpressionBindingListProduct<T>(List<Expression<Func<T, bool>>> expression, ExpressionJoint joint = ExpressionJoint.And) where T : class;
        Expression<Func<T, bool>> GenericExpressionBindingListProductSeller<T>(List<Expression<Func<T, bool>>> expression, ExpressionJoint joint = ExpressionJoint.And) where T : class;

    }
}