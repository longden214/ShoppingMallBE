

loadData(null, null);
$(document).on("click", "#btn-image-banner", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $("#edit-img-banner").val(fileUrl);
    };
    finder.popup();
});
$(document).on('change', '#DataTables_gallery_length', function () {
    var productlength = $(this).val();

    loadData(null, productlength);
});

function loadData(page, pageSize) {
    $.ajax({
        url: '/Gallery/List',
        type: 'GET',
        data: { page: page, pageSize: pageSize },
        dataType: 'json',
        success: function (res) {
            if (res.TotalItems > 0) {
                var data = res.proList;
                var html = '';
                var template = $('#gallary-list').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        Id: item.id,
                        Image: item.image,
                        Status: item.status ? '<div class="badge badge-glow badge-success">Show</div>' : '<div class="badge badge-glow badge-danger">Hidden</div>'
                    });
                });

                $('#gallery_table .tbody').html(html);
                Pagination(res.CurrentPage, res.NumberPage, res.PageSize);
            }
        }
    })
}
$(document).on('click', '.btn-delete-movie', function () {
    var thisBtn = $(this);
    var id = thisBtn.data('id');
    Delete(id);
});
function showModel() {
    $('#myModal').modal('show');
    document.getElementById("vimage").innerHTML = "";
}
function Delete(id) {

    $.ajax({
        url: '/Gallery/DeleteGallery/' + id,
        type: 'POST',
        dataType: 'json',
        success: function (res) {
            $('#gallery_' + id).remove();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}
$(document).on('click', '.btn-change-status-movie', function () {
    var thisBtn = $(this);
    var id = thisBtn.data('id');

    $.ajax({
        url: '/Gallery/EditStatus/' + id,
        type: 'POST',
        dataType: 'json',
        success: function (res) {
            var status = thisBtn.parents(":eq(2)").prev().children("div").html();

            if (status == 'Show') {
                thisBtn.parents(":eq(2)").prev().html('<div class="badge badge-glow badge-danger">Hidden</div>');
            } else {
                thisBtn.parents(":eq(2)").prev().html('<div class="badge badge-glow badge-success">Show</div>');
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
});

function Add() {

    var res = validate();
    if (res == false) {
        return false;
    }
    var x = document.getElementById('Status');

    var g = {
        image: $('#edit-img-banner').val(),
        status: x.checked,
    }

    $.ajax({
        url: '/Gallery/Add',
        data: JSON.stringify(g),
        type: 'POST',
        contentType: 'application/json;charset=utf-8',
        dataType: 'json',
        success: function (result) {

            loadData(null, null);
            $('#edit-img-banner').val('');
            $('#myModal').modal('hide');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
            return false;
        }
    });
}
function validate() {
    isvalid = true;
    if ($('#edit-img-banner').val().trim() == "") {
        $('#edit-img-banner').css('border-color', 'Red');
        document.getElementById("vimage").innerHTML = "The link field is required";
        isvalid = false;
    }
    else {
        $('#image').css('border-color', 'lightgrey');
    }
    return isvalid;
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

        $('#gallery_paginate ul').html(renderTwoSide);
    } else {
        if (curPage != totalPages) {
            render += `<li class="paginate_button page-item next" ><a href="javascript:void(0);" onclick="NextPage(${curPage + 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        } else {
            render += `<li class="paginate_button page-item next disabled" ><a href="javascript:void(0);"  class="page-link">&nbsp;</a></li>`;
        }

        $('#gallery_paginate ul').html(render);
    }
}

function renderPage(index, active = "", pageSize) {
    return `<li class="paginate_button page-item ${active}">
        <a class="page-link" href="javascript:void(0);" onclick="NextPage(${index},${pageSize})">${index}</a>
    </li>`;
}

function NextPage(page, pageSize) {

    loadData(page, pageSize);
}