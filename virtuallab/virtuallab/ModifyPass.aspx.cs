using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using virtuallab.Models;

namespace virtuallab
{
    public partial class ModifyPass : System.Web.UI.Page
    {
        public LoginUser CurrentLoginUser;

        protected void Page_Init(object sender, EventArgs e)
        {
            CurrentLoginUser = SiteMaster.CurrentLoginUser;
            if (CurrentLoginUser == null)
                Response.Redirect("~/");
            else if (CurrentLoginUser.type == 0)
                Response.Redirect("~/ManagerPage");
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            //if (IsValid)
            //{
            //    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
            //    var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
            //    IdentityResult result = manager.ChangePassword(User.Identity.GetUserId(), CurrentPassword.Text, NewPassword.Text);
            //    if (result.Succeeded)
            //    {
            //        var user = manager.FindById(User.Identity.GetUserId());
            //        signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
            //        Response.Redirect("~/Account/Manage?m=ChangePwdSuccess");
            //    }
            //    else
            //    {
            //        AddErrors(result);
            //    }
            //}
        }

    }
}