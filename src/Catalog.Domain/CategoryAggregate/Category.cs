using Catalog.Domain.Entities;
using Catalog.Domain.Enums;
using Catalog.Domain.ProductAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Domain.CategoryAggregate
{
    public class Category : Entity
    {
        public Guid? ParentId { get; set; }
        public string Name { get; private set; }
        public string SeoName { get; private set; }

        public string DisplayName { get; private set; }
        public string Code { get; private set; }
        public int DisplayOrder { get; private set; }
        public string Description { get; private set; }
        public Guid? SuggestedMainId { get; set; }
        public CategoryTypeEnum Type { get; set; }
        public bool HasAll { get; set; }
        public bool IsRequiredIdNumber { get; set; }
        public string LeafPath { get; set; }
        public bool HasProduct { get; set; }
        public bool? IsReturnable { get; set; }
        public int ProductFilterOrder { get; set; }

        private List<CategoryAttribute> _categoryAttributes;
        public IReadOnlyCollection<CategoryAttribute> CategoryAttributes => _categoryAttributes;

        private readonly List<ProductCategory> _productCategories;
        public IReadOnlyCollection<ProductCategory> ProductCategories => _productCategories;
        public CategoryImage CategoryImage { get; protected set; }

        public List<Category> SubCategories { get; set; }

        protected Category()
        {
            _categoryAttributes = new List<CategoryAttribute>();
            _productCategories = new List<ProductCategory>();
            SubCategories = new List<Category>();

        }

        public Category(Guid? parentId,
            string name,
            string displayName,
            string code,
            int displayOrder,
            string description, CategoryTypeEnum type, Guid? suggestedMainId, bool hasAll, bool isRequiredIdNumber, string leafPath, bool hasProduct, string seoName) : this()
        {
            ParentId = parentId;
            Name = name;
            DisplayName = displayName;
            Code = code;
            DisplayOrder = displayOrder;
            Description = description;
            Type = type;
            SuggestedMainId = suggestedMainId;
            HasAll = hasAll;
            IsRequiredIdNumber = isRequiredIdNumber;
            LeafPath = leafPath;
            HasProduct = hasProduct;
            SeoName = seoName;
        }

        public Category(Guid id,
                     string code,
                     string seoName,
                     string name,
                     string displayName,
                     Guid? parentId,
                     bool isActive,
                     bool hasProduct = false,
                     bool isRequiredIdNumber = false,
                     int type = 0,
                     bool hasAll = false,
                     int displayOrder = 1,
                     bool isReturnable = true
                     ) : this()
        {
            Id = id;
            Code = code;
            Name = name;
            DisplayName = displayName;
            ParentId = parentId;
            HasProduct = hasProduct;
            IsRequiredIdNumber = isRequiredIdNumber;
            Type = (CategoryTypeEnum)type;
            HasAll = hasAll;
            IsActive = isActive;
            DisplayOrder = displayOrder;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            SeoName = seoName;
            IsReturnable = isReturnable;
        }

        public void setCategory(Guid? parentId, string name, string displayName, string code, int displayOrder, string description, bool isActive)
        {
            ParentId = parentId;
            Name = name;
            DisplayName = displayName;
            Code = code;
            DisplayOrder = displayOrder;
            Description = description;
            IsActive = isActive;
        }
        public void setCategoryWithoutParentId(string name, string displayName, string code, int displayOrder, string description, bool isActive, bool hasProduct)
        {
            Name = name;
            DisplayName = displayName;
            Code = code;
            DisplayOrder = displayOrder;
            Description = description;
            IsActive = isActive;
            HasProduct = hasProduct;
        }
        public void SetImage(string name, string url, string description)
        {
            CategoryImage = new CategoryImage(Id, name, url, description);
        }

        //TODO: Bunu yapmamıza gerek yok çünkü biz araya mapping tablosu atıyoruz Order -> OrderItem ilişkimiz yok gerek kalmıyor aslında.
        public CategoryAttribute VerifyOrAddCategoryAttribute(Guid attributeId, bool isRequired)
        {
            var existingAttribute = _categoryAttributes.SingleOrDefault(x => x.AttributeId == attributeId);

            if (existingAttribute != null) //TODO: Event throw
                return existingAttribute;

            var categoryAttribute = new CategoryAttribute(Id, attributeId, isRequired, false, false);

            _categoryAttributes.Add(categoryAttribute);

            return categoryAttribute;
        }

        public void GetActiveCategoryAttributes()
        {
            _categoryAttributes = _categoryAttributes.Where(s => s.IsActive).ToList();
        }

        public void DeleteCategoryAttributes(Guid? categoryId)
        {
            var substractedCategoryAttributes = _categoryAttributes.Where(x => x.CategoryId == categoryId);

            foreach (var item in substractedCategoryAttributes)
            {
                item.SetCategoryAttribute(item.CategoryId, item.AttributeId, item.IsRequired, item.IsVariantable, item.IsListed, false);
            }
        }

        public void UpdateCategoryAttributes(List<Guid> attributeIds, Guid categoryId)
        {
            foreach (var attributeId in attributeIds)
            {
                var existingAttribute = _categoryAttributes.Find(x => x.AttributeId == attributeId);

                if (existingAttribute != null)
                {//daha önceden bu attribute var ama durumu pasif, durumları aktif yapılıyor
                    existingAttribute.SetCategoryAttribute(existingAttribute.CategoryId, existingAttribute.AttributeId, existingAttribute.IsRequired, existingAttribute.IsVariantable, existingAttribute.IsListed, true);
                    continue;
                }
                else
                {//daha önceden bu attribute yok, yeni ekleniyor
                    var categoryAttribute = new CategoryAttribute(categoryId, attributeId, true, false, false);
                    _categoryAttributes.Add(categoryAttribute);
                }
            }

            //daha önceden attributeler var ama şimdi seçili gelmemiş,durumları pasife çekilecek
            var substract = _categoryAttributes.Where(x => !attributeIds.Contains(x.AttributeId)).ToList();
            foreach (var item in substract)
            {
                item.SetCategoryAttribute(item.CategoryId, item.AttributeId, item.IsRequired, item.IsVariantable, item.IsListed, false);
            }

        }

        //public async Task LoadCategoryAttributes(ICategoryAttributeRepository categoryAttributeRepository, IAttributeRepository attributeRepository)
        //{
        //    var categoryAttributes = await categoryAttributeRepository.FilterByAsync(c => c.CategoryId == Id);
        //    foreach (var item in categoryAttributes)
        //    {
        //        AttributeList.Add(attributeList.FirstOrDefault(x => x.Id == item.AttributeId));
        //        //AttributeList.Add(await attributeRepository.GetByIdAsync(item.AttributeId));
        //    }
        //}

        //public async Task AddAtributes(List<CategoryAttribute> categoryAttributes,ICategoryAttributeRepository categoryAttributeRepository)
        //{
        //    foreach (var item in categoryAttributes)
        //    {
        //        CategoryAttributes.Add(item);
        //        await categoryAttributeRepository.SaveAsync(item);
        //    }

        //}

        public async Task LoadCategoryImage(ICategoryImageRepository categoryImageRepository)
        {
            var categoryImage = await categoryImageRepository.FilterByAsync(c => c.CategoryId == Id);
            if (categoryImage.Count > 0)
            {
                SetImage(categoryImage.FirstOrDefault()?.Name, categoryImage.FirstOrDefault()?.Url, categoryImage.FirstOrDefault()?.Description);
            }
        }

    }
}