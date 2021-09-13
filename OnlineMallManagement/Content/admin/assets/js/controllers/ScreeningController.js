
if ($('#screening_search').length == 1) {
    var length = $('#DataTables_screening_length option:selected').attr('value');
    var txtSearch = $('#screening_search').val();
    loadData(txtSearch, null, length);
}

$('#screening_search').keyup(function () {
    var txtSearch = $('#screening_search').val();
    $('.screening-table tbody').html('');

    var productlength = $('#DataTables_screening_length option:selected').attr('value');
    loadData(txtSearch, null, productlength);
});

$(document).on('change', '#DataTables_screening_length', function () {
    var productlength = $(this).val();
    var txtSearch = $('#screening_search').val();

    loadData(txtSearch, null, productlength);
});

function loadData(search, page, pageSize) {
    $.ajax({
        url: 'Screening/loadData',
        type: 'GET',
        data: { search: search, page: page, pageSize: pageSize },
        dataType: 'json',
        success: function (res) {
            if (res.TotalItems > 0) {
                var data = res.proList;
                var html = '';
                var template = $('#screening-list').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        ScreeningId: item.ScreeningId,
                        MovieImage: item.MovieImg,
                        MovieName: item.MovieName,
                        RoomName: item.CinemaHall,
                        Date: item.Date,
                        Time: item.Time,
                        Status: item.Status ? '<div class="badge badge-glow badge-success">Coming Soon</div>' : '<div class="badge badge-glow badge-danger">Finished Showing</div>'
                    });
                });

                $('.screening-table tbody').html(html);
                Pagination(res.CurrentPage, res.NumberPage, res.PageSize);
            }
        }
    })
}

function Pagination(curPage, totalPages, pageSize) {

    const delta = 2; // số hiển thị 2 bên curPage VD curPage = 6: ... 4 5 6 7 8 ...
    var truncate = true;
    const range = delta + 4; // use for handle visible number of links left side

    let render = "";
    let renderTwoSide = "";

    if (curPage != 1) {
        render += `<li class="paginate_button page-item previous" ><a href="javascript:void(0);" onclick="NextPage(${curPage - 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
    } else {
        render += `<li class="paginate_button page-item previous disabled" ><a href="javascript:void(0);" class="page-link">&nbsp;</a></li>`;
    }

    let dot = `<li class="paginate_button page-item"><a class="page-link" href="javascript:void(0);">...</a></li>`;
    let countTruncate = 0; // use for ellipsis - truncate left side or right side

    // use for truncate two side
    const numberTruncateLeft = curPage - delta;
    const numberTruncateRight = curPage + delta;

    let active = "";
    for (let pos = 1; pos <= totalPages; pos++) {
        active = pos === curPage ? "active" : "";

        // truncate
        if (totalPages >= 2 * range - 1 && truncate) { // truncate = true có thu gọn bằng ...
            if (numberTruncateLeft > 3 && numberTruncateRight < totalPages - 3 + 1) {//Thu gọn bên trái và cả bên phải
                // truncate 2 side
                if (pos >= numberTruncateLeft && pos <= numberTruncateRight) {
                    renderTwoSide += renderPage(pos, active, pageSize);
                }
            } else {
                // truncate left side or right side
                if (
                    (curPage < range && pos <= range) ||
                    (curPage > totalPages - range && pos >= totalPages - range + 1) ||
                    pos === totalPages ||
                    pos === 1
                ) {//Thu gọn bên trái không thu gọn bên phải || Thu gọn bên phải không thu gọn bên trái
                    render += renderPage(pos, active, pageSize);
                } else {
                    countTruncate++;
                    if (countTruncate === 1) render += dot;
                }
            }
        } else {
            // truncate = false không thu gọn bằng ...
            render += renderPage(pos, active, pageSize);
        }
    }

    if (renderTwoSide) {//nếu có thu gọn ... 
        var prevTwoSide = "";
        if (curPage != 1) {
            prevTwoSide += `<li class="paginate_button page-item previous" ><a href="javascript:void(0);" onclick="NextPage(${curPage - 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        }

        renderTwoSide = prevTwoSide + renderPage(1, "", pageSize) + dot + renderTwoSide + dot + renderPage(totalPages, "", pageSize);

        if (curPage != totalPages) {
            renderTwoSide += `<li class="paginate_button page-item next" ><a href="javascript:void(0);" onclick="NextPage(${curPage + 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        }

        $('#screening_paginate ul').html(renderTwoSide);
    } else {
        if (curPage != totalPages) {
            render += `<li class="paginate_button page-item next" ><a href="javascript:void(0);" onclick="NextPage(${curPage + 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        } else {
            render += `<li class="paginate_button page-item next disabled" ><a href="javascript:void(0);"  class="page-link">&nbsp;</a></li>`;
        }

        $('#screening_paginate ul').html(render);
    }
}

function renderPage(index, active = "", pageSize) {
    return `<li class="paginate_button page-item ${active}">
        <a class="page-link" href="javascript:void(0);" onclick="NextPage(${index},${pageSize})">${index}</a>
    </li>`;
}

function NextPage(page, pageSize) {
    var txtSearch = $('#screening_search').val();

    loadData(txtSearch, page, pageSize);
}

$(document).on('click', '.btn-save-screening', function () {
    if (ValidateScreening()) {

        var _MovieId = $('#form-select-movie').val();
        var _RoomId = $('#form-select-room').val();
        var _Day = $('#screening_day').val();
        var _Time = $('#screening_time').val();


        var Data = {
            Room_IdRoom: _RoomId,
            Movie_Id: _MovieId,
            ScreeningDate: _Day,
            StartTime: _Time,
            Status: true
        };

        SaveScreening(Data);
    }
});



function SaveScreening(model) {
    $.ajax({
        url: 'SaveScreening',
        type: 'POST',
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.result) {
                window.location = res.newurl;
            } else {
                toastr.error('Create failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

        }
    });
}

$(document).on('click', '.btn-edit-screening', function () {
    if (ValidateScreening()) {
        var _screeningId = $('#screening_id').val();
        var _MovieId = $('#form-select-movie').val();
        var _RoomId = $('#form-select-room').val();
        var _Day = $('#screening_day').val();
        var _Time = $('#screening_time').val();


        var Data = {
            Id: _screeningId,
            Room_IdRoom: _RoomId,
            Movie_Id: _MovieId,
            ScreeningDate: _Day,
            StartTime: _Time
        };

        EditScreening(Data);
    }
});

function EditScreening(model) {
    $.ajax({
        url: 'EditScreening',
        type: 'POST',
        data: JSON.stringify(model),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.Result) {
                window.location = res.newurl;
            } else {
                toastr.error('Update failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

        }
    });
}

$(document).on('click', '.btn-delete-screening', function () {
    var thisBtn = $(this);
    var _id = thisBtn.data('id');
    var mv = { id: _id };

    if (_id > 0) {
        $.ajax({
            url: 'Screening/DeleteScreening',
            type: 'POST',
            data: mv,
            success: function (res) {
                if (res) {
                    
                    thisBtn.parents(":eq(3)").remove();

                    var length = $('#DataTables_screening_length option:selected').attr('value');
                    var txtSearch = $('#screening_search').val();
                    loadData(txtSearch, null, length);
                }
                else {
                    toastr.error('Delete failed!', 'Error!', {
                        closeButton: true,
                        tapToDismiss: false
                    });
                }
            }
        });
    }
});

function ValidateScreening() {
    var check = true;

    var _MovieId = $('#form-select-movie').val();
    var _RoomId = $('#form-select-room').val();
    var _Day = $('#screening_day').val();
    var _Time = $('#screening_time').val();

    if (_MovieId == null) {
        $('#form-select-movie').parent().next().html('Movie cannot be empty!');
        check = false;
    } else {
        $('#form-select-movie').parent().next().html('');
    }

    if (_RoomId == null) {
        $('#form-select-room').parent().next().html('Cinema Hall cannot be empty!');
        check = false;
    } else {
        $('#form-select-room').parent().next().html('');
    }

    if (_Day === '') {
        $('#screening_day').next().html('Day cannot be empty!');
        check = false;
    } else {
        $('#screening_day').next().html('');
    }

    if (_Time === '') {
        $('#screening_time').next().html('Time cannot be empty!');
        check = false;
    } else if (checkTime(_RoomId, _Day, _Time)) {
        $('#screening_time').next().html('There is a movie being shown at this time!');
        check = false;
    } else {
        $('#screening_time').next().html('');
    }

    return check;
}

function checkTime(_roomId,_day,_time) {
        var result = false;
        $.ajax({
            url: 'CheckTime',
            type: 'POST',
            data: { _roomId: _roomId, _date: _day, _time: _time },
            async: false,
            success: function (res) {
                result = res.success
            }
        });

        return result;
}