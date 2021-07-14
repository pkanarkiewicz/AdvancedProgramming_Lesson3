using AdvancedProgramming_Lesson3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdvancedProgramming_Lesson3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookItemsController : ControllerBase
    {
        private readonly BookContext _context;

        public BookItemsController(BookContext context)
        {
            _context = context;
        }

        // GET: api/BookItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookItemDTO>>> GetBookItems()
        {
            return await _context.BookItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/BookItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookItemDTO>> GetBookItem(long id)
        {
            var BookItem = await _context.BookItems.FindAsync(id);
            if (BookItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(BookItem);
        }

        [HttpPost]
        [Route("UpdateBookItem")]
        public async Task<ActionResult<BookItemDTO>> UpdateBookItem(BookItemDTO BookItemDTO)
        {
            var BookItem = await _context.BookItems.FindAsync(BookItemDTO.Id);
            if (BookItem == null)
            {
                return NotFound();
            }
            BookItem.Bookname = BookItemDTO.Bookname;
            BookItem.Author = BookItemDTO.Author;
            BookItem.Rating = BookItemDTO.Rating;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookItemExists(BookItemDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateBookItem),
                new { id = BookItem.Id },
                ItemToDTO(BookItem));
        }

        [HttpPost]
        [Route("CreateBookItem")]
        public async Task<ActionResult<BookItemDTO>> CreateBookItem(BookItemDTO BookItemDTO)
        {
            var BookItem = new BookItem
            {
                Bookname = BookItemDTO.Bookname,
                Author = BookItemDTO.Author,
                Rating = BookItemDTO.Rating
            };

            _context.BookItems.Add(BookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBookItem),
                new { id = BookItem.Id },
                ItemToDTO(BookItem));
        }

        // DELETE: api/BookItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookItem>> DeleteBookItem(long id)
        {
            var BookItem = await _context.BookItems.FindAsync(id);
            if (BookItem == null)
            {
                return NotFound();
            }
            _context.BookItems.Remove(BookItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool BookItemExists(long id) =>
            _context.BookItems.Any(e => e.Id == id);

        private static BookItemDTO ItemToDTO(BookItem BookItem) =>
            new BookItemDTO
            {
                Id = BookItem.Id,
                Bookname = BookItem.Bookname,
                Author = BookItem.Author,
                Rating = BookItem.Rating,
            };
    }
}