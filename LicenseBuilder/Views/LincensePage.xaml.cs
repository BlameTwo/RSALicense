using Microsoft.UI.Xaml.Controls;
using LicenseBuilder.ViewModels;

namespace License.Views;

public sealed partial class LincensePage : Page
{
    public LincensePage(LicenseViewModel vm)
    {
        this.InitializeComponent();
        this.ViewModel = vm;
        this.ViewModel.NavigationService.RegisterView(this.frameBase);
    }

    public LicenseViewModel ViewModel { get; }
}
