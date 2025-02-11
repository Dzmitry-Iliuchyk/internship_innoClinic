using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Offices.Application.Dtos;
using Offices.Domain.Models;
using Shared.Events.Contracts.OfficesMessages;
using System.Net.Http.Json;
using System.Reflection;
using System.Text.Json;
namespace Offices.IntegrationTests {
    public class OfficesIntegrationTests: IClassFixture<CustomWebApplicationFactory> {
        private readonly HttpClient _client;

        public OfficesIntegrationTests( CustomWebApplicationFactory factory ) {
            _client = factory.CreateClient();

            Directory.CreateDirectory( Path.Combine( Directory.GetCurrentDirectory(), "Auth" ) );
            using var sw = new StreamWriter( File.Create(Path.Combine("Auth", "public_key.pem")));
            sw.WriteAsync( 
                @"-----BEGIN PUBLIC KEY-----
                MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwydUU+5JL++pw+fAlVFq
                rwcLSHjTiw+pIArZeRRo0CN0MtdW4wA4WqETE3OHrq6n3Sz9y3wHTg+6BQxN1JVh
                fWBQ8B3vnJXdBK/+CDcId0M8gGVSBIYSmwoR/HxFwAb0j2wZd2zy83aattqaa2qD
                6SNnbGc4f76oAKHmpOLOG48kN1sc5iPmOcYoSRwZ/UzzICn68P5/DjtYV2BFydxU
                X7AeYy9eTIsZ7TPtjRivVnhJxWsjlSPrwYyqea72RwfBpnM78XmO4bQnLg2yzFoS
                UuBcGE+PBKVMR8O03jnBMqZ+nXyyTTaR9iOlb0j0yMYQnhR8jbtmv/XfuSQC6Ow/
                kQIDAQAB
                -----END PUBLIC KEY-----
                " );
            sw.Flush();
        }


        [Fact]
        public async Task CreateOffice_ReturnsOfficeId() {
            // Arrange
            var newOffice = new {
                Address = new {
                    City = "TestCity",
                    Street = "TestStreet",
                    HouseNumber = "123",
                    OfficeNumber = "45"
                },
                RegistryPhoneNumber = "+375869595685",
                Status = true
            };

            // Act
            var response = await _client.PostAsJsonAsync( "/api/offices/CreateOffice", newOffice );

            // Assert
            response.EnsureSuccessStatusCode();
            var officeId = await response.Content.ReadAsStringAsync();
            Assert.False( string.IsNullOrEmpty( officeId ) );
        }

        [Fact]
        public async Task GetOffice_ReturnsCorrectOffice() {
            // Arrange: Создаем тестовый офис
            var newOffice = new {
                Address = new {
                    City = "TestCity",
                    Street = "TestStreet",
                    HouseNumber = "123",
                    OfficeNumber = "45"
                },
                RegistryPhoneNumber = "+375997695656",
                Status = true
            };
            var createResponse = await _client.PostAsJsonAsync( "/api/offices/CreateOffice", newOffice );
            var officeId =await createResponse.Content.ReadFromJsonAsync<string>();

            // Act
            var response = await _client.GetAsync( $"/api/offices/{officeId}/GetOffice" );

            // Assert
            response.EnsureSuccessStatusCode();
            var office = await response.Content.ReadFromJsonAsync<Office>();
            Assert.NotNull( office );
            Assert.Equal( officeId, office.Id );
        }

        [Fact]
        public async Task UpdateOffice_ReturnsNoContent() {
            // Arrange: Создаем тестовый офис
            var newOffice = new {
                Address = new {
                    City = "TestCity",
                    Street = "TestStreet",
                    HouseNumber = "123",
                    OfficeNumber = "45"
                },
                RegistryPhoneNumber = "+375569995656",
                Status = true
            };
            var createResponse = await _client.PostAsJsonAsync( "/api/offices/CreateOffice", newOffice );
            var officeId = await createResponse.Content.ReadFromJsonAsync<string>();

            var updateOfficeDto = new {
                Address = new {
                    City = "UpdatedCity",
                    Street = "UpdatedStreet",
                    HouseNumber = "321",
                    OfficeNumber = "99"
                },
                RegistryPhoneNumber = "+375959993434",
                Status = false
            };

            // Act
            var updateResponse = await _client.PutAsJsonAsync( $"/api/offices/UpdateOffice?id={officeId}", updateOfficeDto );

            // Assert
            Assert.Equal( System.Net.HttpStatusCode.NoContent, updateResponse.StatusCode );
        }

        [Fact]
        public async Task GetOffices_ReturnsListOfOffices() {
            // Arrange: Создаем тестовый офис
            var newOffice = new {
                Address = new {
                    City = "TestCity",
                    Street = "TestStreet",
                    HouseNumber = "123",
                    OfficeNumber = "45"
                },
                RegistryPhoneNumber = "+375999924656",
                Status = true
            };
            await _client.PostAsJsonAsync( "/api/offices/CreateOffice", newOffice );

            // Act
            var response = await _client.GetAsync( "/api/offices/GetOffices" );

            // Assert
            response.EnsureSuccessStatusCode();
            var offices = await response.Content.ReadFromJsonAsync<Office[]>();
            Assert.NotNull( offices );
            Assert.NotEmpty( offices );
            Assert.Equal( "TestCity", offices[ 0 ].Address.City );
        }

        [Fact]
        public async Task DeleteOffice_ReturnsNoContent() {
            // Arrange: Создаем тестовый офис
            var newOffice = new {
                Address = new {
                    City = "TestCity",
                    Street = "TestStreet",
                    HouseNumber = "123",
                    OfficeNumber = "45"
                },
                RegistryPhoneNumber = "+375997295656",
                Status = true
            };
            var createResponse = await _client.PostAsJsonAsync( "/api/offices/CreateOffice", newOffice );
             var officeId =await createResponse.Content.ReadFromJsonAsync<string>();

            // Act
            var deleteResponse = await _client.DeleteAsync( $"/api/offices/DeleteOffice?id={officeId}" );

            // Assert
            Assert.Equal( System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode );
        }
    }

}