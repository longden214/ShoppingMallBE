$(document).on('click', '.btn-save-account', function () {
    var _uname = $('#account_username').val();
    var _name = $('#account_name').val();
    var _phone = $('#account_phone').val();
    var _email = $('#account_email').val();

    var model = {
        UserName: _uname,
        DisplayName: _name,
        Phone: _phone,
        Email: _email
    };

    if (ValidateAccount()) {
        editAccount(model);
    }

});


function editAccount(model) {
    $.ajax({
        url: 'Account/EditAccount',
        type: 'POST',
        data: model,
        success: function (res) {
            if (res.success) {
                toastr.success('Update successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });

                $('.acc-displayName').html(res.displayName);
            } else {
                toastr.error('Update failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }
        }
    });
}

function ValidateAccount() {
    var check = true;
    var _uname = $('#account_username').val();
    var _name = $('#account_name').val();
    var _email = $('#account_email').val();
    var _phone = $('#account_phone').val();

    if (_uname === '') {
        $('#account_username').next().html('UserName cannot be empty!');
        check = false;
    } else {
        $('#account_username').next().html(''); 

    }

    if (_name === '') {
        $('#account_name').next().html('DisplayName cannot be empty!');
        check = false;
    } else {
        $('#account_name').next().html('');

    }

    if (_email === '') {
        $('#account_email').next().html('Email cannot be empty!');
        check = false;
    } else if (!_email.match(/^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/)) {
        $('#account_email').next().html('Invalid email!');
        check = false;
    } else {
        $('#account_email').next().html('');
    }

    if (_phone === '') {
        $('#account_phone').next().html('Phone cannot be empty!');
        check = false;
    } else if (!_phone.match(/^(\+\d)?[0-9\-\(\) ]{5,}$/i)) {
        $('#account_phone').next().html('Invalid phone!');
        check = false;
    } else {
        $('#account_phone').next().html('');
    }

    return check;
}

$(document).on('click', '.btn-change-password', function () {
    var _pw = $('#account-retype-new-password').val();

    var model = {
        password: _pw
    };

    if (ValidateAccountPassword()) {
        ChangePassword(model);
    }

});

function ChangePassword(model) {
    $.ajax({
        url: 'Account/ChangePassword',
        type: 'POST',
        data: model,
        success: function (res) {
            if (res.success) {
                toastr.success('Update successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });

                $('#account-old-password').val("");
                $('#account-new-password').val("");
                $('#account-retype-new-password').val("");
            } else {
                toastr.error('Update failed!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }
        }
    });
}

function ValidationPassword(pw) {
    var result = false;
    $.ajax({
        url: 'Account/ValidationPassword',
        type: 'POST',
        data: { pass: pw },
        async: false,
        success: function (res) {
            result = res.success
        }
    });

    return result;
}

function ValidateAccountPassword() {
    var check = true;

    var _oldPass = $('#account-old-password').val();
    var _newPass = $('#account-new-password').val();
    var _retypePass = $('#account-retype-new-password').val();

    if (_oldPass === '') {
        $('#account-old-password').parent().next().html('Old Password cannot be empty!');
        check = false;
    } else if (!ValidationPassword(_oldPass)) {
        $('#account-old-password').parent().next().html('Incorrect Old Password!');
        check = false;
    } else {
        $('#account-old-password').parent().next().html('');
    }

    if (_newPass === '') {
        $('#account-new-password').parent().next().html('New Password cannot be empty!');
        check = false;
    } else if (!_newPass.match(/^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$/)) {
        $('#account-new-password').parent().next().html('Password must be at least 8 characters and contain at least 1 number, 1 uppercase letter and 1 lowercase letter!');
        check = false;
    } else {
        $('#account-new-password').parent().next().html('');
    }

    if (_retypePass === '') {
        $('#account-retype-new-password').parent().next().html('Retype New Password cannot be empty!');
        check = false;
    } else if (_retypePass != _newPass) {
        $('#account-retype-new-password').parent().next().html('Does not match New Password!');
        check = false;
    } else {
        $('#account-retype-new-password').parent().next().html('');

    }

    return check;
}