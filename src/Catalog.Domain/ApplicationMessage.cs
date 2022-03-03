using System.Collections.Generic;

namespace Catalog.Domain
{
    public static class ApplicationMessage
    {
        private const string CommonUserErrorMessage =
            "İşleminiz şu anda gerçekleştirilemiyor. Lütfen daha sonra tekrar deneyiniz.";

        public static string UnhandledError = "CAT_-1";
        public static readonly string Success = "CAT_0";
        public static string AttributeAlreadyExist = "CAT_27";
        public static string AttributeCodeInvalid = "CAT_28";
        public static string CategoryAlreadyExist = "CAT_29";
        public static string InvalidParameter = "CAT_30";
        public static string TimeoutOccurred = "CAT_55";
        public static string UnExpectedHttpResponseReceived = "CAT_56";
        public static string ProductNotFound = "CAT_57";
        public static string EmptyCategoryList = "CAT_58";
        public static string BrandAlreadyExist = "CAT_59";
        public static string EmptyList = "CAT_60";
        public static string InvalidId = "CAT_61";
        public static string InvalidProductId = "CAT_62";
        public static string AlreadyProductBrandId = "CAT_63";
        public static string CategoryNotFound = "CAT_64";
        public static string BrandNotFound = "CAT_65";
        public static string CategoryIsNotLeaf = "CAT_66";
        public static string AttributeAndAttributeValueIdNotFound = "CAT_67";
        public static string AttributeIdAlreadyExist = "CAT_68";
        public static string AttributeNotFound = "CAT_69";
        public static string InvalidPrice = "CAT_70";
        public static string BackOfficeApprove = "CAT_71";
        public static string CategoryAttributeNotFound = "CAT_72";
        public static string EmptySellerProducts = "CAT_73";
        public static string MainCategoryCannotBeSuggested = "CAT_74";
        public static string AttributeValueAlreadyExist = "CAT_75";
        public static string CategoryInstallmentAlreadyExist = "CAT_76";
        public static string ProductSellerAlreadyExist = "CAT_77";
        public static string IsNotSalePrice = "CAT_78";
        public static string NotMaxInstallmentCount = "CAT_79";
        public static string NotPrice = "CAT_80";
        public static string BannerAlreadyExist = "CAT_81";
        public static string CategoryIsNotVirtual = "CAT_82";
        public static string FavoriteProductsEmpty = "CAT_83";
        public static string BannerTypeInvalid = "CAT_84";
        public static string ChannelCodeInvalid = "CAT_85";
        public static string BannerLeafCategory = "CAT_86";
        public static string NotDate = "CAT_87";
        public static string NotBannnerLocation = "CAT_88";
        public static string InvalidBannerLocation = "CAT_89";
        public static string CategoryIdAndFilterModelNotEmpty = "CAT_90";
        public static string FavoriteListNotAdded = "CAT_91";
        public static string PageSizeMaxExceeded = "CAT_92";
        public static string InvalidCatalogProductId = "CAT_93";
        public static string CategoryIdNotOneOrThan = "CAT_94";
        public static string CategoryAttributeAlreadyExist = "CAT_95";
        public static string CategoryAttributeDeleted = "CAT_96";
        public static string CategoryAttributeUpdated = "CAT_97";
        public static string SellerNotAvailable = "CAT_98";
        public static string CategoryNotDelete = "CAT_99";
        public static string SameCodeDiffrentCategory = "CAT_100";
        public static string CategoryNotActive = "CAT_101";
        public static string AttributeNotCorrectFormat = "CAT_102";
        public static string AttributeValueNotCorrectFormat = "CAT_103";
        public static string AttributeMapNotCorrectFormat = "CAT_104";
        public static string AttributeValueOrderError = "CAT_105";
        public static string CategoryNotCorrectFormat = "CAT_106";
        public static string CategoryAttributeNotCorrectFormat = "CAT_107";
        public static string CategoryAttributeMapNotCorrectFormat = "CAT_108";
        public static string BadgeAlreadyExist = "CAT_109";
        public static string ProductBadgeAlreadyExist = "CAT_110";
        public static string InvalidBadge = "CAT_111";
        public static string CategoryNotActiveForCreateProduct = "CAT_112";
        public static string InvalidProductCode = "CAT_113";
        public static string NotVirtualCategory = "CAT_114";
        public static string AttributeValueNotFound = "CAT_115";
        public static string StockCountNotZero = "CAT_116";
        public static string ProductAlreadyCreatedDifferentCategory = "CAT_117";
        public static string ProductWithChannelAlreadyExist = "CAT_118";

        private static readonly Dictionary<string, string> ErrorMessages =
            new Dictionary<string, string>()
            {
                {UnhandledError, "Unhandled exception."},
                {Success, "İşlem Başarıyla Gerçekleştirildi."},
                {AttributeAlreadyExist, "Aynı isimden attribute zaten ekli"},
                {AttributeCodeInvalid, "AttributeCode Invalid"},
                {CategoryAlreadyExist, "Bu isimde kategori daha önceden tanımlanmış"},
                {InvalidParameter, "Invalid parameter."},
                {TimeoutOccurred, "Timeout oluştu."},
                {UnExpectedHttpResponseReceived, "Beklenmedik bir httpCode ile response alındı."},
                {ProductNotFound, "Ürün bulunamadı."},
                {EmptyCategoryList, "Kategori listesinde veri ekli değil."},
                {BrandAlreadyExist, "Bu isimde marka daha önce tanımlanmış!"},
                {EmptyList, "Listelenecek veri bulunamadı!"},
                {InvalidId, "Veriye ait kayıt bulunamadı!"},
                {InvalidProductId, "Ürüne ait Id bulunamadı."},
                {AlreadyProductBrandId, "Bu marka bir ürüne tanımlanmış. Ürüne tanımlanan markayı silemezsiniz!"},
                {CategoryNotFound, "Kategori bulunamadı!"},
                {BrandNotFound, "Brand bulunamadı!"},
                {CategoryIsNotLeaf, "Kategori leaf değildir!"},
                {AttributeAndAttributeValueIdNotFound, "Id'li AttributeId ve AttributeValueId bulunamadı!"},
                {AttributeIdAlreadyExist, "Aynı Id'li Attribute zaten ekli!"},
                {AttributeNotFound, "Id'li AttributeId bulunamadı!"},
                {InvalidPrice, "Geçersiz fiyat bilgisi!"},
                {
                    BackOfficeApprove,
                    "Fiyat değişikliği belirtilen orandan fazla olduğu için ürününüz onaya gönderildi."
                },
                {CategoryAttributeNotFound, "Categoriye ait Attribute bulunamadı!"},
                {EmptySellerProducts, "Satıcıya ait ürün bulunamadı!"},
                {
                    MainCategoryCannotBeSuggested,
                    "Ana kategori ve bir alt kategori önerilen olamaz. Lütfen alt kategorilerden seçiniz."
                },
                {AttributeValueAlreadyExist, "Aynı kategori içerisinde aynı isimden attribute value zaten ekli"},
                {CategoryInstallmentAlreadyExist, "Daha önce bu Kategoriye ait tanımlama yapılmıştır! "},
                {ProductSellerAlreadyExist, "Satıcıya ait bu ürün, daha önce eklenmiştir! Lüften kontrol ediniz. "},
                {IsNotSalePrice, "Satılacak fiyat satış fiyatından büyük olamaz!"},
                {NotMaxInstallmentCount, "Geçersiz taksit sayısı. Taksit sayısı 0-18 arasında olabilir."},
                {NotPrice, "Geçersiz değer!"},
                {BannerAlreadyExist, "Bu Banner zaten ekli."},
                {CategoryIsNotVirtual, "Kategori Virtual kategori olmalıdır!"},
                {FavoriteProductsEmpty, "Henüz begendiğiniz bir ürün bulunmamaktadır!"},
                {BannerTypeInvalid, "Banner tipi geçerli değildir!"},
                {ChannelCodeInvalid, "Kanal  geçerli değildir!"},
                {BannerLeafCategory, "BannerLocation'da girilen Banner Type icin, ActionId Leaf Category olamaz!"},
                {NotDate, "Başlangıç tarihi Bitiş tarihinden büyük olamaz!"},
                {NotBannnerLocation, "Banner Location'ı eklenmemiş!" },
                {InvalidBannerLocation, "Geçersiz banner location!"},
                {CategoryIdAndFilterModelNotEmpty, "Filtreleme alanlarından en az birini girmeniz gerekmektedir!"},
                {FavoriteListNotAdded, "Beğendiklerim listesine eklenemedi!"},
                {PageSizeMaxExceeded, "Sayfa boyutu 500'den küçük olmalıdır." },
                {InvalidCatalogProductId, "Ürün katalogda bulunamadı, Onayda bekleyen ürünlerinizi kontrol ediniz." },
                {CategoryIdNotOneOrThan, "Birden fazla kategori  ile filtreleme yapılamaz!"},
                {CategoryAttributeAlreadyExist, "Bu kategoriye bu attribute zaten ekli!"},
                {CategoryAttributeDeleted, "Silme işlemi gerçekleştirildi!"},
                {SellerNotAvailable, "Satıcı şu anda hizmet verememektedir!"},
                {CategoryAttributeUpdated, "Güncelleme işlemi gerçekleştirildi!"},
                {CategoryNotDelete,"İlgili kategoride ürün olduğu için silinemez!" },
                {SameCodeDiffrentCategory,"Bu kod farklı bir kategoriye aittir!" },
                {CategoryNotActive,"Bu kategori aktif bir kategori değildir bu yüzden bu kategoriye ait ürünü onaylayamazsınız!" },
                {AttributeNotCorrectFormat,"Yüklemek istediğiniz excelde Attribute bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {AttributeValueNotCorrectFormat,"Yüklemek istediğiniz excelde AttributeValue bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {AttributeMapNotCorrectFormat,"Yüklemek istediğiniz excelde AttributeMap bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {AttributeValueOrderError,"AttributeValue sıralamasında bir sorun oluşmuştur. Lütfen sistem yöneticisi ile iletişime geçiniz!" },
                {CategoryNotCorrectFormat,"Yüklemek istediğiniz excelde Category bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {CategoryAttributeNotCorrectFormat,"Yüklemek istediğiniz excelde CategoryAttribute bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {CategoryAttributeMapNotCorrectFormat,"Yüklemek istediğiniz excelde CategoryAttributeMap bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {CategoryNotActiveForCreateProduct,"Bu kategori aktif bir kategori değildir bu yüzden bu kategoriye ait ürün ekleyemezsiniz!" },
                {BadgeAlreadyExist, "Eklenmek istenilen Badge zaten ekli." },
                {ProductBadgeAlreadyExist, "Eklenmek istenilen badge, ürün ve satıcı bazlı zaten ekli." },
                {InvalidBadge, "Eklenmek istenilen badge geçerli değil." },
                {InvalidProductCode, "Geçersiz ürün kodu." },
                {NotVirtualCategory,"Category bulunamadı. Lütfen CategoryId'nin Virtual category oldugundan emin olun." },
                {AttributeValueNotFound,"AttributeValueId bulunamadı!" },

                {StockCountNotZero," kodlu ürünün stok sayısı 0'dan küçük olamaz!" },
                {ProductAlreadyCreatedDifferentCategory," barkodu başka bir kategoride tanımlıdır. Kategori : " },
                {ProductWithChannelAlreadyExist,"Bu ürün belirtilen channel kod ile daha önce eklenmiştir." }

            };

        private static readonly Dictionary<string, string> UserMessages =
            new Dictionary<string, string>()
            {
                {UnhandledError, CommonUserErrorMessage},
                {Success, "İşlem Başarıyla Gerçekleştirildi."},
                {AttributeAlreadyExist, "Aynı isimden attribute zaten ekli"},
                {AttributeCodeInvalid, "AttributeCode Invalid"},
                {CategoryAlreadyExist, "Bu isimde kategori daha önceden tanımlanmış"},
                {InvalidParameter, "Invalid parameter."},
                {TimeoutOccurred, "Timeout oluştu."},
                {UnExpectedHttpResponseReceived, "Beklenmedik bir httpCode ile response alındı."},
                {ProductNotFound, "Ürün bulunamadı."},
                {EmptyCategoryList, "Kategori listesinde veri ekli değil."},
                {BrandAlreadyExist, "Bu isimde marka daha önce tanımlanmış!"},
                {EmptyList, "Listelenecek veri bulunamadı!"},
                {InvalidId, "Veriye ait kayıt bulunamadı!"},
                {InvalidProductId, "Ürüne ait Id bulunamadı."},
                {AlreadyProductBrandId, "Bu marka bir ürüne tanımlanmış. Ürüne tanımlanan markayı silemezsiniz!"},
                {CategoryNotFound, "Kategori bulunamadı!"},
                {BrandNotFound, "Brand bulunamadı!"},
                {CategoryIsNotLeaf, "Kategori leaf değildir!"},
                {AttributeAndAttributeValueIdNotFound, "Id'li AttributeId ve AttributeValueId bulunamadı!"},
                {AttributeIdAlreadyExist, "Aynı Id'li Attribute zaten ekli!"},
                {AttributeNotFound, "Id'li AttributeId bulunamadı!"},
                {InvalidPrice, "Geçersiz fiyat bilgisi!"},
                {
                    BackOfficeApprove,
                    "Fiyat değişikliği belirtilen orandan fazla olduğu için ürününüz onaya gönderildi."
                },
                {CategoryAttributeNotFound, "Categoriye ait Attribute bulunamadı!"},
                {EmptySellerProducts, "Satıcıya ait ürün bulunamadı!"},
                {
                    MainCategoryCannotBeSuggested,
                    "Ana kategori ve bir alt kategori önerilen olamaz. Lütfen alt kategorilerden seçiniz."
                },
                {AttributeValueAlreadyExist, "Aynı kategori içerisinde aynı isimden attribute value zaten ekli"},
                {CategoryInstallmentAlreadyExist, "Daha önce bu Kategoriye ait tanımlama yapılmıştır! "},
                {ProductSellerAlreadyExist, "Satıcıya ait bu ürün, daha önce eklenmiştir! Lüften kontrol ediniz. "},
                {IsNotSalePrice, "Satılacak fiyat satış fiyatından büyük olamaz!"},
                {NotMaxInstallmentCount, "Geçersiz taksit sayısı. Taksit sayısı 0-18 arasında olabilir."},
                {NotPrice, "Geçersiz değer!"},
                {BannerAlreadyExist, "Bu Banner zaten ekli."},
                {CategoryIsNotVirtual, "Kategori Virtual kategori olmalıdır!"},
                {BannerTypeInvalid, "Banner tipi geçerli değildir!"},
                {ChannelCodeInvalid, "Kanal  geçerli değildir!"},
                {InvalidBannerLocation, "Geçersiz anasayfa alanı!"},
                {BannerLeafCategory, "BannerLocation'da girilen Banner Type icin, ActionId Leaf Category olamaz!"},
                {NotDate, "Başlangıç tarihi Bitiş tarihinden büyük olamaz."},
                {NotBannnerLocation, "Banner Location'ı eklenmemiş!" },
                {CategoryIdAndFilterModelNotEmpty, "Filtreleme alanlarından en az birini girmeniz gerekmektedir!"},
                {FavoriteListNotAdded, "Beğendiklerim listesine eklenemedi!"},
                {PageSizeMaxExceeded, "Sayfa boyutu 500'den küçük olmalıdır." },
                {InvalidCatalogProductId, "Ürün katalogda bulunamadı, Onayda bekleyen ürünlerinizi kontrol ediniz." },
                {CategoryIdNotOneOrThan, "Birden fazla Kategori  ile filtreleme yapılamaz!"},
                {CategoryAttributeAlreadyExist, "Bu kategoriye bu özellik zaten ekli!"},
                {CategoryAttributeDeleted, "Silme işlemi gerçekleştirildi!"},
                {SellerNotAvailable, "Satıcı şu anda hizmet verememektedir!"},
                {CategoryAttributeUpdated, "Güncelleme işlemi gerçekleştirildi!"},
                {CategoryNotDelete,"İlgili kategoride ürün olduğu için silinemez!" },
                {SameCodeDiffrentCategory,"Bu kod farklı bir kategoriye aittir!" },
                {CategoryNotActive,"Bu kategori aktif bir kategori değildir bu yüzden bu kategoriye ait ürünü onaylayamazsınız!" },
                {AttributeNotCorrectFormat,"Yüklemek istediğiniz excelde Attribute bilgileri yanlış girilmiştir.Lütfen kontrol ediniz?" },
                {AttributeValueNotCorrectFormat,"Yüklemek istediğiniz excelde AttributeValue bilgileri yanlış girilmiştir.Lütfen kontrol ediniz?" },
                {AttributeMapNotCorrectFormat,"Yüklemek istediğiniz excelde AttributeMap bilgileri yanlış girilmiştir.Lütfen kontrol ediniz?" },
                {AttributeValueOrderError,"AttributeValue sıralamasında bir sorun oluşmuştur. Lütfen sistem yöneticisi ile iletişime geçiniz!" },
                {CategoryNotCorrectFormat,"Yüklemek istediğiniz excelde Category bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {CategoryAttributeNotCorrectFormat,"Yüklemek istediğiniz excelde CategoryAttribute bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {CategoryAttributeMapNotCorrectFormat,"Yüklemek istediğiniz excelde CategoryAttributeMap bilgileri yanlış girilmiştir.Lütfen kontrol ediniz!" },
                {CategoryNotActiveForCreateProduct,"Bu kategori aktif bir kategori değildir bu yüzden bu kategoriye ait ürün ekleyemezsiniz!" },
                {BadgeAlreadyExist, "Eklenmek istenilen Badge zaten ekli." },
                {ProductBadgeAlreadyExist, "Eklenmek istenilen badge, ürün ve satıcı bazlı zaten ekli." },
                {InvalidBadge, "Eklenmek istenilen badge geçerli değil." },
                {InvalidProductCode, "Geçersiz ürün kodu." },
                {NotVirtualCategory,"Category bulunamadı. Lütfen CategoryId'nin Virtual category oldugundan emin olun." },
                {AttributeValueNotFound,"AttributeValueId bulunamadı!" },
                {StockCountNotZero," kodlu ürünün stok sayısı 0'dan küçük olamaz!" },
                {ProductAlreadyCreatedDifferentCategory," barkodu başka bir kategoride tanımlıdır. Kategori : " },
                {ProductWithChannelAlreadyExist,"Bu ürün belirtilen channel kod ile daha önce eklenmiştir." }

            };
        public static string Message(this string code)
        {
            ErrorMessages.TryGetValue(code, out var errorMessage);
            return errorMessage;
        }

        public static string UserMessage(this string code)
        {
            UserMessages.TryGetValue(code, out var errorMessage);
            return errorMessage;
        }
    }
}