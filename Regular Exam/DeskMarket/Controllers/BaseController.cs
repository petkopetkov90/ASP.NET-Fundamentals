using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DeskMarket.Controllers
{
    public class BaseController : Controller
    {
        public string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
