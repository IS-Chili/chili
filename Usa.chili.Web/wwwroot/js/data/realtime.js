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

      axios.get('/data/RealtimeData')
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
