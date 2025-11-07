<%@ Page Title="Seleccionar Evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="SeleccionarEvaluacion.aspx.cs" Inherits="bluesky.Usuario.SeleccionarEvaluacion" %>

<asp:Content ID="MainSelEval" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2 class="fw-bold">Seleccionar Evaluación</h2>
    <ul class="list-group mt-3">
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Evaluación 1 – Básico
            <a href="<%: ResolveUrl("~/Usuario/Evaluacion.aspx?idEval=1") %>" class="btn btn-sm btn-primary">Iniciar</a>
        </li>
        <li class="list-group-item d-flex justify-content-between align-items-center">
            Evaluación 2 – Avanzado
            <a href="<%: ResolveUrl("~/Usuario/Evaluacion.aspx?idEval=2") %>" class="btn btn-sm btn-primary">Iniciar</a>
        </li>
    </ul>
</section>
</asp:Content>
