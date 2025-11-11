<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master" AutoEventWireup="true"
    CodeBehind="CursoEditar.aspx.cs" Inherits="bluesky.Admin.CursoEditar" Title="Editar curso" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="page-header"><asp:Literal ID="litTitulo" runat="server" /></h2>

    <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger" />

    <div class="row">
        <div class="col-md-8">
            <div class="form-group">
                <label for="txtTitulo">Título</label>
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="reqTitulo" runat="server" ControlToValidate="txtTitulo"
                    ErrorMessage="El título es obligatorio" CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="form-group">
                <label for="txtDescripcion">Descripción</label>
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" />
            </div>

            <div class="form-group">
                <label for="ddlArea">Área</label>
                <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="(sin área)" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="ddlInstructor">Instructor</label>
                <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="(sin instructor)" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <div class="row">
                    <div class="col-sm-4">
                        <label for="txtDuracion">Duración (horas)</label>
                        <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" />
                        <asp:RegularExpressionValidator ID="valDuracion" runat="server"
                            ControlToValidate="txtDuracion" ValidationExpression="^\d{1,4}$"
                            ErrorMessage="Ingrese un número válido" CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="col-sm-4">
                        <label for="ddlNivel">Nivel</label>
                        <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Básico" Value="0" />
                            <asp:ListItem Text="Intermedio" Value="1" />
                            <asp:ListItem Text="Avanzado" Value="2" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-sm-4">
                        <label>&nbsp;</label>
                        <div>
                            <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label for="fuPortada">Portada (jpg/png/webp)</label>
                <asp:FileUpload ID="fuPortada" runat="server" CssClass="form-control" />
                <small class="text-muted">Se guardará la ruta en la base de datos. Límite 5 MB.</small>
                <div style="margin-top:10px">
                    <asp:Image ID="imgPreview" runat="server" Width="260" Visible="false" />
                </div>
            </div>

            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary"
                OnClick="btnGuardar_Click" />
            <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/Admin/AdminCursos.aspx"
                CssClass="btn btn-default" Text="Volver" />
            <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" Style="margin-left:10px;"></asp:Label>
        </div>
    </div>
</asp:Content>
