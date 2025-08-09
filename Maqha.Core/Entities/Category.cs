using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.Entities
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        // Menu items in this category collection 
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}
