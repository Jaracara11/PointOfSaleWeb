//using System.ComponentModel.DataAnnotations;

//namespace PointOfSaleWeb.Models
//{
//    public class Order
//    {
//        [Key]
//        public string OrderID { get; set; } = string.Empty;

//        [Required]
//        [StringLength(25, MinimumLength = 3, ErrorMessage = "Username must have between 3 and 50 characters.")]
//        public string User { get; set; } = string.Empty;

//        [Required]
//        public OrderProducts[] Products { get; set; } = Array.Empty<OrderProducts>();

//        [Range(0.01, double.MaxValue, ErrorMessage = "Discount must be greater than zero.")]
//        public decimal? Discount { get; set; }

//        [Required]
//        [Range(0.01, double.MaxValue, ErrorMessage = "Order total must be greater than zero.")]
//        public decimal OrderTotal { get; set; }

//        [Required]
//        public DateTime OrderDate { get; set; } = DateTime.Now;
//    }

//    public class OrderProducts
//    {
//        public int ProductID { get; set; }
//        public int ProductQuantity { get; set; }
//    }
//}
