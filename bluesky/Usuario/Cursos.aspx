<%@ Page Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="Cursos.aspx.cs"
    Inherits="bluesky.Usuario.Cursos" %>


<asp:Content ID="HeadCursos" ContentPlaceHolderID="HeadContent" runat="server"></asp:Content>

<asp:Content ID="MainCursos" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2 class="fw-bold">Mis Cursos</h2>
        <div class="d-flex">
            <input type="text" class="form-control" placeholder="Buscar curso..." />
            <button class="btn btn-primary ms-2"><i class="fa fa-search"></i></button>
        </div>
    </div>

    <div class="row g-3">
        <div class="col-md-4">
            <div class="card p-3 shadow-sm h-100">
                <h5 class="fw-bold">Atención al Cliente</h5>
                <p class="text-muted">Duración: 10 h</p>
                <a href="<%: ResolveUrl("~/Usuario/CursoDetalle.aspx?id=1") %>" class="btn btn-sm btn-primary">Ver curso</a>
            </div>
        </div>
        <div class="col-md-4">
            <div class="card p-3 shadow-sm h-100">
                <h5 class="fw-bold">Seguridad Financiera</h5>
                <p class="text-muted">Duración: 8 h</p>
                <a href="<%: ResolveUrl("~/Usuario/CursoDetalle.aspx?id=2") %>" class="btn btn-sm btn-primary">Ver curso</a>
            </div>
        </div>
    </div>
</section>
</asp:Content>
