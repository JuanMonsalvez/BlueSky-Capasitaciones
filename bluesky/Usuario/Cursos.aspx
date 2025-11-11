<%@ Page Title="Cursos" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="Cursos.aspx.cs" Inherits="bluesky.Publico.Cursos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2 class="page-header">Cursos disponibles</h2>

        <asp:Repeater ID="rpCursos" runat="server">
            <ItemTemplate>
                <div class="row" style="margin-bottom:18px;">
                    <div class="col-sm-3">
                        <img src="<%# Eval("Imagen") %>" class="img-responsive" style="width:100%;height:160px;object-fit:cover;border-radius:8px;" />
                    </div>
                    <div class="col-sm-9">
                        <h4 style="margin-top:0;"><%# Eval("Titulo") %></h4>
                        <p><%# Eval("Resumen") %></p>
                        <span class="label label-default"><%# Eval("Nivel") %></span>
                    </div>
                </div>
                <hr />
            </ItemTemplate>
        </asp:Repeater>

        <asp:Label ID="lblVacio" runat="server" CssClass="text-muted" Visible="false">
            No hay cursos disponibles.
        </asp:Label>
    </div>
</asp:Content>
