using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using Microsoft.AspNetCore.Authorization;



namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        private readonly TodoContext _context;

        public ListController(TodoContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<ListModel>> CreateList(ListModel list)
        {
            _context.Lists.Add(list);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreateList), new { id = list.Id }, list);
        }

        [HttpGet("board/{boardId}")]
        public async Task<ActionResult<IEnumerable<ListModel>>> GetListsByBoard(int boardId)
        {
            return await _context.Lists
                .Where(l => l.BoardId == boardId)
                .Include(l => l.Cards)
                .Select(l => new ListModel
                {
                    Id = l.Id,
                    Title = l.Title,
                    BoardId = l.BoardId,
                    Cards = l.Cards.OrderBy(c => c.Order).ToList()
                })
                .ToListAsync();
        }
    }
}
