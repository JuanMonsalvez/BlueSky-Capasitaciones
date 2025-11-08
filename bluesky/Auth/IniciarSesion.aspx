<%@ Page Title="Iniciar sesión" Language="C#" MasterPageFile="~/MasterPages/Auth.Master"
    AutoEventWireup="true" CodeBehind="IniciarSesion.aspx.cs" Inherits="bluesky.IniciarSesion" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card panel panel-default" style="max-width:480px;margin:40px auto;padding:24px;">
            <h2 class="text-center" style="margin-bottom:16px;">Iniciar sesión</h2>

            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger" />

            <div class="form-group">
                <label for="txtCorreo">Correo electrónico</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqCorreo" runat="server"
                    ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtPassword">Contraseña</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqPass" runat="server"
                    ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <asp:Button ID="btnLogin" runat="server" Text="Entrar" CssClass="btn btn-primary btn-block"
                OnClick="btnLogin_Click" />

            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" EnableViewState="false"
                Style="display:block;margin-top:12px;"></asp:Label>

            <div style="margin-top:10px;">
                <a href="~/Auth/OlvidasteContrasena.aspx" runat="server">¿Olvidaste tu contraseña?</a>
            </div>
        </div>
    </div>
</asp:Content>
