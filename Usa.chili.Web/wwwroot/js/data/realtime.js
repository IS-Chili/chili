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
      now: new Date(),
      model: {
        isMetricUnits: false,
        isWindChill: null
      }
    }
  },
  created: function() {
    const self = this;

    // Set model from localStorage
    this.model.isMetricUnits = localStorage.getItem('isMetricUnits') === 'true';
    if(localStorage.getItem('isWindChill')) {
      this.model.isWindChill = localStorage.getItem('isWindChill') === 'true';
    }

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
    getData: function() {
      const self = this;

      axios.get('/data/RealtimeData', {
        params: {
          isMetricUnits: self.model.isMetricUnits,
          isWindChill: self.model.isWindChill
        }
      })
      .then(function (response) {
        self.realtimeData = response.data;
        Vue.nextTick(function () {
          self.now = new Date();

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
