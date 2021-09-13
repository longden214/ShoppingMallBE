$(document).on("click", "#btn-image-banner", function () {
    var finder = new CKFinder();
    finder.selectActionFunction = function (fileUrl) {
        $("#edit-img-banner").val(fileUrl);
    };
    finder.popup();
});

$(document).ready(function () {
    loadData();

    $("#__BVID__1641").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#datatablesSimple tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

});
function upLoadImage() {

}
function loadData() {
    $.ajax({
        url: "/Banner/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result.data, function (key, item) {
                status = (item.Sratus) ? 'Show' : 'Hide';
                html += '<tr>';
                html += '<td>' + item.Id + '</td>';
                html += '<td>' + '<img src="' + item.Image + '" width="60">' + '</td>';
                html += '<td>' + item.page + '</td>';
                html += '<td>' + item.link + '</td>';
                html += '<td>' + item.OrderBy + '</td>';
                if (item.Sratus) {
                    html += '<td><span data-v-32017d0f="" class="badge text-capitalize badge-light-success badge-pill"> ' + status + ' </span></td >';
                }
                else {
                    html += '<td><span data-v-32017d0f="" class="badge text-capitalize badge-light-warning badge-pill"> ' + status + ' </span></td >';
                }
                if (result.checkRole) {
                    html += '<td><a href="/demo/vuexy-vuejs-admin-dashboard-template/demo-2/apps/users/edit/50" onclick="return getbyID(' + item.Id + ')"class="dropdown-item" role="menuitem" target="_self"><svg data-v-32017d0f="" xmlns="http://www.w3.org/2000/svg" width="14px" height="14px" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="feather feather-edit"><path data-v-32017d0f="" d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path><path data-v-32017d0f="" d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path></svg><span data-v-32017d0f="" class="align-middle ml-50"></span></a></td>';
                }
                html += '</tr>';

            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

}

function getbyID(ID) {

    document.getElementById("vpage").innerHTML = "";
    document.getElementById("vdescription").innerHTML = "";
    document.getElementById("vorderby").innerHTML = "";
    document.getElementById("vlink").innerHTML = "";
    $('#image').css('border-color', 'lightgrey');
    $('#link').css('border-color', 'lightgrey');
    $('#description').css('border-color', 'lightgrey');
    $('#OrderBy').css('border-color', 'lightgrey');
    $('#page').css('border-color', 'lightgrey');
    $('#CreatedDate').css('border-color', 'lightgrey');
    $('#ModifiedDate').css('border-color', 'lightgrey');
    $('#Status').css('border-color', 'lightgrey');
    $('#vpage').show();
    $.ajax({
        url: "/Banner/getbyID/" + ID,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#ID').val(result.Id);
            $('#IDorderValidate').val(result.OrderBy);
            //var img = document.getElementById("images").src;
            //$('image').val(result.Image);
            $('#edit-img-banner').val(result.Image);
            $('#link').val(result.link);
            $('#description').val(result.description);
            $('#OrderBy').val(result.OrderBy);
            $('#ModifiedDate').val(result.ModifiedDate);
            $('#page').val(result.page);
            $('#Statusvad').val(1);

            var x = document.getElementById('Status');
            if (result.Sratus) {
                //var x = $('#Status').is(":checked");
                //$('#Status').ch
                //alert(x);

                x.checked = true;
            } else {
                x.checked = false;
            }

            $('#ModifiedDate').val(result.ModifiedDate);
            $('#CreatedDate').val(result.CreatedDate);

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}

function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var x = document.getElementById('Status');
    if (x.checked == true) {
        var BannerObj = {
            Id: $('#ID').val(),
            image: $('#edit-img-banner').val(),
            link: $('#link').val(),
            description: $('#description').val(),
            OrderBy: $('#OrderBy').val(),
            page: $('#page').val(),
            Sratus: true,
        }
    }
    else {
        var BannerObj = {
            Id: $('#ID').val(),
            image: $('#edit-img-banner').val(),
            link: $('#link').val(),
            description: $('#description').val(),
            OrderBy: $('#OrderBy').val(),
            page: $('#page').val(),
            Sratus: false,
        }
    };
    $.ajax({
        url: "/Banner/Update",
        data: JSON.stringify(BannerObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#ID').val("");
            $('#image').val("");
            $('#link').val("");
            $('#description').val("");
            $('#OrderBy').val("");
            $('#Status').val("");
            $('#page').val("");
            $('#Status').val("");
            $('#ModifiedDate').val("");
            $('#CreatedDate').val("");
            document.getElementById("vorderby").innerHTML = "";
        },
        error: function (errormessage) {
            $('#OrderBy').css('border-color', 'Red');
            document.getElementById("vorderby").innerHTML = "Requied Unique";
            return false;
        }
    });
}
function clearTextBox() {
    $('#ID').val("");
    $('#image').val("");
    $('#link').val("");
    $('#description').val("");
    $('#OrderBy').val("");
    $('#Status').val("");
    $('#page').val("");
    $('#Status').val("");
    $('#ModifiedDate').val("");
    $('#CreatedDate').val("");
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    $('#image').css('border-color', 'lightgrey');
    $('#link').css('border-color', 'lightgrey');
    $('#description').css('border-color', 'lightgrey');
    $('#OrderBy').css('border-color', 'lightgrey');
    $('#page').css('border-color', 'lightgrey');
    $('#CreatedDate').css('border-color', 'lightgrey');
    $('#ModifiedDate').css('border-color', 'lightgrey');
    $('#Status').css('border-color', 'lightgrey');

}

//Valdidation using jquery  
function validate() {
    var isValid = true;
    //var expression = /^[a-z0-9]+([\-\]{1}[a-z0-9]+)*\/[a-z]{2,5}(:[0-9]{1,5})?(\/+)*[a-z]{2,5}(:[0-9]{1,5})?$/gi;
    //var regex = new RegExp(expression);

    if ($('#edit-img-banner').val().trim() == "") {
        $('#edit-img-banner').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#image').css('border-color', 'lightgrey');
    }
    if ($('#link').val().trim() == "") {
        $('#link').css('border-color', 'Red');
        document.getElementById("vlink").innerHTML = "The link field is required";
        isValid = false;
    }
    else {
        $('#link').css('border-color', 'lightgrey');
        document.getElementById("vlink").innerHTML = "";


    }

    if ($('#OrderBy').val().trim() == "") {
        $('#OrderBy').css('border-color', 'Red');
        document.getElementById("vorderby").innerHTML = "The OrderBy field is required";
        isValid = false;
    }
    else {
        $('#OrderBy').css('border-color', 'lightgrey');
        document.getElementById("vorderby").innerHTML = "";

    }
    if ($('#description').val().trim() == "") {
        $('#description').css('border-color', 'Red');
        document.getElementById("vdescription").innerHTML = "The description field is required";
        isValid = false;
    }
    else {
        $('#description').css('border-color', 'lightgrey');
        document.getElementById("vdescription").innerHTML = "";

    }
    if ($('#page').val().trim() == "") {
        $('#page').css('border-color', 'Red');
        document.getElementById("vpage").innerHTML = "The page field is required";
        isValid = false;
    }
    else {
        $('#page').css('border-color', 'lightgrey');
        document.getElementById("vpage").innerHTML = "";

    }

    return isValid;

}