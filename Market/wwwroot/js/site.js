function toggleSidebar() {
    var x = document.getElementsByClassName("sidebar-container")[0];
    var icon = document.getElementById("hamburger-icon");
    if (x.style.display === "flex") {
        x.style.display = "none";
        x.classList.toggle("open");
        x.offsetHeight;
        icon.children[0].className = "fa fa-bars";

    } else {
        x.style.display = "flex";
        x.classList.toggle("open");
        x.offsetHeight;
        icon.children[0].className = "fa-solid fa-xmark";
    }
}



