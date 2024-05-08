using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using WinUIEx;

namespace LicenseBuilder;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        this.InitializeComponent();
        this.ExtendsContentIntoTitleBar = true;
        //this.SystemBackdrop = new DesktopAcrylicBackdrop();
        this.MaxHeight = 700;
        this.MaxWidth = 470;
        this.MinHeight = 700;
        this.MinWidth = 470;
        this.IsResizable = true;
        this.IsMaximizable = false;
    }
}
