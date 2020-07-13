using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WF_Tool.ClassLibrary;

namespace WF_TOOL.Models
{
    public class EmployeeModel
    {
        public string employeetype { get; set; }
        public string emp_localid { get; set; }
        public string emp_name { get; set; }
        public string globalid { get; set; }
        public string grade { get; set; }
        public string company { get; set; }
        public string tittle { get; set; }
        public string btn_Text { get; set; }
        public string department { get; set; }
        public string businessline { get; set; }
        public string cost_center { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public string extension { get; set; }
        //public RequestProcess request_process_type { get; set; }
        public statusEnum request_process_type { get; set; }
        public display_statusEnum request_process_type_display { get; set; }
        public string global_group { get; set; }
        public string localgroup { get; set; }
        public string DateOfJoin { get; set; }
        public string position_class { get; set; }
        public string delegation_band { get; set; }
        public string ad_account { get; set; }
        public string email { get; set; }
        public string cc_name { get; set; }
        public string profile { get; set; }
        public string country_name { get; set; }
        public string job_tittle { get; set; }
        public string pg_name { get; set; }
        public string bl_name { get; set; }
        public bool delegation_flag { get; set; }
       
        public string delegation_emp { get; set; }
        public string vender { get; set; }
        public string line_manager { get; set; }
        public string location { get; set; }
        public string business { get; set; }
        public int approvaltype { get; set; } //Basheer on 24-01-2020 for approved and pending
        public string application_code { get; set; }
        public string adAccountId { get; set; }
        public string admin_access { get; set; }
        public string mobile_extention { get; set; }

        public string status { get; set; } //Basheer on 03-02-2020 for status in my request
        public DateTime fdate { get; set; }
        public DateTime tdate { get; set; }

        public long country_id { get; set; }//Basheer on 13-04-2020 for superadmin
        public long application_id { get; set; }//Basheer on 13-04-2020 for superadmin
    }
    public class AddEmployeeModel
    {
        public string admin_employee_local_id { get; set; }
        [Required(ErrorMessage = "Please enter Employee Name.")]
        public string employee_name { get; set; }
        [Required(ErrorMessage = "Please enter AD User Account.")]
        public string ad_user_account { get; set; }
        [Required(ErrorMessage = "Please enter Employee Type.")]
        public string employee_type { get; set; }
        [Required(ErrorMessage = "Please enter Global Employee ID.")]
        public string global_emp_id { get; set; }
        [Required(ErrorMessage = "Please enter Email Address.")]
        public string email_address { get; set; }
        [Required(ErrorMessage = "Please enter Local Employee ID.")]
        public string local_emp_id { get; set; }
        [Required(ErrorMessage = "Please enter Job Tittle.")]
        public long job_tittle { get; set; }
        public string join_date_string { get; set; }
        public DateTime join_date { get; set; }
        [Required(ErrorMessage = "Please enter Tele Extn.")]
        public string tele_ext { get; set; }
        [Required(ErrorMessage = "Please enter Mobile No.")]
        public string mobile_no { get; set; }
        [Required(ErrorMessage = "Please enter Global Grade.")]
        public string global_grade { get; set; }
        [Required(ErrorMessage = "Please enter Local Grade.")]
        public string local_grade { get; set; }
        [Required(ErrorMessage = "Please enter Position Class.")]
        public string position_class { get; set; }
        [Required(ErrorMessage = "Please select Country.")]
        public long country_id { get; set; }
        [Required(ErrorMessage = "Please select Business.")]
        public long business_id { get; set; }
        [Required(ErrorMessage = "Please select Business Line.")]
        public long business_line_id { get; set; }
        [Required(ErrorMessage = "Please select Product Group.")]
        public long product_group_id { get; set; }
        [Required(ErrorMessage = "Please select Department.")]
        public long department_id { get; set; }
        [Required(ErrorMessage = "Please select Company.")]
        public long company_id { get; set; }
        [Required(ErrorMessage = "Please select WF Profile.")]
        public long profile_id { get; set; }
        [Required(ErrorMessage = "Please select Location.")]
        public long location_id { get; set; }
        [Required(ErrorMessage = "Please select Cost Center.")]
        public long cost_center_id { get; set; }
        //[Required(ErrorMessage = "Please select Delegation Band.")] 20-02-2020 ARCHANA SRISHTI 
        public string delegation_band { get; set; }
        //[Required(ErrorMessage = "Please select Delegation Status .")]20-02-2020 ARCHANA SRISHTI 
        public bool delegation_status { get; set; }
        //[Required(ErrorMessage = "Please select Delegate / Deputy.")]19-02-2020 ARCHANA SRISHTI
        public string delegate_deputy { get; set; }
        //[Required(ErrorMessage = "Please select Vener / Sponser.")]19-02-2020 ARCHANA SRISHTI
        public Nullable<long> venderId { get; set; } //19-02-2020 ARCHANA SRISHTI 
        [Required(ErrorMessage = "Please select Direct Line Manager.")]
        public string line_manager_code { get; set; }
        public string tittle { get; set; }
        public string admin_access { get; set; }
        public string btn_Text { get; set; }
        public gender gender { get; set; }
        public string mobile_extention { get; set; }
        public BooleanValue isAppraisal { get; set; }
        public BooleanValue isTimeSheet { get; set; }
        public BooleanValue deles { get; set; }    //15/06/2020 Alena
    }

    public class ListEmployeeModels
    {
        public List<EmployeeModel> _list = new List<EmployeeModel>();
    }


}


