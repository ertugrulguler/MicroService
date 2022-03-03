namespace Catalog.ApplicationService.Test.CommandHandler
{
    public class CreateCategoryUnitTests
    {
        //[Theory]
        //public async Task Handle_Should_Throw_Exception_When_CategoryAlreadyExist()
        //{
        //    //Mocking
        //    //fake sınıflar, interface'ler etc.
        //    var categoryRepository = new Mock<ICategoryRepository>();
        //    var categoryAttributeRepository = new Mock<ICategoryAttributeRepository>();
        //    var dbContextHandler = new Mock<IDbContextHandler>();
        //    var request = new CreateCategoryCommand {Name = "requestedName"};

        //    //Sut (Service Under Test)
        //    //Sut "Service Under Test"'in kısaltılmışı olarak unit test metotlarında test etmek istediğimiz sınıfın&service'in ismini belirtmek için değişken tanımlamada kullanılan kısaltmadır diyebiliriz.
        //    var sut = new CreateCategoryCommandHandler(
        //        categoryRepository.Object,
        //        categoryAttributeRepository.Object,
        //        dbContextHandler.Object);
        //    //var category = new Category(request.Name);
        //    categoryRepository.Setup(c => c.FindByAsync(x => x.Name == request.Name)).ReturnsAsync(category);

        //    //Expected ; unit test yazdığımız fonksiyonalitenin vermesi beklenen çıktısı, result'ını belirtirken kullanılır.
        //    var expectedMessageCode = ApplicationMessage.CategoryAlreadyExist;

        //    //Actual ; unit test'ini yazdığımız metot yada sınıfın gerçek, o an döndüğü result'ı tanımlarken kullanılır.
        //    Func<Task> actual = async () => { await sut.Handle(request, new CancellationToken()); };

        //    //Assertion
        //    //Actual ve Expected değerlerini karşılaştırırken içerisinde tanımlamalar yapabildiğimiz yapının/metodun/sınıfın ismidir.
        //    actual.Should().Throw<BusinessRuleException>().And.Code.Should().Be(expectedMessageCode);
        //}

        //[Theory]
        //public async Task Handle_Should_Successfully_Save_Category()
        //{
        //    //Mocking
        //    //fake sınıflar, interface'ler etc.
        //    var categoryRepository = new Mock<ICategoryRepository>();
        //    var categoryAttributeRepository = new Mock<ICategoryAttributeRepository>();
        //    var dbContextHandler = new Mock<IDbContextHandler>();
        //    var request = new CreateCategoryCommand {Name = "requestedName"};

        //    //Sut (Service Under Test)
        //    //Sut "Service Under Test"'in kısaltılmışı olarak unit test metotlarında test etmek istediğimiz sınıfın&service'in ismini belirtmek için değişken tanımlamada kullanılan kısaltmadır diyebiliriz.
        //    var sut = new CreateCategoryCommandHandler(
        //        categoryRepository.Object,
        //        categoryAttributeRepository.Object,
        //        dbContextHandler.Object);

        //    var category = new Category(request.Name);

        //    categoryRepository.Setup(c => c.FindByAsync(c => c.Name == request.Name)).ReturnsAsync((Category) null);

        //    categoryRepository.Setup(c => c.SaveAsync(category));
        //    dbContextHandler.Setup(c => c.SaveChangesAsync());

        //    //Expected ; unit test yazdığımız fonksiyonalitenin vermesi beklenen çıktısı, result'ını belirtirken kullanılır.
        //    var expected = new CreateCategoryCommandResult {Success = true};

        //    //Actual ; unit test'ini yazdığımız metot yada sınıfın gerçek, o an döndüğü result'ı tanımlarken kullanılır.
        //    var actual = await sut.Handle(request, new CancellationToken());

        //    //Assertion
        //    //Actual ve Expected değerlerini karşılaştırırken içerisinde tanımlamalar yapabildiğimiz yapının/metodun/sınıfın ismidir.
        //    Assert.AreEqual(expected.Success, actual.Success);
        //    Assert.AreEqual(expected.UserMessage, actual.UserMessage);
        //}
    }
}