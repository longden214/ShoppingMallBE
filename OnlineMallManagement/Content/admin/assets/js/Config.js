$(document).ready(function () {
    loadData();

    loadIcon();
});
function loadIcon() {
    $(document).on("click", "#btn-image-icon", function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (fileUrl) {
            //$("#iconvalue").val(fileUrl);
            document.getElementById("iconvalue").innerHTML = fileUrl;
            document.getElementById("imgicon").src = fileUrl;
        };
        finder.popup();
    });
    $(document).on("click", "#btn-image-icon1", function () {
        var finder = new CKFinder();
        finder.selectActionFunction = function (fileUrl) {
            /*$("#icon1value").val(fileUrl);*/
            document.getElementById("icon1value").innerHTML = fileUrl;
            document.getElementById("imgicon1").src = fileUrl;
        };
        finder.popup();
    });

}
function loadData() {
    $.ajax({
        url: "/Config/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#Mail').css('border-color', 'lightgrey');
            $('#Phone').css('border-color', 'lightgrey');
            $('#Time').css('border-color', 'lightgrey');
            $('#location').css('border-color', 'lightgrey');
            $('#Meta_description').css('border-color', 'lightgrey');
            $('#Meta_Title').css('border-color', 'lightgrey');
            $('#Meta_Keyword').css('border-color', 'lightgrey')
            var sl = document.getElementById('statusLocation');
            var sp = document.getElementById('StatusPhone');
            var st = document.getElementById('StatusTime');
            var sm = document.getElementById('StatusMail');
            var sI = document.getElementById('statusIp');
            var sT = document.getElementById('StatusMeta_Title');
            var sK = document.getElementById('StatusMeta_Keyword');
            var sD = document.getElementById('StatusMeta_description');
            var sicon = document.getElementById('StatusMetaIcon1');
            var sicon1 = document.getElementById('StatusIcon');
            $('#IDlocation').val(result[1].Id);
            $('#location').val(result[1].value);
            if (result[1].Status) {

                sl.checked = true;
            } else {
                sl.checked = false;
            }
            ///Time
            $('#IDTime').val(result[2].Id);
            $('#Time').val(result[2].value);
            if (result[2].Status) {

                st.checked = true;
            } else {
                st.checked = false;
            }
            //Phone
            $('#IDPhone').val(result[3].Id);
            $('#Phone').val(result[3].value);
            if (result[3].Status) {

                sp.checked = true;
            } else {
                sp.checked = false;
            }
            //mail
            $('#IDMail').val(result[4].Id);
            $('#Mail').val(result[4].value);
            if (result[4].Status) {

                sm.checked = true;
            } else {
                sm.checked = false;
            }
            $('#IDIP').val(result[0].Id);
            $('#ip').val(result[0].value);
            if (result[0].Status) {

                sI.checked = true;
            } else {
                sI.checked = false;
            }
            $('#IDMeta_Title').val(result[5].Id);
            $('#Meta_Title').val(result[5].value);
            if (result[5].Status) {

                sT.checked = true;
            } else {
                sT.checked = false;
            }
            $('#IDMeta_Keyword').val(result[6].Id);
            $('#Meta_Keyword').val(result[6].value);
            if (result[6].Status) {

                sK.checked = true;
            } else {
                sK.checked = false;
            }
            $('#IDMeta_description').val(result[7].Id);
            $('#Meta_description').val(result[7].value);
            if (result[7].Status) {

                sD.checked = true;
            } else {
                sD.checked = false;
            }
            $('#IDIcon').val(result[9].Id);
            document.getElementById("iconvalue").innerHTML = result[9].value;
            document.getElementById("imgicon").src = result[9].value;
            if (result[8].Status) {

                sicon.checked = true;
            } else {
                sicon.checked = false;
            }
            document.getElementById("icon1value").innerHTML = result[8].value;
            document.getElementById("imgicon1").src = result[8].value;
            $('#IDIcon1').val(result[8].Id);
            if (result[9].Status) {

                sicon1.checked = true;
            } else {
                sicon1.checked = false;
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }

    var x = document.getElementById('StatusMetaIcon1');
    var v = document.getElementById("icon1value").innerHTML;
    if (x.checked == true) {
        var ConfigIcon1 = {
            Id: $('#IDIcon1').val(),
            value: v,
            Status: true,
        }
    } else {
        var ConfigIcon1 = {
            Id: $('#IDIcon1').val(),

            value: v,
            Status: false,
        }
    }
    var x = document.getElementById('StatusIcon');
    var v = document.getElementById("iconvalue").innerHTML;
    if (x.checked == true) {
        var ConfigIcon = {
            Id: $('#IDIcon').val(),
            value: v,
            Status: true,
        }
    } else {
        var ConfigIcon = {
            Id: $('#IDIcon').val(),
            value: v,
            Status: false,
        }
    }
    var x = document.getElementById('statusLocation');
    if (x.checked == true) {
        var ConfigObjLocation = {
            Id: $('#IDlocation').val(),
            value: $('#location').val(),
            Status: true,
        }
    }
    else {
        var ConfigObjLocation = {
            Id: $('#IDlocation').val(),
            value: $('#location').val(),
            Status: false,
        }

    };
    var x = document.getElementById('StatusTime');
    if (x.checked == true) {
        var ConfigObjTime = {
            Id: $('#IDTime').val(),
            value: $('#Time').val(),
            Status: true,
        }
    }
    else {
        var ConfigObjTime = {
            Id: $('#IDTime').val(),
            value: $('#Time').val(),
            Status: false,
        }

    };
    var x = document.getElementById('StatusPhone');
    if (x.checked == true) {
        var ConfigObjPhone = {
            Id: $('#IDPhone').val(),
            value: $('#Phone').val(),
            Status: true,
        }
    }
    else {
        var ConfigObjPhone = {
            Id: $('#IDPhone').val(),
            value: $('#Phone').val(),
            Status: false,
        }

    };
    var x = document.getElementById('StatusMail');
    if (x.checked == true) {
        var ConfigObjMail = {
            Id: $('#IDMail').val(),
            value: $('#Mail').val(),
            Status: true,
        }
    }
    else {
        var ConfigObjMail = {
            Id: $('#IDMail').val(),
            value: $('#Mail').val(),
            Status: false,
        }

    };
    var x = document.getElementById('statusIp');
    if (x.checked == true) {
        var ConfigObjIP = {
            Id: $('#IDIP').val(),
            value: $('#ip').val(),
            Status: true,
        }
    }
    else {
        var ConfigObjIP = {
            Id: $('#IDIP').val(),
            value: $('#ip').val(),
            Status: false,
        }

    };
    var x = document.getElementById('StatusMeta_Title');
    if (x.checked == true) {
        var TitleObjIP = {
            Id: $('#IDMeta_Title').val(),
            value: $('#Meta_Title').val(),
            Status: true,
        }
    }
    else {
        var TitleObjIP = {
            Id: $('#IDMeta_Title').val(),
            value: $('#IDMeta_Title').val(),
            Status: false,
        }

    };
    var x = document.getElementById('StatusMeta_Keyword');
    if (x.checked == true) {
        var KeywordObjIP = {
            Id: $('#IDMeta_Keyword').val(),
            value: $('#Meta_Keyword').val(),
            Status: true,
        }
    }
    else {
        var KeywordObjIP = {
            Id: $('#IDMeta_Keyword').val(),
            value: $('#Meta_Keyword').val(),
            Status: false,
        }

    };
    var x = document.getElementById('StatusMeta_description');
    if (x.checked == true) {
        var DescriptionObjIP = {
            Id: $('#IDMeta_description').val(),
            value: $('#Meta_description').val(),
            Status: true,
        }
    }
    else {
        var DescriptionObjIP = {
            Id: $('#IDMeta_description').val(),
            value: $('#Meta_description').val(),
            Status: false,
        }

    };
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigIcon1),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigIcon),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(DescriptionObjIP),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(KeywordObjIP),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(TitleObjIP),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigObjTime),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigObjIP),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigObjPhone),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigObjMail),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {

        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });
    $.ajax({
        url: "/Config/Update",
        data: JSON.stringify(ConfigObjLocation),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('.brand-logo img').attr("src", result.logo);
        },
        error: function (errormessage) {
            alert(errormessage.Message);
        }
    });

    toastr['success']('Update Successful', 'Success!', {
        closeButton: true,
        tapToDismiss: false,
        positionClass: "toast-top-right"
    });
}

function validate() {
    var isvalid = true;
    var mailformat = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    if ($('#Mail').val().match(mailformat)) {
        $('#Mail').css('border-color', 'lightgrey');
        document.getElementById("vmail").innerHTML = "";
    }
    else {
        if ($('#Mail').val().trim() == "") {
            $('#Mail').css('border-color', 'red');
            document.getElementById("vmail").innerHTML = "The Mail field is requied";
            isvalid = false;
        }
        else {
            $('#Mail').css('border-color', 'red');
            document.getElementById("vmail").innerHTML = "The Mail field hadn't correct format";
            isvalid = false;
        }
    }
    var phoneno = /^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$/;
    if ($('#Phone').val().match(phoneno)) {
        $('#Phone').css('border-color', 'lightgrey');
        document.getElementById("vphone").innerHTML = "";
    }
    else {
        if ($('#Phone').val().trim() == "") {
            $('#Phone').css('border-color', 'red');
            document.getElementById("vphone").innerHTML = "The Phone field is requied";
            isvalid = false;
        }
        else {
            $('#Phone').css('border-color', 'red');
            document.getElementById("vphone").innerHTML = "The Phone field hadn't correct format";
            isvalid = false;
        }
    }
    if ($('#Time').val().trim() == "") {
        $('#Time').css('border-color', 'red');
        document.getElementById("vtime").innerHTML = "The Time field is requied";
        isvalid = false;
    }
    else {
        $('#Time').css('border-color', 'lightgrey');
        document.getElementById("vtime").innerHTML = "";
    }
    if ($('#location').val().trim() == "") {
        $('#location').css('border-color', 'red');
        document.getElementById("vlocation").innerHTML = "The location field is requied";
        isvalid = false;
    }
    else {
        $('#location').css('border-color', 'lightgrey');
        document.getElementById("vlocation").innerHTML = "";
    }
    if ($('#Meta_description').val().trim() == "") {
        $('#Meta_description').css('border-color', 'red');
        document.getElementById("vmetadescription").innerHTML = "The Meta description field is requied";
        isvalid = false;
    }
    else {
        $('#Meta_description').css('border-color', 'lightgrey');
        document.getElementById("Meta_description").innerHTML = "";
    }
    if ($('#Meta_Title').val().trim() == "") {
        $('#Meta_Title').css('border-color', 'red');
        document.getElementById("vmetatitle").innerHTML = "The Meta Title field is requied";
        isvalid = false;
    }
    else {
        $('#Meta_Title').css('border-color', 'lightgrey');
        document.getElementById("Meta_Title").innerHTML = "";
    }
    if ($('#Meta_Keyword').val().trim() == "") {
        $('#Meta_Keyword').css('border-color', 'red');
        document.getElementById("vmetakeyword").innerHTML = "The Meta Keyword field is requied";
        isvalid = false;
    }
    else {
        $('#Meta_Keyword').css('border-color', 'lightgrey');
        document.getElementById("Meta_Keyword").innerHTML = "";
    }
    if ($('#ip').val().trim() == "") {
        $('#ip').css('border-color', 'red');
        document.getElementById("vip").innerHTML = "The IP field is requied";
        isvalid = false;
    }
    else {
        $('#ip').css('border-color', 'lightgrey');
        document.getElementById("vip").innerHTML = "";
    }
    return isvalid;
}