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
    // Set model from URL params
    const params = new URLSearchParams(window.location.search.substring(1));
    this.model.stationId = (params.get("id") && !isNaN(params.get("id"))) ? Number(params.get("id")) : Core.DEFAULT_STATION;

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
