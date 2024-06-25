namespace BlazorApp.Web;

public class ApiClient(HttpClient httpClient)
{
    public Task<T> GetFromJsonAsync<T>(string path)
    {

        return httpClient.GetFromJsonAsync<T>(path);
    }
}

