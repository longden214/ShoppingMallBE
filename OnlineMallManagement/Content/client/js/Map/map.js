$(document).ready(function () {
    var f1 = document.getElementById('f1');
    $('#slider').hide();
    f1.addEventListener('click', function () {
        $('#f1').addClass("active");
        $('#g').removeClass("active");
        $('#f2').removeClass("active");
        $('#f3').removeClass("active");
        $('#f1i').show();
        $('#gi').hide();
        $('#f2i').hide();
        $('#f3i').hide();
    }, false);
    var g = document.getElementById('g');

    g.addEventListener('click', function () {
        $('#g').addClass("active");
        $('#f1').removeClass("active");
        $('#f2').removeClass("active");
        $('#f3').removeClass("active");
        $('#f1i').hide();
        $('#gi').show();
        $('#f2i').hide();
        $('#f3i').hide();
    }, false);
    var f2 = document.getElementById('f2');

    f2.addEventListener('click', function () {
        $('#f2').addClass("active");
        $('#g').removeClass("active");
        $('#f1').removeClass("active");
        $('#f3').removeClass("active");
        $('#f2i').show();
        $('#gi').hide();
        $('#f1i').hide();
        $('#f3i').hide();
    }, false);
    var f3 = document.getElementById('f3');

    f3.addEventListener('click', function () {
        $('#f3').addClass("active");
        $('#g').removeClass("active");
        $('#f2').removeClass("active");
        $('#f1').removeClass("active");
        $('#f3i').show();
        $('#gi').hide();
        $('#f2i').hide();
        $('#f1i').hide();
    }, false);
    var map = document.getElementById('map');
    map.addEventListener('click', function () {
        $('#iinterpretation').hide();
        $('#slider').hide();
        $('#imap').show();
        $('#map').addClass("active");
        $('#interpretation').removeClass("active");

    }, false);
    var interpretation = document.getElementById('interpretation');
    interpretation.addEventListener('click', function () {
        $('#iinterpretation').show();
        $('#slider').show();
        $('#imap').hide();
        $('#interpretation').addClass("active");
        $('#map').removeClass("active");

    }, false);
});
var slider = document.getElementById('slider'),
    sliderItems = document.getElementById('items'),
    prev = document.getElementById('prev'),
    next = document.getElementById('next');

slide(slider, sliderItems, prev, next);

function slide(wrapper, items, prev, next) {
    var posX1 = 0,
        posX2 = 0,
        posInitial,
        posFinal,
        threshold = 100,
        slides = items.getElementsByClassName('slide'),
        slidesLength = slides.length,
        slideSize = items.getElementsByClassName('slide')[0].offsetWidth,
        firstSlide = slides[0],
        lastSlide = slides[slidesLength - 1],
        cloneFirst = firstSlide.cloneNode(true),
        cloneLast = lastSlide.cloneNode(true),
        index = 0,
        allowShift = true;

    // Clone first and last slide
    items.appendChild(cloneFirst);
    items.insertBefore(cloneLast, firstSlide);
    wrapper.classList.add('loaded');

    // Mouse and Touch events
    items.onmousedown = dragStart;

    // Touch events
    items.addEventListener('touchstart', dragStart);
    items.addEventListener('touchend', dragEnd);
    items.addEventListener('touchmove', dragAction);

    // Click events
    prev.addEventListener('click', function () { shiftSlide(-1) });
    next.addEventListener('click', function () { shiftSlide(1) });

    // Transition events
    items.addEventListener('transitionend', checkIndex);

    function dragStart(e) {
        e = e || window.event;
        e.preventDefault();
        posInitial = items.offsetLeft;

        if (e.type == 'touchstart') {
            posX1 = e.touches[0].clientX;
        } else {
            posX1 = e.clientX;
            document.onmouseup = dragEnd;
            document.onmousemove = dragAction;
        }
    }

    function dragAction(e) {
        e = e || window.event;

        if (e.type == 'touchmove') {
            posX2 = posX1 - e.touches[0].clientX;
            posX1 = e.touches[0].clientX;
        } else {
            posX2 = posX1 - e.clientX;
            posX1 = e.clientX;
        }
        items.style.left = (items.offsetLeft - posX2) + "px";
    }

    function dragEnd(e) {
        posFinal = items.offsetLeft;
        if (posFinal - posInitial < -threshold) {
            shiftSlide(1, 'drag');
        } else if (posFinal - posInitial > threshold) {
            shiftSlide(-1, 'drag');
        } else {
            items.style.left = (posInitial) + "px";
        }

        document.onmouseup = null;
        document.onmousemove = null;
    }

    function shiftSlide(dir, action) {
        items.classList.add('shifting');

        if (allowShift) {
            if (!action) { posInitial = items.offsetLeft; }

            if (dir == 1) {
                items.style.left = (posInitial - slideSize) + "px";
                index++;
            } else if (dir == -1) {
                items.style.left = (posInitial + slideSize) + "px";
                index--;
            }
        };

        allowShift = false;
    }

    function checkIndex() {
        items.classList.remove('shifting');

        if (index == -1) {
            items.style.left = -(slidesLength * slideSize) + "px";
            index = slidesLength - 1;
        }

        if (index == slidesLength) {
            items.style.left = -(1 * slideSize) + "px";
            index = 0;
        }

        allowShift = true;
    }
}