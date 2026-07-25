document.addEventListener('DOMContentLoaded', function () {
  var menuButton = document.querySelector('.menu-button');
  var navMenu = document.querySelector('.nav-menu');

  if (!menuButton || !navMenu) return;

  menuButton.addEventListener('click', function () {
    var isOpen = navMenu.classList.toggle('is-open');
    menuButton.classList.toggle('is-open', isOpen);
    menuButton.setAttribute('aria-expanded', String(isOpen));
  });
});
