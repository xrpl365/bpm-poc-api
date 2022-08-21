using XrplNftTicketing.Business.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pinata.Client;
using System;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace XrplNftTicketing.Business.Services
{
    public class IpfsPinataService : IIpfsService
    {
        private readonly PinataClient _client;
        private readonly string _httpViewerUrl;
        public IpfsPinataService(string key, string secret, string httpViewerUrl)
        {

            var config = new Config
            {
                ApiKey = key,
                ApiSecret = secret
            };

            _client = new PinataClient(config);

            _httpViewerUrl = httpViewerUrl;
        }

        public async Task TestAuthentication()
        {
            var resp = await _client.Data.TestAuthenticationAsync();
        }

        public string ReadTextFile(string hash)
        {
            WebClient client = new WebClient();
            string downloadString = client.DownloadString(_httpViewerUrl + hash);
            return downloadString;
        }

        public async Task<string> UploadJson(JObject jObject)
        {
            return await UploadJson(jObject.ToString(Formatting.Indented));
        }

        public async Task<string> UploadJson(string jsonContent)
        {
            var response = await _client.Pinning.PinJsonToIpfsAsync(jsonContent);

            if (response.IsSuccess)
            {
                //File uploaded to Pinata Cloud and can be accessed on IPFS
                var hash = response.IpfsHash; // eg https://cloudflare-ipfs.com/ipfs/QmR9HwzakHVr67HFzzgJHoRjwzTTt4wtD6KU4NFe2ArYuj
                return hash;
            }
            throw new Exception(response.Error);
        }


        public async Task<string> UploadFile(string fileName, string fileContent)
        {
            var pinataOptions = new PinataOptions();
            var metadata = new PinataMetadata(); // {KeyValues = { {"Author", "Brian Chavez"} }};

            var response = await _client.Pinning.PinFileToIpfsAsync(content =>
            {
                var file = new System.Net.Http.StringContent(fileContent, Encoding.UTF8, MediaTypeNames.Text.Plain);
                content.AddPinataFile(file, fileName);
            }, metadata, pinataOptions);

            if (response.IsSuccess)
            {
                //File uploaded to Pinata Cloud and can be accessed on IPFS
                var hash = response.IpfsHash; // eg https://cloudflare-ipfs.com/ipfs/QmR9HwzakHVr67HFzzgJHoRjwzTTt4wtD6KU4NFe2ArYuj
                return hash;
            }
            throw new Exception(response.Error);
        }

        public async Task<string> UploadFile(string fileName, byte[] data)
        {
            var pinataOptions = new PinataOptions();
            var metadata = new PinataMetadata();

            var response = await _client.Pinning.PinFileToIpfsAsync(content =>
                {
                    var file = new System.Net.Http.ByteArrayContent(data);
                    content.AddPinataFile(file, fileName);
                }, metadata, pinataOptions);

            if (response.IsSuccess)
            {
                //File uploaded to Pinata Cloud and can be accessed on IPFS
                var hash = response.IpfsHash; // eg https://cloudflare-ipfs.com/ipfs/QmR9HwzakHVr67HFzzgJHoRjwzTTt4wtD6KU4NFe2ArYuj
                return hash;
            }
            throw new Exception(response.Error);
        }

        public byte[] GetImageBytes(string httpUrl)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(httpUrl);
            return imageBytes;
        }

    }
}
