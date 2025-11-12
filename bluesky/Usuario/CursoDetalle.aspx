<%@ Page Title="Curso" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CursoDetalle.aspx.cs" Inherits="bluesky.Usuario.CursoDetalle" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />
    <h2><asp:Literal ID="litTitulo" runat="server" /></h2>
    <p><asp:Literal ID="litDescripcion" runat="server" /></p>

    <h4>Materiales del curso</h4>
    <asp:Repeater ID="repMateriales" runat="server">
        <ItemTemplate>
            <div>
                <%# Eval("NombreArchivo") %> —
                <a href="<%# Eval("RutaAlmacenamiento") %>" target="_blank">Ver</a>
                (<%# Eval("Tipo") %>)
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <h4>Evaluaciones disponibles</h4>
    <asp:Repeater ID="repEvaluaciones" runat="server" OnItemCommand="repEvaluaciones_ItemCommand">
    <ItemTemplate>
        <div class="panel panel-default" style="padding:10px; margin-bottom:8px;">
            <h5><%# Eval("Titulo") %></h5>
            <p><strong>Preguntas:</strong> <%# Eval("NumeroPreguntas") %> |
               <strong>Tiempo:</strong> <%# Eval("TiempoMinutos") %> min</p>

            <asp:Button ID="btnRendir" runat="server" Text="Rendir evaluación"
                CommandName="Rendir"
                CommandArgument='<%# Eval("Id") %>'
                CssClass="btn btn-primary btn-sm" />
        </div>
    </ItemTemplate>
</asp:Repeater>

</asp:Content>
