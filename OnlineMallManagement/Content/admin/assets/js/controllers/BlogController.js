//
//start
$(document).ready(function () {
    loadData();
    loadCategory();
    $("body").on("click", ".pagination li a", function (event) {
        event.preventDefault();
        var page = $(this).attr('data-page');

        //load event pagination
        var txtSearch = $("#txtSearch").val();
       
            loadData(txtSearch, page)
        

    });

});
//
// search
function search() {
    var txtSearch = $("#txtSearch").val();
    if (txtSearch != "") {
        loadData(txtSearch)
    }
    else {
        loadData(null);
    }
}
//
// Load category
function loadCategory() {
    $.ajax({
        type: "GET",
        url: "/Admin/Blog/LoadCategory",
        data: "{}",
        success: function (data) {
            var s = '';
            for (var i = 0; i < data.length; i++) {
                s += '<option value="' + data[i].CategoryId + '">' + data[i].CategoryName + '</option>';
            }
            $("#categoryBlogList").html(s);
        }
    });

}
//
// click paging
$(document).on("click", "#btn-blog-image", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $("#edit-blog-img").val(fileUrl);
    };
    finder.popup();
});
//
// load list data
function loadData(txtSearch, page) {
    $.ajax({
        url: "/Admin/Blog/Load",
        type: "GET",
        data: { txtSearch: txtSearch, page: page },
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';

            $.each(result.blog, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.BlogId + '</td>';
                html += '<td>' + '<img src="' + item.Images + '" width="60">' + '</td>';
                html += '<td>' + item.Title + '</td>';
                html += '<td>' + item.BlogTime + '</td>';
                html += '<td>' + item.CategoryBlog + '</td>';
                if (item.Status) {
                    html += '<td> <div class="badge badge-glow badge-success">Show</div> </td>';
                } else {
                    html += '<td> <div class="badge badge-glow badge-danger">Hidden</div> </td>';
                }
                if (result.checkRole) {
                    html += '<td>';
                    html += '<a  href="#0" onclick="return getbyID(' + item.BlogId + ')"> <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-edit-2 mr-50"><path d="M17 3a2.828 2.828 0 1 1 4 4L7.5 20.5 2 22l1.5-5.5L17 3z"></path></svg></a>';
                    html += '<a  href="#" onclick="Delele(' + item.BlogId + ')"> <svg xmlns="http://www.w3.org/2000/svg" width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-trash mr-50"><polyline points="3 6 5 6 21 6"></polyline><path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path></svg></a>';
                    html += '</td>';
                }
                html += '</tr>';


                //create pagination
                var pagination_string = "";
                var pageCurrent = result.pageCurrent;
                var numSize = result.numSize;

                //create button previous
                if (pageCurrent > 1) {
                    var pagePrevious = pageCurrent - 1;
                    pagination_string += '<li class="page-item"> <a class="page-link" href="#0" data-page=' + pagePrevious + ' aria-label="Previous"> <span aria-hidden="true">«</span> </a> </li>';
                }else {
                    pagination_string += '<li class="page-item disabled"> <a class="page-link" href="#0"  aria-label="Previous"> <span aria-hidden="true">«</span> </a> </li>'
                }

                for (var i = 1; i <= numSize; i++) {
                    if (i == pageCurrent) {
                        pagination_string += '<li class="page-item active"><a class="page-link" data-page=' + i + ' href="#0">' + pageCurrent + '</a></li>';
                    }
                    else {
                        pagination_string += '<li class="page-item "><a class="page-link" data-page=' + i + ' href="#0">' + i + '</a></li>';
                    }
                }

                //create button next
                if (pageCurrent >= 1 && pageCurrent < numSize) {
                    var pageNext = pageCurrent + 1;
                    pagination_string += '<li class="page-item"> <a class="page-link" href="#0" data-page=' + pageNext + ' aria-label="Next"> <span aria-hidden="true">»</span> </a> </li>';
                }else {
                    pagination_string += '<li class="page-item disabled"> <a class="page-link" href="#0" aria-label="Next"> <span aria-hidden="true">»</span> </a> </li>';
                }

                //load pagination
                $("#load-pagination").html(pagination_string);
            });

            $('#tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//
// content
CKEDITOR.replace('blog-content');

//
// clear Text box
function clearTextBox() {
    $('#blog-edit-title').val("");
    $('#blog-edit-Meta-title').val("");
    $('#blog-edit-Meta-keyword').val("");
    $('#blog-edit-Meta-description').val("");
    $('#blog-edit-slug').val("");
    $('#blog-edit-description').val("");
    $('#blog-content').val("");
    $('#blog-date').val("");

    document.getElementById("x").checked = true;
    CKEDITOR.instances['blog-content'].setData("");
    $('#edit-blog-img').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();

    $('#blog-edit-title').css('border-color', 'lightgrey');
    $('#blog-edit-Meta-title').css('border-color', 'lightgrey');
    $('#blog-edit-Meta-keyword').css('border-color', 'lightgrey');
    $('#blog-edit-Meta-description').css('border-color', 'lightgrey');
    $('#blog-edit-slug').css('border-color', 'lightgrey');
    $('#blog-edit-description').css('border-color', 'lightgrey');
    $('#blog-content').css('border-color', 'lightgrey');
    $('#blog-date').css('border-color', 'lightgrey');
}
//
// delete blog
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Admin/Blog/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}
//
// get blog by id
function getbyID(id) {
    $('#blog-edit-title').css('border-color', 'lightgrey');
    $('#blog-edit-Meta-title').css('border-color', 'lightgrey');
    $('#blog-edit-Meta-keyword').css('border-color', 'lightgrey');
    $('#blog-edit-Meta-description').css('border-color', 'lightgrey');
    $('#blog-edit-slug').css('border-color', 'lightgrey');
    $('#blog-edit-description').css('border-color', 'lightgrey');
    $('#blog-content').css('border-color', 'lightgrey');
    $('#blog-date').css('border-color', 'lightgrey');

    $.ajax({
        url: "/Admin/Blog/GetById/" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#blog-edit-title').val(result.Title);
            $('#blog-edit-Meta-title').val(result.Meta_title);
            $('#blog-edit-Meta-keyword').val(result.Meta_keyword);
            $('#blog-edit-Meta-description').val(result.Meta_description);
            $('#blog-edit-slug').val(result.slug);
            $('#blog-edit-description').val(result.Description);
            $('#blog-date').val(result.BlogTime);

            $('#blog-date').flatpickr({
                dateFormat: "Y-m-d",
                defaultDate: result.BlogTime
            });
            document.getElementById("btnUpdate").value = result.BlogId;
            $("#edit-blog-img").val(result.Images);
            if (result.Status) {
                document.getElementById("x").checked = true;
            } else {
                document.getElementById("y").checked = true;
            }

            $("#categoryBlogList").val(result.Category_blog_id).change();

            CKEDITOR.instances['blog-content'].setData(result.Content);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    })
}
//
//update blog
function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var blog = {
        BlogId: document.getElementById("btnUpdate").value,
        Description: $('#blog-edit-description').val(),
        BlogTime: new Date($('#blog-date').val()).toISOString(),
        Category_blog_id: document.getElementById("categoryBlogList").value,
        Title: $('#blog-edit-title').val(),
        Status: document.getElementById('x').checked,
        slug: $('#blog-edit-slug').val(),
        Meta_title: $('#blog-edit-Meta-title').val(),
        Meta_keyword: $('#blog-edit-Meta-keyword').val(),
        Meta_description: $('#blog-edit-Meta-description').val(),
        Content: CKEDITOR.instances['blog-content'].getData(),
        Images: $('#edit-blog-img').val()
    };

    $.ajax({
        url: "/Admin/Blog/StoreOrEdit",
        data: JSON.stringify(blog),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#EmployeeID').val("");
            $('#Name').val("");
            $('#Age').val("");
            $('#State').val("");
            $('#Country').val("");
            document.getElementById("x").checked = true;
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
//
// validate
function validate() {
    var isValid = true;
    if ($('#blog-edit-title').val().trim() == "") {
        $('#blog-edit-title').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-edit-title').css('border-color', 'lightgrey');
    }

    if ($('#blog-edit-Meta-title').val().trim() == "") {
        $('#blog-edit-Meta-title').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-edit-Meta-title').css('border-color', 'lightgrey');
    }

    if ($('#blog-edit-Meta-keyword').val().trim() == "") {
        $('#blog-edit-Meta-keyword').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-edit-Meta-keyword').css('border-color', 'lightgrey');
    }

    if ($('#blog-edit-Meta-description').val().trim() == "") {
        $('#blog-edit-Meta-description').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-edit-Meta-description').css('border-color', 'lightgrey');
    }

    if ($('#blog-edit-slug').val().trim() == "") {
        $('#blog-edit-slug').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-edit-slug').css('border-color', 'lightgrey');
    }

    if ($('#blog-edit-description').val().trim() == "") {
        $('#blog-edit-description').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-edit-description').css('border-color', 'lightgrey');
    }

    if ($('#blog-date').val().trim() == "") {
        $('#blog-date').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#blog-date').css('border-color', 'lightgrey');
    }

    if (CKEDITOR.instances['blog-content'].getData().trim() == "") {
        $('#blog-content').css('border-color', 'Red');
        alert("Content not null!!!");
        isValid = false;

    }
    else {
        $('#blog-content').css('border-color', 'lightgrey');
    }

    if ($('#edit-blog-img').val().trim() == "") {
        $('#edit-blog-img').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#edit-blog-img').css('border-color', 'lightgrey');
    }

    return isValid;
}
//
// slug
$('#blog-edit-title').keyup(function () {
    var name = $('#blog-edit-title').val();
    $('#blog-edit-slug').val(slugAutoCategoryBlog(name));
})

function slugAutoCategoryBlog(str) {
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