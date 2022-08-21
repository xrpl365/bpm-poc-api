using Newtonsoft.Json;

namespace XrplNftTicketing.Entities.DTOs
{

        public class VenueMetaDTO
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "address")]
            public string Address { get; set; }

        }
    
}