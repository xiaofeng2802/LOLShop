(function ($) {
    function initWheel(data) {
        function resizeWheel() {
            var initw = ($('body').width() / 2) + 200;
           if (initw < 300) {
               $('#canvas').width(300);
               $('#canvas').height(300);

           } else {
               $('#canvas').width(initw);
               $('#canvas').height(initw);

           }
        }

        //function resize() {
        //    $("#canvas").outerHeight($(window).height() - $("#canvas").offset().top - Math.abs($("#canvas").outerHeight(true) - $("#canvas").outerHeight()));
        //}

        //$(document).ready(function () {
        //    resize();
        //    $(window).on("resize", function () {
        //        resize();
        //    });
        //});

        var wheelSpinning = false;
        // Create image in memory.
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             

        // Create new wheel object specifying the parameters at creation time.
        var theWheel = new Winwheel({
            //'drawMode': 'segmentImage',
            'canvasId': 'canvas',
            'numSegments': data.length,     // Specify number of segments.
            'outerRadius': 200,   // Set outer radius so wheel fits inside the background.  // Set font size as desired.
            'drawMode': 'segmentImage',
            //'imageDirection': 'S',
            'segments': data, // Define segments including colour and text.
            'animation':           // Specify the animation to use.
            {
                'type': 'spinToStop',
                'duration': 7,     // Duration in seconds.
                'spins': 8,     // Number of complete spins.
                'clearTheCanvas': false,
                'callbackFinished': function () {
                    // Get the segment indicated by the pointer on the wheel background which is at 0 degrees.
                    var winningSegment = theWheel.getIndicatedSegment();

                    $.ajax({
                        url: '/User/SaveWheelHistory?priceId=' + winningSegment.id,
                        async: false,
                        type: 'GET',
                        dataType: 'json',
                        success: function (data) {
                            alert(data.message);
                            if (data.point) {
                                $('#currentPoint').html(data.point + " Point.");
                            }

                            if (!data.unLucky) {
                                window.location.href = "https://www.facebook.com/ShopAccThoCon/?ref=ts&fref=ts";
                            }

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
                    $("#flat-slider").slider("enable");

                }
            }
        });

        // Vars used by the code in this page to do power controls.
        // Create image in memory.


        // -------------------------------------------------------
        // Function to handle the onClick on the power buttons.
        // -------------------------------------------------------

        // -------------------------------------------------------
        // Click handler for spin button.
        // -------------------------------------------------------
        function startSpin(e, power) {
            // Ensure that spinning can't be clicked again while already running.
            

            if (wheelSpinning === false) {
                // Based on the power level selected adjust the number of spins for the wheel, the more times is has
                // to rotate with the duration of the animation the quicker the wheel spins.
                theWheel.animation.spins = power;

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
        $('#flat-slider').slider({
            orientation: 'vertical',
            range: false,
            value: 19,
            max: 20,
            animate: "fast",
            slide: function (event, ui) { },
            stop: function (event, ui) {
                $.ajax({
                    url: '/User/GetRandomWheelItem',
                    async: false,
                    type: 'GET',
                    success: function (data) {
                        $("#flat-slider").slider("disable");
                        startSpin(data, (20 - ui.value));
                        $("#flat-slider").slider("value", 19);

                    },
                    error: function (error) {
                        alert("Đã xảy ra lỗi với vòng quay hoặc bạn không đủ số Point(15 points/ lần), bạn hãy liên hệ Admin page để tìm hiểu thêm.");
                    }
                });
            }
        });
     
    }
    $.get('/Management/ReadAllWheelItem', function (data) {
        if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
            alert("Vòng quay may mắn hiện tại chỉ hỗ trợ trên máy tính. Cám ơn bạn.");
            window.location.href = "http://shopthocon.vn/";

        } else {
            initWheel(data);
        }
    });

}(jQuery));