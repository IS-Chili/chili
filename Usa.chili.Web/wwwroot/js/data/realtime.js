$(document).ready(function() {
  $('tbody').scroll(function(e) {
    $('thead').css("left", -$("tbody").scrollLeft());
    $('thead th:nth-child(1)').css("left", $("tbody").scrollLeft());
    $('tbody td:nth-child(1)').css("left", $("tbody").scrollLeft());
  });
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      realtimeData: [],
      now: moment(),
      model: {
        isMetricUnits: false,
        isWindChill: null
      },
      variableTypes: []
    }
  },
  created: function() {
    const self = this;

    // Set model from localStorage
    this.model.isMetricUnits = localStorage.getItem('isMetricUnits') === 'true';
    if(localStorage.getItem('isWindChill')) {
      this.model.isWindChill = localStorage.getItem('isWindChill') === 'true';
    }

    // Get Variable Types
    axios.get('/data/VariableTypeList')
      .then(function (response) {
        self.variableTypes = response.data;
      })
      .catch(function (error) {
        console.log('Get VariableTypeList failed', error);
      });

    // Get data on load
    self.getData();

    // Get data again every 5 minutes
    setInterval(function() {
      self.getData();
    }, 300000);
  },
  computed: {
    nowFormated: function() {
      return this.now.format(Core.DATETIME_FORMAT);
    }
  },
  watch: {
    "model.isMetricUnits": function (value) {
      localStorage.setItem('isMetricUnits', value.toString());
      this.getData();
    },
    "model.isWindChill": function (value) {
      localStorage.setItem('isWindChill', value.toString());
      this.getData();
    }
  },
  methods: {
    getUnit: function(variableType, measurementSystem) {
      if(this.variableTypes && this.variableTypes.length > 0) {
        return measurementSystem === 'Metric'
          ? this.variableTypes.find(x => x.variableType === variableType).metricSymbol
          : this.variableTypes.find(x => x.variableType === variableType).englishSymbol;
      }
    },
    getData: function() {
      const self = this;

      axios.get('/data/RealtimeData', {
        params: {
          isMetricUnits: self.model.isMetricUnits,
          isWindChill: self.model.isWindChill
        }
      })
      .then(function (response) {
        Core.setNullsInArrayToNA(response.data);

        self.realtimeData = response.data;

        Vue.nextTick(function () {
          self.now = moment();

          $('[data-toggle="tooltip"]').tooltip({
            boundary: 'span',
            placement: 'left'
          });
        });
      })
      .catch(function (error) {
        console.log('getRealtimeData failed', error);
      });
    }
  }
});
