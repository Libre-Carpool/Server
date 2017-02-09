<%@ Page Language="C#" AutoEventWireup="true" CodeFile="newride.aspx.cs" Inherits="newride" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Libre Carpool - New ride</title>
    <link rel="stylesheet" type="text/css" href="style.css" />
    <script src="validation.js"></script>
    <script type="text/javascript">
        function onSubmit() {
            var errored = false;
            
            if (document.getElementById("destInput").value == "" || !isValidPlacesID(document.getElementById("destInput").value)) {
                alert("Invalid destination");
                erroed = true;
            }
            if (document.getElementById("departInput").value == "" || !isValidPlacesID(document.getElementById("departInput").value)) {
                alert("Invalid source");
                erroed = true;
            }
            if (document.getElementById("dateInput").value != "" && !isValidDateString(document.getElementById("dateInput").value)) {
                alert("Invalid date");
                erroed = true;
            }
            if (document.getElementById("timeInput").value != "" && !isValidTimeString(document.getElementById("timeInput").value)) {
                alert("Invalid time");
                errored = true;
            }
            if (document.getElementById("throughInput").value != "" && !isValidComment(document.getElementById("throughInput").value)) {
                alert("Invalid \"passing through\" field");
                errored = true;
            }
            if (document.getElementById("commentInput").value != "" && !isValidComment(document.getElementById("commentInput").value)) {
                alert("Invalid comment");
                errored = true;
            }

            return !errored;
        }
    </script>

    <style>
        input {
            font-size: 1em;
            width: 25%;
        }
    </style>
</head>
<body>
    <div class="content">
        <form runat="server">
            <h2>Add / edit</h2>
            
            <asp:TextBox id="destInput" runat="server" Placeholder="Destination"/>
            <asp:TextBox id="departInput" runat="server" Placeholder="Source"/>
            <asp:TextBox id="dateInput" runat="server" Placeholder="Date"/>
            <asp:TextBox id="timeInput" runat="server" Placeholder="Hour"/>
            <asp:TextBox id="throughInput" runat="server" Placeholder="Through"/>
            <asp:TextBox id="commentInput" runat="server" Placeholder="Comment"/>
            <br />
            <input type="submit" value="Save" runat="server" onclick="return onSubmit();"/>
        </form>
    </div>
</body>
</html>
