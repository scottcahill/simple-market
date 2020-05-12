using System.Threading.Tasks;

namespace SmWeb.Services
{
    public interface ISmBlobStorage
    {
        Task<bool> UploadBlob(string filename, System.IO.Stream stream = null);

        Task DeleteBlob(string filename);

    }
}
