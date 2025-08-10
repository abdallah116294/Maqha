using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.DTO
{
    public class CreateTableDTO
    {
        public int TableNumber { get; set; }
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
    }
}
