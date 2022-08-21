using System.Threading.Tasks;
using Xrpl.Client.Models.Transactions;

namespace XrplNftTicketing.Business.Interfaces
{
    public interface IXrplService
    {
        NFToken GetNfTokenFromMetaData(Meta txnResponse, string metaUri);
        Task<Submit> MintNfToken(string seed, string issuer, string metaUrl, uint? transferFee, NFTokenMintFlags nftTokenMintFlags);
        Task<string> NfTokenAcceptBuyOffer(string seed, string nfTokenID, string nftOfferIndex);
        Task<Submit> PaymentSend(string seed, string toAddress, decimal amount, string currencyCode, string issuer);
        Task<ITransactionResponseCommon> TransactionDetail(string transactionHash);
    }
}