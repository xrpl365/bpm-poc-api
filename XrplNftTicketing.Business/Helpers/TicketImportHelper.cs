﻿using XrplNftTicketing.Business.Interfaces;
using XrplNftTicketing.Business.Services;
using XrplNftTicketing.Entities.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xrpl.Client.Models.Transactions;
using XrplNftTicketing.Entities.DTOs.ImportPayloads;
using System;

namespace XrplNftTicketing.Business.Helpers
{
    public static class TicketImportHelper
    {
        /// <summary>
        /// Turns phrases to sentence case
        /// </summary>
        /// <param name="sourcestring"></param>
        /// <returns></returns>
        public static string ToSentenceCase(this string sourcestring)
        {
            // start by converting entire string to lower case
            var lowerCase = sourcestring.ToLower();
            // matches the first sentence of a string, as well as subsequent sentences
            var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
            // MatchEvaluator delegate defines replacement of setence starts to uppercase
            return r.Replace(lowerCase, s => s.Value.ToUpper());

        }

        /// <summary>
        /// Takes ticket collection and exports tickets to ipfs
        /// </summary>
        /// <param name="tickets"></param>
        /// <param name="ipfsService"></param>
        /// <returns></returns>
        public static async Task<bool> UploadImagesToIpfs(this List<TicketMetaDTO> tickets, IIpfsService ipfsService)
        {
            foreach(var ticket in tickets)
            {
                var byteData = ipfsService.GetImageBytes(ticket.OriginalImgUrl);
                ticket.Image  = await ipfsService.UploadFile(ticket.ImgFileName, byteData);
            }
            return true;
        }

        /// <summary>
        /// Serializes ticket objects to json and adds them to ipfs
        /// </summary>
        /// <param name="tickets"></param>
        /// <param name="ipfsService"></param>
        /// <returns></returns>
        public static async Task<bool> UploadMetaDataToIpfs(this List<TicketMetaDTO> tickets, IIpfsService ipfsService)
        {
            foreach (var ticket in tickets)
            {
                var json = JsonConvert.SerializeObject(ticket, Formatting.Indented);
                ticket.IpfsMetaHash = await ipfsService.UploadJson(json);
            }
            return true;
        }

        /// <summary>
        /// MintTicketsToXrplNfts
        /// </summary>
        /// <param name="tickets"></param>
        /// <param name="xrplService"></param>
        /// <param name="walletSeed"></param>
        /// <param name="transFerFee"></param>
        /// <returns></returns>
        public static async Task<List<TicketClaimDto>> MintTicketsToXrplNfts(this List<TicketMetaDTO> tickets, IXrplService xrplService, string walletSeed, uint transFerFee)
        {
            var mintFlags = NFTokenMintFlags.tfBurnable | NFTokenMintFlags.tfTransferable | NFTokenMintFlags.tfTrustLine;
            var nftTokensList = new List<TicketClaimDto>();
            var issuerAddress = XrplService.GetWalletAddressFromSeed(walletSeed);

            foreach (var ticket in tickets)
            {
                var json = JsonConvert.SerializeObject(ticket, Formatting.Indented);
                var mintTxnResult = await xrplService.MintNfToken(walletSeed, null, ticket.IpfsMetaHash, transFerFee, mintFlags);

                // Lookup Created Token Id
                Meta txnMetaData = null;
                ITransactionResponseCommon txnResponse = null;
                while (txnMetaData == null)
                {
                    // wait for trans detail to return meta data
                    txnResponse = await xrplService.TransactionDetail(mintTxnResult.Transaction.Hash);
                    txnMetaData = txnResponse.Meta;
                }
                var nfToken = xrplService.GetNfTokenFromMetaData(txnMetaData, ticket.IpfsMetaHash);

                // Create Ticket Claim Object
                var ticketClaim = new TicketClaimDto()
                {
                    Guid = Guid.Parse("150c995c-43f8-4759-9065-94fc6bf82d41"), // Hard coded for Proof of concepts // Guid.NewGuid(),
                    NftIssuerAddress = issuerAddress,
                    NfTokenId = nfToken.NFTokenID,
                    CreateOfferValue = new Currency() { CurrencyCode = "Xrp", Value = 0.1M }
                };
                nftTokensList.Add(ticketClaim);

            }
            return nftTokensList;
        }
    }
}