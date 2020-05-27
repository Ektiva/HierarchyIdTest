using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HierarchyIdTest1.Models;

namespace HierarchyIdTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DomainTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DomainTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DomainType>>> GetDomainTypes()
        {
            return await _context.DomainTypes.ToListAsync();
        }

        // GET: api/DomainTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DomainType>> GetDomainType(int id)
        {
            var domainType = await _context.DomainTypes.FindAsync(id);

            if (domainType == null)
            {
                return NotFound();
            }

            return domainType;
        }

        // PUT: api/DomainTypes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDomainType(int id, DomainType domainType)
        {
            if (id != domainType.Id)
            {
                return BadRequest();
            }

            _context.Entry(domainType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomainTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DomainTypes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<DomainType>> PostDomainType(DomainType domainType)
        {
            _context.DomainTypes.Add(domainType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDomainType", new { id = domainType.Id }, domainType);
        }

        // DELETE: api/DomainTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DomainType>> DeleteDomainType(int id)
        {
            var domainType = await _context.DomainTypes.FindAsync(id);
            if (domainType == null)
            {
                return NotFound();
            }

            _context.DomainTypes.Remove(domainType);
            await _context.SaveChangesAsync();

            return domainType;
        }

        private bool DomainTypeExists(int id)
        {
            return _context.DomainTypes.Any(e => e.Id == id);
        }
    }
}
