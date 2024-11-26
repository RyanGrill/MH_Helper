using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FirebaseService
{
    private readonly HttpClient _httpClient;
    private const string FirestoreUrl = "https://mh-world-weakness-finder.firebasestorage.app"; 
    private const string ApiKey = "AIzaSyD-zVvlbSZ6knzVLJE9ayYk7uhfL2GKSB0";

    public FirebaseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Function to perform search in Firestore collection
    public async Task<SearchResult> SearchDocuments(string query)
    {
        var url = $"{FirestoreUrl}:runQuery?key={ApiKey}";

        // Construct Firestore query for searching
        var queryBody = new
        {
            structuredQuery = new
            {
                from = new[] {
                    new { collectionId = "{Monsters}" } 
                },
                where = new
                {
                    fieldFilter = new
                    {
                        field = new { fieldPath = "Name" },
                        op = "GREATER_THAN_OR_EQUAL",
                        value = new { stringValue = query }
                    }
                }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(url, queryBody);
        response.EnsureSuccessStatusCode();
        var jsonResponse = await response.Content.ReadAsStringAsync();

        // Deserialize response into a custom result object
        return JsonConvert.DeserializeObject<SearchResult>(jsonResponse);
    }
}

public class SearchResult
{
    public Document[] Documents { get; set; }
}

public class Document
{
    public string Name { get; set; }
    
}