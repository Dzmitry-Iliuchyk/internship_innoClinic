using MassTransit;
using Moq;
using Services.Application;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services.Dtos;
using Services.Application.Exceptions;
using Services.Application.Implementations.Services;
using Services.Domain;
using Shared.Events.Contracts.ServiceMessages;
using System.Linq.Expressions;

namespace Services.UnitTests.ServiceTests {

    public class ServiceServiceTests {
        private readonly Mock<IServiceRepository> _serviceRepositoryMock;
        private readonly Mock<ISpecializationRepository> _specializationRepositoryMock;
        private readonly Mock<IServiceCategoryRepository> _serviceCategoryRepositoryMock;
        private readonly Mock<IPublishEndpoint> _publisherMock;
        private readonly ServiceService _serviceService;

        public ServiceServiceTests() {
            _serviceRepositoryMock = new Mock<IServiceRepository>();
            _specializationRepositoryMock = new Mock<ISpecializationRepository>();
            _serviceCategoryRepositoryMock = new Mock<IServiceCategoryRepository>();
            _publisherMock = new Mock<IPublishEndpoint>();

            _serviceService = new ServiceService(
                _serviceRepositoryMock.Object,
                _specializationRepositoryMock.Object,
                _serviceCategoryRepositoryMock.Object,
                _publisherMock.Object
            );

            MapsterConfiguration.Configure();
        }

        [Fact]
        public async Task CreateAsync_ServiceAlreadyExists_ThrowsException() {
            // Arrange
            var createServiceDto = new CreateServiceDto {
                Name = "TestService",
                Price = 100,
                CategoryName = "TestCategoryName",
                IsActive = true,
                SpecializationName = "TestSpec"
            };

            _serviceRepositoryMock.Setup( r => r.AnyAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( true );

            // Act & Assert
            await Assert.ThrowsAsync<ServiceAlreadyExistException>( () => _serviceService.CreateAsync( createServiceDto ) );
        }

        [Fact]
        public async Task CreateAsync_ValidService_CreatesAndPublishesEvent() {
            // Arrange
            var createServiceDto = new CreateServiceDto {
                Name = "TestService",
                Price = 100,
                CategoryName = "TestCategoryName",
                IsActive = true,
                SpecializationName = "TestSpec"
            };
            var serviceId = Guid.NewGuid();
            _serviceRepositoryMock.Setup( r => r.AnyAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( false );
            _serviceRepositoryMock.Setup( r => r.CreateAsync( It.IsAny<Service>() ) )
                .Returns( Task.CompletedTask );
            _serviceCategoryRepositoryMock.Setup( r => r.GetAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) ).ReturnsAsync( new ServiceCategory {
                Id = 0,
                Name = "TestCategory",
                Services = [],
                TimeSlotSize = TimeSpan.FromMinutes( 30 )
            } );
            _specializationRepositoryMock.Setup( r => r.GetAsync( It.IsAny<Expression<Func<Specialization, bool>>>() ) ).ReturnsAsync( new Specialization {
                Id = 0,
                Name = "TestSpec",
                IsActive = true,
                Services = [],
            } );
            // Act
            var result = await _serviceService.CreateAsync( createServiceDto );

            // Assert
            Assert.NotEqual( Guid.Empty, result );
            _serviceRepositoryMock.Verify( r => r.CreateAsync( It.IsAny<Service>() ), Times.Once );
            _publisherMock.Verify( p => p.Publish( It.IsAny<ServiceCreated>(), default ), Times.Once );
        }


        [Fact]
        public async Task DeleteAsync_ServiceNotFound_ThrowsException() {
            // Arrange
            var serviceId = Guid.NewGuid();
            _serviceRepositoryMock.Setup( r => r.GetLightAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( (Service)null );
            // Act && Assert
            await Assert.ThrowsAsync<ServiceNotFoundException>( () => _serviceService.DeleteAsync( serviceId ) );
        }

        [Fact]
        public async Task DeleteAsync_ValidService_DeletesAndPublishesEvent() {
            // Arrange
            var service = new Service { Id = Guid.NewGuid() };
            _serviceRepositoryMock.Setup( r => r.GetLightAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( service );
            _serviceRepositoryMock.Setup( r => r.DeleteAsync( service ) ).Returns( Task.CompletedTask );
            // Act
            await _serviceService.DeleteAsync( service.Id );
            // Assert
            _serviceRepositoryMock.Verify( r => r.DeleteAsync( service ), Times.Once );
            _publisherMock.Verify( p => p.Publish( It.IsAny<ServiceDeleted>(), default ), Times.Once );
        }

        [Fact]
        public async Task GetAsync_ServiceNotFound_ThrowsException() {
            // Arrange
            var serviceId = Guid.NewGuid();
            _serviceRepositoryMock.Setup( r => r.GetAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( (Service)null );
            // Act && Assert
            await Assert.ThrowsAsync<ServiceNotFoundException>( () => _serviceService.GetAsync( serviceId ) );
        }

        [Fact]
        public async Task GetAsync_ValidService_ReturnsServiceDto() {
            // Arrange
            var service = new Service { Id = Guid.NewGuid(), Name = "Test" };
            _serviceRepositoryMock.Setup( r => r.GetAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( service );
            // ACt
            var result = await _serviceService.GetAsync( service.Id );
            // Assert
            Assert.NotNull( result );
        }

        [Fact]
        public async Task GetAll_ShouldReturnValidResult() {
            //Arrange
            IList<Service> entities = new List<Service>() {
                new Service(){
                    Id = Guid.NewGuid(),
                    Name = "Консультация терапевта",
                    Price = 1500.00m,
                    CategoryId = 1,
                    Category = new ServiceCategory { Id = 1, Name = "Консультации врачей"},
                    SpecializationId = 1,
                    Specialization = new Specialization { Id = 1, Name = "Терапия" },
                    IsActive = true
                },
                new Service(){
                    Id = Guid.NewGuid(),
                    Name = "УЗИ брюшной полости",
                    Price = 2500.00m,
                    CategoryId = 2,
                    Category = new ServiceCategory { Id = 2, Name = "Диагностика" },
                    SpecializationId = 2,
                    Specialization = new Specialization { Id = 2, Name = "Ультразвуковая диагностика" },
                    IsActive = true
                }
            };
            _serviceRepositoryMock.Setup( repo => repo.GetAllAsync() ).Returns( Task.FromResult( entities ) );
            //Act
            var services = await _serviceService.GetAllAsync();
            //Assert
            Assert.Equal( 2, services.Count );
            Assert.Equal( entities[ 0 ].Id, services[ 0 ].Id );
            Assert.Equal( entities[ 1 ].Id, services[ 1 ].Id );
        }

        [Fact]
        public async Task UpdateAsync_ServiceNotFound_ThrowsException() {
            // Arrange
            var updateServiceDto = new UpdateServiceDto { Id = Guid.NewGuid(), Name = "Updated" };
            _serviceRepositoryMock.Setup( r => r.AnyAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( false );
            // Act && Assert
            await Assert.ThrowsAsync<ServiceNotFoundException>( () => _serviceService.UpdateAsync( updateServiceDto ) );
        }

        [Fact]
        public async Task UpdateAsync_ValidService_UpdatesAndPublishesEvent() {
            // Arrange
            var updateServiceDto = new UpdateServiceDto { Id = Guid.NewGuid(), Name = "Updated", Price = 150 };
            _serviceRepositoryMock.Setup( r => r.AnyAsync( It.IsAny<Expression<Func<Service, bool>>>() ) )
                .ReturnsAsync( true );
            _serviceRepositoryMock.Setup( r => r.UpdateAsync( It.IsAny<Service>() ) ).Returns( Task.CompletedTask );
            _serviceCategoryRepositoryMock.Setup( r => r.GetAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) ).ReturnsAsync( new ServiceCategory {
                Id = 0,
                Name = "TestCategory",
                Services = [],
                TimeSlotSize = TimeSpan.FromMinutes( 30 )
            } );
            _specializationRepositoryMock.Setup( r => r.GetAsync( It.IsAny<Expression<Func<Specialization, bool>>>() ) ).ReturnsAsync( new Specialization {
                Id = 0,
                Name = "TestSpec",
                IsActive = true,
                Services = [],
            } );
            // Act
            await _serviceService.UpdateAsync( updateServiceDto );
            // Assert
            _serviceRepositoryMock.Verify( r => r.UpdateAsync( It.IsAny<Service>() ), Times.Once );
            _publisherMock.Verify( p => p.Publish( It.IsAny<ServiceUpdated>(), default ), Times.Once );
        }
    }
}