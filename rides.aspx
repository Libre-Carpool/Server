<%@ Page Language="C#" AutoEventWireup="true" CodeFile="rides.aspx.cs" Inherits="rides" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Libre Carpool - Rides</title>
    <link rel="stylesheet" type="text/css" href="style.css" />
    <script src="validation.js"></script>
    <script type="text/javascript">
        function onSubmit() {
            var errored = false;
            var shouldPostBack = false;

            if (document.getElementById("destInput").value != "" && !isValidPlacesID(document.getElementById("destInput").value)) {
                alert("Invalid destination");
                erroed = true;
            } else shouldPostBack = true;
            if (document.getElementById("departInput").value != "" && !isValidPlacesID(document.getElementById("departInput").value)) {
                alert("Invalid source");
                erroed = true;
            } else shouldPostBack = true;
            if (document.getElementById("dateInput").value != "" && !isValidDateString(document.getElementById("dateInput").value)) {
                alert("Invalid date");
                erroed = true;
            } else shouldPostBack = true;
            if (document.getElementById("timeInput").value != "" && !isValidTimeString(document.getElementById("timeInput").value)) {
                alert("Invalid hour");
                errored = true;
            } else shouldPostBack = true;

            return !errored && shouldPostBack;
        }
    </script>

    <style>
        input {
            font-size: 1em;
            width: 25%;
        }

        table {
            font-size: 0.66em;
        }
    </style>
</head>
<body>
    <div class="content">
        <form runat="server">
            <h2>Filtering</h2>
            <asp:TextBox id="destInput" runat="server" Placeholder="Destination"/>
            <asp:TextBox id="departInput" runat="server" Placeholder="Source"/>
            <asp:TextBox id="dateInput" runat="server" Placeholder="Date"/>
            <asp:TextBox id="timeInput" runat="server" Placeholder="Time"/>
            <br />
            <input type="submit" value="Filter" runat="server" onclick="return onSubmit();"/>
            <div class="divider"></div><br />

            <asp:PlaceHolder id="ridesPanel" runat="server" />
        </form>
    </div>
</body>
</html>
