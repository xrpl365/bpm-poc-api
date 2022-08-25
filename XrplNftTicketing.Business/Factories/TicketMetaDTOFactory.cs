using XrplNftTicketing.Business.Helpers;
using XrplNftTicketing.Entities.DTOs.ImportPayloads;
using XrplNftTicketing.Entities.DTOs;

using System.Collections.Generic;

namespace XrplNftTicketing.Business.Services
{
    public static partial class TicketMetaDTOFactory
    {

        public static List<TicketMetaDTO> GetTicketMetaDataBy(EventPayload evnt)
        {
            var output = new List<TicketMetaDTO>();

            foreach (var evtTicket in evnt.Tickets)
            {
                output.Add( new TicketMetaDTO()
                {
                    BookingNumber = evtTicket.BookingNumber,
                    Description = "Test event ticket creation for xls-20d.",
                    Event = new EventMetaDTO() { Date = evnt.StartDate.EventDisplayDateTime(), Name = evnt.Name },
                    Name = evtTicket.Category.ToSentenceCase() + " ticket for " + evnt.Name,
                    OriginalImgUrl = evtTicket.TicketImageUrl,
                    ImgFileName = evtTicket.SerialNumber + "-" + System.IO.Path.GetFileName(evtTicket.TicketImageUrl),
                    Price = new PriceMetaDTO() { OriginalPrice = evtTicket.Price.Value, CurrencyCode = evtTicket.Price.CurrencyCode },
                    SerialNumber = evtTicket.SerialNumber,
                    Venue = new Entities.DTOs.VenueMetaDTO() { Name = evnt.VenueName, Address = evnt.VenueAddress },
                    TermsAndConditions = evnt.TermsAndConditions + " " + evtTicket.TermsAndConditions,
                    TicketLocation = new TicketLocationMetaDTO() { Value = evtTicket.VenueAreaName },
                    Promoter = evnt.PromoterName
                });

            }


            return output;
        }
    }
}