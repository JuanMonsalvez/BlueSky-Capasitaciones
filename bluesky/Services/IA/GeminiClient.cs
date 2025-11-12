using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace bluesky.Services.IA
{
    public static class GeminiClient
    {
        private static readonly HttpClient http = new HttpClient();

        /// <summary>
        /// Genera texto usando Google Generative Language API (Gemini).
        /// Lee la clave desde env GEMINI_API_KEY o AppSettings["GEMINI_API_KEY"].
        /// </summary>
        public static async Task<string> GenerateTextAsync(string prompt)
        {
            // 1) API key
            var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY")
                         ?? ConfigurationManager.AppSettings["GEMINI_API_KEY"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("Falta configurar GEMINI_API_KEY en Web.config o como variable de entorno.");

            // 2) Endpoint y modelo (con defaults)
            var endpoint = ConfigurationManager.AppSettings["GEMINI_ENDPOINT"];
            if (string.IsNullOrWhiteSpace(endpoint))
                endpoint = "https://generativelanguage.googleapis.com";

            var model = ConfigurationManager.AppSettings["GEMINI_MODEL"];
            if (string.IsNullOrWhiteSpace(model))
                model = "gemini-1.5-pro";

            // 3) URL (v1beta)
            var url = endpoint.TrimEnd('/') + "/v1beta/models/" + model + ":generateContent?key=" + apiKey;

            // 4) Payload básico
            var payload = new
            {
                contents = new[]
                {
                    new {
                        parts = new[] { new { text = prompt } }
                    }
                }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            using (var req = new HttpRequestMessage(HttpMethod.Post, url))
            {
                req.Headers.Accept.Clear();
                req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                req.Content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var resp = await http.SendAsync(req))
                {
                    var raw = await resp.Content.ReadAsStringAsync();

                    if (!resp.IsSuccessStatusCode)
                        throw new InvalidOperationException("Gemini error " + (int)resp.StatusCode + " " + resp.ReasonPhrase + ": " + raw);

                    // Extraer texto de candidates[0].content.parts[0].text
                    try
                    {
                        dynamic root = Newtonsoft.Json.JsonConvert.DeserializeObject(raw);
                        if (root == null || root.candidates == null || root.candidates.Count == 0) return raw;

                        var first = root.candidates[0];
                        if (first == null || first.content == null || first.content.parts == null || first.content.parts.Count == 0) return raw;

                        string text = first.content.parts[0].text;
                        return string.IsNullOrWhiteSpace(text) ? raw : text;
                    }
                    catch
                    {
                        return raw; // fallback crudo
                    }
                }
            }
        }

        // --- Diagnóstico compatible C#6 (sin tuplas) ---
        public class KeyStatus
        {
            public bool Ok { get; set; }
            public string Source { get; set; }  // "ENV" | "Web.config" | "missing"
        }

        public static KeyStatus CheckKey()
        {
            var env = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (!string.IsNullOrWhiteSpace(env)) return new KeyStatus { Ok = true, Source = "ENV" };

            var app = ConfigurationManager.AppSettings["GEMINI_API_KEY"];
            if (!string.IsNullOrWhiteSpace(app)) return new KeyStatus { Ok = true, Source = "Web.config" };

            return new KeyStatus { Ok = false, Source = "missing" };
        }
    }
}
