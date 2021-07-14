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
    public class FilmyItemsController : ControllerBase
    {
        private readonly FilmyContext _context;

        public FilmyItemsController(FilmyContext context)
        {
            _context = context;
        }

        // GET: api/FilmyItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FilmyItemDTO>>> GetFilmyItems()
        {
            return await _context.FilmyItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/FilyItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FilmyItemDTO>> GetFilmyItem(long id)
        {
            var FilmyItem = await _context.FilmyItems.FindAsync(id);
            if (FilmyItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(FilmyItem);
        }

        [HttpPost]
        [Route("UpdateFilmyItem")]
        public async Task<ActionResult<FilmyItemDTO>> UpdateFilmyItem(FilmyItemDTO FilmyItemDTO)
        {
            var FilmyItem = await _context.FilmyItems.FindAsync(FilmyItemDTO.Id);
            if (FilmyItem == null)
            {
                return NotFound();
            }
            FilmyItem.Filmyname = FilmyItemDTO.Filmyname;
            FilmyItem.Author = FilmyItemDTO.Author;
            FilmyItem.Rating = FilmyItemDTO.Rating;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!FilmyItemExists(FilmyItemDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateFilmyItem),
                new { id = FilmyItem.Id },
                ItemToDTO(FilmyItem));
        }

        [HttpPost]
        [Route("CreateFilmyItem")]
        public async Task<ActionResult<FilmyItemDTO>> CreateFilmyItem(FilmyItemDTO FilmyItemDTO)
        {
            var FilmyItem = new FilmyItem
            {
                Filmyname = FilmyItemDTO.Filmyname,
                Author = FilmyItemDTO.Author,
                Rating = FilmyItemDTO.Rating
            };

            _context.FilmyItems.Add(FilmyItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetFilmyItem),
                new { id = FilmyItem.Id },
                ItemToDTO(FilmyItem));
        }

        // DELETE: api/FilmyItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FilmyItem>> DeleteFilmyItem(long id)
        {
            var FilmyItem = await _context.FilmyItems.FindAsync(id);
            if (FilmyItem == null)
            {
                return NotFound();
            }
            _context.FilmyItems.Remove(FilmyItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool FilmyItemExists(long id) =>
            _context.FilmyItems.Any(e => e.Id == id);

        private static FilmyItemDTO ItemToDTO(FilmyItem FilmyItem) =>
            new FilmyItemDTO
            {
                Id = FilmyItem.Id,
                Filmyname = FilmyItem.Filmyname,
                Author = FilmyItem.Author,
                Rating = FilmyItem.Rating,
            };
    }
}
