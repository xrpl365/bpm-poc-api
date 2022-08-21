using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace XrplNftTicketing.Entities.DTOs.ImportPayloads
{
    public class EventPayload
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public string PromoterName { get; set; }
        public string TermsAndConditions { get; set; }
        public List<TicketPayload> Tickets { get; set; }

        public EventPayload() { }

        public static string EventTestLoad()
        {
            var ticketImport = new EventPayload()
            {

                    Name = "Sounds Live",
                    StartDate = new DateTime(2022, 12, 01, 18, 00, 00),
                    EndDate = new DateTime(2022, 12, 01, 23, 23, 00),
                    VenueName = "Parr Hall",
                    VenueAddress = "Palmyra Square S,Warrington, WA1 1BL",
                    PromoterName = "ABC Promotions Live",
                    TermsAndConditions = "Tickets to this Event are issued on behalf of ABC Promotions Live and are subject to the following terms and conditions:\n" +
                    "\n1. In addition to those terms and conditions outlined below, any attendee of the Event agrees to the terms and conditions outlined by our ticket agent See Tickets here." +
                    "\n2. Nobody will be allowed admission to the Event without a valid ticket or pass"
                ,
                Tickets = new List<TicketPayload>()
                {
                    new TicketPayload()
                    {
                        SerialNumber = "000001",
                        BookingNumber = "ASDAS65675",
                        Category = "General Standing",
                        VenueAreaName = "Stalls",
                        Price =  new Currency(){ CurrencyCode ="GBP", Value = 25M},
                        TicketImageUrl = "https://localhost:44379/Content/Images/ticket1.png"},
                    new TicketPayload()
                    {
                        SerialNumber = "000002",
                        BookingNumber = "ASDAS65676",
                        Category = "General Standing",
                        VenueAreaName = "Stalls",
                        Price =  new Currency(){ CurrencyCode ="GBP", Value = 25M},
                        TicketImageUrl = "https://localhost:44379/Content/Images/ticket2.png"},

                }
            };

            return JsonConvert.SerializeObject(ticketImport, Formatting.None);
        }
    }
}
