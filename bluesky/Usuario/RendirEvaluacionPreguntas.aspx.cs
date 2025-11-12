using bluesky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace bluesky.Usuario
{
    public partial class RendirEvaluacionPreguntas : System.Web.UI.Page
    {
        // ViewState keys
        private const string VS_IntentoId = "INTENTO_ID";
        private const string VS_Preguntas = "PREG_LIST";
        private const string VS_Idx = "IDX";
        private const string VS_EvalId = "EVAL_ID";
        private const string VS_EndUtc = "END_UTC"; // DateTime (UTC) fin

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 1) validar querystring
                int intentoId;
                if (!int.TryParse(Request.QueryString["intentoId"], out intentoId) || intentoId <= 0)
                {
                    lblMsg.Text = "Falta el parámetro intentoId.";
                    pnlPregunta.Visible = false;
                    return;
                }

                using (var db = new ApplicationDbContext())
                {
                    var intento = db.IntentosEvaluacion.Find(intentoId);
                    if (intento == null)
                    {
                        lblMsg.Text = "El intento no existe.";
                        pnlPregunta.Visible = false;
                        return;
                    }

                    var eval = db.Evaluaciones.Find(intento.EvaluacionId);
                    if (eval == null || !eval.Activa)
                    {
                        lblMsg.Text = "La evaluación no está disponible.";
                        pnlPregunta.Visible = false;
                        return;
                    }

                    // Si ya finalizado => resultado
                    if (intento.FechaFin.HasValue)
                    {
                        Response.Redirect("~/Usuario/ResultadoEvaluacion.aspx?intentoId=" + intento.Id);
                        return;
                    }

                    // Asegura FechaInicio
                    if (intento.FechaInicio == default(DateTime))
                    {
                        intento.FechaInicio = DateTime.UtcNow;
                        db.SaveChanges();
                    }

                    // Calcula fin y lo cachea
                    var endUtc = intento.FechaInicio.AddMinutes(eval.TiempoMinutos);
                    ViewState[VS_EndUtc] = endUtc;
                    ViewState[VS_IntentoId] = intento.Id;
                    ViewState[VS_EvalId] = eval.Id;

                    // Prepara lista de preguntas
                    var preguntas = db.Preguntas
                        .Where(p => p.EvaluacionId == eval.Id && p.Activa)
                        .OrderBy(p => p.Orden)
                        .Select(p => p.Id)
                        .ToList();

                    if (preguntas.Count == 0)
                    {
                        lblMsg.Text = "La evaluación no tiene preguntas activas.";
                        pnlPregunta.Visible = false;
                        return;
                    }

                    ViewState[VS_Preguntas] = preguntas;
                    ViewState[VS_Idx] = 0;

                    lblTituloEval.Text = eval.Titulo;
                    lblProgreso.Text = $"1 / {preguntas.Count}";

                    // Inicializa etiqueta de tiempo (mm:ss) y payload para JS
                    var secondsLeft = Math.Max(0, (int)(endUtc - DateTime.UtcNow).TotalSeconds);
                    lblTiempo.Text = FormatMMSS(secondsLeft);
                    hfEndEpochMs.Value = ToEpochMs(endUtc).ToString();

                    // Inyecta el script del cronómetro (solo la primera vez)
                    RegisterCountdownScript();

                    // Si el tiempo se acabó justo ahora, finaliza
                    if (secondsLeft <= 0)
                    {
                        FinalizarYCalcular(intento.Id);
                        return;
                    }

                    BindPreguntaActual();
                }
            }
            else
            {
                // En postbacks, si el tiempo expiró, finaliza
                if (TiempoExpirado())
                {
                    FinalizarYCalcular(IntentoId);
                    return;
                }
            }
        }

        private bool TiempoExpirado()
        {
            if (ViewState[VS_EndUtc] == null) return false;
            var endUtc = (DateTime)ViewState[VS_EndUtc];
            return DateTime.UtcNow >= endUtc;
        }

        private static string FormatMMSS(int totalSeconds)
        {
            var m = totalSeconds / 60;
            var s = totalSeconds % 60;
            return $"{m:00}:{s:00}";
        }

        private static long ToEpochMs(DateTime utc)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(utc - epoch).TotalMilliseconds;
        }

        private void RegisterCountdownScript()
        {
            var script = $@"
(function(){{
  var endMs = parseInt(document.getElementById('{hfEndEpochMs.ClientID}').value || '0', 10);
  function pad(n){{return n<10?'0'+n:n;}}
  function tick(){{
    var now = Date.now();
    var left = Math.max(0, Math.floor((endMs - now)/1000));
    var el = document.getElementById('{lblTiempo.ClientID}');
    if (el) el.textContent = pad(Math.floor(left/60))+':'+pad(left%60);
    if (left <= 0){{
      clearInterval(timer);
      // dispara postback a Finalizar
      __doPostBack('{btnFinalizar.UniqueID}','');
    }}
  }}
  var timer = setInterval(tick, 1000);
  tick();
}})();";
            ClientScript.RegisterStartupScript(this.GetType(), "timerEval", script, true);
        }

        private int IntentoId => (int)ViewState[VS_IntentoId];
        private List<int> PregLista => (List<int>)ViewState[VS_Preguntas];
        private int Index
        {
            get { return (int)ViewState[VS_Idx]; }
            set { ViewState[VS_Idx] = value; }
        }

        private void BindPreguntaActual()
        {
            using (var db = new ApplicationDbContext())
            {
                var preguntaId = PregLista[Index];
                var pregunta = db.Preguntas.Find(preguntaId);
                if (pregunta == null)
                {
                    lblMsg.Text = "No se pudo cargar la pregunta.";
                    pnlPregunta.Visible = false;
                    return;
                }

                hfPreguntaId.Value = pregunta.Id.ToString();
                litEnunciado.Text = $"<h4>{pregunta.Orden}. {Server.HtmlEncode(pregunta.Enunciado)}</h4>";

                // cargar alternativas
                var alternativas = db.Alternativas
                    .Where(a => a.PreguntaId == pregunta.Id && a.Activa)
                    .OrderBy(a => a.Orden)
                    .Select(a => new { a.Id, a.Texto })
                    .ToList();

                rblAlternativas.Visible = !pregunta.MultipleRespuesta;
                cblAlternativas.Visible = pregunta.MultipleRespuesta;

                rblAlternativas.Items.Clear();
                cblAlternativas.Items.Clear();

                foreach (var alt in alternativas)
                {
                    if (pregunta.MultipleRespuesta)
                        cblAlternativas.Items.Add(new ListItem(alt.Texto, alt.Id.ToString()));
                    else
                        rblAlternativas.Items.Add(new ListItem(alt.Texto, alt.Id.ToString()));
                }

                // preseleccionar si ya respondió
                var respPrevias = db.IntentosRespuesta
                    .Where(x => x.IntentoEvaluacionId == IntentoId && x.PreguntaId == pregunta.Id)
                    .Select(x => x.AlternativaId)
                    .ToList();

                if (pregunta.MultipleRespuesta)
                {
                    foreach (ListItem it in cblAlternativas.Items)
                        it.Selected = respPrevias.Any(id => id.HasValue && id.Value.ToString() == it.Value);
                }
                else
                {
                    if (respPrevias.Count == 1 && respPrevias[0].HasValue)
                        rblAlternativas.SelectedValue = respPrevias[0].Value.ToString();
                }

                // navegación
                btnAnterior.Visible = Index > 0;
                var esUltima = Index == PregLista.Count - 1;
                btnSiguiente.Visible = !esUltima;
                btnFinalizar.Visible = esUltima;

                lblProgreso.Text = $"{Index + 1} / {PregLista.Count}";
                lblMsg.Text = "";
            }
        }

        private bool GuardarRespuestaActual()
        {
            int preguntaId = int.Parse(hfPreguntaId.Value);

            using (var db = new ApplicationDbContext())
            {
                // limpiar respuestas previas de esa pregunta (upsert simple)
                var prev = db.IntentosRespuesta
                    .Where(x => x.IntentoEvaluacionId == IntentoId && x.PreguntaId == preguntaId)
                    .ToList();
                if (prev.Count > 0)
                {
                    db.IntentosRespuesta.RemoveRange(prev);
                    db.SaveChanges();
                }

                // recolectar selección actual
                var seleccion = new List<int>();

                if (rblAlternativas.Visible)
                {
                    int altId;
                    if (!string.IsNullOrEmpty(rblAlternativas.SelectedValue) &&
                        int.TryParse(rblAlternativas.SelectedValue, out altId))
                    {
                        seleccion.Add(altId);
                    }
                }
                else if (cblAlternativas.Visible)
                {
                    foreach (ListItem it in cblAlternativas.Items)
                    {
                        int altId;
                        if (it.Selected && int.TryParse(it.Value, out altId))
                            seleccion.Add(altId);
                    }
                }

                // si no marcó nada, permitimos continuar pero no insertamos
                if (seleccion.Count == 0)
                    return true;

                // insertar
                foreach (var altId in seleccion)
                {
                    var ir = new IntentoRespuesta
                    {
                        IntentoEvaluacionId = IntentoId,
                        PreguntaId = preguntaId,
                        AlternativaId = altId,
                        EsCorrecta = false,
                        Puntaje = 0
                    };
                    db.IntentosRespuesta.Add(ir);
                }

                db.SaveChanges();
                return true;
            }
        }

        protected void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (TiempoExpirado()) { FinalizarYCalcular(IntentoId); return; }

            if (!GuardarRespuestaActual())
            {
                lblMsg.Text = "No se pudo guardar la respuesta.";
                return;
            }
            if (Index < PregLista.Count - 1)
            {
                Index++;
                BindPreguntaActual();
            }
        }

        protected void btnAnterior_Click(object sender, EventArgs e)
        {
            if (TiempoExpirado()) { FinalizarYCalcular(IntentoId); return; }

            if (Index > 0)
            {
                GuardarRespuestaActual(); // opcional
                Index--;
                BindPreguntaActual();
            }
        }

        protected void btnFinalizar_Click(object sender, EventArgs e)
        {
            FinalizarYCalcular(IntentoId);
        }

        private void FinalizarYCalcular(int intentoId)
        {
            using (var db = new ApplicationDbContext())
            {
                var intento = db.IntentosEvaluacion.Find(intentoId);
                if (intento == null) { lblMsg.Text = "El intento no existe."; return; }

                var eval = db.Evaluaciones.Find(intento.EvaluacionId);
                if (eval == null) { lblMsg.Text = "La evaluación no existe."; return; }

                // Guarda la última respuesta antes de cerrar (si venía por botón)
                if (!TiempoExpirado())
                    GuardarRespuestaActual();

                var preguntasIds = (List<int>)ViewState[VS_Preguntas];

                decimal total = preguntasIds.Count;
                decimal aciertos = 0;

                foreach (var pid in preguntasIds)
                {
                    var correctas = db.Alternativas
                        .Where(a => a.PreguntaId == pid && a.Activa && a.EsCorrecta)
                        .Select(a => a.Id)
                        .OrderBy(x => x)
                        .ToList();

                    var marcadas = db.IntentosRespuesta
                        .Where(r => r.IntentoEvaluacionId == intento.Id && r.PreguntaId == pid && r.AlternativaId.HasValue)
                        .Select(r => r.AlternativaId.Value)
                        .OrderBy(x => x)
                        .ToList();

                    bool ok = correctas.SequenceEqual(marcadas);
                    if (ok) aciertos++;

                    var rs = db.IntentosRespuesta
                        .Where(r => r.IntentoEvaluacionId == intento.Id && r.PreguntaId == pid)
                        .ToList();

                    foreach (var r in rs)
                    {
                        r.EsCorrecta = ok;
                        r.Puntaje = ok ? (100m / total) : 0m;
                    }
                    db.SaveChanges();
                }

                intento.PuntajeObtenido = Math.Round((aciertos / total) * 100m, 2);
                intento.Aprobado = intento.PuntajeObtenido >= eval.PuntajeAprobacion;
                intento.FechaFin = DateTime.UtcNow;
                db.SaveChanges();

                Response.Redirect("~/Usuario/ResultadoEvaluacion.aspx?intentoId=" + intento.Id);
            }
        }
    }
}
