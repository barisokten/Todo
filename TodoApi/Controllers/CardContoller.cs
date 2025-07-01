// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Authorization;

// using TodoApi.Models;

// namespace TodoApi.Controllers
// {
//     [Authorize]
//     [ApiController]
//     [Route("api/[controller]")]
//     public class CardController : ControllerBase
//     {
//         private readonly TodoContext _context;

//         public CardController(TodoContext context)
//         {
//             _context = context;
//         }

//         // Tüm kartlar (isteğe bağlı)
//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Card>>> GetAllCards()
//         {
//             return await _context.Cards.ToListAsync();
//         }

//         // Kart oluştur
//         [HttpPost]
//         public async Task<ActionResult<Card>> CreateCard(Card card)
//         {
//             _context.Cards.Add(card);
//             await _context.SaveChangesAsync();
//             return CreatedAtAction(nameof(CreateCard), new { id = card.Id }, card);
//         }

//         // Kart güncelle
//         [HttpPut("{id}")]
//         public async Task<IActionResult> UpdateCard(int id, Card card)
//         {
//             if (id != card.Id)
//                 return BadRequest();

//             _context.Entry(card).State = EntityState.Modified;
//             await _context.SaveChangesAsync();

//             return NoContent();
//         }

//         // Kart sil
//         [HttpDelete("{id}")]
//         public async Task<IActionResult> DeleteCard(int id)
//         {
//             var card = await _context.Cards.FindAsync(id);
//             if (card == null)
//                 return NotFound();

//             _context.Cards.Remove(card);
//             await _context.SaveChangesAsync();

//             return NoContent();
//         }
//     }
// }
