$(function () {
  $('#datePicker').datetimepicker(Core.dateTimePickerDateOptions);
  $('#timePicker').datetimepicker(Core.dateTimePickerTimeOptions);
  $('[data-toggle="tooltip"]').tooltip();
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: Core.DEFAULT_STATION,
        date: null,
        time: null,
        datetime: null
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
  computed: {
    currentDate: function() {
      return moment().format('MM/DD/YYYY');
    },
    currentTime: function() {
      return moment().format('HH:mm:ss');
    }
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
