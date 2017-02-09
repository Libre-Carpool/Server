<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ctrlpanel.aspx.cs" Inherits="ctrlpanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Libre Carpool - Control panel</title>
    <link rel="stylesheet" type="text/css" href="style.css" />
    <style>
        #addEntryDialog {
            pointer-events:none;
            opacity:0;
            font-size:0.75em;
            position:fixed;
            width:60%;
            padding:1em;
            margin:0;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            background-color:rgba(255, 255, 255, 0.8);
            border-style:solid;
        }

        .exitButton {
            background-color:red;
            color:white;
            font-size:1.25em;
            position:absolute;
            top:0;
            right:0;
            padding:0.5em;
            margin:0.25em;
            cursor:default;
        }

        table input {
            box-shadow: none;
            font-size:1em;
            width:80%;
            height:80%;
            margin:0.5em;
        }
    </style>
    <script src="validation.js"></script>
    <script type="text/javascript">
        function addEntry() {
            document.getElementById("addEntryDialog").style.pointerEvents = "auto";
            document.getElementById("addEntryDialog").style.opacity = "1";
        }

        function disable(e) {
            e.style.pointerEvents = "none";
            e.style.opacity = "0";
        }

        function saveEntry() {
            var phone = document.getElementById("addEntryPhone").value;
            var pass = document.getElementById("addEntryPass").value;

            if (isValidPhone(phone)) {
                if (isValidPassword(pass)) {
                    disable(document.getElementById("addEntryDialog"));
                    return true;
                }
                else {
                    alert("Invalid password");
                }
            }
            else {
                alert("Invalid phone number");
            }
            return false;
        }

        function logout() {
            document.cookie = "username=; password=;";
            location.reload();
        }
    </script>
</head>
<body>
    <div class="content">
        <form runat="server">
            <h2>Control panel</h2>
            <asp:PlaceHolder id="ctrlPanel" runat="server"/>
            <div id="addEntryDialog">
                <span class="exitButton" onclick="disable(parentNode)">X</span>
                <table>
                    <tr style="background-color: #7A9124;color:white">
                        <td colspan="2">
                            <u>Adding a user</u>
                        </td>
                    </tr>
                    <tr style="background-color: #7A9124;color:white">
                        <td>
                            Phone number
                        </td>
                        <td>
                            Temporary password
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input id="addEntryPhone" type="text" runat="server"/>
                        </td>
                        <td>
                            <input id="addEntryPass" type="text" value="" runat="server"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button runat="server" Text="שמור" OnClientClick="return saveEntry()" OnClick="addEntry"/>
                        </td>
                    </tr>
                </table>
            </div>
        </form>

        <a href="login.aspx" onclick="logout()">To log out, click here</a><br />
    </div>
</body>
</html>
