using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Models
{
    public interface ITagObject : IIdObject
    {
        string TagName { get; }
    }
}
