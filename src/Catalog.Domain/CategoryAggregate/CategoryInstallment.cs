using Catalog.Domain.Entities;
using System;

namespace Catalog.Domain.CategoryAggregate
{
    public class CategoryInstallment : Entity
    {
        public Guid CategoryId { get; set; }
        public int MaxInstallmentCount { get; set; }
        public decimal? MinPrice { get; set; }
        public int? NewMaxInstallmentCount { get; set; }



        public CategoryInstallment(Guid categoryId,
            int maxInstallmentCount,
            decimal? minPrice,
            int? newMaxInstallmentCount)

        {
            CategoryId = categoryId;
            MaxInstallmentCount = maxInstallmentCount;
            MinPrice = minPrice;
            NewMaxInstallmentCount = newMaxInstallmentCount;
        }


        public void SetCategoryInstallment(int maxInstallmentCount, decimal? minPrice, int? newMaxInstallmentCount)
        {
            MaxInstallmentCount = maxInstallmentCount;
            MinPrice = minPrice;
            NewMaxInstallmentCount = newMaxInstallmentCount;
        }


    }


}