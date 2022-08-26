using Newtonsoft.Json;
using System;
using XrplNftTicketing.Entities.DTOs.ImportPayloads;

namespace XrplNftTicketing.Entities.DTOs
{
    public class TicketClaimDto
    {
        [JsonProperty(PropertyName = "guid")]
        public Guid Guid { get; set; }

        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }


        [JsonProperty(PropertyName = "nft-issuer-address")]
        public string NftIssuerAddress { get; set; }


        [JsonProperty(PropertyName = "nft-token-id")]
        public string NfTokenId { get; set; }


        [JsonProperty(PropertyName = "create-offer-value")]
        public Currency CreateOfferValue { get; set; }


        [JsonProperty(PropertyName = "nf-token-offer-index")]
        public string NfTokenOfferIndex { get; set; }

        


    }
}
