function setButtonStatus(num) {
    for (var i = 1; i < 9; i++) {                   
        if (i == parseInt(num)) {
            $('#profileType' + i).attr("class", "btn btn-block btn-primary btn-sm");          
        }
        else {
            $('#profileType' + i).attr("class", "btn btn-block btn-default btn-sm");            
        }       
    }
   
}

function doDownloadReportCandidate() {
    var siteroot = window.location.origin;

    console.log("asdscs", siteroot);
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var url = siteroot + '/Report/DownloadCandidateReport?proj=' + proj + '&appVac=' + vacc;
    var win = window.open(url, '_blank');
    win.focus();
}
function doDownload() {
    var siteroot = window.location.origin;
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var vaccgroup = $('#vacgroup').val();
    var region = $('#region').val();
    var city = $('#city').val();
    var sex = $('#sex').val();
    var age = $('#age').val();
    var height = $('#height').val();
    var edu = $('#edu').val();
    var status = $('#status').val();
    var rangedate = $('#RangeSchedule').val();
    var profileType = $('#profileType').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_SCREEN?_project=' + proj + '&_vacancy=' + vacc + '&_vacgroup=' + vaccgroup + '&_region=' + region + '&_city=' + city + '&_status=' + status + '&RangeDate=' + rangedate + '&_sex=' + sex + '&_edu=' + edu + '&_profileType=' + profileType + '&_age_from=' + $('#age_from').val() + '&_age_to=' + $('#age_to').val() + '&_height_from=' + $('#_height_from').val() + '&_height_to=' + $('#height_to').val();
    var win = window.open(url, '_blank');
    win.focus();
}

function doProducInforDownload() {
    var siteroot = window.location.origin;
    var account = $('#accountid').val();
    var province = $('#province').val();
    var rangedate = $('#RangeSchedule').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_PRODUCTINFOR?_province=' + province + '&_account=' + account + '&_rangedate=' + rangedate;
    var win = window.open(url, '_blank');
    win.focus();
}


function doRoundDownload() {
    var siteroot = window.location.origin;
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var vaccgroup = $('#vacgroup').val();
    var region = $('#region').val();
    var city = $('#city').val();
    var score = $('#score').val();
    var round = $('#round').val();
    var rangedate = $('#RangeSchedule').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_ROUND?_project=' + proj + '&_vacancy=' + vacc + '&_round=' + round + '&_vacgroup=' + vaccgroup + '&_region=' + region + '&_city=' + city + '&_score=' + score + '&RangeDate=' + rangedate;
    var win = window.open(url, '_blank');
    win.focus();
}

function doJobDownload() {   
    var siteroot = window.location.origin;
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var vaccgroup = $('#vacgroup').val();
    var region = $('#region').val();
    var city = $('#city').val();
    var status = $('#status').val();    
    var rangedate = $('#RangeSchedule').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_JOB_ACCEPT?_project=' + proj + '&_vacancy=' + vacc + '&_vacgroup=' + vaccgroup + '&_region=' + region + '&_city=' + city + '&_status=' + status + '&RangeDate=' + rangedate;
    var win = window.open(url, '_blank');
    win.focus();
}

function doProjectDownload() {

    var siteroot = window.location.origin;
    var url = siteroot + '//Resource/EX_DOWNLOAD_NEW_PROJECTS?projectid=' + $('#Projects').val() + '&position=' + $('#Vacancies').val() + '&region=' + $('#CityRegionS').val() + '&status=' + $('#Status').val();    
    var win = window.open(url, '_blank');
    win.focus();
}

function doDownloadMailSendJobOffer() {
    var siteroot = window.location.origin;
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var type = $('#profileType').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_MAILSENDJOBOFFER?proj=' + proj + '&appVac=' + vacc + '&appStatus=' + type;
    var win = window.open(url, '_blank');
    win.focus();
}

function doDownloadMailSendInterview() {
    var siteroot = window.location.origin;
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var type = $('#profileType').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_MAILSENDINTERVIEW?proj=' + proj + '&appVac=' + vacc + '&appStatus=' + type;
    var win = window.open(url, '_blank');
    win.focus();
}

function doDownloadDone() {
    var siteroot = window.location.origin;
    var proj = $('#project').val();
    var vacc = $('#vacancy').val();
    var type = $('#profileType').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_DONE?proj=' + proj + '&appVac=' + vacc + '&appStatus=' + type;
    var win = window.open(url, '_blank');
    win.focus();
}
function doDownloadProjectNew() {
    var siteroot = window.location.origin;
    var pro = $('#Project').val();
    var reg = $('#regionselect').val();
    var cit = $('#cityselect').val();
    var sto = $('#storeselect').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_PROJECTS_New?project=' + pro + '&region=' + reg + '&city=' + cit + '&store=' + sto;
    var win = window.open(url, '_blank');
    win.focus();
}
function doDownloadProjectReplace() {
    var siteroot = window.location.origin;
    var pro = $('#Project').val();
    var reg = $('#regionselect').val();
    var cit = $('#cityselect').val();
    var sto = $('#storeselect').val();
    var url = siteroot + '//Resource/EX_DOWNLOAD_PROJECTS_Replace?project=' + pro + '&region=' + reg + '&city=' + cit + '&store=' + sto;
    var win = window.open(url, '_blank');
    win.focus();
}
function updateView(num) {
    $('#profileType').val(num - 1);
    $('#selPage').val(1);
    setButtonStatus(num);
    $('#doFilter').submit(); 
}
function updatePage(num) {
    $('#selPage').val(num);    
    $('#doFilter').submit(); 
}
function updateSize() {
    $('#selPage').val(1);    
    doCommit();
}
function doCommit() {
    $('#doFilter').submit();
}
function viewCandidate(candidateID) {
    $('#candidateIDView').val(candidateID);
    $("#candidateViewSubmit").submit();
    $("#viewInfoCandidate").modal("show");      
}
function showDetail() {   
    $("#viewInfoCandidate").modal("show");
}
function loadingFailed() {
    showMsgBoxSuccess("Lỗi không tải được dữ liệu hệ thống. Vui lòng thử lại!");
}
function trashNote(id) {
    $('#candidateTrash').val(id);    
    $('#trashSubmit').submit();
}
function trashFailed() {
    showMsgBoxSuccess("Xóa hồ sơ thất bại. Kết nối tới máy chủ không thành công.");
}
function RemoveItem(id) {
    $('#container_cand_' + id).remove();
}
function trashOK() {
    RemoveItem($('#candidateTrash').val());;
    showMsgBoxSuccess("Đã xóa hồ sơ thành công.");
}

function trashRestore(id) {
    $('#candidateRestore').val(id);
    $('#restoreSubmit').submit();    
}
function trashRestoreFailed() {
    showMsgBoxSuccess("Phục hồi hồ sơ thất bại. Kết nối tới máy chủ không thành công.");
}
function trashRestoreOK() {
    RemoveItem($('#candidateRestore').val());;
    showMsgBoxSuccess("Đã phục hồi hồ sơ thành công.");
}
function trashAll(id) {
    $('#candidateDelAll').val(id);    
    $('#delSubmit').submit();
}
function moveProject(id) {
    $('#candidateMoveID').val(id);
    $("#moveProjectPanel").modal("show");   
}

function moveFailed() {
    showMsgBoxSuccess("Chuyển dự án thất bại. Kết nối tới máy chủ không thành công.");
}
function moveOK() {
    RemoveItem($('#candidateMoveID').val());;
    showMsgBoxSuccess("Đã chuyển dự án thành công.");
    $("#moveProjectPanel").modal("hide");   
}

function approveProject(id) {
    $('#candidateAppID').val(id);
    $('#approveNote').val('');
    $('input[name="rating"]').prop('checked', false);
    $("#approvePanel").modal("show");
}

function approveFailed() {
    showMsgBoxSuccess("Duyệt hồ sơ thất bại. Kết nối tới máy chủ không thành công.");
}
function approveOK() {
    RemoveItem($('#candidateAppID').val());
    $("#approvePanel").modal("hide");
    showMsgBoxSuccess("Duyệt hồ sơ thành công.");
}
function performAllTask() {
    //build all id and action id_all,id_action
    $('#id_action').val($('#form_action').val());
    if ($('#form_action').val() == '0') {
        showMsgBoxSuccess("Vui lòng chọn đúng thao tác.");
        return;
    }
    //build ids
    var all = $('.ez_checked');
    var values = '';
    for (var i = 0; i < all.length; i++) {
        if ($(all[i]).is(":checked")) {
            values += $(all[i]).attr('data-ok')+',';
        }
    }
    if (values.length == 0) {
        showMsgBoxSuccess("Vui lòng chọn ứng viên.");
        return;
    }
    if ($('#form_action').val()=='2') {
        approveProject(values);
        return;
    }
    //console.log(values);
    $('#id_all').val(values);
    $('#performSubmit').submit();
}

function smallTask(id_action,candidateid) {
    //build all id and action id_all,id_action
    $('#id_action').val(id_action);
    //console.log(values);
    $('#id_all').val(candidateid);
    $('#performSubmit').submit();
}


function performOK() {
    showMsgBoxSuccess("Thực hiện thao tác thành công.");
    $("#approvePanel").modal("hide");
    $('#doFilter').submit();
}
function performFailedOK() {    
    showMsgBoxSuccess("Thao tác thất bại.");
}

function loadCandidateStore(poseddata, candidateID) {
    var html = '<table id="selStoresList" class="table table-bordered table-hover"><thead><tr><th>Mã địa điểm</th><th>Tên địa điểm</th><th>Loại</th><th>Quận/ huyện</th><th>Chỉ tiêu</th><th>Còn lại</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
    for (var i = 0; i < poseddata.length; i++) {
        var _data = poseddata[i];
        html += '<tr><td>' + _data.Code + '</td>';
        html += '<td>' + _data.Name + '</td>';
        html += '<td>' + _data.Type + '</td>';
        html += '<td>' + _data.District + '</td>';
        html += '<td>' + _data.Num + '</td>';
        html += '<td>' + _data.Avail + '</td>';
        if (_data.Avail > 0) {
            html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickAProject(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.Name + '"><i class="fas fa-arrow-right"></i></button></td>';
        }
        else {
            html += '<td><b>Đã đủ</b></td>';
        }
        html += '</tr>';
    }

    html += '</tbody></table>';

    $('#selStoresPanelContent').html(html);
    $('#selStoresList').DataTable({
        "paging": true,
        "lengthChange": true,
        "searching": true,
        "ordering": true,
        "info": false,
        "autoWidth": true
    });
}
function pickAProject(control) {
    var ctl = $(control);
    var html = '<option value="' + ctl.attr('data-root') + '">' + ctl.attr('data-val') + '</option>';
    $('#ChoosenStore').empty();
    $('#ChoosenStore').append(html);
    $('#selStoresPanel').modal('hide');
}