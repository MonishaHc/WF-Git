using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF_Tool.ClassLibrary;
using WF_Tool.DataLibrary;
using WF_Tool.DataLibrary.Data;
using WF_Tool.DataLibrary.Repository;

namespace WF_Tool.DataLibrary.Repository
{
    public class RequestProcessRepository : ApprovalLogRepository
    {
        public WF_DBEntities1 _entity = new WF_DBEntities1();

        public DateTime CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now.ToUniversalTime(), TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));

        public Tuple<bool, string> InsertRequest(string wf_id, long application_id, string emp_local_id, long template_id, string creator_id)
        {
            bool status = false;
            string request_id = "";
            var req = _entity.tb_RequestSeries.FirstOrDefault();
            var wfType = _entity.tb_WFType.Where(x => x.WF_ID == wf_id && x.IsActive == true).FirstOrDefault();
            long req_Id = 0;
            if (req == null)
            {
                var create_request_id_table = _entity.tb_RequestSeries.Create();
                create_request_id_table.RequestNo = 1;
                create_request_id_table.TimeStamp = CurrentTime;
                _entity.tb_RequestSeries.Add(create_request_id_table);
                _entity.SaveChanges();
                req_Id = 1;
                _entity.tb_RequestSeries.FirstOrDefault();
                req = _entity.tb_RequestSeries.FirstOrDefault();
            }
            else
            {
                //Basheer on 24-01-2020 to check if requestid exist or not
                string newreqid = req.RequestNo.ToString();
                var lastrequest = _entity.tb_Request_Hdr.Where(x => x.Request_ID == newreqid).FirstOrDefault();
                if (lastrequest != null)
                {
                    long maxrequest = _entity.tb_Request_Hdr.Select(x => x.Id).Max();
                    string lastrequest_Id = _entity.tb_Request_Hdr.Where(x => x.Id == maxrequest).FirstOrDefault().Request_ID;
                    req_Id = Convert.ToInt64(lastrequest_Id) + 1;
                }
                else
                {
                    req_Id = req.RequestNo;
                }
                //Basheer code end here
            }
            var add_new_request = _entity.tb_Request_Hdr.Create();
            add_new_request.Request_ID = req_Id.ToString();
            add_new_request.WF_ID = wfType.Id;
            add_new_request.Application_ID = application_id;
            add_new_request.Employee_ID = emp_local_id;
            add_new_request.Creater_ID = creator_id;
            add_new_request.Status_ID = "NEW";
            add_new_request.Approval_No = 0.ToString();
            add_new_request.WFTemplate_ID = null;
            add_new_request.WebLink = "";
            add_new_request.IsActive = true;
            add_new_request.Notice = true;
            add_new_request.TimeStamp = CurrentTime;
            _entity.tb_Request_Hdr.Add(add_new_request);
            req.RequestNo = req.RequestNo + 1;
            try
            {
                status = _entity.SaveChanges() > 0;
            }
            catch (Exception ex)
            {

            }
            request_id = add_new_request.Request_ID;
            //add just before return
            //Basheer added log for new request saving 21-01-2020
            string remark = "Request was Saved by";
            InsertApproveLog("NEW", request_id, remark, emp_local_id, creator_id, "", "", "", "", "");
            return new Tuple<bool, string>(status, request_id);
        }

        public Tuple<bool, string, string, string> SubmitRequest(string id, tb_Request_Hdr data)
        {
            bool status = false;
            string[] splitData = id.Split('~');
            var request_id = splitData[0];
            var emp_local_id = splitData[1];
            var wf_id = splitData[2];
            string rolAssign = "";
            string roleDescription = "";
            var wfType = _entity.tb_WFType.Where(x => x.Id == data.WF_ID && x.IsActive == true).FirstOrDefault();
            var myData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
            var profile = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == emp_local_id && x.IsActive == true).FirstOrDefault();
            if (wfType.HaveProfile == true)
            {
                var template = _entity.tb_WF_Template.Where(x => x.WF_ID == data.tb_WFType.Id && x.Profile_ID == profile.Profile_ID && x.Sequence_NO == 1 && x.IsActive == true && x.Action_Flag != 3).FirstOrDefault();
                if (template != null)
                {
                    if (template.Role_ID != null)
                    {
                        var role = Find_RoleDetails(template.tb_Role, data, myData, template.Status_ID);// 21-02-2020 ARCHANA SRISHTI 
                        rolAssign = role.assigned_person_id;
                        roleDescription = role.role_name;
                        var new_entry_for_approve = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id && x.IsActive == true).FirstOrDefault();
                        #region 
                        if (new_entry_for_approve.Employee_ID != rolAssign) // Check the request owner and the approver is same 
                        {
                            new_entry_for_approve.Request_ID = request_id;
                            new_entry_for_approve.WF_ID = wfType.Id;
                            new_entry_for_approve.Approver_ID = rolAssign;
                            new_entry_for_approve.OrgApprover_ID = rolAssign;
                            new_entry_for_approve.Status_ID = template.Status_ID;
                            new_entry_for_approve.Approval_No = 1.ToString();
                            new_entry_for_approve.WFTemplate_ID = template.Id;
                            new_entry_for_approve.IsActive = true;
                            new_entry_for_approve.TimeStamp = CurrentTime;
                            new_entry_for_approve.WebLink = "";
                            new_entry_for_approve.RoleId = template.Role_ID;
                            status = _entity.SaveChanges() > 0;
                        }
                        else
                        {
                            //getting the linemanager for Approverid  //11-03-2020  Nimmi Mohan//Preetha
                            var line_manager = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == myData.Line_Manager && x.IsActive == true).FirstOrDefault();
                            //var businessLine = _entity.tb_BusinessLine.Where(x => x.BL_Id == myData.tb_Department.tb_ProductGroup.BusinessLine_Id && x.BL_Manager != null && x.IsActive == true).FirstOrDefault();
                            //var businessLine = _entity.tb_BusinessLine.Where(x => x.BL_Id == myData.tb_Department.tb_ProductGroup.BusinessLine_Id && x.BL_Manager != null && x.IsActive == true).FirstOrDefault();
                            if (line_manager != null)
                            {
                                new_entry_for_approve.Request_ID = request_id;
                                new_entry_for_approve.WF_ID = wfType.Id;
                                new_entry_for_approve.Approver_ID = myData.Line_Manager;
                                new_entry_for_approve.OrgApprover_ID = rolAssign;
                                new_entry_for_approve.Status_ID = template.Status_ID;
                                new_entry_for_approve.Approval_No = 1.ToString();
                                new_entry_for_approve.WFTemplate_ID = template.Id;
                                new_entry_for_approve.IsActive = true;
                                new_entry_for_approve.TimeStamp = CurrentTime;
                                new_entry_for_approve.WebLink = "";
                                new_entry_for_approve.RoleId = template.Role_ID;
                                status = _entity.SaveChanges() > 0;
                            }
                        }
                        #endregion
                    }
                }
            }
            else
            {
                var template = _entity.tb_WF_Template.Where(x => x.WF_ID == data.tb_WFType.Id && x.Sequence_NO == 1 && x.IsActive == true && x.Action_Flag != 3).FirstOrDefault();
                if (template != null)
                {
                    if (template.Role_ID != null)
                    {
                        var role = Find_RoleDetails(template.tb_Role, data, myData, template.Status_ID);// 21-02-2020 ARCHANA SRISHTI 
                        rolAssign = role.assigned_person_id;
                        roleDescription = role.role_name;
                        var new_entry_for_approve = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id && x.IsActive == true).FirstOrDefault();
                        #region 
                        if (new_entry_for_approve.Employee_ID != rolAssign) // Check the request owner and the approver is not same 
                        {
                            new_entry_for_approve.Request_ID = request_id;
                            new_entry_for_approve.WF_ID = wfType.Id;
                            new_entry_for_approve.Approver_ID = rolAssign;
                            new_entry_for_approve.OrgApprover_ID = rolAssign;
                            new_entry_for_approve.Status_ID = template.Status_ID;
                            new_entry_for_approve.Approval_No = 1.ToString();
                            new_entry_for_approve.WFTemplate_ID = template.Id;
                            new_entry_for_approve.IsActive = true;
                            new_entry_for_approve.TimeStamp = CurrentTime;
                            new_entry_for_approve.WebLink = "";
                            new_entry_for_approve.RoleId = template.Role_ID;
                            status = _entity.SaveChanges() > 0;
                        }
                        else
                        {
                            //getting the linemanager for Approverid  //11-03-2020  Nimmi Mohan//Preetha
                            var line_manager = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == myData.Line_Manager && x.IsActive == true).FirstOrDefault();
                            //var businessLine = _entity.tb_BusinessLine.Where(x => x.BL_Id == myData.tb_Department.tb_ProductGroup.BusinessLine_Id && x.BL_Manager != null && x.IsActive == true).FirstOrDefault();
                            if (line_manager != null)
                            {
                                new_entry_for_approve.Request_ID = request_id;
                                new_entry_for_approve.WF_ID = wfType.Id;
                                new_entry_for_approve.Approver_ID = myData.Line_Manager;
                                new_entry_for_approve.OrgApprover_ID = rolAssign;
                                new_entry_for_approve.Status_ID = template.Status_ID;
                                new_entry_for_approve.Approval_No = 1.ToString();
                                new_entry_for_approve.WFTemplate_ID = template.Id;
                                new_entry_for_approve.IsActive = true;
                                new_entry_for_approve.TimeStamp = CurrentTime;
                                new_entry_for_approve.WebLink = "";
                                new_entry_for_approve.RoleId = template.Role_ID;
                                status = _entity.SaveChanges() > 0;
                            }
                        }
                        #endregion
                    }
                }
            }
            return new Tuple<bool, string, string, string>(status, profile.Emp_Name, roleDescription, rolAssign);
        }

        public Tuple<bool, string> OLD_NOT_USE_ApproveRequest(string request_id, string myId, string reason)
        {
            bool status = false;
            string roleid = "";
            string myPosition = "";
            string myRole = "";
            string msg = "";
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id).OrderByDescending(x => x.Id).FirstOrDefault(); // Last row of the request
            string old_approval_no = data.Approval_No;
            string old_approver_id = data.Approver_ID;
            string old_status = data.Status_ID;
            var my_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == myId && x.IsActive == true).FirstOrDefault();// Approver personal data
            //var role = _entity.tb_Role.Where(x => x.Assigned_ID == myId && x.IsActive == true && x.Application_ID == data.Application_ID).FirstOrDefault();// Approve current role
            var role = _entity.tb_Role.Where(x => x.Id == data.RoleId && x.IsActive == true).FirstOrDefault();
            var initiator_details = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID).FirstOrDefault();// Requset for whom 
            //var wfTypes = _entity.tb_WFType.Where(x => x.Id == data.WF_ID && x.IsActive == true).FirstOrDefault(); //This request wf type data
            //var closingType = _entity.tb_Closing_Type.Where(x => x.Id == data.tb_WFType.Closing_Type_Id && x.IsActive == true).FirstOrDefault();// Closing process of this wf type
            //var country = _entity.tb_Country.Where(x => x.Id == initiator_details.Country_Id && x.IsActive == true).FirstOrDefault();
            //var profile = _entity.tb_Emp_Profile.Where(x => x.Id == initiator_details.Profile_ID && x.IsActive == true).FirstOrDefault();
            //var application = _entity.tb_Application.Where(x => x.Application_Code == data.Application_ID && x.IsActive == true).FirstOrDefault();
            var application = data.tb_Application;
            #region Delegate
            if (data.Approver_ID != data.OrgApprover_ID)
            {
                var delegatePerson = _entity.tb_Role.Where(x => x.IsActive == true).FirstOrDefault();
                roleid = delegatePerson.Role_ID; // checking that this request is coming from the delegate process to me 
                myPosition = delegatePerson.Role_Desc;
                myRole = delegatePerson.Role_Desc;
            }
            else
            {
                roleid = role.Role_ID;
                myPosition = role.Role_Desc;
                myRole = role.Role_Desc;
            }
            #endregion Delegate
            if (myRole != string.Empty)
            {
                #region Role 
                var approver_id = Convert.ToInt32(data.Approval_No);// Not a mistake, its approval no
                int approvalStatus = Convert.ToInt32(statusEnum.Approval);
                var wfTemplate = _entity.tb_WF_Template.Where(x => x.WF_ID == data.WF_ID && x.Profile_ID == initiator_details.Profile_ID && x.Action_Flag == approvalStatus && x.Sequence_NO > approver_id).OrderBy(x => x.Sequence_NO).FirstOrDefault();// For approve
                if (wfTemplate != null)
                {
                    #region 
                    var newApprover = _entity.tb_Role.Where(x => x.Id == wfTemplate.Role_ID && x.IsActive == true).FirstOrDefault();
                    if (newApprover != null) // Next Approver
                    {
                        if (newApprover.Assigned_ID != null) // Have a person for next approve
                        {
                            if (newApprover.Assigned_ID != data.Employee_ID)// I am not the next Approver
                            {
                                var checkDelegate = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == newApprover.Assigned_ID && x.IsActive == true).FirstOrDefault();
                                #region Insert a row for next approver
                                data.Request_ID = data.Request_ID;
                                data.WF_ID = data.WF_ID;
                                data.Application_ID = data.Application_ID;
                                data.Employee_ID = data.Employee_ID;
                                if (checkDelegate.DelegationFlag == true)
                                    data.Approver_ID = checkDelegate.Delegate_Emp_Code == null ? newApprover.Assigned_ID : checkDelegate.Delegate_Emp_Code;
                                else
                                    data.Approver_ID = newApprover.Assigned_ID;
                                data.OrgApprover_ID = newApprover.Assigned_ID;
                                data.Creater_ID = data.Creater_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 1);
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.WebLink = null;
                                data.IsActive = true;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_approval_no, data.Approver_ID, newApprover.Role_Desc, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                }
                                #endregion Insert a row for next approver
                            }
                            else
                            {
                                var bus_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == initiator_details.tb_Department.tb_ProductGroup.BusinessLine_Id && x.IsActive == true).FirstOrDefault();
                                var checkDelegate = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == bus_line.BL_Manager && x.IsActive == true).FirstOrDefault();
                                #region Insert a row for next approver
                                data.Request_ID = data.Request_ID;
                                data.WF_ID = data.WF_ID;
                                data.Application_ID = data.Application_ID;
                                data.Employee_ID = data.Employee_ID;
                                if (checkDelegate.DelegationFlag == true)
                                    data.Approver_ID = checkDelegate.Delegate_Emp_Code == null ? newApprover.Assigned_ID : checkDelegate.Delegate_Emp_Code;
                                else
                                    data.Approver_ID = newApprover.Assigned_ID;
                                data.OrgApprover_ID = newApprover.Assigned_ID;
                                data.Creater_ID = data.Creater_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 1);
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.WebLink = null;
                                data.IsActive = true;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_approval_no, data.Approver_ID, newApprover.Role_Desc, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                }
                                #endregion Insert a row for next approver
                            }
                        }
                        else
                        {
                            msg = "Approve not completed, because the next approver is not assigned!";
                        }
                    }
                    else // Approver not defined
                    {
                        msg = "Next Approver not defined";
                    }
                    #endregion
                }
                else
                {
                    #region
                    if (data.Status_ID == statusEnum.Approval.ToString())// Last approver only give this
                    {
                        #region  Keep Log for APC 
                        var apcLog = _entity.tb_ApprovalLog.Create();
                        apcLog.RequestId = request_id;
                        apcLog.Remark = "Request Approval Cycle Completed by";
                        apcLog.EmployeeId = data.Employee_ID;
                        apcLog.Actor_Id = myId;
                        apcLog.RoleId = myRole;
                        apcLog.SequenceNo = data.Approval_No == null ? 0 : Convert.ToInt32(data.Approval_No);
                        apcLog.TimeStamp = CurrentTime;
                        apcLog.IsActive = true;
                        apcLog.Status = "APC";
                        apcLog.Reason = reason;
                        _entity.tb_ApprovalLog.Add(apcLog);
                        _entity.SaveChanges();
                        #endregion

                        #region Insert APC , means me is the last approver , so i set the After Cycle Complete process
                        var checkDelegate = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Approver_ID && x.IsActive == true).FirstOrDefault();

                        data.Request_ID = data.Request_ID;
                        data.WF_ID = data.WF_ID;
                        data.Application_ID = data.Application_ID;
                        data.Employee_ID = data.Employee_ID;
                        if (checkDelegate.DelegationFlag == true)
                            data.Approver_ID = checkDelegate.Delegate_Emp_Code == null ? data.Approver_ID : checkDelegate.Delegate_Emp_Code;
                        else
                            data.Approver_ID = data.Approver_ID;
                        data.OrgApprover_ID = data.OrgApprover_ID;
                        data.Creater_ID = data.Creater_ID;
                        data.Status_ID = "APC";
                        data.Approval_No = data.Approval_No;
                        data.WFTemplate_ID = data.WFTemplate_ID;
                        data.WebLink = "";
                        data.IsActive = true;
                        data.TimeStamp = CurrentTime;
                        status = _entity.SaveChanges() > 0;
                        string remark = "Request Approval Cycle Completed by";
                        bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_approval_no, "", "", remark, data, "");
                        #endregion Insert APC , means me is the last approver , so i set the After Cycle Complete process
                    }
                    msg = "Approved Successfully";//Confusion
                    if (data.tb_WFType.tb_Closing_Type.Code == "CC") // Means this last approverr can close the cycle , for that can enter a row as CLS automaticaly(Request Closed)
                    {
                        #region Close the Request by this approver
                        data.Request_ID = data.Request_ID;
                        data.WF_ID = data.WF_ID;
                        data.Application_ID = data.Application_ID;
                        data.Employee_ID = data.Employee_ID;
                        data.Approver_ID = myId;
                        data.OrgApprover_ID = myId;
                        data.Creater_ID = data.Creater_ID;
                        data.Status_ID = "CLS";// Request Closed
                        data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);// Because 1 will add while the APC section , then the next process have more more sequence number
                        data.WFTemplate_ID = data.WFTemplate_ID;
                        data.WebLink = "";
                        data.IsActive = true;
                        data.TimeStamp = CurrentTime;
                        status = _entity.SaveChanges() > 0;
                        string remark = "Request was closed by";
                        bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_approval_no, "", "", remark, data, reason);
                        #endregion Close the Request by this approver
                    }
                    else if (data.tb_WFType.tb_Closing_Type.Code == "PC")
                    {
                        var process = Convert.ToInt32(template_ActionFlag.Process) + 1;// PC Means this request waiting for next processing Approvel request for processing 
                        var checkApp = _entity.tb_WF_Template.Where(x => x.WF_ID == data.WF_ID && x.Profile_ID == initiator_details.Profile_ID && x.Action_Flag == process && x.Sequence_NO > approver_id).OrderBy(x => x.Sequence_NO).FirstOrDefault();// For APP
                        if (checkApp != null)
                        {
                            #region Next Process
                            var newApprover = _entity.tb_Role.Where(x => x.Id == checkApp.Role_ID && x.IsActive == true).FirstOrDefault(); // Next Processor 
                            if (newApprover != null)
                            {
                                if (newApprover.Assigned_ID != null)
                                {
                                    var newApproverData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == newApprover.Assigned_ID && x.IsActive == true).FirstOrDefault();
                                    #region Next Process 

                                    #region  Keep Log for APC 
                                    var apcLog = _entity.tb_ApprovalLog.Create();
                                    apcLog.RequestId = request_id;
                                    apcLog.Remark = "Request Approval Cycle Completed by";
                                    apcLog.EmployeeId = data.Employee_ID;
                                    apcLog.Actor_Id = myId;
                                    apcLog.RoleId = myRole;
                                    apcLog.SequenceNo = data.Approval_No == null ? 0 : Convert.ToInt32(data.Approval_No);
                                    apcLog.TimeStamp = CurrentTime;
                                    apcLog.IsActive = true;
                                    apcLog.Status = "APC";
                                    apcLog.Reason = reason;
                                    _entity.tb_ApprovalLog.Add(apcLog);
                                    _entity.SaveChanges();
                                    #endregion

                                    if (newApprover.Assigned_ID == data.Employee_ID)// The request owher is the net approver 
                                    {
                                        var bus_line = _entity.tb_BusinessLine.Where(x => x.BL_Id == initiator_details.tb_Department.tb_ProductGroup.BusinessLine_Id && x.IsActive == true).FirstOrDefault();
                                        newApproverData = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == bus_line.BL_Manager && x.IsActive == true).FirstOrDefault();
                                    }
                                    data.Request_ID = data.Request_ID;
                                    data.WF_ID = data.WF_ID;
                                    data.Application_ID = data.Application_ID;
                                    data.Employee_ID = data.Employee_ID;
                                    if (newApproverData.LocalEmplyee_ID != newApproverData.Delegate_Emp_Code && newApproverData.DelegationFlag == true)
                                    {
                                        data.Approver_ID = newApproverData.Delegate_Emp_Code;
                                    }
                                    else
                                    {
                                        data.Approver_ID = newApproverData.LocalEmplyee_ID;
                                    }
                                    data.OrgApprover_ID = newApprover.Assigned_ID;
                                    data.Creater_ID = data.Creater_ID;
                                    data.Status_ID = checkApp.Status_ID;
                                    data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 1);
                                    data.WFTemplate_ID = data.WFTemplate_ID;
                                    data.WebLink = "";
                                    data.IsActive = true;
                                    data.TimeStamp = CurrentTime;
                                    status = _entity.SaveChanges() > 0;
                                    string remark = "Request is waiting for process";
                                    bool sendMail = Send_Approval_Mail_Login("APP", request_id, myPosition, myRole, data.Employee_ID, myId, old_approval_no, data.Approver_ID, newApprover.Role_Desc, remark, data, "");

                                    if (checkApp.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, checkApp.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception ex)
                                        {
                                            msg = ex.Message;
                                        }
                                    }

                                    #endregion Next Process 
                                }
                                else
                                {
                                    msg = "Approve not completed, because the next approver is not assigned!";
                                }
                            }
                            else
                            {
                                msg = "Next Processor not defined";
                            }
                            #endregion
                        }
                        else
                        {
                            #region Close the process
                            data.Request_ID = data.Request_ID;
                            data.WF_ID = data.WF_ID;
                            data.Application_ID = data.Application_ID;
                            data.Employee_ID = data.Employee_ID;
                            data.Approver_ID = my_data.LocalEmplyee_ID;
                            data.OrgApprover_ID = my_data.LocalEmplyee_ID;
                            data.Creater_ID = data.Creater_ID;
                            data.Status_ID = "CLS";
                            data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No));
                            data.WFTemplate_ID = data.WFTemplate_ID;
                            data.WebLink = "";
                            data.IsActive = true;
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            string remark = "Request was closed by";
                            bool sendMail = Send_Close_Mail_Login(request_id, myPosition, myRole, data.Employee_ID, myId, old_approval_no, remark, data);
                            #endregion Close the process
                        }
                    }
                    #endregion
                }
                #endregion
            }
            return new Tuple<bool, string>(status, msg);
        }

        public Tuple<bool, string, string, string> RequestPaid(string request_id, string my_id)
        {
            bool status = false;
            var data = _entity.tb_Request_Hdr.Where(x => x.IsActive == true && x.Request_ID == request_id).OrderByDescending(x => x.Id).FirstOrDefault();
            string myPosition = "";
            string myRole = "";
            //var application = _entity.tb_Application.Where(x => x.Application_Code == data.Application_ID && x.IsActive == true).FirstOrDefault();
            var template = _entity.tb_WFType.Where(x => x.Id == data.WF_ID && x.IsActive == true).FirstOrDefault();
            var application = data.tb_Application;
            if (data.Approver_ID == data.OrgApprover_ID)
            {
                var role = _entity.tb_Role.Where(x => x.Assigned_ID == data.Approver_ID && x.IsActive == true).FirstOrDefault();
                if (role != null)
                {
                    myPosition = role.Role_Desc;
                    myRole = role.Role_ID;
                }
                else
                {
                    var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == data.Approver_ID && x.Description == my_id && x.IsActive == true).FirstOrDefault();
                    myPosition = universal.Code;
                    myRole = universal.Code;
                }
            }
            else
            {
                var role = _entity.tb_Role.Where(x => x.Assigned_ID == data.OrgApprover_ID && x.IsActive == true).FirstOrDefault();
                myPosition = role.Role_Desc;
                myRole = role.Role_ID;
            }
            data.Request_ID = data.Request_ID;
            data.WF_ID = data.WF_ID;
            data.Application_ID = data.Application_ID;
            data.Employee_ID = data.Employee_ID;
            data.Approver_ID = my_id;
            data.OrgApprover_ID = my_id;
            data.Creater_ID = data.Creater_ID;
            data.Status_ID = "PYD"; ;
            data.Approval_No = data.Approval_No;
            data.WFTemplate_ID = null;
            data.WebLink = "";
            data.IsActive = true;
            data.TimeStamp = CurrentTime;
            data.Process_Complete = 1;
            if (template.WF_ID == "P052")
            {
                //05/06/2020 ALENA SICS START------------for EOSB calculation
                var wftype = _entity.tb_PP_EOSB_Calculation.Where(x => x.RequestId == data.Request_ID).FirstOrDefault();
                if (data.Request_ID == wftype.RequestId)
                {

                    string request;
                    request = Convert.ToString(wftype.EndofServicePayment);
                    var input1 = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request).FirstOrDefault();
                    if (input1.Request_ID == request)
                    {
                        input1.IsLInked = true;

                    }
                }
            }
            //END------------------
            status = _entity.SaveChanges() > 0;
            return new Tuple<bool, string, string, string>(status, myPosition, myPosition, data.Approval_No);
        }

        public Tuple<bool, string, string, string, string, string, tb_Request_Hdr> CloseRequest(string request_id, string my_id)
        {
            bool status = false;
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id).OrderByDescending(x => x.Id).FirstOrDefault(); // Last row of the request
            var application = data.tb_Application;
            string myPosition = "";
            string myRole = "";
            string old_ApproverId = data.Approver_ID;
            string old_ApproverNo = data.Approval_No;
            myRole = data.tb_WF_Template.Role_ID.ToString();
            myPosition = data.tb_WF_Template.tb_Role.Role_Desc;
            data.Approver_ID = my_id;
            data.OrgApprover_ID = my_id;
            data.Status_ID = "CLS";
            data.Process_Complete = 1;
            data.IsActive = true;
            data.TimeStamp = CurrentTime;
            status = _entity.SaveChanges() > 0;
            return new Tuple<bool, string, string, string, string, string, tb_Request_Hdr>(status, myPosition, myRole, old_ApproverId, old_ApproverNo, data.Employee_ID, data);
        }

        public Tuple<bool, string, string, string, string, string, tb_Request_Hdr> RejectRequest(string request_id, string my_id)
        {
            bool status = false;
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id).OrderByDescending(x => x.Id).FirstOrDefault(); // Last row of the request
            //var application = _entity.tb_Application.Where(x => x.Application_Code == data.Application_ID && x.IsActive == true).FirstOrDefault();
            var application = data.tb_Application;
            string myPosition = "";
            string myRole = "";
            string emp_id = data.Employee_ID;
            string old_approverId = data.Approver_ID;
            string old_approverNo = data.Approval_No;
            if (data.Approver_ID == data.OrgApprover_ID)
            {
                var role = _entity.tb_Role.Where(x => x.Assigned_ID == data.Approver_ID && x.IsActive == true).FirstOrDefault();
                myPosition = role.Role_Desc;
                myRole = role.Role_Desc;
            }
            else
            {
                var role = _entity.tb_Role.Where(x => x.Assigned_ID == data.OrgApprover_ID && x.IsActive == true).FirstOrDefault();
                myPosition = role.Role_Desc;
                myRole = role.Role_Desc;
            }
            data.Request_ID = data.Request_ID;
            data.WF_ID = data.WF_ID;
            data.Application_ID = data.Application_ID;
            data.Employee_ID = data.Employee_ID;
            data.Approver_ID = my_id;
            data.OrgApprover_ID = my_id;
            data.Creater_ID = data.Creater_ID;
            data.Status_ID = "REJ";
            data.Approval_No = data.Approval_No;
            data.WFTemplate_ID = null;
            data.WebLink = "";
            data.IsActive = true;
            data.TimeStamp = CurrentTime;
            status = _entity.SaveChanges() > 0;
            return new Tuple<bool, string, string, string, string, string, tb_Request_Hdr>(status, myPosition, myRole, emp_id, old_approverId, old_approverNo, data);
        }
        public Tuple<bool, string, string, string, string, string, tb_Request_Hdr> CancelRequestByProcessor(string request_id, string my_id)
        {
            bool status = false;
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id).OrderByDescending(x => x.Id).FirstOrDefault(); // Last row of the request
            string myPosition = "";
            string myRole = "";
            string emp_id = data.Employee_ID;
            //string old_approverId = data.Approver_ID;
            string old_approverId = my_id;// 21-02-2020 ARCHANA SRISHTI 

            var log = _entity.tb_ApprovalLog.Where(x => x.RequestId == request_id && x.Actor_To == my_id && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
            if (log != null)
            {
                myPosition = log.RoleId_To;
                myRole = log.RoleId_To;
            }
            else // 21-02-2020 ARCHANA SRISHTI 
            {
                var universallookUp = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == data.OrgApprover_ID && x.Description == my_id && x.IsActive == true).FirstOrDefault();
                myPosition = universallookUp.Code;
                myRole = universallookUp.Code;
            }

            data.Request_ID = data.Request_ID;
            data.WF_ID = data.WF_ID;
            data.Application_ID = data.Application_ID;
            data.Employee_ID = data.Employee_ID;
            data.Approver_ID = my_id;
            data.OrgApprover_ID = my_id;
            data.Creater_ID = data.Creater_ID;
            data.Status_ID = "CNL";
            data.Approval_No = data.Approval_No;
            data.WFTemplate_ID = null;
            data.WebLink = "";
            data.IsActive = true;
            data.TimeStamp = CurrentTime;
            status = _entity.SaveChanges() > 0;
            return new Tuple<bool, string, string, string, string, string, tb_Request_Hdr>(status, myPosition, myRole, emp_id, old_approverId, data.Approval_No, data);
        }
        public Tuple<bool, tb_Request_Hdr> RequestRerouting(string request_id, string my_id, string rerouting_to)
        {
            bool status = false;
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id).OrderByDescending(x => x.Id).FirstOrDefault(); // Last row of the request
            string emp_id = data.Employee_ID;
            string old_approverId = data.Approver_ID;
            data.Approver_ID = rerouting_to;
            data.OrgApprover_ID = rerouting_to;
            data.TimeStamp = CurrentTime;
            status = _entity.SaveChanges() > 0;
            return new Tuple<bool, tb_Request_Hdr>(status, data);
        }

        public bool DistributionRequest(tb_Request_Hdr request, long distributionId, string sequence, string mailContent, string my_id, string my_role)//NOT USING 25-02-2020 ARCHANA SRISHTI
        {
            bool status = false;
            int level = Convert.ToInt32(sequence);
            long dis = Convert.ToInt64(distributionId);
            var employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == request.Employee_ID && x.IsActive == true).FirstOrDefault();
            var distribution_list = _entity.tb_DistributionList.Where(x => x.DistributionList_Code == dis && x.WF_Level == level && x.WF_ID == request.WF_ID && x.IsActive == true).ToList();
            //string host = "http://202.191.65.67:4204/";
            string host = _entity.tb_Hostaddress.Where(x => x.IsActive == true).FirstOrDefault().Host_Address;
            var id = request.Id.ToString() + '~' + "0~ForwardView";
            mailContent = "Please click this link for view the request details : " + host + "/Request/DetailedRequest?id=" + id + "~0~1~Distribution"; // Archana 02-07-2020 sRISHTI //12-02-2020 ARCHANA DISTRIBUTION
            foreach (var item in distribution_list) // List the all distribution list persons
            {
                var check_user = Find_Role_UserForDistribution(request, item.Role_Id ?? 0, employee.tb_Location.Country_Id ?? 0, my_id, my_role, mailContent, employee);
                if (check_user == "" && item.Employee_ID != null && item.Employee_ID != string.Empty)
                {
                    Send_Distribution_mail(request.Request_ID, my_id, my_role, item.Employee_ID, "", mailContent, request, employee);
                }
            }
            return status;
        }

        /// <summary>
        /// <head name="This Method is used to send the mail for get the employee oid"></head>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="role_Id"></param>
        /// <param name="application_ID"></param>
        /// <param name="country_code"></param>
        /// <param name="my_id"></param>
        /// <param name="my_role"></param>
        /// <param name="mailContent"></param>
        /// <param name="employee"></param>
        public string Find_Role_UserForGetEmployee(long role_Id, string application_ID, long countryId, tb_WF_Employee employee, string wf_id)
        {
            var country = _entity.tb_Country.Where(x => x.Id == countryId && x.IsActive == true).FirstOrDefault();
            var application = _entity.tb_Application.Where(x => x.Application_Code == application_ID && x.IsActive == true).FirstOrDefault();
            var role = _entity.tb_Role.Where(x => x.Id == role_Id).FirstOrDefault();
            if (role != null)
            {
                if (role.Organization_Flag == true && role.org_type != null)// Checking that we wants to find the user by using Organization type tavle or can normal
                {
                    #region 
                    if (role.org_type.Trim() == "DT")// Check the department table 
                    {
                        #region Department
                        var department = _entity.tb_Department.Where(x => x.IsActive == true && x.Department_Id == employee.Department_Id).FirstOrDefault();
                        if (department != null)
                        {
                            if (role.role_type == "Head" && department.Dept_Manager != null)
                            {
                                return department.Dept_Manager;
                            }
                            else if (role.role_type == "Manager1" && department.Dept_Controller != null)
                            {
                                return department.Dept_Controller;
                            }
                            else if (role.role_type == "Manager2" && department.Dep_Office_Admin != null)
                            {
                                return department.Dep_Office_Admin;
                            }
                        }
                        #endregion Department
                    }
                    else if (role.org_type.Trim() == "BL")// Check the business line table 
                    {
                        #region Business Line
                        var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.tb_Department.tb_ProductGroup.BusinessLine_Id).FirstOrDefault();
                        if (business != null)
                        {
                            if (role.role_type == "Manager1" && business.BL_Manager != null)
                            {
                                return business.BL_Manager;
                            }
                            else if (role.role_type == "Manager2" && business.BL_Controller != null)
                            {
                                return business.BL_Controller;
                            }
                            else if (role.role_type == "Manager3" && business.BL_Office_Admin != null)
                            {
                                return business.BL_Office_Admin;
                            }

                        }
                        #endregion Business Line
                    }
                    else if (role.org_type.Trim() == "B")// Check the business 
                    {
                        #region Business
                        var business = _entity.tb_Business.Where(x => x.Country_Id == country.Id && x.IsActive == true && x.Bus_Id == employee.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Id).FirstOrDefault();
                        if (business != null)
                        {
                            if (role.role_type == "Manager1" && business.Bus_Manager != null)
                            {
                                return business.Bus_Manager;
                            }
                            else if (role.role_type == "Manager2" && business.Bus_Controller != null)
                            {
                                return business.Bus_Controller;
                            }
                            else if (role.role_type == "Manager3" && business.Bus_Office_Admin != null)
                            {
                                return business.Bus_Office_Admin;
                            }
                        }
                        #endregion Business
                    }
                    else if (role.org_type == "PG")// Check the Product group table 
                    {
                        #region Product Group
                        var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.tb_Department.PG_Id).FirstOrDefault();
                        if (product != null)
                        {
                            if (role.role_type == "Manager1" && product.PG_Manager != null)
                            {
                                return product.PG_Manager;
                            }
                            else if (role.role_type == "Manager2" && product.PG_Controller != null)
                            {
                                return product.PG_Controller;
                            }
                            else if (role.role_type == "Manager3" && product.PG_Office_Admin != null)
                            {
                                return product.PG_Office_Admin;
                            }
                        }
                        #endregion Product Group 
                    }
                    #endregion
                }
                else
                {
                    #region 
                    if (role.Assigned_ID != null)
                    {
                        #region Direct 
                        return role.Assigned_ID;
                        #endregion Direct 
                    }
                    else
                    {
                        //var checkLookupTable = _entity.tb_UniversalLookupTable.Where(x => x.Code == role_Id && x.IsActive == true).FirstOrDefault();
                        var checkLookupTable = _entity.tb_UniversalLookupTable.Where(x => x.Code == wf_id && x.Table_Name == role.Role_ID && x.IsActive == true).FirstOrDefault();
                        if (checkLookupTable != null)
                        {
                            //var takeData = _entity.wf_Find_Row(role_Id, application_ID, wf_id, country_code, checkLookupTable.Table_Name).FirstOrDefault();
                            //if (takeData != null)
                            //{
                            //    return takeData.Employee_Id;
                            //}
                            return checkLookupTable.Description;
                        }
                    }
                    #endregion
                }
            }
            return "";
        }

        public tb_WF_Template GetTemplateDetails(long role_ID, tb_WFType wF_ID, long profile_ID, string approvalNo)// this isused to separate the wf type which related to profile or note
        {
            int sequence_no = Convert.ToInt32(approvalNo);
            if (wF_ID.HaveProfile == true)
            {
                var temp = _entity.tb_WF_Template.Where(x => x.Role_ID == role_ID && x.WF_ID == wF_ID.Id && x.Profile_ID == profile_ID && x.IsActive == true && x.Sequence_NO >= sequence_no).FirstOrDefault();
                return temp;
            }
            else
            {
                var temp = _entity.tb_WF_Template.Where(x => x.Role_ID == role_ID && x.WF_ID == wF_ID.Id && x.IsActive == true && x.Sequence_NO >= sequence_no).FirstOrDefault();
                return temp;
            }
        }

        public tb_WF_Template GetTemplateDetails_P016(long role_ID, tb_WFType wF_ID, long profile_ID, string approvalNo)// this isused to separate the wf type which related to profile or note
        {
            int sequence_no = Convert.ToInt32(approvalNo);
            if (wF_ID.HaveProfile == true)
            {
                var temp = _entity.tb_WF_Template.Where(x => x.Role_ID == role_ID && x.WF_ID == wF_ID.Id && x.Profile_ID == profile_ID && x.IsActive == true).FirstOrDefault();
                return temp;
            }
            else
            {
                var temp = _entity.tb_WF_Template.Where(x => x.Role_ID == role_ID && x.WF_ID == wF_ID.Id && x.IsActive == true && x.Sequence_NO >= sequence_no).FirstOrDefault();
                return temp;
            }
        }

        public Tuple<bool, string, long, bool> ApproveRequest(string request_id, string myId, string reason) //28-05-2020
        {
            bool status = false;
            string msg = "Failed";
            string roleId = "";
            string myPosition = "";
            string myRole = "";
            int identify_escalation = 3;
            int approvalStatus = Convert.ToInt32(statusEnum.Approval);
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request_id).FirstOrDefault();
            string old_Approval_np = data.Approval_No;
            string old_approver_id = data.Approver_ID;
            string old_status = data.Status_ID;
            long distributionId = 0; //04-03-2020 ARCHANA K V SRISHI 
            var my_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == myId && x.IsActive == true).FirstOrDefault();
            var initiator_details = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID).FirstOrDefault();// Requset for whom 
            var role = _entity.tb_Role.Where(x => x.Id == data.RoleId && x.IsActive == true).FirstOrDefault();
            int current_sequence = Convert.ToInt32(data.Approval_No);
            long reqId = Convert.ToInt64(data.Request_ID);
            bool isdistributionflag = false; //Basheer on 28-05-2020


            if (role != null)
            {
                myRole = role.Role_Desc;

                #region Find the next approver process
                if (data.tb_WFType.HaveProfile == true)
                {
                    #region Profile Oriented
                    var wfTemplate = _entity.tb_WF_Template.Where(x => x.WF_ID == data.WF_ID && x.Profile_ID == initiator_details.Profile_ID && x.Sequence_NO > current_sequence && x.Action_Flag != identify_escalation).OrderBy(x => x.Sequence_NO).FirstOrDefault();// For approve
                    if (wfTemplate != null)
                    {
                        distributionId = wfTemplate.DistributionList_ID ?? 0; // 04-03-2020 ARCHANA K V SRISHTI 

                        #region Have Next Approver
                        if (wfTemplate.tb_Role != null)
                        {
                            RoleDetails roleDetails = new RoleDetails();
                            //if (data.Approval_No == "1" && data.Status_ID == "INT")
                            //{
                            //    //Preema-09-06-2020
                            //    //request owner's immediate line manager's immediate line manager
                            //    var snd_approver = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Approver_ID).FirstOrDefault();
                            //    roleDetails.deligated_personId = snd_approver.Line_Manager;
                            //    roleDetails.assigned_person_id = snd_approver.Line_Manager;
                            //}
                            //else
                            //{
                            roleDetails = Find_RoleDetails(wfTemplate.tb_Role, data, initiator_details, wfTemplate.Status_ID); // 21-02-2020 ARCHANA SRISHTI                           
                            //}

                            if (roleDetails.role_id == "HRAHEAD" && data.tb_WFType.WF_ID == "A007")//A007-HR Administration Section Head (Region Wise)
                            {
                                var employee_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
                                var region = _entity.tb_Location.Where(x => x.Location_Id == employee_data.Location_Id && x.IsActive == true).FirstOrDefault();
                                var role_region = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == "HRAHEAD" && x.Code == region.Region && x.IsActive == true).FirstOrDefault();

                                data.Approver_ID = role_region.Description;
                                data.OrgApprover_ID = role_region.Description;
                                data.Approval_No = (Convert.ToInt32(data.Approval_No) + 1).ToString();
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.RoleId = wfTemplate.Role_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    #region For History
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, data.Approver_ID, roleDetails.role_name, remark, data, reason);  //Basheer on 19-03-2020
                                    //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, data.Approval_No, data.Approver_ID, roleDetails.role_name, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                    #endregion For History
                                }
                            }

                            else if (roleDetails.role_id == "HRADSPTOFFS" && data.tb_WFType.WF_ID == "A007")//A007-HR Administration Support Officer (Region Wise)
                            {
                                var employee_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
                                var region = _entity.tb_Location.Where(x => x.Location_Id == employee_data.Location_Id && x.IsActive == true).FirstOrDefault();
                                var role_region = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == "HRADSPTOFFS" && x.Code == region.Region && x.IsActive == true).FirstOrDefault();

                                data.Approver_ID = role_region.Description;
                                data.OrgApprover_ID = role_region.Description;
                                data.Approval_No = (Convert.ToInt32(data.Approval_No) + 1).ToString();
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.RoleId = wfTemplate.Role_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    #region For History
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, data.Approver_ID, roleDetails.role_name, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                    #endregion For History
                                }
                            }

                            // 30/06/2020 ALENA SICS FOR A008
                            else if (roleDetails.role_id == "HRADMINHEAD" && data.tb_WFType.WF_ID == "A008")//A008-HR Administration Section Head (Region Wise)
                            {
                                var employee_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
                                var region = _entity.tb_Location.Where(x => x.Location_Id == employee_data.Location_Id && x.IsActive == true).FirstOrDefault();
                                var role_region = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == "HRADMINHEAD-EAST" || x.Table_Name == "HRADMINHEAD-WEST" || x.Table_Name == "HRADMINHEAD-CENTRAL" && x.Code == region.Region && x.IsActive == true).FirstOrDefault();

                                data.Approver_ID = role_region.Description;
                                data.OrgApprover_ID = role_region.Description;
                                data.Approval_No = (Convert.ToInt32(data.Approval_No) + 1).ToString();
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.RoleId = wfTemplate.Role_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    #region For History
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, data.Approver_ID, roleDetails.role_name, remark, data, reason);  //Basheer on 19-03-2020
                                    //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, data.Approval_No, data.Approver_ID, roleDetails.role_name, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                    #endregion For History
                                }
                            }

                            else if (roleDetails.role_id == "HRADSPTOFFS" && data.tb_WFType.WF_ID == "A008")//A008-HR Administration Support Officer (Region Wise)
                            {
                                var employee_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
                                var region = _entity.tb_Location.Where(x => x.Location_Id == employee_data.Location_Id && x.IsActive == true).FirstOrDefault();
                                var role_region = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == "HRADSPTOFFS" && x.Code == region.Region && x.IsActive == true).FirstOrDefault();

                                data.Approver_ID = role_region.Description;
                                data.OrgApprover_ID = role_region.Description;
                                data.Approval_No = (Convert.ToInt32(data.Approval_No) + 1).ToString();
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.RoleId = wfTemplate.Role_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    #region For History
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, data.Approver_ID, roleDetails.role_name, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                    #endregion For History
                                }
                            }
                            // end--
                            else
                            {
                                data.Approver_ID = roleDetails.deligated_personId;
                                data.OrgApprover_ID = roleDetails.assigned_person_id; //21-02-2020 ARCHANA SRISHTI 

                                data.Approval_No = (Convert.ToInt32(data.Approval_No) + 1).ToString();
                                data.WFTemplate_ID = wfTemplate.Id;
                                data.RoleId = wfTemplate.Role_ID;
                                data.Status_ID = wfTemplate.Status_ID;
                                data.TimeStamp = CurrentTime;
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    #region For History
                                    msg = "Approved Successfully";
                                    string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                    string remark = "Request was approved by ";
                                    bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, data.Approver_ID, roleDetails.role_name, remark, data, reason);
                                    if (wfTemplate.DistributionList_ID != null)
                                    {
                                        try
                                        {
                                            DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                        }
                                        catch (Exception es)
                                        {
                                            msg = es.Message;
                                        }
                                    }
                                    #endregion For History
                                }
                            }
                            if (wfTemplate.Action_Flag != approvalStatus)//This is the last approvers
                            {
                                #region  Keep Log for APC 
                                var apcLog = _entity.tb_ApprovalLog.Create();
                                apcLog.RequestId = request_id;
                                apcLog.Remark = "Request Approval Cycle Completed by";
                                apcLog.EmployeeId = data.Employee_ID;
                                apcLog.Actor_Id = myId;
                                apcLog.RoleId = myRole;
                                apcLog.SequenceNo = data.Approval_No == null ? 0 : Convert.ToInt32(data.Approval_No);
                                apcLog.TimeStamp = CurrentTime;
                                apcLog.IsActive = true;
                                apcLog.Status = "APC";
                                apcLog.Reason = reason;

                                _entity.tb_ApprovalLog.Add(apcLog);
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    string remark = "Request Approval Cycle Completed by";
                                    //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                    if (data.tb_WFType.WF_ID == "P034")
                                    {
                                        #region  Insert a row into the Process Header for Travel Agency
                                        var ta = _entity.tb_TA_Business_International.Where(x => x.RequestId == reqId && x.IsActive == true).FirstOrDefault();
                                        if (ta != null)
                                        {
                                            int service_applied = Convert.ToInt32(ServiceStatus.Applied);
                                            #region RENT CAR BOOKING 
                                            if (ta.RentCar_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                //string message = "Request Send to HR Administration Officer ";
                                                //Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                            }
                                            #endregion

                                            #region HOTEL BOOKING 
                                            if (ta.Hotel_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  TICKET BOOKING 
                                            if (ta.Ticket_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-CENTRE" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-WEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-EAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  EXIT ENTRY VISA
                                            if (ta.Exit_Entry_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Exit Entry Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  FOREIGN VISA 
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Foreign Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  TRAVEL INSURANCE  
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Travel Insurance").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                        }
                                        #endregion
                                    }

                                    else if (data.tb_WFType.WF_ID == "P007")
                                    {
                                        #region  Insert a row into the Process Header for Travel Agency
                                        var ta = _entity.tb_TA_Vacation.Where(x => x.RequestId == reqId && x.IsActive == true).FirstOrDefault();
                                        if (ta != null)
                                        {
                                            int service_applied = Convert.ToInt32(ServiceStatus.Applied);
                                            #region RENT CAR BOOKING 
                                            if (ta.RentCar_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                //string message = "Request Send to HR Administration Officer ";
                                                //Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                            }
                                            #endregion

                                            #region HOTEL BOOKING 
                                            if (ta.Hotel_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  TICKET BOOKING 
                                            if (ta.Ticket_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-CENTRE" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-WEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-EAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  EXIT ENTRY VISA
                                            if (ta.Exit_Entry_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Exit Entry Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  FOREIGN VISA 
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Foreign Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  TRAVEL INSURANCE  
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Travel Insurance").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                        }
                                        #endregion
                                    }

                                    else if (data.tb_WFType.WF_ID == "P037")
                                    {
                                        #region  Insert a row into the Process Header for Travel Agency
                                        var ta = _entity.tb_TA_DependentsOnly.Where(x => x.RequestId == reqId && x.IsActive == true).FirstOrDefault();
                                        if (ta != null)
                                        {
                                            int service_applied = Convert.ToInt32(ServiceStatus.Applied);

                                            #region  TICKET BOOKING 
                                            if (ta.Ticket_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-CENTRE" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-WEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-EAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  EXIT ENTRY VISA
                                            if (ta.Exit_Entry_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Exit Entry Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  FOREIGN VISA 
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Foreign Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  TRAVEL INSURANCE  
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Travel Insurance").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion Have Next Approver

                    }
                    else
                    {
                        if (data.tb_WFType.tb_Closing_Type.Code == "CC")
                        {
                            #region CC 
                            data.Status_ID = "CLS";
                            data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);// APC have a approval number increament
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                string remark = "Request was closed by";
                                bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, reason);
                            }
                            #endregion CC
                        }
                        else if (data.tb_WFType.tb_Closing_Type.Code == "PC")
                        {
                            #region CC 
                            data.Status_ID = "CLS";
                            data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                string remark = "Request was closed by";
                                bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, reason);
                            }
                            #endregion CC
                        }
                        else if (data.tb_WFType.tb_Closing_Type.Code == "MC")
                        {
                            //10-07-2020
                            #region approval save 
                            msg = "Approved Successfully";
                            string mailMsg = "Forwarded by " + my_data.Emp_Name;
                            string remarks = "Request was approved by ";
                            bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "Processors", remarks, data, reason);  //Basheer on 19-03-2020

                            #endregion approval
                            //Basheer on 28-05-2020 changed
                            #region MC 
                            data.Status_ID = "APP";
                            data.Approver_ID = null;
                            data.OrgApprover_ID = null;
                            data.WFTemplate_ID = null;
                            data.RoleId = null;
                            //data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 1);  //Basheer on 26-06-2020     ;
                            data.Approval_No = 4.ToString();  //Basheer on 26-06-2020  bcz informtemplate it has set to 4   ;
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                //26-06-2020 Basheer
                                //string remark = "Request was closed by";
                                //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, reason);
                            }
                            #endregion

                            #region  Keep Log for APC 
                            var apcLog = _entity.tb_ApprovalLog.Create();
                            apcLog.RequestId = request_id;
                            apcLog.Remark = "Request Approval Cycle Completed by";
                            apcLog.EmployeeId = data.Employee_ID;
                            apcLog.Actor_Id = myId;
                            apcLog.RoleId = myRole;
                            //apcLog.SequenceNo = data.Approval_No == null ? 0 : Convert.ToInt32(data.Approval_No); //Basheer on 26-06-2020             
                            apcLog.TimeStamp = CurrentTime;
                            apcLog.IsActive = true;
                            apcLog.Status = "APC";
                            apcLog.Reason = reason;

                            _entity.tb_ApprovalLog.Add(apcLog);
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                string remark = "Request Approval Cycle Completed by";
                                //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                bool have_service = false;

                                if (data.tb_WFType.WF_ID == "P034")
                                {
                                    isdistributionflag = true;
                                    have_service = TA_ProcesSave("tb_TA_Business_International", reqId, data.Employee_ID, data.WF_ID, role.Role_ID);
                                }
                                else if (data.tb_WFType.WF_ID == "P007")
                                {
                                    isdistributionflag = true;
                                    have_service = TA_ProcesSave("tb_TA_Vacation", reqId, data.Employee_ID, data.WF_ID, role.Role_ID);
                                }
                                else if (data.tb_WFType.WF_ID == "P037")
                                {
                                    isdistributionflag = true;
                                    have_service = TA_ProcesSave("tb_TA_DependentsOnly", reqId, data.Employee_ID, data.WF_ID, role.Role_ID);
                                }
                                else if (data.tb_WFType.WF_ID == "P047")
                                {

                                }
                                if (have_service == false)// NO SERVICES 
                                {
                                    #region CC 
                                    data.Status_ID = "CLS";
                                    data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);// APC have a approval number increament
                                    data.TimeStamp = CurrentTime;
                                    status = _entity.SaveChanges() > 0;
                                    if (status)
                                    {
                                        string remark1 = "Request was closed by";
                                        bool sendMail1 = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark1, data, reason);
                                    }
                                    #endregion CC
                                }
                            }
                            #endregion

                            //Basheer on 28-05-2020 changed end here
                        }
                    }
                    #endregion Profile Oriented
                }
                else
                {
                    #region Not Profile Oriented
                    var wfTemplate = _entity.tb_WF_Template.Where(x => x.WF_ID == data.WF_ID && x.Sequence_NO > current_sequence && x.Action_Flag != identify_escalation).OrderBy(x => x.Sequence_NO).FirstOrDefault();
                    if (wfTemplate != null)
                    {
                        distributionId = wfTemplate.DistributionList_ID ?? 0; // 04-03-2020 ARCHANA K V SRISHTI 
                        #region Have Next Approver

                        var next_role = _entity.tb_Role.Where(x => x.Id == wfTemplate.Role_ID && x.IsActive == true).FirstOrDefault();
                        if (next_role != null)
                        {
                            RoleDetails roleDetails = new RoleDetails();

                            if (data.tb_WFType.WF_ID == "P016" && old_Approval_np == "1")      //P016-Internal Transfer(Preema)
                            {
                                roleDetails = Find_RoleDetails(next_role, data, my_data, wfTemplate.Status_ID);
                                data.Approver_ID = roleDetails.assigned_person_id;
                                data.OrgApprover_ID = roleDetails.assigned_person_id;
                            }

                            else if (data.tb_WFType.WF_ID == "P016" && old_Approval_np == "2")      //P016-Internal Transfer(Preema)
                            {
                                var transfer_data = _entity.tb_PP_Internal_Transfer.Where(x => x.RequestId == request_id && x.IsActive == true).FirstOrDefault();
                                roleDetails.deligated_personId = transfer_data.Receiving_Manager;
                                roleDetails.assigned_person_id = transfer_data.Receiving_Manager;
                                data.Approver_ID = roleDetails.assigned_person_id;
                                data.OrgApprover_ID = roleDetails.assigned_person_id;
                            }

                            else if (data.tb_WFType.WF_ID == "P016" && old_Approval_np == "3")      //P016-Internal Transfer(Preema)
                            {
                                roleDetails = Find_RoleDetails(next_role, data, my_data, wfTemplate.Status_ID);
                                data.Approver_ID = roleDetails.assigned_person_id;
                                data.OrgApprover_ID = roleDetails.assigned_person_id;
                            }      //P016-Internal Transfer(Preema)

                            else if (data.tb_WFType.WF_ID == "P015" && old_Approval_np == "2")   //P015 Terrin on 17-5-2020

                            {
                                var contoller_data = _entity.tb_PP_TrainingorRecruitmentPayment.Where(x => x.RequestId == request_id && x.IsActive == true).FirstOrDefault();
                                data.Approver_ID = contoller_data.blcontrollerid.ToString();
                                data.OrgApprover_ID = contoller_data.blcontrollerid.ToString();
                            }
                            else
                            {
                                roleDetails = Find_RoleDetails(next_role, data, initiator_details, wfTemplate.Status_ID); // 21-02-2020 ARCHANA SRISHTI 
                                data.Approver_ID = roleDetails.deligated_personId;
                                data.OrgApprover_ID = roleDetails.assigned_person_id; //21-02-2020 ARCHANA SRISHTI 
                            }


                            data.Approval_No = (Convert.ToInt32(data.Approval_No) + 1).ToString();
                            data.RoleId = wfTemplate.Role_ID;
                            data.WFTemplate_ID = wfTemplate.Id;
                            data.Status_ID = wfTemplate.Status_ID;
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                #region For History
                                msg = "Approved Successfully";
                                string mailMsg = "Forwarded by " + my_data.Emp_Name;
                                string remark = "Request was approved by ";


                                bool sendMail = Send_Approval_Mail_Login("INT", request_id, myPosition, role.Role_Desc, data.Employee_ID, myId, old_Approval_np, data.Approver_ID, roleDetails.role_name, remark, data, reason); //Basheer on 19-03-2020

                                //bool sendMail = Send_Approval_Mail_Login("INT", request_id, myPosition, role.Role_Desc, data.Employee_ID, myId, data.Approval_No, data.Approver_ID, roleDetails.role_name, remark, data, reason);
                                if (wfTemplate.DistributionList_ID != null)
                                {
                                    try
                                    {
                                        DistributionRequest(data, wfTemplate.DistributionList_ID ?? 0, data.Approval_No, "", myId, myRole);// Check the distribution and mail the data 
                                    }
                                    catch (Exception es)
                                    {
                                        msg = es.Message;
                                    }
                                }
                                #endregion For History
                            }
                            if (wfTemplate.Action_Flag != approvalStatus)//This is the last approvers
                            {
                                #region  Keep Log for APC 
                                var apcLog = _entity.tb_ApprovalLog.Create();
                                apcLog.RequestId = request_id;
                                apcLog.Remark = "Request Approval Cycle Completed by";
                                apcLog.EmployeeId = data.Employee_ID;
                                apcLog.Actor_Id = myId;
                                apcLog.RoleId = myRole;
                                apcLog.SequenceNo = data.Approval_No == null ? 0 : Convert.ToInt32(data.Approval_No);
                                apcLog.TimeStamp = CurrentTime;
                                apcLog.IsActive = true;
                                apcLog.Status = "APC";
                                apcLog.Reason = reason;

                                _entity.tb_ApprovalLog.Add(apcLog);
                                status = _entity.SaveChanges() > 0;
                                if (status)
                                {
                                    string remark = "Request Approval Cycle Completed by";
                                    //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                    if (data.tb_WFType.WF_ID == "P034")
                                    {
                                        #region  Insert a row into the Process Header for Travel Agency
                                        var ta = _entity.tb_TA_Business_International.Where(x => x.RequestId == reqId && x.IsActive == true).FirstOrDefault();
                                        if (ta != null)
                                        {
                                            int service_applied = Convert.ToInt32(ServiceStatus.Applied);
                                            #region RENT CAR BOOKING 
                                            if (ta.RentCar_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                //string message = "Request Send to HR Administration Officer ";
                                                //Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                            }
                                            #endregion

                                            #region HOTEL BOOKING 
                                            if (ta.Hotel_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  TICKET BOOKING 
                                            if (ta.Ticket_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-CENTRE" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-WEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-EAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  EXIT ENTRY VISA
                                            if (ta.Exit_Entry_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Exit Entry Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  FOREIGN VISA 
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Foreign Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  TRAVEL INSURANCE  
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Travel Insurance").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                        }
                                        #endregion
                                    }

                                    else if (data.tb_WFType.WF_ID == "P007")
                                    {
                                        #region  Insert a row into the Process Header for Travel Agency
                                        var ta = _entity.tb_TA_Vacation.Where(x => x.RequestId == reqId && x.IsActive == true).FirstOrDefault();
                                        if (ta != null)
                                        {
                                            int service_applied = Convert.ToInt32(ServiceStatus.Applied);
                                            #region RENT CAR BOOKING 
                                            if (ta.RentCar_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Rent Car Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                //string message = "Request Send to HR Administration Officer ";
                                                //Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                            }
                                            #endregion

                                            #region HOTEL BOOKING 
                                            if (ta.Hotel_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINCNTER" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINEAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "HRADMINWEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Hotel Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  TICKET BOOKING 
                                            if (ta.Ticket_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-CENTRE" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-WEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-EAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  EXIT ENTRY VISA
                                            if (ta.Exit_Entry_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Exit Entry Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  FOREIGN VISA 
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Foreign Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  TRAVEL INSURANCE  
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Travel Insurance").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion


                                        }
                                        #endregion
                                    }
                                    else if (data.tb_WFType.WF_ID == "P037")
                                    {
                                        #region  Insert a row into the Process Header for Travel Agency
                                        var ta = _entity.tb_TA_DependentsOnly.Where(x => x.RequestId == reqId && x.IsActive == true).FirstOrDefault();
                                        if (ta != null)
                                        {
                                            int service_applied = Convert.ToInt32(ServiceStatus.Applied);
                                            #region  TICKET BOOKING 
                                            if (ta.Ticket_Status == service_applied)
                                            {
                                                if (ta.Location_Id == Convert.ToInt32(Location.Centre))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-CENTRE" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.East))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-WEST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                                else if (ta.Location_Id == Convert.ToInt32(Location.West))
                                                {
                                                    var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "TRAVEL-EAST" && x.IsActive == true).FirstOrDefault();
                                                    var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Ticket Booking").FirstOrDefault();
                                                    var processHr = _entity.tb_ProcessHdr.Create();
                                                    processHr.RequestId = Convert.ToInt64(request_id);
                                                    processHr.RoleId = serviceRole.Role_ID;
                                                    processHr.Button_List = buttonList.ButtonList;
                                                    processHr.IsCompleted = false;
                                                    processHr.IsActive = true;
                                                    processHr.TimeStamp = CurrentTime;
                                                    _entity.tb_ProcessHdr.Add(processHr);
                                                    status = _entity.SaveChanges() > 0;
                                                }
                                            }
                                            #endregion

                                            #region  EXIT ENTRY VISA
                                            if (ta.Exit_Entry_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Exit Entry Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  FOREIGN VISA 
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Foreign Visa").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion

                                            #region  TRAVEL INSURANCE  
                                            if (ta.Foreign_Visa_Status == service_applied)
                                            {
                                                var serviceRole = _entity.tb_Role.Where(x => x.Role_ID.Trim() == "GOVRELOFFICER" && x.IsActive == true).FirstOrDefault();
                                                var buttonList = _entity.tb_ServiceButtonList.Where(x => x.RoleId == serviceRole.Role_ID && x.IsActive == true && x.Service == "Travel Insurance").FirstOrDefault();
                                                var processHr = _entity.tb_ProcessHdr.Create();
                                                processHr.RequestId = Convert.ToInt64(request_id);
                                                processHr.RoleId = serviceRole.Role_ID;
                                                processHr.Button_List = buttonList.ButtonList;
                                                processHr.IsCompleted = false;
                                                processHr.IsActive = true;
                                                processHr.TimeStamp = CurrentTime;
                                                _entity.tb_ProcessHdr.Add(processHr);
                                                status = _entity.SaveChanges() > 0;
                                            }
                                            #endregion


                                        }
                                        #endregion
                                    }
                                }
                                #endregion
                            }
                        }
                        #endregion Have Next Approver
                    }
                    else
                    {
                        if (data.tb_WFType.tb_Closing_Type.Code == "CC")
                        {
                            #region CC 
                            data.Status_ID = "CLS";
                            data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);// APC have a approval number increament
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                string remark = "Request was closed by";
                                bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, reason);
                            }
                            #endregion CC
                        }
                        else if (data.tb_WFType.tb_Closing_Type.Code == "PC")
                        {
                            #region CC 
                            data.Status_ID = "CLS";
                            data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                string remark = "Request was closed by";
                                bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, reason);
                            }
                            #endregion CC
                        }

                        else if (data.tb_WFType.tb_Closing_Type.Code == "MC")
                        {
                            //Preema 30-06-2020
                            #region MC 
                            data.Status_ID = "APP";
                            data.Approver_ID = null;
                            data.OrgApprover_ID = null;
                            data.WFTemplate_ID = null;
                            data.RoleId = null;
                            //data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 1);  //Basheer on 26-06-2020     ;
                            data.Approval_No = 4.ToString();  //Basheer on 26-06-2020  bcz informtemplate it has set to 4   ;
                            data.TimeStamp = CurrentTime;
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                //26-06-2020 Basheer
                                //string remark = "Request was closed by";
                                //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, reason);
                            }
                            #endregion

                            #region  Keep Log for APC 
                            var apcLog = _entity.tb_ApprovalLog.Create();
                            apcLog.RequestId = request_id;
                            apcLog.Remark = "Request Approval Cycle Completed by";
                            apcLog.EmployeeId = data.Employee_ID;
                            apcLog.Actor_Id = myId;
                            apcLog.RoleId = myRole;
                            //apcLog.SequenceNo = data.Approval_No == null ? 0 : Convert.ToInt32(data.Approval_No); //Basheer on 26-06-2020             
                            apcLog.TimeStamp = CurrentTime;
                            apcLog.IsActive = true;
                            apcLog.Status = "APC";
                            apcLog.Reason = reason;

                            _entity.tb_ApprovalLog.Add(apcLog);
                            status = _entity.SaveChanges() > 0;
                            if (status)
                            {
                                string remark = "Request Approval Cycle Completed by";
                                //bool sendMail = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                                bool have_service = false;

                                if (data.tb_WFType.WF_ID == "P037")
                                {
                                    isdistributionflag = true;
                                    have_service = TA_ProcesSave("tb_TA_DependentsOnly", reqId, data.Employee_ID, data.WF_ID, role.Role_ID);
                                }
                                if (have_service == false)// NO SERVICES 
                                {
                                    #region CC 
                                    data.Status_ID = "CLS";
                                    data.Approval_No = Convert.ToString(Convert.ToInt32(data.Approval_No) + 2);// APC have a approval number increament
                                    data.TimeStamp = CurrentTime;
                                    status = _entity.SaveChanges() > 0;
                                    if (status)
                                    {
                                        string remark1 = "Request was closed by";
                                        bool sendMail1 = Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark1, data, reason);
                                    }
                                    #endregion CC
                                }
                            }
                            #endregion

                            //Preema on 30-06-2020 changed end here
                        }

                    }
                    #endregion Not Profile Oriented
                }
                #endregion Find the next approver process
            }
            if (status)
                msg = "Successful";
            return new Tuple<bool, string, long, bool>(status, msg, distributionId, isdistributionflag);// basheer on 28-05-2020
        }

        public RoleDetails Find_RoleDetails(tb_Role next_role, tb_Request_Hdr data, tb_WF_Employee employee, string status)// 21-02-2020 ARCHANA SRISHTI 
        {
            RoleDetails role = new RoleDetails();
            role.role_id = next_role.Role_ID;
            role.role_name = next_role.Role_Desc;
            if (next_role.Organization_Flag == false)
            {
                #region Not Organization 
                if (next_role.Assigned_ID == "NULL" || next_role.Assigned_ID == null) //Basheer on 12-05-2020 changed != to == because of client issue and interchanged contents in else
                {
                    if (next_role.GroupRole == true)
                    {
                        #region Having Multiple 
                        if (next_role.Role_ID == "SROWRIS") // Service Owner 
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == data.tb_WFType.WF_ID).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Table_Name;
                            }
                        }
                        else if (next_role.Role_ID == "SYENGRIS") // Service Engineer
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == data.tb_WFType.WF_ID).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Table_Name;
                            }
                        }
                        else if (next_role.Role_ID == "ILMGR") // Immediate Line Manager
                        {
                            role.assigned_person_id = employee.Line_Manager;
                        }
                        else if (next_role.Role_ID == "HRTO") // HR Travel Officer 
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == employee.tb_Company.Company_Code).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Table_Name;
                            }
                        }
                        else
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Table_Name;
                            }
                        }
                        #endregion Having Multiple 
                    }
                    else
                    {
                        if (data.tb_WFType.WF_ID == "P017")      //P016-Internal Transfer(Preema)
                        {
                            var contract_modification = _entity.tb_PP_Contract_Modification.Where(x => x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (contract_modification != null)
                            {
                                role.assigned_person_id = contract_modification.Releasing_Manager;
                            }
                        }
                        #region Having Single 
                        else if (next_role.Role_ID == "SROWRIS")// Service Owner 
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == data.tb_WFType.WF_ID).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Description;
                            }
                        }
                        else if (next_role.Role_ID == "SYENGRIS")// Service Engineer
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == data.tb_WFType.WF_ID).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Description;
                            }
                        }
                        else if (next_role.Role_ID == "ILMGR") // Immediate Line Manager
                        {
                            role.assigned_person_id = employee.Line_Manager;
                        }
                        else if (next_role.Role_ID == "HRTO") // HR Travel Officer 
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == employee.tb_Company.Company_Code).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Description;
                            }
                        }

                        else if (next_role.Role_ID == "ILMGRSL") // Immediate Line Manager second level Basheer on 01-07-2020  
                        {
                            var snd_approver = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Approver_ID && x.IsActive == true).FirstOrDefault();
                            role.assigned_person_id = snd_approver.Line_Manager;
                        }
                        else if (next_role.Role_ID == "FMGR") // Finance Manager :Added by chitra on 26.06.2020 for SAS01
                        {
                            var Bankguarantee = _entity.tb_SAS_Bank_Guarantee_Application.Where(x => x.RequestId == data.Request_ID && x.IsActive == true).FirstOrDefault();

                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true && x.Code == Bankguarantee.tb_Company.Company_Code).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Description;
                            }
                        }
                        else
                        {
                            var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == next_role.Role_ID && x.IsActive == true).FirstOrDefault();
                            if (universal != null)
                            {
                                role.assigned_person_id = universal.Description;
                            }
                        }
                        #endregion Having Single
                    }
                }
                else
                {

                    #region Normal
                    role.assigned_person_id = next_role.Assigned_ID;
                    #endregion Normal
                }
                #endregion Not Organization 
            }
            else
            {
                #region  Organization 
                if (next_role.org_type == null || next_role.org_type == string.Empty)
                {
                    role.assigned_person_id = employee.Line_Manager;
                }
                else if (next_role.org_type.Trim() == "DT")// Check the department table 
                {
                    #region Department
                    var department = _entity.tb_Department.Where(x => x.IsActive == true && x.Department_Id == employee.Department_Id).FirstOrDefault();
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
                    //var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.tb_Department.tb_ProductGroup.BusinessLine_Id).FirstOrDefault();
                    var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.Businessline_Id).FirstOrDefault(); //24-02-2020 ARCHANA SRISHTI 
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
                    //var business = _entity.tb_Business.Where(x => x.IsActive == true && x.Bus_Id == employee.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Id).FirstOrDefault();
                    var business = _entity.tb_Business.Where(x => x.IsActive == true && x.Bus_Id == employee.Business_Id).FirstOrDefault();//24-02-2020 ARCHANA SRISHTI 
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
                    //var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.tb_Department.PG_Id).FirstOrDefault();
                    var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.Productgroup_Id).FirstOrDefault(); //24-02-2020 ARCHANA SRISHTI 
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

            //p016

            var next_emp = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == role.assigned_person_id && x.IsActive == true).FirstOrDefault();
            #region
            if (next_emp != null)
            {
                role.assigned_person_id = next_emp.LocalEmplyee_ID;
                if (next_emp.DelegationFlag == true)
                    role.deligated_personId = next_emp.Delegate_Emp_Code == null ? next_emp.LocalEmplyee_ID : next_emp.Delegate_Emp_Code;
                else
                    role.deligated_personId = next_emp.LocalEmplyee_ID;

                if (status != "APP" && status != "PIM" && status != "QIM") //21-02-2020 ARCHANA SRISHTI DON'T WANTS TO CHECK THET THE PROCESSOR AND THE CREATOR ARE SAME  
                {// 07-03-2020 ARCHANA K V ADDED PIM AND QIM INTO THIS, BECAUSE THE QIM AND THE PIM ARE PROCESS
                    if (role.deligated_personId == data.Employee_ID && (data.Status_ID == "INT" || data.Status_ID == "ESC")) // ME & APPROVER ARE SAME 
                    { // FIND IMMEDIATE BOSS
                        var line_manager = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employee.Line_Manager && x.IsActive == true).FirstOrDefault();
                        if (line_manager != null)
                        {
                            if (line_manager.DelegationFlag == true)
                                role.deligated_personId = line_manager.Delegate_Emp_Code == null ? line_manager.LocalEmplyee_ID : line_manager.Delegate_Emp_Code;
                            else
                                role.deligated_personId = line_manager.LocalEmplyee_ID;
                        }
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

        /// <summary>
        /// <head name="This Method is used to send the mail for distribution"></head>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="role_Id"></param>
        /// <param name="application_ID"></param>
        /// <param name="country_code"></param>
        /// <param name="my_id"></param>
        /// <param name="my_role"></param>
        /// <param name="mailContent"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        private string Find_Role_UserForDistribution(tb_Request_Hdr request, long role_Id, long countryId, string my_id, string my_role, string mailContent, tb_WF_Employee employee)//NOT USE 25-02-2020 ARCHANA SRISHTI 
        {
            var role = _entity.tb_Role.Where(x => x.Id == role_Id && x.Country_ID == countryId && x.IsActive == true).FirstOrDefault();
            if (role != null)
            {
                //--------------------------------------------
                if (role.Organization_Flag == true && role.org_type != null)// Checking that we wants to find the user by using Organization type tavle or can normal
                {
                    #region 
                    if (role.org_type.Trim() == "DT")// Check the department table 
                    {
                        #region Department
                        var department = _entity.tb_Department.Where(x => x.IsActive == true && x.Department_Id == employee.Department_Id).FirstOrDefault();
                        if (department != null)
                        {
                            if (role.role_type == "Head" && department.Dept_Manager != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, department.Dept_Manager, role.Role_Desc, mailContent, request, employee);
                                return department.Dept_Manager;
                            }
                            else if (role.role_type == "Manager1" && department.Dept_Controller != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, department.Dept_Controller, role.Role_Desc, mailContent, request, employee);
                                return department.Dept_Controller;
                            }
                            else if (role.role_type == "Manager2" && department.Dep_Office_Admin != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, department.Dep_Office_Admin, role.Role_Desc, mailContent, request, employee);
                                return department.Dep_Office_Admin;
                            }
                        }
                        #endregion Department
                    }
                    else if (role.org_type.Trim() == "BL")// Check the business line table 
                    {
                        #region Business Line
                        //var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.tb_Department.tb_ProductGroup.BusinessLine_Id).FirstOrDefault();
                        var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.Businessline_Id).FirstOrDefault();// 24-02-2020 ARCHANA SROSHTI 
                        if (business != null)
                        {
                            if (role.role_type == "MN" && business.BL_Manager != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, business.BL_Manager, role.Role_Desc, mailContent, request, employee);
                                return business.BL_Manager;
                            }
                            else if (role.role_type == "CR" && business.BL_Controller != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, business.BL_Controller, role.Role_Desc, mailContent, request, employee);
                                return business.BL_Controller;
                            }
                            else if (role.role_type == "OA" && business.BL_Office_Admin != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, business.BL_Office_Admin, role.Role_Desc, mailContent, request, employee);
                                return business.BL_Office_Admin;
                            }

                        }
                        #endregion Business Line
                    }
                    else if (role.org_type.Trim() == "B")// Check the business 
                    {
                        #region Business
                        //var business = _entity.tb_Business.Where(x => x.Country_Id == role.Country_ID && x.IsActive == true && x.Bus_Id == employee.tb_Department.tb_ProductGroup.tb_BusinessLine.Business_Id).FirstOrDefault();
                        var business = _entity.tb_Business.Where(x => x.Country_Id == role.Country_ID && x.IsActive == true && x.Bus_Id == employee.Business_Id).FirstOrDefault();//24-02-2020 ARCHANA SRISHTI 
                        if (business != null)
                        {
                            if (role.role_type == "Manager1" && business.Bus_Manager != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, business.Bus_Manager, role.Role_Desc, mailContent, request, employee);
                                return business.Bus_Manager;
                            }
                            else if (role.role_type == "Manager2" && business.Bus_Controller != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, business.Bus_Controller, role.Role_Desc, mailContent, request, employee);
                                return business.Bus_Controller;
                            }
                            else if (role.role_type == "Manager3" && business.Bus_Office_Admin != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, business.Bus_Office_Admin, role.Role_Desc, mailContent, request, employee);
                                return business.Bus_Office_Admin;
                            }
                        }
                        #endregion Business
                    }
                    else if (role.org_type == "PG")// Check the Product group table 
                    {
                        #region Product Group
                        //var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.tb_Department.PG_Id).FirstOrDefault();
                        var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.Productgroup_Id).FirstOrDefault();// 24-02-2020 ARCAHANA SRISHTI 
                        if (product != null)
                        {
                            if (role.role_type == "Manager1" && product.PG_Manager != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, product.PG_Manager, role.Role_Desc, mailContent, request, employee);
                                return product.PG_Manager;
                            }
                            else if (role.role_type == "Manager2" && product.PG_Controller != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, product.PG_Controller, role.Role_Desc, mailContent, request, employee);
                                return product.PG_Controller;
                            }
                            else if (role.role_type == "Manager3" && product.PG_Office_Admin != null)
                            {
                                Send_Distribution_mail(request.Request_ID, my_id, my_role, product.PG_Office_Admin, role.Role_Desc, mailContent, request, employee);
                                return product.PG_Office_Admin;
                            }
                        }
                        #endregion Product Group 
                    }
                    #endregion
                }
                else
                {
                    #region 
                    if (role.Assigned_ID != null)
                    {
                        #region Direct 
                        Send_Distribution_mail(request.Request_ID, my_id, my_role, role.Assigned_ID, role.Role_Desc, mailContent, request, employee);
                        return role.Assigned_ID;
                        #endregion Direct 
                    }
                    else
                    {
                        var checkLookupTable = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == role.Role_ID && x.IsActive == true).ToList();
                        foreach (var item in checkLookupTable)
                        {
                            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == request.Request_ID).FirstOrDefault();
                            if (item.Table_Name == "HRAHEAD" && request.WF_ID == 28)   //A007-HR Administration Section Head (Region Wise)
                            {
                                var employee_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
                                var region = _entity.tb_Location.Where(x => x.Location_Id == employee_data.Location_Id && x.IsActive == true).FirstOrDefault();
                                var role_region = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == "HRAHEAD" && x.Code == region.Region && x.IsActive == true).FirstOrDefault();
                                // data.Approver_ID = role_region.Description;
                                if (item.Code == region.Region)
                                    Send_Distribution_mail(request.Request_ID, my_id, my_role, role_region.Description, item.Code, mailContent, request, employee);
                            }
                            else if (item.Table_Name == "HRADSPTOFFS" && request.WF_ID == 28)   //A007-HR Administration Support Officer (Region Wise)
                            {
                                var employee_data = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Employee_ID && x.IsActive == true).FirstOrDefault();
                                var region = _entity.tb_Location.Where(x => x.Location_Id == employee_data.Location_Id && x.IsActive == true).FirstOrDefault();
                                var role_region = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == "HRADSPTOFFS" && x.Code == region.Region && x.IsActive == true).FirstOrDefault();
                                // data.Approver_ID = role_region.Description;
                                if (item.Code == region.Region)
                                    Send_Distribution_mail(request.Request_ID, my_id, my_role, role_region.Description, item.Code, mailContent, request, employee);
                            }
                            else
                            {

                                Send_Distribution_mail(request.Request_ID, my_id, my_role, item.Description, item.Code, mailContent, request, employee);
                            }
                        }
                        return "";
                    }
                    #endregion
                }
                //----------------------------------------
            }
            return "";
        }
        public string Find_Role_UserOnlyForDistribution(tb_Request_Hdr request, long role_Id, long countryId, string my_id, string my_role, string mailContent, tb_WF_Employee employee)//25-02-2020 ARCHANA SRISHTI 
        {
            var role = _entity.tb_Role.Where(x => x.Id == role_Id && x.Country_ID == countryId && x.IsActive == true).FirstOrDefault();
            if (role != null)
            {
                //--------------------------------------------
                if (role.Organization_Flag == true && role.org_type != null)
                {
                    #region 
                    if (role.org_type.Trim() == "DT")// Check the department table 
                    {
                        #region Department
                        var department = _entity.tb_Department.Where(x => x.IsActive == true && x.Department_Id == employee.Department_Id).FirstOrDefault();
                        if (department != null)
                        {
                            if (role.role_type == "MN" && department.Dept_Manager != null)  //28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return department.Dept_Manager;
                            }
                            else if (role.role_type == "CR" && department.Dept_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return department.Dept_Controller;
                            }
                            else if (role.role_type == "OA" && department.Dep_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return department.Dep_Office_Admin;
                            }
                        }
                        #endregion Department
                    }
                    else if (role.org_type.Trim() == "BL")// Check the business line table 
                    {
                        #region Business Line
                        var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.Businessline_Id).FirstOrDefault();// 24-02-2020 ARCHANA SROSHTI 
                        if (business != null)
                        {
                            if (role.role_type == "MN" && business.BL_Manager != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return business.BL_Manager;
                            }
                            else if (role.role_type == "CR" && business.BL_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return business.BL_Controller;
                            }
                            else if (role.role_type == "OA" && business.BL_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return business.BL_Office_Admin;
                            }

                        }
                        #endregion Business Line
                    }
                    else if (role.org_type.Trim() == "B")// Check the business 
                    {
                        #region Business
                        var business = _entity.tb_Business.Where(x => x.Country_Id == role.Country_ID && x.IsActive == true && x.Bus_Id == employee.Business_Id).FirstOrDefault();//24-02-2020 ARCHANA SRISHTI 
                        if (business != null)
                        {
                            if (role.role_type == "MN" && business.Bus_Manager != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return business.Bus_Manager;
                            }
                            else if (role.role_type == "CR" && business.Bus_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return business.Bus_Controller;
                            }
                            else if (role.role_type == "OA" && business.Bus_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return business.Bus_Office_Admin;
                            }
                        }
                        #endregion Business
                    }
                    else if (role.org_type == "PG")// Check the Product group table 
                    {
                        #region Product Group
                        //var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.tb_Department.PG_Id).FirstOrDefault();
                        var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.Productgroup_Id).FirstOrDefault();// 24-02-2020 ARCAHANA SRISHTI 
                        if (product != null)
                        {
                            if (role.role_type == "MN" && product.PG_Manager != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return product.PG_Manager;
                            }
                            else if (role.role_type == "CR" && product.PG_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return product.PG_Controller;
                            }
                            else if (role.role_type == "OA" && product.PG_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                            {
                                return product.PG_Office_Admin;
                            }
                        }
                        #endregion Product Group 
                    }
                    #endregion
                }
                else
                {
                    #region 
                    if (role.Assigned_ID != null)
                    {
                        #region Direct 
                        return role.Assigned_ID;
                        #endregion Direct 
                    }
                    else
                    {
                        var checkLookupTable = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == role.Role_ID && x.IsActive == true).ToList();
                        string emp = "";
                        foreach (var item in checkLookupTable)
                        {
                            if (emp == string.Empty)
                            {
                                emp = item.Description;
                            }
                            else
                            {
                                emp = emp + "~" + item.Description;
                            }
                        }
                        return "";
                    }
                    #endregion
                }
                //----------------------------------------
            }
            return "";
        }

        //Basheer on 28-05-2020
        public bool TA_ProcesSave(string tablename, long reqId, string employeeid, long wfid, string myrole)
        {
            bool have_service = false;
            bool status = false;
            int service_applied = Convert.ToInt32(ServiceStatus.Applied);
            var employees = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employeeid && x.IsActive == true).FirstOrDefault(); //Basheer on 07-07-2020
            long? locationid = employees.Location_Id;//Basheer on 07-07-2020
            #region  Insert a row into the Process Header    
            var wf_type = _entity.tb_WFType.Where(x => x.Id == wfid && x.IsActive == true).FirstOrDefault();//Preema(25-06-2020)

            if (wf_type.WF_ID == "P034")
            {
                var ta = _entity.Database.SqlQuery<tb_TA_Business_International>(string.Format("SELECT * FROM " + tablename + " WHERE [RequestId] = {0} AND [IsActive] = {1}", reqId, 1)).FirstOrDefault();
                if (ta != null)
                {
                    #region RENT CAR BOOKING 
                    if (ta.RentCar_Status == service_applied)
                    {
                        string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                        if (region == "EAST")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-EAST" && x.Service == "RENTCAR").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        else if (region == "WEST")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-WEST" && x.Service == "RENTCAR").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        else
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-CENTRAL" && x.Service == "RENTCAR").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }

                        //string message = "Request Send to HR Administration Officer ";
                        //Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                    }
                    #endregion
                    #region HOTEL BOOKING 
                    if (ta.Hotel_Status == service_applied)
                    {
                        string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                        if (region == "EAST")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-EAST" && x.Service == "HOTEL").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        else if (region == "WEST")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-WEST" && x.Service == "HOTEL").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        else
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-CENTRAL" && x.Service == "HOTEL").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }

                    }
                    #endregion
                    #region  TICKET BOOKING 
                    //if (ta.Ticket_Status == service_applied) //Basheer on 26-06-2020
                    if (ta.Air_Ticket_Status == service_applied)
                    {
                        string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                        if (region == "EAST")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-EAST" && x.Service == "TICKET").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        else if (region == "WEST")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-WEST" && x.Service == "TICKET").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        else
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-CENTRAL" && x.Service == "TICKET").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                    }
                    #endregion
                    #region  EXIT ENTRY VISA
                    if (ta.Exit_Entry_Visa_Status == service_applied)
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "EXITENTRY").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    #endregion
                    #region  FOREIGN VISA 
                    if (ta.Foreign_Visa_Status == service_applied)
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "FOREGIN").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    #endregion
                    #region  TRAVEL INSURANCE  
                    if (ta.Travel_Insurance_Status == service_applied)
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "TRAVELINSURANCE").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    #endregion
                    #region  CASH ADVANCE  
                    if (ta.Cash_Advance_Status == service_applied)
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "SHAREDACCOUNT" && x.Service == "CASHADVANCE").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    #endregion

                    #region  For JUBILE 
                    string locationarea = _entity.tb_Location.Where(x => x.Location_Id == locationid && x.IsActive == true).FirstOrDefault().Location_Code;
                    if (locationarea == "JBL")
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = "BRANCHMGR";
                        insert_process_data.Button_List = null;
                        insert_process_data.IsCompleted = true;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = true;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;

                        var insert_process_data1 = _entity.tb_ProcessHdr.Create();
                        insert_process_data1.RequestId = reqId;
                        insert_process_data1.RoleId = "BRANCHCDR";
                        insert_process_data1.Button_List = null;
                        insert_process_data1.IsCompleted = true;
                        insert_process_data1.IsActive = true;
                        insert_process_data1.TimeStamp = CurrentTime;
                        insert_process_data1.distribution_flag = true;
                        insert_process_data1.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data1);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    #endregion
                    #region  For Vendor 
                    var vendor = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employeeid && x.EmployeeType_ID == "3" && x.IsActive == true).FirstOrDefault();//Basheer on 07-07-2020
                    if (vendor != null)
                    {
                        long? vendorid = vendor.Vendor_Id;
                        if (vendorid != null || vendorid != 0)
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "VENDOR-" + vendorid;
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                    }
                    #endregion
                    #region  For HR TRAVEL OFFICER 
                    if (myrole != "")
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = "HRTO"; //26-06-2020 New Basheer
                        insert_process_data.Button_List = null;
                        insert_process_data.IsCompleted = true;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = true;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    #endregion

                }
            }

            else if (wf_type.WF_ID == "P007")
            {
                var ta = _entity.Database.SqlQuery<tb_TA_Vacation>(string.Format("SELECT * FROM " + tablename + " WHERE [RequestId] = {0} AND [IsActive] = {1}", reqId, 1)).FirstOrDefault();
                if (ta != null)
                {
                    if (employees.EmployeeType_ID != "3") //Basheer on 07-07-2020
                    {
                        #region RENT CAR BOOKING 
                        if (ta.RentCar_Status == service_applied)
                        {
                            string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                            if (region == "EAST")
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-EAST" && x.Service == "RENTCAR").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                            else if (region == "WEST")
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-WEST" && x.Service == "RENTCAR").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                            else
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-CENTRAL" && x.Service == "RENTCAR").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }

                            //string message = "Request Send to HR Administration Officer ";
                            //Send_Approval_Mail_Login(old_status, request_id, myPosition, myRole, data.Employee_ID, myId, old_Approval_np, "", "", remark, data, "");
                        }
                        #endregion
                        #region HOTEL BOOKING 
                        if (ta.Hotel_Status == service_applied)
                        {
                            string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                            if (region == "EAST")
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-EAST" && x.Service == "HOTEL").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                            else if (region == "WEST")
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-WEST" && x.Service == "HOTEL").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                            else
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "HRADMIN-CENTRAL" && x.Service == "HOTEL").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }

                        }
                        #endregion
                        #region  TICKET BOOKING 
                        //if (ta.Ticket_Status == service_applied) //Basheer on 26-06-2020
                        if (ta.Air_Ticket_Status == service_applied)
                        {
                            string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                            if (region == "EAST")
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-EAST" && x.Service == "TICKET").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                            else if (region == "WEST")
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-WEST" && x.Service == "TICKET").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                            else
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-CENTRAL" && x.Service == "TICKET").FirstOrDefault();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = info.RoleId;
                                insert_process_data.Button_List = info.ButtonList;
                                insert_process_data.IsCompleted = false;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = false;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                        }
                        #endregion
                        #region  EXIT ENTRY VISA
                        if (ta.Exit_Entry_Visa_Status == service_applied)
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "EXITENTRY").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion
                        #region  FOREIGN VISA 
                        if (ta.Foreign_Visa_Status == service_applied)
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "FOREGIN").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion
                        #region  TRAVEL INSURANCE  
                        if (ta.Travel_Insurance_Status == service_applied)
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "TRAVELINSURANCE").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion
                        #region  CASH ADVANCE  
                        if (ta.Cash_Advance_Status == service_applied)
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "SHAREDACCOUNT" && x.Service == "CASHADVANCE").FirstOrDefault();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = info.RoleId;
                            insert_process_data.Button_List = info.ButtonList;
                            insert_process_data.IsCompleted = false;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = false;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion

                        #region  For JUBILE 
                        string locationarea = _entity.tb_Location.Where(x => x.Location_Id == locationid && x.IsActive == true).FirstOrDefault().Location_Code;
                        if (locationarea == "JBL")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "BRANCHMGR";
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;

                            var insert_process_data1 = _entity.tb_ProcessHdr.Create();
                            insert_process_data1.RequestId = reqId;
                            insert_process_data1.RoleId = "BRANCHCDR";
                            insert_process_data1.Button_List = null;
                            insert_process_data1.IsCompleted = true;
                            insert_process_data1.IsActive = true;
                            insert_process_data1.TimeStamp = CurrentTime;
                            insert_process_data1.distribution_flag = true;
                            insert_process_data1.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data1);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion

                        #region  For Vendor 
                        var vendor = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employeeid && x.EmployeeType_ID == "3" && x.IsActive == true).FirstOrDefault();//Basheer on 07-07-2020
                        if (vendor != null)
                        {
                            long? vendorid = vendor.Vendor_Id;
                            if (vendorid != null || vendorid != 0)
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = "VENDOR-" + vendorid;
                                insert_process_data.Button_List = null;
                                insert_process_data.IsCompleted = true;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = true;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = true;
                            }
                        }
                        #endregion

                        #region  For HR TRAVEL OFFICER 
                        if (myrole != "")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "HRTO"; //26-06-2020 New Basheer
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion

                        #region  For HR Document Controller
                        //08-07-2020 Preema
                        if (myrole != "")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "HRDCTR";
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = true;
                        }
                        #endregion
                    }
                    else
                    {
                        #region  For JUBILE 
                        string locationarea = _entity.tb_Location.Where(x => x.Location_Id == locationid && x.IsActive == true).FirstOrDefault().Location_Code;
                        if (locationarea == "JBL")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "BRANCHMGR";
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;

                            var insert_process_data1 = _entity.tb_ProcessHdr.Create();
                            insert_process_data1.RequestId = reqId;
                            insert_process_data1.RoleId = "BRANCHCDR";
                            insert_process_data1.Button_List = null;
                            insert_process_data1.IsCompleted = true;
                            insert_process_data1.IsActive = true;
                            insert_process_data1.TimeStamp = CurrentTime;
                            insert_process_data1.distribution_flag = true;
                            insert_process_data1.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data1);
                            status = _entity.SaveChanges() > 0;
                            have_service = false;
                        }
                        #endregion

                        #region  For Vendor 
                        var vendor = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employeeid && x.EmployeeType_ID == "3" && x.IsActive == true).FirstOrDefault();//Basheer on 07-07-2020
                        if (vendor != null)
                        {
                            long? vendorid = vendor.Vendor_Id;
                            if (vendorid != null || vendorid != 0)
                            {
                                var insert_process_data = _entity.tb_ProcessHdr.Create();
                                insert_process_data.RequestId = reqId;
                                insert_process_data.RoleId = "VENDOR-" + vendorid;
                                insert_process_data.Button_List = null;
                                insert_process_data.IsCompleted = true;
                                insert_process_data.IsActive = true;
                                insert_process_data.TimeStamp = CurrentTime;
                                insert_process_data.distribution_flag = true;
                                insert_process_data.WF_id = wfid;
                                _entity.tb_ProcessHdr.Add(insert_process_data);
                                status = _entity.SaveChanges() > 0;
                                have_service = false;
                            }
                        }
                        #endregion

                        #region  For HR TRAVEL OFFICER 
                        if (myrole != "")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "HRTO"; //26-06-2020 New Basheer
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = false;
                        }
                        #endregion

                        #region  For HR Document Controller
                        //08-07-2020 Preema
                        if (myrole != "")
                        {
                            var insert_process_data = _entity.tb_ProcessHdr.Create();
                            insert_process_data.RequestId = reqId;
                            insert_process_data.RoleId = "HRDCTR";
                            insert_process_data.Button_List = null;
                            insert_process_data.IsCompleted = true;
                            insert_process_data.IsActive = true;
                            insert_process_data.TimeStamp = CurrentTime;
                            insert_process_data.distribution_flag = true;
                            insert_process_data.WF_id = wfid;
                            _entity.tb_ProcessHdr.Add(insert_process_data);
                            status = _entity.SaveChanges() > 0;
                            have_service = false;
                        }
                        #endregion
                    }

                }
            }

            else if (wf_type.WF_ID == "P037")
            {
                var ta = _entity.Database.SqlQuery<tb_TA_DependentsOnly>(string.Format("SELECT * FROM " + tablename + " WHERE [RequestId] = {0} AND [IsActive] = {1}", reqId, 1)).FirstOrDefault();

                #region  TICKET BOOKING 
                //if (ta.Ticket_Status == service_applied) //Basheer on 26-06-2020
                if (ta.Air_Ticket_Status == service_applied)
                {
                    string region = _entity.tb_Location.Where(x => x.IsActive && x.Location_Id == locationid).FirstOrDefault().Region;
                    if (region == "EAST")
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-EAST" && x.Service == "TICKET").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    else if (region == "WEST")
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-WEST" && x.Service == "TICKET").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                    else
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "TRAVEL-CENTRAL" && x.Service == "TICKET").FirstOrDefault();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = info.RoleId;
                        insert_process_data.Button_List = info.ButtonList;
                        insert_process_data.IsCompleted = false;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = false;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                }
                #endregion
                #region  EXIT ENTRY VISA
                if (ta.Exit_Entry_Visa_Status == service_applied)
                {
                    var insert_process_data = _entity.tb_ProcessHdr.Create();
                    var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "EXITENTRY").FirstOrDefault();
                    insert_process_data.RequestId = reqId;
                    insert_process_data.RoleId = info.RoleId;
                    insert_process_data.Button_List = info.ButtonList;
                    insert_process_data.IsCompleted = false;
                    insert_process_data.IsActive = true;
                    insert_process_data.TimeStamp = CurrentTime;
                    insert_process_data.distribution_flag = false;
                    insert_process_data.WF_id = wfid;
                    _entity.tb_ProcessHdr.Add(insert_process_data);
                    status = _entity.SaveChanges() > 0;
                    have_service = true;
                }
                #endregion
                #region  FOREIGN VISA 
                if (ta.Foreign_Visa_Status == service_applied)
                {
                    var insert_process_data = _entity.tb_ProcessHdr.Create();
                    var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "FOREGIN").FirstOrDefault();
                    insert_process_data.RequestId = reqId;
                    insert_process_data.RoleId = info.RoleId;
                    insert_process_data.Button_List = info.ButtonList;
                    insert_process_data.IsCompleted = false;
                    insert_process_data.IsActive = true;
                    insert_process_data.TimeStamp = CurrentTime;
                    insert_process_data.distribution_flag = false;
                    insert_process_data.WF_id = wfid;
                    _entity.tb_ProcessHdr.Add(insert_process_data);
                    status = _entity.SaveChanges() > 0;
                    have_service = true;
                }
                #endregion
                #region  TRAVEL INSURANCE  
                if (ta.Travel_Insurance_Status == service_applied)
                {
                    var insert_process_data = _entity.tb_ProcessHdr.Create();
                    var info = _entity.tb_ServiceButtonList.Where(x => x.RoleId == "GOVRELOFFS" && x.Service == "TRAVELINSURANCE").FirstOrDefault();
                    insert_process_data.RequestId = reqId;
                    insert_process_data.RoleId = info.RoleId;
                    insert_process_data.Button_List = info.ButtonList;
                    insert_process_data.IsCompleted = false;
                    insert_process_data.IsActive = true;
                    insert_process_data.TimeStamp = CurrentTime;
                    insert_process_data.distribution_flag = false;
                    insert_process_data.WF_id = wfid;
                    _entity.tb_ProcessHdr.Add(insert_process_data);
                    status = _entity.SaveChanges() > 0;
                    have_service = true;
                }
                #endregion              

                #region  For JUBILE 
                string locationarea = _entity.tb_Location.Where(x => x.Location_Id == locationid && x.IsActive == true).FirstOrDefault().Location_Code;
                if (locationarea == "JBL")
                {
                    var insert_process_data = _entity.tb_ProcessHdr.Create();
                    insert_process_data.RequestId = reqId;
                    insert_process_data.RoleId = "BRANCHMGR";
                    insert_process_data.Button_List = null;
                    insert_process_data.IsCompleted = true;
                    insert_process_data.IsActive = true;
                    insert_process_data.TimeStamp = CurrentTime;
                    insert_process_data.distribution_flag = true;
                    insert_process_data.WF_id = wfid;
                    _entity.tb_ProcessHdr.Add(insert_process_data);
                    status = _entity.SaveChanges() > 0;

                    var insert_process_data1 = _entity.tb_ProcessHdr.Create();
                    insert_process_data1.RequestId = reqId;
                    insert_process_data1.RoleId = "BRANCHCDR";
                    insert_process_data1.Button_List = null;
                    insert_process_data1.IsCompleted = true;
                    insert_process_data1.IsActive = true;
                    insert_process_data1.TimeStamp = CurrentTime;
                    insert_process_data1.distribution_flag = true;
                    insert_process_data1.WF_id = wfid;
                    _entity.tb_ProcessHdr.Add(insert_process_data1);
                    status = _entity.SaveChanges() > 0;
                    have_service = true;
                }
                #endregion
                #region  For Vendor 
                var vendor = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == employeeid && x.EmployeeType_ID == "3" && x.IsActive == true).FirstOrDefault();//Basheer on 07-07-2020
                if (vendor != null)
                {
                    long? vendorid = vendor.Vendor_Id;
                    if (vendorid != null || vendorid != 0)
                    {
                        var insert_process_data = _entity.tb_ProcessHdr.Create();
                        insert_process_data.RequestId = reqId;
                        insert_process_data.RoleId = "VENDOR-" + vendorid;
                        insert_process_data.Button_List = null;
                        insert_process_data.IsCompleted = true;
                        insert_process_data.IsActive = true;
                        insert_process_data.TimeStamp = CurrentTime;
                        insert_process_data.distribution_flag = true;
                        insert_process_data.WF_id = wfid;
                        _entity.tb_ProcessHdr.Add(insert_process_data);
                        status = _entity.SaveChanges() > 0;
                        have_service = true;
                    }
                }
                #endregion
                #region  For HR TRAVEL OFFICER 
                if (myrole != "")
                {
                    var insert_process_data = _entity.tb_ProcessHdr.Create();
                    insert_process_data.RequestId = reqId;
                    insert_process_data.RoleId = "HRTO"; //26-06-2020 New Basheer
                    insert_process_data.Button_List = null;
                    insert_process_data.IsCompleted = true;
                    insert_process_data.IsActive = true;
                    insert_process_data.TimeStamp = CurrentTime;
                    insert_process_data.distribution_flag = true;
                    insert_process_data.WF_id = wfid;
                    _entity.tb_ProcessHdr.Add(insert_process_data);
                    status = _entity.SaveChanges() > 0;
                    have_service = true;
                }
                #endregion
            }

            #endregion
            return have_service;

        }
        public string Find_Role_ForDistribution_TA(string role_Id, long countryId, tb_WF_Employee employee)
        {
            string[] splitData = role_Id.Split('-');
            if (splitData.Count() > 1)
            {
                string vendor = splitData[0];
                if (vendor == "VENDOR")
                {
                    return role_Id;
                }
            }
            else
            {
                var role = _entity.tb_Role.Where(x => x.Role_ID == role_Id && x.Country_ID == countryId && x.IsActive == true).FirstOrDefault();
                if (role != null)
                {
                    //--------------------------------------------
                    if (role.Organization_Flag == true && role.org_type != null)
                    {
                        #region 
                        if (role.org_type.Trim() == "DT")// Check the department table 
                        {
                            #region Department
                            var department = _entity.tb_Department.Where(x => x.IsActive == true && x.Department_Id == employee.Department_Id).FirstOrDefault();
                            if (department != null)
                            {
                                if (role.role_type == "MN" && department.Dept_Manager != null)  //28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return department.Dept_Manager;
                                }
                                else if (role.role_type == "CR" && department.Dept_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return department.Dept_Controller;
                                }
                                else if (role.role_type == "OA" && department.Dep_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return department.Dep_Office_Admin;
                                }
                            }
                            #endregion Department
                        }
                        else if (role.org_type.Trim() == "BL")// Check the business line table 
                        {
                            #region Business Line
                            var business = _entity.tb_BusinessLine.Where(x => x.IsActive == true && x.BL_Id == employee.Businessline_Id).FirstOrDefault();// 24-02-2020 ARCHANA SROSHTI 
                            if (business != null)
                            {
                                if (role.role_type == "MN" && business.BL_Manager != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return business.BL_Manager;
                                }
                                else if (role.role_type == "CR" && business.BL_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return business.BL_Controller;
                                }
                                else if (role.role_type == "OA" && business.BL_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return business.BL_Office_Admin;
                                }

                            }
                            #endregion Business Line
                        }
                        else if (role.org_type.Trim() == "B")// Check the business 
                        {
                            #region Business
                            var business = _entity.tb_Business.Where(x => x.Country_Id == role.Country_ID && x.IsActive == true && x.Bus_Id == employee.Business_Id).FirstOrDefault();//24-02-2020 ARCHANA SRISHTI 
                            if (business != null)
                            {
                                if (role.role_type == "MN" && business.Bus_Manager != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return business.Bus_Manager;
                                }
                                else if (role.role_type == "CR" && business.Bus_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return business.Bus_Controller;
                                }
                                else if (role.role_type == "OA" && business.Bus_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return business.Bus_Office_Admin;
                                }
                            }
                            #endregion Business
                        }
                        else if (role.org_type == "PG")// Check the Product group table 
                        {
                            #region Product Group
                            //var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.tb_Department.PG_Id).FirstOrDefault();
                            var product = _entity.tb_ProductGroup.Where(x => x.IsActive == true && x.PG_Id == employee.Productgroup_Id).FirstOrDefault();// 24-02-2020 ARCAHANA SRISHTI 
                            if (product != null)
                            {
                                if (role.role_type == "MN" && product.PG_Manager != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return product.PG_Manager;
                                }
                                else if (role.role_type == "CR" && product.PG_Controller != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return product.PG_Controller;
                                }
                                else if (role.role_type == "OA" && product.PG_Office_Admin != null)//28-02-2020 ARCHANA K V SRISHTI 
                                {
                                    return product.PG_Office_Admin;
                                }
                            }
                            #endregion Product Group 
                        }
                        else if (role.org_type.Trim() == "LN")// Basheer on 28-05-2020 
                        {
                            #region JUBAIL

                            var data = _entity.tb_Location.Where(x => x.IsActive == true && x.Location_Code == "JBL").FirstOrDefault();
                            if (data != null)
                            {
                                if (role.role_type == "MN" && data.Branch_Mgr != null)
                                {
                                    return data.Branch_Mgr;
                                }
                                else if (role.role_type == "CR" && data.Branch_cdr != null)
                                {
                                    return data.Branch_cdr;
                                }

                            }
                            #endregion 
                        }
                        #endregion
                    }
                    else
                    {
                        #region 
                        if (role.Assigned_ID != null)
                        {
                            #region Direct 
                            return role.Assigned_ID;
                            #endregion Direct 
                        }
                        else
                        {
                            // var checkLookupTable = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == role.Role_ID && x.IsActive == true).ToList();
                            //Preema 10-07-2020
                            var company = _entity.tb_Company.Where(x => x.Company_Id == employee.Company_Id && x.IsActive == true).FirstOrDefault();
                            var checkLookupTable = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == role.Role_ID && x.Code == company.Company_Code && x.IsActive == true).ToList();
                            string emp = "";
                            foreach (var item in checkLookupTable)
                            {
                                if (emp == string.Empty)
                                {
                                    emp = item.Description;
                                }
                                else
                                {
                                    emp = emp + "~" + item.Description;
                                }
                            }
                            return emp;
                        }
                        #endregion
                    }
                    //----------------------------------------
                }

            }
            return "";

        }
    }
}
