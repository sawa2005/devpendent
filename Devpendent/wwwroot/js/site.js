// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

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