using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpLicense;
using License.Contracts;
using License.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Text;

namespace LicenseBuilder.ViewModels;

public sealed partial class LicenseViewModel : ObservableObject
{
    public INavigationService NavigationService { get; }

    public LicenseViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
        Onload();
    }

    private async void Onload()
    {
        this.OrginKey = await ComputerInfo.GetComputerInfoAsync();
    }

    [ObservableProperty]
    SelectorBarItem _SelectTab;

    partial void OnSelectTabChanged(SelectorBarItem oldValue, SelectorBarItem newValue)
    {
        var tran = new SlideNavigationTransitionInfo();
        tran.Effect =
            newValue.FlowDirection == Microsoft.UI.Xaml.FlowDirection.LeftToRight
                ? SlideNavigationTransitionEffect.FromLeft
                : SlideNavigationTransitionEffect.FromRight;
        if (newValue.Text == "签名")
        {
            NavigationService.NavigationTo<DecryptViewModel>(null, tran);
        }
        else if (newValue.Text == "验证")
        {
            NavigationService.NavigationTo<EncryptViewModel>(null, tran);
        }
    }

    [ObservableProperty]
    string _OrginKey;

    [RelayCommand]
    void BuildRandom()
    {
        string values = "abcdefghijkrmnopqrskuvwxyzABCDEFGHIJKRMNOPQRSKUVWXZY";
        StringBuilder build = new();
        for (int i = 0; i < 39;i++)
        {
            build.Append(values[Random.Shared.Next(0, values.Length)]);
        }
        this.OrginKey = build.ToString();
    }

    partial void OnOrginKeyChanged(string value)
    {
        App.OrginKey = value;
    }
}
