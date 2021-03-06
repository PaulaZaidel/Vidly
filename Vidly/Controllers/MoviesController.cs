﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Linq;
using System.Data.Entity;
using Vidly.ViewModels;
using System.Data.Entity.Validation;

namespace Vidly.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Movies
        public ActionResult Index(int? pageIndex, string sortBy)
        {
            var movies = _context.Movies.Include(c => c.Genre).ToList();

            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);

        }

        public ActionResult MovieForm()
        {
            MovieViewModel modelView = new MovieViewModel
            {
                Genres = _context.Genries.ToList()
            };

            return View(modelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            try
            {
                if (movie.Id == 0)
                {
                    _context.Movies.Add(movie);

                }else
                {
                    var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);
                    movieInDb.Name = movie.Name;
                    movieInDb.ReleaseDate = movie.ReleaseDate;
                    movieInDb.GenreId = movie.GenreId;
                    movieInDb.NumberInStock = movie.NumberInStock;
                }

                _context.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                Console.WriteLine(e);
            }

            return RedirectToAction("Index", "Movies");
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            var viewModel = new MovieViewModel(movie)
            {
                Genres = _context.Genries.ToList()
            };

            return View("MovieForm", viewModel);
        }
    }
}