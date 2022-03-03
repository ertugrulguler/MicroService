using Catalog.ApiContract.Contract;
using Catalog.ApiContract.Request.Command.ProductCommands;
using Catalog.Domain.ProductAggregate;
using Framework.Core.Logging;
using Framework.Core.Model;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Catalog.ApplicationService.Handler.Command.ProductCommands
{
    public class CreateXmlWithProductsCommandHandler : IRequestHandler<CreateXmlWithProductsCommand, ResponseBase<string>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSellerRepository _productSellerRepository;
        private readonly IProductImageRepository _productImageRepository;
        private readonly IAppLogger _appLogger;
        private static string _baseurl;
        private static string _connectionString;
        private static string _cdnHostName;

        public CreateXmlWithProductsCommandHandler(IProductRepository productRepository, IProductSellerRepository productSellerRepository, IProductImageRepository productImageRepository, IConfiguration configuration, IAppLogger appLogger)
        {
            _productRepository = productRepository;
            _productSellerRepository = productSellerRepository;
            _productImageRepository = productImageRepository;
            _baseurl = configuration["BaseUrl"];
            _connectionString = configuration["AzureConnectionString"];
            _cdnHostName = configuration["CDNHostName"];
            _appLogger = appLogger;
        }

        public async Task<ResponseBase<string>> Handle(CreateXmlWithProductsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var merchantItems = new MerchantItems();
                var xmlSerializer = new XmlSerializer(merchantItems.GetType());
                var productSellerIds = new List<Guid>();
                var sellerDeliveryIds = new List<Guid>();

                for (int i = 0; i < request.FeedProductDtos.Count; i++)
                {
                    productSellerIds.Add(Guid.Parse(request.FeedProductDtos[i].ProductSeller));
                    sellerDeliveryIds.Add(Guid.Parse(request.FeedProductDtos[i].SellerDelivery));
                }

                var productIds = await _productSellerRepository.GetProductIdsByProductSellerIds(productSellerIds);

                var imageUrls = await _productImageRepository.GetProductImagesByProductIds(productIds);

                var xmlProducts = await _productRepository.GetXmlProducts(GenerateIdListWithComma(productSellerIds), GenerateIdListWithComma(sellerDeliveryIds));

                var xmlAttributes = await _productRepository.GetXmlAttributes(GenerateIdListWithComma(productIds));

                merchantItems.ProductXmlDtos = new ProductXmlDto[xmlProducts.Count];

                for (int i = 0; i < xmlProducts.Count; i++)
                {
                    var productXmlDto = new ProductXmlDto();

                    productXmlDto.ID = xmlProducts[i].Id;
                    productXmlDto.CategoryID = xmlProducts[i].CategoryId;
                    productXmlDto.CategoryName = CloseDataWithCData(xmlProducts[i].CategoryName);
                    productXmlDto.Brand = CloseDataWithCData(xmlProducts[i].BrandName);
                    productXmlDto.Name = CloseDataWithCData(char.ToUpper(xmlProducts[i].Name[0]) + xmlProducts[i].Name.Substring(1).ToLower());
                    productXmlDto.Field = CloseDataWithCData(xmlProducts[i].CampaignText);
                    productXmlDto.Url = CloseDataWithCData(_baseurl + "/" + xmlProducts[i].SeoName + "-p-" + xmlProducts[i].Code);
                    productXmlDto.PricePlusTax = xmlProducts[i].Price;
                    productXmlDto.UrlMobile = productXmlDto.Url;

                    var images = imageUrls.Where(iu => (iu.ProductId == xmlProducts[i].Id) && (iu.SellerId == xmlProducts[i].SellerId)).ToList();
                    productXmlDto.ImageUrls = new XmlCDataSection[images.Count];
                    for (int k = 0; k < images.Count; k++)
                    {
                        productXmlDto.ImageUrls[k] = CloseDataWithCData(images[k].Url);
                    }
                    productXmlDto.ShippingFee = xmlProducts[i].ShippingPrice;
                    productXmlDto.Stock = xmlProducts[i].StockCount;
                    productXmlDto.ShippingDay = xmlProducts[i].DeliveryDuration;
                    productXmlDto.ShippingDetail = CloseDataWithCData("Saat " + xmlProducts[i].LastHourForDeliveryDuration + ":00' a kadar verilen siparişler aynı gün gonderilir");

                    productXmlDto.Eans = new XmlCDataSection[1];
                    productXmlDto.Eans[0] = CloseDataWithCData(xmlProducts[i].Code);

                    var attributes = xmlAttributes.Where(xa => xa.ProductId == xmlProducts[i].Id).ToList();
                    productXmlDto.Specs = new Spec[attributes.Count];
                    for (int j = 0; j < attributes.Count; j++)
                    {
                        var spec = new Spec();
                        spec.Values = CloseDataWithCData(attributes[j].Value);
                        productXmlDto.Specs[j] = spec;
                    }

                    merchantItems.ProductXmlDtos[i] = productXmlDto;
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    xmlSerializer.Serialize(ms, merchantItems);
                    var cloudBlobContainer = CDNAsync();
                    if (ms.ToArray() != null)
                    {
                        _appLogger.Trace("CreateXmlWithProductsCommandHandler GetBlockBlobReference start", MethodBase.GetCurrentMethod());
                        CloudBlockBlob cloudBlockBlobTemp = cloudBlobContainer.Result.GetBlockBlobReference("production/temp_products.xml");
                        _appLogger.Trace("CreateXmlWithProductsCommandHandler GetBlockBlobReference end", MethodBase.GetCurrentMethod());
                        var isExistFile = await cloudBlockBlobTemp.ExistsAsync();
                        if (isExistFile)
                        {
                            await cloudBlockBlobTemp.DeleteAsync();
                        }
                        cloudBlockBlobTemp.Properties.ContentType = "text/xml";
                        _appLogger.Trace("CreateXmlWithProductsCommandHandler UploadFromByteArrayAsync start", MethodBase.GetCurrentMethod());
                        await cloudBlockBlobTemp.UploadFromByteArrayAsync(ms.ToArray(), 0, ms.ToArray().Length);
                        _appLogger.Trace("CreateXmlWithProductsCommandHandler UploadFromByteArrayAsync end", MethodBase.GetCurrentMethod());
                        CloudBlockBlob cloudBlockBlob = cloudBlobContainer.Result.GetBlockBlobReference("production/products.xml");
                        await cloudBlockBlob.StartCopyAsync(cloudBlockBlobTemp);
                        await cloudBlockBlobTemp.DeleteAsync();
                        var cdnString = _cdnHostName + cloudBlockBlob.Uri.LocalPath;

                        return new ResponseBase<string>
                        {
                            Data = cdnString,
                            Success = true,
                            MessageCode = "201",
                            Message = "ProductXml Files Created"
                        };
                    }
                    return new ResponseBase<string>
                    {
                        Success = false,
                        MessageCode = "404",
                        Message = "Empty Xml File"
                    };
                }

            }
            catch (Exception e)
            {
                return new ResponseBase<string>
                {
                    Success = false,
                    MessageCode = "404",
                    Message = "Error"
                };
            }
        }

        public async Task<CloudBlobContainer> CDNAsync()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("catalogfeedxml");
            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return cloudBlobContainer;
        }

        public XmlCDataSection CloseDataWithCData(string data)
        {
            return new XmlDocument().CreateCDataSection(data);
        }

        public string GenerateIdListWithComma<T>(List<T> list)
        {
            var result = new StringBuilder();
            foreach (var item in list)
            {
                result.Append(item);
                result.Append(',');
            }
            result.Length--; //remove last comma
            return result.ToString();
        }

    }
}
