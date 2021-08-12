using DotNetCoreCRUD.Models;
using DotNetCoreCRUD.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCRUD.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _Context;
        private readonly IToastNotification _toastNotification;
        public MoviesController(ApplicationDbContext Context , IToastNotification toastNotification)
        {
            this._Context = Context;
            this._toastNotification = toastNotification;
        }
        //public IActionResult Index()
        //{
        //    var Movies = _Context.Movies.ToList();
        //    return View(Movies);
        //}
        public async Task<IActionResult>  Index()
        {
            var Movies =await  _Context.Movies.OrderByDescending(m=>m.Rate).ToListAsync();
            return View(Movies);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new MovieFormViewModel()
            {
                Genras = await _Context.Genras.OrderBy(m=>m.Name).ToListAsync()
            };
            return View("MovieForm" ,viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            var files = Request.Form.Files;
             if(!files.Any())
            {
                model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster" , "Please select poster !");
                return View("MovieForm",model);
            }
            var poster = files.SingleOrDefault();
            var allawedExtention = new List<string>() { ".jpg", ".png" };
            if(!allawedExtention.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please select poster (png or jpg )!");
                return View("MovieForm", model);
            }

            if (poster.Length > 1048576)
            {
                model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "poster cannot be more 1Mb!");
                return View("MovieForm", model);
            }


           using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);

            var movie = new Movie()
            {
                Titel = model.Titel,
                Year = model.Year,
                Rate = model.Rate,
                StoryLine = model.StoryLine,
                GenraId = model.GenraId,
                Poster = dataStream.ToArray()

            };

            _Context.Movies.Add(movie);
            _Context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie created successfully");
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var Movie =await  _Context.Movies.FindAsync(id);

            if (Movie == null)
                return NotFound();
            var viewModel = new MovieFormViewModel()
            {
                Id = Movie.Id,
                Titel = Movie.Titel,
                StoryLine = Movie.StoryLine,
                Poster=Movie.Poster,
                Year = Movie.Year,
                Rate = Movie.Rate,
                GenraId = Movie.GenraId,
                Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync()
            };
            return View("MovieForm", viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            var Movie = await _Context.Movies.FindAsync(model.Id);

            if (Movie == null)
                return NotFound();

            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.SingleOrDefault();
                using var dataStream = new MemoryStream();
                await poster.CopyToAsync(dataStream);

                var allawedExtention = new List<string>() { ".jpg", ".png" };
                model.Poster = dataStream.ToArray();
                if (!allawedExtention.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Please select poster (png or jpg )!");
                    return View("MovieForm", model);
                }

                if (poster.Length > 1048576)
                {
                    model.Genras = await _Context.Genras.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "poster cannot be more 1Mb!");
                    return View("MovieForm", model);
                }
                Movie.Poster = model.Poster;
            }

            Movie.Titel = model.Titel;
            Movie.Year = model.Year;
            Movie.Rate = model.Rate;
            Movie.StoryLine = model.StoryLine;
            Movie.GenraId = model.GenraId;

            _Context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Movie updated successfully");
            return RedirectToAction("Index");

        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var Movie = await _Context.Movies.Include(m => m.Genra).SingleOrDefaultAsync(m => m.Id == id);
            if (Movie == null)
                return NotFound();

            return View(Movie);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var movie = await _Context.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            _Context.Movies.Remove(movie);
            _Context.SaveChanges();

            return Ok();
        }
    }
}
