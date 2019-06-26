using System;
using Azure_DevOps_Build_Release_Status;
using System.Data;
using System.Text;
using Azure_DevOps_Build_Release_Status.Tables;
using System.Web.UI.HtmlControls;
using System.Linq;
using WebFormsControlls;
using System.Web.UI;
using System.Collections;

namespace WebFormsControlls
{
    public partial class WebControls : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            var TotalData = new AzureStorageHelper("BuildReleaseDetails", "DefaultEndpointsProtocol=https;AccountName=azurefailuredata;AccountKey=f4bZRwpvAIMAVKVDecrEpAxcdpywAzgszWFTF3m/9BASkx0skkeJvk34/nV19+gYQggdiAxYdcfNOWXLY7hQxw==;EndpointSuffix=core.windows.net");
            var records = TotalData.RetrieveEntity<DevOps>();
            var uniqueprojectlist = records.GroupBy(r => r.projectname).Distinct();

            foreach (var projectname in uniqueprojectlist)
            {

                HtmlGenericControl li = new HtmlGenericControl("li");
                projectmenu.Controls.Add(li);

                HtmlAnchor anchor = new HtmlAnchor();
                anchor.ID = projectname.Key;

                anchor.ServerClick += new EventHandler(OverallStat);

                anchor.InnerText = projectname.Key;
                li.Controls.Add(anchor);
            }

        }



        protected void Page_Load(object sender, EventArgs e)
        {


            if (!this.IsPostBack)
            {
                // table constructor creation 
                HtmlAnchor temp = new HtmlAnchor();
                if (!this.IsPostBack)
                {
                    OverallStat(temp, e);
                }

            }

        }

        //same code as above but with different table queries so that data will populate based on menu item we choose

        protected void OverallStat(object sender, EventArgs e)
        {

            HtmlAnchor menuname = (HtmlAnchor)sender;

            var parent_link= "https://dev.azure.com/legrand-bcs/";            
            var incre = 0;
            string prjname = string.Empty, type = string.Empty, definionid = string.Empty, full_Hyperlink = string.Empty; 
            

            DataTable Table1 = new DataTable();            

            Table1.Columns.AddRange(new DataColumn[10] {
                    new DataColumn("Status",typeof(string)),
                    new DataColumn("Type",typeof(string)),
                    new DataColumn("Project Name", typeof(string)),
                    new DataColumn("Definition Name",typeof(string)),
                    new DataColumn("Definition ID",typeof(string)),
                    new DataColumn("Number",typeof(string)),
                    new DataColumn("Stage (Env)",typeof(string)),
                    new DataColumn("Start Time (UTC)",typeof(string)),
                    new DataColumn("Finish Time (UTC)",typeof(string)),
                    new DataColumn("Requested by",typeof(string))
                });

            //Adding DataRow.
            var TotalData = new AzureStorageHelper("BuildReleaseDetails", "DefaultEndpointsProtocol=https;AccountName=azurefailuredata;AccountKey=f4bZRwpvAIMAVKVDecrEpAxcdpywAzgszWFTF3m/9BASkx0skkeJvk34/nV19+gYQggdiAxYdcfNOWXLY7hQxw==;EndpointSuffix=core.windows.net");
            var records = TotalData.RetrieveEntity<DevOps>();
            WebControls p = new WebControls();

            var uniqueprojectlist = records.GroupBy(r => r.projectname).Distinct();
            
            StringBuilder menustr = new StringBuilder();
            StringBuilder sbtext = new StringBuilder();

            sbtext.Append("<div class='continer-fluid'>");
            sbtext.Append("<div class='well clearfix' '>");
            sbtext.Append("<div class='col-sm-10 vcenter'>");
            var projectdata = menuname.ClientID;

            string SendMenuAid = string.Empty;

            if (menuname.ClientID == "buildstat")
            {
                records = TotalData.RetrieveEntity<DevOps>("type eq 'Build'");
                sbtext.Append("<h3> Build status of all projects</h3>");
                SendMenuAid = "buildstat";               
            }
            else if (menuname.ClientID == "relstat")
            {
                records = TotalData.RetrieveEntity<DevOps>("type eq 'Release'");
                sbtext.Append("<h3>  Release status of all projects</h3>");

                SendMenuAid = "relstat";
            }
            else if (menuname.ClientID == "failstat")
            {                
                records = TotalData.RetrieveEntity<DevOps>("result eq 'failed'");
                sbtext.Append("<h3>  Failure status of all projects</h3>");

                SendMenuAid = "failstat";
            }
            else if (menuname.ClientID == "overallstatus")
            {

                records = TotalData.RetrieveEntity<DevOps>();
                sbtext.Append("<h3> Overall Build and Release status of all projects</h3>");
                SendMenuAid = "overallstatus";
             
            }
            else if (menuname.ClientID == projectdata && projectdata!= "")
            {
                records = TotalData.RetrieveEntity<DevOps>("projectname eq '" + projectdata + "'");
                sbtext.Append("<h3> " + projectdata + " : Build and Release status </h3>");                
            }            
            else
            {
                records = TotalData.RetrieveEntity<DevOps>();
                sbtext.Append("<h3> Overall Build and Release status of all projects</h3>");

            }
            sbtext.Append("</div>");
            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "confirm('" + SendMenuAid + "');", true);
            sbtext.Append("<div class='col-sm-2 vcenter'>");
            sbtext.Append("<button class='btn btn-primary btn-filter'><span class='glyphicon glyphicon-filter'></span> Filters</button>");
            sbtext.Append("</div>");
            sbtext.Append("</div>");

            sbtext.Append("<div class='table-wrapper'>");
            sbtext.Append("<table id = 'table' data-toggle = 'table' data-search = 'true' data-filter-control = 'true' data-show-export = 'true' data-click-to-select = 'true' data-toolbar = '#toolbar' class='table table-fixed table-striped table-hover table-responsive sticky-header text-nowrap'>");
            sbtext.Append("<thead>");
            sbtext.Append("<tr>");

            foreach (DataColumn column in Table1.Columns)
            {
                //sbtext.Append("<th>" + column.ColumnName + "</th>");
                sbtext.Append("<th> " + column.ColumnName + "</th >");
                //sbtext.Append("<th data-field = '" + column.ColumnName + "' data-filter-control = 'select' data-sortable = 'true' > " + column.ColumnName + "</th>");
                //sbtext.Append("<th data-field = 'xxx' data-filter-control = 'select' data-sortable = 'true' > " + column.ColumnName + "</th>");
                //sbtext.Append("<th><input type = 'dropdown' class='form-control' placeholder=" + column.ColumnName +" disabled></th>");               

                //sbtext.Append("<th><input type="dropdown" class="form - control" placeholder="#" disabled></th>");
                //sbtext.Append("<th data-field = Trail data-filter-control = 'Select'>" + column.ColumnName + "</th >");
            }            
            sbtext.Append("</tr>");
            sbtext.Append("</thead>");

            sbtext.Append("<tbody>");                                   
            
            foreach (var item in records)
            {
                Table1.Rows.Add(item.result, item.type, item.projectname, item.definitionname, item.definitionid, item.number, item.environment, item.starttime, item.finishtime, item.requestedby);
            }

            sbtext.Append("<div class='dropdown'>");
            sbtext.Append("<tr>");
            foreach (DataColumn HeaderColumn in Table1.Columns)
            {
                //sbtext.Append("<td>");
                sbtext.Append("<td><div class='btn-group'>");
                sbtext.Append("<button class='btn btn-default dropdown-toggle' type='button' data-toggle='dropdown'> " + HeaderColumn.ColumnName + "<span class='caret'></span></button>");
                sbtext.Append("<ul class='dropdown-menu' role='menu'>");
                sbtext.Append("<li><a href='#'>HTML</a></li>");
                sbtext.Append("<li><a href='#'>CSS</a></li>");                
                sbtext.Append("<li><a href='#'> About Us</a></li>");
                sbtext.Append("</ul>");
                sbtext.Append("</div></td>");
                //sbtext.Append("<tr class ='table-row' data-href = " + full_Hyperlink + ">");
            }

            sbtext.Append("</tr>");
            sbtext.Append("</div>");


            foreach (DataRow row in Table1.Rows)
            {
                incre++;
                foreach (DataColumn linkcolumn in Table1.Columns)
                {

                    if (linkcolumn.ColumnName == "Project Name")
                    {
                        prjname = row[linkcolumn.ColumnName].ToString();

                    }
                    if (linkcolumn.ColumnName == "Type")
                    {
                        type = row[linkcolumn.ColumnName].ToString();
                    }
                    if (linkcolumn.ColumnName == "Definition ID")
                    {
                        definionid = row[linkcolumn.ColumnName].ToString();
                    }

                }

                full_Hyperlink = parent_link + prjname + "/_" + type + "?definitionId=" + definionid;
                full_Hyperlink = Uri.EscapeUriString(full_Hyperlink); 

                sbtext.Append("<tr class ='table-row' data-href = "+ full_Hyperlink + ">");                

                foreach (DataColumn column in Table1.Columns)
                {

                    if (column.ColumnName == "Status" || column.ColumnName == "Type")
                    {
                        if (row[column.ColumnName].ToString() == "")
                        {
                            sbtext.Append("<td>" + "<img class='img - responsive' src = '/Images/notDeployed.png'  title='Not Deployed' width= '35' height= '35'>" + "</td>");
                        }
                        else
                        {
                            sbtext.Append("<td>" + "<img class='img - responsive' src = '/Images/" + row[column.ColumnName].ToString() + ".png'  title= " + row[column.ColumnName].ToString() + " width= '35' height= '35'>" + "</td>");
                        }
                    }

                    else
                    {
                        if(column.ColumnName == "Stage (Env)" && row[column.ColumnName].ToString() == "")
                        {
                            sbtext.Append("<td> Not applicable </td>");
                        }else
                        { 
                        sbtext.Append("<td>" + row[column.ColumnName].ToString() + "</td>");
                        }
                    }

                }
               // sbtext.Append("<a class ='table-row' target='_blank' >");
                //sbtext.Append("<a target = '_blank' ></a>");
                sbtext.Append("</tr>");

            }
            sbtext.Append("</tbody>");
            sbtext.Append("</table>");
            sbtext.Append("</div>");
            sbtext.Append("</div>");

            TabOverStat.Text = sbtext.ToString();
            //return sbtext.ToString();

        }

    }

}