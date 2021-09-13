
loadData('', null, );

$('#user_search').keyup(function () {
    var txtSearch = $('#user_search').val();
    $('.user-table tbody').html('');

    var length = $('#user_select').val();
    loadData(txtSearch, null, length);
});

$(document).on('change', '#user_select', function () {
    var length = $(this).val();
    var txtSearch = $('#user_search').val();

    loadData(txtSearch, null, length);
});

function loadData(search, page, pageSize) {
    $.ajax({
        url: 'User/loadData',
        type: 'GET',
        data: { search: search, page: page, pageSize: pageSize },
        dataType: 'json',
        success: function (res) {
            if (res.TotalItems > 0) {
                var data = res.proList;
                var html = '';
                var template = $('#user-list').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        UserId: item.UserId,
                        UserName: item.DisplayName,
                        Email: item.Email,
                        Role: item.RoleName,
                        Phone: item.Phone,
                    });
                });

                $('.user_table tbody').html(html);
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

        $('#user_paginate ul').html(renderTwoSide);
    } else {
        if (curPage != totalPages) {
            render += `<li class="paginate_button page-item next" ><a href="javascript:void(0);" onclick="NextPage(${curPage + 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        } else {
            render += `<li class="paginate_button page-item next disabled" ><a href="javascript:void(0);"  class="page-link">&nbsp;</a></li>`;
        }

        $('#user_paginate ul').html(render);
    }
}

function renderPage(index, active = "", pageSize) {
    return `<li class="paginate_button page-item ${active}">
        <a class="page-link" href="javascript:void(0);" onclick="NextPage(${index},${pageSize})">${index}</a>
    </li>`;
}

function NextPage(page, pageSize) {
    var txtSearch = $('#user_search').val();

    loadData(txtSearch, page, pageSize);
}

$(document).on('click', '.btn-user-submit', function () {
if (ValidationUser()) {
    var _UserName = $('#user-name').val();
    var _DisplayName = $('#user-displayName').val();
    var _Email = $('#user-email').val();
    var _Phone = $('#user-phone').val();
    var _Pass = $('#user-password').val();
    var _Role = $('#user-role').val();

    var Data = {
        UserName: _UserName,
        DisplayName: _DisplayName,
        Password: _Pass,
        Phone: _Phone,
        Email: _Email
    };
    SaveUser(Data, _Role);

 }
});


$(document).on('click', '.btn-delete-user', function () {
    var userId = $(this).data('id');

    DeleteUser(userId);
});

function DeleteUser(_id) {
    $.ajax({
        url: 'User/Delete',
        type: 'POST',
        data: JSON.stringify({ id: _id }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.result) {
                $('#user_' + _id).remove();

                loadData('', null, 10);
            } else {
                toastr.error('Delete failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

        }
    });
}

function SaveUser(_data,_role) {
    $.ajax({
        url: 'User/SaveUser',
        type: 'POST',
        data: JSON.stringify({ model: _data, _role: _role }),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.result) {
                $(".model-user-form .close").click();

                $('#user-name').val('');
                $('#user-displayName').val('');
                $('#user-email').val('');
                $('#user-phone').val('');
                $('#user-role').val(0).change();

                loadData('', null, 10);
            } else {
                toastr.error('Create failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

        }
    });
}

function checkUName(_uname) {
    var result = false;
    $.ajax({
        url: 'User/CheckUserName',
        type: 'POST',
        data: { uname: _uname },
        async: false,
        success: function (res) {
            result = res.result
        }
    });

    return result;
}

function ValidationUser() {
    var check = true;

    var _UserName = $('#user-name').val();
    var _DisplayName = $('#user-displayName').val();
    var _Email = $('#user-email').val();
    var _Phone = $('#user-phone').val();
    var _Role = $('#user-role').val();

    if (_UserName === '') {
        $('#user-name').next().html('User Name cannot be empty!');
        check = false;
    } else if (checkUName(_UserName)) {
        $('#user-name').next().html('User Name already exists!');
        check = false;
    } else {
        $('#user-name').next().html('');
    }

    if (_DisplayName === '') {
        $('#user-displayName').next().html('Display Name cannot be empty!');
        check = false;
    } else {
        $('#user-displayName').next().html('');
    }

    if (_Email === '') {
        $('#user-email').next().html('Email cannot be empty!');
        check = false;
    } else if (!_Email.match(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/)) {
        $('#user-email').next().html('Invalid email!');
        check = false;
    } else {
        $('#user-email').next().html('');
    }

    if (_Phone === '') {
        $('#user-phone').next().html('Phone cannot be empty!');
        check = false;
    } else if (!_Phone.match(/^(\+\d)?[0-9\-\(\) ]{5,}$/i)) {
        $('#user-phone').next().html('Invalid phone!');
        check = false;
    } else {
        $('#user-phone').next().html('');
    }

    if (_Role == 0) {
        $('#user-role').next().html('Please choose a Role!');
        check = false;
    } else {
        $('#user-role').next().html('');

    }

    return check;
}