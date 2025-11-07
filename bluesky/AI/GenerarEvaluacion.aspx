<%@ Page Title="Generar Evaluación con IA" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="GenerarEvaluacion.aspx.cs" Inherits="bluesky.AI.GenerarEvaluacion" %>

<asp:Content ID="HeadGenEval" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: ResolveUrl("~/Content/css/app.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainGenEval" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2 class="fw-bold mb-4 text-primary">
        <i class="fa fa-robot me-2"></i>Generar Evaluación con Gemini AI
    </h2>

    <div class="card p-4 shadow-sm mb-4">
        <h5 class="fw-semibold mb-3">Subir documento del curso (PDF)</h5>
        <input type="file" id="pdfUpload" class="form-control mb-3" accept=".pdf" />
        <button type="button" class="btn btn-primary">
            <i class="fa fa-magic me-2"></i>Generar Evaluación
        </button>
    </div>

    <div id="resultadoIA" class="mt-4">
        <h4 class="fw-bold text-secondary mb-3">Vista previa de evaluación (simulada)</h4>
        <div class="card p-3 shadow-sm">
            <p class="text-muted">Se generarán automáticamente 15 preguntas tipo test con alternativas.</p>
            <div class="alert alert-info">Simulación: archivo procesado con éxito</div>
            <a href="<%: ResolveUrl("~/Usuario/SeleccionarEvaluacion.aspx") %>" class="btn btn-outline-primary mt-2">
                Ir a Evaluaciones
            </a>
        </div>
    </div>
</section>
</asp:Content>
