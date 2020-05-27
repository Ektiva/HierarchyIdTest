using HierarchyIdTest1.Helper;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HierarchyIdTest1.Models
{
    public class NewDomain
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
        [MaxLength(892)]
        public byte[] Node
        {
            get { return node; }
            set
            {
                node = value;
                nodeSql = node.ToSqlHierarchyId();
            }
        }

        [NotMapped]
        public string NodePath
        {
            //get => nodeSql.ToString();
            //set => Node = value.ToSqlHierarchyId().ToByteArray();
            get { return nodeSql.ToString(); }
            set { Node = value.ToSqlHierarchyId().ToByteArray(); }
        }

        [NotMapped]
        public SqlHierarchyId nodeSql;

        //Privates attributes
        private byte[] node;
        

    }
}
