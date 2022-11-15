using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CorbadoWidgetBackend
{
    public abstract class PasswordVerifyResult
    {
        public abstract JObject GetBody();
    }

    public class PasswordVerifySuccess : PasswordVerifyResult
    {
        private string _redirectUrl;

        public PasswordVerifySuccess(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
        }

        public override JObject GetBody()
        {
            return new JObject()
            {
                { "redirectURL", _redirectUrl }
            };
        }
    }

    public class PasswordVerifyError : PasswordVerifyResult
    {
        /// <summary>
        /// List of errors each containing fieldname and message
        /// </summary>
        private List<(string, string)> _errors;

        public PasswordVerifyError(List<(string, string)> errors)
        {
            _errors = errors;
        }

        public override JObject GetBody()
        {
            return new JObject()
            {
                { "error", new JObject(){
                    { "validation", new JArray()
                    {
                        _errors.Select(e => new JObject()
                        {
                            {"field", e.Item1},
                            {"message", e.Item2}
                        })
                    }
                    }
                }
                }
            };
        }
    }
}
