using System.Collections.Generic;
using Newtonsoft.Json;

namespace Hotel.Common.Extensions
{
    public static class ListExtensions
    {
        public static List<T> ConvertJsonObjectListTo<T>(this List<object> list)
            => JsonConvert.DeserializeObject<List<T>>(JsonConvert.SerializeObject(list));
    }
}