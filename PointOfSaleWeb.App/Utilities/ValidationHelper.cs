using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.App.Utilities
{
    public static class ValidationHelper
    {
        public static ValidationResult DateRangeValidation(DateTime? startDate, DateTime? endDate)
        {
            const string MissingDatesMessage = "Both start date and end date are required.";
            const string InvalidDateRangeMessage = "Start date cannot be greater than end date.";

            if (!startDate.HasValue || !endDate.HasValue)
            {
                return new ValidationResult { Success = false, Message = MissingDatesMessage };
            }

            if (startDate.Value > endDate.Value)
            {
                return new ValidationResult { Success = false, Message = InvalidDateRangeMessage };
            }

            return new ValidationResult { Success = true };
        }
    }
}
