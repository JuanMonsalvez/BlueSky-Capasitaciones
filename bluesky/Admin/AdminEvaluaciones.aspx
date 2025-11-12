<%@ Page Title="Evaluaciones" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminEvaluaciones.aspx.cs" Inherits="bluesky.Admin.AdminEvaluaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container">
    <h2>Evaluaciones</h2>

    <div class="row" style="margin-bottom:12px;">
      <div class="col-md-3">
        <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-control" />
      </div>
      <div class="col-md-3">
        <asp:TextBox ID="txtBuscar" runat="server" CssClass="form-control" placeholder="Título contiene..." />
      </div>
      <div class="col-md-3">
        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-primary" OnClick="btnBuscar_Click" />
        <asp:Button ID="btnNuevo" runat="server" Text="Nueva evaluación" CssClass="btn btn-success" OnClick="btnNuevo_Click" />
      </div>
    </div>

    <asp:GridView ID="gvEval" runat="server" CssClass="table table-striped"
        AutoGenerateColumns="False" DataKeyNames="Id" OnRowCommand="gvEval_RowCommand">
      <Columns>
        <asp:BoundField DataField="Id" HeaderText="Id" />
        <asp:BoundField DataField="CursoTitulo" HeaderText="Curso" />
        <asp:BoundField DataField="Titulo" HeaderText="Título" />
        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
        <asp:BoundField DataField="NumeroPreguntas" HeaderText="#Preg" />
        <asp:BoundField DataField="TiempoMinutos" HeaderText="Min" />
        <asp:BoundField DataField="PuntajeAprobacion" HeaderText="% Aprob." DataFormatString="{0:0.#}" />
        <asp:CheckBoxField DataField="Activa" HeaderText="Activa" />
        <asp:TemplateField HeaderText="Acciones">
          <ItemTemplate>
            <asp:LinkButton runat="server" CommandName="editEval" CommandArgument='<%# Eval("Id") %>'
                CssClass="btn btn-sm btn-primary" Text="Editar" />
            <asp:LinkButton runat="server" CommandName="delEval" CommandArgument='<%# Eval("Id") %>'
                CssClass="btn btn-sm btn-danger" Text="Eliminar"
                OnClientClick="return confirm('¿Eliminar esta evaluación y su contenido relacionado?');" />
          </ItemTemplate>
        </asp:TemplateField>
      </Columns>
    </asp:GridView>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />
  </div>
</asp:Content>
