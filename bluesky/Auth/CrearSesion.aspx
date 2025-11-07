<%@ Page Title="Crear Cuenta" Language="C#" MasterPageFile="~/MasterPages/Auth.Master"
    AutoEventWireup="true" CodeBehind="CrearSesion.aspx.cs" Inherits="bluesky.Auth.CrearSesion" %>

<asp:Content ID="HeadAuthRegister" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainAuthRegister" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card p-4">
            <h2 class="text-center mb-4">Crear cuenta</h2>

            <div class="mb-3">
                <label class="form-label">Nombre completo</label>
                <input type="text" class="form-control" placeholder="Nombre Apellido" required />
            </div>

            <div class="mb-3">
                <label class="form-label">Correo electrónico</label>
                <input type="email" class="form-control" placeholder="tu@correo.com" required />
            </div>

            <div class="mb-3">
                <label class="form-label">Contraseña</label>
                <input type="password" class="form-control" placeholder="********" required />
            </div>

            <a href="<%: ResolveUrl("~/Auth/IniciarSesion.aspx") %>" class="btn btn-primary w-100">Registrarse</a>
        </div>
    </div>
</asp:Content>
