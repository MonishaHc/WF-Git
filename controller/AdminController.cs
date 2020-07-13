using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WF_Tool.ClassLibrary;
using WF_TOOL.Models;


namespace WF_TOOL.Controllers
{
    public class AdminController : BaseController
    {
        //Testing WF on TFS

        /// <summary>
        /// Admin Country , Archana 21-11-2019 
        /// </summary>
        /// <returns></returns>
        public ActionResult AdminHome()
        {
            return View();
        }
        public object IsAdmin(string id)
        {
            string msg = "Failed";
            bool status = false;
            var isAdmin = CheckAuth(id);
            if (isAdmin == string.Empty)
            {
                msg = "Is not an Admin";
            }
            else
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public string CheckAuth(string id)
        {
            #region
            id = id ?? "";
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
                if (localId != null && localId.Count() > 0)
                {
                    localAdId = localId[0];
                }
                var userData = _entity.tb_WF_Employee.Where(x => x.ADAccount == realAdId && x.IsActive == true && x.IsAdmin == 1).FirstOrDefault();//Basheer on 06-04-2020 while changing isadmin bool to in
                if (userData != null)
                {
                    FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
                    Response.Cookies["UserType"].Value = "Admin";
                    Session["id"] = userData.LocalEmplyee_ID;
                    Session["username"] = userData.Emp_Name;
                    _userId = ((string)Session["id"]);
                    _userName = ((string)Session["username"]);

                    return userData.LocalEmplyee_ID;
                }
                else
                {
                    var employee1 = _entity.tb_WF_Employee.Where(x => x.ADAccount == localAdId && x.IsActive == true && x.IsAdmin == 1).FirstOrDefault();//Basheer on 06-04-2020 while changing isadmin bool to in
                    if (employee1 != null)
                    {
                        FormsAuthentication.SetAuthCookie(employee1.LocalEmplyee_ID.ToString(), false);
                        Response.Cookies["UserType"].Value = "Admin";
                        Session["id"] = employee1.LocalEmplyee_ID;
                        Session["username"] = employee1.Emp_Name;
                        _userId = ((string)Session["id"]);
                        _userName = ((string)Session["username"]);

                        return employee1.LocalEmplyee_ID;
                    }
                    else
                    {
                        var empId = Session["id"];
                        if (empId != null)
                        {
                            var employee2 = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == empId && x.IsActive == true && x.IsAdmin == 1).FirstOrDefault(); //Basheer on 06-04-2020 while changing isadmin bool to in
                            if (employee2 != null)
                            {
                                FormsAuthentication.SetAuthCookie(employee2.LocalEmplyee_ID.ToString(), false);
                                Response.Cookies["UserType"].Value = "Admin";
                                Session["id"] = employee2.LocalEmplyee_ID;
                                Session["username"] = employee2.Emp_Name;
                                _userId = ((string)Session["id"]);
                                _userName = ((string)Session["username"]);
                                return employee2.LocalEmplyee_ID;
                            }
                            else
                            {
                                return string.Empty;
                            }
                        }
                        else
                        {
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            #endregion
        }
        public ActionResult CountryHome(string id)
        {
            //id = "";
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                CountryModel model = new CountryModel();
                model.admin_employee_local_id = permission;
                model.country_id = 0;
                model.tittle = "Add";
                model.btn_Text = "Create";
                return View(model);
            }
            else
            {
                return RedirectToAction("RequestPreHome", "Request");
            }
            #region 
            //var realAdId = "";
            //var localAdId = "";
            //var username = User.Identity.Name;
            //string[] addata = username.Split('\\');
            //string[] localId = id.Split('~');
            //try
            //{
            //    if (addata != null && addata.Count() > 0)
            //    {
            //        realAdId = addata[1];
            //    }
            //    if (localId != null && localId.Count() > 1)
            //    {
            //        localAdId = localId[1];
            //    }
            //    var userData = _entity.tb_WF_Employee.Where(x => x.ADAccount == realAdId && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if(userData!=null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);

            //        CountryModel model = new CountryModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.country_id = 0;
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //        return View(model);
            //    }
            //    else
            //    {
            //        var employee1 = _entity.tb_WF_Employee.Where(x => x.ADAccount == localAdId && x.IsActive == true).FirstOrDefault();
            //        if(employee1!=null)
            //        {
            //            FormsAuthentication.SetAuthCookie(employee1.LocalEmplyee_ID.ToString(), false);
            //            Response.Cookies["UserType"].Value = "Admin";
            //            Session["id"] = employee1.LocalEmplyee_ID;
            //            Session["username"] = employee1.Emp_Name;
            //            _userId = ((string)Session["id"]);
            //            _userName = ((string)Session["username"]);

            //            CountryModel model = new CountryModel();
            //            model.admin_employee_local_id = employee1.LocalEmplyee_ID;
            //            model.country_id = 0;
            //            model.tittle = "Add";
            //            model.btn_Text = "Create";
            //            return View(model);
            //        }
            //        else
            //        {
            //            return RedirectToAction("Home", "Account");
            //        }
            //    }
            //}
            //catch
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddCounty(CountryModel model)
        {
            string msg = "Failed";
            bool status = false;
            if (model.country_id == 0)
            {
                #region Add
                if (_entity.tb_Country.Any(x => x.Country_Code.Trim().ToLower() == model.country_code.Trim().ToLower() && x.IsActive == true))
                {
                    msg = "Country code already exits !";
                }
                else if (_entity.tb_Country.Any(x => x.Country_Name.Trim().ToLower() == model.country_name.Trim().ToLower() && x.IsActive == true))
                {
                    msg = "Country name already exits !";
                }
                else
                {
                    var input = _entity.tb_Country.Create();
                    input.Country_Code = model.country_code;
                    input.Country_Name = model.country_name;
                    input.Country_Manager = model.country_Manager;
                    input.IsActive = true;
                    input.TimeStamp = CurrentTime;
                    _entity.tb_Country.Add(input);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Country Add Sucessfully";
                        #region Keep Log
                        string content = " Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added country " + model.country_name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", input.Id.ToString(), "Add");

                        string record = input.Country_Code + " || " + input.Country_Name + " || " + input.Country_Manager + " || " + input.IsActive + " || " + input.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Country", input.Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Country.Any(x => x.Country_Code == model.country_code && x.IsActive == true && x.Id != model.country_id))
                {
                    msg = "Country code already exits !";
                }
                else if (_entity.tb_Country.Any(x => x.Country_Name == model.country_name && x.IsActive == true && x.Id != model.country_id))
                {
                    msg = "Country name already exits !";
                }
                else
                {
                    string Country_Code = "";
                    string Country_Name = "";
                    string Country_Manager = "";
                    var data = _entity.tb_Country.Where(x => x.Id == model.country_id && x.IsActive == true).FirstOrDefault();
                    if (data.Country_Code != model.country_code)
                    {
                        Country_Code = data.Country_Code;
                    }
                    if (data.Country_Name != model.country_name)
                    {
                        Country_Name = data.Country_Name;
                    }
                    if (data.Country_Manager != model.country_Manager)
                    {
                        Country_Manager = data.Country_Manager;
                    }
                    data.Country_Code = model.country_code;
                    data.Country_Name = model.country_name;
                    data.Country_Manager = model.country_Manager;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Country Edit Sucessfully";
                        #region Keep Country Name Edit
                        if (Country_Name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country name " + Country_Name + " to " + model.country_name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", data.Id.ToString(), "Edit", Country_Name, model.country_name);

                            string record = data.Country_Code + " || " + data.Country_Name + " || " + data.Country_Manager + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Country", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Country Code Edit
                        if (Country_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country code " + Country_Code + " to " + model.country_code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", data.Id.ToString(), "Edit", Country_Code, model.country_code);

                            string record = data.Country_Code + " || " + data.Country_Name + " || " + data.Country_Manager + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Country", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Country Owner Edit
                        if (Country_Manager != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country manager " + Country_Manager + " to " + model.country_Manager + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", data.Id.ToString(), "Edit", Country_Manager, model.country_Manager);

                            string record = data.Country_Code + " || " + data.Country_Name + " || " + data.Country_Manager + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Country", data.Id.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CountryList(string id)
        {
            CountryList model = new CountryList();
            model.list = new List<CountryModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            long slNo = 1;
            if (searchType == 0) // All active countries
            {
                #region 
                var country = _entity.tb_Country.Where(x => x.IsActive == true).ToList();
                foreach (var item in country)
                {
                    CountryModel one = new CountryModel();
                    one.country_code = item.Country_Code;
                    one.country_name = item.Country_Name;
                    one.country_Manager = item.Country_Manager;
                    one.country_id = item.Id;
                    one.SlNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 1)// Country code
            {
                #region
                string searchItem = Convert.ToString(splitData[1]);
                var country = _entity.tb_Country.Where(x => x.IsActive == true && x.Country_Code.Contains(searchItem)).ToList();
                foreach (var item in country)
                {
                    CountryModel one = new CountryModel();
                    one.country_code = item.Country_Code;
                    one.country_name = item.Country_Name;
                    one.country_Manager = item.Country_Manager;//aju sics 20-2-2020
                    one.country_id = item.Id;
                    one.SlNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 2)// Country name
            {
                #region 
                string searchItem = Convert.ToString(splitData[1]);
                var country = _entity.tb_Country.Where(x => x.IsActive == true && x.Country_Name.Contains(searchItem)).ToList();
                foreach (var item in country)
                {
                    CountryModel one = new CountryModel();
                    one.country_code = item.Country_Code;
                    one.country_name = item.Country_Name;
                    one.country_Manager = item.Country_Manager;//aju sics 20-2-2020
                    one.country_id = item.Id;
                    one.SlNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            return PartialView("~/Views/Admin/_pv_CountryList.cshtml", model);
        }


        public PartialViewResult EditCountry(string id)
        {
            var permission = CheckAuth(id);
            long countryId = Convert.ToInt64(id);
            CountryModel model = new CountryModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            var data = _entity.tb_Country.Where(x => x.Id == countryId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.country_code = data.Country_Code;
                model.country_name = data.Country_Name;
                model.country_Manager = data.Country_Manager;
                model.country_id = data.Id;
            }
            return PartialView("~/Views/Admin/_pv_AddCountry.cshtml", model);
        }

        public object DeleteCountry(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            CountryModel model = new CountryModel();
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Country.Where(x => x.Id == Id).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Country deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " removed country " + data.Country_Name + " on " + CurrentTime;//26-02-2020
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Admin Businesss , Archana 22-11-2019
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BusinessHome(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                BusinessModel model = new BusinessModel();
                model.admin_employee_local_id = permission;
                model.business_code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);

            //        BusinessModel model = new BusinessModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.business_code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //        model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public PartialViewResult BusinessList(string id)
        {
            BusinessList model = new BusinessList();
            model.list = new List<BusinessModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Business(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                BusinessModel one = new BusinessModel();
                one.business_id = item.Bus_Id;
                one.business_code = item.Business_Code;
                one.business = item.Business_Name;
                one.maganer_name = item.Manager;
                one.controller_name = item.Controller;
                one.office_admin = item.Office_Manager;
                one.country_name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_BusinessList.cshtml", model);
        }

        public object AddBusiness(BusinessModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.business_id == null || model.business_id == 0)
            {
                #region Add
                if (_entity.tb_Business.Any(x => x.Business_Code == model.business_code.Trim() && x.IsActive == true && x.Bus_Id != model.business_id))
                {
                    msg = "Business code already exits !";
                }
                else if (_entity.tb_Business.Any(x => x.Business_Name == model.business.Trim() && x.IsActive == true && x.Bus_Id != model.business_id))
                {
                    msg = "Business already exits !";
                }
                else
                {
                    var bs = _entity.tb_Business.Create();
                    bs.Business_Code = model.business_code;
                    bs.Business_Name = model.business;
                    bs.Bus_Manager = model.manager_id;
                    bs.Bus_Controller = model.controller_id;
                    bs.Bus_Office_Admin = model.office_admin_id;
                    bs.Country_Id = model.country_id;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Business.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Business added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added business " + model.business + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", bs.Bus_Id.ToString(), "Add");

                        string record = bs.Business_Code + "|| " + bs.Business_Name + " || " + bs.Bus_Manager + " || " + bs.Bus_Controller + " || " + bs.Bus_Office_Admin + " || " + bs.Country_Id + " || " + bs.IsActive + "||" + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", bs.Bus_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Business.Any(x => x.Business_Name == model.business && x.IsActive == true && x.Business_Code != model.business_code && x.Bus_Id != model.business_id))
                {
                    msg = "Business already exits !";
                }
                else
                {
                    string Business_Code = "";
                    string Business = "";
                    string Manager_Id = "";
                    string Controller_Id = "";
                    string Office_Admin_Id = "";
                    long Country_Code = 0;

                    var data = _entity.tb_Business.Where(x => x.Bus_Id == model.business_id && x.IsActive == true).FirstOrDefault();
                    #region ChagesChecking
                    if (data.Business_Code != model.business_code)
                    {
                        Business_Code = data.Business_Code;
                    }
                    if (data.Business_Name != model.business)
                    {
                        Business = data.Business_Name;
                    }
                    if (data.Bus_Manager != model.manager_id)
                    {
                        Manager_Id = data.Bus_Manager;
                    }
                    if (data.Bus_Controller != model.controller_id)
                    {
                        Controller_Id = data.Bus_Controller;
                    }
                    if (data.Bus_Office_Admin != model.office_admin_id)
                    {
                        Office_Admin_Id = data.Bus_Office_Admin;
                    }
                    if (data.Country_Id != model.country_id)
                    {
                        Country_Code = data.Country_Id;
                    }
                    #endregion
                    data.Business_Code = model.business_code;
                    data.Business_Name = model.business;
                    data.Bus_Manager = model.manager_id;
                    data.Bus_Controller = model.controller_id;
                    data.Bus_Office_Admin = model.office_admin_id;
                    //18/03/2020 Alena Srishti 
                    //  data.Country_Id = model.country_id;
                    // data.tb_Country.Country_Name = model.country_name; 
                    //////  data.Country_Id = model.country_id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Business Edit Sucessfully";
                        #region Keep BusinessCode Edit
                        if (Business_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business code " + Business_Code + " to " + model.business_code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", Business_Code, model.business_code);

                            string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Business Edit
                        if (Business != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business name " + Business + " to " + model.business + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", Business, model.business);

                            string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Business Manager Edit

                        if (Manager_Id != "")
                        {
                            var managerOld = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Manager_Id && x.IsActive == true).FirstOrDefault();
                            var managerNew = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Bus_Manager && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business manager " + managerOld.Emp_Name + " to " + managerNew.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", managerOld.Emp_Name, managerNew.Emp_Name);

                            string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Business Controller Edit
                        if (Controller_Id != "")
                        {
                            var controllerOld = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Controller_Id && x.IsActive == true).FirstOrDefault();
                            var controllerNew = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Bus_Controller && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business controller " + controllerOld.Emp_Name + " to " + controllerNew.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", controllerOld.Emp_Name, controllerNew.Emp_Name);

                            string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Business Office Admin Edit
                        if (Office_Admin_Id != "")
                        {
                            var officeadminOld = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Office_Admin_Id && x.IsActive == true).FirstOrDefault();
                            var officeadminNew = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Bus_Office_Admin && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business office admin " + officeadminOld.Emp_Name + " to " + officeadminNew.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", officeadminOld.Emp_Name, officeadminNew.Emp_Name);

                            string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Business Country Edit
                        //  18 / 03 / 2020 Alena Srishti
                        //if (Country_Code != 0) 
                        //{
                        //    var old_country = _entity.tb_Country.Where(x => x.Id == Country_Code).FirstOrDefault();
                        //    var new_country = _entity.tb_Country.Where(x => x.Id == model.country_id).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business country " + old_country.Country_Name + " to " + new_country.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", old_country.Country_Name, new_country.Country_Name);

                        //    string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        //}
                        //////if (Country_Code != 0)
                        //////{
                        //////    var old_country = _entity.tb_Country.Where(x => x.Id == Country_Code).FirstOrDefault();
                        //////    var new_country = _entity.tb_Country.Where(x => x.Id == model.country_id).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business country " + old_country.Country_Name + " to " + new_country.Country_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Edit", old_country.Country_Name, new_country.Country_Name);

                        //////    string record = data.Business_Code + "|| " + data.Business_Name + " || " + data.Bus_Manager + " || " + data.Bus_Controller + " || " + data.Bus_Office_Admin + " || " + data.Country_Id + "||" + data.IsActive + "||" + data.TimeStamp;
                        //////    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", data.Bus_Id.ToString(), "Admin");
                        //////}
                        #endregion 
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditBusiness(string id)
        {
            var permission = CheckAuth(id);
            BusinessModel model = new BusinessModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Business.Where(x => x.Bus_Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.business_id = data.Bus_Id;
                model.business_code = data.Business_Code;
                model.business = data.Business_Name;
                model.manager_id = data.Bus_Manager;
                model.controller_id = data.Bus_Controller;
                model.office_admin_id = data.Bus_Office_Admin;
                model.country_id = data.Country_Id;
                //18/03/2020 Alena Srishti
                model.country_name = data.tb_Country.Country_Name;

                model.isEdit = true;
            }
            return PartialView("~/Views/Admin/_pv_AddBusiness.cshtml", model);
        }
        public object DeleteBusiness(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            BusinessModel model = new BusinessModel();
            bool status = false;
            string msg = "Failed";
            int BUid = Convert.ToInt32(id);
            var data = _entity.tb_Business.Where(x => x.Bus_Id == BUid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Business deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " removed business " + data.Business_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Business", data.Bus_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        ///Aju
        ///26/11/
        ///
        public ActionResult BusinessLineHome(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                BusinessLineModel model = new BusinessLineModel();
                model.admin_employee_local_id = permission;
                model.businessLine_code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        BusinessLineModel model = new BusinessLineModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.businessLine_code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //        model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }
        public PartialViewResult BusinesslineList(string id)
        {
            BusinesslineList model = new BusinesslineList();
            model.list = new List<BusinessLineModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Businessline(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                BusinessLineModel one = new BusinessLineModel();
                one.BUL_Id = item.BL_Id;
                one.businessLine_code = item.BusinessLine_Code;
                one.businessLine_Name = item.Business_Line_Name;
                one.businessLine_Manager = item.Manager;
                one.businessLine_Controller = item.Controller;
                one.Business_Name = item.Business_Name;
                one.businessLine_OfficeAdmin = item.Office_Manager;
                one.country_name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_BusinesslineList.cshtml", model);
        }

        public object AddBusinessline(BusinessLineModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.BUL_Id == null || model.BUL_Id == 0)
            {
                #region Add
                if (_entity.tb_BusinessLine.Any(x => x.BusinessLine_Code == model.businessLine_code.Trim() && x.IsActive == true && x.BL_Id != model.BUL_Id))
                {
                    msg = "Businessline code already exits !";
                }
                else if (_entity.tb_BusinessLine.Any(x => x.Business_Line_Name == model.businessLine_Name.Trim() && x.IsActive == true && x.BL_Id != model.BUL_Id))
                {
                    msg = "Businessline already exits !";
                }
                else
                {
                    var bs = _entity.tb_BusinessLine.Create();
                    bs.BusinessLine_Code = model.businessLine_code;
                    bs.Business_Line_Name = model.businessLine_Name;
                    bs.Business_Id = model.businessid;
                    bs.BL_Manager = model.businessLine_Manager;
                    bs.BL_Controller = model.businessLine_Controller;
                    bs.BL_Office_Admin = model.businessLine_OfficeAdmin;
                    //bs.Country_Id = model.country_id;
                    // 18/03/2020 Alena Srishti
                    //   bs.tb_Business.Country_Id = model.country_id;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_BusinessLine.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Businessline added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added businessline " + model.businessLine_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", bs.BL_Id.ToString(), "Add");

                        string record = bs.BusinessLine_Code + " || " + bs.Business_Line_Name + " || " + bs.BL_Manager + " || " + bs.BL_Office_Admin + " || " + bs.Business_Id + "||" + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Business", bs.BL_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_BusinessLine.Any(x => x.BusinessLine_Code == model.businessLine_code && x.IsActive == true && x.BL_Id != model.BUL_Id))//&& x.Business_Line_Name==model.businessLine_Name 
                {
                    msg = "Businessline already exits !";
                }
                else
                {

                    long Business_id = 0;
                    string Manager_Code = "";
                    string Controller_Code = "";
                    string Office_Admin_Code = "";
                    long Country_Id = 0;
                    string bus_line_code = "";
                    string bus_line_name = "";


                    var data = _entity.tb_BusinessLine.Where(x => x.BL_Id == model.BUL_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Business_Id != model.businessid)
                    {
                        Business_id = data.Business_Id;
                    }
                    if (data.BL_Manager != model.businessLine_Manager)
                    {
                        Manager_Code = data.BL_Manager;
                    }
                    if (data.BL_Controller != model.businessLine_Controller)
                    {
                        Controller_Code = data.BL_Controller;
                    }
                    if (data.BL_Office_Admin != model.businessLine_OfficeAdmin)
                    {
                        Office_Admin_Code = data.BL_Office_Admin;
                    }
                    if (data.tb_Business.Country_Id != model.country_id)
                    {
                        Country_Id = data.tb_Business.Country_Id;
                    }
                    if (data.BusinessLine_Code != model.businessLine_code)
                    {
                        bus_line_code = data.BusinessLine_Code;
                    }
                    if (data.Business_Line_Name != model.businessLine_Name)
                    {
                        bus_line_name = data.Business_Line_Name;
                    }

                    #endregion
                    data.BusinessLine_Code = model.businessLine_code;
                    data.Business_Line_Name = model.businessLine_Name;
                    data.BL_Manager = model.businessLine_Manager;
                    data.BL_Controller = model.businessLine_Controller;
                    data.Business_Id = model.businessid;
                    data.BL_Office_Admin = model.businessLine_OfficeAdmin;
                    //  data.tb_Business.Country_Id = model.country_id; // 20/06/2020 ALENA
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Businessline Edit Sucessfully";
                        #region Keep Business Edit
                        if (Business_id != 0)
                        {
                            var old = _entity.tb_Business.Where(x => x.Bus_Id == Business_id && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_Business.Where(x => x.Bus_Id == model.businessid && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business name " + old.Business_Name + " to " + newData.Business_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", old.Business_Name, newData.Business_Name);

                            string record = data.BusinessLine_Code + " || " + data.Business_Line_Name + " || " + data.BL_Manager + " || " + data.BL_Office_Admin + " || " + data.Business_Id + "||" + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_BusinessLine", data.BL_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Country Edit // 20/06/2020 COMMENTED BY ALENA SICS
                        //if (Country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.country_id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);
                        //}
                        #endregion
                        //20-2-2020
                        #region Keep Manager Edit
                        if (Manager_Code != "")
                        {
                            var oldmanagercode = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Manager_Code && x.IsActive == true).FirstOrDefault();
                            var Newmanagercode = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.BL_Manager && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline manager " + oldmanagercode.Emp_Name + " to " + Newmanagercode.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", oldmanagercode.Emp_Name, Newmanagercode.Emp_Name);

                            string record = data.BusinessLine_Code + " || " + data.Business_Line_Name + " || " + data.BL_Manager + " || " + data.BL_Office_Admin + " || " + data.Business_Id + "||" + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_BusinessLine", data.BL_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep Controler edit
                        if (Controller_Code != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Controller_Code && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.BL_Controller && x.IsActive == true).FirstOrDefault();


                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline controller " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.BusinessLine_Code + " || " + data.Business_Line_Name + " || " + data.BL_Manager + " || " + data.BL_Office_Admin + " || " + data.Business_Id + "||" + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_BusinessLine", data.BL_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep officeAdmin edit
                        if (Office_Admin_Code != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Office_Admin_Code && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.BL_Office_Admin && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline office admin " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.BusinessLine_Code + " || " + data.Business_Line_Name + " || " + data.BL_Manager + " || " + data.BL_Office_Admin + " || " + data.Business_Id + "||" + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_BusinessLine", data.BL_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region business linename edit
                        if (bus_line_name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline name " + bus_line_name + " to " + model.businessLine_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", bus_line_name, model.businessLine_Name);

                            string record = data.BusinessLine_Code + " || " + data.Business_Line_Name + " || " + data.BL_Manager + " || " + data.BL_Office_Admin + " || " + data.Business_Id + "||" + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_BusinessLine", data.BL_Id.ToString(), "Admin");
                        }
                        #endregion Business linename edit
                        #region Business Linecode edit
                        if (bus_line_code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline code " + bus_line_code + " to " + model.businessLine_code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Edit", bus_line_code, model.businessLine_code);

                            string record = data.BusinessLine_Code + " || " + data.Business_Line_Name + " || " + data.BL_Manager + " || " + data.BL_Office_Admin + " || " + data.Business_Id + "||" + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_BusinessLine", data.BL_Id.ToString(), "Admin");
                        }
                        #endregion

                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //public object LoadBusinessByCountry(string id)
        public object LoadBusinessByCountry(string id)
        {

            bool status = false;
            string msg = "Failed";
            //            
            long Id1 = Convert.ToInt64(Session["countryname"]);
            var bus1 = WF_Tool.DataLibrary.Data.DropdownData.GetAllBusinessInCountry(Id1).ToList();
            //
            if (Id1 > 0)
            {
                status = true;
                msg = "Success";
                return Json(new { status = status, msg = msg, list = bus1 }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //
                // 16/06/2020 commented below code and added new code below
                long Id = Convert.ToInt64(id);
                ////long s =Convert.ToInt64( id);
                ////    var input = _entity.tb_Country.Where(x => x.Id == s).FirstOrDefault();
                ///    long Id =Convert.ToInt64( input.Id);
                var bus = WF_Tool.DataLibrary.Data.DropdownData.GetAllBusinessInCountry(Id).ToList();
                if (bus.Count > 0 && bus != null)
                {
                    status = true;
                    msg = "Success";
                    return Json(new { status = status, msg = msg, list = bus }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditBusinessline(string id)
        {
            var permission = CheckAuth(id);
            BusinessLineModel model = new BusinessLineModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_BusinessLine.Where(x => x.BL_Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.BUL_Id = data.BL_Id;
                model.businessLine_code = data.BusinessLine_Code;
                model.businessLine_Name = data.Business_Line_Name;
                model.businessLine_Manager = data.BL_Manager;
                model.businessLine_Controller = data.BL_Controller;
                model.businessLine_OfficeAdmin = data.BL_Office_Admin;
                model.businessid = data.Business_Id;
                model.country_id = data.tb_Business.Country_Id;
                //    18/03/2020 code by Alena Srishti start
                model.Business_Name = data.tb_Business.Business_Name;
                model.country_name = data.tb_Business.tb_Country.Country_Name;  //end
                model.isEdit = true;
                //
                Session["countryname"] = data.tb_Business.Country_Id;
                //
            }
            return PartialView("~/Views/Admin/_pv_AddBusinessline.cshtml", model);
        }
        public object DeleteBusinessline(string id)
        {
            var permission = CheckAuth(id);
            BusinessLineModel model = new BusinessLineModel();
            bool status = false;
            string msg = "Failed";
            int BLid = Convert.ToInt32(id);
            var data = _entity.tb_BusinessLine.Where(x => x.BL_Id == BLid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "BusinessLine deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed businessline  " + data.Business_Line_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_BusinessLine", data.BL_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ProductGroup
        /// Aju 27-11-2019
        /// 

        public ActionResult PrGHome(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                PrGModel model = new PrGModel();
                model.admin_employee_local_id = permission;
                model.PrG_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        PrGModel model = new PrGModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.PrG_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //        model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }
        public PartialViewResult PrGList(string id)
        {
            PrGList model = new PrGList();
            model.list = new List<PrGModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_PrG(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                PrGModel one = new PrGModel();
                one.PrG_Id = item.PG_Id;
                one.PrG_Code = item.PG_Code;
                one.PrG_Name = item.PG_Name;
                one.PrG_Manager = item.Manager;
                one.PrG_office_admin = item.Office_Manager;
                one.PrG_Controller = item.Controller;
                one.Business_Line_Name = item.Business_Line_Name;
                one.Business_Name = item.Business_Name;
                one.Country_Name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_PrGpList.cshtml", model);
        }
        public object AddPrG(PrGModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.PrG_Id == null || model.PrG_Id == 0)
            {
                #region Add
                if (_entity.tb_ProductGroup.Any(x => x.PG_Code == model.PrG_Code.Trim() && x.IsActive == true && x.PG_Id != model.PrG_Id))
                {
                    msg = "PG code already exits !";
                }
                else if (_entity.tb_ProductGroup.Any(x => x.PG_Name == model.PrG_Name.Trim() && x.IsActive == true && x.PG_Id != model.PrG_Id))
                {
                    msg = "PG already exits !";
                }
                else
                {
                    var bs = _entity.tb_ProductGroup.Create();
                    bs.BusinessLine_Id = model.Businessline_Id;
                    //bs.Business_Id = model.Business_Id;
                    bs.PG_Code = model.PrG_Code;
                    bs.PG_Name = model.PrG_Name;
                    bs.PG_Manager = model.PrG_Manager;
                    bs.PG_Controller = model.PrG_Controller;
                    bs.PG_Office_Admin = model.PrG_office_admin;
                    //bs.Country_Id = model.Country_Id;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_ProductGroup.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "PG added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added productgroup " + model.PrG_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", bs.PG_Id.ToString(), "Add");

                        string record = bs.PG_Code + " || " + bs.PG_Name + " || " + bs.PG_Manager + " || " + bs.PG_Controller + " || " + bs.PG_Office_Admin + " || " + bs.BusinessLine_Id + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", bs.PG_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_ProductGroup.Any(x => x.PG_Code == model.PrG_Code && x.IsActive == true && x.PG_Id != model.PrG_Id))
                {
                    msg = "PG already exits !";
                }
                else
                {
                    long Business_Id = 0;
                    long BusinessLine_Id = 0;
                    string PG_Manager = "";
                    string PG_Controller = "";
                    string PG_Office_Admin = "";
                    long Country_Id = 0;
                    string PG_Name = "";
                    string PG_Code = "";

                    var data = _entity.tb_ProductGroup.Where(x => x.PG_Id == model.PrG_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.tb_BusinessLine.Business_Id != model.Business_Id)
                    {
                        Business_Id = data.tb_BusinessLine.Business_Id;
                    }
                    if (data.BusinessLine_Id != model.Businessline_Id)
                    {
                        BusinessLine_Id = data.BusinessLine_Id;
                    }
                    if (data.PG_Manager != model.PrG_Manager)
                    {
                        PG_Manager = data.PG_Manager;
                    }
                    if (data.PG_Controller != model.PrG_Controller)
                    {
                        PG_Controller = data.PG_Controller;
                    }
                    if (data.PG_Office_Admin != model.PrG_office_admin)
                    {
                        PG_Office_Admin = data.PG_Office_Admin;
                    }
                    if (data.tb_BusinessLine.tb_Business.Country_Id != model.Country_Id)
                    {
                        Country_Id = data.tb_BusinessLine.tb_Business.Country_Id;
                    }
                    if (data.PG_Name != model.PrG_Name)
                    {
                        PG_Name = data.PG_Name;
                    }
                    if (data.PG_Code != model.PrG_Code)
                    {
                        PG_Code = data.PG_Code;
                    }
                    #endregion
                    data.PG_Code = model.PrG_Code;
                    data.PG_Name = model.PrG_Name;
                    data.PG_Manager = model.PrG_Manager;
                    // 19/03/2020 Alena Srishti start
                    //     data.tb_BusinessLine.Business_Id = model.Business_Id;   
                    //      data.BusinessLine_Id = model.Businessline_Id;   //end
                    //////data.tb_BusinessLine.Business_Id = model.Business_Id;
                    //////data.BusinessLine_Id = model.Businessline_Id;
                    data.PG_Controller = model.PrG_Controller;
                    data.PG_Id = model.PrG_Id;
                    data.PG_Office_Admin = model.PrG_office_admin;
                    // 19/03/2020 Alena Srishti start
                    //        data.tb_BusinessLine.tb_Business.Country_Id = model.Country_Id; end
                    //////data.tb_BusinessLine.tb_Business.Country_Id = model.Country_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "PG Edit Sucessfully";
                        #region Keep Business ID Edit
                        // 19/03/2020  below line commented by Alena Srishti
                        //if (Business_Id != 0)
                        //{
                        //    var old = _entity.tb_Business.Where(x => x.Bus_Id == Business_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Business.Where(x => x.Bus_Id == model.Business_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited Business name " + old.Business_Name + " to " + newData.Business_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Business_Name, newData.Business_Name);
                        //}
                        //////if (Business_Id != 0)
                        //////{
                        //////    var old = _entity.tb_Business.Where(x => x.Bus_Id == Business_Id && x.IsActive == true).FirstOrDefault();
                        //////    var newData = _entity.tb_Business.Where(x => x.Bus_Id == model.Business_Id && x.IsActive == true).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited Business name " + old.Business_Name + " to " + newData.Business_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Business_Name, newData.Business_Name);
                        //////}
                        #endregion
                        #region Keep BusinessLine Edit
                        // 19/03/2020  below line commented by Alena Srishti
                        //if (BusinessLine_Id != 0)
                        //{
                        //    var old = _entity.tb_BusinessLine.Where(x => x.BL_Id == BusinessLine_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_BusinessLine.Where(x => x.BL_Id == model.Businessline_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline name " + old.Business_Line_Name + " to " + newData.Business_Line_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Business_Line_Name, newData.Business_Line_Name);

                        //    string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        //}
                        //////if (BusinessLine_Id != 0)
                        //////{
                        //////    var old = _entity.tb_BusinessLine.Where(x => x.BL_Id == BusinessLine_Id && x.IsActive == true).FirstOrDefault();
                        //////    var newData = _entity.tb_BusinessLine.Where(x => x.BL_Id == model.Businessline_Id && x.IsActive == true).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline name " + old.Business_Line_Name + " to " + newData.Business_Line_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Business_Line_Name, newData.Business_Line_Name);

                        //////    string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //////    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        //////}
                        #endregion
                        #region Keep Country Edit
                        // 19/03/2020  below line commented by Alena Srishti
                        //if (Country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);
                        //}
                        //////if (Country_Id != 0)
                        //////{
                        //////    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //////    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);
                        //////}
                        #endregion
                        #region Keep PG Manager Edit
                        if (PG_Manager != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == PG_Manager && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.PG_Manager && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group manager " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep PGControler edit
                        if (PG_Controller != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == PG_Controller && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.PG_Controller && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group controller " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep PG_officeAdmin edit
                        if (PG_Office_Admin != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == PG_Office_Admin && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.PG_Office_Admin && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group office admin " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region PG name edit
                        if (PG_Name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group name " + PG_Name + " to " + model.PrG_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", PG_Name, model.PrG_Name);

                            string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region PG Linecode edit
                        if (PG_Code != "")
                        {

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group code " + PG_Code + " to " + model.PrG_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Edit", PG_Code, model.PrG_Code);

                            string record = data.PG_Code + " || " + data.PG_Name + " || " + data.PG_Manager + " || " + data.PG_Controller + " || " + data.PG_Office_Admin + " || " + data.BusinessLine_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_ProductGroup", data.PG_Id.ToString(), "Admin");
                        }
                        #endregion

                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public object PGLoadBusinessByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var bus = WF_Tool.DataLibrary.Data.DropdownData.GetAllBusinessInCountry(Id).ToList();
            //var bus = _entity.tb_Business.Where(x => x.Country_Id == Id && x.IsActive == true).ToList();
            if (bus.Count > 0 && bus != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = bus }, JsonRequestBehavior.AllowGet);
        }
        public object LoadBusinessLineByBusiness(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var bus_line = WF_Tool.DataLibrary.Data.DropdownData.GetAllBusinessLineInCountry(Id).ToList();
            if (bus_line.Count > 0 && bus_line != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = bus_line }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult EditPG(string id)
        {
            var permission = CheckAuth(id);
            PrGModel model = new PrGModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_ProductGroup.Where(x => x.PG_Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.PrG_Id = data.PG_Id;
                model.PrG_Code = data.PG_Code;
                model.PrG_Name = data.PG_Name;
                model.PrG_Manager = data.PG_Manager;
                model.PrG_Controller = data.PG_Controller;
                model.PrG_office_admin = data.PG_Office_Admin;
                model.Country_Id = data.tb_BusinessLine.tb_Business.Country_Id;
                model.Business_Id = data.tb_BusinessLine.Business_Id;
                model.Businessline_Id = data.BusinessLine_Id;
                //   19/03/2020 Alena Srishti   start
                model.Country_Name = data.tb_BusinessLine.tb_Business.tb_Country.Country_Name;  //Country name
                model.Business_Name = data.tb_BusinessLine.tb_Business.Business_Name;          //Business name
                model.Business_Line_Name = data.tb_BusinessLine.Business_Line_Name;           //Business line name     //end

                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddPrG.cshtml", model);
        }

        public object DeletePrG(string id)
        {
            var permission = CheckAuth(id);//26-02-2020
            PrGModel model = new PrGModel();
            bool status = false;
            string msg = "Failed";
            int PGid = Convert.ToInt32(id);
            var data = _entity.tb_ProductGroup.Where(x => x.PG_Id == PGid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Product group deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + " - " + " removed product group " + data.PG_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_ProductGroup", data.PG_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        ///Department
        ///27-11
        ///
        public ActionResult DepartmentHome(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                DepartmentModel model = new DepartmentModel();
                model.admin_employee_local_id = permission;
                model.Dep_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        DepartmentModel model = new DepartmentModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Dep_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }
        public object AddDepartment(DepartmentModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Department
                if (_entity.tb_Department.Any(x => x.Department_Code == model.Dep_Code.Trim() && x.IsActive == true))
                {
                    msg = "Department is already exits !";
                }
                else if (_entity.tb_Department.Any(x => x.Department_Name == model.Dep_Name.Trim() && x.IsActive == true))
                {
                    msg = "Department is already exits !";
                }
                else
                {
                    var bs = _entity.tb_Department.Create();
                    bs.Department_Code = model.Dep_Code;
                    bs.Department_Name = model.Dep_Name;
                    bs.Dept_Manager = model.Dep_Manager;
                    bs.Dept_Controller = model.Dep_Controller;
                    bs.Dep_Office_Admin = model.Dep_Office_Admin;
                    bs.PG_Id = model.PG_Id;
                    //bs.Country_Id = model.Country_Id;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Department.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Department added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added department " + model.Dep_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", bs.Department_Id.ToString(), "Add");

                        string record = bs.Department_Code + " || " + bs.Department_Name + " || " + bs.Dept_Manager + " || " + bs.Dep_Office_Admin + " || " + bs.Dept_Controller + " || " + bs.PG_Id + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", bs.Department_Id.ToString(), "Admin");

                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Department.Any(x => x.Department_Name == model.Dep_Name && x.IsActive == true && x.Department_Code != model.Dep_Code && x.Department_Id != model.Dep_Id))
                {
                    msg = "Department already exits !";
                }
                else
                {
                    string Dept_Code = "";
                    string Dept_Name = "";
                    string Dept_Manager = "";
                    string Dept_Controller = "";
                    string Dept_Office_Admin = "";
                    long country_Id = 0;
                    long PG_Id = 0;

                    var data = _entity.tb_Department.Where(x => x.Department_Id == model.Dep_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    // 20/03/2020 commented below line and add a new line Alena Srishti
                    //if (data.Department_Code != model.Dep_Code)
                    //{
                    //    Dept_Code = model.Dep_Code;
                    //}
                    if (data.Department_Code != model.Dep_Code)
                    {
                        Dept_Code = data.Department_Code;
                    }   //    end
                    //////if (data.Department_Code != model.Dep_Code)
                    //////{
                    //////    Dept_Code = model.Dep_Code;
                    //////}
                    if (data.Department_Name != model.Dep_Name)
                    {
                        Dept_Name = data.Department_Name;
                    }
                    if (data.Dept_Manager != model.Dep_Manager)
                    {
                        Dept_Manager = data.Dept_Manager;
                    }
                    if (data.Dept_Controller != model.Dep_Controller)
                    {
                        Dept_Controller = data.Dept_Controller;
                    }
                    if (data.Dep_Office_Admin != model.Dep_Office_Admin)
                    {
                        Dept_Office_Admin = data.Dep_Office_Admin;
                    }
                    //                  19/03/2020 Alena Srishti commented below line
                    //if (data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id != model.Country_Id)
                    //{
                    //    country_Id = data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id;
                    //}
                    //////if (data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id != model.Country_Id)
                    //////{
                    //////    country_Id = data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id;
                    //////}
                    if (data.PG_Id != model.PG_Id)
                    {
                        PG_Id = data.PG_Id;
                    }
                    #endregion
                    data.Department_Code = model.Dep_Code;
                    data.Department_Name = model.Dep_Name;
                    data.Dept_Manager = model.Dep_Manager;
                    data.Dept_Controller = model.Dep_Controller;
                    data.Dep_Office_Admin = model.Dep_Office_Admin;
                    // 19/03/2020 commented by Alena Srishti
                    //  data.PG_Id = model.PG_Id;
                    //  data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id = model.Country_Id;
                    //////data.PG_Id = model.PG_Id;
                    //////data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id = model.Country_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Department Edit Sucessfully";
                        #region Keep Department Code  Edit
                        if (Dept_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited department code " + Dept_Code + " to " + model.Dep_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", Dept_Code, model.Dep_Code);

                            string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Department Name  Edit
                        if (Dept_Name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited department name " + Dept_Name + " to " + model.Dep_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", Dept_Name, model.Dep_Name);

                            string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        }
                        #endregion
                        //20-02-2020
                        #region Keep Department Manager  Edit
                        if (Dept_Manager != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Dept_Manager && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Dept_Manager && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited department manager " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Department Controller Edit
                        if (Dept_Controller != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Dept_Controller && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Dept_Controller && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited department controller " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Department office Admin Edit
                        if (Dept_Office_Admin != "")
                        {
                            var old = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Dept_Office_Admin && x.IsActive == true).FirstOrDefault();
                            var New = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Dep_Office_Admin && x.IsActive == true).FirstOrDefault();

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited department office admin " + old.Emp_Name + " to " + New.Emp_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.Emp_Name, New.Emp_Name);

                            string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep PG Code edit
                        // 19/03/2020 commented by Alena Srishti
                        //if (PG_Id != 0)
                        //{
                        //    var old = _entity.tb_ProductGroup.Where(x => x.PG_Id == PG_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_ProductGroup.Where(x => x.PG_Id == model.PG_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group name" + old.PG_Name + " to " + newData.PG_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.PG_Name, newData.PG_Name);

                        //    string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        //}
                        //////if (PG_Id != 0)
                        //////{
                        //////    var old = _entity.tb_ProductGroup.Where(x => x.PG_Id == PG_Id && x.IsActive == true).FirstOrDefault();
                        //////    var newData = _entity.tb_ProductGroup.Where(x => x.PG_Id == model.PG_Id && x.IsActive == true).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group name" + old.PG_Name + " to " + newData.PG_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.PG_Name, newData.PG_Name);

                        //////    string record = data.Department_Code + " || " + data.Department_Name + " || " + data.Dept_Manager + " || " + data + Dept_Controller + " || " + data.Dep_Office_Admin + " || " + data.PG_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //////    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Department", data.Department_Id.ToString(), "Admin");
                        //////}
                        #endregion
                        #region keep Country Code edit
                        // 19/03/2020 commented by Alena Srishti
                        //if (country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);
                        //}
                        //////if (country_Id != 0)
                        //////{
                        //////    var old = _entity.tb_Country.Where(x => x.Id == country_Id && x.IsActive == true).FirstOrDefault();
                        //////    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);
                        //////}
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult DepartmentList(string id)
        {
            DepartmentList model = new DepartmentList();
            model.list = new List<DepartmentModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Department(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                DepartmentModel one = new DepartmentModel();
                one.Dep_Id = item.Department_Id;
                one.Dep_Code = item.Department_Code;
                one.Dep_Name = item.Department_Name;
                one.Dep_Manager = item.Manager;
                one.Dep_Controller = item.Controller;
                one.Dep_Office_Admin = item.Office_Manager;
                one.PG_Name = item.PG_Name;
                one.Country_Name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_DepartmentList.cshtml", model);
        }
        public object DepLoadPrGByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var PG = WF_Tool.DataLibrary.Data.DropdownData.GetAllPGInCountry(Id).ToList();
            //var bus = _entity.tb_ProductGroup.Where(x => x.tb_BusinessLine.tb_Business.Country_Id == Id && x.IsActive == true).ToList();
            if (PG.Count > 0 && PG != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = PG }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Dep_Edit(string Id)
        {
            var permission = CheckAuth(Id);
            DepartmentModel model = new DepartmentModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long DEId = Convert.ToInt64(Id);
            var data = _entity.tb_Department.Where(x => x.Department_Id == DEId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Dep_Code = data.Department_Code;
                model.Dep_Id = data.Department_Id;
                model.Dep_Name = data.Department_Name;
                model.Dep_Manager = data.Dept_Manager;
                model.Dep_Controller = data.Dept_Controller;
                model.Dep_Office_Admin = data.Dep_Office_Admin;
                model.Country_Id = data.tb_ProductGroup.tb_BusinessLine.tb_Business.Country_Id;
                model.PG_Id = data.tb_ProductGroup.PG_Id;
                //19/03/2020 Alena Srishti

                model.Country_Name = data.tb_ProductGroup.tb_BusinessLine.tb_Business.tb_Country.Country_Name;
                model.PG_Name = data.tb_ProductGroup.PG_Name; //end
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_1AddDepartment.cshtml", model);
        }
        public object DeleteDep(string id)
        {
            var permission = CheckAuth(id);//26-02-2020
            DepartmentModel model = new DepartmentModel();
            bool status = false;
            string msg = "Failed";
            int Depid = Convert.ToInt32(id);
            var data = _entity.tb_Department.Where(x => x.Department_Id == Depid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Department deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + "- " + model.admin_employee_local_id + "- " + " removed department  " + data.Department_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Department", data.Department_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }


        /// Aju
        /// officelocation
        ///27-11-2019
        ///wrk
        public ActionResult Location_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                LocationModel model = new LocationModel();
                model.admin_employee_local_id = permission;
                model.location_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        LocationModel model = new LocationModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.location_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddLocation(LocationModel model)
        {

            bool status = false;
            string msg = "Failed";
            //if (model.Dep_Code == null || model.Dep_Code == "")
            if (model.isEdit == false)
            {
                #region Add Location
                if (_entity.tb_Location.Any(x => x.Location_Code == model.location_Code.Trim() && x.IsActive == true && x.Location_Id != model.Location_Id))
                {
                    msg = "Location  already exits !";
                }
                else if (_entity.tb_Location.Any(x => x.Location == model.location_name.Trim() && x.IsActive == true && x.Location_Id != model.Location_Id))
                {
                    msg = "Location  already exits !";
                }
                else
                {
                    var bs = _entity.tb_Location.Create();
                    bs.Location_Code = model.location_Code;
                    bs.Location = model.location_name;
                    bs.Country_Id = model.Country_Id;
                    //06/04/2020 Alena Sics
                    bs.Region = model.region;   //end
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Location.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Location added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added location " + model.location_name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Location", bs.Location_Id.ToString(), "Add");

                        string record = bs.Location_Code + " || " + bs.Location + " || " + bs.Country_Id + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Location", bs.Location_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Location.Any(x => x.Location_Code == model.location_Code && x.IsActive == true && x.Location_Id != model.Location_Id))
                {
                    msg = "Location already exits !";
                }
                else
                {
                    string Location_Code = "";
                    string Loacation_name = "";
                    long country_Id = 0;
                    //06/04/2020 Alena Sics
                    string Region = ""; //end

                    var data = _entity.tb_Location.Where(x => x.Location_Id == model.Location_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Location_Code != model.location_Code)
                    {
                        Location_Code = data.Location_Code;
                    }
                    if (data.Location != model.location_name)
                    {
                        Loacation_name = data.Location;
                    }
                    if (data.Country_Id != model.Country_Id)
                    {
                        country_Id = data.Country_Id ?? 0;
                    }
                    //06/04/2020 Alena Sics
                    if (data.Region != model.region)
                    {
                        Region = data.Region;
                    } //end
                    #endregion
                    data.Location_Code = model.location_Code;
                    data.Location = model.location_name;
                    // 18/04/2020 Alena 
                    data.Region = model.region; //end
                    // 23/03/2020 commented by Alena Srishti
                    //data.Country_Id = model.Country_Id; /end
                    //////data.Country_Id = model.Country_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Location Edit Sucessfully";
                        #region Keep Region Edit
                        // 18/04/2020 below code by Alena
                        if (Region != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited region " + Region + " to " + model.region + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Location", data.Location_Id.ToString(), "Edit", Region, model.region);

                            string record = data.Location_Code + " || " + data.Location + " || " + data.Country_Id + " || " + data.Region + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Location", data.Location_Id.ToString(), "Admin");
                        } //end
                        #endregion
                        #region Keep Location Code Edit
                        if (Location_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited location code " + Location_Code + " to " + model.location_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Location", data.Location_Id.ToString(), "Edit", Location_Code, model.location_name);

                            string record = data.Location_Code + " || " + data.Location + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Location", data.Location_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Location Edit
                        if (Loacation_name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited location name " + Loacation_name + " to " + model.location_name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Location", data.Location_Id.ToString(), "Edit", Loacation_name, model.location_name);

                            string record = data.Location_Code + " || " + data.Location + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Location", data.Location_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep Country Code edit
                        // 23/03/2020 commented by Alena Srishti
                        //if (country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", data.Location_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);

                        //    string record = data.Location_Code + " || " + data.Location + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Location", data.Location_Id.ToString(), "Admin");
                        //}
                        //////if (country_Id != 0)
                        //////{
                        //////    var old = _entity.tb_Country.Where(x => x.Id == country_Id && x.IsActive == true).FirstOrDefault();
                        //////    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //////    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //////    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Country", data.Location_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);

                        //////    string record = data.Location_Code + " || " + data.Location + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //////    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Location", data.Location_Id.ToString(), "Admin");
                        //////}
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult LocationList(string id)
        {
            LocationList model = new LocationList();
            model.list = new List<LocationModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_location(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                LocationModel one = new LocationModel();
                one.Location_Id = item.Location_Id;
                one.location_Code = item.Location_Code;
                one.location_name = item.Location;
                one.Country_Name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_LocationList.cshtml", model);
        }

        public PartialViewResult Location_Edit(string Id)
        {
            var permission = CheckAuth(Id);
            LocationModel model = new LocationModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long LoId = Convert.ToInt64(Id);
            var data = _entity.tb_Location.Where(x => x.Location_Id == LoId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Location_Id = data.Location_Id;
                model.location_Code = data.Location_Code;
                model.location_name = data.Location;
                model.Country_Id = data.Country_Id ?? 0;
                //  23/03/2020   Alena Srishti
                model.Country_Name = data.tb_Country.Country_Name; //end
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddLocation.cshtml", model);
        }

        public object DeleteLocation(string id)
        {
            var permission = CheckAuth(id);//26-20-2020
            LocationModel model = new LocationModel();
            bool status = false;
            string msg = "Failed";
            int Loid = Convert.ToInt32(id);
            var data = _entity.tb_Location.Where(x => x.Location_Id == Loid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Location deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + "- " + model.admin_employee_local_id + "- " + " removed location  " + data.Location + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Location", data.Location_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        ///vendor
        ///28-11-2019
        ///
        public ActionResult Vendor_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                VendorModel model = new VendorModel();
                model.admin_employee_local_id = permission;
                model.Vendor_code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        VendorModel model = new VendorModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Vendor_code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }
        public object AddVendor(VendorModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.Vendor_id == null || model.Vendor_id == 0)
            {
                #region Add
                if (_entity.tb_Vendor.Any(x => x.Vendor_Code == model.Vendor_code.Trim() && x.IsActive == true && x.Vendor_id != model.Vendor_id))
                {
                    msg = "Vendor already exits !";
                }
                else if (_entity.tb_Vendor.Any(x => x.Vendor_Name == model.V_Name.Trim() && x.IsActive == true && x.Vendor_id != model.Vendor_id))
                {
                    msg = "Vendor already exits !";
                }
                else
                {
                    var bs = _entity.tb_Vendor.Create();
                    bs.Vendor_Code = model.Vendor_code;
                    bs.Vendor_Name = model.V_Name;
                    bs.Email = model.V_email;
                    bs.Contact_No = model.v_contact;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Vendor.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Vendor added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added vendor " + model.V_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Vendor", bs.Vendor_id.ToString(), "Add");

                        string record = bs.Vendor_Code + " || " + bs.Vendor_Name + " || " + bs.Email + " || " + bs.Contact_No + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Vendor", bs.Vendor_id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Vendor.Any(x => x.Vendor_Name == model.V_Name && x.IsActive == true && x.Vendor_id != model.Vendor_id))
                {
                    msg = "Vendor already exits !";
                }
                else
                {

                    string Vd_Code = "";
                    string Vd_name = "";
                    string Vd_Email = "";
                    string Condact = "";


                    var data = _entity.tb_Vendor.Where(x => x.Vendor_id == model.Vendor_id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Vendor_Code != model.Vendor_code)
                    {
                        Vd_Code = data.Vendor_Code;
                    }
                    if (data.Vendor_Name != model.V_Name)
                    {
                        Vd_name = data.Vendor_Name;
                    }
                    if (data.Email != model.V_email)
                    {
                        Vd_Email = data.Email;
                    }
                    if (data.Contact_No != model.v_contact)
                    {
                        Condact = data.Contact_No;
                    }

                    #endregion
                    data.Vendor_Code = model.Vendor_code;
                    data.Vendor_Name = model.V_Name;
                    data.Email = model.V_email;
                    data.Vendor_id = model.Vendor_id;
                    data.Contact_No = model.v_contact;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Vendor Edit Sucessfully";
                        #region Keep Vendor code Edit
                        if (Vd_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited vendor code " + Vd_Code + " to " + model.Vendor_code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Vendor", data.Vendor_id.ToString(), "Edit", Vd_name, model.V_Name);

                            string record = data.Vendor_Code + " || " + data.Vendor_Name + " || " + data.Email + " || " + data.Contact_No + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Vendor", data.Vendor_id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Vendor Name Edit
                        if (Vd_name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited vendor name " + Vd_name + " to " + model.V_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Vendor", data.Vendor_id.ToString(), "Edit", Vd_name, model.V_Name);

                            string record = data.Vendor_Code + " || " + data.Vendor_Name + " || " + data.Email + " || " + data.Contact_No + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Vendor", data.Vendor_id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Email Edit
                        if (Vd_Email != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited vendor email " + Vd_Email + " to " + model.V_email + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Vendor", data.Vendor_id.ToString(), "Edit", Vd_Email, model.V_email);

                            string record = data.Vendor_Code + " || " + data.Vendor_Name + " || " + data.Email + " || " + data.Contact_No + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Vendor", data.Vendor_id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Contact Edit
                        if (Condact != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited contact number " + Condact + " to " + model.v_contact + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Vendor", data.Vendor_id.ToString(), "Edit", Condact, model.v_contact);

                            string record = data.Vendor_Code + " || " + data.Vendor_Name + " || " + data.Email + " || " + data.Contact_No + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Vendor", data.Vendor_id.ToString(), "Admin");
                        }
                        #endregion

                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Vendor_list(string id)
        {
            Vendorlist model = new Vendorlist();
            model.list = new List<VendorModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Vendor(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                VendorModel one = new VendorModel();
                one.Vendor_code = item.Vendor_Code;
                one.V_Name = item.Vendor_Name;
                one.V_email = item.Email;
                one.v_contact = item.Contact_No;
                one.slNo = Convert.ToString(slNo);
                one.Vendor_id = item.Vendor_id;
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_Vendor_list.cshtml", model);
        }

        public PartialViewResult Vendor_Edit(string id)
        {
            var permission = CheckAuth(id);
            VendorModel model = new VendorModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Vendor.Where(x => x.Vendor_id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Vendor_id = data.Vendor_id;
                model.Vendor_code = data.Vendor_Code;
                model.V_Name = data.Vendor_Name;
                model.V_email = data.Email;
                model.v_contact = data.Contact_No;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddVendor.cshtml", model);
        }

        public object DeleteVendor(string id)
        {
            var permission = CheckAuth(id);
            VendorModel model = new VendorModel();//26-02-2020
            bool status = false;
            string msg = "Failed";
            int vendorid = Convert.ToInt32(id);
            var data = _entity.tb_Vendor.Where(x => x.Vendor_id == vendorid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Vendor deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed vendor  " + data.Vendor_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Vendor", data.Vendor_id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        /// Company Details
        /// 28-11-2019
        /// 
        public ActionResult Company_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                CompanyModel model = new CompanyModel();
                model.admin_employee_local_id = permission;
                model.Comapny_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }


            else
            {
                return RedirectToAction("Home", "Account");
            }
        }

        public object AddCompany(CompanyModel model)
        {

            bool status = false;
            string msg = "Failed";
            // 22/04/2020 below code commented by Alena Sics
            //long companyId = 0; 
            // 23/04/2020 below code commented by Alena Sics and created new code below //start
            //    if (model.isEdit == false || model.Company_Id == 0)
            if (model.Company_Id == 0)                                            //end
                                                                                  //////long companyId = 0;
                                                                                  //////if (model.isEdit == false || model.Company_Id == 0)
            {
                //Session["companyid"] = model.Company_Id; // 22/04/2020 by Alena Sics
                #region Add Company
                // 22/04/2020 below code commented by Alena Sics and created new code below //start
                //if (_entity.tb_Company.Any(x => x.Company_Code == model.Comapny_Code.Trim() && x.IsActive == true && x.Company_Id != model.Company_Id))
                //{
                //    msg = "Company  already exits !";
                //}
                //else if (_entity.tb_Company.Any(x => x.Company_Name == model.Company_name.Trim() && x.IsActive == true && x.Company_Id != model.Company_Id))
                //{
                //    msg = "Company  already exits !";
                //}
                if (_entity.tb_Company.Any(x => x.Company_Code == model.Comapny_Code.Trim() && x.Company_Name == model.Company_name.Trim() && x.IsActive == true && x.Company_Id != model.Company_Id))
                {
                    msg = "Company  already exits !";
                } //end
                  // 23/04/2020 code commented by Alena and created new code //start
                  // else 
                else if (model.Company_Id == 0)  //end
                {
                    var bs = _entity.tb_Company.Create();
                    bs.Company_Code = model.Comapny_Code;
                    bs.Company_Name = model.Company_name;
                    bs.Payroll_code = model.payrol;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Company.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    // 16/06/2020 ALENA ADDED NEW CODE HERE
                    Session["companyid"] = bs.Company_Id;
                    // 22/04/2020 code commented by Alena Sics
                    //      var company = _entity.tb_Company.OrderByDescending(x => x.Company_Id).First();
                    //companyId = company.Company_Id; //end
                    //////var company = _entity.tb_Company.OrderByDescending(x => x.Company_Id).First();
                    //////companyId = company.Company_Id;
                    if (status)
                    {
                        msg = "Company added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added company " + model.Company_name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", bs.Company_Id.ToString(), "Add");

                        string record = bs.Company_Code + " || " + bs.Company_Name + " || " + bs.Payroll_code + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Company", bs.Company_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Company.Any(x => x.Company_Name == model.Company_name && x.IsActive == true && x.Company_Code != model.Comapny_Code && x.Company_Id != model.Company_Id))
                {
                    msg = "Company already exits !";
                }
                else
                {
                    string Company_Code = "";
                    string Company_name = "";
                    long payrole = 0;


                    var data = _entity.tb_Company.Where(x => x.Company_Id == model.Company_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Company_Code != model.Comapny_Code)
                    {
                        Company_Code = data.Company_Code;
                    }
                    if (data.Company_Name != model.Company_name)
                    {
                        Company_name = data.Company_Name;
                    }
                    if (data.Payroll_code != model.payrol)
                    {
                        payrole = data.Payroll_code ?? 0;
                    }

                    #endregion
                    data.Company_Code = model.Comapny_Code;
                    data.Company_Name = model.Company_name;
                    data.Payroll_code = model.payrol;
                    // 22/04/2020 code commented by Alena sics
                    //companyId = data.Company_Id;
                    //////companyId = data.Company_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Company Edit Sucessfully";
                        #region Keep Compoany Code Edit
                        if (Company_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited company code " + Company_Code + " to " + model.Comapny_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", data.Company_Id.ToString(), "Edit", Company_Code, model.Comapny_Code);

                            string record = data.Company_Code + " || " + data.Company_Name + " || " + data.Payroll_code + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Company", data.Company_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Company Edit
                        if (Company_name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited company name " + Company_name + " to " + model.Company_name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", data.Company_Id.ToString(), "Edit", Company_name, model.Company_name);

                            string record = data.Company_Code + " || " + data.Company_Name + " || " + data.Payroll_code + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Company", data.Company_Id.ToString(), "Admin");
                        }
                        #endregion

                        #region Keep payrole Edit
                        if (payrole != 0)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited payrolle " + payrole + " to " + model.payrol + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", data.Company_Id.ToString(), "Edit", payrole.ToString(), model.payrol.ToString());

                            string record = data.Company_Code + " || " + data.Company_Name + " || " + data.Payroll_code + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Company", data.Company_Id.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = " Changed ";
                }
                #endregion
            }
            // 22/04/2020 below code commented and created a new code below by Alena Sics
            //return Json(new { status = status, msg = msg, companyId = companyId }, JsonRequestBehavior.AllowGet); 
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
            ////// return Json(new { status = status, msg = msg, companyId = companyId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddcompanyAddress(long? compnayId, List<AddressData> Addressdata)
        {
            // 22/04/2020 code by Alena Sics
            string msg = "Failed";   //end
            AddressData model = new AddressData();

            foreach (var address in Addressdata)
            {
                var addressData = _entity.tb_CompanyAddress.Where(x => x.companyid == address.addrsssid).FirstOrDefault();
                if (addressData != null)
                {
                    if (address.address1 != addressData.CmpAddress1)
                    {
                        #region Keep Log
                        string content = "Admin " + Session["username"] + " added company addressline1 " + model.address1 + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", address.address1.ToString(), "Add");


                        #endregion Keep Log
                    }
                    if (address.address2 != addressData.CmpAddress2)
                    {
                        #region Keep Log
                        string content = "Admin " + Session["username"] + " added company addressline2 " + model.address2 + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", address.address1.ToString(), "Add");


                        #endregion Keep Log
                    }
                    if (address.phonenumber != Convert.ToInt32(addressData.Phonenumber))
                    {
                        #region Keep Log
                        string content = "Admin " + Session["username"] + " added company Phone number " + model.phonenumber + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", address.address1.ToString(), "Add");


                        #endregion Keep Log
                    }

                    addressData.CmpAddress1 = address.address1;
                    addressData.CmpAddress2 = address.address2;
                    addressData.Phonenumber = address.phonenumber.ToString();
                    addressData.IsActive = true;
                    addressData.TimeStamp = CurrentTime;
                    var resut = _entity.SaveChanges() > 0;
                }
                else
                {
                    //log new address

                    var data = _entity.tb_CompanyAddress.Create();
                    // 22/04/2020 code commented by Alena Sics and created a new code  //start
                    //data.companyid = compnayId??0;
                    data.companyid = Convert.ToInt64(Session["companyId"]);
                    // } //end
                    ////////data.companyid = compnayId??0;
                    data.CmpAddress1 = address.address1;
                    data.CmpAddress2 = address.address2;
                    data.Phonenumber = address.phonenumber.ToString();
                    data.IsActive = true;
                    data.TimeStamp = CurrentTime;
                    _entity.tb_CompanyAddress.Add(data);
                    var resut = _entity.SaveChanges() > 0;
                    if (resut)
                    {
                        // 22/04/2020 code by Alena sics start
                        msg = "Company added successfuly"; // end
                        // 20/06/2020 ALENA SICS
                        // string content = "Admin " + Session["username"] + " added company address  " + model.address1 + " on " + CurrentTime;
                        string content = "Admin " + Session["username"] + " added company address  " + address.address1 + " and " + address.address2 + " on " + CurrentTime; //END
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", address.address1.ToString(), "Add");

                    }
                }
            }
            // 22/04/2020 code commented by Alena sics and created new code below
            //return Json("", JsonRequestBehavior.AllowGet);
            return Json(new { msg = msg }, JsonRequestBehavior.AllowGet);
            //////return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteAddress(long CmpAddress_ID)
        {
            var addressData = _entity.tb_CompanyAddress.Where(x => x.companyid == CmpAddress_ID).FirstOrDefault();
            if (addressData != null)
            {
                var company = _entity.tb_Company.Where(x => x.Company_Id == addressData.companyid).FirstOrDefault();
                addressData.IsActive = false;
                var resut = _entity.SaveChanges() > 0;

                //log
                #region Keep Log
                string content = "Admin " + Session["username"] + " removed company  " + addressData.CmpAddress1 + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", addressData.companyid.ToString(), "Removed");
                #endregion Keep Log
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CompanyList(string id)
        {
            Companylist model = new Companylist();
            model.list = new List<CompanyModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_company(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                CompanyModel one = new CompanyModel();
                one.Comapny_Code = item.Company_Code;
                one.Company_name = item.Company_Name;
                one.payrol = item.Payroll_code ?? 0;
                one.Company_Id = item.Company_Id;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_CompanyList.cshtml", model);
        }


        public ActionResult GetCompanyById(string id)
        {
            var permission = CheckAuth(id);
            CompanyModel model = new CompanyModel();
            // 22/04/2020 code by Alena Sics
            AddressData one = new AddressData();  //end
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var company = _entity.tb_Company.Where(x => x.Company_Id == Id && x.IsActive == true).FirstOrDefault();
            if (company != null)
            {
                model.Company_Id = company.Company_Id;
                model.Comapny_Code = company.Company_Code;
                model.Company_name = company.Company_Name;
                model.payrol = company.Payroll_code ?? 0;
                model.isEdit = true;

            }
            // 22/04/2020 commented by Alena Sics And created new code below
            //var address = _entity.tb_CompanyAddress.Where(x => x.companyid == Id && x.IsActive == true);
            var address = _entity.tb_CompanyAddress.Where(x => x.companyid == company.Company_Id && x.IsActive == true).FirstOrDefault();
            // 22/04/2020 below code by Alena Sics  //start
            if (address != null)
            {
                one.companyid = address.companyid;
                one.address1 = address.CmpAddress1;
                one.address2 = address.CmpAddress2;
                one.addrsssid = address.CmpAddress_Id;
                one.IsEdit = true;
            }                                //end
                                             // 22/04/2020 below code commented and created a new code below by Alena Sics
                                             //return Json(new { company = model, address = address }, JsonRequestBehavior.AllowGet);
                                             //  return Json(new { company = model }, JsonRequestBehavior.AllowGet); //correct
            return Json(new { company = model, address = one }, JsonRequestBehavior.AllowGet);  //end
            //return PartialView("~/Views/Admin/_pv_AddCompany.cshtml", model); 
        }

        public object DeleteCompany(string id)
        {
            var permission = CheckAuth(id);//26-2-2020
            CompanyModel model = new CompanyModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Company.Where(x => x.Company_Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            var cAdd = _entity.tb_CompanyAddress.Where(x => x.companyid == Delid).FirstOrDefault();
            cAdd.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Company deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + " -" + " removed company  " + data.Company_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Company", data.Company_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        ///job
        ///28-11-2019
        ///
        public ActionResult Job_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                JobModel model = new JobModel();
                model.admin_employee_local_id = permission;
                model.Job_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        JobModel model = new JobModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Job_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }
        public object AddJob(JobModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Job
                if (_entity.tb_Job.Any(x => x.Job_Code == model.Job_Code.Trim() && x.IsActive == true && x.Job_Id != model.Job_Id))
                {
                    msg = "Job already exits !";
                }
                else if (_entity.tb_Job.Any(x => x.Job_tittle == model.Job_tittle.Trim() && x.IsActive == true && x.Job_Id != model.Job_Id))
                {
                    msg = "Job already exits !";
                }
                else
                {
                    var bs = _entity.tb_Job.Create();
                    bs.Job_Code = model.Job_Code;
                    bs.Job_tittle = model.Job_tittle;
                    bs.Group = model.Group;
                    bs.Hourly_Rate = model.Hourly_Rate;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Job.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Job added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added job " + model.Job_tittle + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Job", bs.Job_Code.ToString(), "Add");

                        string record = bs.Job_Code + " || " + bs.Job_tittle + " || " + bs.Group + " || " + bs.Hourly_Rate + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Job", bs.Job_Code.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Job.Any(x => x.Job_Code == model.Job_Code && x.IsActive == true && x.Job_tittle != model.Job_tittle && x.Job_Id != model.Job_Id))
                {
                    msg = "Job already exits !";
                }
                else
                {
                    string Job_Code = "";
                    string Job_Tittle = "";
                    string Group = "";
                    string Hourly_rate = "";

                    var data = _entity.tb_Job.Where(x => x.Job_Id == model.Job_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Job_Code != model.Job_Code)
                    {
                        Job_Code = data.Job_Code;
                    }
                    if (data.Job_tittle != model.Job_tittle)
                    {
                        Job_Tittle = data.Job_tittle;
                    }
                    if (data.Group != model.Group)
                    {
                        Group = data.Group;
                    }
                    if (data.Hourly_Rate != model.Hourly_Rate)
                    {
                        Hourly_rate = data.Hourly_Rate;
                    }
                    #endregion
                    data.Job_Code = model.Job_Code;
                    data.Job_tittle = model.Job_tittle;
                    data.Group = model.Group;
                    data.Hourly_Rate = model.Hourly_Rate;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Job Edit Sucessfully";
                        #region Keep Job Code Edit
                        if (Job_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited job code " + Job_Code + " to " + model.Job_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Job", data.Job_Id.ToString(), "Edit", Job_Code, model.Job_Code);

                            string record = data.Job_Code + " || " + data.Job_tittle + " || " + data.Group + " || " + data.Hourly_Rate + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Job", data.Job_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Job Edit
                        if (Job_Tittle != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited job tittle " + Job_Tittle + " to " + model.Job_tittle + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Job", data.Job_Id.ToString(), "Edit", Job_Tittle, model.Job_tittle);

                            string record = data.Job_Code + " || " + data.Job_tittle + " || " + data.Group + " || " + data.Hourly_Rate + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Job", data.Job_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Group Edit
                        if (Group != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited group " + Group + " to " + model.Group + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Job", data.Job_Id.ToString(), "Edit", Group, model.Group);

                            string record = data.Job_Code + " || " + data.Job_tittle + " || " + data.Group + " || " + data.Hourly_Rate + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Job", data.Job_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Hourly Rate Edit
                        if (Hourly_rate != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited hourly rate " + Hourly_rate + " to " + model.Hourly_Rate + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Job", data.Job_Id.ToString(), "Edit", Hourly_rate, model.Hourly_Rate);

                            string record = data.Job_Code + " || " + data.Job_tittle + " || " + data.Group + " || " + data.Hourly_Rate + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Job", data.Job_Id.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult JobList(string id)
        {
            JobList model = new JobList();
            model.list = new List<JobModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_job(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                JobModel one = new JobModel();
                one.Job_Code = item.Job_Code;
                one.Job_Id = item.Job_Id;
                one.Job_tittle = item.Job_tittle;
                one.Group = item.Group;
                one.Hourly_Rate = item.Hourly_Rate;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_JobList.cshtml", model);
        }

        public PartialViewResult Job_Edit(string id)
        {
            var permission = CheckAuth(id);
            JobModel model = new JobModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Job.Where(x => x.Job_Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Job_Id = data.Job_Id;
                model.Job_Code = data.Job_Code;
                model.Job_tittle = data.Job_tittle;
                model.Group = data.Group;
                model.Hourly_Rate = data.Hourly_Rate;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddJob.cshtml", model);
        }

        public object DeleteJob(string id)
        {
            var permission = CheckAuth(id);//26-02-2020
            JobModel model = new JobModel();
            bool status = false;
            string msg = "Failed";
            int DelJobid = Convert.ToInt32(id);
            var data = _entity.tb_Job.Where(x => x.Job_Id == DelJobid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Job deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + "- " + model.admin_employee_local_id + "- " + " removed job  " + data.Job_tittle + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Job", data.Job_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /////Domain
        /////30-11-2019
        /////
        public ActionResult Domain_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                DomainModel model = new DomainModel();
                model.admin_employee_local_id = permission;
                model.Domain_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        DomainModel model = new DomainModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Domain_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //        model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddDomain(DomainModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.Domain_Id == null || model.Domain_Id == 0)
            {
                #region Add Domain
                if (_entity.tb_Domain.Any(x => x.Domain_Code.Trim() == model.Domain_Code.Trim() && x.IsActive == true && x.Domain_ID != model.Domain_Id))
                {
                    msg = "Domain  already exits !";
                }
                else if (_entity.tb_Domain.Any(x => x.Domain_Code == model.Domain_Code.Trim() && x.IsActive == true && x.Domain_ID != model.Domain_Id))
                {
                    msg = "Domain  already exits !";
                }
                else
                {
                    var bs = _entity.tb_Domain.Create();
                    bs.Domain_ID = model.Domain_Id;
                    bs.Domain_Code = model.Domain_Code;
                    bs.Domain_Name = model.Domain_Name;
                    bs.Domain_Owner = model.Domain_Owner;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Domain.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Domain added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added domain " + model.Domain_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Domain", bs.Domain_ID.ToString(), "Add");


                        string record = bs.Domain_Code + " || " + bs.Domain_Name + " || " + bs.Domain_Owner + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Domain", bs.Domain_ID.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Domain.Any(x => x.Domain_Code == model.Domain_Code && x.IsActive == true && x.Domain_ID != model.Domain_Id))
                {
                    msg = "Domain already exits !";
                }
                else
                {
                    string Domain_Code = "";
                    string Domain_Name = "";
                    string Domain_Owner = "";


                    var data = _entity.tb_Domain.Where(x => x.Domain_ID == model.Domain_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Domain_Name != model.Domain_Name)
                    {
                        Domain_Name = data.Domain_Name;
                    }
                    #endregion
                    #region Checking
                    if (data.Domain_Code != model.Domain_Code)
                    {
                        Domain_Code = data.Domain_Code;
                    }
                    #endregion
                    #region Checking
                    if (data.Domain_Owner != model.Domain_Owner)
                    {
                        Domain_Owner = data.Domain_Owner;
                    }
                    #endregion
                    data.Domain_Code = model.Domain_Code;
                    data.Domain_Name = model.Domain_Name;
                    data.Domain_Owner = model.Domain_Owner;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Domain Edit Sucessfully";
                        #region Keep Domain Edit
                        if (Domain_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited domain code " + Domain_Code + " to " + model.Domain_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Domain", data.Domain_ID.ToString(), "Edit", Domain_Code.ToString(), model.Domain_Code);

                            string record = data.Domain_Code + " || " + data.Domain_Name + " || " + data.Domain_Owner + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Domain", data.Domain_ID.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Domain Name Edit
                        if (Domain_Name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited domain name " + Domain_Name + " to " + model.Domain_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Domain", data.Domain_ID.ToString(), "Edit", Domain_Name, model.Domain_Name);

                            string record = data.Domain_Code + " || " + data.Domain_Name + " || " + data.Domain_Owner + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Domain", data.Domain_ID.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Domain Owner Edit
                        if (Domain_Owner != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited domain manager " + Domain_Owner + " to " + model.Domain_Owner + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Domain", data.Domain_Owner.ToString(), "Edit", Domain_Owner, model.Domain_Owner);

                            string record = data.Domain_Code + " || " + data.Domain_Name + " || " + data.Domain_Owner + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Domain", data.Domain_ID.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        //public object GetAllEmplDomainHead()
        //{
        //    bool status = false;
        //    string msg = "Failed";
        //    var result = new WF_Tool.DataLibrary.Data.DropdownData().GetAllEmp();
        //    return Json(new { status = status, msg = msg, list = result }, JsonRequestBehavior.AllowGet);
        //}


        public PartialViewResult DomainList(string id)
        {
            DomainList model = new Models.DomainList();
            model.list = new List<DomainModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Domain(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                DomainModel one = new DomainModel();
                one.Domain_Id = item.Domain_ID;
                one.Domain_Code = item.Domain_Code;
                one.Domain_Name = item.Domain_Name;
                // 11/04/2020 code commented by Alena Sics   start
                // one.Domain_Owner = item.Domain_Owner;
                //////one.Domain_Owner = item.Domain_Owner;
                // 11/04/2020 below code by Alena Sics
                //  one.Application_Owner = item.Application_Owner;
                var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Domain_Owner).FirstOrDefault();
                if (employee != null)
                {
                    one.Domain_Owner = employee.Emp_Name;
                }
                else
                {
                    one.Domain_Owner = string.Empty;
                }    // end
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_DomainList.cshtml", model);
        }

        public PartialViewResult Domain_Edit(string id)
        {
            var permission = CheckAuth(id);
            DomainModel model = new DomainModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Domain.Where(x => x.Domain_ID == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Domain_Id = data.Domain_ID;
                model.Domain_Code = data.Domain_Code;
                model.Domain_Name = data.Domain_Name;
                model.Domain_Owner = data.Domain_Owner;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddDomain.cshtml", model);
        }

        public object DeleteDomain(string id)
        {
            var permission = CheckAuth(id);//26-2-2020
            DomainModel model = new DomainModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Domain.Where(x => x.Domain_ID == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Domain deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed domain  " + data.Domain_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Domain", data.Domain_ID.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /////Application
        /////30-11-2019
        /////

        public ActionResult Application_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                ApplicationModel model = new ApplicationModel();
                model.admin_employee_local_id = permission;
                model.Application_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        ApplicationModel model = new ApplicationModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Application_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddApplication(ApplicationModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Application
                if (_entity.tb_Application.Any(x => x.Application_Code == model.Application_Code.Trim() && x.IsActive == true && x.Application_Id != model.Application_Id))
                {
                    msg = "Application  already exits !";
                }
                else if (_entity.tb_Application.Any(x => x.Application_Name == model.Application_Name.Trim() && x.IsActive == true && x.Application_Id != model.Application_Id))
                {
                    msg = "Application  already exits !";
                }
                else
                {
                    var bs = _entity.tb_Application.Create();
                    bs.DomainId = Convert.ToInt32(model.Domain);
                    bs.Application_Code = model.Application_Code;
                    bs.Application_Name = model.Application_Name;
                    bs.Application_Owner = model.Application_Owner;
                    bs.Country_Id = Convert.ToInt32(model.Country_Id);
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Application.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Application added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added application " + model.Application_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", bs.Application_Id.ToString(), "Add");

                        string record = bs.Application_Code + " || " + bs.Application_Name + " || " + bs.Application_Owner + " || " + bs.DomainId + " || " + bs.Country_Id + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Application", bs.Application_Id.ToString(), "Admin");

                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Application.Any(x => x.Application_Code == model.Application_Code && x.IsActive == true && x.Application_Name != model.Application_Name && x.Application_Id != model.Application_Id))
                {
                    msg = "Application already exits !";
                }
                else
                {
                    string Application_Code = "";
                    string Application_Name = "";
                    string Application_Owner = "";
                    // 09/04/2020 code commented by Alena sics
                    //  long Country_Id = 0;
                    //////long Country_Id = 0;
                    long Domain_Id = 0;

                    var data = _entity.tb_Application.Where(x => x.Application_Id == model.Application_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.DomainId != model.Domain)
                    {
                        Domain_Id = data.DomainId;
                    }
                    if (data.Application_Code != model.Application_Code)
                    {
                        // 24/03/2020 commented by Alena Srishti
                        //Application_Code = data.Application_Name;
                        // 24/03/2020 Alena Srishti
                        Application_Code = data.Application_Code;
                        //////Application_Code = data.Application_Name;
                    }
                    // 09/04/2020 code commented by Alena sics
                    //if (data.Country_Id != model.Country_Id)
                    //{
                    //    Country_Id = data.Country_Id;
                    //}
                    //////if (data.Country_Id != model.Country_Id)
                    //////{
                    //////    Country_Id = data.Country_Id;
                    //////}
                    if (data.Application_Name != model.Application_Name)
                    {
                        Application_Name = data.Application_Name;
                    }
                    if (data.Application_Owner != model.Application_Owner)
                    {
                        Application_Owner = data.Application_Owner;
                    }
                    #endregion
                    data.Application_Code = model.Application_Code;
                    data.Application_Name = model.Application_Name;
                    data.Application_Owner = model.Application_Owner;
                    data.DomainId = model.Domain;
                    // 23/03/2020 commented by Alena Srishti
                    // data.Country_Id = model.Country_Id;
                    //////data.Country_Id = model.Country_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Application Edit Sucessfully";
                        #region Keep Domain Edit
                        if (Domain_Id != 0)
                        {
                            var old = _entity.tb_Domain.Where(x => x.Domain_ID == Domain_Id && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_Domain.Where(x => x.Domain_ID == model.Domain && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited domain name" + old.Domain_Name + " to " + newData.Domain_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", data.Application_Id.ToString(), "Edit", old.Domain_Name, newData.Domain_Name);

                            string record = data.Application_Code + " || " + data.Application_Name + " || " + data.Application_Owner + " || " + data.DomainId + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Application", data.Application_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Application Id Edit
                        if (Application_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited application code" + Application_Code + " to " + model.Application_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", data.Application_Id.ToString(), "Edit", Application_Code, model.Application_Code);

                            string record = data.Application_Code + " || " + data.Application_Name + " || " + data.Application_Owner + " || " + data.DomainId + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Application", data.Application_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Application Name Edit
                        if (Application_Name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited application name " + Application_Name + " to " + model.Application_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", data.Application_Id.ToString(), "Edit", Application_Name, model.Application_Name);

                            string record = data.Application_Code + " || " + data.Application_Name + " || " + data.Application_Owner + " || " + data.DomainId + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Application", data.Application_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Application Owner Edit
                        if (Application_Owner != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited application manager " + Application_Owner + " to " + model.Application_Owner + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", data.Application_Owner.ToString(), "Edit", Application_Owner, model.Application_Owner);

                            string record = data.Application_Code + " || " + data.Application_Name + " || " + data.Application_Owner + " || " + data.DomainId + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Application", data.Application_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Country Edit
                        // 23/03/2020 commented by Alena Srishti
                        //if (Country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", data.Application_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);

                        //    string record = data.Application_Code + " || " + data.Application_Name + " || " + data.Application_Owner + " || " + data.DomainId + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Application", data.Application_Id.ToString(), "Admin");
                        //}
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //public object GetAllEmplApplicationHead()
        //{
        //    bool status = false;
        //    string msg = "Failed";
        //    var result = new WF_Tool.DataLibrary.Data.DropdownData().GetAllEmp();
        //    return Json(new { status = status, msg = msg, list = result }, JsonRequestBehavior.AllowGet);
        //}

        public PartialViewResult ApplicationList(string id)
        {
            Applicationlist model = new Models.Applicationlist();
            model.list = new List<ApplicationModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Application(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                ApplicationModel one = new ApplicationModel();
                one.Application_Code = item.Application_Code;
                one.Application_Name = item.Application_Name;
                //09/04/2020 below code commented by Alena Sics and created new code //start
                //  one.Application_Owner = item.Application_Owner;
                var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.Application_Owner).FirstOrDefault();
                if (employee != null)
                {
                    one.Application_Owner = employee.Emp_Name;
                }
                else
                {
                    one.Application_Owner = string.Empty;
                }    // end
                one.Application_Id = item.Application_Id;
                one.Domain_Name = item.Domain_Name;
                one.Country_Name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_ApplicationList.cshtml", model);
        }

        public PartialViewResult Application_Edit(string id)
        {
            var permission = CheckAuth(id);
            ApplicationModel model = new ApplicationModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Application.Where(x => x.Application_Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Application_Code = data.Application_Code;
                model.Application_Name = data.Application_Name;
                model.Application_Owner = data.Application_Owner;
                model.Domain = data.DomainId;
                model.Application_Id = data.Application_Id;
                model.Country_Id = data.Country_Id;
                // 23/03/2020 below code by  Alena Srishti
                model.Country_Name = data.tb_Country.Country_Name; //end
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddApplication.cshtml", model);
        }

        public object DeleteApplication(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            ApplicationModel model = new ApplicationModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Application.Where(x => x.Application_Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Application deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + "- " + model.admin_employee_local_id + " -" + " removed application  " + data.Application_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Application", data.Application_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        ///3/12/2019
        /// CC
        ///aju

        public ActionResult CC_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                CostCenterModel model = new CostCenterModel();
                model.admin_employee_local_id = permission;
                model.CC_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        CostCenterModel model = new CostCenterModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.CC_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }
        public object Add_CC(CostCenterModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.CC_Id == null || model.CC_Id == 0)
            {
                #region Add
                if (_entity.tb_CostCenter.Any(x => x.CC_Code == model.CC_Code.Trim() && x.IsActive == true && x.CC_Id == model.CC_Id))
                {
                    msg = "CostCenter code already exits !";
                }
                else if (_entity.tb_CostCenter.Any(x => x.CC_Name == model.CC_Name.Trim() && x.IsActive == true && x.CC_Id == model.CC_Id))
                {
                    msg = "CostCenter Name already exits !";
                }
                else
                {
                    var bs = _entity.tb_CostCenter.Create();
                    bs.CC_Code = model.CC_Code;
                    bs.CC_Name = model.CC_Name;
                    bs.BU_ControllerID = model.BU_ControllerId;
                    bs.BL_ControllerID = model.BL_ControllerId;
                    bs.PG_ControllerID = model.PG_ControllerId;
                    bs.Country_Id = model.Country_Id;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_CostCenter.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "CostCenter added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added costcenter " + model.CC_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", bs.CC_Id.ToString(), "Add");

                        string record = bs.CC_Code + " || " + bs.CC_Name + " || " + bs.PG_ControllerID + " || " + bs.BL_ControllerID + " || " + bs.BU_ControllerID + " || " + bs.Country_Id + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", bs.CC_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_CostCenter.Any(x => x.CC_Name == model.CC_Name && x.IsActive == true && x.CC_Id != model.CC_Id))
                {
                    msg = "CostCenter already exits !";
                }
                else
                {
                    string CC_Name = "";
                    string CC_Code = "";
                    long BL_ControllerId = 0;
                    long BU_ControllerId = 0;
                    long PG_ControllerId = 0;
                    //string BL_ControllerId = "";
                    //string BU_ControllerId = "";
                    //string PG_ControllerId = "";
                    // 28/03/2020 below code commented by Alena sics
                    //  long Country_Id = 0;  

                    var data = _entity.tb_CostCenter.Where(x => x.CC_Id == model.CC_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.CC_Name != model.CC_Name)
                    {
                        CC_Name = data.CC_Name;
                    }
                    if (data.CC_Code != model.CC_Code)
                    {
                        CC_Code = data.CC_Code;
                    }
                    if (data.BL_ControllerID != model.BL_ControllerId && model.BL_ControllerId != 0)
                    {
                        BL_ControllerId = data.BL_ControllerID ?? 0;
                    }
                    if (data.BU_ControllerID != model.BU_ControllerId && model.BU_ControllerId != 0)
                    {
                        BU_ControllerId = data.BU_ControllerID ?? 0;
                    }
                    if (data.PG_ControllerID != model.PG_ControllerId && model.PG_ControllerId != 0)
                    {
                        PG_ControllerId = data.PG_ControllerID ?? 0;
                    }
                    // 28/03/2020 below code commented by Alena sics
                    //if (data.Country_Id != model.Country_Id)
                    //{
                    //    Country_Id = data.Country_Id;
                    //}
                    //////if (data.Country_Id != model.Country_Id)
                    //////{
                    //////    Country_Id = data.Country_Id;
                    //////}

                    #endregion
                    data.CC_Name = model.CC_Name;
                    data.CC_Code = model.CC_Code;
                    data.BL_ControllerID = model.BL_ControllerId;
                    data.BU_ControllerID = model.BU_ControllerId;
                    data.PG_ControllerID = model.PG_ControllerId;
                    // 28/03/2020 below code commented by Alena sics
                    //data.Country_Id = model.Country_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "CostCenter Edit Sucessfully";
                        #region Keep CostCenter Name Edit
                        if (CC_Name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited costcenter name " + CC_Name + " to " + model.CC_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Edit", CC_Name, model.CC_Name);

                            string record = data.CC_Code + " || " + data.CC_Name + " || " + data.PG_ControllerID + " || " + data.BL_ControllerID + " || " + data.BU_ControllerID + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", data.CC_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep CostCenter Code Edit
                        if (CC_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited costcenter code " + CC_Code + " to " + model.CC_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Edit", CC_Code, model.CC_Code);

                            string record = data.CC_Code + " || " + data.CC_Name + " || " + data.PG_ControllerID + " || " + data.BL_ControllerID + " || " + data.BU_ControllerID + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", data.CC_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep BusinessLineId Edit
                        if (BL_ControllerId != 0)
                        {
                            var old = _entity.tb_BusinessLine.Where(x => x.BL_Id == BL_ControllerId && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_BusinessLine.Where(x => x.BL_Id == model.BL_ControllerId && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited businessline controller name" + old.BL_Controller + " to " + newData.BL_Controller + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Edit", old.BL_Controller, newData.BL_Controller);

                            string record = data.CC_Code + " || " + data.CC_Name + " || " + data.PG_ControllerID + " || " + data.BL_ControllerID + " || " + data.BU_ControllerID + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", data.CC_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep BU_ControllerID Edit
                        if (BU_ControllerId != 0)
                        {
                            var old = _entity.tb_Business.Where(x => x.Bus_Id == BU_ControllerId && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_Business.Where(x => x.Bus_Id == model.BU_ControllerId && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited business controller name" + old.Bus_Controller + " to " + newData.Bus_Controller + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Edit", old.Bus_Controller, newData.Bus_Controller);

                            string record = data.CC_Code + " || " + data.CC_Name + " || " + data.PG_ControllerID + " || " + data.BL_ControllerID + " || " + data.BU_ControllerID + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", data.CC_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep PGControlerID edit
                        if (PG_ControllerId != 0)
                        {
                            var old = _entity.tb_ProductGroup.Where(x => x.PG_Id == PG_ControllerId && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_ProductGroup.Where(x => x.PG_Id == model.PG_ControllerId && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited product group controller name" + old.PG_Controller + " to " + newData.PG_Controller + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Edit", old.PG_Controller, newData.PG_Controller);

                            string record = data.CC_Code + " || " + data.CC_Name + " || " + data.PG_ControllerID + " || " + data.BL_ControllerID + " || " + data.BU_ControllerID + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", data.CC_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region keep Country edit
                        // 20/03/2020  commented by Alena Srishti
                        //if (Country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);

                        //    string record = data.CC_Code + " || " + data.CC_Name + " || " + data.PG_ControllerID + " || " + data.BL_ControllerID + " || " + data.BU_ControllerID + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_CostCenter", data.CC_Id.ToString(), "Admin");
                        //}
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult CostCenter_List(string id)
        {
            CCList model = new CCList();
            model.CC_list = new List<CostCenterModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_CostCenter(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                CostCenterModel one = new CostCenterModel();
                one.CC_Id = item.CC_Id;
                one.CC_Name = item.CC_Name;
                one.CC_Code = item.CC_Code;
                one.PrG_Id = item.PG_Controller;
                one.Businessline_Id = item.BL_Controller;
                one.Business_Id = item.Business_Controller;
                one.Country_Name = item.Country_Name;
                one.slNo = Convert.ToString(slNo);
                model.CC_list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_CostCenter_List.cshtml", model);
        }


        public object CCLoadBusinessLineByBusiness(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var bus = _entity.tb_BusinessLine.Where(x => x.Business_Id == Id && x.IsActive == true).ToList();
            if (bus.Count > 0 && bus != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = bus }, JsonRequestBehavior.AllowGet);
        }

        public object CCLoadPGByBusinessLine(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var bus = _entity.tb_ProductGroup.Where(x => x.BusinessLine_Id == Id && x.IsActive == true).ToList();
            if (bus.Count > 0 && bus != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = bus }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditCostCenter(string id)
        {
            var permission = CheckAuth(id);
            CostCenterModel model = new CostCenterModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long cocId = Convert.ToInt64(id);
            var data = _entity.tb_CostCenter.Where(x => x.CC_Id == cocId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                long busId = Convert.ToInt64(data.BU_ControllerID);
                var bus = _entity.tb_Business.Where(x => x.Bus_Id == busId).FirstOrDefault();

                long blId = Convert.ToInt64(data.BL_ControllerID);
                var bl = _entity.tb_BusinessLine.Where(x => x.BL_Id == blId).FirstOrDefault();

                long pgId = Convert.ToInt64(data.PG_ControllerID);
                var pg = _entity.tb_ProductGroup.Where(x => x.PG_Id == pgId).FirstOrDefault();

                model.CC_Id = data.CC_Id;
                model.CC_Code = data.CC_Code;
                model.CC_Name = data.CC_Name;
                model.PG_Controller = data.PG_ControllerID + "~" + pg.PG_Controller;
                model.BL_Controller = data.BL_ControllerID + "~" + bl.BL_Controller;
                model.BU_Controller = data.BU_ControllerID + "~" + bus.Bus_Controller;
                model.Country_Id = data.Country_Id;
                model.PG_ControllerId = data.PG_ControllerID ?? 0;
                model.BL_ControllerId = data.BL_ControllerID ?? 0;
                model.BU_ControllerId = data.BU_ControllerID ?? 0;
                //   20/03/2020 below code by Alena Srishti
                model.Country_Name = data.tb_Country.Country_Name;
                model.isEdit = true;
            }
            return PartialView("~/views/admin/_pv_Add_CC.cshtml", model);
        }

        public object DeleteCostCenter(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            CostCenterModel model = new CostCenterModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_CostCenter.Where(x => x.CC_Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "CostCenter deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed costcenter " + data.CC_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_CostCenter", data.CC_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }


        ///aju
        ///profile
        ///7/12/2019
        public ActionResult Profile_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                ProfileModel model = new ProfileModel();
                model.admin_employee_local_id = permission;
                model.Profile_Id = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        ProfileModel model = new ProfileModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Profile_Id = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddProfile(ProfileModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Profile
                if (_entity.tb_Emp_Profile.Any(x => x.Profile_ID == model.Profile_Id.Trim() && x.IsActive == true))
                {
                    msg = "Profile already exits !";
                }
                else if (_entity.tb_Emp_Profile.Any(x => x.Profile_Desc == model.Profile_Desc.Trim() && x.IsActive == true))
                {
                    msg = "Profile already exits !";
                }
                else
                {
                    var bs = _entity.tb_Emp_Profile.Create();
                    bs.Profile_ID = model.Profile_Id;
                    bs.Profile_Desc = model.Profile_Desc;
                    bs.Country_Id = model.Country_Id;
                    bs.p_rank = Convert.ToInt64(model.p_rank);
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Emp_Profile.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Profile added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added profile " + model.Profile_Id + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Emp_Profile", bs.Id.ToString(), "Add");

                        string record = bs.Profile_ID + " || " + bs.Profile_Desc + " || " + bs.Country_Id + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Emp_Profile", bs.Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Emp_Profile.Any(x => x.Profile_ID == model.Profile_Id && x.IsActive == true && x.Id != model.Id))
                {
                    msg = "Profile already exits !";
                }
                else
                {
                    string Profile_ID = "";
                    string Profile_Descr = "";
                    long Country_Id = 0;
                    string rank = "";


                    var data = _entity.tb_Emp_Profile.Where(x => x.Id == model.Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Profile_ID != model.Profile_Id)
                    {
                        Profile_ID = data.Profile_ID;
                    }
                    if (data.Profile_Desc != model.Profile_Desc)
                    {
                        Profile_Descr = data.Profile_Desc;
                    }
                    if (data.Country_Id != model.Country_Id)
                    {
                        Country_Id = data.Country_Id ?? 0;
                    }
                    if (data.p_rank != Convert.ToInt64(model.p_rank))
                    {
                        rank = Convert.ToString(data.p_rank);
                    }

                    #endregion
                    data.Profile_ID = model.Profile_Id;
                    data.Profile_Desc = model.Profile_Desc;
                    // 24/03/2020 commented by Alena Srishti
                    // data.Country_Id = model.Country_Id;
                    //////data.Country_Id = model.Country_Id;
                    data.p_rank = Convert.ToInt64(model.p_rank);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Profile Edit Sucessfully";
                        #region Keep profile Id
                        if (Profile_ID != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited profile  " + Profile_ID + " to " + model.Profile_Id + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Emp_Profile", data.Id.ToString(), "Edit", Profile_ID, model.Profile_Id);

                            string record = data.Profile_ID + " || " + data.Profile_Desc + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.p_rank;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Emp_Profile", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Profile Dsc Edit
                        if (Profile_Descr != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited profile description" + Profile_Descr + " to " + model.Profile_Desc + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Emp_Profile", data.Id.ToString(), "Edit", Profile_Descr, model.Profile_Desc);

                            string record = data.Profile_ID + " || " + data.Profile_Desc + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.p_rank;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Emp_Profile", data.Id.ToString(), "Admin");
                        }
                        #endregion

                        #region Keep Profile Rank Edit
                        if (rank != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited profile rank" + rank + " to " + model.p_rank + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Emp_Profile", data.Id.ToString(), "Edit", rank.ToString(), model.p_rank.ToString());

                            string record = data.Profile_ID + " || " + data.Profile_Desc + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.p_rank;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Emp_Profile", data.Id.ToString(), "Admin");
                        }
                        #endregion

                        #region Keep Country Edit
                        // 24/03/2020 commented by Alena Srishti
                        //if (Country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited country " + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Emp_Profile", data.Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);

                        //    string record = data.Profile_ID + " || " + data.Profile_Desc + " || " + data.Country_Id + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.p_rank;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Emp_Profile", data.Id.ToString(), "Admin");
                        //}
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ProfileList(string id)
        {
            Profilelist model = new Profilelist();
            model.list = new List<ProfileModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_Profile(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                ProfileModel one = new ProfileModel();
                one.Id = item.ProId;
                one.Profile_Id = item.Profile_ID;
                one.Profile_Desc = item.Profile_Desc;
                one.Country_Name = item.Country_Name;
                one.p_rank = Convert.ToString(item.p_rank);
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_ProfileList.cshtml", model);
        }

        public PartialViewResult Profile_Edit(string id)
        {
            var permission = CheckAuth(id);
            ProfileModel model = new ProfileModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long PId = Convert.ToInt64(id);
            var data = _entity.tb_Emp_Profile.Where(x => x.Id == PId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Id = data.Id;
                model.Profile_Id = data.Profile_ID;
                model.Profile_Desc = data.Profile_Desc;
                model.p_rank = Convert.ToString(data.p_rank);
                model.Country_Id = data.Country_Id ?? 0;
                //24/03/2020 below code by Alena Srishti
                model.Country_Name = data.tb_Country.Country_Name; //end
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddProfile.cshtml", model);
        }

        public object DeleteProfile(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            ProfileModel model = new ProfileModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Emp_Profile.Where(x => x.Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Profile deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed profile  " + data.Profile_ID + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Emp_Profile", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        ///status
        ///wrk
        ///
        public ActionResult ClosingProgress_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                Closing_ProgressModel model = new Closing_ProgressModel();
                model.admin_employee_local_id = permission;
                model.Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        Closing_ProgressModel model = new Closing_ProgressModel();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }


        public object AddClosing(Closing_ProgressModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Closing
                if (_entity.tb_Closing_Type.Any(x => x.Code == model.Code.Trim() && x.IsActive == true))
                {
                    msg = "Closing Progress already exits !";
                }
                else if (_entity.tb_Closing_Type.Any(x => x.Description == model.Desc.Trim() && x.IsActive == true))
                {
                    msg = "Closing Progress  already exits !";
                }
                else
                {
                    var bs = _entity.tb_Closing_Type.Create();
                    bs.Code = model.Code;
                    bs.Description = model.Desc;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Closing_Type.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Closing Type added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added closing type " + model.Code + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Closing_Type", bs.Id.ToString(), "Add");

                        string record = bs.Code + " || " + bs.Description + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Closing_Type", bs.Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Closing_Type.Any(x => x.Code == model.Code && x.IsActive == true && x.Id != model.Id))
                {
                    msg = "Closing Type already exits !";
                }
                else
                {
                    string Code = "";
                    string Descr = "";


                    var data = _entity.tb_Closing_Type.Where(x => x.Id == model.Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Code != model.Code)
                    {
                        Code = data.Code;
                    }
                    if (data.Description != model.Desc)
                    {
                        Descr = data.Description;
                    }


                    #endregion
                    data.Code = model.Code;
                    data.Description = model.Desc;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Closing Type Edit Sucessfully";
                        #region Keep Code 
                        if (Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited closing type code  " + Code + " to " + model.Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Closing_Type", data.Id.ToString(), "Edit", Code, model.Code);

                            string record = data.Code + " || " + data.Description + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Closing_Type", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Description
                        if (Descr != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited closing type description" + Descr + " to " + model.Desc + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Closing_Type", data.Description.ToString(), "Edit", Descr, model.Desc);

                            string record = data.Code + " || " + data.Description + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Closing_Type", data.Id.ToString(), "Admin");
                        }
                        #endregion

                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult Closing_ProgressList(string id)
        {
            CloseList model = new Models.CloseList();
            model.list = new List<Closing_ProgressModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var country = _entity.wf_ClosingProgress(searchType, searchItem).ToList();
            foreach (var item in country)
            {
                Closing_ProgressModel one = new Closing_ProgressModel();
                one.Id = item.Id;
                one.Code = item.Code;
                one.Desc = item.Description;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_Closing_ProgressList.cshtml", model);
        }

        public PartialViewResult Closing_Edit(string id)
        {
            var permission = CheckAuth(id);
            Closing_ProgressModel model = new Closing_ProgressModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long PId = Convert.ToInt64(id);
            var data = _entity.tb_Closing_Type.Where(x => x.Id == PId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {

                model.Code = data.Code;
                model.Desc = data.Description;
                model.Id = data.Id;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddClosing.cshtml", model);
        }

        public object Delete_Closing(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            Closing_ProgressModel model = new Closing_ProgressModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Closing_Type.Where(x => x.Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Closing Progress deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + " - " + " removed closing progress  " + data.Code + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Closing_Type", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// Aju
        /// Status
        ///


        public ActionResult Status_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                Status_Model model = new Status_Model();
                model.admin_employee_local_id = permission;
                model.S_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }

        }
        public object AddStatus(Status_Model model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Status
                if (_entity.tb_Status.Any(x => x.Status_ID == model.S_Code.Trim() && x.IsActive == true && x.S_Id != model.S_Id))
                {
                    msg = "Status already exits !";
                }
                else if (_entity.tb_Status.Any(x => x.Status_Desc == model.S_Descr.Trim() && x.IsActive == true && x.S_Id != model.S_Id))
                {
                    msg = "Status already exits !";
                }
                else
                {
                    var bs = _entity.tb_Status.Create();
                    bs.Status_ID = model.S_Code;
                    bs.Status_Desc = model.S_Descr;
                    bs.Message_Text = model.S_message;
                    bs.Application_ID = Convert.ToInt32(model.Application_ID);
                    //bs.Country_Id = model.Country_Id;
                    bs.IsActive = true;
                    bs.TimeStamp = CurrentTime;
                    _entity.tb_Status.Add(bs);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Status added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added status " + model.S_Code + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", bs.S_Id.ToString(), "Add");

                        string record = bs.Status_ID + " || " + bs.Status_Desc + " || " + bs.Message_Text + " || " + bs.Application_ID + " || " + bs.IsActive + " || " + bs.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Status", bs.S_Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Status.Any(x => x.Status_ID == model.S_Code && x.IsActive == true && x.S_Id != model.S_Id))
                {
                    msg = "Status already exits !";
                }
                else
                {
                    string Status_Code = "";
                    string Status_Descr = "";
                    string Status_message = "";
                    long Application_id = 0;
                    long Country_Id = 0;


                    var data = _entity.tb_Status.Where(x => x.S_Id == model.S_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Status_ID != model.S_Code)
                    {
                        Status_Code = data.Status_ID;
                    }
                    if (data.Status_Desc != model.S_Descr)
                    {
                        Status_Descr = data.Status_Desc;
                    }
                    if (data.Message_Text != model.S_message)
                    {
                        Status_message = data.Message_Text;
                    }
                    if (data.Application_ID != Convert.ToInt64(model.Application_ID))
                    {
                        Application_id = data.Application_ID;
                    }
                    //if (data.Country_Id != model.Country_Id)
                    //{
                    //    Country_Id = data.Country_Id;
                    //}

                    #endregion
                    data.Status_ID = model.S_Code;
                    data.Status_Desc = model.S_Descr;
                    data.Message_Text = model.S_message;
                    data.Application_ID = Convert.ToInt64(model.Application_ID);
                    //data.Country_Id = model.Country_Id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Status Edit Sucessfully";
                        #region Keep Code 
                        if (Status_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited status code  " + Status_Code + " to " + model.S_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", data.S_Id.ToString(), "Edit", Status_Code, model.S_Code);

                            string record = data.Status_ID + " || " + data.Status_Desc + " || " + data.Message_Text + " || " + data.Application_ID + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Status", data.S_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Description
                        if (Status_Descr != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited status description" + Status_Descr + " to " + model.S_Descr + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", data.S_Id.ToString(), "Edit", Status_Descr, model.S_Descr);

                            string record = data.Status_ID + " || " + data.Status_Desc + " || " + data.Message_Text + " || " + data.Application_ID + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Status", data.S_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Status Message
                        if (Status_message != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited status message" + Status_message + " to " + model.S_message + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", data.S_Id.ToString(), "Edit", Status_message, model.S_message);

                            string record = data.Status_ID + " || " + data.Status_Desc + " || " + data.Message_Text + " || " + data.Application_ID + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Status", data.S_Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Application
                        // 24/03/2020 application commented by Alena Sristi
                        //if (Application_id != 0)
                        //{
                        //    var old = _entity.tb_Application.Where(x => x.Application_Id == Application_id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Application.Where(x => x.Application_Id == model.Application_ID && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited application name" + old.Application_Name + " to " + newData.Application_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", data.S_Id.ToString(), "Edit", old.Application_Name, newData.Application_Name);

                        //    string record = data.Status_ID + " || " + data.Status_Desc + " || " + data.Message_Text + " || " + data.Application_ID + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Status", data.S_Id.ToString(), "Admin");

                        //}
                        #endregion
                        //#region Keep Country Edit
                        //if (Country_Id != 0)
                        //{
                        //    var old = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                        //    var newData = _entity.tb_Country.Where(x => x.Id == model.Country_Id && x.IsActive == true).FirstOrDefault();
                        //    string content = "Admin " + Session["username"] + " edited country name" + old.Country_Name + " to " + newData.Country_Name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", data.S_Id.ToString(), "Edit", old.Country_Name, newData.Country_Name);
                        //}
                        //#endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Status_List(string id)
        {
            StatusList model = new Models.StatusList();
            model.list = new List<Status_Model>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var status = _entity.wf_Status(searchType, searchItem).ToList();
            foreach (var item in status)
            {
                Status_Model one = new Status_Model();
                one.S_Id = item.S_Id;
                one.S_Code = item.Status_ID;
                one.S_Descr = item.Status_Desc;
                one.S_message = item.Message_Text;
                one.Country_Name = item.Country_Name;
                one.App_Id = item.Application_Name;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_Status_List.cshtml", model);
        }
        public object StatusLoadappliByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var App = WF_Tool.DataLibrary.Data.DropdownData.GetAllApplicationInCountry(Id).ToList();
            //var bus = _entity.tb_Application.Where(x => x.Country_Id == Id && x.IsActive == true).ToList();
            if (App.Count > 0 && App != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = App }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Status_Edit(string id)
        {
            var permission = CheckAuth(id);
            Status_Model model = new Status_Model();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long SId = Convert.ToInt64(id);
            var data = _entity.tb_Status.Where(x => x.S_Id == SId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.S_Id = data.S_Id;
                model.S_Code = data.Status_ID;
                model.S_Descr = data.Status_Desc;
                model.S_message = data.Message_Text;
                model.Application_ID = data.Application_ID;
                model.Country_Id = data.tb_Application.tb_Country.Id;
                // 24/03/2020 Alena Srishti
                model.Country_Name = data.tb_Application.tb_Country.Country_Name;
                // 24/03/2020 code created by Alena Srishti
                model.Application_Name = data.tb_Application.Application_Name; //end
                model.S_message = data.Message_Text;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddStatus.cshtml", model);
        }

        public object Delete_Status(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            Status_Model model = new Status_Model();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Status.Where(x => x.S_Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Status deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed status  " + data.Status_ID + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Status", data.S_Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //aju sics

        public ActionResult Email_Distribution_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                Email_Distribution_Model model = new Email_Distribution_Model();
                model.admin_employee_local_id = permission;
                model.tittle = "Add";
                model.btn_Text = "Create";
                //model.DistributionList_Code = "";
                //model.Application_ID = 0;
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        Email_Distribution_Model model = new Email_Distribution_Model();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       //model.DistributionList_Code = "";
            //       //model.Application_ID = 0;
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddEmail_Distribution(Email_Distribution_Model model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Distribution
                if (_entity.tb_DistributionList.Any(x => x.DistributionList_Code == model.DistributionList_Code && model.DistributionList_Code != 0 && x.IsActive == true && x.Id != model.EM_DS_Id))
                {
                    msg = "Distribution already exits !";
                }
                else if (_entity.tb_DistributionList.Any(x => x.WF_ID == model.Wf_ID && x.Application_ID == model.Application_ID && x.IsActive == true))
                {
                    #region Append
                    var old = _entity.tb_DistributionList.Where(x => x.WF_ID == model.Wf_ID && x.Application_ID == model.Application_ID && x.IsActive == true).FirstOrDefault();
                    // 16/04/2020 code by Alena sics
                    var mxId = _entity.tb_DistributionList.Where(x => x.IsActive == true).Max(x => x.DistributionList_Code);
                    model.DistributionList_Code = mxId + 1;
                    if (_entity.tb_DistributionList.Any(x => x.Order_No == model.orderno && x.Status_ID == model.Status_Id && x.WF_Level == model.Wf_Level))
                    {
                        model.DistributionList_Code = mxId;
                    }              //end
                    string[] splitData = model.role_id_list.Split('~');
                    foreach (var item in splitData)
                    {
                        var roleId = Convert.ToInt64(item);
                        // 16/04/2020 below code commented by Alena sics
                        //if (_entity.tb_DistributionList.Any(x => x.DistributionList_Code == old.DistributionList_Code && x.Role_Id == roleId && x.IsActive == true))
                        //{

                        //}
                        // 16/04/2020 below code commented by Alena sics and created new code
                        //else
                        if (_entity.tb_DistributionList.Any(x => x.DistributionList_Code != old.DistributionList_Code && x.Role_Id != roleId && x.IsActive == true))
                        {
                            var bs = _entity.tb_DistributionList.Create();
                            // 16/04/2020 code commented by alena and added new code
                            //bs.DistributionList_Code = old.DistributionList_Code; 
                            bs.DistributionList_Code = model.DistributionList_Code; //end
                            //////bs.DistributionList_Code = old.DistributionList_Code;
                            bs.Application_ID = model.Application_ID;
                            bs.WF_ID = model.Wf_ID;
                            bs.WF_Level = model.Wf_Level;
                            bs.Employee_ID = model.Employee_Id;
                            bs.Status_ID = model.Status_Id;
                            bs.Role_Id = Convert.ToInt64(item);
                            // 03/04/2020 code by Alena Sics
                            bs.Order_No = model.orderno; //end
                            bs.AccessType = model.Access_Type;
                            bs.IsActive = true;
                            bs.TimeStamp = CurrentTime;
                            _entity.tb_DistributionList.Add(bs);
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                msg = "Distribution added successfuly";
                                #region Keep Log
                                //26-02-2020 Aju
                                var newData = _entity.tb_WFType.Where(x => x.Id == model.Wf_ID && x.IsActive == true).FirstOrDefault();
                                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added email distribution " + newData.WF_App_Name + " on " + CurrentTime;
                                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", bs.Id.ToString(), "Add");

                                string record = bs.DistributionList_Code + "||" + bs.Application_ID + "||" + bs.WF_ID + "||" + bs.WF_Level + "||" + bs.Status_ID + "||" + bs.Role_Id + "||" + bs.Employee_ID + "||" + bs.AccessType + "||" + bs.IsActive + "||" + "||" + bs.TimeStamp;
                                bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", bs.Id.ToString(), "Admin");
                                #endregion Keep Log
                            }
                        }
                    }
                    if (splitData.Count() == 0)
                    {
                        var bs = _entity.tb_DistributionList.Create();
                        // 16/04/2020 alena commented and added new one
                        //bs.DistributionList_Code = old.DistributionList_Code; 
                        bs.DistributionList_Code = model.DistributionList_Code; //end
                        //////bs.DistributionList_Code = old.DistributionList_Code;
                        bs.Application_ID = model.Application_ID;
                        bs.WF_ID = model.Wf_ID;
                        bs.WF_Level = model.Wf_Level;
                        bs.Employee_ID = model.Employee_Id;
                        bs.Status_ID = model.Status_Id;
                        bs.Role_Id = null;
                        //03/04/2020 Alena Sics
                        bs.Order_No = model.orderno; //end
                        bs.AccessType = model.Access_Type;
                        bs.IsActive = true;
                        bs.TimeStamp = CurrentTime;
                        _entity.tb_DistributionList.Add(bs);
                        status = _entity.SaveChanges() > 0;
                        if (status)
                        {
                            msg = "Distribution added successfuly";
                            #region Keep Log
                            //26-02-2020 Aju
                            var newData = _entity.tb_WFType.Where(x => x.Id == model.Wf_ID && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added email distribution " + newData.WF_App_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", bs.Id.ToString(), "Add");

                            string record = bs.DistributionList_Code + "||" + bs.Application_ID + "||" + bs.WF_ID + "||" + bs.WF_Level + "||" + bs.Status_ID + "||" + bs.Role_Id + "||" + bs.Employee_ID + "||" + bs.AccessType + "||" + bs.IsActive + "||" + "||" + bs.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", bs.Id.ToString(), "Admin");
                            #endregion Keep Log
                        }
                    }
                    #endregion
                }
                //07/04/2020 code commented and added new by Alena Sics
                // else if (model.role_id_list != "" && model.role_id_list != null)
                else if (_entity.tb_DistributionList.Any(x => x.WF_ID != model.Wf_ID && model.role_id_list != "" && model.role_id_list != null))
                {
                    #region Roll
                    var mxId = _entity.tb_DistributionList.Where(x => x.IsActive == true).Max(x => x.DistributionList_Code);
                    //  01/04/2020 code by Alena
                    int mxid = 1;   //end
                    if (mxId != null)
                        // Alena Sics commented below line and added new line 01/04/2020
                        //model.DistributionList_Code = (mxId + 1);
                        model.DistributionList_Code = (mxId + mxid);    //end
                    {
                        // 01/04/2020  commented by Alena sics
                        //model.DistributionList_Code = '1';
                        string[] splitData = model.role_id_list.Split('~');
                        foreach (var item in splitData)
                        {
                            var bs = _entity.tb_DistributionList.Create();
                            bs.DistributionList_Code = model.DistributionList_Code;
                            bs.Application_ID = model.Application_ID;
                            bs.WF_ID = model.Wf_ID;
                            bs.WF_Level = model.Wf_Level;
                            bs.Employee_ID = model.Employee_Id;
                            bs.Status_ID = model.Status_Id;
                            bs.Role_Id = Convert.ToInt64(item);
                            //03/04/2020 Alena Sics
                            bs.Order_No = model.orderno; //end
                            bs.AccessType = model.Access_Type;
                            bs.IsActive = true;
                            bs.TimeStamp = CurrentTime;
                            _entity.tb_DistributionList.Add(bs);
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                msg = "Distribution added successfuly";
                                #region Keep Log
                                //26-02-2020 aju
                                var newData = _entity.tb_WFType.Where(x => x.Id == model.Wf_ID && x.IsActive == true).FirstOrDefault();
                                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added email distribution" + newData.WF_App_Name + " on " + CurrentTime;
                                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", bs.Id.ToString(), "Add");

                                string record = bs.DistributionList_Code + "||" + bs.Application_ID + "||" + bs.WF_ID + "||" + bs.WF_Level + "||" + bs.Status_ID + "||" + bs.Role_Id + "||" + bs.Employee_ID + "||" + bs.AccessType + "||" + bs.IsActive + "||" + "||" + bs.TimeStamp;
                                bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", bs.Id.ToString(), "Admin");
                                #endregion Keep Log
                            }
                        }
                        if (splitData.Count() == 0)
                        {
                            var bs = _entity.tb_DistributionList.Create();
                            bs.DistributionList_Code = model.DistributionList_Code;
                            bs.Application_ID = model.Application_ID;
                            bs.WF_ID = model.Wf_ID;
                            bs.WF_Level = model.Wf_Level;
                            bs.Employee_ID = model.Employee_Id;
                            bs.Status_ID = model.Status_Id;
                            bs.Role_Id = null;
                            //03/04/2020 Alena Sics
                            bs.Order_No = model.orderno; //end
                            bs.AccessType = model.Access_Type;
                            bs.IsActive = true;
                            bs.TimeStamp = CurrentTime;
                            _entity.tb_DistributionList.Add(bs);
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                msg = "Distribution added successfuly";
                                #region Keep Log
                                //26-02-2020
                                var newData = _entity.tb_WFType.Where(x => x.Id == model.Wf_ID && x.IsActive == true).FirstOrDefault();
                                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added email distribution " + newData.WF_App_Name + " on " + CurrentTime;
                                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", bs.Id.ToString(), "Add");

                                string record = bs.DistributionList_Code + "||" + bs.Application_ID + "||" + bs.WF_ID + "||" + bs.WF_Level + "||" + bs.Status_ID + "||" + bs.Role_Id + "||" + bs.Employee_ID + "||" + bs.AccessType + "||" + bs.IsActive + "||" + "||" + bs.TimeStamp;
                                bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", bs.Id.ToString(), "Admin");
                                #endregion Keep Log
                            }
                        }
                    }

                    #endregion
                }
                #endregion
            }
            else
            {
                #region Edit
                //if (_entity.tb_DistributionList.Any(x => x.WF_Level == model.Wf_Level && x.IsActive == true && x.Id != model.EM_DS_Id))
                if (_entity.tb_DistributionList.Any(x => x.DistributionList_Code == model.DistributionList_Code && model.DistributionList_Code != 0 && x.IsActive == true && x.Id != model.EM_DS_Id))
                {
                    msg = "Distribution already exits !";
                }
                else
                {
                    //string Distribution_Code = "";
                    long Application_ID = 0;
                    long Wf_ID = 0;
                    int Wf_Level = 0;
                    string Employee_Id = "";
                    long Status_Id = 0;
                    string Accesstype = "";
                    long Role = 0;
                    long Role1 = 0;     // 27/03/2020 Alena Sics created
                    long orderno = 0; // 18/04/2020 Alena Sics  created

                    var data = _entity.tb_DistributionList.Where(x => x.Id == model.EM_DS_Id && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    //if (data.DistributionList_Code != model.DistributionList_Code)
                    //{
                    //    Distribution_Code = data.DistributionList_Code;
                    //}
                    if (data.Application_ID != model.Application_ID)
                    {
                        Application_ID = data.Application_ID ?? 0;
                    }
                    if (data.WF_ID != model.Wf_ID)
                    {
                        Wf_ID = data.WF_ID ?? 0;
                    }
                    // 18/04/2020 code by Alena
                    if (data.Order_No != model.orderno)
                    {
                        orderno = data.Order_No ?? 0;
                    }   //end
                    if (data.WF_Level != model.Wf_Level)
                    {
                        Wf_Level = data.WF_Level ?? 0;
                    }
                    if (data.Employee_ID != model.Employee_Id)
                    {
                        Employee_Id = data.Employee_ID;
                    }
                    if (data.Status_ID != model.Status_Id)
                    {
                        Status_Id = data.Status_ID ?? 0;
                    }
                    if (data.AccessType != model.Access_Type)
                    {
                        Accesstype = data.AccessType;
                    }
                    if (data.Role_Id != Convert.ToInt64(model.role_id_list))
                    {
                        Role = data.Role_Id ?? 0;
                        Role1 = Convert.ToInt64(model.role_id_list);   // 27/03/2020 alena  sics created
                    }

                    #endregion
                    //data.DistributionList_Code = model.DistributionList_Code;
                    data.Application_ID = model.Application_ID;
                    data.WF_ID = model.Wf_ID;
                    data.WF_Level = model.Wf_Level;
                    data.Employee_ID = model.Employee_Id;
                    data.Status_ID = model.Status_Id;
                    data.AccessType = model.Access_Type;
                    // 03/04/2020 code by Alena Sics
                    data.Order_No = model.orderno;   //end
                    data.Role_Id = Convert.ToInt64(model.role_id_list);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Distribution  Edit Sucessfully";
                        //#region Keep Distribution 
                        //if (Distribution_Code != "")
                        //{
                        //    string content = "Admin " + Session["username"] + " editedDistribution Code  " + Distribution_Code + " to " + model.DistributionList_Code + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", Distribution_Code, model.DistributionList_Code);
                        //}
                        //#endregion
                        //18/04/2020 code by Alena Sics
                        #region Keep Orderno
                        if (orderno != 0)
                        {

                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited orderno " + orderno + " to " + model.orderno + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", orderno.ToString(), model.orderno.ToString());

                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");
                        }  //end
                        #endregion
                        #region Keep Application
                        if (Application_ID != 0)
                        {
                            var old = _entity.tb_Application.Where(x => x.Application_Id == Application_ID && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_Application.Where(x => x.Application_Id == model.Application_ID && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited application name" + old.Application_Name + " to " + newData.Application_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", old.Application_Name, newData.Application_Name);

                            // 18/04/2020 below code commented by Alena Sics
                            //   string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            // string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Wf_ID
                        if (Wf_ID != 0)
                        {
                            var old = _entity.tb_WFType.Where(x => x.Id == Wf_ID && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_WFType.Where(x => x.Id == model.Wf_ID && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited wf type" + old.WF_ID + " to " + newData.WF_ID + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", old.WF_ID, newData.WF_ID);

                            // 18/04/2020 below code commented by Alena Sics
                            //   string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            //string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Wf Level
                        if (Wf_Level != 0)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited wf level" + Wf_Level + " to " + model.Wf_Level + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", Wf_Level.ToString(), model.Wf_Level.ToString());

                            // 18/04/2020 below code commented by Alena Sics
                            //   string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            //  string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");

                        }
                        #endregion
                        #region Keep Employee
                        if (Employee_Id != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited employee" + Employee_Id + " to " + model.Employee_Id + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", Employee_Id, model.Employee_Id);

                            // 18/04/2020 below code commented by Alena Sics
                            //   string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            // string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");

                        }
                        #endregion
                        #region Keep Status Edit
                        if (Status_Id != 0)
                        {
                            var old = _entity.tb_Status.Where(x => x.S_Id == Status_Id && x.IsActive == true).FirstOrDefault();
                            var newData = _entity.tb_Status.Where(x => x.S_Id == model.Status_Id && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited status" + old.Status_ID + " to " + newData.Status_ID + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", old.Status_ID, newData.Status_ID);

                            // 18/04/2020 below code commented by Alena Sics
                            //   string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            // string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");
                        }
                        #endregion

                        #region Keep AccessType 
                        if (Accesstype != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited accesstype  " + Accesstype + " to " + model.Access_Type + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", Accesstype, model.Access_Type);

                            // 18/04/2020 below code commented by Alena Sics
                            //   string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            // string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");
                        }
                        #endregion

                        #region Keep Role 
                        if (Role != 0)
                        {
                            var old = _entity.tb_Role.Where(x => x.Id == Role && x.IsActive == true).FirstOrDefault();
                            //27/03/2020 Alena Sics commented and added new code
                            //var nwRol = _entity.tb_Role.Where(x => x.Id == Role && x.IsActive == true).FirstOrDefault();
                            var nwRol = _entity.tb_Role.Where(x => x.Id == Role1 && x.IsActive == true).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited role  " + old.Role_ID + " to " + nwRol.Role_ID + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Edit", old.Role_ID, nwRol.Role_ID);

                            // 18/04/2020 code commented by alena Sics
                            string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Role_Id + " || " + data.Order_No;
                            // string record = data.DistributionList_Code + " || " + data.Application_ID + " || " + data.WF_ID + " || " + data.WF_Level + " || " + data.Status_ID + " || " + data.Employee_ID + " || " + data.AccessType + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_DistributionList", data.Id.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }

        public object DistLoadApplByDomain(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var App_D = WF_Tool.DataLibrary.Data.DropdownData.GetAllApplicationInDomain(Id).ToList();
            //var bus = _entity.tb_Application.Where(x => x.DomainId == Id && x.IsActive == true).ToList();
            if (App_D.Count > 0 && App_D != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = App_D }, JsonRequestBehavior.AllowGet);
        }
        public object DistLoad_Wftype_ByApplication(string id)
        {
            bool status = false;
            string msg = "Failed";
            long wAp_id = Convert.ToInt64(id);
            var App_D = WF_Tool.DataLibrary.Data.DropdownData.GetAllWfTypeInApplication(wAp_id).ToList();
            //var Dis = _entity.tb_WFType.Where(x => x.Application_ID == wAp_id && x.IsActive == true).ToList();
            if (App_D.Count > 0 && App_D != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = App_D }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Email_Distribution_List(string id)
        {
            Distributionlist model = new Models.Distributionlist();
            model.list = new List<Email_Distribution_Model>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var email_dis = _entity.wf_Email_Distribution(searchType, searchItem).ToList();
            foreach (var item in email_dis)
            {
                Email_Distribution_Model one = new Email_Distribution_Model();
                one.EM_DS_Id = item.Id;
                //one.DistributionList_Code = item.DistributionList_Code;
                one.Appplication_Name = item.Application_Name;
                one.work_flow = item.WF_ID;
                one.role_id_list = item.Role_ID;
                one.Employee_Id = item.Emp_Name;
                one.Wf_Level = item.WF_Level ?? 0;
                one.Access_Type = item.AccessType;
                one.Status_Name = item.Status_ID;
                // 03/04/2020 Alena Sics
                one.orderno = item.Order_No; //end
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_Email_Distribution_List.cshtml", model);
        }

        public PartialViewResult Email_Distribution_Edit(string id)
        {
            var permission = CheckAuth(id);
            Email_Distribution_Model model = new Email_Distribution_Model();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Dis_Id = Convert.ToInt64(id);
            var data = _entity.tb_DistributionList.Where(x => x.Id == Dis_Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.EM_DS_Id = data.Id;
                model.DistributionList_Code = data.DistributionList_Code;
                model.Domain = data.tb_Application.DomainId;
                model.Application_ID = data.Application_ID ?? 0;
                // 26/03/2020 Alena Srishti    //start
                model.Domain_Name = data.tb_Application.tb_Domain.Domain_Name;
                model.Appplication_Name = data.tb_Application.Application_Name;
                model.Role_Name = data.Role_Id.ToString();
                // 03/04/2020  Alena Sics
                model.orderno = Convert.ToInt32(data.Order_No);     //end
                model.Wf_ID = data.WF_ID ?? 0;
                model.role_id_list = data.Role_Id.ToString();
                model.Wf_Level = data.WF_Level ?? 0;
                model.Employee_Id = data.Employee_ID;
                model.Status_Id = data.Status_ID ?? 0;
                model.Access_Type = data.AccessType;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddEmail_Distribution.cshtml", model);

        }

        public object Delete_Email_Distri(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            Email_Distribution_Model model = new Email_Distribution_Model();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_DistributionList.Where(x => x.Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Email Distribution deleted successfully";
                #region Keep Log
                var wf = _entity.tb_WFType.Where(x => x.Id == data.WF_ID && x.IsActive == true).FirstOrDefault();
                var newData = _entity.tb_Role.Where(x => x.Id == data.Role_Id && x.IsActive == true).FirstOrDefault();
                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " removed " + newData.Role_ID + " from " + wf.WF_ID + "  email distribution  " + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_DistributionList", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// aju Sics
        /// </summary>
        /// <param name="Srishti"></param>
        /// <returns></returns>
        public ActionResult AddButtonHome(string btid)
        {
            var permission = CheckAuth(btid);
            if (permission != string.Empty)
            {
                Button_Model model = new Button_Model();
                model.admin_employee_local_id = permission;
                model.btn_Code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (btid == null || btid == string.Empty)
            //{
            //    btid = Convert.ToString(Session["id"]);
            //}
            //if (btid != string.Empty && btid != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == btid.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        Button_Model model = new Button_Model();
            //        model.admin_employee_local_id = userData.LocalEmplyee_ID;
            //        model.btn_Code = "";
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.isEdit = false;
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object AddButton(Button_Model model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region Add Button
                if (_entity.tb_Button.Any(x => x.Code == model.btn_Code.Trim() && x.IsActive == true && x.Id != model.btn_ID))
                {
                    msg = "Button already exits !";
                }
                else if (_entity.tb_Button.Any(x => x.Description == model.btn_Description.Trim() && x.IsActive == true && x.Id != model.btn_ID))
                {
                    msg = "Button already exits !";
                }
                else
                {
                    var bt = _entity.tb_Button.Create();
                    bt.Code = model.btn_Code;
                    bt.Description = model.btn_Description;
                    bt.Button_Image = model.file_upload;
                    bt.Have_Additional_Info = Convert.ToBoolean(model.addition_info);
                    bt.IsActive = true;
                    bt.TimeStamp = CurrentTime;
                    _entity.tb_Button.Add(bt);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Button added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added button " + model.btn_Code + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Button", bt.Id.ToString(), "Add");

                        string record = bt.Code + " || " + bt.Description + " || " + bt.Button_Image + " || " + bt.Have_Additional_Info + " || " + bt.IsActive + " || " + bt.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Button", bt.Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_Button.Any(x => x.Code == model.btn_Code && x.IsActive == true && x.Id != model.btn_ID))
                {
                    msg = "Button already exits !";
                }
                else
                {
                    string Button_Code = "";
                    string Button_Description = "";
                    string BT_Img = "";
                    int Addition_Info = 0;


                    var data = _entity.tb_Button.Where(x => x.Id == model.btn_ID && x.IsActive == true).FirstOrDefault();
                    #region Checking
                    if (data.Code != model.btn_Code)
                    {
                        Button_Code = data.Code;
                    }
                    if (data.Description != model.btn_Description)
                    {
                        Button_Description = data.Description;
                    }
                    if (data.Button_Image != model.file_upload)
                    {
                        BT_Img = data.Button_Image;
                    }
                    if (data.Have_Additional_Info != Convert.ToBoolean(model.addition_info))
                    {
                        Addition_Info = Convert.ToInt32(data.Have_Additional_Info);
                    }
                    #endregion
                    data.Code = model.btn_Code;
                    data.Description = model.btn_Description;
                    data.Button_Image = model.file_upload;
                    data.Have_Additional_Info = Convert.ToBoolean(model.addition_info);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Button Edit Sucessfully";
                        #region Keep Code 
                        if (Button_Code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited button code  " + Button_Code + " to " + model.btn_Code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Button", data.Id.ToString(), "Edit", Button_Code, model.btn_Code);

                            string record = data.Code + " || " + data.Description + " || " + data.Button_Image + " || " + data.Have_Additional_Info + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Button", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Description
                        if (Button_Description != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited button description" + Button_Description + " to " + model.btn_Description + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Button", data.Id.ToString(), "Edit", Button_Description, model.btn_Description);
                            string record = data.Code + " || " + data.Description + " || " + data.Button_Image + " || " + data.Have_Additional_Info + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Button", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Status Message
                        if (BT_Img != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited status message" + BT_Img + " to " + model.file_upload + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Button", data.Id.ToString(), "Edit", BT_Img, model.file_upload);

                            string record = data.Code + " || " + data.Description + " || " + data.Button_Image + " || " + data.Have_Additional_Info + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Button", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Addition Information
                        if (Addition_Info == 0)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited additional message" + Addition_Info + " to " + (model.addition_info == 1 ? "True" : "False") + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Button", data.Id.ToString(), "Edit", BT_Img, (model.addition_info == 1 ? "True" : "False"));

                            string record = data.Code + " || " + data.Description + " || " + data.Button_Image + " || " + data.Have_Additional_Info + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Button", data.Id.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public object UploadFile()
        {
            bool status = false;
            string msg = "Failed";
            string fileSave = "";
            try
            {
                if (Request.Files.Count > 0)
                {
                    var httpPostedFile = Request.Files[0];
                    string folderPath = Server.MapPath("~/Media/Buttonfiles/");
                    var split = httpPostedFile.FileName.Split('.').ToList();
                    var ext = split[(split.Count() - 1)];
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);
                    string fileName = Guid.NewGuid().ToString() + "." + ext;
                    var img = Server.MapPath("~/Media/Buttonfiles/" + fileName);
                    fileSave = "/Media/Buttonfiles/" + fileName;
                    httpPostedFile.SaveAs(img);
                    msg = "Success";
                    status = true;
                }
            }
            catch
            {

            }
            return Json(new { status = status, msg = msg, fileSave = fileSave }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Button_List(string id)
        {
            Buttonlist model = new Models.Buttonlist();
            model.list = new List<Button_Model>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var status = _entity.wf_button(searchType, searchItem).ToList();
            foreach (var item in status)
            {
                Button_Model one = new Button_Model();
                one.btn_ID = item.Id;
                one.btn_Code = item.Code;
                one.btn_Description = item.Description;
                one.addition_info = item.Have_Additional_Info ? 1 : 0;
                one.file_upload = item.Button_Image;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_Button_List.cshtml", model);
        }

        public PartialViewResult Button_Edit(string id)
        {
            var permission = CheckAuth(id);
            Button_Model model = new Button_Model();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long SId = Convert.ToInt64(id);
            var data = _entity.tb_Button.Where(x => x.Id == SId && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.btn_ID = data.Id;
                model.btn_Code = data.Code;
                model.btn_Description = data.Description;
                model.file_upload = data.Button_Image;
                model.addition_info = data.Have_Additional_Info ? 1 : 0;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddButton.cshtml", model);
        }

        public object Delete_Button(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            Button_Model model = new Button_Model();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_Button.Where(x => x.Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Button deleted successfully";
                #region Keep Log   
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed button  " + data.Id + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Button", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddEmployee(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                AddEmployeeModel model = new AddEmployeeModel();
                model.admin_employee_local_id = permission;
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.join_date_string = CurrentTime.ToShortDateString();
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            #region 
            //if (id == null || id == string.Empty)
            //{
            //    id = Convert.ToString(Session["id"]);
            //}
            //if (id != string.Empty && id != null)
            //{
            //    var userData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id.Trim() && x.IsActive == true && x.IsAdmin == true).FirstOrDefault();
            //    if (userData != null)
            //    {
            //        FormsAuthentication.SetAuthCookie(userData.LocalEmplyee_ID.ToString(), false);
            //        Response.Cookies["UserType"].Value = "Admin";
            //        Session["id"] = userData.LocalEmplyee_ID;
            //        Session["username"] = userData.Emp_Name;
            //        _userId = ((string)Session["id"]);
            //        _userName = ((string)Session["username"]);
            //        AddEmployeeModel model = new AddEmployeeModel();
            //        model.tittle = "Add";
            //        model.btn_Text = "Create";
            //       model.join_date_string = CurrentTime.ToShortDateString();
            //        return View(model);
            //    }
            //    else
            //    {
            //        return RedirectToAction("Home", "Account");
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Home", "Account");
            //}
            #endregion
        }

        public object ProfileByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var profile = WF_Tool.DataLibrary.Data.DropdownData.GetAllProfiles(Id).ToList();
            if (profile.Count > 0 && profile != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = profile }, JsonRequestBehavior.AllowGet);
        }
        public object LocationByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var location = WF_Tool.DataLibrary.Data.DropdownData.GetAllLocationInCountry(Id).ToList();
            if (location.Count > 0 && location != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = location }, JsonRequestBehavior.AllowGet);
        }

        public object CostcenterByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var cc = WF_Tool.DataLibrary.Data.DropdownData.GetAllCostCenterByEntry(id).ToList();
            if (cc.Count > 0 && cc != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = cc }, JsonRequestBehavior.AllowGet);
        }
        public object AddEmployeeDetails(AddEmployeeModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (_entity.tb_WF_Employee.Any(x => x.LocalEmplyee_ID == model.local_emp_id.Trim() && x.IsActive == true))
            {
                msg = "Local Employee ID already exits !";
            }
            else if (_entity.tb_WF_Employee.Any(x => x.Global_Group == model.global_emp_id.Trim() && x.IsActive == true))
            {
                msg = "Global Employee ID already exits !";
            }
            else
            {
                var newEmp = _entity.tb_WF_Employee.Create();
                newEmp.GblEmp_ID = model.global_emp_id;
                newEmp.Emp_Name = model.employee_name;
                newEmp.CC_Id = model.cost_center_id;
                newEmp.Profile_ID = model.profile_id;
                newEmp.Country_Id = model.country_id;
                newEmp.LocalEmplyee_ID = model.local_emp_id;
                newEmp.Job_Tittle_Id = model.job_tittle;
                newEmp.Global_Group = model.global_grade;
                newEmp.Local_Group = model.local_grade;
                if (model.department_id != 0) // 19-02-2020 ARCHANA SRISHTI 
                {
                    newEmp.Department_Id = model.department_id;
                }
                newEmp.Productgroup_Id = model.product_group_id;
                newEmp.Businessline_Id = model.business_line_id;
                newEmp.Business_Id = model.business_id;
                newEmp.Location_Id = model.location_id;
                newEmp.Tele_Ext = model.tele_ext;
                newEmp.Mobile_No = model.mobile_no;
                //if (model.join_date_string != null)
                //    try
                //    {
                //        newEmp.Date_Join = Convert.ToDateTime(model.join_date_string);
                //    }
                //    catch
                //    {
                //        newEmp.Date_Join = CurrentTime;
                //    }
                // 21/04/2020 commented by Alena Sics
                //if (model.join_date.Year != 1) // 20-02-2020 ARCHANA SRISHTI 
                //{
                //    try
                //    {
                //        newEmp.Date_Join = Convert.ToDateTime(model.join_date);
                //    }
                //    catch
                //    {
                //        newEmp.Date_Join = CurrentTime;
                //    }
                //}
                // 21/04/2020 by Alena Sics
                newEmp.Date_Join = Convert.ToDateTime(model.join_date_string); //end
                newEmp.PositionClass_ID = model.position_class;
                newEmp.EmployeeType_ID = model.employee_type;
                newEmp.DelegationBand = model.delegation_band;
                newEmp.ADAccount = model.ad_user_account;
                newEmp.eMail = model.email_address;
                newEmp.Company_Id = model.company_id;
                newEmp.DelegationFlag = model.delegation_status;
                newEmp.Delegate_Emp_Code = model.delegate_deputy;
                newEmp.Vendor_Id = model.venderId;
                newEmp.Line_Manager = model.line_manager_code;
                newEmp.IsActive = true;
                newEmp.TimeStamp = CurrentTime;
                newEmp.Admin_Access = model.admin_access; // 20-02-2020 ARCHANA SRISHTI 
                if (model.admin_access == "SuperAdmin")
                {
                    newEmp.IsAdmin = 1; //Basheer on 06-04-2020 while changing isadmin bool to in
                }
                else if (model.admin_access == "None")
                {
                    newEmp.IsAdmin = 0; //Basheer on 06-04-2020 while changing isadmin bool to in
                }
                //newEmp.Admin_Access = model.admin_access;
                newEmp.Gender = model.gender == 0 ? "Male" : "Female"; // Archana Added new fields Gender  11-02-2020
                newEmp.IsAppraisal = Convert.ToBoolean(model.isAppraisal); // Archana added new field is appraisal 11-02-2020
                newEmp.IsTimeSheet = Convert.ToBoolean(model.isTimeSheet);// Archana added new field is time stamp 11-02-2020
                newEmp.MobileExtension = model.mobile_extention;//aju added new field mobileextention 15-2-2020
                                                                //30/03/2020 by Alena Sics
                newEmp.DelegationFlag = Convert.ToBoolean(model.deles); //end  
                _entity.tb_WF_Employee.Add(newEmp);
                try
                {
                    status = _entity.SaveChanges() > 0;
                }
                catch (Exception ex)
                {

                }
                if (status == true)
                {
                    msg = "Successful";
                    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " add new employee name " + model.employee_name + " on " + CurrentTime;
                    bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", model.local_emp_id.ToString(), "Add");

                    string record = newEmp.GblEmp_ID + " || " + newEmp.Emp_Name + " || " + newEmp.CC_Id + " || " + newEmp.Profile_ID + " || " + newEmp.Gender + " || " + newEmp.LocalEmplyee_ID + " || " + newEmp.Job_Tittle_Id + " || " +
                        newEmp.Global_Group + " || " + newEmp.Local_Group + " || " + newEmp.Department_Id + " || " + newEmp.Location_Id + " || " + newEmp.Tele_Ext + " || " + newEmp.Mobile_No + " || " + newEmp.Date_Join + " || " + newEmp.PositionClass_ID + " || " + newEmp.EmployeeType_ID +
                        " || " + newEmp.DelegationBand + " || " + newEmp.ADAccount + " || " + newEmp.eMail + " || " + newEmp.Company_Id + " || " + newEmp.DelegationFlag + " || " + newEmp.Delegate_Emp_Code + " || " + newEmp.Vendor_Id + " || " + newEmp.Line_Manager + " || " + newEmp.IsActive + " || " +
                        " || " + newEmp.TimeStamp + " || " + newEmp.IsAdmin + " || " + newEmp.Country_Id + " || " + newEmp.Business_Id + " || " + newEmp.Businessline_Id + " || " + newEmp.Productgroup_Id + " || " + " || " + newEmp.Admin_Access + " || " + newEmp.IsAppraisal + " || " + newEmp.IsTimeSheet;
                    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", newEmp.Id.ToString(), "Admin");
                }
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewEmployee()
        {
            var permission = CheckAuth("");
            if (permission != string.Empty)
            {
                ListEmployeeModels model = new ListEmployeeModels();
                model._list = new List<EmployeeModel>();
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
            //ListEmployeeModels model = new ListEmployeeModels();
            //model._list = new List<EmployeeModel>();
            //return View(model);
        }
        public PartialViewResult EmployeeList(string id)
        {
            ListEmployeeModels model = new ListEmployeeModels();
            model._list = new List<EmployeeModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            #region 
            var data = _entity.wf_EmployeeDetails(searchType, searchItem).ToList();
            try
            {
                foreach (var item in data)
                {
                    EmployeeModel one = new EmployeeModel();
                    one.emp_name = item.Emp_Name;
                    one.globalid = item.GblEmp_ID;
                    one.emp_localid = item.LocalEmplyee_ID;
                    one.global_group = item.Global_Group;
                    one.telephone = item.Tele_Ext;
                    one.mobile = item.Mobile_No;
                    one.location = item.Location;
                    one.company = item.Company_Name;
                    one.employeetype = item.EmployeeType_ID;
                    one.department = item.Department_Name;
                    one.localgroup = item.Local_Group;
                    one.DateOfJoin = Convert.ToString(item.Date_Join);
                    one.position_class = item.PositionClass_ID;
                    one.delegation_band = item.DelegationBand;
                    one.ad_account = item.ADAccount;
                    one.email = item.eMail;
                    one.cc_name = item.CC_Name;
                    one.profile = item.Profile_Desc;
                    one.country_name = item.Country_Name;
                    one.job_tittle = item.Job_tittle;
                    one.pg_name = item.PG_Name;
                    one.businessline = item.Business_Line_Name;
                    one.business = item.Business_Name;
                    one.delegation_flag = item.DelegationFlag ?? false;
                    one.delegation_emp = item.Delegation_emp;
                    one.vender = item.Vendor_Name;
                    one.line_manager = item.LineManager;
                    one.admin_access = item.Admin_Access;
                    one.mobile_extention = item.MobileExtension;
                    model._list.Add(one);
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("~/Views/Admin/_pv_EmployeeList.cshtml", model);
        }

        public object DeleteEmployee(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            AddEmployeeModel model = new AddEmployeeModel();
            bool status = false;
            string msg = "Failed";
            var data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id && x.IsActive == true).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Employee deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " remove employee " + data.Emp_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", data.LocalEmplyee_ID.ToString(), "Remove");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditEmployee(string id)
        {
            var permission = CheckAuth(Session["id"].ToString());
            if (permission != string.Empty)
            {
                AddEmployeeModel model = new AddEmployeeModel();
                model.tittle = "Edit";
                model.btn_Text = "Save";
                model.admin_employee_local_id = permission;
                var data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == id && x.IsActive == true).FirstOrDefault();
                model.employee_name = data.Emp_Name;
                model.ad_user_account = data.ADAccount;
                model.employee_type = data.EmployeeType_ID;
                model.global_emp_id = data.GblEmp_ID;
                model.email_address = data.eMail;
                model.local_emp_id = data.LocalEmplyee_ID;
                model.job_tittle = data.Job_Tittle_Id ?? 0;
                model.join_date = data.Date_Join ?? CurrentTime;
                model.join_date_string = data.Date_Join.ToString() ?? CurrentTime.ToShortDateString();
                model.tele_ext = data.Tele_Ext;
                model.mobile_no = data.Mobile_No;
                model.local_grade = data.Local_Group;
                model.global_grade = data.Global_Group;
                model.position_class = data.PositionClass_ID;

                //20-02-2020 ARCHANA SRISHTI COMMENDED THESE LINES AND ADD NEW LINES 
                //model.country_id = data.tb_Location.Country_Id ?? 0;
                //model.business_id = data.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Id;
                //model.business_line_id = data.tb_Department.tb_ProductGroup.BusinessLine_Id;
                //model.product_group_id = data.tb_Department.PG_Id;

                model.country_id = data.Country_Id ?? 0;
                model.business_id = data.Business_Id ?? 0;
                model.business_line_id = data.Businessline_Id ?? 0;
                model.product_group_id = data.Productgroup_Id ?? 0;

                model.department_id = data.Department_Id ?? 0;
                model.company_id = data.Company_Id ?? 0;
                model.profile_id = data.Profile_ID ?? 0;
                model.location_id = data.Location_Id ?? 0;
                model.cost_center_id = data.CC_Id ?? 0;
                model.delegation_band = data.DelegationBand;
                //   30/03/2020 commented by Alena sics
                // model.delegation_status = data.DelegationFlag ?? false;
                model.delegate_deputy = data.Delegate_Emp_Code;
                model.venderId = data.Vendor_Id ?? 0;
                model.line_manager_code = data.Line_Manager;
                model.admin_access = data.Admin_Access;
                model.mobile_extention = data.MobileExtension;
                //Archana 11-02-2020 Extra data in Employee 
                if (data.Gender == "Female")
                    model.gender = gender.Female;
                else
                    model.gender = gender.Male;
                if (data.IsAppraisal == true)
                    model.isAppraisal = BooleanValue.Yes;
                else
                    model.isAppraisal = BooleanValue.No;
                if (data.IsTimeSheet == true)
                    model.isTimeSheet = BooleanValue.Yes;
                else
                    model.isTimeSheet = BooleanValue.No;
                //30/03/2020 below code by Alena sics   //start
                if (data.DelegationFlag == true)
                    model.deles = BooleanValue.Yes;
                else
                    model.deles = BooleanValue.No;     //end

                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }

        }

        public object EditEmployeeDetails(AddEmployeeModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (_entity.tb_WF_Employee.Any(x => x.Global_Group == model.global_emp_id.Trim() && x.IsActive == true && x.LocalEmplyee_ID == model.local_emp_id))
            {
                msg = "Global Employee ID already exits !";
            }
            else
            {
                #region
                var editEmp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == model.local_emp_id && x.IsActive == true).FirstOrDefault();
                string GblEmp_ID = editEmp.GblEmp_ID;
                string Emp_Name = editEmp.Emp_Name;
                long CC_Id = editEmp.CC_Id ?? 0;
                long Profile_ID = editEmp.Profile_ID ?? 0;
                //long Country_Id = editEmp.Country_Id ?? 0;
                long Job_Tittle_Id = editEmp.Job_Tittle_Id ?? 0;
                string Global_Group = editEmp.Global_Group;
                string Local_Group = editEmp.Local_Group;
                long Department_Id = editEmp.Department_Id ?? 0;
                long PG_Id = editEmp.Productgroup_Id ?? 0;
                long BusinessLine_Id = editEmp.Businessline_Id ?? 0;
                long Business_Id = editEmp.Business_Id ?? 0;
                long Location_Id = editEmp.Location_Id ?? 0;
                string Tele_Ext = editEmp.Tele_Ext;
                string Mobile_No = editEmp.Mobile_No;
                string join_date_string = editEmp.Date_Join.ToString();
                string PositionClass_ID = editEmp.PositionClass_ID;
                string EmployeeType_ID = editEmp.EmployeeType_ID;
                string DelegationBand = editEmp.DelegationBand;
                string ADAccount = editEmp.ADAccount;
                string eMail = editEmp.eMail;
                long Company_Id = editEmp.Company_Id ?? 0;
                bool DelegationFlag = editEmp.DelegationFlag ?? true;
                string Delegate_Emp_Code = editEmp.Delegate_Emp_Code;
                long Vendor_Id = editEmp.Vendor_Id ?? 0;
                string adminaccess = editEmp.Admin_Access;
                string Line_Manager = editEmp.Line_Manager;
                string mobile_extention = editEmp.MobileExtension;
                //Archana 11-02-2020 Extra data in the employee table
                string gender = editEmp.Gender ?? "";
                string isAppraisal = editEmp.IsAppraisal == true ? "Yes" : "No";
                string isTimeSheet = editEmp.IsTimeSheet == true ? "Yes" : "No";
                // 30/03/2020 below code by Alena sics     //start
                string deless = editEmp.DelegationFlag == true ? "Yes" : "No";   //end
                #endregion

                #region Edit
                #region
                editEmp.GblEmp_ID = model.global_emp_id;
                editEmp.Emp_Name = model.employee_name;
                editEmp.CC_Id = model.cost_center_id;
                editEmp.Profile_ID = model.profile_id;
                //editEmp.Country_Id = model.country_id;
                editEmp.Job_Tittle_Id = model.job_tittle;
                editEmp.Global_Group = model.global_grade;
                editEmp.Local_Group = model.local_grade;
                if (model.department_id == 0)//19-02-2020 arcahana srtishti
                {
                    editEmp.Department_Id = null;
                }
                else
                {
                    editEmp.Department_Id = model.department_id;
                }
                editEmp.Productgroup_Id = model.product_group_id;
                editEmp.Businessline_Id = model.business_line_id;
                editEmp.Business_Id = model.business_id;
                editEmp.Location_Id = model.location_id;
                editEmp.Tele_Ext = model.tele_ext;
                editEmp.Mobile_No = model.mobile_no;
                //Archana Srishti 11-02-2020 Add extra data in to the employee table 
                editEmp.Gender = Convert.ToString(model.gender);
                editEmp.IsAppraisal = Convert.ToBoolean(model.isAppraisal);
                editEmp.IsTimeSheet = Convert.ToBoolean(model.isTimeSheet);
                //30/03/2020 below code by Alena sics                      //start
                editEmp.DelegationFlag = Convert.ToBoolean(model.deles);     //end
                if (model.join_date != null)
                    try
                    {
                        editEmp.Date_Join = Convert.ToDateTime(model.join_date).Year == 001 ? CurrentTime : Convert.ToDateTime(model.join_date);
                    }
                    catch
                    {
                        editEmp.Date_Join = CurrentTime;
                    }
                editEmp.PositionClass_ID = model.position_class;
                editEmp.EmployeeType_ID = model.employee_type;
                editEmp.DelegationBand = model.delegation_band;
                editEmp.ADAccount = model.ad_user_account;
                editEmp.eMail = model.email_address;
                editEmp.Company_Id = model.company_id;
                // 30/03/2020 commented by Alena sics
                // editEmp.DelegationFlag = model.delegation_status;
                editEmp.Delegate_Emp_Code = model.delegate_deputy;
                editEmp.Vendor_Id = model.venderId;
                editEmp.Line_Manager = model.line_manager_code;
                editEmp.IsActive = true;
                editEmp.TimeStamp = CurrentTime;
                editEmp.MobileExtension = model.mobile_extention;
                //if (editEmp.Admin_Access == "SuperAdmin")
                //{
                //    editEmp.IsAdmin = true;
                //}
                //else if (editEmp.Admin_Access == "None")
                //{
                //    editEmp.IsAdmin = false;
                //}
                editEmp.Admin_Access = model.admin_access;
                if (model.admin_access == "SuperAdmin") // 20-02-2020 ARCHANA SRISHTI 
                {
                    editEmp.IsAdmin = 1; //Basheer on 06-04-2020 while changing isadmin bool to in
                }
                else
                {
                    editEmp.IsAdmin = 0; //Basheer on 06-04-2020 while changing isadmin bool to in
                }
                try // 19-02-2020 II ARCHANA SRISHTI  
                {
                    status = _entity.SaveChanges() > 0;
                }
                catch (Exception ex)
                {

                }
                #endregion
                if (status == true)
                {
                    msg = "Successful";
                    //31/03/2020 code by Alena sics    //start 
                    if (deless != (editEmp.DelegationFlag == true ? "Yes" : "No"))
                    {
                        string content = "Admin" + Session["username"] + "-" + model.admin_employee_local_id + "-" + "edit delegation status" + deless + " to " + (editEmp.DelegationFlag == true ? "Yes" : "No") + "on" + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", deless, (editEmp.DelegationFlag == true ? "Yes" : "No"));

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                        editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                        " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                        " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");

                    }                               //end
                    if (GblEmp_ID != editEmp.GblEmp_ID)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit global employee ID " + GblEmp_ID + " to " + editEmp.GblEmp_ID + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", GblEmp_ID, editEmp.GblEmp_ID);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Emp_Name != editEmp.Emp_Name)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit employee name " + Emp_Name + " to " + editEmp.Emp_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", Emp_Name, editEmp.Emp_Name);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Global_Group != editEmp.Global_Group)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit global group Id " + Global_Group + " to " + editEmp.Global_Group + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", Global_Group, editEmp.Global_Group);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Local_Group != editEmp.Local_Group)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit local group Id " + Local_Group + " to " + editEmp.Local_Group + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", Local_Group, editEmp.Local_Group);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Tele_Ext != editEmp.Tele_Ext)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit tele extn " + Tele_Ext + " to " + editEmp.Tele_Ext + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", Tele_Ext, editEmp.Tele_Ext);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Mobile_No != editEmp.Mobile_No)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit mobile number " + Mobile_No + " to " + editEmp.Mobile_No + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", Mobile_No, editEmp.Mobile_No);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                        editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                        " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                        " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (join_date_string != editEmp.Date_Join.ToString())
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit join date " + join_date_string + " to " + editEmp.Date_Join.ToString() + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", join_date_string, editEmp.Date_Join.ToString());

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (PositionClass_ID != editEmp.PositionClass_ID)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit position class " + PositionClass_ID + " to " + editEmp.PositionClass_ID + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", PositionClass_ID, editEmp.PositionClass_ID);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                        editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                        " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                        " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (EmployeeType_ID != editEmp.EmployeeType_ID)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit employee type " + EmployeeType_ID + " to " + editEmp.EmployeeType_ID + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", EmployeeType_ID, editEmp.EmployeeType_ID);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                        editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                        " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                        " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (DelegationBand != editEmp.DelegationBand)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit delegation band " + DelegationBand + " to " + editEmp.DelegationBand + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", DelegationBand, editEmp.DelegationBand);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                        editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                        " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                        " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (ADAccount != editEmp.ADAccount)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit AD user account " + ADAccount + " to " + editEmp.ADAccount + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", ADAccount, editEmp.ADAccount);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (eMail != editEmp.eMail)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit email address " + eMail + " to " + editEmp.eMail + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", eMail, editEmp.eMail);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (adminaccess != editEmp.Admin_Access)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit admin access " + adminaccess + " to " + editEmp.Admin_Access + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", adminaccess, editEmp.Admin_Access);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                         editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                         " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                         " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Delegate_Emp_Code != editEmp.Delegate_Emp_Code)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit delegate / deputy " + Delegate_Emp_Code + " to " + editEmp.Delegate_Emp_Code + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", Delegate_Emp_Code, editEmp.Delegate_Emp_Code);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                        editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                        " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                        " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (Line_Manager != editEmp.Line_Manager) // 19-02-2020 II Archana Srishti 
                    {
                        var lineOld = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == Line_Manager && x.IsActive == true).FirstOrDefault();
                        var lineNew = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == editEmp.Line_Manager && x.IsActive == true).FirstOrDefault();
                        if (lineOld != null && lineNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit line manager " + lineOld == null ? "" : lineOld.Emp_Name + "(" + Line_Manager + ")" + " to " + lineNew == null ? "" : lineNew.Emp_Name + "(" + editEmp.Line_Manager + ")" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", lineOld == null ? "" : lineOld.Emp_Name + "(" + Line_Manager + ")", lineNew == null ? "" : lineNew.Emp_Name + "(" + editEmp.Line_Manager + ")");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                             editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                             " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                             " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (lineOld == null && lineNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit line manager None  " + "(" + Line_Manager + ")" + " to " + lineNew == null ? "" : lineNew.Emp_Name + "(" + editEmp.Line_Manager + ")" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None" + "(" + Line_Manager + ")", lineNew == null ? "" : lineNew.Emp_Name + "(" + editEmp.Line_Manager + ")");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                             editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                             " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                             " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (lineNew == null && lineOld != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit line manager " + lineOld == null ? "" : lineOld.Emp_Name + "(" + Line_Manager + ")" + " to None (" + editEmp.Line_Manager + ")" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", lineOld == null ? "" : lineOld.Emp_Name + "(" + Line_Manager + ")", "None" + "(" + editEmp.Line_Manager + ")");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                             editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                             " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                             " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    if (CC_Id != editEmp.CC_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var ccOld = _entity.tb_CostCenter.Where(x => x.CC_Id == CC_Id && x.IsActive == true).FirstOrDefault();
                        var ccNew = _entity.tb_CostCenter.Where(x => x.CC_Id == editEmp.CC_Id && x.IsActive == true).FirstOrDefault();
                        if (ccOld != null && ccNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit cost center " + ccOld == null ? "" : ccOld.CC_Name + " to " + ccNew == null ? "" : ccNew.CC_Name + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", ccOld == null ? "" : ccOld.CC_Name, ccNew == null ? "" : ccNew.CC_Name);
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit cost center " + ccOld.CC_Name + " to " + ccNew.CC_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", ccOld == null ? "" : ccOld.CC_Name, ccNew == null ? "" : ccNew.CC_Name);
                            //end
                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                             editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                             " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                             " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (ccOld == null && ccNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit cost center " + "None" + " to " + ccNew == null ? "" : ccNew.CC_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", ccNew == null ? "" : ccNew.CC_Name);

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                             editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                             " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                             " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (ccOld != null && ccNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit cost center " + ccOld == null ? "" : ccOld.CC_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", ccOld == null ? "" : ccOld.CC_Name, "None");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                             editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                             " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                             " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    if (Profile_ID != editEmp.Profile_ID)// 19-02-2020 II Archana Srishti 
                    {
                        var profileOld = _entity.tb_Emp_Profile.Where(x => x.Id == Profile_ID && x.IsActive == true).FirstOrDefault();
                        var profileNew = _entity.tb_Emp_Profile.Where(x => x.Id == editEmp.Profile_ID && x.IsActive == true).FirstOrDefault();

                        if (profileOld != null && profileNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit profile  " + profileOld == null ? "" : profileOld.Profile_Desc + " to " + profileNew == null ? "" : profileNew.Profile_Desc + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", profileOld == null ? "" : profileOld.Profile_Desc, profileNew == null ? "" : profileNew.Profile_Desc);
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit profile  " + profileOld.Profile_Desc + " to " + profileNew.Profile_Desc + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", profileOld == null ? "" : profileOld.Profile_Desc, profileNew == null ? "" : profileNew.Profile_Desc);
                            //end

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                            editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                            " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                            " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (profileOld == null && profileNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit profile  " + "None" + " to " + profileNew == null ? "" : profileNew.Profile_Desc + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", profileNew == null ? "" : profileNew.Profile_Desc);

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                            editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                            " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                            " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (profileOld != null && profileNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit profile  " + profileOld == null ? "" : profileOld.Profile_Desc + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", profileOld == null ? "" : profileOld.Profile_Desc, "None");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                            editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                            " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                            " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    //if (Country_Id != editEmp.Country_Id)
                    //{
                    //    var countryOld = _entity.tb_Country.Where(x => x.Id == Country_Id && x.IsActive == true).FirstOrDefault();
                    //    var countryNew = _entity.tb_Country.Where(x => x.Id == editEmp.Country_Id && x.IsActive == true).FirstOrDefault();
                    //    string content = "Admin " + Session["username"] + " edit Country  " + countryOld == null ? "" : countryOld.Country_Name + " to " + countryNew == null ? "" : countryNew.Country_Name + " on " + CurrentTime;
                    //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", countryOld == null ? "" : countryOld.Country_Name, countryNew == null ? "" : countryNew.Country_Name);
                    //}
                    if (Job_Tittle_Id != editEmp.Job_Tittle_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var jobOld = _entity.tb_Job.Where(x => x.Job_Id == Job_Tittle_Id && x.IsActive == true).FirstOrDefault();
                        var jobNew = _entity.tb_Job.Where(x => x.Job_Id == editEmp.Job_Tittle_Id && x.IsActive == true).FirstOrDefault();
                        if (jobOld != null && jobNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit job tittle  " + jobOld == null ? "" : jobOld.Job_tittle + " to " + jobNew == null ? "" : jobNew.Job_tittle + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", jobOld == null ? "" : jobOld.Job_tittle, jobNew == null ? "" : jobNew.Job_tittle);
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit job tittle  " + jobOld.Job_tittle + " to " + jobNew.Job_tittle + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", jobOld == null ? "" : jobOld.Job_tittle, jobNew == null ? "" : jobNew.Job_tittle);
                            //end

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                            editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                            " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                            " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (jobOld != null && jobNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit job tittle  " + jobOld == null ? "" : jobOld.Job_tittle + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", jobOld == null ? "" : jobOld.Job_tittle, "None");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                            editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                            " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                            " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (jobOld == null && jobNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit job tittle  " + "None" + " to " + jobNew == null ? "" : jobNew.Job_tittle + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", jobNew == null ? "" : jobNew.Job_tittle);

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                            editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                            " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                            " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    if (Vendor_Id != editEmp.Vendor_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var venderOld = _entity.tb_Vendor.Where(x => x.Vendor_id == Vendor_Id && x.IsActive == true).FirstOrDefault();
                        var venderNew = _entity.tb_Vendor.Where(x => x.Vendor_id == editEmp.Vendor_Id && x.IsActive == true).FirstOrDefault();
                        if (venderOld != null && venderNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit vender  " + venderOld == null ? "" : venderOld.Vendor_Name + " to " + venderNew == null ? "" : venderNew.Vendor_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", venderOld == null ? "" : venderOld.Vendor_Name, venderNew == null ? "" : venderNew.Vendor_Name);
                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (venderOld == null && venderNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit vender  " + "None" + " to " + venderNew == null ? "" : venderNew.Vendor_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", venderNew == null ? "" : venderNew.Vendor_Name);
                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (venderOld != null && venderNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit vender  " + venderOld == null ? "" : venderOld.Vendor_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", venderOld == null ? "" : venderOld.Vendor_Name, "None");
                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    if (Department_Id != editEmp.Department_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var depOld = _entity.tb_Department.Where(x => x.Department_Id == Department_Id && x.IsActive == true).FirstOrDefault();
                        var depNew = _entity.tb_Department.Where(x => x.Department_Id == editEmp.Department_Id && x.IsActive == true).FirstOrDefault();
                        if (depOld != null && depNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit department  " + depOld == null ? "" : depOld.Department_Name + " to " + depNew == null ? "" : depNew.Department_Name + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", depOld == null ? "" : depOld.Department_Name, depNew == null ? "" : depNew.Department_Name);
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit department  " + depOld.Department_Name + " to " + depNew.Department_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", depOld == null ? "" : depOld.Department_Name, depNew == null ? "" : depNew.Department_Name);
                            //end

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (depOld == null && depNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit department  " + "None" + " to " + depNew == null ? "" : depNew.Department_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", depNew == null ? "" : depNew.Department_Name);

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (depOld != null && depNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit department  " + depOld == null ? "" : depOld.Department_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", depOld == null ? "" : depOld.Department_Name, "None");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    //Archana Srishti  11-02-2020 Add extra data into Employee table 
                    if (gender != editEmp.Gender)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit gender " + gender + " to " + editEmp.Gender ?? "" + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", gender, editEmp.Gender);

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                       editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                       " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                       " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }

                    if (isAppraisal != (editEmp.IsAppraisal == true ? "Yes" : "No"))
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit appraisal " + isAppraisal + " to " + (editEmp.IsAppraisal == true ? "Yes" : "No") + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", isAppraisal, (editEmp.IsAppraisal == true ? "Yes" : "No"));

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                       editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                       " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                       " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    if (isTimeSheet != (editEmp.IsTimeSheet == true ? "Yes" : "No"))
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit time sheet " + isTimeSheet + " to " + (editEmp.IsTimeSheet == true ? "Yes" : "No") + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", isTimeSheet, (editEmp.IsTimeSheet == true ? "Yes" : "No"));

                        string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                       editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                       " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                       " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                    }
                    //Archana Srishti  11-02-2020 Add extra data into Employee table 
                    if (PG_Id != editEmp.Productgroup_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var pgOld = _entity.tb_ProductGroup.Where(x => x.PG_Id == PG_Id && x.IsActive == true).FirstOrDefault();
                        var pgNew = _entity.tb_ProductGroup.Where(x => x.PG_Id == editEmp.Productgroup_Id && x.IsActive == true).FirstOrDefault();
                        if (pgOld != null && pgNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + " edit Product Group  " + pgOld == null ? "" : pgOld.PG_Name + " to " + pgNew == null ? "" : pgNew.PG_Name + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", pgOld == null ? "" : pgOld.PG_Name, pgNew == null ? "" : pgNew.PG_Name);
                            string content = "Admin " + Session["username"] + " edit Product Group  " + pgOld.PG_Name + " to " + pgNew.PG_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", pgOld == null ? "" : pgOld.PG_Name, pgNew == null ? "" : pgNew.PG_Name);
                            //end
                        }
                        else if (pgOld == null && pgNew != null)
                        {
                            string content = "Admin " + Session["username"] + " edit Product Group  " + "None" + " to " + pgNew == null ? "" : pgNew.PG_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", pgNew == null ? "" : pgNew.PG_Name);
                        }
                        else if (pgOld != null && pgNew == null)
                        {
                            string content = "Admin " + Session["username"] + " edit Product Group  " + pgOld == null ? "" : pgOld.PG_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", pgOld == null ? "" : pgOld.PG_Name, "None");
                        }
                    }
                    if (BusinessLine_Id != editEmp.Businessline_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var blOld = _entity.tb_BusinessLine.Where(x => x.BL_Id == BusinessLine_Id && x.IsActive == true).FirstOrDefault();
                        var blNew = _entity.tb_BusinessLine.Where(x => x.BL_Id == editEmp.Businessline_Id && x.IsActive == true).FirstOrDefault();
                        if (blOld != null && blNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + " edit Business Line  " + blOld == null ? "" : blOld.Business_Line_Name + " to " + blNew == null ? "" : blNew.Business_Line_Name + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", blOld == null ? "" : blOld.Business_Line_Name, blNew == null ? "" : blNew.Business_Line_Name);
                            string content = "Admin " + Session["username"] + " edit Business Line  " + blOld.Business_Line_Name + " to " + blNew.Business_Line_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", blOld == null ? "" : blOld.Business_Line_Name, blNew == null ? "" : blNew.Business_Line_Name);
                            //end

                        }
                        else if (blOld == null && blNew != null)
                        {
                            string content = "Admin " + Session["username"] + " edit Business Line  " + "None" + " to " + blNew == null ? "" : blNew.Business_Line_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", blNew == null ? "" : blNew.Business_Line_Name);
                        }
                        else if (blOld != null && blNew == null)
                        {
                            string content = "Admin " + Session["username"] + " edit Business Line  " + blOld == null ? "" : blOld.Business_Line_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", blOld == null ? "" : blOld.Business_Line_Name, "None");
                        }
                    }
                    if (Business_Id != editEmp.Business_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var bOld = _entity.tb_Business.Where(x => x.Bus_Id == Business_Id && x.IsActive == true).FirstOrDefault();
                        var bNew = _entity.tb_Business.Where(x => x.Bus_Id == editEmp.Business_Id && x.IsActive == true).FirstOrDefault();
                        if (bOld != null && bNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + " edit Business " + bOld == null ? "" : bOld.Business_Name + " to " + bNew == null ? "" : bNew.Business_Name + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", bOld == null ? "" : bOld.Business_Name, bNew == null ? "" : bNew.Business_Name);
                            string content = "Admin " + Session["username"] + " edit Business " + bOld.Business_Name + " to " + bNew.Business_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", bOld == null ? "" : bOld.Business_Name, bNew == null ? "" : bNew.Business_Name);
                            //end
                        }
                        else if (bOld == null && bNew != null)
                        {
                            string content = "Admin " + Session["username"] + " edit Business " + "None" + " to " + bNew == null ? "" : bNew.Business_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", bNew == null ? "" : bNew.Business_Name);
                        }
                        else if (bOld != null && bNew == null)
                        {
                            string content = "Admin " + Session["username"] + " edit Business " + bOld == null ? "" : bOld.Business_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", bOld == null ? "" : bOld.Business_Name, "None");
                        }
                    }
                    if (Location_Id != editEmp.Location_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var lOld = _entity.tb_Location.Where(x => x.Location_Id == Location_Id && x.IsActive == true).FirstOrDefault();
                        var lNew = _entity.tb_Location.Where(x => x.Location_Id == editEmp.Location_Id && x.IsActive == true).FirstOrDefault();

                        if (lOld != null && lNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit location  " + lOld == null ? "" : lOld.Location + " to " + lNew == null ? "" : lNew.Location + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", lOld == null ? "" : lOld.Location, lNew == null ? "" : lNew.Location);
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit location  " + lOld.Location + " to " + lNew.Location + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", lOld == null ? "" : lOld.Location, lNew == null ? "" : lNew.Location);
                            //end


                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (lOld == null && lNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit location  " + "None" + " to " + lNew == null ? "" : lNew.Location + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", lNew == null ? "" : lNew.Location);

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (lOld != null && lNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit location  " + lOld == null ? "" : lOld.Location + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", lOld == null ? "" : lOld.Location, "None");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    if (Company_Id != editEmp.Company_Id)// 19-02-2020 II Archana Srishti 
                    {
                        var cOld = _entity.tb_Company.Where(x => x.Company_Id == Company_Id && x.IsActive == true).FirstOrDefault();
                        var cNew = _entity.tb_Company.Where(x => x.Company_Id == editEmp.Company_Id && x.IsActive == true).FirstOrDefault();
                        if (cOld != null && cNew != null)
                        {
                            //31/03/2020    commented below code and added new by Alena sics since the log displays in wrong format    // start
                            //string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit company  " + cOld == null ? "" : cOld.Company_Name + " to " + cNew == null ? "" : cNew.Company_Name + " on " + CurrentTime;
                            //bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", cOld == null ? "" : cOld.Company_Name, cNew == null ? "" : cNew.Company_Name);
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit company  " + cOld.Company_Name + " to " + cNew.Company_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", cOld == null ? "" : cOld.Company_Name, cNew == null ? "" : cNew.Company_Name);
                            //end

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (cOld == null && cNew != null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit company  " + "None" + " to " + cNew == null ? "" : cNew.Company_Name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", "None", cNew == null ? "" : cNew.Company_Name);

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                        else if (cOld != null && cNew == null)
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit company  " + cOld == null ? "" : cOld.Company_Name + " to " + "None" + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", cOld == null ? "" : cOld.Company_Name, "None");

                            string record = editEmp.GblEmp_ID + " || " + editEmp.Emp_Name + " || " + editEmp.CC_Id + " || " + editEmp.Profile_ID + " || " + editEmp.Gender + " || " + editEmp.LocalEmplyee_ID + " || " + editEmp.Job_Tittle_Id + " || " +
                           editEmp.Global_Group + " || " + editEmp.Local_Group + " || " + editEmp.Department_Id + " || " + editEmp.Location_Id + " || " + editEmp.Tele_Ext + " || " + editEmp.Mobile_No + " || " + editEmp.Date_Join + " || " + editEmp.PositionClass_ID + " || " + editEmp.EmployeeType_ID +
                           " || " + editEmp.DelegationBand + " || " + editEmp.ADAccount + " || " + editEmp.eMail + " || " + editEmp.Company_Id + " || " + editEmp.DelegationFlag + " || " + editEmp.Delegate_Emp_Code + " || " + editEmp.Vendor_Id + " || " + editEmp.Line_Manager + " || " + editEmp.IsActive + " || " +
                           " || " + editEmp.TimeStamp + " || " + editEmp.IsAdmin + " || " + editEmp.Country_Id + " || " + editEmp.Business_Id + " || " + editEmp.Businessline_Id + " || " + editEmp.Productgroup_Id + " || " + editEmp.Admin_Access + " || " + editEmp.IsAppraisal + " || " + editEmp.IsTimeSheet;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Employee", editEmp.Id.ToString(), "Admin");
                        }
                    }
                    if (mobile_extention != editEmp.MobileExtension)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edit mobile extention " + mobile_extention + " to " + editEmp.MobileExtension + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Employee", editEmp.LocalEmplyee_ID.ToString(), "Edit", mobile_extention, editEmp.MobileExtension);
                    }
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WFTemplateHome(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                WFTemplateModel model = new WFTemplateModel();
                model.admin_employee_local_id = permission;
                model.tittle = "Add";
                model.btn_Text = "Create";
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
        }

        public object AddWFTemplateMain(WFTemplateModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.id == null || model.id == 0)
            {
                #region Add
                if (_entity.tb_WFTemplateMain.Any(x => x.Template_Name.Trim().ToLower() == model.template_name.Trim().ToLower() && x.IsActive == true))
                {
                    msg = "Template Name is already exits !";
                }
                // 15/04/2020 code by Alena Sics
                else if (_entity.tb_WFTemplateMain.Any(x => x.WfType_Id == model.wftype_id && x.IsActive == true))
                {
                    msg = "WF Type already exists";
                }       //end
                else
                {
                    var tm = _entity.tb_WFTemplateMain.Create();
                    tm.Template_Name = model.template_name;
                    tm.WfType_Id = model.wftype_id;
                    tm.IsActive = true;
                    tm.TimeStamp = CurrentTime;
                    _entity.tb_WFTemplateMain.Add(tm);
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "WF Template added successfuly";
                        #region Keep Log
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added WF template " + model.template_name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFTemplateMain", tm.Id.ToString(), "Add");

                        string record = tm.Template_Name + " || " + tm.WfType_Id + " || " + tm.IsActive + " || " + tm.TimeStamp;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFTemplateMain", tm.Id.ToString(), "Admin");
                        #endregion Keep Log
                    }
                }
                #endregion
            }
            else
            {
                #region Edit
                if (_entity.tb_WFTemplateMain.Any(x => x.Template_Name.Trim().ToLower() == model.template_name.Trim().ToLower() && x.IsActive == true && x.Id != model.id))
                {
                    msg = "WF Template already exits !";
                }
                else
                {
                    string template_name = "";
                    long wftype = 0;
                    long country_Id = 0;

                    var data = _entity.tb_WFTemplateMain.Where(x => x.Id == model.id && x.IsActive == true).FirstOrDefault();
                    #region ChagesChecking
                    if (data.Template_Name != model.template_name)
                    {
                        template_name = data.Template_Name;
                    }
                    if (data.WfType_Id != model.wftype_id)
                    {
                        wftype = data.WfType_Id;
                    }
                    #endregion

                    data.Template_Name = model.template_name;
                    data.WfType_Id = model.wftype_id;
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "WF Template Edit Sucessfully";
                        #region Keep template name Edit
                        if (template_name != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template name " + template_name + " to " + model.template_name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFTemplateMain", data.Id.ToString(), "Edit", template_name, model.template_name);

                            string record = data.Template_Name + " || " + data.WfType_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFTemplateMain", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep wf type Edit
                        if (wftype != 0)
                        {
                            var old = _entity.tb_WFType.Where(x => x.Id == wftype).FirstOrDefault();
                            var newType = _entity.tb_WFType.Where(x => x.Id == model.wftype_id).FirstOrDefault();
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type " + old.WF_ID + " to " + newType.WF_ID + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFTemplateMain", data.Id.ToString(), "Edit", old.WF_ID, newType.WF_ID);

                            string record = data.Template_Name + " || " + data.WfType_Id + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFTemplateMain", data.Id.ToString(), "Admin");
                        }
                        #endregion
                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TemplateHeadList(string id)
        {
            WFTemplateModel model = new WFTemplateModel();
            model._list = new List<WFTemplateList>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            int slNo = 1;
            if (searchType == 0)
            {
                #region 
                var data = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true).ToList();
                foreach (var item in data)
                {
                    WFTemplateList one = new WFTemplateList();
                    one.template = item.Template_Name;
                    one.country = item.tb_WFType.tb_Country.Country_Name + " ( " + item.tb_WFType.tb_Country.Country_Code + " )";
                    one.wftype = item.tb_WFType.WF_App_Name + " ( " + item.tb_WFType.WF_ID + " )";
                    one.id = item.Id;
                    one.slNo = slNo;
                    model._list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 1)// Search with WF Type
            {
                #region
                var data = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTemplateList one = new WFTemplateList();
                    one.template = item.Template_Name;
                    one.country = item.tb_WFType.tb_Country.Country_Name + " ( " + item.tb_WFType.tb_Country.Country_Code + " )";
                    one.wftype = item.tb_WFType.WF_App_Name + " ( " + item.tb_WFType.WF_ID + " )";
                    one.id = item.Id;
                    one.slNo = slNo;
                    model._list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 2)// Search with country
            {
                #region
                var data = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.tb_Country.Country_Name.Contains(searchItem) || x.tb_WFType.tb_Country.Country_Code.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTemplateList one = new WFTemplateList();
                    one.template = item.Template_Name;
                    one.country = item.tb_WFType.tb_Country.Country_Name + " ( " + item.tb_WFType.tb_Country.Country_Code + " )";
                    one.wftype = item.tb_WFType.WF_App_Name + " ( " + item.tb_WFType.WF_ID + " )";
                    one.id = item.Id;
                    one.slNo = slNo;
                    model._list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            return PartialView("~/Views/Admin/_pv_TemplateHeadList.cshtml", model);
        }

        public object WFTypeByCountry(string id)
        {
            bool status = false;
            string msg = "Failed";
            long Id = Convert.ToInt64(id);
            var wftype = _entity.tb_WFType.Where(x => x.Country_Id == Id && x.IsActive == true).ToList();
            if (wftype.Count > 0 && wftype != null)
            {
                status = true;
                msg = "Success";
            }
            return Json(new { status = status, msg = msg, list = wftype }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditTemplateMain(string id)
        {
            var permission = CheckAuth(id);
            WFTemplateModel model = new WFTemplateModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_WFTemplateMain.Where(x => x.Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.template_name = data.Template_Name;
                model.country_id = data.tb_WFType.tb_Country.Id;
                // 25/03/2020 Alena Srishti  //start
                model.Country_Name = data.tb_WFType.tb_Country.Country_Name;
                model.wf_type = data.tb_WFType.WF_App_Name;//end
                model.wftype_id = data.WfType_Id;
                model.id = data.Id;
            }
            return PartialView("~/Views/Admin/_pv_AddWFTemplate.cshtml", model);
        }

        public object DeleteTemplate(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            WFTemplateModel model = new WFTemplateModel();
            bool status = false;
            string msg = "Failed";
            long tem_id = Convert.ToInt64(id);
            var data = _entity.tb_WFTemplateMain.Where(x => x.Id == tem_id).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "WF Templatemain deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed WF template " + data.Template_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFTemplateMain", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TemplateFlowHome(string id)
        {
            var permission = CheckAuth(id);
            long mainId = Convert.ToInt64(id);
            wfTemplateflow model = new wfTemplateflow();
            model.admin_employee_local_id = permission;
            var maindata = _entity.tb_WFTemplateMain.Where(x => x.Id == mainId && x.IsActive == true).FirstOrDefault();
            model.mainid = mainId;
            model.template_name = maindata.Template_Name;
            model.country_id = maindata.tb_WFType.Country_Id ?? 0;
            model.country_name = maindata.tb_WFType.tb_Country.Country_Name + " ( " + maindata.tb_WFType.tb_Country.Country_Code + " )";
            model.wf_id = maindata.WfType_Id;
            model.wftype = maindata.tb_WFType.WF_App_Name + " ( " + maindata.tb_WFType.WF_ID + " )";
            model.details_id = 0;
            model.tittle = "Add";
            model.btn_Text = "Create";
            return View(model);
        }
        public object AddWFTemplateFlow(wfTemplateflow model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.details_id == null || model.details_id == 0)
            {
                #region Add
                var data = _entity.tb_WF_Template.Create();
                data.WF_Template_ID = model.mainid;
                data.WF_Template_Name = model.template_name;
                data.WF_ID = model.wf_id;
                //data.Country_Id = model.country_id;
                data.Role_ID = model.roleid;
                data.Profile_ID = model.profile_id;
                data.Sequence_NO = model.sequence;
                data.Status_ID = model.status_id;
                data.Template_Flag = "1";
                data.DistributionList_ID = model.distribution_id;
                if (data.Action_Flag == 3)
                {
                    model.actionflag = 0;
                }
                data.Action_Flag = Convert.ToInt32(model.actionflag);
                data.IsActive = true;
                data.TimeStamp = CurrentTime;
                data.Edit_Option = model.can_edit == 0 ? "R" : "E";
                data.Button_List = model.button_list_string;
                _entity.tb_WF_Template.Add(data);
                status = _entity.SaveChanges() > 0;
                if (status)
                {
                    msg = "Successful";
                    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " add new WF template step in  " + model.template_name + " on " + CurrentTime;
                    bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Add");

                    string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                }
                #endregion 
            }
            else
            {
                #region Edit 
                long roleId = 0;
                long profileId = 0;
                int sequence = 0;
                string template_status = "";
                long distr = 0;
                string edit = "";
                string extra = "";
                string removed = "";
                var data = _entity.tb_WF_Template.Where(x => x.Id == model.details_id && x.IsActive == true).FirstOrDefault();

                #region Check List
                if (data.Role_ID != model.roleid)
                {
                    roleId = data.Role_ID ?? 0;
                }
                if (data.Profile_ID != model.profile_id)
                {
                    profileId = data.Profile_ID ?? 0;
                }
                if (data.Sequence_NO != model.sequence)
                {
                    sequence = data.Sequence_NO ?? 0;
                }
                if (data.DistributionList_ID != model.distribution_id)
                {
                    distr = data.DistributionList_ID ?? 0;
                }
                if (data.Status_ID != model.status_id)
                {
                    template_status = data.Status_ID;
                }
                if (data.Button_List != model.button_list_string)
                {
                    if (data.Button_List != null && data.Button_List != string.Empty)
                    {
                        var oldList = data.Button_List.Split('~').ToList();
                        var newList = model.button_list_string.Split('~').ToList();
                        var removedList = oldList.Where(x => !newList.Any(y => y == x)).ToList();//Have in old, not now
                        var extraList = newList.Where(x => !oldList.Any(y => y == x)).ToList();// Have now,but not in old
                        if (extraList != null && extraList.Count > 0)
                        {
                            extra = extraList.Aggregate((x, y) => x + "," + y);
                        }
                        if (removedList != null && removedList.Count > 0)
                        {
                            removed = removedList.Aggregate((x, y) => x + "," + y);
                        }
                    }
                    else if (model.button_list_string != null && model.button_list_string != string.Empty)
                    {
                        var newList = model.button_list_string.Split('~').ToList();
                        extra = newList.Aggregate((x, y) => x + "," + y);
                    }

                }
                var ab = Convert.ToInt32(model.can_edit);
                if (ab == 1 && data.Edit_Option == "R")
                {
                    edit = "Read Only";
                }
                else if (ab == 0 && data.Edit_Option == "E")
                {
                    edit = "Edit";
                }
                #endregion Check List

                data.Role_ID = model.roleid;
                data.Profile_ID = model.profile_id;
                data.Sequence_NO = model.sequence;
                if (model.distribution_id == 0)
                {
                    data.DistributionList_ID = null;
                }
                else
                {
                    data.DistributionList_ID = model.distribution_id;
                }
                data.Edit_Option = Convert.ToInt32(model.can_edit) == 1 ? "E" : "R";
                data.Action_Flag = Convert.ToInt32(model.actionflag);
                data.Button_List = model.button_list_string;
                data.Status_ID = model.status_id;
                try
                {
                    status = _entity.SaveChanges() > 0;
                }
                catch (Exception ex)
                {
                    var xx = ex.Message;
                }
                if (status)
                {
                    msg = "WF Template Edit Sucessfully";
                    #region Keep Role Edit
                    if (roleId != 0)
                    {
                        var oldRole = _entity.tb_Role.Where(x => x.Id == roleId).FirstOrDefault();
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template role " + (oldRole != null ? oldRole.Role_Desc : "") + " to " + data.tb_Role.Role_Desc + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", (oldRole != null ? oldRole.Role_Desc : ""), data.tb_Role.Role_Desc);

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion

                    #region Keep Profile Edit
                    if (profileId != 0 && profileId != null)
                    {
                        var oldProfile = _entity.tb_Emp_Profile.Where(x => x.Id == profileId).FirstOrDefault();
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template profile " + (oldProfile != null ? oldProfile.Profile_Desc : "") + " to " + (data.Profile_ID != null ? data.tb_Emp_Profile.Profile_Desc : "") + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", (oldProfile != null ? oldProfile.Profile_Desc : ""), (data.Profile_ID != null ? data.tb_Emp_Profile.Profile_Desc : ""));

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion

                    #region Keep Sequence Edit
                    if (sequence != 0)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template sequence number " + sequence + " to " + data.Sequence_NO + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", sequence.ToString(), data.Sequence_NO.ToString());

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion

                    #region Keep Distribution Edit
                    if (distr != 0 && distr != null)
                    {
                        var oldDis = _entity.tb_DistributionList.Where(x => x.DistributionList_Code == distr).FirstOrDefault();
                        var newDis = _entity.tb_DistributionList.Where(x => x.DistributionList_Code == data.DistributionList_ID && x.IsActive == true).FirstOrDefault();
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template distribution " + (oldDis != null ? oldDis.tb_WFType.WF_ID : "") + " to " + (newDis != null ? newDis.tb_WFType.WF_ID : "") + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", (oldDis != null ? oldDis.tb_WFType.WF_ID : ""), (newDis != null ? newDis.tb_WFType.WF_ID : ""));

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion

                    #region Keep Status Edit
                    if (template_status != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template status " + template_status + " to " + data.Status_ID + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", template_status, data.Status_ID);

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion

                    #region Keep Access Type Edit
                    if (edit != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF template access type " + edit + " to " + (data.Edit_Option == "E" ? "Edit" : "Read") + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", edit, (data.Edit_Option == "E" ? "Edit" : "Read"));

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion

                    #region Keep Button List Edit
                    if (extra != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added extra " + extra + " buttons on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", extra, model.button_list_string);

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    if (removed != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " removed  " + removed + " buttons on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Edit", removed, model.button_list_string);

                        string record = data.WF_Template_ID + " || " + data.WF_Template_Name + " || " + data.WF_ID + " || " + data.Role_ID + " || " + data.Profile_ID + " || " + data.Sequence_NO + " || " + data.Status_ID + data.Template_Flag + "|| " + " || " + data.DistributionList_ID + " || " + data.Button_List + " || " + data.Action_Flag + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.Edit_Option;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WF_Template", data.Id.ToString(), "Admin");
                    }
                    #endregion
                }
                #endregion
            }

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult TemplateDetailsList(string id)
        {
            //LocationList model = new LocationList();
            //model.list = new List<LocationModel>();
            //wfTemplateflow model = new wfTemplateflow();
            //model.list = new List<Template_Flow_Details>();
            wfTemplateflow model = new wfTemplateflow();
            model.list = new List<Template_Flow_Details>();
            //
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            var mainId = Convert.ToInt64(splitData[2]);
            int slNo = 1;
            if (searchType == 0)
            {
                #region 
                // 17/06/2020 ALENA SICS COMMENTED BELOW CODE AND ADDED NEW CODE BELOW 
                // var data = _entity.tb_WF_Template.Where(x => x.IsActive == true && x.WF_Template_ID == mainId).ToList();
                var data = _entity.tb_WF_Template.Where(x => x.IsActive == true && x.WF_Template_ID == mainId && x.DistributionList_ID != null).ToList(); //END------
                foreach (var item in data)
                {
                    Template_Flow_Details one = new Template_Flow_Details();
                    one.slno = slNo;
                    one.id = item.Id;
                    one.role = item.tb_Role.Role_Desc + " ( " + item.tb_Role.Role_ID + " )";
                    one.profile = item.Profile_ID == null ? "" : item.tb_Emp_Profile.Profile_Desc + " ( " + item.tb_Emp_Profile.Profile_ID + " )";
                    one.sequence_number = item.Sequence_NO.ToString();
                    // 13/04/2020 code commented by Alena sics and added new code
                    //one.distribution = item.DistributionList_ID == null ? "" : item.tb_WFType.WF_ID;
                    var distribution = item.DistributionList_ID;
                    var s = _entity.tb_DistributionList.Where(xx => xx.DistributionList_Code == item.DistributionList_ID).FirstOrDefault();
                    var orderno = s.Order_No;
                    var wftypename = _entity.tb_WF_Template.Where(item1 => item1.WF_ID == item.WF_ID).FirstOrDefault();
                    long wftype = wftypename.WF_ID;
                    string WFtypename = wftypename.tb_WFType.WF_ID;
                    string k = string.Join("-", WFtypename, orderno);
                    one.distribution = k.ToString();
                    // end

                    one.edit_option = item.Edit_Option == "R" ? "Read Access" : "Edit Access";
                    one.status = item.Status_ID;
                    one.head_id = item.WF_Template_ID ?? 0;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 1)// Search with Role
            {
                #region
                var data = _entity.tb_WF_Template.Where(x => x.IsActive == true && x.WF_Template_ID == mainId && (x.tb_Role.Role_Desc.Contains(searchItem) || x.tb_Role.Role_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    Template_Flow_Details one = new Template_Flow_Details();
                    one.slno = slNo;
                    one.id = item.Id;
                    one.role = item.tb_Role.Role_Desc + " ( " + item.tb_Role.Role_ID + " )";
                    one.profile = item.Profile_ID == null ? "" : item.tb_Emp_Profile.Profile_Desc + " ( " + item.tb_Emp_Profile.Profile_ID + " )";
                    one.sequence_number = item.Sequence_NO.ToString();
                    one.distribution = item.DistributionList_ID == null ? "" : _entity.tb_DistributionList.Where(x => x.DistributionList_Code == item.DistributionList_ID && x.IsActive == true).FirstOrDefault().tb_WFType.WF_ID;
                    one.edit_option = item.Edit_Option == "R" ? "Read Access" : "Edit Access";
                    one.head_id = item.WF_Template_ID ?? 0;
                    one.status = item.Status_ID;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            return PartialView("~/Views/Admin/_pv_TemplateFlowList.cshtml", model);
        }

        public PartialViewResult EditTemplateFlow(string id)
        {
            var permission = CheckAuth(id);
            wfTemplateflow model = new wfTemplateflow();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_WF_Template.Where(x => x.Id == Id).FirstOrDefault();
            model.actionflag = (Template_Table_Action_Flag)data.Action_Flag;
            if (data.Edit_Option.Trim() == "E")
                model.can_edit = BooleanValue.Yes;
            else
                model.can_edit = BooleanValue.No;
            model.country_id = data.tb_WFTemplateMain.tb_WFType.Country_Id ?? 0;
            model.country_name = data.tb_WFTemplateMain.tb_WFType.tb_Country.Country_Name ?? "";
            model.distribution_id = data.DistributionList_ID ?? 0;
            model.details_id = data.Id;
            model.mainid = data.WF_Template_ID ?? 0;
            model.profile_id = data.Profile_ID;
            model.roleid = data.Role_ID ?? 0;
            model.sequence = data.Sequence_NO ?? 0;
            model.status_id = data.Status_ID;
            model.template_name = data.WF_Template_Name;
            model.wftype = data.tb_WFType.WF_ID;
            model.wf_id = data.WF_ID;
            model.button_list_string = data.Button_List;
            return PartialView("~/Views/Admin/_pv_AddTemplate_Flow.cshtml", model);
        }
        public object DeleteTemplateFlow(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            wfTemplateflow model = new wfTemplateflow();
            bool status = false;
            string msg = "Failed";
            long tem_id = Convert.ToInt64(id);
            var data = _entity.tb_WF_Template.Where(x => x.Id == tem_id).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Template Flow deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed WF templateflow " + data.WF_Template_Name + " ( " + data.tb_Role.Role_ID + " )" + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WF_Template", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //Basheer on 12-12-2019 role
        public ActionResult Role(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                RoleNewModel model = new RoleNewModel();
                model.admin_employee_local_id = permission;
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.employeegroup = new List<EmployeeGroup>();
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
        }
        [HttpGet]
        public PartialViewResult GetOrganizationType(string id)
        {
            var model = new Models.RoleNewModel();
            long countryid;
            string[] splitData = id.Split('~');
            var orgtype = Convert.ToString(splitData[0]);


            if (orgtype == "0")
            {
                model.OrganizationType = 0;
                return PartialView("~/Views/Admin/_pv_RoleSub_Organization_Yes.cshtml", model);
            }
            else
            {
                countryid = Convert.ToInt32(splitData[1]);
                model.CountryId = countryid;
                model.OrganizationType = 1;
                return PartialView("~/Views/Admin/_pv_RoleSub_Organization_No.cshtml", model);
            }
            // TempData["LEVELID"]= testId;

        }
        [HttpGet]
        public PartialViewResult GetViaEmployeeType(string id)
        {
            string[] splitData = id.Split('~');
            var employeetype = Convert.ToString(splitData[0]);
            long countryid = Convert.ToInt32(splitData[1]);

            var model = new Models.RoleNewModel();
            model.CountryId = countryid;
            if (employeetype == "1")
            {
                model.EmployeeType = 1;
                return PartialView("~/Views/Admin/_pv_RoleSub_Organization_No_DirectEmployee.cshtml", model);
            }
            else
            {
                TempData["EmployeeGroup"] = null;
                model.EmployeeType = 0;
                return PartialView("~/Views/Admin/_pv_RoleSub_Organization_No_indirectEmployee.cshtml", model);
            }
        }

        public PartialViewResult EmployeeGroupListAdd(RoleNewModel model)
        {
            model.EmployeeType = 0;

            if (TempData["EmployeeGroup"] == null)
            {
                TempData["EmployeeGroup"] = model.employeegroup;
            }
            else
            {
                var list = (List<EmployeeGroup>)TempData["EmployeeGroup"];
                foreach (var item in list)
                {
                    EmployeeGroup one = new EmployeeGroup();
                    one.EmployeeID = item.EmployeeID;
                    one.Code = item.Code;
                    one.TableName = item.TableName;
                    one.Description = _entity.tb_WF_Employee.Where(x => x.IsActive == true && x.LocalEmplyee_ID == item.EmployeeID).FirstOrDefault().Emp_Name;
                    model.employeegroup.Add(one);
                }

                TempData["EmployeeGroup"] = "";
                TempData["EmployeeGroup"] = model.employeegroup;
            }

            return PartialView("~/Views/Admin/_pv_RoleSub_Organization_No_indirectEmployee.cshtml", model);
        }
        public PartialViewResult DeleteGroupEmployee(RoleNewModel model)
        {
            var list = model.employeegroup;
            var itemToRemove = list.Where(r => r.Code == model.DeleteCode).FirstOrDefault();
            model.employeegroup.Remove(itemToRemove);
            TempData["EmployeeGroup"] = model.employeegroup;
            return PartialView("~/Views/Admin/_pv_RoleSub_Organization_No_indirectEmployee.cshtml", model);
        }

        public object AddRoleInfo(RoleNewModel model)
        {
            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {
                #region ADD
                model.employeegroup = (List<EmployeeGroup>)TempData["EmployeeGroup"];
                if (model.CountryId != 0)
                {
                    if (model.RoleId != null)
                    {
                        if (model.RoleTitle != null)
                        {
                            if (model.ApplicationId != 0)
                            {
                                if (model.OrganizationType == 0)
                                {
                                    if (_entity.tb_Role.Any(x => x.Role_ID == model.RoleId && x.IsActive == true))
                                    {
                                        msg = "Role already exits !";
                                    }
                                    else
                                    {
                                        var bs = _entity.tb_Role.Create();
                                        bs.Country_ID = model.CountryId ?? 0;
                                        bs.Role_ID = model.RoleId;
                                        bs.Role_Desc = model.RoleTitle;
                                        if (model.OrganizationType == 1)
                                        {
                                            bs.Organization_Flag = false;
                                        }
                                        else
                                        {
                                            bs.Organization_Flag = true;
                                        }
                                        //bs.Organization_Flag = Convert.ToBoolean(model.OrganizationType);
                                        //  bs.Application_ID = model.ApplicationId;
                                        bs.org_type = model.RoleTable.ToString();
                                        bs.role_type = model.CoulmnName.ToString();
                                        //bs.GroupRole = Convert.ToBoolean(0);
                                        bs.IsActive = true;
                                        bs.TimeStamp = CurrentTime;
                                        _entity.tb_Role.Add(bs);
                                        status = _entity.SaveChanges() > 0;
                                        if (status)
                                        {
                                            msg = "Role added successfuly";
                                            #region Keep Log
                                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added role " + model.RoleTitle + " on " + CurrentTime;
                                            bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Role", bs.Role_ID.ToString(), "Add");

                                            string record = bs.Role_ID + " || " + bs.Role_Desc + " || " + bs.Organization_Flag + " || " + bs.Assigned_ID + " || " + bs.Column_Position + " || " + bs.IsActive + " || " + bs.TimeStamp + " || " + bs.role_type + " || " + bs.org_type + " || " + bs.Country_ID + " || " + bs.GroupRole;
                                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Role", bs.Role_ID.ToString(), "Admin");
                                            #endregion Keep Log
                                        }
                                    }

                                }
                                else
                                {
                                    if (model.GroupRole != 0)
                                    {
                                        if (model.EmployeeType == 1)
                                        {
                                            if (model.EmployeeId != "")
                                            {
                                                if (_entity.tb_Role.Any(x => x.Role_ID == model.RoleId && x.IsActive == true))
                                                {
                                                    msg = "Role already exits !";
                                                }
                                                else
                                                {
                                                    var bs = _entity.tb_Role.Create();
                                                    bs.Country_ID = model.CountryId ?? 0;
                                                    bs.Role_ID = model.RoleId;
                                                    bs.Role_Desc = model.RoleTitle;
                                                    if (model.OrganizationType == 1)
                                                    {
                                                        bs.Organization_Flag = false;
                                                    }
                                                    else
                                                    {
                                                        bs.Organization_Flag = true;
                                                    }
                                                    //bs.Organization_Flag = Convert.ToBoolean(model.OrganizationType);
                                                    // bs.Application_ID = model.ApplicationId;
                                                    //bs.org_type = model.RoleTable.ToString();
                                                    //bs.role_type = model.CoulmnName.ToString();
                                                    if (model.GroupRole == 2)
                                                    {
                                                        bs.GroupRole = false;
                                                    }
                                                    else
                                                    {
                                                        bs.GroupRole = true;
                                                    }
                                                    //bs.GroupRole = Convert.ToBoolean(model.GroupRole);
                                                    bs.Assigned_ID = model.EmployeeId;
                                                    bs.IsActive = true;
                                                    bs.TimeStamp = CurrentTime;
                                                    _entity.tb_Role.Add(bs);
                                                    status = _entity.SaveChanges() > 0;
                                                    if (status)
                                                    {
                                                        msg = "Role added successfuly";
                                                        #region Keep Log
                                                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added role " + model.RoleTitle + " on " + CurrentTime;
                                                        bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Role", bs.Role_ID.ToString(), "Add");

                                                        string record = bs.Role_ID + " || " + bs.Role_Desc + " || " + bs.Organization_Flag + " || " + bs.Assigned_ID + " || " + bs.Column_Position + " || " + bs.IsActive + " || " + bs.TimeStamp + " || " + bs.role_type + " || " + bs.org_type + " || " + bs.Country_ID + " || " + bs.GroupRole;
                                                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Role", bs.Role_ID.ToString(), "Admin");
                                                        #endregion Keep Log
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                msg = "Please Choose Employee!";
                                            }

                                        }
                                        else
                                        {
                                            //group employee grid save
                                            if (model.employeegroup.Count() > 0)
                                            {
                                                if (_entity.tb_Role.Any(x => x.Role_ID == model.RoleId && x.IsActive == true))
                                                {
                                                    msg = "Role already exits !";
                                                }
                                                else
                                                {
                                                    var bs = _entity.tb_Role.Create();
                                                    bs.Country_ID = model.CountryId ?? 0;
                                                    bs.Role_ID = model.RoleId;
                                                    bs.Role_Desc = model.RoleTitle;
                                                    if (model.OrganizationType == 1)
                                                    {
                                                        bs.Organization_Flag = false;
                                                    }
                                                    else
                                                    {
                                                        bs.Organization_Flag = true;
                                                    }
                                                    //bs.Organization_Flag = Convert.ToBoolean(model.OrganizationType);
                                                    //bs.Application_ID = model.ApplicationId;
                                                    //bs.org_type = model.RoleTable.ToString();
                                                    //bs.role_type = model.CoulmnName.ToString();
                                                    if (model.GroupRole == 2)
                                                    {
                                                        bs.GroupRole = false;
                                                    }
                                                    else
                                                    {
                                                        bs.GroupRole = true;
                                                    }
                                                    bs.Assigned_ID = model.EmployeeId;
                                                    bs.IsActive = true;
                                                    bs.TimeStamp = CurrentTime;
                                                    _entity.tb_Role.Add(bs);
                                                    status = _entity.SaveChanges() > 0;
                                                    //Save grid details
                                                    if (status)
                                                    {
                                                        foreach (var item in model.employeegroup)
                                                        {
                                                            var universaltable = _entity.tb_UniversalLookupTable.Create();
                                                            universaltable.Code = item.Code;
                                                            universaltable.Table_Name = item.TableName;
                                                            universaltable.Description = _entity.tb_WF_Employee.Where(x => x.IsActive == true && x.LocalEmplyee_ID == item.EmployeeID).FirstOrDefault().LocalEmplyee_ID;
                                                            universaltable.IsActive = true;
                                                            universaltable.TimeStamp = CurrentTime;
                                                            _entity.tb_UniversalLookupTable.Add(universaltable);
                                                            status = _entity.SaveChanges() > 0;
                                                        }
                                                        if (status)
                                                        {
                                                            var table = _entity.tb_Tables.Create();
                                                            table.TableName = model.RoleId;
                                                            table.Description = "Role Created";
                                                            table.IsActive = true;
                                                            table.TimeStamp = CurrentTime;
                                                            _entity.tb_Tables.Add(table);
                                                            status = _entity.SaveChanges() > 0;

                                                        }
                                                        if (status)
                                                        {
                                                            msg = "Role added successfuly";
                                                            #region Keep Log
                                                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added role " + model.RoleTitle + " on " + CurrentTime;
                                                            bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Role", bs.Id.ToString(), "Add");

                                                            string record = bs.Role_ID + " || " + bs.Role_Desc + " || " + bs.Organization_Flag + " || " + bs.Assigned_ID + " || " + bs.Column_Position + " || " + bs.IsActive + " || " + bs.TimeStamp + " || " + bs.role_type + " || " + bs.org_type + " || " + bs.Country_ID + " || " + bs.GroupRole;
                                                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Role", bs.Role_ID.ToString(), "Admin");
                                                            #endregion Keep Log
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                msg = "Please Add Employe Details!";
                                            }

                                        }

                                    }
                                    else
                                    {
                                        msg = "Please Choose GroupRole!";
                                    }
                                }

                            }
                            else
                            {
                                msg = "Please Choose Application!";
                            }

                        }
                        else
                        {
                            msg = "Please Select RoleTitle!";
                        }

                    }
                    else
                    {
                        msg = "Please Select RoleID!";
                    }

                }
                else
                {
                    msg = "Please Choose Country!";
                }

                #endregion add
            }
            else
            {
                #region Edit
                var data = _entity.tb_Role.FirstOrDefault(z => z.Id == model.Id && z.IsActive == true);
                string roleid = data.Role_ID;
                string roletitle = data.Role_Desc;
                if (data != null)
                {
                    data.Role_ID = model.RoleId;
                    data.Role_Desc = model.RoleTitle;
                }
                status = _entity.SaveChanges() > 0 ? true : false;
                msg = msg = "Role edited successfuly";
                if (status)
                {
                    var data1 = _entity.tb_UniversalLookupTable.Where(z => z.Table_Name == roleid && z.IsActive == true).ToList();
                    if (data1.Count() > 0)
                    {
                        foreach (var item in data1)
                        {
                            item.Table_Name = model.RoleId;
                        }
                        status = _entity.SaveChanges() > 0 ? true : false;
                    }
                    var tableData = _entity.tb_Tables.Where(x => x.TableName == roleid && x.IsActive == true && x.Description == "Role Created").FirstOrDefault();
                    tableData.TableName = model.RoleId;//18-02-2020 02 ARCHANA 
                    _entity.SaveChanges();

                }

                if (status == true)
                {
                    if (roleid != model.RoleId)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited role " + roleid + " to " + model.RoleId + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Role", model.RoleId.ToString(), "Edit", roleid, model.RoleId);

                        string record = data.Role_ID + " || " + data.Role_Desc + " || " + data.Organization_Flag + " || " + data.Assigned_ID + " || " + data.Column_Position + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.role_type + " || " + data.org_type + " || " + data.Country_ID + " || " + data.GroupRole;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Role", data.Role_ID.ToString(), "Admin");
                    }
                    if (roletitle != model.RoleTitle)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited role title " + roletitle + " to " + model.RoleTitle + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Role", model.RoleId.ToString(), "Edit", roletitle, model.RoleTitle);

                        string record = data.Role_ID + " || " + data.Role_Desc + " || " + data.Organization_Flag + " || " + data.Assigned_ID + " || " + data.Column_Position + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.role_type + " || " + data.org_type + " || " + data.Country_ID + " || " + data.GroupRole;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Role", data.Role_ID.ToString(), "Admin");
                    }
                }
                #endregion Edit
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public object CheckRoleId(string roleid)
        {
            bool status = true; ;
            string msg = "ok";
            int count = _entity.tb_Role.Where(x => x.IsActive == true && x.Role_ID == roleid).Count();
            if (count > 0)
            {
                status = false;
                msg = "RoleId already exist";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }

        public object DeleteRoleMain(string id)
        {
            bool status = false;
            string msg = "Failed";
            int roleid = Convert.ToInt32(id);
            var data = _entity.tb_Role.Where(x => x.Id == roleid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Role deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " removed role " + data.Role_Desc + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_role", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult EditRole(string id)
        {
            var permission = CheckAuth(id);
            RoleNewModel model = new RoleNewModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_Role.Where(x => x.Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.CountryId = data.Country_ID;
                model.RoleId = data.Role_ID;
                model.RoleTitle = data.Role_Desc;
                // model.ApplicationId = data.Application_ID;
                model.OrganizationType = Convert.ToInt32(data.Organization_Flag);
                model.Id = Id;
                model.isEdit = true;
            }
            return PartialView("~/Views/Admin/_pv_AddRoleMain.cshtml", model);
        }

        //WfType starting Basheer 
        public ActionResult WFType(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                WFTypeModel model = new WFTypeModel();
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.admin_employee_local_id = permission;
                model.Escalation_Flag = 1;
                model.IsPaid_Request = 1;
                model.HaveProfile = 1;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }
        }

        public object AddWFType(WFTypeModel model)
        {
            // boolean value 1 means "NO" and 0 means  "YES"
            bool status = false;
            string msg = "Failed";

            if (model.Id == 0)
            {
                #region ADD               
                if (model.WF_Id != null)
                {
                    if (model.WF_Name != null)
                    {
                        if (model.Country_Id != 0)
                        {
                            if (model.Business_Id != 0)
                            {
                                if (model.BusinessLine_Id != 0)
                                {
                                    if (model.Application_Id != 0)
                                    {
                                        if (_entity.tb_WFType.Any(x => x.WF_ID == model.WF_Id && x.IsActive == true))
                                        {
                                            msg = "WF Type already exits !";
                                        }
                                        else
                                        {

                                            var data = _entity.tb_WFType.Create();
                                            data.WF_ID = model.WF_Id;
                                            data.WF_App_Name = model.WF_Name;
                                            data.Country_Id = model.Country_Id;
                                            // data.Business_Id = model.Business_Id;
                                            data.BusinessLine_Id = model.BusinessLine_Id;
                                            data.Application_ID = model.Application_Id;
                                            data.ProcessOwner = model.Employee_Id;
                                            data.Closing_Type_Id = model.Closing_Type;
                                            data.Escalation_Flag = Convert.ToBoolean(model.Escalation_Flag);
                                            data.IsPaid_Request = Convert.ToBoolean(model.IsPaid_Request);
                                            data.HaveProfile = Convert.ToBoolean(model.HaveProfile);
                                            //if (model.Escalation_Flag == 0)
                                            //{
                                            //    data.Escalation_Flag = true;
                                            //}
                                            //else
                                            //{
                                            //    data.Escalation_Flag = false;
                                            //}
                                            //if (model.IsPaid_Request == 0)
                                            //{
                                            //    data.IsPaid_Request = true;
                                            //}
                                            //else
                                            //{
                                            //    data.IsPaid_Request = false;
                                            //}
                                            //if (model.HaveProfile == 0)
                                            //{
                                            //    data.HaveProfile = true;
                                            //}
                                            //else
                                            //{
                                            //    data.HaveProfile = false;
                                            //}
                                            data.IsActive = true;
                                            data.TimeStamp = CurrentTime;
                                            _entity.tb_WFType.Add(data);
                                            status = _entity.SaveChanges() > 0;
                                            if (status)
                                            {
                                                msg = "WF Type added successfuly";
                                                #region Keep Log
                                                string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added WF type " + model.WF_Name + " on " + CurrentTime;
                                                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.WF_ID.ToString(), "Add");

                                                string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                                                bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                                                #endregion Keep Log
                                            }
                                        }

                                    }
                                    else
                                    {
                                        msg = "Please Choose Application!";
                                    }
                                }
                                else
                                {
                                    msg = "Please Choose Business Line!";
                                }
                            }
                            else
                            {
                                msg = "Please Choose Business!";
                            }
                        }
                        else
                        {
                            msg = "Please Choose Country!";
                        }
                    }
                    else
                    {
                        msg = "Please Choose WF Name!";
                    }
                }
                else
                {
                    msg = "Please Choose WF ID!";
                }
                #endregion add
            }
            else
            {
                #region EDIT

                string wfname = "";
                string wfid = "";
                long? country_Id = 0;
                long? businessline = 0;
                long? applicationid = 0;
                string employeeid = "";
                int isescalate = 2;
                int ispaidrequest = 2;
                int haveprofile = 2;
                long? closingtype = 0;


                int isescalatechange = 0;
                int ispaidrequestchange = 0;
                int haveprofilechange = 0;

                var data = _entity.tb_WFType.Where(x => x.Id == model.Id && x.IsActive == true).FirstOrDefault();
                ////Value changing from boolean to int for flags
                //if (data.Escalation_Flag == true)
                //{
                //    isescalatechange = 0;
                //}
                //else
                //{
                //    isescalatechange = 1;
                //}
                //if (data.IsPaid_Request == true)
                //{
                //    ispaidrequestchange = 0;
                //}
                //else
                //{
                //    ispaidrequestchange = 1;
                //}
                //if (data.HaveProfile == true)
                //{
                //    haveprofilechange = 0;
                //}
                //else
                //{
                //    haveprofilechange = 1;
                //}
                ////End changing


                #region ChagesChecking
                if (data.WF_ID != model.WF_Id)
                {
                    wfid = data.WF_ID;
                }
                if (data.WF_App_Name != model.WF_Name)
                {
                    wfname = data.WF_App_Name;
                }
                if (data.Country_Id != model.Country_Id)
                {
                    country_Id = data.Country_Id;
                }
                if (data.BusinessLine_Id != model.BusinessLine_Id)
                {
                    businessline = data.BusinessLine_Id;
                }
                if (data.Application_ID != model.Application_Id)
                {
                    applicationid = data.Application_ID;
                }
                if (data.ProcessOwner != model.Employee_Id)
                {
                    employeeid = data.ProcessOwner;
                }
                if (Convert.ToInt32(data.Escalation_Flag) != model.Escalation_Flag)
                {
                    isescalate = Convert.ToInt32(data.Escalation_Flag);
                }
                if (Convert.ToInt32(data.IsPaid_Request) != model.IsPaid_Request)
                {
                    ispaidrequest = Convert.ToInt32(data.IsPaid_Request);
                }
                if (Convert.ToInt32(data.HaveProfile) != model.HaveProfile)
                {
                    haveprofile = Convert.ToInt32(data.HaveProfile);
                }
                if (data.Closing_Type_Id != model.Closing_Type)
                {
                    closingtype = data.Closing_Type_Id;
                }

                #endregion

                data.WF_ID = model.WF_Id;
                data.WF_App_Name = model.WF_Name;
                // 25/03/2020 below line commented by Alena Srishti
                //data.Country_Id = model.Country_Id;
                data.BusinessLine_Id = model.BusinessLine_Id;
                // 25/03/2020 below line commented by Alena Srishti
                //data.Application_ID = model.Application_Id;
                data.ProcessOwner = model.Employee_Id;
                data.Closing_Type_Id = model.Closing_Type;
                data.Escalation_Flag = Convert.ToBoolean(model.Escalation_Flag);
                data.IsPaid_Request = Convert.ToBoolean(model.IsPaid_Request);
                data.HaveProfile = Convert.ToBoolean(model.HaveProfile);
                //if (model.Escalation_Flag == 0)
                //{
                //    data.Escalation_Flag = true;
                //}
                //else
                //{
                //    data.Escalation_Flag = false;
                //}
                //if (model.IsPaid_Request == 0)
                //{
                //    data.IsPaid_Request = true;
                //}
                //else
                //{
                //    data.IsPaid_Request = false;
                //}
                //if (model.HaveProfile == 0)
                //{
                //    data.HaveProfile = true;
                //}
                //else
                //{
                //    data.HaveProfile = false;
                //}
                status = _entity.SaveChanges() > 0;
                if (status)
                {
                    msg = "WFType Edit Sucessfully";
                    #region  EditLog
                    if (wfname != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type name " + wfname + " to " + model.WF_Name + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", wfname, model.WF_Name);

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (wfid != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type ID " + wfid + " to " + model.WF_Id + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", wfid, model.WF_Id);

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (country_Id != 0)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type country " + country_Id + " to " + model.Country_Id + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", country_Id.ToString(), model.Country_Id.ToString());

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (businessline != 0)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type businessline " + businessline + " to " + model.BusinessLine_Id + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", businessline.ToString(), model.BusinessLine_Id.ToString());

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    // 30/03/2020 commented by Alena sics
                    //if (applicationid != 0)
                    //{
                    //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type application " + applicationid + " to " + model.Application_Id + " on " + CurrentTime;
                    //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", applicationid.ToString(), model.Application_Id.ToString());

                    //    string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                    //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    //}
                    if (employeeid != "")
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type processowner " + employeeid + " to " + model.Employee_Id + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", employeeid, model.Employee_Id);

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (isescalate != 2)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type is escalate " + Convert.ToBoolean(isescalate) + " to " + Convert.ToBoolean(model.Escalation_Flag) + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", Convert.ToBoolean(isescalate).ToString(), Convert.ToBoolean(model.Escalation_Flag).ToString());

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (ispaidrequest != 2)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type ispaidrequest " + Convert.ToBoolean(ispaidrequest) + " to " + Convert.ToBoolean(model.IsPaid_Request) + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", Convert.ToBoolean(ispaidrequest).ToString(), Convert.ToBoolean(model.IsPaid_Request).ToString());

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (haveprofile != 2)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type haveprofile " + Convert.ToBoolean(haveprofile) + " to " + Convert.ToBoolean(model.HaveProfile) + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", Convert.ToBoolean(haveprofile).ToString(), Convert.ToBoolean(model.HaveProfile).ToString());

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    if (closingtype != 0)
                    {
                        string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited WF type closing type " + closingtype + " to " + model.Closing_Type + " on " + CurrentTime;
                        bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Edit", closingtype.ToString(), model.Closing_Type.ToString());

                        string record = data.WF_ID + " || " + data.WF_App_Name + " || " + data.Country_Id + " || " + data.BusinessLine_Id + " || " + data.Application_ID + " || " + data.Closing_Type_Id + " || " + data.WF_App_url + " || " + data.Escalation_Flag + " || " + data.ProcessOwner + " || " + data.IsActive + " || " + data.TimeStamp + " || " + data.IsPaid_Request + " || " + data.HaveProfile;
                        bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_WFType", data.WF_ID.ToString(), "Admin");
                    }
                    #endregion

                }
                else
                    msg = "No changes !";

                #endregion
            }

            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }
        public PartialViewResult WFTypeList(string id)
        {
            WFTypeModel model = new WFTypeModel();
            model.list = new List<WFTypeList>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            int slNo = 1;
            if (searchType == 0)
            {
                #region 
                var data = _entity.tb_WFType.Where(x => x.IsActive == true).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    /*-----------20/06/2020 ALENA SICS COMMENTED BELOW CODE AND ADDED NEW CODE------------*/
                    //  one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.BusinessLine_Id != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    else
                        one.BusinessLine = "";
                    /*end------------------------------------------------------------------------*/
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }

                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 1)
            {
                #region
                var data = _entity.tb_WFType.Where(x => x.IsActive == true && x.WF_ID.Contains(searchItem)).ToList();
                //var data1 = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }

                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 2)
            {
                #region
                var data = _entity.tb_WFType.Where(x => x.IsActive == true && x.WF_App_Name.Contains(searchItem)).ToList();
                //var data1 = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }

                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 3)
            {
                #region
                var data = _entity.tb_WFType.Where(x => x.IsActive == true && (x.tb_Country.Country_Name.Contains(searchItem))).ToList();
                //var data1 = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }

                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 4)
            {
                #region
                var data = _entity.tb_WFType.Where(x => x.IsActive == true && (x.tb_BusinessLine.Business_Line_Name.Contains(searchItem))).ToList();
                //var data1 = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }

                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 5)
            {
                #region
                var data = _entity.tb_WFType.Where(x => x.IsActive == true && (x.tb_Application.Application_Name.Contains(searchItem))).ToList();
                //var data1 = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }

                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 6)
            {
                #region
                var data = _entity.tb_WFType.Where(x => x.IsActive == true && (x.tb_Closing_Type.Description.Contains(searchItem))).ToList();
                //var data1 = _entity.tb_WFTemplateMain.Where(x => x.IsActive == true && (x.tb_WFType.WF_App_Name.Contains(searchItem) || x.tb_WFType.WF_ID.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    WFTypeList one = new WFTypeList();
                    one.Id = item.Id;
                    one.WF_Id = item.WF_ID;
                    one.WF_Name = item.WF_App_Name;
                    one.Country = item.tb_Country.Country_Name;
                    one.BusinessLine = item.tb_BusinessLine.Business_Line_Name;
                    if (item.Application_ID != null) // 21-02-2020 ARCHANA SRISHTI 
                        one.Application = item.tb_Application.Application_Name;
                    else
                        one.Application = "";
                    var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == item.ProcessOwner).FirstOrDefault();
                    if (employee != null)
                    {
                        one.Employee = employee.Emp_Name;
                    }
                    else
                    {
                        one.Employee = string.Empty;
                    }
                    one.Closing_Type = item.tb_Closing_Type.Description;
                    one.Escalation_Flag = item.Escalation_Flag;
                    one.IsPaid_Request = item.IsPaid_Request;
                    one.HaveProfile = item.HaveProfile;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            return PartialView("~/Views/Admin/_pv_WFTypeList.cshtml", model);
        }

        public object DeleteWFType(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            WFTypeModel model = new WFTypeModel();
            bool status = false;
            string msg = "Failed";
            long tem_id = Convert.ToInt64(id);
            var data = _entity.tb_WFType.Where(x => x.Id == tem_id).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "WFType deleted successfully";
                #region Keep Log
                string content = "Admin " + Session["username"] + " -" + model.admin_employee_local_id + " - " + " removed WF type " + data.WF_App_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_WFType", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EditWFType(string id)
        {
            var permission = CheckAuth(id);
            WFTypeModel model = new WFTypeModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long Id = Convert.ToInt64(id);
            var data = _entity.tb_WFType.Where(x => x.Id == Id && x.IsActive == true).FirstOrDefault();
            if (data != null)
            {
                model.Id = data.Id;
                model.WF_Id = data.WF_ID;
                model.WF_Name = data.WF_App_Name;
                model.Country_Id = data.Country_Id;
                model.BusinessLine_Id = data.BusinessLine_Id;
                model.Application_Id = data.Application_ID;
                model.Closing_Type = data.Closing_Type_Id;
                model.Employee_Id = data.ProcessOwner;
                model.Business_Id = data.tb_BusinessLine.Business_Id;
                model.IsEdit = true;
                model.Escalation_Flag = Convert.ToInt32(data.Escalation_Flag);
                model.IsPaid_Request = Convert.ToInt32(data.IsPaid_Request);
                model.HaveProfile = Convert.ToInt32(data.HaveProfile);

                //if (data.Escalation_Flag == true)
                //{
                //    model.Escalation_Flag = 0;
                //}
                //else
                //{
                //    model.Escalation_Flag = 1;
                //}
                //if (data.IsPaid_Request == true)
                //{
                //    model.IsPaid_Request = 0;
                //}
                //else
                //{
                //    model.IsPaid_Request = 1;
                //}
                //if (data.HaveProfile == true)
                //{
                //    model.HaveProfile = 0;
                //}
                //else
                //{
                //    model.HaveProfile = 1;
                //}
            }
            return PartialView("~/Views/Admin/_pv_AddWFType.cshtml", model);
        }
        public object CheckWFTypeId(string typeid)
        {
            bool status = true; ;
            string msg = "ok";
            int count = _entity.tb_WFType.Where(x => x.IsActive == true && x.WF_ID == typeid).Count();
            if (count > 0)
            {
                status = false;
                msg = "WFType ID already exist";
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);

        }

        public PartialViewResult WFRoleList(string id)
        {
            RoleNewModel model = new RoleNewModel();
            model.list = new List<RoleNewList>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            int slNo = 1;
            if (searchType == 0)
            {
                #region 
                var data = _entity.tb_Role.Where(x => x.IsActive == true).ToList();
                foreach (var item in data)
                {
                    RoleNewList one = new RoleNewList();
                    one.Id = item.Id;
                    one.CountryId = item.tb_Country.Country_Name;
                    one.RoleId = item.Role_ID;
                    one.RoleTitle = item.Role_Desc;
                    one.OrganizationType = item.Organization_Flag;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 1)
            {
                #region
                var data = _entity.tb_Role.Where(x => x.IsActive == true && (x.tb_Country.Country_Name.Contains(searchItem))).ToList();
                foreach (var item in data)
                {
                    RoleNewList one = new RoleNewList();
                    one.Id = item.Id;
                    one.CountryId = item.tb_Country.Country_Name;
                    one.RoleId = item.Role_ID;
                    one.RoleTitle = item.Role_Desc;
                    one.OrganizationType = item.Organization_Flag;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 2)
            {
                #region
                var data = _entity.tb_Role.Where(x => x.IsActive == true && x.Role_ID.Contains(searchItem)).ToList();
                foreach (var item in data)
                {
                    RoleNewList one = new RoleNewList();
                    one.Id = item.Id;
                    one.CountryId = item.tb_Country.Country_Name;
                    one.RoleId = item.Role_ID;
                    one.RoleTitle = item.Role_Desc;
                    one.OrganizationType = item.Organization_Flag;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            else if (searchType == 3)
            {
                #region
                var data = _entity.tb_Role.Where(x => x.IsActive == true && x.Role_Desc.Contains(searchItem)).ToList();
                foreach (var item in data)
                {
                    RoleNewList one = new RoleNewList();
                    one.Id = item.Id;
                    one.CountryId = item.tb_Country.Country_Name;
                    one.RoleId = item.Role_ID;
                    one.RoleTitle = item.Role_Desc;
                    one.OrganizationType = item.Organization_Flag;
                    one.slNo = slNo;
                    model.list.Add(one);
                    slNo = slNo + 1;
                }
                #endregion
            }
            return PartialView("~/Views/Admin/_pv_RoleList.cshtml", model);
        }

        //WF Type Basheer Close 

        ///aju universal table
        ///
        public ActionResult Table_Home(string id)
        {
            var permission = CheckAuth(id);
            if (permission != string.Empty)
            {
                TableModel model = new TableModel();
                model.admin_employee_local_id = permission;
                model.UNL_Table_code = "";
                model.tittle = "Add";
                model.btn_Text = "Create";
                model.isEdit = false;
                return View(model);
            }
            else
            {
                return RedirectToAction("Home", "Account");
            }


        }

        public object AddTable(TableModel model)
        {

            bool status = false;
            string msg = "Failed";
            if (model.isEdit == false)
            {

                if (model.UNL_Table_code == model.UNL_Table_name && model.UNL_Table_name == model.UNL_Table_description)
                {
                    msg = "Can not use same value for universal table !";
                }
                else
                {
                    #region Add Universal lookup table  and table exsiting
                    if (_entity.tb_Tables.Any(x => x.TableName == model.UNL_Table_name.Trim() && x.IsActive == true))
                    {
                        var UN = _entity.tb_UniversalLookupTable.Create();
                        UN.Code = model.UNL_Table_code;
                        UN.Table_Name = model.UNL_Table_name;
                        UN.Description = model.UNL_Table_description;
                        UN.IsActive = true;
                        UN.TimeStamp = CurrentTime;
                        _entity.tb_UniversalLookupTable.Add(UN);
                        status = _entity.SaveChanges() > 0;
                        msg = "Universal Lookup Table added successfuly";

                    }
                    else
                    {
                        var UN = _entity.tb_UniversalLookupTable.Create();
                        UN.Code = model.UNL_Table_code;
                        UN.Table_Name = model.UNL_Table_name;
                        UN.Description = model.UNL_Table_description;
                        UN.IsActive = true;
                        UN.TimeStamp = CurrentTime;
                        _entity.tb_UniversalLookupTable.Add(UN);
                        var tb = _entity.tb_Tables.Create();
                        tb.TableName = model.UNL_Table_name;
                        tb.Description = model.table_description;
                        tb.IsActive = true;
                        tb.TimeStamp = CurrentTime;
                        _entity.tb_Tables.Add(tb);
                        status = _entity.SaveChanges() > 0;
                        if (status)
                        {
                            msg = "Universal Lookup Table added successfuly";
                            #region Keep Log
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " added universal lookup table " + model.UNL_Table_name + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_UniversalLookupTable", UN.Id.ToString(), "Add");

                            string record = UN.Code + " || " + UN.Table_Name + " || " + UN.Description + " || " + UN.IsActive + " || " + UN.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_UniversalLookupTable", UN.Id.ToString(), "Admin");
                            #endregion Keep Log

                        }
                    }
                    #endregion
                }
            }
            else
            {
                #region Edit
                if (_entity.tb_UniversalLookupTable.Any(x => x.Code == model.UNL_Table_code && x.IsActive == true && x.Id != model.UNL_table_id))
                {
                    msg = "Universal Lookup Table already exits !";
                }
                else
                {
                    string UL_code = "";
                    //07/04/2020 commented by alena
                    //string UL_name = "";
                    string UL_Description = "";
                    //06/04/2020 code by Alena sics
                    string description = "";

                    var data = _entity.tb_UniversalLookupTable.Where(x => x.Id == model.UNL_table_id && x.IsActive == true).FirstOrDefault();
                    //08/04/2020 Alena sics
                    var q = _entity.tb_Tables.Where(x => x.TableName == data.Table_Name).FirstOrDefault(); //end

                    #region Checking ULT
                    if (data.Code != model.UNL_Table_code)
                    {
                        UL_code = data.Code;
                    }
                    //07/04/2020 commented by alena and created new code below for table descriptn
                    //if (data.Table_Name != model.UNL_Table_description)
                    //{
                    //    UL_name = data.Table_Name;
                    //}
                    if (q.Description != model.table_description)
                    {
                        description = q.Description;
                    }                                //end
                    if (data.Description != model.UNL_Table_description)
                    {
                        UL_Description = data.Description;
                    }
                    #endregion
                    data.Code = model.UNL_Table_code;
                    //07/04/2020 commented by alena
                    //data.Table_Name = model.UNL_Table_name;
                    data.Description = model.UNL_Table_description;
                    //  08/04/2020 Alena Sics
                    q.Description = model.table_description; //end
                    status = _entity.SaveChanges() > 0;
                    if (status)
                    {
                        msg = "Universal Lookup Table Edit Sucessfully";
                        #region Keep Code 
                        if (UL_code != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited universal code  " + UL_code + " to " + model.UNL_Table_code + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_UniversalLookupTable", data.Id.ToString(), "Edit", UL_code, model.UNL_Table_code);

                            string record = data.Code + " || " + data.Table_Name + " || " + data.Description + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_UniversalLookupTable", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep Name
                        //07/04/2020 commented by alena
                        //if (UL_name != "")
                        //{
                        //    string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited universal lookup table name" + UL_name + " to " + model.UNL_Table_name + " on " + CurrentTime;
                        //    bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_UniversalLookupTable", data.Id.ToString(), "Edit", UL_name, model.UNL_Table_name);

                        //    string record = data.Code + " || " + data.Table_Name + " || " + data.Description + " || " + data.IsActive + " || " + data.TimeStamp;
                        //    bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_UniversalLookupTable", data.Id.ToString(), "Admin");
                        //}
                        #endregion
                        #region Keep UNL Description
                        if (UL_Description != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited universal lookup table Description  " + UL_Description + " to " + model.UNL_Table_description + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_UniversalLookupTable", data.Id.ToString(), "Edit", UL_Description, model.UNL_Table_description);

                            string record = data.Code + " || " + data.Table_Name + " || " + data.Description + " || " + data.IsActive + " || " + data.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_UniversalLookupTable", data.Id.ToString(), "Admin");
                        }
                        #endregion
                        #region Keep table Description
                        //07/04/2020 Alena  sics
                        if (description != "")
                        {
                            string content = "Admin " + Session["username"] + "-" + model.admin_employee_local_id + "-" + " edited table Description  " + description + " to " + model.table_description + " on " + CurrentTime;
                            bool KeepProcessLog = _plr.EditProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_Tables", q.Id.ToString(), "Edit", description, model.table_description);

                            string record = q.TableName + " || " + q.Description + " || " + q.IsActive + " || " + q.TimeStamp;
                            bool keepAuditlog = _alr.Admin_AuditLog(record, model.admin_employee_local_id, "tb_Tables", q.Id.ToString(), "Admin");
                        } //end
                        #endregion

                    }
                    else
                        msg = "No changes !";
                }
                #endregion
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Table_List(string id)
        {
            UniversalList model = new Models.UniversalList();
            model.list = new List<TableModel>();
            string[] splitData = id.Split('~');
            var searchType = Convert.ToInt32(splitData[0]);
            var searchItem = Convert.ToString(splitData[1]);
            long slNo = 1;
            #region 
            var status = _entity.wf_universallookuptable(searchType, searchItem).ToList();
            foreach (var item in status)
            {
                TableModel one = new TableModel();
                one.UNL_table_id = item.Id;
                one.UNL_Table_code = item.Code;
                one.UNL_Table_name = item.Table_Name;
                one.UNL_Table_description = item.Description;
                one.slNo = Convert.ToString(slNo);
                model.list.Add(one);
                slNo = slNo + 1;
            }
            #endregion
            return PartialView("~/Views/Admin/_pv_Table_List.cshtml", model);
        }


        public PartialViewResult Table_Edit(string id)
        {
            var permission = CheckAuth(id);
            TableModel model = new TableModel();
            model.tittle = "Edit";
            model.btn_Text = "Save";
            model.admin_employee_local_id = permission;
            long UId = Convert.ToInt64(id);
            var data = _entity.tb_UniversalLookupTable.Where(x => x.Id == UId && x.IsActive == true).FirstOrDefault();
            ////08/04/2020 code by alena
            var q = _entity.tb_Tables.Where(x => x.TableName == data.Table_Name).FirstOrDefault();//end
            if (data != null)
            {
                ////08/04/2020 alena
                model.table_description = q.Description;  //end
                model.UNL_table_id = data.Id;
                model.UNL_Table_code = data.Code;
                model.UNL_Table_name = data.Table_Name;
                model.UNL_Table_description = data.Description;
                model.isEdit = true;

            }
            return PartialView("~/views/admin/_pv_AddTable.cshtml", model);
        }

        [HttpPost]
        public object GetTable()
        {
            bool status = false;
            string msg = "Failed";
            var result = new WF_Tool.DataLibrary.Data.DropdownData().GetAllTable();
            return Json(new { status = status, msg = msg, list = result }, JsonRequestBehavior.AllowGet);
        }
        public object Delete_Table(string id)
        {
            var permission = CheckAuth(id);//25-2-2020
            TableModel model = new TableModel();
            bool status = false;
            string msg = "Failed";
            int Delid = Convert.ToInt32(id);
            var data = _entity.tb_UniversalLookupTable.Where(x => x.Id == Delid).FirstOrDefault();
            data.IsActive = false;
            status = _entity.SaveChanges() > 0;
            if (status)
            {
                msg = "Universal lookup table deleted successfully";
                #region Keep Log
                string content = "Admin" + Session["username"] + " -" + model.admin_employee_local_id + "- " + " removed universal lookup table   " + data.Table_Name + " on " + CurrentTime;
                bool KeepProcessLog = _plr.AddProcessLog(content, Convert.ToString(Session["userid"]), "Admin", "tb_UniversalLookupTable", data.Id.ToString(), "Removed");
                #endregion Keep Log
            }
            return Json(new { status = status, msg = msg }, JsonRequestBehavior.AllowGet);
        }

        //Basheer on 06-04-2020
        //public ActionResult RequestList(string id)
        //{

        //    var permission = CheckAuth(id);
        //    if (permission != string.Empty)
        //    {
        //        var employee1 = _entity.tb_WF_Employee.Where(x => x.ADAccount == permission && x.IsActive == true).FirstOrDefault();
        //        EmployeeModel model = new EmployeeModel();
        //        model.emp_localid = employee1.LocalEmplyee_ID;
        //        model.adAccountId = employee1.ADAccount;
        //        Session["id"] = employee1.LocalEmplyee_ID;
        //        Session["username"] = employee1.Emp_Name;
        //        Session["adAccount"] = employee1.ADAccount;
        //        model.ad_account = employee1.ADAccount;
        //        return View(model);

        //    }
        //    else
        //    {
        //        return RedirectToAction("RequestPreHome", "Request");
        //    }

        //}
    }
}



