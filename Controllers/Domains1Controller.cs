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
    public class Domains1Controller : ControllerBase
    {
        private readonly AppDbContext _context;

        public Domains1Controller(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Domains1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Domain>>> GetDomains()
        {
            return await _context.Domains.ToListAsync();
        }

        // GET: api/Domains1/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Domain>> GetDomain(int id)
        {
            var domain = await _context.Domains.FindAsync(id);

            if (domain == null)
            {
                return NotFound();
            }

            return domain;
        }

        // PUT: api/Domains1/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDomain(int id, Domain domain)
        {
            if (id != domain.DomainId)
            {
                return BadRequest();
            }

            _context.Entry(domain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DomainExists(id))
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

        // POST: api/Domains1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Domain>> PostDomain(Domain domain)
        {
            _context.Domains.Add(domain);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDomain", new { id = domain.DomainId }, domain);
        }

        // DELETE: api/Domains1/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Domain>> DeleteDomain(int id)
        {
            var domain = await _context.Domains.FindAsync(id);
            if (domain == null)
            {
                return NotFound();
            }

            _context.Domains.Remove(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        private bool DomainExists(int id)
        {
            return _context.Domains.Any(e => e.DomainId == id);
        }
    }
}
