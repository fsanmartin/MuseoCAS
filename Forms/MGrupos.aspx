<%@ Page Title="Maestro de Grupos de Acceso" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="MGrupos.aspx.vb" Inherits="Forms_MGrupos" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <h1>Maestro de Grupos de Acceso</h1>

    <table>
        <tr>
            <td colspan="3" style="text-align: right">
                <a href="VWGrupos.aspx"><img alt="Volver" src="../Icons/return_32.png" /></a>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="LabelsError" HeaderText="Debe ingresar datos en los siguientes campos obligatorios:" />
            </td>
        </tr>
        <tr>
            <td colspan="3"><h3>Información del Grupo</h3></td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                <asp:TextBox ID="txtID" runat="server" CssClass="textbox" Width="69px" ReadOnly="True"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblGrupo" runat="server" CssClass="Labels" Text="Grupo:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtGrupo" runat="server" CssClass="textbox" MaxLength="30" Width="200px"></asp:TextBox>
            </td>
            <td>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtGrupo" CssClass="LabelsError" ErrorMessage="Grupo">*</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDesc" runat="server" CssClass="Labels" Text="Descripción:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtDesc" runat="server" MaxLength="100" TextMode="MultiLine" Width="200px" CssClass="textbox"></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
        </tr>
        </table>

    <br />
    <table>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3"><h3>Funcionalidad Acceso para Visualizar</h3></td>
            <td colspan="3" style="text-align: center"><h3>Acceso adicional</h3></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3" style="text-align: center"><h4>Vistas</h4></td>
            <td colspan="3">&nbsp</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3">
                <asp:CheckBoxList ID="cblVW" runat="server">
                </asp:CheckBoxList>
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3" style="text-align: center"><h4>Formularios</h4></td>
            <td colspan="3">&nbsp</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="3">
                <asp:CheckBoxList ID="cblView" runat="server">
                </asp:CheckBoxList>
            </td>
            <td>
                <asp:CheckBoxList ID="cblNew" runat="server">
                </asp:CheckBoxList>
                </td>
            <td>
                <asp:CheckBoxList ID="cblEdit" runat="server">
                </asp:CheckBoxList>
                </td>
            <td>
                <asp:CheckBoxList ID="cblDelete" runat="server">
                </asp:CheckBoxList>
                </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" AlternateText="Grabar" ImageUrl="~/Icons/save.png" ToolTip="Grabar registro" />
            </td>
            <td>
                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Icons/edit_32.png" ToolTip="Modificar registro" />
            </td>
            <td>
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Icons/delete_32.png" ToolTip="Eliminar registro" OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" />
            </td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
    </asp:Content>

<asp:Content ID="Content3" runat="server" contentplaceholderid="head">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>


