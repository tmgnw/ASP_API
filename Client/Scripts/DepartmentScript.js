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
                html += '<td>' + momen(Dept.CreateDate).format('DD-MM-YYYY') + '</td>';
                html += '<td>' + momen(Dept.UpdateDate).format('DD-MM-YYYY') + '</td>';
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
            });
        }
        else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
        }
    });
}