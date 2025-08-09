using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maqha.Repository.Identity
{
    public class AppIdentityRole:IdentityRole
    {
        public string Name { get; set; }
        public AppIdentityRole(string name): base(name)
        {
            Name = name;
        }        
    }
}
