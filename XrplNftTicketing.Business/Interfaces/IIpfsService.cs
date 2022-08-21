using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace XrplNftTicketing.Business.Interfaces
{
    public interface IIpfsService
    {
        string ReadTextFile(string hash);
        Task<string> UploadFile(string fileName, byte[] data);
        Task<string> UploadFile(string fileName, string fileContent);
        Task<string> UploadJson(JObject jObject);
        Task<string> UploadJson(string jsonContent);
        byte[] GetImageBytes(string url);


    }
}