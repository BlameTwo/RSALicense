using System;
using System.IO;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpLicense;
using CSharpLicense.Helpers;
using CSharpLicense.Models;
using License.Models;
using LicenseBuilder.Contracts;
using Windows.ApplicationModel.DataTransfer;

namespace License.ViewModels;

public sealed partial class DecryptViewModel : ObservableObject
{
    public DecryptViewModel(IPickerService pickerService)
    {
        PickerService = pickerService;
        this.SignHelper = new();
    }

    public SignHelper SignHelper { get; private set; }

    [ObservableProperty]
    string _MD5String;

    [ObservableProperty]
    string _EncryptString;

    public IPickerService PickerService { get; }

    [ObservableProperty]
    DateTimeOffset _LicenseTime;

    [ObservableProperty]
    string _LogText = "";

    [RelayCommand]
    async Task BuilderDecrypt()
    {
        this.MD5String = ComputerInfo.GetHashString(await ComputerInfo.GetComputerInfoAsync());
        LogText += $"Hash:{MD5String}\r\n";
        this.EncryptString = await SignHelper.SignDataAsync(
            new CurrencyLicense() { BuildTime = DateTime.Now, User = MD5String, LicenseTime = this.LicenseTime.Date }
        );
        if (EncryptString != null)
            LogText += $"生成注册数据成功\r\n";
        else
            LogText += "生成失败\r\n";
    }

    [RelayCommand]
    async Task ImportPublicKey()
    {
        try
        {
            var filePath = await PickerService.OpenFilePath(".publicKey");
            if (filePath == null)
                return;
            using (var fs = new StreamReader(filePath))
            {
                SignHelper.PublicKey = await fs.ReadToEndAsync();
                LogText += $"导入公钥成功\r\n";
            }
        }
        catch (Exception)
        {
            LogText += $"公钥文件错误，请重试或重新生成\r\n";
        }
    }

    [RelayCommand]
    async Task ImportPrivateKey()
    {
        try
        {
            var filePath = await PickerService.OpenFilePath(".privateKey");
            if (filePath == null)
                return;
            using (var fs = new StreamReader(filePath))
            {
                SignHelper.PrivateKey = await fs.ReadToEndAsync();
                LogText += $"加载私钥成功\r\n";
            }
        }
        catch (Exception)
        {
            LogText += $"私钥文件错误，请重试或重新生成\r\n";
        }
    }

    [RelayCommand]
    void BuilderKey()
    {
        this.SignHelper = new();
        LogText += $"生成密钥成功\r\n";
    }

    [RelayCommand]
    async Task SavePublicKey()
    {
        try
        {
            var file = await PickerService.GetSavePath(".publicKey");
            if (file == null)
                return;
            using (var writer = File.CreateText(file))
            {
                await writer.WriteAsync(SignHelper.PublicKey);
            }
            LogText += $"保存公钥成功，地址:{file}\r\n";
        }
        catch (Exception ex)
        {
            LogText += $"公钥保存，{ex.Message}\r\n";
        }

    }

    [RelayCommand]
    async Task SavePrivateKey()
    {
        try
        {
            var file = await PickerService.GetSavePath(".privateKey");
            if (file == null)
                return;
            using (var writer = File.CreateText(file))
            {
                await writer.WriteAsync(SignHelper.PrivateKey);
            }
            LogText += $"保存私钥成功，地址:{file}\r\n";
        }
        catch (Exception ex)
        {
            LogText += $"私钥保存，{ex.Message}\r\n";
        }
    }

    [RelayCommand]
    void CopyBoard()
    {
        var package = new DataPackage();
        package.SetText(this.EncryptString);
        Clipboard.SetContent(package);

        LogText += $"已复制到剪切板\r\n";
    }

    [RelayCommand]
    async Task SaveDecrypt()
    {
        try
        {
            var file = await PickerService.GetSavePath(".key");
            if (file == null)
                return;
            using (var writer = File.CreateText(file))
            {
                await writer.WriteAsync(this.EncryptString);
            }
            LogText += $"注册码保存成功\r\n";
        }
        catch (Exception ex)
        {

            LogText += $"注册码保存失败,{ex.Message}\r\n";
        }
        
    }
}
