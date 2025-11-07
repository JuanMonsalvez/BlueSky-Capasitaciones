<%@ Page Title="Iniciar Sesión" Language="C#" MasterPageFile="~/MasterPages/Auth.Master"
    AutoEventWireup="true" CodeBehind="IniciarSesion.aspx.cs" Inherits="bluesky.Auth.IniciarSesion" %>

<asp:Content ID="HeadAuthLogin" ContentPlaceHolderID="HeadContent" runat="server">
    <!-- Opcional: estilos/metas específicos -->
</asp:Content>

<asp:Content ID="MainAuthLogin" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card p-4">
            <h2 class="text-center mb-4">Iniciar sesión</h2>

            <div class="mb-3">
                <label class="form-label">Correo electrónico</label>
                <input type="email" class="form-control" placeholder="tu@correo.com" required />
            </div>

            <div class="mb-3">
                <label class="form-label">Contraseña</label>
                <input type="password" class="form-control" placeholder="********" required />
            </div>

            <a href="<%: ResolveUrl("~/Usuario/Cursos.aspx") %>" class="btn btn-primary w-100">Entrar</a>

            <div class="text-center mt-3">
                <a href="<%: ResolveUrl("~/Auth/OlvidasteContrasena.aspx") %>">¿Olvidaste tu contraseña?</a><br />
                <a href="<%: ResolveUrl("~/Auth/CrearSesion.aspx") %>">Crear cuenta</a>
            </div>
        </div>
    </div>
</asp:Content>
