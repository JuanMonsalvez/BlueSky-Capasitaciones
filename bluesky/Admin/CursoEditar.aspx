<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CursoEditar.aspx.cs"
    Inherits="bluesky.Admin.CursoEditar" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="page-header"><asp:Literal ID="litTitulo" runat="server" /></h2>

    <asp:ValidationSummary ID="valsum" runat="server" CssClass="text-danger" />

    <div class="form-horizontal">
        <div class="form-group">
            <label class="col-sm-2 control-label">Título</label>
            <div class="col-sm-6">
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqTitulo" runat="server"
                    ControlToValidate="txtTitulo" ErrorMessage="El título es obligatorio"
                    Display="Dynamic" CssClass="text-danger" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-sm-2 control-label">Descripción</label>
            <div class="col-sm-6">
                <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-sm-2 control-label">Activo</label>
            <div class="col-sm-6">
                <asp:CheckBox ID="chkActivo" runat="server" />
            </div>
        </div>

        <div class="form-group">
            <div class="col-sm-offset-2 col-sm-6">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
                <a class="btn btn-default" href="<%: ResolveUrl("~/Admin/AdminCursos.aspx") %>">Volver</a>
                <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Style="margin-left:12px;" />
            </div>
        </div>
    </div>
</asp:Content>
