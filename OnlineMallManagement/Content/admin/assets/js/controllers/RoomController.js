loadData();

function loadData() {
    $.ajax({
        url: 'Room/loadData',
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            if (res.TotalItems > 0) {
                var data = res.RoomList;
                var html = '';
                var template = $('#room-list').html();
                $.each(data, function (i, item) {
                    html += Mustache.render(template, {
                        RoomId: item.IdRoom,
                        RoomName: item.RoomName
                    });
                });
            }

            $('.room_table tbody').html(html);
        }
    })
}

$(document).on('click', '#btn-add-room', function () {
    var roomName = $('#room-name').val();
    var roomId = $('#room-id').val();
  
    var model = {
        IdRoom: roomId,
        RoomName: roomName
    };

    if (Validate()) {
        AddRoom(model);
    }

});

function AddRoom(model) {
    $.ajax({
        url: 'Room/Create',
        type: 'POST',
        data: model,
        success: function (res) {
            if (res.success && !res.edit) {
                toastr.success('Create successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            } else if (!res.success && !res.edit) {
                toastr.error('The Cinema Hall already exists!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }

            if (res.success && res.edit) {
                toastr.success('Update successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            } else if (!res.success && res.edit) {
                toastr.error('The Cinema Hall already exists!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }
            $('#room-id').val(0);
            loadData();
        }
    });
}

$(document).on('click', '.room-edit', function () {
    var id = Number($(this).data('id'));

    GetDataById(id);
});

function GetDataById(_id) {
    $.ajax({
        url: 'Room/GetById',
        type: 'GET',
        data: { id: _id },
        success: function (res) {

            $('.card-title').html("Edit Cinema Hall");

            $('#room-id').val(res.IdRoom);
            $('#room-name').val(res.RoomName);
        }
    });
}

$(document).on('click', '.room-delete', function () {
    var id = Number($(this).data('id'));

    DeleteRoom(id);
});

function DeleteRoom(id) {
    $.ajax({
        url: 'Room/Delete',
        type: 'POST',
        data: { id: id },
        success: function (res) {
            if (res.success) {
                loadData();

                $('#room-name').val('');
                 $('#room-id').val(0);

                toastr.success('Delete successful!', 'Success!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            } else {
                toastr.error('Cinema hall contains links to seats!', 'Error!', {
                    closeButton: true,
                    tapToDismiss: false
                });
            }
        }
    });
}

function Validate() {
    var check = true;
    var roomName = $('#room-name').val();

    if (roomName === '') {
        $('#room-name').parent().next().html('Name cannot be empty!');
        check = false;
    } else {
        $('#room-name').parent().next().html('');
    }

    return check;
}