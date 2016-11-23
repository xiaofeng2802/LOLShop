$(document).ready(function() {
    $('span[data-fake-pass]').mousedown(function (e) {
        if (e.button === 0) {
            $(this).hide();
            $(this).next().show();
        }
        return true;
    });

    $(document).mouseup(function (e) {
        if (e.button === 0) {
            $('span[data-fake-pass]').show();
            $('span[data-pass]').hide();
        }
        return true;
    });
});