using LicenseBuilder.Contracts;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.Storage;
using System;

namespace LicenseBuilder.Services;

class PickerService : IPickerService
{
    public async Task<string> GetSavePath(string extention)
    {
        FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        savePicker.FileTypeChoices.Add("注册文件", new List<string>() { extention });
        StorageFile file = await savePicker.PickSaveFileAsync();
        if (file == null) return null;
        return file.Path;
    }

    public async Task<string> OpenFilePath(string extention)
    {
        FileOpenPicker savePicker = new Windows.Storage.Pickers.FileOpenPicker();
        var window = App.MainWindow;
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
        WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);
        savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        savePicker.FileTypeFilter.Add(extention);
        StorageFile file = await savePicker.PickSingleFileAsync();
        if (file == null) return null;
        return file.Path;
    }
}