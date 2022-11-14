using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorbadoLibrary
{
    public class AuthData
    {
        public string username;
        public string password;
        public AuthData(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
