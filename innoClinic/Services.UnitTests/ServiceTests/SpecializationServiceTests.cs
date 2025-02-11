using MassTransit;
using Moq;
using Services.Application.Abstractions.Repositories;
using Services.Application.Abstractions.Services.Dtos;
using Services.Application.Implementations.Services;
using Services.Domain;
using Shared.Events.Contracts.ServiceMessages;
using System.Linq.Expressions;

namespace Services.UnitTests.ServiceTests {
    public class SpecializationServiceTests {
        private readonly Mock<ISpecializationRepository> _specializationRepositoryMock;
        private readonly Mock<IPublishEndpoint> _publisherMock;
        private readonly SpecializationService _specializationService;

        public SpecializationServiceTests() {
            _specializationRepositoryMock = new Mock<ISpecializationRepository>();
            _publisherMock = new Mock<IPublishEndpoint>();
            _specializationService = new SpecializationService( _specializationRepositoryMock.Object, _publisherMock.Object );
        }

        [Fact]
        public async Task CreateAsync_ValidSpecialization_CreatesSuccessfully() {
            // Arrange
            var createDto = new CreateSpecializationDto { Name = "NewSpecialization", IsActive = true };
            _specializationRepositoryMock
                .Setup( repo => repo.CreateAsync( It.IsAny<Specialization>() ) )
                .ReturnsAsync( 1 );

            // Act
            var result = await _specializationService.CreateAsync( createDto );

            // Assert
            Assert.Equal( 1, result );
            _specializationRepositoryMock.Verify( repo => repo.CreateAsync( It.IsAny<Specialization>() ), Times.Once );
            _publisherMock.Verify( p => p.Publish( It.IsAny<SpecializationCreated>(), default ), Times.Once );
        }

        [Fact]
        public async Task DeleteAsync_SpecializationNotFound_ThrowsException() {
            // Arrange
            int specId = 1;
            _specializationRepositoryMock
                .Setup( repo => repo.GetLightAsync( It.IsAny<Expression<Func<Specialization, bool>>>() ) )
                .ReturnsAsync( (Specialization)null );

            // Act & Assert
            await Assert.ThrowsAsync<Exception>( () => _specializationService.DeleteAsync( specId ) );
        }

        [Fact]
        public async Task DeleteAsync_ValidSpecialization_DeletesSuccessfully() {
            // Arrange
            int specId = 1;
            var specialization = new Specialization { Id = specId, Name = "SpecializationToDelete" };
            _specializationRepositoryMock
                .Setup( repo => repo.GetLightAsync( It.IsAny<Expression<Func<Specialization, bool>>>() ) )
                .ReturnsAsync( specialization );
            _specializationRepositoryMock
                .Setup( repo => repo.DeleteAsync( specialization ) )
                .Returns( Task.CompletedTask );

            // Act
            await _specializationService.DeleteAsync( specId );

            // Assert
            _specializationRepositoryMock.Verify( repo => repo.DeleteAsync( specialization ), Times.Once );
            _publisherMock.Verify( p => p.Publish( It.IsAny<SpecializationDeleted>(), default ), Times.Once );
        }

        [Fact]
        public async Task GetAllAsync_ReturnsSpecializationList() {
            // Arrange
            var specializations = new List<Specialization>
            {
            new Specialization { Id = 1, Name = "Spec1" },
            new Specialization { Id = 2, Name = "Spec2" }
        };
            _specializationRepositoryMock
                .Setup( repo => repo.GetAllAsync() )
                .ReturnsAsync( specializations );

            // Act
            var result = await _specializationService.GetAllNamesAsync();

            // Assert
            Assert.NotNull( result );
            Assert.Equal( 2, result.Count );
        }

        [Fact]
        public async Task GetAsync_ValidSpecialization_ReturnsSpecializationDto() {
            // Arrange
            int specId = 1;
            var specialization = new Specialization { Id = specId, Name = "Spec1" };
            _specializationRepositoryMock
                .Setup( repo => repo.GetAsync( It.IsAny<Expression<Func<Specialization, bool>>>() ) )
                .ReturnsAsync( specialization );

            // Act
            var result = await _specializationService.GetAsync( specId );

            // Assert
            Assert.NotNull( result );
            Assert.Equal( specId, result.Id );
        }

        [Fact]
        public async Task UpdateAsync_ValidSpecialization_UpdatesSuccessfully() {
            // Arrange
            var updateDto = new SpecializationDto { Id = 1, Name = "UpdatedSpecialization", IsActive = true };
            _specializationRepositoryMock
                .Setup( repo => repo.UpdateAsync( It.IsAny<Specialization>() ) )
                .Returns( Task.CompletedTask );

            // Act
            await _specializationService.UpdateAsync( updateDto );

            // Assert
            _specializationRepositoryMock.Verify( repo => repo.UpdateAsync( It.IsAny<Specialization>() ), Times.Once );
            _publisherMock.Verify( p => p.Publish( It.IsAny<SpecializationUpdated>(), default ), Times.Once );
        }
    }
}