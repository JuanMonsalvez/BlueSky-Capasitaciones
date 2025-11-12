<%@ Page Title="Reportes" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminReportes.aspx.cs" Inherits="bluesky.Admin.AdminReportes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="max-width: 1100px;">
        <h2 class="page-header">Reportes (Excel)</h2>

        <asp:Panel runat="server" CssClass="panel panel-default">
            <div class="panel-body">
                <div class="row">

                    <div class="col-sm-4">
                        <label for="ddlReporte">Tipo de reporte</label>
                        <asp:DropDownList ID="ddlReporte" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Usuarios" Text="Usuarios" />
                            <asp:ListItem Value="Cursos" Text="Cursos" />
                            <asp:ListItem Value="Inscripciones" Text="Inscripciones (Usuario x Curso)" />
                            <asp:ListItem Value="Evaluaciones" Text="Evaluaciones" />
                            <asp:ListItem Value="Intentos" Text="Intentos de Evaluación" />
                        </asp:DropDownList>
                    </div>

                    <div class="col-sm-3">
                        <label for="txtDesde">Desde (opcional)</label>
                        <asp:TextBox ID="txtDesde" runat="server" CssClass="form-control" placeholder="yyyy-MM-dd" />
                        <small class="text-muted">Filtra por fecha de creación/inscripción</small>
                    </div>

                    <div class="col-sm-3">
                        <label for="txtHasta">Hasta (opcional)</label>
                        <asp:TextBox ID="txtHasta" runat="server" CssClass="form-control" placeholder="yyyy-MM-dd" />
                    </div>

                    <div class="col-sm-2" style="margin-top:25px;">
                        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-primary btn-block"
                            Text="Exportar a Excel" OnClick="btnExport_Click" />
                    </div>
                </div>

                <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" EnableViewState="false"
                    Style="display:block;margin-top:12px;"></asp:Label>
            </div>
        </asp:Panel>

        <p class="text-muted">
            * Si dejas las fechas vacías, se exportan todos los registros del tipo seleccionado.
        </p>
    </div>
</asp:Content>
