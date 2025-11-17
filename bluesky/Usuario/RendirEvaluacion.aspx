<%@ Page Title="Rendir evaluación"
    Language="C#"
    MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true"
    CodeBehind="RendirEvaluacion.aspx.cs"
    Inherits="bluesky.Usuario.RendirEvaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hfEvaluacionId" runat="server" />

    <div class="container" style="max-width:800px;margin-top:20px;">
        <div class="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">
                    <asp:Literal ID="litTituloCurso" runat="server" /> –
                    <asp:Literal ID="litTituloEvaluacion" runat="server" />
                </h3>
            </div>
            <div class="panel-body">
                <p>
                    <strong>Tiempo:</strong>
                    <asp:Literal ID="litTiempo" runat="server" /> minutos<br />
                    <strong>Preguntas:</strong>
                    <asp:Literal ID="litPreguntas" runat="server" />
                </p>

                <hr />

                <p>
                    <strong>Política de intentos:</strong>
                    <asp:Literal ID="litPolitica" runat="server" />
                </p>

                <p>
                    <asp:Label ID="lblIntentosInfo" runat="server" CssClass="text-info" />
                    <br />
                    <asp:Label ID="lblIntentoActualInfo" runat="server" CssClass="text-info" />
                    <br />
                    <asp:Label ID="lblCooldownInfo" runat="server" CssClass="text-warning" />
                </p>

                <asp:Label ID="lblError" runat="server" CssClass="text-danger" />

                <div style="margin-top:15px;">
                    <asp:Button ID="btnComenzar" runat="server"
                        CssClass="btn btn-primary"
                        Text="Comenzar evaluación"
                        OnClick="btnComenzar_Click" />
                    <a id="lnkVolverCurso" runat="server" class="btn btn-link">Volver al curso</a>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
