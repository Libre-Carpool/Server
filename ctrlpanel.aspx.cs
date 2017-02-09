using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ctrlpanel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request.Cookies["username"] != null && Request.Cookies["password"] != null)
        {
            string username = Request.Cookies["username"].Value;
            string password = Request.Cookies["password"].Value;
            if (Validation.isValidPhoneNumber(username) && Validation.isValidPassword(password))
            {
                UsersDatabaseAccessor db = new UsersDatabaseAccessor(username, password);
                if (db.isLoggedIn())
                {
                    createTable(db);
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

    private void createTable(UsersDatabaseAccessor db)
    {
        Table table = new Table();
        ctrlPanel.Controls.Add(table);

        createTitle(table, db);
        createBody(table, db);
    }

    private void createTitle(Table table, UsersDatabaseAccessor db)
    {
        if (db.CanAdd())
        {
            int colspan = 1;
            if (db.CanDelete()) colspan++;
            if (db.CanModify()) colspan += 3;

            TableRow additionTitleRow = new TableRow();
            table.Controls.Add(additionTitleRow);
            additionTitleRow.Style.Add("background-color", "#7A9124");
            additionTitleRow.Style.Add("color", "white");

            TableCell additionTitleCell = new TableCell { ColumnSpan = colspan };
            additionTitleRow.Controls.Add(additionTitleCell);

            Button additionTitleButton = new Button();
            additionTitleCell.Controls.Add(additionTitleButton);
            additionTitleButton.Text = "Adding";
            additionTitleButton.OnClientClick = "addEntry(); return false";
        }

        TableRow permissioTitlenRow = new TableRow();
        table.Controls.Add(permissioTitlenRow);
        permissioTitlenRow.Style.Add("background-color", "#7A9124");
        permissioTitlenRow.Style.Add("color", "white");
        permissioTitlenRow.Controls.Add(new TableCell());

        if (db.CanModify())
        {
            permissioTitlenRow.Controls.Add(new TableCell { ColumnSpan = 3, Text = "Permissions" });
        }

        if (db.CanDelete())
        {
            permissioTitlenRow.Controls.Add(new TableCell());
        }

        TableRow titleRow = new TableRow();
        table.Controls.Add(titleRow);
        titleRow.Style.Add("background-color", "#7A9124");
        titleRow.Style.Add("color", "white");
        
        titleRow.Controls.Add(new TableCell { Text = "Phone number" });

        if (db.CanModify())
        {
            titleRow.Controls.Add(new TableCell { Text = "Adding" });
            titleRow.Controls.Add(new TableCell { Text = "Deleting" });
            titleRow.Controls.Add(new TableCell { Text = "Managing" });
        }

        if (db.CanDelete())
        {
            titleRow.Controls.Add(new TableCell { Text = "Deletion" });
        }
    }

    private void createBody(Table table, UsersDatabaseAccessor db)
    {
        foreach (var entry in db.getAll())
        {
            TableRow bodyRow = new TableRow();
            table.Controls.Add(bodyRow);
            
            bodyRow.Controls.Add(new TableCell { Text = entry.phone });

            if (db.CanModify())
            {
                TableCell canAddInputCell = new TableCell();
                TableCell canDeleteInputCell = new TableCell();
                TableCell canModifyInputCell = new TableCell();
                bodyRow.Controls.Add(canAddInputCell);
                bodyRow.Controls.Add(canDeleteInputCell);
                bodyRow.Controls.Add(canModifyInputCell);

                CheckBox canAddInput = new CheckBox { ID = "canadd_" + entry.phone, AutoPostBack=true };
                CheckBox canDeleteInput = new CheckBox { ID = "candel_" + entry.phone, AutoPostBack = true };
                CheckBox canModifyInput = new CheckBox { ID = "canmod_" + entry.phone, AutoPostBack = true };
                canAddInputCell.Controls.Add(canAddInput);
                canDeleteInputCell.Controls.Add(canDeleteInput);
                canModifyInputCell.Controls.Add(canModifyInput);
                
                if (entry.canAdd)
                {
                    canAddInput.Checked = true;
                }
                if (entry.canDel)
                {
                    canDeleteInput.Checked = true;
                }
                if (entry.canMod)
                {
                    canModifyInput.Checked = true;
                }

                canAddInput.CheckedChanged += permitAdd;
                canDeleteInput.CheckedChanged += permitDelete;
                canModifyInput.CheckedChanged += permitModify;
            }

            if (db.CanDelete())
            {
                TableCell deleteInputCell = new TableCell();
                bodyRow.Controls.Add(deleteInputCell);

                Button deleteInput = new Button { Text = "Delete", ID = "delete_" + entry.phone };
                deleteInputCell.Controls.Add(deleteInput);
                deleteInput.Click += deleteEntry;
            }
        }
    }

    private void permitAdd(Object sender, EventArgs e)
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
                    if (db.CanModify())
                    {
                        db.switchAdd(((CheckBox)sender).ID.Split('_')[1]);
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }
    }

    private void permitDelete(Object sender, EventArgs e)
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
                    if (db.CanModify())
                    {
                        db.switchDelete(((CheckBox)sender).ID.Split('_')[1]);
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }
    }

    private void permitModify(Object sender, EventArgs e)
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
                    if (db.CanModify())
                    {
                        db.switchModify(((CheckBox)sender).ID.Split('_')[1]);
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }
    }

    private void deleteEntry(object sender, EventArgs e)
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
                    if (db.CanDelete())
                    {
                        db.deleteUser(((Button)sender).ID.Split('_')[1]);
                        Response.Redirect(Request.RawUrl);
                    }
                }
            }
        }
    }

    public void addEntry(Object sender, EventArgs e)
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
                    if (db.CanAdd())
                    {
                        string entryUsername = addEntryPhone.Value;
                        string entryPassword = addEntryPass.Value;
                        if (Validation.isValidPhoneNumber(entryUsername) && Validation.isValidPassword(entryPassword))
                        {
                            db.addUser(entryUsername, entryPassword);
                            Response.Redirect(Request.RawUrl);
                        }
                    }
                }
            }
        }
    }
}