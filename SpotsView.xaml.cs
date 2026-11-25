using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using CatchLog.Wpf.Models;
using CatchLog.Wpf.Services;

namespace CatchLog.Wpf.Views;

public partial class SpotsView : UserControl
{
    public SpotsView()
    {
        InitializeComponent();
        Loaded += async (_, _) => await LoadData();
    }

    private async Task LoadData() =>
        SpotsGrid.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Spot>>("spots");

    private async void Add_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SpotNameBox.Text)) return;
        await ApiService.Client.PostAsJsonAsync("spots", new Spot { Name = SpotNameBox.Text, WaterType = WaterTypeBox.Text });
        SpotNameBox.Clear();
        WaterTypeBox.Clear();
        await LoadData();
    }

    private async void Delete_Click(object sender, RoutedEventArgs e)
    {
        int id = (int)((Button)sender).Tag;
        await ApiService.Client.DeleteAsync($"spots/{id}");
        await LoadData();
    }

    private async void FilterLake_Click(object sender, RoutedEventArgs e) =>
        SpotsGrid.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Spot>>("spots/by-watertype/Tó");

    private async void FilterRiver_Click(object sender, RoutedEventArgs e) =>
        SpotsGrid.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Spot>>("spots/by-watertype/Folyó");

    private async void Reset_Click(object sender, RoutedEventArgs e) => await LoadData();
}