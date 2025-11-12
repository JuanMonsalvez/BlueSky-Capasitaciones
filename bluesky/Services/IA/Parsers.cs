using Newtonsoft.Json;
using System;
using bluesky.Services.IA;

namespace bluesky.Services.IA
{
    public static class Parsers
    {
        public static EvalResult ParseEvalResult(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new Exception("JSON vacío desde IA.");
            // Algunos modelos devuelven Markdown con ```json ... ```
            json = json.Trim();
            if (json.StartsWith("```"))
            {
                var idx = json.IndexOf('{');
                var last = json.LastIndexOf('}');
                if (idx >= 0 && last > idx) json = json.Substring(idx, (last - idx) + 1);
            }

            var res = JsonConvert.DeserializeObject<EvalResult>(json);
            if (res == null) throw new Exception("No se pudo deserializar JSON de IA.");
            return res;
        }
    }
}
