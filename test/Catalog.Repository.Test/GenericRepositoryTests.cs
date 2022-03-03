namespace Catalog.Repository.Test
{
    public class GenericRepositoryTests
    {
        //[Theory]
        //public async Task Save_Should_Success()
        //{
        //    var option = new DbContextOptionsBuilder<CatalogDbContext>()
        //        .UseInMemoryDatabase("catalog")
        //        .Options;

        //    using (var context = new CatalogDbContext(option))
        //    {
        //        var dbContextHandler = new DbContextHandler(context);
        //        var categoryRepo = new CategoryRepository(context);

        //        var actual = new Category("categoryName");

        //        await categoryRepo.SaveAsync(actual);

        //        await dbContextHandler.SaveChangesAsync();

        //        var expected = await categoryRepo.GetByIdAsync(actual.Id);

        //        //Assertion
        //        Assert.IsNotNull(expected);
        //        Assert.AreEqual(actual.Name, expected.Name);
        //        Assert.AreEqual(actual.Id, expected.Id);
        //        Assert.AreEqual(actual, expected);
        //    }
        //}

        //public void Save_Should_Fail_When_ConnString_Empty()
        //{
        //}
    }
}