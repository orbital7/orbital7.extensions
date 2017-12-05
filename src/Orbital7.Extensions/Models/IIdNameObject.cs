using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public interface IIdNameObject : IIdObject
    {
        string Name { get; }
    }
}
