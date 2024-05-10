using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Useful_CSharp_Extension_Methods
{
    public static class StreamExtensions
    {
        public static T DeserializeToObject<T>(string xmlpathortext) where T : class
        {

            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            if (!string.IsNullOrEmpty(xmlpathortext) && xmlpathortext.TrimStart().StartsWith("<"))
            {
                using (TextReader reader = new StringReader(xmlpathortext))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(xmlpathortext))
                {
                    return (T)ser.Deserialize(sr);
                }
            }

        }
        public static T Deserialize<T>(this Stream s)
        {
            using (StreamReader reader = new StreamReader(s))
            using (JsonTextReader jsonReader = new JsonTextReader(reader))
            {
                JsonSerializer ser = new JsonSerializer();
                return ser.Deserialize<T>(jsonReader);
            }
        }

    }
}
