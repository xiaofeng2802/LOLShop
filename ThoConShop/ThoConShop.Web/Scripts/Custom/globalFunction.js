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
    $('#myModal').modal();

    var options = [];

$( '.dropdown-menu a' ).on( 'click', function( event ) {

   var $target = $( event.currentTarget ),
       val = $target.attr( 'data-value' ),
       $inp = $target.find( 'input' ),
       idx;

   //if ( ( idx = options.indexOf( val ) ) > -1 ) {ssss
   //   options.splice( idx, 1 );
   //   setTimeout( function() { $inp.prop( 'checked', false ) }, 0);
   //} else {
   //   options.push( val );
   //   setTimeout( function() { $inp.prop( 'checked', true ) }, 0);
   //}

      
   console.log(options);
   if (val) {
         return false;
   }
    return true;
});

    //// Get the modal
    //var modal = document.getElementById('myModal');

    //// Get the button that opens the modal

    //// Get the <span> element that closes the modal

    //// When the user clicks on the button, open the modal

    //// When the user clicks on <span> (x), close the modal
    //$('.close-custom').click(function () {
    //    modal.style.display = "none";
    //});


    //// When the user clicks anywhere outside of the modal, close it
    //$(window).click(function () {
    //    if (event.target === modal) {
    //        modal.style.display = "none";
    //    }
    //});
    //modal.style.display = "block";
});