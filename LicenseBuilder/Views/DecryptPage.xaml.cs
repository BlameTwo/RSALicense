using License.ViewModels;
using LicenseBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace License.Views;

public sealed partial class DecryptPage : Page
{
    public DecryptPage()
    {
        this.InitializeComponent();
        this.ViewModel = App.Service.GetService<DecryptViewModel>();
    }

    public DecryptViewModel ViewModel { get; }
}
