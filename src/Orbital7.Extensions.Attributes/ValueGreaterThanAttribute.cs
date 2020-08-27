using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
    public class ValueGreaterThanAttribute : ValueCompareAttribute
    {
        protected override string CompareAction
        {
            get { return "greater than"; }
        }

        public ValueGreaterThanAttribute(string propertyName, bool allowEqualValues = false)
            : base(propertyName, allowEqualValues)
        {
            
        }

        protected override bool CompareValue(double thisValue, double compareValue)
        {
            return thisValue > compareValue;
        }
    }
}
