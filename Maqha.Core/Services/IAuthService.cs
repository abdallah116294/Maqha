using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maqha.Utilities.DTO;

namespace Maqha.Core.Services
{
    public interface IAuthService
    {
        Task<ResponseDTO<object>>GenerateToken(LoginDTO loginDTo);
    }
}
