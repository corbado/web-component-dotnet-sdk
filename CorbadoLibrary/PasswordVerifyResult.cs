using System.Text.Json.Nodes;

namespace CorbadoLibrary
{
    public abstract class PasswordVerifyResult
    {
        public abstract JsonObject GetBody();
    }

    public class PasswordVerifySuccess : PasswordVerifyResult
    {
        private string _redirectUrl;

        public PasswordVerifySuccess(string redirectUrl)
        {
            _redirectUrl = redirectUrl;
        }

        public override JsonObject GetBody()
        {
            return new JsonObject()
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

        public override JsonObject GetBody()
        {
            return new JsonObject()
            {
                { "error", new JsonObject(){
                    { "validation", new JsonArray()
                    {
                        _errors.Select(e => new JsonObject()
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
