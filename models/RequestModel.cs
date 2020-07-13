using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WF_Tool.ClassLibrary;

namespace WF_TOOL.Models
{
    public class RequestModel
    {
        public string employeetype { get; set; }
        [Required(ErrorMessage = "Required")]
        public string domainid { get; set; }
        public string applicationid { get; set; }
        public string wftypeid { get; set; }//Services like Bank LoanRequest
        public string filepath { get; set; }
        public string employeeid { get; set; }
        public string car_type { get; set; }
        public string payment_type { get; set; }
        public string hotel_location { get; set; }
        public long country_id { get; set; }
        public long taid { get; set; }
        public long abb_locations { get; set; }      
        public string ad_account { get; set; }
        public string application_code { get; set; }
        public string application { get; set; }
        public string WF_ID { get; set; }//  Sibi 
        public string currenctType { get; set; }//28-02-2020 ARCHANA K V SRISHTI 
        public string status_message { get; set; }
        public long allow_id { get; set; } //Terrin
        //P016-Internal Transfer(Preema)
        public InternalTransferModel InternalTransferModel { get; set; }
        //P017-Contract Modification(Preema)
        public ContractModificationModel ContractModificationModel { get; set; }

        //P007-Vacation(Preema)
        public VacationModel VacationModel { get; set; }

        // 14/05/2020 Alena Sics
        public string endof_service { get; set; }
        // 09/06/2020 ALENA EXTERNAL TRAINING  
        public string training_course { get; set; }
        public string course_attachment { get; set; }
        public string detailed_justification { get; set; }
        public string external_agreement { get; set; }
        public long external_courseID { get; set; }
        public string[] strExternal { get; set; }
        public string trainingtype { get; set; }

        public string location { get; set; }//END------------
        [Required(ErrorMessage = "Please select Employee Group.")]
        public string description { get; set; }//28.05.2020 Chitra V P030
        [Required(ErrorMessage = "Please Select Location.")]
        public long location_id { get; set; }
        [Required(ErrorMessage = "Please select Company.")]
        public long Company_Id { get; set; }//26.06.2020 Chitra srishti SAS01 
        public long PG_Id { get; set; }//26.06.2020 Chitra srishti SAS01 
        public long BL_Id { get; set; }//26.06.2020 Chitra srishti SAS01 
        public long wfcreatetype { get; set; } //Basheer on 28-05-2020

        public string mod_requestid { get; set; }//Basheer on 28-05-2020

        public BusinessInternational businessinternational { get; set; } //Basheer on 28-05-2020
                                                                         //Anzeem 09-06-2020
        public SitevisitInternational sitevisitinternational { get; set; }
        //public SiteVisitInternational _sitevisitinternational { get; set; }

        public IList<SiteFindingsList> siteFindingList { get; set; }//anzeem on 03-06-2020
        public IList<ProblemTypeList> problemTypeList { get; set; }//anzeem on 03-06-2020

        public string CourseListName { get; set; } // George on 29-06-2020

    }

    public class WaitingRequests
    {
        public long req_id { get; set; }//Table primary key
        public string request_id { get; set; }
        public long application_id { get; set; }
        public string application { get; set; }
        public string employee_local_id { get; set; }
        public string wf_type_id { get; set; }
        public int count { get; set; }
        public string employee_name { get; set; }
        public string approver_id { get; set; }
        public string button_type { get; set; }
        public string company { get; set; }
        public string business { get; set; }
        public string business_line { get; set; }
        public string pro_group { get; set; }
        public string date { get; set; }
        public string wf_domain { get; set; }
        public string wf_type { get; set; }
        public string final_status { get; set; }
        public string current_actor { get; set; }
        public long process_table_id { get; set; }
        public string wf_type_name { get; set; } //Basheer on 08-05-2020
        public string roleid { get; set; } //Basheer on 28-05-2020
    }
    public class WaitingRequestingList
    {
        public List<WaitingRequests> list { get; set; }
        public string typeid { get; set; }
        public string myId { get; set; }
        public DateTime fromdate { get; set; } //Basheer on 03-02-2020
        public DateTime todate { get; set; }//Basheer on 03-02-2020
        public string status { get; set; }//Basheer on 03-02-2020
    }
    public class WaitingRequestDetails
    {
        public string ad_account { get; set; }
        public long req_id { get; set; }// Request table id
        public string req_id_only { get; set; }// Request  id
        public string my_role { get; set; }
        public string approver_id { get; set; }// with application  id
        public string my_Process_type { get; set; }// what i wants to do in this request
        public string request_id { get; set; }
        public string emp_name { get; set; }
        public string global_id { get; set; }
        public string local_id { get; set; }
        public string company { get; set; }
        public string job_tittle { get; set; }
        public string department { get; set; }
        public string business_line { get; set; }
        public string cost_center { get; set; }
        public string mobile_phone { get; set; }
        public string telephone { get; set; }
        public string application_id { get; set; }
        public string wf_type { get; set; }
        public string domain { get; set; }
        public string application { get; set; }
        public string service_required { get; set; }
        public int level { get; set; }
        public string my_process_code { get; set; }
        public bool haveProfile { get; set; }
        public string request_profile { get; set; }
        public long template_id { get; set; }
        public string location { get; set; }// 12-02-2020 Archana 
        public string extension { get; set; }// 12-02-2020 Archana 
        public string date_created { get; set; }//12-02-2020 Archana
        public long endofservice { get; set; } //EOSB CALCULATION // 15/05/2020 ALENA
                                               // 24/06/2020 ALENA SICS FOR A008
        public string pickup_at { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string quantity { get; set; }
        public string drivername { get; set; }
        public string Mobile_No { get; set; }
        public string Employee_id { get; set; }
        public string employee_name { get; set; }
        public string carmodel { get; set; }
        public string payment_type { get; set; }
        public EmployeePickupModel EmployeePickupModel { get; set; }
        //END----------------------------------------------------------------
        // 01/07/2020 ALENA SICS FOR A009
        public string drop_at { get; set; }
        public EmployeeDropModel EmployeeDropModel { get; set; }
        public string quantity_drop { get; set; }
        public string drivername_drop { get; set; }
        public string Mobile_No_drop { get; set; }
        public string Employee_id_drop { get; set; }
        public string carmodel_drop { get; set; }
        //end---------------------------------------------------------------------
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
        public string to_iban { get; set; }
        public string bank_attachment { get; set; }
        public string remark { get; set; }
        //07-05-2020 Nimmi P099
        public string cheque_account_no { get; set; }
        //public string to_iban { get; set; }
        public string supplier_to { get; set; }
        //----------------------------------------
        public string title { get; set; }
        public string my_button { get; set; }
        public bool can_edit { get; set; }
        public bool can_view { get; set; }
        public bool refresh_view { get; set; }
        public string cheque_date_string { get; set; }
        public bool canEscalate { get; set; }
        public int escalation_No { get; set; }
        public bool is_hold { get; set; }
        public bool is_first_approver { get; set; }
        public string can_distribute { get; set; }
        public string my_role_code { get; set; }
        //public InfrastructureChange _InfrastructureChange { get; set; }
        public IntroductionCertificateModel IntroductionCertificateModel { get; set; }

        public Applicationfor_mobile Applicationfor_mobile { get; set; } //P060 Terrin on 31/3/2020

        public MedicalInsuranceApplication MedicalInsuranceApplication { get; set; }//P029 Terrin
        public LetterToRealEstateModel LetterToRealEstateModel { get; set; }   //P012 Nimmi
        //------------------Basheer P013-------------------
        public long tanumber { get; set; }
        public string tasupplier { get; set; }
        public string tachequeaccountno { get; set; }
        public string taamountype { get; set; }
        public string request_empid { get; set; }
        public long taid { get; set; }

        //P023 Nimmi

        public string reason { get; set; }
        public int employee_grade { get; set; }
        public DateTime joining_date { get; set; }
        public string att_quotation_filepath { get; set; }
        public decimal car_cost_reimbursement { get; set; }
        public string first_loan { get; set; }
        public string subsequent_loan { get; set; }
        public decimal car_quotation_amount { get; set; }
        public decimal maximum_entitlement { get; set; }
        public decimal monthly_installment { get; set; }
        public DateTime effective_date { get; set; }
        public string effective_date_string { get; set; }  //04-03-2020 Nimmi
        public string joining_date_string { get; set; }  //07-04-2020 Nimmi
        //----------------------

        //Basheer on 27-01-2020  to check the view & buttons of page
        public string dataview_id { get; set; }// Request  id

        //------------------P015-------------------
        public long? blcontrollerid { get; set; }
        public string traname { get; set; }
        public string chargecostcenter { get; set; }
        public string chargeaccount { get; set; }
        public string currenctType { get; set; }
        public string rt_remarks { get; set; }
        public long country_id { get; set; }
        public string next_processor_role { get; set; }
        public string next_process { get; set; }
        public long carloanrequest_number { get; set; }
        public string checkstatus { get; set; } //Basheer on 04-02-2020 to checkstatus for backtoinitiator or not



        //-----Print 28/02/2020 by Nimmi Mohan------//
        public string my_id { get; set; }
        public string WF_ID { get; set; }
        public long request_table_id { get; set; }
        public long my_country_id { get; set; }
        public string button_code { get; set; }
        public string application_code { get; set; }
        //------------end-----------//

        // ----P060-------Terrin on31/3/2020

        public string allow_pergrp { get; set; }
        public string mob_remark { get; set; }
        public string justification { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
        //A007-Accommodation in Hotel/Compound(Preema)
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
        public List<string> guest = new List<string>();
        public string emp_local_id { get; set; }
        public string filedata { get; set; }
        public List<FileList> _FileList { get; set; }
        public long wf_template_id { get; set; }

        public string creator_id { get; set; }
        public long application_ids { get; set; }
        public string approaximate_date_string { get; set; }
        public string approaximate_time_string { get; set; }
        public string from_period_string { get; set; }
        public string to_period_string { get; set; }
        public string error { get; set; }

        //P049-Other Personnel Services(Preema)
        public string request_details { get; set; }
        public string document_price { get; set; }
        public string document_price_value { get; set; } //terrin on 6/6/2020
        public bool is_hr_department { get; set; }
        //P061-ESAP Contribution(Preema)
        public ESAP_ContributionModel ESAP_Contribution { get; set; }
        //P062-Retirement Contribution(Preema)
        public RetirementContributionModel RetirementContributionModel { get; set; }
        //P024-Bank Loan Request(Preema)
        public BankLoanRequestModel BankLoanRequestModel { get; set; }

        //P016-Internal Transfer(Preema)
        public InternalTransferModel InternalTransferModel { get; set; }

        //P016-Internal Transfer(Preema)
        public ContractModificationModel ContractModificationModel { get; set; }
        //P007-Vacation(Preema)
        public VacationModel VacationModel { get; set; }

        //-----------vyas P041 --------------------------------------
        public bool is_compliance { get; set; }

        public string compliance_approval_date { get; set; }

        public string Last_dayof_work { get; set; }

        public string Return_to_duty { get; set; }

        public bool is_workflow_delegated { get; set; }

        public string justification_provided { get; set; }

        public string address_during_absence { get; set; }

        public string Contact_number { get; set; }

        public int Child_Birth_Leave { get; set; }

        public int Saturday { get; set; }

        public int Friday { get; set; }

        public int Total { get; set; }
        //----------P025  Nimmi Mohan 30-03-2020-----------------------
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

        public string termination_date_string { get; set; }
        public string eb_ISService_Deactivation_Date_string { get; set; }
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
                                                                //---------------------------------
        public string description { get; set; }//03.06.2020 Chitra V P030
        public EducationalAssistanceModel EducationalAssistanceModel { get; set; }//P030 Chitra 03.06.2020
        public List<EducationalAssistanceModel> _Education { get; set; }//P030 Chitra 03.06.2020
        public TicketRefundModel TicketRefundModel { get; set; }  //P003-Ticket Refund (Chitra)16.06.2020
        public BankGuaranteeModel BankGuaranteeModel { get; set; }  //SAS01-Bank Guarantee (Chitra)25.06.2020
        public ExpenseReportModel ExpenseReport { get; set; } //P045 Chitra 08.07.2020
        public TrainingCertificateModel TrainingCertificateModel { get; set; }  //T007 --Terrin 2-7-2020
        public BusinessInternational BusinessInternational { get; set; } //28-05-2020
        public SitevisitInternational sitevisitinternational { get; set; }
        public IList<SiteFindingsList> siteFindingList { get; set; }//anzeem on 03-06-2020
        public IList<ProblemTypeList> problemTypeList { get; set; }//anzeem on 03-06-2020

        public TrainingFolderModel TrainingFolderModel { get; set; }
        public InHouseTrainingModel InhouseTraining { get; set; } // T004 George 07-01-20
        public List<InHouseTrainingCourseModel> _CourseList { get; set; } // T004 George 07-01-20

        public ExternalTrainingModel ExternalTraining { get; set; } // T001-External Training(george)13-07-2020
        public List<ExternalTrainingDetailModel> _TrainingDetails { get; set; } // T001-External Training(george)13-07-2020


        public string taroleid { get; set; }//28-05-2020
        public bool taisprocessor { get; set; }//28-05-2020
        public bool ta_can_edit { get; set; }
    }
    public class FileListPrint //Multiple File Attachement for Print 16-03-2020
    {
        public int filebatch { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
    }

    public class SubmitRequest
    {
        public string my_id { get; set; }
        public string request_id { get; set; }
        public long request_table_id { get; set; }
        public int escalation_no { get; set; }
        public string escalated_role { get; set; }
        public string escalated_to { get; set; }
        public string reason { get; set; }
        public string my_role { get; set; }
        public string forward_emp_id { get; set; }
        public long my_country_id { get; set; }
        public string delegation_band { get; set; }
        public string distribution_id { get; set; }
        public string button_code { get; set; }
        public long process_id { get; set; }
        //Basheer for P015 to add level to check if approver needed to select from dropdownlist or not
        public int level { get; set; }
        public long bl_controller { get; set; }
        public string next_processor_role { get; set; }
        public string next_process { get; set; }
        public string wftype { get; set; } //Basheer on 04-02-2020 to set wftype for submitting
        public string status { get; set; } //Basheer on 04-02-2020 to check status for changing cancel function
        public string eForwardUrl { get; set; }// 12-02-2020 ARCHANA FOR EFORWARD URL IN JQUERY
        public string distributionUrl { get; set; }//12-02-2020 ARCHANA FOR DISTRIBUTION  URL IN JQUERY 
        public string reqemployeeid { get; set; } //Basheer on 05-03-2020 
        public string request_details { get; set; }
        public string document_price { get; set; }

        public bool is_hr_department { get; set; }
        public string emp_local_id { get; set; }

        public string my_Process_type { get; set; }// what i wants to do in this request
        public TicketRefundModel TicketRefundModel { get; set; } //chitra V on 26.06.2020
        public string endof_service { get; set; }//chitra V on 30.06.2020 (P052)
        public string remarks { get; set; }

        public bool is_compliance { get; set; }

        public string compliance_approval_date { get; set; }

        public string Last_dayof_work { get; set; }

        public string Return_to_duty { get; set; }

        public bool is_workflow_delegated { get; set; }

        public string justification_provided { get; set; }

        public string address_during_absence { get; set; }

        public string Contact_number { get; set; }

        public int Child_Birth_Leave { get; set; }

        public int Saturday { get; set; }

        public int Friday { get; set; }

        public int Total { get; set; }
        public bool taisprocessor { get; set; }//07-07-2020
    }

    public class InfrastructureChange
    {
        #region Common
        public string ad_account { get; set; }
        public long req_id { get; set; }// Request table id
        public string req_id_only { get; set; }// Request  id
        public string my_role { get; set; }
        public string approver_id { get; set; }// with application  id
        public string my_Process_type { get; set; }// what i wants to do in this request
        public string request_id { get; set; }
        public string emp_name { get; set; }
        public string global_id { get; set; }
        public string local_id { get; set; }
        public string company { get; set; }
        public string job_tittle { get; set; }
        public string department { get; set; }
        public string business_line { get; set; }
        public string cost_center { get; set; }
        public string mobile_phone { get; set; }
        public string telephone { get; set; }
        public string application_id { get; set; }
        public string wf_type { get; set; }
        public string domain { get; set; }
        public string application { get; set; }
        public string service_required { get; set; }
        public string title { get; set; }
        public string my_button { get; set; }
        public bool can_edit { get; set; }
        public string cheque_date_string { get; set; }
        public bool canEscalate { get; set; }
        public int escalation_No { get; set; }
        public bool is_hold { get; set; }
        public bool is_first_approver { get; set; }
        public string can_distribute { get; set; }
        public string my_role_code { get; set; }
        public int level { get; set; }
        public string my_process_code { get; set; }
        public bool haveProfile { get; set; }
        public string request_profile { get; set; }
        public long template_id { get; set; }
        public string checkstatus { get; set; }
        #endregion
        public string change_summary { get; set; }
        public string detailed_description { get; set; }
        public int clarification { get; set; }
        public string proposed_plan { get; set; }
        public string business_impact { get; set; }
        public string file_path { get; set; }
        public string fallback_options { get; set; }
        public string positive_risk { get; set; }
        public string negative_risk { get; set; }
        // Service Owner entering details
        public string support_staff { get; set; }
        public DateTime expected_completion { get; set; }
        public string expected_completion_string { get; set; }
        public int implement_to { get; set; }
        public string remark { get; set; }
        public string service_engineers_code { get; set; }
        public string service_engineer_name { get; set; }
        public long country_id { get; set; }
        public string next_processor_role { get; set; }
        public string next_process { get; set; }
        //Service engineer entering details
        public int q_test_result { get; set; }
        public string q_eng_remark { get; set; }
        public string q_eng_file_path { get; set; }
        public int p_test_result { get; set; }
        public string p_eng_remark { get; set; }
        public string p_eng_file_path { get; set; }
        //Service owner post details 
        public int imp_result { get; set; }
        public string post_remark { get; set; }
        //Basheer on 27-01-2020  to check the view & buttons of page
        public string dataview_id { get; set; }// Request  id
    }

    public class SpecialSubmitRequest
    {
        public string my_id { get; set; }
        public string request_id { get; set; }
        public long request_table_id { get; set; }
        public string reason { get; set; }
        public string my_role { get; set; }
        public string my_role_code { get; set; }
        public string forward_emp_id { get; set; }
        public string my_country_id { get; set; }
        public string delegation_band { get; set; }
        public string distribution_id { get; set; }
        public int level { get; set; }
        public string service_engineer_code { get; set; }
        public string next_processor_role { get; set; }
        public string next_process { get; set; }
    }

    public class HRPaymentrequest
    {
        #region Common
        public long req_id { get; set; }// Request table id
        public string req_id_only { get; set; }// Request  id
        public string my_role { get; set; }
        public string approver_id { get; set; }// with application  id
        public string my_Process_type { get; set; }// what i wants to do in this request
        public string request_id { get; set; }
        public string emp_name { get; set; }
        public string global_id { get; set; }
        public string local_id { get; set; }
        public string company { get; set; }
        public string job_tittle { get; set; }
        public string department { get; set; }
        public string business_line { get; set; }
        public string cost_center { get; set; }
        public string mobile_phone { get; set; }
        public string telephone { get; set; }
        public string application_id { get; set; }
        public string wf_type { get; set; }
        public string domain { get; set; }
        public string application { get; set; }
        public string service_required { get; set; }
        public string title { get; set; }
        public string my_button { get; set; }
        public bool can_edit { get; set; }
        public string cheque_date_string { get; set; }
        public bool canEscalate { get; set; }
        public int escalation_No { get; set; }
        public bool is_hold { get; set; }
        public bool is_first_approver { get; set; }
        public string can_distribute { get; set; }
        public string my_role_code { get; set; }
        public int level { get; set; }
        public string my_process_code { get; set; }
        public bool haveProfile { get; set; }
        public string request_profile { get; set; }
        public string country_code { get; set; }
        public string emp_local_id { get; set; }
        public long template_id { get; set; }
        public string location { get; set; }// 12-02-2020 Archana 
        public string extension { get; set; }// 12-02-2020 Archana 
        public string date_created { get; set; }//12-02-2020 Archana


        //-----Print 07/03/2020 by Nimmi Mohan------//
        public string my_id { get; set; }
        public string WF_ID { get; set; }
        public long request_table_id { get; set; }
        public long my_country_id { get; set; }
        public string button_code { get; set; }
        public string application_code { get; set; }
        public string Currency { get; set; }  //13-03-2020 Nimmi







        //------------end-----------//






        #endregion

        public string ad_account { get; set; }
        public long table_Id { get; set; }
        public string RequestId { get; set; }
        public string PaymentMode { get; set; }
        public decimal Amount_SAR { get; set; }
        public string Account_No { get; set; }
        public string PurposeText { get; set; }
        public string supplier { get; set; }
        public string Payable_To { get; set; }
        public string File_Attachment { get; set; }
        public string Remark { get; set; }
        public string From_BankName { get; set; }
        public string From_Address { get; set; }
        public string From_Account_No { get; set; }
        public string To_Benificiary { get; set; }
        public string To_BankName { get; set; }
        public string To_Address { get; set; }
        public string To_Account_No { get; set; }
        public string To_IBAN { get; set; }
        public DateTime Cheque_Date { get; set; }
        //p099
        public long carloanrequest_number { get; set; } //  P099 NIMMI  05-05-2020
        //----------Non HR-----------------
        public string contract_local_no { get; set; }
        public string back_invoice_no { get; set; }
        public string project { get; set; }
        public string year_booked { get; set; }
        public string customer { get; set; }
        public string dataview_id { get; set; }// Request  id
        public string checkstatus { get; set; } //Basheer on 04-02-2020 to checkstatus for backtoinitiator or not
        public string currenctType { get; set; } // 28-02-2020 ARCHANA K V SRISHTI 

        public string filedata { get; set; } //Basheer on 18-03-2020 
        public List<FileList> _FileList { get; set; }  //Basheer on 18-03-2020 

        public string creator_id { get; set; } //Basheer on 24-03-2020 
    }


    public class ApproveLogDetails
    {
        public string type { get; set; }
        public string req_id { get; set; }
    }

    public class BusinesInternational
    {
        public string ad_account { get; set; }
        public string wf_id { get; set; }
        public long application_id { get; set; }
        public string emp_local_id { get; set; }
        public string creator_id { get; set; }
        public string request_id { get; set; }
        public string requestId { get; set; }
        public string place_to_visit { get; set; }
        public string reason { get; set; }
        public string remark_one { get; set; }
        public int is_complaince_approval_required { get; set; }
        public DateTime compliance_approval_date { get; set; }
        public DateTime last_day_of_work { get; set; }
        public DateTime return_to_duty { get; set; }        
        public long wf_template_id { get; set; }
        public int workflow_delegated { get; set; }
        public string justification_no_delegation { get; set; }
        public int possible_video_conference { get; set; }
        public string justification_for_no_video_conference { get; set; }
        public string address_during_absence { get; set; }
        public string telephone { get; set; }
        public string mode_of_travel { get; set; }
        public long location_id { get; set; }
        public int required_exit_visa { get; set; }
        public string type_of_exit_visa { get; set; }
        public string travel_visa_charged_to { get; set; }
        public int required_foreign_visa { get; set; }
        public string foreign_visa_countries { get; set; }
        public string foreign_visa_quantity { get; set; }

        public int required_travel_insurance { get; set; }
        public string travel_insurance_countries { get; set; }

        public string travel_insurance_quantity { get; set; }
        public int required_rent_car { get; set; }
        public string rent_car_charged_to { get; set; }
        public string rent_car_project_no { get; set; }
        public string car_type { get; set; }
        public string rent_car_picked_up_at { get; set; }
        public DateTime rent_car_pick_up_date { get; set; }
        public string rent_car_pick_up_time { get; set; }
        public string rent_car_payment_type { get; set; }
        public DateTime rent_car_return_date { get; set; }
        public string rent_car_return_time { get; set; }
        public string rent_car_remark { get; set; }

        public int required_hotel_booking { get; set; }

        public string hotel_booking_charged_to { get; set; }
        public string hptel_booking_project_no { get; set; }
        public string hotel_name { get; set; }
        public string hotel_location { get; set; }

        public string type_of_room { get; set; }
        public string room_preferences { get; set; }
        public int number_of_rooms { get; set; }
        public string hotel_booking_payment_mode { get; set; }
        public DateTime hotel_booking_check_in_date { get; set; }
        public string hotel_check_in_time { get; set; }
        public DateTime hotel_booking_check_out_date { get; set; }
        public string hotel_check_out_time { get; set; }
        public string hotel_booking_remark { get; set; }
        public DateTime departure_date { get; set; }
        public string departure_flight_number { get; set; }
        public DateTime return_date { get; set; }
        public string return_flight_number { get; set; }
        public string travel_routing { get; set; }
        public string type_of_ticket { get; set; }
        public string note { get; set; }
        public string file_path { get; set; }
        public int cash_advance { get; set; }
        public int amx_holder { get; set; }
        public string salary_advance { get; set; }
        public string bank_account { get; set; }
        public string ticket_cost { get; set; }
        public string iban { get; set; }
        public string hotel_cost { get; set; }
        public string daily_allowance { get; set; }
        public string other_expenses { get; set; }
        public string travel_advance_remark { get; set; }
        public string travel_advance_total { get; set; }
        public List<Dependentsinfo> _dependInfo { get; set; }
        public string employee_ticket_number { get; set; }
        public DateTime date_of_issue { get; set; }
        public string ticket_price { get; set; }
        public List<TravelAgencyInfo> travel_agency_info { get; set; }     
        public string revalidation_charge { get; set; }
        public string total_ticket_price { get; set; }
        public int over_all_ticket_status { get; set; }
        public int busines_days { get; set; }
        public int fiday { get; set; }
        public int saturday { get; set; }
        public int total_days { get; set; }
        public string dependents_name { get; set; }
        public string dependents_relation { get; set; }
        public string dependents_age { get; set; }
        public string dependents_visa_type { get; set; }
        public string dependents_ta_type { get; set; }
        public string dependents_remarks { get; set; }
        public List<FileList> _FileList { get; set; }  //Basheer on 28-05-2020     
        public string depent_ticket_number { get; set; }
        public string depent_issue_date { get; set; }
        public string depent_ticket_price { get; set; }



        public string visa_with { get; set; }//Basheer on 26-06-2020 
        public string visa_duration { get; set; }//Basheer on 26-06-2020 
        public string employee_date_of_issue { get; set; }//Basheer on 26-06-2020 
        public string employee_ticket_price { get; set; }//Basheer on 26-06-2020 

        public string Car_Return_date_date_string { get; set; } //03-07-2020

    }
    public class Dependentsinfo
    {
        public string ad_account { get; set; }
        public string name { get; set; }
        public string relation_ship { get; set; }
        public string age { get; set; }
        public string visa_type { get; set; }
        public string ta_type { get; set; }
        public string remarks { get; set; }
    }
    public class TravelAgencyInfo
    {
        public string ad_account { get; set; }
        public string depent_ticket_number { get; set; }
        public DateTime issue_date { get; set; }
        public string ticket_price { get; set; }
    }

    //P007-Vacation(Preema)
    public class VacationModel
    {
        public long wfcreatetype { get; set; }
        public string mod_requestid { get; set; }
        public Location abb_locations { get; set; }
        public string location_name { get; set; }
        public string ad_account { get; set; }
        public string wf_id { get; set; }
        public long application_id { get; set; }
        public string emp_local_id { get; set; }
        public string creator_id { get; set; }
        public string request_id { get; set; }
        public string requestId { get; set; }
        public string place_to_visit { get; set; }
        public string reason { get; set; }
        public string remark_one { get; set; }
        public int is_complaince_approval_required { get; set; }
        public string str_complaince_approval_required { get; set; }
        public DateTime compliance_approval_date { get; set; }
        public DateTime last_day_of_work { get; set; }
        public DateTime return_to_duty { get; set; }

        public string str_compliance_approval_date { get; set; }
        public string str_last_day_of_work { get; set; }
        public string str_return_to_duty { get; set; }

        public long wf_template_id { get; set; }
        public int workflow_delegated { get; set; }
        public string str_workflow_delegated { get; set; }
        public string justification_no_delegation { get; set; }       
        public string address_during_absence { get; set; }
        public string telephone { get; set; }
        public string mode_of_travel { get; set; }
        public long location_id { get; set; }
        public int required_exit_visa { get; set; }
        public string type_of_exit_visa { get; set; }
        public string travel_visa_charged_to { get; set; }
        public int required_foreign_visa { get; set; }
        public string str_required_foreign_visa { get; set; }
        public string foreign_visa_countries { get; set; }
        public string foreign_visa_quantity { get; set; }
        public int required_travel_insurance { get; set; }
        public string str_required_travel_insurance { get; set; }
        public string travel_insurance_countries { get; set; }
        public string travel_insurance_quantity { get; set; }
        public int required_rent_car { get; set; }
        public string str_required_rent_car { get; set; }
        public string rent_car_charged_to { get; set; }
        public string rent_car_project_no { get; set; }
        public string car_type { get; set; }
        public string rent_car_picked_up_at { get; set; }
        public DateTime rent_car_pick_up_date { get; set; }
        public string str_rent_car_pick_up_date { get; set; }
        public string rent_car_pick_up_time { get; set; }
        public string rent_car_payment_type { get; set; }
        public DateTime rent_car_return_date { get; set; }
        public string str_rent_car_return_date { get; set; }
        public string rent_car_return_time { get; set; }
        public string rent_car_remark { get; set; }
        public int required_hotel_booking { get; set; }
        public string str_required_hotel_booking { get; set; }
        public string hotel_booking_charged_to { get; set; }
        public string hotel_booking_project_no { get; set; }
        public string hotel_name { get; set; }
        public string hotel_location { get; set; }
        public string type_of_room { get; set; }
        public string room_preferences { get; set; }
        public int number_of_rooms { get; set; }
        public string hotel_booking_payment_mode { get; set; }
        public DateTime hotel_booking_check_in_date { get; set; }
        public string str_hotel_booking_check_in_date { get; set; }
        public string hotel_check_in_time { get; set; }
        public DateTime hotel_booking_check_out_date { get; set; }
        public string str_hotel_booking_check_out_date { get; set; }
        public string hotel_check_out_time { get; set; }
        public string hotel_booking_remark { get; set; }
        public DateTime departure_date { get; set; }
        public string str_departure_date { get; set; }
        public string departure_flight_number { get; set; }
        public DateTime return_date { get; set; }
        public string str_return_date { get; set; }
        public string return_flight_number { get; set; }
        public string travel_routing { get; set; }
        public string type_of_ticket { get; set; }
        public string note { get; set; }
        public string file_path { get; set; } 
        public List<FileList> _FileList { get; set; }     
        public int cash_advance { get; set; }
        public int amx_holder { get; set; }
        public string str_cash_advance { get; set; }
        public string str_amx_holder { get; set; }
        public string salary_advance { get; set; }
        public string bank_account { get; set; }
        public string ticket_cost { get; set; }
        public string iban { get; set; }
        public string hotel_cost { get; set; }
        public string daily_allowance { get; set; }
        public string other_expenses { get; set; }
        public string travel_advance_remark { get; set; }
        public string travel_advance_total { get; set; }      
        public string dependents_name { get; set; }
        public string dependents_relation { get; set; }
        public string dependents_age { get; set; }
        public string dependents_visa_type { get; set; }
        public string dependents_ta_type { get; set; }
        public string dependents_remarks { get; set; }      
        public string employee_ticket_number { get; set; }
        public string employee_date_of_issue { get; set; }
        public string employee_ticket_price { get; set; }
        public string depent_ticket_number { get; set; }
        public string depent_issue_date { get; set; }
        public string depent_ticket_price { get; set; }
        public string revalidation_charge { get; set; }
        public string total_ticket_price { get; set; }
        public int over_all_ticket_status { get; set; }
        public string str_over_all_ticket_status { get; set; }
        public string ticket_charged_to { get; set; }
        public string from_period { get; set; }
        public string to_period { get; set; }

        public DateTime from_period_date { get; set; }
        public DateTime to_period_date { get; set; }
        public string str_from_period_date { get; set; }
        public string str_to_period_date { get; set; }
        public int Vacation { get; set; }
        public int Leave_WO_Pay { get; set; }
        public int Holidays { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public string Total_No_of_Days { get; set; }      
        public string Visa_Amount_Claim { get; set; }
        public string Taxi_Fare_Claim { get; set; }
        public string HR_Remarks_Visa { get; set; }
        public string HR_Remarks_Taxi { get; set; }
        public string Visa_Duration { get; set; }
        public string Visa_With { get; set; }
        public List<TA_DependentsInfo> _dependentsInfo { get; set; }
        public List<TA_TravelAgencyInfo> _travelAgencyInfo { get; set; }

        public List<FileListPrint> _FileListPrint { get; set; }
    }
    public class BusinessInternational
    {
        #region Common
        public long req_id { get; set; }// Request table id
        public string req_id_only { get; set; }// Request  id
        public string my_role { get; set; }
        public string approver_id { get; set; }// with application  id
        public string my_Process_type { get; set; }// what i wants to do in this request
        public string request_id { get; set; }
        public string emp_name { get; set; }
        public string global_id { get; set; }
        public string local_id { get; set; }
        public string company { get; set; }
        public string job_tittle { get; set; }
        public string department { get; set; }
        public string business_line { get; set; }
        public string cost_center { get; set; }
        public string mobile_phone { get; set; }
        public string telephone { get; set; }
        public string application_id { get; set; }
        public string wf_type { get; set; }
        public string domain { get; set; }
        public string application { get; set; }
        public string service_required { get; set; }
        public string title { get; set; }
        public string my_button { get; set; }
        public bool can_edit { get; set; }
        public string cheque_date_string { get; set; }
        public bool canEscalate { get; set; }
        public int escalation_No { get; set; }
        public bool is_hold { get; set; }
        public bool is_first_approver { get; set; }
        public string can_distribute { get; set; }
        public string my_role_code { get; set; }
        public int level { get; set; }
        public string my_process_code { get; set; }
        public bool haveProfile { get; set; }
        public string request_profile { get; set; }
        public long template_id { get; set; }
        public string country_code { get; set; }
        public string ad_account { get; set; }
        public string checkstatus { get; set; }

        #endregion

        public string id { get; set; }
        public string payment_type { get; set; }
        public long abb_locations { get; set; }
        public string hotel_location { get; set; }
        public string Place_Visit { get; set; }
        public string Reasons { get; set; }
        public string Remark { get; set; }
        public int Is_Compliance_Approval_Required { get; set; }
        public DateTime Compliance_Approval_Date { get; set; }
        public DateTime Last_Day_Of_Work { get; set; }
        public DateTime Return_To_Duty { get; set; }
        public int Is_WorkFlow_delegated { get; set; }
        public string Justification_Not_Delegated { get; set; }
        public int IsPossible_Video_Conference { get; set; }
        public string Justification_No_Video_Conference { get; set; }
        public string Address_During_Absence { get; set; }
        public string Telephone_No { get; set; }
        public string Mode_Of_Travel { get; set; }
        public long Location_Id { get; set; }
        public int Required_Ext_or_Reentry_Visa { get; set; }
        public string Type_of_Required_Ext_or_Reentry_Visa { get; set; }
        public string Visa_Charged_to { get; set; }
        public int Required_Foreign_Visa { get; set; }
        public string Foreign_Visa_Countries { get; set; }
        public string Foreign_Visa_Quantity { get; set; }
        public int Requied_Travel_Insurance { get; set; }
        public string Travel_Insurance_Countries { get; set; }
        public string Travel_Insurance_Quantity { get; set; }
        public int Required_RentCar { get; set; }
        public string RentCar_Charged_to { get; set; }
        public string RentCar_ProjectNo { get; set; }
        public string Car_Type { get; set; }
        public string Car_PickUp_at { get; set; }
        public DateTime Car_PickUp_date { get; set; }
        public string Car_PickUp_Time { get; set; }
        public string Car_Payment_Type { get; set; }
        public DateTime Car_Return_date { get; set; }
        public string Car_Return_Time { get; set; }
        public string Car_Remark { get; set; }
        public int Required_Hotel_Booking { get; set; }
        public string HotelBooking_Charged_to { get; set; }
        public string Hotel_ProjectNo { get; set; }
        public string Hotel_Name { get; set; }
        public string Type_Of_rooms { get; set; }
        //public string Hotel_Location { get; set; } //26-06-2020
        public string Room_Preference { get; set; }
        public string Number_Of_Rooms { get; set; }
        public string HotelBooking_Payment_Type { get; set; }
        public DateTime Hote_Checking_Date { get; set; }
        public string Hotel_Check_In_Time { get; set; }
        public DateTime Hotel_Check_Out_Date { get; set; }
        public string Hotel_Check_Out_Time { get; set; }
        public string Hotel_Remark { get; set; }
        public DateTime Departure_Date { get; set; }
        public string Departure_Flight_Number { get; set; }
        public DateTime Return_Date { get; set; }
        public string Return_Flight_Number { get; set; }
        public string Travel_Routing { get; set; }
        public string Note { get; set; }
        public int Cash_Advance { get; set; }
        public int Amex_Holder { get; set; }
        public string Salary_Advance { get; set; }
        public string Bank_account { get; set; }
        public string Ticket_Cost { get; set; }
        public string IBAN { get; set; }
        public string Hotel_Cost { get; set; }
        public string Daily_allowance { get; set; }
        public string Other_Expenses { get; set; }
        public string Advance_Remark { get; set; }
        public string Travel_Advance_Total { get; set; }
        public string Revalidation_Charge { get; set; }
        public string Total_Ticket_Price { get; set; }
        public int Ticket_Status { get; set; }
        public int Business_Days { get; set; }
        public int Friday { get; set; }
        public int Saturday { get; set; }
        public int Total { get; set; }
        public int HR_Administration_Service { get; set; }
        public int Travel_Agency_Service { get; set; }
        public int Shared_Accounting_Service { get; set; }
        public int Govt_Relation_Officer { get; set; }
        public List<TA_DependentsInfo> _dependentsInfo { get; set; }
        public List<TA_TravelAgencyInfo> _travelAgencyInfo { get; set; }
        public string emp_ticket_number { get; set; }
        public string emp_issue_date { get; set; }
        public string emp_ticket_price { get; set; }
        public long process_table_id { get; set; }
        //Basheer on 27-01-2020  to check the view & buttons of page
        public string dataview_id { get; set; }// Request  id

        public string compliance_approval_date_string { get; set; } //28-05-2020
        public string Last_Day_Of_Work_date_string { get; set; }//28-05-2020
        public string Return_To_Duty_date_string { get; set; }//28-05-2020
        public string Car_PickUp_date_date_string { get; set; } //28-05-2020
        public string Hote_Checking_Date_date_string { get; set; }//28-05-2020
        public string Hotel_Check_Out_Date_date_string { get; set; }//28-05-2020
        public string Departure_Date_date_string { get; set; } //28-05-2020
        public string Return_Date_date_string { get; set; }//28-05-2020

        public string type_of_ticket { get; set; }//26-06-2020

        public string Visa_with { get; set; }//26-06-2020
        public string Visa_duration { get; set; }//26-06-2020
        public string str_complaince_approval_required { get; set; } //03-07-2020
        public string str_is_video_conference_required { get; set; } //03-07-2020
        public string str_workflow_delegated { get; set; }//03-07-2020
        public string str_required_foreign_visa { get; set; }//03-07-2020
        public string str_required_travel_insurance { get; set; }//03-07-2020
        public string str_required_rent_car { get; set; }//03-07-2020
        public string str_required_hotel_booking { get; set; }//03-07-2020
        public string str_over_all_ticket_status { get; set; }//03-07-2020
        public string str_cash_advance { get; set; }//03-07-2020
        public string str_amx_holder { get; set; }//03-07-2020
        public string location_name { get; set; }//03-07-2020
        public string ticket_charged_to { get; set; }//03-07-2020
        public string Ticket_Charged_To { get; set; }//03-07-2020

        public List<FileListPrint> _FileListPrint { get; set; }//03-07-2020

        public string Car_Return_date_date_string { get; set; } //03-07-2020

     

    }
    public class EducationalAssistanceModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string Employee_Group { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }
        public string Child_Name { get; set; }

        public decimal Grand_Total { get; set; }
        public decimal Total_Entitlement { get; set; }
        public decimal Amount_Paid { get; set; }
        public decimal Amount_Approved { get; set; }
        public decimal Paid_Payroll { get; set; }
        public decimal School_Fees { get; set; }

        public decimal Transport_Fees { get; set; }


        public decimal Others { get; set; }


        public decimal Foreign_Currency { get; set; }


        public decimal Exchange_Rate { get; set; }

        public string Location { get; set; }

        public long Location_Id { get; set; }

        public long Edu_Id { get; set; }
        public string Remarks { get; set; }


        public DateTime Birth_Date { get; set; }


        public DateTime From_Date { get; set; }


        public DateTime To_Date { get; set; }
        public string Attachment_Filepath { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
        public string Date_birth_string_date { get; set; }
        public string From_Date_string_date { get; set; }
        public string To_Date_string_date { get; set; }

    }

    //Author:Chitra Srishti on 16.06.2020 
    //Author:Chitra Srishti on 16.06.2020 
    public class TicketRefundModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string EmployeeID { get; set; }

        public string TA_Request_No { get; set; }
        public string TicketRouting { get; set; }
        public string TicketNumber { get; set; }
        public string RequestDetails { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
    }
    //Author:Chitra Srishti on 24.06.2020 
    public class BankGuaranteeModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }


        public long Company_Id { get; set; }

        public string Company_Name { get; set; }
        public int Guarantee_Type { get; set; }
        public string BenfName { get; set; }
        public string BenfAdress1 { get; set; }
        public string BenfAdress2 { get; set; }
        public string BenfTelephone { get; set; }
        public string BenfFax { get; set; }
        public string Currency { get; set; }

        public decimal CurrencyValue { get; set; }
        public string Amount { get; set; }
        public int ContractPercent { get; set; }
        public decimal ContractTotal { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public string From_Date_string_date { get; set; }
        public string To_Date_string_date { get; set; }
        public string Description { get; set; }
        public string CustPONo { get; set; }
        public string ABBQutnNo { get; set; }
        public string WBSNo { get; set; }
        public long BL_Id { get; set; }
        public string BL_Code { get; set; }
        public long PG_Id { get; set; }
        public string PGNo { get; set; }
        public string CostCenter { get; set; }
        public string Branch { get; set; }
        public string CollectorName { get; set; }
        public string Remarks { get; set; }
        public string GuaranteeNo { get; set; }
        public string Bank { get; set; }
        public string AccountRemarks { get; set; }
        public string Comments { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
    }

    // Terrin on 30/6/2020 -----T006
    public class TrainingFolderModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string quantity { get; set; }
        public DateTime Date_training { get; set; }
        public string Date_training_string { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }
        public string Attachment_Filepath { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
    }

    // Terrin on 2/7/2020 -----T007
    public class TrainingCertificateModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string Titlecourse { get; set; }
        public DateTime Course_period_from { get; set; }

        public DateTime Course_period_to { get; set; }
        public string Course_period_from_string { get; set; }
        public string Course_period_to_string { get; set; }
        public string Location { get; set; }
        public string Clientname { get; set; }
        public string Noof_particants { get; set; }
        public string Noof_trainer { get; set; }
        public string[] Nameof_participants { get; set; }
        public string nameofparticipants { get; set; }

        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }
        public string Attachment_Filepath { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
    }
    public class TA_DependentsInfo
    {
        public long id { get; set; }
        public string name { get; set; }
        public string relation_ship { get; set; }
        public string age { get; set; }
        public string visa_type { get; set; }
        public string ta_type { get; set; }
        public string remark { get; set; }
    }
    public class TA_TravelAgencyInfo
    {
        public string ticket_number { get; set; }
        public DateTime issue_date { get; set; }
        public string ticket_price { get; set; }
    }

    // A009-Arrangement of Employee Drop(Preema)
    public class EmployeeDropModel
    {
        public string request_id { get; set; }
        public string wf_id { get; set; }

        public string emp_local_id { get; set; }

        public string filedata { get; set; }
        public List<FileList> _FileList { get; set; }
        public long wf_template_id { get; set; }

        public string creator_id { get; set; }
        public long application_id { get; set; }
        // 02/07/2020 ALENA SICS
        public string attachment_filepath { get; set; }//END----
        public long cost_center { get; set; }
        public string employee_name { get; set; }
        public string drop_at { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string remarks { get; set; }


        public string quantity { get; set; }
        public long driver_id { get; set; }
        public string driver_name { get; set; }
        public string mobile_no { get; set; }
        public string emp_id { get; set; }
        public string car_model { get; set; }

    }

    // A008-Arrangement of Employee PickUp(Preema)
    public class EmployeePickupModel
    {
        public string request_id { get; set; }

        public string wf_id { get; set; }

        public string emp_local_id { get; set; }

        public string filedata { get; set; }
        public List<FileList> _FileList { get; set; }
        public long wf_template_id { get; set; }

        public string creator_id { get; set; }
        public long application_id { get; set; }
        // 02/07/2020 ALENA SICS
        public string attachment_filepath { get; set; }//END----
        public Int32 cost_center { get; set; }
        public string employee_name { get; set; }
        public string pickup_at { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string remarks { get; set; }

        public string quantity { get; set; }
        public string driver_name { get; set; }
        public string mobile_no { get; set; }
        public string emp_id { get; set; }
        public string car_model { get; set; }
    }

    // P049-Other Personnel Services(Preema)
    public class OtherPersonnelServicesModel
    {
        public string request_id { get; set; }
        public string wf_id { get; set; }
        public string emp_local_id { get; set; }
        public string local_id { get; set; }
        public string req_id_only { get; set; }// Request  id
        public string my_role { get; set; }

        public string filedata { get; set; }
        public List<FileList> _FileList { get; set; }
        public long wf_template_id { get; set; }

        public string creator_id { get; set; }
        public long application_id { get; set; }

        public string request_details { get; set; }
        public string document_price { get; set; }

        public bool is_hr_department { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }

        public string status { get; set; }

    }
    // P061-ESAP Contribution(Preema)
    public class ESAP_ContributionModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string For_the_Period_of { get; set; }
        public string Remarks { get; set; }
        public string Company_Name { get; set; }
        public string Payroll_Code { get; set; }
        public decimal Total_Amount_in_USD { get; set; }
        public decimal Grand_Total { get; set; }
        public string Note { get; set; }

        public string[] strCompany { get; set; }

        public string[] strPayrollCode { get; set; }

        public string[] strTotal { get; set; }

        public List<string> lstCompanyId = new List<string>();
        public List<string> lstCompanyName = new List<string>();
        public List<string> lstPayrollCode = new List<string>();
        public List<string> lstTotal = new List<string>();

        public List<FileList> _FileList { get; set; }

        public List<FileListPrint> _FileListPrint { get; set; }

        public string status { get; set; }
    }

    //P062-Retirement Contribution(Preema)
    public class RetirementContributionModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string For_the_Period_of { get; set; }
        public string Remarks { get; set; }
        public string Company_Name { get; set; }
        public string Payroll_Code { get; set; }
        public Decimal Total_Amount_in_USD { get; set; }
        public Decimal Grand_Total { get; set; }
        public string Bank_Details { get; set; }

        public string[] strCompany { get; set; }

        public string[] strPayrollCode { get; set; }

        public string[] strTotal { get; set; }

        public List<string> lstCompanyId = new List<string>();
        public List<string> lstCompanyName = new List<string>();
        public List<string> lstPayrollCode = new List<string>();
        public List<string> lstTotal = new List<string>();

        public List<FileList> _FileList { get; set; }

        public List<FileListPrint> _FileListPrint { get; set; }

        public string status { get; set; }
    }

    //P024-Bank Loan Request(Preema)
    public class BankLoanRequestModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }
        public string Bank_Name { get; set; }
        public string Account_No { get; set; }
        public decimal Loan_Amount { get; set; }
        public string Purpose { get; set; }
        public string Nationality { get; set; }
        public string Saudi_Id { get; set; }
        public string Date_of_Hire { get; set; }
        public string End_of_Service_Benefit { get; set; }
        public string As_of_Date { get; set; }

        public List<FileListPrint> _FileListPrint { get; set; }

    }

    //P016-Internal Transfer(Preema)
    public class InternalTransferModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string WF_ID { get; set; }

        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }

        public long country_id { get; set; }

        public long business_id { get; set; }

        public string Transfer_Type { get; set; }

        public string Releasing_Manager { get; set; }
        public string Releasing_Manager_Id { get; set; }

        public string Receiving_Manager { get; set; }
        public string Receiving_Manager_Id { get; set; }

        public string Transfer_From { get; set; }
        public string Transfer_To { get; set; }
        public string Effective_Date { get; set; }

        public string From_Company { get; set; }
        public long From_Company_id { get; set; }

        public string To_Company { get; set; }
        public long To_Company_Id { get; set; }

        public string From_Business_Line { get; set; }
        public long From_Business_Line_id { get; set; }

        public string To_Business_Line { get; set; }
        public long To_Business_Line_Id { get; set; }

        public string From_Product_Group { get; set; }
        public long From_Product_Group_id { get; set; }

        public string To_Product_Group { get; set; }
        public long To_Product_Group_Id { get; set; }

        public string From_Department { get; set; }
        public long From_Department_id { get; set; }

        public string To_Department { get; set; }
        public long To_Department_Id { get; set; }

        public string From_Position { get; set; }
        public string From_Position_id { get; set; }

        public string To_Position { get; set; }
        public string To_Position_Id { get; set; }

        public string From_Global_Grade { get; set; }
        public string From_Global_Grade_id { get; set; }

        public string To_Global_Grade { get; set; }
        public string To_Global_Grade_Id { get; set; }

        public string From_Local_Grade { get; set; }
        public string From_Local_Grade_Id { get; set; }

        public string To_Local_Grade { get; set; }
        public string To_Local_Grade_Id { get; set; }

        public string From_Cost_Center { get; set; }
        public long From_Cost_Center_id { get; set; }

        public string To_Cost_Center { get; set; }
        public long To_Cost_Center_Id { get; set; }

        public string From_status { get; set; }

        public string To_status { get; set; }

        public string From_Notice_Period { get; set; }

        public string To_Notice_Period { get; set; }

        public string From_Location { get; set; }
        public long From_Location_id { get; set; }

        public string To_Location { get; set; }
        public long To_Location_Id { get; set; }

        public string From_Basic_Salary { get; set; }

        public string To_Basic_Salary { get; set; }

        public string From_Annual_Housing { get; set; }

        public string To_Annual_Housing { get; set; }

        public string From_Car_Cost { get; set; }

        public string To_Car_Cost { get; set; }

        public string From_Transport { get; set; }

        public string To_Transport { get; set; }

        public string From_Travel_Allowance { get; set; }

        public string To_Travel_Allowance { get; set; }

        public string From_Mobile_Allowance { get; set; }

        public string To_Mobile_Allowance { get; set; }

        public string Employee_Id { get; set; }

        public List<FileListPrint> _FileListPrint { get; set; }
    }

    //P017-Contract Modification(Preema)
    public class ContractModificationModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string WF_ID { get; set; }

        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string status { get; set; }

        public long country_id { get; set; }

        public long business_id { get; set; }

        public string Contract_Type { get; set; }

        public string Releasing_Manager { get; set; }
        public string Releasing_Manager_Id { get; set; }

        public string Effective_Date { get; set; }

        public string From_Company { get; set; }
        public long From_Company_id { get; set; }

        public string To_Company { get; set; }
        public long To_Company_Id { get; set; }

        public string From_Business_Line { get; set; }
        public long From_Business_Line_id { get; set; }

        public string To_Business_Line { get; set; }
        public long To_Business_Line_Id { get; set; }

        public string From_Product_Group { get; set; }
        public long From_Product_Group_id { get; set; }

        public string To_Product_Group { get; set; }
        public long To_Product_Group_Id { get; set; }

        public string From_Department { get; set; }
        public long From_Department_id { get; set; }

        public string To_Department { get; set; }
        public long To_Department_Id { get; set; }

        public string From_Position { get; set; }
        public string From_Position_id { get; set; }

        public string To_Position { get; set; }
        public string To_Position_Id { get; set; }

        public string From_Global_Grade { get; set; }
        public string From_Global_Grade_id { get; set; }

        public string To_Global_Grade { get; set; }
        public string To_Global_Grade_Id { get; set; }

        public string From_Local_Grade { get; set; }
        public string From_Local_Grade_Id { get; set; }

        public string To_Local_Grade { get; set; }
        public string To_Local_Grade_Id { get; set; }

        public string From_Cost_Center { get; set; }
        public long From_Cost_Center_id { get; set; }

        public string To_Cost_Center { get; set; }
        public long To_Cost_Center_Id { get; set; }

        public string From_status { get; set; }

        public string To_status { get; set; }

        public string From_Notice_Period { get; set; }

        public string To_Notice_Period { get; set; }

        public string From_Location { get; set; }
        public long From_Location_id { get; set; }

        public string To_Location { get; set; }
        public long To_Location_Id { get; set; }

        public string From_Basic_Salary { get; set; }

        public string To_Basic_Salary { get; set; }

        public string From_Annual_Housing { get; set; }

        public string To_Annual_Housing { get; set; }

        public string From_Car_Cost { get; set; }

        public string To_Car_Cost { get; set; }

        public string From_Transport { get; set; }

        public string To_Transport { get; set; }

        public string From_Travel_Allowance { get; set; }

        public string To_Travel_Allowance { get; set; }

        public string From_Mobile_Allowance { get; set; }

        public string To_Mobile_Allowance { get; set; }

        public string Employee_Id { get; set; }

        public List<FileListPrint> _FileListPrint { get; set; }

    }

    //Anzeem 09-06-20
    //public class SiteVisitInternational
    //{
    //    #region Common
    //    public long req_id { get; set; }// Request table id
    //    public string req_id_only { get; set; }// Request  id
    //    public string my_role { get; set; }
    //    public string approver_id { get; set; }// with application  id
    //    public string my_Process_type { get; set; }// what i wants to do in this request
    //    public string request_id { get; set; }
    //    public string emp_name { get; set; }
    //    public string global_id { get; set; }
    //    public string local_id { get; set; }
    //    public string company { get; set; }
    //    public string job_tittle { get; set; }
    //    public string department { get; set; }
    //    public string business_line { get; set; }
    //    public string cost_center { get; set; }
    //    public string mobile_phone { get; set; }
    //    public string telephone { get; set; }
    //    public string application_id { get; set; }
    //    public string wf_type { get; set; }
    //    public string domain { get; set; }
    //    public string application { get; set; }
    //    public string service_required { get; set; }
    //    public string title { get; set; }
    //    public string my_button { get; set; }
    //    public bool can_edit { get; set; }
    //    public string cheque_date_string { get; set; }
    //    public bool canEscalate { get; set; }
    //    public int escalation_No { get; set; }
    //    public bool is_hold { get; set; }
    //    public bool is_first_approver { get; set; }
    //    public string can_distribute { get; set; }
    //    public string my_role_code { get; set; }
    //    public int level { get; set; }
    //    public string my_process_code { get; set; }
    //    public bool haveProfile { get; set; }
    //    public string request_profile { get; set; }
    //    public long template_id { get; set; }
    //    public string country_code { get; set; }
    //    public string ad_account { get; set; }
    //    public string checkstatus { get; set; }

    //    #endregion

    //    public string id { get; set; }
    //    public string payment_type { get; set; }
    //    public Location abb_locations { get; set; }
    //    public string hotel_location { get; set; }
    //    public string Place_Visit { get; set; }
    //    public string customer_problem { get; set; }
    //    public string Remark { get; set; }
    //    public int Is_Compliance_Approval_Required { get; set; }
    //    public DateTime Compliance_Approval_Date { get; set; }
    //    public DateTime Last_Day_Of_Work { get; set; }
    //    public DateTime Return_To_Duty { get; set; }
    //    public int Is_WorkFlow_delegated { get; set; }
    //    public string Justification_Not_Delegated { get; set; }
    //    public int IsPossible_Video_Conference { get; set; }
    //    public string Justification_No_Video_Conference { get; set; }
    //    public string Address_During_Absence { get; set; }
    //    public string Telephone_No { get; set; }
    //    public string Mode_Of_Travel { get; set; }
    //    public long Location_Id { get; set; }
    //    public int Required_Ext_or_Reentry_Visa { get; set; }
    //    public string Type_of_Required_Ext_or_Reentry_Visa { get; set; }
    //    public string Visa_Charged_to { get; set; }
    //    public int Required_Foreign_Visa { get; set; }
    //    public string Foreign_Visa_Countries { get; set; }
    //    public string Foreign_Visa_Quantity { get; set; }
    //    public int Requied_Travel_Insurance { get; set; }
    //    public string Travel_Insurance_Countries { get; set; }
    //    public string Travel_Insurance_Quantity { get; set; }
    //    public int Required_RentCar { get; set; }
    //    public string RentCar_Charged_to { get; set; }
    //    public string RentCar_ProjectNo { get; set; }
    //    public string Car_Type { get; set; }
    //    public string Car_PickUp_at { get; set; }
    //    public DateTime Car_PickUp_date { get; set; }
    //    public string Car_PickUp_Time { get; set; }
    //    public string Car_Payment_Type { get; set; }
    //    public DateTime Car_Return_date { get; set; }
    //    public string Car_Return_Time { get; set; }
    //    public string Car_Remark { get; set; }
    //    public int Required_Hotel_Booking { get; set; }
    //    public string HotelBooking_Charged_to { get; set; }
    //    public string Hotel_ProjectNo { get; set; }
    //    public string Hotel_Name { get; set; }
    //    public string Type_Of_rooms { get; set; }
    //    public string Room_Preference { get; set; }
    //    public string Number_Of_Rooms { get; set; }
    //    public string HotelBooking_Payment_Type { get; set; }
    //    public DateTime Hote_Checking_Date { get; set; }
    //    public string Hotel_Check_In_Time { get; set; }
    //    public DateTime Hotel_Check_Out_Date { get; set; }
    //    public string Hotel_Check_Out_Time { get; set; }
    //    public string Hotel_Remark { get; set; }
    //    public DateTime Departure_Date { get; set; }
    //    public string Departure_Flight_Number { get; set; }
    //    public DateTime Return_Date { get; set; }
    //    public string Return_Flight_Number { get; set; }
    //    public string Travel_Routing { get; set; }
    //    public string Note { get; set; }
    //    public int Cash_Advance { get; set; }
    //    public int Amex_Holder { get; set; }
    //    public string Salary_Advance { get; set; }
    //    public string Bank_account { get; set; }
    //    public string Ticket_Cost { get; set; }
    //    public string IBAN { get; set; }
    //    public string Hotel_Cost { get; set; }
    //    public string Daily_allowance { get; set; }
    //    public string Other_Expenses { get; set; }
    //    public string Advance_Remark { get; set; }
    //    public string Travel_Advance_Total { get; set; }
    //    public string Revalidation_Charge { get; set; }
    //    public string Total_Ticket_Price { get; set; }
    //    public int Ticket_Status { get; set; }
    //    public int Business_Days { get; set; }
    //    public int Friday { get; set; }
    //    public int Saturday { get; set; }
    //    public int Total { get; set; }
    //    public int HR_Administration_Service { get; set; }
    //    public int Travel_Agency_Service { get; set; }
    //    public int Shared_Accounting_Service { get; set; }
    //    public int Govt_Relation_Officer { get; set; }
    //    public List<TA_TravelAgencyInfo> _travelAgencyInfo { get; set; }
    //    public List<SiteFindingsList> siteFindingList { get; set; }//anzeem on 03-06-2020
    //    public List<ProblemTypeList> problemTypeList { get; set; }//anzeem on 03-06-2020
    //    public string emp_ticket_number { get; set; }
    //    public string emp_issue_date { get; set; }
    //    public string emp_ticket_price { get; set; }
    //    public long process_table_id { get; set; }
    //    public string dataview_id { get; set; }// Request  id
    //    public string compliance_approval_date_string { get; set; } //28-05-2020
    //    public string Last_Day_Of_Work_date_string { get; set; }//28-05-2020
    //    public string Return_To_Duty_date_string { get; set; }//28-05-2020
    //    public string Car_PickUp_date_date_string { get; set; } //28-05-2020
    //    public string Hote_Checking_Date_date_string { get; set; }//28-05-2020
    //    public string Hotel_Check_Out_Date_date_string { get; set; }//28-05-2020
    //    public string Departure_Date_date_string { get; set; } //28-05-2020
    //    public string Return_Date_date_string { get; set; }//28-05-2020

    //    public string Customer_Name { get; set; }
    //    public string WBS { get; set; }
    //    public string CCRP_No { get; set; }
    //    public bool IsWarranty { get; set; }
    //    public bool IsCheckList_Attached { get; set; }
    //    public long BusinessId { get; set; }
    //    public long BusinessLineId { get; set; }
    //    public long ProductGroupId { get; set; }
    //}
    public class SitevisitInternational
    {
        public string ad_account { get; set; }
        public string wf_id { get; set; }
        public long application_id { get; set; }
        public string emp_local_id { get; set; }
        public string creator_id { get; set; }
        public string request_id { get; set; }
        public string requestId { get; set; }
        public string place_to_visit { get; set; }
        public string customer_problem { get; set; }
        public string remark_one { get; set; }
        public int is_complaince_approval_required { get; set; }
        public DateTime compliance_approval_date { get; set; }
        public DateTime last_day_of_work { get; set; }
        public DateTime return_to_duty { get; set; }
        public long wf_template_id { get; set; }
        public int workflow_delegated { get; set; }

        public string justification_no_delegation { get; set; }
        public string address_during_absence { get; set; }
        public string telephone { get; set; }
        public string mode_of_travel { get; set; }
        public long location_id { get; set; }
        public int required_exit_visa { get; set; }
        public string type_of_exit_visa { get; set; }
        public string travel_visa_charged_to { get; set; }
        public string visa_duration { get; set; }
        public string passport { get; set; }
        public int required_foreign_visa { get; set; }
        public string foreign_visa_countries { get; set; }
        public string foreign_visa_quantity { get; set; }

        public int required_travel_insurance { get; set; }
        public string travel_insurance_countries { get; set; }

        public string travel_insurance_quantity { get; set; }
        public int required_rent_car { get; set; }
        public string rent_car_charged_to { get; set; }
        public string rent_car_project_no { get; set; }
        public string car_type { get; set; }
        public string rent_car_picked_up_at { get; set; }
        public DateTime rent_car_pick_up_date { get; set; }
        public string rent_car_pick_up_time { get; set; }
        public string rent_car_payment_type { get; set; }
        public DateTime rent_car_return_date { get; set; }
        public string rent_car_return_time { get; set; }
        public string rent_car_remark { get; set; }

        public int required_hotel_booking { get; set; }

        public string hotel_booking_charged_to { get; set; }
        public string hotel_booking_project_no { get; set; }
        public string hotel_name { get; set; }
        public string hotel_location { get; set; }

        public string type_of_room { get; set; }
        public string room_preferences { get; set; }
        public int number_of_rooms { get; set; }
        public string hotel_booking_payment_mode { get; set; }
        public DateTime hotel_booking_check_in_date { get; set; }
        public string hotel_check_in_time { get; set; }
        public DateTime hotel_booking_check_out_date { get; set; }
        public string hotel_check_out_time { get; set; }
        public string hotel_booking_remark { get; set; }
        public DateTime departure_date { get; set; }
        public string departure_flight_number { get; set; }
        public DateTime return_date { get; set; }
        public string return_flight_number { get; set; }
        public string travel_routing { get; set; }
        public string type_of_ticket { get; set; }
        public string note { get; set; }
        public string file_path { get; set; }
        public int cash_advance { get; set; }
        public int amx_holder { get; set; }
        public string salary_advance { get; set; }
        public string bank_account { get; set; }
        public string ticket_cost { get; set; }
        public string iban { get; set; }
        public string hotel_cost { get; set; }
        public string daily_allowance { get; set; }
        public string other_expenses { get; set; }
        public string travel_advance_remark { get; set; }
        public string travel_advance_total { get; set; }
        public string employee_ticket_number { get; set; }
        public DateTime date_of_issue { get; set; }
        public string ticket_price { get; set; }
        public List<TA_TravelAgencyInfo> _travelAgencyInfo { get; set; }
        public List<SiteFindingsList> siteFindingList { get; set; }//anzeem on 03-06-2020
        public List<ProblemTypeList> problemTypeList { get; set; }//anzeem on 03-06-2020
        public string revalidation_charge { get; set; }
        public string total_ticket_price { get; set; }
        public int over_all_ticket_status { get; set; }
        public int busines_days { get; set; }
        public int friday { get; set; }
        public int saturday { get; set; }
        public int total_days { get; set; }
        public List<FileList> _FileList { get; set; }
        public string most_problem_unit { get; set; }
        public string problems { get; set; }
        public bool site_report { get; set; }
        public string additional_expense { get; set; }
        public bool checklist { get; set; }

        public string Customer_Name { get; set; }
        public string WBS { get; set; }
        public string CCRP_No { get; set; }
        public bool IsWarranty { get; set; }
        public bool previsit_checklist { get; set; }
        public int int_previsit_checklist { get; set; }
        public long business { get; set; }
        public long businessline { get; set; }
        public string business_line { get; set; }
        public long productgroup { get; set; }

        public string compliance_approval_date_string { get; set; } 
        public string Last_Day_Of_Work_date_string { get; set; }
        public string Return_To_Duty_date_string { get; set; }
        public string Car_PickUp_date_date_string { get; set; } 
        public string Hote_Checking_Date_date_string { get; set; }
        public string Hotel_Check_Out_Date_date_string { get; set; }
        public string Departure_Date_date_string { get; set; } 
        public string Return_Date_date_string { get; set; }
        public Location abb_locations { get; set; }

         public string employee_date_of_issue { get; set; }
        public string employee_ticket_price { get; set; }
        public string str_complaince_approval_required { get; set; }
        public string str_compliance_approval_date { get; set; }
        public string str_last_day_of_work { get; set; }
        public string str_return_to_duty { get; set; }
        public string str_workflow_delegated { get; set; }
        public string location_name { get; set; }
        public string str_required_foreign_visa { get; set; }
        public string str_required_travel_insurance { get; set; }
        public string str_required_rent_car { get; set; }
        public string str_rent_car_pick_up_date { get; set; }
        public string str_rent_car_return_date { get; set; }
        public string str_required_hotel_booking { get; set; }
        public string str_hotel_booking_check_in_date { get; set; }
        public string str_hotel_booking_check_out_date { get; set; }
        public string str_departure_date { get; set; }
        public string str_return_date { get; set; }
        public string str_amx_holder { get; set; }
        public string str_cash_advance { get; set; }
        public string strWarranty { get; set; }
        public string str_checklist { get; set; }
        public string str_site_report { get; set; }
    }
    public class ProblemTypeList
    {
        public int count { get; set; }
        public string problem_type { get; set; }

    }
    public class SiteFindingsList
    {
        public int count { get; set; }
        public long id { get; set; }
        public string problem_type { get; set; }
        public string problem_detail { get; set; }
        public bool IsChecked { get; set; }

    }

    #region For T004 In-House Training by George 30-06-2020
    public class InHouseTrainingModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Remarks { get; set; }

        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public string Attachment_Filepath { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
        public string From_Date_string_date { get; set; }
        public string To_Date_string_date { get; set; }
    }

    public class InHouseTrainingCourseModel
    {
        public long Id { get; set; }
        public long TrainingId { get; set; }
        public long CourseId { get; set; }
        public string Code { get; set; }
        public string Course_Name { get; set; }
        public string Type { get; set; }
        public DateTime From_Date { get; set; }
        public DateTime To_Date { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }

        public string From_Date_string_date { get; set; }
        public string To_Date_string_date { get; set; }
    }
    #endregion

    public class ExpenseReportModel //P045 chitra Srishti 08.07.2020
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public string RequestType { get; set; }
        public string ChargeTo { get; set; }
        public string PlacesVisited { get; set; }
        public string Region { get; set; }
        public string PersonnelVisited { get; set; }
        public string BusinessLine { get; set; }
        public string TARequest { get; set; }
        public Nullable<System.DateTime> TARequestDate { get; set; }
        public Nullable<System.DateTime> LastDayOfWork { get; set; }
        public Nullable<System.DateTime> ReturnToDuty { get; set; }
        public Nullable<bool> ComplianceApproved { get; set; }
        public Nullable<System.DateTime> ComplianceApprovalDate { get; set; }
        public string FilePath { get; set; }
        public Nullable<System.DateTime> TimeStamp { get; set; }
        public Nullable<decimal> AmtTotal { get; set; }
        public Nullable<decimal> LessAdvance { get; set; }
        public Nullable<decimal> TicketsPaidByCo { get; set; }
        public Nullable<decimal> NetToReceive { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public List<ExpenseReportAllowanceModel> ExpenseReportAllowanceList { get; set; }
        public List<ExpenseReportDetailModel> ExpenseReportDetailList { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
    }
    public class ExpenseReportAllowanceModel
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public Nullable<long> ER_ParentId { get; set; }
        public string Place { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.TimeSpan> FromHours { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<System.TimeSpan> ToHours { get; set; }
        public Nullable<decimal> DaysHours { get; set; }
        public Nullable<decimal> DailyAllowance { get; set; }
        public Nullable<decimal> AmtLocal { get; set; }
        public Nullable<long> SequenceNum { get; set; }
    } //P045 chitra Srishti 08.07.2020
    public class ExpenseReportDetailModel //P045 chitra Srishti 08.07.2020
    {
        public long Id { get; set; }
        public string RequestId { get; set; }
        public Nullable<long> ER_ParentId { get; set; }
        public Nullable<long> AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public string AirTickets { get; set; }
        public string Taxi { get; set; }
        public Nullable<decimal> KM { get; set; }
        public Nullable<decimal> Allowance { get; set; }
        public string Description1 { get; set; }
        public string Description2 { get; set; }
        public string Currency { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> ExchangeRate { get; set; }
        public Nullable<decimal> AmtLocal { get; set; }
        public Nullable<decimal> AmtTotal { get; set; }
        public Nullable<decimal> LessAdvance { get; set; }
        public Nullable<decimal> TicketsPaidByCo { get; set; }
        public Nullable<decimal> NetToReceive { get; set; }
        public Nullable<long> SequenceNum { get; set; }
        public string TicketNumbers { get; set; }
        public string ERAccount { get; set; }
    }


    #region T001 External Training by George Srishti 13-07-2020

    public class ExternalTrainingModel
    {
        public long Id { get; set; }

        public string RequestId { get; set; }

        public string Justification { get; set; }

        public string Years { get; set; }

        public decimal? GrandTotal { get; set; }

        public string Status { get; set; }

        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Attachment_Filepath { get; set; }
        public List<FileListPrint> _FileListPrint { get; set; }
    }

    public class ExternalTrainingDetailModel
    {
        public long Id { get; set; }

        public long ExternTrainingId { get; set; }

        public string RequestId { get; set; }

        public string Course_Name { get; set; }

        public long CourseId { get; set; }

        public DateTime From_Date { get; set; }

        public DateTime To_Date { get; set; }

        public string Training_Type { get; set; }

        public string Location { get; set; }

        public decimal? Cost { get; set; }

        public int? NoofDays { get; set; }

        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public string From_Date_string_date { get; set; }
        public string To_Date_string_date { get; set; }
    }


    public class ExternalCourseDataModel
    {
        public long Id { get; set; }

        public string Course_Name { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public string TrainingType { get; set; }

        public string Location { get; set; }

        public decimal? Cost { get; set; }

        public bool IsActive { get; set; }

        public string From_Date_string_date { get; set; }

        public string To_Date_string_date { get; set; }
    }

    #endregion
}


