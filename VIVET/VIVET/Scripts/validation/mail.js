function validateForm() {
    var _templatename = $('#TemplateName');
    var _subject = $('#Subject');
    var _content = $('#compose-textarea');

    var message = '';
    var allowPosting = true;
    if (_templatename.val() == null || _templatename.val() == "" || _templatename.val().lenght == 0) {
        message += "- Bạn phải nhâp tên mẫu mail!<br/>";
        allowPosting = false;
    }
    if (_subject.val() == null || _subject.val() == "" || _subject.val().lenght == 0) {
        message += "- Bạn phải nhâp chủ đề mail!<br/>";
        allowPosting = false;
    }
    if (_content.val() == "" || _content.val() == "                                " || _content.val().lenght == 0) {
        message += "- Bạn phải nhâp nội dung mẫu mail!<br/>";
        allowPosting = false;
    }

    if (!allowPosting) {
        showMsgBox(message);
    }
    return allowPosting;
}

function InsertMailSuccess() {
    $('#divMessage').val('');
    window.history.back();
    showMsgBoxSuccess("Đã thêm mẫu mail " + _templatename.val() + " thành công!");
}
function InsertMailFailed() {
    $('#divMessage').val('');
    showMsgBoxSuccess("Có lỗi hệ thống khi thêm mẫu mail!");
}
function UpdateMailSuccess() {
    $('#divMessage').val('');
    window.history.back();
    showMsgBoxSuccess("Đã cập nhật mẫu mail " + _templatename.val() + " thành công!");
}
function UpdateMailFailed() {
    $('#divMessage').val('');
    showMsgBoxSuccess("Có lỗi hệ thống khi thêm mẫu mail!");
}