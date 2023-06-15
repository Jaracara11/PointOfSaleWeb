using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PointOfSaleWeb.Models;
using PointOfSaleWeb.Repository.Interfaces;

namespace PointOfSaleWeb.App.Controllers.Sale
{
    [Route("api/sale")]
    [Authorize]
    [ApiController]
    public class SaleController : ControllerBase
    {

    }
}
