$(function () {
  $('[data-toggle="tooltip"]').tooltip({
    boundary: 'span',
    placement: 'left'
  });
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      realtimeData: []
    }
  },
  created: function() {
    this.getData();
  },
  methods: {
    getData: function() {
      const self = this

      axios.get('/data/RealtimeData')
      .then(function (response) {
        self.realtimeData = response.data;
      })
      .catch(function (error) {
        console.log('getRealtimeData failed', error);
      });
    }
  }
});
