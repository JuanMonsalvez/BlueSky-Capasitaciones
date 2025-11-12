<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminEvalIA.aspx.cs" Inherits="bluesky.Admin.AdminEvalIA" MasterPageFile="~/MasterPages/Site.Master" %>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <h2>Generar preguntas con IA</h2>
    <asp:Label ID="lblHdr" runat="server" CssClass="text-muted" />

    <div class="panel panel-default" style="margin-top:12px;">
        <div class="panel-heading">Parámetros</div>
        <div class="panel-body">
            <div class="row">
                <div class="col-sm-6">
                    <label>Tipo</label>
                    <asp:DropDownList ID="ddlTipo" runat="server" CssClass="form-control">
                        <asp:ListItem Value="1">Diagnóstica</asp:ListItem>
                        <asp:ListItem Value="2">Formativa</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">Sumativa</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-6">
                    <label>Número de preguntas</label>
                    <asp:DropDownList ID="ddlNum" runat="server" CssClass="form-control">
                        <asp:ListItem>10</asp:ListItem>
                        <asp:ListItem Selected="True">15</asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem>25</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row" style="margin-top:10px;">
                <div class="col-sm-4">
                    <label>Dificultad</label>
                    <asp:DropDownList ID="ddlDif" runat="server" CssClass="form-control">
                        <asp:ListItem>Básica</asp:ListItem>
                        <asp:ListItem Selected="True">Intermedia</asp:ListItem>
                        <asp:ListItem>Avanzada</asp:ListItem>
                        <asp:ListItem>Mixta</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-4">
                    <label>Tiempo (min)</label>
                    <asp:DropDownList ID="ddlTiempo" runat="server" CssClass="form-control">
                        <asp:ListItem>20</asp:ListItem>
                        <asp:ListItem Selected="True">30</asp:ListItem>
                        <asp:ListItem>45</asp:ListItem>
                        <asp:ListItem>60</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-4">
                    <label>Aprobación (%)</label>
                    <asp:DropDownList ID="ddlAprob" runat="server" CssClass="form-control">
                        <asp:ListItem>50</asp:ListItem>
                        <asp:ListItem Selected="True">60</asp:ListItem>
                        <asp:ListItem>70</asp:ListItem>
                        <asp:ListItem>80</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row" style="margin-top:10px;">
                <div class="col-sm-6">
                    <label>Cobertura</label>
                    <asp:DropDownList ID="ddlCobertura" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True">Uniforme por módulos</asp:ListItem>
                        <asp:ListItem>Libre (mezcla global)</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-6">
                    <label>Proporción Conceptual/Aplicada</label>
                    <asp:DropDownList ID="ddlMix" runat="server" CssClass="form-control">
                        <asp:ListItem Value="70/30" Selected="True">70% Conceptual / 30% Aplicada</asp:ListItem>
                        <asp:ListItem Value="50/50">50% Conceptual / 50% Aplicada</asp:ListItem>
                        <asp:ListItem Value="30/70">30% Conceptual / 70% Aplicada</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div style="margin-top:12px;">
                <asp:Button ID="btnPreview" runat="server" Text="Vista previa (3)" CssClass="btn btn-info" OnClick="btnPreview_Click" />
                <asp:Button ID="btnGenerar" runat="server" Text="Generar y guardar" CssClass="btn btn-primary" OnClick="btnGenerar_Click" />
                <asp:Label ID="lblMsg" runat="server" CssClass="text-danger" style="margin-left:10px;"></asp:Label>
            </div>
        </div>
    </div>

    <asp:Panel ID="pPreview" runat="server" Visible="false">
        <h4>Vista previa</h4>
        <asp:GridView ID="gvPreview" runat="server" CssClass="table table-bordered" AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField HeaderText="Pregunta">
                    <ItemTemplate>
                        <div><strong><%# Eval("Pregunta") %></strong></div>
                        <ol type="A">
                            <li><%# Eval("A") %></li>
                            <li><%# Eval("B") %></li>
                            <li><%# Eval("C") %></li>
                            <li><%# Eval("D") %></li>
                        </ol>
                        <div><em>Correcta: <%# Eval("Correcta") %></em></div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
