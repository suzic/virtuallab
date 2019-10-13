using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class ManagerPage : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser;
        protected int StudentCount;

        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 1)
                Response.Redirect("~/StudentPage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected int GetStudentCount()
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            int studentCount = 0;
            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM [bhStudent] WHERE (record_status = 1)";
                studentCount = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sSqlConn.Close();
            }
            return studentCount;
        }

        protected int GetTaskCount(int ExperimentId)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            int TaskCount = 0;
            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "SELECT COUNT(*) FROM [bhTask] WHERE (fid_experiment = @id_experiment)";
                cmd.Parameters.Add("@id_experiment", SqlDbType.VarChar, 32).Value = ExperimentId.ToString();
                TaskCount = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sSqlConn.Close();
            }

            return TaskCount;
        }

        protected void gvExperiment_DataBinding(object sender, EventArgs e)
        {
            StudentCount = GetStudentCount();
        }

        protected void gvExperiment_DataBound(object sender, EventArgs e)
        {

        }

        protected void gvExperiment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Label lbID = (Label)e.Row.FindControl("lbID");
            Label lbTaskRatio = (Label)e.Row.FindControl("lbTaskRatio");
            if (lbID == null || lbTaskRatio == null)
                return;

            int taskCount = GetTaskCount(Convert.ToInt32(lbID.Text));
            lbTaskRatio.Text = taskCount.ToString() + "/" + StudentCount.ToString();

            LinkButton lbDistribute = (LinkButton)e.Row.FindControl("lbDistribute");
            Label lbTemplateUri = (Label)e.Row.FindControl("lbTemplateUri");
            if (string.IsNullOrEmpty(lbTemplateUri.Text) || lbTemplateUri.Text.Equals("uri"))
                lbDistribute.Visible = false;
        }

        protected void gvExperiment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvExperiment.SelectedDataKey == null)
                return;
            SelectExperiment(Convert.ToInt32(gvExperiment.SelectedDataKey["id_experiment"]));
        }

        protected void DistributeTasks(object sender, CommandEventArgs e)
        {

        }

        protected void InsertExperiment(object sender, CommandEventArgs e)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);

            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "INSERT INTO bhExperiment(record_status, title, template_uri, rjson_uri, memo, create_date, update_date) VALUES (1, '未命名实验', 'uri', 'uri', '实验描述文字', @date, @date)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sSqlConn.Close();
            }

            // Refresh
            gvExperiment.DataBind();
            gvExperiment.PageIndex = gvExperiment.PageCount - 1;
            gvExperiment.SelectedIndex = gvExperiment.Rows.Count - 1;
            gvExperiment_SelectedIndexChanged(this, EventArgs.Empty);
        }

        protected void SelectExperiment(int ExperimentId)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            DataSet outDS = new DataSet();

            try
            {
                sSqlConn.Open();
                string cmdText = "SELECT [title], [rjson_uri], [template_uri], [memo], [create_date], [update_date], [delete_date], [record_status] FROM [bhExperiment] WHERE (id_experiment = @id)";
                SqlCommand cmd = new SqlCommand(cmdText);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ExperimentId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.Connection = sSqlConn;
                da.FillSchema(outDS, SchemaType.Source, "EXPERIMENT");
                da.Fill(outDS, "EXPERIMENT");

                TextBox tbTitle = (TextBox)fvExperiment.FindControl("tbTitle");
                TextBox tbDetail = (TextBox)fvExperiment.FindControl("tbDetail");
                TextBox tbCodeUri = (TextBox)fvExperiment.FindControl("tbCodeUri");
                TextBox tbRjsonUri = (TextBox)fvExperiment.FindControl("tbRjsonUri");
                Label lbCreateDate = (Label)fvExperiment.FindControl("lbCreateDate");
                Label lbUpdateDate = (Label)fvExperiment.FindControl("lbUpdateDate");

                tbTitle.Text = outDS.Tables["EXPERIMENT"].Rows[0]["title"].ToString();
                tbDetail.Text = outDS.Tables["EXPERIMENT"].Rows[0]["memo"].ToString();
                tbCodeUri.Text = outDS.Tables["EXPERIMENT"].Rows[0]["template_uri"].ToString();
                tbRjsonUri.Text = outDS.Tables["EXPERIMENT"].Rows[0]["rjson_uri"].ToString();
                lbCreateDate.Text = outDS.Tables["EXPERIMENT"].Rows[0]["create_date"].ToString();
                lbUpdateDate.Text = outDS.Tables["EXPERIMENT"].Rows[0]["update_date"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sSqlConn.Close();
            }
        }
    }
}