(function ($) {




    function initWheel(data) {
        resizeWheel();

        function resizeWheel() {
            var initw = ($('body').width() / 2) + 100;
           if (initw < 300) {
               $('#canvas').width(300);
               $('#canvas').height(300);

           } else {
               $('#canvas').width(initw);
               $('#canvas').height(initw);

           }
       }
        $(window).resize(function() {
            resizeWheel();

        });

        $('#prizePointer').click(function () {
            $.ajax({
                url: '/User/GetRandomWheelItem',
                async: false,
                type: 'GET',
                success: function(data) {
                    startSpin(data);
                },
                error: function(data) {
                    alert("Bạn không đủ số Point để quay, xin vui lòng nạp thêm thẻ. Cám ơn!");
                }
            });
        });

        var wheelPower = 3;
        var wheelSpinning = false;
        // Create image in memory.


        // Create new wheel object specifying the parameters at creation time.
        var theWheel = new Winwheel({
            'drawMode': 'segmentImage',
            'canvasId': 'canvas',
            'numSegments': data.length,     // Specify number of segments.
            'outerRadius': 212,
            'innerRadius': 50,   // Set outer radius so wheel fits inside the background.  // Set font size as desired.
            //'drawMode': 'segmentImage',
            //'imageDirection': 'S',
            'segments': data, // Define segments including colour and text.
            'animation':           // Specify the animation to use.
            {
                'type': 'spinToStop',
                'duration': 5,     // Duration in seconds.
                'spins': 8,     // Number of complete spins.
                'callbackFinished': function () {
                    // Get the segment indicated by the pointer on the wheel background which is at 0 degrees.
                    var winningSegment = theWheel.getIndicatedSegment();

                    $.ajax({
                        url: '/User/SaveWheelHistory?priceId=' + winningSegment.id,
                        async: false,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            alert(data);
                        },
                        error: function (data) {
                            alert("Bạn không đủ số Point để quay, xin vui lòng nạp thêm thẻ. Cám ơn!");
                        }
                    });

                    // Do basic alert of the segment text. You would probably want to do something more interesting with this information.
                    

               

                    theWheel.stopAnimation(false);  // Stop the animation, false as param so does not call callback function.
                    theWheel.rotationAngle = 0;     // Re-set the wheel angle to 0 degrees.
                    theWheel.draw();
                    wheelSpinning = false;
                  
                }
            }
        });

        // Vars used by the code in this page to do power controls.

     
        // -------------------------------------------------------
        // Function to handle the onClick on the power buttons.
        // -------------------------------------------------------

        // -------------------------------------------------------
        // Click handler for spin button.
        // -------------------------------------------------------
        function startSpin(e) {
            // Ensure that spinning can't be clicked again while already running.
            

            if (wheelSpinning === false) {
                // Based on the power level selected adjust the number of spins for the wheel, the more times is has
                // to rotate with the duration of the animation the quicker the wheel spins.
                if (wheelPower === 1) {
                    theWheel.animation.spins = 3;
                }
                else if (wheelPower === 2) {
                    theWheel.animation.spins = 8;
                }
                else if (wheelPower === 3) {
                    theWheel.animation.spins = 15;
                }

                // Disable the spin button so can't click again while wheel is spinning.
                //document.getElementById('spin_button').src = "spin_off.png";
                //document.getElementById('spin_button').className = "";

                // Begin the spin animation by calling startAnimation on the wheel object.

                if (e) {
                    var stopAt = theWheel.getRandomForSegment(e);
                    theWheel.animation.stopAngle = stopAt;
                    theWheel.startAnimation();

                }

                // Set to true so that power can't be changed and spin button re-enabled during
                // the current animation. The user will have to reset before spinning again.
                wheelSpinning = true;
            }
        }

        // -------------------------------------------------------
        // Function for reset button.
        // -------------------------------------------------------
   
        // -------------------------------------------------------
        // Called when the spin animation has finished by the callback feature of the wheel because I specified callback in the parameters.
        // -------------------------------------------------------
     
     
    }

    $(document).ready(function() {
        $.get('/Management/ReadAllWheelItem', function (data) {
            initWheel(data);
        });


        $(".slider")

     .slider({
         min: 0,
         max: 3,
         orientation: "vertical"
     })

     .slider("pips", {
         rest: "label",
         step: "3"
     });
    });

}(jQuery));