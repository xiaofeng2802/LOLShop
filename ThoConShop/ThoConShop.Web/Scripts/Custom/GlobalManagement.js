$(document).ready(function() {
    $('#deleteButton').click(function(event) {
        var result = confirm("Bạn có muốn xóa dữ liệu này ko ?");
        if (!result) {
            event.preventDefault();
        }
    });
});