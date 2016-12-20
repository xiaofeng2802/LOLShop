$(document).ready(function() {
    $('#deleteButton').on('click', function(event) {
        var result = confirm("Bạn có muốn xóa dữ liệu này ko ?");
        if (!result) {
            event.preventDefault();
        }
    });

    $('#applyAll').on('click', function (event) {
        var result = confirm("Bạn có muốn cập nhật hết không?");
        if (!result) {
            event.preventDefault();
        }
    });
});





