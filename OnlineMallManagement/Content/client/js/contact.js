
$('.rd-mailform button').on("click", function (e) {
    if (ValidateContact()) {

        var model = {
            Email: $('#contact-email').val(),
            FirstName: $('#contact-first-name').val(),
            LastName: $('#contact-last-name').val(),
            Phone: $('#contact-phone').val(),
            Content: $('#contact-message').val()
        };

        $.ajax({
            type: "POST",
            url: "/Contact/CreateFeedback",
            data: JSON.stringify(model),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (res) {
                if (res.status) {
                    toastr.success('Successfully sent', 'Success!', {
                        closeButton: true,
                        tapToDismiss: false,
                        positionClass: "toast-bottom-right",
                        progressBar: true
                    });
                } else {
                    toastr.error('Aw, snap! Something went wrong.', 'Error!', {
                        closeButton: true,
                        tapToDismiss: false,
                        positionClass: "toast-bottom-right",
                        progressBar: true
                    });
                }

                    $('#contact-email').val(''),
                    $('#contact-first-name').val(''),
                    $('#contact-last-name').val(''),
                    $('#contact-phone').val(''),
                    $('#contact-message').val('')
            },
            error: function (data) {
                toastr.error('Aw, snap! Something went wrong.', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false,
                    positionClass: "toast-bottom-right",
                    progressBar: true
                });

                    $('#contact-email').val(''),
                    $('#contact-first-name').val(''),
                    $('#contact-last-name').val(''),
                    $('#contact-phone').val(''),
                    $('#contact-message').val('')
            }
        });
    }
});

function ValidateContact() {
    var check = true;

    var _email = $('#contact-email').val();
    var _firstName = $('#contact-first-name').val();
    var _lastName = $('#contact-last-name').val();
    var _phone = $('#contact-phone').val();
    var _content = $('#contact-message').val();

    if (_firstName == '') {
        $('#contact-first-name').next().html('The text field is required.');
        check = false;
    } else {
        $('#contact-first-name').next().html('');
    }

    if (_lastName == '') {
        $('#contact-last-name').next().html('The text field is required.');
        check = false;
    } else {
        $('#contact-last-name').next().html('');
    }

    if (_content == '') {
        $('#contact-message').next().html('The text field is required.');
        check = false;
    } else {
        $('#contact-message').next().html('');
    }

    if (_email == '') {
        $('#contact-email').next().html('The text field is required.');
        check = false;
    } else if (!_email.match(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/)) {
        $('#contact-email').next().html('The email is not a valid email.');
        check = false;
    } else {
        $('#contact-email').next().html('');
    }

    if (_phone == '') {
        $('#contact-phone').next().html('The text field is required.');
        check = false;
    } else if (!_phone.match(/^(\+\d)?[0-9\-\(\) ]{5,}$/i)) {
        $('#contact-phone').next().html('Invalid phone number format');
        check = false;
    } else {
        $('#contact-phone').next().html('');
    }

    return check;
}

