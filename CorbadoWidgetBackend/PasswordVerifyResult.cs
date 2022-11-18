using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace CorbadoWidgetBackend
{
    public abstract class PasswordVerifyResult
    {
        public abstract JObject GetBody();

        public abstract int GetStatusCode();
    }

    public class PasswordVerifySuccess : PasswordVerifyResult
    {
        public string redirectUrl;
        public string sessionToken;

        public PasswordVerifySuccess(string redirectUrl, string sessionToken)
        {
            this.redirectUrl = redirectUrl;
            this.sessionToken = sessionToken;
        }
        public override int GetStatusCode()
        {
            return 200;
        }
        public override JObject GetBody()
        {
            return new JObject
            {
                { "redirectUrl", redirectUrl },
                {"sessionToken", sessionToken }
            };
        }
    }

    public class PasswordVerifyError : PasswordVerifyResult
    {

        public override int GetStatusCode()
        {
            return 400;
        }
        public override JObject GetBody()
        {
            return new JObject();
        }
    }
}
