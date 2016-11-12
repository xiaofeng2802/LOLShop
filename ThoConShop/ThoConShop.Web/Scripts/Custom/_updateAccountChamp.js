$(document)
    .ready(function () {
        var accountId = $('#accountId').val();
        $.get('/Management/ChampDataSource/?accountId=' + accountId,
            function (data) {
                $('#champ').tagging(data);
            });
    });