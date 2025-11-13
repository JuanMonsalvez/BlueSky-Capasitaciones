<%@ Page Language="C#" 
    MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" 
    CodeBehind="AdminEvaluacionPreguntas.aspx.cs" 
    Inherits="bluesky.Admin.AdminEvaluacionPreguntas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin-top:20px;">
        <asp:HiddenField ID="hfEvalId" runat="server" />

        <h2>
            Preguntas de evaluación
            <small>
                <asp:Label ID="lblCurso" runat="server"></asp:Label> - 
                <asp:Label ID="lblEvaluacion" runat="server"></asp:Label>
            </small>
        </h2>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" EnableViewState="false"></asp:Label>

        <div style="margin:15px 0;">
            <asp:Button ID="btnVolver" runat="server" Text="&lt; Volver a evaluaciones" 
                CssClass="btn btn-default"
                OnClick="btnVolver_Click" />

            <asp:Button ID="btnNuevaPregunta" runat="server" Text="Nueva pregunta"
                CssClass="btn btn-primary" 
                Style="margin-left:10px;"
                OnClick="btnNuevaPregunta_Click" />
        </div>

        <asp:GridView ID="gvPreguntas" runat="server" 
            CssClass="table table-striped table-bordered"
            AutoGenerateColumns="False"
            OnRowCommand="gvPreguntas_RowCommand"
            DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Orden" HeaderText="#" ItemStyle-Width="40px" />
                <asp:BoundField DataField="EnunciadoResumen" HeaderText="Pregunta" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoría" />
                <asp:BoundField DataField="DificultadTexto" HeaderText="Dificultad" ItemStyle-Width="100px" />
                <asp:BoundField DataField="AlternativasCount" HeaderText="# Alternativas" ItemStyle-Width="80px" />
                <asp:TemplateField HeaderText="Acciones" ItemStyle-Width="160px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditar" runat="server"
                            CommandName="EditarPregunta"
                            CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-xs btn-primary">
                            Editar
                        </asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="lnkEliminar" runat="server"
                            CommandName="EliminarPregunta"
                            CommandArgument='<%# Eval("Id") %>'
                            CssClass="btn btn-xs btn-danger"
                            OnClientClick="return confirm('¿Seguro que deseas eliminar esta pregunta?');">
                            Eliminar
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                <div class="alert alert-info">
                    Esta evaluación aún no tiene preguntas. Usa el botón <strong>Nueva pregunta</strong>.
                </div>
            </EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>
