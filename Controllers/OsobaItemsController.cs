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
    public class OsobaItemsController : ControllerBase
    {
        private readonly OsobaContext _context;

        public OsobaItemsController(OsobaContext context)
        {
            _context = context;
        }

        // GET: api/OsobaItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OsobaItemDTO>>> GetOsobaItems()
        {
            return await _context.OsobaItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/OsobaItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OsobaItemDTO>> GetOsobaItem(long id)
        {
            var OsobaItem = await _context.OsobaItems.FindAsync(id);
            if (OsobaItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(OsobaItem);
        }

        [HttpPost]
        [Route("UpdateOsobaItem")]
        public async Task<ActionResult<OsobaItemDTO>> UpdateOsobaItem(OsobaItemDTO OsobaItemDTO)
        {
            var OsobaItem = await _context.OsobaItems.FindAsync(OsobaItemDTO.Id);
            if (OsobaItem == null)
            {
                return NotFound();
            }
            OsobaItem.Name = OsobaItemDTO.Name;
            OsobaItem.Secname = OsobaItemDTO.Secname;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!OsobaItemExists(OsobaItemDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateOsobaItem),
                new { id = OsobaItem.Id },
                ItemToDTO(OsobaItem));
        }

        [HttpPost]
        [Route("CreateOsobaItem")]
        public async Task<ActionResult<OsobaItemDTO>> CreateOsobaItem(OsobaItemDTO OsobaItemDTO)
        {
            var OsobaItem = new OsobaItem
            {
                Name = OsobaItemDTO.Name,
                Secname = OsobaItemDTO.Secname
            };

            _context.OsobaItems.Add(OsobaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetOsobaItem),
                new { id = OsobaItem.Id },
                ItemToDTO(OsobaItem));
        }

        // DELETE: api/OsobaItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OsobaItem>> DeleteOsobaItem(long id)
        {
            var OsobaItem = await _context.OsobaItems.FindAsync(id);
            if (OsobaItem == null)
            {
                return NotFound();
            }
            _context.OsobaItems.Remove(OsobaItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool OsobaItemExists(long id) =>
            _context.OsobaItems.Any(e => e.Id == id);

        private static OsobaItemDTO ItemToDTO(OsobaItem OsobaItem) =>
            new OsobaItemDTO
            {
                Id = OsobaItem.Id,
                Name = OsobaItem.Name,
                Secname = OsobaItem.Secname,
            };
    }
}
