using License.ViewModels;
using LicenseBuilder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;

namespace License.Views
{
    public sealed partial class EncryptPage : Page
    {
        public EncryptPage()
        {
            this.InitializeComponent();
            this.ViewModel = App.Service.GetService<EncryptViewModel>();
        }

        public EncryptViewModel ViewModel { get; }
    }
}
