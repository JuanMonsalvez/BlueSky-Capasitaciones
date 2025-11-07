<%@ Page Title="Resultado de Evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ResultadoEvaluacion.aspx.cs" Inherits="bluesky.Usuario.ResultadoEvaluacion" %>

<asp:Content ID="MainResEval" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4 text-center">
    <div class="card p-4 mx-auto" style="max-width:600px;">
        <h2 class="text-success fw-bold">¡Evaluación completada!</h2>
        <p class="fs-5">Puntaje: <strong>90 %</strong></p>
        <a href="<%: ResolveUrl("~/Usuario/Cursos.aspx") %>" class="btn btn-primary mt-2">Volver a mis cursos</a>
    </div>
</section>
</asp:Content>
