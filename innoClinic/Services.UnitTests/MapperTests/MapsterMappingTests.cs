using Mapster;
using Services.Application;
using Services.Application.Abstractions.Services.Dtos;
using Services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.UnitTests.MapperTests {
    public class MapsterMappingTests {
        public MapsterMappingTests() {
            MapsterConfiguration.Configure();
        }

        [Fact]
        public void ServiceToServiceDto_Mapping_IsCorrect() {
            var service = new Service {
                Id = Guid.NewGuid(),
                Name = "Test Service",
                Price = 100,
                IsActive = true,
                Specialization = new Specialization { Name = "Test Specialization" },
                Category = new ServiceCategory { Name = "Test Category" }
            };

            var dto = service.Adapt<ServiceDto>();

            Assert.Equal( service.Id, dto.Id );
            Assert.Equal( service.Name, dto.Name );
            Assert.Equal( service.Price, dto.Price );
            Assert.Equal( service.Specialization.Name, dto.SpecializationName );
            Assert.Equal( service.Category.Name, dto.CategoryName );
            Assert.Equal( service.IsActive, dto.IsActive );
        }

        [Fact]
        public void CreateServiceDtoToService_Mapping_IsCorrect() {
            var dto = new CreateServiceDto {
                Name = "New Service",
                Price = 200,
                IsActive = false,
                SpecializationName = "New Specialization",
                CategoryName = "New Category"
            };

            var service = dto.Adapt<Service>();

            Assert.Equal( dto.Name, service.Name );
            Assert.Equal( dto.Price, service.Price );
            Assert.Equal( dto.SpecializationName, service.Specialization.Name );
            Assert.Equal( dto.CategoryName, service.Category.Name );
            Assert.Equal( dto.IsActive, service.IsActive );
        }
        [Fact]
        public void CreateServiceCategoryDtoToServiceCategory_Mapping_IsCorrect() {
            var dto = new CreateServiceCategoryDto {
                Name = "New Service",
                IsActive = false,
            };

            var service = dto.Adapt<ServiceCategory>();

            Assert.Equal( dto.IsActive, service.IsActive );
            Assert.Equal( dto.Name, service.Name );
        }
        [Fact]
        public void ServiceCategoryToServiceCategoryDto_Mapping_IsCorrect() {
            var dto = new ServiceCategory {
                Id = 1,
                Name = "New Service",
                IsActive = false,
                TimeSlotSize = TimeSpan.FromMinutes(30)
            };

            var service = dto.Adapt<ServiceCategoryDto>();

            Assert.Equal( dto.TimeSlotSize, service.TimeSlotSize );
            Assert.Equal( dto.IsActive, service.IsActive );
            Assert.Equal( dto.Name, service.Name );
            Assert.Equal( dto.Id, service.Id);
        }

        [Fact]
        public void UpdateServiceDtoToService_Mapping_IsCorrect() {
            var dto = new UpdateServiceDto {
                Id = Guid.NewGuid(),
                Name = "Updated Service",
                Price = 300,
                IsActive = true,
                SpecializationName = "Updated Specialization",
                CategoryName = "Updated Category"
            };

            var service = dto.Adapt<Service>();

            Assert.Equal( dto.Id, service.Id );
            Assert.Equal( dto.Name, service.Name );
            Assert.Equal( dto.Price, service.Price );
            Assert.Equal( dto.SpecializationName, service.Specialization.Name );
            Assert.Equal( dto.CategoryName, service.Category.Name );
            Assert.Equal( dto.IsActive, service.IsActive );
        }
        [Fact]
        public void CreateSpecializationDtoToSpecialization_Mapping_IsCorrect() {
            var dto = new CreateSpecializationDto {
                Name = "Updated Service",       
                IsActive = true
            };

            var service = dto.Adapt<Specialization>();

            Assert.Equal( dto.Name, service.Name );
            Assert.Equal( dto.IsActive, service.IsActive );
        }

        [Fact]
        public void SpecializationToSpecializationDto_Mapping_IsCorrect() {
            var dto = new Specialization {
                Id = 3,
                Name = "Updated Service",       
                IsActive = true
            };

            var service = dto.Adapt<SpecializationDto>();

            Assert.Equal( dto.Id, service.Id);
            Assert.Equal( dto.Name, service.Name );
            Assert.Equal( dto.IsActive, service.IsActive );
        }
    }

}
