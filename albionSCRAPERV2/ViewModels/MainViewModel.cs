using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Windows.Input;
using albionSCRAPERV2.Models;

namespace albionSCRAPERV2.ViewModels;

public class MainViewModel
{
     public ObservableCollection<Item> AllItems { get; } = new();
    public ObservableCollection<Item> FilteredItems { get; } = new();

    public ObservableCollection<string> Categories { get; } = new();
    public ObservableCollection<string> Subcategories { get; } = new();
    public ObservableCollection<string> Factions { get; } = new();
    public ICommand LoadNextItemCommand { get; }
    public ICommand SearchingItemsCommand { get; }
    
    public Action? OnOpenSearchingItemsView { get; set; }

    private string? selectedCategory;
    public string? SelectedCategory
    {
        get => selectedCategory;
        set
        {
            if (SetProperty(ref selectedCategory, value))
            {
                UpdateSubcategories();
                FilterItems();
            }
        }
    }

    private string? selectedSubcategory;
    public string? SelectedSubcategory
    {
        get => selectedSubcategory;
        set
        {
            if (SetProperty(ref selectedSubcategory, value))
            {
                FilterItems();
            }
        }
    }

    private string? selectedFaction;
    public string? SelectedFaction
    {
        get => selectedFaction;
        set
        {
            if (SetProperty(ref selectedFaction, value))
            {
                FilterItems();
            }
        }
    }

    public MainViewModel()
    {
        LoadDataFromEmbeddedJson();
        LoadNextItemCommand = new Command(LoadNextItem);
        SearchingItemsCommand = new Command(SearchingItems);

    }

    private void LoadDataFromEmbeddedJson()
    {
        try
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "albionSCRAPERV2fa.Data.coscos.json";

            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            var names = assembly.GetManifestResourceNames();
            foreach (var name in names)
            {
                Console.WriteLine("Zasob:" + name);
            }

            if (stream == null) return;


            using var reader = new StreamReader(stream);
            string json = reader.ReadToEnd();

            var items = JsonSerializer.Deserialize<List<Item>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (items != null)
            {
                foreach (var item in items)
                {
                    if (!string.IsNullOrWhiteSpace(item.UniqueName) && item.UniqueName.StartsWith("T"))
                        AllItems.Add(item);
                }
            }

            Categories.Clear();
            foreach (var cat in AllItems.Select(i => i.Category).Distinct().OrderBy(c => c))
            {
                Categories.Add(cat);
                
                Factions.Clear();
                foreach (var faction in AllItems.Select(i => i.Faction).Distinct().OrderBy(f => f))
                    Factions.Add(faction);

                FilterItems();
            }
            Console.WriteLine("Wczytano rekordów: " + AllItems.Count);
            
        }
        catch (Exception ex)
        {
            Console.WriteLine("Błąd ładowania danych: " + ex.Message);
        }
    }

    private void UpdateSubcategories()
    {
        Subcategories.Clear();

        if (string.IsNullOrWhiteSpace(SelectedCategory)) return;

        var subs = AllItems
            .Where(i => i.Category == SelectedCategory)
            .Select(i => i.Subcategory)
            .Distinct()
            .OrderBy(s => s);

        foreach (var sub in subs)
            Subcategories.Add(sub);
    }

    private void FilterItems()
    {
        FilteredItems.Clear();

        //var filtered = AllItems.AsEnumerable();
        //
        // if (!string.IsNullOrWhiteSpace(SelectedCategory))
        //     filtered = filtered.Where(i => i.Category == SelectedCategory);
        //
        // if (!string.IsNullOrWhiteSpace(SelectedSubcategory))
        //     filtered = filtered.Where(i => i.Subcategory == SelectedSubcategory);
        //
        // if (!string.IsNullOrWhiteSpace(SelectedFaction))
        //     filtered = filtered.Where(i => i.Faction == SelectedFaction);

        foreach (var item in AllItems)
            FilteredItems.Add(item);
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
        storage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
    
    private void LoadNextItem()
    {
        
    }
    
    private void SearchingItems()
    {
        OnOpenSearchingItemsView?.Invoke();
    }
}