using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using CatchLog.Wpf.Models;
using CatchLog.Wpf.Services;

namespace CatchLog.Wpf.Views;

public partial class AnglersView : UserControl
{
    public AnglersView()
    {
        InitializeComponent();
        Loaded += async (_, _) => await LoadData();
    }

    private async Task LoadData() =>
        AnglersGrid.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Angler>>("anglers");

    private async void Add_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameBox.Text)) return;
        await ApiService.Client.PostAsJsonAsync("anglers", new Angler { Name = NameBox.Text, LicenseNumber = LicenseBox.Text });
        NameBox.Clear();
        LicenseBox.Clear();
        await LoadData();
    }

    private async void Delete_Click(object sender, RoutedEventArgs e)
    {
        int id = (int)((Button)sender).Tag;
        await ApiService.Client.DeleteAsync($"anglers/{id}");
        await LoadData();
    }

    private async void Search_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SearchBox.Text)) { await LoadData(); return; }
        AnglersGrid.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Angler>>($"anglers/search/{SearchBox.Text}");
    }

    private async void Reset_Click(object sender, RoutedEventArgs e)
    {
        SearchBox.Clear();
        await LoadData();
    }
}