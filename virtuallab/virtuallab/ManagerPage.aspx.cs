using Newtonsoft.Json;
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

        /// <summary>
        /// 获取所有未禁用学生的数量
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 获取某个实验已经分发下去的任务数（每个任务代表该实验分配给了一个学生）
        /// </summary>
        /// <param name="ExperimentId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 数据将要绑定到数据表
        /// </summary>
        protected void gvExperiment_DataBinding(object sender, EventArgs e)
        {
            StudentCount = GetStudentCount();
        }

        /// <summary>
        /// 数据已经绑定到数据表
        /// </summary>
        protected void gvExperiment_DataBound(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 数据逐行绑定完成后的处理，主要用于显示任务分配情况和分配按钮
        /// </summary>
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

        /// <summary>
        /// 选择变更
        /// </summary>
        protected void gvExperiment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gvExperiment.SelectedDataKey == null)
                return;
            SelectExperiment(Convert.ToInt32(gvExperiment.SelectedDataKey["id_experiment"]));
        }

        /// <summary>
        /// 将一个实验分配给所有未禁用的学生
        /// </summary>
        protected void DistributeTasks(object sender, CommandEventArgs e)
        {

        }

        /// <summary>
        /// 新建一个实验
        /// </summary>
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

        /// <summary>
        /// 更新一个实验
        /// </summary>
        protected void UpdateExperiment(object sender, CommandEventArgs e)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            Label lbExpID = (Label)fvExperiment.FindControl("lbExpID");
            TextBox tbTitle = (TextBox)fvExperiment.FindControl("tbTitle");
            TextBox tbDetail = (TextBox)fvExperiment.FindControl("tbDetail");
            TextBox tbCodeUri = (TextBox)fvExperiment.FindControl("tbCodeUri");
            TextBox tbRjsonUri = (TextBox)fvExperiment.FindControl("tbRjsonUri");
            Label lbUpdateDate = (Label)fvExperiment.FindControl("lbUpdateDate");

            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "UPDATE bhExperiment SET title = @title, memo = @memo, template_uri = @template_uri, rjson_uri = @rjson_uri, update_date=@update_date WHERE (id_experiment = @id_experiment)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@title", SqlDbType.VarChar, 64).Value = tbTitle.Text;
                cmd.Parameters.Add("@memo", SqlDbType.Text).Value = tbDetail.Text;
                cmd.Parameters.Add("@template_uri", SqlDbType.VarChar, 64).Value = tbCodeUri.Text;
                cmd.Parameters.Add("@rjson_uri", SqlDbType.VarChar, 64).Value = tbRjsonUri.Text;
                cmd.Parameters.Add("@update_date", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@id_experiment", SqlDbType.Int).Value = Convert.ToInt32(lbExpID.Text);
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
            int oldIndex = gvExperiment.PageIndex;
            int oldSelectIndex = gvExperiment.SelectedIndex;
            gvExperiment.DataBind();
            gvExperiment.PageIndex = oldIndex;
            gvExperiment.SelectedIndex = oldSelectIndex;
        }

        /// <summary>
        /// 选择一个实验
        /// </summary>
        /// <param name="ExperimentId"></param>
        protected void SelectExperiment(int ExperimentId)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            DataSet outDS = new DataSet();

            try
            {
                sSqlConn.Open();
                string cmdText = "SELECT [id_experiment], [title], [rjson_uri], [template_uri], [memo], [create_date], [update_date], [delete_date], [record_status] FROM [bhExperiment] WHERE (id_experiment = @id)";
                SqlCommand cmd = new SqlCommand(cmdText);
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ExperimentId;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.Connection = sSqlConn;
                da.FillSchema(outDS, SchemaType.Source, "EXPERIMENT");
                da.Fill(outDS, "EXPERIMENT");

                Label lbExpID = (Label)fvExperiment.FindControl("lbExpID");
                TextBox tbTitle = (TextBox)fvExperiment.FindControl("tbTitle");
                TextBox tbDetail = (TextBox)fvExperiment.FindControl("tbDetail");
                TextBox tbCodeUri = (TextBox)fvExperiment.FindControl("tbCodeUri");
                TextBox tbRjsonUri = (TextBox)fvExperiment.FindControl("tbRjsonUri");
                Label lbCreateDate = (Label)fvExperiment.FindControl("lbCreateDate");
                Label lbUpdateDate = (Label)fvExperiment.FindControl("lbUpdateDate");

                lbExpID.Text = outDS.Tables["EXPERIMENT"].Rows[0]["id_experiment"].ToString();
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