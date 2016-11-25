$(document)
    .ready(function () {
        $.get('/Management/SkinDataSource',
            function (data) {
                $('.typeahead').typeahead({ source:data });
            });
    });