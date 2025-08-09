using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.DTO
{
    public  class ResultCategoryWithMenuItemDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<CategoriesMenuItemDTO> MenuItems { get; set; }
    }
}
