loadData(0, null, 10);


$(document).on('change', '#DataTables_seat_length', function () {
    var length = $(this).val();
    var hallId = $('#cinema_hall_select').val();

    loadData(hallId, null, length);
});

$(document).on('change', '#cinema_hall_select', function () {
    var length = $('#DataTables_seat_length').val();
    var hallId = $(this).val();

    loadData(hallId, null, length);
});


function loadData(hall, page, pageSize) {
    $.ajax({
        url: 'Seat/loadData',
        type: 'GET',
        data: { hallId: hall, page: page, pageSize: pageSize },
        dataType: 'json',
        success: function (res) {
            if (res.TotalItems > 0) {
                var data = res.proList;
                var html = '';
                var template = $('#seat-list').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        SeatId: item.SeatId,
                        SeatName: item.SeatName,
                        SeatPrice: item.SeatPrice,
                        RoomName: item.SeatHall
                    });
                });

                $('.seat_table tbody').html(html);
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

        $('#seat-navigation nav ul').html(renderTwoSide);
    } else {
        if (curPage != totalPages) {
            render += `<li class="paginate_button page-item next" ><a href="javascript:void(0);" onclick="NextPage(${curPage + 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        } else {
            render += `<li class="paginate_button page-item next disabled" ><a href="javascript:void(0);"  class="page-link">&nbsp;</a></li>`;
        }

        $('#seat-navigation nav ul').html(render);
    }
}

function renderPage(index, active = "", pageSize) {
    return `<li class="paginate_button page-item ${active}">
        <a class="page-link" href="javascript:void(0);" onclick="NextPage(${index},${pageSize})">${index}</a>
    </li>`;
}

function NextPage(page, pageSize) {
    var hallId = $('#cinema_hall_select').val();

    loadData(hallId, page, pageSize);
}

$(document).on('click', '.seat-edit', function () {
    var id = Number($(this).data('id'));

    GetDataById(id);
});

function GetDataById(_id) {
    $.ajax({
        url: 'Seat/GetById',
        type: 'GET',
        data: { id: _id },
        success: function (res) {

            $('#seat-id').val(res.IdSeat);
            $('#seat-name').val(res.SeatName);
            $('#seat-price').val(res.price);

            $('#cinema_hall_select-form').val(res.IdRoom).change();
        }
    });
}

$(document).on('click', '#btn-add-seat', function () {
    if (Validate()) {
        var _seatId = $('#seat-id').val();
        var _seatName = $('#seat-name').val();
        var _seatPrice = $('#seat-price').val();
        var _seatHall = parseInt($('#cinema_hall_select-form option:selected').val());

        var model = {
            IdSeat: _seatId,
            SeatName: _seatName,
            price: _seatPrice,
            IdRoom: _seatHall
        };

        AddSeat(model);
    }
});

function AddSeat(model) {
    $.ajax({
        url: 'Seat/Create',
        type: 'POST',
        data: model,
        success: function (res) {
            if (res.success && !res.edit) {
                toastr.success('Create successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            } else if (!res.success && !res.edit) {
                toastr.error('Create failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

            if (res.success && res.edit) {
                toastr.success('Update successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            } else if (!res.success && res.edit) {
                toastr.error('Update failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }
            $('#seat-id').val(0);
            loadData($('#cinema_hall_select').val(), null, 10);
        }
    });
}

$(document).on('click', '.seat-delete', function () {
    var id = Number($(this).data('id'));

    DeleteSeat(id);
});

function DeleteSeat(id) {
    $.ajax({
        url: 'Seat/Delete',
        type: 'POST',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                loadData(0, null, 10);

                $('#seat-id').val(0);
                $('#seat-name').val('');
                $('#seat-price').val('');
                $('#cinema_hall_select-form').val(0).change();

                toastr.success('Delete successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            } else {
                toastr.error('Delete failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }
        }
    });
}

function Validate() {
    var check = false;
    var _seatName = $('#seat-name').val();
    var _seatPrice = $('#seat-price').val();
    var _seatHall = parseInt($('#cinema_hall_select-form option:selected').val());

    if (_seatName === '') {
        $('#seat-name').parent().next().html('Name cannot be empty!');
        check = false;
    } else {
        $('#seat-name').parent().next().html('');
        check = true;
    }

    if (_seatPrice === '') {
        $('#seat-price').parent().next().html('Price cannot be empty!');
        check = false;
    } else if (!_seatPrice.match(/^\d+(?:[.,]\d+)*$/)) {
        $('#seat-price').parent().next().html('Invalid price!');
        check = false;
    } else {
        $('#seat-price').parent().next().html('');
        check = true;
    }
    
    if (_seatHall == 0) {
        $('#cinema_hall_select-form').parent().next().html('Please choose a cinema hall!');
        check = false;
    } else {
        $('#cinema_hall_select-form').parent().next().html('');
        check = true;
    }

    return check;
}