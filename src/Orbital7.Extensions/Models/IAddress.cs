using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public interface IAddress
    {
        string Address1 { get; set; }

        string Address2 { get; set; }

        string City { get; set; }

        USState State { get; set; }

        string ZipCode { get; set; }
    }
}
