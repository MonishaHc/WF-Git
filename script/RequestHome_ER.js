var public_PrintApprove = false;

// For design - 13-11-2019
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
                    EmployeeCuntryDetails(empId);
                }
                else if (currentId == 'P012') {    //19-03-2020 Nimmi
                    EmployeeCuntryDetails(empId);
                }
                else if (currentId == 'P016' || currentId == 'P017') {      //P016-Internal Transfer(Preema)

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

    ///.............................................
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
        if (obj.wf_id == 'P045') {
            SaveExpenseReport(obj, "create");
           

        
        }
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

//Basheer on 23-01-2020 to edit the details
$(document).on('click', '#edit_request_btn', function () {
    EditRequest();
});
function EditRequest() {
    var obj = new Object();
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    //------------- P045 ER HR Related done by Chitra V srishti 08.07.2020-------------------------
 if (obj.wf_id == 'P045') {
        SaveExpenseReport(obj, "edit");
    }

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
//Basheer on 27-02-2020 P034
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


$(document).on('change', '#rent_car_id', function () {

    var car_rent = $('#rent_car_id :selected').val();
    if (car_rent == 1) {
        $('.rent_car_details_section').show();
    }
    else {
        $('.rent_car_details_section').hide();
    }
});

$(document).on('change', '#hotel_booking_id', function () {

    var hotel_booking = $('#hotel_booking_id :selected').val();
    if (hotel_booking == 1) {
        $('.hotel_booking_details_section').show();
    }
    else {
        $('.hotel_booking_details_section').hide();
    }
});

$(document).on('change', '#amex_holder_id', function () {

    var amex_holder = $('#amex_holder_id :selected').val();
    if (amex_holder == 0) {
        $('.cash_advance_details_section').show();
    }
    else {
        $('.cash_advance_details_section').hide();
    }
});

$(document).on('change', '#cash_advance_id', function () {

    var cashadavnce = $('#cash_advance_id :selected').val();
    if (cashadavnce == 1) {
        $('.cash_advance_details_advance_section').show();
    }
    else {
        $('.cash_advance_details_advance_section').hide();
    }
});

//function for Save and Edit For P045 Module done by : // Chitra Srishti on 08.07.2020//strt 
function SaveExpenseReport(obj, action) {
    var ExpenseReportObj = new Object();
    var validAllownace = true;
    var valid = ValidateExpenseReport(obj.wf_id);
    if (obj.wf_id == "P008") { validAllownace = ValidateAllowanceDetails(); }
    var validBlcontr = ValidateBLConDetails();
    if ((!valid) || (!validAllownace) || (!validBlcontr)) {
        toastrError("Please fill the mandatory fields");
        return;
    }

    //continue if validation is successful
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
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    ExpenseReportObj.application_id = $('#application-list-drop-id').val();
    ExpenseReportObj.emp_local_id = $('.employee-list-drop-id :selected').val();
    ExpenseReportObj.creator_id = $('#emp_identify_id').val();
    ExpenseReportObj.request_id = requestId;

    var dp_RequestType = document.getElementById("reqType");
    ExpenseReportObj.RequestId = dp_RequestType.options[dp_RequestType.selectedIndex].value;
    ExpenseReportObj.RequestType = dp_RequestType.options[dp_RequestType.selectedIndex].text;

    ExpenseReportObj.ChargeTo = $('#chrg_To').val();
    ExpenseReportObj.PlacesVisited = $('#place_Visit').val();
    ExpenseReportObj.Region = $('#drp_Region').val();
    ExpenseReportObj.PersonnelVisited = $('#person_Company').val();
    ExpenseReportObj.BusinessLine = $('#blLine').val();
    ExpenseReportObj.TARequest = $('#ta_Req').val();
    ExpenseReportObj.TARequestDate = $('#taDate').val();
    ExpenseReportObj.LastDayOfWork = $('#last_Day').val();
    ExpenseReportObj.ReturnToDuty = $('#ret_Duty').val();
    ExpenseReportObj.ComplianceApproved = $('#isComplain_Approv').val();
    ExpenseReportObj.ComplianceApprovalDate = $('#cmp_ApprovDate').val();

    var totalAmount = 0;
    //Construct daily allowance object
    ExpenseReportObj.ExpenseReportAllowanceList = [];
    var ExpenseReportAllwnceObj;
    var loopThruIdx = 1;
    var sequenceNum = 1;
    $('.allwncetable input').each(function (i, obj) {
        if (loopThruIdx % 8 == 1) {
            ExpenseReportAllwnceObj = new Object();
            ExpenseReportAllwnceObj.Place = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 2) {
            ExpenseReportAllwnceObj.FromDate = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 3) {
            ExpenseReportAllwnceObj.FromHours = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 4) {
            ExpenseReportAllwnceObj.ToDate = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 5) {
            ExpenseReportAllwnceObj.ToHours = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 6) {
            ExpenseReportAllwnceObj.DaysHours = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 7) {
            ExpenseReportAllwnceObj.DailyAllowance = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 0) {
            loopThruIdx = 1;
            if (ExpenseReportAllwnceObj.Place != '' && ExpenseReportAllwnceObj.FromDate != '' && ExpenseReportAllwnceObj.FromHours != '' && ExpenseReportAllwnceObj.ToDate != '' && ExpenseReportAllwnceObj.ToHours != '' && ExpenseReportAllwnceObj.DaysHours != '' && ExpenseReportAllwnceObj.DailyAllowance != '') {
                ExpenseReportAllwnceObj.AmtLocal = ExpenseReportAllwnceObj.DaysHours * ExpenseReportAllwnceObj.DailyAllowance;
                totalAmount += ExpenseReportAllwnceObj.AmtLocal;
                ExpenseReportAllwnceObj.SequenceNum = sequenceNum++;
                ExpenseReportObj.ExpenseReportAllowanceList.push(ExpenseReportAllwnceObj);
            }
        }
    });

    //Construct other details object
    ExpenseReportObj.ExpenseReportDetailList = [];
    var ExpenseReportDetailObj;

    //Transportation - Air
    ExpenseReportDetailObj = new Object();
    ExpenseReportDetailObj.AccountTypeId = 1;
    ExpenseReportDetailObj.AccountTypeName = "Transportation - Air";
    ExpenseReportDetailObj.Description1 = $('#air_Tick').val();
    ExpenseReportDetailObj.Currency = $('#ddl_airTickCr').val();
    ExpenseReportDetailObj.Amount = $('#air_Amount').val();
    ExpenseReportDetailObj.ExchangeRate = $('#air_ExeRate').val();
    if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
        ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
        totalAmount += ExpenseReportDetailObj.AmtLocal;
        ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
    }

    //Transportation - Taxi
    ExpenseReportDetailObj = new Object();
    ExpenseReportDetailObj.AccountTypeId = 2;
    ExpenseReportDetailObj.AccountTypeName = "Transportation - Taxi";
    ExpenseReportDetailObj.Description1 = $('#taxi').val();
    ExpenseReportDetailObj.Currency = $('#ddl_taxi_CurrType').val();
    ExpenseReportDetailObj.Amount = $('#taxi_Amt').val();
    ExpenseReportDetailObj.ExchangeRate = $('#taxi_ExeRate').val();
    if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
        ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
        totalAmount += ExpenseReportDetailObj.AmtLocal;
        ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
    }

    //Transportation - Others
    ExpenseReportDetailObj = new Object();
    ExpenseReportDetailObj.AccountTypeId = 3;
    ExpenseReportDetailObj.AccountTypeName = "Transportation - Others";
    ExpenseReportDetailObj.Description1 = $('#others').val();
    ExpenseReportDetailObj.Currency = $('#ddl_OthCurrType').val();
    ExpenseReportDetailObj.Amount = $('#oth_Amt').val();
    ExpenseReportDetailObj.ExchangeRate = $('#oth_Exe_Rate').val();
    if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
        ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
        totalAmount += ExpenseReportDetailObj.AmtLocal;
        ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
    }

    if (obj.wf_id == 'P008') {
        //Mileage Allowance
        ExpenseReportDetailObj = new Object();
        ExpenseReportDetailObj.AccountTypeId = 4;
        ExpenseReportDetailObj.AccountTypeName = "Mileage Allowance";
        ExpenseReportDetailObj.KM = $('#km').val();
        ExpenseReportDetailObj.Amount = $('#id_Allowance').val();
        ExpenseReportDetailObj.Description1 = $('#descriptionAllow').val();
        ExpenseReportDetailObj.Currency = $('#ddl_allow_cur').val();
        ExpenseReportDetailObj.ExchangeRate = $('#allowExe').val();
        if (ExpenseReportDetailObj.KM != '' && ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
            ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.KM * ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
            totalAmount += ExpenseReportDetailObj.AmtLocal;
            ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
        }

        //Entertainment
        ExpenseReportDetailObj = new Object();
        ExpenseReportDetailObj.AccountTypeId = 5;
        ExpenseReportDetailObj.AccountTypeName = "Entertainment";
        ExpenseReportDetailObj.Amount = $('#amtent').val();
        ExpenseReportDetailObj.Description1 = $('#specifyper').val();
        ExpenseReportDetailObj.Currency = $('#ddlentcur').val();
        ExpenseReportDetailObj.ExchangeRate = $('#exeent').val();
        if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
            ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
            totalAmount += ExpenseReportDetailObj.AmtLocal;
            ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
        }

        //Hotel Accomodation
        ExpenseReportDetailObj = new Object();
        ExpenseReportDetailObj.AccountTypeId = 6;
        ExpenseReportDetailObj.AccountTypeName = "Hotel Accomodation";
        ExpenseReportDetailObj.Amount = $('#HotAmnt').val();
        ExpenseReportDetailObj.Description1 = $('#hottext').val();
        ExpenseReportDetailObj.Currency = $('#ddl_hot_cur').val();
        ExpenseReportDetailObj.ExchangeRate = $('#HotExe').val();
        if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
            ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
            totalAmount += ExpenseReportDetailObj.AmtLocal;
            ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
        }

        //External hire of vehicles
        ExpenseReportDetailObj = new Object();
        ExpenseReportDetailObj.AccountTypeId = 7;
        ExpenseReportDetailObj.AccountTypeName = "External hire of vehicles";
        ExpenseReportDetailObj.Amount = $('#vehAmnt').val();
        ExpenseReportDetailObj.Description1 = $('#veh_text').val();
        ExpenseReportDetailObj.Currency = $('#ddl_veh_cur').val();
        ExpenseReportDetailObj.ExchangeRate = $('#vehexe').val();
        if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
            ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
            totalAmount += ExpenseReportDetailObj.AmtLocal;
            ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
        }

        //Telephone
        ExpenseReportDetailObj = new Object();
        ExpenseReportDetailObj.AccountTypeId = 8;
        ExpenseReportDetailObj.AccountTypeName = "Telephone";
        ExpenseReportDetailObj.Amount = $('#telamount').val();
        ExpenseReportDetailObj.Description1 = $('#telText').val();
        ExpenseReportDetailObj.Currency = $('#ddl_tel').val();
        ExpenseReportDetailObj.ExchangeRate = $('#telexe').val();
        if (ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
            ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
            totalAmount += ExpenseReportDetailObj.AmtLocal;
            ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
        }
    }

    var loopThruIdx = 1;
    var sequenceNum = 1;
    $('.detailstable .blconinput').each(function (i, obj) {
        if (loopThruIdx % 6 == 1) {
            ExpenseReportDetailObj = new Object();
            ExpenseReportDetailObj.AccountTypeId = 9;
            ExpenseReportDetailObj.AccountTypeName = "BL Controllers";
            ExpenseReportDetailObj.ERAccount = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 2) {
            ExpenseReportDetailObj.Description1 = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 3) {
            ExpenseReportDetailObj.Currency = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 4) {
            ExpenseReportDetailObj.Amount = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 5) {
            ExpenseReportDetailObj.ExchangeRate = this.value;
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 0) {
            loopThruIdx = 1;
            if (ExpenseReportDetailObj.ERAccount != '' && ExpenseReportDetailObj.Description1 != '' && ExpenseReportDetailObj.Amount != '' && ExpenseReportDetailObj.ExchangeRate != '') {
                ExpenseReportDetailObj.AmtLocal = ExpenseReportDetailObj.Amount * ExpenseReportDetailObj.ExchangeRate;
                totalAmount += ExpenseReportDetailObj.AmtLocal;
                ExpenseReportDetailObj.SequenceNum = sequenceNum++;
                ExpenseReportObj.ExpenseReportDetailList.push(ExpenseReportDetailObj);
            }
        }
    });

    ExpenseReportObj.AmtTotal = totalAmount;
    ExpenseReportObj.LessAdvance = $('#amtLessAdv').val() == '' ? "0" : $('#amtLessAdv').val();
    ExpenseReportObj.TicketsPaidByCo = $('#ticketByCo').val() == '' ? "0" : $('#ticketByCo').val();
    ExpenseReportObj.NetToReceive = totalAmount - (parseFloat(ExpenseReportObj.LessAdvance) + parseFloat(ExpenseReportObj.TicketsPaidByCo));

    obj.ExpenseReportModel = ExpenseReportObj;

    var url = "";
    if (obj.wf_id == 'P008') {
        if (action == "create") {
            url = "/Request/Submit_ER_Non_Project_Related";
        } else if (action == "edit") {
            url = "/Request/Submit_ER_Non_Project_Related_Edit_After_Save";
        }
    } else if (obj.wf_id == 'P045') {
        if (action == "create") {
            var url = "/Request/Submit_ER_HR_Related";
        } else if (action == "edit") {
            url = "/Request/Submit_ER_HR_Related_Edit_After_Save";
        }
    }

    //save changes DB
    $(".se-pre-con").show();
    $.ajax({
        type: "POST",
        url: url,
        dataType: "json",
        global: false,
        data: obj,
        success: function (response) {
            if (response.Status) {
                if (action == "create") {
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

                } else if (action == "edit") {
                    $(".se-pre-con").hide();
                    toastrSuccess('Changes Saved Succesfully');
                    $('#edit_request_btn').css('display', 'block');
                    $('#forward_request_btn').css('display', 'block');
                    $('#submit_request_btn').css('display', 'block');
                    $('#submit_request_btn').prop('disabled', false);
                    $('#cancel_request_btn').css('display', 'block');
                    $('#cancel_request_btn').prop('disabled', false);
                }
            }
            else {
                if (action == "create") {
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

                } else if (action == "edit") {
                    $(".se-pre-con").hide();
                }
            }
        },
    });
}
function ValidateExpenseReport(expenseType) {
    var valid = true; //logic to be updated
    if ($('#blLine').val() == "") {
        $('#blLine_required').css('display', 'block');
        valid = false;
    }
    else {
        $('#blLine_required').css('display', 'none');
    }
    if ($('#chrg_To').val() == "") {
        $('#chrg_To_required').css('display', 'block');
        valid = false;
    }
    else {
        $('#chrg_To_required').css('display', 'none');
    }
    if ($('#place_Visit').val() == "") {
        $('#place_Visit_required').css('display', 'block');
        valid = false;
    }
    else {
        $('#place_Visit_required').css('display', 'none');

    }
    if ($('#person_Company').val() == "") {
        $('#person_Company_required').css('display', 'block');
        valid = false;
    }
    else {
        $('#person_Company_required').css('display', 'none');
    }
    if ($('#ta_Req').val() != "") {//if TA selected
        if ($('#taDate').val() == "") {
            $('#taDate_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#taDate_required').css('display', 'none');
        }
        if ($('#last_Day').val() == "") {
            $('#last_Day_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#last_Day_required').css('display', 'none');
        }
        if ($('#cmp_ApprovDate').val() == "") {
            $('#cmp_ApprovDate_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#cmp_ApprovDate_required').css('display', 'none');
        }
    }
    else {
        $('#cmp_ApprovDate_required').css('display', 'none');
        $('#last_Day_required').css('display', 'none');
        $('#taDate_required').css('display', 'none');

    }
    //chcek for second part div 
    if (($('#air_Tick').val() != "") || ($('#air_Amount').val() != "") || ($('#air_ExeRate').val() != "")) {//selected any value
        if ($('#air_Tick').val() == "") {
            $('#air_Tick_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#air_Tick_required').css('display', 'none');
        }

        //if ($('#ddl_airTickCr').val() == "") {
        //    $('#ddl_airTickCr_required').css('display', 'block');
        //    valid = false;
        //}
        //else {
        //    $('#ddl_airTickCr_required').css('display', 'none');
        //}
        if ($('#air_Amount').val() == "") {
            $('#air_Amountr_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#air_Amountr_required').css('display', 'none');
        }
        if ($('#air_ExeRate').val() == "") {
            $('#air_ExeRate_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#air_ExeRate_required').css('display', 'none');
        }
    }
    else {
        $('#air_ExeRate_required').css('display', 'none');
        $('#air_Amountr_required').css('display', 'none');
        $('#air_Tick_required').css('display', 'none');
    }
    if (($('#taxi').val() != "")  || ($('#taxi_Amt').val() != "") || ($('#taxi_ExeRate').val() != "")) {//Taxi
        if ($('#taxi').val() == "") {
            $('#taxi_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#taxi_required').css('display', 'none');
        }
        //if ($('#ddl_taxi_CurrType').val() == "") {
        //    $('#ddl_taxi_CurrType_required').css('display', 'block');
        //    valid = false;
        //}
        //else {
        //    $('#ddl_taxi_CurrType_required').css('display', 'none');
        //}
        if ($('#taxi_Amt').val() == "") {
            $('#taxi_Amt_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#taxi_Amt_required').css('display', 'none');
        }
        if ($('#taxi_ExeRate').val() == "") {
            $('#taxi_ExeRate_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#taxi_ExeRate_required').css('display', 'none');
        }
    }
    else {
        $('#taxi_required').css('display', 'none');
        $('#taxi_ExeRate_required').css('display', 'none');
        $('#taxi_Amt_required').css('display', 'none');
    }
    if (($('#others').val() != "") || ($('#oth_Amt').val() != "") || ($('#oth_Exe_Rate').val() != "")) {//Others
        if ($('#others').val() == "") {
            $('#others_required	').css('display', 'block');
            valid = false;
        }
        else {
            $('#others_required	').css('display', 'none');
        }
        //if ($('#ddl_OthCurrType').val() == "") {
        //    $('#ddl_OthCurrType_required').css('display', 'block');
        //    valid = false;
        //}
        //else {
        //    $('#ddl_OthCurrType_required').css('display', 'none');
        //}
        if ($('#oth_Amt').val() == "") {
            $('#oth_Amt_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#oth_Amt_required').css('display', 'none');
        }
        if ($('#oth_Exe_Rate').val() == "") {
            $('#oth_Exe_Rate_required').css('display', 'block');
            valid = false;
        }
        else {
            $('#oth_Exe_Rate_required').css('display', 'none');
        }
    }
    else {
        $('#oth_Exe_Rate_required').css('display', 'none');
        $('#oth_Amt_required').css('display', 'none');
        $('#others_required	').css('display', 'none');
    }
    //chcek for P008
    if (expenseType == "P008") {
        if (($('#km').val() != "") || ($('#id_Allowance').val() != "") || ($('#allowExe').val() != "") || ($('#descriptionAllow').val() != "")) {//millegeallownace
            if ($('#km').val() == "") {
                $('#km_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#km_required').css('display', 'none');
            }
            if ($('#id_Allowance').val() == "") {
                $('#id_Allowance_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#id_Allowance_required').css('display', 'none');
            }
            //if ($('#ddl_allow_cur').val() == "") {
            //    $('#ddl_allow_cur_required').css('display', 'block');
            //    valid = false;
            //}
            //else {
            //    $('#ddl_allow_cur_required').css('display', 'none');
            //}
            if ($('#allowExe').val() == "") {
                $('#allowExe_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#allowExe_required').css('display', 'none');
            }
            if ($('#descriptionAllow').val() == "") {
                $('#descriptionAllow_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#descriptionAllow_required').css('display', 'none');
            }
        }
        else {
            $('#descriptionAllow_required').css('display', 'none');
            $('#allowExe_required').css('display', 'none');
            $('#id_Allowance_required').css('display', 'none');
            $('#km_required').css('display', 'none');
        }
        if (($('#specifyper').val() != "")  || ($('#amtent').val() != "") || ($('#exeent').val() != "")) {//Entertainment
            if ($('#specifyper').val() == "") {
                $('#specifyper_required	').css('display', 'block');
                valid = false;
            }
            else {
                $('#specifyper_required	').css('display', 'none');
            }
            //if ($('#ddlentcur').val() == "") {
            //    $('#ddlentcur_required').css('display', 'block');
            //    valid = false;
            //}
            //else {
            //    $('#ddlentcur_required').css('display', 'none');
            //}
            if ($('#amtent').val() == "") {
                $('#amtentr_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#amtentr_required').css('display', 'none');
            }
            if ($('#exeent').val() == "") {
                $('#exeent_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#exeent_required').css('display', 'none');
            }
        }
        else {
            $('#exeent_required').css('display', 'none');
            $('#amtentr_required').css('display', 'none');
            $('#specifyper_required	').css('display', 'none');
        }

        if (($('#hottext').val() != "") || ($('#HotAmnt').val() != "") || ($('#HotExe').val() != "")) {//Hotel
            if ($('#hottext').val() == "") {
                $('#hottext_required	').css('display', 'block');
                valid = false;
            }
            else {
                $('#hottext_required').css('display', 'none');
            }
            //if ($('#ddl_hot_cur').val() == "") {
            //    $('#ddl_hot_cur_required').css('display', 'block');
            //    valid = false;
            //}
            //else {
            //    $('#ddl_hot_cur_required').css('display', 'none');
            //}
            if ($('#HotAmnt').val() == "") {
                $('#HotAmnt_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#HotAmnt_required').css('display', 'none');
            }
            if ($('#HotExe').val() == "") {
                $('#HotExe_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#HotExe_required').css('display', 'none');
            }
        }
        else {
            $('#HotExe_required').css('display', 'none');
            $('#HotAmnt_required').css('display', 'none');
            $('#hottext_required').css('display', 'none');

        }

        if (($('#veh_text').val() != "")  || ($('#vehAmnt').val() != "") || ($('#vehexe').val() != "")) {//vehicles
            if ($('#veh_text').val() == "") {
                $('#veh_text_required	').css('display', 'block');
                valid = false;
            }
            else {
                $('#veh_text_required').css('display', 'none');
            }
            //if ($('#ddl_veh_cur').val() == "") {
            //    $('#ddl_veh_curt_required').css('display', 'block');
            //    valid = false;
            //}
            //else {
            //    $('#ddl_veh_curt_required').css('display', 'none');
            //}
            if ($('#vehAmnt').val() == "") {
                $('#vehAmnt_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#vehAmnt_required').css('display', 'none');
            }
            if ($('#vehexe').val() == "") {
                $('#vehexe_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#vehexe_required').css('display', 'none');
            }
        }
        else {
            $('#vehexe_required').css('display', 'none');
            $('#vehAmnt_required').css('display', 'none');
            $('#veh_text_required').css('display', 'none');

        }

        if (($('#telText').val() != "")  || ($('#telamount').val() != "") || ($('#telexe').val() != "")) {//Telphone
            if ($('#telText').val() == "") {
                $('#telText_required	').css('display', 'block');
                valid = false;
            }
            else {
                $('#telText_required').css('display', 'none');
            }
            //if ($('#ddl_tel').val() == "") {
            //    $('#ddl_tel_required').css('display', 'block');
            //    valid = false;
            //}
            //else {
            //    $('#ddl_tel_required').css('display', 'none');
            //}
            if ($('#telamount').val() == "") {
                $('#telamount_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#telamount_required').css('display', 'none');
            }
            if ($('#telexe').val() == "") {
                $('#telexe_required').css('display', 'block');
                valid = false;
            }
            else {
                $('#telexe_required').css('display', 'none');
            }
        }
        else{
            $('#telexe_required').css('display', 'none');
            $('#telamount_required').css('display', 'none');
            $('#telText_required').css('display', 'none');
        }
    }

    return valid;
}

//P008 Daily wllowance check(Shiraj/Afsal 17/06/2020)
function ValidateAllowanceDetails() {  //this will return an array of validation span ids (eg. "dlyalwnce_place_req0", "dlyalwnce_fromhours_req2" etc.)
   
    var loopThruIdx = 1;
    var currentRow = 0;
    var valuePresent = false;
    var invalidIds = [];
    var invalidIdsOfThisRow = [];
    $('.allwncetable input').each(function (i, obj) {
        if (loopThruIdx % 8 == 1) {
            var id = this.id;
            currentRow = id[id.length - 1];
            valuePresent = false;
            invalidIdsOfThisRow = [];

            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_place_req_' + currentRow + '').css('display', 'none');
            } else {
                invalidIdsOfThisRow.push('dlyalwnce_place_req_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 2) {
            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_fromdate_req_' + currentRow + '').css('display', 'none');
            } else {
                invalidIdsOfThisRow.push('dlyalwnce_fromdate_req_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 3) {
            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_fromhours_req_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('dlyalwnce_fromhours_req_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 4) {
            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_todate_req_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('dlyalwnce_todate_req_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 5) {
            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_tohours_req_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('dlyalwnce_tohours_req_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 6) {
            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_dayshours_req_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('dlyalwnce_dayshours_req_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 7) {
            if (this.value != '') {
                valuePresent = true;
                $('#dlyalwnce_dlyalwnce_req_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('dlyalwnce_dlyalwnce_req_' + currentRow);
            }
            if (valuePresent && invalidIdsOfThisRow.length > 0) {
                invalidIds = invalidIds.concat(invalidIdsOfThisRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 8 == 0) {
            //validation not required for this field
            loopThruIdx = 1;
        }
    });
    var IsvalidAllownace = true;
    for (i = 0; i < invalidIds.length; i++) {
        IsvalidAllownace = false;
        $('#' + invalidIds[i] + '').css('display', 'block');
    }
    return IsvalidAllownace;
}
//P008 BlControleers(Shiraj/Afsal 17/06/2020)
function ValidateBLConDetails() {
    var loopThruIdx = 1;
    var currentRow = 0;
    var valuePresent = false;
    var invalidIds = [];
    var invalidIdsOfThisRow = [];
    $('.detailstable .blconinput').each(function (i, obj) {
        if (loopThruIdx % 6 == 1) {
            var id = this.id;
            currentRow = id[id.length - 1];
            valuePresent = false;
            invalidIdsOfThisRow = [];

            if ((this.value != '') && (this.value != "--Choose--")) {
                valuePresent = true;
                $('#BlCtr_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('BlCtr_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 2) {
            if (this.value != '') {
                valuePresent = true;
                $('#Bltxt_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('Bltxt_' + currentRow);
            }
            loopThruIdx++;
        }
        else if (loopThruIdx % 6 == 3) {
            //if (this.value != '') {
            //    valuePresent = true;
            //    $('#Blcur_' + currentRow + '').css('display', 'none');

            //} else {
            //    invalidIdsOfThisRow.push('Blcur_' + currentRow);
            //}
            loopThruIdx++;
        }
        else if (loopThruIdx % 6 == 4) {
            if (this.value != '') {
                valuePresent = true;
                $('#BlAmt_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('BlAmt_' + currentRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 5) {
            if (this.value != '') {
                valuePresent = true;
                $('#BlEx_' + currentRow + '').css('display', 'none');

            } else {
                invalidIdsOfThisRow.push('BlEx_' + currentRow);
            }
            if (valuePresent && invalidIdsOfThisRow.length > 0) {
                invalidIds = invalidIds.concat(invalidIdsOfThisRow);
            }
            loopThruIdx++;
        } else if (loopThruIdx % 6 == 0) {
            //validation not required for this field
            loopThruIdx = 1;
        }
    });
    var IsValidBL = true;
    for (i = 0; i < invalidIds.length; i++) {
        IsValidBL = false;
        $('#' + invalidIds[i] + '').css('display', 'block');
    }
    return IsValidBL;

}

//function for Save and Edit For P003 Module done by : // Chitra Srishti on 08.07.2020//end 
$(document).on('change', '.wf_createtype', function () {
    var wf_createtype = $('.wf_createtype :selected').val();
    if (wf_createtype == 1) {
        $('.mod_requestid').css('display', 'block');
        $('.id_label').css('display', 'block');


    }
    else {
        $('.mod_requestid').css('display', 'none');
        $('.id_label').css('display', 'none');
    }

});
$(document).on('change', '.mod_requestid', function () {

    $(".se-pre-con").show();
    var id = $(this).val();
    $.ajax({
        url: '/Request/EditBusinessInternational/' + id,
        type: "GET",
        dataType: 'html',
        cache: false,
        success: function (data) {
            $('#business_internationalmain').html('');
            $('#business_internationalmain').html(data);
            $(".se-pre-con").hide();
        }
    });
});



//-------------------------.End........................................


