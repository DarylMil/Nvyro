// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var allLinks = $("#navbar-main-nvyro li");
var currentBasePath  = window.location.pathname.toLowerCase().split("/");
if (currentBasePath[1] == "") {
    allLinks[0].classList.add("active")
} else if (currentBasePath.includes("user") && currentBasePath.includes("register")) {
    allLinks[3].classList.add("active")
} else if (currentBasePath.includes("user") && currentBasePath.includes("login")) {
    allLinks[4].classList.add("active")
} else if (currentBasePath.includes("admin")) {
    allLinks[3].classList.add("active")
} else if (currentBasePath.includes("user") && currentBasePath.includes("userdashboard")) {
    allLinks[3].classList.add("active")
}