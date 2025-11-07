<%@ Page Title="Panel de Administración" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="bluesky.Admin.AdminDashboard" %>

<asp:Content ID="HeadAdminDashboard" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainAdminDashboard" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2 class="fw-bold mb-4">Panel de Administración</h2>

    <div class="row g-4">
        <div class="col-md-3">
            <div class="card p-3 text-center shadow-sm">
                <h5 class="text-primary">Usuarios</h5>
                <p class="display-6 fw-bold">120</p>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card p-3 text-center shadow-sm">
                <h5 class="text-success">Cursos</h5>
                <p class="display-6 fw-bold">35</p>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card p-3 text-center shadow-sm">
                <h5 class="text-warning">Evaluaciones</h5>
                <p class="display-6 fw-bold">95</p>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card p-3 text-center shadow-sm">
                <h5 class="text-info">Certificados</h5>
                <p class="display-6 fw-bold">72</p>
            </div>
        </div>
    </div>

    <div class="mt-5 text-center">
        <a href="<%: ResolveUrl("~/Admin/AdminCursos.aspx") %>" class="btn btn-primary mx-2">Gestionar Cursos</a>
        <a href="<%: ResolveUrl("~/Admin/AdminUsuarios.aspx") %>" class="btn btn-outline-primary mx-2">Usuarios</a>
        <a href="<%: ResolveUrl("~/Admin/AdminReportes.aspx") %>" class="btn btn-outline-secondary mx-2">Reportes</a>
    </div>
</section>
</asp:Content>
