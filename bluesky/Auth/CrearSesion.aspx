<%@ Page Title="Crear cuenta" Language="C#" MasterPageFile="~/MasterPages/Auth.Master"
    AutoEventWireup="true" CodeBehind="CrearSesion.aspx.cs" Inherits="bluesky.Auth.CrearSesion" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-page">
        <div class="auth-card panel panel-default" style="max-width:520px;margin:40px auto;padding:24px;">
            <h2 class="text-center" style="margin-bottom:16px;">Crear cuenta</h2>

            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger" />

            <div class="form-group">
                <label for="txtNombre">Nombre completo</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqNombre" runat="server"
                    ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtCorreo">Correo</label>
                <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqCorreo" runat="server"
                    ControlToValidate="txtCorreo" ErrorMessage="El correo es obligatorio"
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="valCorreo" runat="server"
                    ControlToValidate="txtCorreo"
                    ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                    ErrorMessage="Formato de correo inválido"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtPass">Contraseña</label>
                <asp:TextBox ID="txtPass" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqPass" runat="server"
                    ControlToValidate="txtPass" ErrorMessage="La contraseña es obligatoria"
                    CssClass="text-danger" Display="Dynamic" />
                <asp:RegularExpressionValidator ID="valPassStrong" runat="server"
                    ControlToValidate="txtPass"
                    ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"
                    ErrorMessage="Mín. 8 caracteres, con mayúscula, minúscula y número"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtPass2">Confirmar contraseña</label>
                <asp:TextBox ID="txtPass2" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:CompareValidator ID="valPassMatch" runat="server"
                    ControlToValidate="txtPass2" ControlToCompare="txtPass"
                    ErrorMessage="Las contraseñas no coinciden"
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <asp:Button ID="btnCrear" runat="server" Text="Crear cuenta" CssClass="btn btn-primary btn-block"
                OnClick="btnCrear_Click" />
            <asp:Label ID="lblMensaje" runat="server" CssClass="text-danger" EnableViewState="false"
                Style="display:block;margin-top:12px;"></asp:Label>
        </div>
    </div>
</asp:Content>
