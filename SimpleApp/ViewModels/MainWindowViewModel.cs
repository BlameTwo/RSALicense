using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpLicense;
using CSharpLicense.Helpers;
using CSharpLicense.Models;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SimpleApp.ViewModels;

public sealed partial class MainWindowViewModel:ObservableObject
{
    DispatcherTimer time = null;
    public MainWindowViewModel()
    {
        time = new DispatcherTimer();
        time.Interval = TimeSpan.FromSeconds(1);
    }

    [RelayCommand]
    async Task Loaded()
    {
        this.MachineID = await ComputerInfo.GetComputerInfoAsync();
    }

    [ObservableProperty]
    string _MachineID;

    [ObservableProperty]
    int _Time;

    [ObservableProperty]
    string _SelectFile;

    [ObservableProperty]
    string _License;

    partial void OnTimeChanged(int value)
    {
        if (Time == 0)
        {
            Environment.Exit(0);
        }
    }


    [RelayCommand]
    void SelectFileMethod()
    {
        OpenFileDialog dialog = new OpenFileDialog();
        dialog.Filter = "注册文件|*.publicKey";
        if(dialog.ShowDialog() == true)
        {
            this.SelectFile = dialog.FileName;
        }
    }

    [RelayCommand]
    async Task Verify()
    {
        try
        {
            var publicKey = File.ReadAllText(this.SelectFile,Encoding.UTF8);
            if (publicKey == null)
            {
                return;
            }
            VerifySignHelper verifySignHelper = new(publicKey);
            var result =  await verifySignHelper.VerifySignatureAsync<CurrencyLicense>(License);
            var info = ComputerInfo.GetHashString(await ComputerInfo.GetComputerInfoAsync());
            if (result.User == info)
            {
                MessageBox.Show($"注册成功！剩余可用时间{(result.LicenseTime-DateTime.Now).TotalDays}");
            }
            else
            {
                MessageBox.Show($"校验成功，但不是本机器的授权码");
            }
        }
        catch (Exception)
        {
            
        }
    }
}