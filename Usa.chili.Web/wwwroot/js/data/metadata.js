$(function () {
  $('#metadataSelection').select2(Core.select2Options);
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: Core.DEFAULT_STATION
      },
      lastStationId: null,
      stationInfo: {}
    }
  },
  created: function() {
    this.lastStationId = this.model.stationId;
    Core.populateStationDropdown(this, false);
    Core.getStationInfo(this, this.model.stationId);
  },
  methods: {
    go: function () {
      // Get station info if station changed
      if(this.lastStationId != this.model.stationId) {
        this.lastStationId = this.model.stationId;
        Core.getStationInfo(this, this.model.stationId);
      }
    }
  }
});
