<%@ Page Title="Cursos" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="Cursos.aspx.cs" Inherits="bluesky.Publico.Cursos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="page-title">Catálogo de cursos</h2>

    <asp:Label ID="lblVacio" runat="server" CssClass="text-muted" Visible="false"
        Text="No hay cursos disponibles por ahora."></asp:Label>

    <div class="row">
        <asp:Repeater ID="rpCursos" runat="server">
            <ItemTemplate>
                <div class="col-sm-6 col-md-4" style="margin-bottom:20px;">
                    <div class="card" style="border:1px solid #eee; border-radius:10px; overflow:hidden;">
                        <img src='<%# Eval("Imagen") %>' alt="Portada" class="img-responsive" />
                        <div class="card-body" style="padding:12px 14px;">
                            <h4 style="margin:6px 0;"><%# Eval("Titulo") %></h4>
                            <div class="text-muted" style="min-height:48px;"><%# Eval("Resumen") %></div>
                            <div class="small text-muted" style="margin-top:6px;">Nivel: <%# Eval("Nivel") %></div>

                            <a runat="server" class="btn btn-primary btn-sm" 
                               href='<%# Eval("LinkDetalle") %>' 
                               style="margin-top:10px;">Ver detalle</a>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>
