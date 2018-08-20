using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.ViewModels;
using TestMakerFreeWebApp.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TestMakerFreeWebApp.Controllers
{
    [Authorize]
    [Route("api/quiz/{quizId}/question/{qId}/[controller]")]
    public class AnswerController : Controller
    {
        private readonly AppDbContext _context;

        public AnswerController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Delete: api/answer/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer == null) return NotFound($"no answer id = {id} found");

            _context.Entry(answer).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{answerId}")]
        public async Task<IActionResult> Get(int answerId)
        {
            var answer = await _context.Answers.FindAsync(answerId);
            if (answer == null) return NotFound($"no answer id = {answerId} found");

            return Json(answer.Adapt<AnswerViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int qId)
        {
            var answers = _context.Answers.Where(a => a.QuestionId == qId);

            return Json((await answers.ToListAsync()).Adapt<AnswerViewModel[]>());
        }

        /// <summary>
        /// Post: api/answer
        /// Add an new answer
        /// </summary>
        /// <param name="model">a new answer</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AnswerViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id != 0)
            {
                ModelState.AddModelError("id", "Adding an answer, id should not be set");
                return BadRequest(ModelState);
            }

            if (!await _context.Questions.AnyAsync(q => q.Id == model.QuestionId && q.QuizId == model.QuizId))
            {
                return NotFound("no question found");
            }

            var answer = new Answer
            {
                QuizId = model.QuizId,
                QuestionId = model.QuestionId,
                Value = model.Value,
                Text = model.Text
            };

            await _context.AddAsync(answer);
            await _context.SaveChangesAsync();

            return Ok(answer.Adapt<AnswerViewModel>());
        }

        /// <summary>
        /// Put: api/answer/{id}
        /// Edit the answer with the given {id}
        /// </summary>
        /// <param name="id">the specific answer's id</param>
        /// <param name="model">the specific edited answer</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AnswerViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id == 0)
            {
                ModelState.AddModelError("id", "update an answer, id should be set");
                return BadRequest(ModelState);
            }

            var answer = await _context.Answers.FindAsync(model.Id);
            if (answer == null) return NotFound(@"no answer id = {model.id} found ");

            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}