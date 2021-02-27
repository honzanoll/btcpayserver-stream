using Newtonsoft.Json;

namespace BTCPayServer.Stream.Common.Extensions
{
    public static class StringExtensions
    {
        public static T FromJson<T>(this string value) where T : new()
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch { }
            }

            return default(T);
        }

        public static T FromJsonOrNew<T>(this string value) where T : new()
        {
            T objectValue = FromJson<T>(value);

            if (objectValue != null)
                return objectValue;

            return new T();
        }

        public static string TrimEnd(this string input, string suffixToRemove)
        {
            if (input != null && suffixToRemove != null && input.EndsWith(suffixToRemove))
                return input.Substring(0, input.Length - suffixToRemove.Length);
            else
                return input;
        }
    }
}
