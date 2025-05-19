namespace albionSCRAPERV2.Services;

public class AlbionApiService
{
    private readonly HttpClient _httpClient;

    AlbionApiService()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://europe.albion-online-data.com")
        };
    }

    public async Task GetPricesAsync(string itemsIds, string location, string qualities)
    {
        var endpoint = $"/api/v2/stats/prices/{itemsIds}.json?locations={location}&qualities={qualities}";
        
        var response = await _httpClient.GetAsync(endpoint);

        response.EnsureSuccessStatusCode();
        
        string json = await response.Content.ReadAsStringAsync();

    }

}