using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class newride : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
        {
            string username = Request.Cookies["username"].Value;
            string password = Request.Cookies["password"].Value;
            if (Validation.isValidPhoneNumber(username) && Validation.isValidPassword(password))
            {
                UsersDatabaseAccessor db = new UsersDatabaseAccessor(username, password);
                if (db.isLoggedIn())
                {
                    if (IsPostBack)
                    {
                        string dest = "";
                        string depart = "";
                        string date = "";
                        string time = "";
                        string through = "";
                        string comment = "";
                        if (destInput.Text != "" && Validation.isValidPlacesID(destInput.Text))
                            dest = destInput.Text;
                        if (departInput.Text != "" && Validation.isValidPlacesID(departInput.Text))
                            depart = departInput.Text;
                        if (dateInput.Text != "" && Validation.isValidDateString(dateInput.Text))
                            date = dateInput.Text;
                        if (timeInput.Text != "" && Validation.isValidTimeString(timeInput.Text))
                            time = timeInput.Text;
                        if (throughInput.Text != "" && Validation.isValidComment(throughInput.Text))
                            through = throughInput.Text;
                        if (commentInput.Text != "" && Validation.isValidComment(commentInput.Text))
                            comment = commentInput.Text;

                        if(dest == "" || depart == "")
                        {
                            return;
                        }

                        DateTime dt = default(DateTime);
                        if (date != "")
                        {
                            dt = DateTime.Parse(date);
                        }
                        else
                        {
                            dt = DateTime.Today;
                        }

                        if (time != "")
                        {
                            dt = dt.Add(DateTime.Parse(time).TimeOfDay);
                        }
                        else
                        {
                            if(date == "")
                            {
                                dt = DateTime.Now;
                            }
                        }

                        RidesDatabaseAccessor rdb = new RidesDatabaseAccessor();
                        rdb.updateEntry(new RidesDatabaseAccessor.Entry
                        {
                            phone = username,
                            destinationID = dest,
                            departureID = depart,
                            time = dt,
                            through = through,
                            comment = comment
                        });
                    }
                }
                else
                {
                    Response.Cookies["username"].Expires = DateTime.Now;
                    Response.Cookies["password"].Expires = DateTime.Now;
                    Response.Redirect("login.aspx");
                }
            }
            else
            {
                Response.Cookies["username"].Expires = DateTime.Now;
                Response.Cookies["password"].Expires = DateTime.Now;
                Response.Redirect("login.aspx");
            }
        }
        else
        {
            Response.Redirect("login.aspx");
        }
    }
}