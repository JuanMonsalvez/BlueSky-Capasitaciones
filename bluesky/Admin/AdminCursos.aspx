<%@ Page Title="Gestión de Cursos" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminCursos.aspx.cs" Inherits="bluesky.Admin.AdminCursos" %>

<asp:Content ID="HeadAdminCursos" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainAdminCursos" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="fw-bold">Gestión de Cursos</h2>
        <div>
            <a class="btn btn-primary">Nuevo Curso</a>
            <a href="<%: ResolveUrl("~/Admin/AdminDashboard.aspx") %>" class="btn btn-outline-secondary ms-2">Volver</a>
        </div>
    </div>

    <table class="table table-hover align-middle">
        <thead class="table-light">
            <tr>
                <th>Nombre</th>
                <th>Duración</th>
                <th>Área</th>
                <th class="text-end">Acciones</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Atención al Cliente</td>
                <td>10 h</td>
                <td>Comunicaciones</td>
                <td class="text-end">
                    <a class="btn btn-sm btn-outline-primary">Editar</a>
                    <a class="btn btn-sm btn-outline-danger ms-1">Eliminar</a>
                </td>
            </tr>
            <tr>
                <td>Seguridad Financiera</td>
                <td>8 h</td>
                <td>Finanzas</td>
                <td class="text-end">
                    <a class="btn btn-sm btn-outline-primary">Editar</a>
                    <a class="btn btn-sm btn-outline-danger ms-1">Eliminar</a>
                </td>
            </tr>
        </tbody>
    </table>
</section>
</asp:Content>
