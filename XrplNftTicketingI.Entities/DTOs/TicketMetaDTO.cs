using Newtonsoft.Json;

namespace XrplNftTicketing.Entities.DTOs
{
    
        public class TicketMetaDTO
        {
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }


            [JsonProperty(PropertyName = "description")]
            public string Description { get; set; }


            [JsonProperty(PropertyName = "image")]
            public string Image { get; set; }

            [JsonProperty(PropertyName = "serial-number ")]
            public string SerialNumber { get; set; }

            [JsonProperty(PropertyName = "booking-number")]
            public string BookingNumber { get; set; }

            [JsonProperty(PropertyName = "terms-and-conditions")]
            public string TermsAndConditions { get; set; }

            [JsonProperty(PropertyName = "ticket-location")]
            public TicketLocationMetaDTO TicketLocation { get; set; }

 
            [JsonProperty(PropertyName = "event")]
            public EventMetaDTO Event { get; set; }

            [JsonProperty(PropertyName = "venue")]
            public VenueMetaDTO Venue { get; set; }

            [JsonProperty(PropertyName = "price")]
            public PriceMetaDTO Price { get; set; }

            [JsonIgnore]
            public string NFTokenId { get; set; }

            [JsonIgnore]
            public string IpfsMetaHash { get; set; }

            [JsonIgnore]
            public string OriginalImgUrl { get; set; }

            [JsonIgnore]
            public string ImgFileName { get; set; }

        }
 
}