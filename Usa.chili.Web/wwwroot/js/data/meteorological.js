const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: Core.DEFAULT_STATION,
        beginDate: moment().format(Core.DATE_FORMAT),
        endDate: moment().format(Core.DATE_FORMAT),
        downloadFormat: 1,
        selectedVariables: []
      },
      variables: [],
      formatOptions: [
        {
          id: 1,
          text: "CSV"
        },
        {
          id: 2,
          text: "Fixed"
        }
      ],
      selectAllVariables: false,
      error: null,
      isLoading: false
    }
  },
  created: function () {
    Core.populateStationDropdown(this, false);

    // Get Variable list from JSON file
    this.getVariables();
  },
  methods: {
    getVariables: function () {
      const self = this;

      axios.get('/json/variables.json')
        .then(function (response) {
          self.variables = response.data;
        })
        .catch(function (error) {
          console.log('getVariables failed', error);
        });
    },
    selectAll: function () {
      const self = this;

      self.model.selectedVariables = [];

      if (self.selectAllVariables) {
        self.variables.forEach(function (item) {
          self.model.selectedVariables.push(item.name);
        });
      }
    },
    selectOne: function () {
      if (this.model.selectedVariables
          && this.model.selectedVariables.length > 0
          && this.model.selectedVariables.length != this.variables.length) {
        $('#selectAll').prop('indeterminate', true);
      }
      else if (this.model.selectedVariables
        && this.model.selectedVariables.length > 0
        && this.model.selectedVariables.length == this.variables.length) {
        $('#selectAll').prop('indeterminate', false);
        this.selectAllVariables = true;
      }
      else {
        $('#selectAll').prop('indeterminate', false);
        this.selectAllVariables = false;
      }
    },
    download: function () {
      // TODO
    }
  }
});
