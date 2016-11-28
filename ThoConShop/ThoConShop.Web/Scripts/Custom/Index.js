$(document).ready(function() {
    $("form.formSell").submit(function (event) {
        var result = confirm("Bạn có muốn mua tài khoản này không ?");
        if (!result) {
            event.preventDefault();
        }
    });
});