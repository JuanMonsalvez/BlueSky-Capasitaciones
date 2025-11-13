<%@ Page Language="C#" 
    MasterPageFile="~/MasterPages/Site.Master"
    AutoEventWireup="true" 
    CodeBehind="AdminPreguntaEditar.aspx.cs" 
    Inherits="bluesky.Admin.AdminPreguntaEditar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin-top:20px;">
        <asp:HiddenField ID="hfEvalId" runat="server" />
        <asp:HiddenField ID="hfPreguntaId" runat="server" />

        <h2>
            <asp:Literal ID="litTitulo" runat="server" />
            <small>
                <asp:Label ID="lblCurso" runat="server"></asp:Label> - 
                <asp:Label ID="lblEvaluacion" runat="server"></asp:Label>
            </small>
        </h2>

        <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" EnableViewState="false"></asp:Label>

        <div class="panel panel-default" style="margin-top:15px;">
            <div class="panel-heading">Datos de la pregunta</div>
            <div class="panel-body">
                <div class="form-group">
                    <label for="txtEnunciado">Enunciado</label>
                    <asp:TextBox ID="txtEnunciado" runat="server" 
                        CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    <asp:RequiredFieldValidator ID="reqEnunciado" runat="server"
                        ControlToValidate="txtEnunciado"
                        ErrorMessage="El enunciado es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="form-group">
                    <label for="txtCategoria">Categoría (opcional)</label>
                    <asp:TextBox ID="txtCategoria" runat="server" CssClass="form-control" />
                </div>

                <div class="form-group">
                    <label for="ddlDificultad">Dificultad</label>
                    <asp:DropDownList ID="ddlDificultad" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Básica" Value="1" />
                        <asp:ListItem Text="Media" Value="2" />
                        <asp:ListItem Text="Avanzada" Value="3" />
                    </asp:DropDownList>
                </div>
            </div>
        </div>

        <div class="panel panel-default">
            <div class="panel-heading">Alternativas (una correcta)</div>
            <div class="panel-body">
                <p>Marca qué alternativa es la correcta.</p>

                <table class="table table-bordered">
                    <thead>
                        <tr>
                            <th style="width:40px;">Correcta</th>
                            <th>Texto alternativa</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td class="text-center">
                                <asp:RadioButton ID="rbCorrecta1" runat="server" GroupName="altCorrecta" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlt1" runat="server" CssClass="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td class="text-center">
                                <asp:RadioButton ID="rbCorrecta2" runat="server" GroupName="altCorrecta" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlt2" runat="server" CssClass="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td class="text-center">
                                <asp:RadioButton ID="rbCorrecta3" runat="server" GroupName="altCorrecta" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlt3" runat="server" CssClass="form-control" />
                            </td>
                        </tr>
                        <tr>
                            <td class="text-center">
                                <asp:RadioButton ID="rbCorrecta4" runat="server" GroupName="altCorrecta" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtAlt4" runat="server" CssClass="form-control" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div style="margin-top:10px;">
            <asp:Button ID="btnGuardar" runat="server" 
                Text="Guardar pregunta" 
                CssClass="btn btn-primary"
                OnClick="btnGuardar_Click" />

            <asp:Button ID="btnCancelar" runat="server"
                Text="Cancelar"
                CssClass="btn btn-default"
                CausesValidation="false"
                OnClick="btnCancelar_Click"
                Style="margin-left:10px;" />
        </div>
    </div>
</asp:Content>
