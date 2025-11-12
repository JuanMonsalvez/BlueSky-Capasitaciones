<%@ Page Title="Evaluaciones" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminEvaluaciones.aspx.cs" Inherits="bluesky.Admin.AdminEvaluaciones" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Administrar Evaluaciones</h2>
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />
        <hr />

        <asp:Button ID="btnNuevaEval" runat="server" Text="Nueva Evaluación" CssClass="btn btn-primary"
            OnClick="btnNuevaEval_Click" />
        <a href="GenerarPreguntasIA.aspx" class="btn btn-success">Generar Preguntas con IA</a>

        <hr />
        <asp:GridView ID="gvEvaluaciones" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
            DataKeyNames="Id" OnRowCommand="gvEvaluaciones_RowCommand">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" />
                <asp:BoundField DataField="CursoTitulo" HeaderText="Curso" />
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                <asp:BoundField DataField="NumeroPreguntas" HeaderText="Preguntas" />
                <asp:BoundField DataField="TiempoMinutos" HeaderText="Tiempo (min)" />
                <asp:BoundField DataField="PuntajeAprobacion" HeaderText="Aprobación (%)" />
                <asp:TemplateField HeaderText="Acciones">
                <ItemTemplate>
                    <asp:LinkButton runat="server" CssClass="btn btn-sm btn-info"
                        CommandName="Editar" CommandArgument='<%# Eval("Id") %>'>Editar</asp:LinkButton>
                    <asp:LinkButton runat="server" CssClass="btn btn-sm btn-danger"
                        CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                        OnClientClick="return confirm('¿Eliminar esta evaluación?');">Eliminar</asp:LinkButton>
                    <a href='GenerarPreguntasIA.aspx?evaluacionId=<%# Eval("Id") %>' 
                       class="btn btn-sm btn-success">Generar Preguntas IA</a>
                 </ItemTemplate>
            </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
