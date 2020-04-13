var Departments = [];
//------------------------------------------------------------//
$(document).ready(function () {
    table = $('#Devisi').dataTable({
        "ajax": {
            url: "/Devisi/LoadDevisi",
            type: "GET",
            dataType: "json",
            dataSrc: "",
        },
        "columnDefs": [
            { "orderable": false, "targets": 4 },
            { "searchable": false, "targets": 4 }
        ],
        //dom: 'Bfrtip',
        //buttons: [
        //    'csv', 'excel', 'pdf'
        //],
        "columns": [
            { "data": "Name" },
            { "data": "DepartmentName" },
            {
                "data": "CreateDate", "render": function (data) {
                    return moment(data).format('DD MMMM YYYY, h:mm a');
                }
            },
            {
                "data": "UpdateDate", "render": function (data) {
                    var dateupdate = "Not Updated Yet";
                    var nulldate = null;
                    if (data == nulldate) {
                        return dateupdate;
                    } else {
                        return moment(data).format('DD MMMM YYYY, h:mm a');
                    }
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return " <td><button type='button' class='btn btn-warning' id='Update' onclick=GetById('" + row.Id + "');>Edit</button> <button type='button' class='btn btn-danger' id='Delete' onclick=Delete('" + row.Id + "');>Delete</button ></td >";
                }
            },
        ]
    });
});
//------------------------------------------------------------//
document.getElementById("Add2").addEventListener("click", function () {
    $('#Id').val('');
    $('#Name').val('');
    //$('#DepartmentOption').val('');
    $('#Save').show();
    $('#Update').hide();
    loadDepartment2($('#DepartmentOption'));
});
//------------------------------------------------------------//
function loadDepartment2(element) {
    if (Departments.length == 0) {
        $.ajax({
            type: "GET",
            url: "/Department/LoadDepartment",
            success: function (data) {
                Departments = data;
                renderDepartment(element);
            }
        })
    }
    else {
        renderDepartment(element);
    }
} //Load data department
function renderDepartment(element) {
    var $option = $(element);
    $option.empty();
    $option.append($('<option/>').val('0').text('Select Department').hide());
    $.each(Departments, function (i, val) {
        $option.append($('<option/>').val(val.Id).text(val.Name));
    })
} 
//------------------------------------------------------------//
function GetById(Id) {
    $.ajax({
        url: "/Devisi/GetById/" + Id,
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        async: false,
        success: function (result) {
            const obj = JSON.parse(result);
            $('#Id').val(obj.Id);
            $('#Name').val(obj.Name);
            $('#DepartmentOption').val(obj.DepartmentId);
            $('#myModalDiv').modal('show');
            $('#Update').show();
            $('#Save').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responsText);
        }
    })
}
//------------------------------------------------------------//
function Save() {
    $.fn.dataTable.ext.errMode = 'none';
    table = $('#Devisi').DataTable({
        "ajax": {
            url: "/Devisi/LoadDevisi"
        }
    });
    var Divisi = new Object();
    Divisi.Name = $('#Name').val();
    Divisi.DepartmentId = $('#DepartmentOption').val();
    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: '/Devisi/InsertOrUpdate/',
            data: Divisi
        }).then((result) => {
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Devisi Add Successfully',
                    timer: 2500
                }).then(function () {
                    $('#myModalDiv').modal('hide');
                    table.ajax.reload();
                    $('#Id').val('');
                    $('#Name').val('');
                    $('#DepartmentOption').val('');
                });
            }
            else {
                Swal.fire('Error', 'Failed to Add', 'error');
            }
        })
    }
}
//------------------------------------------------------------//
function Edit() {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Devisi').DataTable({
        "ajax": {
            url: "/Devisi/LoadDevisi"
        }
    });
    var Division = new Object();
    Division.Id = $('#Id').val();
    Division.Name = $('#Name').val();
    Division.DepartmentId = $('#DepartmentOption').val();
    if ($('#Name').val() == "") {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Name Cannot be Empty',
        })
        return false;
    } else {
        $.ajax({
            type: 'POST',
            url: '/Devisi/InsertOrUpdate',
            data: Division
        }).then((result) => {
            debugger;
            if (result.StatusCode == 200) {
                Swal.fire({
                    icon: 'success',
                    potition: 'center',
                    title: 'Division Update Successfully',
                    timer: 2500
                }).then(function () {
                    $('#myModalDiv').modal('hide');
                    table.ajax.reload();
                    $('#Id').val('');
                    $('#Name').val('');
                    $('#DepartmentOption').val('');
                });
            } else {
                Swal.fire('Error', 'Failed to Update', 'error');
            }
        })
    }
}
//------------------------------------------------------------//
function Delete(Id) {
    $.fn.dataTable.ext.errMode = 'none';
    var table = $('#Devisi').DataTable({
        "ajax": {
            url: "/Devisi/LoadDevisi"
        }
    });
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.value) {
            debugger;
            $.ajax({
                url: "/Devisi/Delete/",
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
                        $('#myModalDiv').modal('hide');
                        table.ajax.reload();
                        $('#Id').val('');
                        $('#Name').val('');
                        $('#DepartmentOption').val('');
                    });
                }
                else {
                    Swal.fire({
                        icon: 'error',
                        title: 'error',
                        text: 'Failed to Delete',
                    })
                }
            })
        }
    });
}