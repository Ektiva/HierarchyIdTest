using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HierarchyIdTest1.Models;
using Microsoft.SqlServer.Types;
using HierarchyIdTest1.Helper;

namespace HierarchyIdTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewDomainsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public NewDomainsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/NewDomains
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewDomain>>> GetNewDomains()
        {
            return await _context.NewDomains.ToListAsync();
        }

        // GET: api/NewDomains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewDomain>> GetNewDomain(int id)
        {
            var newDomain = await _context.NewDomains.FindAsync(id);

            if (newDomain == null)
            {
                return NotFound();
            }

            return newDomain;
        }

        // GET: api/NewDomains/GetLastChild/Root
        [HttpGet("{GetLastChild}/{name}")]
        public async Task<ActionResult<NewDomain>> GetLastChild(string name)
        {
            var listNode = _context.NewDomains.Where(x => x.Parentt == name)
                .OrderByDescending(x => x.Node)
                .FirstOrDefault();

            return listNode;
        }

        // PUT: api/NewDomains/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewDomain(int id, NewDomain newDomain)
        {
            if (id != newDomain.DomainId)
            {
                return BadRequest();
            }

            _context.Entry(newDomain).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewDomainExists(id))
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

        // POST: api/NewDomains
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<NewDomain>> PostNewDomain(NewDomain newDomain)
        {
            //Check if the Domain Type is valid
            if (newDomain.DomainTypeId > 7 || newDomain.DomainTypeId < 1)
            {
                return BadRequest(new { message = @"The Domain Type Id is not valid. Valid numbers => 1: Root, 2: Company, 3: Sales Division, 4: Sales District, 5: Profit Center, 6: Distribution Center, 7: Operation Center" });
            }

            if (newDomain.Parentt == "N/A")
            {
                newDomain.NodePath = "/1/";
            }
            else
            {
                var parentNode = new byte[2147483591];
                var lastChild = new NewDomain();
                try
                {
                    parentNode = _context.NewDomains.FirstOrDefault(x => x.DomainName == newDomain.Parentt).Node;
                }
                catch(Exception ex)
                {
                    return BadRequest(new { message = "Parent does not exist. Please verified the spelling or Add this as a parent first. => Error message: " + ex.Message });
                }

                try
                {
                    lastChild = _context.NewDomains.Where(x => x.Parentt == newDomain.Parentt)
                        .OrderByDescending(x => x.Node)
                        .FirstOrDefault();

                    SqlHierarchyId lastSqlNode = HierarchyExtensions.ToSqlHierarchyId(lastChild.Node);

                    newDomain.Node = HierarchyExtensions.ToByteArray(HierarchyExtensions.ToSqlHierarchyId(parentNode).GetDescendant(lastSqlNode, new SqlHierarchyId()));
                }
                catch (Exception ex)
                {
                    newDomain.Node = HierarchyExtensions.ToByteArray(HierarchyExtensions.ToSqlHierarchyId(parentNode).GetDescendant(new SqlHierarchyId(), new SqlHierarchyId()));
                }
            }
            _context.NewDomains.Add(newDomain);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNewDomain", new { id = newDomain.DomainId }, newDomain);
        }

        // DELETE: api/NewDomains/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NewDomain>> DeleteNewDomain(int id)
        {
            var newDomain = await _context.NewDomains.FindAsync(id);
            if (newDomain == null)
            {
                return NotFound();
            }

            _context.NewDomains.Remove(newDomain);
            await _context.SaveChangesAsync();

            return newDomain;
        }

        private bool NewDomainExists(int id)
        {
            return _context.NewDomains.Any(e => e.DomainId == id);
        }
    }
}
