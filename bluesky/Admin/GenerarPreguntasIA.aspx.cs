using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.UI;
using bluesky.App_Code;            // AdminPage
using bluesky.Models;              // DbContext + modelos
using bluesky.Services.IA;         // GeminiClient
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bluesky.Admin
{
    public partial class GenerarPreguntasIA : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            int evaluacionId;
            if (!int.TryParse(Request.QueryString["evaluacionId"], out evaluacionId))
            {
                lblEstado.Text = "Falta ?evaluacionId en la URL.";
                btnGenerar.Enabled = false;
                return;
            }

            using (var db = new ApplicationDbContext())
            {
                var eva = db.Evaluaciones.Find(evaluacionId);
                if (eva == null)
                {
                    lblEstado.Text = "Evaluación no encontrada.";
                    btnGenerar.Enabled = false;
                    return;
                }

                var curso = db.Cursos.Find(eva.CursoId);
                if (curso == null)
                {
                    lblEstado.Text = "Curso no encontrado.";
                    btnGenerar.Enabled = false;
                    return;
                }

                // Validar que el curso tenga materiales (PDF/PPT/etc)
                var tieneMaterial = db.CursoMateriales
                    .Any(m => m.CursoId == curso.Id && m.Activo);

                if (!tieneMaterial)
                {
                    lblEstado.Text = "⚠️ Este curso no tiene materiales. Agrega PDF/PPT antes de generar con IA.";
                    btnGenerar.Enabled = false;
                    return;
                }

                hfEvaluacionId.Value = eva.Id.ToString();
                hfCursoId.Value = curso.Id.ToString();

                lblCurso.Text = curso.Titulo;
                lblEvaluacion.Text = $"{eva.Titulo} (Tipo: {eva.Tipo}, Versión: {eva.Version})";
            }
        }

        protected async void btnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                int cursoId, evaluacionId;
                if (!int.TryParse(hfCursoId.Value, out cursoId) ||
                    !int.TryParse(hfEvaluacionId.Value, out evaluacionId))
                {
                    lblEstado.Text = "IDs inválidos en el formulario.";
                    return;
                }

                var cantidad = int.Parse(ddlCantidad.SelectedValue);         // 5, 10, 15…
                var dificultad = ddlDificultad.SelectedValue;                // basica|media|avanzada

                string prompt;
                Curso curso;
                Evaluacion eva;

                using (var db = new ApplicationDbContext())
                {
                    curso = db.Cursos.Find(cursoId);
                    eva = db.Evaluaciones.Find(evaluacionId);

                    // Revalidar que haya materiales activos
                    var materiales = db.CursoMateriales
                        .Where(m => m.CursoId == cursoId && m.Activo)
                        .Select(m => new { m.NombreArchivo, m.Tipo, m.RutaAlmacenamiento })
                        .ToList();

                    if (materiales.Count == 0)
                    {
                        lblEstado.Text = "⚠️ Este curso no tiene materiales. Agrega PDF/PPT antes de generar con IA.";
                        return;
                    }

                    var listado = string.Join("\n", materiales.Take(12)
                        .Select(m => $"- {m.NombreArchivo} ({m.Tipo}) [{m.RutaAlmacenamiento}]"));

                    // Prompt endurecido para forzar JSON puro
                    prompt =
$@"Eres un generador de evaluaciones. Crea {cantidad} preguntas de alternativa única
para el curso '{curso.Titulo}'. Nivel: {dificultad}.
Usa como referencia los materiales listados (títulos/rutas). No inventes rutas.

DEVUELVE SOLO JSON VÁLIDO (sin ``` ni texto adicional).
Estructura exacta:
{{
  ""preguntas"": [
    {{
      ""enunciado"": ""texto de la pregunta"",
      ""alternativas"": [
        {{ ""texto"": ""..."", ""correcta"": true }},
        {{ ""texto"": ""..."", ""correcta"": false }},
        {{ ""texto"": ""..."", ""correcta"": false }},
        {{ ""texto"": ""..."", ""correcta"": false }}
      ],
      ""categoria"": ""opcional"",
      ""dificultad"": ""basica|media|avanzada""
    }}
  ]
}}

Materiales:
{listado}";
                }

                lblEstado.Text = "Llamando a IA…";
                var raw = await GeminiClient.GenerateTextAsync(prompt);

                // Parseo robusto del output
                var (ok, parsed, errorMsg) = ParseIaOutput(raw, cantidad);

                if (!ok || parsed == null || parsed.Count == 0)
                {
                    lblEstado.Text = "❌ No se pudo interpretar el JSON de la IA. " +
                                     (string.IsNullOrWhiteSpace(errorMsg) ? "" : ("Detalle: " + errorMsg));
                    pnlPreview.Visible = true;
                    litPreview.Text = Server.HtmlEncode(raw); // para diagnóstico
                    return;
                }

                // Normalizar alternativas: 4 opciones, exactamente 1 correcta
                foreach (var p in parsed)
                {
                    var alts = (p.Alternativas ?? new List<IaAlt>())
                        .Where(a => !string.IsNullOrWhiteSpace(a.Texto))
                        .Take(4)
                        .ToList();

                    while (alts.Count < 4) alts.Add(new IaAlt { Texto = "Opción", Correcta = false });

                    if (!alts.Any(a => a.Correcta))
                        alts[0].Correcta = true;

                    if (alts.Count(a => a.Correcta) > 1)
                    {
                        bool marcada = false;
                        for (int i = 0; i < alts.Count; i++)
                        {
                            if (alts[i].Correcta && !marcada) { marcada = true; continue; }
                            alts[i].Correcta = false;
                        }
                    }

                    p.Alternativas = alts;
                }

                var count = await GuardarPreguntasAsync(evaluacionId, parsed);

                pnlPreview.Visible = true;
                litPreview.Text = string.Join(
                    "\n\n",
                    parsed.Take(3).Select(p =>
                        $"- {p.Enunciado}\n  A) {p.Alternativas[0].Texto}\n  B) {p.Alternativas[1].Texto}\n  C) {p.Alternativas[2].Texto}\n  D) {p.Alternativas[3].Texto}"));

                lblEstado.Text = $"✅ Se guardaron {count} preguntas.";
                // Si quieres redirigir al listado:
                // Response.Redirect("~/Admin/AdminEvaluaciones.aspx");
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error al generar: " + ex.Message;
            }
        }

        // ======== Tipos DTO de parseo ========

        private sealed class IaAlt
        {
            public string Texto { get; set; }
            public bool Correcta { get; set; }
        }

        private sealed class IaPregunta
        {
            public string Enunciado { get; set; }
            public string Categoria { get; set; }
            public string Dificultad { get; set; }
            public List<IaAlt> Alternativas { get; set; } = new List<IaAlt>();
        }

        // ======== Parseo robusto del output de IA ========

        private static string StripCodeFences(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return raw;
            raw = raw.Trim();

            // Quita ```json ... ``` o ``` ... ```
            if (raw.StartsWith("```"))
            {
                int firstNl = raw.IndexOf('\n');
                if (firstNl >= 0) raw = raw.Substring(firstNl + 1);

                int lastFence = raw.LastIndexOf("```", StringComparison.Ordinal);
                if (lastFence > 0) raw = raw.Substring(0, lastFence);
            }
            return raw.Trim();
        }

        private static string ExtractJsonBlock(string raw)
        {
            // 1) si ya es JSON válido, úsalo
            try { JToken.Parse(raw); return raw; } catch { /* sigue */ }

            // 2) tomar desde la primera '{' hasta la última '}' que contenga "preguntas"
            int first = raw.IndexOf('{');
            int last = raw.LastIndexOf('}');
            if (first >= 0 && last > first)
            {
                var sub = raw.Substring(first, last - first + 1);
                if (sub.Contains("\"preguntas\"") || sub.Contains("'preguntas'"))
                    return sub;
            }

            // 3) búsqueda simple de bloque con "preguntas"
            for (int i = 0; i < raw.Length; i++)
            {
                if (raw[i] == '{')
                {
                    for (int j = raw.Length - 1; j > i; j--)
                    {
                        if (raw[j] == '}')
                        {
                            var sub = raw.Substring(i, j - i + 1);
                            if (sub.Contains("\"preguntas\"") || sub.Contains("'preguntas'"))
                                return sub;
                        }
                    }
                }
            }

            return raw; // último recurso (fallará en el Parse y lo informaremos)
        }

        private (bool ok, List<IaPregunta> preguntas, string error) ParseIaOutput(string raw, int expected)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(raw))
                    return (false, null, "Respuesta vacía de IA.");

                var clean = StripCodeFences(raw);
                var json = ExtractJsonBlock(clean);

                var root = JToken.Parse(json);

                List<IaPregunta> result = null;

                if (root.Type == JTokenType.Object && root["preguntas"] != null)
                {
                    result = root["preguntas"].ToObject<List<IaPregunta>>();
                }
                else if (root.Type == JTokenType.Array)
                {
                    result = root.ToObject<List<IaPregunta>>();
                }

                if (result == null)
                    return (false, null, "No se encontró el arreglo 'preguntas'.");

                result = result
                    .Where(p => p != null && !string.IsNullOrWhiteSpace(p.Enunciado))
                    .Take(Math.Max(1, expected))
                    .ToList();

                if (result.Count == 0)
                    return (false, null, "El arreglo 'preguntas' quedó vacío tras limpieza.");

                return (true, result, null);
            }
            catch (Exception ex)
            {
                return (false, null, "Excepción al parsear: " + ex.Message);
            }
        }

        // ======== Guardado en BD ========

        private async Task<int> GuardarPreguntasAsync(int evaluacionId, List<IaPregunta> preguntas)
        {
            using (var db = new ApplicationDbContext())
            {
                var eva = db.Evaluaciones.Find(evaluacionId);
                if (eva == null) throw new InvalidOperationException("Evaluación no encontrada.");

                int inserted = 0;
                foreach (var p in preguntas)
                {
                    if (string.IsNullOrWhiteSpace(p.Enunciado)) continue;

                    var pregunta = new Pregunta
                    {
                        EvaluacionId = evaluacionId,
                        Enunciado = p.Enunciado.Trim(),
                        Categoria = string.IsNullOrWhiteSpace(p.Categoria) ? null : p.Categoria.Trim(),
                        Dificultad = (DificultadPregunta)DificultadFromString(p.Dificultad),
                        MultipleRespuesta = false,
                        Orden = inserted + 1,
                        Activa = true
                    };
                    db.Preguntas.Add(pregunta);
                    await db.SaveChangesAsync();

                    int ord = 1;
                    foreach (var a in p.Alternativas)
                    {
                        db.Alternativas.Add(new Alternativa
                        {
                            PreguntaId = pregunta.Id,
                            Texto = string.IsNullOrWhiteSpace(a.Texto) ? "Opción" : a.Texto.Trim(),
                            EsCorrecta = a.Correcta,
                            Orden = ord++,
                            Activa = true
                        });
                    }

                    await db.SaveChangesAsync();
                    inserted++;
                }

                return inserted;
            }
        }

        private int DificultadFromString(string s)
        {
            var v = (s ?? "").Trim().ToLowerInvariant();
            if (v.StartsWith("ava")) return 3; // avanzada
            if (v.StartsWith("bas")) return 1; // básica
            return 2;                          // media (default)
        }
    }
}
