$(document).ready(function() {
    $("#formSell").submit(function (event) {
        var result = confirm("Bạn có muốn mua tài khoản này không ?");
        if (!result) {
            event.preventDefault();
        }
    });
});