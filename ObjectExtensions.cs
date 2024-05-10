using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Useful_CSharp_Extension_Methods
{
    public static class ObjectExtensions
    {
        public static string GetQueryString(this object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select GetParameterAndValues(p, obj);

            return string.Join("&", properties.ToArray());
        }
#nullable enable
        public static string? GetParameterAndValues(PropertyInfo p, object obj)
        {
            List<string> parameters = new List<string>();
            var value = p.GetValue(obj);
            if (value == null)
            {
                return "";
            }
            else if (value is IList)
            {
                var parameterValues = value as IList;
                if (parameterValues != null)
                {
                    foreach (var parameterValue in parameterValues)
                    {
                        parameters.Add(p.Name + "=" + parameterValue);
                    }
                }
            }
            else
            {
                parameters.Add(p.Name + "=" + value?.ToString());
            }

            return string.Join("&", parameters);
        }
        public static Dictionary<string, object> ToDictionary(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dictionary;
        }
    }
}
