<%@ Page Title="Curso - Editar" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="CursoEditar.aspx.cs" Inherits="bluesky.Admin.CursoEditar" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2><asp:Literal ID="litTitulo" runat="server" /></h2>
        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" />

        <asp:ValidationSummary runat="server" CssClass="text-danger" />

        <div class="row">
            <div class="col-md-7">
                <div class="form-group">
                    <label>Título</label>
                    <asp:TextBox ID="txtTitulo" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTitulo"
                        ErrorMessage="El título es obligatorio" CssClass="text-danger" />
                </div>

                <div class="form-group">
                    <label>Descripción</label>
                    <asp:TextBox ID="txtDescripcion" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label>Área</label>
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label>Instructor</label>
                    <asp:DropDownList ID="ddlInstructor" runat="server" CssClass="form-control" />
                </div>

                <div class="form-row">
                    <div class="col-md-4">
                        <label>Duración (h)</label>
                        <asp:TextBox ID="txtDuracion" runat="server" CssClass="form-control" />
                    </div>
                    <div class="col-md-4">
                        <label>Nivel</label>
                        <asp:DropDownList ID="ddlNivel" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Básico" Value="1" />
                            <asp:ListItem Text="Intermedio" Value="2" />
                            <asp:ListItem Text="Avanzado" Value="3" />
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label>&nbsp;</label><br />
                        <asp:CheckBox ID="chkActivo" runat="server" Text="Activo" />
                    </div>
                </div>

                <hr />
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar curso" CssClass="btn btn-primary"
                    OnClick="btnGuardar_Click" />
                <a href="~/Admin/AdminCursos.aspx" runat="server" class="btn btn-default">Volver</a>
            </div>

            <div class="col-md-5">
                <h4>Portada</h4>
                <asp:Image ID="imgPreview" runat="server" Visible="false" CssClass="img-responsive" />
                <asp:FileUpload ID="fuPortada" runat="server" />

                <hr />
                <h4>Materiales del curso (PDF/PPT)</h4>
                <asp:FileUpload ID="fuMaterial" runat="server" />
                <asp:DropDownList ID="ddlTipoMaterial" runat="server" CssClass="form-control" style="margin-top:6px;">
                    <asp:ListItem Text="PDF" Value="1" />
                    <asp:ListItem Text="PPT/PPTX" Value="1" /> <%-- lo guardamos como Pdf/Otro, da igual visualmente --%>
                </asp:DropDownList>
                <asp:Button ID="btnSubirMaterial" runat="server" Text="Subir material" CssClass="btn btn-success"
                    OnClick="btnSubirMaterial_Click" style="margin-top:6px;" />
                <asp:Label ID="lblMatMsg" runat="server" CssClass="text-danger" />

                <asp:GridView ID="gvMateriales" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="NombreArchivo" HeaderText="Archivo" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                        <asp:HyperLinkField Text="Ver" DataNavigateUrlFields="RutaAlmacenamiento" HeaderText="Link" />
                        <asp:CommandField ShowDeleteButton="true" DeleteText="Eliminar" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
