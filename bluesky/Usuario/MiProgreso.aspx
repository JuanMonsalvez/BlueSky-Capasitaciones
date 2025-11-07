<%@ Page Title="Mi Progreso" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="MiProgreso.aspx.cs" Inherits="bluesky.Usuario.MiProgreso" %>

<asp:Content ID="MainProg" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2 class="fw-bold mb-3">Mi Progreso</h2>
    <canvas id="grafico" style="max-width:520px;"></canvas>
</section>

<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
    const ctx = document.getElementById('grafico');
    new Chart(ctx, { type: 'doughnut', data: { labels: ['Completadas', 'Pendientes'], datasets: [{ data: [7, 3], backgroundColor: ['#1e90ff', '#ccc'] }] } })
</script>
</asp:Content>
