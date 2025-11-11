<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminCursos.aspx.cs"
    Inherits="bluesky.Admin.AdminCursos" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="page-header">Administrar Cursos</h2>

    <div class="row" style="margin-bottom:12px;">
        <div class="col-sm-6">
            <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control" placeholder="Buscar por título..." />
        </div>
        <div class="col-sm-3">
            <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="btn btn-default" OnClick="btnBuscar_Click" />
            <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" CssClass="btn btn-link" OnClick="btnLimpiar_Click" />
        </div>
        <div class="col-sm-3 text-right">
            <a class="btn btn-primary" href="<%: ResolveUrl("~/Admin/CursoEditar.aspx") %>">
                + Nuevo Curso
            </a>
        </div>
    </div>

    <asp:GridView ID="gvCursos" runat="server" CssClass="table table-bordered table-striped"
        AutoGenerateColumns="False" AllowPaging="true" PageSize="10"
        OnPageIndexChanging="gvCursos_PageIndexChanging" DataKeyNames="Id">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" ItemStyle-Width="60" />
            <asp:BoundField DataField="Titulo" HeaderText="Título" />
            <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ItemStyle-HorizontalAlign="Center" />
            <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="180" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <a class="btn btn-xs btn-default" href='<%# ResolveUrl("~/Admin/CursoEditar.aspx?id=" + Eval("Id")) %>'>Editar</a>
                    <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-xs btn-danger"
                        CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                        OnCommand="btnEliminar_Command"
                        OnClientClick="return confirm('¿Eliminar este curso? Esta acción no se puede deshacer.');">
                        Eliminar
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <PagerStyle CssClass="pagination-plain" />
        <EmptyDataTemplate>
            <div class="alert alert-info">No hay cursos que coincidan con el filtro.</div>
        </EmptyDataTemplate>
    </asp:GridView>

    <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />
</asp:Content>
