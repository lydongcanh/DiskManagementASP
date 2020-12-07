var _optVacancy = false;
var _optSource = false;
var _optGroup = false;
var _optFullname = false;
var _optDOB = false;
var _optSex = false;
var _optHeight = false;
var _optWeight = false;
var _optTel = false;
var _optEmail = false;
var _optPlace = false;
var _optEdu = false;
var _optFacebook = false;
var _optZalo = false;
var _optCity = false;
var _optDistrict = false;
var _optExpectationPlaces = false;
var _optYearExperience = false;
var _optExperience = false;
var _optStartTime = false;
var _optEmployeeName = false;
var _optEmployeeCode = false;
var _optPhoto = false;
var _optCV = false;
var _optAgreement = false;
var _optVacGroup = false;

function form_validate() {
    var message = '';
    var allowPosting = true;
    if (_optVacancy) {
        if ($('#selVacancies').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải chọn vị trí ứng tuyển!<br/>";
        }
    }
    if (_optSource) {
        if ($('#optSource').val() == null || $('#optSource').val().length == 0) {
            allowPosting = false;
            message += "- Bạn cần cho biết bạn biết thông tin tuyển dụng này từ đâu!<br/>";
        }
    }
    if (_optGroup) {
        if ($('#optGroup').val() == null || $('#optGroup').val().length == 0) {
            allowPosting = false;
            message += "- Bạn cần nêu thông tin hội/ nhóm cụ thể!<br/>";
        }
    }
    if (_optFullname) {
        if ($('#optFullname').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải điền tên đầy đủ của bạn!<br/>";
        }
    }
    if (_optDOB) {
        if ($('#optDOB').val() == null || $('#optDOB').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp ngày sinh của bạn!<br/>";
        }
    }
    if (_optSex) {
        if ($("input[name='optSex']:checked").val() == null || $("input[name='optSex']:checked").val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải chọn giới tính!<br/>";
        }
    }
    if (_optHeight) {
        if ($('#optHeight').val() == null || $('#optHeight').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp chiều cao (tính bằng cm)!<br/>";
        }
    }
    if (_optWeight) {
        if ($('#optWeight').val() == null || $('#optWeight').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp cân nặng (tính bằng kg)!<br/>";
        }
    }
    if (_optTel) {
        if ($('#optTel').val() == null || $('#optTel').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp số điện thoại liên hệ!<br/>";
        }
    }
    if (_optEmail) {
        if ($('#optEmail').val() == null || $('#optEmail').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp địa chỉ email!<br/>";
        }
        else {
            if (/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test($('#optEmail').val()) == false) {
                allowPosting = false;
                message += "- Bạn phải nhập đúng địa chỉ email!<br/>";
            }
        }
    }
    if (_optPlace) {
        //validate email
        if ($('#optPlace').val() == null || $('#optPlace').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp địa chỉ nơi ở của bạn!<br/>";
        }
    }
    if (_optEdu) {
        if ($('#optEdu').val() == null || $('#optEdu').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải chọn trình độ học vấn!<br/>";
        }
    }

    if (_optFacebook) {
        if ($('#optFacebook').val() == null || $('#optFacebook').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp địa chỉ facebook của bạn!<br/>";
        }
    }
    if (_optZalo) {
        if ($('#optZalo').val() == null || $('#optZalo').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải cung cấp số điện thoại mà bạn đang dùng Zalo!<br/>";
        }
    }


    if (_optCity) {
        if ($('#selCities').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải chọn tỉnh/thành ứng tuyển!<br/>";
        }
    } 
    if (_optDistrict) {
        if ($('#optDistrict').val() != null && $('#optDistrict').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải chọn nơi bạn muốn làm việc!<br/>";
        }
    } 
    if (_optExpectationPlaces) {
        if ($('#optExpectationPlaces').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải chọn nơi có thể muốn làm việc!<br/>";
        }
    }
    
    if (_optYearExperience) {
        if ($('#optYearExperience').val() == null || $('#optYearExperience').val().length == 0) {
            allowPosting = false;
            message += "- Bạn vui lòng chọn số năm kinh nghiệm!<br/>";
        }
    }
    if (_optExperience) {
        if (($('#optExperience').val() == null || $('#optExperience').val().length == 0) && $('#optYearExperience').val() != 'NONE') {
            allowPosting = false;
            message += "- Bạn phải cung cấp thông tin kinh nghiệm làm việc của bạn!<br/>";
        }
    }
    if (_optVacGroup) {
        if (($('#optVacGroup').val() == null || $('#optVacGroup').val().length == 0) && $('#optYearExperience').val() != 'NONE') {
            allowPosting = false;
            message += "- Bạn phải cung cấp tên ngành hàng mà bạn có kinh nghiệm làm việc!<br/>";
        }
    }
    if (_optStartTime) {
        if ($('#optStartTime').val()==null||$('#optStartTime').val().length == 0) {
            allowPosting = false;
            message += "- Bạn phải ngày bạn có thể bắt đầu làm việc!<br/>";
        }
    }
   
    if (_optEmployeeName) {
        if ($('#optEmployeeName').val()==null||$('#optEmployeeName').val().length == 0) {
            allowPosting = false;
            message += "- Bạn cần cung cấp tên nhân viên giới thiệu!<br/>";
        }
    }
    if (_optEmployeeCode) {
        if ($('#optEmployeeCode').val()==null||$('#optEmployeeCode').val().length == 0) {
            allowPosting = false;
            message += "- Bạn cần cung cấp mã của nhân viên giới thiệu!<br/>";
        }
    }
    
    if (_optPhoto) {
        if ($('#optPhoto').val()==null || $('#optPhoto').val().length == 0) {
            allowPosting = false;
            message += "- Bạn cần chọn các hình chân dung của bạn!<br/>";
        }
    }
    if (_optCV) {
        if ($('#optCV').val()==null||$('#optCV').val().length == 0) {
            allowPosting = false;
            message += "- Bạn cần chọn CV của bạn để gởi cho chúng tôi!<br/>";
        }
    }
    if (_optAgreement) {
        if ($("input[name='optAgreement']:checked").val()==null||$("input[name='optAgreement']:checked").val().length == 0 || $("input[name='optAgreement']:checked").val()=="2") {
            allowPosting = false;
            message += "<b>Bạn phải đồng ý với thỏa thuận của chúng tôi trước khi gởi hồ sơ!</b><br/>";
        }
    }
    if (!allowPosting) {
        showMsgBox(message);
    }
    return allowPosting;
}
function checkDuplication() {
    fetch(window.location.origin + "/EForm/CheckDuplicate?email=" + $('#optEmail').val() +"&phonenumber=" + $('#optTel').val()+"&name=" + $('#optFullname').val())
                .then(res => res.json())
        .then(data => {
            if (data.id == '0') {
                $('#validation_Duplication_content').html('<b>' + data.text + '</b>');
                //show two button
                $('#close_val').show();
                $('#confirm_val').show();
            }
            else if (data.id == '1'){
                //submit the form
                $('#mainform').submit();
            }
        });
}
function finalValidation() {
    if (form_validate()) {
        $('#validation_Duplication_content').html('<img src="' + my_loader_path + '"/><br/><b>Vui lòng chờ...!<b>');
        $('#close_val').hide();
        $('#confirm_val').hide();
        $('#validation_Duplication').modal('show');
        checkDuplication();
    }
}