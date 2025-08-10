using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.DTO
{
    public class CreateOrderDTO
    {
        public int TableId { get; set; }
        public List<CreateOrderItemDTO> OrderItems { get; set; }
    }
}
