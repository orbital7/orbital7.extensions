using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public class ApiException
        : Exception
    {
        public const string DEFAULT_KEY = "ERROR";

        public string Key { get; set; }

        public ApiException(
            string key,
            string message)
            : base(message)
        {
            this.Key = key;
        }

        public ApiException(
            string message)
            : this(DEFAULT_KEY, message)
        {
            
        }

        public ApiErrorResult ToApiErrorResult()
        {
            return new ApiErrorResult()
            {
                Key = this.Key,
                Message = this.Message,
            };
        }
    }
}
