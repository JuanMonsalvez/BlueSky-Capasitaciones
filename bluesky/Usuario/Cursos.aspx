<%@ Page Title="Mis cursos" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="Cursos.aspx.cs" Inherits="bluesky.Usuario.Cursos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-3">Mis cursos</h2>

    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="alert alert-info">
        Aún no estás inscrito en ningún curso.
    </asp:Panel>

    <asp:Repeater ID="rptCursos" runat="server">
        <HeaderTemplate>
            <div class="row">
        </HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-6 col-lg-4 mb-3">
                <div class="card h-100">
                    <div class="card-body">
                        <h5 class="card-title"><%# Eval("Titulo") %></h5>
                        <p class="card-text"><%# Eval("DescripcionCorta") %></p>
                        <p class="small text-muted mb-2">
                            Nivel: <%# Eval("Nivel") %> · Duración: <%# Eval("DuracionHoras") %> hrs
                        </p>
                        <!-- Link a detalles si lo tienes creado -->
                        <%-- <a class="btn btn-primary btn-sm" href='<%# Eval("LinkDetalle") %>'>Ver</a> --%>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate>
            </div>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
