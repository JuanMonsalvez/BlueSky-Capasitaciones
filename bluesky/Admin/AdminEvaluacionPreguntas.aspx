<%@ Page Title="Preguntas de Evaluación" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="AdminEvaluacionPreguntas.aspx.cs"
    Inherits="bluesky.Admin.AdminEvaluacionPreguntas" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="max-width:1100px">
        <h2 class="mb-3">
            Preguntas de evaluación
            <small class="text-muted">(<asp:Literal ID="litEvalTitulo" runat="server" />)</small>
        </h2>

        <div class="mb-3">
            <a id="lnkVolver" runat="server" class="btn btn-default">← Volver a Evaluaciones</a>
            <a id="lnkNueva" runat="server" class="btn btn-primary">+ Nueva pregunta</a>
        </div>

        <asp:Panel ID="pnlVacio" runat="server" Visible="false" CssClass="alert alert-info">
            Aún no hay preguntas para esta evaluación.
        </asp:Panel>

        <asp:Repeater ID="repPreguntas" runat="server">
            <HeaderTemplate>
                <table class="table table-bordered table-striped">
                    <thead>
                        <tr>
                            <th style="width:70px;">#</th>
                            <th>Enunciado</th>
                            <th style="width:120px;">Dificultad</th>
                            <th style="width:120px;">Múltiple</th>
                            <th style="width:140px;">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%# Eval("Orden") %></td>
                    <td>
                        <div><strong><%# Eval("Enunciado") %></strong></div>
                        <small class="text-muted">
                            <%# string.IsNullOrEmpty((string)Eval("Categoria")) ? "" : "Categoría: " + Eval("Categoria") %>
                        </small>
                        <div style="margin-top:6px;">
                            <asp:Repeater ID="repAlternativas" runat="server" DataSource='<%# Eval("Alternativas") %>'>
                                <ItemTemplate>
                                    <div>
                                        • <%# Eval("Texto") %>
                                        <%# (bool)Eval("EsCorrecta") ? " <span class=\"label label-success\">correcta</span>" : "" %>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </td>
                    <td><%# Eval("Dificultad") %></td>
                    <td><%# ((bool)Eval("MultipleRespuesta")) ? "Sí" : "No" %></td>
                    <td>
                        <asp:LinkButton ID="btnEditar" runat="server" CssClass="btn btn-xs btn-primary"
                            CommandName="edit" CommandArgument='<%# Eval("Id") %>'>Editar</asp:LinkButton>
                        <asp:LinkButton ID="btnEliminar" runat="server" CssClass="btn btn-xs btn-danger"
                            OnClientClick="return confirm('¿Eliminar esta pregunta?');"
                            CommandName="del" CommandArgument='<%# Eval("Id") %>'>Eliminar</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <FooterTemplate>
                    </tbody>
                </table>
            </FooterTemplate>
        </asp:Repeater>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger"></asp:Label>
    </div>
</asp:Content>


