using System;

public partial class login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(IsPostBack)
        {
            String username = unameInput.Text;
            String password = pwordInput.Text;

            if (validate())
            {
                if (UsersDatabaseAccessor.login(username, password))
                {
                    Response.Cookies["username"].Value = username;
                    Response.Cookies["username"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["password"].Value = password; // todo: hash password with salt
                    Response.Cookies["password"].Expires = DateTime.Now.AddDays(30);
                    Response.Redirect("ctrlpanel.aspx");
                }
                else
                {
                    failureTooltip.Style.Add("height", "100%");
                }
            }
            else
            {
                return;
            }
        }
    }

    private bool validate()
    {
        bool valid = true;
        String username = unameInput.Text;
        String password = pwordInput.Text;

        if (!Validation.isValidPhoneNumber(username) || !Validation.isValidPassword(password))
        {
            failureTooltip.Style.Add("height", "100%");
            valid = false;
        }
        if(!ReCaptcha.checkCaptcha(this))
        {
            captchaTooltip.Style.Add("height", "100%");
            valid = false;
        }

        return valid;
    }
}