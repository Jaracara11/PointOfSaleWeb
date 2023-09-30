using System.ComponentModel.DataAnnotations;

namespace PointOfSaleWeb.Models
{
    public class DateRangeRequest
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime InitialDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FinalDate { get; set; }
    }
}
