﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CorbadoWidgetBackend
{
    public abstract class AuthMethodsResult
    {
        private UserStatus userStatus;
        public AuthMethodsResult(UserStatus userStatus)
        {
            this.userStatus = userStatus;
        }

        public int getStatusCode()
        {
            return (int) userStatus;
        }
        public abstract JObject getBody();
    }

    public class AuthMethodsSuccess : AuthMethodsResult
    {
        public List<string> emails;
        public List<string> phoneNumbers;
        public bool hasPassword;

        public AuthMethodsSuccess() : base(UserStatus.Permitted)
        {
            emails = new List<string>();
            phoneNumbers = new List<string>();
            hasPassword = false;
        }

        public AuthMethodsSuccess( List<string> emails, List<string> phoneNumbers, bool hasPassword) : base(UserStatus.Permitted)
        {
            this.emails = emails;
            this.phoneNumbers = phoneNumbers;
            this.hasPassword = hasPassword;

        }

        public override JObject getBody()
        {

            JObject data = new JObject();
            JArray methods = new JArray();

            //Convert authentication methods to valid json
            if (this.emails.Count > 0)
            {
                methods.Add("email");
                JArray emails = new JArray();
                this.emails.ForEach(email => { emails.Add(email); });
                data.Add("emails", emails);
            }
            if (this.phoneNumbers.Count > 0)
            {
                methods.Add("phone_number");
                JArray phoneNumbers = new JArray();
                this.phoneNumbers.ForEach(phoneNumber => { phoneNumbers.Add(phoneNumber); });
                data.Add("phone_numbers", phoneNumbers);
            }
            if (this.hasPassword)
            {
                methods.Add("password");
            }

            data.Add("methods", methods);
            JObject body = new JObject()
            {
                {"data", data }
            };
            return body;
        }
    }

    public class AuthMethodsError : AuthMethodsResult
    {
        public AuthMethodsError(UserStatus userStatus) : base(userStatus)
        {
        }

        public override JObject getBody()
        {
            return new JObject();
        }
    }
}
