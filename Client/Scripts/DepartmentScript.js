$(document).ready(function () {
    $('#Department').dataTable({
        "ajax": loadDepartment(),
        "responsive": true,
    });
    $('[data-toggle="tooltip"]').tooltip();
});

function loadDepartment() {
    $.ajax({
        url: "/Department/LoadDepartment",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            debugger;
            var html = '';
            $.each(result, function (key, Dept) {
                html += '<tr>';
                html += '<td>' + Dept.Name + '</td>';
                html += '<td>' + moment(Dept.CreateDate).format('DD-MM-YYYY') + '</td>';
                if (Department.UpdateDate == null) {
                    html += '<td> Not Updated yet </td>';
                }
                else {
                    html += '<td>' + moment(Dept.UpdateDate).format('DD-MM-YYYY') + '</td>';
                }
                html += '<td><button type="button" class="btn btn-warning" id="Update" onclick="return GetById(' + Dept.Id + ')">Edit </button>';
                html += '<button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + Dept.Id + ')">Delete </button></td>';
                html += '</tr>';
            });
            $('.deptbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText)
        }
    });
}

function Save() {
    debugger;
    var Department = new Object();
    Department.Name = $('#Name').val();
    $.ajax({
        url: "/Department/InsertOrUpdate",
        type: "POST",
        data: Department
    }).then((result) => {
        debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Department Added Successfully'
            }).then((result) => {
                if (result.value) {
                    location.reload()
                }
            });
        }
        else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    });
}

function ClearScreen() {
    $('#Id').val('');
    $('#Name').val('');
    $('#Edit').hide();
    $('#Save').show();
}

//function GetById(Id) {
//    $.ajax({
//        url: "/Department/GetById/" + Id,
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        async: false,
//        success: function (result) {
//            const obj = JSON.parse(result);
//            $('#Id').val('obj.Id');
//            $('#Name').val('obj.Name');
//            $('#myModal').modal('show');
//            $('#Update').show();
//            $('#Save').hide();
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText)
//        }
//    });
//}

function Delete(Id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        debugger;
        $.ajax({
            url: "/Department/Delete/",
            data: { Id: Id }
        }).then((result) => {
            if (result.value) {
                debugger;
                if (result.StatusCode == 200) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully',
                        timer: 1000
                    });
                    window.location.href = "/Department";
                }
                else {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            }
        })
    });
}