using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.Entities
{
    public class Order:BaseEntity
    {
        //Property for Total price of the order
        public decimal TotalPrice { get; set; }
        //Property for Order Status
        public string Status { get; set; }
        //Property for Order Date
        public DateTime? UpdatAt { get; set; }
        //Property for List Of Order Items
        public List<OrderItem> OrderItems { get; set; }
    }
}
