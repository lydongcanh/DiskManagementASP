function showMsgBox(message) {
    var _error_box = $('#validation_msgBox_content');
    if (_error_box.length < 1) {
        //tạo messagebox
        var html ='<div class="modal fade" id="validation_msgBox" role="dialog" aria-hidden="true"><div class="modal-dialog" role="document"><div class="modal-content"><div class="modal-header"><h5 class="modal-title"> Báo lỗi</h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"> <span aria-hidden="true">&times;</span></button></div><div class="card-body" id="validation_msgBox_content"></div><div class="card-footer"><button type="button" class="btn  btn-warning float-right" data-dismiss="modal" aria-label="Close">Đồng ý</button></div></div></div></div >';
        $("body").append(html);
    }
    $('#validation_msgBox_content').html(message);
    $('#validation_msgBox').modal('show'); 
}

function showMsgBoxSuccess(message) {
    var _error_box = $('#validation_sc_msgBox_content');
    if (_error_box.length < 1) {
        //tạo messagebox
        var html = '<div class="modal fade" id="validation_sc_msgBox" role="dialog" aria-hidden="true"><div class="modal-dialog" role="document"><div class="modal-content"><div class="modal-header"><h5 class="modal-title"> Thông báo</h5><button type="button" class="close" data-dismiss="modal" aria-label="Close"> <span aria-hidden="true">&times;</span></button></div><div class="card-body" id="validation_sc_msgBox_content"></div><div class="card-footer"><button type="button" class="btn  btn-primary float-right" data-dismiss="modal" aria-label="Close">Đồng ý</button></div></div></div></div >';
        $("body").append(html);
    }
    $('#validation_sc_msgBox_content').html(message);
    $('#validation_sc_msgBox').modal('show');
}