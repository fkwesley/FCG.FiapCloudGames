using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.DTO.Auth
{
    public class LoginRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
