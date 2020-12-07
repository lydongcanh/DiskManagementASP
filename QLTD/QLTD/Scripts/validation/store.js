function validateForm() {
    var _code = $('#Code');
    var _name = $('#Name');
    var _phone = $('#Phone');
    var _customer = $('#Customer');
    var _street = $('#Street');
    var _ward = $('#Ward');
    var _district = $('#District');
    var _city = $('#City');
    
    var message = '';
    var allowPosting = true;
    if (_code.val() == null || _code.val() == "" || _code.val().lenght == 0) {
        message += "- Bạn chưa nhập mã cửa hàng!<br/>";
        allowPosting = false;
    }
    if (_name.val() == null || _name.val() == "" || _name.val().lenght == 0) {
        message += "- Bạn chưa nhập tên cửa hàng!<br/>";
        allowPosting = false;
    }
    if (_phone.val() == "" || _phone.val() == "" || _phone.val().lenght == 0) {
        message += "- Bạn chưa nhập số điện thoại!<br/>";
        allowPosting = false;
    }
    if (_customer.val() == null || _customer.val() == ""  || _customer.val().lenght == 0) {
        message += "- Bạn phải chọn khách hàng!<br/>";
        allowPosting = false;
    }
    if (_street.val() == null || _street.val() == "" || _street.val().lenght == 0) {
        message += "- Bạn phải nhập địa chỉ!<br/>";
        allowPosting = false;
    }
    if (_ward.val() == null || _ward.val() == "" || _ward.val().lenght == 0) {
        message += "- Bạn chưa nhập Phường!<br/>";
        allowPosting = false;
    }
    if (_district.val() == null || _district.val() == "" || _district.val().lenght == 0) {
        message += "- Bạn chưa nhập Quận/ Huyện!<br/>";
        allowPosting = false;
    }
    if (_city.val() == null || _city.val() == "" || _city.val().lenght == 0) {
        message += "- Bạn phải chọn Tỉnh/ Thành!<br/>";
        allowPosting = false;
    }

    if (!allowPosting) {
        showMsgBox(message);
    }
    return allowPosting;
}

function InsertStoreSuccess() {
    $('#AddStaffModal').modal('hide');
    $('#divMessage').val('');
    showMsgBoxSuccess("Đã thêm cửa hàng " + $('#Code').val() + " thành công!");
}
function InsertStoreFailed() {
    $('#AddStaffModal').modal('hide');
    $('#divMessage').val('');
    showMsgBoxSuccess("Có lỗi hệ thống khi thêm cửa hàng!");
}
function UpdateStoreSuccess() {
    $('#divMessage').val('');
    window.history.back();
    showMsgBoxSuccess("Đã cập nhật cửa hàng " + $('#Code').val() + " thành công!");
}
function UpdateStoreFailed() {
    $('#divMessage').val('');
    showMsgBoxSuccess("Có lỗi hệ thống khi thêm cửa hàng!");
}