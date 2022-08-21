using Newtonsoft.Json;

namespace XrplNftTicketing.Business.Services
{
    public static partial class TicketMetaDTOFactory
    {
        public class VenueMetaDTO
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "address")]
            public string Address { get; set; }

        }
    }
}