using Microsoft.EntityFrameworkCore;
using Services.DataAccess;
using Services.DataAccess.Repositories;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UnitTests.RepositoryTests {

    public class ServiceRepositoryTests {
        private readonly DbContextOptions<ServiceContext> _dbContextOptions;

        public ServiceRepositoryTests() {
            _dbContextOptions = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase( databaseName: Guid.NewGuid().ToString() )
                .Options;
        }

        [Fact]
        public async Task CreateAsync_AddsServiceToDatabase() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new ServiceRepository( context );
            var service = new Service { Id = Guid.NewGuid(), Name = "Test Service" };

            await repository.CreateAsync( service );

            var storedService = await context.Services.FirstOrDefaultAsync( s => s.Id == service.Id );
            Assert.NotNull( storedService );
            Assert.Equal( "Test Service", storedService.Name );
        }

        [Fact]
        public async Task DeleteAsync_RemovesServiceFromDatabase() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new ServiceRepository( context );
            var service = new Service { Id = Guid.NewGuid(), Name = "Service to Delete" };
            context.Services.Add( service );
            await context.SaveChangesAsync();

            await repository.DeleteAsync( service );

            var storedService = await context.Services.FirstOrDefaultAsync( s => s.Id == service.Id );
            Assert.Null( storedService );
        }

        [Fact]
        public async Task UpdateAsync_UpdatesServiceInDatabase() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new ServiceRepository( context );
            var service = new Service { Id = Guid.NewGuid(), Name = "Old Name" };
            context.Services.Add( service );
            await context.SaveChangesAsync();

            service.Name = "New Name";
            await repository.UpdateAsync( service );

            var storedService = await context.Services.FirstOrDefaultAsync( s => s.Id == service.Id );
            Assert.NotNull( storedService );
            Assert.Equal( "New Name", storedService.Name );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllServices() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new ServiceRepository( context );
            var category = new ServiceCategory {
                Id = 0,
                Name = "Test",
                TimeSlotSize = TimeSpan.FromMinutes(30)
            };
            var spec = new Specialization {
                Id = 0,
                IsActive = true,
                Name = "test",
            };
            var services = new List<Service>
            {
            new Service { Id = Guid.NewGuid(), Name = "Service 1", Category = category, Specialization = spec  },
            new Service { Id = Guid.NewGuid(), Name = "Service 2", Category = category, Specialization = spec  }
        };
            context.Services.AddRange( services );
            await context.SaveChangesAsync();

            var result = await repository.GetAllAsync();
            Assert.NotNull( result );
            Assert.Equal( 2, result.Count );
        }

        [Fact]
        public async Task GetAsync_ReturnsCorrectService() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new ServiceRepository( context );
            var category = new ServiceCategory {
                Id = 0,
                Name = "Test",
                TimeSlotSize = TimeSpan.FromMinutes( 30 )
            };
            var spec = new Specialization {
                Id = 0,
                IsActive = true,
                Name = "test",
            };
            var service = new Service { Id = Guid.NewGuid(), Name = "Specific Service", Category = category, Specialization = spec };
            context.Services.Add( service );
            await context.SaveChangesAsync();

            var result = await repository.GetAsync( s => s.Id == service.Id );
            Assert.NotNull( result );
            Assert.Equal( "Specific Service", result.Name );
        }

        [Fact]
        public async Task AnyAsync_ReturnsTrueIfServiceExists() {
            using var context = new ServiceContext( _dbContextOptions );
            var repository = new ServiceRepository( context );
            var service = new Service { Id = Guid.NewGuid(), Name = "Existing Service" };
            context.Services.Add( service );
            await context.SaveChangesAsync();

            var exists = await repository.AnyAsync( s => s.Name == "Existing Service" );
            Assert.True( exists );
        }
    }

}
