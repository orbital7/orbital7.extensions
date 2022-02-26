using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel.DataAnnotations
{
    public class ValueLessThanAttribute : ValueCompareAttribute
    {
        protected override string CompareAction
        {
            get { return "greater than"; }
        }

        public ValueLessThanAttribute(string propertyName, bool allowEqualValues = false)
            : base(propertyName, allowEqualValues)
        {

        }

        protected override bool CompareValue(double thisValue, double compareValue)
        {
            return thisValue < compareValue;
        }
    }
}
