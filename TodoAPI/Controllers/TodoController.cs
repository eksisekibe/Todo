using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Data;
using TodoAPI.Entity;
using Microsoft.EntityFrameworkCore;
using Todo.Data;

namespace TodoAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    
    public class TodoController : ControllerBase
    {
        private readonly DatabaseContexts _dbcontext;

        public TodoController(DatabaseContexts dbcontext)
        {
            _dbcontext = dbcontext;
        }

        // GET `/GetAll`
        /// <summary>
        /// Tüm kayıtlı kullanıcıları alır.
        /// </summary>
        /// <remarks>Sistem tarafından desteklenen tüm kullanıcılar ve bu kullanıcıların kimlikleri bu uç nokta ile alınabilir.</remarks>
        /// <response code="200">Success</response>
        /// <response code="500">Oops! Can't response right now!</response>

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll()
        {
            return await _dbcontext.TodoItems.ToListAsync();
        }

        //GET `/Get/{id}`
        /// </summary>
        /// <remarks>Belirli bir kullanıcı bilgisi alın</remarks>
        /// <param name="id">Kullanıcı Tanımlayıcı</param>
        /// <response code="200">Success</response>
        /// <response code="500">Oops! Can't response right now!</response>
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> Get(long id)
        {
            var todo = await _dbcontext.TodoItems.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return todo;
        }

        //PUT `/Put/{id}`
        /// <summary>
        ///  Kullanıcıları Güncelle
        /// </summary>
        /// <param name="id">Kullanıcı Tanımlayıcı</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Oops! Can't response right now!</response>
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, TodoItem todo)
        {
            todo.Id = id;

            _dbcontext.Entry(todo).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok($"Todo {id} updated successfully!");
        }

        //POST `/Create/{id}`
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<ActionResult<TodoItem>> Create(long id, TodoItem todoItem)
        {
            todoItem.Id = id;
            _dbcontext.TodoItems.Add(todoItem);
            await _dbcontext.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
        }

        //DELETE `/Delete/{id}`
        /// <summary>
        ///  Kullanıcıları Sil
        /// </summary>
        /// <param name="id">Kullanıcı Tanımlayıcı</param>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Oops! Can't response right now!</response>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var todoItem = await _dbcontext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _dbcontext.TodoItems.Remove(todoItem);
            await _dbcontext.SaveChangesAsync();

            return Ok($"Todo {id} deleted successfully!");
        }

        private bool TodoItemExists(long id)
        {
            return _dbcontext.TodoItems.Any(x => x.Id == id);
        }
    }
}
