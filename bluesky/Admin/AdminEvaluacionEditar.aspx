<%@ Page Title="Editar Evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminEvaluacionEditar.aspx.cs" Inherits="bluesky.Admin.EvaluacionEditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container">
    <h2><asp:Literal ID="litTitulo" runat="server" /></h2>
    <asp:ValidationSummary ID="valSum" runat="server" CssClass="text-danger" />

    <div class="row">
      <div class="col-md-6">
        <div class="form-group">
          <label>Curso</label>
          <asp:DropDownList ID="ddlCurso" runat="server" CssClass="form-control" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCurso" InitialValue="" ErrorMessage="Curso requerido" CssClass="text-danger" />
        </div>
        <div class="form-group">
          <label>Título</label>
          <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" />
          <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitulo" ErrorMessage="Título requerido" CssClass="text-danger" />
        </div>
        <div class="form-group">
          <label>Tipo</label>
          <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control" />
        </div>
        <div class="form-group">
          <label>Número de preguntas</label>
          <asp:TextBox ID="txtNumeroPreguntas" runat="server" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="form-group">
          <label>Tiempo (minutos)</label>
          <asp:TextBox ID="txtTiempo" runat="server" CssClass="form-control" TextMode="Number" />
        </div>
        <div class="form-group">
          <label>Puntaje aprobación (%)</label>
          <asp:TextBox ID="txtPuntaje" runat="server" CssClass="form-control" Text="60" />
        </div>
        <div class="form-group">
          <label>Activa</label>
          <asp:CheckBox ID="chkActiva" runat="server" Checked="true" />
        </div>

        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
        <a runat="server" href="~/Admin/AdminEvaluaciones.aspx" class="btn btn-default">Cancelar</a>

        <asp:Button ID="btnEditarPreguntas" runat="server"
            Text="Editar preguntas"
            CssClass="btn btn-info"
            OnClick="btnEditarPreguntas_Click"
            CausesValidation="false"
            Style="margin-left:10px;" />


        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Style="display:block;margin-top:10px;" />
      </div>
    </div>
  </div>
</asp:Content>
