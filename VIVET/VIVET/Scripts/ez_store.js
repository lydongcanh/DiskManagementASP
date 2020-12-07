function loadStores(urlTarget, cities, elemContainer, gridContainer, checkedList) {
    var choosen = [];
    if (checkedList !== null)
        choosen = checkedList.split(',');
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { _cities: cities },
        success: function (poseddata) {
            var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th>Tỉnh/ thành</th><th>Tên cửa hàng</th><th>Chọn</th></tr></thead ><tbody>';
            for (var i = 0; i < poseddata.length; i++) {
                var _data = poseddata[i];
                html += `<tr><td>` + _data.CityName + '</td><td>' + _data.StoreName + '</td>';
                html += '<td><input type="checkbox" id="' + gridContainer + '_chk' + i.toString() + '" name="' + gridContainer + '_chk' + i.toString() + '" data-root="' + _data.StoreID + '"';
                if (choosen.indexOf(_data.StoreID.toString()) !== -1) { html += ' checked'; }
                html += '></td></tr>';
            }

            html += '</tbody></table>';

            $('#' + elemContainer).html(html);
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