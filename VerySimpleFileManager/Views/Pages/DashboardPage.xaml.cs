using VerySimpleFileManager.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace VerySimpleFileManager.Views.Pages;

public partial class DashboardPage : INavigableView<DashboardViewModel>
{
    public DashboardViewModel ViewModel { get; }

    public DashboardPage(DashboardViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}