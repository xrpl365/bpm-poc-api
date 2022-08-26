using System.Threading.Tasks;
using Xrpl.Client.Models.Transactions;
using XrplNftTicketing.Entities.DTOs;

namespace XrplNftTicketing.Business.Interfaces
{
    public interface IXrplService
    {
        NFToken GetNfTokenFromMetaData(Meta txnResponse, string metaUri);
        Task<Submit> MintNfToken(string seed, string issuer, string metaUrl, uint? transferFee, NFTokenMintFlags nftTokenMintFlags);
        Task<bool> NfTokenAcceptBuyOffer(string seed, TicketClaimDto ticketClaim);
        Task<Submit> PaymentSend(string seed, string toAddress, decimal amount, string currencyCode, string issuer);
        Task<ITransactionResponseCommon> TransactionDetail(string transactionHash);
    }
}