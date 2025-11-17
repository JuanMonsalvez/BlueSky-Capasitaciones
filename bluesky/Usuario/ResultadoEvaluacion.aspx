<%@ Page Title="Resultado de la evaluación"
    Language="C#"
    MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true"
    CodeBehind="ResultadoEvaluacion.aspx.cs"
    Inherits="bluesky.Usuario.ResultadoEvaluacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container" style="max-width: 720px; margin-top: 30px;">
        <asp:Label ID="lblMensaje" runat="server"
                   CssClass="alert alert-warning"
                   Visible="false"></asp:Label>

        <asp:Panel ID="pnlResultado" runat="server" Visible="false" CssClass="panel panel-default">
            <div class="panel-heading">
                <h3 class="panel-title">
                    Resultado de la evaluación
                </h3>
            </div>
            <div class="panel-body">
                <h4>
                    <asp:Label ID="lblCurso" runat="server" />
                </h4>
                <p>
                    <strong>Evaluación:</strong>
                    <asp:Label ID="lblEvaluacion" runat="server" />
                    <br />
                    <strong>Intento:</strong>
                    <asp:Label ID="lblIntento" runat="server" />
                </p>

                <hr />

                <p>
                    <strong>Total de preguntas:</strong>
                    <asp:Label ID="lblTotalPreguntas" runat="server" />
                    <br />
                    <strong>Correctas:</strong>
                    <asp:Label ID="lblCorrectas" runat="server" />
                    <br />
                    <strong>Incorrectas:</strong>
                    <asp:Label ID="lblIncorrectas" runat="server" />
                    <br />
                    <strong>Porcentaje:</strong>
                    <asp:Label ID="lblPorcentaje" runat="server" />
                </p>

                <p>
                    <strong>Resultado:</strong>
                    <asp:Label ID="lblResultado" runat="server" />
                </p>

                <p>
                    <small>
                        Fecha de término:
                        <asp:Label ID="lblFechaTermino" runat="server" />
                    </small>
                </p>

                <hr />

                <a runat="server" href="~/Usuario/Cursos.aspx" class="btn btn-default">
                    Volver a mis cursos
                </a>
                <a runat="server" href="~/Usuario/CursoDetalle.aspx" id="lnkVolverCurso"
                   class="btn btn-primary">
                    Volver al curso
                </a>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
