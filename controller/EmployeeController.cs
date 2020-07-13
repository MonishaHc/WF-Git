using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WF_TOOL.Models;

namespace WF_TOOL.Controllers
{
    public class EmployeeController : BaseController
    {
        public ActionResult EmployeeHome()
        {
            var userId = Session["UserId"];
            if (userId != null)
            {
                EmployeeModel model = new EmployeeModel();
                model.emp_localid = userId.ToString();
                return View(model);
            }
            else
            {
                return RedirectToAction("Account", "Home");
            }
        }

        public PartialViewResult ListMyRequests(string id)
        {
            WaitingRequestingList model = new WaitingRequestingList();
            model.list = new List<WaitingRequests>();
            string[] splitData = id.Split('~');
            model.myId = splitData[0];
            int count = 0;
            var me = _entity.tb_Role.Where(x => x.Assigned_ID == model.myId).FirstOrDefault();
            var data = _entity.tb_Request_Hdr.Where(x => x.Employee_ID == model.myId && x.IsActive == true).ToList();
            foreach (var item in data)
            {
                //var application = _entity.tb_Application.Where(x => x.Application_Code == item.Application_ID && x.IsActive == true).FirstOrDefault();
                var application = item.tb_Application;
                WaitingRequests one = new WaitingRequests();
                one.req_id = item.Id;
                one.request_id = item.Request_ID;
                one.application_id = item.Application_ID ?? 0;
                one.employee_local_id = item.Employee_ID;
                one.wf_type_id = item.tb_WFType.WF_ID;
                one.count = count + 1;
                one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BKI")
                {
                    one.button_type = "Approve";
                }
                else if (item.Status_ID == "APP")
                {
                    var checkPaid = _entity.tb_WFType.Where(x => x.WF_ID == item.tb_WFType.WF_ID && x.IsActive == true).FirstOrDefault();
                    one.button_type = checkPaid.IsPaid_Request == true ? "Paid" : "Close";
                }
                else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
                {
                    var checkPaid = _entity.tb_WFType.Where(x => x.WF_ID == item.tb_WFType.WF_ID && x.IsActive == true).FirstOrDefault();
                    one.button_type = checkPaid.IsPaid_Request == true ? "Paid" : "Close";
                }
                else if (item.Status_ID == "CNL")
                {
                    one.button_type = "Cancel";
                }
                var current = _ApprovalLogRepository.Check_Request_Last_Stage(item.Request_ID);
                one.final_status = current.Item1;
                one.current_actor = current.Item2;
                model.list.Add(one);
                count = count + 1;
            }
            return PartialView("~/Views/Employee/_pv_my_request_list.cshtml", model);
        }

        //Basheer on 24-01-2020 for employee home
        public ActionResult MyRequest(string id)
        {
            //var userId = Session["UserId"];
            //if (userId != null)
            //{
            //    EmployeeModel model = new EmployeeModel();
            //    model.emp_localid = userId.ToString();
            //    model.application_code = "id";
            //    return View(model);
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            var realAdId = "";
            var localAdId = "";
            var username = User.Identity.Name;
            string[] addata = username.Split('\\');
            string[] localId = id.Split('~');
            try
            {
                if (addata != null && addata.Count() > 0)
                {
                    realAdId = addata[1];
                }
                if (localId != null && localId.Count() > 1)
                {
                    localAdId = localId[1];
                }
                var employee = _entity.tb_WF_Employee.Where(x => x.ADAccount == realAdId && x.IsActive == true).FirstOrDefault();
                if (employee != null)
                {
                    EmployeeModel model = new EmployeeModel();
                    model.emp_localid = employee.LocalEmplyee_ID;
                    model.adAccountId = employee.ADAccount;
                    Session["id"] = employee.LocalEmplyee_ID;
                    Session["username"] = employee.Emp_Name;
                    Session["adAccount"] = employee.ADAccount;
                    model.application_code = localId[0];
                    model.fdate = CurrentTime; //Basheer on 03-02-2020
                    model.tdate = CurrentTime; //Basheer on 03-02-2020
                    return View(model);
                }
                else
                {
                    var employee1 = _entity.tb_WF_Employee.Where(x => x.ADAccount == localAdId && x.IsActive == true).FirstOrDefault();
                    if (employee1 != null)
                    {
                        EmployeeModel model = new EmployeeModel();
                        model.emp_localid = employee1.LocalEmplyee_ID;
                        model.adAccountId = employee1.ADAccount;
                        Session["id"] = employee1.LocalEmplyee_ID;
                        Session["username"] = employee1.Emp_Name;
                        Session["adAccount"] = employee1.ADAccount;
                        model.application_code = localId[0];
                        model.fdate = CurrentTime; //Basheer on 03-02-2020
                        model.tdate = CurrentTime; //Basheer on 03-02-2020
                        return View(model);
                    }
                    else
                    {
                        var adAccount = Convert.ToString(Session["adAccount"]) == null ? "" : Convert.ToString(Session["adAccount"]);
                        var employee2 = _entity.tb_WF_Employee.Where(x => x.ADAccount == adAccount && x.IsActive == true).FirstOrDefault();
                        if (employee2 != null)
                        {
                            EmployeeModel model = new EmployeeModel();
                            model.emp_localid = employee2.LocalEmplyee_ID;
                            model.adAccountId = employee2.ADAccount;
                            Session["id"] = employee2.LocalEmplyee_ID;
                            Session["username"] = employee2.Emp_Name;
                            Session["adAccount"] = employee2.ADAccount;
                            model.application_code = localId[0];
                            model.fdate = CurrentTime; //Basheer on 03-02-2020
                            model.tdate = CurrentTime; //Basheer on 03-02-2020
                            return View(model);
                        }
                        else
                        {
                            return RedirectToAction("RequestPreHome", "Request");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("RequestPreHome", "Request");
            }
        }
        public ActionResult MyApproval(string id)
        {
            var realAdId = "";
            var localAdId = "";
            var username = User.Identity.Name;
            string[] addata = username.Split('\\');
            string[] localId = id.Split('~');
            try
            {
                if (addata != null && addata.Count() > 0)
                {
                    realAdId = addata[1];
                }
                if (localId != null && localId.Count() > 1)
                {
                    localAdId = localId[1];
                }
                var employee = _entity.tb_WF_Employee.Where(x => x.ADAccount == realAdId && x.IsActive == true).FirstOrDefault();
                if (employee != null)
                {
                    EmployeeModel model = new EmployeeModel();
                    model.emp_localid = employee.LocalEmplyee_ID;
                    model.adAccountId = employee.ADAccount;
                    Session["id"] = employee.LocalEmplyee_ID;
                    Session["username"] = employee.Emp_Name;
                    Session["adAccount"] = employee.ADAccount;
                    model.application_code = localId[0];
                    model.ad_account = employee.ADAccount;
                    return View(model);
                }
                else
                {
                    var employee1 = _entity.tb_WF_Employee.Where(x => x.ADAccount == localAdId && x.IsActive == true).FirstOrDefault();
                    if (employee1 != null)
                    {
                        EmployeeModel model = new EmployeeModel();
                        model.emp_localid = employee1.LocalEmplyee_ID;
                        model.adAccountId = employee1.ADAccount;
                        Session["id"] = employee1.LocalEmplyee_ID;
                        Session["username"] = employee1.Emp_Name;
                        Session["adAccount"] = employee1.ADAccount;
                        model.application_code = localId[0];
                        model.ad_account = employee1.ADAccount;
                        return View(model);
                    }
                    else
                    {
                        var adAccount = Convert.ToString(Session["adAccount"]) == null ? "" : Convert.ToString(Session["adAccount"]);
                        var employee2 = _entity.tb_WF_Employee.Where(x => x.ADAccount == adAccount && x.IsActive == true).FirstOrDefault();
                        if (employee2 != null)
                        {
                            EmployeeModel model = new EmployeeModel();
                            model.emp_localid = employee2.LocalEmplyee_ID;
                            model.adAccountId = employee2.ADAccount;
                            Session["id"] = employee2.LocalEmplyee_ID;
                            Session["username"] = employee2.Emp_Name;
                            Session["adAccount"] = employee2.ADAccount;
                            model.ad_account = employee2.ADAccount;// 19-02-2020 Archana  
                            model.application_code = localId[0];
                            model.fdate = CurrentTime; //Basheer on 03-02-2020
                            model.tdate = CurrentTime; //Basheer on 03-02-2020
                            return View(model);
                        }
                        else
                        {
                            return RedirectToAction("RequestPreHome", "Request");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("RequestPreHome", "Request");
            }
        }

        //public PartialViewResult ListMyRequestsNew(string id) //Commented by basheer on 04-0 
        //{
        //    WaitingRequestingList model = new WaitingRequestingList();
        //    model.list = new List<WaitingRequests>();
        //    string[] splitData = id.Split('~');
        //    model.myId = splitData[1];
        //    model.typeid = splitData[0];
        //    var applicationCode = splitData[2];
        //    int count = 0;
        //    var myName = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == model.myId && x.IsActive == true).FirstOrDefault();
        //    var me = _entity.tb_Role.Where(x => x.Assigned_ID == model.myId).FirstOrDefault();
        //    //var data = _entity.tb_Request_Hdr.Where(x => x.Approver_ID == model.myId && x.IsActive == true && (x.Status_ID != "CLS" || x.Status_ID != "PYD" || x.Status_ID != "REJ" || x.Status_ID!="NEW")).ToList();
        //    var data = _entity.tb_Request_Hdr.Where(x => x.Employee_ID == model.myId && x.IsActive == true && x.tb_Application.Application_Code.ToUpper() == applicationCode.ToUpper()).ToList();
        //    foreach (var item in data)
        //    {
        //        #region 
        //        var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
        //        var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
        //        //var business = _entity.tb_Business.Where(x => x.Bus_Id == emp.Business_Id && x.IsActive == true).FirstOrDefault();
        //        //var bussiness_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == emp.BusinessLine_Id && x.IsActive == true).FirstOrDefault();
        //        //var pro_group = _entity.tb_ProductGroup.Where(x => x.PG_Id == emp.PG_Id && x.IsActive == true).FirstOrDefault();
        //        //var application = _entity.tb_Application.Where(x => x.Application_Code == item.Application_ID && x.IsActive == true).FirstOrDefault();
        //        //var domain = _entity.tb_Domain.Where(x => x.Domain_ID == application.DomainId && x.IsActive == true).FirstOrDefault();
        //        WaitingRequests one = new WaitingRequests();
        //        one.req_id = item.Id;
        //        one.request_id = item.Request_ID;
        //        one.application_id = item.Application_ID ?? 0;
        //        one.application = item.tb_Application.Application_Code;
        //        one.employee_local_id = item.Employee_ID;
        //        one.wf_type_id = item.tb_WFType.WF_ID;
        //        one.count = count + 1;
        //        one.employee_name = emp.Emp_Name;
        //        //one.company = company == null ? "" : company.Company_Name;
        //        //one.business = emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business.Business_Name;
        //        //one.business_line = emp.tb_Department.tb_ProductGroup.tb_BusinessLine == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Line_Name;
        //        one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
        //        one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
        //        one.process_table_id = 0;
        //        //one.wf_domain = domain.Domain_Name;
        //        //one.wf_domain = item.tb_Application.tb_Domain.Domain_Name;
        //        if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BKI" || item.Status_ID == "QIM" || item.Status_ID == "PIM")
        //        {
        //            one.button_type = "Approve";
        //        }
        //        else if (item.Status_ID == "APP")
        //        {
        //            if (item.tb_WFType.IsPaid_Request == true)
        //            {
        //                one.button_type = "Paid";
        //            }
        //            else
        //            {
        //                one.button_type = "Close";
        //            }
        //        }
        //        else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
        //        {
        //            //var checkPaid = _entity.tb_WFType.Where(x => x.WF_ID == item.WF_ID && x.IsActive == true).FirstOrDefault();
        //            one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
        //        }
        //        var current = _ApprovalLogRepository.Check_Request_Last_Stage_For_MyRequest(item.Request_ID);
        //        one.final_status = current.Item1;
        //        one.current_actor = current.Item2;
        //        model.list.Add(one);
        //        count = count + 1;
        //        #endregion 
        //    }
        //    return PartialView("~/Views/Employee/_pv_my_request.cshtml", model);
        //} // LISTING OF EMPLOYEE REQUEST
        public PartialViewResult ListMyRequestsNew(string id)
        {
            WaitingRequestingList model = new WaitingRequestingList();
            model.list = new List<WaitingRequests>();
            string[] splitData = id.Split('~');
            model.myId = splitData[1];
            model.typeid = splitData[0];
            model.fromdate = Convert.ToDateTime(splitData[3]);
            model.todate = Convert.ToDateTime(splitData[4]);
            var status = string.Empty;
            var wftype = string.Empty; //Basheer on 13-02-2020 for WFtype in filter
            var applicationCode = splitData[2];
            int count = 0;
            var myName = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == model.myId && x.IsActive == true).FirstOrDefault();
            var me = _entity.tb_Role.Where(x => x.Assigned_ID == model.myId).FirstOrDefault();
            #region old code without SP
            //if (splitData[5] == "")
            //{
            //    var data = _entity.tb_Request_Hdr.Where(x => x.Employee_ID == model.myId && x.IsActive == true && x.tb_Application.Application_Code.ToUpper() == applicationCode.ToUpper() && EntityFunctions.TruncateTime(x.TimeStamp) >= model.fromdate.Date && EntityFunctions.TruncateTime(x.TimeStamp) <= model.todate.Date).ToList();
            //    //var data = _entity.tb_Request_Hdr.Where(x => x.Employee_ID == model.myId && x.IsActive == true && x.tb_Application.Application_Code.ToUpper() == applicationCode.ToUpper()).ToList();
            //    foreach (var item in data)
            //    {
            //        #region 
            //        var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
            //        var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
            //        //var business = _entity.tb_Business.Where(x => x.Bus_Id == emp.Business_Id && x.IsActive == true).FirstOrDefault();
            //        //var bussiness_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == emp.BusinessLine_Id && x.IsActive == true).FirstOrDefault();
            //        //var pro_group = _entity.tb_ProductGroup.Where(x => x.PG_Id == emp.PG_Id && x.IsActive == true).FirstOrDefault();
            //        //var application = _entity.tb_Application.Where(x => x.Application_Code == item.Application_ID && x.IsActive == true).FirstOrDefault();
            //        //var domain = _entity.tb_Domain.Where(x => x.Domain_ID == application.DomainId && x.IsActive == true).FirstOrDefault();
            //        WaitingRequests one = new WaitingRequests();
            //        one.req_id = item.Id;
            //        one.request_id = item.Request_ID;
            //        one.application_id = item.Application_ID ?? 0;
            //        one.application = item.tb_Application.Application_Code;
            //        one.employee_local_id = item.Employee_ID;
            //        one.wf_type_id = item.tb_WFType.WF_ID;
            //        one.count = count + 1;
            //        one.employee_name = emp.Emp_Name;
            //        //one.company = company == null ? "" : company.Company_Name;
            //        //one.business = emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business.Business_Name;
            //        //one.business_line = emp.tb_Department.tb_ProductGroup.tb_BusinessLine == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Line_Name;
            //        one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
            //        one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
            //        one.process_table_id = 0;
            //        //one.wf_domain = domain.Domain_Name;
            //        //one.wf_domain = item.tb_Application.tb_Domain.Domain_Name;
            //        if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BK1" || item.Status_ID == "QIM" || item.Status_ID == "PIM")
            //        {
            //            one.button_type = "Approve";
            //        }
            //        else if (item.Status_ID == "APP")
            //        {
            //            if (item.tb_WFType.IsPaid_Request == true)
            //            {
            //                one.button_type = "Paid";
            //            }
            //            else
            //            {
            //                one.button_type = "Close";
            //            }
            //        }
            //        else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
            //        {
            //            //var checkPaid = _entity.tb_WFType.Where(x => x.WF_ID == item.WF_ID && x.IsActive == true).FirstOrDefault();
            //            one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
            //        }
            //        var current = _ApprovalLogRepository.Check_Request_Last_Stage_For_MyRequest(item.Request_ID);
            //        one.final_status = current.Item1;
            //        one.current_actor = current.Item2;
            //        model.list.Add(one);
            //        count = count + 1;
            //        #endregion    
            //    }
            //}
            //else
            //{
            //    var data = _entity.tb_Request_Hdr.Where(x => x.Employee_ID == model.myId && x.IsActive == true && x.tb_Application.Application_Code.ToUpper() == applicationCode.ToUpper() && EntityFunctions.TruncateTime(x.TimeStamp) >= model.fromdate.Date && EntityFunctions.TruncateTime(x.TimeStamp) <= model.todate.Date && x.Status_ID == model.status).ToList();
            //    foreach (var item in data)
            //    {
            //        #region 
            //        var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
            //        var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
            //        //var business = _entity.tb_Business.Where(x => x.Bus_Id == emp.Business_Id && x.IsActive == true).FirstOrDefault();
            //        //var bussiness_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == emp.BusinessLine_Id && x.IsActive == true).FirstOrDefault();
            //        //var pro_group = _entity.tb_ProductGroup.Where(x => x.PG_Id == emp.PG_Id && x.IsActive == true).FirstOrDefault();
            //        //var application = _entity.tb_Application.Where(x => x.Application_Code == item.Application_ID && x.IsActive == true).FirstOrDefault();
            //        //var domain = _entity.tb_Domain.Where(x => x.Domain_ID == application.DomainId && x.IsActive == true).FirstOrDefault();
            //        WaitingRequests one = new WaitingRequests();
            //        one.req_id = item.Id;
            //        one.request_id = item.Request_ID;
            //        one.application_id = item.Application_ID ?? 0;
            //        one.application = item.tb_Application.Application_Code;
            //        one.employee_local_id = item.Employee_ID;
            //        one.wf_type_id = item.tb_WFType.WF_ID;
            //        one.count = count + 1;
            //        one.employee_name = emp.Emp_Name;
            //        //one.company = company == null ? "" : company.Company_Name;
            //        //one.business = emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business.Business_Name;
            //        //one.business_line = emp.tb_Department.tb_ProductGroup.tb_BusinessLine == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Line_Name;
            //        one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
            //        one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
            //        one.process_table_id = 0;
            //        //one.wf_domain = domain.Domain_Name;
            //        //one.wf_domain = item.tb_Application.tb_Domain.Domain_Name;
            //        if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BK1" || item.Status_ID == "QIM" || item.Status_ID == "PIM")
            //        {
            //            one.button_type = "Approve";
            //        }
            //        else if (item.Status_ID == "APP")
            //        {
            //            if (item.tb_WFType.IsPaid_Request == true)
            //            {
            //                one.button_type = "Paid";
            //            }
            //            else
            //            {
            //                one.button_type = "Close";
            //            }
            //        }
            //        else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
            //        {
            //            //var checkPaid = _entity.tb_WFType.Where(x => x.WF_ID == item.WF_ID && x.IsActive == true).FirstOrDefault();
            //            one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
            //        }
            //        var current = _ApprovalLogRepository.Check_Request_Last_Stage_For_MyRequest(item.Request_ID);
            //        one.final_status = current.Item1;
            //        one.current_actor = current.Item2;
            //        model.list.Add(one);
            //        count = count + 1;
            //        #endregion
            //    }
            //}
            #endregion.

            try
            {
                status = splitData[5];
            }
            catch
            {

            }
            //Basheer on 13-02-2020 for WFtype in filter
            try
            {
                wftype = splitData[6];
            }
            catch
            {

            }
            //var maindata = _entity.sp_ListMyRequests(applicationCode, model.myId, model.fromdate, model.todate, status, wftype); //Basheer on 24-03-2020 to add creator in my request list
            var maindata = _entity.sp_ListMyRequests(applicationCode, model.myId, model.fromdate, model.todate, status, wftype);
            foreach (var item in maindata)
            {
                WaitingRequests one = new WaitingRequests();
                one.req_id = item.Id;
                one.request_id = item.Request_ID;
                one.application_id = item.Application_ID ?? 0;
                one.application = item.Application_Code;
                one.employee_local_id = item.Employee_ID;
                one.wf_type_id = item.WF_ID;
                one.count = count + 1;
                one.employee_name = item.Emp_Name;
                one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                one.button_type = "";
                one.process_table_id = item.Id;
                one.final_status = item.Status_Desc;
                one.current_actor = item.Role_Desc + " " + item.Approver ?? null;
                one.wf_type_name = item.WF_App_Name; //basheer on 08-05-2020
                model.list.Add(one);
                count = count + 1;
            }
            return PartialView("~/Views/Employee/_pv_my_request.cshtml", model);
        } // LISTING OF EMPLOYEE REQUEST

        public PartialViewResult ListMyWaitingRequests(string id)
        {
            WaitingRequestingList model = new WaitingRequestingList();
            model.list = new List<WaitingRequests>();
            string[] splitData = id.Split('~');
            model.myId = splitData[1];
            model.typeid = splitData[0];
            var applicationCode = splitData[2];
            int approvaltype = Convert.ToInt32(splitData[0]);
            int count = 0;
            var year = Convert.ToInt64(splitData[3]);
            //var myName = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == model.myId && x.IsActive == true).FirstOrDefault();
            //var me = _entity.tb_Role.Where(x => x.Assigned_ID == model.myId).FirstOrDefault();
            if (approvaltype == 0)
            {
                #region 
                //var data = _entity.tb_Request_Hdr.Where(x => x.tb_Application.Application_Code == applicationCode && x.Approver_ID == model.myId && x.IsActive == true && (x.Status_ID == "INT" || x.Status_ID == "HLD" || x.Status_ID == "APP" || x.Status_ID == "APC" || x.Status_ID == "BKI" || x.Status_ID == "QIM" || x.Status_ID == "PIM" || x.Status_ID == "UPC")).ToList();
                //foreach (var item in data)
                //{
                //    #region 
                //    var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
                //    var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
                //    WaitingRequests one = new WaitingRequests();
                //    one.req_id = item.Id;
                //    one.request_id = item.Request_ID;
                //    one.application_id = item.Application_ID ?? 0;
                //    one.application = item.tb_Application.Application_Code;
                //    one.employee_local_id = item.Employee_ID;
                //    one.wf_type_id = item.tb_WFType.WF_ID;
                //    one.count = count + 1;
                //    one.employee_name = emp.Emp_Name;
                //    one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
                //    one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                //    one.process_table_id = 0;
                //    if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BKI" || item.Status_ID == "QIM" || item.Status_ID == "PIM")
                //    {
                //        one.button_type = "Approve";
                //    }
                //    else if (item.Status_ID == "APP")
                //    {
                //        if (item.tb_WFType.IsPaid_Request == true)
                //        {
                //            one.button_type = "Paid";
                //        }
                //        else
                //        {
                //            one.button_type = "Close";
                //        }
                //    }
                //    else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
                //    {
                //        one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
                //    }
                //    var current = _ApprovalLogRepository.Check_Request_Last_Stage_For_WaitingApproval(item.Request_ID);
                //    one.final_status = current.Item1;
                //    one.current_actor = current.Item2;
                //    model.list.Add(one);
                //    count = count + 1;
                //    #endregion
                //}
                //var my_specialRoles = _entity.tb_UniversalLookupTable.Where(x => x.Description == model.myId && x.IsActive == true).ToList();
                //foreach (var d in my_specialRoles)
                //{
                //    var my_special_requests = _entity.tb_Request_Hdr.Where(x => x.tb_Application.Application_Code == applicationCode && x.Approver_ID == d.Table_Name).ToList();
                //    foreach (var item in my_special_requests)
                //    {
                //        #region 
                //        var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
                //        var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
                //        //var business = _entity.tb_Business.Where(x => x.Bus_Id == emp.Business_Id && x.IsActive == true).FirstOrDefault();
                //        //var bussiness_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == emp.BusinessLine_Id && x.IsActive == true).FirstOrDefault();
                //        //var pro_group = _entity.tb_ProductGroup.Where(x => x.PG_Id == emp.PG_Id && x.IsActive == true).FirstOrDefault();
                //        //var application = _entity.tb_Application.Where(x => x.Application_Code == item.Application_ID && x.IsActive == true).FirstOrDefault();
                //        //var domain = _entity.tb_Domain.Where(x => x.Domain_ID == application.DomainId && x.IsActive == true).FirstOrDefault();
                //        WaitingRequests one = new WaitingRequests();
                //        one.req_id = item.Id;
                //        one.request_id = item.Request_ID;
                //        one.application_id = item.Application_ID ?? 0;
                //        one.application = item.tb_Application.Application_Code;
                //        one.employee_local_id = item.Employee_ID;
                //        one.wf_type_id = item.tb_WFType.WF_ID;
                //        one.count = count + 1;
                //        one.employee_name = emp.Emp_Name;
                //        one.process_table_id = 0;
                //        //one.company = company == null ? "" : company.Company_Name;
                //        //one.business = emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.tb_Business.Business_Name;
                //        //one.business_line = emp.tb_Department.tb_ProductGroup.tb_BusinessLine == null ? "" : emp.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Line_Name;
                //        one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
                //        one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                //        //one.wf_domain = item.tb_Application.tb_Domain.Domain_Name;
                //        if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BKI" || item.Status_ID == "QIM" || item.Status_ID == "PIM")
                //        {
                //            one.button_type = "Approve";
                //        }
                //        else if (item.Status_ID == "APP")
                //        {
                //            one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
                //        }
                //        else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
                //        {
                //            //var checkPaid = _entity.tb_WFType.Where(x => x.WF_ID == item.WF_ID && x.IsActive == true).FirstOrDefault();
                //            one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
                //        }
                //        var current = _ApprovalLogRepository.Check_Request_Last_ByUniversal(item.Request_ID);
                //        one.final_status = current;
                //        one.current_actor = d.Code + " " + myName.Emp_Name;
                //        model.list.Add(one);
                //        count = count + 1;
                //        #endregion
                //    }
                //    var process = _entity.tb_ProcessHdr.Where(x => x.RoleId == d.Table_Name && x.IsActive == true && x.IsCompleted == false).GroupBy(x => x.RequestId).Select(x => x.FirstOrDefault()).ToList();
                //    foreach (var p in process)
                //    {
                //        #region 
                //        var item = _entity.tb_Request_Hdr.Where(x => x.Request_ID == p.RequestId.ToString() && x.IsActive == true).FirstOrDefault();
                //        var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
                //        var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
                //        WaitingRequests one = new WaitingRequests();
                //        one.req_id = item.Id;
                //        one.request_id = item.Request_ID;
                //        one.application_id = item.Application_ID ?? 0;
                //        one.application = item.tb_Application.Application_Code;
                //        one.employee_local_id = item.Employee_ID;
                //        one.wf_type_id = item.tb_WFType.WF_ID;
                //        one.count = count + 1;
                //        one.employee_name = emp.Emp_Name;
                //        one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
                //        one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                //        one.button_type = "";
                //        one.process_table_id = p.Id;
                //        var current = _ApprovalLogRepository.Check_Request_Last_ByUniversal(item.Request_ID);
                //        one.final_status = current;
                //        one.current_actor = d.Code + " " + myName.Emp_Name;
                //        model.list.Add(one);
                //        count = count + 1;
                //        #endregion
                //    }
                //}
                #endregion

                #region 
                var maindata = _entity.sp_ListRequestsForApprove(applicationCode, model.myId, year);
                foreach (var item in maindata)
                {
                    WaitingRequests one = new WaitingRequests();
                    one.req_id = item.Id ?? 0;
                    one.request_id = item.request_Id;
                    one.application_id = item.ApplicationId ?? 0;
                    one.application = item.Application_Code;
                    one.employee_local_id = item.Employee_Id;
                    one.wf_type_id = item.WF_Id;
                    one.count = count + 1;
                    one.employee_name = item.Emp_Name;
                    one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                    one.button_type = "";
                    one.process_table_id = item.Id ?? 0;
                    one.final_status = item.Status_Desc;
                    one.current_actor = item.Role_Desc + " " + item.Approver ?? null;
                    one.wf_type_name = item.WF_App_Name; //basheer on 08-05-2020
                    one.roleid = item.Role_ID;//basheer on 28-05-2020
                    model.list.Add(one);
                    count = count + 1;
                }
                #endregion 
                return PartialView("~/Views/Employee/_pv_my_approvalwaiting.cshtml", model);
            }
            else
            {
                #region 
                //var datafromlog = _entity.tb_ApprovalLog.Where(x => x.Actor_To == model.myId && x.IsActive == true && (x.Status != "NEW" || x.Status != "SUB" || x.Status != "NULL") && (x.Actor_Id != "FINANCE" || x.Actor_Id != "FINANCENONHR")).Select(x => x.RequestId).Distinct().ToList();
                //foreach (var requestid in datafromlog)
                //{
                //    var data = _entity.tb_Request_Hdr.Where(x => x.tb_Application.Application_Code == applicationCode && x.Request_ID == requestid && x.IsActive == true).ToList();
                //    foreach (var item in data)
                //    {
                //        #region 
                //        var emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Employee_ID).FirstOrDefault();
                //        var company = _entity.tb_Company.Where(x => x.Company_Id == emp.Company_Id && x.IsActive == true).FirstOrDefault();
                //        WaitingRequests one = new WaitingRequests();
                //        one.req_id = item.Id;
                //        one.request_id = item.Request_ID;
                //        one.application_id = item.Application_ID ?? 0;
                //        one.application = item.tb_Application.Application_Code;
                //        one.employee_local_id = item.Employee_ID;
                //        one.wf_type_id = item.tb_WFType.WF_ID;
                //        one.count = count + 1;
                //        one.employee_name = emp.Emp_Name;
                //        one.pro_group = emp.tb_Department.tb_ProductGroup == null ? "" : emp.tb_Department.tb_ProductGroup.PG_Name;
                //        one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                //        one.process_table_id = 0;
                //        if (item.Status_ID == "INT" || item.Status_ID == "HLD" || item.Status_ID == "BKI" || item.Status_ID == "QIM" || item.Status_ID == "PIM")
                //        {
                //            one.button_type = "Approve";
                //        }
                //        else if (item.Status_ID == "APP")
                //        {
                //            if (item.tb_WFType.IsPaid_Request == true)
                //            {
                //                one.button_type = "Paid";
                //            }
                //            else
                //            {
                //                one.button_type = "Close";
                //            }
                //        }
                //        else if (item.Status_ID == "CLR" || item.Status_ID == "PYD" && item.Process_Complete == 0)
                //        {
                //            one.button_type = item.tb_WFType.IsPaid_Request == true ? "Paid" : "Close";
                //        }
                //        var current = _ApprovalLogRepository.Check_Request_Last_Stage_For_WaitingApproval(item.Request_ID);
                //        one.final_status = current.Item1;
                //        one.current_actor = current.Item2;
                //        model.list.Add(one);
                //        count = count + 1;
                //        #endregion
                //    }
                //}
                #endregion
                var dataMain = _entity.sp_ListRequestsITouched(applicationCode, model.myId, year);
                foreach (var item in dataMain)
                {
                    WaitingRequests one = new WaitingRequests();
                    one.req_id = item.Id ?? 0;
                    one.request_id = item.request_Id;
                    one.application_id = item.ApplicationId ?? 0;
                    one.application = item.Application_Code;
                    one.employee_local_id = item.Employee_Id;
                    one.wf_type_id = item.WF_Id;
                    one.count = count + 1;
                    one.employee_name = item.Emp_Name;
                    one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                    one.process_table_id = 0;
                    one.button_type = "";
                    one.final_status = item.Status_Desc;
                    //Basheer on 06-04-2020
                    //var check = _entity.tb_Request_Hdr.Where(x => x.Request_ID == item.request_Id && x.IsActive == true).FirstOrDefault();
                    //if(_entity.tb_Role.Where(x=> x.Id == check.RoleId && x.IsActive == true).FirstOrDefault().GroupRole == true)
                    //{
                    //    if (check.Process_Complete == 1 || check.Status_ID == "CNL")
                    //    {
                    //        var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Approver && x.IsActive == true).FirstOrDefault().Emp_Name;
                    //        one.current_actor = item.Role_Desc + " " + employee ?? null;
                    //    }
                    //    else
                    //    {
                    //        one.current_actor = item.Role_Desc + " " + item.Approver ?? null;
                    //    }
                    //}
                    //else
                    //{
                    //    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Approver && x.IsActive == true).FirstOrDefault().Emp_Name;
                    //    one.current_actor = item.Role_Desc + " " + employee ?? null;
                    //}
                    one.current_actor = item.Role_Desc + " " + item.Approver ?? null;
                    one.wf_type_name = item.WF_App_Name; //basheer on 08-05-2020
                    model.list.Add(one);
                    count = count + 1;
                }
                return PartialView("~/Views/Employee/_pv_my_request.cshtml", model);
            }

        }


        //Basheer on 13-02-2020
        public PartialViewResult ListMyWaitingRequestsSearch(string id)
        {
            WaitingRequestingList model = new WaitingRequestingList();
            model.list = new List<WaitingRequests>();
            string[] splitData = id.Split('~');
            model.myId = splitData[1];
            model.typeid = splitData[0];
            var applicationCode = splitData[2];
            int approvaltype = Convert.ToInt32(splitData[0]);
            int searchtype = Convert.ToInt32(splitData[4]);
            var searchitem = splitData[5];
            int count = 0;
            var year = Convert.ToInt64(splitData[3]);
            if (approvaltype == 0)
            {
                #region 
                var maindata = _entity.sp_ListRequestsForApproveForSearch(applicationCode, model.myId, year, searchtype, searchitem);
                foreach (var item in maindata)
                {
                    WaitingRequests one = new WaitingRequests();
                    one.req_id = item.Id ?? 0;
                    one.request_id = item.request_Id;
                    one.application_id = item.ApplicationId ?? 0;
                    one.application = item.Application_Code;
                    one.employee_local_id = item.Employee_Id;
                    one.wf_type_id = item.WF_Id;
                    one.count = count + 1;
                    one.employee_name = item.Emp_Name;
                    one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                    one.button_type = "";
                    one.process_table_id = item.Id ?? 0;
                    one.final_status = item.Status_Desc;
                    one.current_actor = item.Role_Desc + " " + item.Approver ?? null;
                    one.wf_type_name = item.WF_App_Name; //basheer on 08-05-2020
                    model.list.Add(one);
                    count = count + 1;
                }
                #endregion 
                return PartialView("~/Views/Employee/_pv_my_approvalwaiting.cshtml", model);
            }
            else
            {

                var dataMain = _entity.sp_ListRequestsITouchedForSearch(applicationCode, model.myId, year, searchtype, searchitem);
                foreach (var item in dataMain)
                {
                    WaitingRequests one = new WaitingRequests();
                    one.req_id = item.Id ?? 0;
                    one.request_id = item.request_Id;
                    one.application_id = item.ApplicationId ?? 0;
                    one.application = item.Application_Code;
                    one.employee_local_id = item.Employee_Id;
                    one.wf_type_id = item.WF_Id;
                    one.count = count + 1;
                    one.employee_name = item.Emp_Name;
                    one.date = Convert.ToDateTime(item.TimeStamp).ToShortDateString();
                    one.process_table_id = 0;
                    one.button_type = "";
                    one.final_status = item.Status_Desc;
                    one.current_actor = item.Role_Desc + " " + item.Approver ?? null;
                    one.wf_type_name = item.WF_App_Name; //basheer on 08-05-2020
                    model.list.Add(one);
                    count = count + 1;
                }
                return PartialView("~/Views/Employee/_pv_my_request.cshtml", model);
            }

        }
    }
}