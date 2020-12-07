function loadProjects(urlTarget, projectContent, gridContainer) {   
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async:true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>ID</th><th>Tên dự án</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.id + '</td><td>' + _data.value + '</td>';
                html += '<td><input class="btn bg-gradient-success btn-sm" value=">>" onclick="pickOne(this);" type="button"' + '" data-root="' + _data.key + '"' + '" data-val="' + _data.value + '"';
                html += '></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + projectContent).html(html);
            $('#' + gridContainer).DataTable({
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": false,
                "autoWidth": true,
            });

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}
function loadMailConfig(urlTarget, mailconfigContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Email gửi</th><th>Email CC</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.EmailSend + '</td>';
                html += '<td>' + _data.EmailCC + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickMailConfig(this);" type="button"' + '" data-root="' + _data.Id + '"' + '" data-val="' + _data.EmailSend + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + mailconfigContent).html(html);
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