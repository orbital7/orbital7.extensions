using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public class ApiException
        : Exception
    {
        public string Key { get; set; }

        public ApiException(
            string key,
            string message)
            : base(message)
        {
            this.Key = key;
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
