using System.Threading.Tasks;

namespace FiscalCode.Utilities
{
    public interface IPhotoLibrary
    {
        Task<bool> SavePhotoAsync(byte[] data, string folder, string filename);
    }
}
