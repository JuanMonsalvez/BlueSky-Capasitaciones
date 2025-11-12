using System;
using System.Collections.Generic;
using System.Linq;

namespace bluesky.Services.IA
{
    public static class Validadores
    {
        public static void ValidaEstructura(EvalResult r, out List<string> errores)
        {
            errores = new List<string>();
            if (r == null) { errores.Add("Sin objeto resultado"); return; }
            if (r.Preguntas == null || r.Preguntas.Count == 0)
                errores.Add("Sin preguntas");

            var ens = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var p in r.Preguntas ?? Enumerable.Empty<EvalPregunta>())
            {
                if (string.IsNullOrWhiteSpace(p.Enunciado))
                    errores.Add("Pregunta sin enunciado");
                else
                {
                    var key = p.Enunciado.Trim();
                    if (ens.Contains(key)) errores.Add("Enunciado duplicado: " + key);
                    ens.Add(key);
                }

                if (p.Alternativas == null || p.Alternativas.Count < 3)
                    errores.Add("Pregunta con menos de 3 alternativas");

                var correctas = p.Alternativas?.Count(a => a.EsCorrecta) ?? 0;
                if (p.MultipleRespuesta)
                {
                    if (correctas < 2 || correctas > 3)
                        errores.Add("Pregunta múltiple con #correctas inválidas (esperado 2-3)");
                }
                else
                {
                    if (correctas != 1) errores.Add("Pregunta simple debe tener exactamente 1 correcta");
                }

                foreach (var a in p.Alternativas ?? Enumerable.Empty<EvalAlt>())
                {
                    if (string.IsNullOrWhiteSpace(a.Texto))
                        errores.Add("Alternativa vacía");
                }
            }
        }
    }
}
