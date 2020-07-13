using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_Tool.DataLibrary.Data
{
    public class DataFromDB : BaseReference
    {

        public List<ApproverList_ForLog> ApproverList(string request_id, string applicaton)
        {
            var list = new List<ApproverList_ForLog>();
            string[] splitData = request_id.Split('-');
            var reqId = splitData[1];
            var data = _entity.tb_ApprovalLog.Where(x => x.RequestId == reqId && x.IsActive == true).OrderBy(x => x.Id).ToList();
            foreach (var item in data)
            {
                ApproverList_ForLog one = new ApproverList_ForLog();
                var one_rqt = new WF_Tool.DataLibrary.Data.ApprovalLog(item.Id);
                #region APC
                //if (item.Status == "APC")
                //{// THe processor Approve and complete the approval cycle . So the log wants to show it as Approve and history wants to show it as cycle completion 
                //    one = new ApproverList_ForLog();
                //    if (item.Actor_Id != null && item.Actor_Id != string.Empty)
                //    {
                //        one.approver_name = one_rqt.From_Name();
                //    }
                //    one.role_name = item.RoleId == null ? "" : item.RoleId;
                //    one.reason = item.Reason;
                //    one.remark = item.Remark;
                //    one.status = "INT";
                //    one.msg = "";
                //    one.is_approve = true;
                //    list.Add(one);
                //}
                #endregion APC
                one = new ApproverList_ForLog();
                if (item.Actor_Id != null && item.Actor_Id != string.Empty)
                {
                    one.approver_name = one_rqt.From_Name();
                }
                one.role_name = item.RoleId == null ? "" : item.RoleId;
                one.reason = item.Reason;
                one.remark = item.Remark;
                one.status = item.Status;
                one.msg = Request_Process_Message(one_rqt, applicaton.ToUpper());
                if (item.Status == "INT" || item.Status == "QIM" || item.Status == "PIM")
                {
                    one.is_approve = true;
                }
                list.Add(one);

                if (one.is_approve == true)
                {
                    ApproverList_ForLog two = new ApproverList_ForLog();
                    one = new ApproverList_ForLog();
                    if (item.Actor_Id != null && item.Actor_Id != string.Empty)
                    {
                        one.approver_name = one_rqt.From_Name();
                    }
                    one.role_name = item.RoleId == null ? "" : item.RoleId;
                    one.reason = item.Reason;
                    one.remark = item.Remark;
                    one.status = item.Status;
                    one.msg = applicaton + "-" + one_rqt.RequestId + " Forwarded by " + item.RoleId + " " + one_rqt.From_Name() + " to " + item.RoleId_To + " " + one_rqt.To_Name() + " on " + item.TimeStamp + " for next action.";
                    one.is_approve = false;
                    list.Add(one);
                }
            }
            return list;
        }

        private string Request_Process_Message(ApprovalLog one_rqt, string applicaton)
        {


            //Basheer canged SOME PARTS on 17-01-2020 to show role

            string msg = "";
            if (one_rqt.Status == "NEW")
            {
                msg = msg = applicaton + "-" + one_rqt.RequestId + " was Saved by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";
                //msg = applicaton + "-" + one_rqt.RequestId + " was Submited from " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
            }
            else if (one_rqt.Status == "SUB")
            {
                msg = applicaton + "-" + one_rqt.RequestId + " was Submited by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
            }
            else if (one_rqt.Status == "INT" || one_rqt.Status == "QIM" || one_rqt.Status == "PIM")
            {
                msg = applicaton + "-" + one_rqt.RequestId + " was Approved by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";
            }
            else if (one_rqt.Status == "APC")
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Approval Cycle completed by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
            }
            else if (one_rqt.Status == "PYD" && one_rqt.SequenceNo != 0) //Basheer on 28-05-2020
            {
                msg = applicaton + "-" + one_rqt.RequestId + " was Paid and Closed by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
            }
            else if (one_rqt.Status == "REJ")
            {
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was Rejected by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was Rejected by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ". ";
                }

            }
            else if (one_rqt.Status == "CLS")
            {
                msg = applicaton + "-" + one_rqt.RequestId + " was Closed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
            }
            else if (one_rqt.Status == "HLD")
            {
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was Hold by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was Hold by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ". ";
                }

            }
            else if (one_rqt.Status == "FWD")
            {
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was eforwarded by " + one_rqt.From_Name() + " to " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ".";
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was eforwarded by " + one_rqt.From_Name() + " to " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ".";
                }

            }
            else if (one_rqt.Status == "ESC")
            {
                //Basheer canged on 17-01-2020 to show role
                //msg = applicaton + "-" + one_rqt.RequestId + " was escalated by " + one_rqt.From_Name() + " to " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ".";
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was escalated by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " to " + one_rqt.RoleId_To + ' ' + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ".";
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was escalated by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " to " + one_rqt.RoleId_To + ' ' + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ".";
                }
            }
            else if (one_rqt.Status == "BKP")
            {
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was send back to previous approver  by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";// 24-02-2020 ARCHANA SRISHTI 
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was send back to previous approver by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ".";// 24-02-2020 ARCHANA SRISHTI 
                }

            }
            else if (one_rqt.Status == "BKI")
            {
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was send back to " + one_rqt.To_Name() + " by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was send back to " + one_rqt.To_Name() + " by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ".";
                }

            }
            else if (one_rqt.Status == "CNL")
            {
                if (one_rqt.Reason == null || one_rqt.Reason == "") //Basheer on 04-02-2020
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was cancelled by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + "."; //21-02-2020 ARCHANA SRISHI UPDATED WITH ROLE ID
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was cancelled by " + one_rqt.RoleId + ' ' + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ".";//21-02-2020 ARCHANA SRISHI UPDATED WITH ROLE ID
                }
            }
            else if (one_rqt.Status == "EDT")
            {
                msg = applicaton + "-" + one_rqt.RequestId + " was edited by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";
            }
            else if (one_rqt.Status == "UPC")
            {
                msg = applicaton + "-" + one_rqt.RequestId + " is currently being processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";//28-02-2020 ARCHANA K V SRISHTI 
            }
            else if (one_rqt.Status == "RRT")
            {
                if (one_rqt.Reason == null)
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was rerouted by " + one_rqt.From_Name() + " to " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ".";
                }
                else
                {
                    msg = applicaton + "-" + one_rqt.RequestId + " was rerouted by " + one_rqt.From_Name() + " to " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ", Remark : " + one_rqt.Reason + ".";
                }

            }
            else if (one_rqt.Status == "DST")
            {
                msg = one_rqt.RoleId_To + " " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ".";
            }
            else if (one_rqt.Status == "EDIT") //Basheer on 06-02-2020
            {
                msg = msg = applicaton + "-" + one_rqt.RequestId + " was Edited by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ".";
                //msg = applicaton + "-" + one_rqt.RequestId + " was Submited from " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp + ". ";
            }
            //Basheer on 28-05-2020 for TA requests
            else if (one_rqt.Status == "TAP")// CASH ADVANCE NOT READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Cash advance (Cash Paid) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "NTP")// CASH ADVANCE NOT READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Cash advance (Cash not Paid) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "TRD")// TICKET READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Travel Ticket Booking (Ticket Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "TNR") // TICKET NOT READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Travel Ticket Booking (Ticket not Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "CRD") //RENT CAR READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Rent a car Booking (Car Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "CNR") // RENT CAR NOT READY 
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Rent a car Booking (Car not Ready) is  Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "HBD") // HOTEL BOOKING READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Hotel Booking (Hotel Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "HNR") // HOTEL BOOKING NOT READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Hotel Booking (Hotel not Ready) not Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "VRD") // EXIT ENTRY VISA READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Exit Entry Visa (Visa Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "VNR") // EXIT ENTRY VISA NOT READY 
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Exit Entry Visa (Visa not Ready) is  Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "FRD") // FOREIGN VISA READY
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Foreign Visa (Visa Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "FNR") // FOREIGN VISA NOT READY 
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Foreign Visa (Visa not Ready) is  Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "IRD") // TRAVEL INSURANCE READY 
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Travel Insurance (Insurance Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "INR") // TRAVEL INSURANCE NOT READY 
            {
                msg = applicaton + "-" + one_rqt.RequestId + " Travel Insurance (Insurance not Ready) is Processed by " + one_rqt.From_Name() + " on " + one_rqt.TimeStamp;

            }
            else if (one_rqt.Status == "PRSEMAIL") //07-07-2020 Basheer
            {
                msg = one_rqt.RoleId_To + " " + one_rqt.To_Name() + " on " + one_rqt.TimeStamp + ".";
            }
            return msg;
        }

        public List<EditableFormsList> GetEditableForms(bool can_edit, string myrole, string wfType)
        {
            List<EditableFormsList> _list = new List<EditableFormsList>();
            if (can_edit == true)
            {
                var listData = _entity.tb_FormTemplate.Where(x => x.ROle_Id == myrole && x.tb_WFType.WF_ID == wfType && x.IsActive == true).ToList();
                foreach (var item in listData)
                {
                    EditableFormsList one = new Data.EditableFormsList();
                    one.editable_form = item.Form_Id;
                    _list.Add(one);
                }
            }
            return _list;
        }

        //27-04-2020 Nimmi

        public List<Forms_Edit_List> GetFormsEdit(bool can_edit, string myrole, string wfType)
        {
            List<Forms_Edit_List> _list = new List<Forms_Edit_List>();
            if (can_edit == true)
            {
                var listData = _entity.tb_FormTemplate.Where(x => x.ROle_Id == myrole && x.tb_WFType.WF_ID == wfType && x.IsActive == true).ToList();
                foreach (var item in listData)
                {
                    Forms_Edit_List one = new Data.Forms_Edit_List();
                    one.editable_form = item.Form_Id;
                    one.can_view = item.Can_View ?? false;
                    one.can_edit = item.Can_Edit ?? false;

                    _list.Add(one);
                }
            }
            return _list;
        }

        public List<RequestFormsProcessingList> GetRequestForms(string my_process, string myrole, string wfType, int level)
        {
            List<RequestFormsProcessingList> _list = new List<RequestFormsProcessingList>();
            var listData = _entity.tb_FormTemplate.Where(x => x.ROle_Id == myrole && x.tb_WFType.WF_ID == wfType && x.IsActive == true && x.Level == level && x.Status_Id == my_process).ToList();
            foreach (var item in listData)
            {
                RequestFormsProcessingList one = new Data.RequestFormsProcessingList();
                one.form_name = item.Form_Id;
                one.can_view = item.Can_View ?? false;
                one.can_edit = item.Can_Edit ?? false;
                _list.Add(one);
            }
            return _list;
        }

        public tb_WF_Template GetNextProcess(int request_level, string wf_type, bool haveProfile, long countryId, string profile_id)
        {
            if (haveProfile == true)
            {
                var tempate = _entity.tb_WF_Template.Where(x => x.tb_WFType.WF_ID == wf_type && x.tb_WFType.Country_Id == countryId && x.Sequence_NO == request_level && x.IsActive == true && x.tb_Emp_Profile.Profile_ID == profile_id).FirstOrDefault();
                return tempate;
            }
            else
            {
                var tempate = _entity.tb_WF_Template.Where(x => x.tb_WFType.WF_ID == wf_type && x.tb_WFType.Country_Id == countryId && x.Sequence_NO == request_level && x.IsActive == true).FirstOrDefault();
                return tempate;
            }
        }

        public List<Request_Buttons> GetAllAvailableButtons(long template_id)
        {
            string host = _entity.tb_Hostaddress.Where(x => x.IsActive == true).FirstOrDefault().Host_Address;
            var data = _entity.tb_WF_Template.Where(x => x.Id == template_id && x.IsActive == true).FirstOrDefault();
            List<Request_Buttons> lists = new List<Request_Buttons>(); //Basheer on 14-03-2020 changed list to lists
            if (data != null)
            {
                if (data.Button_List != null)
                {
                    foreach (var item in data.Button_List.Split('~').ToList())
                    {
                        var button = _entity.tb_Button.Where(x => x.Code.Trim().ToLower() == item.ToLower().Trim() && x.IsActive == true).FirstOrDefault(); //Added order by on 13-03-2020 by basheer
                        if (button != null)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = button.Code.Trim();
                            one.button_name = button.Description;
                            one.button_image = host + button.Button_Image;
                            one.have_additional_info = button.Have_Additional_Info;
                            one.buttonorder = button.Button_Order; //Added by basheer on 14-03-2020
                            lists.Add(one);
                        }
                    }
                }
            }

            var list = lists.OrderBy(x => x.buttonorder).ToList();
            return list;
        }

        public List<tb_UniversalLookupTable> GetRequestContent(string wftype)
        {
            var data = _entity.tb_UniversalLookupTable.Where(x => x.Code == wftype && x.IsActive == true).ToList();
            return data;
        }

        public List<Request_Buttons> GetAllAvailableProcessButtons(string requestid, string roleid, string wf_Id) //Basheer on 28-05-2020 copy paste entire items
        {
            string[] splitData = requestid.Split('-');
            long reqid = Convert.ToInt32(splitData[1]);
            long wf_type_id = 0;
            if (wf_Id != "")
            {
                wf_type_id = Convert.ToInt32(wf_Id);
            }
            string host = _entity.tb_Hostaddress.Where(x => x.IsActive == true).FirstOrDefault().Host_Address;
            var data = _entity.tb_ProcessHdr.Where(x => x.RequestId == reqid && x.IsActive == true && x.IsCompleted == false && x.RoleId == roleid && x.WF_id == wf_type_id).ToList(); //Basheer on 28-05-2020
            List<Request_Buttons> list = new List<Request_Buttons>();
            if (data.Count() != 0) //Basheer on 07-07-2020
            {
                foreach (var btn in data)
                {
                    if (btn.Button_List != null)
                    {
                        foreach (var item in btn.Button_List.Split('~').ToList())
                        {
                            var button = _entity.tb_Button.Where(x => x.Code.Trim().ToLower() == item.ToLower().Trim() && x.IsActive == true).FirstOrDefault();
                            if (button != null)
                            {
                                Request_Buttons one = new Request_Buttons();
                                one.button_code = button.Code.Trim();
                                one.button_name = button.Description;
                                one.button_image = host + button.Button_Image;
                                one.have_additional_info = button.Have_Additional_Info;
                                one.processId = btn.Id;//Basheer on 28-05-2020
                                list.Add(one);
                            }
                        }
                    }
                }
            }
            else
            {
                var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //added order by on 13-03-2020 basheer
                if (button != null)
                {
                    foreach (var item in button)
                    {
                        Request_Buttons one = new Request_Buttons();
                        one.button_code = item.Code.Trim();
                        one.button_name = item.Description;
                        one.button_image = host + item.Button_Image;
                        one.have_additional_info = item.Have_Additional_Info;
                        list.Add(one);

                    }

                }
            }

            return list;
        }
        public List<Request_Buttons> GetAllAvailableButtonsNew(string requestid)
        {
            string host = _entity.tb_Hostaddress.Where(x => x.IsActive == true).FirstOrDefault().Host_Address;
            string[] splitData = requestid.Split('-');
            string reqid = splitData[1];
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == reqid && x.IsActive == true).FirstOrDefault();
            List<Request_Buttons> list = new List<Request_Buttons>();
            if (data != null)
            {
                if (data.Approval_No != "1" || data.Approval_No == "0" || data.Status_ID == "CNL" || data.Status_ID == "CLS" || data.Status_ID == "REJ")
                {
                    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //added order by on 13-03-2020 basheer
                    if (button != null)
                    {
                        foreach (var item in button)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = item.Code.Trim();
                            one.button_name = item.Description;
                            one.button_image = host + item.Button_Image;
                            one.have_additional_info = item.Have_Additional_Info;
                            list.Add(one);

                        }

                    }
                }
                else
                {
                    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW" || x.Code.Trim().ToLower() == "CNL") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //Added order by on 13-03-2020 basheer
                    if (button != null)
                    {
                        foreach (var item in button)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = item.Code.Trim();
                            one.button_name = item.Description;
                            one.button_image = host + item.Button_Image;
                            one.have_additional_info = item.Have_Additional_Info;
                            list.Add(one);

                        }

                    }

                }
            }

            return list;
        }




        ///aju sics 31-01-2020///

        public List<Log_history> Loghistorylist(string application)
        {
            var list = new List<Log_history>();
            var data = _entity.tb_ProcessLog.Where(x => x.IsActive == true).OrderBy(x => x.Id).ToList();
            foreach (var item in data)
            {
                Log_history one = new Log_history();
                var one_rqt = new WF_Tool.DataLibrary.Data.Adminloghistory(item.Id);

                one = new Log_history();

                one.content = item.Content;
                one.localemployee = item.LocalEmplyee_ID;
                one.usertype = item.UserType;
                one.itemtable = item.Item_Table;
                one.itemtablecode = item.Item_Table_Code;
                one.process = item.Process;
                one.item_prev = item.Item_Previous;
                one.item_current = item.Item_Current;
                one.message = item.Content + " Change by " + "  " + item.Item_Previous + " to " + item.Item_Current + "";
                one.IsActive = false;
                list.Add(one);
            }

            return list;
        }

        //Basheer on 04-02-2020 for buttons for initator with status BKI

        public List<Request_Buttons> GetAllAvailableButtonsBKI(string requestid)
        {
            string host = _entity.tb_Hostaddress.Where(x => x.IsActive == true).FirstOrDefault().Host_Address;
            string[] splitData = requestid.Split('-');
            string reqid = splitData[1];
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == reqid && x.IsActive == true).FirstOrDefault();
            List<Request_Buttons> list = new List<Request_Buttons>();
            if (data != null)
            {
                if (data.Approval_No != "0")
                {
                    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW" || x.Code.Trim().ToLower() == "SUT") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //Added order by on 13-03-2020 by basheer
                    if (button != null)
                    {
                        foreach (var item in button)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = item.Code.Trim();
                            one.button_name = item.Description;
                            one.button_image = host + item.Button_Image;
                            one.have_additional_info = item.Have_Additional_Info;
                            list.Add(one);

                        }

                    }
                }
                else
                {
                    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW" || x.Code.Trim().ToLower() == "CNL" || x.Code.Trim().ToLower() == "SUT") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //Added order by on 13-03-2020 by basheer
                    if (button != null)
                    {
                        foreach (var item in button)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = item.Code.Trim();
                            one.button_name = item.Description;
                            one.button_image = host + item.Button_Image;
                            one.have_additional_info = item.Have_Additional_Info;
                            list.Add(one);

                        }

                    }

                }
            }

            return list;
        }

        //=--------------------------Bassheer code end here


        //Basheer on 27-02-2020 

        public List<tb_ReqestAttachments> Getattachments(string requestid)
        {
            string[] splitData = requestid.Split('-');
            string reqid = splitData[1];
            var attachment = _entity.tb_ReqestAttachments.Where(x => x.Request_id == reqid && x.IsActive == true).ToList();
            return attachment;
        }

        //Basheer on 14-03-2020 for refreshing pages

        public List<Request_Buttons> GetAllAvailableButtonsRefresh(string requestid)
        {
            string host = _entity.tb_Hostaddress.Where(x => x.IsActive == true).FirstOrDefault().Host_Address;
            string[] splitData = requestid.Split('-');
            string reqid = splitData[1];
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == reqid && x.IsActive == true).FirstOrDefault();
            List<Request_Buttons> list = new List<Request_Buttons>();
            if (data != null)
            {
                if (data.Approval_No != "1" || data.Approval_No == "0" || data.Status_ID == "CNL" || data.Status_ID == "CLS" || data.Status_ID == "REJ") //Terrin on 15/6/2020
                {
                    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //added order by on 13-03-2020 basheer
                    if (button != null)
                    {
                        foreach (var item in button)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = item.Code.Trim();
                            one.button_name = item.Description;
                            one.button_image = host + item.Button_Image;
                            one.have_additional_info = item.Have_Additional_Info;
                            list.Add(one);

                        }

                    }
                }
                else
                {
                    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW" /*|| x.Code.Trim().ToLower() == "CNL"*/) && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //Added order by on 13-03-2020 by basheer //Terrin 15/6/2020
                    if (button != null)
                    {
                        foreach (var item in button)
                        {
                            Request_Buttons one = new Request_Buttons();
                            one.button_code = item.Code.Trim();
                            one.button_name = item.Description;
                            one.button_image = host + item.Button_Image;
                            one.have_additional_info = item.Have_Additional_Info;
                            list.Add(one);

                        }

                    }
                }


                //Basheer on 28-03-2020
                //if (data.Status_ID == "CNL") 
                //{
                //    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //added order by on 13-03-2020 basheer
                //    if (button != null)
                //    {
                //        foreach (var item in button)
                //        {
                //            Request_Buttons one = new Request_Buttons();
                //            one.button_code = item.Code.Trim();
                //            one.button_name = item.Description;
                //            one.button_image = host + item.Button_Image;
                //            one.have_additional_info = item.Have_Additional_Info;
                //            list.Add(one);

                //        }

                //    }
                //}
                //else
                //{
                //    var button = _entity.tb_Button.Where(x => (x.Code.Trim().ToLower() == "PRN" || x.Code.Trim().ToLower() == "EFW") && x.IsActive == true).OrderBy(x => x.Button_Order).ToList(); //Added order by on 13-03-2020 by basheer
                //    if (button != null)
                //    {
                //        foreach (var item in button)
                //        {
                //            Request_Buttons one = new Request_Buttons();
                //            one.button_code = item.Code.Trim();
                //            one.button_name = item.Description;
                //            one.button_image = host + item.Button_Image;
                //            one.have_additional_info = item.Have_Additional_Info;
                //            list.Add(one);

                //        }

                //    }
                //}
                //Basheer code end here
            }
            return list;
        }

        //=--------------------------Bassheer code end here

        //Basheer on 27-02-2020 

        public bool checkapprover(string requestid, string approverid)
        {
            string[] splitData = requestid.Split('-');
            string reqid = splitData[1];
            var data = _entity.tb_Request_Hdr.Where(x => x.Request_ID == reqid && x.IsActive == true).FirstOrDefault();
            var data_employee = _entity.tb_WF_Employee.Where(x => x.LocalEmplyee_ID == data.Approver_ID && x.IsActive == true).FirstOrDefault();
            //if (data.Status_ID == "BKI" || data.Status_ID == "NEW")
            //{
            //    if (data.Employee_ID == approverid)
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            var rolecheck = _entity.tb_Role.Where(x => x.Id == data.RoleId && x.IsActive == true).FirstOrDefault();
            if (rolecheck != null) //Basheer on 27-03-2020
            {
                if (data.Status_ID == "CLS" || data.Status_ID == "PYD" || data.Status_ID == "REJ" || data.Status_ID == "CNL") //Basheer on 20-04-2020 /Basheer on 22-04-2020 added rej and cnl
                {
                    return false;
                }
                else
                {
                    if (rolecheck.GroupRole == true)
                    {
                        var universal = _entity.tb_UniversalLookupTable.Where(x => x.Table_Name == rolecheck.Role_ID && x.IsActive == true).ToList();
                        if (universal.Any(x => x.Description == approverid))
                        {
                            return true;
                        }
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (data.Approver_ID == approverid)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false; //Basheer on 27-03-2020
            }
        }

        //}
        public string Getrequester(string adaccount)
        {
            string empid = _entity.tb_WF_Employee.Where(x => x.ADAccount == adaccount && x.IsActive == true).FirstOrDefault().LocalEmplyee_ID;
            return empid;
        }

        //P062-Retirement Contribution(Preema)
        public string GetDescription(string wftype, string table_name)
        {
            string description = _entity.tb_UniversalLookupTable.Where(x => x.Code == wftype && x.IsActive == true && x.Table_Name == table_name).FirstOrDefault().Description;
            return description;
        }

        public string GetApprovalLogStatus(string requestid)
        {
            string[] splitData = requestid.Split('-');
            var reqId = splitData[1];
            var approver_status = _entity.tb_Request_Hdr_History.Where(x => x.Request_ID == reqId && x.IsActive == true).FirstOrDefault();
            var approver_log_status = _entity.tb_ApprovalLog.Where(x => x.RequestId == reqId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
            var request_history = _entity.tb_Request_Hdr_History.Where(x => x.Request_ID == reqId && x.IsActive == true).FirstOrDefault();
            if (approver_status == null)
            {
                return "SUB";
            }
            else if (request_history.Status_ID == "BKI" && request_history.Approval_No == "0" && approver_log_status.Status == "SUB")
            {
                return "SUB";

            }
            return approver_status.Status_ID;
        }
        public string GetEscalationStatus(string requestid)
        {
            string[] splitData = requestid.Split('-');
            var reqId = splitData[1];
            var approver_status = _entity.tb_ApprovalLog.Where(x => x.RequestId == reqId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
            return approver_status.Status;
        }

        //P016-Internal Transfer(Preema)
        //public string GetApprovalLogStatus(string requestid)
        //{
        //    string[] splitData = requestid.Split('-');
        //    var reqId = splitData[1];
        //    var approver_status = _entity.tb_ApprovalLog.Where(x => x.RequestId == reqId && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
        //    return approver_status.Status;
        //}

        //P016-Internal Transfer(Preema)
        public List<RequestFormsProcessingList> GetViewForms_Refresh(string wfType, string requestid, string form_name)
        {
            string[] splitData = requestid.Split('-');
            var reqId = splitData[1];
            List<RequestFormsProcessingList> _list = new List<RequestFormsProcessingList>();
            var Request = _entity.tb_Request_Hdr.Where(x => x.Request_ID == reqId && x.IsActive == true).FirstOrDefault();
            var Request_History = _entity.tb_Request_Hdr_History.Where(x => x.Request_ID == reqId && x.IsActive == true).FirstOrDefault();
            if (Request_History != null && Request_History.RoleId != "")
            {
                int id = 0;
                int Approval_No = 0;

                //if (Request_History.RoleId == "")
                //{
                //    id = Convert.ToInt32(Request.RoleId);
                //    Approval_No = Convert.ToInt32(Request.Approval_No);
                //}
                //else
                //{
                id = Convert.ToInt32(Request_History.RoleId);
                Approval_No = Convert.ToInt32(Request_History.Approval_No);
                // }

                var Role = _entity.tb_Role.Where(x => x.Id == id && x.IsActive == true).FirstOrDefault();

                if (Approval_No > 0)
                {
                    var listData = _entity.tb_FormTemplate.Where(x => x.Form_Id == form_name && x.tb_WFType.WF_ID == wfType && x.IsActive == true && x.Level == Approval_No && x.ROle_Id == Role.Role_ID).ToList();
                    foreach (var item in listData)
                    {
                        RequestFormsProcessingList one = new Data.RequestFormsProcessingList();
                        one.form_name = item.Form_Id;
                        one.can_view = item.Can_View ?? false;
                        one.can_edit = item.Can_Edit ?? false;
                        _list.Add(one);
                    }
                }
            }

            return _list;
        }
    }

}
