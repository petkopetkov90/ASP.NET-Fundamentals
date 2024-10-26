using GameZone.Data;
using GameZone.Data.Models;
using GameZone.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GameZone.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly GameZoneDbContext context;

        public GameController(GameZoneDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            ICollection<GameViewModel> models = await context.Games
                .AsNoTracking()
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName,
                    ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd")
                })
                .ToListAsync();


            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new GameInputModel
            {
                Genres = await context.Genres
                    .AsNoTracking()
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(GameInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var game = new Game
            {
                Title = model.Title,
                Description = model.Description,
                GenreId = model.GenreId,
                ImageUrl = model.ImageUrl,
                ReleasedOn = DateTime.Parse(model.ReleasedOn),
                PublisherId = userId
            };

            await context.Games.AddAsync(game);
            await context.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> MyZone()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ICollection<GameViewModel> models = await context.Games
                .Include(g => g.GamersGames)
                .ThenInclude(gg => gg.Gamer)
                .AsNoTracking()
                .Where(g => g.GamersGames.Any(gg => gg.GamerId == userId))
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl,
                    Genre = g.Genre.Name,
                    Publisher = g.Publisher.UserName,
                    ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd")
                })
                .ToListAsync();


            return View(models);
        }

        [HttpGet]
        public async Task<IActionResult> AddToMyZone(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var game = await context.Games
                .Include(g => g.GamersGames)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return RedirectToAction("All");
            }

            if (game!.GamersGames.Any(gg =>
                    (gg.Game == game && gg.GamerId == userId)))
            {
                return RedirectToAction("All");
            }

            var gamerGame = new GamerGame
            {
                Game = game!,
                GamerId = userId
            };

            game!.GamersGames.Add(gamerGame);

            await context.SaveChangesAsync();

            return RedirectToAction("MyZone");
        }

        [HttpGet]
        public async Task<IActionResult> StrikeOut(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var game = await context.Games
                .Include(g => g.GamersGames)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                RedirectToAction("All");
            }

            var gamerGame = game!.GamersGames.FirstOrDefault(gg =>
                (gg.Game == game && gg.GamerId == userId));

            if (gamerGame != null)
            {
                game.GamersGames.Remove(gamerGame);
                await context.SaveChangesAsync();
            }

            return RedirectToAction("MyZone");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await context.Games
                .AsNoTracking()
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                var refererUrl = Request.Headers["Referer"].ToString();

                if (!string.IsNullOrEmpty(refererUrl))
                {
                    return Redirect(refererUrl);
                }

                return RedirectToAction("All");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (game.PublisherId != userId)
            {
                return RedirectToAction("All");
            }

            var model = new GameInputModel
            {
                Id = game.Id,
                Title = game.Title,
                ImageUrl = game.ImageUrl,
                Description = game.Description,
                ReleasedOn = game.ReleasedOn.ToString("yyyy-MM-dd"),
                GenreId = game.GenreId,
                Genres = await context.Genres
                    .AsNoTracking()
                    .ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GameInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var game = await context.Games.FindAsync(model.Id);

            if (game == null || game.PublisherId != userId)
            {
                return RedirectToAction("All");
            }

            game.Title = model.Title;
            game.ImageUrl = model.ImageUrl;
            game.Description = model.Description;
            game.ReleasedOn = DateTime.Parse(model.ReleasedOn);
            game.GenreId = model.GenreId;

            await context.SaveChangesAsync();

            return RedirectToAction("All");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GameViewModel
                {
                    Id = g.Id,
                    ImageUrl = g.ImageUrl,
                    Title = g.Title,
                    Description = g.Description,
                    Genre = g.Genre.Name,
                    ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd"),
                    Publisher = g.Publisher.UserName

                })
                .FirstOrDefaultAsync();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await context.Games
                .AsNoTracking()
                .Where(g => g.Id == id)
                .Select(g => new GameViewModel
                {
                    ImageUrl = g.ImageUrl,
                    Title = g.Title,
                    Description = g.Description,
                    Genre = g.Genre.Name,
                    ReleasedOn = g.ReleasedOn.ToString("yyyy-MM-dd"),
                    Publisher = g.Publisher.UserName

                })
                .FirstOrDefaultAsync();

            if (model == null)
            {
                return RedirectToAction("All");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(GameViewModel model)
        {
            var game = await context.Games.FindAsync(model.Id);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (game == null || game.PublisherId != userId)
            {
                var refererUrl = Request.Headers["Referer"].ToString();

                if (!string.IsNullOrEmpty(refererUrl))
                {
                    return Redirect(refererUrl);
                }

            }

            context.Games.Remove(game!);
            await context.SaveChangesAsync();

            return RedirectToAction("All");
        }
    }
}
