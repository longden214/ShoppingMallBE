

loadData('', null, 12,null);

$(document).on("click", ".btn-search-movie", function () {
    var value = $('#movie-search-client').val();
    var show = $('#select-movie-bar').val();
    var date = $('#movie-filter-date').val();

    loadData(value, null, show,date);
});

$(document).on('change', '#select-movie-bar', function () {
    var length = $(this).val();
    var txtSearch = $('#movie-search-client').val();
    var date = $('#movie-filter-date').val();

    loadData(txtSearch, null, length,date);
});

$("#movie-filter-date").on("input", function () {
   
    var show = $('#select-movie-bar').val();
    var txtSearch = $('#movie-search-client').val();
    var date = $(this).val();

    loadData(txtSearch, null, show, date);
});

function loadData(search, page, pageSize,_date) {
    $.ajax({
        url: 'loadData',
        type: 'GET',
        data: { search: search, page: page, pageSize: pageSize, date : _date },
        dataType: 'json',
        success: function (res) {
           
                var data = res.proList;
                var html = '';
                var template = $('#movies-list-client').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        MovieId: item._IdMovie,
                        Name: item._MoviveName,
                        Image: item._Image,
                        Duration: item._Duration,
                        Age: item._AgeRestriction,
                        ReleaseDate: item._ReleaseDate,
                        Actors: item._Actors,
                        Genres: GetGenre(item._IdMovie)
                    });
                });

                $('.movie__list-full').html(html);
                Pagination(res.CurrentPage, res.NumberPage, res.PageSize);
            if (res.TotalItems > 0) {
                $('.movie-client-pagination').css("display", "block");
            } else {
                $('.movie-client-pagination').css("display", "none");
            }
        }
    })
}

function GetGenre(_MovieId) {
    var result = '';
    $.ajax({
        url: 'GetGenres',
        type: 'GET',
        data: { MovieId: _MovieId },
        dataType: 'json',
        async: false,
        success: function (res) {
            $.each(res.genreList, function (i, item) {
                result += item.Name + " | ";
            });
        }
    });

    var genres = result.slice(0, result.length - 3);

    return genres;
}

function Pagination(curPage, totalPages, pageSize) {

    const delta = 2; // số hiển thị 2 bên curPage VD curPage = 6: ... 4 5 6 7 8 ...
    var truncate = true;
    const range = delta + 4; // use for handle visible number of links left side

    let render = "";
    let renderTwoSide = "";

    if (curPage != 1) {
        
        render += `<a href="#0" onclick="NextPage(${curPage - 1},${pageSize})"><i class="fas fa-angle-double-left"></i><span>Prev</span></a>`;
    } else {
        render += `<a href="#0" ><i class="fas fa-angle-double-left"></i><span>Prev</span></a>`;
    }

    let dot = `<a href="javascript:void(0);">...</a>`;
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
            prevTwoSide += `<a href="#0" onclick="NextPage(${curPage - 1},${pageSize})"><i class="fas fa-angle-double-left"></i><span>Prev</span></a>`;
        }

        renderTwoSide = prevTwoSide + renderPage(1, "", pageSize) + dot + renderTwoSide + dot + renderPage(totalPages, "", pageSize);

        if (curPage != totalPages) {
            
            renderTwoSide += `<a href="#0" onclick="NextPage(${curPage + 1},${pageSize})"><span>Next</span><i class="fas fa-angle-double-right"></i></a>`;
        }

        $('.movie-client-pagination').html(renderTwoSide);
    } else {
        if (curPage != totalPages) {
            render += `<a href="#0" onclick="NextPage(${curPage + 1},${pageSize})"><span>Next</span><i class="fas fa-angle-double-right"></i></a>`;
        } else {
            render += `<a href="#0" ><span>Next</span><i class="fas fa-angle-double-right"></i></a>`;
        }

        $('.movie-client-pagination').html(render);
    }
}

function renderPage(index, active = "", pageSize) {
    
    return `<a href="#0" onclick="NextPage(${index},${pageSize})" class="${active}">${index}</a>`;
}

function NextPage(page, pageSize) {
    var txtSearch = $('#movie-search-client').val();
    var date = $('#movie-filter-date').val();

    loadData(txtSearch, page, pageSize,date);
}