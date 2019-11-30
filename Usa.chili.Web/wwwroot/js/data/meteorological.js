$(function () {
  $('#downloadFormat').select2(Core.select2Options);
  $('#beginDate').datetimepicker(Core.dateTimePickerDateOptions);
  $('#endDate').datetimepicker(Core.dateTimePickerDateOptions);
});

/*const App = new Vue({
  el: '#app',
  data: function () {
    return {
      metData: {},
      now: new Date()
    }
  },
  created: function() {
    const self = this;

    // Get data on load
    self.getData();

    // Get data again every 5 minutes
    setInterval(function() {
      self.getData();
    }, 300000);
  },
  computed: {
    nowFormated: function() {
      return moment(this.now).format('MM/DD/YYYY HH:mm:ss');
    }
  },
  methods: {
    getData: function() {
      const self = this;

      axios.get('/data/StationData')
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
});*/

const App = new Vue({
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
  }
});
