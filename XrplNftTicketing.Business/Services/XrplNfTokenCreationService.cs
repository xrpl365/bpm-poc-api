using XrplNftTicketing.Business.Helpers;
using XrplNftTicketing.Business.Interfaces;
using XrplNftTicketing.Entities.DTOs.ImportPayloads;
using System.Collections.Generic;
using System.Threading.Tasks;
using XrplNftTicketing.Entities.Configurations;
using XrplNftTicketing.Entities.DTOs;
using System;

namespace XrplNftTicketing.Business.Services
{
    public static class XrplNfTokenCreationService
    {
        /// <summary>
        /// CreateNftTickets
        /// </summary>
        /// <param name="xrplSettings"></param>
        /// <param name="eventPayload"></param>
        /// <param name="ipfsService"></param>
        /// <param name="xrplService"></param>
        /// <returns></returns>
        public static async Task<List<TicketClaimDto>> CreateNftTickets(XrplSettings xrplSettings, EventPayload eventPayload, IIpfsService ipfsService, IXrplService xrplService, string resourcePath)
        {
            // Transform payload to Meta data Structure 
            var ticketMetaDataCollection = TicketMetaDTOFactory.GetTicketMetaDataBy(eventPayload);

            // Create Ticket Images on IPFS 
            object p = await ticketMetaDataCollection.UploadImagesToIpfs(ipfsService, resourcePath);

            // upload meta data to ipfs
            await ticketMetaDataCollection.UploadMetaDataToIpfs(ipfsService);

            // Create XRPL NFT's
            var result = await ticketMetaDataCollection.MintTicketsToXrplNfts(xrplSettings, xrplService);

            return result;

        }


        public static Task ClaimEventTicket(TicketClaimDto ticketClaim)
        {


            throw new NotImplementedException();
        }
    }
}