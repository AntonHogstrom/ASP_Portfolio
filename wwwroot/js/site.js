//Menu Button, click to toggle on and off (Mobile)
const hamburger = document.querySelector(".mobileMenuButton");
const mobileMenu = document.getElementById("mainNavOptions");

hamburger.addEventListener("click", () => {
    if (mobileMenu.style.display != "flex") {
        mobileMenu.style.display = "flex";
    } else {
        mobileMenu.style.display = "none";
    }
})

//Check window size on resize to make menu not display none when bigger than mobile
window.addEventListener("resize", () => {
    if (window.innerWidth >= 680) {
        mobileMenu.style.display = "flex";
    }
})

//Admin toggle button, click to toggle admin NAV
const button = document.getElementById("toggleAdmin");
const adminNavOptions = document.getElementById("adminNavOptions");

button.addEventListener("click", () => {
    if (adminNavOptions.style.display != "block") {
        adminNavOptions.style.display = "block";
        localStorage.setItem("admin", "on");
    } else {
        adminNavOptions.style.display = "none";
        localStorage.removeItem("admin");
    }
})

//Shows admin options if admin variable exists in localstorage, keeps admin options open when changing page
if (localStorage.getItem("admin")) {
    adminNavOptions.style.display = "block";
}