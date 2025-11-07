<%@ Page Title="Detalle del Curso" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CursoDetalle.aspx.cs" Inherits="bluesky.Usuario.CursoDetalle" %>

<asp:Content ID="HeadDetalle" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainDetalle" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2>Seguridad Financiera</h2>
    <p class="text-muted">Duración: 8 horas · Nivel: Intermedio</p>

    <h4>Descripción</h4>
    <p>Aprende los principios clave de seguridad financiera corporativa.</p>

    <h4>Objetivos</h4>
    <ul><li>Identificar riesgos</li><li>Aplicar medidas de seguridad</li></ul>

    <div class="mt-3">
        <a href="<%: ResolveUrl("~/Usuario/SeleccionarEvaluacion.aspx?curso=2") %>" class="btn btn-primary">Rendir evaluación</a>
        <a href="<%: ResolveUrl("~/Usuario/Cursos.aspx") %>" class="btn btn-outline-secondary">Volver</a>
    </div>
</section>
</asp:Content>
