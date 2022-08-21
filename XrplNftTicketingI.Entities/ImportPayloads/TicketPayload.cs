using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace XrplNftTicketing.Entities.DTOs.ImportPayloads
{
    public class TicketPayload
    {
        public string OwnerUserId { get; set; }
        public string SerialNumber { get; set; }
        public string BookingNumber { get; set; }
        public string TicketImageUrl { get; set; }
        public string TermsAndConditions{ get; set; }
        public string CustomText1 { get; set; }
        public string CustomText2 { get; set; }

        public Currency Price { get; set; }

        public string VenueAreaName { get; set; }
        public string VenueSubAreaName { get; set; }
        public string Category { get; set; }

        public TicketPayload() { }
 
    }
    public partial class Currency
    {
        [JsonProperty(PropertyName = "currency-code")]
        public string CurrencyCode { get; set; }

        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }

    }
}
