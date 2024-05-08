using CSharpLicense;
using License.Contracts;
using License.Services;
using License.ViewModels;
using License.Views;
using LicenseBuilder.Contracts;
using LicenseBuilder.Services;
using LicenseBuilder.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;

namespace LicenseBuilder;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        App.Service = new ServiceCollection()
            .AddSingleton<INavigationService,NavigationService>()
            .AddTransient<IPickerService,PickerService>()
            .AddSingleton<IPageService,PageService>()
            .AddTransient<LicenseViewModel>()
            .AddTransient<LincensePage>()
            .AddTransient<DecryptViewModel>()
            .AddTransient<EncryptViewModel>()
            .AddTransient<DecryptPage>()
            .AddTransient<EncryptPage>()
            .BuildServiceProvider();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();
        MainWindow.Content = App.Service.GetService<LincensePage>();
        MainWindow.Activate();
    }

    public static IServiceProvider Service { get;private set; }
    public static string OrginKey { get; internal set; }

    public static Window MainWindow;
}
