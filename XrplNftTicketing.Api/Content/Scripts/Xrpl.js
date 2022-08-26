var userAccountSeed = "";
var userAccountAddress = "";
var userWallet = null;
var wssNetwork = "";
var client = null;

async function xrplConnect() {

    client = new xrpl.Client(wssNetwork)
    await client.connect()

}

async function createBuyOffer(nfTokenID, owner, offerAmount) {

    // NFTokenCreateOffer transaction 
    const transactionBlob = {
        "TransactionType": "NFTokenCreateOffer",
        "Account": userAccountAddress,
        "Owner": owner,
        "NFTokenID": nfTokenID,
        "Amount": offerAmount.value.toString()
    }

    // Submit transaction 
    return await client.submitAndWait(transactionBlob, { wallet: userWallet })

}

async function getBuyOffer(nftokenID) {

    let nftBuyOffers
    try {
        nftBuyOffers = await client.request({
            method: "nft_buy_offers",
            nft_id: nftokenID
        })
    } catch (err) {
        alert('No buy offers.')
    }
    return nftBuyOffers.result.offers;
}


