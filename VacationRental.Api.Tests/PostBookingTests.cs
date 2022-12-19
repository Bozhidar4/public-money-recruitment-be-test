using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using VacationRental.Api.Models;
using Xunit;

namespace VacationRental.Api.Tests
{
    [Collection("Integration")]
    public class PostBookingTests : IClassFixture<IntegrationFixture>
    {
        private IntegrationFixture _fixture;

        public PostBookingTests(IntegrationFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAGetReturnsTheCreatedBooking()
        {
            // Arrange
            var postRentalRequest = new RentalBindingModel
            {
                Units = 4,
                PreparationTimeInDays = 1
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                 RentalId = postRentalResult.Id,
                 Nights = 3,
                 Start = new DateTime(2001, 01, 01)
            };

            ResourceIdViewModel postBookingResult;
            using (var postBookingResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                Assert.True(postBookingResponse.IsSuccessStatusCode);
                postBookingResult = await postBookingResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            // Act
            using (var getBookingResponse = await _fixture.Client.GetAsync($"/api/v1/bookings/{postBookingResult.Id}"))
            {
                // Assert
                Assert.True(getBookingResponse.IsSuccessStatusCode);

                var getBookingResult = await getBookingResponse.Content.ReadAsAsync<BookingViewModel>();
                Assert.Equal(postBookingRequest.RentalId, getBookingResult.RentalId);
                Assert.Equal(postBookingRequest.Nights, getBookingResult.Nights);
                Assert.Equal(postBookingRequest.Start, getBookingResult.Start);
            }
        }

        [Fact]
        public async Task GivenCompleteRequest_WhenPostBooking_ThenAPostReturnsAnErrorWhenThereIsOverbooking()
        {
            // Arrange
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBooking1Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 3,
                Start = new DateTime(2002, 01, 01)
            };

            using (var postBooking1Response = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBooking1Request))
            {
                Assert.True(postBooking1Response.IsSuccessStatusCode);
            }

            var postBooking2Request = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 1,
                Start = new DateTime(2002, 01, 02)
            };

            // Act
            using (var postBooking2Response = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBooking2Request))
            {
                // Assert
                Assert.False(postBooking2Response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.InternalServerError, postBooking2Response.StatusCode);
            }
        }

        [Fact]
        public async Task GivenNightsInputNotPositive_WhenPostBooking_ThenAPostReturnsAnError()
        {
            // Arrange
            var postRentalRequest = new RentalBindingModel
            {
                Units = 1,
                PreparationTimeInDays = 2
            };

            ResourceIdViewModel postRentalResult;
            using (var postRentalResponse = await _fixture.Client.PostAsJsonAsync($"/api/v1/rentals", postRentalRequest))
            {
                Assert.True(postRentalResponse.IsSuccessStatusCode);
                postRentalResult = await postRentalResponse.Content.ReadAsAsync<ResourceIdViewModel>();
            }

            var postBookingRequest = new BookingBindingModel
            {
                RentalId = postRentalResult.Id,
                Nights = 0,
                Start = new DateTime(2002, 01, 01)
            };

            // Act
            using (var postBooking2Response = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                // Assert
                Assert.False(postBooking2Response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postBooking2Response.StatusCode);
            }
        }

        [Fact]
        public async Task GivenNoRentals_WhenPostBooking_ThenAPostReturnsAnError()
        {
            // Arrange
            var postBookingRequest = new BookingBindingModel
            {
                RentalId = 1,
                Nights = 0,
                Start = new DateTime(2002, 01, 01)
            };

            // Act
            using (var postBooking2Response = await _fixture.Client.PostAsJsonAsync($"/api/v1/bookings", postBookingRequest))
            {
                // Assert
                Assert.False(postBooking2Response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.BadRequest, postBooking2Response.StatusCode);
            }
        }
    }
}
