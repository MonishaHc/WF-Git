using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WF_Tool.DataLibrary.Data
{
    class CommonClass
    {
    }
    public class EscalationPersonsList
    {
        public string roleId_name { get; set; }
        public string emp_localId { get; set; }
    }

    public class ApproverList_ForLog
    {
        public string role_name { get; set; }
        public string approver_name { get; set; }
        public string remark { get; set; }
        public bool is_approve { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public string msg { get; set; }
    }
    public class EditableFormsList
    {
        public string editable_form { get; set; }
       
    }

    public class Forms_Edit_List
    {
        public string editable_form { get; set; }
        public bool can_view { get; set; } //Nimmi 27-04-2020
        public bool can_edit { get; set; }//Nimmi 27-04-2020

    }
    public class RequestFormsProcessingList
    {
        public string form_name { get; set; }
        public bool can_view { get; set; }
        public bool can_edit { get; set; }

    }
    public class RoleDetails
    {
        public string role_id { get; set;}
        public string role_name { get; set; }
        public string assigned_person_id { get; set; }
        public string deligated_personId { get; set; }
    }
    public class Request_Buttons
    {
        public string button_code { get; set; }
        public string button_name { get; set; }
        public string button_image { get; set; }
        public bool have_additional_info { get; set; }
        public long processId { get; set; }
        public int? buttonorder { get; set; } //Basheer on 14-03-2020
    }

    //Aju on 31-01-2020

    public class Log_history
    {
        public long Id { get; set; }
        public string content { get; set; }
        public string localemployee { get; set; }
        public string usertype { get; set; }
        public string itemtable { get; set; }
        public string itemtablecode { get; set; }
        public string process { get; set; }
        public string item_prev { get; set; }
        public string item_current { get; set; }
        public string message { get; set; }
        public bool IsActive { get; set; }
    }

    //Basheer on 26-02-2020 
    public class filedetails
    {
        public string filename { get; set; }
        public string filepath { get; set; }


    }






}
