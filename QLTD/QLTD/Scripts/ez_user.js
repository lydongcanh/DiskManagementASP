function loadUser(urlTarget, userContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { psignal: '' },
        async: true,
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tên Team leader</th><th style="width:80px;">Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += '<tr><td>' + _data.value + '</td>';
                html += '<td><button class="btn bg-gradient-success btn-sm" value="" onclick="pickOne(this);" type="button"' + '" data-root="' + _data.key + '"' + '" data-val="' + _data.value + '"';
                html += '><i class="fas fa-arrow-right"></i></button></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + userContent).html(html);
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
