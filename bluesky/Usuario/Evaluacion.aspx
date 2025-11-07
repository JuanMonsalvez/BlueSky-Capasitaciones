<%@ Page Title="Evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="Evaluacion.aspx.cs" Inherits="bluesky.Usuario.Evaluacion" %>

<asp:Content ID="MainEval" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2 class="fw-bold mb-4">Evaluación – Seguridad Financiera</h2>

    <div class="card p-3 mb-3">
        <h5>1. ¿Qué es un riesgo financiero?</h5>
        <label><input type="radio" name="p1" /> Pérdida de capital</label><br />
        <label><input type="radio" name="p1" /> Ganancia inesperada</label>
    </div>

    <div class="text-end">
        <a href="<%: ResolveUrl("~/Usuario/ResultadoEvaluacion.aspx?idEval=1") %>" class="btn btn-success">Finalizar</a>
    </div>
</section>
</asp:Content>
