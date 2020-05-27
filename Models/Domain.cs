using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using System.Data.Entity.Hierarchy;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchyIdTest1.Models
{
    public class Domain
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
        public int HighLevel { get; set; }
        // [Required]
        //[MaxLength(892)]
        //public SqlHierarchyId Level { get; set; }
        public byte[] Level { get; set; }
    }
}
