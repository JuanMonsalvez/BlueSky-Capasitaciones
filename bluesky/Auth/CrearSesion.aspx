<%@ Page Title="Crear cuenta" Language="C#" MasterPageFile="~/Masterpages/Auth.Master" AutoEventWireup="true" CodeBehind="CrearSesion.aspx.cs" Inherits="bluesky.Auth.CrearSesion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card card p-4">
            <h2 class="mb-3">Crear cuenta</h2>

            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger mb-3" DisplayMode="BulletList" />

            <div class="mb-3">
                <label for="txtNombre" class="form-label">Nombre completo</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                    ErrorMessage="El nombre es obligatorio" CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
                <label for="txtCorreo" class="form-label">Correo</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" TextMode="Email" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCorreo"
                    ErrorMessage="El correo es obligatorio" CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCorreo"
                    ErrorMessage="Correo inválido"
                    CssClass="text-danger" Display="Dynamic"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" />
            </div>

            <div class="mb-3">
                <label for="txtPassword" class="form-label">Contraseña</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="La contraseña es obligatoria" CssClass="text-danger" Display="Dynamic" />
                <!-- Política: mínimo 8, al menos 1 mayúscula, 1 minúscula y 1 número -->
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtPassword"
                    ErrorMessage="La contraseña debe tener mínimo 8 caracteres, con mayúscula, minúscula y número."
                    CssClass="text-danger" Display="Dynamic"
                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" />
            </div>

            <div class="mb-3">
                <label for="txtPassword2" class="form-label">Repetir contraseña</label>
                <asp:TextBox ID="txtPassword2" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPassword2"
                    ErrorMessage="Repite la contraseña" CssClass="text-danger" Display="Dynamic" />
                <asp:CompareValidator runat="server" ControlToValidate="txtPassword2" ControlToCompare="txtPassword"
                    ErrorMessage="Las contraseñas no coinciden" CssClass="text-danger" Display="Dynamic" />
            </div>

            <asp:Button ID="btnCrear" runat="server" CssClass="btn btn-primary w-100" Text="Crear cuenta" OnClick="btnCrear_Click" />

            <div class="mt-3">
                <a href="<%: ResolveUrl("~/Auth/IniciarSesion") %>">¿Ya tienes cuenta? Inicia sesión</a>
            </div>
        </div>
    </div>
</asp:Content>
