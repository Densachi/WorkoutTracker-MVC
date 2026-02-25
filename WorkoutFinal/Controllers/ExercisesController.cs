using Azure.Core.Pipeline;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Collections.Generic;
using WorkoutFinal.Models;
using WorkoutFinal.Services;

namespace WorkoutFinal.Controllers
{
    public class ExercisesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment environment;

        public ExercisesController(ApplicationDbContext context, IWebHostEnvironment environment) 
        {
            this.context = context;
            this.environment = environment;
        }
        public IActionResult Index()
        {
            var exercises = context.Exercises.ToList();
            return View(exercises);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ExerciseDto exerciseDto)
        {
            if (exerciseDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }

            if (!ModelState.IsValid)
            {
                return View(exerciseDto);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(exerciseDto.ImageFile!.FileName);

            string imageFullPath = environment.WebRootPath + "/exercises/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                exerciseDto.ImageFile.CopyTo(stream);
            }

            Exercise exercise = new Exercise()
            {
                Name = exerciseDto.Name,
                Type = exerciseDto.Type,
                Set = exerciseDto.Set,
                Rep = exerciseDto.Rep,
                Description = exerciseDto.Description,
                ImageFileName = newFileName,
                CreatedAt = DateTime.Now,
            };

            context.Exercises.Add(exercise);
            context.SaveChanges();


            return RedirectToAction("Index", "Exercises");




        }

        public IActionResult Edit(int id)
        {
            var exercise = context.Exercises.Find(id);

            if(exercise == null)
            {
                return RedirectToAction("Index", "Exercises");
            }

            var exerciseDto = new ExerciseDto()
            {
                Name = exercise.Name,
                Type = exercise.Type,
                Set = exercise.Set,
                Rep = exercise.Rep,
                Description = exercise.Description,
            };

            ViewData["ExerciseId"] = exercise.Id;
            ViewData["ImageFileName"] = exercise.ImageFileName;
            ViewData["CreatedAt"] = exercise.CreatedAt.ToString("MM/dd/yyyy");

            return View(exerciseDto);
        }

        [HttpPost]
        public IActionResult Edit(int id, ExerciseDto exerciseDto)
        {
            var exercise = context.Exercises.Find(id);

            if (exercise == null)
            {
                return RedirectToAction("Index", "Exercises");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ExerciseId"] = exercise.Id;
                ViewData["ImageFileName"] = exercise.ImageFileName;
                ViewData["CreatedAt"] = exercise.CreatedAt.ToString("MM/dd/yyyy");

                return View(exerciseDto);
            }

            string newFileName = exercise.ImageFileName;
            if (exerciseDto.ImageFile != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(exerciseDto.ImageFile.FileName);

                string imageFullPath = environment.WebRootPath + "/exercises/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    exerciseDto.ImageFile.CopyTo(stream);
                }

                string oldImageFullPath = environment.WebRootPath + "/exercises/" + exercise.ImageFileName;
                System.IO.File.Delete(imageFullPath);

            }

            exercise.Name = exerciseDto.Name;
            exercise.Type = exerciseDto.Type;
            exercise.Set = exerciseDto.Set;
            exercise.Rep = exerciseDto.Rep;
            exercise.Description = exerciseDto.Description;
            exercise.ImageFileName = newFileName;

            context.SaveChanges();
            return RedirectToAction("Index", "Exercises");
        }
        
            public IActionResult Delete(int id) 
            {
                var exercise = context.Exercises.Find(id);
                if (exercise == null) 
                {

                    return RedirectToAction("Index", "Exercises");
                }
                string imageFullPath = environment.WebRootPath + "/exercises/" + exercise.ImageFileName;
                System.IO.File.Delete(imageFullPath);

                context.Exercises.Remove(exercise);
                context.SaveChanges();
                return RedirectToAction("Index", "Exercises");
            }

        
    }
}
