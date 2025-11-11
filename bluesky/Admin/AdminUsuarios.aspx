<%@ Page Title="Administrar usuarios" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminUsuarios.aspx.cs" Inherits="bluesky.Admin.AdminUsuarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="max-width:1100px">

        <h2 class="mb-3">Administrar usuarios</h2>

        <!-- Alertas -->
        <asp:Label ID="lblMsg" runat="server" EnableViewState="false" />

        <!-- Crear usuario -->
        <div class="panel panel-default" style="margin-bottom:20px;">
            <div class="panel-heading"><strong>Crear nuevo usuario</strong></div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-sm-4">
                        <label>Nombre completo</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="reqNombre" runat="server"
                            ControlToValidate="txtNombre" ErrorMessage="Nombre requerido"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-sm-4">
                        <label>Correo</label>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="reqCorreo" runat="server"
                            ControlToValidate="txtCorreo" ErrorMessage="Correo requerido"
                            CssClass="text-danger" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="valCorreo" runat="server"
                            ControlToValidate="txtCorreo"
                            ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$"
                            ErrorMessage="Correo inválido" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-sm-4">
                        <label>Rol</label>
                        <asp:DropDownList ID="ddlRolNuevo" runat="server" CssClass="form-control" />
                    </div>
                </div>

                <div class="row" style="margin-top:12px;">
                    <div class="col-sm-4">
                        <label>Contraseña</label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="reqPass" runat="server"
                            ControlToValidate="txtPassword" ErrorMessage="Contraseña requerida"
                            CssClass="text-danger" Display="Dynamic" />
                        <!-- Política: 8+ con mayúscula, minúscula y número -->
                        <asp:RegularExpressionValidator ID="valPass" runat="server"
                            ControlToValidate="txtPassword"
                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"
                            ErrorMessage="Mínimo 8 caracteres con mayúscula, minúscula y número"
                            CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-sm-4">
                        <label>&nbsp;</label>
                        <asp:Button ID="btnCrear" runat="server" Text="Crear usuario"
                            CssClass="btn btn-primary btn-block" OnClick="btnCrear_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Listado / edición rápida -->
        <asp:GridView ID="gvUsuarios" runat="server" AutoGenerateColumns="False" CssClass="table table-striped"
            DataKeyNames="Id" OnRowCommand="gvUsuarios_RowCommand" OnRowDataBound="gvUsuarios_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="true" ItemStyle-Width="60" />
                <asp:BoundField DataField="NombreCompleto" HeaderText="Nombre" />
                <asp:BoundField DataField="Correo" HeaderText="Correo" />
                <asp:TemplateField HeaderText="Rol" ItemStyle-Width="160">
                    <ItemTemplate>
                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Activo" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="90">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkActivo" runat="server" Checked='<%# Eval("Activo") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="FechaRegistro" HeaderText="Registrado"
                    DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="120" />
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="240">
                    <ItemTemplate>
                        <asp:LinkButton ID="btnGuardar" runat="server" CommandName="Guardar" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-sm btn-success" Text="Guardar" />
                        &nbsp;
                        <asp:LinkButton ID="btnReset" runat="server" CommandName="ResetPass" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-sm btn-warning" Text="Reset pass" OnClientClick="return confirm('¿Resetear contraseña?');" />
                        &nbsp;
                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="Eliminar" CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-sm btn-danger" Text="Eliminar" OnClientClick="return confirm('¿Eliminar usuario?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</asp:Content>
