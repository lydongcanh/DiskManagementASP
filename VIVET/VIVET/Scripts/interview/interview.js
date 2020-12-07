function LoadSuccess(data) {
    $("#CandidateContent").html("");
    var table = `<table class="table table-hover w-100" id="SearchResultTable">
                            <thead>
                                <tr>
                                    <th>
                                        STT
                                    </th>
                                    <th>
                                        Hình ảnh
                                    </th>
                                    <th>
                                        Vị trí ứng tuyển
                                    </th>
                                    <th>
                                        Tên buổi phỏng vấn
                                    </th>
                                    <th>
                                        Vòng phỏng vấn
                                    </th>
                                    <th>
                                        Họ tên
                                    </th>
                                    <th>
                                        Giới tính
                                    </th>
                                    <th>
                                        Tuổi
                                    </th>
                                    <th>
                                        Ngày phỏng vấn
                                    </th>
                                    <th>
                                        Thời gian phỏng vấn
                                    </th>
                                    <th>
                                        Tỉnh thành
                                    </th>
                                    <th>
                                        Xem thông tin/ Phỏng vấn ngay
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="TableContent"></tbody>
                        </table>`;
    $("#CandidateContent").append(table);
    $.each(data, function (i, item) {
        var rows = `<tr>
                                        <td data-search="${item.Index}" data-order="${item.Index}">${item.Index}</td>
                                        <td><img src="${window.origin}/Resource/EZIMGA?appx=${item.Image}" class="img-thumbnail" style="maxwidth:200px !important;" alt="Không có hình"/></td>
                                        <td data-search="${item.Vacancies}" data-order="${item.Vacancies}">${$.each(item.Vacancies, (el) => `<p>${el}</p>`)}</td>
                                        <td data-search="${item.InterviewName}" data-order="${item.InterviewName}">${item.InterviewName}</td>
                                        <td data-search="${item.Round}" data-order="${item.Round}">${item.Round}</td>
                                        <td data-search="${item.Name}" data-order="${item.Name}">${item.Name}</td>
                                        <td data-search="${item.Sex}" data-order="${item.Sex}">${item.Sex}</td>
                                        <td data-search="${item.Age}" data-order="${item.Age}">${item.Age}</td>
                                        <td data-search="${item.Date}" data-order="${item.Date}">${item.Date}</td>
                                        <td data-search="${item.Time}" data-order="${item.Time}">${item.Time}</td>
                                        <td data-search="${item.City}" data-order="${item.City}">${item.City}</td>
                                        <td><button class="btn btn-primary btn-sm detail-candidate" onclick="showDetail(${item.Id});" >
                                            <i class="fas fa-folder">
                                            </i>
                                        </button>
                                        <a class="btn btn-primary btn-sm style="color:white;" href="/Interview/Round?round=${item.IntRound}&candidate=${item.Id}" >
                                           <i class="fas fa-angle-double-right"></i>
                                        </a></td>
                                    </tr>`;

        $("#TableContent").append(rows);
    });

    $("#SearchResultTable").DataTable({
        scrollX: true,
        paging: true,
        lengthChange: true,
        searching: true,
        searchPlaceholder: 'Tìm ứng viên trong danh sách này...',
        ordering: true,
        order: [[1, "asc"]],
        autoWidth: true,
        columns: [
            { data: 'Index' },
            { data: 'Image' },
            { data: 'Vacancies' },
            { data: 'InterviewName' },
            { data: 'Round' },
            { data: 'Name' },
            { data: 'Sex' },
            { data: 'Age' },
            { data: 'Date' },
            { data: 'Time' },
            { data: 'City' },
            { data: 'Id' }
        ],
        columnDefs: [
            { targets: [1, 11], orderable: false }
        ]
    });
}

function LoadFailed(err) {
    toastr.error(err);
}

function LoadInterview(data) {
    $("#InterviewContent").html("");
    var table = `<table class="table table-hover w-100" id="SearchResultTable">
                            <thead>
                                <tr>
                                    <th>
                                        STT
                                    </th>
                                    <th>
                                        Tên buổi phỏng vấn
                                    </th>
                                    <th>
                                        Dự án
                                    </th>
                                     <th>
                                        Vị trí phỏng vấn
                                    </th>
                                    <th>
                                        Vòng phỏng vấn
                                    </th>
                                    <th>
                                        Phỏng vấn viên
                                    </th>
                                    <th>
                                        Số điện thoại
                                    </th>
                                    <th>
                                        Thời gian bắt đầu
                                    </th>
                                    <th>
                                        Số lượng ứng viên
                                    </th>
                                    <th>
                                       Khu vực phỏng vấn
                                    </th>
                                    <th>
                                       Chỉnh sửa/ Danh sách phỏng vấn 
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="TableContent"></tbody>
                        </table>`;
    $("#InterviewContent").append(table);
    $.each(data, function (i, item) {
        var rows = `<tr>
                                        <td data-search="${item.Index}" data-order="${item.Index}">${item.Index}</td>
                                        <td data-search="${item.InterviewName}" data-order="${item.InterviewName}">${item.InterviewName}</td>
                                        <td data-search="${item.ProjectName}" data-order="${item.ProjectName}">${item.ProjectName}</td>
                                        <td data-search="${item.Vacancies}" data-order="${item.Vacancies}">${$.each(item.Vacancies, (el) => `<p>${el}</p>`)}</td>
                                        <td data-search="${item.Round}" data-order="${item.Round}">${item.Round}</td>
                                        <td data-search="${item.Interviewers}" data-order="${item.Interviewers}">${item.Interviewers}</td>
                                        <td data-search="${item.InterviewerPhone}" data-order="${item.InterviewerPhone}">${item.InterviewerPhone}</td>
                                        <td data-search="${item.StartTime}" data-order="${item.StartTime}">${item.StartTime}</td>
                                        <td data-search="${item.CountCandidate}" data-order="${item.CountCandidate}">${item.CountCandidate}</td>
                                        <td data-search="${item.Location}" data-order="${item.Location}">${item.Location}</td>
                                        <td>
                                            <a class="btn btn-primary btn-sm detail-candidate" href="/Interview/Edit/${item.Id}" >
                                                <i class="fas fa-pencil-alt">Sửa
                                                </i>
                                            </a>
                                            <a class="btn btn-primary btn-sm detail-candidate" href="/Interview/Round?round=${item.IntRound}&interview=${item.Id}" >
                                                                                           <i class="fas fa-eye">Phỏng vấn</i>
                                            </a>
                                        </td>
                                    </tr>`;


        $("#TableContent").append(rows);
    });

    $("#SearchResultTable").DataTable({
        scrollX: true,
        paging: true,
        lengthChange: true,
        searching: true,
        searchPlaceholder: 'Tìm ứng viên trong danh sách này...',
        ordering: true,
        order: [[1, "asc"]],
        autoWidth: true,
        columns: [
            { data: 'Index' },
            { data: 'InterviewName' },
            { data: 'ProjectName' },
            { data: 'Vacancies' },
            { data: 'Round' },
            { data: 'Interviewers' },
            { data: 'InterviewerPhone' },
            { data: 'StartTime' },
            { data: 'CountCandidate' },
            { data: 'Location' }
        ],
        columnDefs: [
            { targets: [10], orderable: false }
        ]
    });
}

function showDetail(id) {
    if (id) {        
        fetch(window.location.origin + "/Interview/Detail/" + id)
            .then(res => res.json())
            .then(data => {                
                $("#CandidateName").html(data.CandidateName);
                $("#FullName").val(data.CandidateName);
                $("#Email").val(data.Email);
                $("#Phone").val(data.Phone);
                $("#Address").val(data.Address);
                $("#Age").val(data.Age);
                $("#Facebook").val(data.Facebook);
                $("#Experiences").val(data.Experiences);
                $("#Comment").val(data.Comment);
                $("#Sex").val(data.Sex);
                $("#Zalo").val(data.Zalo);
                $("#Position").val(data.Position);
                $("#RoundPassed").val(data.RoundPassed);
                $("#Height").val(data.Height);
                $("#Weight").val(data.Weight);
                $("#EducationLevel").val(data.EducationLevel);
                $("#SubmissionDate").val(data.SubmissionDate);
                $("#StartedDate").val(data.StartedTime);
                $("#Form").val(data.Form);
                $("#InternalSourceName").val(data.InternalSourceName);
                $("#InternalSourceCode").val(data.InternalSourceCode);
                $("#Region").val(data.Region);
                
                $("#Cities").val(cities = data.Cities);
                $.each(data.Image, function (i, v) {
                    $("#Image" + (i + 1)).attr('src', window.origin + `/Resource/EZIMGA?appx=${v}`);
                });
                $("#CV").attr('href', window.origin + `/Resource/EZDOC?appx=${data.CV}`);

                $("#detailModal").modal("show");                
            });

    }
}