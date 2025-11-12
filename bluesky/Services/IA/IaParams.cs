namespace bluesky.Services.IA
{
    public class IaParams
    {
        public int CursoId { get; set; }
        public int TipoEval { get; set; }          // 1 diag, 2 form, 3 sum
        public int NPreguntas { get; set; }
        public int DificultadObjetivo { get; set; } // 0=mixta, 1..3
        public int TiempoMin { get; set; }
        public int PuntajeAprob { get; set; }
        public string Cobertura { get; set; }       // "uniforme" | "libre"
        public int PctConceptual { get; set; }      // debe sumar 100 con PctAplicada
        public int PctAplicada { get; set; }
    }
}
