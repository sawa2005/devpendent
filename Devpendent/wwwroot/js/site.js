// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const canvas = document.querySelector("canvas");

if (canvas != null) {
    const ctx = canvas.getContext("2d");

    let mouseMoved = false;
    let mouseStopped = true;

    const debounce = (callback, wait) => {
        let timeoutId = null;
        return (...args) => {
            window.clearTimeout(timeoutId);
            timeoutId = window.setTimeout(() => {
                callback(args);
            }, wait);
        };
    }

    const pointer = {
        x: 0.5 * window.innerWidth,
        y: 0.5 * window.innerHeight
    };

    const params = {
        pointsNumber: 40,
        widthFactor: 10,
        mouseThreshold: 0.5,
        spring: 0.25,
        friction: 0.5
    };

    const trail = new Array(params.pointsNumber);

    for (let i = 0; i < params.pointsNumber; i++) {
        trail[i] = {
            x: pointer.x,
            y: pointer.y,
            dx: 0,
            dy: 0
        };
    }

    window.addEventListener("click", (e) => {
        updateMousePosition(e.pageX, e.pageY);
    });

    window.addEventListener("mousemove", (e) => {
        mouseMoved = true;
        mouseStopped = false;
        updateMousePosition(e.pageX, e.pageY);
    });

    document.addEventListener("mousemove", debounce(() => mouseStopped = true, 1000));

    window.addEventListener("touchmove", (e) => {
        mouseMoved = true;
        updateMousePosition(e.targetTouches[0].pageX, e.targetTouches[0].pageY);
    });

    function updateMousePosition(eX, eY) {
        pointer.x = eX;
        pointer.y = eY;
    }

    setupCanvas();
    update(0);

    window.addEventListener("resize", setupCanvas);

    function update(t) {
        if (mouseStopped) {
            pointer.x = (0.5 + 0.3 * Math.cos(0.002 * t) * Math.sin(0.005 * t)) * window.innerWidth;
            pointer.y = (0.5 + 0.2 * Math.cos(0.005 * t) + 0.1 * Math.sin(0.01 * t)) * window.innerHeight;
        }

        ctx.clearRect(0, 0, canvas.width, canvas.height);

        trail.forEach((p, pIdx) => {
            const prev = pIdx === 0 ? pointer : trail[pIdx - 1];
            const spring = pIdx === 0 ? 0.4 * params.spring : params.spring;

            p.dx += (prev.x - p.x) * spring;
            p.dy += (prev.y - p.y) * spring;
            p.dx *= params.friction;
            p.dy *= params.friction;
            p.x += p.dx;
            p.y += p.dy;
        });

        var gradient = ctx.createLinearGradient(0, 0, canvas.width, canvas.height);

        gradient.addColorStop(0, "#3f6739");
        gradient.addColorStop(1, "#20a761");

        ctx.strokeStyle = gradient;
        ctx.linecap = "round";
        ctx.beginPath();
        ctx.moveTo(trail[0].x, trail[0].y);

        for (let i = 1; i < trail.length - 1; i++) {
            const xc = 0.5 * (trail[i].x + trail[i + 1].x);
            const yc = 0.5 * (trail[i].y + trail[i + 1].y);

            ctx.quadraticCurveTo(trail[i].x, trail[i].y, xc, yc);
            ctx.lineWidth = params.widthFactor * (params.pointsNumber - i);
            ctx.stroke();
        }

        ctx.lineTo(trail[trail.length - 1].x, trail[trail.length - 1].y);
        ctx.stroke();

        window.requestAnimationFrame(update);
    }

    function setupCanvas() {
        canvas.width = window.innerWidth;
        canvas.height = window.innerHeight;
    }
}

$(function () {
    if ($("a.confirmDeletion").length) {
        $("a.confirmDeletion").click(() => {
            if (!confirm("Confirm deletion")) return false;
        });
    }

    if ($("div.alert.notification").length) {
        setTimeout(() => {
            $("div.alert.notification").fadeOut();
        }, 2000);
    }
});

function readURL(input) {
    if (input.files && input.files[0]) {
        let reader = new FileReader();

        reader.onload = function (e) {
            $("img#imgpreview").attr("src", e.target.result).width(300);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

const info = document.getElementById('info');
const passwordForm = document.getElementById('Input_Password');

if (passwordForm != null) {
    passwordForm.addEventListener('focus', () => {
        info.style.display = 'block';
    })

    passwordForm.addEventListener('focusout', () => {
        info.style.display = 'none';
    })
}

const message = document.querySelector('.message');
const profileForm = document.querySelector('#profile-form');

if (profileForm != null) {
    profileForm.addEventListener('input', () => {
        message.style.visibility = 'visible';
        message.style.bottom = '50px';
    })

    message.addEventListener('click', () => {
        message.style.bottom = '-100vh';
    })
}

const navbar = document.querySelector('.navbar-collapse');
const text = document.querySelector('.home-text');

function callback(mutationsList, observer) {
    console.log('Mutations:', mutationsList)
    console.log('Observer:', observer)
    mutationsList.forEach(mutation => {
        if (mutation.attributeName === 'class') {
            if (navbar.classList.contains('show') || navbar.classList.contains('collapsing')) {
                text.style.opacity = 0;
            }

            else {
                text.style.opacity = 1;
            }
        }
    })
}

const mutationObserver = new MutationObserver(callback)

mutationObserver.observe(navbar, { attributes: true })