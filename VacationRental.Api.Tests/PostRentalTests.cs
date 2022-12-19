using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostRentalTests : IClassFixture<IntegrationFixture>
    {
        private IntegrationFixture _fixture;

        public PostRentalTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostRental_ThenAGetReturnsTheCreatedRental()
        {
            // Arrange
            var request = new RentalBindingModel
            {
                Units = 25,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            // Act
            using (var getResponse = await _fixture.Client.GetAsync($"/api/v1/rentals/{postResult.Id}"))
            {
                // Assert
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(request.Units, getResult.Units);
            }
        }

        [Fact]
        public async Task GivenUnitsIsNotPositive_WhenPostRental_ThenAPostReturnsAnError()
        {
            // Arrange
            var request = new RentalBindingModel
            {
                Units = -1,
                PreparationTimeInDays = 2
            };

            // Act
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                // Assert
                Assert.False(postResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenPreparationTimeInDaysIsNotPositive_WhenPostRental_ThenAPostReturnsAnError()
        {
            // Arrange
            var request = new RentalBindingModel
            {
                Units = -1,
                PreparationTimeInDays = 2
            };

            // Act
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                // Assert
                Assert.False(postResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
            }
        }
    }
}
