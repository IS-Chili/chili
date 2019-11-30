$(function () {
  $('#datePicker').datetimepicker(Core.dateTimePickerDateOptions);
  $('#timePicker').datetimepicker(Core.dateTimePickerTimeOptions);
  $('[data-toggle="tooltip"]').tooltip();
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      statInfo: {},
      metData: {},
      now: new Date()
    }
  },
  created: function() {
    const self = this;

    // Get parameters from URL for stationID and dateTime 
    var queryParams = new Array();
    Core.getURLParameters(this);

    // Get data on load
    self.getData(queryParams);

    // Get data again every 5 minutes
    setInterval(function() {
      self.getData();
    }, 300000);

    Core.populateStationDropdown(this, false);
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
    getData: function(stationParams) {
      const self = this;

      axios.get('/data/StationInfo', {
          params: {
            stationid: stationParams.id,
            dateTime: stationParams.dateTime
          }
      })
      .then(function (response) {
        self.statInfo = response.data;
      })
      .catch(function (error) {
        console.log('getMeteorologicalData failed', error);
      });

      axios.get('/data/StationData', {
          params: {
            stationid: stationParams.id,
            dateTime: stationParams.dateTime
          }
      }) 
      .then(function (response) {
        self.metData = response.data;
        Vue.nextTick(function () {
          self.now = new Date();

          $('[data-toggle="tooltip"]').tooltip({
            boundary: 'span',
            placement: 'left'
          });
        });
      })
      .catch(function (error) {
        console.log('getMeteorologicalData failed', error);
      });
    }
  }
});

/*const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: 1
      }
    }
  },
  created: function() {
    Core.populateStationDropdown(this, false);
  },
  computed: {
    currentDate: function() {
      return moment().format('MM/DD/YYYY');
    },
    currentTime: function() {
      return moment().format('HH:mm:ss');
    }
  }
});*/
