using CSharpLicense.Models;
using System.Windows;

namespace SimpleApp
{
    public partial class App : Application
    {
        public static CurrencyLicense License = new CurrencyLicense();
        public App()
        {
            this.InitializeComponent();
        }
    }
}
