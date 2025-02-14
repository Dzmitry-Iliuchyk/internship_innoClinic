using Microsoft.EntityFrameworkCore;
using Services.DataAccess;
using Services.DataAccess.Repositories;
using Services.Domain;

namespace Services.UnitTests.RepositoryTests {

    public class SpecializationRepositoryTests {
        private readonly DbContextOptions<ServiceContext> _dbContextOptions;

        public SpecializationRepositoryTests() {
            _dbContextOptions = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase( databaseName: Guid.NewGuid().ToString() )
                .Options;
        }

        [Fact]
        public async Task CreateAsync_ShouldAddSpecializationAndReturnId() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new SpecializationRepository( context );
            var specialization = new Specialization { Name = "TestSpecialization" };

            var id = await repository.CreateAsync( specialization );

            Assert.NotEqual( 0, id );
            Assert.Equal( 1, context.Specializations.Count() );
        }

        [Fact]
        public async Task AnyAsync_ShouldReturnTrueIfSpecializationExists() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new SpecializationRepository( context );
            var specialization = new Specialization { Name = "ExistingSpecialization" };
            context.Specializations.Add( specialization );
            await context.SaveChangesAsync();

            var exists = await repository.AnyAsync( s => s.Name == "ExistingSpecialization" );
            Assert.True( exists );
        }

        [Fact]
        public async Task AnyAsync_ShouldReturnFalseIfSpecializationDoesNotExist() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new SpecializationRepository( context );
            var exists = await repository.AnyAsync( s => s.Name == "NonExistent" );
            Assert.False( exists );
        }

        [Fact]
        public async Task GetAsync_ShouldReturnSpecializationWithServices() {
            using var context = new ServiceContext( _dbContextOptions );
            var specialization = new Specialization { Name = "TestSpecialization", Services = new List<Service> { new Service { Name = "TestService" } } };
            context.Specializations.Add( specialization );
            await context.SaveChangesAsync();

            var repository = new SpecializationRepository( context );
            var result = await repository.GetAsync( s => s.Name == "TestSpecialization" );

            Assert.NotNull( result );
            Assert.Equal( "TestSpecialization", result.Name );
            Assert.Single( result.Services );
        }

        [Fact]
        public async Task GetAsync_ShouldReturnNullIfSpecializationDoesNotExist() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new SpecializationRepository( context );
            var result = await repository.GetAsync( s => s.Name == "NonExistent" );

            Assert.Null( result );
        }
    }

}
