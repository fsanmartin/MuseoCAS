<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PruebaBorrar.aspx.vb" Inherits="PruebaBorrar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>jQuery UI Date Time Picker Simple</title>
  <link type="text/css"rel="stylesheet"
     href="App_Themes/Normal/jquery-ui.css" />   
  <script type="text/javascript"
     src="Scripts/jquery-1.8.3.min.js"></script>
  <script type="text/javascript"
     src="Scripts/jquery-ui-1.10.4.min.js"></script>
  <script type="text/javascript">
      $(document).ready(function () {
          $('#txtFechaSimple').datepicker(
          {
              dateFormat: "dd-mm-yy",
              changeMonth: true,
              changeYear: true,
          });
      });
  </script> 
</head>
<body>
  <form id="form1" runat="server">
    <div><asp:TextBox ID="txtFechaSimple" runat="server"></asp:TextBox>
      </div>
  </form>
</body>
</html> 
