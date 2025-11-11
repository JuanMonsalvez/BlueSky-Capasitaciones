<%@ Page Title="Cursos (Admin)" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminCursos.aspx.cs" Inherits="bluesky.Admin.AdminCursos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="page-header">Administración de Cursos</h2>

        <div class="text-right" style="margin-bottom:12px;">
            <a class="btn btn-primary" href="<%: ResolveUrl("~/Admin/CursoEditar.aspx") %>">+ Nuevo curso</a>
        </div>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />

        <asp:GridView ID="gvCursos" runat="server" CssClass="table table-striped table-bordered"
            AutoGenerateColumns="false" AllowPaging="true" PageSize="10"
            OnPageIndexChanging="gvCursos_PageIndexChanging"
            OnRowCommand="gvCursos_RowCommand" OnRowDataBound="gvCursos_RowDataBound"
            DataKeyNames="Id">

            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ItemStyle-Width="60px" />
                <asp:TemplateField HeaderText="Portada" ItemStyle-Width="120px">
                    <ItemTemplate>
                        <asp:Image ID="imgPortada" runat="server" Width="110" Height="70" Style="object-fit:cover;border-radius:6px;" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Titulo" HeaderText="Título" />
                <asp:BoundField DataField="Nivel" HeaderText="Nivel" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                <asp:BoundField DataField="FechaCreacion" HeaderText="Creado" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="180px">
                    <ItemTemplate>
                        <a class="btn btn-sm btn-default" href='<%# ResolveUrl("~/Admin/CursoEditar.aspx?id=" + Eval("Id")) %>'>Editar</a>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-sm btn-danger"
                            CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                            OnClientClick="return confirm('¿Eliminar este curso?');">Eliminar</asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
