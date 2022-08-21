using Newtonsoft.Json;

namespace XrplNftTicketing.Entities.DTOs
{
    
        public class PriceMetaDTO
        {
            [JsonProperty(PropertyName = "original-price")]
            public decimal OriginalPrice { get; set; }

            [JsonProperty(PropertyName = "currency-code")]
            public string CurrencyCode { get; set; }
        }
    
}