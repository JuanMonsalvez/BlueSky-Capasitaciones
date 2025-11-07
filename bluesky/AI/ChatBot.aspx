<%@ Page Title="ChatBot Asistente" Language="C#" MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" CodeBehind="ChatBot.aspx.cs" Inherits="bluesky.AI.ChatBot" %>

<asp:Content ID="HeadChatBot" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="<%: ResolveUrl("~/Content/css/ChatBot.css") %>" rel="stylesheet" />
</asp:Content>

<asp:Content ID="MainChatBot" ContentPlaceHolderID="MainContent" runat="server">
<section class="container py-4">
    <h2 class="fw-bold mb-3"><i class="fa fa-comments me-2"></i>Asistente BlueSky AI</h2>
    <p class="text-muted">Haz tus consultas sobre cursos, evaluaciones o políticas internas.</p>

    <div class="chat-container mt-4 shadow-sm">
        <div class="chat-box" id="chatBox">
            <div class="chat-message bot">👋 ¡Hola! Soy tu asistente BlueSky AI, ¿en qué puedo ayudarte?</div>
        </div>

        <div class="chat-input d-flex mt-2">
            <input type="text" id="userInput" class="form-control" placeholder="Escribe tu mensaje..." />
            <button type="button" class="btn btn-primary ms-2" onclick="sendMessage()">
                <i class="fa fa-paper-plane"></i>
            </button>
        </div>
    </div>
</section>

<script>
function sendMessage() {
    const box = document.getElementById('chatBox');
    const input = document.getElementById('userInput');
    if (!input.value.trim()) return;
    const userMsg = document.createElement('div');
    userMsg.className = 'chat-message user';
    userMsg.textContent = input.value;
    box.appendChild(userMsg);
    input.value = '';

    const botMsg = document.createElement('div');
    botMsg.className = 'chat-message bot';
    botMsg.textContent = "🤖 (Simulación) Estoy procesando tu consulta...";
    box.appendChild(botMsg);

    box.scrollTop = box.scrollHeight;
}
</script>
</asp:Content>
