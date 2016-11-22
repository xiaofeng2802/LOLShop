(function ($) {

    function initWheel(data) {

        $('#quay').click(function () {
            $.get('/User/GetRandomWheelItem', function(data) {
                startSpin(data);
            });
        });

        var wheelPower = 3;
        var wheelSpinning = false;

        // Create new wheel object specifying the parameters at creation time.
        var theWheel = new Winwheel({
            'drawText': true,
            'canvasId': 'canvas',
            'numSegments': data.length,     // Specify number of segments.
            'outerRadius': 212,   // Set outer radius so wheel fits inside the background.
            'textFontSize': 28,    // Set font size as desired.
            //'drawMode': 'segmentImage',
            //'imageDirection': 'S',
            'segments': data, // Define segments including colour and text.
            //[
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 1' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 2' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 3' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 4' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 5' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 6' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 7' },
            //   { 'image': '../Images/LuckyItem/d9d68421-cc8c-4339-8a90-9b74f491d40c.jpg', 'text': 'Prize 8' }
            //],
            'animation':           // Specify the animation to use.
            {
                'type': 'spinToStop',
                'duration': 5,     // Duration in seconds.
                'spins': 8,     // Number of complete spins.
                'callbackFinished': function () {
                    // Get the segment indicated by the pointer on the wheel background which is at 0 degrees.
                    var winningSegment = theWheel.getIndicatedSegment();

                    // Do basic alert of the segment text. You would probably want to do something more interesting with this information.
                    alert("You have won " + winningSegment.text);

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
        function powerSelected(powerLevel) {
            // Ensure that power can't be changed while wheel is spinning.
            if (wheelSpinning === false) {
                // Reset all to grey incase this is not the first time the user has selected the power.
                document.getElementById('pw1').className = "";
                document.getElementById('pw2').className = "";
                document.getElementById('pw3').className = "";

                // Now light up all cells below-and-including the one selected by changing the class.
                if (powerLevel >= 1) {
                    document.getElementById('pw1').className = "pw1";
                }

                if (powerLevel >= 2) {
                    document.getElementById('pw2').className = "pw2";
                }

                if (powerLevel >= 3) {
                    document.getElementById('pw3').className = "pw3";
                }

                // Set wheelPower var used when spin button is clicked.
                wheelPower = powerLevel;

                // Light up the spin button by changing it's source image and adding a clickable class to it.
                //document.getElementById('spin_button').src = "spin_on.png";
                document.getElementById('spin_button').className = "clickable";
            }
        }

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
        function resetWheel() {
            theWheel.stopAnimation(false);  // Stop the animation, false as param so does not call callback function.
            theWheel.rotationAngle = 0;     // Re-set the wheel angle to 0 degrees.
            theWheel.draw();                // Call draw to render changes to the wheel.

            document.getElementById('pw1').className = "";  // Remove all colours from the power level indicators.
            document.getElementById('pw2').className = "";
            document.getElementById('pw3').className = "";

            wheelSpinning = false;          // Reset to false to power buttons and spin can be clicked again.
        }

        // -------------------------------------------------------
        // Called when the spin animation has finished by the callback feature of the wheel because I specified callback in the parameters.
        // -------------------------------------------------------
        function alertPrize() {
            wheelSpinning = false;
            // Get the segment indicated by the pointer on the wheel background which is at 0 degrees.
            var winningSegment = theWheel.getIndicatedSegment();

            // Do basic alert of the segment text. You would probably want to do something more interesting with this information.
            alert("You have won " + winningSegment.text);
        }
        // Usual pointer drawing code.
        //drawTriangle();

        //function drawTriangle() {
        //    // Get the canvas context the wheel uses.
        //    var ctx = theWheel.ctx;

        //    ctx.strokeStyle = 'navy';  // Set line colour.
        //    ctx.fillStyle = 'aqua';  // Set fill colour.
        //    ctx.lineWidth = 2;
        //    ctx.beginPath();           // Begin path.
        //    ctx.moveTo(170, 5);        // Move to initial position.
        //    ctx.lineTo(230, 5);        // Draw lines to make the shape.
        //    ctx.lineTo(200, 40);
        //    ctx.lineTo(171, 5);
        //    ctx.stroke();              // Complete the path by stroking (draw lines).
        //    ctx.fill();                // Then fill.
        //}
    }

    $(document).ready(function() {
        $.get('/Management/ReadAllWheelItem', function (data) {
            initWheel(data);
        });
    });

    

 

}(jQuery));