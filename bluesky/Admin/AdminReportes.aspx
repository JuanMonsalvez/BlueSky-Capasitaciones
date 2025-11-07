<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminReportes.aspx.cs" Inherits="bluesky.Admin.AdminReportes" %>

<asp:Content ID="HeadAdminReportes" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainAdminReportes" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="fw-bold">Reportes</h2>
        <a href="<%: ResolveUrl("~/Admin/AdminDashboard.aspx") %>" class="btn btn-outline-secondary">Volver</a>
    </div>

    <p class="text-muted">Exporta métricas de cursos, evaluaciones y certificaciones.</p>

    <div class="d-flex gap-2 mb-4">
        <a class="btn btn-outline-secondary">Exportar Excel</a>
        <a class="btn btn-outline-secondary">Exportar PDF</a>
    </div>

    <div class="card p-3 shadow-sm">
        <h5 class="fw-bold mb-3">KPIs (mock)</h5>
        <ul class="mb-0">
            <li>Tasa de aprobación últimos 30 días: 82 %</li>
            <li>Nuevos certificados emitidos: 27</li>
            <li>Promedio de intentos por evaluación: 1.4</li>
        </ul>
    </div>
</section>
</asp:Content>
