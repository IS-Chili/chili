$(function () {
  $('[data-toggle="tooltip"]').tooltip({
    boundary: 'span',
    placement: 'left'
  });
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      model: {

      }
    }
  }
});
