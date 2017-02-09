using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rides : System.Web.UI.Page
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
                    RidesDatabaseAccessor rdb = new RidesDatabaseAccessor ();
                    if(IsPostBack)
                    {
                        string dest   = "";
                        string depart = "";
                        string date   = "";
                        string time   = "";
                        if (destInput.Text != "" && Validation.isValidPlacesID(destInput.Text))
                            dest = destInput.Text;
                        if (departInput.Text != "" && Validation.isValidPlacesID(departInput.Text))
                            depart = departInput.Text;
                        if (dateInput.Text != "" && Validation.isValidDateString(dateInput.Text))
                            date = dateInput.Text;
                        if (timeInput.Text != "" && Validation.isValidTimeString(timeInput.Text))
                            time = timeInput.Text;
                        
                        RidesDatabaseAccessor.Filters filters = new RidesDatabaseAccessor.Filters
                        {
                            destinationID = dest,
                            departureID   = depart
                        };
                        if(date != "" || time != "")
                        {
                            DateTime dateObj;
                            if(date != "")
                            {
                                dateObj = DateTime.Parse(date);
                            }
                            else
                            {
                                dateObj = DateTime.Today;
                            }

                            if(time != "")
                            {
                                filters.time = dateObj.Add(DateTime.Parse(time).TimeOfDay);
                            }
                            else
                            {
                                filters.time = dateObj;
                            }
                        }
                        rdb.setFilters(filters);
                    }
                    createTable(rdb);
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

    private void createTable(RidesDatabaseAccessor db)
    {
        Table table = new Table();
        table.ID = "ridesTable";
        ridesPanel.Controls.Add(table);

        createTitle(table);
        createBody(table, db);
    }

    private void createTitle(Table table)
    {
        TableRow headerRow = new TableRow();
        table.Controls.Add(headerRow);
        headerRow.Style.Add("background-color", "#7A9124");
        headerRow.Style.Add("color", "white");

        headerRow.Controls.Add(new TableCell { Text = "Phone number" });
        headerRow.Controls.Add(new TableCell { Text = "Destination" });
        headerRow.Controls.Add(new TableCell { Text = "Source" });
        headerRow.Controls.Add(new TableCell { Text = "Date" });
        headerRow.Controls.Add(new TableCell { Text = "Time" });
        headerRow.Controls.Add(new TableCell { Text = "Through" });
        headerRow.Controls.Add(new TableCell { Text = "Comments" });
    }

    private void createBody(Table table, RidesDatabaseAccessor db)
    {
        foreach (var entry in db.getFiltered())
        {
            TableRow row = new TableRow();
            table.Controls.Add(row);

            row.Controls.Add(new TableCell { Text = entry.phone });
            row.Controls.Add(new TableCell { Text = entry.destinationID });
            row.Controls.Add(new TableCell { Text = entry.departureID });
            row.Controls.Add(new TableCell { Text = entry.time.ToShortDateString() });
            row.Controls.Add(new TableCell { Text = entry.time.ToShortTimeString() });
            row.Controls.Add(new TableCell { Text = entry.through });
            row.Controls.Add(new TableCell { Text = entry.comment });
        }
    }
}