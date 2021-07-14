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
    public class AutorItemsController : ControllerBase
    {
        private readonly AutorContext _context;

        public AutorItemsController(AutorContext context)
        {
            _context = context;
        }

        // GET: api/AutorItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AutorItemDTO>>> GetAutorItems()
        {
            return await _context.AutorItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/AutorItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AutorItemDTO>> GetAutorItem(long id)
        {
            var AutorItem = await _context.AutorItems.FindAsync(id);
            if (AutorItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(AutorItem);
        }

        [HttpPost]
        [Route("UpdateAutorItem")]
        public async Task<ActionResult<AutorItemDTO>> UpdateAutorItem(AutorItemDTO AutorItemDTO)
        {
            var AutorItem = await _context.AutorItems.FindAsync(AutorItemDTO.Id);
            if (AutorItem == null)
            {
                return NotFound();
            }
            AutorItem.Name = AutorItemDTO.Name;
            AutorItem.Secname = AutorItemDTO.Secname;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!AutorItemExists(AutorItemDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateAutorItem),
                new { id = AutorItem.Id },
                ItemToDTO(AutorItem));
        }

        [HttpPost]
        [Route("CreateAutorItem")]
        public async Task<ActionResult<AutorItemDTO>> CreateAutorItem(AutorItemDTO AutorItemDTO)
        {
            var AutorItem = new AutorItem
            {
                Name = AutorItemDTO.Name,
                Secname = AutorItemDTO.Secname
            };

            _context.AutorItems.Add(AutorItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAutorItem),
                new { id = AutorItem.Id },
                ItemToDTO(AutorItem));
        }

        // DELETE: api/AutorItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AutorItem>> DeleteAutorItem(long id)
        {
            var AutorItem = await _context.AutorItems.FindAsync(id);
            if (AutorItem == null)
            {
                return NotFound();
            }
            _context.AutorItems.Remove(AutorItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool AutorItemExists(long id) =>
            _context.AutorItems.Any(e => e.Id == id);

        private static AutorItemDTO ItemToDTO(AutorItem AutorItem) =>
            new AutorItemDTO
            {
                Id = AutorItem.Id,
                Name = AutorItem.Name,
                Secname = AutorItem.Secname,
            };
    }
}