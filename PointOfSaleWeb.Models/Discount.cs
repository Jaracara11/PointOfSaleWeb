using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSaleWeb.Models
{
    internal class Discount
    {
        public int UserRoleID { get; set; }
        public string DiscountsAvailable { get; set; } = string.Empty;
    }
}
