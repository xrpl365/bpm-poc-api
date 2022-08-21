
namespace XrplNftTicketing.Entities.Xrpl
{
    public class XrplWallet
    {
        public readonly string Address;
        public readonly string PrivateKey;

        public XrplWallet(string address, string privateKey)
        {
            Address = address;
            PrivateKey = privateKey;
        }

    }
}
