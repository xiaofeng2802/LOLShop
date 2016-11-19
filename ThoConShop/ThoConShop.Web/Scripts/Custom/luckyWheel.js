﻿(function ($) {
    var venues = [{ "name": "1900 Mexican Grill", "type": "Mexican" }, { "name": "300 East", "type": "American/Upscale" }, { "name": "Angry Ale's", "type": "American/Pub " }, { "name": "Azteca", "type": "Mexican" }, { "name": "Bedder Bedder and Moore", "type": "Sandwiches, Salads, Soups" }, { "name": "Boading", "type": "Chinese" }, { "name": "Brazwell's Premium Pub", "type": "American/Pub " }, { "name": "Brio Tuscan Grille", "type": "Italian" }, { "name": "Brixx", "type": "Pizza/Salads" }, { "name": "Café at 6100", "type": "Home Cooking" }, { "name": "California Pizza Kitchen", "type": "Pizza/Salads" }, { "name": "Chick-Fil-A", "type": "Fast Food" }, { "name": "City Tavern", "type": "American/Upscale" }, { "name": "Copper", "type": "Indian" }, { "name": "Cowfish", "type": "Sushi" }, { "name": "Duckworth's", "type": "American/Pub " }, { "name": "Eddie's Place", "type": "American/Pub " }, { "name": "El Camino", "type": "Mexican" }, { "name": "Fairview Plaza Restaurant", "type": "Home Cooking" }, { "name": "Firebird's Wood Fired Grill", "type": "American/Upscale" }, { "name": "Firehouse Subs", "type": "Sandwiches, Salads, Soups" }, { "name": "Flying Biscuit", "type": "American/Fast" }, { "name": "Fuel", "type": "Pizza" }, { "name": "Good Food on Montford", "type": "American/Upscale" }, { "name": "Harper's Restaurant", "type": "American/Upscale" }, { "name": "Hawthorne's Pizza", "type": "Pizza/Italian" }, { "name": "Luisa's Brick Oven Pizza", "type": "Pizza/Italian" }, { "name": "Maverick Rock Taco", "type": "Mexican" }, { "name": "McAllister's", "type": "Sandwiches, Salads, Soups" }, { "name": "Mellow Mushroom", "type": "Pizza/Salads" }, { "name": "Moe's", "type": "Mexican" }, { "name": "Moosehead Grill", "type": "American/Pub " }, { "name": "Paco's Tacos and Tequila", "type": "Mexican" }, { "name": "Panera ", "type": "Sandwiches, Salads, Soups" }, { "name": "PF Chang's", "type": "Chinese" }, { "name": "Portofinos", "type": "Pizza/Italian" }, { "name": "Qdoba", "type": "Mexican" }, { "name": "Rooster's Wood Fire Kitchen", "type": "American/Upscale" }, { "name": "Rusty's Deli", "type": "Sandwiches, Salads, Soups" }, { "name": "Taco Bell", "type": "Fast Food" }, { "name": "Taco Mac", "type": "American/Pub " }, { "name": "Terrace Café", "type": "American/Upscale" }, { "name": "The Roasting Company", "type": "American/Fast" }, { "name": "Village Tavern", "type": "American/Upscale" }, { "name": "Which Wich?", "type": "Sandwiches, Salads, Soups" }, { "name": "Zack's Hamburgers", "type": "American/Fast" }];

    // Helpers
    var blackHex = '#333',
        whiteHex = '#fff',
        shuffle = function (o) {
            for (var j, x, i = o.length; i; j = parseInt(Math.random() * i), x = o[--i], o[i] = o[j], o[j] = x)
                ;
            return o;
        },
        halfPI = Math.PI / 2,
        doublePI = Math.PI * 2;

    String.prototype.hashCode = function () {
        // See http://www.cse.yorku.ca/~oz/hash.html		
        var hash = 5381,
            i;
        for (i = 0; i < this.length; i++) {
            char = this.charCodeAt(i);
            hash = ((hash << 5) + hash) + char;
            hash = hash & hash; // Convert to 32bit integer
        }
        return hash;
    };

    Number.prototype.mod = function (n) {
        return ((this % n) + n) % n;
    };

    // WHEEL!
    var wheel = {
        timerHandle: 0,
        timerDelay: 33,

        angleCurrent: 0,
        angleDelta: 0,

        size: 290,

        canvasContext: null,

        colors: ['#003366', '#FF6600', '#CCCC00', '#006600', '#3333CC', '#CC0066', '#FF3300', '#009900', '#6600CC', '#33CC33', '#0066CC', '#FF0066', '#3300FF', '#00CC00', '#FFCC00'],

        segments: [],

        seg_colors: [], // Cache of segments to colors

        maxSpeed: Math.PI / 16,

        upTime: 1000, // How long to spin up for (in ms)
        downTime: 5000, // How long to slow down for (in ms)

        spinStart: 0,

        frames: 0,

        centerX: 300,
        centerY: 300,

        spin: function () {
            // Start the wheel only if it's not already spinning
            if (wheel.timerHandle == 0) {
                wheel.spinStart = new Date().getTime();
                wheel.maxSpeed = Math.PI / (16 + Math.random()); // Randomly vary how hard the spin is
                wheel.frames = 0;
                wheel.sound.play();

                wheel.timerHandle = setInterval(wheel.onTimerTick, wheel.timerDelay);
            }
        },

        onTimerTick: function () {
            var duration = (new Date().getTime() - wheel.spinStart),
                progress = 0,
                finished = false;

            wheel.frames++;
            wheel.draw();

            if (duration < wheel.upTime) {
                progress = duration / wheel.upTime;
                wheel.angleDelta = wheel.maxSpeed
						* Math.sin(progress * halfPI);
            } else {
                progress = duration / wheel.downTime;
                wheel.angleDelta = wheel.maxSpeed
						* Math.sin(progress * halfPI + halfPI);
                if (progress >= 1) {
                    finished = true;
                }
            }

            wheel.angleCurrent += wheel.angleDelta;
            while (wheel.angleCurrent >= doublePI) {
                // Keep the angle in a reasonable range
                wheel.angleCurrent -= doublePI;
            }
            if (finished) {
                clearInterval(wheel.timerHandle);
                wheel.timerHandle = 0;
                wheel.angleDelta = 0;

                if (console) { console.log((wheel.frames / duration * 1000) + " FPS"); }
            }

            /*
			// Display RPM
			var rpm = (wheel.angleDelta * (1000 / wheel.timerDelay) * 60) / (Math.PI * 2);
			$("#counter").html( Math.round(rpm) + " RPM" );
			 */
        },

        init: function (optionList) {
            try {
                wheel.initWheel();
                wheel.initAudio();
                wheel.initCanvas();
                wheel.draw();

                $.extend(wheel, optionList);

            } catch (exceptionData) {
                alert('Wheel is not loaded ' + exceptionData);
            }

        },

        initAudio: function () {
            var sound = document.createElement('audio');
            sound.setAttribute('src', 'wheel.mp3');
            wheel.sound = sound;
        },

        initCanvas: function () {
            var canvas = $('#canvas')[0];
            canvas.addEventListener("click", wheel.spin, false);
            wheel.canvasContext = canvas.getContext("2d");
        },

        initWheel: function () {
            shuffle(wheel.colors);
        },

        // Called when segments have changed
        update: function () {
            // Ensure we start mid way on a item
            //var r = Math.floor(Math.random() * wheel.segments.length);
            var r = 0,
                segments = wheel.segments,
			    len = segments.length,
                colors = wheel.colors,
			    colorLen = colors.length,
                seg_color = [], // Generate a color cache (so we have consistant coloring)
                i
            wheel.angleCurrent = ((r + 0.5) / wheel.segments.length) * doublePI;

            for (i = 0; i < len; i++) {
                seg_color.push(colors[segments[i].hashCode().mod(colorLen)]);
            }
            wheel.seg_color = seg_color;

            wheel.draw();
        },

        draw: function () {
            wheel.clear();
            wheel.drawWheel();
            wheel.drawNeedle();
        },

        clear: function () {
            wheel.canvasContext.clearRect(0, 0, 1000, 800);
        },

        drawNeedle: function () {
            var ctx = wheel.canvasContext,
                centerX = wheel.centerX,
                centerY = wheel.centerY,
                size = wheel.size,
                i,
                centerSize = centerX + size,
                len = wheel.segments.length,
                winner;

            ctx.lineWidth = 2;
            ctx.strokeStyle = blackHex;
            ctx.fillStyle = whiteHex;

            ctx.beginPath();

            ctx.moveTo(centerSize - 10, centerY);
            ctx.lineTo(centerSize + 10, centerY - 10);
            ctx.lineTo(centerSize + 10, centerY + 10);
            ctx.closePath();

            ctx.stroke();
            ctx.fill();

            // Which segment is being pointed to?
            i = len - Math.floor((wheel.angleCurrent / doublePI) * len) - 1;

            // Now draw the winning name
            ctx.textAlign = "left";
            ctx.textBaseline = "middle";
            ctx.fillStyle = blackHex;
            ctx.font = "2em Arial";
            winner = wheel.segments[i] || 'Choose at least 1 Venue';
            ctx.fillText(winner, centerSize + 20, centerY);
        },

        drawSegment: function (key, lastAngle, angle) {
            var ctx = wheel.canvasContext,
                centerX = wheel.centerX,
                centerY = wheel.centerY,
                size = wheel.size,
                colors = wheel.seg_color,
                value = wheel.segments[key];

            //ctx.save();
            ctx.beginPath();

            // Start in the centre
            ctx.moveTo(centerX, centerY);
            ctx.arc(centerX, centerY, size, lastAngle, angle, false); // Draw an arc around the edge
            ctx.lineTo(centerX, centerY); // Now draw a line back to the centre

            // Clip anything that follows to this area
            //ctx.clip(); // It would be best to clip, but we can double performance without it
            ctx.closePath();

            ctx.fillStyle = colors[key];
            ctx.fill();
            ctx.stroke();

            // Now draw the text
            ctx.save(); // The save ensures this works on Android devices
            ctx.translate(centerX, centerY);
            ctx.rotate((lastAngle + angle) / 2);

            ctx.fillStyle = whiteHex;
            ctx.fillText(value.substr(0, 20), size - 15, 0);
            ctx.restore();
        },

        drawWheel: function () {
            var ctx = wheel.canvasContext,
                angleCurrent = wheel.angleCurrent,
                lastAngle = angleCurrent,
                len = wheel.segments.length,
                centerX = wheel.centerX,
                centerY = wheel.centerY,
                size = wheel.size,
                angle,
                i;

            ctx.lineWidth = 1;
            ctx.strokeStyle = blackHex;
            ctx.textBaseline = "middle";
            ctx.textAlign = "right";
            ctx.font = "1em Arial";

            for (i = 1; i <= len; i++) {
                angle = doublePI * (i / len) + angleCurrent;
                wheel.drawSegment(i - 1, lastAngle, angle);
                lastAngle = angle;
            }

            // Draw a center circle
            ctx.beginPath();
            ctx.arc(centerX, centerY, 20, 0, doublePI, false);
            ctx.closePath();

            ctx.fillStyle = whiteHex;
            //ctx.strokeStyle = blackHex;
            ctx.fill();
            ctx.stroke();

            // Draw outer circle
            ctx.beginPath();
            ctx.arc(centerX, centerY, size, 0, doublePI, false);
            ctx.closePath();

            ctx.lineWidth = 10;
            //ctx.strokeStyle = blackHex;
            ctx.stroke();
        }
    };
    $(function () {
        var $venues = $('#venues'),
            $venueName = $('#name'),
            $venueType = $('#types'),
            venueTypes = [],
            $list = $('<ul/>'),
            $types = $('<ul/>'),
            $filterToggler = $('#filterToggle'),
            arrayUnique = function (a) {
                return a.reduce(function (p, c) {
                    if (p.indexOf(c) < 0) { p.push(c); }
                    return p;
                }, []);
            };

        $.each(venues, function (index, venue) {
            $list.append(
		        $("<li/>")
		        .append(
	                $("<input />").attr({
	                    id: 'venue-' + index
                        , name: venue.name
                        , value: venue.name
                        , type: 'checkbox'
                        , checked: true
	                })
	                .change(function () {
	                    var cbox = this,
                            segments = wheel.segments,
                            i = segments.indexOf(cbox.value);

	                    if (cbox.checked && i === -1) {
	                        segments.push(cbox.value);
	                    } else if (!cbox.checked && i !== -1) {
	                        segments.splice(i, 1);
	                    }

	                    segments.sort();
	                    wheel.update();
	                })

		        ).append(
	                $('<label />').attr({
	                    'for': 'venue-' + index
	                })
	                .text(venue.name)
		        )
		    );
            venueTypes.push(venue.type);
        });
        $.each(arrayUnique(venueTypes), function (index, venue) {
            $types.append(
                $("<li/>")
                .append(
                    $("<input />").attr({
                        id: 'venue-type-' + index,
                        name: venue,
                        value: venue,
                        type: 'checkbox',
                        checked: true
                    })
                    .change(function() {
                        var $this = $(this), i;
                        for (i = 0; i < venues.length; i++) {
                            if (venues[i].type === $this.val()) {
                                $('[name="' + venues[i].name + '"]').prop("checked", $this.prop('checked')).trigger('change');
                            }
                        }
                    })
                ).append(
                    $('<label />').attr({
                        'for': 'venue-' + index
                    })
                    .text(venue)
                )
            );
        });

        $venueName.append($list);
        $venueType.append($types);
        // Uses the tinysort plugin, but our array is sorted for now.
        //$list.find('>li').tsort("input", {attr: "value"});

        wheel.init();

        $.each($venueName.find('ul input:checked'), function (key, cbox) {
            wheel.segments.push(cbox.value);
        });

        wheel.update();
        $venues.slideUp().data("open", false);
        $filterToggler.on("click", function () {
            if ($venues.data("open")) {
                $venues.slideUp().data("open", false);
            } else {
                $venues.slideDown().data("open", true);
            }
        });

        $('.checkAll').on("click", function () {
            $(this).parent().next('div').find('input').prop('checked', $(this).prop('checked')).trigger("change");
        });
    });
}(jQuery));