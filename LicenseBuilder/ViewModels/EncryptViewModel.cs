using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpLicense;
using CSharpLicense.Helpers;
using CSharpLicense.Models;
using License.Contracts;
using License.Models;
using LicenseBuilder;
using LicenseBuilder.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace License.ViewModels;

public sealed partial class EncryptViewModel : ObservableObject
{
    public EncryptViewModel(INavigationService navigationService, IPickerService pickerService)
    {
        NavigationService = navigationService;
        PickerService = pickerService;
    }

    string _password;

    [ObservableProperty]
    string _KeyString;

    [ObservableProperty]
    string _LogText;

    public INavigationService NavigationService { get; }
    public IPickerService PickerService { get; }

    [RelayCommand]
    async Task ReadPassword()
    {
        var filePath = await PickerService.OpenFilePath(".publicKey");
        if (filePath == null)
            return;
        using (var fs = new StreamReader(filePath))
        {
            this._password = await fs.ReadToEndAsync();
            LogText += $"导入公钥成功\r\n";
        }
    }

    [RelayCommand]
    async Task Decryption()
    {
        try
        {
            VerifySignHelper verifySignHelper = new(_password);
            var result = await verifySignHelper.VerifySignatureAsync<CurrencyLicense>(
                KeyString
            );
            if (result != null)
            {
                LogText += $"校验成功！ ID:{result.User} \r\n 授权到期日期：{result.LicenseTime.ToString("G")}";
            }
            else
            {
                LogText += $"注册失败\r\n";
            }
        }
        catch (Exception)
        {
            LogText += $"注册失败，密钥错误！\r\n";
        }
    }
}
