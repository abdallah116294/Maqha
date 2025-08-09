using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Utilities.DTO
{
    public class UpdateMenuItemDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        //file 
        public IFormFile? ImageFile { get; set; }
        public bool? IsAvailable { get; set; }
        public int? CategoryId { get; set; }
    }
}
