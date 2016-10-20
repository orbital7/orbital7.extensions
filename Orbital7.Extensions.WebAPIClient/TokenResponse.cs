using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Orbital7.Extensions.WebAPIClient
{
    [DataContract]
    public class TokenResponse
    {
        [DataMember(Name = "access_token")]
        public string Token { get; set; }

        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }

        [DataMember(Name = "userName")]
        public string Username { get; set; }

        [DataMember(Name = ".issued")]
        public string DateIssuedString { get; set; }

        [DataMember(Name = ".expires")]
        public string DateExpiresString { get; set; }

        [DataMember(Name = "expires_in")]
        public int ExpiresIn { get; set; }

        public DateTime DateIssued
        {
            get { return Convert.ToDateTime(this.DateIssuedString); }
        }

        public DateTime DateExpires
        {
            get { return Convert.ToDateTime(this.DateExpiresString); }
        }

        public override string ToString()
        {
            return this.Token;
        }
    }
}
