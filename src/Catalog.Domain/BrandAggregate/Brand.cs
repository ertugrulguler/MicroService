using Catalog.Domain.Entities;

namespace Catalog.Domain.BrandAggregate
{
    public class Brand : Entity
    {
        public string Name { get; protected set; }
        public string LogoUrl { get; protected set; }
        public string WebSite { get; protected set; }
        public string SeoName { get; protected set; }

        protected Brand()
        {

        }

        public Brand(string name, string logoUrl, string webSite, string seoName) : this()
        {
            Name = name;
            LogoUrl = logoUrl;
            WebSite = webSite;
            SeoName = seoName;
        }

        public void SetBrand(string name, string logoUrl, string webSite, string seoName)
        {
            Name = name;
            LogoUrl = logoUrl;
            WebSite = webSite;
            SeoName = seoName;
        }
    }
}