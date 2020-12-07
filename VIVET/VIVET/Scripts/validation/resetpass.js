function validateSubmission() {
    var _pass1 = $('#NewPassword');
    var _pass2 = $('#ConfirmedPassword');

    var message = '';
    var allowPosting = true;
    if (_pass1.val() != _pass2.val()) {
        message += "- Mật khẩu mới và xác nhận mật khẩu mới không trùng nhau <br/>";
        allowPosting = false;
    }
    if (_pass1.val() == "" || _pass1.val() == null || _pass2.val() == "" || _pass2.val() == null) {
        message += "Vui lòng không bỏ trống cả mật khẩu mới và xác nhận mật khẩu mới. </br>";
        allowPosting = false;
    }           
    
    if (!allowPosting) {
        showMsgBox(message);
    }
    return allowPosting;
}

function loadPWPanel(userID) {
    $('#userid').val(userID);
    $('#resetPWD').modal('show');
}
function passWDSuccess() {
    $('#resetPWD').modal('hide');
    $('#divMessage').val('');     
    showMsgBoxSuccess("Đã đổi mật khẩu thành công!");
}
function passWDFailed() {    
    $('#divMessage').val('');
    showMsgBoxSuccess("Có lỗi hệ thống khi thay đổi mật khẩu!");
}