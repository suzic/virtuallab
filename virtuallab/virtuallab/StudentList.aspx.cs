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
    /// <summary>
    /// 学生列表页面的后台代码
    /// </summary>
    public partial class StudentList : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser;

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
        /// 插入学生数据的SQL操作
        /// </summary>
        protected void InsertStudent(object sender, CommandEventArgs e)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);

            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "SELECT MAX(id_student) FROM [bhStudent]";
                int currentMaxID = (int)cmd.ExecuteScalar();

                cmd.CommandText = "INSERT INTO bhStudent(record_status, phone, belong, email, grade, gender, name, password, alias) VALUES (0, '', '', '', '', 0, '未命名', @alias, @alias)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@alias", SqlDbType.VarChar, 32).Value = "alias" + (currentMaxID + 1).ToString();
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
            gvStudents.DataBind();
            gvStudents.PageIndex = gvStudents.PageCount - 1;
            //gvStudents.SelectedIndex = gvStudents.Rows.Count - 1;
        }

        /// <summary>
        /// 更新学生数据的SQL操作
        /// </summary>
        protected void UpdateStudent(object sender, CommandEventArgs e)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);
            int nIndex = gvStudents.EditIndex;
            Label lbStudentID = (Label)gvStudents.Rows[nIndex].FindControl("lbStudentID");
            TextBox tbName = (TextBox)gvStudents.Rows[nIndex].FindControl("tbName");
            TextBox tbAlias = (TextBox)gvStudents.Rows[nIndex].FindControl("tbAlias");
            TextBox tbGrade = (TextBox)gvStudents.Rows[nIndex].FindControl("tbGrade");
            TextBox tbBelong = (TextBox)gvStudents.Rows[nIndex].FindControl("tbBelong");
            TextBox tbPhone = (TextBox)gvStudents.Rows[nIndex].FindControl("tbPhone");
            TextBox tbMail = (TextBox)gvStudents.Rows[nIndex].FindControl("tbMail");
            DropDownList ddlGenderChange = (DropDownList)gvStudents.Rows[nIndex].FindControl("ddlGenderChange");

            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "UPDATE bhStudent SET alias = @alias, password = @alias, name = @name, gender = @gender, grade = @grade, belong = @belong, phone = @phone, email = @email WHERE (id_student = @id_student)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@id_student", SqlDbType.Int).Value = Convert.ToInt32(lbStudentID.Text);
                cmd.Parameters.Add("@alias", SqlDbType.VarChar, 32).Value = tbAlias.Text;
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 16).Value = tbName.Text;
                cmd.Parameters.Add("@gender", SqlDbType.SmallInt).Value = Convert.ToInt16(ddlGenderChange.SelectedValue);
                cmd.Parameters.Add("@grade", SqlDbType.VarChar, 16).Value = tbGrade.Text;
                cmd.Parameters.Add("@belong", SqlDbType.VarChar, 32).Value = tbBelong.Text;
                cmd.Parameters.Add("@phone", SqlDbType.VarChar, 16).Value = tbPhone.Text;
                cmd.Parameters.Add("@email", SqlDbType.VarChar, 32).Value = tbMail.Text;

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
        }

        /// <summary>
        /// 对学生进行启用/禁用操作
        /// </summary>
        protected void EnableStudent(object sender, CommandEventArgs e)
        {
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection sSqlConn = new SqlConnection(sConnString);

            int nIndex = ((GridViewRow)((LinkButton)sender).NamingContainer).RowIndex;
            Label lbStudentID = (Label)gvStudents.Rows[nIndex].FindControl("lbStudentID");
            LinkButton lbEnable = (LinkButton)gvStudents.Rows[nIndex].FindControl("lbEnable");

            try
            {
                sSqlConn.Open();
                SqlCommand cmd = sSqlConn.CreateCommand();
                cmd.CommandText = "UPDATE bhStudent SET record_status = @record_status WHERE (id_student = @id_student)";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("@id_student", SqlDbType.Int).Value = Convert.ToInt32(lbStudentID.Text);
                cmd.Parameters.Add("@record_status", SqlDbType.SmallInt).Value = Convert.ToInt16(lbEnable.Visible);
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
        }
    }
}
