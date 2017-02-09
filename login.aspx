<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Libre Carpool - Login</title>
    <link rel="stylesheet" type="text/css" href="style.css" />
    <script src='https://www.google.com/recaptcha/api.js'></script>
    <script src="validation.js"></script>
    <script type="text/javascript">
        function onSubmit() {
            var errored = false;
            if (!isValidPhoneNumber(document.getElementById("unameInput").value)) {
                errored = true;
            }
            if (!isValidPassword(document.getElementById("pwordInput").value)) {
                errored = true;
            }

            if (errored) {
                document.getElementById("failureTooltip").style.height = "100%";
            }
            else {
                document.getElementById("failureTooltip").style.height = "0";
            }

            return !errored;
        }
    </script>
    <style>
        .g-recaptcha div {
            margin-left: auto;
            margin-right: auto;
        }
    </style>
</head>
<body>
    <div class="content">
        <h2>Login</h2>
        <form id="loginForm" runat="server" method="post">
            <div id="failureTooltip" class="tooltip" runat="server">Phone number or password is invalid</div>
            <asp:TextBox id="unameInput" runat="server" Placeholder="Phone number"/><br />
            <asp:TextBox id="pwordInput" TextMode="password" runat="server" Placeholder="Password"/><br />
            <div id="captchaTooltip" class="tooltip" runat="server">Try again</div>
            <div class="g-recaptcha" data-sitekey="6LeIxAcTAAAAAJcZVRqyHh71UMIEGNQ_MXjiZKhI"></div> <!-- WARNING: FAKE SITE KEY -->
            <input type="submit" id="submitInput" runat="server" value="Log in" onclick="return onSubmit();"/><br />
        </form>
    </div>
</body>
</html>
