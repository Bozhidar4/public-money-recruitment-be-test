namespace VacationRental.Api.Core
{
    public static class VacationRentalConstants
    {
        public const int MinimumValue = 1;

        public const string NotAvailableErrorMessage = "Not available.";
        public const string NightCountErrorMessage = "Nights must be positive.";
        public const string UnitCountErrorMessage = "Units must be positive.";
        public const string PreparationDaysCountErrorMessage = "Preparation days must be positive.";
        public const string BookingNotFoundErrorMessage = "Booking not found.";
        public const string RentalNotFoundErrorMessage = "Rental not found.";
        public const string RentalOverlappingErrorMessage = "There is overlapping, the update cannot be performed.";
    }
}
