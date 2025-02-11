using Microsoft.EntityFrameworkCore;
using Services.DataAccess;
using Services.DataAccess.Repositories;
using Services.Domain;

namespace Services.UnitTests.RepositoryTests {
    public class ServiceCategoryRepositoryTests {
        private ServiceContext GetDbContext() {
            var options = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase( databaseName: Guid.NewGuid().ToString() )
                .Options;
            return new ServiceContext( options );
        }

        [Fact]
        public async Task CreateAsync_AddsServiceCategoryToDatabase() {
            using var context = GetDbContext();
            var repository = new ServiceCategoryRepository( context );
            var category = new ServiceCategory { Name = "TestCategory" };

            var id = await repository.CreateAsync( category );
            var result = await context.ServiceCategories.FindAsync( id );

            Assert.NotNull( result );
            Assert.Equal( "TestCategory", result.Name );
        }

        [Fact]
        public async Task AnyAsync_ReturnsTrueIfExists() {
            using var context = GetDbContext();
            var repository = new ServiceCategoryRepository( context );
            var category = new ServiceCategory { Name = "TestCategory" };
            await context.ServiceCategories.AddAsync( category );
            await context.SaveChangesAsync();

            var exists = await repository.AnyAsync( c => c.Name == "TestCategory" );

            Assert.True( exists );
        }

        [Fact]
        public async Task AnyAsync_ReturnsFalseIfNotExists() {
            using var context = GetDbContext();
            var repository = new ServiceCategoryRepository( context );
            var exists = await repository.AnyAsync( c => c.Name == "NonExistentCategory" );

            Assert.False( exists );
        }

        [Fact]
        public async Task GetAsync_ReturnsCorrectCategory() {
            using var context = GetDbContext();
            var repository = new ServiceCategoryRepository( context );
            var category = new ServiceCategory { Name = "TestCategory" };
            await context.ServiceCategories.AddAsync( category );
            await context.SaveChangesAsync();

            var result = await repository.GetAsync( c => c.Name == "TestCategory" );

            Assert.NotNull( result );
            Assert.Equal( "TestCategory", result.Name );
        }

        [Fact]
        public async Task GetLightAsync_ReturnsCorrectCategoryWithoutServices() {
            using var context = GetDbContext();
            var repository = new ServiceCategoryRepository( context );
            var category = new ServiceCategory { Name = "TestCategory" };
            await context.ServiceCategories.AddAsync( category );
            await context.SaveChangesAsync();

            var result = await repository.GetLightAsync( c => c.Name == "TestCategory" );

            Assert.NotNull( result );
            Assert.Equal( "TestCategory", result.Name );
        }
    }

}
