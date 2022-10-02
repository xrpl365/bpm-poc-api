# bpm-poc-api


This Dot Net solutions will run in Visual Studio Code 2022. A fork has been taken of the Xrpl.c codebase. Debug Dll versions of this are found within the codebase. The fork was taken to allow it to run in .Net Core, but some other changes have been made to enable NFT transactions.

Curent demo keys have been left in-place to make running easy. New keys can be optained as follows:

New keys can be entered into \XrplNftTicketing.Api\appsettings.json

  - For Ipfs this app is currently using Pinata, and a free Pinata account can be used (https://app.pinata.cloud/) to get and set the key and secret in appsettings.json.
  - An XRPL account seed from a test faucet account see: https://xrpl.org/xrp-testnet-faucet.html

