﻿

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
        if (obj.wf_id == 'A008') {
            Save_A008();
        }
        else if (obj.wf_id == 'A009') {
            Save_A009();
        }
       
    }
});

// Request Submit ,while create the approver data********************SUBMIT BUTTON CLICK HERE *********************************
$(document).on('click', '#submit_request_btn', function () {
    $(".se-pre-con").show();
    var x = false;
    $('#submit_request_btn').prop('disabled', true);
    $('#forward_request_btn').css('display', 'none');
    $('#cancel_request_btn').prop('disabled', true);

    //var emp_localId = $('.employee-list-drop-id :selected').val(); //Basheer on 25-03-2020
    var emp_localId = $('#emp_identify_id').val();

    var wf_id = $('.wf-types-list-drop-id :selected').val();

    if (requestId == "" || requestId == undefined || requestId == "0") {
        if (wf_id == 'P007' || wf_id == 'P037') {
            requestId = $('#hdn_request_id').val();
        }
    }
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
                $(".se-pre-con").hide();
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
                $(".se-pre-con").hide();
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
    if (obj.wf_id == 'A008') {
        Edit_A008();
    }
    else if (obj.wf_id == 'A009') {
        Edit_A009();
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
    if (requestId == "" || requestId == undefined || requestId == "0") {
        var wf_id = $('.wf-types-list-drop-id :selected').val();
        if (wf_id == 'P007' || wf_id == 'P037') {
            requestId = $('#hdn_request_id').val();
        }
    }
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
// 03/07/2020 ALENA SICS
function Save_A008() {
    var validation = true;
    var dependevalidation = true;
    var application_name = $('#application_code_id').val();
    var obj = new Object();

    obj._FileList = [];
    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

    obj.cost_center = CostCenter_value;
    // obj.cost_center = $('.Cost_Center').val();
    obj.pickup_at = $('.Drop_At').val();
    obj.employee_name = $('.Employee_Name').val();
    obj.date = $('.Drop_Date').val();
    obj.time = $('.Drop_Time').val();
    obj.attachment_filepath = afterSaveCommonFilePath;
    obj.remarks = $('.Remarks').val();
    //// for administration 
    //obj.quatity = $('.Quantity').val();
    //obj.Mobile_No = $('.Mobile_No').val();
    //obj.Employee_id = $('.Emp_Id').val();
    //obj.drivername = $('.GetDriver-list-drop-class : selected').val();
    //obj.carmodel = $('.Car_Model').val();
    ////end-------

    if (obj.cost_center == "") {
        x = true;
        $('#Cost_Center_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        $('#Cost_Center_required').css('display', 'none');
        x = false;
    }
    if (obj.employee_name == "") {
        x = true;
        $('#Employee_Name_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        $('#Employee_Name_required').css('display', 'none');
        x = false;
    }
    if (obj.pickup_at == "") {
        x = true;
        $('#Drop_At_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {

        $('#Drop_At_required').css('display', 'none');
        x = false;
    }
    if (obj.date == "") {
        x = true;
        $('#Drop_Date_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {

        $('#Drop_Date_required').css('display', 'none');
        x = false;
    }
    if (obj.time == "") {
        x = true;
        $('#Drop_Time_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {

        $('#Drop_Time_required').css('display', 'none');
        x = false;
    }
    if (obj.cost_center == "" || obj.employee_name == "" || obj.pickup_at == "" || obj.d == "" || obj.da == "" || obj.time == "") {
        x = true;
    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_AO_EmployeePickUp",
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
        toastrError("Please fill all the mandatory fields"); //Terrin on 14/5/2020

    }


} 
// 03/07/2020 ALENA SICS
function Edit_A008() {

    var validation = true;
    var dependevalidation = true;
    var application_name = $('#application_code_id').val();
    var obj = new Object();

    obj._FileList = [];

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

    obj.cost_center = CostCenter_value;
    // obj.cost_center = $('.cost_center-drop-down').val();
    obj.employee_name = $('.Employee_Name').val();
    obj.pickup_at = $('.Drop_At').val();
    obj.date = $('.Drop_Date').val();
    obj.time = $('.Drop_Time').val();
    obj.remarks = $('.Remarks').val();
    obj.attachment_filepath = afterSaveCommonFilePath;
    // for administration only (save)
    //var ddl_driver = document.getElementById("GetDriver-list-drop-class");
    //var driver_value = ddl_driver.options[ddl_driver.selectedIndex].value;
    //var driver_text = ddl_driver.options[ddl_driver.selectedIndex].text;
    //var driver_mobile = ddl_d.options[ddl_d.selectedIndex].Mobile_No;

    //obj.drivername = driver_value;
    obj.quantity = $('.Quantity').val();
    //  obj.drivername = $('.GetDriver-list-drop-class : selected').val();
    obj.drivername = $('.GetDriver-list-drop-class').val();
    //obj.mobile_number = $('.Mobile_No').val();
    obj.Mobile_No = $('.Mobile_No').val();
    //  obj.Mobile_No = $('.Mobile_No').val();
    //obj.emp_Id = $('.Emp_Id').val();
    obj.Employee_id = $('.Emp_Id').val();
    obj.carmodel = $('.Car_Model').val();
    // end-------------------
    if (obj.cost_center == "") {
        x = true;
        $('#Cost_Center_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Cost_Center_required').css('display', 'none');
    }
    if (obj.employee_name == "") {
        x = true;
        $('#Employee_Name_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Employee_Name_required').css('display', 'none');
    }
    if (obj.pickup_at == "") {
        x = true;
        $('#Drop_At_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Drop_At_required').css('display', 'none');

    }
    if (obj.time == "") {
        x = true;
        $('#Drop_Time_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Drop_Time_required').css('display', 'none');

    }
    if (obj.date == "") {
        x = true;
        $('#Drop_Date_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Drop_Date_required').css('display', 'none');

    }
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            //url: "/Request/Submit_AO_EmployeePickUp_Edit_After_Save",
            url: "/Request/Edit_AO_EmployeePickUp",
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
//A009-Arrangement of Employee Drop(Preema) and modified by ALENA
function Save_A009() {
    var validation = true;

    var application_name = $('#application_code_id').val();
    var obj = new Object();


    obj._FileList = [];


    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }

    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

    obj.cost_center = CostCenter_value;

    obj.employee_name = $('.Employee_Name').val();
    // 03/07/2020 ALENA SICS
   // obj.drop_at = $('.Drop_At').val();
    obj.drop_at = $('.Drop_At').val(); //END---
    obj.date = $('.Drop_Date').val();

    obj.time = $('.Drop_Time').val();

    obj.remarks = $('.Remarks').val();
    // 03/07/2020 ALENA SICS
    obj.attachment_filepath = afterSaveCommonFilePath;
    //END---


    //obj.quantity = $('.Quantity').val();

    //obj.driver_name = $('.Driver_Name').val();

    //obj.mobile_no = $('.Mobile_No').val();

    //obj.emp_id = $('.Emp_Id').val();

    //obj.car_model = $('.Car_Model').val();



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
    // 03/07/2020 ALENA SICS COMMENTED BELOW CODE AND ADDED NEW CODE

    //if ($('.Drop_At').val() == '') {
    //    $('#Drop_At_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    return;
    //}
    //else {
    //    x = false;
    //    $('#Drop_At_required').css('display', 'none');
    //}
    if ($('.DropAt').val() == '') {
        $('#Drop_At_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#Drop_At_required').css('display', 'none');
    }// END---
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
            url: "/Request/Submit_AO_EmployeeDrop",
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

//A009-Arrangement of Employee Drop(Preema) and commented by ALENA
function Edit_A009() {
    var validation = true;

    var application_name = $('#application_code_id').val();
    var obj = new Object();


    obj._FileList = [];

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }

    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    // 06/07/2020 ALENA SICS 
    obj.req_id_only = requestId;

    var ddl_CostCenter = document.getElementById("cost_center-drop-down");
    var CostCenter_value = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].value;
    var CostCenter_text = ddl_CostCenter.options[ddl_CostCenter.selectedIndex].text;

    obj.cost_center = CostCenter_value;
    obj.employee_name = $('.Employee_Name').val();
  obj.drop_at = $('.Drop_At').val();

    obj.date = $('.Drop_Date').val();
    
    obj.time = $('.Drop_Time').val();

    obj.remarks = $('.Remarks').val();
    // 03/07/2020 ALENA SICS 
    obj.attachment_filepath = afterSaveCommonFilePath;
    //END--
    // 02/07/2020 ALENA SICS  for administration only (save)
    //var ddl_driver = document.getElementById("GetDriver-list-drop-class");
    //var driver_value = ddl_driver.options[ddl_driver.selectedIndex].value;
    //var driver_text = ddl_driver.options[ddl_driver.selectedIndex].text;
    //var driver_mobile = ddl_d.options[ddl_d.selectedIndex].Mobile_No;

    obj.drivername = $('.GetDriver-list-drop-class').val();
    obj.quantity = $('.Quantity').val();
    obj.Mobile_No = $('.Mobile_No').val();
    obj.Employee_id = $('.Emp_Id').val();
    obj.carmodel = $('.Car_Model').val();
    // end-------------------
    // 02/07/2020 ALENA SICS ----------------------------------------------------------------------------------------------------------
    //if (CostCenter_text == '-- Choose --') {
    //    $('#Cost_Center_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    return; }
    //else {
    //    x = false;
    //    $('#Cost_Center_required').css('display', 'none'); }
    //if ($('.Employee_Name').val() == '') {
    //    $('#Employee_Name_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    return; }
    //else {
    //    x = false;
    //    $('#Employee_Name_required').css('display', 'none'); }
    //if ($('.Drop_At').val() == '') {
    //    $('#Drop_At_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    return; }
    //else {
    //    x = false;
    //    $('#Drop_At_required').css('display', 'none'); }

    //if ($('.Drop_Time').val() == '') {
    //    $('#Drop_Time_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    return; }
    //else {
    //    x = false;
    //    $('#Drop_Time_required').css('display', 'none'); }
    if (obj.cost_center == "") {
        x = true;
        $('#Cost_Center_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Cost_Center_required').css('display', 'none');
    }
    if (obj.employee_name == "") {
        x = true;
        $('#Employee_Name_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Employee_Name_required').css('display', 'none');
    }
    // 03/07/2020 ALENA SICS COMMENTED THIS AND ADDED NEW CODE

    //if (obj.drop_at == "") {
    //    x = true;
    //    $('#Drop_At_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //}
    //else {
    //    x = false;
    //    $('#Drop_At_required').css('display', 'none');
    //}
    if (obj.dropat == "") {
        x = true;
        $('#Drop_At_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Drop_At_required').css('display', 'none');
    }
    if (obj.time == "") {
        x = true;
        $('#Drop_Time_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Drop_Time_required').css('display', 'none');
    }
    if (obj.date == "") {
        x = true;
        $('#Drop_Date_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
    }
    else {
        x = false;
        $('#Drop_Date_required').css('display', 'none');
    }
    // END----------------------------------------------------------------------------------------------------------------------------------
    if (x == false) {

        $(".se-pre-con").show();

        $.ajax({
            type: "POST",
            // 02/07/2020 ALENA SICS FOR A009
            //url: "/Request/Submit_AO_EmployeeDrop_Edit_After_Save",
            url: "/Request/Edit_AO_EmployeeDrop", //end------------
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

//P007-Vacation(Preema)
function Save_P007() {

    var validation = true;
    var dependevalidation = true;
    //var TravelAgencyValidation = true;
    var application_name = $('#application_code_id').val();
    var obj = new Object();
    var Vacation_Obj = new Object();
    obj._FileList = [];

    obj.request_id = requestId;
    Vacation_Obj.request_id = requestId;

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    //var wf_createtype_vacation = $('.wf_createtype_vacation :selected').val();
    //if (wf_createtype_vacation == 1) {
    //    var requestId = $('.mod_requestid_vacation :selected').val();
    //    Vacation_Obj.request_id = requestId;
    //    obj.request_id = requestId;
    //}

    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();


    Vacation_Obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    Vacation_Obj.application_id = $('#application-list-drop-id').val();
    Vacation_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Vacation_Obj.creator_id = $('#emp_identify_id').val();

    Vacation_Obj.place_to_visit = $('#place_to_visit_id').val();
    if (Vacation_Obj.place_to_visit == "") {
        $('#place_to_visit_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#place_to_visit_required').css('display', 'none');
    }
    Vacation_Obj.reason = $('#reason_id').val();
    if (Vacation_Obj.reason == "") {
        $('#reason_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#reason_required').css('display', 'none');
    }
    Vacation_Obj.remark_one = $('#remark_one_id').val();
    if (Vacation_Obj.remark_one == "") {
        $('#remarks_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#remarks_required').css('display', 'none');
    }

    Vacation_Obj.is_complaince_approval_required = $('#compliance_approval_required_id :selected').val();
    if (Vacation_Obj.is_complaince_approval_required == 1) {
        Vacation_Obj.compliance_approval_date = $('#compliance_approval_date_id').val();
        if (Vacation_Obj.compliance_approval_date == "") {
            $('#compliance_approval_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#compliance_approval_date_required').css('display', 'none');
        }
    }

    Vacation_Obj.last_day_of_work = $('#last_day_of_work_id').val();
    if (Vacation_Obj.last_day_of_work == "") {
        $('#last_day_of_work_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#last_day_of_work_required').css('display', 'none');
    }

    Vacation_Obj.return_to_duty = $('#requrn_to_duty_id').val();
    if (Vacation_Obj.return_to_duty == "") {
        $('#requrn_to_duty_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#requrn_to_duty_required').css('display', 'none');
    }

    Vacation_Obj.workflow_delegated = $('#workflow_delegated_id :selected').val();
    if (Vacation_Obj.workflow_delegated == 0) {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
        if (Vacation_Obj.justification_no_delegation == "") {
            $('#workflow_delegation_justification_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#workflow_delegation_justification_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
    }


    Vacation_Obj.address_during_absence = $('#new_address_id').val();
    if (Vacation_Obj.address_during_absence == "") {
        $('#new_address_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#new_address_id_required').css('display', 'none');
    }
    Vacation_Obj.telephone = $('#telephone_number_id').val();
    if (Vacation_Obj.telephone == "") {
        $('#telephone_number_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#telephone_number_required').css('display', 'none');
    }

    Vacation_Obj.mode_of_travel = $('#mode_of_travel :selected').val();


    Vacation_Obj.Visa_Duration = $("input[name='visa_duration']:checked").val();
    Vacation_Obj.Visa_With = $("input[name='passport']:checked").val();


    if (Vacation_Obj.mode_of_travel == "Air") {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        if (Vacation_Obj.location_id == "") {
            $('#travel_location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_location_required').css('display', 'none');
        }

        if (Vacation_Obj.Visa_With == "None") {
            $('#passport_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#passport_required').css('display', 'none');
        }


    }
    else {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        $('#travel_location_required').css('display', 'none');
        $('#passport_required').css('display', 'none');
    }

    Vacation_Obj.type_of_exit_visa = $("input[name='exit_visa_type']:checked").val();
    Vacation_Obj.travel_visa_charged_to = $("input[name='visa_charged_to']:checked").val();


    Vacation_Obj.required_foreign_visa = $('#foreign_visa_id :selected').val();
    Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
    if (Vacation_Obj.required_foreign_visa == 1) {

        if (Vacation_Obj.foreign_visa_countries == "") {
            $('#countries_for_foreign_visa_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#countries_for_foreign_visa_required').css('display', 'none');
        }
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        if (Vacation_Obj.foreign_visa_quantity == "") {
            $('#foregion_visa_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#foregion_visa_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        $('#countries_for_foreign_visa_required').css('display', 'none');
        $('#foregion_visa_quantity_required').css('display', 'none');
    }



    Vacation_Obj.required_travel_insurance = $('#travel_insurance_id :selected').val();


    if (Vacation_Obj.required_travel_insurance == 1) {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        if (Vacation_Obj.travel_insurance_countries == "") {
            $('#travel_insurance_countries_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_countries_required').css('display', 'none');
        }
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        if (Vacation_Obj.travel_insurance_quantity == "") {
            $('#travel_insurance_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        $('#travel_insurance_countries_required').css('display', 'none');
        $('#travel_insurance_quantity_required').css('display', 'none');
    }

    Vacation_Obj.required_rent_car = $('#rent_car_id :selected').val();

    if (Vacation_Obj.required_rent_car == 1) {

        Vacation_Obj.rent_car_charged_to = $('#rent_car_charged_to_id').val();

        if (Vacation_Obj.rent_car_charged_to == "Project") {
            Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
            if (Vacation_Obj.rent_car_project_no == "") {
                $('#rent_car_project_no_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                validation = false;
            }
            else {
                $('#rent_car_project_no_required').css('display', 'none');
            }
        }
        else {
            Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
        }

        Vacation_Obj.car_type = $('#ddl_car_type_data :selected').val();
        if (Vacation_Obj.car_type == "") {
            $('#car_type_data_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_type_data_required').css('display', 'none');
        }

        Vacation_Obj.rent_car_picked_up_at = $('#car_pick_up_at_id').val();

        if (Vacation_Obj.rent_car_picked_up_at == "") {
            $('#car_pick_up_at_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_pick_up_at_required').css('display', 'none');
        }

        Vacation_Obj.rent_car_pick_up_date = $('#car_pick_up_date_id').val();
        if (Vacation_Obj.rent_car_pick_up_date == "") {
            $('#car_pick_up_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_pick_up_date_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_pick_up_time = $('#car_pick_up_time_id').val();
        if (Vacation_Obj.rent_car_pick_up_time == "") {
            $('#car_pick_up_time_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_pick_up_time_required').css('display', 'none');
        }

        Vacation_Obj.rent_car_return_date = $('#rent_car_return_date_id').val();
        if (Vacation_Obj.rent_car_return_date == "") {
            $('#rent_car_return_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#rent_car_return_date_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_return_time = $('#rent_car_return_time_id').val();
        if (Vacation_Obj.rent_car_return_time == "") {
            $('#rent_car_return_time_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#rent_car_return_time_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_payment_type = $('.car_payment_type').val();
        if (Vacation_Obj.rent_car_payment_type == "") {
            $('#car_payment_type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_payment_type_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_remark = $('#remark_two_id').val();
    }


    Vacation_Obj.required_hotel_booking = $('#hotel_booking_id :selected').val();
    if (Vacation_Obj.required_hotel_booking == 1) {
        Vacation_Obj.hotel_booking_charged_to = $('#hotel_booking_charged_to_id').val();
        if (Vacation_Obj.hotel_booking_charged_to == "Project") {
            Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
            if (Vacation_Obj.hptel_booking_project_no == "") {
                $('#hotel_booking_project_no_id_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                validation = false;
            }
            else {
                $('#hotel_booking_project_no_id_required').css('display', 'none');
            }
        }
        else {
            Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
        }


        Vacation_Obj.hotel_name = $('#hotel_name_id').val();
        if (Vacation_Obj.hotel_name == "") {
            $('#hotel_name_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_name_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_location = $('#hotel_location').val();
        if (Vacation_Obj.hotel_location == "") {
            $('#hotel_location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_location_required').css('display', 'none');
        }
        Vacation_Obj.type_of_room = $('#type_of_rooms_id').val();
        Vacation_Obj.room_preferences = $('#room_preferences_id').val();
        Vacation_Obj.number_of_rooms = $('#number_of_rooms_id').val();

        if (Vacation_Obj.number_of_rooms == "") {
            $('#number_of_rooms_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#number_of_rooms_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_booking_payment_mode = $('.hotel_payment_type').val();
        Vacation_Obj.hotel_booking_check_in_date = $('#room_check_in_date_id').val();

        if (Vacation_Obj.hotel_booking_payment_mode == "") {
            $('#hotel_payment_type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_payment_type_required').css('display', 'none');
        }

        if (Vacation_Obj.hotel_booking_check_in_date == "") {
            $('#room_check_in_date_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_in_date_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_check_in_time = $('#room_check_in_time_id').val();
        if (Vacation_Obj.hotel_check_in_time == "") {
            $('#room_check_in_time_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_in_time_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_booking_check_out_date = $('#room_check_out_date_id').val();
        if (Vacation_Obj.hotel_booking_check_out_date == "") {
            $('#room_check_out_date_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_out_date_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_check_out_time = $('#room_check_out_time_id').val();
        if (Vacation_Obj.hotel_check_out_time == "") {
            $('#room_check_out_time_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_out_time_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_booking_remark = $('#remark_three_id').val();
    }

    Vacation_Obj.departure_date = $('#departure_date_id').val();
    Vacation_Obj.departure_flight_number = $('#depature_flight_no_id').val();
    Vacation_Obj.return_date = $('#departure_return_date_id').val();
    Vacation_Obj.return_flight_number = $('#return_flight_number_id').val();
    Vacation_Obj.travel_routing = $('#travel_route_id').val();
    Vacation_Obj.type_of_ticket = $('#type_of_ticket_id').val();
    Vacation_Obj.note = $('#note_id').val();


    if (Vacation_Obj.departure_date == "") {
        $('#departure_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.return_date == "") {
        $('#departure_return_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_return_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.travel_routing == "") {
        $('#travel_route_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#travel_route_id_required').css('display', 'none');
    }
    if (Vacation_Obj.mode_of_travel == "Air") {
        if (Vacation_Obj.departure_flight_number == "") {
            $('#depature_flight_no_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#depature_flight_no_id_required').css('display', 'none');
        }
        if (Vacation_Obj.return_flight_number == "") {
            $('#return_flight_number_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#return_flight_number_id_required').css('display', 'none');
        }
    }

    Vacation_Obj.amx_holder = $('#amex_holder_id :selected').val();
    Vacation_Obj.cash_advance = $('#cash_advance_id :selected').val();
    Vacation_Obj.salary_advance = $('#salary_advance_id').val();
    Vacation_Obj.bank_account = $('#bank_account_id').val();
    Vacation_Obj.ticket_cost = $('#ticket_cost_id').val();
    Vacation_Obj.hotel_cost = $('#hotel_cost_id').val();
    Vacation_Obj.daily_allowance = $('#daily_allowance_id').val();
    Vacation_Obj.other_expenses = $('#other_expense_id').val();
    Vacation_Obj.travel_advance_remark = $('#remark_four_id').val();
    Vacation_Obj.travel_advance_total = $('#travel_advance_id').val();
    Vacation_Obj.iban = $('#iban_id').val();

    if (Vacation_Obj.cash_advance == 1) {
        if (Vacation_Obj.salary_advance == "") {
            $('#salary_advance_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#salary_advance_id_required').css('display', 'none');
        }
        if (Vacation_Obj.bank_account != "") {

            if (Vacation_Obj.iban == "") {
                $('#iban_id_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                validation = false;
            }
            else {
                $('#iban_id_required').css('display', 'none');
            }
        }
        if (Vacation_Obj.ticket_cost == "") {
            $('#ticket_cost_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#ticket_cost_id_required').css('display', 'none');
        }
        if (Vacation_Obj.hotel_cost == "") {
            $('#hotel_cost_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_cost_id_required').css('display', 'none');
        }
        if (Vacation_Obj.daily_allowance == "") {
            $('#daily_allowance_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#daily_allowance_id_required').css('display', 'none');
        }
        if (Vacation_Obj.other_expenses == "") {
            $('#other_expense_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#other_expense_id_required').css('display', 'none');
        }
    }

    //Number of Days Information
    Vacation_Obj.ticket_charged_to = $('#dp_Ticket_Charge :selected').val();
    Vacation_Obj.from_period = $('#txt_from_period').val();
    Vacation_Obj.to_period = $('#txt_to_period').val();
    Vacation_Obj.Vacation = $('#txt_Vacation').val();
    Vacation_Obj.Leave_WO_Pay = $('#txt_Leave').val();
    Vacation_Obj.Holidays = $('#txt_Holidays').val();
    Vacation_Obj.Friday = $('#txt_Friday').val();
    Vacation_Obj.Saturday = $('#txt_Saturday').val();
    Vacation_Obj.Total_No_of_Days = $('#txt_Total').val();

    if (Vacation_Obj.ticket_charged_to == 1) {

        if (Vacation_Obj.from_period == "") {
            $('#spn_From_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        if (Vacation_Obj.to_period == "") {
            $('#spn_To_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
    }

    ////Employee entitlement to be processed by SAS

    //Vacation_Obj.HR_Remarks_Visa = $('#div_Visa_Remarks').text().trim();
    //Vacation_Obj.HR_Remarks_Taxi = $('#div_Taxi_Remarks').text().trim();


    //if ($("input[name='Visa_Amount']:checked").val() == "Yes") {
    //    Vacation_Obj.Visa_Amount_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Visa_Amount_Claim = "False";
    //}

    //if ($("input[name='Taxi_Fare']:checked").val() == "Yes") {
    //    Vacation_Obj.Taxi_Fare_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Taxi_Fare_Claim = "False";
    //}

    //if (Vacation_Obj.Visa_Amount_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Visa_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //if (Vacation_Obj.Taxi_Fare_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Taxi_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //Dependents Information
    Vacation_Obj.Dependentsinfo = [];
    $(".dependents_details").each(function () {
        var data = new Object();
        var currentRow = $(this).closest("tr");
        data.name = currentRow.find("td:eq(0)").text();
        data.relation_ship = currentRow.find("td:eq(1)").text();
        data.age = currentRow.find("td:eq(2)").text();
        data.visa_type = currentRow.find("td:eq(3)").val();
        data.ta_type = currentRow.find("td:eq(4)").text();
        data.remarks = currentRow.find("td:eq(5)").text();
        Vacation_Obj.Dependentsinfo.push(data);
    });

    $(".dependents_name").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_name != '' && Vacation_Obj.dependents_name != null) {
            Vacation_Obj.dependents_name = Vacation_Obj.dependents_name + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_name = xx;

        }
    });
    $(".dependents_relationship").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_relation != '' && Vacation_Obj.dependents_relation != null) {
            Vacation_Obj.dependents_relation = Vacation_Obj.dependents_relation + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_relation = xx;
        }
    });
    $(".dependents_age").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_age != '' && Vacation_Obj.dependents_age != null) {
            Vacation_Obj.dependents_age = Vacation_Obj.dependents_age + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_age = xx;
        }
    });
    $(".dependents_visa_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_visa_type != '' && Vacation_Obj.dependents_visa_type != null) {
            Vacation_Obj.dependents_visa_type = Vacation_Obj.dependents_visa_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_visa_type = xx;
        }
    });
    $(".dependents_ta_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_ta_type != '' && Vacation_Obj.dependents_ta_type != null) {
            Vacation_Obj.dependents_ta_type = Vacation_Obj.dependents_ta_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_ta_type = xx;
        }
    });
    $(".dependents_remarks").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_remarks != '' && Vacation_Obj.dependents_remarks != null) {
            Vacation_Obj.dependents_remarks = Vacation_Obj.dependents_remarks + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_remarks = xx;
        }
    });

    if (Vacation_Obj.dependents_name != undefined) {
        var depname = Vacation_Obj.dependents_name.split("~");
        var age = Vacation_Obj.dependents_age.split("~");
        var relation = Vacation_Obj.dependents_relation.split("~");
        for (i = 0; i < depname.length; i++) {
            if (depname[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }
            }
            else if (age[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else if (relation[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else {
                if (dependevalidation == false) {

                }
                else {
                    dependevalidation = true;
                }
            }
        }
    }

    ////Travel Agency Information
    //Vacation_Obj.employee_ticket_number = $('#employee_ticket_number_id').val();
    //Vacation_Obj.employee_date_of_issue = $('#date_of_issue_id').val();
    //Vacation_Obj.employee_ticket_price = $('#ticket_price_id').val();

    //Vacation_Obj.revalidation_charge = $('#revalution_charge_id').val();
    //Vacation_Obj.total_ticket_price = $('#total_ticket_price_id').val();
    //Vacation_Obj.over_all_ticket_status = $('#ddl_Ticket_Status :selected').val();

    //$(".depe_ticket_number").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_number != '' && Vacation_Obj.depent_ticket_number != null) {
    //        Vacation_Obj.depent_ticket_number = Vacation_Obj.depent_ticket_number + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_number = xx;

    //    }
    //});
    //$(".depe_ticket_date_of_issue ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_issue_date != '' && Vacation_Obj.depent_issue_date != null) {
    //        Vacation_Obj.depent_issue_date = Vacation_Obj.depent_issue_date + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_issue_date = xx;

    //    }
    //});

    //$(".depe_ticket_price  ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_price != '' && Vacation_Obj.depent_ticket_price != null) {
    //        Vacation_Obj.depent_ticket_price = Vacation_Obj.depent_ticket_price + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_price = xx;

    //    }
    //});

    //var depe_ticket_number = Vacation_Obj.depent_ticket_number.split("~");
    //var depe_ticket_date_of_issue = Vacation_Obj.depent_issue_date.split("~");
    //var depe_ticket_price = Vacation_Obj.depent_ticket_price.split("~");

    //for (i = 0; i < depe_ticket_number.length; i++) {
    //    if (depe_ticket_number[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }
    //    }
    //    else if (depe_ticket_date_of_issue[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else if (depe_ticket_price[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else {
    //        if (TravelAgencyValidation == false) {

    //        }
    //        else {
    //            TravelAgencyValidation = true;
    //        }
    //    }
    //}    


    if (validation == false) {
        toastrError("Please fill all the mandatory fields.");
        return;
    }
    else {
        if (Vacation_Obj.last_day_of_work != "" && Vacation_Obj.return_to_duty != "") {
            if (Date.parse(Vacation_Obj.last_day_of_work) > Date.parse(Vacation_Obj.return_to_duty)) {
                toastrError("Return to Duty Date should not be greater than Last Day of Work.");
                $('#last_day_of_work_required').css('display', 'block');
                return;
            }
            else {
                $('#last_day_of_work_required').css('display', 'none');
            }
        }
        if (Vacation_Obj.rent_car_pick_up_date != "" && Vacation_Obj.rent_car_return_date != "") {
            if (Date.parse(Vacation_Obj.rent_car_pick_up_date) > Date.parse(Vacation_Obj.rent_car_return_date)) {
                toastrError("Pick up Date should not be greater than Return Date.");
                $('#car_pick_up_date_required').css('display', 'block');
                return;
            }
            else {
                $('#car_pick_up_date_required').css('display', 'none');
            }
        }
        if (Vacation_Obj.hotel_booking_check_in_date != "" && Vacation_Obj.hotel_booking_check_out_date != "") {
            if (Date.parse(Vacation_Obj.hotel_booking_check_in_date) >= Date.parse(Vacation_Obj.hotel_booking_check_out_date)) {
                toastrError("Check in Date should not be greater than Check out Date.");
                $('#room_check_in_date_id_required').css('display', 'block');
                return;
            }
            else {
                $('#room_check_in_date_id_required').css('display', 'none');
            }
        }

        if (Vacation_Obj.departure_date != "" && Vacation_Obj.return_date != "") {
            if (Date.parse(Vacation_Obj.departure_date) >= Date.parse(Vacation_Obj.return_date)) {
                toastrError("Departure Date should not be greater than Return Date.");
                $('#departure_date_id_required').css('display', 'block');
                return;
            }
            else {
                $('#departure_date_id_required').css('display', 'none');
            }
        }

        if (Vacation_Obj.from_period != "" && Vacation_Obj.to_period != "") {
            if (Date.parse(Vacation_Obj.from_period) > Date.parse(Vacation_Obj.to_period)) {
                toastrError("Ticket utilize for the period from should not be greater than to date.");
                $('#spn_From_Period_required').css('display', 'block');
                return;
            }
            else {
                $('#spn_From_Period_required').css('display', 'none');
            }
        }
    }

    if (dependevalidation == false) {
        toastrError("Please fill all the dependent information.");
        return;
    }

    //if (TravelAgencyValidation == false) {
    //    toastrError("Please fill all the travel agency information");
    //    return;
    //}              

    if (validation == true && dependevalidation == true) {
        obj.VacationModel = Vacation_Obj;
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TA_Vacation",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $('#save_request_btn').css('display', 'none');
                    $('#submit_request_btn').css('display', 'block');
                    requestId = response.Request_Id;
                    var request_full_Id = application_name + '-' + requestId;
                    $('#hdn_request_id').val(requestId);
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
                    $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    requestId = $('#hdn_request_id').val();
                }
            },
        });

    }

}

//P007-Vacation(Preema)
function Edit_P007() {
    var validation = true;
    var dependevalidation = true;
    //var TravelAgencyValidation = true;
    var application_name = $('#application_code_id').val();
    var obj = new Object();
    var Vacation_Obj = new Object();
    obj._FileList = [];
    requestId = $('#hdn_request_id').val();
    obj.request_id = requestId;
    Vacation_Obj.request_id = requestId;

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    //var wf_createtype_vacation = $('.wf_createtype_vacation :selected').val();
    //if (wf_createtype_vacation == 1) {
    //    var requestId = $('.mod_requestid_vacation :selected').val();
    //    Vacation_Obj.request_id = requestId;
    //    obj.request_id = requestId;
    //}

    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();


    Vacation_Obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    Vacation_Obj.application_id = $('#application-list-drop-id').val();
    Vacation_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Vacation_Obj.creator_id = $('#emp_identify_id').val();

    Vacation_Obj.place_to_visit = $('#place_to_visit_id').val();
    if (Vacation_Obj.place_to_visit == "") {
        $('#place_to_visit_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#place_to_visit_required').css('display', 'none');
    }
    Vacation_Obj.reason = $('#reason_id').val();
    if (Vacation_Obj.reason == "") {
        $('#reason_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#reason_required').css('display', 'none');
    }
    Vacation_Obj.remark_one = $('#remark_one_id').val();
    if (Vacation_Obj.remark_one == "") {
        $('#remarks_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#remarks_required').css('display', 'none');
    }

    Vacation_Obj.is_complaince_approval_required = $('#compliance_approval_required_id :selected').val();
    if (Vacation_Obj.is_complaince_approval_required == 1) {
        Vacation_Obj.compliance_approval_date = $('#compliance_approval_date_id').val();
        if (Vacation_Obj.compliance_approval_date == "") {
            $('#compliance_approval_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#compliance_approval_date_required').css('display', 'none');
        }
    }

    Vacation_Obj.last_day_of_work = $('#last_day_of_work_id').val();
    if (Vacation_Obj.last_day_of_work == "") {
        $('#last_day_of_work_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#last_day_of_work_required').css('display', 'none');
    }

    Vacation_Obj.return_to_duty = $('#requrn_to_duty_id').val();
    if (Vacation_Obj.return_to_duty == "") {
        $('#requrn_to_duty_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#requrn_to_duty_required').css('display', 'none');
    }

    Vacation_Obj.workflow_delegated = $('#workflow_delegated_id :selected').val();
    if (Vacation_Obj.workflow_delegated == 0) {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
        if (Vacation_Obj.justification_no_delegation == "") {
            $('#workflow_delegation_justification_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#workflow_delegation_justification_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
    }


    Vacation_Obj.address_during_absence = $('#new_address_id').val();
    if (Vacation_Obj.address_during_absence == "") {
        $('#new_address_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#new_address_id_required').css('display', 'none');
    }
    Vacation_Obj.telephone = $('#telephone_number_id').val();
    if (Vacation_Obj.telephone == "") {
        $('#telephone_number_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#telephone_number_required').css('display', 'none');
    }

    Vacation_Obj.mode_of_travel = $('#mode_of_travel :selected').val();


    Vacation_Obj.Visa_Duration = $("input[name='visa_duration']:checked").val();
    Vacation_Obj.Visa_With = $("input[name='passport']:checked").val();


    if (Vacation_Obj.mode_of_travel == "Air") {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        if (Vacation_Obj.location_id == "") {
            $('#travel_location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_location_required').css('display', 'none');
        }

        if (Vacation_Obj.Visa_With == "None") {
            $('#passport_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#passport_required').css('display', 'none');
        }


    }
    else {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        $('#travel_location_required').css('display', 'none');
        $('#passport_required').css('display', 'none');
    }

    Vacation_Obj.type_of_exit_visa = $("input[name='exit_visa_type']:checked").val();
    Vacation_Obj.travel_visa_charged_to = $("input[name='visa_charged_to']:checked").val();


    Vacation_Obj.required_foreign_visa = $('#foreign_visa_id :selected').val();
    Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
    if (Vacation_Obj.required_foreign_visa == 1) {

        if (Vacation_Obj.foreign_visa_countries == "") {
            $('#countries_for_foreign_visa_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#countries_for_foreign_visa_required').css('display', 'none');
        }
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        if (Vacation_Obj.foreign_visa_quantity == "") {
            $('#foregion_visa_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#foregion_visa_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        $('#countries_for_foreign_visa_required').css('display', 'none');
        $('#foregion_visa_quantity_required').css('display', 'none');
    }



    Vacation_Obj.required_travel_insurance = $('#travel_insurance_id :selected').val();


    if (Vacation_Obj.required_travel_insurance == 1) {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        if (Vacation_Obj.travel_insurance_countries == "") {
            $('#travel_insurance_countries_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_countries_required').css('display', 'none');
        }
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        if (Vacation_Obj.travel_insurance_quantity == "") {
            $('#travel_insurance_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        $('#travel_insurance_countries_required').css('display', 'none');
        $('#travel_insurance_quantity_required').css('display', 'none');
    }

    Vacation_Obj.required_rent_car = $('#rent_car_id :selected').val();

    if (Vacation_Obj.required_rent_car == 1) {

        Vacation_Obj.rent_car_charged_to = $('#rent_car_charged_to_id').val();

        if (Vacation_Obj.rent_car_charged_to == "Project") {
            Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
            if (Vacation_Obj.rent_car_project_no == "") {
                $('#rent_car_project_no_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                validation = false;
            }
            else {
                $('#rent_car_project_no_required').css('display', 'none');
            }
        }
        else {
            Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
        }

        Vacation_Obj.car_type = $('#ddl_car_type_data :selected').val();
        if (Vacation_Obj.car_type == "") {
            $('#car_type_data_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_type_data_required').css('display', 'none');
        }

        Vacation_Obj.rent_car_picked_up_at = $('#car_pick_up_at_id').val();

        if (Vacation_Obj.rent_car_picked_up_at == "") {
            $('#car_pick_up_at_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_pick_up_at_required').css('display', 'none');
        }

        Vacation_Obj.rent_car_pick_up_date = $('#car_pick_up_date_id').val();
        if (Vacation_Obj.rent_car_pick_up_date == "") {
            $('#car_pick_up_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_pick_up_date_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_pick_up_time = $('#car_pick_up_time_id').val();
        if (Vacation_Obj.rent_car_pick_up_time == "") {
            $('#car_pick_up_time_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_pick_up_time_required').css('display', 'none');
        }

        Vacation_Obj.rent_car_return_date = $('#rent_car_return_date_id').val();
        if (Vacation_Obj.rent_car_return_date == "") {
            $('#rent_car_return_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#rent_car_return_date_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_return_time = $('#rent_car_return_time_id').val();
        if (Vacation_Obj.rent_car_return_time == "") {
            $('#rent_car_return_time_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#rent_car_return_time_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_payment_type = $('.car_payment_type').val();
        if (Vacation_Obj.rent_car_payment_type == "") {
            $('#car_payment_type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#car_payment_type_required').css('display', 'none');
        }
        Vacation_Obj.rent_car_remark = $('#remark_two_id').val();
    }


    Vacation_Obj.required_hotel_booking = $('#hotel_booking_id :selected').val();
    if (Vacation_Obj.required_hotel_booking == 1) {
        Vacation_Obj.hotel_booking_charged_to = $('#hotel_booking_charged_to_id').val();
        if (Vacation_Obj.hotel_booking_charged_to == "Project") {
            Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
            if (Vacation_Obj.hptel_booking_project_no == "") {
                $('#hotel_booking_project_no_id_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                validation = false;
            }
            else {
                $('#hotel_booking_project_no_id_required').css('display', 'none');
            }
        }
        else {
            Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
        }


        Vacation_Obj.hotel_name = $('#hotel_name_id').val();
        if (Vacation_Obj.hotel_name == "") {
            $('#hotel_name_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_name_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_location = $('#hotel_location').val();
        if (Vacation_Obj.hotel_location == "") {
            $('#hotel_location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_location_required').css('display', 'none');
        }
        Vacation_Obj.type_of_room = $('#type_of_rooms_id').val();
        Vacation_Obj.room_preferences = $('#room_preferences_id').val();
        Vacation_Obj.number_of_rooms = $('#number_of_rooms_id').val();

        if (Vacation_Obj.number_of_rooms == "") {
            $('#number_of_rooms_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#number_of_rooms_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_booking_payment_mode = $('.hotel_payment_type').val();
        Vacation_Obj.hotel_booking_check_in_date = $('#room_check_in_date_id').val();

        if (Vacation_Obj.hotel_booking_payment_mode == "") {
            $('#hotel_payment_type_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_payment_type_required').css('display', 'none');
        }

        if (Vacation_Obj.hotel_booking_check_in_date == "") {
            $('#room_check_in_date_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_in_date_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_check_in_time = $('#room_check_in_time_id').val();
        if (Vacation_Obj.hotel_check_in_time == "") {
            $('#room_check_in_time_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_in_time_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_booking_check_out_date = $('#room_check_out_date_id').val();
        if (Vacation_Obj.hotel_booking_check_out_date == "") {
            $('#room_check_out_date_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_out_date_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_check_out_time = $('#room_check_out_time_id').val();
        if (Vacation_Obj.hotel_check_out_time == "") {
            $('#room_check_out_time_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#room_check_out_time_id_required').css('display', 'none');
        }
        Vacation_Obj.hotel_booking_remark = $('#remark_three_id').val();
    }

    Vacation_Obj.departure_date = $('#departure_date_id').val();
    Vacation_Obj.departure_flight_number = $('#depature_flight_no_id').val();
    Vacation_Obj.return_date = $('#departure_return_date_id').val();
    Vacation_Obj.return_flight_number = $('#return_flight_number_id').val();
    Vacation_Obj.travel_routing = $('#travel_route_id').val();
    Vacation_Obj.type_of_ticket = $('#type_of_ticket_id').val();
    Vacation_Obj.note = $('#note_id').val();


    if (Vacation_Obj.departure_date == "") {
        $('#departure_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.return_date == "") {
        $('#departure_return_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_return_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.travel_routing == "") {
        $('#travel_route_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#travel_route_id_required').css('display', 'none');
    }
    if (Vacation_Obj.mode_of_travel == "Air") {
        if (Vacation_Obj.departure_flight_number == "") {
            $('#depature_flight_no_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#depature_flight_no_id_required').css('display', 'none');
        }
        if (Vacation_Obj.return_flight_number == "") {
            $('#return_flight_number_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#return_flight_number_id_required').css('display', 'none');
        }
    }

    Vacation_Obj.amx_holder = $('#amex_holder_id :selected').val();
    Vacation_Obj.cash_advance = $('#cash_advance_id :selected').val();
    Vacation_Obj.salary_advance = $('#salary_advance_id').val();
    Vacation_Obj.bank_account = $('#bank_account_id').val();
    Vacation_Obj.ticket_cost = $('#ticket_cost_id').val();
    Vacation_Obj.hotel_cost = $('#hotel_cost_id').val();
    Vacation_Obj.daily_allowance = $('#daily_allowance_id').val();
    Vacation_Obj.other_expenses = $('#other_expense_id').val();
    Vacation_Obj.travel_advance_remark = $('#remark_four_id').val();
    Vacation_Obj.travel_advance_total = $('#travel_advance_id').val();
    Vacation_Obj.iban = $('#iban_id').val();

    if (Vacation_Obj.cash_advance == 1) {
        if (Vacation_Obj.salary_advance == "") {
            $('#salary_advance_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#salary_advance_id_required').css('display', 'none');
        }
        if (Vacation_Obj.bank_account != "") {

            if (Vacation_Obj.iban == "") {
                $('#iban_id_required').css('display', 'block');
                $('#submit_request_btn').prop('disabled', false);
                validation = false;
            }
            else {
                $('#iban_id_required').css('display', 'none');
            }
        }
        if (Vacation_Obj.ticket_cost == "") {
            $('#ticket_cost_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#ticket_cost_id_required').css('display', 'none');
        }
        if (Vacation_Obj.hotel_cost == "") {
            $('#hotel_cost_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#hotel_cost_id_required').css('display', 'none');
        }
        if (Vacation_Obj.daily_allowance == "") {
            $('#daily_allowance_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#daily_allowance_id_required').css('display', 'none');
        }
        if (Vacation_Obj.other_expenses == "") {
            $('#other_expense_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#other_expense_id_required').css('display', 'none');
        }
    }

    //Number of Days Information
    Vacation_Obj.ticket_charged_to = $('#dp_Ticket_Charge :selected').val();
    Vacation_Obj.from_period = $('#txt_from_period').val();
    Vacation_Obj.to_period = $('#txt_to_period').val();
    Vacation_Obj.Vacation = $('#txt_Vacation').val();
    Vacation_Obj.Leave_WO_Pay = $('#txt_Leave').val();
    Vacation_Obj.Holidays = $('#txt_Holidays').val();
    Vacation_Obj.Friday = $('#txt_Friday').val();
    Vacation_Obj.Saturday = $('#txt_Saturday').val();
    Vacation_Obj.Total_No_of_Days = $('#txt_Total').val();

    if (Vacation_Obj.ticket_charged_to == 1) {

        if (Vacation_Obj.from_period == "") {
            $('#spn_From_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        if (Vacation_Obj.to_period == "") {
            $('#spn_To_Period_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
    }

    ////Employee entitlement to be processed by SAS

    //Vacation_Obj.HR_Remarks_Visa = $('#div_Visa_Remarks').text().trim();
    //Vacation_Obj.HR_Remarks_Taxi = $('#div_Taxi_Remarks').text().trim();


    //if ($("input[name='Visa_Amount']:checked").val() == "Yes") {
    //    Vacation_Obj.Visa_Amount_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Visa_Amount_Claim = "False";
    //}

    //if ($("input[name='Taxi_Fare']:checked").val() == "Yes") {
    //    Vacation_Obj.Taxi_Fare_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Taxi_Fare_Claim = "False";
    //}

    //if (Vacation_Obj.Visa_Amount_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Visa_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //if (Vacation_Obj.Taxi_Fare_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Taxi_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //Dependents Information
    Vacation_Obj.Dependentsinfo = [];
    $(".dependents_details").each(function () {
        var data = new Object();
        var currentRow = $(this).closest("tr");
        data.name = currentRow.find("td:eq(0)").text();
        data.relation_ship = currentRow.find("td:eq(1)").text();
        data.age = currentRow.find("td:eq(2)").text();
        data.visa_type = currentRow.find("td:eq(3)").val();
        data.ta_type = currentRow.find("td:eq(4)").text();
        data.remarks = currentRow.find("td:eq(5)").text();
        Vacation_Obj.Dependentsinfo.push(data);
    });

    $(".dependents_name").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_name != '' && Vacation_Obj.dependents_name != null) {
            Vacation_Obj.dependents_name = Vacation_Obj.dependents_name + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_name = xx;

        }
    });
    $(".dependents_relationship").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_relation != '' && Vacation_Obj.dependents_relation != null) {
            Vacation_Obj.dependents_relation = Vacation_Obj.dependents_relation + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_relation = xx;
        }
    });
    $(".dependents_age").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_age != '' && Vacation_Obj.dependents_age != null) {
            Vacation_Obj.dependents_age = Vacation_Obj.dependents_age + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_age = xx;
        }
    });
    $(".dependents_visa_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_visa_type != '' && Vacation_Obj.dependents_visa_type != null) {
            Vacation_Obj.dependents_visa_type = Vacation_Obj.dependents_visa_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_visa_type = xx;
        }
    });
    $(".dependents_ta_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_ta_type != '' && Vacation_Obj.dependents_ta_type != null) {
            Vacation_Obj.dependents_ta_type = Vacation_Obj.dependents_ta_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_ta_type = xx;
        }
    });
    $(".dependents_remarks").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_remarks != '' && Vacation_Obj.dependents_remarks != null) {
            Vacation_Obj.dependents_remarks = Vacation_Obj.dependents_remarks + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_remarks = xx;
        }
    });

    if (Vacation_Obj.dependents_name != undefined) {
        var depname = Vacation_Obj.dependents_name.split("~");
        var age = Vacation_Obj.dependents_age.split("~");
        var relation = Vacation_Obj.dependents_relation.split("~");
        for (i = 0; i < depname.length; i++) {
            if (depname[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }
            }
            else if (age[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else if (relation[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else {
                if (dependevalidation == false) {

                }
                else {
                    dependevalidation = true;
                }
            }
        }
    }

    ////Travel Agency Information
    //Vacation_Obj.employee_ticket_number = $('#employee_ticket_number_id').val();
    //Vacation_Obj.employee_date_of_issue = $('#date_of_issue_id').val();
    //Vacation_Obj.employee_ticket_price = $('#ticket_price_id').val();

    //Vacation_Obj.revalidation_charge = $('#revalution_charge_id').val();
    //Vacation_Obj.total_ticket_price = $('#total_ticket_price_id').val();
    //Vacation_Obj.over_all_ticket_status = $('#ddl_Ticket_Status :selected').val();

    //$(".depe_ticket_number").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_number != '' && Vacation_Obj.depent_ticket_number != null) {
    //        Vacation_Obj.depent_ticket_number = Vacation_Obj.depent_ticket_number + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_number = xx;

    //    }
    //});
    //$(".depe_ticket_date_of_issue ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_issue_date != '' && Vacation_Obj.depent_issue_date != null) {
    //        Vacation_Obj.depent_issue_date = Vacation_Obj.depent_issue_date + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_issue_date = xx;

    //    }
    //});

    //$(".depe_ticket_price  ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_price != '' && Vacation_Obj.depent_ticket_price != null) {
    //        Vacation_Obj.depent_ticket_price = Vacation_Obj.depent_ticket_price + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_price = xx;

    //    }
    //});

    //var depe_ticket_number = Vacation_Obj.depent_ticket_number.split("~");
    //var depe_ticket_date_of_issue = Vacation_Obj.depent_issue_date.split("~");
    //var depe_ticket_price = Vacation_Obj.depent_ticket_price.split("~");

    //for (i = 0; i < depe_ticket_number.length; i++) {
    //    if (depe_ticket_number[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }
    //    }
    //    else if (depe_ticket_date_of_issue[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else if (depe_ticket_price[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else {
    //        if (TravelAgencyValidation == false) {

    //        }
    //        else {
    //            TravelAgencyValidation = true;
    //        }
    //    }
    //}    


    if (validation == false) {
        toastrError("Please fill all the mandatory fields.");
        return;
    }
    else {
        if (Vacation_Obj.last_day_of_work != "" && Vacation_Obj.return_to_duty != "") {
            if (Date.parse(Vacation_Obj.last_day_of_work) > Date.parse(Vacation_Obj.return_to_duty)) {
                toastrError("Return to Duty Date should not be greater than Last Day of Work.");
                $('#last_day_of_work_required').css('display', 'block');
                return;
            }
            else {
                $('#last_day_of_work_required').css('display', 'none');
            }
        }
        if (Vacation_Obj.rent_car_pick_up_date != "" && Vacation_Obj.rent_car_return_date != "") {
            if (Date.parse(Vacation_Obj.rent_car_pick_up_date) > Date.parse(Vacation_Obj.rent_car_return_date)) {
                toastrError("Pick up Date should not be greater than Return Date.");
                $('#car_pick_up_date_required').css('display', 'block');
                return;
            }
            else {
                $('#car_pick_up_date_required').css('display', 'none');
            }
        }
        if (Vacation_Obj.hotel_booking_check_in_date != "" && Vacation_Obj.hotel_booking_check_out_date != "") {
            if (Date.parse(Vacation_Obj.hotel_booking_check_in_date) >= Date.parse(Vacation_Obj.hotel_booking_check_out_date)) {
                toastrError("Check in Date should not be greater than Check out Date.");
                $('#room_check_in_date_id_required').css('display', 'block');
                return;
            }
            else {
                $('#room_check_in_date_id_required').css('display', 'none');
            }
        }

        if (Vacation_Obj.departure_date != "" && Vacation_Obj.return_date != "") {
            if (Date.parse(Vacation_Obj.departure_date) >= Date.parse(Vacation_Obj.return_date)) {
                toastrError("Departure Date should not be greater than Return Date.");
                $('#departure_date_id_required').css('display', 'block');
                return;
            }
            else {
                $('#departure_date_id_required').css('display', 'none');
            }
        }

        if (Vacation_Obj.from_period != "" && Vacation_Obj.to_period != "") {
            if (Date.parse(Vacation_Obj.from_period) > Date.parse(Vacation_Obj.to_period)) {
                toastrError("Ticket utilize for the period from should not be greater than to date.");
                $('#spn_From_Period_required').css('display', 'block');
                return;
            }
            else {
                $('#spn_From_Period_required').css('display', 'none');
            }
        }
    }

    if (dependevalidation == false) {
        toastrError("Please fill all the dependent information.");
        return;
    }

    //if (TravelAgencyValidation == false) {
    //    toastrError("Please fill all the travel agency information");
    //    return;
    //}

    if (validation == true && dependevalidation == true) {
        obj.VacationModel = Vacation_Obj;
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_Edit_TA_Vacation",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $(".se-pre-con").hide();
                    requestId = $('#hdn_request_id').val();
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
                    requestId = $('#hdn_request_id').val();
                }
            },
        });
    }
}

//P037-Dependents Only(Preema)
function Save_P037() {
    debugger;
    var validation = true;
    var dependevalidation = true;
    //var TravelAgencyValidation = true;
    var application_name = $('#application_code_id').val();
    var obj = new Object();
    var Vacation_Obj = new Object();
    obj._FileList = [];

    obj.request_id = requestId;
    Vacation_Obj.request_id = requestId;

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    //var wf_createtype_vacation = $('.wf_createtype_vacation :selected').val();
    //if (wf_createtype_vacation == 1) {
    //    var requestId = $('.mod_requestid_vacation :selected').val();
    //    Vacation_Obj.request_id = requestId;
    //    obj.request_id = requestId;
    //}

    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();


    Vacation_Obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    Vacation_Obj.application_id = $('#application-list-drop-id').val();
    Vacation_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Vacation_Obj.creator_id = $('#emp_identify_id').val();

    //Vacation_Obj.place_to_visit = $('#place_to_visit_id').val();
    //if (Vacation_Obj.place_to_visit == "") {
    //    $('#place_to_visit_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#place_to_visit_required').css('display', 'none');
    //}
    //Vacation_Obj.reason = $('#reason_id').val();
    //if (Vacation_Obj.reason == "") {
    //    $('#reason_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#reason_required').css('display', 'none');
    //}
    //Vacation_Obj.remark_one = $('#remark_one_id').val();
    //if (Vacation_Obj.remark_one == "") {
    //    $('#remarks_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#remarks_required').css('display', 'none');
    //}

    Vacation_Obj.is_complaince_approval_required = $('#compliance_approval_required_id :selected').val();
    if (Vacation_Obj.is_complaince_approval_required == 1) {
        Vacation_Obj.compliance_approval_date = $('#compliance_approval_date_id').val();
        if (Vacation_Obj.compliance_approval_date == "") {
            $('#compliance_approval_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#compliance_approval_date_required').css('display', 'none');
        }
    }

    //Vacation_Obj.last_day_of_work = $('#last_day_of_work_id').val();
    //if (Vacation_Obj.last_day_of_work == "") {
    //    $('#last_day_of_work_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#last_day_of_work_required').css('display', 'none');
    //}

    //Vacation_Obj.return_to_duty = $('#requrn_to_duty_id').val();
    //if (Vacation_Obj.return_to_duty == "") {
    //    $('#requrn_to_duty_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#requrn_to_duty_required').css('display', 'none');
    //}

    Vacation_Obj.workflow_delegated = $('#workflow_delegated_id :selected').val();
    if (Vacation_Obj.workflow_delegated == 0) {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
        if (Vacation_Obj.justification_no_delegation == "") {
            $('#workflow_delegation_justification_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#workflow_delegation_justification_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
    }


    //Vacation_Obj.address_during_absence = $('#new_address_id').val();
    //if (Vacation_Obj.address_during_absence == "") {
    //    $('#new_address_id_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#new_address_id_required').css('display', 'none');
    //}
    //Vacation_Obj.telephone = $('#telephone_number_id').val();
    //if (Vacation_Obj.telephone == "") {
    //    $('#telephone_number_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#telephone_number_required').css('display', 'none');
    //}

    Vacation_Obj.mode_of_travel = $('#mode_of_travel :selected').val();


    Vacation_Obj.Visa_Duration = $("input[name='visa_duration']:checked").val();
    Vacation_Obj.Visa_With = $("input[name='passport']:checked").val();


    if (Vacation_Obj.mode_of_travel == "Air") {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        if (Vacation_Obj.location_id == "") {
            $('#travel_location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_location_required').css('display', 'none');
        }

        if (Vacation_Obj.Visa_With == "None") {
            $('#passport_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#passport_required').css('display', 'none');
        }


    }
    else {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        $('#travel_location_required').css('display', 'none');
        $('#passport_required').css('display', 'none');
    }

    Vacation_Obj.type_of_exit_visa = $("input[name='exit_visa_type']:checked").val();
    Vacation_Obj.travel_visa_charged_to = $("input[name='visa_charged_to']:checked").val();


    Vacation_Obj.required_foreign_visa = $('#foreign_visa_id :selected').val();
    Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
    if (Vacation_Obj.required_foreign_visa == 1) {

        if (Vacation_Obj.foreign_visa_countries == "") {
            $('#countries_for_foreign_visa_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#countries_for_foreign_visa_required').css('display', 'none');
        }
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        if (Vacation_Obj.foreign_visa_quantity == "") {
            $('#foregion_visa_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#foregion_visa_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        $('#countries_for_foreign_visa_required').css('display', 'none');
        $('#foregion_visa_quantity_required').css('display', 'none');
    }



    Vacation_Obj.required_travel_insurance = $('#travel_insurance_id :selected').val();


    if (Vacation_Obj.required_travel_insurance == 1) {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        if (Vacation_Obj.travel_insurance_countries == "") {
            $('#travel_insurance_countries_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_countries_required').css('display', 'none');
        }
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        if (Vacation_Obj.travel_insurance_quantity == "") {
            $('#travel_insurance_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        $('#travel_insurance_countries_required').css('display', 'none');
        $('#travel_insurance_quantity_required').css('display', 'none');
    }

    //Vacation_Obj.required_rent_car = $('#rent_car_id :selected').val();

    //if (Vacation_Obj.required_rent_car == 1) {

    //    Vacation_Obj.rent_car_charged_to = $('#rent_car_charged_to_id').val();

    //    if (Vacation_Obj.rent_car_charged_to == "Project") {
    //        Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
    //        if (Vacation_Obj.rent_car_project_no == "") {
    //            $('#rent_car_project_no_required').css('display', 'block');
    //            $('#submit_request_btn').prop('disabled', false);
    //            validation = false;
    //        }
    //        else {
    //            $('#rent_car_project_no_required').css('display', 'none');
    //        }
    //    }
    //    else {
    //        Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
    //    }

    //    Vacation_Obj.car_type = $('#ddl_car_type_data :selected').val();
    //    if (Vacation_Obj.car_type == "") {
    //        $('#car_type_data_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_type_data_required').css('display', 'none');
    //    }

    //    Vacation_Obj.rent_car_picked_up_at = $('#car_pick_up_at_id').val();

    //    if (Vacation_Obj.rent_car_picked_up_at == "") {
    //        $('#car_pick_up_at_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_pick_up_at_required').css('display', 'none');
    //    }

    //    Vacation_Obj.rent_car_pick_up_date = $('#car_pick_up_date_id').val();
    //    if (Vacation_Obj.rent_car_pick_up_date == "") {
    //        $('#car_pick_up_date_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_pick_up_date_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_pick_up_time = $('#car_pick_up_time_id').val();
    //    if (Vacation_Obj.rent_car_pick_up_time == "") {
    //        $('#car_pick_up_time_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_pick_up_time_required').css('display', 'none');
    //    }

    //    Vacation_Obj.rent_car_return_date = $('#rent_car_return_date_id').val();
    //    if (Vacation_Obj.rent_car_return_date == "") {
    //        $('#rent_car_return_date_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#rent_car_return_date_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_return_time = $('#rent_car_return_time_id').val();
    //    if (Vacation_Obj.rent_car_return_time == "") {
    //        $('#rent_car_return_time_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#rent_car_return_time_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_payment_type = $('.car_payment_type').val();
    //    if (Vacation_Obj.rent_car_payment_type == "") {
    //        $('#car_payment_type_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_payment_type_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_remark = $('#remark_two_id').val();
    //}


    //Vacation_Obj.required_hotel_booking = $('#hotel_booking_id :selected').val();
    //if (Vacation_Obj.required_hotel_booking == 1) {
    //    Vacation_Obj.hotel_booking_charged_to = $('#hotel_booking_charged_to_id').val();
    //    if (Vacation_Obj.hotel_booking_charged_to == "Project") {
    //        Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
    //        if (Vacation_Obj.hptel_booking_project_no == "") {
    //            $('#hotel_booking_project_no_id_required').css('display', 'block');
    //            $('#submit_request_btn').prop('disabled', false);
    //            validation = false;
    //        }
    //        else {
    //            $('#hotel_booking_project_no_id_required').css('display', 'none');
    //        }
    //    }
    //    else {
    //        Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
    //    }


    //    Vacation_Obj.hotel_name = $('#hotel_name_id').val();
    //    if (Vacation_Obj.hotel_name == "") {
    //        $('#hotel_name_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_name_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_location = $('#hotel_location').val();
    //    if (Vacation_Obj.hotel_location == "") {
    //        $('#hotel_location_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_location_required').css('display', 'none');
    //    }
    //    Vacation_Obj.type_of_room = $('#type_of_rooms_id').val();
    //    Vacation_Obj.room_preferences = $('#room_preferences_id').val();
    //    Vacation_Obj.number_of_rooms = $('#number_of_rooms_id').val();

    //    if (Vacation_Obj.number_of_rooms == "") {
    //        $('#number_of_rooms_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#number_of_rooms_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_booking_payment_mode = $('.hotel_payment_type').val();
    //    Vacation_Obj.hotel_booking_check_in_date = $('#room_check_in_date_id').val();

    //    if (Vacation_Obj.hotel_booking_payment_mode == "") {
    //        $('#hotel_payment_type_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_payment_type_required').css('display', 'none');
    //    }

    //    if (Vacation_Obj.hotel_booking_check_in_date == "") {
    //        $('#room_check_in_date_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_in_date_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_check_in_time = $('#room_check_in_time_id').val();
    //    if (Vacation_Obj.hotel_check_in_time == "") {
    //        $('#room_check_in_time_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_in_time_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_booking_check_out_date = $('#room_check_out_date_id').val();
    //    if (Vacation_Obj.hotel_booking_check_out_date == "") {
    //        $('#room_check_out_date_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_out_date_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_check_out_time = $('#room_check_out_time_id').val();
    //    if (Vacation_Obj.hotel_check_out_time == "") {
    //        $('#room_check_out_time_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_out_time_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_booking_remark = $('#remark_three_id').val();
    //}

    Vacation_Obj.departure_date = $('#departure_date_id').val();
    Vacation_Obj.departure_flight_number = $('#depature_flight_no_id').val();
    Vacation_Obj.return_date = $('#departure_return_date_id').val();
    Vacation_Obj.return_flight_number = $('#return_flight_number_id').val();
    Vacation_Obj.travel_routing = $('#travel_route_id').val();
    Vacation_Obj.type_of_ticket = $('#type_of_ticket_id').val();
    Vacation_Obj.note = $('#note_id').val();


    if (Vacation_Obj.departure_date == "") {
        $('#departure_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.return_date == "") {
        $('#departure_return_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_return_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.travel_routing == "") {
        $('#travel_route_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#travel_route_id_required').css('display', 'none');
    }
    if (Vacation_Obj.mode_of_travel == "Air") {
        if (Vacation_Obj.departure_flight_number == "") {
            $('#depature_flight_no_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#depature_flight_no_id_required').css('display', 'none');
        }
        if (Vacation_Obj.return_flight_number == "") {
            $('#return_flight_number_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#return_flight_number_id_required').css('display', 'none');
        }
    }

    //Vacation_Obj.amx_holder = $('#amex_holder_id :selected').val();
    //Vacation_Obj.cash_advance = $('#cash_advance_id :selected').val();
    //Vacation_Obj.salary_advance = $('#salary_advance_id').val();
    //Vacation_Obj.bank_account = $('#bank_account_id').val();
    //Vacation_Obj.ticket_cost = $('#ticket_cost_id').val();
    //Vacation_Obj.hotel_cost = $('#hotel_cost_id').val();
    //Vacation_Obj.daily_allowance = $('#daily_allowance_id').val();
    //Vacation_Obj.other_expenses = $('#other_expense_id').val();
    //Vacation_Obj.travel_advance_remark = $('#remark_four_id').val();
    //Vacation_Obj.travel_advance_total = $('#travel_advance_id').val();
    //Vacation_Obj.iban = $('#iban_id').val();

    //if (Vacation_Obj.cash_advance == 1) {
    //    if (Vacation_Obj.salary_advance == "") {
    //        $('#salary_advance_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#salary_advance_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.bank_account != "") {

    //        if (Vacation_Obj.iban == "") {
    //            $('#iban_id_required').css('display', 'block');
    //            $('#submit_request_btn').prop('disabled', false);
    //            validation = false;
    //        }
    //        else {
    //            $('#iban_id_required').css('display', 'none');
    //        }
    //    }
    //    if (Vacation_Obj.ticket_cost == "") {
    //        $('#ticket_cost_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#ticket_cost_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.hotel_cost == "") {
    //        $('#hotel_cost_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_cost_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.daily_allowance == "") {
    //        $('#daily_allowance_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#daily_allowance_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.other_expenses == "") {
    //        $('#other_expense_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#other_expense_id_required').css('display', 'none');
    //    }
    //}

    ////Number of Days Information
    //Vacation_Obj.ticket_charged_to = $('#dp_Ticket_Charge :selected').val();
    //Vacation_Obj.from_period = $('#txt_from_period').val();
    //Vacation_Obj.to_period = $('#txt_to_period').val();
    //Vacation_Obj.Vacation = $('#txt_Vacation').val();
    //Vacation_Obj.Leave_WO_Pay = $('#txt_Leave').val();
    //Vacation_Obj.Holidays = $('#txt_Holidays').val();
    //Vacation_Obj.Friday = $('#txt_Friday').val();
    //Vacation_Obj.Saturday = $('#txt_Saturday').val();
    //Vacation_Obj.Total_No_of_Days = $('#txt_Total').val();

    //if (Vacation_Obj.ticket_charged_to == 1) {

    //    if (Vacation_Obj.from_period == "") {
    //        $('#spn_From_Period_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    if (Vacation_Obj.to_period == "") {
    //        $('#spn_To_Period_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    ////Employee entitlement to be processed by SAS

    //Vacation_Obj.HR_Remarks_Visa = $('#div_Visa_Remarks').text().trim();
    //Vacation_Obj.HR_Remarks_Taxi = $('#div_Taxi_Remarks').text().trim();


    //if ($("input[name='Visa_Amount']:checked").val() == "Yes") {
    //    Vacation_Obj.Visa_Amount_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Visa_Amount_Claim = "False";
    //}

    //if ($("input[name='Taxi_Fare']:checked").val() == "Yes") {
    //    Vacation_Obj.Taxi_Fare_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Taxi_Fare_Claim = "False";
    //}

    //if (Vacation_Obj.Visa_Amount_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Visa_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //if (Vacation_Obj.Taxi_Fare_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Taxi_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //Dependents Information
    Vacation_Obj.Dependentsinfo = [];
    $(".dependents_details").each(function () {
        var data = new Object();
        var currentRow = $(this).closest("tr");
        data.name = currentRow.find("td:eq(0)").text();
        data.relation_ship = currentRow.find("td:eq(1)").text();
        data.age = currentRow.find("td:eq(2)").text();
        data.visa_type = currentRow.find("td:eq(3)").val();
        data.ta_type = currentRow.find("td:eq(4)").text();
        data.remarks = currentRow.find("td:eq(5)").text();
        Vacation_Obj.Dependentsinfo.push(data);
    });

    $(".dependents_name").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_name != '' && Vacation_Obj.dependents_name != null) {
            Vacation_Obj.dependents_name = Vacation_Obj.dependents_name + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_name = xx;

        }
    });
    $(".dependents_relationship").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_relation != '' && Vacation_Obj.dependents_relation != null) {
            Vacation_Obj.dependents_relation = Vacation_Obj.dependents_relation + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_relation = xx;
        }
    });
    $(".dependents_age").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_age != '' && Vacation_Obj.dependents_age != null) {
            Vacation_Obj.dependents_age = Vacation_Obj.dependents_age + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_age = xx;
        }
    });
    $(".dependents_visa_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_visa_type != '' && Vacation_Obj.dependents_visa_type != null) {
            Vacation_Obj.dependents_visa_type = Vacation_Obj.dependents_visa_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_visa_type = xx;
        }
    });
    $(".dependents_ta_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_ta_type != '' && Vacation_Obj.dependents_ta_type != null) {
            Vacation_Obj.dependents_ta_type = Vacation_Obj.dependents_ta_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_ta_type = xx;
        }
    });
    $(".dependents_remarks").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_remarks != '' && Vacation_Obj.dependents_remarks != null) {
            Vacation_Obj.dependents_remarks = Vacation_Obj.dependents_remarks + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_remarks = xx;
        }
    });

    if (Vacation_Obj.dependents_name != undefined) {
        var depname = Vacation_Obj.dependents_name.split("~");
        var age = Vacation_Obj.dependents_age.split("~");
        var relation = Vacation_Obj.dependents_relation.split("~");
        for (i = 0; i < depname.length; i++) {
            if (depname[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }
            }
            else if (age[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else if (relation[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else {
                if (dependevalidation == false) {

                }
                else {
                    dependevalidation = true;
                }
            }
        }
    }

    ////Travel Agency Information
    //Vacation_Obj.employee_ticket_number = $('#employee_ticket_number_id').val();
    //Vacation_Obj.employee_date_of_issue = $('#date_of_issue_id').val();
    //Vacation_Obj.employee_ticket_price = $('#ticket_price_id').val();

    //Vacation_Obj.revalidation_charge = $('#revalution_charge_id').val();
    //Vacation_Obj.total_ticket_price = $('#total_ticket_price_id').val();
    //Vacation_Obj.over_all_ticket_status = $('#ddl_Ticket_Status :selected').val();

    //$(".depe_ticket_number").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_number != '' && Vacation_Obj.depent_ticket_number != null) {
    //        Vacation_Obj.depent_ticket_number = Vacation_Obj.depent_ticket_number + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_number = xx;

    //    }
    //});
    //$(".depe_ticket_date_of_issue ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_issue_date != '' && Vacation_Obj.depent_issue_date != null) {
    //        Vacation_Obj.depent_issue_date = Vacation_Obj.depent_issue_date + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_issue_date = xx;

    //    }
    //});

    //$(".depe_ticket_price  ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_price != '' && Vacation_Obj.depent_ticket_price != null) {
    //        Vacation_Obj.depent_ticket_price = Vacation_Obj.depent_ticket_price + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_price = xx;

    //    }
    //});

    //var depe_ticket_number = Vacation_Obj.depent_ticket_number.split("~");
    //var depe_ticket_date_of_issue = Vacation_Obj.depent_issue_date.split("~");
    //var depe_ticket_price = Vacation_Obj.depent_ticket_price.split("~");

    //for (i = 0; i < depe_ticket_number.length; i++) {
    //    if (depe_ticket_number[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }
    //    }
    //    else if (depe_ticket_date_of_issue[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else if (depe_ticket_price[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else {
    //        if (TravelAgencyValidation == false) {

    //        }
    //        else {
    //            TravelAgencyValidation = true;
    //        }
    //    }
    //}    


    if (validation == false) {
        toastrError("Please fill all the mandatory fields.");
        return;
    }
    else {
        //if (Vacation_Obj.last_day_of_work != "" && Vacation_Obj.return_to_duty != "") {
        //    if (Date.parse(Vacation_Obj.last_day_of_work) > Date.parse(Vacation_Obj.return_to_duty)) {
        //        toastrError("Return to Duty Date should not be greater than Last Day of Work.");
        //        $('#last_day_of_work_required').css('display', 'block');
        //        return;
        //    }
        //    else {
        //        $('#last_day_of_work_required').css('display', 'none');
        //    }
        //}
        //if (Vacation_Obj.rent_car_pick_up_date != "" && Vacation_Obj.rent_car_return_date != "") {
        //    if (Date.parse(Vacation_Obj.rent_car_pick_up_date) > Date.parse(Vacation_Obj.rent_car_return_date)) {
        //        toastrError("Pick up Date should not be greater than Return Date.");
        //        $('#car_pick_up_date_required').css('display', 'block');
        //        return;
        //    }
        //    else {
        //        $('#car_pick_up_date_required').css('display', 'none');
        //    }
        //}
        //if (Vacation_Obj.hotel_booking_check_in_date != "" && Vacation_Obj.hotel_booking_check_out_date != "") {
        //    if (Date.parse(Vacation_Obj.hotel_booking_check_in_date) >= Date.parse(Vacation_Obj.hotel_booking_check_out_date)) {
        //        toastrError("Check in Date should not be greater than Check out Date.");
        //        $('#room_check_in_date_id_required').css('display', 'block');
        //        return;
        //    }
        //    else {
        //        $('#room_check_in_date_id_required').css('display', 'none');
        //    }
        //}

        if (Vacation_Obj.departure_date != "" && Vacation_Obj.return_date != "") {
            if (Date.parse(Vacation_Obj.departure_date) >= Date.parse(Vacation_Obj.return_date)) {
                toastrError("Departure Date should not be greater than Return Date.");
                $('#departure_date_id_required').css('display', 'block');
                return;
            }
            else {
                $('#departure_date_id_required').css('display', 'none');
            }
        }

        if (Vacation_Obj.from_period != "" && Vacation_Obj.to_period != "") {
            if (Date.parse(Vacation_Obj.from_period) > Date.parse(Vacation_Obj.to_period)) {
                toastrError("Ticket utilize for the period from should not be greater than to date.");
                $('#spn_From_Period_required').css('display', 'block');
                return;
            }
            else {
                $('#spn_From_Period_required').css('display', 'none');
            }
        }
    }

    if (dependevalidation == false) {
        toastrError("Please fill all the dependent information.");
        return;
    }

    //if (TravelAgencyValidation == false) {
    //    toastrError("Please fill all the travel agency information");
    //    return;
    //}              

    if (validation == true && dependevalidation == true) {
        obj.VacationModel = Vacation_Obj;
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TA_DependentsOnly",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $('#save_request_btn').css('display', 'none');
                    $('#submit_request_btn').css('display', 'block');
                    requestId = response.Request_Id;
                    var request_full_Id = application_name + '-' + requestId;
                    $('#hdn_request_id').val(requestId);
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
                    $('#edit_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    $('#forward_request_btn').css('display', 'none');//Basheer on 23-01-2020
                    requestId = $('#hdn_request_id').val();
                }
            },
        });

    }

}

//P037-Dependents Only(Preema)
function Edit_P037() {
    var validation = true;
    var dependevalidation = true;
    //var TravelAgencyValidation = true;
    var application_name = $('#application_code_id').val();
    var obj = new Object();
    var Vacation_Obj = new Object();
    obj._FileList = [];
    requestId = $('#hdn_request_id').val();
    obj.request_id = requestId;
    Vacation_Obj.request_id = requestId;

    for (let i = 0; i < lists.length; i++) {
        var x = new Object();
        x.filename = lists[i].filename;
        x.filepath = lists[i].filepath;
        x.filebatch = lists[i].filetype;
        obj._FileList.push(x);
    }
    //var wf_createtype_vacation = $('.wf_createtype_vacation :selected').val();
    //if (wf_createtype_vacation == 1) {
    //    var requestId = $('.mod_requestid_vacation :selected').val();
    //    Vacation_Obj.request_id = requestId;
    //    obj.request_id = requestId;
    //}

    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();


    Vacation_Obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    Vacation_Obj.application_id = $('#application-list-drop-id').val();
    Vacation_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Vacation_Obj.creator_id = $('#emp_identify_id').val();

    //Vacation_Obj.place_to_visit = $('#place_to_visit_id').val();
    //if (Vacation_Obj.place_to_visit == "") {
    //    $('#place_to_visit_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#place_to_visit_required').css('display', 'none');
    //}
    //Vacation_Obj.reason = $('#reason_id').val();
    //if (Vacation_Obj.reason == "") {
    //    $('#reason_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#reason_required').css('display', 'none');
    //}
    //Vacation_Obj.remark_one = $('#remark_one_id').val();
    //if (Vacation_Obj.remark_one == "") {
    //    $('#remarks_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#remarks_required').css('display', 'none');
    //}

    Vacation_Obj.is_complaince_approval_required = $('#compliance_approval_required_id :selected').val();
    if (Vacation_Obj.is_complaince_approval_required == 1) {
        Vacation_Obj.compliance_approval_date = $('#compliance_approval_date_id').val();
        if (Vacation_Obj.compliance_approval_date == "") {
            $('#compliance_approval_date_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#compliance_approval_date_required').css('display', 'none');
        }
    }

    //Vacation_Obj.last_day_of_work = $('#last_day_of_work_id').val();
    //if (Vacation_Obj.last_day_of_work == "") {
    //    $('#last_day_of_work_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#last_day_of_work_required').css('display', 'none');
    //}

    //Vacation_Obj.return_to_duty = $('#requrn_to_duty_id').val();
    //if (Vacation_Obj.return_to_duty == "") {
    //    $('#requrn_to_duty_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#requrn_to_duty_required').css('display', 'none');
    //}

    Vacation_Obj.workflow_delegated = $('#workflow_delegated_id :selected').val();
    if (Vacation_Obj.workflow_delegated == 0) {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
        if (Vacation_Obj.justification_no_delegation == "") {
            $('#workflow_delegation_justification_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#workflow_delegation_justification_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.justification_no_delegation = $('#workflow_delegation_justification_id').val();
    }


    //Vacation_Obj.address_during_absence = $('#new_address_id').val();
    //if (Vacation_Obj.address_during_absence == "") {
    //    $('#new_address_id_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#new_address_id_required').css('display', 'none');
    //}
    //Vacation_Obj.telephone = $('#telephone_number_id').val();
    //if (Vacation_Obj.telephone == "") {
    //    $('#telephone_number_required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    validation = false;
    //}
    //else {
    //    $('#telephone_number_required').css('display', 'none');
    //}

    Vacation_Obj.mode_of_travel = $('#mode_of_travel :selected').val();


    Vacation_Obj.Visa_Duration = $("input[name='visa_duration']:checked").val();
    Vacation_Obj.Visa_With = $("input[name='passport']:checked").val();


    if (Vacation_Obj.mode_of_travel == "Air") {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        if (Vacation_Obj.location_id == "") {
            $('#travel_location_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_location_required').css('display', 'none');
        }

        if (Vacation_Obj.Visa_With == "None") {
            $('#passport_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#passport_required').css('display', 'none');
        }


    }
    else {
        Vacation_Obj.location_id = $('#travel_location_id :selected').val();
        $('#travel_location_required').css('display', 'none');
        $('#passport_required').css('display', 'none');
    }

    Vacation_Obj.type_of_exit_visa = $("input[name='exit_visa_type']:checked").val();
    Vacation_Obj.travel_visa_charged_to = $("input[name='visa_charged_to']:checked").val();


    Vacation_Obj.required_foreign_visa = $('#foreign_visa_id :selected').val();
    Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
    if (Vacation_Obj.required_foreign_visa == 1) {

        if (Vacation_Obj.foreign_visa_countries == "") {
            $('#countries_for_foreign_visa_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#countries_for_foreign_visa_required').css('display', 'none');
        }
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        if (Vacation_Obj.foreign_visa_quantity == "") {
            $('#foregion_visa_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#foregion_visa_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.foreign_visa_countries = $('#countries_for_foreign_visa_id').val();
        Vacation_Obj.foreign_visa_quantity = $('#foregion_visa_quantity_id').val();
        $('#countries_for_foreign_visa_required').css('display', 'none');
        $('#foregion_visa_quantity_required').css('display', 'none');
    }



    Vacation_Obj.required_travel_insurance = $('#travel_insurance_id :selected').val();


    if (Vacation_Obj.required_travel_insurance == 1) {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        if (Vacation_Obj.travel_insurance_countries == "") {
            $('#travel_insurance_countries_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_countries_required').css('display', 'none');
        }
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        if (Vacation_Obj.travel_insurance_quantity == "") {
            $('#travel_insurance_quantity_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#travel_insurance_quantity_required').css('display', 'none');
        }
    }
    else {
        Vacation_Obj.travel_insurance_countries = $('#travel_insurance_countries_id').val();
        Vacation_Obj.travel_insurance_quantity = $('#travel_insurance_quantity_id').val();
        $('#travel_insurance_countries_required').css('display', 'none');
        $('#travel_insurance_quantity_required').css('display', 'none');
    }

    //Vacation_Obj.required_rent_car = $('#rent_car_id :selected').val();

    //if (Vacation_Obj.required_rent_car == 1) {

    //    Vacation_Obj.rent_car_charged_to = $('#rent_car_charged_to_id').val();

    //    if (Vacation_Obj.rent_car_charged_to == "Project") {
    //        Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
    //        if (Vacation_Obj.rent_car_project_no == "") {
    //            $('#rent_car_project_no_required').css('display', 'block');
    //            $('#submit_request_btn').prop('disabled', false);
    //            validation = false;
    //        }
    //        else {
    //            $('#rent_car_project_no_required').css('display', 'none');
    //        }
    //    }
    //    else {
    //        Vacation_Obj.rent_car_project_no = $('#rent_car_project_no_id').val();
    //    }

    //    Vacation_Obj.car_type = $('#ddl_car_type_data :selected').val();
    //    if (Vacation_Obj.car_type == "") {
    //        $('#car_type_data_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_type_data_required').css('display', 'none');
    //    }

    //    Vacation_Obj.rent_car_picked_up_at = $('#car_pick_up_at_id').val();

    //    if (Vacation_Obj.rent_car_picked_up_at == "") {
    //        $('#car_pick_up_at_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_pick_up_at_required').css('display', 'none');
    //    }

    //    Vacation_Obj.rent_car_pick_up_date = $('#car_pick_up_date_id').val();
    //    if (Vacation_Obj.rent_car_pick_up_date == "") {
    //        $('#car_pick_up_date_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_pick_up_date_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_pick_up_time = $('#car_pick_up_time_id').val();
    //    if (Vacation_Obj.rent_car_pick_up_time == "") {
    //        $('#car_pick_up_time_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_pick_up_time_required').css('display', 'none');
    //    }

    //    Vacation_Obj.rent_car_return_date = $('#rent_car_return_date_id').val();
    //    if (Vacation_Obj.rent_car_return_date == "") {
    //        $('#rent_car_return_date_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#rent_car_return_date_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_return_time = $('#rent_car_return_time_id').val();
    //    if (Vacation_Obj.rent_car_return_time == "") {
    //        $('#rent_car_return_time_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#rent_car_return_time_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_payment_type = $('.car_payment_type').val();
    //    if (Vacation_Obj.rent_car_payment_type == "") {
    //        $('#car_payment_type_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#car_payment_type_required').css('display', 'none');
    //    }
    //    Vacation_Obj.rent_car_remark = $('#remark_two_id').val();
    //}


    //Vacation_Obj.required_hotel_booking = $('#hotel_booking_id :selected').val();
    //if (Vacation_Obj.required_hotel_booking == 1) {
    //    Vacation_Obj.hotel_booking_charged_to = $('#hotel_booking_charged_to_id').val();
    //    if (Vacation_Obj.hotel_booking_charged_to == "Project") {
    //        Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
    //        if (Vacation_Obj.hptel_booking_project_no == "") {
    //            $('#hotel_booking_project_no_id_required').css('display', 'block');
    //            $('#submit_request_btn').prop('disabled', false);
    //            validation = false;
    //        }
    //        else {
    //            $('#hotel_booking_project_no_id_required').css('display', 'none');
    //        }
    //    }
    //    else {
    //        Vacation_Obj.hptel_booking_project_no = $('#hotel_booking_project_no_id').val();
    //    }


    //    Vacation_Obj.hotel_name = $('#hotel_name_id').val();
    //    if (Vacation_Obj.hotel_name == "") {
    //        $('#hotel_name_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_name_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_location = $('#hotel_location').val();
    //    if (Vacation_Obj.hotel_location == "") {
    //        $('#hotel_location_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_location_required').css('display', 'none');
    //    }
    //    Vacation_Obj.type_of_room = $('#type_of_rooms_id').val();
    //    Vacation_Obj.room_preferences = $('#room_preferences_id').val();
    //    Vacation_Obj.number_of_rooms = $('#number_of_rooms_id').val();

    //    if (Vacation_Obj.number_of_rooms == "") {
    //        $('#number_of_rooms_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#number_of_rooms_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_booking_payment_mode = $('.hotel_payment_type').val();
    //    Vacation_Obj.hotel_booking_check_in_date = $('#room_check_in_date_id').val();

    //    if (Vacation_Obj.hotel_booking_payment_mode == "") {
    //        $('#hotel_payment_type_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_payment_type_required').css('display', 'none');
    //    }

    //    if (Vacation_Obj.hotel_booking_check_in_date == "") {
    //        $('#room_check_in_date_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_in_date_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_check_in_time = $('#room_check_in_time_id').val();
    //    if (Vacation_Obj.hotel_check_in_time == "") {
    //        $('#room_check_in_time_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_in_time_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_booking_check_out_date = $('#room_check_out_date_id').val();
    //    if (Vacation_Obj.hotel_booking_check_out_date == "") {
    //        $('#room_check_out_date_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_out_date_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_check_out_time = $('#room_check_out_time_id').val();
    //    if (Vacation_Obj.hotel_check_out_time == "") {
    //        $('#room_check_out_time_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#room_check_out_time_id_required').css('display', 'none');
    //    }
    //    Vacation_Obj.hotel_booking_remark = $('#remark_three_id').val();
    //}

    Vacation_Obj.departure_date = $('#departure_date_id').val();
    Vacation_Obj.departure_flight_number = $('#depature_flight_no_id').val();
    Vacation_Obj.return_date = $('#departure_return_date_id').val();
    Vacation_Obj.return_flight_number = $('#return_flight_number_id').val();
    Vacation_Obj.travel_routing = $('#travel_route_id').val();
    Vacation_Obj.type_of_ticket = $('#type_of_ticket_id').val();
    Vacation_Obj.note = $('#note_id').val();


    if (Vacation_Obj.departure_date == "") {
        $('#departure_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.return_date == "") {
        $('#departure_return_date_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#departure_return_date_id_required').css('display', 'none');
    }
    if (Vacation_Obj.travel_routing == "") {
        $('#travel_route_id_required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        validation = false;
    }
    else {
        $('#travel_route_id_required').css('display', 'none');
    }
    if (Vacation_Obj.mode_of_travel == "Air") {
        if (Vacation_Obj.departure_flight_number == "") {
            $('#depature_flight_no_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#depature_flight_no_id_required').css('display', 'none');
        }
        if (Vacation_Obj.return_flight_number == "") {
            $('#return_flight_number_id_required').css('display', 'block');
            $('#submit_request_btn').prop('disabled', false);
            validation = false;
        }
        else {
            $('#return_flight_number_id_required').css('display', 'none');
        }
    }

    //Vacation_Obj.amx_holder = $('#amex_holder_id :selected').val();
    //Vacation_Obj.cash_advance = $('#cash_advance_id :selected').val();
    //Vacation_Obj.salary_advance = $('#salary_advance_id').val();
    //Vacation_Obj.bank_account = $('#bank_account_id').val();
    //Vacation_Obj.ticket_cost = $('#ticket_cost_id').val();
    //Vacation_Obj.hotel_cost = $('#hotel_cost_id').val();
    //Vacation_Obj.daily_allowance = $('#daily_allowance_id').val();
    //Vacation_Obj.other_expenses = $('#other_expense_id').val();
    //Vacation_Obj.travel_advance_remark = $('#remark_four_id').val();
    //Vacation_Obj.travel_advance_total = $('#travel_advance_id').val();
    //Vacation_Obj.iban = $('#iban_id').val();

    //if (Vacation_Obj.cash_advance == 1) {
    //    if (Vacation_Obj.salary_advance == "") {
    //        $('#salary_advance_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#salary_advance_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.bank_account != "") {

    //        if (Vacation_Obj.iban == "") {
    //            $('#iban_id_required').css('display', 'block');
    //            $('#submit_request_btn').prop('disabled', false);
    //            validation = false;
    //        }
    //        else {
    //            $('#iban_id_required').css('display', 'none');
    //        }
    //    }
    //    if (Vacation_Obj.ticket_cost == "") {
    //        $('#ticket_cost_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#ticket_cost_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.hotel_cost == "") {
    //        $('#hotel_cost_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#hotel_cost_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.daily_allowance == "") {
    //        $('#daily_allowance_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#daily_allowance_id_required').css('display', 'none');
    //    }
    //    if (Vacation_Obj.other_expenses == "") {
    //        $('#other_expense_id_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    else {
    //        $('#other_expense_id_required').css('display', 'none');
    //    }
    //}

    ////Number of Days Information
    //Vacation_Obj.ticket_charged_to = $('#dp_Ticket_Charge :selected').val();
    //Vacation_Obj.from_period = $('#txt_from_period').val();
    //Vacation_Obj.to_period = $('#txt_to_period').val();
    //Vacation_Obj.Vacation = $('#txt_Vacation').val();
    //Vacation_Obj.Leave_WO_Pay = $('#txt_Leave').val();
    //Vacation_Obj.Holidays = $('#txt_Holidays').val();
    //Vacation_Obj.Friday = $('#txt_Friday').val();
    //Vacation_Obj.Saturday = $('#txt_Saturday').val();
    //Vacation_Obj.Total_No_of_Days = $('#txt_Total').val();

    //if (Vacation_Obj.ticket_charged_to == 1) {

    //    if (Vacation_Obj.from_period == "") {
    //        $('#spn_From_Period_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //    if (Vacation_Obj.to_period == "") {
    //        $('#spn_To_Period_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    ////Employee entitlement to be processed by SAS

    //Vacation_Obj.HR_Remarks_Visa = $('#div_Visa_Remarks').text().trim();
    //Vacation_Obj.HR_Remarks_Taxi = $('#div_Taxi_Remarks').text().trim();


    //if ($("input[name='Visa_Amount']:checked").val() == "Yes") {
    //    Vacation_Obj.Visa_Amount_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Visa_Amount_Claim = "False";
    //}

    //if ($("input[name='Taxi_Fare']:checked").val() == "Yes") {
    //    Vacation_Obj.Taxi_Fare_Claim = "True";
    //}
    //else {
    //    Vacation_Obj.Taxi_Fare_Claim = "False";
    //}

    //if (Vacation_Obj.Visa_Amount_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Visa_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //if (Vacation_Obj.Taxi_Fare_Claim == "True") {
    //    if (Vacation_Obj.HR_Remarks_Visa == "") {
    //        $('#spn_Taxi_Remarks_required').css('display', 'block');
    //        $('#submit_request_btn').prop('disabled', false);
    //        validation = false;
    //    }
    //}

    //Dependents Information
    Vacation_Obj.Dependentsinfo = [];
    $(".dependents_details").each(function () {
        var data = new Object();
        var currentRow = $(this).closest("tr");
        data.name = currentRow.find("td:eq(0)").text();
        data.relation_ship = currentRow.find("td:eq(1)").text();
        data.age = currentRow.find("td:eq(2)").text();
        data.visa_type = currentRow.find("td:eq(3)").val();
        data.ta_type = currentRow.find("td:eq(4)").text();
        data.remarks = currentRow.find("td:eq(5)").text();
        Vacation_Obj.Dependentsinfo.push(data);
    });

    $(".dependents_name").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_name != '' && Vacation_Obj.dependents_name != null) {
            Vacation_Obj.dependents_name = Vacation_Obj.dependents_name + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_name = xx;

        }
    });
    $(".dependents_relationship").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_relation != '' && Vacation_Obj.dependents_relation != null) {
            Vacation_Obj.dependents_relation = Vacation_Obj.dependents_relation + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_relation = xx;
        }
    });
    $(".dependents_age").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_age != '' && Vacation_Obj.dependents_age != null) {
            Vacation_Obj.dependents_age = Vacation_Obj.dependents_age + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_age = xx;
        }
    });
    $(".dependents_visa_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_visa_type != '' && Vacation_Obj.dependents_visa_type != null) {
            Vacation_Obj.dependents_visa_type = Vacation_Obj.dependents_visa_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_visa_type = xx;
        }
    });
    $(".dependents_ta_type").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_ta_type != '' && Vacation_Obj.dependents_ta_type != null) {
            Vacation_Obj.dependents_ta_type = Vacation_Obj.dependents_ta_type + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_ta_type = xx;
        }
    });
    $(".dependents_remarks").each(function () {
        var xx = $(this).val();
        if (Vacation_Obj.dependents_remarks != '' && Vacation_Obj.dependents_remarks != null) {
            Vacation_Obj.dependents_remarks = Vacation_Obj.dependents_remarks + '~' + xx;
        }
        else {
            Vacation_Obj.dependents_remarks = xx;
        }
    });

    if (Vacation_Obj.dependents_name != undefined) {
        var depname = Vacation_Obj.dependents_name.split("~");
        var age = Vacation_Obj.dependents_age.split("~");
        var relation = Vacation_Obj.dependents_relation.split("~");
        for (i = 0; i < depname.length; i++) {
            if (depname[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }
            }
            else if (age[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else if (relation[i] != "") {
                if (depname[i] != "" && age[i] != "" && relation[i] != "") {
                    dependevalidation = true;
                }
                else {
                    dependevalidation = false;
                    break;
                }

            }
            else {
                if (dependevalidation == false) {

                }
                else {
                    dependevalidation = true;
                }
            }
        }
    }

    ////Travel Agency Information
    //Vacation_Obj.employee_ticket_number = $('#employee_ticket_number_id').val();
    //Vacation_Obj.employee_date_of_issue = $('#date_of_issue_id').val();
    //Vacation_Obj.employee_ticket_price = $('#ticket_price_id').val();

    //Vacation_Obj.revalidation_charge = $('#revalution_charge_id').val();
    //Vacation_Obj.total_ticket_price = $('#total_ticket_price_id').val();
    //Vacation_Obj.over_all_ticket_status = $('#ddl_Ticket_Status :selected').val();

    //$(".depe_ticket_number").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_number != '' && Vacation_Obj.depent_ticket_number != null) {
    //        Vacation_Obj.depent_ticket_number = Vacation_Obj.depent_ticket_number + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_number = xx;

    //    }
    //});
    //$(".depe_ticket_date_of_issue ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_issue_date != '' && Vacation_Obj.depent_issue_date != null) {
    //        Vacation_Obj.depent_issue_date = Vacation_Obj.depent_issue_date + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_issue_date = xx;

    //    }
    //});

    //$(".depe_ticket_price  ").each(function () {
    //    var xx = $(this).val();
    //    if (Vacation_Obj.depent_ticket_price != '' && Vacation_Obj.depent_ticket_price != null) {
    //        Vacation_Obj.depent_ticket_price = Vacation_Obj.depent_ticket_price + '~' + xx;
    //    }
    //    else {
    //        Vacation_Obj.depent_ticket_price = xx;

    //    }
    //});

    //var depe_ticket_number = Vacation_Obj.depent_ticket_number.split("~");
    //var depe_ticket_date_of_issue = Vacation_Obj.depent_issue_date.split("~");
    //var depe_ticket_price = Vacation_Obj.depent_ticket_price.split("~");

    //for (i = 0; i < depe_ticket_number.length; i++) {
    //    if (depe_ticket_number[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }
    //    }
    //    else if (depe_ticket_date_of_issue[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else if (depe_ticket_price[i] != "") {
    //        if (depe_ticket_number[i] != "" && depe_ticket_date_of_issue[i] != "" && depe_ticket_price[i] != "") {
    //            TravelAgencyValidation = true;
    //        }
    //        else {
    //            TravelAgencyValidation = false;
    //            break;
    //        }

    //    }
    //    else {
    //        if (TravelAgencyValidation == false) {

    //        }
    //        else {
    //            TravelAgencyValidation = true;
    //        }
    //    }
    //}    


    if (validation == false) {
        toastrError("Please fill all the mandatory fields.");
        return;
    }
    else {
        //if (Vacation_Obj.last_day_of_work != "" && Vacation_Obj.return_to_duty != "") {
        //    if (Date.parse(Vacation_Obj.last_day_of_work) > Date.parse(Vacation_Obj.return_to_duty)) {
        //        toastrError("Return to Duty Date should not be greater than Last Day of Work.");
        //        $('#last_day_of_work_required').css('display', 'block');
        //        return;
        //    }
        //    else {
        //        $('#last_day_of_work_required').css('display', 'none');
        //    }
        //}
        //if (Vacation_Obj.rent_car_pick_up_date != "" && Vacation_Obj.rent_car_return_date != "") {
        //    if (Date.parse(Vacation_Obj.rent_car_pick_up_date) > Date.parse(Vacation_Obj.rent_car_return_date)) {
        //        toastrError("Pick up Date should not be greater than Return Date.");
        //        $('#car_pick_up_date_required').css('display', 'block');
        //        return;
        //    }
        //    else {
        //        $('#car_pick_up_date_required').css('display', 'none');
        //    }
        //}
        //if (Vacation_Obj.hotel_booking_check_in_date != "" && Vacation_Obj.hotel_booking_check_out_date != "") {
        //    if (Date.parse(Vacation_Obj.hotel_booking_check_in_date) >= Date.parse(Vacation_Obj.hotel_booking_check_out_date)) {
        //        toastrError("Check in Date should not be greater than Check out Date.");
        //        $('#room_check_in_date_id_required').css('display', 'block');
        //        return;
        //    }
        //    else {
        //        $('#room_check_in_date_id_required').css('display', 'none');
        //    }
        //}

        if (Vacation_Obj.departure_date != "" && Vacation_Obj.return_date != "") {
            if (Date.parse(Vacation_Obj.departure_date) >= Date.parse(Vacation_Obj.return_date)) {
                toastrError("Departure Date should not be greater than Return Date.");
                $('#departure_date_id_required').css('display', 'block');
                return;
            }
            else {
                $('#departure_date_id_required').css('display', 'none');
            }
        }

        if (Vacation_Obj.from_period != "" && Vacation_Obj.to_period != "") {
            if (Date.parse(Vacation_Obj.from_period) > Date.parse(Vacation_Obj.to_period)) {
                toastrError("Ticket utilize for the period from should not be greater than to date.");
                $('#spn_From_Period_required').css('display', 'block');
                return;
            }
            else {
                $('#spn_From_Period_required').css('display', 'none');
            }
        }
    }

    if (dependevalidation == false) {
        toastrError("Please fill all the dependent information.");
        return;
    }

    //if (TravelAgencyValidation == false) {
    //    toastrError("Please fill all the travel agency information");
    //    return;
    //}

    if (validation == true && dependevalidation == true) {
        obj.VacationModel = Vacation_Obj;
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_Edit_TA_DependentsOnly",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {
                if (response.Status) {
                    $(".se-pre-con").hide();
                    requestId = $('#hdn_request_id').val();
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
                    requestId = $('#hdn_request_id').val();
                }
            },
        });
    }
}