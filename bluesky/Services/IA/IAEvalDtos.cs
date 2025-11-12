using System.Collections.Generic;

namespace bluesky.Services.IA
{
    public class EvalRequest
    {
        public int NPreguntas { get; set; }
        public int DificultadObjetivo { get; set; } // 0=mixta, 1-3
        public string Cobertura { get; set; }       // "uniforme" | "libre"
        public int PctConceptual { get; set; }
        public int PctAplicada { get; set; }
        public int TipoEvaluacion { get; set; }     // 1 diag, 2 form, 3 sum
        public int TiempoMin { get; set; }
        public int PuntajeAprob { get; set; }
        public string Contenido { get; set; }
    }

    public class EvalResult
    {
        public EvalMeta Evaluacion { get; set; }
        public List<EvalPregunta> Preguntas { get; set; } = new List<EvalPregunta>();
    }

    public class EvalMeta
    {
        public string Titulo { get; set; }
        public int numero_preguntas { get; set; }
        public int tiempo_minutos { get; set; }
        public int puntaje_aprobacion { get; set; }
        public string tipo { get; set; } // texto
    }

    public class EvalPregunta
    {
        public string Enunciado { get; set; }
        public string Categoria { get; set; }
        public int Dificultad { get; set; }
        public bool MultipleRespuesta { get; set; }
        public List<EvalAlt> Alternativas { get; set; } = new List<EvalAlt>();
    }

    public class EvalAlt
    {
        public string Texto { get; set; }
        public bool EsCorrecta { get; set; }
    }
}
