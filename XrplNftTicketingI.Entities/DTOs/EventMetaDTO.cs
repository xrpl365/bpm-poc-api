using Newtonsoft.Json;

namespace XrplNftTicketing.Entities.DTOs
{

        public class EventMetaDTO
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }

            [JsonProperty(PropertyName = "date")]
            public string Date { get; set; }

        }
    
}