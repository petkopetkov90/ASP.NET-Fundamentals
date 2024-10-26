using DeskMarket.Data;
using DeskMarket.Data.Models;
using DeskMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using static DeskMarket.Common.Constants;

namespace DeskMarket.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            //TODO: separate layers / services / repo

            this.context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var model = await context.Products
                .Where(p => p.IsDeleted == false)
                .AsNoTracking()
                .Include(p => p.ProductsClients)
                .Select(p => new ProductIndexViewModel
                {
                    Id = p.Id,
                    ImageUrl = p.ImageUrl,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    IsSeller = p.SellerId == userId,
                    HasBought = p.ProductsClients.Any(pc => pc.ClientId == userId)

                })
                .ToListAsync();


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new ProductInputModel
            {
                Categories = await context.Categories
                    .AsNoTracking()
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ProductInputModel model)
        {
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                model.Categories = await context.Categories.ToListAsync();
                return View(model);
            }

            var product = new Product
            {
                ProductName = model.ProductName,
                Description = model.Description,
                Price = model.Price,
                ImageUrl = model.ImageUrl,
                SellerId = userId,
                AddedOn = DateTime.ParseExact(model.AddedOn, DateFormatString, CultureInfo.InvariantCulture),
                CategoryId = model.CategoryId,
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = GetUserId();

            var model = await context.Products
                .Where(p => p.IsDeleted == false && p.Id == id)
                .Select(p => new ProductEditModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    AddedOn = p.AddedOn.ToString(DateFormatString),
                    SellerId = p.SellerId,
                    CategoryId = p.CategoryId,

                })
                .FirstOrDefaultAsync();

            if (model == null || model.SellerId != userId)
            {
                return RedirectToAction("Index");
            }

            model.Categories = await context.Categories.ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditModel model, int id)
        {
            var userId = GetUserId();

            if (!ModelState.IsValid)
            {
                model.Categories = await context.Categories.ToListAsync();
                return View(model);
            }

            var product = await context.Products.FindAsync(id);

            if (product == null || product.IsDeleted)
            {
                return RedirectToAction("Index");
            }

            product.ProductName = model.ProductName;
            product.Description = model.Description;
            product.Price = model.Price;
            product.ImageUrl = model.ImageUrl;
            product.SellerId = userId;
            product.CategoryId = model.CategoryId;

            if (DateTime.TryParseExact(model.AddedOn, DateFormatString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime addedOn))
            {
                product.AddedOn = addedOn;
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Details", id);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = GetUserId();

            var model = await context.Products
                .Where(p => p.IsDeleted == false && p.Id == id)
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Seller)
                .Include(p => p.ProductsClients)
                .Select(p => new ProductDetailsViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    Description = p.Description,
                    CategoryName = p.Category.Name,
                    AddedOn = p.AddedOn.ToString(DateFormatString),
                    Seller = p.Seller.UserName!,
                    HasBought = p.ProductsClients.Any(pc => pc.ClientId == userId),

                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int id)
        {
            var userId = GetUserId();

            var product = await context.Products
                .Where(p => p.Id == id && p.IsDeleted == false)
                .Include(p => p.ProductsClients)
                .FirstOrDefaultAsync();

            if (product != null &&
                product!.ProductsClients.Any(pc => pc.ClientId == userId) == false)
            {
                product.ProductsClients.Add(new ProductClient
                {
                    ProductId = product.Id,
                    ClientId = userId,
                });

                await context.SaveChangesAsync();

                return RedirectToAction("Cart");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            var userId = GetUserId();

            var model = await context.Products
                .Where(p => p.IsDeleted == false)
                .Where(p => p.ProductsClients.Any(pc => pc.ClientId == userId))
                .AsNoTracking()
                .Select(p => new ProductCartViewModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl
                })
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int id)
        {
            var userId = GetUserId();

            var product = await context.Products
                .Where(p => p.Id == id && p.IsDeleted == false)
                .Include(p => p.ProductsClients)
                .FirstOrDefaultAsync();

            if (product != null && product.ProductsClients.Any())
            {
                var productClient = product.ProductsClients.FirstOrDefault(p => p.ClientId == userId);

                if (productClient != null)
                {
                    product.ProductsClients.Remove(productClient);
                    await context.SaveChangesAsync();
                }
            }

            return RedirectToAction("Cart");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetUserId();

            var model = await context.Products
                .Where(p => p.IsDeleted == false && p.Id == id)
                .AsNoTracking()
                .Include(p => p.Seller)
                .Select(p => new ProductDeleteModel
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    SellerId = p.SellerId,
                    Seller = p.Seller.UserName!

                })
                .FirstOrDefaultAsync();

            if (model == null || model.SellerId != userId)
            {
                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductDeleteModel model)
        {
            var userId = GetUserId();

            var product = await context.Products
                .Where(p => p.Id == model.Id && p.IsDeleted == false)
                .FirstOrDefaultAsync();

            if (product != null && product.SellerId == userId)
            {
                product.IsDeleted = true;
                await context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
