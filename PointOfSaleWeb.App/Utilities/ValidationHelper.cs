using PointOfSaleWeb.Models;

namespace PointOfSaleWeb.App.Utilities
{
    public static class ValidationHelper
    {
        public static ValidationResult DateRangeValidation(DateTime? initialDate, DateTime? finalDate)
        {
            var result = new ValidationResult();

            if (!initialDate.HasValue || !finalDate.HasValue)
            {
                result.Success = false;
                result.Message = "Both InitialDate and FinalDate are required.";

                return result;
            }
             
            if (initialDate.Value > finalDate.Value)
            {
                result.Success = false;
                result.Message = "InitialDate cannot be greater than FinalDate.";

                return result;
            }

            result.Success = true;

            return result;
        }
    }
}
