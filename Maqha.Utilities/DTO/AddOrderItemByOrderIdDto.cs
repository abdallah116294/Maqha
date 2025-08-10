using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.DTO
{
    public class AddOrderItemByOrderIdDto
    {
        public int OrderId { get; set; }
        public CreateOrderItemDTO OrderItem { get; set; }
    }
}
