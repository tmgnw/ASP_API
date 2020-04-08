$(document).ready(function () {
    $('#Edit').hide();
    loadDepartment();

    $(function () {
        $('[data-toggle="tooltip"]').tooltip()
    })
    //$('#Department').dataTable({
    //    "ajax": loadDepartment(),
    //    "responsive": true,
    //});
    //$('[data-toggle="tooltip"]').tooltip();
});

//document.getElementById("Add").addEventListener("Click", function () {
//    ClearScreen();
//});

function loadDepartment() {
    $.ajax({
        url: "/Department/LoadDepartment",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
    }).done(function (data) {
        $('#Department').dataTable({
            "data": data,
            "columns": [
                //{ "data": "Id" },
                { "data": "Name" },
                {
                    "data": "CreateDate", "render": function (data) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                },
                {
                    "data": "UpdateDate", "render": function (data) {
                        var text = "Not Update Yet";
                        var nulldate = "";
                        if (data == nulldate) {
                            return text();
                        } else {
                            return moment(data).format('DD/MM/YYYY');
                        }
                    }
                },
                {
                    data: null, orderable: false, render: function (data, type, row) {
                        return '<button type="button" class="btn btn-warning" id="Update" data-toggle="tooltip" data-placement="top" title="Update" onclick="return GetById(' + row.Id + ')"><i class="mdi mdi-pencil"></i></button> &nbsp; <button type="button" class="btn btn-danger" id="Delete" data-toggle="tooltip" data-placement="top" title="Delete" onclick="return Delete(' + row.Id + ')"><i class="mdi mdi-delete"></i></button>';
                    }
                }
            ]
        })
        /*.fnReloadAjax();*/
        /*.ajax.reload();*/
    });
}

//function loadDepartment() {
//    $.ajax({
//        url: "/Department/LoadDepartment",
//        type: "GET",
//        contentType: "application/json;charset=utf-8",
//        dataType: "json",
//        success: function (result) {
//            debugger;
//            var html = '';
//            $.each(result, function (key, Dept) {
//                html += '<tr>';
//                html += '<td>' + Dept.Name + '</td>';
//                html += '<td>' + moment(Dept.CreateDate).format('DD-MM-YYYY') + '</td>';
//                html += '<td>' + moment(Dept.UpdateDate).format('DD-MM-YYYY') + '</td>';
//                //if (Department.UpdateDate == null) {
//                //    html += '<td> Not Updated yet </td>';
//                //}
//                //else {
//                //    html += '<td>' + moment(Dept.UpdateDate).format('DD-MM-YYYY') + '</td>';
//                //}
//                html += '<td><button type="button" class="btn btn-warning" id="Update" onclick="return GetById(' + Dept.Id + ')">Edit </button>';
//                html += '<button type="button" class="btn btn-danger" id="Delete" onclick="return Delete(' + Dept.Id + ')">Delete </button></td>';
//                html += '</tr>';
//            });
//            $('.deptbody').html(html);
//        },
//        error: function (errormessage) {
//            alert(errormessage.responseText)+
//        }
//    });
//}

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
                icon: 'success',
                title: 'Department Added Successfully'
            }).then((result) => {
                if (result.value) {
                    //table.ajax.reload()
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

function GetById(Id) {
    debugger;
    $.ajax({
        url: "/Department/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.Name);
            $('#myModal').modal('show');
            $('#Update').show();
            $('#Save').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText)
        }
    });
}

function Edit() {
    var Department = new Object();
    Department.Id = $('#Id').val();
    Department.Name = $('#Name').val();
    $.ajax({
        type: "POST",
        url: "/Department/InsertOrUpdate",
        data: Department
    }).then((result) => {
        debugger;
        if (result.StatusCode == 200) {
            Swal.fire({
                position: 'center',
                icon: 'success',
                title: 'Department Added Successfully',
                timer: 2000
            }).then(function () {
                location.reload();
                ClearScreen();
                //table.ajax.reload();
            });
        }
        else {
            Swal.fire('Error', 'Failed to Input', 'error');
            ClearScreen();
            //table.ajax.reload();
        }
    });
}

function Delete(Id) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/Department/Delete/",
                data: { Id: Id }
            }).then((result) => {
                debugger;
                if (result.StatusCode == 200) {
                    Swal.fire({
                        icon: 'success',
                        position: 'center',
                        title: 'Delete Successfully',
                        timer: 2000
                    }).then(function () {
                        //table.ajax.reload();
                        location.reload();
                        ClearScreen();
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'error',
                        text: 'Failed to Delete',
                    })
                    //table.ajax.reload();
                    ClearScreen();
                }
            })
        }
    });
}