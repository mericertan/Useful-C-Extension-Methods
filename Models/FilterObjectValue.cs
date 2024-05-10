using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Useful_CSharp_Extension_Methods.Models
{
    public class FilterObjectValue<T>
    {
        public T Value { get; set; }
        public string Condition { get; set; }
    }
}
