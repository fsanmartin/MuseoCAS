<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="MCodigos.aspx.vb" Inherits="Forms_MCodigos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .textbox {}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" Runat="Server">
    <link type="text/css"rel="stylesheet"
        href="App_Themes/Normal/jquery-ui.css" />   
    <script type="text/javascript"
        src="Scripts/jquery-1.8.3.min.js"></script>
    <script type="text/javascript"
        src="Scripts/jquery-ui-1.10.4.min.js"></script>
    <script type="text/javascript">
        $.noConflict();
        $(document).ready(function () {
            $('#cpMainContent_txtRespFecha').datepicker(
            {
                dateFormat: "dd-mm-yy",
                changeMonth: true,
                changeYear: true,
            });
        });
    </script> 

    <h1>&nbsp;<asp:ImageButton ID="btnVolver" runat="server" CommandName="btnVolver" ImageUrl="~/Icons/return_32.png" />
&nbsp;Tabla de Códigos Generalizados</h1>
    <table id="tblAdministracion">
        <tr>
            <td colspan="6" style="text-align: right">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:ValidationSummary ID="valSummary" runat="server" CssClass="LabelsError" HeaderText="Debe ingresar datos en los siguientes campos obligatorios:" />
                <asp:Label ID="lblErrorMessages" runat="server" CssClass="LabelsError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="6"><h3>INGRESE EL VALOR</h3></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblID" runat="server" CssClass="Labels" Text="Tabla:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboTabla" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblNumInventario" runat="server" CssClass="Labels" Text="Valor:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtValor" runat="server" CssClass="textbox"></asp:TextBox>
                <asp:Label ID="lblObligatorio" runat="server" CssClass="LabelsError" Text="*"></asp:Label>
                <asp:RequiredFieldValidator ID="valValor" runat="server" ControlToValidate="txtValor" CssClass="LabelsError" ErrorMessage="Campo Valor">Obligatorio</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblDigitador0" runat="server" CssClass="Labels" Text="Auditoría:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="lblDigitador" runat="server" CssClass="Labels" Text="Digitador:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtDigitador" runat="server" CssClass="textbox" ReadOnly="True" Width="155px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
            <td colspan="2">
                <asp:Label ID="lblDigFecha" runat="server" CssClass="Labels" Text="Fecha (dd/mm/aaaa):"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtDigFecha" runat="server" CssClass="textbox" ReadOnly="True" Width="155px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:TextBox ID="txtID" runat="server" ReadOnly="True" Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <asp:ImageButton ID="btnSave" runat="server" OnClick="btnSave_Click" AlternateText="Grabar" ImageUrl="~/Icons/save.png" ToolTip="Grabar registro" />
            </td>
            <td colspan="2">
                <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/Icons/edit_32.png" ToolTip="Modificar registro" />
            </td>
            <td colspan="2">
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Icons/delete_32.png" ToolTip="Eliminar registro" OnClientClick="return confirm('¿Esta seguro de eliminar este registro?');" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

