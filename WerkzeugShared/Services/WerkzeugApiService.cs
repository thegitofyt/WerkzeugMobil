using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using WerkzeugShared.DTO;

namespace WerkzeugShared.Services
{
    public class WerkzeugApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000"; // Or your actual host IP + port

        public WerkzeugApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<WerkzeugDto>> GetWerkzeugeAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<WerkzeugDto>>($"{BaseUrl}/werkzeuge")
                   ?? new List<WerkzeugDto>();
        }

        public async Task<List<ProjektDTO>> GetProjekteAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProjektDTO>>($"{BaseUrl}/werkzeuge/projekte")
                   ?? new List<ProjektDTO>();
        }

        public async Task<List<ToolDTO>> GetToolsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ToolDTO>>($"{BaseUrl}/werkzeuge/tools")
                   ?? new List<ToolDTO>();
        }
    }
}

