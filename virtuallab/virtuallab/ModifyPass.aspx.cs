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
    /// 学生修改密码的界面
    /// </summary>
    public partial class ModifyPass : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser
        {
            get { return ((SiteMaster)Master).CurrentLoginUser; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 执行修改密码相关的SQL操作
        /// </summary>
        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            // 获取数据库读取连接字符串并建立连接
            string sConnString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // 准备查询命令和接受查询结果的数据工具集，进行查询，结果通过da格式化填充到ds中
            using (SqlConnection sSqlConn = new SqlConnection(sConnString))
            {
                SqlCommand cmd = sSqlConn.CreateCommand();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                try
                {
                    sSqlConn.Open();

                    // 1st step: verify old password
                    cmd.CommandText = "SELECT DISTINCT [alias] FROM bhStudent WHERE (id_student = @id_student) and (password = @old_password)";
                    cmd.Parameters.Add("@old_password", SqlDbType.VarChar, 32).Value = CurrentPassword.Text;
                    cmd.Parameters.Add("@id_student", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserID"].Value);
                    var retValue = cmd.ExecuteScalar();
                    if (retValue == null)
                        return;
                    string ret = retValue.ToString();
                    if (string.IsNullOrEmpty(ret))
                        return;

                    // 2nd step: update to new password
                    cmd.CommandText = "UPDATE bhStudent SET password = @new_password WHERE (id_student = @id_student)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@new_password", SqlDbType.VarChar, 32).Value = NewPassword.Text;
                    cmd.Parameters.Add("@id_student", SqlDbType.Int).Value = Convert.ToInt32(Request.Cookies["UserID"].Value);
                    cmd.ExecuteNonQuery();

                    ((SiteMaster)Master).ResetLoginUser();
                    ((SiteMaster)Master).LogPart.ActiveViewIndex = 0;
                    Request.Cookies["LoginStudent"].Value = "";
                    Request.Cookies["LoginManager"].Value = "";
                    Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
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
}