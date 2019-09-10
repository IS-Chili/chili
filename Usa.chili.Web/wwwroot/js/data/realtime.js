$(function () {
  $('[data-toggle="tooltip"]').tooltip({
    boundary: 'span'
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
