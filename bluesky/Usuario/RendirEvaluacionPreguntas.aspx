<%@ Page Title="Rendir evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="RendirEvaluacionPreguntas.aspx.cs"
    Inherits="bluesky.Usuario.RendirEvaluacionPreguntas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="sm1" runat="server" />

    <div class="container" style="max-width:900px;">
        <h2 class="mb-3">
            <asp:Label ID="lblTituloEval" runat="server" Text="Evaluación"></asp:Label>
        </h2>

        <div class="row mb-2">
            <div class="col-sm-6">
                <strong>Progreso:</strong>
                <asp:Label ID="lblProgreso" runat="server" />
            </div>
            <div class="col-sm-6 text-right">
                <strong>Tiempo:</strong>
                <asp:Label ID="lblTiempo" runat="server" />
            </div>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" EnableViewState="false" /><br />

        <!-- Guarda el fin en epoch ms para el JS -->
        <asp:HiddenField ID="hfEndEpochMs" runat="server" />

        <asp:Panel ID="pnlPregunta" runat="server">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <asp:Literal ID="litEnunciado" runat="server" />
                </div>
                <div class="panel-body">
                    <!-- Alternativas (exclusivas) -->
                    <asp:RadioButtonList ID="rblAlternativas" runat="server" CssClass="list-unstyled" />

                    <!-- Alternativas (múltiples) -->
                    <asp:CheckBoxList ID="cblAlternativas" runat="server" CssClass="list-unstyled" />

                    <asp:HiddenField ID="hfPreguntaId" runat="server" />
                </div>
            </div>

            <div class="d-flex" style="gap:8px;">
                <asp:Button ID="btnAnterior" runat="server" CssClass="btn btn-default" Text="Anterior" OnClick="btnAnterior_Click" />
                <asp:Button ID="btnSiguiente" runat="server" CssClass="btn btn-primary" Text="Siguiente" OnClick="btnSiguiente_Click" />
                <asp:Button ID="btnFinalizar" runat="server" CssClass="btn btn-success" Text="Finalizar" OnClick="btnFinalizar_Click" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
