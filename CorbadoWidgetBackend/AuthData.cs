using System;
using System.Collections.Generic;
using System.Text;

namespace CorbadoWidgetBackend
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
