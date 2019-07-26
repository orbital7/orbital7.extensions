using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public class ApiErrorResult
    {
        public string Key { get; set; }

        public string Message { get; set; }

        public ApiErrorResult()
        {

        }

        public ApiErrorResult(
            string key,
            string message)
        {
            this.Key = key;
            this.Message = message;
        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
