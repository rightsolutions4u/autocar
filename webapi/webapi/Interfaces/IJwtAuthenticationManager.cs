using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Interfaces
{
    //added by Mohtashim on 27/08/2020
    public interface IJwtAuthenticationManager
    {
        string Authenticate(string username, string password);
    }
}
