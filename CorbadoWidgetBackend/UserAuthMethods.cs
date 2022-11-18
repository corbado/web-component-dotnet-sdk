using System;
using System.Collections.Generic;
using System.Text;

namespace CorbadoWidgetBackend
{
    public class UserAuthMethods
    {
        public List<string> emails;
        public List<string> phoneNumbers;
        public bool hasPassword;

        public UserAuthMethods()
        {
            emails = new List<string>();
            phoneNumbers = new List<string>();
            hasPassword = false;
        }

        public UserAuthMethods(List<string> emails, List<string> phoneNumbers, bool hasPassword)
        {
            this.emails = emails;
            this.phoneNumbers = phoneNumbers;
            this.hasPassword = hasPassword;

        }

    }
}
