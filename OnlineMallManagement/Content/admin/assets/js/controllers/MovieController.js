

if ($('#pro_search').length == 1) {
    var productlength = $('#DataTables_product_length option:selected').attr('value');
    var txtSearch = $('#pro_search').val();
    loadData(txtSearch, null, productlength);
}

$('#pro_search').keyup(function () {
    var txtSearch = $('#pro_search').val();
    $('.pro-table tbody').html('');

    var productlength = $('#DataTables_product_length option:selected').attr('value');
    loadData(txtSearch, null, productlength);
});

$(document).on('change', '#DataTables_product_length', function () {
    var productlength = $(this).val();
    var txtSearch = $('#pro_search').val();

    loadData(txtSearch, null, productlength);
});


$(".upload-image-list").on("click", ".upload-image-item .image-bg", function () {
    var finder = new CKFinder();
    var row = $(this);
    finder.selectActionFunction = function (fileUrl) {
        row.next().attr('style', "background-image:url(" + fileUrl + ")");
        row.next().attr('data-url', fileUrl);
    };
    finder.popup();
});

//$(document).on('change', '#DataTables_product_length', function () {
//    var productlength = $(this).val();
//    var txtSearch = $('#pro_search').val();

//    loadData(txtSearch, null, productlength);
//});

$(document).on("click", ".btn-banner__movie", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $('#movie__banner').val(fileUrl);
    };
    finder.popup();
});

$(document).on("click", ".btn-image__movie", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $('#movie__image').val(fileUrl);
    };
    finder.popup();
});

$('#movie__name').keyup(function () {
    var name = $('#movie__name').val();

    $('#movie__slug').val(slugAuto(name));
});

function loadData(search, page, pageSize) {
    $.ajax({
        url: 'Movie/loadData',
        type: 'GET',
        data: { search: search, page: page, pageSize: pageSize },
        dataType: 'json',
        success: function (res) {
            if (res.TotalItems > 0) {
                var data = res.proList;
                var html = '';
                var template = $('#movies-list').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        MovieId: item._IdMovie,
                        MovieName: item._MoviveName,
                        Image: item._Image,
                        Duration: item._Duration,
                        AgeRestriction: item._AgeRestriction,
                        ReleaseDate: item._ReleaseDate,
                        Status: item._Status ? '<div class="badge badge-glow badge-success">Show</div>' : '<div class="badge badge-glow badge-danger">Hidden</div>'
                    });
                });

                $('.movie-table tbody').html(html);
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

        $('#product_paginate ul').html(renderTwoSide);
    } else {
        if (curPage != totalPages) {
            render += `<li class="paginate_button page-item next" ><a href="javascript:void(0);" onclick="NextPage(${curPage + 1},${pageSize})" class="page-link">&nbsp;</a></li>`;
        } else {
            render += `<li class="paginate_button page-item next disabled" ><a href="javascript:void(0);"  class="page-link">&nbsp;</a></li>`;
        }

        $('#product_paginate ul').html(render);
    }
}

function renderPage(index, active = "", pageSize) {
    return `<li class="paginate_button page-item ${active}">
        <a class="page-link" href="javascript:void(0);" onclick="NextPage(${index},${pageSize})">${index}</a>
    </li>`;
}

function NextPage(page, pageSize) {
    var txtSearch = $('#pro_search').val();

    loadData(txtSearch, page, pageSize);
}

function slugAuto(str) {
    // Chuyển hết sang chữ thường
    str = str.toLowerCase();

    // xóa dấu
    str = str.replace(/(à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ)/g, 'a');
    str = str.replace(/(è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ)/g, 'e');
    str = str.replace(/(ì|í|ị|ỉ|ĩ)/g, 'i');
    str = str.replace(/(ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ)/g, 'o');
    str = str.replace(/(ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ)/g, 'u');
    str = str.replace(/(ỳ|ý|ỵ|ỷ|ỹ)/g, 'y');
    str = str.replace(/(đ)/g, 'd');

    // Xóa ký tự đặc biệt
    str = str.replace(/([^0-9a-z-\s])/g, '');

    // Xóa khoảng trắng thay bằng ký tự -
    str = str.replace(/(\s+)/g, '-');

    // xóa phần dự - ở đầu
    str = str.replace(/^-+/g, '');

    // xóa phần dư - ở cuối
    str = str.replace(/-+$/g, '');

    // return
    return str;
};

$(document).on('click', '.btn-save-movie', function () {
    if (ValidationMovie()) {
    var _Name = $('#movie__name').val();
    var _Duration = $('#movie__duration').val();
    var _Actors = $('#movie__actors').val();
    var _AgeRestriction = $('#movie__age-restriction').val();
    var _Description = $('#movie__description').val();
    var _Language = $('#movie__language').val();
    var _ReleaseDate = $('#movie__release-date').val();
    var _Country = $('#movie__country').val();

    var _Slug = $('#movie__slug').val();
    var _MetaTitle = $('#movie__meta-title').val();
    var _MetaKeyword = $('#movie__meta-keyword').val();
    var _MetaDescription = $('#movie__meta-description').val();
    var _Banner = $('#movie__banner').val();
    var _Status = $('.movie__status:checked').attr('value') == 1 ? true : false;

    var _Image = $('#movie__image').val();
    var _Photos = new Array();
    $(".upload-image-list .upload-image-item").each(function () {
        var row = $(this);
        var url = row.find(".show-image").attr('data-url');

        _Photos.push(url);
    });

    var _genres = $('.movie__genres').val();

    var movieData = {
        MoviveName: _Name,
        Image: _Image,
        Duration: _Duration,
        Actors: _Actors,
        AgeRestriction: _AgeRestriction,
        Description: _Description,
        Language: _Language,
        ReleaseDate: _ReleaseDate,
        Country: _Country,
        Slug: _Slug,
        Meta_title: _MetaTitle,
        Meta_keyword: _MetaKeyword,
        Meta_description: _MetaDescription,
        Status: _Status,
        banner: _Banner,
        photos: _Photos.toString()
    };

    SaveMovie(movieData, _genres);

    }
});

function SaveMovie(_movieData, _genres) {
    $.ajax({
        url: 'SaveMovie',
        type: 'POST',
        data: JSON.stringify(_movieData),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.movieid > 0) {
                if (_genres != null && _genres.length > 0) {
                    $.ajax({
                        url: 'SaveGenre',
                        type: 'POST',
                        data: JSON.stringify({ genres: _genres, movieId: res.movieid }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (res) {
                            if (res.Result) {
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
            } else {
                toastr.error('Create failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

        }
    });
}

$(document).on('click', '.btn-edit-movie', function () {
    if (ValidationMovie()) {
        var _id = $('#movie__id').val();
        var _Name = $('#movie__name').val();
        var _Duration = $('#movie__duration').val();
        var _Actors = $('#movie__actors').val();
        var _AgeRestriction = $('#movie__age-restriction').val();
        var _Description = $('#movie__description').val();
        var _Language = $('#movie__language').val();
        var _ReleaseDate = $('#movie__release-date').val();
        var _Country = $('#movie__country').val();

        var _Slug = $('#movie__slug').val();
        var _MetaTitle = $('#movie__meta-title').val();
        var _MetaKeyword = $('#movie__meta-keyword').val();
        var _MetaDescription = $('#movie__meta-description').val();
        var _Banner = $('#movie__banner').val();
        var _Status = $('.movie__status:checked').attr('value') == 1 ? true : false;

        var _Image = $('#movie__image').val();
        var _Photos = new Array();
        $(".upload-image-list .upload-image-item").each(function () {
            var row = $(this);
            var url = row.find(".show-image").attr('data-url');

            _Photos.push(url);
        });

        var _genres = $('.movie__genres').val();

        var movieData = {
            IdMovie: _id,
            MoviveName: _Name,
            Image: _Image,
            Duration: _Duration,
            Actors: _Actors,
            AgeRestriction: _AgeRestriction,
            Description: _Description,
            Language: _Language,
            ReleaseDate: _ReleaseDate,
            Country: _Country,
            Slug: _Slug,
            Meta_title: _MetaTitle,
            Meta_keyword: _MetaKeyword,
            Meta_description: _MetaDescription,
            Status: _Status,
            banner: _Banner,
            photos: _Photos.toString()
        };

        EditMovie(movieData, _genres);

    }
});

function EditMovie(_movieData,_genres) {
    $.ajax({
        url: 'EditMovie',
        type: 'POST',
        data: JSON.stringify(_movieData),
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        success: function (res) {
            if (res.movieid > 0) {
                if (_genres != null && _genres.length > 0) {
                    $.ajax({
                        url: 'SaveGenre',
                        type: 'POST',
                        data: JSON.stringify({ genres: _genres, movieId: res.movieid }),
                        dataType: "json",
                        contentType: "application/json; charset=utf-8",
                        success: function (res) {
                            if (res.Result) {
                                window.location = res.newurl;
                            } else {
                                toastr.error('Update failed !', 'Error!', {
                                    closeButton: true,
                                    tapToDismiss: false
                                });
                            }
                        }
                    });
                }
            } else {
                toastr.error('Update failed !', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

        }
    });
}

$(document).on('click', '.btn-change-status-movie', function () {
    var thisBtn = $(this);
    var idMovie = thisBtn.data('id');
    var mv = { id: idMovie };

    if (idMovie > 0) {
        $.ajax({
            url: 'Movie/EditStatus',
            type: 'POST',
            data: mv,
            success: function (res) {
                if (res) {
                    var status = thisBtn.parents(":eq(2)").prev().children("div").html();

                    if (status == 'Show') {
                        thisBtn.parents(":eq(2)").prev().html('<div class="badge badge-glow badge-danger">Hidden</div>');
                    } else {
                        thisBtn.parents(":eq(2)").prev().html('<div class="badge badge-glow badge-success">Show</div>');
                    }
                }
                else {
                    toastr.error('Update failed!', 'Error!', {
                        closeButton: true,
                        tapToDismiss: false
                    });
                }
            }
        });
    }
});

$(document).on('click', '.btn-delete-movie', function () {
    var thisBtn = $(this);
    var idMovie = thisBtn.data('id');
    var mv = { id: idMovie };

    if (idMovie > 0) {
        $.ajax({
            url: 'Movie/DeleteMovie',
            type: 'POST',
            data: mv,
            success: function (res) {
                if (res) {
                    thisBtn.parents(":eq(3)").remove();
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

function ValidationMovie() {
    var check = true;
    var _Name = $('#movie__name').val();
    var _Duration = $('#movie__duration').val();
    var _Actors = $('#movie__actors').val();
    var _Description = $('#movie__description').val();
    var _Language = $('#movie__language').val();
    var _ReleaseDate = $('#movie__release-date').val();
    var _Country = $('#movie__country').val();

    var _Slug = $('#movie__slug').val();
    var _MetaTitle = $('#movie__meta-title').val();
    var _MetaKeyword = $('#movie__meta-keyword').val();
    var _MetaDescription = $('#movie__meta-description').val();
    var _Banner = $('#movie__banner').val();
    var _Image = $('#movie__image').val();

    if (_Name === '') {
        $('#movie__name').next().html('Movie Name cannot be empty!');
        check = false;
    } else {
        $('#movie__name').next().html('');
    }

    if (_Duration === '') {
        $('#movie__duration').next().html('Duration cannot be empty!');
        check = false;
    } else {
        $('#movie__duration').next().html('');
    }

    if (_Actors === '') {
        $('#movie__actors').next().html('Actors cannot be empty!');
        check = false;
    } else {
        $('#movie__actors').next().html('');
    }

    if (_Description === '') {
        $('#movie__description').next().html('Description cannot be empty!');
        check = false;
    } else {
        $('#movie__description').next().html('');
    }

    if (_Language === '') {
        $('#movie__language').next().html('Language cannot be empty!');
        check = false;
    } else {
        $('#movie__language').next().html('');
    }

    if (_ReleaseDate === '') {
        $('#movie__release-date').next().html('ReleaseDate cannot be empty!');
        check = false;
    } else {
        $('#movie__release-date').next().html('');
    }

    if (_Country === '') {
        $('#movie__country').next().html('Country cannot be empty!');
        check = false;
    } else {
        $('#movie__country').next().html('');
    }

    if (_Slug === '') {
        $('#movie__slug').next().html('Slug cannot be empty!');
        check = false;
    } else {
        $('#movie__slug').next().html('');
    }

    if (_MetaTitle === '') {
        $('#movie__meta-title').next().html('Meta Title cannot be empty!');
        check = false;
    } else {
        $('#movie__meta-title').next().html('');
    }

    if (_MetaKeyword === '') {
        $('#movie__meta-keyword').next().html('Meta Keyword cannot be empty!');
        check = false;
    } else {
        $('#movie__meta-keyword').next().html('');
    }

    if (_MetaDescription === '') {
        $('#movie__meta-description').next().html('Meta Description cannot be empty!');
        check = false;
    } else {
        $('#movie__meta-description').next().html('');
    }

    if (_Banner === '') {
        $('#movie__banner').parent().parent().next().html('Banner cannot be empty!');
        check = false;
    } else {
        $('#movie__banner').parent().parent().next().html('');
    }

    if (_Image === '') {
        $('#movie__image').parent().parent().next().html('Image cannot be empty!');
        check = false;
    } else {
        $('#movie__image').parent().parent().next().html('');
    }

    $(".upload-image-list .upload-image-item").each(function () {
        var row = $(this);
        var url = row.find(".show-image").attr('data-url');

        if (url === '') {
            row.css('border', '1px dashed red');
            row.children('.icon-adds').css('border', '1px dashed red');
            row.children('.icon-adds').html('<i class="fas fa-exclamation-triangle"></i>').css('color', 'red');

            check = false;
        } else {
            row.css('border', '1px dashed gray');
            row.children('.icon-adds').css('border', '1px dashed gray');
            row.children('.icon-adds').html('<i class="fas fa-plus"></i>').css('color', 'currentColor');
        }
    });

    var _genre = $('.movie__genres').val();
    if (_genre.length == 0 || _genre == null) {
        $('.movie__genres').parent().next().html('Genre cannot be empty!!');
        check = false;
    } else {
        $('.movie__genres').parent().next().html('');
    }

    return check;
}