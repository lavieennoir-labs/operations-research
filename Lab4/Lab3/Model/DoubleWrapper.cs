using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3.Model
{
    class DoubleWrapper
    {
        public double Value { get; set; } = 0.0;

        public static implicit operator DoubleWrapper(Double val)
        {
            return new DoubleWrapper { Value = val };
        }

        public static implicit operator Double(DoubleWrapper val)
        {
            return val.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    class StringWrapper
    {
        public String Value { get; set; } = "";

        public static implicit operator StringWrapper(String val)
        {
            return new StringWrapper { Value = val };
        }

        public static implicit operator String(StringWrapper val)
        {
            return val.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
