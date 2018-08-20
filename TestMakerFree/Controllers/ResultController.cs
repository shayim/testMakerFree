using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TestMakerFreeWebApp.Data;
using TestMakerFreeWebApp.Models;
using TestMakerFreeWebApp.ViewModels;

namespace TestMakerFreeWebApp.Controllers
{
    [Authorize]
    [Route("api/quiz/{quizId}/[controller]")]
    public class ResultController : Controller
    {
        private readonly AppDbContext _context;

        public ResultController(AppDbContext context)
        {
            _context = context;
        }

        [HttpDelete("{resultId}")]
        public async Task<IActionResult> Delete(int resultId)
        {
            var result = await _context.Results.FindAsync(resultId);
            if (result == null) return NotFound(@"no result id = {resultId} found");

            _context.Entry(result).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{resultId}")]
        public async Task<IActionResult> Get(int resultId)
        {
            var result = await _context.Results.FindAsync(resultId);
            if (result == null) return NotFound(@"no result id = {resultId} found");

            return Json(result.Adapt<ResultViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int quizId)
        {
            var results = _context.Results.Where(r => r.QuizId == quizId);

            return Json((await results.ToListAsync()).Adapt<ResultViewModel[]>());
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ResultViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id != 0)
            {
                ModelState.AddModelError("id", "Add new result the id should not be set.");
                return BadRequest(ModelState);
            }
            if (!_context.Quizzes.Any(q => q.Id == model.QuizId))
            {
                return NotFound(@"no quiz id = {model.quizId} found");
            }

            var result = new Result
            {
                QuizId = model.QuizId,
                Text = model.Text,
                MinValue = model.MinValue,
                MaxValue = model.MaxValue
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            return Json(result.Adapt<ResultViewModel>());
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]ResultViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (model.Id == 0)
            {
                ModelState.AddModelError("id", "Add new result the id should be set.");
                return BadRequest(ModelState);
            }

            var result = await _context.Results.FindAsync(model.Id);
            if (result == null) return NotFound(@"result id = {model.Id} not found");

            result.MaxValue = model.MaxValue;
            result.MinValue = model.MinValue;
            result.Text = model.Text;
            result.LastModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}