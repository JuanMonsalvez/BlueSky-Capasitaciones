<%@ Page Title="Gestión de Usuarios" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="bluesky.Admin.AdminUsuarios" %>

<asp:Content ID="HeadAdminUsuarios" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainAdminUsuarios" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="fw-bold">Gestión de Usuarios</h2>
        <div>
            <a class="btn btn-primary">Nuevo Usuario</a>
            <a href="<%: ResolveUrl("~/Admin/AdminDashboard.aspx") %>" class="btn btn-outline-secondary ms-2">Volver</a>
        </div>
    </div>

    <table class="table table-striped align-middle">
        <thead class="table-light">
            <tr>
                <th>Nombre</th>
                <th>Correo</th>
                <th>Rol</th>
                <th class="text-end">Acciones</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td>Juan Monsalvez</td>
                <td>juan@empresa.com</td>
                <td>Admin</td>
                <td class="text-end">
                    <a class="btn btn-sm btn-outline-primary">Editar</a>
                    <a class="btn btn-sm btn-outline-danger ms-1">Eliminar</a>
                </td>
            </tr>
            <tr>
                <td>Lorenzo Soto</td>
                <td>lorenzo@empresa.com</td>
                <td>Usuario</td>
                <td class="text-end">
                    <a class="btn btn-sm btn-outline-primary">Editar</a>
                    <a class="btn btn-sm btn-outline-danger ms-1">Eliminar</a>
                </td>
            </tr>
        </tbody>
    </table>
</section>
</asp:Content>
