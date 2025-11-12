<%@ Page Title="Rendir evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RendirEvaluacion.aspx.cs" Inherits="bluesky.Usuario.RendirEvaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlBody" runat="server" Visible="false">
        <h2><asp:Literal ID="litTitulo" runat="server" /></h2>
        <p class="text-muted">
            <b>Curso:</b> <asp:Literal ID="litCurso" runat="server" /> |
            <b>Tipo:</b> <asp:Literal ID="litTipo" runat="server" /> |
            <b>N° preguntas:</b> <asp:Literal ID="litPreguntas" runat="server" /> |
            <b>Tiempo:</b> <asp:Literal ID="litTiempo" runat="server" /> min |
            <b>Aprobación:</b> <asp:Literal ID="litAprob" runat="server" />%
        </p>

        <asp:HiddenField ID="hdnEvaluacionId" runat="server" />
        <asp:HiddenField ID="hdnIntentoId" runat="server" />

        <asp:Button ID="btnIniciar" runat="server" CssClass="btn btn-primary" Text="Iniciar evaluación"
            OnClick="btnIniciar_Click" />

        <asp:Label ID="lblInfo" runat="server" CssClass="text-info" Visible="false" />
    </asp:Panel>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Visible="false" />
</asp:Content>
