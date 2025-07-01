using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BoardsController : ControllerBase
    {
        private readonly TodoContext _context;

        public BoardsController(TodoContext context)
        {
            _context = context;
        }

        // ✅ Tek bir board'ı ID ile getir (Vue tarafında fetch için gerekli)
        [HttpGet]
        public async Task<ActionResult<Board>> GetBoard()
        {
            var userId = User.FindFirst("sub")?.Value;
            var board = await _context.Boards
                .Where(b => b.UserId == userId)
                .Include(b => b.Lists)
                .ThenInclude(l => l.Cards.OrderBy(c => c.Order))
                .Select(b => new Board
                {
                    Id = b.Id,
                    Title = b.Title,
                    UserId = b.UserId,
                    Lists = b.Lists.Select(l => new ListModel
                    {
                        Id = l.Id,
                        Title = l.Title,
                        BoardId = l.BoardId,
                        Cards = l.Cards.OrderBy(c => c.Order).ToList()
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (board == null)
                return NotFound();

            return board;
        }

        // ➕ Yeni board oluştur
        [HttpPost]
        public async Task<ActionResult<Board>> CreateBoard(Board board)
        {
            board.UserId = User.Identity?.Name;

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoard), new { id = board.Id }, board);
        }
    }
}
