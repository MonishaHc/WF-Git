using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WF_Tool.ClassLibrary;
using WF_Tool.DataLibrary;
using WF_Tool.DataLibrary.Repository;

namespace WF_TOOL.Controllers
{
    public class BaseController : Controller
    {
        protected WF_DBEntities1 _entity = new WF_DBEntities1();

        //public DateTime CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(System.DateTime.Now.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        //21-10-2020 ARCHANA SRISHTI COMMENDED THESE LINE AND AVOID THE TIME CONVERSION ACCORDING TO CLIENT 
        public DateTime CurrentTime = DateTime.Now;
        public ApprovalLogRepository _ApprovalLogRepository = new ApprovalLogRepository();
        public RequestProcessRepository _rpp = new RequestProcessRepository();
        public ProcessLogRepository _plr = new ProcessLogRepository();
        public AuditLogRepository _alr = new AuditLogRepository();
        public string _userId;
        public string _userName;     

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(Session["id"]!=null)
            {
                //_entity.Configuration.ProxyCreationEnabled = false;
                string localId = Convert.ToString(Session["id"]);
                var data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == localId && x.IsActive == true).FirstOrDefault();
                Session["id"] = data.LocalEmplyee_ID;
                Session["username"] = data.Emp_Name;
            }
        }
    }
}