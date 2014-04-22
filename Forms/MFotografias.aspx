<%@ Page Title="Ficha para Registro de Fotografías" Language="VB" MasterPageFile="~/MasterPages/Frontend.master" AutoEventWireup="false" CodeFile="MFotografias.aspx.vb" Inherits="Forms_MFotografias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .auto-style2 {
            height: 30px;
        }
        .auto-style3 {
            width: 32px;
            height: 32px;
        }
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
            $('#cpMainContent_txtFechaEntrada').datepicker(
            {
                dateFormat: "dd-mm-yy",
                changeMonth: true,
                changeYear: true,
            });
        });
    </script> 

    <h1>Ficha para Registro de Fotografías</h1>
    <table id="tblAdministracion">
        <tr>
            <td colspan="6" style="text-align: right">
                <a href="VWAudiovisual.aspx"><img alt="Volver" class="auto-style3" src="../Icons/return_32.png" /></a>
            </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:ValidationSummary ID="valSummary" runat="server" CssClass="LabelsError" HeaderText="Debe ingresar datos en los siguientes campos obligatorios:" />
                <asp:Label ID="lblErrorMessages" runat="server" CssClass="LabelsError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="6"><h3>ADMINISTRACIÓN</h3></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblID" runat="server" CssClass="Labels" Text="ID:" Visible="False"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtID" runat="server" CssClass="read_only" ReadOnly="True" Visible="False"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblNumInventario" runat="server" CssClass="Labels" Text="Número de inventario:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtNumInventario" runat="server" CssClass="textbox"></asp:TextBox>
                <asp:Label ID="lblObligatorio" runat="server" CssClass="LabelsError" Text="*"></asp:Label>
                <asp:RequiredFieldValidator ID="valNumInventario" runat="server" ControlToValidate="txtNumInventario" CssClass="LabelsError" ErrorMessage="Número de inventario">Obligatorio</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblUbicacion" runat="server" Text="Ubicación actual:" CssClass="Labels"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboDeposito" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblEstante" runat="server" Text="Estante (E.C.F):" CssClass="Labels"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboEstante" runat="server" CssClass="textbox">
                </asp:DropDownList>
                <asp:DropDownList ID="cboColumna" runat="server" CssClass="textbox">
                </asp:DropDownList>
                <asp:DropDownList ID="cboFila" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="6"><h3>IDENTIFICACIÓN</h3></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTipoBien" runat="server" Text="Tipo de bien:" CssClass="Labels"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboTipoBien" runat="server" CssClass="textbox">
                </asp:DropDownList>
                <%--<asp:RequiredFieldValidator ID="valTipoBien" runat="server" ControlToValidate="cboTipoBien" CssClass="LabelsError" ErrorMessage="Tipo de Bien">Obligatorio</asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTitulo" runat="server" Text="Título:" CssClass="Labels"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtTitulo" runat="server" CssClass="textbox" MaxLength="100" Width="350px"></asp:TextBox>
                <asp:Label ID="lblObligatorio0" runat="server" CssClass="LabelsError" Text="*"></asp:Label>
                <asp:RequiredFieldValidator ID="valTitulo" runat="server" ControlToValidate="txtTitulo" CssClass="LabelsError" ErrorMessage="Título">Obligatorio</asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblAutor" runat="server" CssClass="Labels" Text="Autor:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtAutor" runat="server" CssClass="textbox" MaxLength="100" Width="350px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTema7" runat="server" CssClass="Labels" Text="Año:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboAnio" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblAutor0" runat="server" CssClass="Labels" Text="Lugar:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtLugar" runat="server" CssClass="textbox" MaxLength="200" Width="350px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblFondo" runat="server" CssClass="Labels" Text="Fondo o colección:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtFondo" runat="server" CssClass="textbox" MaxLength="200" Width="350px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblAutor2" runat="server" CssClass="Labels" Text="Fecha de entrada:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtFechaEntrada" runat="server" CssClass="textbox" MaxLength="10" Width="155px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblAutor3" runat="server" CssClass="Labels" Text="Modo adquisición:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboAdquisicion" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblProcedencia" runat="server" CssClass="Labels" Text="Procedencia:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtProcedencia" runat="server" CssClass="textbox" MaxLength="200" Width="282px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6"><h3>DESCRIPCIÓN DEL CONTENIDO</h3></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label1" runat="server" CssClass="Labels" Text="Especialidad del contenido:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboEspecialidad" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label2" runat="server" CssClass="Labels" Text="Estructura formal:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboEstructura" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label3" runat="server" CssClass="Labels" Text="Descripción:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="textbox" Height="70px" TextMode="MultiLine" Width="350px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="6"><h3>DESCRIPCIÓN FÍSICA</h3></td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label4" runat="server" CssClass="Labels" Text="Tipo de material:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboTipoMaterial" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label5" runat="server" CssClass="Labels" Text="Tono:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboTono" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="Label6" runat="server" CssClass="Labels" Text="Soporte (imagen):"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboSoporteImagen" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td rowspan="2" colspan="2">
                <asp:Label ID="lblDimensiones" runat="server" CssClass="Labels" Text="Dimensiones:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="lblContenedor" runat="server" Text="Soporte (alto/ancho/prof.):" CssClass="Labels"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtSopAlto" runat="server" Width="38px" CssClass="textbox"></asp:TextBox>
                <asp:TextBox ID="txtSopAncho" runat="server" Width="38px" CssClass="textbox"></asp:TextBox>
                <asp:TextBox ID="txtSopProf" runat="server" Width="38px" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblContenedor0" runat="server" Text="Imagen (alto/ancho/prof.):" CssClass="Labels"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtContAlto" runat="server" Width="38px" CssClass="textbox"></asp:TextBox>
                <asp:TextBox ID="txtContAncho" runat="server" Width="38px" CssClass="textbox"></asp:TextBox>
                <asp:TextBox ID="txtContProf" runat="server" Width="38px" CssClass="textbox"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblConservacion" runat="server" CssClass="Labels" Text="Estado de conversación:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:DropDownList ID="cboConservacion" runat="server" CssClass="textbox">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblInscripciones" runat="server" CssClass="Labels" Text="Inscripciones/Marcas:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtInscripciones" runat="server" CssClass="textbox" MaxLength="200" Width="282px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblObservaciones" runat="server" CssClass="Labels" Text="Observaciones:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtObservaciones" runat="server" CssClass="textbox" Height="70px" TextMode="MultiLine" Width="350px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblAdquisicion0" runat="server" CssClass="Labels" Text="Palabras clave:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtPalabrasClaves" runat="server" CssClass="textbox" MaxLength="200" Width="282px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblResponsable" runat="server" CssClass="Labels" Text="Responsable:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:Label ID="lblRespNombre" runat="server" CssClass="Labels" Text="Nombre:"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRespNombre" runat="server" CssClass="textbox" Width="155px"></asp:TextBox>
                <%--<asp:Label ID="lblObligatorio2" runat="server" CssClass="LabelsError" Text="*"></asp:Label>--%><%--<asp:RequiredFieldValidator ID="valRespNombre" runat="server" ControlToValidate="txtRespNombre" CssClass="LabelsError" ErrorMessage="Responsable -&gt; Nombre">Obligatorio</asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr>
            <td colspan="2"></td>
            <td colspan="2">
                <asp:Label ID="lblRespFecha" runat="server" CssClass="Labels" Text="Fecha (dd/mm/aaaa):"></asp:Label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtRespFecha" runat="server" CssClass="textbox" MaxLength="10" Width="155px"></asp:TextBox>
                <%--<asp:CustomValidator ID="cvalRespFecha" runat="server" CssClass="LabelsError" ErrorMessage="Responsable -&gt; Fecha: Fecha inválida">(*)</asp:CustomValidator>--%>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
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
            <td colspan="2">&nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
            <td colspan="2">
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblUploadFile" runat="server" CssClass="Labels" Text="Cargar imágenes:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:FileUpload ID="fileUpload" runat="server" Width="356px" CssClass="textbox" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblTituloImagen" runat="server" CssClass="Labels" Text="Título para la imagen:"></asp:Label>
            </td>
            <td colspan="4">
                <asp:TextBox ID="txtImageTitle" runat="server" CssClass="textbox" MaxLength="200" Width="282px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="auto-style2"></td>
            <td colspan="2" class="auto-style2">
                <asp:Button ID="btnUpload" runat="server" Text="Subir Imagen" Width="107px" />
            </td>
            <td colspan="2" class="auto-style2">
                <asp:Label ID="lblErrUpload" runat="server" CssClass="LabelsError"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
            <td colspan="4">
                <asp:Image ID="imgUpload" runat="server" />
                <asp:Image ID="imgFotografias" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="6">
                &nbsp;</td>
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
                <asp:ImageButton ID="btnDelete" runat="server" ImageUrl="~/Icons/delete_32.png" ToolTip="Eliminar registro" />
            </td>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

