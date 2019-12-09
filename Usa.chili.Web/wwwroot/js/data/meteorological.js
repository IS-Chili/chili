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
      this.error = null;

      if (this.model.stationId
        && this.model.beginDate
        && this.model.endDate
        && this.model.downloadFormat
        && this.model.selectedVariables.length > 0) {
        const beginDateMoment = moment(this.model.beginDate, Core.DATETIME_FORMAT);
        const endDateMoment = moment(this.model.endDate, Core.DATETIME_FORMAT);

        if (endDateMoment.isBefore(beginDateMoment, 'day')) {
          this.error = "The begin date is later than the end date";
        }
        else if (moment.duration(endDateMoment.diff(beginDateMoment)).asDays() > 31) {
          this.error = "You have selected more that 31 days";
        }
        else {
          const params = {
            id: this.model.stationId,
            beginDate: this.model.beginDate,
            endDate: this.model.endDate,
            downloadFormat: this.model.downloadFormat,
            variables: this.model.selectedVariables
          };

          window.location = '/Data/MeteorologicalDownload?' + new URLSearchParams(params).toString();
        }
      }
      else {
        this.error = "Please select a station, begin date, end date, download format, and variable";
      }

    }
  }
});
