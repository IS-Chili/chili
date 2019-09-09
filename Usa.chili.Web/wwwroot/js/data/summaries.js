$(function () {
  $('#dayMonthSelection').select2(Core.select2Options);
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
