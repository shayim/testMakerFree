using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private AppDbContext _context;

        public QuizController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]/{num:int?}")]
        public async Task<IActionResult> ByRandom(int num = 10)
        {
            var quizzes = await _context.Quizzes.OrderBy(q => Guid.NewGuid()).Take(num).ToListAsync();

            return new JsonResult(quizzes.Adapt<QuizViewModel[]>());
        }

        [HttpGet("[action]/{num:int?}")]
        public async Task<IActionResult> ByTitle(int num = 10)
        {
            var quizzes = _context.Quizzes.OrderBy(q => q.Title).Take(num);
            return new JsonResult((await quizzes.ToListAsync()).Adapt<QuizViewModel[]>());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, string userId)
        {
            var quiz = await _context.Quizzes.SingleOrDefaultAsync(q => q.Id == id && q.UserId == userId);
            if (quiz == null) return NotFound();

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Get: api/quiz/{id}
        /// retrieve the Quiz with the given {id}
        /// </summary>
        /// <param name="id">The id of an existing quiz</param>
        /// <returns>the quiz with the given {id} or null</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);
            if (quiz == null) return NotFound();
            return Json(quiz.Adapt<QuizViewModel>());
        }

        [HttpGet("[action]/{num:int?}")]
        public async Task<IActionResult> Latest(int num = 10)
        {
            var quizzes = _context.Quizzes.OrderByDescending(q => q.LastModifiedDate).Take(num);

            return new JsonResult((await quizzes.ToListAsync()).Adapt<QuizViewModel[]>());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuizViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id != 0)
            {
                ModelState.AddModelError(nameof(model.Id), "Add New Quiz id should not be set");
                return BadRequest(ModelState);
            }

            var entity = _context.Quizzes.Add(model.Adapt<Quiz>()).Entity;
            await _context.SaveChangesAsync();

            //            return CreatedAtRoute(nameof(Get), new { id = entity.Id });
            return Ok(entity.Adapt<QuizViewModel>());
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]QuizViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id == 0)
            {
                ModelState.AddModelError(nameof(model.Id), "Edit Quiz id should be set");
                return BadRequest(ModelState);
            }

            var quiz = await _context.Quizzes.SingleOrDefaultAsync(q => q.Id == model.Id && q.UserId == model.UserId);
            if (quiz == null) return NotFound();
            try
            {
                quiz.Title = model.Title;
                quiz.Description = model.Description;
                quiz.Text = model.Text;
                quiz.Notes = model.Notes;
                quiz.LastModifiedDate = DateTime.Now;
                //                _context.Entry(quiz).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok();
        }
    }
}