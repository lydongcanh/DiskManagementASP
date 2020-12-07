function loadReceivedProfile(urlTarget, mailContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tên mẫu mail</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.TemplateName + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickReceivedProfile(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.TemplateName + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + mailContent).html(html);
            $('#' + gridContainer).DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true
            });

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}
function loadInterviewInvited(urlTarget, mailContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tên mẫu mail</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.TemplateName + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickInterviewInvited(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.TemplateName + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + mailContent).html(html);
            $('#' + gridContainer).DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true
            });

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}
function loadJobOffer(urlTarget, mailContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tên mẫu mail</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.TemplateName + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickJobOffer(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.TemplateName + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + mailContent).html(html);
            $('#' + gridContainer).DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true
            });

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}
function loadReject(urlTarget, mailContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tên mẫu mail</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.TemplateName + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickReject(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.TemplateName + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + mailContent).html(html);
            $('#' + gridContainer).DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true
            });

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}
function loadWaitRecruited(urlTarget, mailContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tên mẫu mail</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.TemplateName + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickWaitRecruited(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.TemplateName + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + mailContent).html(html);
            $('#' + gridContainer).DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true
            });

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}

