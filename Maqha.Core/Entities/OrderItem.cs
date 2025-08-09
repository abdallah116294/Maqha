using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Core.Entities
{
    public class OrderItem:BaseEntity
    {
        //FK for Order 
        public int OrderId { get; set; }
        //Fk for MenuItem
        public int MenuItemId { get; set; }
        //Navigation Property for MenuItem
        public MenuItem MenuItem { get; set; }
        //Quantity of the item in the order
        public int Quantity { get; set; }
        //Price of the item in the order
        public decimal Price { get; set; }


    }
}
