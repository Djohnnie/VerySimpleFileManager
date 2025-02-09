using System.Diagnostics;
using VerySimpleFileManager.ViewModels.Pages;
using Wpf.Ui.Controls;

namespace VerySimpleFileManager.Views.Pages;

public partial class FileBrowserPage : INavigableView<FileBrowserViewModel>
{
    public FileBrowserViewModel ViewModel { get; }

    public FileBrowserPage(FileBrowserViewModel viewModel)
    {
        ViewModel = viewModel;

        Debug.WriteLine(this.Tag);
        DataContext = this;

        InitializeComponent();
    }

    internal void SetPageTag(string pageTag)
    {
        ViewModel.Temp = pageTag;
    }

    private void Page_Unloaded(object sender, RoutedEventArgs e)
    {
        ViewModel.Dispose();
    }
}