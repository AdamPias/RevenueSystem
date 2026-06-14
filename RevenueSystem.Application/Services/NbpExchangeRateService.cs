using System.Net;
using System.Text.Json;
using RevenueSystem.Application.Services;

namespace RevenueSystem.Infrastructure.Services;

public class NbpExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public NbpExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> GetExchangeRateAsync(string currencyCode)
    {
        if (string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Equals("PLN", StringComparison.OrdinalIgnoreCase))
            return 1m;

        try
        {
            var responseA = await _httpClient.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode.ToLower()}/?format=json");
            
            if (responseA.IsSuccessStatusCode)
            {
                return await ParseNbpResponseAsync(responseA);
            }
            
            if (responseA.StatusCode == HttpStatusCode.NotFound)
            {
                var responseB = await _httpClient.GetAsync($"http://api.nbp.pl/api/exchangerates/rates/b/{currencyCode.ToLower()}/?format=json");
                
                if (responseB.IsSuccessStatusCode)
                {
                    return await ParseNbpResponseAsync(responseB);
                }
            }
            
            throw new ArgumentException($"Nie udało się pobrać kursu dla waluty {currencyCode.ToUpper()}. Upewnij się, że podano poprawny 3-literowy kod waluty.");
        }
        catch (HttpRequestException)
        {
            throw new Exception("Błąd połączenia z serwisem NBP.");
        }
    }
    
    private async Task<decimal> ParseNbpResponseAsync(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        var nbpData = JsonSerializer.Deserialize<NbpResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (nbpData?.Rates == null || nbpData.Rates.Count == 0)
            throw new Exception("Brak danych o kursie w odpowiedzi z NBP.");

        return nbpData.Rates[0].Mid;
    }
    
    private class NbpResponse
    {
        public List<NbpRate> Rates { get; set; } = new();
    }

    private class NbpRate
    {
        public decimal Mid { get; set; }
    }
}