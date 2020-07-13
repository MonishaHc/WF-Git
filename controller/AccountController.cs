using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WF_TOOL.Models;

namespace WF_TOOL.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Home()
        {
            var username = User.Identity.Name;
            var xy = Session["UserId"];
            if(xy!=null)
            {
                var emp = _entity.tb_WF_Employee.Where(x => x.ADAccount.Trim() == xy.ToString() && x.IsActive == true).FirstOrDefault();
                if (emp != null)
                {
                    Session["User"] = emp;
                    Session["UserId"] = emp.LocalEmplyee_ID;
                }
                return RedirectToAction("RequestHome", "Request",new { id=xy});
            }
            else
            {
                return View();
            }
        }
        public object CheckCredentials(string id)
        {
            bool status = false;
            string msg = "";
            if (id != null || id != string.Empty)
            {
                RequestModel model = new RequestModel();
                model.employeetype = id;
                try
                {
                    var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID.Trim() == id.Trim() && x.IsActive == true).FirstOrDefault();
                    if (emp != null)
                    {
                        Session["User"] = emp;
                        Session["UserId"] = emp.LocalEmplyee_ID;
                        status = true;
                        msg = "";
                    }
                    else
                    {
                        msg = "No such Employee";
                    }
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
            }
            else
            {
                msg = "Invalid";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }



    }
}