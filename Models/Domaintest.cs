using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchyIdTest1.Models
{
    public class Domaintest
    {
        [Key]
        public int DomainId { get; set; }
        [Required]
        [StringLength(256)]
        public string DomainName { get; set; }
        [Required]
        public int DomainTypeId { get; set; }

        [Required]
        public string Parentt { get; set; }
        //[Required]
        //[MaxLength(892)]
        //public byte[] Level { get; set; }
        public string Path { get; set; }
        public int HighLevel { get; set; }
        [Required]
        public SqlHierarchyId Node { get; set; }


        public Domaintest(int id, string name, int typeId, string parent, string path, int hLevel, SqlHierarchyId node)
        {
            DomainId = id;
            DomainName = name;
            DomainTypeId = typeId;
            Parentt = parent;
            Path = path;
            HighLevel = hLevel;
            Node = node;
        }
        public Domaintest(int id, string name, int typeId, string parent, string path, int hLevel)
        {
            DomainId = id;
            DomainName = name;
            DomainTypeId = typeId;
            Parentt = parent;
            Path = path;
            HighLevel = hLevel;
        }

        public Domaintest()
        {

        }

    }
}
