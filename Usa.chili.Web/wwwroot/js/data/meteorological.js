$(function () {
  $('#downloadFormat').select2(Core.select2Options);
  $('#beginDate').datetimepicker(Core.dateTimePickerDateOptions);
  $('#endDate').datetimepicker(Core.dateTimePickerDateOptions);
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: Core.DEFAULT_STATION,
        selectedVariables: [],
      },
      variables: [],
      selectAllVariables: false
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
