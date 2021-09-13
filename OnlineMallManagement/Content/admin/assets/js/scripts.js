$(document).on("click", "#btn-image-photoShop", function () {
    console.log("1");
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        console.log("2 " + fileUrl);
        $("#edit-img-photoShop").val(fileUrl);
    };
    finder.popup();
});
$(document).on("click", "#btn-image-logoShop", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $("#edit-img-logoShop").val(fileUrl);
    };
    finder.popup();
});
$(document).on("click", "#btn-image-event", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $("#edit-img-event").val(fileUrl);
    };
    finder.popup();
});
$(document).on("click", "#btn-image-product", function () {
    console.log("1");
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        console.log("2 " + fileUrl);
        $("edit-img-product").val(fileUrl);
    };
    finder.popup();
});
$('#shop_name').keyup(function () {
    var name = $('#shop_name').val();
    $('#shop_slug').val(slugAutoShop(name));
})
function slugAutoShop(str) {
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

$('#event_name').keyup(function () {
    var name = $('#event_name').val();
    $('#event_slug').val(slugAutoEvent(name));
})


$('#categoryBlog_name').keyup(function () {
    var name = $('#categoryBlog_name').val();
    $('#categoryBlog_slug').val(slugAutoEvent(name));
})

function slugAutoEvent(str) {
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
$(document).on("click", "#btn-image-Shopproduct", function () {
    console.log("1");
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        console.log("2 " + fileUrl);
        $("#edit-img-Shopproduct").val(fileUrl);
    };
    finder.popup();
});