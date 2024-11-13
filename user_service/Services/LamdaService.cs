using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using user_service.model;
using user_service.Repository.Interfaces;

namespace user_service.Services
{
    public class LambdaService : ILambdaService
    {
        private readonly HttpClient _httpClient;
        private readonly string _lambdaUrl = "https://1co87hes01.execute-api.eu-north-1.amazonaws.com/getUsers/";

        public LambdaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<User>> GetUsersFromLambda()
        {
            var response = await _httpClient.GetAsync(_lambdaUrl);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(responseBody);

            return users;
        }
    }
}