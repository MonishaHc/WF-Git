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
        if (obj.wf_id == 'T006') {
            Save_T006();
        }

        if (obj.wf_id == 'T007') {
            Save_T007();
        }
        else if (obj.wf_id == 'T004') // Save For T004 In-House-Training By George on 07-07-2020
        {
            Save_T004();
        }
        else if (obj.wf_id == 'T001') // Save For T001 External Training By George on 13-07-2020
        {
            Save_T001();
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
    $(".se-pre-con").show();//George on 13-07-2020
    //EditRequest();
    $.ajax({
        url: '/Request/SubmitRequestForApprove?id=' + id,
        type: "GET",
        success: function (result) {
            if (result.Status) {

                $('#forward_request_btn').css('display', 'block');
                
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

                //T004 disable everything after submit
                $("#tbl_Course").find("input,button,textarea,select").attr("disabled", "disabled");
                $('#TableError').hide();
                $('.SelectError').hide();

                //T001 disable everything after submit
                $("#tbl_dependent").find("input,button,textarea,select").attr("disabled", "disabled");
                $('#justification').attr("disabled", "disabled");
                $('#NoofYears').attr("disabled", "disabled");
                $('.CourseError').hide();
                $('#TableError').hide();
                $('#JustfyError').hide();

                $(".se-pre-con").hide();//George on 13-07-2020
                toastrSuccess('Your request successfully submitted');;//George on 13-07-2020
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
    if (obj.wf_id == 'T006') {
        Edit_T006();
    }
    if (obj.wf_id == 'T007') {
        Edit_T007();
    }
        // Savechanges for T004 In-House Training By George on 07-07-2020
    else if (obj.wf_id == 'T004') {
        Edit_T004();
    }
        // Savechanges for T001 External Training By George on 13-07-2020
    else if (obj.wf_id == 'T001') {
        Edit_T001();
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
    $(".se-pre-con").show();//George on 13-07-2020
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
                $(".se-pre-con").hide();//George on 13-07-2020
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


// Saving  T004 by George 07-07-2020
function Save_T004() {

    var obj = new Object();
    var Training_Obj = new Object();
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

    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.RequestId = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    //To save records  in master table//
    Training_Obj.Remarks = $('.Training_Remarks').val();
    obj.InHouseTrainingModel = Training_Obj;

    var rowCount = $('#tbl_Course tbody tr').length;

    if (rowCount == 0) {
        $('#TableError').html("Please Select Atleast one Course for training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#TableError').hide();
    }
    var hasdata = 0;
    var CourseDetails = new Array();
    $('#tbl_Course tbody tr').each(function () {
        var row = $(this);
        var tdObject = $(this).find('td:eq(1)'); //locate the <td> holding select;
        var selectObject = tdObject.find("select"); //grab the <select> tag assuming that there will be only single select box within that <td>
        var selectval = selectObject.val();
        if (selectval != "0") {
            hasdata = 1;
            var EachCourseobject = {};
            EachCourseobject.Code = row.find('#Code').val();
            EachCourseobject.Course_Name = row.find('#CourseName').val();
            EachCourseobject.Type = row.find('#Type').val();
            EachCourseobject.From_Date = row.find('#FromDate').val();
            EachCourseobject.To_Date = row.find('#ToDate').val();
            CourseDetails.push(EachCourseobject);
        }

    });
    //debugger;
    if (CourseDetails.length === 0) {
    }
    else {
        CourseDetails = DeleteDuplicate(CourseDetails);
    }



    if (hasdata == 1) {
        x = false;
        $('#TableError').hide();
    }
    else {
        $('#TableError').html("Please Select Atleast one Course for training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    //alert(CourseDetails);
    obj._CourseInformation = CourseDetails;

    if (x == false) {
        $(".se-pre-con").show();

        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_InHouseTraining",
            dataType: "json",
            global: false,
            data: obj,
            success: function (response) {

                if (response.Status) {
                    debugger;
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
                    $('#TableError').hide();
                    $('.SelectError').hide();
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
function Edit_T004() {
    debugger;
    var obj = new Object();
    var Training_Obj = new Object();
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

    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.request_id = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    //To save records  in master table//
    Training_Obj.Remarks = $('.Training_Remarks').val();
    obj.InHouseTrainingModel = Training_Obj;

    var rowCount = $('#tbl_Course tbody tr').length;

    if (rowCount == 0) {
        $('#TableError').html("Please Select Atleast one Course for training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#TableError').hide();
    }


    var hasdata = 0;
    var CourseDetails = new Array();
    $('#tbl_Course tbody tr').each(function () {
        var row = $(this);
        var tdObject = $(this).find('td:eq(1)'); //locate the <td> holding select;
        var selectObject = tdObject.find("select"); //grab the <select> tag assuming that there will be only single select box within that <td>
        var selectval = selectObject.val();
        if (selectval != "0") {
            hasdata = 1;
            var EachCourseobject = {};
            EachCourseobject.Code = row.find('#Code').val();
            EachCourseobject.Course_Name = row.find('#CourseName').val();
            EachCourseobject.Type = row.find('#Type').val();
            EachCourseobject.From_Date = row.find('#FromDate').val();
            EachCourseobject.To_Date = row.find('#ToDate').val();
            CourseDetails.push(EachCourseobject);
        }
    });

    if (CourseDetails.length === 0) {
    }
    else {
        CourseDetails = DeleteDuplicate(CourseDetails);
    }
    if (hasdata == 1) {
        x = false;
        $('#TableError').hide();
    }
    else {
        $('#TableError').html("Please Select Atleast one Course for training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    //alert(CourseDetails);
    obj._CourseInformation = CourseDetails;
    if (x == false) {

        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_InHouseTraining_Edit_After_Save",
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
                    $('#TableError').hide();
                    $('.SelectError').hide();
                }
                else {
                    $(".se-pre-con").hide();
                }
            }
        });

    }
    else {
        $('#submit_request_btn').prop('disabled', false);
    }
}
// End of Saving T004 by George 07-07-2020

// Saving  T001 by George 07-07-2020
function Save_T001() {
    debugger;
    var obj = new Object();
    var Training_Obj = new Object();
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

    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.RequestId = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    //To save records  in base table Justification,Years//
    var justify = $('#justification').val();
    if (justify.trim() == '') {
        x = true;
        toastrError("Please Enter Detailed justification!!");
        $('#JustfyError').show();
        $('#submit_request_btn').prop('disabled', false);
        return;
    }
    else {
        x = false;
        $('#JustfyError').hide();
    }
    var NoofYears = $('#NoofYears').val();
    Training_Obj.Justification = justify;
    Training_Obj.Years = NoofYears;
    Training_Obj.GrandTotal = $('#txt_Grand_Total').val();
    obj.ExternalTrainingModel = Training_Obj;

    var rowCount = $('#tbl_dependent tbody tr').length;

    if (rowCount == 0) {
        $('#TableError').html("Please Select Atleast one Course for Training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#TableError').hide();
    }


    var hasdata = 0;
    var CourseDetails = new Array();
    $('#tbl_dependent tbody tr').each(function () {
        var row = $(this);
        var tdObject = $(this).find('td:eq(1)'); //locate the <td> holding select;
        var selectObject = tdObject.find('.GetExternalCourse-list-drop-class option:selected'); //grab the <select> tag assuming that there will be only single select box within that <td>
        var selectval = selectObject.val();

        if (selectval != '') {
            var selecttext = selectObject.text();
            hasdata = 1;
            var EachCourseobject = {};
            EachCourseobject.Course_Name = selecttext;
            EachCourseobject.From_Date = row.find('#datefrom_').val();
            EachCourseobject.To_Date = row.find('#dateto_').val();
            EachCourseobject.Training_Type = row.find('#trainingtypeid_').val();
            EachCourseobject.Location = row.find('#locationid_').val();
            EachCourseobject.Cost = row.find('#cost_').val();
            EachCourseobject.NoofDays = row.find('#NoofDays').val();
            CourseDetails.push(EachCourseobject);
        }
    });
    //debugger;
    if (CourseDetails.length === 0) {
    }
    else {
        CourseDetails = DeleteDuplicate(CourseDetails);
    }



    if (hasdata == 1) {
        x = false;
        $('#TableError').hide();
    }
    else {
        $('#TableError').html("Please Select Atleast one Course for training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }

    obj._TrainingDetails = CourseDetails;

    if (x == false) {
        $(".se-pre-con").show();

        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_ExternalTraining",
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
                    $('.CourseError').hide();
                    $('#TableError').hide();
                    $('#JustfyError').hide();
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
function Edit_T001() {
    debugger;
    var obj = new Object();
    var Training_Obj = new Object();
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

    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.request_id = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;
    //To save records  in master table//
    var justify = $('#justification').val();
    if (justify.trim() == '') {
        x = true;
        toastrError("Please Enter Detailed justification!!");
        $('#JustfyError').show();
        $('#submit_request_btn').prop('disabled', false);
        return;
    }
    else {
        x = false;
        $('#JustfyError').hide();
    }
    var NoofYears = $('#NoofYears').val();
    Training_Obj.Justification = justify;
    Training_Obj.Years = NoofYears;
    Training_Obj.GrandTotal = $('#txt_Grand_Total').val();

    obj.ExternalTrainingModel = Training_Obj;

    var rowCount = $('#tbl_dependent tbody tr').length;

    if (rowCount == 0) {
        $('#TableError').html("Please Select Atleast one Course for Training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }
    else {
        x = false;
        $('#TableError').hide();
    }


    var hasdata = 0;
    var CourseDetails = new Array();
    $('#tbl_dependent tbody tr').each(function () {
        var row = $(this);
        var tdObject = $(this).find('td:eq(1)'); //locate the <td> holding select;
        var selectObject = tdObject.find('.GetExternalCourse-list-drop-class option:selected'); //grab the <select> tag assuming that there will be only single select box within that <td>
        var selectval = selectObject.val();

        if (selectval != '') {
            var selecttext = selectObject.text();
            hasdata = 1;
            var EachCourseobject = {};
            EachCourseobject.Course_Name = selecttext;
            EachCourseobject.From_Date = row.find('#datefrom_').val();
            EachCourseobject.To_Date = row.find('#dateto_').val();
            EachCourseobject.Training_Type = row.find('#trainingtypeid_').val();
            EachCourseobject.Location = row.find('#locationid_').val();
            EachCourseobject.Cost = row.find('#cost_').val();
            EachCourseobject.NoofDays = row.find('#NoofDays').val();
            CourseDetails.push(EachCourseobject);
        }
    });
    //debugger;
    if (CourseDetails.length === 0) {
    }
    else {
        CourseDetails = DeleteDuplicate(CourseDetails);
    }



    if (hasdata == 1) {
        x = false;
        $('#TableError').hide();
    }
    else {
        $('#TableError').html("Please Select Atleast one Course for training purpose");
        $('#TableError').show();
        toastrError("Please Select Atleast one Course for training purpose");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        return;
    }

    obj._TrainingDetails = CourseDetails;

    if (x == false) {

        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_ExternalTraining_Edit_After_Save",
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
                    $('.CourseError').hide();
                    $('#TableError').hide();
                    $('#JustfyError').hide();
                }
                else {
                    $(".se-pre-con").hide();
                }
            }
        });

    }
    else {
        $('#submit_request_btn').prop('disabled', false);
    }
}
// End of Saving T001 by George 07-07-2020

function Save_T006() {
    debugger
    var obj = new Object();
    var Training_Obj = new Object();

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

    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.request_id = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    Training_Obj.Date_training = $('.Date_Training').val();
    Training_Obj.quantity = $('.quantity').val();
    
    obj.TrainingFolderModel = Training_Obj;
    ///////////////////Js validations strted here/////////////////////////////
    if (Training_Obj.Date_training == "") {
        $('#Date_Training_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        //return;
    }
    else {
        x = false;
        $('#Date_Training_required').css('display', 'none');
    }
    if (Training_Obj.quantity == 0) {
        $('#quantity-required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        // return;
    }
    else {
        x = false;
        $('#quantity-required').css('display', 'none');

    }

    if (x == true) {
        toastrError("Please fill the Mandatory fields");
    }
    ///////////////////Js validations end here/////////////////////////////
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_TrainingFolder",
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
                    $('#edit_request_btn').css('display', 'none');
                    $('#forward_request_btn').css('display', 'none');

                }

            },

        });
    }
}

function Save_T007() {
    debugger;
    var obj = new Object();
    var Trainingcertificate_Obj = new Object();
    var x = false;
    obj._FileListPrint = [];
    for (let i = 0; i < lists.length; i++) {
        var ax = new Object();
        ax.filename = lists[i].filename;
        ax.filepath = lists[i].filepath;
        ax.filebatch = lists[i].filetype;
        obj._FileListPrint.push(ax);
    }
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.application_ids = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;

    Trainingcertificate_Obj.application_id = $('#application-list-drop-id').val();
    Trainingcertificate_Obj.application_ids = $('#application-list-drop-id').val();
    Trainingcertificate_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Trainingcertificate_Obj.creator_id = $('#emp_identify_id').val();
    Trainingcertificate_Obj.request_id = requestId;
    //Trainingcertificate_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    Trainingcertificate_Obj.Titlecourse = $('.title_course').val();
    Trainingcertificate_Obj.Course_period_from = $('.Period_from').val();
    Trainingcertificate_Obj.Course_period_to = $('.Period_to').val();
    Trainingcertificate_Obj.Location = $('.location').val();
    Trainingcertificate_Obj.Clientname = $('.client').val();
    Trainingcertificate_Obj.Noof_particants = $('.npart').val();
    Trainingcertificate_Obj.Noof_trainer = $('.instructor').val();

    var nameArray = new Array();
    $("input[name=Name_particitants]").each(function () {
        nameArray.push($(this).val());
    });
    Trainingcertificate_Obj.Nameof_participants = nameArray;

    
    obj.TrainingCertificateModel = Trainingcertificate_Obj;

 
    //if (Training_Obj.Date_training == "") {
    //    $('#Date_Training_required').css('display', 'block');
    //    toastrError("Please fill the Mandatory fields");
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    //return;
    //}
    //else {
    //    x = false;
    //    $('#Date_Training_required').css('display', 'none');
    //}
    //if (Training_Obj.quantity == 0) {
    //    $('#quantity-required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    // return;
    //}
    //else {
    //    x = false;
    //    $('#quantity-required').css('display', 'none');

    //}

    //if (x == true) {
    //    toastrError("Please fill the Mandatory fields");
    //}

   
    if (x == false) {
        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_TrainingCertificate",
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
                    $('#edit_request_btn').css('display', 'none');
                    $('#forward_request_btn').css('display', 'none');

                }

            },

        });
    }
}
function Edit_T006() {
    ///////////////////////////////////////////////Parameters section strted here//////////////////////////////////
    var obj = new Object();
    var Training_Obj = new Object();
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
    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.request_id = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    Training_Obj.application_id = $('#application-list-drop-id').val();
    Training_Obj.application_ids = $('#application-list-drop-id').val();
    Training_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Training_Obj.creator_id = $('#emp_identify_id').val();
    Training_Obj.request_id = requestId;
    Training_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    Training_Obj.Date_training = $('.Date_Training').val();
    Training_Obj.quantity = $('.quantity').val();

    obj.TrainingFolderModel = Training_Obj;
 
    ///////////////////////////////////////////////Parameters section end  here//////////////////////////////////
    ///////////////////Js validations strted here/////////////////////////////
    if (Training_Obj.Date_training == "") {
        $('#Date_Training_required').css('display', 'block');
        toastrError("Please fill the Mandatory fields");
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        //return;
    }
    else {
        x = false;
        $('#Date_Training_required').css('display', 'none');
    }
    if (Training_Obj.quantity == 0) {
        $('#quantity-required').css('display', 'block');
        $('#submit_request_btn').prop('disabled', false);
        x = true;
        // return;
    }
    else {
        x = false;
        $('#quantity-required').css('display', 'none');

    }

    if (x == true) {
        toastrError("Please fill the Mandatory fields");
    }



    ///////////////////Js validations end here/////////////////////////////

    if (x == false) {

        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_TrainingFolder_Edit_After_Save",
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

function Edit_T007() {
    ///////////////////////////////////////////////Parameters section strted here//////////////////////////////////
    var obj = new Object();
    var x = false;
    var Trainingcertificate_Obj = new Object();
    obj._FileListPrint = [];
    for (let i = 0; i < lists.length; i++) {
        var ax = new Object();
        ax.filename = lists[i].filename;
        ax.filepath = lists[i].filepath;
        ax.filebatch = lists[i].filetype;
        obj._FileListPrint.push(ax);
    }
    var application_name = $('#application_code_id').val();
    $('#application-required').css('display', 'none');
    obj.wf_id = $('.wf-types-list-drop-id :selected').val();
    obj.application_id = $('#application-list-drop-id').val();
    obj.application_ids = $('#application-list-drop-id').val();
    obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    obj.creator_id = $('#emp_identify_id').val();
    obj.request_id = requestId;
    Trainingcertificate_Obj.application_id = $('#application-list-drop-id').val();
    Trainingcertificate_Obj.application_ids = $('#application-list-drop-id').val();
    Trainingcertificate_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Trainingcertificate_Obj.creator_id = $('#emp_identify_id').val();
    Trainingcertificate_Obj.request_id = requestId;
    Trainingcertificate_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    Trainingcertificate_Obj.application_id = $('#application-list-drop-id').val();
    Trainingcertificate_Obj.application_ids = $('#application-list-drop-id').val();
    Trainingcertificate_Obj.emp_local_id = $('.employee-list-drop-id :selected').val();
    Trainingcertificate_Obj.creator_id = $('#emp_identify_id').val();
    Trainingcertificate_Obj.request_id = requestId;
   // Trainingcertificate_Obj.Attachment_Filepath = afterSaveCommonFilePath;

    Trainingcertificate_Obj.Titlecourse = $('.title_course').val();
    Trainingcertificate_Obj.Course_period_from = $('.Period_from').val();
    Trainingcertificate_Obj.Course_period_to = $('.Period_to').val();
    Trainingcertificate_Obj.Location = $('.location').val();
    Trainingcertificate_Obj.Clientname = $('.client').val();
    Trainingcertificate_Obj.Noof_particants = $('.npart').val();
    Trainingcertificate_Obj.Noof_trainer = $('.instructor').val();

    var nameArray = new Array();
    $("input[name=Name_particitants]").each(function () {
        nameArray.push($(this).val());
    });
    Trainingcertificate_Obj.Nameof_participants = nameArray;


    obj.TrainingCertificateModel = Trainingcertificate_Obj;


    ///////////////////////////////////////////////Parameters section end  here//////////////////////////////////
    ///////////////////Js validations strted here/////////////////////////////
    //if ( Trainingcertificate_Obj.Titlecourse == "") {
    //    $('#title_course-required').css('display', 'block');
    //    toastrError("Please fill the Mandatory fields");
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    //return;
    //}
    //else {
    //    x = false;
    //    $('#title_course-required').css('display', 'none');
    //}
    //if ( Trainingcertificate_Obj.Course_period_from == 0) {
    //    $('#Period_from-required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    // return;
    //}
    //else {
    //    x = false;
    //    $('#Period_from-required').css('display', 'none');

    //}

    //if (x == true) {
    //    toastrError("Please fill the Mandatory fields");
    //}

    //if ( Trainingcertificate_Obj.Course_period_to == "") {
    //    $('#Period_to-required').css('display', 'block');
    //    toastrError("Please fill the Mandatory fields");
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    //return;
    //}
    //else {
    //    x = false;
    //    $('#Period_to-required').css('display', 'none');
    //}
    //if ( Trainingcertificate_Obj.Location == 0) {
    //    $('#location-required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    // return;
    //}
    //else {
    //    x = false;
    //    $('#location-required').css('display', 'none');

    //}
    //if ( Trainingcertificate_Obj.Location == 0) {
    //    $('#location-required').css('display', 'block');
    //    $('#submit_request_btn').prop('disabled', false);
    //    x = true;
    //    // return;
    //}
    //else {
    //    x = false;
    //    $('#location-required').css('display', 'none');

    //}

    //if (x == true) {
    //    toastrError("Please fill the Mandatory fields");
    //}



    ///////////////////Js validations end here/////////////////////////////

    if (x == false) {

        $(".se-pre-con").show();
        $.ajax({
            type: "POST",
            url: "/Request/Submit_TR_TrainingCertificate_Edit_After_Save",
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
    debugger;
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



function DeleteDuplicate(arr) {
    var result = new Array();
    var itemsFound = {};
    for (var i = 0, l = arr.length; i < l; i++) {
        var stringified = JSON.stringify(arr[i]);
        if (itemsFound[stringified]) { continue; }
        result.push(arr[i]);
        itemsFound[stringified] = true;
    }

    return result;

}

