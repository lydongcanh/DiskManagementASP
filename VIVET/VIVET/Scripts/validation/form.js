function form_validate() {
    var _formname = $('#formName');
    var _proj = $('#proj');

    var message = '';
    var allowPosting = true;
    if (_formname.val().length < 3) {
        message+="- Tên form phải ít nhất 3 ký tự!<br/>";
        allowPosting = false;
    }
    if (_proj.val()==null) {
        message += "- Bạn phải chọn dự án!<br/>";
        allowPosting = false;
    }
    if (!allowPosting) {
        showMsgBox(message);
    }
    return allowPosting;
}
