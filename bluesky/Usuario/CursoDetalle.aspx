<%@ Page Title="Detalle del curso" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CursoDetalle.aspx.cs" Inherits="bluesky.Usuario.CursoDetalle" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:Panel ID="pnlBody" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-5">
                <asp:Image ID="imgPortada" runat="server" CssClass="img-responsive img-thumbnail" />
            </div>
            <div class="col-md-7">
                <h2><asp:Literal ID="litTitulo" runat="server" /></h2>
                <p class="text-muted">
                    <b>Área:</b> <asp:Literal ID="litArea" runat="server" /> |
                    <b>Nivel:</b> <asp:Literal ID="litNivel" runat="server" /> |
                    <b>Duración:</b> <asp:Literal ID="litDuracion" runat="server" /> horas
                </p>
                <p><asp:Literal ID="litDescripcion" runat="server" /></p>

                <div style="margin-top:12px;">
                    <asp:HyperLink ID="lnkEvaluacion" runat="server" CssClass="btn btn-primary" Visible="false">
                        Rendir evaluación
                    </asp:HyperLink>
                    <asp:Label ID="lblSinEval" runat="server" CssClass="text-muted" Visible="false"
                               Text="Este curso aún no tiene evaluaciones activas."></asp:Label>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Visible="false" />

</asp:Content>
