using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Produces("application/json")]
    [Route("api/quiz/{quizId}/[controller]")]
    public class QuestionController : Controller
    {
        private readonly AppDbContext _context;

        public QuestionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int quizId, int id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(q => q.QuizId == quizId && q.Id == id);
            if (question == null) return NotFound("question not found");

            _context.Entry(question).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int quizId, int id)
        {
            var question = await _context.Questions.SingleOrDefaultAsync(q => q.QuizId == quizId && q.Id == id);
            if (question == null) return NotFound("question not found");

            return Json(question.Adapt<QuestionViewModel>());
        }

        [HttpGet]
        public IActionResult GetAll(int quizId)
        {
            var questions = _context.Questions.Where(q => q.QuizId == quizId);

            return Json(questions.Adapt<QuestionViewModel[]>());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]QuestionViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id != 0)
            {
                ModelState.AddModelError("id", "for adding question id should not be set");
                return BadRequest(ModelState);
            }
            if (model.QuizId == 0)
            {
                ModelState.AddModelError("quizId", "for adding question quizId should be set");

                return BadRequest(ModelState);
            }

            var quiz = _context.Quizzes.Find(model.QuizId);
            if (quiz == null) return NotFound("quiz not found");

            var newQuestion = new Question
            {
                Text = model.Text,
                QuizId = model.QuizId
            };

            _context.Add(newQuestion);

            await _context.SaveChangesAsync();

            return Ok(newQuestion.Adapt<QuizViewModel>());
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]QuestionViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var question = await _context.Questions.SingleOrDefaultAsync(q => q.QuizId == model.QuizId && q.Id == model.Id);
            if (question == null) return NotFound("question not found");

            question.Text = model.Text;
            question.LastModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}