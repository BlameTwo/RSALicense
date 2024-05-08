using CommunityToolkit.Mvvm.ComponentModel;
using License.Contracts;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml;
using System.Collections.Generic;

namespace License.Services;

public class NavigationService : INavigationService
{

    public NavigationService(IPageService pageService)
    {
        PageService = pageService;
    }

    private object _parameter;

    public IPageService PageService { get; }

    public Frame RootFrame { get; private set; }

    public bool CanGoBack => RootFrame != null ? RootFrame.CanGoBack : false;

    public bool CanGoForward => RootFrame != null ? RootFrame.CanGoForward : false;

    public event NavigatedEventHandler Navigated;
    public event NavigationFailedEventHandler NavigationFailed;


    public Dictionary<string, object> Tags { get; set; } = new();

    public bool GoBack()
    {
        if (RootFrame.CanGoBack)
        {
            ElementSoundPlayer.Play(ElementSoundKind.GoBack);
            RootFrame.GoBack();
            return true;
        }
        return false;
    }

    public bool GoForward()
    {
        if (RootFrame.CanGoForward)
        {
            RootFrame.GoForward();
            return true;
        }
        return false;
    }

    public bool NavigationTo(string key, object args, NavigationTransitionInfo transition)
    {
        var pageType = PageService.GetPage(key);
        if (pageType == null)
            return false;
        if (
            RootFrame != null && RootFrame.Content?.GetType() != pageType
            || args != null && !args.Equals(_parameter)
        )
        {
            _parameter = args;
            return RootFrame.Navigate(pageType, _parameter, transition);
        }
        return false;
    }

    public void RegisterView(Frame frame)
    {
        RootFrame = frame;
        RootFrame.Navigated += Navigated;
        RootFrame.NavigationFailed += NavigationFailed;
    }

    public void UnRegisterView()
    {
        RootFrame.Navigated -= Navigated;
        RootFrame.NavigationFailed -= NavigationFailed;
        RootFrame = null;
    }

    public bool NavigationTo<ViewModel>(object args, NavigationTransitionInfo transition)
        where ViewModel : ObservableObject => NavigationTo(typeof(ViewModel).FullName, args,new DrillInNavigationTransitionInfo());
}
