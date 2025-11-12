<%@ Page Language="C#" Async="true"
    MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true"
    CodeBehind="GenerarPreguntasIA.aspx.cs"
    Inherits="bluesky.Admin.GenerarPreguntasIA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="max-width: 920px; margin: 20px auto;">
        <h2>Generar preguntas con IA</h2>

        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger" />

        <asp:HiddenField ID="hfCursoId" runat="server" />
        <asp:HiddenField ID="hfEvaluacionId" runat="server" />

        <div class="panel panel-default" style="padding:16px;">
            <p><strong>Curso:</strong> <asp:Label ID="lblCurso" runat="server" /></p>
            <p><strong>Evaluación:</strong> <asp:Label ID="lblEvaluacion" runat="server" /></p>

            <div class="row" style="margin-top:12px;">
                <div class="col-md-4">
                    <label for="ddlCantidad">Cantidad de preguntas</label>
                    <asp:DropDownList ID="ddlCantidad" runat="server" CssClass="form-control">
                        <asp:ListItem Value="5" Text="5" />
                        <asp:ListItem Value="10" Text="10" Selected="True" />
                        <asp:ListItem Value="15" Text="15" />
                        <asp:ListItem Value="20" Text="20" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label for="ddlDificultad">Dificultad</label>
                    <asp:DropDownList ID="ddlDificultad" runat="server" CssClass="form-control">
                        <asp:ListItem Value="basica" Text="Básica" />
                        <asp:ListItem Value="media" Text="Media" Selected="True" />
                        <asp:ListItem Value="avanzada" Text="Avanzada" />
                    </asp:DropDownList>
                </div>
            </div>

            <div style="margin-top:16px;">
                <asp:Button ID="btnGenerar" runat="server" CssClass="btn btn-primary"
                    Text="Generar con IA"
                    OnClick="btnGenerar_Click" />
                <asp:Label ID="lblEstado" runat="server" CssClass="text-info" Style="margin-left:12px;"></asp:Label>
            </div>
        </div>

        <asp:Panel ID="pnlPreview" runat="server" Visible="false" CssClass="panel panel-default" Style="padding:16px;">
            <h4>Vista previa (primeras preguntas)</h4>
            <pre style="white-space:pre-wrap;"><asp:Literal ID="litPreview" runat="server" /></pre>
        </asp:Panel>
    </div>
</asp:Content>
