$(document).ready(function () {
    loadDataTable();
});

var dataTable;
function loadDataTable() {
    dataTable = $('#tableData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "10%" },
            { data: 'author', "width": "20%" },
            { data: 'price', "width": "10%" },
            { data: 'category.name', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                    return ` 
                    <div class="btn-group" role="group">
                        <a href="/admin/product/upsert?id=${data}" class="btn btn-primary">
                            <i class="bi bi-pencil-square"></i> 
                        </a>
                        <a onClick=deleteProduct('/admin/product/delete/${data}') class="btn btn-danger mx-2">
                            <i class="bi bi-trash-fill"></i> 
                        </a>
                    </div>
                    `
                },
                "width": "10%"
            },
        ]
    });
}

function deleteProduct (url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    })
} 
