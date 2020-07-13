using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WF_Tool.DataLibrary.Repository;

namespace WF_Tool.DataLibrary.Data
{
    public class DropdownData
    {
        protected static WF_DBEntities1 _Entities = new WF_DBEntities1();
        public static List<SelectListItem> GetAllDomain()
        {
            var input = _Entities.tb_Domain.Where(x => x.IsActive == true).OrderBy(x => x.Domain_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Domain_Name, Value = x.Domain_ID.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllApplication(string domainid)
        {
            var d_id = Convert.ToInt64(domainid);
            var input = _Entities.tb_Application.Where(x => x.DomainId == d_id && x.IsActive == true).OrderBy(x => x.Application_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Application_Name, Value = x.Application_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllWFTypes(string applicationid)
        {
            long appId = Convert.ToInt64(applicationid);
            var input = _Entities.tb_WFType.Where(x => x.Application_ID == appId && x.IsActive == true).OrderBy(x => x.WF_App_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_App_Name, Value = x.WF_ID }).ToList();
        }
        public static List<SelectListItem> GetAllEmployee()
        {
            var input = _Entities.tb_WF_Employee.Where(x => x.IsActive == true).OrderBy(x => x.Emp_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.LocalEmplyee_ID + " " + x.Emp_Name, Value = x.LocalEmplyee_ID }).ToList();
        }
        public static List<SelectListItem> GetEmployeeGroup()//To Fill EmployeeGroup P030 Educational Assistance module //Chitra V :srishti28.05.2020
        {

            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "EDUALLOWANCE").OrderBy(x => x.Table_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description }).ToList();

        }
        public static List<SelectListItem> GetAllPGNoByBusinessline()//To Fill PGNo SAS01 module //Chitra V :srishti26.06.2020
        {
            //long bl = Convert.ToInt64(id);
            var PG = _Entities.tb_ProductGroup.Where(x => x.IsActive == true).OrderBy(x => x.PG_Code).ToList();
            return PG.Select(x => new SelectListItem { Text = x.PG_Code, Value = x.PG_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllBLCodeByBusinessline()//To Fill BL Code SAS01 module //Chitra V :srishti26.06.2020
        {
            //long bl = Convert.ToInt64(id);
            var PG = _Entities.tb_BusinessLine.Where(x => x.IsActive == true).OrderBy(x => x.BusinessLine_Code).ToList();
            return PG.Select(x => new SelectListItem { Text = x.BusinessLine_Code, Value = x.BL_Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetTANumber(string employeeid)//To Fill TANo P003 module //Chitra V :srishti16.06.2020
        {
            var input = _Entities.wf_GetTANo(employeeid).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }
        // 08/07/2020 CHITRA SICS ER HR RELATED -------------
        public static List<SelectListItem> GetRequestType(string Tb_name)      //afsal 10/06/2020
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == Tb_name).OrderBy(x => x.Table_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description }).ToList();
        }
        public static List<SelectListItem> GetBlCode(string Tb_name)      //afsal 10/06/2020
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == Tb_name).OrderBy(x => x.Table_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Code, Value = x.Code }).ToList();
        }
        public static List<SelectListItem> GetRequestType(string Tb_name, string BindVal)      //afsal 10/06/2020
        {
            List<SelectListItem> SortedList = new List<SelectListItem>();
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == Tb_name).OrderBy(x => x.Table_Name).ToList();
            SortedList = input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description }).ToList();
            for (var i = 0; i < SortedList.Count; i++)
            {
                if (SortedList[i].Value == BindVal)
                {
                    SortedList[i].Selected = true;
                }

            }
            return SortedList;

        }
        public static List<SelectListItem> GetBL(string Tb_name, string BindVal)      //afsal 10/06/2020
        {
            List<SelectListItem> SortedList = new List<SelectListItem>();
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == Tb_name).OrderBy(x => x.Table_Name).ToList();
            SortedList = input.Select(x => new SelectListItem { Text = x.Code, Value = x.Code }).ToList();
            for (var i = 0; i < SortedList.Count; i++)
            {
                if (SortedList[i].Value == BindVal)
                {
                    SortedList[i].Selected = true;
                }

            }
            return SortedList;

        }
        public static List<SelectListItem> GetTaRequest(string employeeId)      //afsal 19/06/2020
        {
            var input = _Entities.tb_Request_Hdr.Where(x => x.Application_ID == 8 && x.IsActive == true && (x.Status_ID == "CLS" || x.Status_ID == "PYD") && x.Employee_ID == employeeId).OrderBy(x => x.Request_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID }).ToList();
        }

        // 15/05/2020 ALENA SICS EOSB CALCULATION  -------------
        public static List<SelectListItem> GetP025Request(string employeeid)//To fill End of service p025 module//Chitra V srishti on 29.06.2020
        {
            var input = _Entities.wf_GetEndofServiceRequest(employeeid).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }

        // 29/06/2020 ALENA SICS GET ALL DRIVER DETAILS
        public static List<SelectListItem> GetAllDriverDetails()
        {
            var input = _Entities.tb_Drivers.ToList();
            return input.Select(x => new SelectListItem { Text = x.Driver_Name, Value = x.Id.ToString() }).ToList();
        }
        public static List<SelectListItem> EscalatedToList(string request_id, string my_id, int escalation_no)
        {
            var requests = _Entities.tb_Request_Hdr.Where(x => x.Request_ID == request_id && x.IsActive == true).FirstOrDefault();
            var currentescalation = requests.tb_WF_Template;

            //Fixed Escalation issue – shows same person(Preema)

            var profile_type = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == requests.Employee_ID && x.IsActive == true).FirstOrDefault();
            var initiator = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == requests.Employee_ID && x.IsActive == true).FirstOrDefault();

            var template_type = _Entities.tb_WF_Template.Where(x => x.WF_ID == requests.WF_ID && x.IsActive == true && x.Profile_ID != null).ToList();

            var template_data = _Entities.tb_WF_Template.Where(x => x.WF_Template_ID == requests.tb_WF_Template.WF_Template_ID && x.WF_ID == requests.WF_ID && x.Sequence_NO == escalation_no && x.Status_ID == "ESC" && x.IsActive == true).OrderBy(x => x.Sequence_NO).ToList();

            if (template_type.Count > 0)
            {
                template_data = _Entities.tb_WF_Template.Where(x => x.WF_Template_ID == requests.tb_WF_Template.WF_Template_ID && x.WF_ID == requests.WF_ID && x.Sequence_NO == escalation_no && x.Status_ID == "ESC" && x.IsActive == true && x.Profile_ID == profile_type.Profile_ID).OrderBy(x => x.Sequence_NO).ToList();

            }

            List<EscalationPersonsList> list = new List<EscalationPersonsList>();
            foreach (var item in template_data)
            {
                EscalationPersonsList one = new EscalationPersonsList();
                //var roleTable = _Entities.tb_Role.Where(x => x.Id == item.Role_ID && x.IsActive == true ).FirstOrDefault();
                RoleDetails roleTable = Find_RoleDetails(item.tb_Role, requests, initiator);
                if (roleTable != null)
                {
                    var emp = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == roleTable.assigned_person_id && x.IsActive == true).FirstOrDefault();
                    one.roleId_name = roleTable.role_name + " - " + emp.Emp_Name;
                    one.emp_localId = roleTable.assigned_person_id + "~" + item.Id;
                    list.Add(one);
                    //if (emp != null)
                    //{
                    //    if (emp.LocalEmplyee_ID != my_id)
                    //    {
                    //        if (emp.Delegate_Emp_Code == emp.LocalEmplyee_ID)
                    //        {
                    //            one.roleId_name = roleTable.role_name + " - " + emp.Emp_Name;
                    //            one.emp_localId = emp.LocalEmplyee_ID + '~' + item.Id;
                    //        }
                    //        else if (emp.DelegationFlag == false)
                    //        {
                    //            one.roleId_name = roleTable.role_name + " - " + emp.Emp_Name;
                    //            one.emp_localId = emp.LocalEmplyee_ID + '~' + item.Id;
                    //        }
                    //        else
                    //        {
                    //            var newEmp = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == roleTable.assigned_person_id && x.IsActive == true).FirstOrDefault();
                    //            one.roleId_name = roleTable.role_name + " - " + newEmp.Emp_Name;
                    //            one.emp_localId = newEmp.LocalEmplyee_ID + '~' + item.Id;
                    //        }
                    //        list.Add(one);
                    //    }
                    //}
                }
            }
            return list.Select(x => new SelectListItem { Text = x.roleId_name, Value = x.emp_localId }).ToList();
        }

        public static List<SelectListItem> GetAllDepartmentsByProductGroup(string id)
        {
            long Id = Convert.ToInt64(id);
            var department = _Entities.tb_Department.Where(x => x.PG_Id == Id && x.IsActive == true).OrderBy(x => x.Department_Name).ToList();
            return department.Select(x => new SelectListItem { Text = x.Department_Name + " ( " + x.Department_Code + " )", Value = x.Department_Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllPGByBusinessline(string id)
        {
            long bl = Convert.ToInt64(id);
            var PG = _Entities.tb_ProductGroup.Where(x => x.BusinessLine_Id == bl && x.IsActive == true).OrderBy(x => x.PG_Name).ToList();
            return PG.Select(x => new SelectListItem { Text = x.PG_Name, Value = x.PG_Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllEmployeesInCountry(string country_Id)
        {
            long countryId = Convert.ToInt64(country_Id);
            //var input = _Entities.tb_WF_Employee.Where(x => x.tb_Location.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.Emp_Name).ToList(); //Basheer on 22-04-2020
            var input = _Entities.tb_WF_Employee.Where(x => x.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.Emp_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.LocalEmplyee_ID + " - " + x.Emp_Name, Value = x.LocalEmplyee_ID }).ToList();
        }
        public static List<SelectListItem> GetAllEmployeesInSameDelegate(string myId, string requestempid)//27-02-2020 ARCHANA SRISHTI 
        {
            var input = _Entities.tb_WF_Employee.Where(x => x.IsActive == true && x.LocalEmplyee_ID != myId && x.LocalEmplyee_ID != requestempid).OrderBy(x => x.Emp_Name).ToList();
            //return input.Select(x => new SelectListItem { Text = x.Emp_Name, Value = x.LocalEmplyee_ID }).ToList(); //Basheer on 27-04-2020
            return input.Select(x => new SelectListItem { Text = x.LocalEmplyee_ID + " - " + x.Emp_Name, Value = x.LocalEmplyee_ID }).ToList();
        }
        public static List<SelectListItem> GetAllCurrency()//28-02-2020 ARCHANA SRISHTI 
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "CURC").OrderBy(x => x.Table_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description }).ToList();
        }
        public static List<SelectListItem> GetAllCountries()
        {
            var input = _Entities.tb_Country.Where(x => x.IsActive == true).OrderBy(x => x.Country_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Country_Name, Value = x.Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllUsersByRoleFromLookUp(long role_id, string wf_type)
        {
            var role = _Entities.tb_Role.Where(x => x.Id == role_id).FirstOrDefault();
            var input = _Entities.wf_GetAllUsersByRoleFromLookUp(wf_type, role.Role_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Emp_Name, Value = x.LocalEmplyee_ID }).ToList();
        }
        public static List<SelectListItem> GetAllBusinesscontrollersByCountry(string id)
        {
            var countryId = Convert.ToInt64(id);
            var bus = _Entities.wf_GetBus_Controller(countryId).OrderBy(x => x.Emp_Name).ToList();
            return bus.Select(x => new SelectListItem { Text = x.Emp_Name, Value = x.Bus_Controller.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllBusinesslinecontrollersByBusiness(string id)
        {
            var Business_Id = Convert.ToInt64(id);
            var bus_line = _Entities.wf_Get_BusLineby_Controller(Business_Id).OrderBy(x => x.Emp_Name).ToList();
            return bus_line.Select(x => new SelectListItem { Text = x.Emp_Name, Value = x.BL_Controller.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllPGcontrollersByBusinessline(string id)
        {
            var BusinessLine_Id = Convert.ToInt32(id);
            var PG = _Entities.wf_Get_PGby_Controller(BusinessLine_Id).OrderBy(x => x.Emp_Name).ToList();
            return PG.Select(x => new SelectListItem { Text = x.Emp_Name, Value = x.PG_Controller.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllCompanies()
        {
            var input = _Entities.tb_Company.Where(x => x.IsActive == true).OrderBy(x => x.Company_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Company_Name + " ( " + x.Company_Code + " )", Value = x.Company_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllVenders()
        {
            var input = _Entities.tb_Vendor.Where(x => x.IsActive == true).OrderBy(x => x.Vendor_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Vendor_Name, Value = x.Vendor_id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllJobs()
        {
            var input = _Entities.tb_Job.Where(x => x.IsActive == true).OrderBy(x => x.Job_tittle).ToList();
            return input.Select(x => new SelectListItem { Text = x.Job_tittle, Value = x.Job_Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetFullWFTypes()
        {
            var input = _Entities.tb_WFType.Where(x => x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_App_Name + " ( " + x.WF_ID + " ) ", Value = x.Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetFullWFTypesByCountry(string id)
        {
            long country = Convert.ToInt64(id);
            var input = _Entities.tb_WFType.Where(x => x.IsActive == true && x.Country_Id == country).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_App_Name + " ( " + x.WF_ID + " ) ", Value = x.Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllRolesFromCountry(long id)
        {
            long countryId = Convert.ToInt64(id);
            var iput = _Entities.tb_Role.Where(x => x.Country_ID == countryId && x.IsActive == true).OrderBy(x => x.Role_ID).ToList();
            return iput.Select(x => new SelectListItem { Text = x.Role_Desc + " ( " + x.Role_ID + " )", Value = x.Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllProfiles(long id)
        {
            long country = Convert.ToInt64(id);
            var input = _Entities.tb_Emp_Profile.Where(x => x.IsActive == true && x.Country_Id == country).OrderBy(x => x.Profile_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Profile_Desc + " ( " + x.Profile_ID + " )", Value = x.Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllStatusFromCountry()
        {
            var input = _Entities.tb_Status.Where(x => x.IsActive == true).OrderBy(x => x.Status_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Status_Desc + " (" + x.Status_ID + ")", Value = x.Status_ID }).ToList();
        }
        // 15/06/2020 ALENA COMMNENTED BELOW CODE AND ADDED NEW CODE BELOW

        //public static List<SelectListItem> GetAllDistribution(long id)
        //{
        //    long wfid = Convert.ToInt64(id);
        //    var input = _Entities.tb_DistributionList.Where(x => x.WF_ID == wfid && x.IsActive == true).Select(x => new { x.DistributionList_Code, x.tb_WFType.WF_ID }).Distinct().OrderBy(x => x.DistributionList_Code).ToList();
        //    return input.Select(x => new SelectListItem { Text = x.WF_ID, Value = x.DistributionList_Code.ToString() }).ToList();
        //}
        public static List<SelectListItem> GetAllDistribution(long id)
        {
            long wfid = Convert.ToInt64(id);
            var input = _Entities.tb_DistributionList.Where(x => x.WF_ID == wfid && x.IsActive == true).Select(x => new { x.DistributionList_Code, x.tb_WFType.WF_ID, x.Order_No }).Distinct().OrderBy(x => x.DistributionList_Code).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_ID + " - " + x.Order_No, Value = x.DistributionList_Code.ToString() }).ToList();

        }
        public static List<SelectListItem> GetAllCostCenterByEntry(string id)
        {
            long country_id = Convert.ToInt64(id);
            var input = _Entities.tb_CostCenter.Where(x => x.Country_Id == country_id && x.IsActive == true).OrderBy(x => x.CC_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.CC_Name + " ( " + x.CC_Code + " )", Value = x.CC_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllLocationInCountry(long countryId)
        {
            var input = _Entities.tb_Location.Where(x => x.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.Location).ToList();
            return input.Select(x => new SelectListItem { Text = x.Location + " ( " + x.Location_Code + " )", Value = x.Location_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllBusinessInCountry(long countryId)
        {
            var input = _Entities.tb_Business.Where(x => x.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.Business_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Business_Name + " ( " + x.Business_Code + " )", Value = x.Bus_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllBusinessLineInCountry(long businessId)
        {
            var input = _Entities.tb_BusinessLine.Where(x => x.Business_Id == businessId && x.IsActive == true).OrderBy(x => x.Business_Line_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Business_Line_Name + " ( " + x.BusinessLine_Code + " )", Value = x.BL_Id.ToString() }).ToList();
        }
        public List<SelectListItem> GetAllButtons()
        {
            var input = _Entities.tb_Button.Where(x => x.IsActive == true).OrderBy(x => x.Description).ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Code }).ToList();
        }
        public static List<SelectListItem> GetAllPGInCountry(long countryId)
        {
            var input = _Entities.tb_ProductGroup.Where(x => x.tb_BusinessLine.tb_Business.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.PG_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.PG_Name + " ( " + x.PG_Code + " )", Value = x.PG_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllApplicationInCountry(long countryId)
        {
            var input = _Entities.tb_Application.Where(x => x.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.Application_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Application_Name + " ( " + x.Application_Code + " )", Value = x.Application_Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllApplicationInDomain(long domainId)
        {
            var input = _Entities.tb_Application.Where(x => x.DomainId == domainId && x.IsActive == true).OrderBy(x => x.Application_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Application_Name + " ( " + x.Application_Code + " )", Value = x.Application_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllWfTypeInApplication(long App_ID)
        {
            var input = _Entities.tb_WFType.Where(x => x.Application_ID == App_ID && x.IsActive == true).OrderBy(x => x.WF_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_ID + " ( " + x.WF_App_Name + " )", Value = x.Id.ToString() }).ToList();
        }
        //GetAllWf_Type
        public static List<SelectListItem> GetAllWf_Type()
        {
            var input = _Entities.tb_WFType.Where(x => x.IsActive == true).OrderBy(x => x.WF_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_ID, Value = x.Id.ToString() }).ToList();
        }
        //GetAllStatus
        public static List<SelectListItem> GetAllStatus()
        {
            var input = _Entities.tb_Status.Where(x => x.IsActive == true).OrderBy(x => x.Status_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Status_ID, Value = x.S_Id.ToString() }).ToList();
        }
        //listAllRole
        public List<SelectListItem> ListAllRole()
        {
            var input = _Entities.tb_Role.Where(x => x.IsActive == true).OrderBy(x => x.Role_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Role_ID, Value = x.Id.ToString() }).ToList();
        }
        // List All Location without any condetion
        public static List<SelectListItem> ListAllLocations()
        {
            var input = _Entities.tb_Location.Where(x => x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Location, Value = x.Location_Id.ToString() }).ToList();
        }
        // List All Car type from the Universal Look Up table 
        public static List<SelectListItem> ListAllCarType()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "CARTYPE").ToList(); //Basheer on 28-05-2020
            return input.Select(x => new SelectListItem { Text = x.Code, Value = x.Code.ToString() }).ToList();
        }
        // List All Payment type from the Universal Look Up table 
        public static List<SelectListItem> ListAllPaymentType()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "PAYMENTTYPE").ToList();//Basheer on 28-05-2020
            return input.Select(x => new SelectListItem { Text = x.Code, Value = x.Code.ToString() }).ToList();
        }
        public static RoleDetails Find_RoleDetails(tb_Role next_role, tb_Request_Hdr data, tb_WF_Employee employee)
        {
            RoleDetails role = new RoleDetails();
            role.role_id = next_role.Role_ID;
            role.role_name = next_role.Role_Desc;
            if (next_role.Organization_Flag == false)
            {
                if (next_role.Assigned_ID != null)
                {
                    #region Normal
                    role.assigned_person_id = next_role.Assigned_ID;
                    #endregion Normal
                }
                else
                {
                    if (next_role.GroupRole == true)
                    {
                        #region Having Multiple 
                        var universal = _Entities.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true).FirstOrDefault();
                        if (universal != null)
                        {
                            role.assigned_person_id = universal.Table_Name;
                        }
                        #endregion Having Multiple 
                    }
                    else
                    {
                        #region Having Single 
                        var universal = _Entities.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && (x.Code == null || x.Code == data.tb_WFType.WF_ID)).FirstOrDefault();
                        if (universal != null)
                        {
                            role.assigned_person_id = universal.Description;
                        }
                        #endregion Having Single
                    }
                }
            }
            else
            {
                #region 
                if (next_role.org_type == null || next_role.org_type == string.Empty)
                {
                    role.assigned_person_id = employee.Line_Manager;
                }
                else if (next_role.org_type.Trim() == "DT")// Check the department table 
                {
                    #region Department
                    var department = _Entities.tb_Department.Where(x => x.IsActive == true && x.Department_Id == employee.Department_Id).FirstOrDefault();
                    if (department != null)
                    {
                        if (next_role.role_type == "MN" && department.Dept_Manager != null)
                        {
                            role.assigned_person_id = department.Dept_Manager;
                        }
                        else if (next_role.role_type == "CR" && department.Dept_Controller != null)
                        {
                            role.assigned_person_id = department.Dept_Controller;
                        }
                        else if (next_role.role_type == "OA" && department.Dep_Office_Admin != null)
                        {
                            role.assigned_person_id = department.Dep_Office_Admin;
                        }
                    }
                    #endregion Department
                }
                else if (next_role.org_type.Trim() == "BL")// Check the business line table 
                {
                    #region Business Line
                    var business = _Entities.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.tb_Department.tb_ProductGroup.BusinessLine_Id).FirstOrDefault();
                    if (business != null)
                    {
                        if (next_role.role_type == "MN" && business.BL_Manager != null)
                        {
                            role.assigned_person_id = business.BL_Manager;
                        }
                        else if (next_role.role_type == "CR" && business.BL_Controller != null)
                        {
                            role.assigned_person_id = business.BL_Controller;
                        }
                        else if (next_role.role_type == "OA" && business.BL_Office_Admin != null)
                        {
                            role.assigned_person_id = business.BL_Office_Admin;
                        }

                    }
                    #endregion Business Line
                }
                else if (next_role.org_type.Trim() == "B")// Check the business 
                {
                    #region Business
                    var business = _Entities.tb_Business.Where(x => x.IsActive == true && x.Bus_Id == employee.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Id).FirstOrDefault();
                    if (business != null)
                    {
                        if (next_role.role_type == "MN" && business.Bus_Manager != null)
                        {
                            role.assigned_person_id = business.Bus_Manager;
                        }
                        else if (next_role.role_type == "CR" && business.Bus_Controller != null)
                        {
                            role.assigned_person_id = business.Bus_Controller;
                        }
                        else if (next_role.role_type == "OA" && business.Bus_Office_Admin != null)
                        {
                            role.assigned_person_id = business.Bus_Office_Admin;
                        }
                    }
                    #endregion Business
                }
                else if (next_role.org_type == "PG")// Check the Product group table 
                {
                    #region Product Group
                    var product = _Entities.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.tb_Department.PG_Id).FirstOrDefault();
                    if (product != null)
                    {
                        if (next_role.role_type == "MN" && product.PG_Manager != null)
                        {
                            role.assigned_person_id = product.PG_Manager;
                        }
                        else if (next_role.role_type == "CR" && product.PG_Controller != null)
                        {
                            role.assigned_person_id = product.PG_Controller;
                        }
                        else if (next_role.role_type == "OA" && product.PG_Office_Admin != null)
                        {
                            role.assigned_person_id = product.PG_Office_Admin;
                        }
                    }
                    #endregion Product Group 
                }
                #endregion
            }
            var next_emp = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == role.assigned_person_id && x.IsActive == true).FirstOrDefault();
            #region
            if (next_emp != null)
            {
                role.assigned_person_id = next_emp.LocalEmplyee_ID;
                if (next_emp.DelegationFlag == true)
                    role.deligated_personId = next_emp.Delegate_Emp_Code == null ? next_emp.LocalEmplyee_ID : next_emp.Delegate_Emp_Code;
                else
                    role.deligated_personId = next_emp.LocalEmplyee_ID;
                if (role.deligated_personId == data.Employee_ID)
                {
                    //var bus_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == employee.BusinessLine_Id && x.Country_Id == country.Id && x.IsActive == true).FirstOrDefault();
                    //var line_manager = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == bus_line.BL_Manager && x.IsActive == true).FirstOrDefault();
                    var line_manager = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employee.Line_Manager && x.IsActive == true).FirstOrDefault();
                    if (line_manager != null)
                    {
                        if (line_manager.DelegationFlag == true)
                            role.assigned_person_id = line_manager.Delegate_Emp_Code == null ? line_manager.LocalEmplyee_ID : line_manager.Delegate_Emp_Code;
                        else
                            role.assigned_person_id = line_manager.LocalEmplyee_ID;
                    }
                }
            }
            else
            {
                role.deligated_personId = role.assigned_person_id;
            }
            #endregion
            return role;
        }

        //Basheer on 13-12-2019 to get employee details
        public static List<SelectListItem> GetAllEmployeeByCountry(long? countrycode)
        {
            var input = _Entities.tb_WF_Employee.Where(x => x.IsActive == true && x.Location_Id == countrycode).ToList();
            return input.Select(x => new SelectListItem { Text = x.LocalEmplyee_ID + " " + x.Emp_Name, Value = x.LocalEmplyee_ID }).ToList();
        }
        public static List<SelectListItem> GetApplication()
        {
            var input = _Entities.tb_Application.Where(x => x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Application_Name, Value = x.Application_Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAlClosingType()
        {
            var input = _Entities.tb_Closing_Type.Where(x => x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Code, Value = x.Id.ToString() }).ToList();
        }

        public static List<SelectListItem> GetWFTypeLoadBusinessByCountry(string id)
        {
            long Id = Convert.ToInt64(id);
            var input = _Entities.tb_Business.Where(x => x.Country_Id == Id && x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Business_Name, Value = x.Bus_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetWFTypeLoadBusinessLineByBusiness(string id)
        {
            long Id = Convert.ToInt64(id);
            var input = _Entities.tb_BusinessLine.Where(x => x.Business_Id == Id && x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Business_Line_Name, Value = x.BL_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetWFTypeLoadApplicationByCountry(string id)
        {
            long Id = Convert.ToInt64(id);
            var input = _Entities.tb_Application.Where(x => x.Country_Id == Id && x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Application_Name, Value = x.Application_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetWFTypeLoadEmployeeByCountry(string id)
        {
            long Id = Convert.ToInt64(id);
            var input = _Entities.tb_WF_Employee.Where(x => x.Location_Id == Id && x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.LocalEmplyee_ID + " " + x.Emp_Name, Value = x.LocalEmplyee_ID.ToString() }).ToList();
        }
        // Basheer 07/01/2020----------------Stop

        /// <summary>
        /// Sibi 03-01-2020 .....Pending
        /// </summary>
        /// <param name="LocationId"></param>
        /// <returns></returns>

        public static List<SelectListItem> GetEmployee_Location_Id_By_Cuntry(string localEmplyee_ID)
        {
            //long LocationId = Convert.ToInt64(_Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == localEmplyee_ID && x.IsActive == true).Select(x => x.Location_Id).FirstOrDefault());
            //var input = _Entities.tb_Location.Where(x => x.Country_Id == LocationId && x.IsActive == true).ToList();
            //return input.Select(x => new SelectListItem { Text = x.Location, Value = x.Location_Code }).ToList();

            //28-02-2020 ARCHANA KV SRISHTI 
            long CountryId = Convert.ToInt64(_Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == localEmplyee_ID && x.IsActive == true).Select(x => x.Country_Id).FirstOrDefault());
            var input = _Entities.tb_Location.Where(x => x.Country_Id == CountryId && x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Location, Value = x.Location_Id.ToString() }).ToList();
        }

        //-----------------Sibi Stop ------------------

        public List<string> GetAllEmp()
        {
            var employee = _Entities.tb_WF_Employee.Where(z => z.IsActive == true).Select(x => x.Emp_Name).ToList();
            return employee;
        }
        public List<string> GetAllTable()
        {
            var employee = _Entities.tb_Tables.Where(z => z.IsActive == true).Select(x => x.TableName).ToList();
            return employee;
        }

        //basheer on 08-01-2020 P013 Start
        public static List<SelectListItem> GetTravelRequests(string employeeid)
        {
            //CHange P034 to as per need in travel allowance
            var input = _Entities.tb_Request_Hdr.Where(x => x.IsActive == true && x.Employee_ID == employeeid && (x.tb_WFType.WF_ID == "P034") && x.WF_ID == x.tb_WFType.Id && x.Process_Complete == null).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }
        public static List<SelectListItem> Getcurrency()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "CURC").ToList();
            return input.Select(x => new SelectListItem { Text = x.Code, Value = x.Code.ToString() }).ToList();
        }

        //Terrin on 30/3/2020
        public static List<SelectListItem> GetAllowancepergrp()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.Table_Name == "ALLOWANCE").Where(x => x.Code == "p060").ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description.ToString() }).ToList();
        }
        public static List<SelectListItem> LoadWFTypeList(string applicationCode)
        {
            var input = _Entities.tb_WFType.Where(x => x.tb_Application.Application_Code == applicationCode && x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.WF_App_Name + " (" + x.WF_ID + " )", Value = x.WF_ID.ToString() }).ToList();
        }

        public tb_Application GetApplicationId(string applicationCode)
        {
            var data = _Entities.tb_Application.Where(x => x.Application_Code == applicationCode && x.IsActive == true).FirstOrDefault();
            return data;
        }
        public static List<SelectListItem> GetAllBusinesslinecontrollersByBusinessline()
        {
            var bus_line = _Entities.tb_BusinessLine.Where(x => x.BL_Id == x.tb_WFType.Where(y => y.WF_ID == "P015").FirstOrDefault().BusinessLine_Id).ToList();
            return bus_line.Select(x => new SelectListItem { Text = _Entities.tb_WF_Employee.Where(y => y.LocalEmplyee_ID == x.BL_Controller).FirstOrDefault().Emp_Name + ' ' + x.BL_Controller, Value = x.BL_Controller }).ToList();
        }

        //Basheer on 04-02-2020
        public static List<SelectListItem> GetAllStatusRequest()
        {
            var input = _Entities.tb_Status.Where(x => x.IsActive == true && x.Status_ID != "BKP" && x.Status_ID != "APC").OrderBy(x => x.Status_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Status_ID + "~" + x.Status_Desc, Value = x.Status_ID }).ToList();
        }
        //public static List<SelectListItem> GetCarLoanPaymentRequests(string employeeid)
        //{
        //    var input = _Entities.tb_Request_Hdr.Where(x => x.IsActive == true && x.Employee_ID == employeeid && (x.tb_WFType.WF_ID == "P023") && x.WF_ID == x.tb_WFType.Id && x.Process_Complete == null).ToList();
        //    return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        //}
        ///aju Srishti 07/01/2020
        public static List<SelectListItem> GetAllEmployeesmanager()
        {
            var input = _Entities.tb_WF_Employee.Where(x => x.IsActive == true).OrderBy(x => x.Emp_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.LocalEmplyee_ID + " " + x.Emp_Name, Value = x.Emp_Name }).ToList();
        }

        //Basheer on 14-02-2020
        public static List<SelectListItem> GetAllStatusRequestSearch()
        {
            var input = _Entities.tb_Status.Where(x => x.IsActive == true && x.Status_ID != "NEW" && x.Status_ID != "SUB" && x.Status_ID != "BKP" && x.Status_ID != "APC" && x.Status_ID != "NULL").OrderBy(x => x.Status_ID).ToList();
            return input.Select(x => new SelectListItem { Text = x.Status_ID + "~" + x.Status_Desc, Value = x.Status_ID }).ToList();
        }

        //aju18-02-2020
        public static List<SelectListItem> GetAllGlobalgrade()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "GLOBALGRADE").ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description.ToString() }).ToList();
        }

        public static List<SelectListItem> GetAllDeligationBand()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "DELIBAND").ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description.ToString() }).ToList();
        }


        public static List<SelectListItem> GetAlllocalgrade()
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "LOCALGRADE").ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description.ToString() }).ToList();
        }

        public static List<SelectListItem> GetCarLoanPaymentRequests(string employeeid) //Nimmi Mohan 27-03-2020
        {
            var input = _Entities.tb_Request_Hdr.Where(x => x.IsActive == true && x.Employee_ID == employeeid && (x.tb_WFType.WF_ID == "P023") && x.WF_ID == x.tb_WFType.Id && x.Status_ID == "CLS" && x.Process_Complete == 1).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }

        public static List<SelectListItem> GetReasonForClearance()      //30/03/2020 Nimmi P025
        {
            var input = _Entities.tb_UniversalLookupTable.Where(x => x.IsActive == true && x.Table_Name == "ENDOFSERVICE").ToList();
            return input.Select(x => new SelectListItem { Text = x.Description, Value = x.Description.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllRecurimentTrainingController()
        {
            // 07/07/2020 Alena changed below and added new code provided by Basheer
            //var query = (from x in _Entities.tb_Business select new { A = x.Bus_Controller })
            //.Union(from y in _Entities.tb_BusinessLine select new { A = y.BL_Controller })
            //.Union(from a in _Entities.tb_ProductGroup select new { A = a.PG_Controller })
            //.Union(from b in _Entities.tb_Department select new { A = b.Dept_Controller }).ToList();

            ////return query.Select(z => new SelectListItem { Text = _Entities.tb_WF_Employee.Where(s => s.LocalEmplyee_ID == z.A).FirstOrDefault().Emp_Name + ' ' + z.A, Value = z.A }).ToList();
            //return query.Select(z => new SelectListItem { Text = z.A + "--" + _Entities.tb_WF_Employee.Where(s => s.LocalEmplyee_ID == z.A).FirstOrDefault().Emp_Name, Value = z.A }).ToList();
            var query = (from x in _Entities.tb_Business select new { A = x.Bus_Controller })
           .Union(from y in _Entities.tb_BusinessLine select new { A = y.BL_Controller })
           .Union(from a in _Entities.tb_ProductGroup select new { A = a.PG_Controller })
           .Union(from b in _Entities.tb_Department select new { A = b.Dept_Controller }).ToList();
            var data = query.Where(x => _Entities.tb_WF_Employee.Any(c => c.LocalEmplyee_ID == x.A)).ToList();
            return data.Select(z => new SelectListItem { Text = z.A + "--" + _Entities.tb_WF_Employee.Where(s => s.LocalEmplyee_ID == z.A).FirstOrDefault().Emp_Name, Value = z.A }).ToList();


            //return query.Select(z => new SelectListItem { Text = z.A + "--" + _Entities.tb_WF_Employee.Where(s => s.LocalEmplyee_ID == z.A).FirstOrDefault().Emp_Name, Value = z.A }).ToList();
        }
        public static List<SelectListItem> GetAllTARequestsforEmployee(string employeeid, string wf_id)     //Baasheer on 28-05-2020 for P034
        {
            long wfid = _Entities.tb_WFType.Where(x => x.WF_ID == wf_id && x.IsActive == true).FirstOrDefault().Id;
            var input = _Entities.tb_Request_Hdr.Where(x => x.IsActive == true && x.Employee_ID == employeeid && x.WF_ID == wfid).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }

        //vyas - 23-06-2020-
        public static List<SelectListItem> GetAllTARequestsforEmployee_P0411(string employeeid)     //Baasheer on 28-05-2020 for P034
        {
            long wfid = _Entities.tb_WFType.Where(x => x.WF_ID == "P041" && x.IsActive == true).FirstOrDefault().Id;
            var input = _Entities.tb_Request_Hdr.Where(x => x.IsActive == true && x.Employee_ID == employeeid && x.WF_ID == wfid).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }

        //Anzeem - 01-06-2020
        public static List<SelectListItem> GetAllBusinessInEmployeeCountry(string employeeId)
        {
            var countryId = _Entities.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employeeId).Select(x => x.Country_Id).FirstOrDefault();
            var input = _Entities.tb_Business.Where(x => x.Country_Id == countryId && x.IsActive == true).OrderBy(x => x.Business_Name).ToList();
            return input.Select(x => new SelectListItem { Text = x.Business_Name + " ( " + x.Business_Code + " )", Value = x.Bus_Id.ToString() }).ToList();
        }
        public static List<SelectListItem> GetAllSiteVisitRequestsforEmployee(string employeeid)
        {
            long wfid = _Entities.tb_WFType.Where(x => x.WF_ID == "P067" && x.IsActive == true).FirstOrDefault().Id;
            var input = _Entities.tb_Request_Hdr.Where(x => x.IsActive == true && x.Employee_ID == employeeid && x.WF_ID == wfid).ToList();
            return input.Select(x => new SelectListItem { Text = x.Request_ID, Value = x.Request_ID.ToString() }).ToList();
        }
        // By George on 29-06-2020 For Getting Course Details(T004)
        public static List<SelectListItem> GetAllCourseDetails()
        {
            var CourseInfo = _Entities.tb_TR_InHouse_Course.Where(x => x.IsActive == true).ToList();
            return CourseInfo.Select(x => new SelectListItem { Text = x.Code + " - " + x.Course_Name, Value = x.Id.ToString() }).ToList();
        }

        // By George on 07-07-2020 For Getting Course Details(T001)
        public static List<SelectListItem> GetExternalCourseDetails()
        {
            var input = _Entities.tb_TR_External_Courses.Where(x => x.IsActive == true).ToList();
            return input.Select(x => new SelectListItem { Text = x.Course_Name, Value = x.Id.ToString() }).ToList();
        }
    }
}

