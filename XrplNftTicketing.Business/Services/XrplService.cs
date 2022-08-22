using Xrpl.Client;
using Xrpl.Wallet;
using System.Diagnostics;
using Xrpl.Client.Models.Transactions;
using Newtonsoft.Json.Linq;
using Xrpl.Client.Models.Methods;
using Xrpl.Client.Models.Common;
using System.Threading.Tasks;
using System;
using Xrpl.Client.Extensions;
using XrplNftTicketing.Entities.Xrpl;
using Ripple.Keypairs;
using XrplNftTicketing.Business.Interfaces;
using System.Linq;

namespace XrplNftTicketing.Business.Services
{
    public class XrplService : IXrplService
    {
        private readonly IRippleClient _client;// = new RippleClient("wss://s.altnet.rippletest.net:51233");

        public XrplService(string url)
        {
            _client = new RippleClient(url);
            _client.Connect();
        }


        /// <summary>
        /// TransactionDetail
        /// </summary>
        /// <param name="transactionHash"></param>
        /// <returns></returns>
        public async Task<ITransactionResponseCommon> TransactionDetail(string transactionHash)
        {
            return await _client.Transaction(transactionHash);
        }

        /// <summary>
        /// Get NFToken From MetaData
        /// </summary>
        /// <param name="txnResponse"></param>
        /// <param name="metaUri"></param>
        /// <returns></returns>
        public NFToken GetNfTokenFromMetaData(Meta txnMeta, string metaUri)
        {
            var hexUri = metaUri.ToHex();

            foreach (var affectedNodes in txnMeta.AffectedNodes.Where(a => a.ModifiedNode != null && a.ModifiedNode.FinalFields != null))
            {

                if (affectedNodes.ModifiedNode.FinalFields.NFTokens != null)
                {
                    var token = affectedNodes.ModifiedNode.FinalFields.NFTokens.FirstOrDefault(f => f.NFToken.URI.Equals(hexUri));
                    if (token != null)
                        return token.NFToken;

                }
            }
            throw new Exception("NFToken not found: " + metaUri);
        }



        /// <summary>
        /// Payment Send
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="toAddress"></param>
        /// <param name="amount"></param>
        /// <param name="currencyCode"></param>
        /// <param name="issuer"></param>
        /// <returns></returns>
        public async Task<Submit> PaymentSend(string seed, string toAddress, decimal amount, string currencyCode, string issuer)
        {

            var wallet = Wallet(seed);
            var accountInfo = await _client.AccountInfo(wallet.Address);

            var currency = currencyCode == "XRP"
                                ? new Currency { ValueAsXrp = amount }
                                : new Currency { CurrencyCode = currencyCode, Issuer = issuer };


            IPaymentTransaction paymentTransaction = new PaymentTransaction
            {
                Account = wallet.Address,
                Destination = toAddress,
                Amount = currency,
                Sequence = accountInfo.AccountData.Sequence
            };

            // send txn
            return await sendTransaction(seed, paymentTransaction.ToJson());

        }


        /// <summary>
        /// Mints NFT Token
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="issuer"></param>
        /// <param name="metaUrl">This is intended to be a URI that points to the data or metadata associated with the NFT.</param>
        /// <param name="transferFee">he value specifies the fee charged by the issuer for secondary sales of the NFToken, if such sales are allowed. Valid values for this field are between 0 and 50000 inclusive, allowing transfer rates of between 0.00% and 50.00% in increments of 0.001. If this field is provided, the transaction MUST have the tfTransferable flag enabled.</param>
        /// <param name="nftTokenMintFlags">NFTokenMintFlags</param>
        /// <returns></returns>      
        public async Task<Submit> MintNfToken(string seed, string issuer, string metaUrl, uint? transferFee, NFTokenMintFlags nftTokenMintFlags)
        {
            // Wallet is a signer
            var wallet = Wallet(seed);
            var accountInfo = await _client.AccountInfo(wallet.Address);

            var tokenMintTransaction = new NFTokenMintTransaction
            {
                Account = wallet.Address,
                URI = metaUrl.ToHex(),
                Issuer = string.IsNullOrEmpty(issuer) ? null : issuer,
                TransferFee = transferFee,
                Flags = nftTokenMintFlags,
                Sequence = accountInfo.AccountData.Sequence
            };

            // send txn
            return await sendTransaction(seed, tokenMintTransaction.ToJson());


        }

        /// <summary>
        /// NfTokenAcceptBuyOffer
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="nfTokenID"></param>
        /// <param name="nftOfferIndex"></param>
        /// <returns></returns>
        public async Task<string> NfTokenAcceptBuyOffer(string seed, string nfTokenID, string nftOfferIndex)
        {
            // Wallet is a signer
            var wallet = Wallet(seed);
            var accountInfo = await _client.AccountInfo(wallet.Address);

            var acceptOfferTransaction = new NFTokenAcceptOfferTransaction
            {
                Account = wallet.Address,
                NFTokenBuyOffer = nftOfferIndex,
                Sequence = accountInfo.AccountData.Sequence
            };

            // send txn
            var txn = await sendTransaction(seed, acceptOfferTransaction.ToJson());
            return nfTokenID;


        } 


        public static string GetWalletAddressFromSeed(string seed)
        {
            return Wallet(seed).Address;
        }

        private async Task<Submit> sendTransaction(string seed, string transactionJson)
        {
            Debug.WriteLine(transactionJson);

            //# sign the transaction
            TxSigner signer = TxSigner.FromSecret(seed);  //secret is not sent to server, offline signing only
            var jobj = JObject.Parse(transactionJson);
            SignedTx signedTx = signer.SignJson(jobj);
            //# submit the transaction
            SubmitBlobRequest request = new SubmitBlobRequest();
            request.TransactionBlob = signedTx.TxBlob;

            Submit result = await _client.SubmitTransactionBlob(request);
            CheckTransaction(result);
            return result;
        }

        private bool CheckTransaction(Submit result)
        {
            Debug.WriteLine(result.EngineResult);

            if (result.EngineResult != "tesSUCCESS")
                throw new Exception(result.EngineResult + ". " + result.EngineResultMessage);

            return true;
        }
        private static XrplWallet Wallet(string seedString)
        {

            var seed = Seed.FromBase58(seedString); //.FromRandom();
            var pair = seed.KeyPair();
            return new XrplWallet(pair.Id(), seed.ToString());

        }

        ~XrplService()
        {
            _client.Disconnect();

        }
    }
}
