using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WF_TOOL.Models
{
    public class EmployeeRequestModel
    {
        public string request_id { get; set; }
        public string wf_id { get; set; }
        public long application_id { get; set; }
        public string emp_local_id { get; set; }
        public string approver_id { get; set; }
        public string org_approer_id { get; set; }
        public string creator_id { get; set; }
        public string status_id { get; set; }
        public int approver_no { get; set; }
        public long wf_template_id { get; set; }
        public int escalation_no { get; set; }
        public string web_link { get; set; }
        public string payment_mode { get; set; }
        public DateTime cheque_date { get; set; }
        public decimal amount_sar { get; set; }
        public string purpose_text { get; set; }
        public string payable_to { get; set; }
        public string attachment_filepath { get; set; }
        public string from_bank { get; set; }
        public string from_addreess { get; set; }
        public string from_account_no { get; set; }
        public string to_beneficiary { get; set; }
        public string to_bankname { get; set; }
        public string to_address { get; set; }
        public string to_account_no { get; set; }
        public string bank_attachment { get; set; }
        public string remark { get; set; }
        public string cheque_account_no { get; set; }
        public string to_iban { get; set; }
        public string supplier_to { get; set; }

        //-------------P010----------------
        public string contract_local { get; set; }
        public string backcharge_invoice { get; set; }
        public string project { get; set; }
        public string year_booked { get; set; }
        public string customer { get; set; }
        //------------------P013-------------------
        public long tanumber { get; set; }
        public string tasupplier { get; set; }
        public string tachequeaccountno { get; set; }
        public string taamountype { get; set; }
        public string cheque_date_string { get; set; }
        // 13/05/2020 Alena Sics P052
        public long endofservice { get; set; }
        // 09/06/2020 ALENA EXTERNAL TRAINING  
        public string training_course { get; set; }
        public string detailed_justification { get; set; }
        public string external_agreement { get; set; }
        public long external_courseID { get; set; }
        public string coursename { get; set; }
        public DateTime datefrom { get; set; }
        public DateTime dateto { get; set; }
        public string trainingtype { get; set; }
        public string traininglocation { get; set; }
        public decimal cost { get; set; }//END------------
        //P023 Nimmi
        public string reason { get; set; }
        public int employee_grade { get; set; }
        public DateTime joining_date { get; set; }
        public string att_quotation_filepath { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal car_cost_reimbursement { get; set; }
        public string first_loan { get; set; }
        public string subsequent_loan { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal car_quotation_amount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal maximum_entitlement { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.#}")]
        public decimal monthly_installment { get; set; }
        public DateTime effective_date { get; set; }

        //------------------P015-------------------
        public long table_Id { get; set; }
        public string local_id { get; set; }
        public long request_table_id { get; set; }
        public long my_country_id { get; set; }
        public string button_code { get; set; }
        public string application_code { get; set; }
        public string req_id_only { get; set; }
        public string my_role { get; set; }

        public string my_id { get; set; }
        public long? blcontrollerid { get; set; }

        public string traname { get; set; }
        public string chargecostcenter { get; set; }
        public string chargeaccount { get; set; }

        public string rt_remarks { get; set; }
        public long carloanrequest_number { get; set; } //  P099 NIMMI 
        public string currenctType { get; set; }//28-02-2020 ARCHANA K V SRISHTI 

        public string filedata { get; set; } //Basheer on 25-02-2020 for fileupload
        public List<FileList> _FileList { get; set; }  //Basheer on 27-02-2020 
        public List<FileListPrint> _FileListPrint { get; set; }
        public string status_message { get; set; }

        //Accommodation in hotel/compound
        public Int32 accommodation_id { get; set; }
        public string accommodation_type { get; set; }
        public string[] guest_name { get; set; }

        public string hotel_name { get; set; }
        public string hotel_location { get; set; }
        public string room_type { get; set; }
        public string room_preference { get; set; }

        public string no_of_room { get; set; }

        public string hotel_address { get; set; }
        public string contact_person { get; set; }
        public string fax { get; set; }

        public DateTime approaximate_date { get; set; }
        public DateTime approaximate_time { get; set; }

        public DateTime from_period { get; set; }
        public DateTime to_period { get; set; }
        public string remarks { get; set; }
        //P061-ESAP Contribution(Preema)
        public ESAP_ContributionModel ESAP_ContributionModel { get; set; }
        // P062-Retirement Contribution(Preema)
        public RetirementContributionModel RetirementContributionModel { get; set; }
        //P024-Bank Loan Request(Preema)
        public BankLoanRequestModel BankLoanRequestModel { get; set; }

        //P016-Internal Transfer(Preema)
        public InternalTransferModel InternalTransferModel { get; set; }

        //P017-Contract Modification(Preema)
        public ContractModificationModel ContractModificationModel { get; set; }

        //P007-Vacation(Preema)
        public VacationModel VacationModel { get; set; }

        public SitevisitInternational sitevisitinternational { get; set; }

        //----------P025  Nimmi Mohan 16-04-2020----------
        public string reason_clearance { get; set; }
        public DateTime termination_Date { get; set; }
        public string eb_Toolbox_Returned { get; set; }
        public string eb_Workstation_Cleared { get; set; }
        public string eb_OfficialBusiness_Documents { get; set; }
        public string eb_SiteProject_Clearance { get; set; }
        public string eb_Uniform { get; set; }
        public string eb_Safety_Equipment { get; set; }
        public string eb_AllWorkflow_Approvals { get; set; }
        public DateTime eb_ISService_Deactivation_Date { get; set; }
        public string eb_Assigned_Delegate { get; set; }
        public string ad_HousingHousehold_cleared { get; set; }
        public string ad_Util_Water_cleared { get; set; }
        public string ad_CarGarageKey_Returned { get; set; }
        public string ad_Gatepass_Returned { get; set; }
        public string ad_Mobile_SimCard_Returned { get; set; }
        public string ad_CompanyID_Returned { get; set; }
        public decimal tr_Amount_SAR { get; set; }
        public string tr_ExternalTraining_Cost { get; set; }
        public string is_Desktop_Returned { get; set; }
        public string ft_Clearance_Obtained { get; set; }
        public string ft_eBank_Token { get; set; }
        public string ae_Clearance_Obtained { get; set; }
        public string hr_CarLoan_Cleared { get; set; }
        public string hr_SalaryAdvances_Settled { get; set; }
        public string hr_CompanyStamp_Returned { get; set; }
        public string hr_MedicalInsurance_Returned { get; set; }
        public string hr_Visa_Mastercard_Communicated { get; set; }
        public string hr_Savingcurrent_communicated { get; set; }
        public string hr_Remarks { get; set; }
        public string hr_Attachment_Filepath { get; set; }
        //07-05-2020
        public string eb_toolbx_returned_date { get; set; }
        public string workstation_cleared_date { get; set; }
        public string official_business_doc_date { get; set; }
        public string site_project_clearance_date { get; set; }
        public string uni_form_date { get; set; }
        public string safety_equipment_date { get; set; }
        public string workflow_approvals_date { get; set; }
        public string housing_household_date { get; set; }
        public string car_garage_key_date { get; set; }
        public string gatepass_sticker_date { get; set; }
        public string Mobile_returned_date { get; set; }
        public string company_id_returned_date { get; set; }
        public string utilities_water_etc_date { get; set; }
        public string desktop_etc_cleared_date { get; set; }
        public string clearance_obtained_fin_date { get; set; }
        public string e_bank_token_date { get; set; }
        public string clearance_obtained_ae_date { get; set; }
        public string car_loan_cleared_date { get; set; }
        public string salary_advance_settled_date { get; set; }
        public string company_stamp_returned_date { get; set; }
        public string medical_insurance_date { get; set; }
        public string visa_master_commun_date { get; set; }
        public string sav_current_account_date { get; set; }
        public string external_training_cost_date { get; set; } //terrin p025
        public List<EducationalAssistanceModel> _Educationassistance { get; set; }
        public EducationalAssistanceModel EducationalAssistanceModel { get; set; }   //P030-Educational Assistance(Chitra)28.05.2020
        public TicketRefundModel TicketRefundModel { get; set; }  //P003-Ticket Refund (Chitra)16.06.2020
        public BankGuaranteeModel BankGuaranteeModel { get; set; }  //SAS01-Bank Guarantee(Chitra)25.06.2020
        public ExpenseReportModel ExpenseReportModel { get; set; }//P045-(Chitra)08.07.2020
        public TrainingFolderModel TrainingFolderModel { get; set; }  //Terrin on 30-6-2020
        public TrainingCertificateModel TrainingCertificateModel { get; set; }
        public InHouseTrainingModel InHouseTrainingModel { get; set; } // T004-In-House Training(george)30-06-20
        public List<InHouseTrainingCourseModel> _CourseInformation { get; set; } // T004-In-House Training(george)30-06-20

        public ExternalTrainingModel ExternalTrainingModel { get; set; } // T001-External Training(george)13-07-2020
        public List<ExternalTrainingDetailModel> _TrainingDetails { get; set; } // T001-External Training(george)13-07-2020
    }

    public class FileList //Basheer on 27-02-2020 
    {
        public int filebatch { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
    }
    public class InfrastructureChangeModel
    {
        public string request_id { get; set; }
        public string wf_id { get; set; }
        public long application_id { get; set; }
        public string emp_local_id { get; set; }
        public string approver_id { get; set; }
        public string org_approer_id { get; set; }
        public string creator_id { get; set; }
        public string status_id { get; set; }
        public int approver_no { get; set; }
        public long wf_template_id { get; set; }
        public int escalation_no { get; set; }
        public string web_link { get; set; }

        public string change_summary { get; set; }
        public string detailed_description { get; set; }
        public string proposed_plan { get; set; }
        public string impct { get; set; }
        public string fallback_options { get; set; }
        public string file_path { get; set; }
        public string positive_risk_assessment { get; set; }
        public string negative_risk_assessment { get; set; }
        public int clarification { get; set; }// 0: Normal and 1: Emergency
    }
























}

