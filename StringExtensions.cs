using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Useful_CSharp_Extension_Methods
{
    public static class StringExtensions
    {
        public static string ToFirstCharToUpper(this string input)
        {
            if (string.IsNullOrEmpty(input.Trim()) || input.IndexOf('.') < 0)
            {
                return input;
            }
            else
            {
                return input switch
                {
                    null => throw new ArgumentNullException(nameof(input)),
                    "" => throw new ArgumentException($"{nameof(input)} boş olamaz", nameof(input)),
                    _ => input[0].ToString().ToUpper() + input.Substring(1)
                };
            }
        }

        public static string SplitAndTakeNumberedPart(this string input, char separator, int ordernum)
        {

            if (string.IsNullOrEmpty(input.Trim()) || input.IndexOf('.') < 0)
            {
                return input;
            }
            else
            {
                return input switch
                {
                    null => throw new ArgumentNullException(nameof(input)),
                    "" => throw new ArgumentException($"{nameof(input)} boş olamaz", nameof(input)),
                    _ => input.Split(separator)[ordernum]
                };
            }
        }
    }
}
