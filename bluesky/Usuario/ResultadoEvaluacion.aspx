<%@ Page Title="Resultado de la evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ResultadoEvaluacion.aspx.cs" Inherits="bluesky.Usuario.ResultadoEvaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlBody" runat="server" Visible="false">
        <h2>Resultado</h2>
        <p><b>Evaluación:</b> <asp:Literal ID="litEval" runat="server" /></p>
        <p><b>Puntaje:</b> <asp:Literal ID="litPuntaje" runat="server" />%</p>
        <p><b>Estado:</b> <asp:Literal ID="litEstado" runat="server" /></p>

        <div style="margin-top:12px;">
            <asp:HyperLink ID="lnkVolverCurso" runat="server" CssClass="btn btn-default">Volver al curso</asp:HyperLink>
        </div>
    </asp:Panel>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Visible="false" />
</asp:Content>
