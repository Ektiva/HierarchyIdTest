using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HierarchyIdTest1.Models;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;
using System.IO;
using System.Reflection;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;

namespace HierarchyIdTest1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DomainsController(AppDbContext context)
        {
            _context = context;
        }

        //// GET: api/Domains
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Domain>>> GetDomains()
        //{

        //     var result = await _context.Domains.ToListAsync();

        //    return result;
        //}

        // GET: api/Domains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Domaintest>> GetDomain(int id)
        {
            object domain = (from x in _context.Domains.Where(x => x.DomainId == id)
                             select new
                             {
                                 Id = x.DomainId,
                                 Name = x.DomainName,
                                 Type = x.DomainTypeId,
                                 Parent = x.Parentt,
                                 Path = x.Level.ToString(),
                                 HighLevel = x.HighLevel
                             }).FirstOrDefault();
            
            //var type = domain.GetType();
            //var strNode = (string) type.GetProperty("Path").GetValue(domain, null);

            dynamic dyn = domain;
            
            int myId = dyn.Id;
            string strName = dyn.Name;
            int iType = dyn.Type;
            string strParent = dyn.Parent;
            string strNode = dyn.Path;
            //int hLevel = dyn.HighLevel;
            SqlHierarchyId node = SqlHierarchyId.Parse(dyn.Path);
            int hLevel = (int) node.GetLevel();

            Domaintest result = new Domaintest(myId, strName, iType, strParent, strNode, hLevel);

            return result;
        }

        // GET: api/Domains/
        [HttpGet]
        public async Task<ActionResult<List<Domaintest>>> GetDomains()
        {
            object domain = (from x in _context.Domains
                             select new
                             {
                                 Id = x.DomainId,
                                 Name = x.DomainName,
                                 Type = x.DomainTypeId,
                                 Parent = x.Parentt,
                                 Path = x.Level.ToString(),
                                 HighLevel = x.HighLevel
                             }).ToList();

            //var type = domain.GetType();
            //var strNode = (string) type.GetProperty("Path").GetValue(domain, null);

            List<Domaintest> resultList = new List<Domaintest>();
            dynamic myDyn = domain;
            foreach(var dyn in myDyn)
            {
                int myId = dyn.Id;
                string strName = dyn.Name;
                int iType = dyn.Type;
                string strParent = dyn.Parent;
                string strNode = dyn.Path;
                int hLevel = dyn.HighLevel;
                SqlHierarchyId node = SqlHierarchyId.Parse(dyn.Path);

                Domaintest result = new Domaintest(myId, strName, iType, strParent, strNode, hLevel);
                resultList.Add(result);
            }



            return resultList;
        }

        // GET: api/Domains/
        [HttpGet("{GetDescendants}/{name}")]
        public async Task<ActionResult<List<Domaintest>>> GetDescendants(string name)
        {
            object mainDomain = (from x in _context.Domains.Where(x => x.DomainName == name)
                             select new
                             {
                                 Id = x.DomainId,
                                 Name = x.DomainName,
                                 Type = x.DomainTypeId,
                                 Parent = x.Parentt,
                                 Path = x.Level.ToString(),
                                 HighLevel = x.HighLevel
                             }).FirstOrDefault();

            dynamic mainDyn = mainDomain;

            int hL = mainDyn.HighLevel;
            SqlHierarchyId mainNode = SqlHierarchyId.Parse(mainDyn.Path);

            object domain = (from x in _context.Domains.Where(x => x.HighLevel > hL)
                             select new
                             {
                                 Id = x.DomainId,
                                 Name = x.DomainName,
                                 Type = x.DomainTypeId,
                                 Parent = x.Parentt,
                                 Path = x.Level.ToString(),
                                 HighLevel = x.HighLevel
                             }).ToList();
            //var type = domain.GetType();
            //var strNode = (string) type.GetProperty("Path").GetValue(domain, null);

            List<Domaintest> resultList = new List<Domaintest>();
            dynamic listDyn = domain;
            foreach (var dyn in listDyn)
            {
                int myId = dyn.Id;
                string strName = dyn.Name;
                int iType = dyn.Type;
                string strParent = dyn.Parent;
                string strNode = dyn.Path;
                int hLevel = dyn.HighLevel;
                SqlHierarchyId node = SqlHierarchyId.Parse(dyn.Path);

                Domaintest result = new Domaintest(myId, strName, iType, strParent, strNode, hLevel);
                if (node.IsDescendantOf(mainNode))
                {
                    resultList.Add(result);
                }
                
            }

            return resultList;
        }

        // PUT: api/Domains/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDomain(int id, Domain domain)
        //{
        //    if (id != domain.DomainId)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(domain).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DomainExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // PUT: api/Domains
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<Domain>> PutDomain(int id, Domain domain)
        {
            Domain toInsert = new Domain();
            toInsert.DomainName = domain.DomainName;
            toInsert.DomainTypeId = domain.DomainTypeId;
            toInsert.Parentt = domain.Parentt;
            toInsert.HighLevel = domain.HighLevel;
            //if (domain.Parentt == "N/A")
            //{
            //    SqlString level = new SqlString ( "/1/" );
            //    toInsert.Level = SqlHierarchyId.Parse("/1/");
            //}else
            //{
            //    //SqlString level = new SqlString("/1/2/");
            //    toInsert.Level = SqlHierarchyId.Parse("/1/3/");
            //}
            SqlHierarchyId id1 = new SqlHierarchyId();
            if (id == 2)
            {
                id1 = SqlHierarchyId.Parse("/1/1/");
            }
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    id1.Write(writer);
                    toInsert.Level = stream.ToArray();
                }
            }
            //_context.Domains.Add(toInsert);
            _context.Entry(toInsert).State = EntityState.Modified;
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

        // POST: api/Domains
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Domain>> PostDomain(Domaintest domain)
        {
            Domain toInsert = new Domain();
            toInsert.DomainName = domain.DomainName;
            toInsert.DomainTypeId = domain.DomainTypeId;
            toInsert.Parentt = domain.Parentt;
            toInsert.HighLevel = domain.HighLevel;
            //if (domain.Parentt == "N/A")
            //{
            //    SqlString level = new SqlString ( "/1/" );
            //    toInsert.Level = SqlHierarchyId.Parse("/1/");
            //}else
            //{
            //    //SqlString level = new SqlString("/1/2/");
            //    toInsert.Level = SqlHierarchyId.Parse("/1/3/");
            //}
            SqlHierarchyId id1 = SqlHierarchyId.Parse(domain.Path);
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    id1.Write(writer);
                    toInsert.Level = stream.ToArray();
                }
            }
            _context.Domains.Add(toInsert);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                
            }

            return CreatedAtAction("GetDomain", new { id = toInsert.DomainId }, toInsert);
        }

        // DELETE: api/Domains/5
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
