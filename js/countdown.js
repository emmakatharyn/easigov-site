(function () {
  var deadline = new Date("December 15, 2026 23:59:59");
  var intervalId = null;

  function animateTick(el) {
    el.classList.remove("tick");
    void el.offsetWidth; // force reflow so animation restarts
    el.classList.add("tick");
  }

  function setVal(el, val) {
    var formatted = String(val).padStart(2, "0");
    if (el.textContent !== formatted) {
      el.textContent = formatted;
      animateTick(el);
    }
  }

  function updateCountdown() {
    var diff = deadline - new Date();
    var daysEl = document.getElementById("countdown-days");
    if (!daysEl) return;

    if (diff <= 0) {
      ["countdown-days", "countdown-hours", "countdown-minutes", "countdown-seconds"].forEach(function (id) {
        var el = document.getElementById(id);
        if (el) el.textContent = "00";
      });
      clearInterval(intervalId);
      return;
    }

    setVal(daysEl, Math.floor(diff / (1000 * 60 * 60 * 24)));
    setVal(document.getElementById("countdown-hours"), Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60)));
    setVal(document.getElementById("countdown-minutes"), Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60)));
    setVal(document.getElementById("countdown-seconds"), Math.floor((diff % (1000 * 60)) / 1000));
  }

  // Call timeIsTicking() from the browser console to preview the banner.
  // To go live: remove the style="display:none" from the .enrollment-banner in the HTML.
  window.timeIsTicking = function () {
    var banner = document.querySelector(".enrollment-banner");
    if (!banner) {
      console.warn("timeIsTicking: no .enrollment-banner found on this page");
      return;
    }
    banner.style.display = "";
    updateCountdown();
    if (!intervalId) {
      intervalId = setInterval(updateCountdown, 1000);
    }
  };
})();
