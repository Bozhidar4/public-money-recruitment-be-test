using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PutRentalTests : IClassFixture<IntegrationFixture>
    {
        private IntegrationFixture _fixture;

        public PutRentalTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPutRental_ThenAGetReturnsTheCorrectValuesOfTheUpdatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var updatedRequest = new RentalUpdateModel
            {
                Id = postResult.Id,
                Units = 3,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel putResult;
            using (var putResponse = await _fixture.Client.PutAsJsonAsync($"/api/v1/rentals", updatedRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _fixture.Client.GetAsync($"/api/v1/rentals/{putResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(updatedRequest.Units, getResult.Units);
                Assert.Equal(updatedRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenCompleteRequestAndBookings_WhenPutRental_ThenAGetReturnsTheCorrectValuesOfTheUpdatedRental()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 05)
            };

            using (var postBooking2Response = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                Assert.True(postBooking2Response.IsSuccessStatusCode);
            }

            var updatedRequest = new RentalUpdateModel
            {
                Id = postResult.Id,
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel putResult;
            using (var putResponse = await _fixture.Client.PutAsJsonAsync($"/api/v1/rentals", updatedRequest))
            {
                Assert.True(putResponse.IsSuccessStatusCode);
                putResult = await putResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            using (var getResponse = await _fixture.Client.GetAsync($"/api/v1/rentals/{putResult.Id}"))
            {
                Assert.True(getResponse.IsSuccessStatusCode);

                var getResult = await getResponse.Content.ReadAsAsync<RentalViewModel>();
                Assert.Equal(updatedRequest.Units, getResult.Units);
                Assert.Equal(updatedRequest.PreparationTimeInDays, getResult.PreparationTimeInDays);
            }
        }

        [Fact]
        public async Task GivenNoRentals_WhenPutRental_ThenAPutReturnsAnError()
        {
            _fixture = new IntegrationFixture();

            var updatedRequest = new RentalUpdateModel
            {
                Id = 1,
                Units = 3,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel putResult;
            using (var putResponse = await _fixture.Client.PutAsJsonAsync($"/api/v1/rentals", updatedRequest))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.InternalServerError, putResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenUnitsIsNotPosiive_WhenPutRental_ThenAPutReturnsAnError()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var updatedRequest = new RentalUpdateModel
            {
                Id = postResult.Id,
                Units = -3,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel putResult;
            using (var putResponse = await _fixture.Client.PutAsJsonAsync($"/api/v1/rentals", updatedRequest))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
            }
        }

        [Fact]
        public async Task GivenPreparationTimeInDaysIsZero_WhenPutRental_ThenAPutReturnsAnError()
        {
            var request = new RentalBindingModel
            {
                Units = 2,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postResult;
            using (var postResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", request))
            {
                Assert.True(postResponse.IsSuccessStatusCode);
                postResult = await postResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var updatedRequest = new RentalUpdateModel
            {
                Id = postResult.Id,
                Units = 3,
                PreparationTimeInDays = 0
            };

            ResourceIdViewModel putResult;
            using (var putResponse = await _fixture.Client.PutAsJsonAsync($"/api/v1/rentals", updatedRequest))
            {
                Assert.False(putResponse.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
            }
        }
    }
}
