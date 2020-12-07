function loadSchedule(urlTarget, data, projectContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: data,
        async: true,
        success: function (poseddata) {
            if (poseddata.success) {
                var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover"><thead><tr><th></th><th>#</th><th>Tên ứng viên</th><th>Ngày nộp hồ sơ</th><th>Điểm hồ sơ</th><th>Trình độ học vấn</th><th>Chiều cao</th><th>Nguồn ứng viên</th><th>Ngày phỏng vấn</th><th>Thời gian bắt đầu</th><th>Thời gian kết thúc</th></tr ></thead ><tbody>';
                $.each(poseddata.result, function (i, item) {
                    html += `<tr>
                                        <td>${item.Id}</td>
                                        <td data-order="${i + 1}">${i + 1}</td>
                                        <td data-search="${item.FullName}" data-order="${item.FullName}">${item.FullName}</td>
                                        <td data-search="${item.SubmissionDate}" data-order="${item.SubmissionDate}">${item.SubmissionDate}</td>
                                        <td data-search="${item.ProfileScore}" data-order="${item.ProfileScore}">${item.ProfileScore}</td>
                                        <td data-search="${item.EducationLevel}" data-order="${item.EducationLevel}">${item.EducationLevel}</td>
                                        <td data-search="${item.Height}" data-order="${item.Height}">${item.Height}</td>
                                        <td data-search="${item.CandidateSource}" data-order="${item.CandidateSource}">${item.CandidateSource}</td>
                                        <td><input type='text' name="Date" class="form-control date-select" readonly required value="${item.Date}" /></td>
                                        <td><input type="text" name="StartTime" class="form-control time-select" value="${item.StartTime}" readonly required></td>
                                        <td><input type="text" name="EndTime" class="form-control time-select" value="${item.EndTime}" readonly required></td>
                                    </tr>`;
                });

                html += '</tbody></table > ';

                $('#' + projectContent).append(html);
                $('#' + gridContainer).DataTable({
                    scrollX: true,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "order": [[1, "asc"]],
                    "info": false,
                    "autoWidth": true,
                    "columns": [
                        { data: 'Id' },
                        { data: 'Index' },
                        { data: 'FullName' },
                        { data: 'SubmissionDate' },
                        { data: 'ProfileScore' },
                        { data: 'EducationLevel' },
                        { data: 'Height' },
                        { data: 'CandidateSource' },
                        { data: 'Date' },
                        { data: 'StartTime' },
                        { data: 'EndTime' }
                    ],
                    "columnDefs": [
                        { "targets": [0], "visible": false },
                        { "targets": [8, 9, 10], "orderable": false }
                    ]

                });

                $('.date-select').daterangepicker({
                    timePicker: false,
                    singleDatePicker: true,
                    autoclose: true,
                    locale: {
                        format: 'DD/MM/YY'
                    }
                });
                $('.time-select').timepicker({
                    showInputs: true,
                    icons: {
                        up: 'fas fa-plus',
                        down: 'fas fa-minus'
                    }
                });
            } else {
                toastr.error(poseddata.message);
                return;
            }


        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}

function loadScheduleNow(urlTarget, data, projectContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: data,
        async: true,
        success: function (poseddata) {
            if (poseddata.success) {
                var html = '<table id="' + gridContainer + '" class="table table-bordered table-hover w-100"><thead><tr><th>#</th><th>Tên ứng viên</th><th>Ngày nộp hồ sơ</th><th>Điểm hồ sơ</th><th>Trình độ học vấn</th><th>Chiều cao</th><th>Nguồn ứng viên</th><th>Chọn<input type="checkbox" id="select-all" class="form-control" /></th></tr></thead><tbody>';
                $.each(poseddata.result, function (i, item) {
                    console.log(item);
                    html += `<tr>
                                        <td>${item.Id}</td>
                                        <td data-search="${item.FullName}" data-order="${item.FullName}">${item.FullName}</td>
                                        <td data-search="${item.SubmissionDate}" data-order="${item.SubmissionDate}">${item.SubmissionDate}</td>
                                        <td data-search="${item.ProfileScore}" data-order="${item.ProfileScore}">${item.ProfileScore}</td>
                                        <td data-search="${item.EducationLevel}" data-order="${item.EducationLevel}">${item.EducationLevel}</td>
                                        <td data-search="${item.Height}" data-order="${item.Height}">${item.Height}</td>
                                        <td data-search="${item.CandidateSource}" data-order="${item.CandidateSource}">${item.CandidateSource}</td>
                                        <td><input type="checkbox" class="form-control" name="isSelect" value=""></td>
                                    </tr>`;
                });

                html += '</tbody></table > ';

                $('#' + projectContent).append(html);
                $('#' + gridContainer).DataTable({
                    scrollX: true,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "order": [[1, "asc"]],
                    "info": false,
                    "autoWidth": true,
                    "columns": [
                        { data: 'Id' },
                        { data: 'FullName' },
                        { data: 'SubmissionDate' },
                        { data: 'ProfileScore' },
                        { data: 'EducationLevel' },
                        { data: 'Height' },
                        { data: 'CandidateSource' },
                        { data: 'isSelect' }
                    ],
                    "columnDefs": [
                        { "targets": [0], "visible": false },
                        { targets: [7], orderable: false }
                    ]

                });

                $("#select-all").on('click', function () {
                    var table = $("#TableCandidate").DataTable();
                    var rows = table.rows({ "search": "applied" }).nodes();
                    $('input[type="checkbox"]', rows).prop('checked', this.checked);
                });
            } else {
                toastr.warning(poseddata.message);
                return;
            }
        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}

function loadSelect(urlTarget, data, projectContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: data,
        async: true,
        success: function (poseddata) {
            if (poseddata.success) {
                var html = `<table id="${gridContainer}" class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th></th>
                            <th>#</th>
                            <th>Tên ứng viên</th>
                            <th>Ngày nộp hồ sơ</th>
                            <th>Điểm hồ sơ</th>
                            <th>Trình độ học vấn</th>
                            <th>Chiều cao</th>
                            <th>Nguồn ứng viên</th>
                            <th>Ngày phỏng vấn<input type='text' id="HeadDate" class="form-control date-select" readonly /></th>
                            <th>Thời gian bắt đầu<input type="text" id="HeadStartTime" class="form-control time-select" value="9 00" readonly></th>
                            <th>Thời gian kết thúc<input type="text" id="HeadEndTime" class="form-control time-select" value="17 00" readonly></th>
                            <th>Chọn<input type="checkbox" id="select-all" class="form-control" /></th>
                        </tr>
                    </thead ><tbody>`;

                $.each(poseddata.result, function (i, item) {
                    html += `<tr>
                                        <td>${item.Id}</td>
                                        <td data-order="${i + 1}">${i + 1}</td>
                                        <td data-search="${item.FullName}" data-order="${item.FullName}">${item.FullName}</td>
                                        <td data-search="${item.SubmissionDate}" data-order="${item.SubmissionDate}">${item.SubmissionDate}</td>
                                        <td data-search="${item.ProfileScore}" data-order="${item.ProfileScore}">${item.ProfileScore}</td>
                                        <td data-search="${item.EducationLevel}" data-order="${item.EducationLevel}">${item.EducationLevel}</td>
                                        <td data-search="${item.Height}" data-order="${item.Height}">${item.Height}</td>
                                        <td data-search="${item.CandidateSource}" data-order="${item.CandidateSource}">${item.CandidateSource}</td>
                                        <td><input type='text' name="Date" class="form-control date-select" readonly required value="${item.Date}" /></td>
                                        <td><input type="text" name="StartTime" class="form-control time-select" value="${item.StartTime}" readonly required></td>
                                        <td><input type="text" name="EndTime" class="form-control time-select" value="${item.EndTime}" readonly required></td>
                                       <td><input type="checkbox" class="form-control" name="isSelect" value=""></td>
                                    </tr>`;
                });

                html += '</tbody></table > ';

                $('#' + projectContent).append(html);
                $('#' + gridContainer).DataTable({
                    scrollX: true,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "order": [[1, "asc"]],
                    "info": false,
                    "autoWidth": true,
                    "columns": [
                        { data: 'Id' },
                        { data: 'Index' },
                        { data: 'FullName' },
                        { data: 'SubmissionDate' },
                        { data: 'ProfileScore' },
                        { data: 'EducationLevel' },
                        { data: 'Height' },
                        { data: 'CandidateSource' },
                        { data: 'Date' },
                        { data: 'StartTime' },
                        { data: 'EndTime' },
                        { data: 'isSelect' }
                    ],
                    "columnDefs": [
                        { "targets": [0], "visible": false },
                        { "targets": [8, 9, 10, 11], "orderable": false }
                    ]
                });
                $('.date-select').daterangepicker({
                    timePicker: false,
                    singleDatePicker: true,
                    autoclose: true,
                    locale: {
                        format: 'DD/MM/YY'
                    }
                });
                $('.time-select').timepicker({
                    showInputs: true,
                    icons: {
                        up: 'fas fa-plus',
                        down: 'fas fa-minus'
                    }
                });

                $("#select-all").on('click', function () {
                    var table = $("#TableCandidate").DataTable();
                    var rows = table.rows({ "search": "applied" }).nodes();
                    $('input[type="checkbox"]', rows).prop('checked', this.checked);
                });

                $("input[name='isSelect']").change(function () {
                    var selected = true;
                    $.each($("input[name='isSelect']"), function (i, item) {
                        if (!$(item).prop('checked')) {
                            selected = false;
                        }
                    });

                    $("#select-all").prop('checked', selected);

                });

                $(gridContainer + ' tbody').on('change', 'input[type="checkbox"]', function () {
                    // If checkbox is not checked
                    if (!this.checked) {
                        var el = $('#select-all').get(0);
                        // If "Select all" control is checked and has 'indeterminate' property
                        if (el && el.checked && ('indeterminate' in el)) {
                            // Set visual state of "Select all" control
                            // as 'indeterminate'
                            el.indeterminate = true;
                        }
                    }
                });

                $("#HeadDate").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='Date']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadStartTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='StartTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadEndTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='EndTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
            } else {
                toastr.error(poseddata.message);
                return;
            }

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}

function loadSelectOne(urlTarget, data, projectContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { candidate: data },
        async: true,
        success: function (poseddata) {
            if (poseddata.success) {

                var interview = poseddata.interview;
                $('#Project').append(`<option selected value="${interview.Project.Id}"> 
                                       ${interview.Project.ProjectName} 
                                  </option>`);


                var html = `<table id="${gridContainer}" class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th></th>
                            <th>#</th>
                            <th>Tên ứng viên</th>
                            <th>Ngày nộp hồ sơ</th>
                            <th>Điểm hồ sơ</th>
                            <th>Trình độ học vấn</th>
                            <th>Chiều cao</th>
                            <th>Nguồn ứng viên</th>
                            <th>Ngày phỏng vấn<input type='text' id="HeadDate" class="form-control date-select" readonly /></th>
                            <th>Thời gian bắt đầu<input type="text" id="HeadStartTime" class="form-control time-select" value="9 00" readonly></th>
                            <th>Thời gian kết thúc<input type="text" id="HeadEndTime" class="form-control time-select" value="17 00" readonly></th>
                            <th>Chọn<input type="checkbox" id="select-all" class="form-control" /></th>
                        </tr>
                    </thead ><tbody>`;

                $.each(poseddata.result, function (i, item) {
                    html += `<tr>
                                        <td>${item.Id}</td>
                                        <td data-order="${i + 1}">${i + 1}</td>
                                        <td data-search="${item.FullName}" data-order="${item.FullName}">${item.FullName}</td>
                                        <td data-search="${item.SubmissionDate}" data-order="${item.SubmissionDate}">${item.SubmissionDate}</td>
                                        <td data-search="${item.ProfileScore}" data-order="${item.ProfileScore}">${item.ProfileScore}</td>
                                        <td data-search="${item.EducationLevel}" data-order="${item.EducationLevel}">${item.EducationLevel}</td>
                                        <td data-search="${item.Height}" data-order="${item.Height}">${item.Height}</td>
                                        <td data-search="${item.CandidateSource}" data-order="${item.CandidateSource}">${item.CandidateSource}</td>
                                        <td><input type='text' name="Date" class="form-control date-select" readonly required value="${item.Date}" /></td>
                                        <td><input type="text" name="StartTime" class="form-control time-select" value="${item.StartTime}" readonly required></td>
                                        <td><input type="text" name="EndTime" class="form-control time-select" value="${item.EndTime}" readonly required></td>
                                       <td><input type="checkbox" class="form-control" name="isSelect" checked=${item.isSelect} value=""></td>
                                    </tr>`;
                });

                html += '</tbody></table > ';

                $('#' + projectContent).append(html);
                $('#' + gridContainer).DataTable({
                    scrollX: true,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "order": [[1, "asc"]],
                    "info": false,
                    "autoWidth": true,
                    "columns": [
                        { data: 'Id' },
                        { data: 'Index' },
                        { data: 'FullName' },
                        { data: 'SubmissionDate' },
                        { data: 'ProfileScore' },
                        { data: 'EducationLevel' },
                        { data: 'Height' },
                        { data: 'CandidateSource' },
                        { data: 'Date' },
                        { data: 'StartTime' },
                        { data: 'EndTime' },
                        { data: 'isSelect' }
                    ],
                    "columnDefs": [
                        { "targets": [0], "visible": false },
                        { "targets": [8, 9, 10, 11], "orderable": false }
                    ]
                });
                $('.date-select').daterangepicker({
                    timePicker: false,
                    singleDatePicker: true,
                    autoclose: true,
                    locale: {
                        format: 'DD/MM/YY'
                    }
                });
                $('.time-select').timepicker({
                    showInputs: true,
                    icons: {
                        up: 'fas fa-plus',
                        down: 'fas fa-minus'
                    }
                });

                $("#select-all").on('click', function () {
                    var table = $("#TableCandidate").DataTable();
                    var rows = table.rows({ "search": "applied" }).nodes();
                    $('input[type="checkbox"]', rows).prop('checked', this.checked);
                });

                $(gridContainer + ' tbody').on('change', 'input[type="checkbox"]', function () {
                    // If checkbox is not checked
                    if (!this.checked) {
                        var el = $('#select-all').get(0);
                        // If "Select all" control is checked and has 'indeterminate' property
                        if (el && el.checked && ('indeterminate' in el)) {
                            // Set visual state of "Select all" control
                            // as 'indeterminate'
                            el.indeterminate = true;
                        }
                    }
                });

                $("#HeadDate").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='Date']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadStartTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='StartTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadEndTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='EndTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
            } else {
                toastr.error(poseddata.message);
                return;
            }

        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}


function loadEdit(urlTarget, data, projectContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        contentType: 'application/json; charset=utf-8',
        type: "POST",
        dataType: 'json',
        data: JSON.stringify({ interview: data }),
        async: true,
        success: function (poseddata) {

            if (poseddata.success) {
                var html = `<table id="${gridContainer}" class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th></th>
                            <th>#</th>
                            <th>Tên ứng viên</th>
                            <th>Ngày nộp hồ sơ</th>
                            <th>Điểm hồ sơ</th>
                            <th>Trình độ học vấn</th>
                            <th>Chiều cao</th>
                            <th>Nguồn ứng viên</th>
                            <th>Ngày phỏng vấn<input type='text' id="HeadDate" class="form-control date-select" readonly /></th>
                            <th>Thời gian bắt đầu<input type="text" id="HeadStartTime" class="form-control time-select" value="9 00" readonly></th>
                            <th>Thời gian kết thúc<input type="text" id="HeadEndTime" class="form-control time-select" value="17 00" readonly></th>
                            <th>Chọn<input type="checkbox" id="select-all" class="form-control" /></th>
                        </tr>
                    </thead ><tbody>`;

                $.each(poseddata.result, function (i, item) {
                    html += `<tr>
                                        <td>${item.Id}</td>
                                        <td data-order="${i + 1}">${i + 1}</td>
                                        <td data-search="${item.FullName}" data-order="${item.FullName}">${item.FullName}</td>
                                        <td data-search="${item.SubmissionDate}" data-order="${item.SubmissionDate}">${item.SubmissionDate}</td>
                                        <td data-search="${item.ProfileScore}" data-order="${item.ProfileScore}">${item.ProfileScore}</td>
                                        <td data-search="${item.EducationLevel}" data-order="${item.EducationLevel}">${item.EducationLevel}</td>
                                        <td data-search="${item.Height}" data-order="${item.Height}">${item.Height}</td>
                                        <td data-search="${item.CandidateSource}" data-order="${item.CandidateSource}">${item.CandidateSource}</td>
                                        <td><input type='text' name="Date" class="form-control date-select" readonly required value="${item.Date}" /></td>
                                        <td><input type="text" name="StartTime" class="form-control time-select" value="${item.StartTime}" readonly required></td>
                                        <td><input type="text" name="EndTime" class="form-control time-select" value="${item.EndTime}" readonly required></td>
                                       <td><input type="checkbox" class="form-control" name="isSelect" value=""></td>
                                    </tr>`;
                });

                html += '</tbody></table > ';

                $('#' + projectContent).append(html);
                $('#' + gridContainer).DataTable({
                    scrollX: true,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "order": [[1, "asc"]],
                    "info": false,
                    "autoWidth": true,
                    "columns": [
                        { data: 'Id' },
                        { data: 'Index' },
                        { data: 'FullName' },
                        { data: 'SubmissionDate' },
                        { data: 'ProfileScore' },
                        { data: 'EducationLevel' },
                        { data: 'Height' },
                        { data: 'CandidateSource' },
                        { data: 'Date' },
                        { data: 'StartTime' },
                        { data: 'EndTime' },
                        { data: 'isSelect' }
                    ],
                    "columnDefs": [
                        { "targets": [0], "visible": false },
                        { "targets": [8, 9, 10, 11], "orderable": false }
                    ]
                });
                $('.date-select').daterangepicker({
                    timePicker: false,
                    singleDatePicker: true,
                    autoclose: true,
                    locale: {
                        format: 'DD/MM/YY'
                    }
                });
                $('.time-select').timepicker({
                    showInputs: true,
                    icons: {
                        up: 'fas fa-plus',
                        down: 'fas fa-minus'
                    }
                });

                $("#select-all").on('click', function () {
                    var table = $("#TableCandidate").DataTable();
                    var rows = table.rows({ "search": "applied" }).nodes();
                    $('input[type="checkbox"]', rows).prop('checked', this.checked);
                });

                $(gridContainer + ' tbody').on('change', 'input[type="checkbox"]', function () {
                    // If checkbox is not checked
                    if (!this.checked) {
                        var el = $('#select-all').get(0);
                        // If "Select all" control is checked and has 'indeterminate' property
                        if (el && el.checked && ('indeterminate' in el)) {
                            // Set visual state of "Select all" control
                            // as 'indeterminate'
                            el.indeterminate = true;
                        }
                    }
                });

                $("#HeadDate").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='Date']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadStartTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='StartTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadEndTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='EndTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
            } else {
                toastr.error(poseddata.message);
                return;
            }


        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}

function loadEditOne(urlTarget, data, projectContent, gridContainer) {
    $.ajax({
        url: urlTarget,
        dataType: 'json',
        data: { idInterview: data },
        async: true,
        success: function (poseddata) {
            if (poseddata.success) {
                var interview = poseddata.interview;
                console.log(interview);
                $("#InterviewName").val(interview.InterviewName);
                $("#StartDate").val(interview.StartDate);
                $("#Interviewer").val(interview.Interviewer);
                console.log(interview.InterviewerPhone);
                $("#InterviewerPhone").val(interview.InterviewerPhone);
                $("#Location").val(interview.Location);
                $("#RangeTime").val(interview.RangeTime);
                $('#Project').append(`<option selected value="${interview.Project.Id}"> 
                                       ${interview.Project.ProjectName} 
                                  </option>`);

                $("#CityRegion").append(`<option selected>${interview.CityRegion}</option>`)

                $.each(interview.District, function (i, item) {
                    $('#District').append(`<option selected>${item.text}</option>`);
                });


                $('#Location').summernote();

                var html = `<table id="${gridContainer}" class="table table-bordered table-hover">
                    <thead>
                        <tr>
                            <th></th>
                            <th>#</th>
                            <th>Tên ứng viên</th>
                            <th>Ngày nộp hồ sơ</th>
                            <th>Điểm hồ sơ</th>
                            <th>Trình độ học vấn</th>
                            <th>Chiều cao</th>
                            <th>Nguồn ứng viên</th>
                            <th>Ngày phỏng vấn<input type='text' id="HeadDate" class="form-control date-select" readonly /></th>
                            <th>Thời gian bắt đầu<input type="text" id="HeadStartTime" class="form-control time-select" value="9 00" readonly></th>
                            <th>Thời gian kết thúc<input type="text" id="HeadEndTime" class="form-control time-select" value="17 00" readonly></th>
                            <th>Chọn<input type="checkbox" id="select-all" class="form-control" /></th>
                        </tr>
                    </thead ><tbody>`;

                $.each(poseddata.result, function (i, item) {
                    console.log(item.SubmissionDate);
                    html += `<tr>
                                        <td>${item.Id}</td>
                                        <td data-order="${i + 1}">${i + 1}</td>
                                        <td data-search="${item.FullName}" data-order="${item.FullName}">${item.FullName}</td>
                                        <td data-search="${item.SubmissionDate}" data-order="${item.SubmissionDate}">${item.SubmissionDate}</td>
                                        <td data-search="${item.ProfileScore}" data-order="${item.ProfileScore}">${item.ProfileScore}</td>
                                        <td data-search="${item.EducationLevel}" data-order="${item.EducationLevel}">${item.EducationLevel}</td>
                                        <td data-search="${item.Height}" data-order="${item.Height}">${item.Height}</td>
                                        <td data-search="${item.CandidateSource}" data-order="${item.CandidateSource}">${item.CandidateSource}</td>
                                        <td><input type='text' name="Date" class="form-control date-select" readonly required value="${item.Date}" /></td>
                                        <td><input type="text" name="StartTime" class="form-control time-select" value="${item.StartTime}" readonly required></td>
                                        <td><input type="text" name="EndTime" class="form-control time-select" value="${item.EndTime}" readonly required></td>
                                       <td><input type="checkbox" class="form-control" name="isSelect" value="false" ></td>
                                    </tr>`;
                });

                html += '</tbody></table > ';

                $('#' + projectContent).append(html);
                $('#' + gridContainer).DataTable({
                    scrollX: true,
                    "paging": true,
                    "lengthChange": true,
                    "searching": true,
                    "ordering": true,
                    "order": [[1, "asc"]],
                    "info": false,
                    "autoWidth": true,
                    "columns": [
                        { data: 'Id' },
                        { data: 'Index' },
                        { data: 'FullName' },
                        { data: 'SubmissionDate' },
                        { data: 'ProfileScore' },
                        { data: 'EducationLevel' },
                        { data: 'Height' },
                        { data: 'CandidateSource' },
                        { data: 'Date' },
                        { data: 'StartTime' },
                        { data: 'EndTime' },
                        { data: 'isSelect' }
                    ],
                    "columnDefs": [
                        { "targets": [0], "visible": false },
                        { "targets": [8, 9, 10, 11], "orderable": false }
                    ]
                });
                $('.date-select').daterangepicker({
                    timePicker: false,
                    singleDatePicker: true,
                    autoclose: true,
                    locale: {
                        format: 'DD/MM/YY'
                    }
                });
                $('.time-select').timepicker({
                    showInputs: true,
                    icons: {
                        up: 'fas fa-plus',
                        down: 'fas fa-minus'
                    }
                });

                $("#select-all").on('click', function () {
                    var table = $("#TableCandidate").DataTable();
                    var rows = table.rows({ "search": "applied" }).nodes();
                    $('input[type="checkbox"]', rows).prop('checked', this.checked);
                });

                $("input[name='isSelect']").change(function () {
                    var selected = true;
                    $.each($("input[name='isSelect']"), function (i, item) {
                        if (!$(item).prop('checked')) {
                            selected = false;
                        }
                    });

                    $("#select-all").prop('checked', selected);

                });

                $(gridContainer + ' tbody').on('change', 'input[type="checkbox"]', function () {
                    // If checkbox is not checked
                    if (!this.checked) {
                        var el = $('#select-all').get(0);
                        // If "Select all" control is checked and has 'indeterminate' property
                        if (el && el.checked && ('indeterminate' in el)) {
                            // Set visual state of "Select all" control
                            // as 'indeterminate'
                            el.indeterminate = true;
                        }
                    }
                });

                $("#HeadDate").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='Date']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadStartTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='StartTime']").forEach(v => {
                        $(v).val(text);
                    });
                });
                $("#HeadEndTime").change(function () {
                    var text = $(this).val();
                    document.querySelectorAll("input[name='EndTime']").forEach(v => {
                        $(v).val(text);
                    });
                });

            } else {
                window.history.back();
            }


        },
        error: function (xhr, status, error) {
            alert("Error" + error);
        }
    });
}

