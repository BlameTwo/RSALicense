using System.Threading.Tasks;

namespace LicenseBuilder.Contracts;

public interface IPickerService
{
    public Task<string> GetSavePath(string extention);

    public Task<string> OpenFilePath(string extention);
}