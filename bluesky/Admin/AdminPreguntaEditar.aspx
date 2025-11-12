<%@ Page Title="Editar Pregunta" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminPreguntaEditar.aspx.cs"
    Inherits="bluesky.Admin.AdminPreguntaEditar" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="max-width:900px">
        <h2 class="mb-3"><asp:Literal ID="litTitulo" runat="server" /></h2>

        <asp:ValidationSummary ID="valSummary" runat="server" CssClass="text-danger" />

        <div class="row">
            <div class="col-md-8">
                <div class="form-group">
                    <label>Enunciado</label>
                    <asp:TextBox ID="txtEnunciado" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    <asp:RequiredFieldValidator ID="reqEnun" runat="server" ControlToValidate="txtEnunciado"
                        ErrorMessage="El enunciado es obligatorio" CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label>Categoría (opcional)</label>
                    <asp:TextBox ID="txtCategoria" runat="server" CssClass="form-control" />
                </div>

                <div class="form-inline" style="gap:10px;">
                    <div class="form-group">
                        <label>Dificultad</label>
                        <asp:TextBox ID="txtDificultad" runat="server" CssClass="form-control" Text="1" Width="80" />
                    </div>
                    <div class="form-group">
                        <label>Orden</label>
                        <asp:TextBox ID="txtOrden" runat="server" CssClass="form-control" Text="1" Width="80" />
                    </div>
                    <div class="checkbox" style="margin-left:12px;">
                        <label>
                            <asp:CheckBox ID="chkMultiple" runat="server" /> Múltiple respuesta
                        </label>
                    </div>
                    <div class="checkbox" style="margin-left:12px;">
                        <label>
                            <asp:CheckBox ID="chkActiva" runat="server" Checked="true" /> Activa
                        </label>
                    </div>
                </div>

                <hr />
                <h4>Alternativas</h4>
                <p class="text-muted">Define entre 2 y 6 alternativas. Debe haber al menos 1 correcta.</p>

                <asp:Repeater ID="repAlternativas" runat="server">
                    <HeaderTemplate>
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th style="width:60px;">Orden</th>
                                    <th>Texto</th>
                                    <th style="width:120px;">¿Correcta?</th>
                                </tr>
                            </thead>
                            <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtAltOrden" runat="server" CssClass="form-control" Text='<%# Eval("Orden") %>' />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAltTexto" runat="server" CssClass="form-control" Text='<%# Eval("Texto") %>' />
                            </td>
                            <td class="text-center">
                                <asp:CheckBox ID="chkAltCorrecta" runat="server" Checked='<%# Eval("EsCorrecta") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                            </tbody>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>

                <div class="mb-3">
                    <asp:Button ID="btnGuardar" runat="server" CssClass="btn btn-primary" Text="Guardar"
                        OnClick="btnGuardar_Click" />
                    <asp:HyperLink ID="lnkVolver" runat="server" CssClass="btn btn-default">Cancelar</asp:HyperLink>
                </div>

                <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
