using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LabHate.Data;
using Lab4.Data;
using Lab4.Models;
using Microsoft.EntityFrameworkCore;

namespace LabHate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoTaskController : ControllerBase
    {
        private readonly TodoTaskDbContext _context;

        public TodoTaskController(TodoTaskDbContext context) => _context = context;
        [HttpGet]
        public async Task<IEnumerable<TodoTask>> Get()
            => await _context.TodoTasks.ToListAsync();

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoTask), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var todoTask = await _context.TodoTasks.FindAsync(id);
            return todoTask == null ? NotFound() : Ok(todoTask);
        }

        [HttpGet("searchurgency/{urgency}")]
        [ProducesResponseType(typeof(TodoTask), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<List<TodoTask>> GetByUrgency(Priority urgency)
        {
            var todoTasks = _context.TodoTasks.Where(x => x.Priority == urgency);
            return todoTasks == null ? NotFound() : Ok(todoTasks);
        }

        [HttpGet("searchlast")]
        [ProducesResponseType(typeof(TodoTask), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLast()
        {
            var todoTask = await _context.TodoTasks.OrderBy(x => x.Id).LastOrDefaultAsync();
            return todoTask == null ? NotFound() : Ok(todoTask); 
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create(TodoTask todoTask)
        {
            await _context.TodoTasks.AddAsync(todoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = todoTask.Id }, todoTask);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, TodoTask todoTask)
        {
            if (id != todoTask.Id) return BadRequest();

            _context.Entry(todoTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var todoTaskToDelete = await _context.TodoTasks.FindAsync(id);
            if (todoTaskToDelete == null) return NotFound();

            _context.TodoTasks.Remove(todoTaskToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 
