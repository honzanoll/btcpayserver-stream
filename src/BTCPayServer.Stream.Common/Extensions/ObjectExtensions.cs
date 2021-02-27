using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BTCPayServer.Stream.Common.Extensions
{
    public static class ObjectExtensions
    {
        #region Public methods
        
        public static string ToJson(this object input)
        {
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.SerializeObject(input, serializerSettings);
        }

        #endregion
    }
}
