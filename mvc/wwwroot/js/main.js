function openNav() {
  document.getElementById("mySidenav").classList.add("open");
  document.getElementById("contentContain").classList.add("content");
  document.getElementById("controller").classList.add("content");
}

function closeNav() {
  document.getElementById("mySidenav").classList.remove("open");
  document.getElementById("contentContain").classList.remove("content");
  document.getElementById("controller").classList.remove("content");
}