using DeskMarket.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DeskMarket.Controllers
{
    public class HomeController : BaseController
    {
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}