using System.Globalization;
using System.Net.Http.Json;
using System.Windows;
using System.Windows.Controls;
using CatchLog.Wpf.Models;
using CatchLog.Wpf.Services;

namespace CatchLog.Wpf.Views;

public partial class CatchesView : UserControl
{
    public CatchesView()
    {
        InitializeComponent();
        Loaded += async (_, _) => await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            CatchesGrid.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<CatchRecord>>("catches");
            AnglerCombo.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Angler>>("anglers");
            SpotCombo.ItemsSource = await ApiService.Client.GetFromJsonAsync<List<Spot>>("spots");

            if (AnglerCombo.Items.Count > 0) AnglerCombo.SelectedIndex = 0;
            if (SpotCombo.Items.Count > 0) SpotCombo.SelectedIndex = 0;
        }
        catch
        {
            MessageBox.Show("Nem sikerült csatlakozni az API-hoz! Indítsd el előbb a CatchLog.Api projektet!", "Kapcsolati hiba");
        }
    }

    private async void AddCatch_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(SpeciesBox.Text) || !double.TryParse(WeightBox.Text.Replace(',', '.'), CultureInfo.InvariantCulture, out double weight))
        {
            MessageBox.Show("Adj meg érvényes halfajt és súlyt!");
            return;
        }

        var newCatch = new CatchRecord
        {
            Species = SpeciesBox.Text,
            WeightKg = weight,
            AnglerId = (int)AnglerCombo.SelectedValue,
            SpotId = (int)SpotCombo.SelectedValue,
            CaughtAt = DateTime.Now
        };

        await ApiService.Client.PostAsJsonAsync("catches", newCatch);
        SpeciesBox.Clear();
        WeightBox.Clear();
        await LoadData();
    }

    private async void DeleteCatch_Click(object sender, RoutedEventArgs e)
    {
        int id = (int)((Button)sender).Tag;
        await ApiService.Client.DeleteAsync($"catches/{id}");
        await LoadData();
    }

    private async void BiggestCatch_Click(object sender, RoutedEventArgs e)
    {
        var biggest = await ApiService.Client.GetFromJsonAsync<CatchRecord>("catches/biggest");
        if (biggest != null)
            CustomResultText.Text = $"Rekord: {biggest.Species} ({biggest.WeightKg} kg) - Fogta: {biggest.Angler?.Name}";
    }

    private async void TotalWeight_Click(object sender, RoutedEventArgs e)
    {
        if (AnglerCombo.SelectedItem is Angler angler)
        {
            var total = await ApiService.Client.GetFromJsonAsync<double>($"catches/total-weight/{angler.Id}");
            CustomResultText.Text = $"{angler.Name} összes fogása: {total} kg";
        }
    }
}