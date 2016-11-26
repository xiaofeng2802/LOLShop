$(document)
    .ready(function () {
        $.get('/Management/SkinDataSource',
            function (data) {
                $('#skinFilter').typeahead({ source: data });
            });
        $.get('/Management/ChampDataSource',
          function (data) {
              $('#champFilter').typeahead({ source: data });
          });
    });