using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace EventsAPI.Business.Helpers
{
    public static class TicketImportHelper
    {

        public static byte[] GetImgData(this TicketImport ticket)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(ticket.TicketImageUrl);
            return imageBytes;
        }

        public static JObject GetMetaData(this TicketImport ticket, string ipfsImgHash, EventData evnt)
        {
            dynamic jsonObject = new JObject();
            jsonObject.name = ticket.Category.ToSentenceCase() + " ticket for " + evnt.SummaryDescription;
            jsonObject.description = "Batch test NFT mint for XLS-20d NFT-Devnet.";
            jsonObject.image = ticket.IpfsData.ImageUrl;
            jsonObject["event-name"] = evnt.Name;
            jsonObject["event-date"] = evnt.StartDateTimeDisplay;
            jsonObject.venue = evnt.VenueName;
            jsonObject.address = evnt.VenueAddress;

            if (!string.IsNullOrEmpty(evnt.TermsAndConditions))
                jsonObject["terms-and-conditions"] = evnt.TermsAndConditions;

            AddTicketAttributes(jsonObject, ticket);
            return jsonObject;

        }

        private static void AddTicketAttributes(dynamic jsonObject, TicketImport ticket)
        {

            if (!string.IsNullOrEmpty(ticket.SerialNumber))
                jsonObject["serial-number"] = ticket.SerialNumber;
            if (!string.IsNullOrEmpty(ticket.BookingNumber))
                jsonObject["booking-number"] = ticket.BookingNumber;
            if (!string.IsNullOrEmpty(ticket.Category))
                jsonObject.category = ticket.Category;
            if (ticket.Location != null)
                jsonObject.location = ticket.Location.ToString;
            if (ticket.Value != null)
                jsonObject.value = ticket.Value.ToString();
            if (!string.IsNullOrEmpty(ticket.TermsAndConditions))
                jsonObject["terms-and-conditions-other"] = ticket.TermsAndConditions;

           

        }

        private static string ToSentenceCase(this string sourcestring)
        {
            // start by converting entire string to lower case
            var lowerCase = sourcestring.ToLower();
            // matches the first sentence of a string, as well as subsequent sentences
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            // MatchEvaluator delegate defines replacement of setence starts to uppercase
            return r.Replace(lowerCase, s => s.Value.ToUpper());

        }

    }
}