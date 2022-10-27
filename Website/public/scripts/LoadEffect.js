function checkElements() {
  var elements = document.querySelectorAll(
    ".fadeIn, .swipeFromLeft, .swipeFromRight"
  );
  for (var i = 0; i < elements.length; i++) {
    var element = elements[i];
    if (element.getBoundingClientRect().top < window.innerHeight) {
      element.classList.add("visible");
    }
  }
}

$(document).ready(function () {
  checkElements();
});

$(window).scroll(function () {
  checkElements();
});
