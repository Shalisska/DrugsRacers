using DrugsRacers.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers
{
    static class Infra
    {
        const string realAddress = "http://51.15.100.12:5000/";

        const string anotherAddress = "http://51.158.109.80:5000/";

        const string testAddress = "http://127.0.0.1:5000/";

        const string currentAddress = anotherAddress;

        static TokenDto _token;

        public static void Init()
        {
            var login = new LoginDto();
            _token = Post<TokenDto>("raceapi/Auth/Login", login);
        }

        public static T Post<T>(string path, object body, bool isTokenNeed = false) where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(currentAddress);
                if (isTokenNeed)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.Token);
                var response = client.PostAsJsonAsync(path, body);
                var content = response.Result;
                T result = null;
                if (response.IsCompleted)
                {
                    result = content.Content.ReadAsAsync<T>().Result;
                }
                return result;
            }
        }

        public static T Put<T>(string path, object body, bool isTokenNeed = false) where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(currentAddress);
                if (isTokenNeed)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.Token);
                var response = client.PutAsJsonAsync(path, body);
                var content = response.Result;
                T result = null;
                if (response.IsCompleted)
                {
                    result = content.Content.ReadAsAsync<T>().Result;
                }
                return result;
            }
        }

        public static T Get<T>(string path, bool isTokenNeed = false) where T : class
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(currentAddress);
                if (isTokenNeed)
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.Token);
                var response = client.GetAsync(path);
                var content = response.Result;
                T result = null;
                if (response.IsCompleted)
                {
                    result = content.Content.ReadAsAsync<T>().Result;
                }
                return result;
            }
        }

        public static void Get(string sessionId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(testAddress);
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token.Token);
                    var response = client.GetAsync($"/raceapi/race?sessionId={sessionId}");
                    var content = response.Result;
                }
            }
            catch { }
        }

        public static PlayerSessionInfoDto GetSessionInfo()
        {
            var map = new PlayDto() { Map = "yinyang" };
            var info = Infra.Post<PlayerSessionInfoDto>("raceapi/race", map, true);
            return info;
        }
    }
}
