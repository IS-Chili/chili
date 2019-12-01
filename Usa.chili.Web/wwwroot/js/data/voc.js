$(function () {
  $('#dayMonthSelection').select2(Core.select2Options);
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: Core.DEFAULT_STATION
      }
    }
  },
  created: function() {
    Core.populateStationDropdown(this, false);
  },
  methods: {
    download: function () {
      // TODO
    }
  }
});
