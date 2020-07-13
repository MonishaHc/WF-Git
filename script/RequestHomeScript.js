    var public_PrintApprove = false;
$(".rotate").click(function () {
    $(this).toggleClass("down");
})
var commonFile = "";
var bankFile = "";
var afterSaveCommonFilePath = "";
var afterSaveBankFilePath = "";
var requestId = "";
var request_table_id = 0;
//---------------------------P005----------------------------------------------
$(document).ready(function () {
    $('.process-btn-section').hide();
    $('.process-btn-section').css('display', 'none');
    var empId = $('#emp_identify_id').val();
    FillEmployeeDetails(empId);
    $('#submit_request_btn').css('display', 'none');
    $('#edit_request_btn').css('display', 'none');  //Basheer on 23-01-2020
    $('.wf-types-list-drop-id').prop("disabled", true);
});

$(document).on('click', '.click_event', function () {
    $('.wf-types-list-drop-id').prop("disabled", false);
});

// List the Applications in Domain Dropdown
$(document).on('change', '.domain-list-drop-id', function () {
    $('#domainid').val($(this).val());
    $('#applicationid').val("0");
    $('#wftypeid').val("0");
    $(".application-list-drop-id").html("");
    $(".wf-types-list-drop-id").html("");
    $('.process-btn-section').hide();
    var id = $(this).val();
    if (id != '') {
        $.ajax({
            url: '/Data/LoadApplicatonList?domainid=' + id,
            type: "GET",
            success: function (result) {
                if (result.list.length > 0) {
                    $(".application-list-drop-id").html("");
                    $(".application-list-drop-id").append($('<option></option>').val("0").html('-- Select Application --'));
                    $.each(result.list, function (i, item) {
                        $(".application-list-drop-id").append($('<option></option>').val(item.Value).html(item.Text));
                    });
                    $(".application-list-drop-id").prop("disabled", false);
                }
                else {
                    $(".application-list-drop-id").html("");
                    $(".application-list-drop-id").append($('<option></option>').val("0").html('-- No Application Found --'));
                    $(".application-list-drop-id").prop("disabled", true);
                    $('#applicationid').val("0");
                }
            }
        });
    }
});
// List the Services/WF Types in Application Dropdown
$(document).on('change', '.application-list-drop-id', function () {
    $('#applicationid').val($(this).val());
    $(".wf-types-list-drop-id").html("");
    $('#wftypeid').val("0");
    $('.process-btn-section').hide();
    $('.application_name').text($('#application_code_id').val());
    var id = $(this).val();
    if (id != '') {
        $.ajax({
            url: '/Data/LoadWFTypeList?applicationId=' + id,
            type: "GET",
            success: function (result) {
                if (result.list.length > 0) {
                    $(".wf-types-list-drop-id").html("");
                    $(".wf-types-list-drop-id").append($('<option></option>').val("0").html('-- Select Service --'));
                    $.each(result.list, function (i, item) {
                        $(".wf-types-list-drop-id").append($('<option></option>').val(item.Value).html(item.Text));
                    });
                    $(".wf-types-list-drop-id").prop("disabled", false);
                }
                else {
                    $(".wf-types-list-drop-id").html("");
                    $(".wf-types-list-drop-id").append($('<option></option>').val("0").html('-- No Service Found --'));
                    $(".wf-types-list-drop-id").prop("disabled", true);
                    $('#wftypeid').val("0");
                }
            }
        });
    }
});
// Set the page respect to the service
$(document).on('change', '.wf-types-list-drop-id', function () {
    $('#wftypeid').val($(this).val());
    $('.process-btn-section').hide();
    var empId = $('.employee-list-drop-id :selected').val();
    $('#application-required').hide();
    $('.application_name').text($('#application_code_id').val());
    var id = $(this).val();
    if (id != "0") {
        id = id + '~' + empId;
        $.ajax({
            url: '/Request/SetViewRespectToWFType/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.view_with_wftype_id').html('');
                $('.view_with_wftype_id').html(data);
                $('.request_status_now').text('');
                $('.process-btn-section').show();
                $('.process-btn-section').css('display', 'block');
                $('#cancel_request_btn').prop('disabled', false);
                $('#save_request_btn').prop('disabled', false);
                var currentId = $('.wf-types-list-drop-id').val();
                if (currentId == 'P011') {
                    EmployeeCuntryDetails(empId);                }
                else if (currentId == 'P012') {    
                    EmployeeCuntryDetails(empId);                }
                else if (currentId == 'P016' || currentId == 'P017') {  
                    EmployeeDetails_P016(empId, currentId);
                }
                $('#edit_request_btn').css('display', 'none');
                $('#submit_request_btn').css('display', 'none');
                $('#forward_request_btn').css('display', 'none');
                $('#cancel_request_btn').css('display', 'none');
                $('#save_request_btn').css('display', 'block');
                $('#print_request_btn').css('display', 'none'); //basheer changed to none on 03-03-2020
                $('.request_status_now').val('');
                commonFile = "";
                bankFile = "";
                afterSaveCommonFilePath = "";
                afterSaveBankFilePath = "";
                requestId = "";
                request_table_id = 0;
            }
        });
    }
    else {
        $('.process-btn-section').hide();
    }
});
// Fill Employee Details
function FillEmployeeDetails(empid) {
    if (empid != '') {
        $.ajax({
            url: '/Request/GetEmployeeDetails',
            type: "POST",
            data: { 'empid': empid },
            dataType: 'JSON',
            success: function (data) {
                var x = data.globalid + ' : ' + data.grade; // 27-02-2020 ARCHANA SRISHTI
                $('.empName').val(data.emp_name);
                $('.globalEmpId').val(x);
                $('.companyEmp').val(data.company);
                $('.empJobTitle').val(data.tittle);
                $('.empDepartment').val(data.department);
                $('.empBusinessLine').val(data.businessline);
                $('.empCostCenter').val(data.cost_center);
                $('.empMobileNo').val(data.mobile);
                $('.empTelephone').val(data.telephone);
            }
        });
    }
}
// Employee List change will change the employee details
$(document).on('change', '.employee-list-drop-id', function () {
    var id = $(this).val();
    FillEmployeeDetails(id);
    ////Sibi 06-01-2020.............................
    var currentId = $('.wf-types-list-drop-id :selected').val();
    if (currentId == 'P011') {
        EmployeeCuntryDetails(id);
    }
    else if (currentId == 'P012') {  //19-03-2020 Nimmi
        EmployeeCuntryDetails(id);
    }
    else if (currentId == 'P016' || currentId == 'P017') {
        //P016-Internal Transfer(Preema)
        EmployeeDetails_P016(id, currentId);
        $('#hdn_Count').val('1');
    }
});

// Bank transfer radio btn click
$(document).on('click', '.bank_radio_btn', function () {
    var id = $('.employee-list-drop-id :selected').val();
    var wf_id = $('#wftypeid').val();
    //P053-GOSI Payment(Preema)
    if (wf_id == "P053") {
        if (id != "0") {
            $.ajax({
                url: '/Request/GetBankdetails_GOSI_Payment_Request/' + id,
                type: "GET",
                dataType: 'html',
                success: function (data) {
                    $('.payment-mode-id').html('');
                    $('.payment-mode-id').html(data);
                }
            });
        }
    }
    else {
        if (id != "0") {
            $.ajax({
                url: '/Request/GetBankdetailsForPPrequest/' + id,
                type: "GET",
                dataType: 'html',
                success: function (data) {
                    $('.payment-mode-id').html('');
                    $('.payment-mode-id').html(data);
                }
            });
        }
    }
});
// Cheque radio button click
$(document).on('click', '.cheque_radio_btn', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequest/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Terrin on 25/3/20
// Bank transfer radio btn click-P057
$(document).on('click', '.bank_radio_btnp_P057', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestP057/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});
// Cheque radio button click-P057
$(document).on('click', '.cheque_radio_btnp_P057', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP057/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Common File Path
$(document).on('change', '.common_filePath_pp', function () {
    var source = $(this);
    var formData = new FormData();
    commonFile = this.files[0];
    if (commonFile != null) {
        SaveCommonFilePath();
    }
});

// Save Common File Path
function SaveCommonFilePath() {
    var formData = new FormData();
    formData.append("documentFile", commonFile);
    var xxxx = formData.hasOwnProperty(name).toLocaleString();
    var maxFileSize = 1048760;// 2MB=2*1024*1024  ‭‬
    console.log(commonFile);
    console.log(commonFile.size);
    var fileSize = commonFile.size;
    if (fileSize > maxFileSize) {
        toastrError('Please reduce the size of the file .');
        $('.common_filePath_pp').val('');
        commonFile = null;
        afterSaveCommonFilePath = "";
    } else {
        $.ajax({
            type: "POST",
            url: "/Request/UploadFile",
            dataType: 'json',
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
            },
            success: function (response) {
                if (response.status) {
                    afterSaveCommonFilePath = response.fileSave;
                }
                else {
                }
            },
        });
    }
};

// Bank File Path
$(document).on('change', '.bank_file_path', function () {
    var source = $(this);
    var formData = new FormData();
    bankFile = this.files[0];
    if (bankFile != null) {
        SaveBankFilePath();
    }
});
// Save Bank File Path
function SaveBankFilePath() {
    var formData = new FormData();
    formData.append("documentFile", bankFile);
    var xxxx = formData.hasOwnProperty(name).toLocaleString();
    var maxFileSize = 1048760;// 2MB=2*1024*1024
    var fileSize = bankFile.size;
    if (fileSize > maxFileSize) {
        toastrError('Please reduce the size of the file .');
        $('.bank_file_path').val('');
        bankFile = null;
        afterSaveBankFilePath = "";
    } else {
        $.ajax({
            type: "POST",
            url: "/Request/UploadFile",
            dataType: 'json',
            data: formData,
            contentType: false,
            processData: false,
            beforeSend: function (xhr) {
            },
            success: function (response) {
                if (response.status) {
                    afterSaveBankFilePath = response.fileSave;
                }
                else {
                }
            },
        });
    }
}
// Request save as initiator*****************************************SAVE BUTTON CLICK HERE ***********************************
$(document).on('click', '#save_request_btn', function () {
    $('.wf-types-list-drop-id').prop("disabled", true);
    var x = false;
    var application_name = $('#application_code_id').val();
    var obj = new Object();
    obj.wf_id = $('.wf-types-list-drop-id').val();
    if ($('.wf-types-list-drop-id :selected').val() == "0") {
        $('#application-required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        if (obj.wf_id == 'P055') {
            //Basheer on 25-02-2020
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.remark = $('.remark_p055').val();    //Terrin on 12/5/2020
                //Terrin on 12/5/2020
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    $('#cheque_date_required').css('display', 'none');
                    x = false;
                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {

                    $('#cheque_purpose_text_required').css('display', 'none');
                    x = false;
                }

                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                //obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();   //Terrin on 12/5/2020
                obj.remark = $('.remark_p055').val();
                //Terrin on 12/5/2020
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                    // return;
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');
                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                    //return;
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                    // return;
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                    // return;
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                    //  return;
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                    //  return;
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');
                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }

                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }

            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_BalanceHousingAllowance",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020

                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //$('.request_status_now').text('New request not submitted');
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields"); //Terrin on 14/5/2020

            }
        }// Balance Housing Allowance P055
            //14/05/2020 Alena Sics EOSB CALCULATION
        else if (obj.wf_id == 'P052') {
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn_P052").prop('checked') == true) { // Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date_P052').val();
                obj.amount_sar = $('.cheque_amt_P052').val();
                obj.purpose_text = $('.cheque_purpose_P052').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to_P052').val();
                obj.remark = $('.cheque_remark_P052').val();
                obj.endofservice = $('.endrequest-reqid').val();
                if ($('.cheque_amt').val() == '') {
                    $('#cheque_amtsar_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amtsar_required').css('display', 'none');
                }
                if (obj.endofservice == "") {
                    $('#endofservice_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    $('#endofservice_required').css('display', 'none');
                    x = false;
                }
                if (obj.amount_sar == "") {
                    $('#cheque_amtsar_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    $('#cheque_amtsar_required').css('display', 'none');
                    x = false;
                }
                if (obj.cheque_date == "") {
                    $('#cheque_date_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    $('#cheque_date_required').css('display', 'none');
                    x = false;
                }
                if (obj.purpose_text == "") {
                    $('#cheque_purpose_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    $('#cheque_purpose_required').css('display', 'none');
                    x = false;
                }
                //28/05/2020 ALENA SICS
                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "" || obj.endofservice == "") {
                    x = true;
                }
                //end
            }
                // 29/05/2020 ALENA SICS
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_amount_P052').val();;
                obj.from_bank = $('.from_bank_P052').val();
                obj.from_addreess = $('.from_address_P052').val();
                obj.from_account_no = $('.from_accountno_P052').val();
                obj.to_beneficiary = $('.to_benificiary_P052').val();
                obj.to_bankname = $('.to_bank_P052').val();
                obj.to_address = $('.to_address_P052').val();
                obj.to_account_no = $('.to_accountno_P052').val();
                obj.to_iban = $('.to_iban_P052').val();
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_purpose_P052').val();
                obj.remark = $('.bank_remark_P052').val();
                obj.endofservice = $('.endrequest-reqid').val();
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');
                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_address_required').css('display', 'none');
                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }
                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_benificiary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_benificiary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }
                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');
                }
                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_iban_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_purpose_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_purpose_required').css('display', 'none');
                }
                if (obj.endofservice == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#endofservice_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#endofservice_required').css('display', 'none');
                }
                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "" || obj.endofservice == "") {
                    x = true;
                }
            } // END
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_EOSBCalculation",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }
                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields"); //28/05/2020 ALENA
            }
        } // EOSB CALCULATION P052
            // Terrin 0n 25/03/20
        else if (obj.wf_id == 'P057') {
            //Basheer on 25-02-2020
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btnp_P057").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.remark = $('.remark_p055').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');

                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);

                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }


                if (obj.payable_to == "") {
                    x = true;
                    $('#cheque_payable_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {

                    x = false;
                    $('#cheque_payable_required').css('display', 'none');
                }


                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }




            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_iban = $('.to_bank_iban').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();


                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }
                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }



            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_SalaryforEmployeeunderlqamaprocesspayment",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020

                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //$('.request_status_now').text('New request not submitted');
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {

                toastrError("Please fill all the mandatory fields");
            }
        } // P057-Salary for Employee Under Iqama process Payment
        else if (obj.wf_id == 'IS05') {
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            obj.change_summary = $('.change_summary_text').val();
            if (obj.change_summary == '') {
                toastrError("Please enter the Change Summary!");
                $('#cancel_request_btn').prop('disabled', false);
                $('#save_request_btn').prop('disabled', false);
                $('.change_summary_text').focus();
            }
            else {
                obj.detailed_description = $('.detailed_description_text').val();
                if (obj.detailed_description == '') {
                    toastrError("Please enter the detailed description !");
                    $('#cancel_request_btn').prop('disabled', false);
                    $('#save_request_btn').prop('disabled', false);
                    $('.detailed_description_text').focus();
                }
                else {
                    obj.proposed_plan = $('.proposed_plan_text').val();
                    if (obj.proposed_plan == '') {
                        toastrError("Please enter the proposed plan !");
                        $('#cancel_request_btn').prop('disabled', false);
                        $('#save_request_btn').prop('disabled', false);
                        $('.proposed_plan_text').focus();
                    }
                    else {
                        obj.impct = $('.business_system_impact_analysis_text').val();
                        if (obj.impct == '') {
                            toastrError("Please enter the business/system impact analysis !");
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#save_request_btn').prop('disabled', false);
                            $('.business_system_impact_analysis_text').focus();
                        }
                        else {
                            obj.fallback_options = $('.fallback_options_text').val();
                            if (obj.fallback_options == '') {
                                toastrError("Please enter the Fallback Options !");
                                $('#cancel_request_btn').prop('disabled', false);
                                $('#save_request_btn').prop('disabled', false);
                                $('.fallback_options_text').focus();
                            }
                            else {
                                obj.positive_risk_assessment = $('.positive_risk_assessments_text').val();
                                if (obj.positive_risk_assessment == '') {
                                    toastrError("Please enter the positive risk assessments !");
                                    $('#cancel_request_btn').prop('disabled', false);
                                    $('#save_request_btn').prop('disabled', false);
                                    $('.positive_risk_assessments_text').focus();
                                }
                                else {
                                    obj.negative_risk_assessment = $('.negative_risk_assessments_text').val();
                                    if (obj.negative_risk_assessment == '') {
                                        toastrError("Please enter the negative risk assessments !");
                                        $('#cancel_request_btn').prop('disabled', false);
                                        $('#save_request_btn').prop('disabled', false);
                                        $('.negative_risk_assessments_text').focus();
                                    }
                                    else {
                                        obj.file_path = afterSaveCommonFilePath;
                                        if (obj.file_path == '') {
                                            toastrError("Please select the file !");
                                            $('#cancel_request_btn').prop('disabled', false);
                                            $('#save_request_btn').prop('disabled', false);
                                        }
                                        obj.clarification = $('#request_clarification_drop :selected').val();
                                        console.log(obj);
                                        $(".se-pre-con").show();
                                        $.ajax({
                                            type: "POST",
                                            url: "/IS/Submit_IS_InfrasructureChangeManagement",
                                            dataType: "json",
                                            global: false,
                                            data: obj,
                                            success: function (response) {
                                                if (response.Status) {
                                                    $('#save_request_btn').css('display', 'none');
                                                    $('#submit_request_btn').css('display', 'block');
                                                    requestId = response.Request_Id;
                                                    var request_full_Id = application_name + '-' + requestId;
                                                    //$('#request_id_display').text(request_full_Id);
                                                    $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                                                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                                                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                                                    $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                                                    $('#submit_request_btn').prop('disabled', false);
                                                    $('#cancel_request_btn').css('display', 'block');
                                                    $('#cancel_request_btn').prop('disabled', false);
                                                    $(".se-pre-con").hide();
                                                    toastrSuccess(response.Message);
                                                    //$('.request_status_now').text('New request not submitted');
                                                    //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                                                    var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                                                    $('.request_status_now').text(msg);

                                                    public_PrintApprove = true;
                                                }
                                                else {
                                                    $(".se-pre-con").hide();
                                                    toastrError(response.Message);
                                                    $('#save_request_btn').css('display', 'block');
                                                    $('#submit_request_btn').css('display', 'none');
                                                    requestId = "";
                                                    $('#submit_request_btn').prop('disabled', true);
                                                    $('#cancel_request_btn').css('display', 'block');
                                                    $('#cancel_request_btn').prop('disabled', false);
                                                    $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                                                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                                                }

                                            },
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }// Infrastructure Change
        else if (obj.wf_id == 'P009') {
            //Basheer on 16-03-2020
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn_P009").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.cheque_account_no = $('.cheque_account_no').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.supplier_to = $('.cheque_supplier').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.remark = $('.attachment_remark').val();
                obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');
                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }
                if (obj.cheque_account_no == "") {
                    x = true;
                    $('#cheque_account_no_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_account_no_required').css('display', 'none');
                }
                if (obj.supplier_to == "") {
                    x = true;
                    $('#cheque_supplier_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_supplier_required').css('display', 'none');
                }

                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "" || obj.cheque_account_no == "" || obj.supplier_to == "") {
                    x = true;
                }
            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_bank_iban').val();
                obj.purpose_text = $('.bank_remark').val();
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.remark = $('.attachment_remark').val();
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');
                }
                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }               
            }
            //----------------------------------------------------------------
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_HRRelatedPaymentRequest",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;                          
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields"); //Terrin on 20/5/2020
            }
            //----------------------------------------------------------------
        }// HR Related Payment Request
        else if (obj.wf_id == 'P010') {
            //Basheer on 16-03-2020
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.contract_local = $('.contract_local_no_cls').val();
            obj.backcharge_invoice = $('.backcharge_invoice_to_cls').val();
            obj.project = $('.project_cls').val();
            obj.year_booked = $('.year_booked_cls').val();
            obj.customer = $('.customer_cls').val();
            obj.request_id = requestId;
            if (obj.contract_local == '') {
                $('#contract_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#contract_required').css('display', 'none');
            }
            if (obj.backcharge_invoice == "") {
                $('#invoice_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#invoice_required').css('display', 'none');
                x = false;
            }
            if (obj.project == '') {
                $('#Project_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#Project_required').css('display', 'none');
            }
            if (obj.year_booked == "") {
                $('#year_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#year_required').css('display', 'none');
                x = false;
            }
            if (obj.customer == "") {
                $('#customer_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#customer_required').css('display', 'none');
                x = false;
            }
            if ($(".cheque_radio_btn_P010").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.cheque_account_no = $('.cheque_account_no').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.supplier_to = $('.cheque_supplier').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.remark = $('.attachment_remark').val();
                obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
                if ($('.cheque_amt').val() == '') {
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.cheque_date == "") {
                    $('#cheque_date_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    $('#cheque_date_required').css('display', 'none');
                    x = false;
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');
                }
                if (obj.cheque_account_no == "") {
                    x = true;
                    $('#cheque_account_no_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_account_no_required').css('display', 'none');
                }
                if (obj.supplier_to == "") {
                    x = true;
                    $('#cheque_supplier_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_supplier_required').css('display', 'none');
                }
                if (obj.contract_local == "" || obj.backcharge_invoice == "" || obj.project == "" || obj.year_booked == "" || obj.customer == "" || obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "" || obj.cheque_account_no == "" || obj.supplier_to == "") {
                    x = true;
                }
            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_bank_iban').val();
                obj.purpose_text = $('.bank_remark').val();
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.remark = $('.attachment_remark').val();
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');
                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }
                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }
                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');
                }
                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');
                }
                if (obj.contract_local == "" || obj.backcharge_invoice == "" || obj.project == "" || obj.year_booked == "" || obj.customer == "" || obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_NonHRRelatedPaymentRequest",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }
                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }
        }
            // NonHR Related Payment Request
            //---------------------------------------------------------------------------//
            // Created  :Sibi......02-01-2010
            // Modified : None
            // Salary Advance for New Arrival Payment - P051
        else if (obj.wf_id == 'P051') {
            //Basheer on 16-03-2020
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.remark = $('.remark_p055').val();
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');
                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');
                }
                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.from_bank = $('.from_bank_name').val(); //12-02-2020 Sibi Edited
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name_Second').val();
                obj.to_address = $('.to_bank_address_Second').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');
                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');
                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }

                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Salary_Advance_for_New_Arrival_Payment",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //$('.request_status_now').text('New request not submitted');
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }
        }
            // End Sibi
            //Archana Starts 05/01/2020
        else if (obj.wf_id == 'P034') {
            Save_P034();
        }
            //------------- P030 Educational Assistance done by Chitra V srishti 28.05.2020-------------------------
        else if (obj.wf_id == 'P030') {
            Save_P030();
        }
            //------------- P003 Ticket Refund done by Chitra V srishti 16.06.2020-------------------------
        else if (obj.wf_id == 'P003') {
            Save_P003();
        }
        else if (obj.wf_id == 'P050') {
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn_P050").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.remark = $('.remark_p055').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');
                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);                    
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }

                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();  // Terrin on 20/5/2020
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }

                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }


            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_SettlingAllowancePayment",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);

                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }
        }

            // End Basheer

        else if (obj.wf_id == 'P013') {
            Save_P013();
        }
            //Terrin on 30/3/2020..............P060..............save_request_btn...........................ss

        else if (obj.wf_id == 'P060') {

            //Terrin on 30-3-2020

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            //obj.notes = $('#divNotes').text();

            obj.Allowance_per_group = $('.Allowance_per_group :selected').val();
            obj.Remarks = $('.Remarks').val();
            obj.Justification = $('.Justification').val();
            obj.Allowance_Date = $('.Allowance_Date').val();

            if ($('.Allowance_per_group :selected').val() == "") {
                $('#Allowance_per_group-required').css('display', 'block');
                //$('#submit_request_btn save_request_btn').prop('disabled', false);
                $('#save_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                $('#Allowance_per_group-required').css('display', 'none');
            }

            if ($('.Remarks').val() == '') {
                $('#Remark-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                //return;
            }
            else {
                x = false;
                $('#Remark-required').css('display', 'none');
            }

            if ($('.Justification').val() == '') {
                $('#Justification-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                // return;
            }
            else {
                x = false;
                $('#Justification-required').css('display', 'none');
            }

            //if ($('.Allowance_Date').val() == '') {
            //    $('#Allowance_Date_required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    return;
            //}
            //else {
            //    x = false;
            //    $('#Allowance_Date_required').css('display', 'none');
            //}

            if (obj.Allowance_per_group == "" || obj.Remarks == "" || obj.Justification == "") {
                x = true;
            }



            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Application_Mobile_Allowance",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);

                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }


        }

            //-----------Terrin on 7/4/2020 ------------P029----------

        else if (obj.wf_id == 'P029') {
            var i = 0;

            obj._FileList = [];

            for (let i = 0; i < lists.length; i++) {
                var ax = new Object();
                ax.filename = lists[i].filename;
                ax.filepath = lists[i].filepath;
                ax.filebatch = lists[i].filetype;
                obj._FileList.push(ax);
            }

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.application_ids = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            obj.App_Type = $('.Application_Type :selected').val();
            obj.Add_details = $('.AddDetails :selected').val();
            obj.Effective = $('.Effective').val();
            obj.Date_Employee = $('.Date_Employee').val();
            obj.Iqama_no = $('.Iqama_no').val();
            obj.MedIns_Remarks = $('.MedIns_Remarks').val();
            obj.Attachment_Filepath = afterSaveCommonFilePath;

            var Insurance_dependence = new Array();
            $('#tbl_dependent tbody tr').each(function () {
                var row = $(this);
                var Insurancedependence = {};
                Insurancedependence.Name = row.find('#id-InsName').val();
                Insurancedependence.Date_of_birth = row.find('.Ins_Date').val();
                //Insurancedependence.Sex = row.find('#Gender'+i).val();
                Insurancedependence.Sex = row.find('input:radio:checked').val();
                Insurancedependence.Relation = row.find('#id-InsRelation').val();
                Insurancedependence.Depend_class = row.find('#id-InsClass').val();
                Insurance_dependence.push(Insurancedependence);
            });

            obj._Insurance_dependence = Insurance_dependence;


            if (obj.App_Type == '--Select--') {
                $('#application-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                // return;
            }
            else {
                x = false;
                $('#application-required').css('display', 'none');
            }

            if (obj.Add_details == '--Select--') {
                $('#AddDetails-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                //return;
            }
            else {
                x = false;
                $('#AddDetails-required').css('display', 'none');
            }

            if ($('.Effective').val() == '') {
                $('#Effective_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                //return;
            }
            else {
                x = false;
                $('#Effective_required').css('display', 'none');
            }

            if ($('.Date_Employee').val() == '') {
                $('#Date_Employee_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                // return;
            }
            else {
                x = false;
                $('#Date_Employee_required').css('display', 'none');
            }

            if ($('.Iqama_no').val() == '') {
                $('#id-Iqama_no-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                //return;
            }
            else {
                x = false;
                $('#id-Iqama_no-required').css('display', 'none');
            }


            //if ($('.MedIns_Remarks').val() == '') {
            //    $('#id-MedIns_Remarks-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false;
            //    $('#id-MedIns_Remarks-required').css('display', 'none');
            //}

            if (obj.App_Type == "--Select--" || obj.Add_details == "--Select--" || obj.Effective == "" || obj.Date_Employee == "" || obj.Iqama_no == "") {
                x = true;
            }

            if (obj.Add_details == 'Employee' && obj.Effective != "" && obj.Date_Employee != "" && obj.Iqama_no != "" && obj.App_Type != "--Select--") {
                x = false;
            }

            var b = 0;
            if (obj.Add_details == 'Dependent') {

                $('#tbl_dependent tbody tr').find(function () {
                    //  if ($(this).val() == '') {
                    if (($(this).find('.Ins_Name1').val() == '') || ($(this).find('.InsDate1').val() == '') || ($(this).find('.InsRelation1').val() == '') || ($(this).find('.InsClass1').val() == '')) {
                        b = 1;
                        $('#Ins_Name-required').css('display', 'block');
                        toastrError("Please fill all the mandatory fields");
                        x = true;
                        // return;
                    }

                    else {
                        b = 2;
                        $('#Ins_Name-required').css('display', 'none');
                        x = false;

                        if (x == false && b == 2) {

                            $(".se-pre-con").show();

                            $.ajax({
                                type: "POST",
                                url: "/Request/Submit_MedicalInsurance",
                                dataType: "json",
                                global: false,
                                data: obj,
                                success: function (response) {

                                    if (response.Status) {

                                        $('#save_request_btn').css('display', 'none');
                                        $('#submit_request_btn').css('display', 'block');
                                        requestId = response.Request_Id;

                                        var request_full_Id = application_name + '-' + requestId;

                                        $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                                        $('#edit_request_btn').css('display', 'block');
                                        $('#forward_request_btn').css('display', 'block');
                                        $('#print_request_btn').css('display', 'block');

                                        $('#submit_request_btn').prop('disabled', false);
                                        $('#cancel_request_btn').css('display', 'block');
                                        $('#cancel_request_btn').prop('disabled', false);
                                        $(".se-pre-con").hide();
                                        toastrSuccess(response.Message);

                                        var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                                        $('.request_status_now').text(msg);

                                        public_PrintApprove = true;
                                    }
                                    else {

                                        $(".se-pre-con").hide();
                                        toastrError(response.Message);
                                        $('#save_request_btn').css('display', 'block');
                                        $('#submit_request_btn').css('display', 'none');
                                        requestId = "";
                                        $('#submit_request_btn').prop('disabled', true);
                                        $('#cancel_request_btn').css('display', 'block');
                                        $('#cancel_request_btn').prop('disabled', false);
                                        $('#edit_request_btn').css('display', 'none');
                                        $('#forward_request_btn').css('display', 'none');

                                    }

                                },

                            });


                        }

                        else {
                            toastrError("Please fill all the mandatory fields");
                        }
                    }
                });




            }
            else {
                if (x == false) {

                    $(".se-pre-con").show();

                    $.ajax({
                        type: "POST",
                        url: "/Request/Submit_MedicalInsurance",
                        dataType: "json",
                        global: false,
                        data: obj,
                        success: function (response) {

                            if (response.Status) {

                                $('#save_request_btn').css('display', 'none');
                                $('#submit_request_btn').css('display', 'block');
                                requestId = response.Request_Id;

                                var request_full_Id = application_name + '-' + requestId;

                                $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                                $('#edit_request_btn').css('display', 'block');
                                $('#forward_request_btn').css('display', 'block');
                                $('#print_request_btn').css('display', 'block');

                                $('#submit_request_btn').prop('disabled', false);
                                $('#cancel_request_btn').css('display', 'block');
                                $('#cancel_request_btn').prop('disabled', false);
                                $(".se-pre-con").hide();
                                toastrSuccess(response.Message);

                                var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                                $('.request_status_now').text(msg);

                                public_PrintApprove = true;
                            }
                            else {

                                $(".se-pre-con").hide();
                                toastrError(response.Message);
                                $('#save_request_btn').css('display', 'block');
                                $('#submit_request_btn').css('display', 'none');
                                requestId = "";
                                $('#submit_request_btn').prop('disabled', true);
                                $('#cancel_request_btn').css('display', 'block');
                                $('#cancel_request_btn').prop('disabled', false);
                                $('#edit_request_btn').css('display', 'none');
                                $('#forward_request_btn').css('display', 'none');

                            }

                        },

                    });


                }
                else {
                    toastrError("Please fill all the mandatory fields");
                }

            }
            if (x == true) {
                toastrError("Please fill all the mandatory fields");
            }
        }

            //Add sibi 03-01-2020.............pending........save_request_btn...........................ss
        else if (obj.wf_id == 'P011') {
            //Basheer on 16-03-2020
            obj._FileList = [];


            for (let i = 0; i < lists.length; i++) {
                var ax = new Object();
                ax.filename = lists[i].filename;
                ax.filepath = lists[i].filepath;
                ax.filebatch = lists[i].filetype;
                obj._FileList.push(ax);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            obj.LocalEmplyee_ID = $('.employee-list-drop-id :selected').val();
            obj.Iqama_Number = $('.Iqama_Number').val();
            obj.Certificate_with_Salary = $('.Certificate_with_Salary :selected').val();
            obj.Chamber_Of_Commerce_Stamp = $('.Chamber_Of_Commerce_Stamp :selected').val();
            obj.Ministry_Of_Foreign_Affairs = $('.Ministry_Of_Foreign_Affairs :selected').val();
            //obj.Location_Code = $('.wf-cuntry-list-drop-id :selected').text();
            obj.Location_Id = $('.wf-cuntry-list-drop-id :selected').val();
            obj.Iqama_Identification = $('.Iqama_Identification').val();
            obj.Attachment_Filepath = afterSaveCommonFilePath;


            if ($('.Iqama_Number').val() == '') {
                $('#id-IqamaNumber-required').css('display', 'block');
                //$('#submit_request_btn save_request_btn').prop('disabled', false);
                $('#save_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                    
                $('#id-IqamaNumber-required').css('display', 'none');
            }
            if (obj.Certificate_with_Salary == '--select--') {
                x = true;
                $('#certificate_required').css('display', 'block');
                $('#save_request_btn').prop('disabled', false);
                   
            }
            else {
                x = false;
                  
                $('#certificate_required').css('display', 'none');
            }

            if (obj.Chamber_Of_Commerce_Stamp == '--select--') {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#Commerce-required').css('display', 'block');
            }
            else {
                x = false;
                $('#Commerce-required').css('display', 'none');

            }

            if (obj.Ministry_Of_Foreign_Affairs == '--select--') {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#affairs-required').css('display', 'block');
            }
            else {
                x = false;
                $('#affairs-required').css('display', 'none');
            }

            if (obj.Location_Id == 0) {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#cuntry-required').css('display', 'block');
            }
            else {
                x = false;
                $('#cuntry-required').css('display', 'none');

            }
            if (obj.Iqama_Identification == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#joftitle-required').css('display', 'block');
            }
            else {
                x = false;
                $('#joftitle-required').css('display', 'none');

            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Introduction_Certificate",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);

                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }


        }

            //-------------------------------End---------------------------------------
        else if (obj.wf_id == 'P056') {
            //Basheer on 16-03-2020
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn_p056").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.remark = $('.remark_p055').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');
                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }
                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }
                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }
                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }
            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_RelocationAllowance",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //$('.request_status_now').text('New request not submitted');
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);

                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
               
            }

        }// Save For Relocation Allowance P056 By Nimmi Mohan
        else if (obj.wf_id == 'P012') {

            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var ax = new Object();
                ax.filename = lists[i].filename;
                ax.filepath = lists[i].filepath;
                ax.filebatch = lists[i].filetype;
                obj._FileList.push(ax);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            obj.LocalEmplyee_ID = $('.employee-list-drop-id :selected').val();
            obj.Iqama_Number = $('.Iqama_Number').val();
            obj.Certificate_with_Salary = $('.Certificate_with_Salary :selected').val();
            obj.Chamber_Of_Commerce_Stamp = $('.Chamber_Of_Commerce_Stamp :selected').val();
            obj.Ministry_Of_Foreign_Affairs = $('.Ministry_Of_Foreign_Affairs :selected').val();
            obj.Location_Id = $('.wf-cuntry-list-drop-id :selected').val();
            obj.Iqama_Identification = $('.Iqama_Identification').val();
            obj.Attachment_Filepath = afterSaveCommonFilePath;


            if ($('.Iqama_Number').val() == '') {
                $('#id-IqamaNumber-required').css('display', 'block');
                $('#save_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                   
                $('#id-IqamaNumber-required').css('display', 'none');
            }

            if (obj.Certificate_with_Salary == '--select--') {
                $('#certificate_required').css('display', 'block');
                $('#save_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                  
                $('#certificate_required').css('display', 'none');
            }

            if (obj.Chamber_Of_Commerce_Stamp == '--select--') {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#Commerce-required').css('display', 'block');
            }
            else {
                x = false;
                $('#Commerce-required').css('display', 'none');

            }

            if (obj.Ministry_Of_Foreign_Affairs == '--select--') {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#affairs-required').css('display', 'block');
            }
            else {
                x = false;
                $('#affairs-required').css('display', 'none');
            }

            if (obj.Location_Id == 0) {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#cuntry-required').css('display', 'block');
            }
            else {
                x = false;
                $('#cuntry-required').css('display', 'none');

            }


            if (obj.Iqama_Identification == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#joftitle-required').css('display', 'block');
            }
            else {
                x = false;
                $('#joftitle-required').css('display', 'none');

            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Letter_To_RealEstate",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            //$('#forward_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'block');//21-04-2020 Nimmi Mohan
                            $('#print_request_btn').css('display', 'block');
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }
        }//Save for Letter to Real Estate P012 By Nimmi Mohan on 18-03-2020
        else if (obj.wf_id == 'P023') {
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var ax = new Object();
                ax.filename = lists[i].filename;
                ax.filepath = lists[i].filepath;
                ax.filebatch = lists[i].filetype;
                obj._FileList.push(ax);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            obj.reason = $('.cr_reason').val();
            obj.employee_grade = $('.cr_employee_grade').val();
            obj.joining_date = $('.cr_joining_date').val();
            obj.attachment_filepath = afterSaveCommonFilePath;
            if (obj.reason == "") {
                x = true;
                $('#reason_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                x = false;
                $('#reason_required').css('display', 'none');
            }

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_CarLoanRequest",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //$('.request_status_now').text('New request not submitted');
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }
        }    //Save For Car Loan Request P023 By Nimmi Mohan on 07-05-2020

            //-------------------------------------P015 Basheer---------------------------------
        else if (obj.wf_id == 'P015') {
            Save_P015();
        }

            //-------------------------------End---------------------------------------

        else if (obj.wf_id == 'P054') {

            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn_p054").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.remark = $('.remark_p055').val();
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);

                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');

                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);

                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }
                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }

            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();
                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');

                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }
                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }


                //  }
            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_NoSubmissionTimesheet",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },
                });
            }
            else {

                toastrError("Please fill all the mandatory fields");

            }
            // }

        }   //Save For No Submission Of Timesheet Payment(P054) By Nimmi Mohan on 24-03-2020


        else if (obj.wf_id == 'P099') {


            obj._FileList = [];  //27-03-2020
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            obj.carloanrequest_number = $('.carloan-payment-reqid').val();

            if ($(".cheque_radio_btn_p099").prop('checked') == true) {// Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.cheque_account_no = $('.cheque_account_no').val();//04-05-2020 Nimmi
                obj.supplier_to = $('.cheque_supplier').val();//04-05-2020 Nimmi
                obj.remark = $('.attachment_remark').val();//04-05-2020 Nimmi
                obj.currenctType = $('.currency_type_dropdown_id').val();//04-05-2020 Nimmi

                if (obj.carloanrequest_number == "") {
                    $('#requestno_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);

                }
                else {
                    $('#requestno_required').css('display', 'none');
                    x = false;

                }
                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');

                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);

                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }

                if (obj.cheque_date == "") {
                    x = true;
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_date_required').css('display', 'none');

                }
                if ($('.cheque_amt').val() == '') {
                    x = true;
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);

                }
                else {
                    x = false;
                    obj.attachment_filepath = afterSaveCommonFilePath;
                    $('#cheque_amt_required').css('display', 'none');
                }
                if (obj.purpose_text == "") {
                    x = true;
                    $('#cheque_purpose_text_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {
                    x = false;
                    $('#cheque_purpose_text_required').css('display', 'none');

                }
                if (obj.cheque_account_no == "") {
                    x = true;
                    $('#cheque_account_no_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {

                    x = false;
                    $('#cheque_account_no_required').css('display', 'none');
                }

                if (obj.supplier_to == "") {
                    x = true;
                    $('#cheque_supplier_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                }
                else {

                    x = false;
                    $('#cheque_supplier_required').css('display', 'none');
                }

                if (obj.carloanrequest_number == "" || obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "" || obj.cheque_account_no == "" || obj.supplier_to == "") {
                    x = true;
                }


                //20-04-2020 Nimmi



            }
            else {
                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.currenctType = $('.currency_type_dropdown_id').val(); //04-05-2020 Nimmi
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.remark = $('.attachment_remark').val();
                obj.purpose_text = $('.bank_purpose_text').val();
                //obj.remark = $('.attachment_remark').val();
                obj.to_iban = $('.to_bank_iban').val(); //04-05-2020 Nimmi




                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }

                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }

                //20-04-2020 Nimmi
                if (obj.carloanrequest_number == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#requestno_required').css('display', 'block');

                }

                else {
                    x = false;
                    $('#requestno_required').css('display', 'none');
                }

                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }


                if (obj.carloanrequest_number == "" || obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }

            }
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_CarLoanPayment",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020

                            $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                            $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            //$('.request_status_now').text('New request not submitted');
                            //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                            $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }
        }//  Save For P099 Carloan payment by Nimmi Mohan on 7-05-2020

            //A007-Accommodation in Hotel/Compound(Preema)
        else if (obj.wf_id == 'A007') {

            obj._FileList = [];


            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.application_ids = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            var ddl_AccommodationType = document.getElementById("ddl_AccommodationType");
            var AccommodationType = ddl_AccommodationType.options[ddl_AccommodationType.selectedIndex].text;


            obj.accommodation_type = AccommodationType;


            obj.hotel_name = $('.Hotel_Name').val();


            var ddl_HotelLocation = document.getElementById("ddl_HotelLocation");
            var HotelLocation = ddl_HotelLocation.options[ddl_HotelLocation.selectedIndex].text;
            obj.hotel_location = HotelLocation;

            var ddl_RoomType = document.getElementById("ddl_RoomType");
            var RoomType = ddl_RoomType.options[ddl_RoomType.selectedIndex].text;

            obj.room_type = RoomType;


            var ddl_RoomPreference = document.getElementById("ddl_RoomPreference");
            var RoomPreference = ddl_RoomPreference.options[ddl_RoomPreference.selectedIndex].text;

            obj.room_preference = RoomPreference;

            obj.no_of_room = $('.No_of_room').val();

            obj.hotel_address = $('.Hotel_Address').val();

            obj.contact_person = $('.Contact_Person').val();

            obj.fax = $('.Fax').val();

            obj.approaximate_date = $('.Approximate_Date').val();
            obj.approaximate_time = $('.Approximate_Time').val();

            var payment = document.getElementById("payment_type");
            var strPayment = payment.options[payment.selectedIndex].value;

            obj.payment_mode = strPayment;

            obj.from_period = $('.From_Period').val();

            obj.to_period = $('.To_Period').val();
            obj.remarks = $('.Remarks').val();

            var guestArray = new Array();
            $("input[name=Guest_Name]").each(function () {
                guestArray.push($(this).val());
            });
            obj.guest_name = guestArray;


            if (AccommodationType == '--Select--') {
                $('#Accommodation_Type_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Accommodation_Type_required').css('display', 'none');
            }
            if ($('.Hotel_Name').val() == '') {
                $('#Hotel_Name_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Hotel_Name_required').css('display', 'none');
            }



            if (HotelLocation == '--Select--') {
                $('#Hotel_Location_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Hotel_Location_required').css('display', 'none');
            }

            if (RoomType == '--Select--') {
                $('#Room_Type_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Room_Type_required').css('display', 'none');
            }


            if (RoomPreference == '--Select--') {
                $('#Room_Preference_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Room_Preference_required').css('display', 'none');
            }

            if ($('.No_of_room').val() == '') {
                $('#No_of_room_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#No_of_room_required').css('display', 'none');
            }

            if ($('.Hotel_Address').val() == '') {
                $('#Hotel_Address_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Hotel_Address_required').css('display', 'none');
            }

            if ($('.Contact_Person').val() == '') {
                $('#Contact_Person_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Contact_Person_required').css('display', 'none');
            }

            if ($('.Approximate_Date').val() == '') {
                $('#Approximate_Date_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Approximate_Date_required').css('display', 'none');
            }

            if ($('.Approximate_Time').val() == '') {
                $('#Approximate_Time_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Approximate_Time_required').css('display', 'none');
            }


            if (strPayment == '--Select--') {
                $('#Payment_Type_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#Payment_Type_required').css('display', 'none');
            }


            if ($('.From_Period').val() == '') {
                $('#From_Period_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#From_Period_required').css('display', 'none');
            }

            if ($('.To_Period').val() == '') {
                $('#To_Period_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#To_Period_required').css('display', 'none');
            }


            if (x == false) {

                $(".se-pre-con").show();

                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_AO_Accommodation",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {

                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;

                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);

                            public_PrintApprove = true;
                        }
                        else {

                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');

                        }

                    },

                });


            }


        }
        //    // 23/06/2020 ALENA SICS FOR A008
        //else if (obj.wf_id == 'A008') {
        //    obj._FileList = [];
        //    for (let i = 0; i < lists.length; i++) {
        //        var x = new Object();
        //        x.filename = lists[i].filename;
        //        x.filepath = lists[i].filepath;
        //        x.filebatch = lists[i].filetype;
        //        obj._FileList.push(x);
        //    }
        //    $('#application-required').css('display', 'none');
        //    obj.application_id = $('#application-list-drop-id').val();
        //    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        //    obj.creator_id = $('#emp_identify_id').val();
        //    obj.request_id = requestId;

        //    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
        //    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
        //    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

        //    obj.cost_center = CostCenter_value;
        //    // obj.cost_center = $('.Cost_Center').val();
        //    obj.pickup_at = $('.Drop_At').val();
        //    obj.employee_name = $('.Employee_Name').val();
        //    obj.date = $('.Drop_Date').val();
        //    obj.time = $('.Drop_Time').val();
        //    obj.remarks = $('.Remarks').val();
        //    //// for administration 
        //    //obj.quatity = $('.Quantity').val();
        //    //obj.Mobile_No = $('.Mobile_No').val();
        //    //obj.Employee_id = $('.Emp_Id').val();
        //    //obj.drivername = $('.GetDriver-list-drop-class : selected').val();
        //    //obj.carmodel = $('.Car_Model').val();
        //    ////end-------

        //    if (obj.cost_center == "") {
        //        x = true;
        //        $('#Cost_Center_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //    }
        //    else {
        //        $('#Cost_Center_required').css('display', 'none');
        //        x = false;
        //    }
        //    if (obj.employee_name == "") {
        //        x = true;
        //        $('#Employee_Name_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //    }
        //    else {
        //        $('#Employee_Name_required').css('display', 'none');
        //        x = false;
        //    }
        //    if (obj.pickup_at == "") {
        //        x = true;
        //        $('#Drop_At_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //    }
        //    else {

        //        $('#Drop_At_required').css('display', 'none');
        //        x = false;
        //    }
        //    if (obj.date == "") {
        //        x = true;
        //        $('#Drop_Date_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //    }
        //    else {

        //        $('#Drop_Date_required').css('display', 'none');
        //        x = false;
        //    }
        //    if (obj.time == "") {
        //        x = true;
        //        $('#Drop_Time_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //    }
        //    else {

        //        $('#Drop_Time_required').css('display', 'none');
        //        x = false;
        //    }
        //    if (obj.cost_center == "" || obj.employee_name == "" || obj.pickup_at == "" || obj.d == "" || obj.da == "" || obj.time == "") {
        //        x = true;
        //    }
        //    if (x == false) {
        //        $(".se-pre-con").show();
        //        $.ajax({
        //            type: "POST",
        //            url: "/Request/Submit_AO_EmployeePickUp",
        //            dataType: "json",
        //            global: false,
        //            data: obj,
        //            success: function (response) {
        //                if (response.Status) {
        //                    $('#save_request_btn').css('display', 'none');
        //                    $('#submit_request_btn').css('display', 'block');
        //                    requestId = response.Request_Id;

        //                    var request_full_Id = application_name + '-' + requestId;
        //                    //$('#request_id_display').text(request_full_Id);
        //                    $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
        //                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
        //                    $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
        //                    $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
        //                    $('#submit_request_btn').prop('disabled', false);
        //                    $('#cancel_request_btn').css('display', 'block');
        //                    $('#cancel_request_btn').prop('disabled', false);
        //                    $(".se-pre-con").hide();
        //                    toastrSuccess(response.Message);
        //                    //var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
        //                    var msg = $('#application_code').val() + '-' + requestId + ' not submitted'; //Basheer on 13-03-2020
        //                    $('.request_status_now').text(msg);

        //                    public_PrintApprove = true;
        //                }
        //                else {
        //                    $(".se-pre-con").hide();
        //                    toastrError(response.Message);
        //                    $('#save_request_btn').css('display', 'block');
        //                    $('#submit_request_btn').css('display', 'none');
        //                    requestId = "";
        //                    $('#submit_request_btn').prop('disabled', true);
        //                    $('#cancel_request_btn').css('display', 'block');
        //                    $('#cancel_request_btn').prop('disabled', false);

        //                    $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
        //                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
        //                }

        //            },
        //        });
        //    }
        //    else {
        //        toastrError("Please fill all the mandatory fields"); //Terrin on 14/5/2020

        //    }
        //}
        //    //A009-Arrangement of Employee Drop(Preema)
        //else if (obj.wf_id == 'A009') {

        //    obj._FileList = [];


        //    for (let i = 0; i < lists.length; i++) {
        //        var x = new Object();
        //        x.filename = lists[i].filename;
        //        x.filepath = lists[i].filepath;
        //        x.filebatch = lists[i].filetype;
        //        obj._FileList.push(x);
        //    }

        //    $('#application-required').css('display', 'none');
        //    obj.application_id = $('#application-list-drop-id').val();
        //    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        //    obj.creator_id = $('#emp_identify_id').val();
        //    obj.request_id = requestId;

        //    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
        //    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
        //    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

        //    obj.cost_center = CostCenter_value;

        //    obj.employee_name = $('.Employee_Name').val();

        //    obj.drop_at = $('.Drop_At').val();

        //    obj.date = $('.Drop_Date').val();

        //    obj.time = $('.Drop_Time').val();

        //    obj.remarks = $('.Remarks').val();


        //    //obj.quantity = $('.Quantity').val();

        //    //obj.driver_name = $('.Driver_Name').val();

        //    //obj.mobile_no = $('.Mobile_No').val();

        //    //obj.emp_id = $('.Emp_Id').val();

        //    //obj.car_model = $('.Car_Model').val();



        //    if (CostCenter_text == '-- Choose --') {
        //        $('#Cost_Center_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //        x = true;
        //        return;
        //    }
        //    else {
        //        x = false;
        //        $('#Cost_Center_required').css('display', 'none');
        //    }


        //    if ($('.Employee_Name').val() == '') {
        //        $('#Employee_Name_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //        x = true;
        //        return;
        //    }
        //    else {
        //        x = false;
        //        $('#Employee_Name_required').css('display', 'none');
        //    }

        //    if ($('.Drop_At').val() == '') {
        //        $('#Drop_At_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //        x = true;
        //        return;
        //    }
        //    else {
        //        x = false;
        //        $('#Drop_At_required').css('display', 'none');
        //    }

        //    if ($('.Drop_Time').val() == '') {
        //        $('#Drop_Time_required').css('display', 'block');
        //        $('#submit_request_btn').prop('disabled', false);
        //        x = true;
        //        return;
        //    }
        //    else {
        //        x = false;
        //        $('#Drop_Time_required').css('display', 'none');
        //    }


        //    if (x == false) {

        //        $(".se-pre-con").show();

        //        $.ajax({
        //            type: "POST",
        //            url: "/Request/Submit_AO_EmployeeDrop",
        //            dataType: "json",
        //            global: false,
        //            data: obj,
        //            success: function (response) {

        //                if (response.Status) {

        //                    $('#save_request_btn').css('display', 'none');
        //                    $('#submit_request_btn').css('display', 'block');
        //                    requestId = response.Request_Id;

        //                    var request_full_Id = application_name + '-' + requestId;

        //                    $('#request_id_display').text($('#application_code').val() + '-' + requestId);

        //                    $('#edit_request_btn').css('display', 'block');
        //                    $('#forward_request_btn').css('display', 'block');
        //                    $('#print_request_btn').css('display', 'block');

        //                    $('#submit_request_btn').prop('disabled', false);
        //                    $('#cancel_request_btn').css('display', 'block');
        //                    $('#cancel_request_btn').prop('disabled', false);
        //                    $(".se-pre-con").hide();
        //                    toastrSuccess(response.Message);

        //                    var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
        //                    $('.request_status_now').text(msg);

        //                    public_PrintApprove = true;
        //                }
        //                else {
        //                    $(".se-pre-con").hide();
        //                    toastrError(response.Message);
        //                    $('#save_request_btn').css('display', 'block');
        //                    $('#submit_request_btn').css('display', 'none');
        //                    requestId = "";
        //                    $('#submit_request_btn').prop('disabled', true);
        //                    $('#cancel_request_btn').css('display', 'block');
        //                    $('#cancel_request_btn').prop('disabled', false);
        //                    $('#edit_request_btn').css('display', 'none');
        //                    $('#forward_request_btn').css('display', 'none');

        //                }

        //            },
        //        });
        //    }

        //}

            //P049-Other Personnel Services(Preema)
        else if (obj.wf_id == 'P049') {

            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }


            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            obj.request_details = $('#txt_Request_Detail').val();

            if (obj.request_details == "") {
                $('#request_detail_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#request_detail_required').css('display', 'none');
            }

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_OtherPersonnelServices",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },

                });
            }

        }

            //P061-ESAP Contribution(Preema)
        else if (obj.wf_id == 'P061') {

            var Esap_Obj = new Object();
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            Esap_Obj.application_id = $('#application-list-drop-id').val();
            Esap_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            Esap_Obj.creator_id = $('#emp_identify_id').val();
            Esap_Obj.request_id = requestId;

            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            Esap_Obj.For_the_Period_of = $(".class-For-the-Period-of").val();
            Esap_Obj.Remarks = $("#divRemarks").text();



            var ddl_company1 = document.getElementById("1");
            var company1 = ddl_company1.options[ddl_company1.selectedIndex].text;

            var ddl_company2 = document.getElementById("2");
            var company2 = ddl_company2.options[ddl_company2.selectedIndex].text;

            var ddl_company3 = document.getElementById("3");
            var company3 = ddl_company3.options[ddl_company3.selectedIndex].text;

            var Total_Amount_in_USD_1 = $("#id-TotalAmountinUSD_1").val();
            var Total_Amount_in_USD_2 = $("#id-TotalAmountinUSD_2").val();
            var Total_Amount_in_USD_3 = $("#id-TotalAmountinUSD_3").val();

            if (Esap_Obj.For_the_Period_of == "") {
                $('#span-id-For-the-Period-of-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#span-id-For-the-Period-of-required').css('display', 'none');
            }

            if (company1 == '--Choose--') {
                $('#id-span-GetallCompany-list_1').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-GetallCompany-list_1').css('display', 'none');
            }


            if (Total_Amount_in_USD_1 == "" || Total_Amount_in_USD_1 == undefined) {
                $('#id-span-Total-Amountin-USD_1').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-Total-Amountin-USD_1').css('display', 'none');
            }

            if (company2 != '--Choose--') {
                //if (company2 == '--Choose--') {
                //    $('#id-span-GetallCompany-list_2').css('display', 'block');
                //    $('#submit_request_btn').prop('disabled', false);
                //    x = true;
                //    return;
                //}
                //else {
                //    x = false;
                //    $('#id-span-GetallCompany-list_2').css('display', 'none');
                //}

                if (Total_Amount_in_USD_2 == "" || Total_Amount_in_USD_2 == undefined) {
                    $('#id-span-Total-Amountin-USD_2').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    return;
                }
                else {
                    x = false;
                    $('#id-span-Total-Amountin-USD_2').css('display', 'none');
                }
            }
            //if (company3 == '--Choose--') {
            //    $('#id-span-GetallCompany-list_3').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    return;
            //}
            //else {
            //    x = false;
            //    $('#id-span-GetallCompany-list_3').css('display', 'none');
            //}
            if (company3 != '--Choose--') {
                if (Total_Amount_in_USD_3 == "" || Total_Amount_in_USD_3 == undefined) {
                    $('#id-span-Total-Amountin-USD_3').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    return;
                }
                else {
                    x = false;
                    $('#id-span-Total-Amountin-USD_3').css('display', 'none');
                }
            }

            var CompanyArray = new Array();
            $(".GetallCompany-list-drop-class").each(function () {
                CompanyArray.push($(this).val());
            });

            Esap_Obj.strCompany = CompanyArray;

            var PayrollArray = new Array();
            $(".Payroll_Code").each(function () {
                PayrollArray.push($(this).val());
            });

            Esap_Obj.strPayrollCode = PayrollArray;

            var TotalArray = new Array();
            $(".Class-Amount").each(function () {
                TotalArray.push($(this).val());
            });

            Esap_Obj.strTotal = TotalArray;


            Esap_Obj.Grand_Total = $(".GrandTotal").val();
            Esap_Obj.Note = $("textarea#id-textarea-Note").val();

            obj.ESAP_ContributionModel = Esap_Obj;

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_ESAP_Contribution",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },

                });
            }

        }

            //P061-ESAP Contribution(Preema)
        else if (obj.wf_id == 'P062') {

            var Esap_Obj = new Object();
            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            Esap_Obj.application_id = $('#application-list-drop-id').val();
            Esap_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            Esap_Obj.creator_id = $('#emp_identify_id').val();
            Esap_Obj.request_id = requestId;

            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            Esap_Obj.For_the_Period_of = $(".For-the-Period-of").val();
            Esap_Obj.Remarks = $('#divRemarks').text();
            Esap_Obj.Bank_Details = $('#divBankDetails').text();


            var ddl_company1 = document.getElementById("1");
            var company1 = ddl_company1.options[ddl_company1.selectedIndex].text;

            var ddl_company2 = document.getElementById("2");
            var company2 = ddl_company2.options[ddl_company2.selectedIndex].text;

            var ddl_company3 = document.getElementById("3");
            var company3 = ddl_company3.options[ddl_company3.selectedIndex].text;

            var Total_Amount_in_USD_1 = $("#id-TotalAmountinUSD_1").val();
            var Total_Amount_in_USD_2 = $("#id-TotalAmountinUSD_2").val();
            var Total_Amount_in_USD_3 = $("#id-TotalAmountinUSD_3").val();

            if (Esap_Obj.For_the_Period_of == "") {
                $('#span-id-For-the-Period-of-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#span-id-For-the-Period-of-required').css('display', 'none');
            }

            if (Esap_Obj.Remarks == "") {
                $('#span-id-remark-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#span-id-remark-required').css('display', 'none');
            }

            if (Esap_Obj.Bank_Details == "") {
                $('#span-id-bank-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#span-id-bank-required').css('display', 'none');
            }


            if (company1 == '--Choose--') {
                $('#id-span-GetallCompany-list_1').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-GetallCompany-list_1').css('display', 'none');
            }


            if (Total_Amount_in_USD_1 == "" || Total_Amount_in_USD_1 == undefined) {
                $('#id-span-Total-Amountin-USD_1').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-Total-Amountin-USD_1').css('display', 'none');
            }

            if (company2 != '--Choose--') {
                //if (company2 == '--Choose--') {
                //    $('#id-span-GetallCompany-list_2').css('display', 'block');
                //    $('#submit_request_btn').prop('disabled', false);
                //    x = true;
                //    return;
                //}
                //else {
                //    x = false;
                //    $('#id-span-GetallCompany-list_2').css('display', 'none');
                //}

                if (Total_Amount_in_USD_2 == "" || Total_Amount_in_USD_2 == undefined) {
                    $('#id-span-Total-Amountin-USD_2').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    return;
                }
                else {
                    x = false;
                    $('#id-span-Total-Amountin-USD_2').css('display', 'none');
                }
            }
            //if (company3 == '--Choose--') {
            //    $('#id-span-GetallCompany-list_3').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    return;
            //}
            //else {
            //    x = false;
            //    $('#id-span-GetallCompany-list_3').css('display', 'none');
            //}
            if (company3 != '--Choose--') {
                if (Total_Amount_in_USD_3 == "" || Total_Amount_in_USD_3 == undefined) {
                    $('#id-span-Total-Amountin-USD_3').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    return;
                }
                else {
                    x = false;
                    $('#id-span-Total-Amountin-USD_3').css('display', 'none');
                }
            }

            var CompanyArray = new Array();
            $(".GetallCompany-list-drop").each(function () {
                CompanyArray.push($(this).val());
            });

            Esap_Obj.strCompany = CompanyArray;

            var PayrollArray = new Array();
            $(".Payroll_Code").each(function () {
                PayrollArray.push($(this).val());
            });

            Esap_Obj.strPayrollCode = PayrollArray;

            var TotalArray = new Array();
            $(".Amount").each(function () {
                TotalArray.push($(this).val());
            });

            Esap_Obj.strTotal = TotalArray;


            Esap_Obj.Grand_Total = $(".GrandTotal").val();


            obj.RetirementContributionModel = Esap_Obj;

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_RetirementContribution",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },

                });
            }

        }

            //P053-GOSI Payment(Preema)
        else if (obj.wf_id == 'P053') {

            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }

            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            if ($(".cheque_radio_btn").prop('checked') == true) { // Payment mode is cheque
                obj.payment_mode = "C";
                obj.cheque_date = $('.cheque_date').val();
                obj.amount_sar = $('.cheque_amt').val();
                obj.purpose_text = $('.cheque_purpose_text').val();
                obj.attachment_filepath = $('.attachment_filepath').val();
                obj.payable_to = $('.cheque_payable_to').val();
                obj.remark = $('.remark_p055').val();

                if ($('.cheque_date').val() == '') {
                    $('#cheque_date_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    //return;
                }
                else {
                    x = false;
                    obj.cheque_date = $('.cheque_date').val();
                    $('#cheque_date_required').css('display', 'none');
                }


                if ($('.cheque_amt').val() == '') {
                    $('#cheque_amt_required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    //return;
                }
                else {
                    x = false;
                    obj.amount_sar = $('.cheque_amt').val();
                    $('#cheque_amt_required').css('display', 'none');
                }

                if (obj.purpose_text == "") {
                    $('#cheque_purpose_text_required').css('display', 'block');
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    //return;
                }
                else {
                    $('#cheque_purpose_text_required').css('display', 'none');
                    x = false;
                    obj.purpose_text = $('.cheque_purpose_text').val();
                }

                if (obj.cheque_date == "" || obj.amount_sar == "" || obj.purpose_text == "") {
                    x = true;
                }

            }

            else {

                obj.payment_mode = "B";// Payment mode is Bank
                obj.amount_sar = $('.bank_wise_amount').val();
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();
                obj.to_iban = $('.to_iban').val();

                if (obj.amount_sar == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#bank_amt_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#bank_amt_required').css('display', 'none');
                }
                if (obj.from_bank == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_name_required').css('display', 'none');

                }
                if (obj.from_addreess == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_address_required').css('display', 'none');

                }
                if (obj.from_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#from_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#from_bank_account_no_required').css('display', 'none');
                }

                if (obj.to_beneficiary == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_benificary_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_benificary_required').css('display', 'none');
                }
                if (obj.to_bankname == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_name_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_name_required').css('display', 'none');
                }

                if (obj.to_address == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_address_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_address_required').css('display', 'none');
                }
                if (obj.to_account_no == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_bank_account_no_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_bank_account_no_required').css('display', 'none');

                }

                if (obj.to_iban == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_iban_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_iban_required').css('display', 'none');

                }

                if (obj.purpose_text == "") {
                    x = true;
                    $('#submit_request_btn').prop('disabled', false);
                    $('#to_purpose_text_required').css('display', 'block');
                }
                else {
                    x = false;
                    $('#to_purpose_text_required').css('display', 'none');

                }


                if (obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
                    x = true;
                }

            }

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_GOSI_Payment",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;

                            var request_full_Id = application_name + '-' + requestId;

                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },
                });
            }
            else {
                toastrError("Please fill all the mandatory fields");
            }

        }

            //P024-Bank Loan Request(Preema)
        else if (obj.wf_id == 'P024') {

            var Bank_Obj = new Object();

            obj._FileList = [];

            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }


            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            Bank_Obj.application_id = $('#application-list-drop-id').val();
            Bank_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            Bank_Obj.creator_id = $('#emp_identify_id').val();
            Bank_Obj.request_id = requestId;

            Bank_Obj.Bank_Name = $('#txt_Bank_Name').val();
            Bank_Obj.Account_No = $('#txt_Account_No').val();
            Bank_Obj.Loan_Amount = $('#txt_Loan_Amount').val();
            Bank_Obj.Date_of_Hire = $('#txt_Date_of_Hire').val();

            var Nationality = $('input:radio[name=Nationality]:checked').val();

            Bank_Obj.Nationality = Nationality;

            Bank_Obj.Saudi_Id = $('#txt_Saudi_ID').val();
            Bank_Obj.Purpose = $('#divPurpose').text();
            Bank_Obj.End_of_Service_Benefit = "";
            Bank_Obj.As_of_Date = "";

            obj.BankLoanRequestModel = Bank_Obj;

            if (Bank_Obj.Bank_Name == "") {
                $('#Bank_Name_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Bank_Obj.Account_No == "") {
                $('#Bank_AccNo_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Bank_Obj.Loan_Amount == "") {
                $('#Loan_Amount_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Bank_Obj.Date_of_Hire == "") {
                $('#Date_of_Hire_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Bank_Obj.Nationality == "" || Bank_Obj.Nationality == undefined) {
                $('#Nationality_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Bank_Obj.Saudi_Id == "") {
                $('#Saudi_ID_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            if (Bank_Obj.Purpose == "") {
                $('#bank_purpose_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }


            if (x == true) {
                toastrError("Please fill the mandatory fields");
                return;
            }
            else {
                $('#Bank_Name_required').css('display', 'none');
                $('#Bank_AccNo_required').css('display', 'none');
                $('#Loan_Amount_required').css('display', 'none');
                $('#Date_of_Hire_required').css('display', 'none');
                $('#Nationality_required').css('display', 'none');
                $('#Saudi_ID_required').css('display', 'none');
                $('#bank_purpose_required').css('display', 'none');
                x = false;
            }

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Bank_Loan_Request",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },

                });
            }

        }

            //P016-Internal Transfer(Preema)
        else if (obj.wf_id == 'P016') {
            $('#hdn_Count').val('1');
            var Transfer_Obj = new Object();

            obj._FileList = [];

            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }


            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            Transfer_Obj.application_id = $('#application-list-drop-id').val();
            Transfer_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            Transfer_Obj.creator_id = $('#emp_identify_id').val();
            Transfer_Obj.request_id = requestId;

            if ($("#rbtn_Promotion").prop('checked') == true) {
                Transfer_Obj.Transfer_Type = "Promotion";
            }
            else {
                Transfer_Obj.Transfer_Type = "Transfer";
            }

            Transfer_Obj.Employee_Id = $('#hdn_Employee_Id').val();
            Transfer_Obj.Releasing_Manager = $('#txt_ReleasingManager').val();
            Transfer_Obj.Releasing_Manager_Id = $('#hdn_ReleasingManager_Id').val();

            var dp_ReceivingManager = document.getElementById("dp_ReceivingManager");
            Transfer_Obj.Receiving_Manager = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].text;
            Transfer_Obj.Receiving_Manager_Id = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].value;

            Transfer_Obj.Transfer_From = $('#txt_Transfer_From').val();
            Transfer_Obj.Transfer_To = $('#txt_Transfer_To').val();
            Transfer_Obj.Effective_Date = $('#txt_EffectiveDate').val();

            Transfer_Obj.From_Company = $('#txt_From_Company').val();
            Transfer_Obj.From_Company_id = $('#hdn_From_Company_Id').val();

            var dp_To_Company_Id = document.getElementById("dp_To_Company_Id");
            Transfer_Obj.To_Company = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].text;
            Transfer_Obj.To_Company_Id = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].value;

            Transfer_Obj.From_Business_Line = $('#txt_From_BusinessLine').val();
            Transfer_Obj.From_Business_Line_id = $('#hdn_From_BusinessLine_Id').val();

            var dp_To_BusinessLine = document.getElementById("dp_To_BusinessLine");
            Transfer_Obj.To_Business_Line = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].text;
            Transfer_Obj.To_Business_Line_Id = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].value;

            Transfer_Obj.From_Product_Group = $('#txt_From_ProductGroup').val();
            Transfer_Obj.From_Product_Group_id = $('#hdn_From_ProductGroup_Id').val();

            var dp_To_ProductGroup = document.getElementById("dp_To_ProductGroup");
            Transfer_Obj.To_Product_Group = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].text;
            Transfer_Obj.To_Product_Group_Id = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].value;

            Transfer_Obj.From_Department = $('#txt_From_Department').val();
            Transfer_Obj.From_Department_id = $('#hdn_From_Department_Id').val();

            var dp_To_Department = document.getElementById("dp_To_Department");
            Transfer_Obj.To_Department = dp_To_Department.options[dp_To_Department.selectedIndex].text;
            Transfer_Obj.To_Department_Id = dp_To_Department.options[dp_To_Department.selectedIndex].value;

            Transfer_Obj.From_Position = $('#txt_From_Position').val();
            Transfer_Obj.From_Position_id = $('#hdn_From_Position_Id').val();

            var dp_To_Position = document.getElementById("dp_To_Position");
            Transfer_Obj.To_Position = dp_To_Position.options[dp_To_Position.selectedIndex].text;
            Transfer_Obj.To_Position_Id = dp_To_Position.options[dp_To_Position.selectedIndex].value;

            Transfer_Obj.From_Global_Grade = $('#txt_From_Global_Grade').val();
            Transfer_Obj.From_Global_Grade_id = $('#hdn_From_Global_Grade_Id').val();

            var dp_To_Global_Grade = document.getElementById("dp_To_Global_Grade");
            Transfer_Obj.To_Global_Grade = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].text;
            Transfer_Obj.To_Global_Grade_Id = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].value;

            Transfer_Obj.From_Local_Grade = $('#txt_From_Local_Grade').val();
            Transfer_Obj.From_Local_Grade_id = $('#hdn_From_Local_Grade_Id').val();

            var dp_To_Local_Grade = document.getElementById("dp_To_Local_Grade");
            Transfer_Obj.To_Local_Grade = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].text;
            Transfer_Obj.To_Local_Grade_Id = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].value;

            Transfer_Obj.From_Cost_Center = $('#txt_From_Cost_Center').val();
            Transfer_Obj.From_Cost_Center_id = $('#hdn_From_Cost_Center_Id').val();

            var dp_To_Cost_Center = document.getElementById("dp_To_Cost_Center");
            Transfer_Obj.To_Cost_Center = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].text;
            Transfer_Obj.To_Cost_Center_Id = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].value;

            var dp_from_Status = document.getElementById("dp_From_Status");
            Transfer_Obj.From_status = dp_from_Status.options[dp_from_Status.selectedIndex].text;

            var dp_To_Status = document.getElementById("dp_To_Status");
            Transfer_Obj.To_status = dp_To_Status.options[dp_To_Status.selectedIndex].text;

            Transfer_Obj.From_Notice_Period = $('#txt_From_Notice_Period').val();
            Transfer_Obj.To_Notice_Period = $('#txt_To_Notice_Period').val();

            Transfer_Obj.From_Location = $('#txt_From_Location').val();
            Transfer_Obj.From_Location_id = $('#hdn_From_Location_Id').val();

            var dp_To_Location = document.getElementById("dp_To_Location");
            Transfer_Obj.To_Location = dp_To_Location.options[dp_To_Location.selectedIndex].text;
            Transfer_Obj.To_Location_Id = dp_To_Location.options[dp_To_Location.selectedIndex].value;

            Transfer_Obj.From_Basic_Salary = $('#txt_From_BasicSalary').val();
            Transfer_Obj.To_Basic_Salary = $('#txt_To_BasicSalary').val();
            Transfer_Obj.From_Annual_Housing = $('#txt_From_AnnualHousing').val();
            Transfer_Obj.To_Annual_Housing = $('#txt_To_AnnualHousing').val();
            Transfer_Obj.From_Car_Cost = $('#txt_From_CarCost').val();
            Transfer_Obj.To_Car_Cost = $('#txt_To_CarCost').val();
            Transfer_Obj.From_Transport = $('#txt_From_Transport').val();
            Transfer_Obj.To_Transport = $('#txt_To_Transport').val();
            Transfer_Obj.From_Travel_Allowance = $('#txt_From_TravelHardshipAllowance').val();
            Transfer_Obj.To_Travel_Allowance = $('#txt_To_TravelHardshipAllowance').val();
            Transfer_Obj.From_Mobile_Allowance = $('#txt_From_MobileAllowance').val();
            Transfer_Obj.To_Mobile_Allowance = $('#txt_To_MobileAllowance').val();

            obj.InternalTransferModel = Transfer_Obj;

            if (Transfer_Obj.Receiving_Manager == "-- Choose --") {
                $('#spn_ReceivingManager_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.Transfer_From == "") {
                $('#spn_Transfer_From_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.Transfer_To == "") {
                $('#spn_Transfer_To_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.Effective_Date == "") {
                $('#spn_EffectiveDate_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }


            if (Transfer_Obj.From_status == "-- Choose --") {
                $('#spn_From_Status_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Notice_Period == "") {
                $('#spn_From_Notice_Period_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Basic_Salary == "") {
                $('#spn_From_BasicSalary_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Annual_Housing == "") {
                $('#spn_From_AnnualHousing_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Car_Cost == "") {
                $('#spn_From_CarCost_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Transport == "") {
                $('#spn_From_Transport_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Travel_Allowance == "") {
                $('#spn_From_TravelHardshipAllowance_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Mobile_Allowance == "") {
                $('#spn_From_MobileAllowance_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (x == true) {
                toastrError("Please fill the mandatory fields");
                return;
            }
            else {
                $('#spn_ReceivingManager_required').css('display', 'none');
                $('#spn_Transfer_From_required').css('display', 'none');
                $('#spn_Transfer_To_required').css('display', 'none');
                $('#spn_EffectiveDate_required').css('display', 'none');
                $('#spn_From_Status_required').css('display', 'none');
                $('#spn_From_Notice_Period_required').css('display', 'none');
                $('#spn_From_BasicSalary_required').css('display', 'none');
                $('#spn_From_AnnualHousing_required').css('display', 'none');
                $('#spn_From_CarCost_required').css('display', 'none');
                $('#spn_From_Transport_required').css('display', 'none');
                $('#spn_From_TravelHardshipAllowance_required').css('display', 'none');
                $('#spn_From_MobileAllowance_required').css('display', 'none');
                x = false;
            }

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Internal_Transfer",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },

                });
            }

        }

            //P017-Contract Modification(Preema)
        else if (obj.wf_id == 'P017') {

            $('#hdn_Count').val('1');
            var Transfer_Obj = new Object();

            obj._FileList = [];

            for (let i = 0; i < lists.length; i++) {
                var x = new Object();
                x.filename = lists[i].filename;
                x.filepath = lists[i].filepath;
                x.filebatch = lists[i].filetype;
                obj._FileList.push(x);
            }


            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;

            Transfer_Obj.application_id = $('#application-list-drop-id').val();
            Transfer_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            Transfer_Obj.creator_id = $('#emp_identify_id').val();
            Transfer_Obj.request_id = requestId;

            if ($("#rbtn_Salary_Adjustment").prop('checked') == true) {
                Transfer_Obj.Contract_Type = " Salary Adjustment";
            }
            else {
                Transfer_Obj.Contract_Type = "Contract Renewal";
            }

            Transfer_Obj.Employee_Id = $('#hdn_Employee_Id').val();

            var dp_ReceivingManager = document.getElementById("dp_ReleasingManager");
            Transfer_Obj.Releasing_Manager = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].text;
            Transfer_Obj.Releasing_Manager_Id = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].value;

            Transfer_Obj.Effective_Date = $('#txt_EffectiveDate').val();

            Transfer_Obj.From_Company = $('#txt_From_Company').val();
            Transfer_Obj.From_Company_id = $('#hdn_From_Company_Id').val();

            var dp_To_Company_Id = document.getElementById("dp_To_Company_Id");
            Transfer_Obj.To_Company = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].text;
            Transfer_Obj.To_Company_Id = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].value;

            Transfer_Obj.From_Business_Line = $('#txt_From_BusinessLine').val();
            Transfer_Obj.From_Business_Line_id = $('#hdn_From_BusinessLine_Id').val();

            var dp_To_BusinessLine = document.getElementById("dp_To_BusinessLine");
            Transfer_Obj.To_Business_Line = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].text;
            Transfer_Obj.To_Business_Line_Id = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].value;

            Transfer_Obj.From_Product_Group = $('#txt_From_ProductGroup').val();
            Transfer_Obj.From_Product_Group_id = $('#hdn_From_ProductGroup_Id').val();

            var dp_To_ProductGroup = document.getElementById("dp_To_ProductGroup");
            Transfer_Obj.To_Product_Group = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].text;
            Transfer_Obj.To_Product_Group_Id = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].value;

            Transfer_Obj.From_Department = $('#txt_From_Department').val();
            Transfer_Obj.From_Department_id = $('#hdn_From_Department_Id').val();

            var dp_To_Department = document.getElementById("dp_To_Department");
            Transfer_Obj.To_Department = dp_To_Department.options[dp_To_Department.selectedIndex].text;
            Transfer_Obj.To_Department_Id = dp_To_Department.options[dp_To_Department.selectedIndex].value;

            Transfer_Obj.From_Position = $('#txt_From_Position').val();
            Transfer_Obj.From_Position_id = $('#hdn_From_Position_Id').val();

            var dp_To_Position = document.getElementById("dp_To_Position");
            Transfer_Obj.To_Position = dp_To_Position.options[dp_To_Position.selectedIndex].text;
            Transfer_Obj.To_Position_Id = dp_To_Position.options[dp_To_Position.selectedIndex].value;

            Transfer_Obj.From_Global_Grade = $('#txt_From_Global_Grade').val();
            Transfer_Obj.From_Global_Grade_id = $('#hdn_From_Global_Grade_Id').val();

            var dp_To_Global_Grade = document.getElementById("dp_To_Global_Grade");
            Transfer_Obj.To_Global_Grade = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].text;
            Transfer_Obj.To_Global_Grade_Id = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].value;

            Transfer_Obj.From_Local_Grade = $('#txt_From_Local_Grade').val();
            Transfer_Obj.From_Local_Grade_id = $('#hdn_From_Local_Grade_Id').val();

            var dp_To_Local_Grade = document.getElementById("dp_To_Local_Grade");
            Transfer_Obj.To_Local_Grade = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].text;
            Transfer_Obj.To_Local_Grade_Id = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].value;

            Transfer_Obj.From_Cost_Center = $('#txt_From_Cost_Center').val();
            Transfer_Obj.From_Cost_Center_id = $('#hdn_From_Cost_Center_Id').val();

            var dp_To_Cost_Center = document.getElementById("dp_To_Cost_Center");
            Transfer_Obj.To_Cost_Center = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].text;
            Transfer_Obj.To_Cost_Center_Id = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].value;

            var dp_from_Status = document.getElementById("dp_From_Status");
            Transfer_Obj.From_status = dp_from_Status.options[dp_from_Status.selectedIndex].text;

            var dp_To_Status = document.getElementById("dp_To_Status");
            Transfer_Obj.To_status = dp_To_Status.options[dp_To_Status.selectedIndex].text;

            Transfer_Obj.From_Notice_Period = $('#txt_From_Notice_Period').val();
            Transfer_Obj.To_Notice_Period = $('#txt_To_Notice_Period').val();

            Transfer_Obj.From_Location = $('#txt_From_Location').val();
            Transfer_Obj.From_Location_id = $('#hdn_From_Location_Id').val();

            var dp_To_Location = document.getElementById("dp_To_Location");
            Transfer_Obj.To_Location = dp_To_Location.options[dp_To_Location.selectedIndex].text;
            Transfer_Obj.To_Location_Id = dp_To_Location.options[dp_To_Location.selectedIndex].value;

            Transfer_Obj.From_Basic_Salary = $('#txt_From_BasicSalary').val();
            Transfer_Obj.To_Basic_Salary = $('#txt_To_BasicSalary').val();
            Transfer_Obj.From_Annual_Housing = $('#txt_From_AnnualHousing').val();
            Transfer_Obj.To_Annual_Housing = $('#txt_To_AnnualHousing').val();
            Transfer_Obj.From_Car_Cost = $('#txt_From_CarCost').val();
            Transfer_Obj.To_Car_Cost = $('#txt_To_CarCost').val();
            Transfer_Obj.From_Transport = $('#txt_From_Transport').val();
            Transfer_Obj.To_Transport = $('#txt_To_Transport').val();
            Transfer_Obj.From_Travel_Allowance = $('#txt_From_TravelHardshipAllowance').val();
            Transfer_Obj.To_Travel_Allowance = $('#txt_To_TravelHardshipAllowance').val();
            Transfer_Obj.From_Mobile_Allowance = $('#txt_From_MobileAllowance').val();
            Transfer_Obj.To_Mobile_Allowance = $('#txt_To_MobileAllowance').val();

            obj.ContractModificationModel = Transfer_Obj;

            if (Transfer_Obj.Releasing_Manager == "-- Choose --") {
                $('#spn_ReleasingManager_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.Effective_Date == "") {
                $('#spn_EffectiveDate_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_status == "-- Choose --") {
                $('#spn_From_Status_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Notice_Period == "") {
                $('#spn_From_Notice_Period_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Basic_Salary == "") {
                $('#spn_From_BasicSalary_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Annual_Housing == "") {
                $('#spn_From_AnnualHousing_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Car_Cost == "") {
                $('#spn_From_CarCost_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Transport == "") {
                $('#spn_From_Transport_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Travel_Allowance == "") {
                $('#spn_From_TravelHardshipAllowance_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (Transfer_Obj.From_Mobile_Allowance == "") {
                $('#spn_From_MobileAllowance_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }

            if (x == true) {
                toastrError("Please fill the mandatory fields");
                return;
            }
            else {
                $('#spn_ReleasingManager_required').css('display', 'none');
                $('#spn_EffectiveDate_required').css('display', 'none');
                $('#spn_From_Status_required').css('display', 'none');
                $('#spn_From_Notice_Period_required').css('display', 'none');
                $('#spn_From_BasicSalary_required').css('display', 'none');
                $('#spn_From_AnnualHousing_required').css('display', 'none');
                $('#spn_From_CarCost_required').css('display', 'none');
                $('#spn_From_Transport_required').css('display', 'none');
                $('#spn_From_TravelHardshipAllowance_required').css('display', 'none');
                $('#spn_From_MobileAllowance_required').css('display', 'none');
                x = false;
            }

            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_Contract_Modification",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {

                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');

                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);

                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }

                    },

                });
            }

        }

        else if (obj.wf_id == 'P025') {

            obj._FileList = [];
            for (let i = 0; i < lists.length; i++) {
                var ex = new Object();
                ex.filename = lists[i].filename;
                ex.filepath = lists[i].filepath;
                ex.filebatch = lists[i].filetype;
                obj._FileList.push(ex);
            }
            $('#application-required').css('display', 'none');
            obj.application_id = $('#application-list-drop-id').val();
            obj.emp_local_id = $('.employee-list-drop-id :selected').val();
            obj.creator_id = $('#emp_identify_id').val();
            obj.request_id = requestId;
            obj.reason_clearance = $('.Endofservice_reasonfor_clearance').val();
            obj.termination_Date = $('.terminatn_date').val();
            obj.eb_Toolbox_Returned = $('.toolbx_returned').val();
            obj.eb_Workstation_Cleared = $('.workstation_cleared').val();
            obj.eb_OfficialBusiness_Documents = $('.official_business_doc').val();
            obj.eb_SiteProject_Clearance = $('.site_project_clearance').val();
            obj.eb_Uniform = $('.uni_form').val();
            obj.eb_Safety_Equipment = $('.safety_equipment').val();
            obj.eb_AllWorkflow_Approvals = $('.workflow_approvals').val();
            obj.eb_ISService_Deactivation_Date = $('.is_service_deactivtn_date').val();
            obj.eb_Assigned_Delegate = $('.assigned_delegate').val();
            obj.ad_HousingHousehold_cleared = $('.housing_household').val();
            obj.ad_Util_Water_cleared = $('.utilities_water_etc').val();
            obj.ad_CarGarageKey_Returned = $('.car_garage_key').val();
            obj.ad_Gatepass_Returned = $('.gatepass_sticker').val();
            obj.ad_Mobile_SimCard_Returned = $('.Mobile_returned').val();
            obj.ad_CompanyID_Returned = $('.company_id_returned').val();
            obj.tr_Amount_SAR = $('.sar_amount').val();
            obj.tr_ExternalTraining_Cost = $('.ExternalTraining_Cost').val();
            obj.is_Desktop_Returned = $('.desktop_etc_cleared').val();
            obj.ft_Clearance_Obtained = $('.clearance_obtained_fin').val();
            obj.ft_eBank_Token = $('.e_bank_token').val();
            obj.ae_Clearance_Obtained = $('.clearance_obtained_ae').val();
            obj.hr_CarLoan_Cleared = $('.car_loan_cleared').val();
            obj.hr_SalaryAdvances_Settled = $('.salary_advance_settled').val();
            obj.hr_CompanyStamp_Returned = $('.company_stamp_returned').val();
            obj.hr_MedicalInsurance_Returned = $('.medical_insurance').val();
            obj.hr_Visa_Mastercard_Communicated = $('.visa_master_commun').val();
            obj.hr_Savingcurrent_communicated = $('.sav_current_account').val();
            obj.hr_Remarks = $('.res_remarks').val();
            obj.hr_Attachment_Filepath = afterSaveCommonFilePath;

            //05-05-2020
            obj.eb_toolbx_returned_date = $('.toolbx_returned_date').val();
            obj.workstation_cleared_date = $('.workstation_cleared_date').val();
            obj.official_business_doc_date = $('.official_business_doc_date').val();
            obj.site_project_clearance_date = $('.site_project_clearance_date').val();
            obj.uni_form_date = $('.uni_form_date').val();
            obj.safety_equipment_date = $('.safety_equipment_date').val();
            obj.workflow_approvals_date = $('.workflow_approvals_date').val();
            obj.housing_household_date = $('.housing_household_date').val();
            obj.car_garage_key_date = $('.car_garage_key_date').val();
            obj.gatepass_sticker_date = $('.gatepass_sticker_date').val();
            obj.Mobile_returned_date = $('.Mobile_returned_date').val();
            obj.company_id_returned_date = $('.company_id_returned_date').val();
            obj.utilities_water_etc_date = $('.utilities_water_etc_date').val();
            obj.desktop_etc_cleared_date = $('.desktop_etc_cleared_date').val();
            obj.clearance_obtained_fin_date = $('.clearance_obtained_fin_date').val();
            obj.e_bank_token_date = $('.e_bank_token_date').val();
            obj.clearance_obtained_ae_date = $('.clearance_obtained_ae_date').val();
            obj.car_loan_cleared_date = $('.car_loan_cleared_date').val();
            obj.salary_advance_settled_date = $('.salary_advance_settled_date').val();
            obj.company_stamp_returned_date = $('.company_stamp_returned_date').val();
            obj.medical_insurance_date = $('.medical_insurance_date').val();
            obj.visa_master_commun_date = $('.visa_master_commun_date').val();
            obj.sav_current_account_date = $('.sav_current_account_date').val();
            obj.external_training_cost_date = $('.ExternalTraining_Cost_date').val();


            //Terrin 11-06-2020
            
         
          
            if (obj.reason_clearance == "") {
                
                $('#Endofservice_reasonfor_clearance-required').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    x = true;
                    //return;
                }
                else {
                    x = false
                    $('#Endofservice_reasonfor_clearance-required').css('display', 'none');
                }
            if (obj.termination_Date == "") {

                $('#terminatn_date-required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                //return;
            }
            else {
                x = false
                $('#terminatn_date-required').css('display', 'none');
            }


            //if (obj.eb_Toolbox_Returned == "--select--") {

            //    $('#toolbx_returned-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#toolbx_returned-required').css('display', 'none');
            //}

            //if (obj.eb_Workstation_Cleared == "--select--") {

            //    $('#workstation_cleared-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#workstation_cleared-required').css('display', 'none');
            //}


            //if (obj.eb_OfficialBusiness_Documents == "--select--") {

            //    $('#official_business_doc-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#official_business_doc-required').css('display', 'none');
            //}
            

            //if (obj.eb_SiteProject_Clearance == "--select--") {

            //    $('#site_project_clearance-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#site_project_clearance-required').css('display', 'none');
            //}

           

            //if (obj.eb_Uniform == "--select--") {

            //    $('#uni_form-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#uni_form-required').css('display', 'none');
            //}


            //if (obj.eb_Safety_Equipment == "--select--") {

            //    $('#safety_equipment-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#safety_equipment-required').css('display', 'none');
            //}


            //if (obj.eb_AllWorkflow_Approvals == "") {

            //    $('#workflow_approvals-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#workflow_approvals-required').css('display', 'none');
            //}
           
            //if (obj.eb_ISService_Deactivation_Date == "") {

            //    $('#is_service_deactivtn_date-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#is_service_deactivtn_date-required').css('display', 'none');
            //}
            //if (obj.eb_Assigned_Delegate == "") {

            //    $('#assigned_delegate-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#assigned_delegate-required').css('display', 'none');
            //}
            //if (obj.ad_HousingHousehold_cleared == "--select--") {

            //    $('#housing_household-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#housing_household-required').css('display', 'none');
            //}

            //if (obj.ad_Util_Water_cleared == "--select--") {

            //    $('#utilities_water_etc-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#utilities_water_etc-required').css('display', 'none');
            //}

            //if (obj.ad_CarGarageKey_Returned == "--select--") {

            //    $('#car_garage_key-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#car_garage_key-required').css('display', 'none');
            //}

            //if (obj.ad_Gatepass_Returned == "--select--") {

            //    $('#gatepass_sticker-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#gatepass_sticker-required').css('display', 'none');
            //}
            //if (obj.ad_Mobile_SimCard_Returned == "--select--") {

            //    $('#Mobile_returned-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#Mobile_returned-required').css('display', 'none');
            //}
            
            //if (obj.ad_CompanyID_Returned == "--select--") {

            //    $('#company_id_returned-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#company_id_returned-required').css('display', 'none');
            //}
            //if (obj.tr_Amount_SAR == "") {

            //    $('#sar_amount-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#sar_amount-required').css('display', 'none');
            //}
            //if (obj.tr_ExternalTraining_Cost == "--select--") {

            //    $('#ExternalTraining_Cost-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#ExternalTraining_Cost-required').css('display', 'none');
            //}
            
            //if (obj.is_Desktop_Returned == "--select--") {

            //    $('#desktop_etc_cleared-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#desktop_etc_cleared-required').css('display', 'none');
            //}
            //if (obj.ft_Clearance_Obtained == "--select--") {

            //    $('#clearance_obtained_fin-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#clearance_obtained_fin-required').css('display', 'none');
            //}
            //if (obj.ft_eBank_Token == "--select--") {

            //    $('#e_bank_token-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#e_bank_token-required').css('display', 'none');
            //}
            
            //if (obj.ae_Clearance_Obtained == "--select--") {

            //    $('#clearance_obtained_ae-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#clearance_obtained_ae-required').css('display', 'none');
            //}
            //if (obj.hr_CarLoan_Cleared == "--select--") {

            //    $('#car_loan_cleared-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#car_loan_cleared-required').css('display', 'none');
            //}
            //if (obj.hr_SalaryAdvances_Settled == "--select--") {

            //    $('#salary_advance_settled-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#salary_advance_settled-required').css('display', 'none');
            //}
            //if (obj.hr_CompanyStamp_Returned == "--select--") {

            //    $('#company_stamp_returned-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#company_stamp_returned-required').css('display', 'none');
            //}
           
            //if (obj.hr_MedicalInsurance_Returned == "--select--") {

            //    $('#medical_insurance-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#medical_insurance-required').css('display', 'none');
            //}
            //if (obj.hr_Visa_Mastercard_Communicated == "--select--") {

            //    $('#visa_master_commun-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#visa_master_commun-required').css('display', 'none');
            //}
            

            //if (obj.hr_Savingcurrent_communicated == "--select--") {

            //    $('#sav_current_account-required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    //return;
            //}
            //else {
            //    x = false
            //    $('#sav_current_account-required').css('display', 'none');
            //}

           



            //if (obj.eb_Toolbox_Returned == 'Cleared') {
            //    if (obj.eb_toolbx_returned_date == "") {
            //        $('#toolbx_returned_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#toolbx_returned_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#toolbx_returned_date-required').css('display', 'none');
            //}
            //if (obj.eb_Workstation_Cleared == 'Cleared') {
            //    if (obj.workstation_cleared_date == "") {
            //        $('#workstation_cleared_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#workstation_cleared_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#workstation_cleared_date-required').css('display', 'none');
            //}

            //if (obj.eb_OfficialBusiness_Documents == 'Cleared') {
            //    if (obj.official_business_doc_date == "") {
            //        $('#official_business_doc_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#official_business_doc_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#official_business_doc_date-required').css('display', 'none');
            //}

            //if (obj.eb_SiteProject_Clearance == 'Cleared') {
            //    if (obj.site_project_clearance_date == "") {
            //        $('#site_project_clearance_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#site_project_clearance_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#site_project_clearance_date-required').css('display', 'none');
            //}
          
            //if (obj.eb_Uniform == 'Cleared') {
            //    if (obj.uni_form_date == "") {
            //        $('#uni_form_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#uni_form_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#uni_form_date-required').css('display', 'none');
            //}

            //if (obj.eb_Safety_Equipment == 'Cleared') {
            //    if (obj.safety_equipment_date == "") {
            //        $('#safety_equipment_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#safety_equipment_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#safety_equipment_date-required').css('display', 'none');
            //}
            //if (obj.eb_AllWorkflow_Approvals == 'Cleared') {
            //    if (obj.workflow_approvals_date == "") {
            //        $('#workflow_approvals_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#workflow_approvals_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#workflow_approvals_date-required').css('display', 'none');
            //}
            //if (obj.ad_HousingHousehold_cleared == 'Cleared') {
            //    if (obj.housing_household_date == "") {
            //        $('#housing_household_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#housing_household_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#housing_household_date-required').css('display', 'none');
            //}

            //if (obj.ad_CarGarageKey_Returned == 'Cleared') {
            //    if (obj.car_garage_key_date == "") {
            //        $('#car_garage_key_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#car_garage_key_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#car_garage_key_date-required').css('display', 'none');
            //}


            //if (obj.ad_Gatepass_Returned == 'Cleared') {
            //    if (obj.gatepass_sticker_date == "") {
            //        $('#gatepass_sticker_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#gatepass_sticker_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#gatepass_sticker_date-required').css('display', 'none');
            //}

            //if (obj.ad_Mobile_SimCard_Returned == 'Cleared') {
            //    if (obj.Mobile_returned_date == "") {
            //        $('#Mobile_returned_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#Mobile_returned_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#Mobile_returned_date-required').css('display', 'none');
            //}
         
            //if (obj.ad_CompanyID_Returned == 'Cleared') {
            //    if (obj.company_id_returned_date == "") {
            //        $('#company_id_returned_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#company_id_returned_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#company_id_returned_date-required').css('display', 'none');
            //}
            //if (obj.ad_Util_Water_cleared == 'Cleared') {
            //    if (obj.utilities_water_etc_date == "") {
            //        $('#utilities_water_etc_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#utilities_water_etc_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#utilities_water_etc_date-required').css('display', 'none');
            //}
           
            //if (obj.is_Desktop_Returned == 'Cleared') {
            //    if (obj.desktop_etc_cleared_date == "") {
            //        $('#desktop_etc_cleared_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#desktop_etc_cleared_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#desktop_etc_cleared_date-required').css('display', 'none');
            //}

            //if (obj.ft_Clearance_Obtained == 'Cleared') {
            //    if (obj.clearance_obtained_fin_date == "") {
            //        $('#clearance_obtained_fin_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#clearance_obtained_fin_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#clearance_obtained_fin_date-required').css('display', 'none');
            //}
            //if (obj.ft_eBank_Token == 'Cleared') {
            //    if (obj.e_bank_token_date == "") {
            //        $('#e_bank_token_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#e_bank_token_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#e_bank_token_date-required').css('display', 'none');
            //}

            //if (obj.ae_Clearance_Obtained == 'Cleared') {
            //    if (obj.clearance_obtained_ae_date == "") {
            //        $('#clearance_obtained_ae_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#e_bank_token_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#clearance_obtained_ae_date-required').css('display', 'none');
            //}
          
            //if (obj.hr_CarLoan_Cleared == 'Cleared') {
            //    if (obj.car_loan_cleared_date == "") {
            //        $('#car_loan_cleared_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#car_loan_cleared_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#car_loan_cleared_date-required').css('display', 'none');
            //}

            //if (obj.hr_SalaryAdvances_Settled == 'Cleared') {
            //    if (obj.salary_advance_settled_date == "") {
            //        $('#salary_advance_settled_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#salary_advance_settled_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#salary_advance_settled_date-required').css('display', 'none');
            //}
            //if (obj.hr_CompanyStamp_Returned == 'Cleared') {
            //    if (obj.company_stamp_returned_date == "") {
            //        $('#company_stamp_returned_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#company_stamp_returned_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#company_stamp_returned_date-required').css('display', 'none');
            //}
            //if (obj.hr_MedicalInsurance_Returned == 'Cleared') {
            //    if (obj.medical_insurance_date == "") {
            //        $('#medical_insurance_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#medical_insurance_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#medical_insurance_date-required').css('display', 'none');
            //}
          
            //if (obj.hr_Visa_Mastercard_Communicated == 'Cleared') {
            //    if (obj.visa_master_commun_date == "") {
            //        $('#visa_master_commun_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#visa_master_commun_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#visa_master_commun_date-required').css('display', 'none');
            //}
            //if (obj.hr_Savingcurrent_communicated == 'Cleared') {
            //    if (obj.sav_current_account_date == "") {
            //        $('#sav_current_account_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#sav_current_account_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#sav_current_account_date-required').css('display', 'none');
            //}
            //if (obj.tr_ExternalTraining_Cost == 'Cleared') {
            //    if (obj.external_training_cost_date == "") {
            //        $('#ExternalTraining_Cost_date-required').css('display', 'block');
            //        $('#submit_request_btn').prop('disabled', false);
            //        x = true;
            //        //return;
            //    }
            //    else {
            //        x = false
            //        $('#ExternalTraining_Cost_date-required').css('display', 'none');
            //    }
            //}
            //else {
            //    x = false
            //    $('#ExternalTraining_Cost_date-required').css('display', 'none');
            //}

            //if (obj.reason_clearance == "" || obj.termination_Date == "" || obj.eb_Toolbox_Returned == "--select--" || obj.eb_Workstation_Cleared == "--select--" || obj.eb_OfficialBusiness_Documents == "--select--" || obj.eb_SiteProject_Clearance == "--select--" || obj.eb_Uniform == "--select--" || obj.eb_Safety_Equipment == "--select--" || obj.eb_AllWorkflow_Approvals == "" || obj.eb_ISService_Deactivation_Date == "" || obj.eb_Assigned_Delegate == "" || obj.ad_HousingHousehold_cleared == "--select--" || obj.ad_Util_Water_cleared == "--select--" || obj.ad_CarGarageKey_Returned == "--select--" || obj.ad_Gatepass_Returned == "--select--" || obj.ad_Mobile_SimCard_Returned == "--select--" || obj.ad_CompanyID_Returned == "--select--" || obj.tr_Amount_SAR == "" || obj.tr_ExternalTraining_Cost == "--select--" || obj.is_Desktop_Returned == "--select--" || obj.ft_Clearance_Obtained == "--select--" || obj.ft_eBank_Token == "--select--" || obj.ae_Clearance_Obtained == "--select--" || obj.hr_CarLoan_Cleared == "--select--" || obj.hr_SalaryAdvances_Settled == "--select--" || obj.hr_CompanyStamp_Returned == "--select--" || obj.hr_MedicalInsurance_Returned == "--select--" || obj.hr_Visa_Mastercard_Communicated == "--select--" || obj.hr_Savingcurrent_communicated == "--select--") {
            //    x = true;
            //}


            if (obj.reason_clearance == "" || obj.termination_Date == "" ) {
                x = true;
            }

            

            //obj.attachment_filepath = afterSaveCommonFilePath; 05-04-2020 Nimmi
            if (x == false) {
                $(".se-pre-con").show();
                $.ajax({
                    type: "POST",
                    url: "/Request/Submit_PP_EndofServiceClearance",
                    dataType: "json",
                    global: false,
                    data: obj,
                    success: function (response) {
                        if (response.Status) {
                            $('#save_request_btn').css('display', 'none');
                            $('#submit_request_btn').css('display', 'block');
                            requestId = response.Request_Id;
                            var request_full_Id = application_name + '-' + requestId;
                            //$('#request_id_display').text(request_full_Id);
                            $('#request_id_display').text($('#application_code').val() + '-' + requestId);
                            $('#edit_request_btn').css('display', 'block');
                            $('#forward_request_btn').css('display', 'block');
                            $('#print_request_btn').css('display', 'block');
                            $('#submit_request_btn').prop('disabled', false);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $(".se-pre-con").hide();
                            toastrSuccess(response.Message);
                            var msg = $('#application_code').val() + '-' + requestId + ' not submitted';
                            $('.request_status_now').text(msg);
                            public_PrintApprove = true;
                        }
                        else {
                            $(".se-pre-con").hide();
                            toastrError(response.Message);
                            $('#save_request_btn').css('display', 'block');
                            $('#submit_request_btn').css('display', 'none');
                            requestId = "";
                            $('#submit_request_btn').prop('disabled', true);
                            $('#cancel_request_btn').css('display', 'block');
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#edit_request_btn').css('display', 'none');
                            $('#forward_request_btn').css('display', 'none');
                        }
                    },
                });
            }
            else {
           
                toastrError("Please fill all the mandatory fields");
            
            }
        } // Save For P025 EndofServiceClearance By Nimmi Mohan on 07-05-2020


    }
});
// Request Submit ,while create the approver data********************SUBMIT BUTTON CLICK HERE *********************************
$(document).on('click', '#submit_request_btn', function () {

    var x = false;
    $('#submit_request_btn').prop('disabled', true);
    $('#forward_request_btn').css('display', 'none');
    $('#cancel_request_btn').prop('disabled', true);

    //var emp_localId = $('.employee-list-drop-id :selected').val(); //Basheer on 25-03-2020
    var emp_localId = $('#emp_identify_id').val();

    var wf_id = $('.wf-types-list-drop-id :selected').val();
    var id = requestId + '~' + emp_localId + '~' + wf_id;

    $('#edit_request_btn').css('display', 'none'); //Basheer on 30-01-2020
    $('#submit_request_btn').css('display', 'none');//Basheer on 30-01-2020
    $('#cancel_request_btn').css('display', 'none'); //Basheer on 30-01-2020
    $('#print_request_btn').css('display', 'none');//Basheer on 30-01-2020

    //EditRequest();
    $.ajax({
        url: '/Request/SubmitRequestForApprove?id=' + id,
        type: "GET",
        success: function (result) {
            if (result.Status) {

                $('#forward_request_btn').css('display', 'block');
                toastrSuccess('Your request successfully submitted');
                request_table_id = result.Request_Table_Id;
                $('#submit_request_btn').css('display', 'none');
                $('#cancel_request_btn').css('display', 'block');
                $('#cancel_request_btn').prop('disabled', false);
                //$('.request_status_now').text('Request submitted for approval');
                //var msg = 'Request ' +$('#application_code').val() + '-'+  requestId + ' submitted for approval';
                var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' submitted by ' + result.statusemployeename + ' for approval of ' + result.statusroldesc; //Basheer on 13-03-2020
                $('.request_status_now').text(msg);

                $('#edit_request_btn').css('display', 'none'); //Basheer on 23-01-2020
                $('#print_request_btn').css('display', 'block');

            }
            else {

                console.log(result.Message);
                toastrError("Request not submited!");
                $('#submit_request_btn').prop('disabled', true);
                $('#submit_request_btn').css('display', 'none');
                $('#save_request_btn').css('display', 'block');
                $('#save_request_btn').prop('disabled', false);
                $('#forward_request_btn').prop('disabled', true);
                $('#forward_request_btn').css('display', 'none');
                $('#cancel_request_btn').css('display', 'block');
                $('#cancel_request_btn').prop('disabled', false);
                $('#edit_request_btn').prop('disabled', true);//Basheer on 23-01-2020
                $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                $('#print_request_btn').css('display', 'block');
            }
        }
    });

});

// Request Edit Before Submition and after the save******************EDIT BUTTON CLICK HERE ***********************************
function EditRequest() {
    var obj = new Object();
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    if (obj.wf_id == 'P055') {
        //25-02-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.remark = $('.remark_p055').val();
            if (obj.cheque_date == "") {
                x = true;
                $('#cheque_date_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                // return;
            }
            else {
                x = false;
                $('#cheque_date_required').css('display', 'none');

            }
            if ($('.cheque_amt').val() == '') {
                x = true;
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                //return;

            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.purpose_text == "") {
                x = true;
                $('#cheque_purpose_text_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                // return;
            }
            else {
                x = false;
                $('#cheque_purpose_text_required').css('display', 'none');

            }


            //if (obj.payable_to == "") {
            //    x = true;
            //    $('#cheque_payable_required').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    return;
            //}
            //else {

            //    x = false;
            //    $('#cheque_payable_required').css('display', 'none');
            //}


        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            obj.from_bank = $('.from_bank_name').val();
            obj.from_addreess = $('.from_bank_address').val();
            obj.from_account_no = $('.from_bank_account_no').val();
            obj.to_beneficiary = $('.to_bank_benificiary').val();
            obj.to_bankname = $('.to_bank_name').val();
            obj.to_address = $('.to_bank_address').val();
            obj.to_account_no = $('.to_bank_account_no').val();
            obj.to_iban = $('.to_iban').val();
            //obj.bank_attachment = afterSaveBankFilePath;
            obj.attachment_filepath = afterSaveCommonFilePath;
            obj.purpose_text = $('.bank_remark').val();
            obj.remark = $('.remark_p055').val();

            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
                // return;
            }
            else {
                x = false;
                $('#bank_amt_required').css('display', 'none');
            }
            if (obj.from_bank == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_bank_name_required').css('display', 'block');
                // return;
            }
            else {
                x = false;
                $('#from_bank_name_required').css('display', 'none');

            }
            if (obj.from_addreess == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_address_required').css('display', 'block');
                //return;
            }
            else {
                x = false;
                $('#from_address_required').css('display', 'none');

            }
            if (obj.from_account_no == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_bank_account_no_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#from_bank_account_no_required').css('display', 'none');
            }

            if (obj.to_beneficiary == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_benificary_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#to_benificary_required').css('display', 'none');
            }
            if (obj.to_bankname == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_bank_name_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#to_bank_name_required').css('display', 'none');
            }

            if (obj.to_address == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_address_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#to_address_required').css('display', 'none');
            }
            if (obj.to_account_no == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_bank_account_no_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#to_bank_account_no_required').css('display', 'none');

            }

            if (obj.to_iban == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_iban_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#to_iban_required').css('display', 'none');

            }

            if (obj.purpose_text == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_purpose_text_required').css('display', 'block');
                return;
            }
            else {
                x = false;
                $('#to_purpose_text_required').css('display', 'none');

            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_BalanceHousingAllowance_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 30-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 30-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 30-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 30-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 30-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 30-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }
    else if (obj.wf_id == 'P057') {
        // Terrin on 25-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btnp_P057").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.remark = $('.remark_p055').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_bank_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();

            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_SalaryforEmployeeUnderIqamaprocessPayment_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 30-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 30-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 30-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 30-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 30-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 30-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }
        // 14/05/2020 ALENA SICS EOSB CALCULATION
    else if (obj.wf_id == 'P052') {
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn_P052").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date_P052').val();
            obj.amount_sar = $('.cheque_amt_P052').val();
            obj.purpose_text = $('.cheque_purpose_P052').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to_P052').val();
            obj.remark = $('.cheque_remark_P052').val();
            // 24/05/2020 code commented by Alena Sics and added new below
            //obj.endofservice = $('.endofservice').val();
            obj.endofservice = $('.endrequest-reqid').val();
            if ($('.cheque_amt_P052').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_amount_P052').val(); //28/05/2020 alena
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_P052').val();
                obj.from_addreess = $('.from_address_P052').val();
                obj.from_account_no = $('.from_accountno_P052').val();
                obj.to_beneficiary = $('.to_benificiary_P052').val();
                obj.to_bankname = $('.to_bank_P052').val();
                obj.to_address = $('.to_address_P052').val();
                obj.to_account_no = $('.to_accountno_P052').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.remark = $('.bank_remark_P052').val();
                obj.to_iban = $('.to_iban_P052').val();
                obj.purpose_text = $('.bank_purpose_P052').val();
                // 24/05/2020 code commented by Alena Sics and added new below
                //obj.endofservice = $('.bankendofservice_P052').val();
                obj.endofservice = $('.endrequest-reqid').val();
                obj.amount_sar = $('.bank_amount_P052').val();
            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_EOSBCalculation_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }

    }
        //END
    else if (obj.wf_id == 'IS05') {
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.change_summary = $('.change_summary_text').val();
        if (obj.change_summary == '') {
            toastrError("Please enter the Change Summary!");
            $('#cancel_request_btn').prop('disabled', false);
            $('#save_request_btn').prop('disabled', false);
            $('.change_summary_text').focus();
        }
        else {
            obj.detailed_description = $('.detailed_description_text').val();
            if (obj.detailed_description == '') {
                toastrError("Please enter the detailed description !");
                $('#cancel_request_btn').prop('disabled', false);
                $('#save_request_btn').prop('disabled', false);
                $('.detailed_description_text').focus();
            }
            else {
                obj.proposed_plan = $('.proposed_plan_text').val();
                if (obj.proposed_plan == '') {
                    toastrError("Please enter the proposed plan !");
                    $('#cancel_request_btn').prop('disabled', false);
                    $('#save_request_btn').prop('disabled', false);
                    $('.proposed_plan_text').focus();
                }
                else {
                    obj.impct = $('.business_system_impact_analysis_text').val();
                    if (obj.impct == '') {
                        toastrError("Please enter the business/system impact analysis !");
                        $('#cancel_request_btn').prop('disabled', false);
                        $('#save_request_btn').prop('disabled', false);
                        $('.business_system_impact_analysis_text').focus();
                    }
                    else {
                        obj.fallback_options = $('.fallback_options_text').val();
                        if (obj.fallback_options == '') {
                            toastrError("Please enter the Fallback Options !");
                            $('#cancel_request_btn').prop('disabled', false);
                            $('#save_request_btn').prop('disabled', false);
                            $('.fallback_options_text').focus();
                        }
                        else {
                            obj.positive_risk_assessment = $('.positive_risk_assessments_text').val();
                            if (obj.positive_risk_assessment == '') {
                                toastrError("Please enter the positive risk !");
                                $('#cancel_request_btn').prop('disabled', false);
                                $('#save_request_btn').prop('disabled', false);
                                $('.positive_risk_assessments_text').focus();
                            }
                            else {
                                obj.negative_risk_assessment = $('.negative_risk_assessments_text').val();
                                if (obj.negative_risk_assessment == '') {
                                    toastrError("Please enter the negative !");
                                    $('#cancel_request_btn').prop('disabled', false);
                                    $('#save_request_btn').prop('disabled', false);
                                    $('.negative_risk_assessments_text').focus();
                                }
                                else {
                                    obj.file_path = afterSaveCommonFilePath;
                                    if (obj.file_path == '') {
                                        toastrError("Please select the file !");
                                        $('#cancel_request_btn').prop('disabled', false);
                                        $('#save_request_btn').prop('disabled', false);
                                    }
                                    obj.clarification = $('#request_clarification_drop :selected').val();
                                    console.log(obj);
                                    $(".se-pre-con").show();
                                    $.ajax({
                                        type: "POST",
                                        url: "/IS/Submit_IS_InfrastructureChangeManagement_Edit_After_Save",
                                        dataType: "json",
                                        global: false,
                                        data: obj,
                                        success: function (response) {
                                            if (response.Status) {
                                                $(".se-pre-con").hide();
                                                toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                                                $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                                                $('#forward_request_btn').css('display', 'none');//Basheer on 30-01-2020
                                                $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                                                $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                                                $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                                                $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                                            }
                                            else {
                                                $(".se-pre-con").hide();
                                            }

                                        },
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    else if (obj.wf_id == 'P009') {
        //Basheer on 16-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn_P009").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.cheque_account_no = $('.cheque_account_no').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.supplier_to = $('.cheque_supplier').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.remark = $('.attachment_remark').val();
            obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_bank_iban').val();
                obj.purpose_text = $('.bank_remark').val();
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.remark = $('.attachment_remark').val();
            }
        }
        //----------------------------------------------------------------
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_HRRelated_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {

                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }

                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
            $('#cancel_request_btn').css('display', 'block');
            $('#cancel_request_btn').prop('disabled', false);
        }
        //----------------------------------------------------------------
    }// HR Related Payment Request
    else if (obj.wf_id == 'P010') {
        //Basheer on 16-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.contract_local = $('.contract_local_no_cls').val();
        obj.backcharge_invoice = $('.backcharge_invoice_to_cls').val();
        obj.project = $('.project_cls').val();
        obj.year_booked = $('.year_booked_cls').val();
        obj.customer = $('.customer_cls').val();
        obj.request_id = requestId;

        if (obj.contract_local == '') {
            $('#contract_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            x = false;
            obj.attachment_filepath = afterSaveCommonFilePath;
            $('#contract_required').css('display', 'none');
        }
        if (obj.backcharge_invoice == "") {
            $('#invoice_required').css('display', 'block');
            x = true;
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            $('#invoice_required').css('display', 'none');
            x = false;
        }

        if (obj.project == '') {
            $('#Project_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            x = false;
            obj.attachment_filepath = afterSaveCommonFilePath;
            $('#Project_required').css('display', 'none');
        }
        if (obj.year_booked == "") {
            $('#year_required').css('display', 'block');
            x = true;
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            $('#year_required').css('display', 'none');
            x = false;
        }

        if (obj.customer == "") {
            $('#customer_required').css('display', 'block');
            x = true;
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            $('#customer_required').css('display', 'none');
            x = false;
        }

        if ($(".cheque_radio_btn_P010").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.cheque_account_no = $('.cheque_account_no').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.supplier_to = $('.cheque_supplier').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.remark = $('.attachment_remark').val();
            obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }

            if (obj.purpose_text == "") {
                x = true;
                $('#cheque_purpose_text_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                x = false;
                $('#cheque_purpose_text_required').css('display', 'none');

            }
            if (obj.cheque_account_no == "") {
                x = true;
                $('#cheque_account_no_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
            }
            else {

                x = false;
                $('#cheque_account_no_required').css('display', 'none');
            }

            if (obj.supplier_to == "") {
                x = true;
                $('#cheque_supplier_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
            }
            else {

                x = false;
                $('#cheque_supplier_required').css('display', 'none');
            }


        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            obj.currenctType = $('.currency_type_dropdown_id').val();//28-02-2020 ARCHANA K V SRISHTI
            obj.from_bank = $('.from_bank_name').val();
            obj.from_addreess = $('.from_bank_address').val();
            obj.from_account_no = $('.from_bank_account_no').val();
            obj.to_beneficiary = $('.to_bank_benificiary').val();
            obj.to_bankname = $('.to_bank_name').val();
            obj.to_address = $('.to_bank_address').val();
            obj.to_account_no = $('.to_bank_account_no').val();
            obj.to_iban = $('.to_bank_iban').val();
            obj.attachment_filepath = afterSaveCommonFilePath;
            obj.purpose_text = $('.bank_remark').val();
            obj.remark = $('.attachment_remark').val();
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                $('#bank_amt_required').css('display', 'none');
            }
            if (obj.from_bank == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_bank_name_required').css('display', 'block');
            }
            else {
                x = false;
                $('#from_bank_name_required').css('display', 'none');

            }
            if (obj.from_addreess == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_address_required').css('display', 'block');
            }
            else {
                x = false;
                $('#from_address_required').css('display', 'none');

            }
            if (obj.from_account_no == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_bank_account_no_required').css('display', 'block');
            }
            else {
                x = false;
                $('#from_bank_account_no_required').css('display', 'none');
            }

            if (obj.to_beneficiary == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_benificary_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_benificary_required').css('display', 'none');
            }
            if (obj.to_bankname == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_bank_name_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_bank_name_required').css('display', 'none');
            }

            if (obj.to_address == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_address_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_address_required').css('display', 'none');
            }
            if (obj.to_account_no == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_bank_account_no_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_bank_account_no_required').css('display', 'none');

            }

            if (obj.to_iban == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_iban_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_iban_required').css('display', 'none');

            }

            if (obj.purpose_text == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_purpose_text_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_purpose_text_required').css('display', 'none');

            }

        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_NonHRRelated_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }

                },
            });
        }
        else {
            $('#save_request_btn').css('display', 'block');
            $('#save_request_btn').css('disabled', false);
            $('#submit_request_btn').css('display', 'none');
            $('#submit_request_btn').prop('disabled', true);
            $('#cancel_request_btn').css('display', 'block');
            $('#cancel_request_btn').prop('disabled', false);
            toastrError("Please enter the correct details !");
        }
    }// NonHR Related Payment Request
        //---------------------------------------------------------------------------//
        // Created  :Sibi......01-01-2010
        // Modified : None
        // Salary Advance for New Arrival Payment - P051
    else if (obj.wf_id == 'P051') {
        //Basheer on 16-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.remark = $('.remark_p055').val();
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val(); //12-02-2020 Sibi Edited
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name_Second').val();
                obj.to_address = $('.to_bank_address_Second').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();

            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_Salary_Advance_for_New_Arrival_Payment_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }
        // End Sibi
        //---------------------------------------------------------------------------//
        //Basheer code start here for settling allowance payment (p050) on 06-01-2020
    else if (obj.wf_id == 'P050') {
        //Basheer on 16-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn_P050").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.remark = $('.remark_p055').val();
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();
            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_SettlingAllowancePayment_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }


    else if (obj.wf_id == 'P013') {
        Submit_P013();
    }
        //P030 Educational Assistance done by Chitra V :srishti28.05.2020
    else if (obj.wf_id == 'P030') {
        Edit_P030();
    }
        //............P003 TicketRefund done by Chitra V :srishti on 16.06.2020.................

    else if (obj.wf_id == 'P003') {
        Edit_P003();

    }
        //Add sibi 03-01-2020.............pending........save_request_btn...........................
    else if (obj.wf_id == 'P011') {
        //Basheer on 16-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        var x = false;
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        obj.LocalEmplyee_ID = $('.employee-list-drop-id :selected').val();
        obj.Iqama_Number = $('.Iqama_Number').val();
        obj.Certificate_with_Salary = $('.Certificate_with_Salary :selected').val();
        obj.Chamber_Of_Commerce_Stamp = $('.Chamber_Of_Commerce_Stamp :selected').val();
        obj.Ministry_Of_Foreign_Affairs = $('.Ministry_Of_Foreign_Affairs :selected').val();
        //obj.Location_Code = $('.wf-cuntry-list-drop-id :selected').val();
        obj.Location_Id = $('.wf-cuntry-list-drop-id :selected').val();  //Nimmi 14-03-2020
        obj.Iqama_Identification = $('.Iqama_Identification').val();
        obj.Attachment_Filepath = afterSaveCommonFilePath;


        if ($('.Iqama_Number').val() == '') {
            $('#id-IqamaNumber-required').css('display', 'block');
            //$('#submit_request_btn save_request_btn').prop('disabled', false);
            $('#save_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            $('#save_request_btn').prop('disabled', true);
        }


        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Edit_PP_Introduction_Certificate",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }


    }
        //-------------------------------End---------------------------------------
    else if (obj.wf_id == 'P060') {
        //Terrin on 30-03-2020

        var x = false;
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.Allowance_per_group = $('.Allowance_per_group :selected').val();
        obj.Remarks = $('.Remarks').val();
        obj.Justification = $('.Justification').val();
        obj.Allowance_Date = $('.Allowance_Date').val();

        if ($('.Allowance_per_group :selected').val() == '--Choose--') {
            $('#Allowance_per_group-required').css('display', 'block');
            //$('#submit_request_btn save_request_btn').prop('disabled', false);
            $('#save_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            $('#save_request_btn').prop('disabled', true);
        }


        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Edit_PP_Mobile_Allowance",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }


    }
    else if (obj.wf_id == 'P034') {
        Edit_P034();
    }
    else if (obj.wf_id == 'P056') {
        //Basheer on 16-03-2020
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn_p056").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.remark = $('.remark_p055').val();
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();

            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_RelocationAllowance_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                        $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
                        $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                        $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                        $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }   //Save Changes For Relocation Allowance P056 By Nimmi Mohan

    else if (obj.wf_id == 'P012') {
        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        var x = false;
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        obj.LocalEmplyee_ID = $('.employee-list-drop-id :selected').val();
        obj.Iqama_Number = $('.Iqama_Number').val();
        obj.Certificate_with_Salary = $('.Certificate_with_Salary :selected').val();
        obj.Chamber_Of_Commerce_Stamp = $('.Chamber_Of_Commerce_Stamp :selected').val();
        obj.Ministry_Of_Foreign_Affairs = $('.Ministry_Of_Foreign_Affairs :selected').val();
        //obj.Location_Code = $('.wf-cuntry-list-drop-id :selected').val();
        obj.Location_Id = $('.wf-cuntry-list-drop-id :selected').val();  //Nimmi 14-03-2020
        obj.Iqama_Identification = $('.Iqama_Identification').val();
        obj.Attachment_Filepath = afterSaveCommonFilePath;


        if ($('.Iqama_Number').val() == '') {
            $('#id-IqamaNumber-required').css('display', 'block');
            $('#save_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            $('#save_request_btn').prop('disabled', true);
        }


        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_Letter_To_RealEstate_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        //$('#forward_request_btn').css('display', 'none');
                        $('#forward_request_btn').css('display', 'block');//21-04-2020 Nimmi Mohan
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }


    }   //Save Changes For Letter to RealEstate P012 On 23-03-2020 By Nimmi Mohan

    else if (obj.wf_id == 'P023') {



        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }

        var x = false;
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.reason = $('.cr_reason').val();
        obj.employee_grade = $('.cr_employee_grade').val();
        obj.joining_date = $('.cr_joining_date').val();
        //obj.att_quotation_filepath = afterSaveAttachQuotation; 29-04-2020
        //obj.car_cost_reimbursement = $('.carcost_reimb').val();
        //obj.car_quotation_amount = $('.car_quotation_amount').val();
        //obj.maximum_entitlement = $('.max_entitlement').val();
        //obj.monthly_installment = $('.month_Installment').val();
        //obj.effective_date = $('.car_effective_date').val();
        obj.attachment_filepath = afterSaveCommonFilePath;
        //if ($(".frstloan_radio_tn").prop('checked') == true) {
        //    obj.first_loan = "Yes";
        //}
        //else {

        //    obj.first_loan = "No";
        //}

        //if ($(".subloan_radio_tn").prop('checked') == true) {
        //    obj.subsequent_loan = "Yes";
        //}
        //else {

        //    obj.subsequent_loan = "No";
        //}




        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_CarLoanRequest_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        //else {
        //    $('#submit_request_btn').prop('disabled', false);
        //}

    }      //Save Changes For P023 Car Loan Request 07-05-2020 By Nimmi Mohan

    else if (obj.wf_id == 'P015') {
        Submit_P015();
    }

    else if (obj.wf_id == 'P054') {

        obj._FileList = [];
        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        if ($(".cheque_radio_btn_p054").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.remark = $('.remark_p055').val();
            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.to_iban = $('.to_iban').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.purpose_text = $('.bank_remark').val();
                obj.remark = $('.remark_p055').val();
            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_NoSubmissionTimesheet_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }  //Save Changes For No Submission Of Timesheet Payment(P054) By Nimmi Mohan on 24-03-2020


    else if (obj.wf_id == 'P099') {

        obj._FileList = []; //27-03-2020

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }


        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.carloanrequest_number = $('.carloan-payment-reqid').val();
        if ($(".cheque_radio_btn_p099").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.cheque_account_no = $('.cheque_account_no').val();//04-05-2020 Nimmi
            obj.supplier_to = $('.cheque_supplier').val();//04-05-2020 Nimmi
            obj.remark = $('.attachment_remark').val();//04-05-2020 Nimmi
            obj.currenctType = $('.currency_type_dropdown_id').val();//04-05-2020 Nimmi

            if ($('.cheque_amt').val() == '') {
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.cheque_date == "") {
                $('#cheque_date_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                $('#cheque_date_required').css('display', 'none');
                x = false;
            }

            //20-04-2020 Nimmi

            if (obj.carloanrequest_number == "") {
                $('#requestno_required').css('display', 'block');
                x = true;
                $('#submit_request_btn').prop('disabled', false);

            }
            else {
                $('#requestno_required').css('display', 'none');
                x = false;

            }
        }
        else {
            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            obj.currenctType = $('.currency_type_dropdown_id').val(); //04-05-2020 Nimmi
            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }


                //20-04-2020 Nimmi
            else if (obj.carloanrequest_number == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#requestno_required').css('display', 'block');

            }


            else {
                x = false;
                obj.from_bank = $('.from_bank_name').val();
                obj.from_addreess = $('.from_bank_address').val();
                obj.from_account_no = $('.from_bank_account_no').val();
                obj.to_beneficiary = $('.to_bank_benificiary').val();
                obj.to_bankname = $('.to_bank_name').val();
                obj.to_address = $('.to_bank_address').val();
                obj.to_account_no = $('.to_bank_account_no').val();
                obj.bank_attachment = afterSaveBankFilePath;
                obj.attachment_filepath = afterSaveCommonFilePath;
                obj.remark = $('.attachment_remark').val();
                obj.purpose_text = $('.bank_purpose_text').val();
                obj.to_iban = $('.to_bank_iban').val(); //04-05-2020 Nimmi
            }
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_CarLoanPayment_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }  //Save changes for P099 CarLoan Payment By Nimmi Mohan on 7-05-2020

        //A007-Accommodation in Hotel/Compound(Preema)
    else if (obj.wf_id == 'A007') {

        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.application_ids = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;


        var ddl_AccommodationType = document.getElementById("ddl_AccommodationType");
        var AccommodationType = ddl_AccommodationType.options[ddl_AccommodationType.selectedIndex].text;


        obj.accommodation_type = AccommodationType;


        obj.hotel_name = $('.Hotel_Name').val();


        var ddl_HotelLocation = document.getElementById("ddl_HotelLocation");
        var HotelLocation = ddl_HotelLocation.options[ddl_HotelLocation.selectedIndex].text;
        obj.hotel_location = HotelLocation;

        var ddl_RoomType = document.getElementById("ddl_RoomType");
        var RoomType = ddl_RoomType.options[ddl_RoomType.selectedIndex].text;

        obj.room_type = RoomType;


        var ddl_RoomPreference = document.getElementById("ddl_RoomPreference");
        var RoomPreference = ddl_RoomPreference.options[ddl_RoomPreference.selectedIndex].text;

        obj.room_preference = RoomPreference;

        obj.no_of_room = $('.No_of_room').val();

        obj.hotel_address = $('.Hotel_Address').val();

        obj.contact_person = $('.Contact_Person').val();

        obj.fax = $('.Fax').val();

        obj.approaximate_date = $('.Approximate_Date').val();
        obj.approaximate_time = $('.Approximate_Time').val();

        var payment = document.getElementById("payment_type");
        var strPayment = payment.options[payment.selectedIndex].value;

        obj.payment_mode = strPayment;

        obj.from_period = $('.From_Period').val();

        obj.to_period = $('.To_Period').val();
        obj.remarks = $('.Remarks').val();

        var guestArray = new Array();
        $("input[name=Guest_Name]").each(function () {
            guestArray.push($(this).val());
        });
        obj.guest_name = guestArray;


        if (AccommodationType == '--Select--') {
            $('#Accommodation_Type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Accommodation_Type_required').css('display', 'none');
        }
        if ($('.Hotel_Name').val() == '') {
            $('#Hotel_Name_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Hotel_Name_required').css('display', 'none');
        }



        if (HotelLocation == '--Select--') {
            $('#Hotel_Location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Hotel_Location_required').css('display', 'none');
        }

        if (RoomType == '--Select--') {
            $('#Room_Type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Room_Type_required').css('display', 'none');
        }


        if (RoomPreference == '--Select--') {
            $('#Room_Preference_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Room_Preference_required').css('display', 'none');
        }

        if ($('.No_of_room').val() == '') {
            $('#No_of_room_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#No_of_room_required').css('display', 'none');
        }

        if ($('.Hotel_Address').val() == '') {
            $('#Hotel_Address_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Hotel_Address_required').css('display', 'none');
        }

        if ($('.Contact_Person').val() == '') {
            $('#Contact_Person_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Contact_Person_required').css('display', 'none');
        }

        if ($('.Approximate_Date').val() == '') {
            $('#Approximate_Date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Approximate_Date_required').css('display', 'none');
        }

        if ($('.Approximate_Time').val() == '') {
            $('#Approximate_Time_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Approximate_Time_required').css('display', 'none');
        }


        if (strPayment == '--Select--') {
            $('#Payment_Type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Payment_Type_required').css('display', 'none');
        }


        if ($('.From_Period').val() == '') {
            $('#From_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#From_Period_required').css('display', 'none');
        }

        if ($('.To_Period').val() == '') {
            $('#To_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#To_Period_required').css('display', 'none');
        }


        if (x == false) {

            $(".se-pre-con").show();

            $.ajax({
                type: "POST",
                url: "/Request/Submit_AO_Accommodation_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });

        }
        else {
            $('#submit_request_btn').prop('disabled', false);

        }


    }
    //// 23/06/2020 ALENA SICS FOR A008
    //if (obj.wf_id == 'A008') {

    //    $('#application-required').css('display', 'none');
    //    obj.application_id = $('#application-list-drop-id').val();
    //    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    //    obj.creator_id = $('#emp_identify_id').val();
    //    obj.request_id = requestId;

    //    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
    //    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
    //    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

    //    obj.cost_center = CostCenter_value;
    //    // obj.cost_center = $('.cost_center-drop-down').val();
    //    obj.employee_name = $('.Employee_Name').val();
    //    obj.pickup_at = $('.Drop_At').val();
    //    obj.date = $('.Drop_Date').val();
    //    obj.time = $('.Drop_Time').val();
    //    obj.remarks = $('.Remarks').val();
    //    // for administration only (save)
    //    var ddl_driver = document.getElementById("GetDriver-list-drop-class");
    //    var driver_value = ddl_driver.options[ddl_driver.selectedIndex].value;
    //    var driver_text = ddl_driver.options[ddl_driver.selectedIndex].text;
    //    var driver_mobile = ddl_d.options[ddl_d.selectedIndex].Mobile_No;

    //    obj.drivername = driver_value;
    //    obj.quantity = $('.Quantity').val();
    //    //  obj.drivername = $('.GetDriver-list-drop-class : selected').val();
    //    //obj.mobile_number = $('.Mobile_No').val();
    //    obj.Mobile_No = driver_mobile;
    //    //  obj.Mobile_No = $('.Mobile_No').val();
    //    //obj.emp_Id = $('.Emp_Id').val();
    //    obj.Employee_id = $('.Emp_Id').val();
    //    obj.carmodel = $('.Car_Model').val();
    //    // end-------------------
    //    if (obj.cost_center == "") {
    //        x = true;
    //        $('#Cost_Center_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //    }
    //    else {
    //        x = false;
    //        $('#Cost_Center_required').css('display', 'none');
    //    }
    //    if (obj.employee_name == "") {
    //        x = true;
    //        $('#Employee_Name_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //    }
    //    else {
    //        x = false;
    //        $('#Employee_Name_required').css('display', 'none');
    //    }
    //    if (obj.pickup_at == "") {
    //        x = true;
    //        $('#Drop_At_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //    }
    //    else {
    //        x = false;
    //        $('#Drop_At_required').css('display', 'none');

    //    }
    //    if (obj.time == "") {
    //        x = true;
    //        $('#Drop_Time_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //    }
    //    else {
    //        x = false;
    //        $('#Drop_Time_required').css('display', 'none');

    //    }
    //    if (obj.date == "") {
    //        x = true;
    //        $('#Drop_Date_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //    }
    //    else {
    //        x = false;
    //        $('#Drop_Date_required').css('display', 'none');

    //    }
    //    if (x == false) {
    //        $(".se-pre-con").show();
    //        $.ajax({
    //            type: "POST",
    //            //url: "/Request/Submit_AO_EmployeePickUp_Edit_After_Save",
    //            url: "/Request/Edit_AO_EmployeePickUp",
    //            dataType: "json",
    //            global: false,
    //            data: obj,
    //            success: function (response) {
    //                if (response.Status) {
    //                    $(".se-pre-con").hide();
    //                    toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
    //                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
    //                    $('#forward_request_btn').css('display', 'block');//Basheer on 13-02-2020
    //                    $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
    //                    $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
    //                    $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
    //                    $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
    //                }
    //                else {
    //                    $(".se-pre-con").hide();
    //                }
    //            },
    //        });
    //    }

    //}
        //------P029-----Terrin Edit on 8/4/2020-----
    else if (obj.wf_id == 'P029') {

        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.application_ids = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.App_Type = $('.Application_Type :selected').val();
        obj.Add_details = $('.AddDetails :selected').val();
        obj.Effective = $('.Effective').val();
        obj.Date_Employee = $('.Date_Employee').val();
        obj.Iqama_no = $('.Iqama_no').val();
        obj.MedIns_Remarks = $('.MedIns_Remarks').val();
        obj.Attachment_Filepath = afterSaveCommonFilePath;

        var Insurance_dependence = new Array();
        $('#tbl_dependent tbody tr').each(function () {
            var row = $(this);
            var Insurancedependence = {};
            Insurancedependence.Name = row.find('#id-InsName').val();
            Insurancedependence.Date_of_birth = row.find('.Ins_Date').val();
            Insurancedependence.Sex = row.find('input:radio:checked').val();
            Insurancedependence.Relation = row.find('#id-InsRelation').val();
            Insurancedependence.Depend_class = row.find('#id-InsClass').val();
            Insurance_dependence.push(Insurancedependence);
        });

        obj._Insurance_dependence = Insurance_dependence;



        if (obj.App_Type == '--Select--') {
            $('#application-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#application-required').css('display', 'none');
        }


        if (x == false) {

            $(".se-pre-con").show();

            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_MedicalInsurance_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });

        }
        else {
            $('#submit_request_btn').prop('disabled', false);

        }


    }
        //A009-Arrangement of Employee Drop(Preema)
    else if (obj.wf_id == 'A009') {

        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }

        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;


        var ddl_CostCenter = document.getElementById("cost_center-drop-down");
        var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
        var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

        obj.cost_center = CostCenter_value;
        obj.employee_name = $('.Employee_Name').val();

        obj.drop_at = $('.Drop_At').val();

        obj.date = $('.Drop_Date').val();

        obj.time = $('.Drop_Time').val();

        obj.remarks = $('.Remarks').val();


        if (CostCenter_text == '-- Choose --') {
            $('#Cost_Center_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Cost_Center_required').css('display', 'none');
        }


        if ($('.Employee_Name').val() == '') {
            $('#Employee_Name_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Employee_Name_required').css('display', 'none');
        }

        if ($('.Drop_At').val() == '') {
            $('#Drop_At_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Drop_At_required').css('display', 'none');
        }

        if ($('.Drop_Time').val() == '') {
            $('#Drop_Time_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#Drop_Time_required').css('display', 'none');
        }
        if (x == false) {

            $(".se-pre-con").show();

            $.ajax({
                type: "POST",
                url: "/Request/Submit_AO_EmployeeDrop_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });

        }
        else {
            $('#submit_request_btn').prop('disabled', false);

        }
    }

        //P049-Other Personnel Services(Preema)
    else if (obj.wf_id == 'P049') {

        obj._FileList = [];
        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.request_details = $('#txt_Request_Detail').val();

        if (obj.request_details == "") {
            $('#request_detail_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#request_detail_required').css('display', 'none');
        }

        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_OtherPersonnelServices_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }
        //P061-ESAP Contribution(Preema)
    else if (obj.wf_id == 'P061') {
        $('#application-required').css('display', 'none');

        var Esap_Obj = new Object();
        obj._FileList = [];
        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }

        $('#application-required').css('display', 'none');
        Esap_Obj.application_id = $('#application-list-drop-id').val();
        Esap_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        Esap_Obj.creator_id = $('#emp_identify_id').val();
        Esap_Obj.request_id = requestId;

        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        Esap_Obj.For_the_Period_of = $(".class-For-the-Period-of").val();
        Esap_Obj.Remarks = $("#divRemarks").text();



        var ddl_company1 = document.getElementById("1");
        var company1 = ddl_company1.options[ddl_company1.selectedIndex].text;

        var ddl_company2 = document.getElementById("2");
        var company2 = ddl_company2.options[ddl_company2.selectedIndex].text;

        var ddl_company3 = document.getElementById("3");
        var company3 = ddl_company3.options[ddl_company3.selectedIndex].text;

        var Total_Amount_in_USD_1 = $("#id-TotalAmountinUSD_1").val();
        var Total_Amount_in_USD_2 = $("#id-TotalAmountinUSD_2").val();
        var Total_Amount_in_USD_3 = $("#id-TotalAmountinUSD_3").val();

        if (Esap_Obj.For_the_Period_of == "") {
            $('#span-id-For-the-Period-of-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#span-id-For-the-Period-of-required').css('display', 'none');
        }

        if (company1 == '--Choose--') {
            $('#id-span-GetallCompany-list_1').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#id-span-GetallCompany-list_1').css('display', 'none');
        }


        if (Total_Amount_in_USD_1 == "" || Total_Amount_in_USD_1 == undefined) {
            $('#id-span-Total-Amountin-USD_1').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#id-span-Total-Amountin-USD_1').css('display', 'none');
        }

        if (company2 != '--Choose--') {
            //if (company2 == '--Choose--') {
            //    $('#id-span-GetallCompany-list_2').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    return;
            //}
            //else {
            //    x = false;
            //    $('#id-span-GetallCompany-list_2').css('display', 'none');
            //}

            if (Total_Amount_in_USD_2 == "" || Total_Amount_in_USD_2 == undefined) {
                $('#id-span-Total-Amountin-USD_2').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-Total-Amountin-USD_2').css('display', 'none');
            }
        }
        //if (company3 == '--Choose--') {
        //    $('#id-span-GetallCompany-list_3').css('display', 'block');
        //    $('#submit_request_btn').prop('disabled', false);
        //    x = true;
        //    return;
        //}
        //else {
        //    x = false;
        //    $('#id-span-GetallCompany-list_3').css('display', 'none');
        //}
        if (company3 != '--Choose--') {
            if (Total_Amount_in_USD_3 == "" || Total_Amount_in_USD_3 == undefined) {
                $('#id-span-Total-Amountin-USD_3').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-Total-Amountin-USD_3').css('display', 'none');
            }
        }

        var CompanyArray = new Array();
        $(".GetallCompany-list-drop-class").each(function () {
            CompanyArray.push($(this).val());
        });

        Esap_Obj.strCompany = CompanyArray;

        var PayrollArray = new Array();
        $(".Payroll_Code").each(function () {
            PayrollArray.push($(this).val());
        });

        Esap_Obj.strPayrollCode = PayrollArray;

        var TotalArray = new Array();
        $(".Class-Amount").each(function () {
            TotalArray.push($(this).val());
        });

        Esap_Obj.strTotal = TotalArray;


        Esap_Obj.Grand_Total = $(".GrandTotal").val();
        Esap_Obj.Note = $("textarea#id-textarea-Note").val();

        obj.ESAP_ContributionModel = Esap_Obj;

        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_ESAP_Contribution_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {

                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }
        //P062-Retirement Contribution(Preema)
    else if (obj.wf_id == 'P062') {
        $('#application-required').css('display', 'none');

        var Esap_Obj = new Object();
        obj._FileList = [];
        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }

        $('#application-required').css('display', 'none');
        Esap_Obj.application_id = $('#application-list-drop-id').val();
        Esap_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        Esap_Obj.creator_id = $('#emp_identify_id').val();
        Esap_Obj.request_id = requestId;

        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        Esap_Obj.For_the_Period_of = $(".For-the-Period-of").val();
        Esap_Obj.Remarks = $('#divRemarks').text();
        Esap_Obj.Bank_Details = $('#divBankDetails').text();


        var ddl_company1 = document.getElementById("1");
        var company1 = ddl_company1.options[ddl_company1.selectedIndex].text;

        var ddl_company2 = document.getElementById("2");
        var company2 = ddl_company2.options[ddl_company2.selectedIndex].text;

        var ddl_company3 = document.getElementById("3");
        var company3 = ddl_company3.options[ddl_company3.selectedIndex].text;

        var Total_Amount_in_USD_1 = $("#id-TotalAmountinUSD_1").val();
        var Total_Amount_in_USD_2 = $("#id-TotalAmountinUSD_2").val();
        var Total_Amount_in_USD_3 = $("#id-TotalAmountinUSD_3").val();

        if (Esap_Obj.For_the_Period_of == "") {
            $('#span-id-For-the-Period-of-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#span-id-For-the-Period-of-required').css('display', 'none');
        }

        if (Esap_Obj.Remarks == "") {
            $('#span-id-remark-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#span-id-remark-required').css('display', 'none');
        }

        if (Esap_Obj.Bank_Details == "") {
            $('#span-id-bank-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#span-id-bank-required').css('display', 'none');
        }


        if (company1 == '--Choose--') {
            $('#id-span-GetallCompany-list_1').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#id-span-GetallCompany-list_1').css('display', 'none');
        }


        if (Total_Amount_in_USD_1 == "" || Total_Amount_in_USD_1 == undefined) {
            $('#id-span-Total-Amountin-USD_1').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
            return;
        }
        else {
            x = false;
            $('#id-span-Total-Amountin-USD_1').css('display', 'none');
        }

        if (company2 != '--Choose--') {
            //if (company2 == '--Choose--') {
            //    $('#id-span-GetallCompany-list_2').css('display', 'block');
            //    $('#submit_request_btn').prop('disabled', false);
            //    x = true;
            //    return;
            //}
            //else {
            //    x = false;
            //    $('#id-span-GetallCompany-list_2').css('display', 'none');
            //}

            if (Total_Amount_in_USD_2 == "" || Total_Amount_in_USD_2 == undefined) {
                $('#id-span-Total-Amountin-USD_2').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-Total-Amountin-USD_2').css('display', 'none');
            }
        }
        //if (company3 == '--Choose--') {
        //    $('#id-span-GetallCompany-list_3').css('display', 'block');
        //    $('#submit_request_btn').prop('disabled', false);
        //    x = true;
        //    return;
        //}
        //else {
        //    x = false;
        //    $('#id-span-GetallCompany-list_3').css('display', 'none');
        //}
        if (company3 != '--Choose--') {
            if (Total_Amount_in_USD_3 == "" || Total_Amount_in_USD_3 == undefined) {
                $('#id-span-Total-Amountin-USD_3').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                x = true;
                return;
            }
            else {
                x = false;
                $('#id-span-Total-Amountin-USD_3').css('display', 'none');
            }
        }

        var CompanyArray = new Array();
        $(".GetallCompany-list-drop").each(function () {
            CompanyArray.push($(this).val());
        });

        Esap_Obj.strCompany = CompanyArray;

        var PayrollArray = new Array();
        $(".Payroll_Code").each(function () {
            PayrollArray.push($(this).val());
        });

        Esap_Obj.strPayrollCode = PayrollArray;

        var TotalArray = new Array();
        $(".Amount").each(function () {
            TotalArray.push($(this).val());
        });

        Esap_Obj.strTotal = TotalArray;


        Esap_Obj.Grand_Total = $(".GrandTotal").val();



        obj.RetirementContributionModel = Esap_Obj;

        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_RetirementContribution_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {

                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }
        //P053-GOSI Payment(Preema)
    else if (obj.wf_id == 'P053') {

        obj._FileList = [];
        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }

        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        if ($(".cheque_radio_btn").prop('checked') == true) {// Payment mode is cheque
            obj.payment_mode = "C";
            obj.cheque_date = $('.cheque_date').val();
            obj.amount_sar = $('.cheque_amt').val();
            obj.purpose_text = $('.cheque_purpose_text').val();
            obj.attachment_filepath = $('.attachment_filepath').val();
            obj.payable_to = $('.cheque_payable_to').val();
            obj.remark = $('.remark_p055').val();
            if (obj.cheque_date == "") {
                x = true;
                $('#cheque_date_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                x = false;
                $('#cheque_date_required').css('display', 'none');

            }
            if ($('.cheque_amt').val() == '') {
                x = true;
                $('#cheque_amt_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);

            }
            else {
                x = false;
                obj.attachment_filepath = afterSaveCommonFilePath;
                $('#cheque_amt_required').css('display', 'none');
            }
            if (obj.purpose_text == "") {
                x = true;
                $('#cheque_purpose_text_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
            }
            else {
                x = false;
                $('#cheque_purpose_text_required').css('display', 'none');

            }
        }
        else {

            obj.payment_mode = "B";// Payment mode is Bank
            obj.amount_sar = $('.bank_wise_amount').val();
            obj.from_bank = $('.from_bank_name').val();
            obj.from_addreess = $('.from_bank_address').val();
            obj.from_account_no = $('.from_bank_account_no').val();
            obj.to_beneficiary = $('.to_bank_benificiary').val();
            obj.to_bankname = $('.to_bank_name').val();
            obj.to_address = $('.to_bank_address').val();
            obj.to_account_no = $('.to_bank_account_no').val();
            obj.bank_attachment = afterSaveBankFilePath;
            obj.attachment_filepath = afterSaveCommonFilePath;
            obj.purpose_text = $('.bank_remark').val();
            obj.remark = $('.remark_p055').val();
            obj.to_iban = $('.to_iban').val();

            if (obj.amount_sar == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#bank_amt_required').css('display', 'block');
            }
            else {
                x = false;
                $('#bank_amt_required').css('display', 'none');
            }
            if (obj.from_bank == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_bank_name_required').css('display', 'block');
            }
            else {
                x = false;
                $('#from_bank_name_required').css('display', 'none');

            }
            if (obj.from_addreess == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_address_required').css('display', 'block');
            }
            else {
                x = false;
                $('#from_address_required').css('display', 'none');

            }
            if (obj.from_account_no == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#from_bank_account_no_required').css('display', 'block');
            }
            else {
                x = false;
                $('#from_bank_account_no_required').css('display', 'none');
            }

            if (obj.to_beneficiary == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_benificary_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_benificary_required').css('display', 'none');
            }
            if (obj.to_bankname == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_bank_name_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_bank_name_required').css('display', 'none');
            }

            if (obj.to_address == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_address_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_address_required').css('display', 'none');
            }
            if (obj.to_account_no == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_bank_account_no_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_bank_account_no_required').css('display', 'none');

            }

            if (obj.to_iban == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_iban_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_iban_required').css('display', 'none');

            }

            if (obj.purpose_text == "") {
                x = true;
                $('#submit_request_btn').prop('disabled', false);
                $('#to_purpose_text_required').css('display', 'block');
            }
            else {
                x = false;
                $('#to_purpose_text_required').css('display', 'none');

            }


        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_GOSI_Payment_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }

        //P024-Bank Loan Request(Preema)
    else if (obj.wf_id == 'P024') {

        var Bank_Obj = new Object();

        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }


        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        Bank_Obj.application_id = $('#application-list-drop-id').val();
        Bank_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        Bank_Obj.creator_id = $('#emp_identify_id').val();
        Bank_Obj.request_id = requestId;

        Bank_Obj.Bank_Name = $('#txt_Bank_Name').val();
        Bank_Obj.Account_No = $('#txt_Account_No').val();
        Bank_Obj.Loan_Amount = $('#txt_Loan_Amount').val();
        Bank_Obj.Date_of_Hire = $('#txt_Date_of_Hire').val();

        var Nationality = $('input:radio[name=Nationality]:checked').val();

        Bank_Obj.Nationality = Nationality;

        Bank_Obj.Saudi_Id = $('#txt_Saudi_ID').val();
        Bank_Obj.Purpose = $('#divPurpose').text();
        Bank_Obj.End_of_Service_Benefit = "";
        Bank_Obj.As_of_Date = "";

        obj.BankLoanRequestModel = Bank_Obj;

        if (Bank_Obj.Bank_Name == "") {
            $('#Bank_Name_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Bank_Obj.Account_No == "") {
            $('#Bank_AccNo_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Bank_Obj.Loan_Amount == "") {
            $('#Loan_Amount_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Bank_Obj.Date_of_Hire == "") {
            $('#Date_of_Hire_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Bank_Obj.Nationality == "" || Bank_Obj.Nationality == undefined) {
            $('#Nationality_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Bank_Obj.Saudi_Id == "") {
            $('#Saudi_ID_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (x == true) {
            toastrError("Please fill the mandatory fields");
            return;
        }
        else {
            $('#Bank_Name_required').css('display', 'none');
            $('#Bank_AccNo_required').css('display', 'none');
            $('#Loan_Amount_required').css('display', 'none');
            $('#Date_of_Hire_required').css('display', 'none');
            $('#Nationality_required').css('display', 'none');
            $('#Saudi_ID_required').css('display', 'none');
            x = false;
        }

        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_Bank_Loan_Request_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }

        //P016-Internal Transfer(Preema)
    else if (obj.wf_id == 'P016') {
        $('#hdn_Count').val('1');
        var Transfer_Obj = new Object();

        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }


        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        Transfer_Obj.application_id = $('#application-list-drop-id').val();
        Transfer_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        Transfer_Obj.creator_id = $('#emp_identify_id').val();
        Transfer_Obj.request_id = requestId;

        if ($("#rbtn_Promotion").prop('checked') == true) {
            Transfer_Obj.Transfer_Type = "Promotion";
        }
        else {
            Transfer_Obj.Transfer_Type = "Transfer";
        }

        Transfer_Obj.Employee_Id = $('#hdn_Employee_Id').val();
        Transfer_Obj.Releasing_Manager = $('#txt_ReleasingManager').val();
        Transfer_Obj.Releasing_Manager_Id = $('#hdn_ReleasingManager_Id').val();

        var dp_ReceivingManager = document.getElementById("dp_ReceivingManager");
        Transfer_Obj.Receiving_Manager = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].text;
        Transfer_Obj.Receiving_Manager_Id = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].value;

        Transfer_Obj.Transfer_From = $('#txt_Transfer_From').val();
        Transfer_Obj.Transfer_To = $('#txt_Transfer_To').val();
        Transfer_Obj.Effective_Date = $('#txt_EffectiveDate').val();

        Transfer_Obj.From_Company = $('#txt_From_Company').val();
        Transfer_Obj.From_Company_id = $('#hdn_From_Company_Id').val();

        var dp_To_Company_Id = document.getElementById("dp_To_Company_Id");
        Transfer_Obj.To_Company = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].text;
        Transfer_Obj.To_Company_Id = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].value;

        Transfer_Obj.From_Business_Line = $('#txt_From_BusinessLine').val();
        Transfer_Obj.From_Business_Line_id = $('#hdn_From_BusinessLine_Id').val();

        var dp_To_BusinessLine = document.getElementById("dp_To_BusinessLine");
        Transfer_Obj.To_Business_Line = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].text;
        Transfer_Obj.To_Business_Line_Id = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].value;

        Transfer_Obj.From_Product_Group = $('#txt_From_ProductGroup').val();
        Transfer_Obj.From_Product_Group_id = $('#hdn_From_ProductGroup_Id').val();

        var dp_To_ProductGroup = document.getElementById("dp_To_ProductGroup");
        Transfer_Obj.To_Product_Group = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].text;
        Transfer_Obj.To_Product_Group_Id = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].value;

        Transfer_Obj.From_Department = $('#txt_From_Department').val();
        Transfer_Obj.From_Department_id = $('#hdn_From_Department_Id').val();

        var dp_To_Department = document.getElementById("dp_To_Department");
        Transfer_Obj.To_Department = dp_To_Department.options[dp_To_Department.selectedIndex].text;
        Transfer_Obj.To_Department_Id = dp_To_Department.options[dp_To_Department.selectedIndex].value;

        Transfer_Obj.From_Position = $('#txt_From_Position').val();
        Transfer_Obj.From_Position_id = $('#hdn_From_Position_Id').val();

        var dp_To_Position = document.getElementById("dp_To_Position");
        Transfer_Obj.To_Position = dp_To_Position.options[dp_To_Position.selectedIndex].text;
        Transfer_Obj.To_Position_Id = dp_To_Position.options[dp_To_Position.selectedIndex].value;

        Transfer_Obj.From_Global_Grade = $('#txt_From_Global_Grade').val();
        Transfer_Obj.From_Global_Grade_id = $('#hdn_From_Global_Grade_Id').val();

        var dp_To_Global_Grade = document.getElementById("dp_To_Global_Grade");
        Transfer_Obj.To_Global_Grade = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].text;
        Transfer_Obj.To_Global_Grade_Id = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].value;

        Transfer_Obj.From_Local_Grade = $('#txt_From_Local_Grade').val();
        Transfer_Obj.From_Local_Grade_id = $('#hdn_From_Local_Grade_Id').val();

        var dp_To_Local_Grade = document.getElementById("dp_To_Local_Grade");
        Transfer_Obj.To_Local_Grade = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].text;
        Transfer_Obj.To_Local_Grade_Id = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].value;

        Transfer_Obj.From_Cost_Center = $('#txt_From_Cost_Center').val();
        Transfer_Obj.From_Cost_Center_id = $('#hdn_From_Cost_Center_Id').val();

        var dp_To_Cost_Center = document.getElementById("dp_To_Cost_Center");
        Transfer_Obj.To_Cost_Center = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].text;
        Transfer_Obj.To_Cost_Center_Id = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].value;

        var dp_from_Status = document.getElementById("dp_From_Status");
        Transfer_Obj.From_status = dp_from_Status.options[dp_from_Status.selectedIndex].text;

        var dp_To_Status = document.getElementById("dp_To_Status");
        Transfer_Obj.To_status = dp_To_Status.options[dp_To_Status.selectedIndex].text;

        Transfer_Obj.From_Notice_Period = $('#txt_From_Notice_Period').val();
        Transfer_Obj.To_Notice_Period = $('#txt_To_Notice_Period').val();

        Transfer_Obj.From_Location = $('#txt_From_Location').val();
        Transfer_Obj.From_Location_id = $('#hdn_From_Location_Id').val();

        var dp_To_Location = document.getElementById("dp_To_Location");
        Transfer_Obj.To_Location = dp_To_Location.options[dp_To_Location.selectedIndex].text;
        Transfer_Obj.To_Location_Id = dp_To_Location.options[dp_To_Location.selectedIndex].value;

        Transfer_Obj.From_Basic_Salary = $('#txt_From_BasicSalary').val();
        Transfer_Obj.To_Basic_Salary = $('#txt_To_BasicSalary').val();
        Transfer_Obj.From_Annual_Housing = $('#txt_From_AnnualHousing').val();
        Transfer_Obj.To_Annual_Housing = $('#txt_To_AnnualHousing').val();
        Transfer_Obj.From_Car_Cost = $('#txt_From_CarCost').val();
        Transfer_Obj.To_Car_Cost = $('#txt_To_CarCost').val();
        Transfer_Obj.From_Transport = $('#txt_From_Transport').val();
        Transfer_Obj.To_Transport = $('#txt_To_Transport').val();
        Transfer_Obj.From_Travel_Allowance = $('#txt_From_TravelHardshipAllowance').val();
        Transfer_Obj.To_Travel_Allowance = $('#txt_To_TravelHardshipAllowance').val();
        Transfer_Obj.From_Mobile_Allowance = $('#txt_From_MobileAllowance').val();
        Transfer_Obj.To_Mobile_Allowance = $('#txt_To_MobileAllowance').val();

        obj.InternalTransferModel = Transfer_Obj;

        if (Transfer_Obj.Receiving_Manager == "-- Choose --") {
            $('#spn_ReceivingManager_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.Transfer_From == "") {
            $('#spn_Transfer_From_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.Transfer_To == "") {
            $('#spn_Transfer_To_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.Effective_Date == "") {
            $('#spn_EffectiveDate_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_status == "-- Choose --") {
            $('#spn_From_Status_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Notice_Period == "") {
            $('#spn_From_Notice_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Basic_Salary == "") {
            $('#spn_From_BasicSalary_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Annual_Housing == "") {
            $('#spn_From_AnnualHousing_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Car_Cost == "") {
            $('#spn_From_CarCost_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Transport == "") {
            $('#spn_From_Transport_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Travel_Allowance == "") {
            $('#spn_From_TravelHardshipAllowance_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Mobile_Allowance == "") {
            $('#spn_From_MobileAllowance_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (x == true) {
            toastrError("Please fill the mandatory fields");
            return;
        }
        else {
            $('#spn_ReceivingManager_required').css('display', 'none');
            $('#spn_Transfer_From_required').css('display', 'none');
            $('#spn_Transfer_To_required').css('display', 'none');
            $('#spn_EffectiveDate_required').css('display', 'none');
            $('#spn_From_Status_required').css('display', 'none');
            $('#spn_From_Notice_Period_required').css('display', 'none');
            $('#spn_From_BasicSalary_required').css('display', 'none');
            $('#spn_From_AnnualHousing_required').css('display', 'none');
            $('#spn_From_CarCost_required').css('display', 'none');
            $('#spn_From_Transport_required').css('display', 'none');
            $('#spn_From_TravelHardshipAllowance_required').css('display', 'none');
            $('#spn_From_MobileAllowance_required').css('display', 'none');
            x = false;
        }

        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_Internal_Transfer_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }

        //P017-Contract Modification(Preema)
    else if (obj.wf_id == 'P017') {
        $('#hdn_Count').val('1');
        var Transfer_Obj = new Object();

        obj._FileList = [];

        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }


        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;

        Transfer_Obj.application_id = $('#application-list-drop-id').val();
        Transfer_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        Transfer_Obj.creator_id = $('#emp_identify_id').val();
        Transfer_Obj.request_id = requestId;

        if ($("#rbtn_Salary_Adjustment").prop('checked') == true) {
            Transfer_Obj.Contract_Type = " Salary Adjustment";
        }
        else {
            Transfer_Obj.Contract_Type = "Contract Renewal";
        }

        Transfer_Obj.Employee_Id = $('#hdn_Employee_Id').val();

        var dp_ReceivingManager = document.getElementById("dp_ReleasingManager");
        Transfer_Obj.Releasing_Manager = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].text;
        Transfer_Obj.Releasing_Manager_Id = dp_ReceivingManager.options[dp_ReceivingManager.selectedIndex].value;

        Transfer_Obj.Effective_Date = $('#txt_EffectiveDate').val();

        Transfer_Obj.From_Company = $('#txt_From_Company').val();
        Transfer_Obj.From_Company_id = $('#hdn_From_Company_Id').val();

        var dp_To_Company_Id = document.getElementById("dp_To_Company_Id");
        Transfer_Obj.To_Company = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].text;
        Transfer_Obj.To_Company_Id = dp_To_Company_Id.options[dp_To_Company_Id.selectedIndex].value;

        Transfer_Obj.From_Business_Line = $('#txt_From_BusinessLine').val();
        Transfer_Obj.From_Business_Line_id = $('#hdn_From_BusinessLine_Id').val();

        var dp_To_BusinessLine = document.getElementById("dp_To_BusinessLine");
        Transfer_Obj.To_Business_Line = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].text;
        Transfer_Obj.To_Business_Line_Id = dp_To_BusinessLine.options[dp_To_BusinessLine.selectedIndex].value;

        Transfer_Obj.From_Product_Group = $('#txt_From_ProductGroup').val();
        Transfer_Obj.From_Product_Group_id = $('#hdn_From_ProductGroup_Id').val();

        var dp_To_ProductGroup = document.getElementById("dp_To_ProductGroup");
        Transfer_Obj.To_Product_Group = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].text;
        Transfer_Obj.To_Product_Group_Id = dp_To_ProductGroup.options[dp_To_ProductGroup.selectedIndex].value;

        Transfer_Obj.From_Department = $('#txt_From_Department').val();
        Transfer_Obj.From_Department_id = $('#hdn_From_Department_Id').val();

        var dp_To_Department = document.getElementById("dp_To_Department");
        Transfer_Obj.To_Department = dp_To_Department.options[dp_To_Department.selectedIndex].text;
        Transfer_Obj.To_Department_Id = dp_To_Department.options[dp_To_Department.selectedIndex].value;

        Transfer_Obj.From_Position = $('#txt_From_Position').val();
        Transfer_Obj.From_Position_id = $('#hdn_From_Position_Id').val();

        var dp_To_Position = document.getElementById("dp_To_Position");
        Transfer_Obj.To_Position = dp_To_Position.options[dp_To_Position.selectedIndex].text;
        Transfer_Obj.To_Position_Id = dp_To_Position.options[dp_To_Position.selectedIndex].value;

        Transfer_Obj.From_Global_Grade = $('#txt_From_Global_Grade').val();
        Transfer_Obj.From_Global_Grade_id = $('#hdn_From_Global_Grade_Id').val();

        var dp_To_Global_Grade = document.getElementById("dp_To_Global_Grade");
        Transfer_Obj.To_Global_Grade = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].text;
        Transfer_Obj.To_Global_Grade_Id = dp_To_Global_Grade.options[dp_To_Global_Grade.selectedIndex].value;

        Transfer_Obj.From_Local_Grade = $('#txt_From_Local_Grade').val();
        Transfer_Obj.From_Local_Grade_id = $('#hdn_From_Local_Grade_Id').val();

        var dp_To_Local_Grade = document.getElementById("dp_To_Local_Grade");
        Transfer_Obj.To_Local_Grade = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].text;
        Transfer_Obj.To_Local_Grade_Id = dp_To_Local_Grade.options[dp_To_Local_Grade.selectedIndex].value;

        Transfer_Obj.From_Cost_Center = $('#txt_From_Cost_Center').val();
        Transfer_Obj.From_Cost_Center_id = $('#hdn_From_Cost_Center_Id').val();

        var dp_To_Cost_Center = document.getElementById("dp_To_Cost_Center");
        Transfer_Obj.To_Cost_Center = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].text;
        Transfer_Obj.To_Cost_Center_Id = dp_To_Cost_Center.options[dp_To_Cost_Center.selectedIndex].value;

        var dp_from_Status = document.getElementById("dp_From_Status");
        Transfer_Obj.From_status = dp_from_Status.options[dp_from_Status.selectedIndex].text;

        var dp_To_Status = document.getElementById("dp_To_Status");
        Transfer_Obj.To_status = dp_To_Status.options[dp_To_Status.selectedIndex].text;

        Transfer_Obj.From_Notice_Period = $('#txt_From_Notice_Period').val();
        Transfer_Obj.To_Notice_Period = $('#txt_To_Notice_Period').val();

        Transfer_Obj.From_Location = $('#txt_From_Location').val();
        Transfer_Obj.From_Location_id = $('#hdn_From_Location_Id').val();

        var dp_To_Location = document.getElementById("dp_To_Location");
        Transfer_Obj.To_Location = dp_To_Location.options[dp_To_Location.selectedIndex].text;
        Transfer_Obj.To_Location_Id = dp_To_Location.options[dp_To_Location.selectedIndex].value;

        Transfer_Obj.From_Basic_Salary = $('#txt_From_BasicSalary').val();
        Transfer_Obj.To_Basic_Salary = $('#txt_To_BasicSalary').val();
        Transfer_Obj.From_Annual_Housing = $('#txt_From_AnnualHousing').val();
        Transfer_Obj.To_Annual_Housing = $('#txt_To_AnnualHousing').val();
        Transfer_Obj.From_Car_Cost = $('#txt_From_CarCost').val();
        Transfer_Obj.To_Car_Cost = $('#txt_To_CarCost').val();
        Transfer_Obj.From_Transport = $('#txt_From_Transport').val();
        Transfer_Obj.To_Transport = $('#txt_To_Transport').val();
        Transfer_Obj.From_Travel_Allowance = $('#txt_From_TravelHardshipAllowance').val();
        Transfer_Obj.To_Travel_Allowance = $('#txt_To_TravelHardshipAllowance').val();
        Transfer_Obj.From_Mobile_Allowance = $('#txt_From_MobileAllowance').val();
        Transfer_Obj.To_Mobile_Allowance = $('#txt_To_MobileAllowance').val();

        obj.ContractModificationModel = Transfer_Obj;

        if (Transfer_Obj.Releasing_Manager == "-- Choose --") {
            $('#spn_ReleasingManager_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.Effective_Date == "") {
            $('#spn_EffectiveDate_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_status == "-- Choose --") {
            $('#spn_From_Status_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Notice_Period == "") {
            $('#spn_From_Notice_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Basic_Salary == "") {
            $('#spn_From_BasicSalary_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Annual_Housing == "") {
            $('#spn_From_AnnualHousing_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Car_Cost == "") {
            $('#spn_From_CarCost_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Transport == "") {
            $('#spn_From_Transport_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Travel_Allowance == "") {
            $('#spn_From_TravelHardshipAllowance_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (Transfer_Obj.From_Mobile_Allowance == "") {
            $('#spn_From_MobileAllowance_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }

        if (x == true) {
            toastrError("Please fill the mandatory fields");
            return;
        }
        else {
            $('#spn_ReleasingManager_required').css('display', 'none');
            $('#spn_EffectiveDate_required').css('display', 'none');
            $('#spn_From_Status_required').css('display', 'none');
            $('#spn_From_Notice_Period_required').css('display', 'none');
            $('#spn_From_BasicSalary_required').css('display', 'none');
            $('#spn_From_AnnualHousing_required').css('display', 'none');
            $('#spn_From_CarCost_required').css('display', 'none');
            $('#spn_From_Transport_required').css('display', 'none');
            $('#spn_From_TravelHardshipAllowance_required').css('display', 'none');
            $('#spn_From_MobileAllowance_required').css('display', 'none');
            x = false;
        }
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_Contract_Modification_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
        else {
            $('#submit_request_btn').prop('disabled', false);
        }
    }

    else if (obj.wf_id == 'P025') {
        obj._FileList = [];
        for (let i = 0; i < lists.length; i++) {
            var x = new Object();
            x.filename = lists[i].filename;
            x.filepath = lists[i].filepath;
            x.filebatch = lists[i].filetype;
            obj._FileList.push(x);
        }
        var x = false;
        $('#application-required').css('display', 'none');
        obj.application_id = $('#application-list-drop-id').val();
        obj.emp_local_id = $('.employee-list-drop-id :selected').val();
        obj.creator_id = $('#emp_identify_id').val();
        obj.request_id = requestId;
        obj.reason_clearance = $('.Endofservice_reasonfor_clearance').val();
        obj.termination_Date = $('.terminatn_date').val();
        obj.eb_Toolbox_Returned = $('.toolbx_returned').val();
        obj.eb_Workstation_Cleared = $('.workstation_cleared').val();
        obj.eb_OfficialBusiness_Documents = $('.official_business_doc').val();
        obj.eb_SiteProject_Clearance = $('.site_project_clearance').val();
        obj.eb_Uniform = $('.uni_form').val();
        obj.eb_Safety_Equipment = $('.safety_equipment').val();
        obj.eb_AllWorkflow_Approvals = $('.workflow_approvals').val();
        obj.eb_ISService_Deactivation_Date = $('.is_service_deactivtn_date').val();
        obj.eb_Assigned_Delegate = $('.assigned_delegate').val();
        obj.ad_HousingHousehold_cleared = $('.housing_household').val();
        obj.ad_Util_Water_cleared = $('.utilities_water_etc').val();
        obj.ad_CarGarageKey_Returned = $('.car_garage_key').val();
        obj.ad_Gatepass_Returned = $('.gatepass_sticker').val();
        obj.ad_Mobile_SimCard_Returned = $('.Mobile_returned').val();
        obj.ad_CompanyID_Returned = $('.company_id_returned').val();
        obj.tr_Amount_SAR = $('.sar_amount').val();
        obj.tr_ExternalTraining_Cost = $('.ExternalTraining_Cost').val();
        obj.is_Desktop_Returned = $('.desktop_etc_cleared').val();
        obj.ft_Clearance_Obtained = $('.clearance_obtained_fin').val();
        obj.ft_eBank_Token = $('.e_bank_token').val();
        obj.ae_Clearance_Obtained = $('.clearance_obtained_ae').val();
        obj.hr_CarLoan_Cleared = $('.car_loan_cleared').val();
        obj.hr_SalaryAdvances_Settled = $('.salary_advance_settled').val();
        obj.hr_CompanyStamp_Returned = $('.company_stamp_returned').val();
        obj.hr_MedicalInsurance_Returned = $('.medical_insurance').val();
        obj.hr_Visa_Mastercard_Communicated = $('.visa_master_commun').val();
        obj.hr_Savingcurrent_communicated = $('.sav_current_account').val();
        obj.hr_Remarks = $('.res_remarks').val();
        obj.hr_Attachment_Filepath = afterSaveCommonFilePath;

        //05-05-2020
        obj.eb_toolbx_returned_date = $('.toolbx_returned_date').val();
        obj.workstation_cleared_date = $('.workstation_cleared_date').val();
        obj.official_business_doc_date = $('.official_business_doc_date').val();
        obj.site_project_clearance_date = $('.site_project_clearance_date').val();
        obj.uni_form_date = $('.uni_form_date').val();
        obj.safety_equipment_date = $('.safety_equipment_date').val();
        obj.workflow_approvals_date = $('.workflow_approvals_date').val();
        obj.housing_household_date = $('.housing_household_date').val();
        obj.car_garage_key_date = $('.car_garage_key_date').val();
        obj.gatepass_sticker_date = $('.gatepass_sticker_date').val();
        obj.Mobile_returned_date = $('.Mobile_returned_date').val();
        obj.company_id_returned_date = $('.company_id_returned_date').val();
        obj.utilities_water_etc_date = $('.utilities_water_etc_date').val();
        obj.desktop_etc_cleared_date = $('.desktop_etc_cleared_date').val();
        obj.clearance_obtained_fin_date = $('.clearance_obtained_fin_date').val();
        obj.e_bank_token_date = $('.e_bank_token_date').val();
        obj.clearance_obtained_ae_date = $('.clearance_obtained_ae_date').val();
        obj.car_loan_cleared_date = $('.car_loan_cleared_date').val();
        obj.salary_advance_settled_date = $('.salary_advance_settled_date').val();
        obj.company_stamp_returned_date = $('.company_stamp_returned_date').val();
        obj.medical_insurance_date = $('.medical_insurance_date').val();
        obj.visa_master_commun_date = $('.visa_master_commun_date').val();
        obj.sav_current_account_date = $('.sav_current_account_date').val();
        obj.external_training_cost_date = $('.ExternalTraining_Cost_date').val();

        //obj.attachment_filepath = afterSaveCommonFilePath;  04-05-2020 Nimmi
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_EndofServiceClearance_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess('Changes Saved Succesfully');
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });
        }
    }//Savechanges for P025 End of ServiceClearance By Nimmi Mohan on 07-05-2020

}

// Forward the request After Submit the request
$(document).on('click', '#forward_request_btn', function () {

    $('#cancel_request_btn').css('display', 'none');
    var obj = new Object();
    obj.my_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    obj.request_table_id = request_table_id;
    obj.my_role = "";
    var id = obj.my_id + '~' + obj.request_id + '~' + obj.request_table_id + '~' + obj.my_role;
    $.ajax({
        url: '/Request/PP_Request_Forward/' + id,
        dataType: 'html',
        success: function (data) {
            $('#view_forward_List').html(data);
            $('#view_forward_List').modal('show');
        },
    });
    $('#cancel_request_btn').css('display', 'block');
});

// Submit the forward , then goes the email
$(document).on('click', '.detailed_button_click', function () {

    var obj = new Object();
    obj.my_id = $('#popup_my_id').val();
    obj.request_id = $('#popup_request_id').val();
    obj.my_role = $('#popup_my_role').val();
    obj.request_table_id = $('#popup_request_table_id').val();
    //Basheer on 30-01-2020 to get employeeid
    //obj.forward_emp_id = $('.employee_lost_dropdown_id:selected').val();
    obj.forward_emp_id = $('.employee_lost_dropdown_id').val();
    //obj.forward_emp_id = $('#emp_identify_id').val();
    obj.EmployeeId = $('#emp_identify_id').val();
    obj.reason = $('.hold_reason_text').val();

    var xorKey = 2020; //17-02-2020 ARCHANA SRISHTI
    var source = obj.request_table_id + '~0~' + "ForwardView" + '~0~' + 1 + '~' + obj.forward_emp_id;
    var result = "";
    for (i = 0; i < source.length; ++i) {
        result += String.fromCharCode(xorKey ^ source.charCodeAt(i));
    }
    obj.eForwardUrl = result;
    if (obj.forward_emp_id != 0) {
        $.ajax({
            type: "POST",
            url: "/Request/Submit_RequestForward",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    toastrSuccess(response.Message);
                    $('#view_forward_List').modal('hide');
                }
                else {
                    toastrError(response.Message);
                }
            },
        });
    }
    else {
        toastrError("Please select an employee!");
    }
});

// Initiator Cancel the request by himself
$(document).on('click', '#cancel_request_btn', function () {

    $('#submit_request_btn').prop('disabled', true);
    $('#forward_request_btn').css('display', 'none');
    $('#cancel_request_btn').css('display', 'none');
    $('#cancel_request_btn').prop('disabled', true);
    var obj = new Object();
    obj.my_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    $.ajax({
        type: "POST",
        url: "/Request/Submit_Cancel_Request_By_Creator",
        dataType: "json",
        global: false,
        data: obj,
        success: function (response) {
            if (response.Status) {
                // window.location.href = '/Request/RequestHome/';
                $('.view_with_wftype_id').html('');
                $('.process-btn-section').hide();
                $('.wf-types-list-drop-id').prop("disabled", true);
                $('#edit_request_btn').css('display', 'none'); //Basheer on 30-01-2020
                $('#submit_request_btn').css('display', 'none');//Basheer on 30-01-2020
                //$('#cancel_request_btn').css('display', 'none'); //Basheer on 30-01-2020
                $('#print_request_btn').css('display', 'none');//Basheer on 30-01-2020
                $('#save_request_btn').css('display', 'none'); //Basheer on 30-01-2020
                $('#forward_request_btn').css('display', 'none');
                toastrSuccess(response.Message);
            }
            else {
                toastrError(response.Message);
            }
        },
    });
});

$(".bank_wise_amount").on('change keyup paste', function () {

    $('#bank_amt_required').css('display', 'none');
    $('#save_request_btn').prop('disabled', false);
});

$(".cheque_amt").on('change keyup paste', function () {

    $('#cheque_date_required').css('display', 'none');
    $('#save_request_btn').prop('disabled', false);
});

//---------------------------P009----------------------------------------------

// Bank transfer radio btn click
$(document).on('click', '.bank_radio_btnp_P009', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestp009/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click
$(document).on('click', '.cheque_radio_btn_P009', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP009/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// 14/05/2020 Alena Sics EOSB CALCULATION START------
$(document).on('click', '.bank_radio_btn_P052', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestp052/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click
$(document).on('click', '.cheque_radio_btn_P052', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP052/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

//---------------------------P010-------------------------------------------------------------------------------------------
// Bank transfer radio btn click
$(document).on('click', '.bank_radio_btnp_P010', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestP010/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});
// Cheque radio button click
$(document).on('click', '.cheque_radio_btn_P010', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP010/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Bank transfer radio btn click  P054
$(document).on('click', '.bank_radio_btn_p054', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestP054/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click P054
$(document).on('click', '.cheque_radio_btn_p054', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP054/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

    

//Sibi 06-01-2020
function EmployeeCuntryDetails(emp_Id) {

    $.ajax({
        url: '/Data/GetEmployee_Location_Id_By_Cuntry',
        type: "GET",
        data: { localEmplyee_ID: emp_Id },
        success: function (result) {
            if (result.list.length > 0) {
                $(".wf-cuntry-list-drop-id").html("");
                $(".wf-cuntry-list-drop-id").append($('<option></option>').val("0").html('-- Select Location --'));
                $.each(result.list, function (i, item) {
                    $(".wf-cuntry-list-drop-id").append($('<option></option>').val(item.Value).html(item.Text));
                });
                $(".wf-cuntry-list-drop-id").prop("disabled", false);
            }
            else {
                $(".wf-cuntry-list-drop-id").html("");
                $(".wf-cuntry-list-drop-id").append($('<option></option>').val("0").html('-- No country Found --'));
                $(".wf-cuntry-list-drop-id").prop("disabled", true);
                //$('#wftypeid').val("0");
            }
        }
    });
}

// Bank transfer radio btn click
$(document).on('click', '.bank_radio_btn_P050', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestp050/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click
$(document).on('click', '.cheque_radio_btn_P050', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP050/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Bank transfer radio btn click
$(document).on('click', '.bank_radio_btn_P013', function () {

    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestp013/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click
$(document).on('click', '.cheque_radio_btn_P013', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP013/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

$(document).on('change', '.vacation-advance-reqid', function () {
    var reqid = $(this).val();
    if (reqid != '') {
        $.ajax({
            url: '/Request/GetTaRequestDetails',
            type: "POST",
            data: { 'reqid': reqid },
            dataType: 'JSON',
            success: function (data) {
                $('.lastdayofwork').val(data.lastdate);
                $('.returntoduty').val(data.returntoduty);

            }
        });
    }

});

function Save_P013() {
    var obj = new Object();
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    obj.tanumber = $('.vacation-advance-reqid').val();
    obj.taamountype = $('.vacation-advance-currency').val();
    if ($(".cheque_radio_btn_P013").prop('checked') == true) {// Payment mode is cheque
        obj.payment_mode = "C";
        obj.cheque_date = $('.cheque_date').val();
        obj.amount_sar = $('.cheque_amt').val();
        obj.purpose_text = $('.cheque_purpose_text').val();
        obj.attachment_filepath = $('.attachment_filepath').val();
        obj.payable_to = $('.cheque_payable_to').val();
        obj.tasupplier = $('.cheque_supplier').val();
        obj.tachequeaccountno = $('.cheque_acoountno').val();

        if ($('.cheque_amt').val() == '') {
            $('#cheque_amt_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            x = false;
            obj.attachment_filepath = afterSaveCommonFilePath;
            $('#cheque_amt_required').css('display', 'none');
        }
        if (obj.cheque_date == "") {
            $('#cheque_date_required').css('display', 'block');
            x = true;
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            $('#cheque_date_required').css('display', 'none');
            x = false;
        }
    }
    else {
        obj.payment_mode = "B";// Payment mode is Bank
        obj.amount_sar = $('.bank_wise_amount').val();
        if (obj.amount_sar == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#bank_amt_required').css('display', 'block');
        }
        else {
            x = false;
            obj.from_bank = $('.from_bank_name').val();
            obj.from_addreess = $('.from_bank_address').val();
            obj.from_account_no = $('.from_bank_account_no').val();
            obj.to_beneficiary = $('.to_bank_benificiary').val();
            obj.to_bankname = $('.to_bank_name').val();
            obj.to_address = $('.to_bank_address').val();
            obj.to_account_no = $('.to_bank_account_no').val();
            obj.bank_attachment = afterSaveBankFilePath;
            obj.attachment_filepath = afterSaveCommonFilePath;
            obj.remark = $('.bank_remark').val();
        }
    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_PP_VacationAdvancePayment",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $('#save_request_btn').css('display', 'none');
                    $('#submit_request_btn').css('display', 'block');
                    requestId = response.Request_Id;

                    var request_full_Id = application_name + '-' + requestId;
                    //$('#request_id_display').text(request_full_Id);
                    $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020
                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                    $('#submit_request_btn').prop('disabled', false);
                    $('#cancel_request_btn').css('display', 'block');
                    $('#cancel_request_btn').prop('disabled', false);
                    $(".se-pre-con").hide();
                    toastrSuccess(response.Message);
                    $('.request_status_now').text('New request not submitted');

                }
                else {
                    $(".se-pre-con").hide();
                    toastrError(response.Message);
                    $('#save_request_btn').css('display', 'block');
                    $('#submit_request_btn').css('display', 'none');
                    requestId = "";
                    $('#submit_request_btn').prop('disabled', true);
                    $('#cancel_request_btn').css('display', 'block');
                    $('#cancel_request_btn').prop('disabled', false);
                    $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                }

            },
        });
    }
    else {
        $('#submit_request_btn').prop('disabled', false);
        $('#cancel_request_btn').css('display', 'block');
        $('#cancel_request_btn').prop('disabled', false);
    }
}

function Submit_P013() {

    var obj = new Object();
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    obj.tanumber = $('.vacation-advance-reqid').val();
    obj.taamountype = $('.vacation-advance-currency').val();
    if ($(".cheque_radio_btn_P013").prop('checked') == true) {// Payment mode is cheque
        obj.payment_mode = "C";
        obj.cheque_date = $('.cheque_date').val();
        obj.amount_sar = $('.cheque_amt').val();
        obj.purpose_text = $('.cheque_purpose_text').val();
        obj.attachment_filepath = $('.attachment_filepath').val();
        obj.payable_to = $('.cheque_payable_to').val();
        obj.tasupplier = $('.cheque_supplier').val();
        obj.tachequeaccountno = $('.cheque_acoountno').val();
        if ($('.cheque_amt').val() == '') {
            $('#cheque_amt_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            x = true;
        }
        else {
            x = false;
            obj.attachment_filepath = afterSaveCommonFilePath;
            $('#cheque_amt_required').css('display', 'none');
        }
        if (obj.cheque_date == "") {
            $('#cheque_date_required').css('display', 'block');
            x = true;
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            $('#cheque_date_required').css('display', 'none');
            x = false;
        }
    }
    else {
        obj.payment_mode = "B";// Payment mode is Bank
        obj.amount_sar = $('.bank_wise_amount').val();
        if (obj.amount_sar == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#bank_amt_required').css('display', 'block');
        }
        else {
            x = false;
            obj.from_bank = $('.from_bank_name').val();
            obj.from_addreess = $('.from_bank_address').val();
            obj.from_account_no = $('.from_bank_account_no').val();
            obj.to_beneficiary = $('.to_bank_benificiary').val();
            obj.to_bankname = $('.to_bank_name').val();
            obj.to_address = $('.to_bank_address').val();
            obj.to_account_no = $('.to_bank_account_no').val();
            obj.bank_attachment = afterSaveBankFilePath;
            obj.attachment_filepath = afterSaveCommonFilePath;
            obj.remark = $('.bank_remark').val();
        }
    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_PP_VacationAdvancePayment_Edit_After_Save",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $(".se-pre-con").hide();
                    toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'none');//Basheer on 30-01-2020
                    $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                }
                else {
                    $(".se-pre-con").hide();
                }
            },
        });
    }
    else {
        $('#submit_request_btn').prop('disabled', false);
    }
}

$(document).on('change', '.vacation-advance-reqid', function () {
    var reqid = $(this).val();
    if (reqid != '') {
        $.ajax({
            url: '/Request/GetTaRequestDetails',
            type: "POST",
            data: { 'reqid': reqid },
            dataType: 'JSON',
            success: function (data) {
                $('.lastdayofwork').val(data.lastdate);
                $('.returntoduty').val(data.returntoduty);

            }
        });
    }
});

// Bank transfer radio btn click P056
$(document).on('click', '.bank_radio_btn_p056', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestP056/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click P056
$(document).on('click', '.cheque_radio_btn_p056', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP056/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

//Basheer on 23-01-2020 to edit the details
$(document).on('click', '#edit_request_btn', function () {
    EditRequest();
});

//-------------------------------------------P015 start here---------------------------------------
// Bank transfer radio btn click
$(document).on('click', '.bank_radio_P015', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestp015/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click
$(document).on('click', '.cheque_radio_P015', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP015/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});
//function for Save and Edit For P030 Module done by : // Chitra Srishti  on 25.06.2020//strt
function Save_P030() {

    var obj = new Object();
    var Education_Obj = new Object();
    obj._FileList = [];
    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.application_ids = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    Education_Obj.application_id = $('#application-list-drop-id').val();
    Education_Obj.application_ids = $('#application-list-drop-id').val();
    Education_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Education_Obj.creator_id = $('#emp_identify_id').val();
    Education_Obj.request_id = requestId;
    Education_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    //To save records  in master table//
    var ddl_Employeegroup = document.getElementById("ddl_Employeegroup");
    var EmployeeGroup = ddl_Employeegroup.options[ddl_Employeegroup.selectedIndex].text;
    Education_Obj.Employee_Group = EmployeeGroup;
    Education_Obj.Remarks = $('.EduAss_Remarks').val();
    Education_Obj.Grand_Total = $('.Total_paid').val();
    obj.EducationalAssistanceModel = Education_Obj;






    if (Education_Obj.Employee_Group == '--Select--') {
        $('#Employee_Group_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields.");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Employee_Group_required').css('display', 'none');
    }
    //To save records  in details table//
    $('#tbl_Education tbody tr').each(function () {
        var row = $(this);
        if (($(this).find('.Ins_Name').val() == '') && ($(this).find('.Ins_Birth_Date').val() == '01/01/0001') && ($(this).find('.Ins_Transport').val() == '') && ($(this).find('.Ins_Others').val() == '') && ($(this).find('.Ins_Exchange_Rate').val() == '') && ($(this).find('.Ins_FromDate').val() == '01/01/0001') && ($(this).find('.Ins_ToDate').val() == '01/01/0001') && ($(this).find('.Ins_Location').val() == 0)) {
            toastrError("Please fill minimum one Row. ");
            x = true;
            return;
        }
        else {
            x = false;
        }

    });
    if (x == false) {
        var EducationDetails = new Array();
        $('#tbl_Education tbody tr').each(function () {
            var row = $(this);
            if (($(this).find('.Ins_Name').val() != '') && ($(this).find('.Ins_Birth_Date').val() != '01/01/0001') && ($(this).find('.Ins_Transport').val() != '') && ($(this).find('.Ins_Others').val() != '') && ($(this).find('.Ins_Exchange_Rate').val() != '') && ($(this).find('.Ins_FromDate').val() != '01/01/0001') && ($(this).find('.Ins_ToDate').val() != '01/01/0001') && ($(this).find('.Ins_Location').val() != 0)) {

                var EducationAssistanceobject = {};
                EducationAssistanceobject.Child_Name = row.find('.Ins_Name').val();
                EducationAssistanceobject.Birth_Date = row.find('.Ins_Birth_Date').val();
                EducationAssistanceobject.School_Fees = row.find('.Ins_School').val();
                EducationAssistanceobject.Transport_Fees = row.find('.Ins_Transport').val();
                EducationAssistanceobject.Others = row.find('.Ins_Others').val();
                EducationAssistanceobject.Foreign_Currency = row.find('.Ins_Foreign_Currency').val();
                EducationAssistanceobject.Exchange_Rate = row.find('.Ins_Exchange_Rate').val();
                EducationAssistanceobject.From_Date = row.find('.Ins_FromDate').val();
                EducationAssistanceobject.To_Date = row.find('.Ins_ToDate').val();
                EducationAssistanceobject.Location_Id = row.find('.Ins_Location').val();
                EducationDetails.push(EducationAssistanceobject);
                x = false;


            }
            else {
                toastrError("Please fill all the details in the row.");
                x = true;
                return;
            }
        });
        obj._Educationassistance = EducationDetails;
        if (x == false) {
            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_EducationalAssistance",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {

                    if (response.Status) {

                        $('#save_request_btn').css('display', 'none');
                        $('#submit_request_btn').css('display', 'block');
                        requestId = response.Request_Id;

                        var request_full_Id = application_name + '-' + requestId;

                        $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#print_request_btn').css('display', 'block');

                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                        $(".se-pre-con").hide();
                        toastrSuccess(response.Message);

                        var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                        $('.request_status_now').text(msg);

                        public_PrintApprove = true;
                    }
                    else {

                        $(".se-pre-con").hide();
                        toastrError(response.Message);
                        $('#save_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'none');
                        requestId = "";
                        $('#submit_request_btn').prop('disabled', true);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                        $('#edit_request_btn').css('display', 'none');
                        $('#forward_request_btn').css('display', 'none');

                    }

                },

            });
        }

    }
}
function Edit_P030() {


    var obj = new Object();
    var Education_Obj = new Object();
    obj._FileList = [];
    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.application_ids = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    Education_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    Education_Obj.application_id = $('#application-list-drop-id').val();
    Education_Obj.application_ids = $('#application-list-drop-id').val();
    Education_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Education_Obj.creator_id = $('#emp_identify_id').val();
    Education_Obj.request_id = requestId;
    //To save records  in master table//
    var ddl_Employeegroup = document.getElementById("ddl_Employeegroup");
    var EmployeeGroup = ddl_Employeegroup.options[ddl_Employeegroup.selectedIndex].text;
    Education_Obj.Employee_Group = EmployeeGroup;
    Education_Obj.Remarks = $('.EduAss_Remarks').val();
    Education_Obj.Grand_Total = $('.Total_paid').val();
    Education_Obj.Total_Entitlement = $('.Total_Entitlement').val();
    Education_Obj.Amount_Approved = $('.Amount_Paid').val();
    Education_Obj.Amount_Paid = $('.Amount_Approved').val();
    Education_Obj.Paid_Payroll = $('.Paid_Payroll').val();
    obj.EducationalAssistanceModel = Education_Obj;


    if (Education_Obj.Employee_Group == '--Select--') {
        $('#Employee_Group_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields.");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Employee_Group_required').css('display', 'none');
    }
    $('#tbl_Education tbody tr').each(function () {
        var row = $(this);
        if (($(this).find('.Ins_Name').val() == '') && ($(this).find('.Ins_Birth_Date').val() == '01/01/0001') && ($(this).find('.Ins_Transport').val() == '') && ($(this).find('.Ins_Others').val() == '') && ($(this).find('.Ins_Exchange_Rate').val() == '') && ($(this).find('.Ins_FromDate').val() == '01/01/0001') && ($(this).find('.Ins_ToDate').val() == '01/01/0001') && ($(this).find('.Ins_Location').val() == 0)) {
            toastrError("Please fill minimum one Row. ");
            x = true;
            return;
        }
        else {
            x = false;
        }

    });
    if (x == false) {
        var EducationDetails = new Array();
        $('#tbl_Education tbody tr').each(function () {
            var row = $(this);
            if (($(this).find('.Ins_Name').val() != '') && ($(this).find('.Ins_Birth_Date').val() != '01/01/0001') && ($(this).find('.Ins_Transport').val() != '') && ($(this).find('.Ins_Others').val() != '') && ($(this).find('.Ins_Exchange_Rate').val() != '') && ($(this).find('.Ins_FromDate').val() != '01/01/0001') && ($(this).find('.Ins_ToDate').val() != '01/01/0001') && ($(this).find('.Ins_Location').val() != 0)) {

                var EducationAssistanceobject = {};
                EducationAssistanceobject.Child_Name = row.find('.Ins_Name').val();
                EducationAssistanceobject.Birth_Date = row.find('.Ins_Birth_Date').val();
                EducationAssistanceobject.School_Fees = row.find('.Ins_School').val();
                EducationAssistanceobject.Transport_Fees = row.find('.Ins_Transport').val();
                EducationAssistanceobject.Others = row.find('.Ins_Others').val();
                EducationAssistanceobject.Foreign_Currency = row.find('.Ins_Foreign_Currency').val();
                EducationAssistanceobject.Exchange_Rate = row.find('.Ins_Exchange_Rate').val();
                EducationAssistanceobject.From_Date = row.find('.Ins_FromDate').val();
                EducationAssistanceobject.To_Date = row.find('.Ins_ToDate').val();
                EducationAssistanceobject.Location_Id = row.find('.Ins_Location').val();
                EducationDetails.push(EducationAssistanceobject);
                x = false;

            }
            else {
                toastrError("Please fill all the details in the row");
                x = true;
                return;
            }
        });
        obj._Educationassistance = EducationDetails;
        if (x == false) {

            $(".se-pre-con").show();
            $.ajax({
                type: "POST",
                url: "/Request/Submit_PP_EducationalAssistance_Edit_After_Save",
                dataType: "json",
                global: false,
                data: obj,
                success: function (response) {
                    if (response.Status) {
                        $(".se-pre-con").hide();
                        toastrSuccess("Changes Saved Sucessfully");
                        $('#edit_request_btn').css('display', 'block');
                        $('#forward_request_btn').css('display', 'block');
                        $('#submit_request_btn').css('display', 'block');
                        $('#submit_request_btn').prop('disabled', false);
                        $('#cancel_request_btn').css('display', 'block');
                        $('#cancel_request_btn').prop('disabled', false);
                    }
                    else {
                        $(".se-pre-con").hide();
                    }
                },
            });

        }
        else {
            $('#submit_request_btn').prop('disabled', false);

        }
    }
}
//function for Save and Edit For P030 Module done by  : // Chitra Srishti on 25.06.2020//end 
//function for Save and Edit For P003 Module done by : // Chitra Srishti on 25.06.2020//strt
function Save_P003() {

    var obj = new Object();
    var Ticket_Obj = new Object();

    obj._FileList = [];
    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.application_ids = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    Ticket_Obj.application_id = $('#application-list-drop-id').val();
    Ticket_Obj.application_ids = $('#application-list-drop-id').val();
    Ticket_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Ticket_Obj.creator_id = $('#emp_identify_id').val();
    Ticket_Obj.request_id = requestId;
    Ticket_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    var ddl_TA_Request_No = document.getElementById("ddl_TA_Request_No");
    var TA_Number = ddl_TA_Request_No.options[ddl_TA_Request_No.selectedIndex].text;
    Ticket_Obj.TA_Request_No = TA_Number;
    Ticket_Obj.TicketNumber = $('.Ticket_No').val();
    Ticket_Obj.TicketRouting = $('.Ticket_Routing').val();
    Ticket_Obj.RequestDetails = $('.RequestDetails').val();
    obj.TicketRefundModel = Ticket_Obj;

    if (Ticket_Obj.TA_Request_No == '--Select--') {
        $('#TA_Request_No_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#TA_Request_No_required').css('display', 'none');
    }

    if ($('.Ticket_No').val() == "") {
        $('#Ticket_No_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Ticket_No_required').css('display', 'none');
    }
    if ($('.Ticket_Routing').val() == "") {
        $('#Ticket_Routing_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Ticket_Routing_required').css('display', 'none');
    }
    if ($('.RequestDetails').val() == "") {
        $('#Request_Details_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Request_Details_required').css('display', 'none');
    }
    if (obj._FileList.length == 0) {
        $('#File_upload_required').css('display', 'block');
        toastrError("Please Attach One Document");
        x = true;
        return;
    }
    else {
        x = false;
        $('#File_upload_required').css('display', 'none');
    }
    if (x == true) {
        toastrError("Please fill the Mandatory fields");
    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_PP_Ticket_Refund",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {

                if (response.Status) {

                    $('#save_request_btn').css('display', 'none');
                    $('#submit_request_btn').css('display', 'block');
                    requestId = response.Request_Id;

                    var request_full_Id = application_name + '-' + requestId;

                    $('#request_id_display').text($('#application_code').val() + '-' + requestId);

                    $('#edit_request_btn').css('display', 'block');
                    $('#forward_request_btn').css('display', 'block');
                    $('#print_request_btn').css('display', 'block');

                    $('#submit_request_btn').prop('disabled', false);
                    $('#cancel_request_btn').css('display', 'block');
                    $('#cancel_request_btn').prop('disabled', false);
                    $(".se-pre-con").hide();
                    toastrSuccess(response.Message);
                    var msg = 'Request ' + $('#application_code').val() + '-' + requestId + ' not submitted';
                    $('.request_status_now').text(msg);

                    public_PrintApprove = true;
                }
                else {

                    $(".se-pre-con").hide();
                    toastrError(response.Message);
                    $('#save_request_btn').css('display', 'block');
                    $('#submit_request_btn').css('display', 'none');
                    requestId = "";
                    $('#submit_request_btn').prop('disabled', true);
                    $('#cancel_request_btn').css('display', 'none');
                    //$('#cancel_request_btn').prop('disabled', false);
                    $('#edit_request_btn').css('display', 'none');
                    $('#forward_request_btn').css('display', 'none');

                }

            },

        });
    }


}
function Edit_P003() {

    var obj = new Object();
    var Ticket_Obj = new Object();
    obj._FileList = [];
    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.application_ids = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    Ticket_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    Ticket_Obj.application_id = $('#application-list-drop-id').val();
    Ticket_Obj.application_ids = $('#application-list-drop-id').val();
    Ticket_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Ticket_Obj.creator_id = $('#emp_identify_id').val();
    Ticket_Obj.request_id = requestId;
    //To save records  in master table//
    var ddl_TA_Request_No = document.getElementById("ddl_TA_Request_No");
    var TA_Number = ddl_TA_Request_No.options[ddl_TA_Request_No.selectedIndex].text;
    Ticket_Obj.TA_Request_No = TA_Number;
    Ticket_Obj.TicketNumber = $('.Ticket_No').val();
    Ticket_Obj.TicketRouting = $('.Ticket_Routing').val();
    Ticket_Obj.RequestDetails = $('.RequestDetails').val();
    obj.TicketRefundModel = Ticket_Obj;

    if (Ticket_Obj.TA_Request_No == '--Select--') {
        $('#TA_Request_No_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#TA_Request_No_required').css('display', 'none');
    }
    if ($('.Ticket_No').val() == "") {
        $('#Ticket_No_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Ticket_No_required').css('display', 'none');
    }
    if ($('.Ticket_Routing').val() == "") {
        $('#Ticket_Routing_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Ticket_Routing_required').css('display', 'none');
    }
    if ($('.RequestDetails').val() == "") {
        $('#Request_Details_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Request_Details_required').css('display', 'none');
    }
    if (obj._FileList.length == 0) {
        $('#File_upload_required').css('display', 'block');
        toastrError("Please Attach One Document");
        x = true;
        return;
    }
    else {
        x = false;
        $('#File_upload_required').css('display', 'none');
    }
    if (x == false) {

        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_PP_Ticket_Refund_Edit_After_Save",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {


                    toastrSuccess('Changes Saved Succesfully');
                    $(".se-pre-con").hide();
                    $('#edit_request_btn').css('display', 'block');
                    $('#forward_request_btn').css('display', 'block');
                    $('#submit_request_btn').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    $('#cancel_request_btn').css('display', 'block');
                    $('#cancel_request_btn').prop('disabled', false);
                }
                else {
                    $(".se-pre-con").hide();
                    toastrError(response.Message);
                }
            },
        });

    }
    else {
        $('#submit_request_btn').prop('disabled', false);

    }

}
//function for Save and Edit For P003 Module done by : // Chitra Srishti on 25.06.2020//end 

function Save_P015() {   //Terin on 12/5/2020
    debugger;
    var obj = new Object();
    obj._FileList = [];


    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    var application_name = $('#application-list-drop-id :selected').text();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    obj.currenctType = $('.currency_type_dropdown_id').val();
    obj.blcontrollerid = $('.Rt_controller_id').val();
    obj.traname = $('.Rt_Name').val();
    obj.chargecostcenter = $('.Charge_cost').val();
    obj.chargeaccount = $('.Charge_account').val();
    obj.rt_remarks = $('.remark_p015').val();
    if ($(".cheque_radio_P015").prop('checked') == true) {// Payment mode is cheque
        obj.payment_mode = "C";
        obj.cheque_date = $('.cheque_date').val();
        obj.amount_sar = $('.cheque_amt').val();
        obj.purpose_text = $('.cheque_purpose_text').val();
        obj.attachment_filepath = $('.attachment_filepath').val();
        obj.payable_to = $('.cheque_payable_to').val();

        if ($('.Rt_controller_id :selected').val() == "") {
            x = true;
            $('#Controller-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Controller-required').css('display', 'none');

        }

        if (obj.traname == "") {
            x = true;
            $('#Rt_Name_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Rt_Name_required').css('display', 'none');

        }




        if (obj.chargecostcenter == "") {
            x = true;
            $('#Charge_cost-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Charge_cost-required').css('display', 'none');

        }
        if (obj.chargeaccount == "") {
            x = true;
            $('#Charge_account-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Charge_account-required').css('display', 'none');

        }
        if (obj.cheque_date == "") {
            x = true;
            $('#cheque_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            x = false;
            $('#cheque_date_required').css('display', 'none');

        }

        if (obj.currenctType == "") {
            x = true;
            $('#currency_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            x = false;
            $('#currency_required').css('display', 'none');

        }
        if ($('.cheque_amt').val() == '') {
            x = true;
            $('#cheque_amt_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);


        }
        else {
            x = false;
            obj.attachment_filepath = afterSaveCommonFilePath;
            $('#cheque_amt_required').css('display', 'none');
        }
        if (obj.purpose_text == "") {
            x = true;
            $('#cheque_purpose_text_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#cheque_purpose_text_required').css('display', 'none');

        }

        if (obj.blcontrollerid == "" || obj.traname == "" || obj.chargecostcenter == "" || obj.chargeaccount == "" || obj.cheque_date == "" || obj.currenctType == "" || obj.amount_sar == "" || obj.purpose_text == "") {
            x = true;
        }




    }
    else {
        obj.payment_mode = "B";// Payment mode is Bank
        obj.amount_sar = $('.bank_wise_amount').val();
        obj.from_bank = $('.from_bank_name').val();
        obj.from_addreess = $('.from_bank_address').val();
        obj.from_account_no = $('.from_bank_account_no').val();
        obj.to_beneficiary = $('.to_bank_benificiary').val();
        obj.to_bankname = $('.to_bank_name').val();
        obj.to_address = $('.to_bank_address').val();
        obj.to_account_no = $('.to_bank_account_no').val();
        obj.to_iban = $('.to_iban').val();
        //  obj.bank_attachment = afterSaveBankFilePath;
        // obj.attachment_filepath = $('.attachment_filepath').val();
        obj.attachment_filepath = afterSaveCommonFilePath;
        obj.purpose_text = $('.bank_remark').val();

        if ($('.Rt_controller_id :selected').val() == "") {
            x = true;
            $('#Controller-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Controller-required').css('display', 'none');

        }

        if (obj.traname == "") {
            x = true;
            $('#Rt_Name_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Rt_Name_required').css('display', 'none');

        }

        if (obj.chargecostcenter == "") {
            x = true;
            $('#Charge_cost-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Charge_cost-required').css('display', 'none');

        }
        if (obj.chargeaccount == "") {
            x = true;
            $('#Charge_account-required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#Charge_account-required').css('display', 'none');

        }
        if (obj.currenctType == "") {
            x = true;
            $('#currency_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            x = false;
            $('#currency_required').css('display', 'none');

        }

        if (obj.amount_sar == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#bank_amt_required').css('display', 'block');

        }
        else {
            x = false;
            $('#bank_amt_required').css('display', 'none');
        }
        if (obj.from_bank == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#bank_name_required').css('display', 'block');

        }
        else {
            x = false;
            $('#bank_name_required').css('display', 'none');

        }
        if (obj.from_addreess == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#address_required').css('display', 'block');

        }
        else {
            x = false;
            $('#address_required').css('display', 'none');

        }
        if (obj.from_account_no == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#Account_no_required').css('display', 'block');

        }
        else {
            x = false;
            $('#Account_no_required').css('display', 'none');
        }

        if (obj.to_beneficiary == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#benificary_required').css('display', 'block');

        }
        else {
            x = false;
            $('#benificary_required').css('display', 'none');
        }
        if (obj.to_bankname == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_bank_name_required').css('display', 'block');

        }
        else {
            x = false;
            $('#to_bank_name_required').css('display', 'none');
        }

        if (obj.to_address == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_address_required').css('display', 'block');

        }
        else {
            x = false;
            $('#to_address_required').css('display', 'none');
        }
        if (obj.to_account_no == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_account_required').css('display', 'block');

        }
        else {
            x = false;
            $('#to_account_required').css('display', 'none');

        }

        if (obj.to_iban == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#iban_required').css('display', 'block');

        }
        else {
            x = false;
            $('#iban_required').css('display', 'none');

        }

        if (obj.purpose_text == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_purpose_text_required').css('display', 'block');

        }
        else {
            x = false;
            $('#to_purpose_text_required').css('display', 'none');

        }

        if (obj.blcontrollerid == "" || obj.traname == "" || obj.chargecostcenter == "" || obj.chargeaccount == "" || obj.currenctType == "" || obj.amount_sar == "" || obj.from_bank == "" || obj.from_addreess == "" || obj.from_account_no == "" || obj.to_beneficiary == "" || obj.to_bankname == "" || obj.to_address == "" || obj.to_account_no == "" || obj.to_iban == "" || obj.purpose_text == "") {
            x = true;
        }



    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_PP_Recruitment_Training_Payment",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $('#save_request_btn').css('display', 'none');
                    $('#submit_request_btn').css('display', 'block');
                    requestId = response.Request_Id;
                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#print_request_btn').css('display', 'block');//Basheer on 30-01-2020
                    var request_full_Id = application_name + '-' + requestId;
                    //$('#request_id_display').text(request_full_Id);
                    $('#request_id_display').text($('#application_code').val() + '-' + requestId); // Archana Srishti 07-02-2020

                    $('#submit_request_btn').prop('disabled', false);
                    $('#cancel_request_btn').css('display', 'block');
                    $('#cancel_request_btn').prop('disabled', false);
                    $(".se-pre-con").hide();
                    toastrSuccess(response.Message);
                    $('.request_status_now').text('New request not submitted');

                }
                else {
                    $(".se-pre-con").hide();
                    toastrError(response.Message);
                    $('#save_request_btn').css('display', 'block');
                    $('#submit_request_btn').css('display', 'none');
                    requestId = "";
                    $('#submit_request_btn').prop('disabled', true);
                    $('#cancel_request_btn').css('display', 'none');
                    // $('#cancel_request_btn').prop('disabled', false);
                    $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                }

            },
        });
    }
    else {
        toastrError("Please fill all the mandatory fields"); //Terrin on 14/5/2020
        $('#submit_request_btn').prop('disabled', false);
        $('#cancel_request_btn').css('display', 'none');
        //$('#cancel_request_btn').prop('disabled', false);
    }
}

function Submit_P015() {

    var obj = new Object();
    obj._FileList = [];

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    var application_name = $('#application-list-drop-id :selected').text();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    obj.currenctType = $('.currency_type_dropdown_id').val();
    obj.blcontrollerid = $('.Rt_controller_id').val();
    obj.traname = $('.Rt_Name').val();
    obj.chargecostcenter = $('.Charge_cost').val();
    obj.chargeaccount = $('.Charge_account').val();
    obj.rt_remarks = $('.remark_p015').val();
    if ($(".cheque_radio_P015").prop('checked') == true) {// Payment mode is cheque
        obj.payment_mode = "C";
        obj.cheque_date = $('.cheque_date').val();
        obj.amount_sar = $('.cheque_amt').val();
        obj.purpose_text = $('.cheque_purpose_text').val();
        obj.attachment_filepath = $('.attachment_filepath').val();
        obj.payable_to = $('.cheque_payable_to').val();

        if (obj.cheque_date == "") {
            x = true;
            $('#cheque_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);

        }
        else {
            x = false;
            $('#cheque_date_required').css('display', 'none');

        }
        if ($('.cheque_amt').val() == '') {
            x = true;
            $('#cheque_amt_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);


        }
        else {
            x = false;
            obj.attachment_filepath = afterSaveCommonFilePath;
            $('#cheque_amt_required').css('display', 'none');
        }
        if (obj.purpose_text == "") {
            x = true;
            $('#cheque_purpose_text_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
        }
        else {
            x = false;
            $('#cheque_purpose_text_required').css('display', 'none');

        }
    }
    else {
        obj.payment_mode = "B";// Payment mode is Bank
        obj.amount_sar = $('.bank_wise_amount').val();
        obj.from_bank = $('.from_bank_name').val();
        obj.from_addreess = $('.from_bank_address').val();
        obj.from_account_no = $('.from_bank_account_no').val();
        obj.to_beneficiary = $('.to_bank_benificiary').val();
        obj.to_bankname = $('.to_bank_name').val();
        obj.to_address = $('.to_bank_address').val();
        obj.to_account_no = $('.to_bank_account_no').val();
        obj.to_iban = $('.to_iban').val();
        obj.attachment_filepath = $('.attachment_filepath').val();
        obj.purpose_text = $('.bank_remark').val();

        if (obj.amount_sar == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#bank_amt_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#bank_amt_required').css('display', 'none');
        }
        if (obj.from_bank == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#bank_name_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#bank_name_required').css('display', 'none');

        }
        if (obj.from_addreess == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#address_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#address_required').css('display', 'none');

        }
        if (obj.from_account_no == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#Account_no_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#Account_no_required').css('display', 'none');
        }

        if (obj.to_beneficiary == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#benificary_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#benificary_required').css('display', 'none');
        }
        if (obj.to_bankname == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_bank_name_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#to_bank_name_required').css('display', 'none');
        }

        if (obj.to_address == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_address_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#to_address_required').css('display', 'none');
        }
        if (obj.to_account_no == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_account_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#to_account_required').css('display', 'none');

        }

        if (obj.to_iban == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#iban_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#iban_required').css('display', 'none');

        }

        if (obj.purpose_text == "") {
            x = true;
            $('#submit_request_btn').prop('disabled', false);
            $('#to_purpose_text_required').css('display', 'block');
            return;
        }
        else {
            x = false;
            $('#to_purpose_text_required').css('display', 'none');

        }
    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_PP_Recruitment_Training_Edit_After_Save",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $(".se-pre-con").hide();
                    toastrSuccess('Changes Saved Succesfully'); //Basheer on 23-01-2020
                    $('#edit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'block');//Basheer on 30-01-2020
                    $('#submit_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#submit_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                    $('#cancel_request_btn').css('display', 'block');//Basheer on 23-01-2020
                    $('#cancel_request_btn').prop('disabled', false);//Basheer on 23-01-2020
                }
                else {
                    $(".se-pre-con").hide();
                }
            },
        });
    }
    else {
        $('#submit_request_btn').prop('disabled', false);
    }

}


// Bank transfer radio btn click P099
$(document).on('click', '.bank_radio_btn_p099', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequestP099/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

// Cheque radio button click P099
$(document).on('click', '.cheque_radio_btn_p099', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetChequedetailsForPPrequestP099/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

////Bank transfer radio btn click P051
$(document).on('click', '.bank_radio_btn_SalaryAdvance', function () {
    var id = $('.employee-list-drop-id :selected').val();
    if (id != "0") {
        $.ajax({
            url: '/Request/GetBankdetailsForPPrequest51/' + id,
            type: "GET",
            dataType: 'html',
            success: function (data) {
                $('.payment-mode-id').html('');
                $('.payment-mode-id').html(data);
            }
        });
    }
});

//Basheer on 27-02-2020
var lists = [];

$(document).on('click', '#print_request_btn', function () {
    //Print page encryption(Preema)
    var obj = new Object();
    obj.my_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    obj.request_table_id = request_table_id;
    obj.my_role = "";
    var xorKey = 2020;
    var result = "";
    var id = obj.my_id + '~' + obj.request_id + '~' + obj.request_table_id + '~' + obj.my_role;
    for (i = 0; i < id.length; ++i) {
        result += String.fromCharCode(xorKey ^ id.charCodeAt(i));
    }
    window.open('/Request/Print_Page_Creatorside/' + result, '_blank');

});

// Terrin on 13-06-2020
$(document).on('change', '.terminatn_date', function (){
    debugger;
    // $('.terminatn_date input').on('change', function () {

        var datearray = $('.terminatn_date').val().split("-");
        var montharray = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];
        var year =  datearray[0];
    //var month = montharray.indexOf(datearray[1]) + 1;
        var month = datearray[1];
        var day = datearray[2];
        var minDate = (year + "-" + month + "-" + day);
        $('.toolbx_returned_date').attr('max', minDate);
        $('.workstation_cleared_date').attr('max', minDate);
        $('.official_business_doc_date').attr('max', minDate);
        $('.site_project_clearance_date').attr('max', minDate);
        $('.uni_form_date').attr('max', minDate);
        $('.safety_equipment_date').attr('max', minDate);
        $('.workflow_approvals_date').attr('max', minDate);
        $('.housing_household_date').attr('max', minDate);
        $('.car_garage_key_date').attr('max', minDate);
        $('.gatepass_sticker_date').attr('max', minDate);
        $('.Mobile_returned_date').attr('max', minDate);
        $('.company_id_returned_date').attr('max', minDate);
        $('.utilities_water_etc_date').attr('max', minDate);
        $('.desktop_etc_cleared_date').attr('max', minDate);
        $('.clearance_obtained_fin_date').attr('max', minDate);
        $('.e_bank_token_date').attr('max', minDate);
        $('.clearance_obtained_ae_date').attr('max', minDate);
        $('.car_loan_cleared_date').attr('max', minDate);
        $('.salary_advance_settled_date').attr('max', minDate);
        $('.company_stamp_returned_date').attr('max', minDate);
        $('.medical_insurance_date').attr('max', minDate);
        $('.visa_master_commun_date').attr('max', minDate);
        $('.sav_current_account_date').attr('max', minDate);
        $('.is_service_deactivtn_date').attr('max', minDate);
        
       

});

$(document).on('click', '.toolbx_returned_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val()=='')
    {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.toolbx_returned_date').prop('disabled', true);
    }
    else {
        $('.toolbx_returned_date').prop('disabled', false);
    }

});

$(document).on('click', '.workstation_cleared_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val()=='')
    {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.workstation_cleared_date').prop('disabled', true);
    }
    else {
        $('.workstation_cleared_date').prop('disabled', false);
    }

});

$(document).on('click', '.official_business_doc_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.official_business_doc_date').prop('disabled', true);
    }
    else {
        $('.official_business_doc_date').prop('disabled', false);
    }

});

$(document).on('click', '.site_project_clearance_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.site_project_clearance_date').prop('disabled', true);
    }
    else {
        $('.site_project_clearance_date').prop('disabled', false);
    }

});

$(document).on('click', '.uni_form_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.uni_form_date').prop('disabled', true);
    }
    else {
        $('.uni_form_date').prop('disabled', false);
    }

});


$(document).on('click', '.safety_equipment_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.safety_equipment_date').prop('disabled', true);
    }
    else {
        $('.safety_equipment_date').prop('disabled', false);
    }

});

$(document).on('click', '.workflow_approvals_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.workflow_approvals_date').prop('disabled', true);
    }
    else {
        $('.workflow_approvals_date').prop('disabled', false);
    }

});

$(document).on('click', '.housing_household_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.housing_household_date').prop('disabled', true);
    }
    else {
        $('.housing_household_date').prop('disabled', false);
    }

});

$(document).on('click', '.car_garage_key_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.car_garage_key_date').prop('disabled', true);
    }
    else {
        $('.car_garage_key_date').prop('disabled', false);
    }

});

$(document).on('click', '.gatepass_sticker_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.gatepass_sticker_date').prop('disabled', true);
    }
    else {
        $('.gatepass_sticker_date').prop('disabled', false);
    }

});

$(document).on('click', '.Mobile_returned_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.Mobile_returned_date').prop('disabled', true);
    }
    else {
        $('.Mobile_returned_date').prop('disabled', false);
    }

});

$(document).on('click', '.company_id_returned_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.company_id_returned_date').prop('disabled', true);
    }
    else {
        $('.company_id_returned_date').prop('disabled', false);
    }

});

$(document).on('click', '.utilities_water_etc_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('#terminatn_date').css('display', 'block');
        toastrError("Please fill the Terimination Date");
        $('.utilities_water_etc_date').prop('disabled', true);
    }
    else {
        $('.utilities_water_etc_date').prop('disabled', false);
    }

});

$(document).on('click', '.terminatn_date', function () {
    debugger;
    // $('.terminatn_date input').on('change', function () {
    if ($('.terminatn_date').val() == '') {
        $('.toolbx_returned_date').prop('disabled', false);
        $('.workstation_cleared_date').prop('disabled', false);
        $('.official_business_doc_date').prop('disabled', false);
        $('.site_project_clearance_date').prop('disabled', false);
        $('.uni_form_date').prop('disabled', false);
        $('.safety_equipment_date').prop('disabled', false);
        $('.workflow_approvals_date').prop('disabled', false);
        $('.housing_household_date').prop('disabled', false);
        $('.car_garage_key_date').prop('disabled', false);
        $('.gatepass_sticker_date').prop('disabled', false);
        $('.Mobile_returned_date').prop('disabled', false);
        $('.company_id_returned_date').prop('disabled', false);
        $('.utilities_water_etc_date').prop('disabled', false);
        $('.desktop_etc_cleared_date').prop('disabled', false);
        $('.clearance_obtained_fin_date').prop('disabled', false);
        $('.e_bank_token_date').prop('disabled', false);
        $('.clearance_obtained_ae_date').prop('disabled', false);
        $('.car_loan_cleared_date').prop('disabled', false);
        $('.salary_advance_settled_date').prop('disabled', false);
        $('.company_stamp_returned_date').prop('disabled', false);
        $('.medical_insurance_date').prop('disabled', false);
        $('.visa_master_commun_date').prop('disabled', false);
        $('.sav_current_account_date').prop('disabled', false);
    }
    

});




//P016-Internal Transfer(Preema)
function EmployeeDetails_P016(emp_id, wf_type) {
    if (emp_id != "") {
        $.ajax({
            url: '/Request/GetEmployee_Details',
            type: "GET",
            data: { empId: emp_id },
            success: function (response) {
                var option = ""
                $.each
                    (
                        response, function (index, item) {

                            $('#hdn_Employee_Id').val(item.LocalEmplyee_ID);
                            if (item.Company_Name != null) {
                                $('#txt_From_Company').val(item.Company_Name + " ( " + item.Company_Code + " )");
                            }
                            else {
                                $('#txt_From_Company').val('');
                            }
                            $('#hdn_From_Company_Id').val(item.Company_Id);
                            if (item.Business_Line_Name != null) {
                                $('#txt_From_BusinessLine').val(item.Business_Line_Name + " ( " + item.BusinessLine_Code + " )");
                            }
                            else {
                                $('#txt_From_BusinessLine').val('');
                            }

                            $('#hdn_From_BusinessLine_Id').val(item.Businessline_Id);

                            $('#txt_From_ProductGroup').val(item.PG_Name);
                            $('#hdn_From_ProductGroup_Id').val(item.Productgroup_Id);
                            if (item.Department_Name != null) {
                                $('#txt_From_Department').val(item.Department_Name + " ( " + item.Department_Code + " )");
                            }
                            else {
                                $('#txt_From_Department').val('');
                            }

                            $('#hdn_From_Department_Id').val(item.Department_Id);

                            $('#txt_From_Position').val(item.Position);
                            $('#hdn_From_Position_Id').val(item.PositionClass_ID);

                            $('#txt_From_Global_Grade').val(item.Global_Group);
                            $('#hdn_From_Global_Grade_Id').val(item.Global_Group);

                            $('#txt_From_Local_Grade').val(item.Local_Group);
                            $('#hdn_From_Local_Grade_Id').val(item.Local_Group);

                            if (item.CC_Name != null) {
                                $('#txt_From_Cost_Center').val(item.CC_Name + " ( " + item.CC_Code + " )");
                            }
                            else {
                                $('#txt_From_Cost_Center').val('');
                            }

                            $('#hdn_From_Cost_Center_Id').val(item.CC_Id);
                            if (item.Location != null) {
                                $('#txt_From_Location').val(item.Location + " ( " + item.Location_Code + " )");
                            }
                            else {
                                $('#txt_From_Location').val('');
                            }

                            $('#hdn_From_Location_Id').val(item.Location_Id);

                            $('#txt_ReleasingManager').val(response.Status);

                            $('#hdn_ReleasingManager_Id').val(item.Line_Manager);
                            $('#hdn_Business_Id').val(item.Business_Id);

                            FillBusinessLine(item.Business_Id);

                            if (item.Line_Manager != null) {

                                FillReceivingManager(item.Line_Manager);

                                if (wf_type == "P017") {
                                    FillLineManager(item.Line_Manager);
                                }
                            }

                            return false;
                        });
            }
        });
    }
}

//P016-Internal Transfer(Preema)
function FillBusinessLine(bus_id) {
    if (bus_id != '') {
        $.ajax({
            url: '/Request/LoadBusinessLineByBusiness?id=' + bus_id,
            type: "GET",
            success: function (result) {

                if (result.list.length > 0) {
                    $("#dp_To_BusinessLine").html("");
                    $("#dp_To_BusinessLine").append($('<option></option>').val("").html('-- No Change --'));
                    $.each(result.list, function (i, item) {
                        $("#dp_To_BusinessLine").append($('<option></option>').val(item.Value).html(item.Text));
                    });

                }
                else {
                    $("#dp_To_BusinessLine").html("");
                    $("#dp_To_BusinessLine").append($('<option></option>').val("").html('-- No Change --'));

                }
            }
        });
    }
}
//P016-Internal Transfer(Preema)
function FillReceivingManager(ReleasingManager_Id) {

    if (ReleasingManager_Id != '') {
        $.ajax({
            url: '/Request/LoadReceivingManagers?id=' + ReleasingManager_Id,
            type: "GET",
            success: function (result) {

                if (result.list.length > 0) {
                    $("#dp_ReceivingManager").html("");
                    $("#dp_ReceivingManager").append($('<option></option>').val("").html('-- Choose --'));
                    $.each(result.list, function (i, item) {
                        $("#dp_ReceivingManager").append($('<option></option>').val(item.LocalEmplyee_ID).html(item.LocalEmplyee_ID + " " + item.Emp_Name));
                    });

                }
                else {
                    $("#dp_ReceivingManager").html("");
                    $("#dp_ReceivingManager").append($('<option></option>').val("").html('-- Choose --'));
                }


            }
        });
    }
}
//P017-Contract Modification(Preema)
function FillLineManager(line_manager) {

    $.ajax({
        url: '/Request/LoadLineManagers',
        type: "GET",
        success: function (result) {

            if (result.list.length > 0) {
                $("#dp_ReleasingManager").html("");
                $("#dp_ReleasingManager").append($('<option></option>').val("").html('-- Choose --'));
                $.each(result.list, function (i, item) {

                    if (line_manager == item.LocalEmplyee_ID) {

                        $("#dp_ReleasingManager").append($('<option selected ></option>').val(item.LocalEmplyee_ID).html(item.LocalEmplyee_ID + " " + item.Emp_Name));
                    }
                    else {
                        $("#dp_ReleasingManager").append($('<option></option>').val(item.LocalEmplyee_ID).html(item.LocalEmplyee_ID + " " + item.Emp_Name));
                    }
                });

            }
            else {
                $("#dp_ReleasingManager").html("");
                $("#dp_ReleasingManager").append($('<option></option>').val("").html('-- Choose --'));

            }
        }
    });
}




