using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.Entities
{
    public class Table:BaseEntity
    {
        public int TableNumber { get; set; }
        public int Capactiy { get; set; }
        public bool IsActive { get; set; }
    }
}
