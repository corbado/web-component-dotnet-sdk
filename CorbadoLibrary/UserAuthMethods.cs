namespace CorbadoLibrary
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

        public UserAuthMethods(List<string>? emails, List<string>? phoneNumbers, bool? hasPassword)
        {
            this.emails = emails ?? new List<string>();
            this.phoneNumbers = phoneNumbers ?? new List<string>();
            this.hasPassword = hasPassword ?? false;

        }

    }
}
