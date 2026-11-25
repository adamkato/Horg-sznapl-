using System.Windows;
using CatchLog.Wpf.Views;

namespace CatchLog.Wpf;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainContent.Content = new CatchesView();
    }

    private void OpenCatches_Click(object sender, RoutedEventArgs e) => MainContent.Content = new CatchesView();
    private void OpenAnglers_Click(object sender, RoutedEventArgs e) => MainContent.Content = new AnglersView();
    private void OpenSpots_Click(object sender, RoutedEventArgs e) => MainContent.Content = new SpotsView();
}