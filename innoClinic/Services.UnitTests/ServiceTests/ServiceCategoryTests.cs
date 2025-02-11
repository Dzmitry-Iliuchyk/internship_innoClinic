using Moq;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services.Dtos;
using Services.Application.Exceptions;
using Services.Application.Implementations.Services;
using Services.Domain;
using System.Linq.Expressions;

namespace Services.UnitTests.ServiceTests {
    public class ServiceCategoryServiceTests {
        private readonly Mock<IServiceCategoryRepository> _serviceCategoryRepositoryMock;
        private readonly ServiceCategoryService _serviceCategoryService;

        public ServiceCategoryServiceTests() {
            _serviceCategoryRepositoryMock = new Mock<IServiceCategoryRepository>();
            _serviceCategoryService = new ServiceCategoryService( _serviceCategoryRepositoryMock.Object );
        }

        [Fact]
        public async Task CreateAsync_ServiceCategoryAlreadyExists_ThrowsException() {
            // Arrange
            var createDto = new CreateServiceCategoryDto { Name = "ExistingCategory" };
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( true );

            // Act & Assert
            await Assert.ThrowsAsync<ServiceCategoryAlreadyExistException>( () => _serviceCategoryService.CreateAsync( createDto ) );
        }

        [Fact]
        public async Task CreateAsync_ValidServiceCategory_CreatesSuccessfully() {
            // Arrange
            var createDto = new CreateServiceCategoryDto { Name = "NewCategory" };
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( false );
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.CreateAsync( It.IsAny<ServiceCategory>() ) )
                .ReturnsAsync( 1 );

            // Act
            var result = await _serviceCategoryService.CreateAsync( createDto );

            // Assert
            Assert.Equal( 1, result );
            _serviceCategoryRepositoryMock.Verify( repo => repo.CreateAsync( It.IsAny<ServiceCategory>() ), Times.Once );
        }

        [Fact]
        public async Task DeleteAsync_ServiceCategoryNotFound_ThrowsException() {
            // Arrange
            int categoryId = 1;
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.GetLightAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( (ServiceCategory)null );

            // Act & Assert
            await Assert.ThrowsAsync<ServiceCategoryNotFoundException>( () => _serviceCategoryService.DeleteAsync( categoryId ) );
        }

        [Fact]
        public async Task DeleteAsync_ValidServiceCategory_DeletesSuccessfully() {
            // Arrange
            int categoryId = 1;
            var serviceCategory = new ServiceCategory { Id = categoryId, Name = "CategoryToDelete" };
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.GetLightAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( serviceCategory );
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.DeleteAsync( serviceCategory ) )
                .Returns( Task.CompletedTask );

            // Act
            await _serviceCategoryService.DeleteAsync( categoryId );

            // Assert
            _serviceCategoryRepositoryMock.Verify( repo => repo.DeleteAsync( serviceCategory ), Times.Once );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsServiceCategoryList() {
            // Arrange
            var categories = new List<ServiceCategory>
            {
            new ServiceCategory { Id = 1, Name = "Category1" },
            new ServiceCategory { Id = 2, Name = "Category2" }
        };
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.GetAllAsync() )
                .ReturnsAsync( categories );

            // Act
            var result = await _serviceCategoryService.GetAllAsync();

            // Assert
            Assert.NotNull( result );
            Assert.Equal( 2, result.Count );
        }

        [Fact]
        public async Task GetAsync_ServiceCategoryNotFound_ThrowsException() {
            // Arrange
            int categoryId = 1;
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( false );

            // Act & Assert
            await Assert.ThrowsAsync<ServiceCategoryNotFoundException>( () => _serviceCategoryService.GetAsync( categoryId ) );
        }

        [Fact]
        public async Task GetAsync_ValidServiceCategory_ReturnsServiceCategoryDto() {
            // Arrange
            int categoryId = 1;
            var serviceCategory = new ServiceCategory { Id = categoryId, Name = "Category1" };
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( true );
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.GetAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( serviceCategory );

            // Act
            var result = await _serviceCategoryService.GetAsync( categoryId );

            // Assert
            Assert.NotNull( result );
            Assert.Equal( categoryId, result.Id );
        }

        [Fact]
        public async Task UpdateAsync_ServiceCategoryNotFound_ThrowsException() {
            // Arrange
            var updateDto = new ServiceCategoryDto { Id = 1, Name = "UpdatedCategory" };
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( false );

            // Act & Assert
            await Assert.ThrowsAsync<ServiceCategoryNotFoundException>( () => _serviceCategoryService.UpdateAsync( updateDto ) );
        }

        [Fact]
        public async Task UpdateAsync_ServiceCategoryNameAlreadyExists_ThrowsException() {
            // Arrange
            var updateDto = new ServiceCategoryDto { Id = 1, Name = "ExistingCategory" };
            _serviceCategoryRepositoryMock
                .SetupSequence( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( true )  // Exists by Id
                .ReturnsAsync( true ); // Exists by Name

            // Act & Assert
            await Assert.ThrowsAsync<ServiceCategoryAlreadyExistException>( () => _serviceCategoryService.UpdateAsync( updateDto ) );
        }

        [Fact]
        public async Task UpdateAsync_ValidServiceCategory_UpdatesSuccessfully() {
            // Arrange
            var updateDto = new ServiceCategoryDto { Id = 1, Name = "UpdatedCategory" };
            _serviceCategoryRepositoryMock
                .SetupSequence( repo => repo.AnyAsync( It.IsAny<Expression<Func<ServiceCategory, bool>>>() ) )
                .ReturnsAsync( true )  // Exists by Id
                .ReturnsAsync( false ); // Name does not exist
            _serviceCategoryRepositoryMock
                .Setup( repo => repo.UpdateAsync( It.IsAny<ServiceCategory>() ) )
                .Returns( Task.CompletedTask );

            // Act
            await _serviceCategoryService.UpdateAsync( updateDto );

            // Assert
            _serviceCategoryRepositoryMock.Verify( repo => repo.UpdateAsync( It.IsAny<ServiceCategory>() ), Times.Once );
        }
    }

}