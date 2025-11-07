<%@ Page Title="Recuperar Contraseña" Language="C#" MasterPageFile="~/MasterPages/Auth.Master"
    AutoEventWireup="true" CodeBehind="OlvidasteContrasena.aspx.cs" Inherits="bluesky.Auth.OlvidasteContrasena" %>

<asp:Content ID="HeadAuthForgot" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainAuthForgot" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card p-4">
            <h2 class="text-center mb-2">Recuperar contraseña</h2>
            <p class="text-muted text-center mb-4">Ingresa tu correo y te enviaremos instrucciones</p>

            <div class="mb-3">
                <label class="form-label">Correo electrónico</label>
                <input type="email" class="form-control" placeholder="tu@correo.com" required />
            </div>

            <a href="<%: ResolveUrl("~/Auth/IniciarSesion.aspx") %>" class="btn btn-primary w-100">Enviar enlace</a>
        </div>
    </div>
</asp:Content>
