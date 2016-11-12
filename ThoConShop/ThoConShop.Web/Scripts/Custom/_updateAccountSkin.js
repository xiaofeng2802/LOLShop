
$(document)
    .ready(function () {
        var accountId = $('#accountId').val();
        $.get('/Management/SkinDataSource/?accountId=' + accountId,
            function (data) {
                $('#skin').tagging(data);
            });
    });