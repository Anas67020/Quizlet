using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Quizlet.Model
{
    public class QuizApi
    {
        private readonly HttpClient client;

        public QuizApi()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://quizzapp.game-creators.de/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void SetToken(string token)
        {
            client.DefaultRequestHeaders.Remove("X-Auth-Token");
            if (string.IsNullOrWhiteSpace(token) == false)
                client.DefaultRequestHeaders.Add("X-Auth-Token", token);
        }

        // PUT /api/v1/user/{email}/signup
        public async Task<HttpResponseMessage> SignupAsync(string email, SignupRequest body)
        {
            string safeEmail = Uri.EscapeDataString(email);

            string json = JsonConvert.SerializeObject(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // User registrieren
            return await client.PutAsync($"api/v1/user/{safeEmail}/signup", content);
        }

        // GET /api/v1/user/{user}/signin (mit JSON Body)
        public async Task<HttpResponseMessage> SigninAsync(string user, string password)
        {
            string safeUser = Uri.EscapeDataString(user);

            var body = new SigninRequest();
            body.Password = password;

            string json = JsonConvert.SerializeObject(body);

            var req = new HttpRequestMessage(HttpMethod.Get, $"api/v1/user/{safeUser}/signin");
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // User einloggen
            return await client.SendAsync(req);
        }

        // GET /api/v1/user/{user}
        public async Task<HttpResponseMessage> GetUserAsync(string token, string user)
        {
            SetToken(token);

            string safeUser = Uri.EscapeDataString(user);

            // Userdaten laden
            return await client.GetAsync($"api/v1/user/{safeUser}");
        }

        // PATCH /api/v1/user/{user}
        public async Task<HttpResponseMessage> UpdateUserAsync(string token, string user, UpdateUserRequest body)
        {
            SetToken(token);

            string safeUser = Uri.EscapeDataString(user);

            string json = JsonConvert.SerializeObject(body);

            var req = new HttpRequestMessage(new HttpMethod("PATCH"), $"api/v1/user/{safeUser}");
            req.Content = new StringContent(json, Encoding.UTF8, "application/json");

            // Userdaten ändern
            return await client.SendAsync(req);
        }
    }
}
