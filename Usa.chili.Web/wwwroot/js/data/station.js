const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: Core.DEFAULT_STATION,
        date: null,
        time: null,
        dateTime: null
      },
      dateTimeRecorded: null,
      lastStationId: null,
      dateParam: null,
      stationInfo: {},
      stationData: [],
      error: null,
      isLoading: false
    }
  },
  created: function() {
    Core.populateStationDropdown(this, false);

    // Set model from URL params
    const params = new URLSearchParams(window.location.search.substring(1));
    this.model.stationId = (params.get("id") && !isNaN(params.get("id"))) ? Number(params.get("id")) : Core.DEFAULT_STATION;
    this.model.date = params.get("date") ? moment(params.get("date"), Core.DATE_PARAM_FORMAT).format(Core.DATE_FORMAT) : null;
    this.model.time = params.get("time") ? moment(params.get("time"), Core.TIME_FORMAT).format(Core.TIME_FORMAT) : null;

    this.lastStationId = this.model.stationId;

    Core.getStationInfo(this, this.model.stationId);

    this.GetStationData();
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
      this.GetStationData();
    },
    next: function () {
      if (this.model.dateTime) {
        this.model.dateTime.add(1, 'days');
        $("#datePicker").data('datetimepicker').date(this.model.dateTime);
        $("#timePicker").data('datetimepicker').date(this.model.dateTime);
      }
      this.GetStationData();
    },
    previous: function () {
      if (this.model.dateTime) {
        this.model.dateTime.add(-1, 'days');
        $("#datePicker").data('datetimepicker').date(this.model.dateTime);
        $("#timePicker").data('datetimepicker').date(this.model.dateTime);
      }
      this.GetStationData();
    },
    lastDateTime: function () {
      $("#datePicker").data('datetimepicker').date(null);
      $("#timePicker").data('datetimepicker').date(null);
      this.GetStationData();
    },

    GetStationData: function () {
      const self = this;

      // Prevent multiple requests
      if (self.isLoading) {
        return;
      }

      if(this.model.date && this.model.time) {
        this.model.dateTime = moment(this.model.date + " " + this.model.time, Core.DATETIME_FORMAT);
      }
      else {
        this.model.dateTime = null;
      }

      const params = {
        id: self.model.stationId,
        date: self.model.date ? moment(self.model.date, Core.DATE_FORMAT).format(Core.DATE_PARAM_FORMAT) : '',
        time: self.model.time ? moment(self.model.time, Core.TIME_FORMAT).format(Core.TIME_FORMAT) : ''
      };

      // Replace parameters to the URL
      history.replaceState(null, null, '/Data/Station?' + new URLSearchParams(params).toString());

      // Remove error
      self.error = null;

      // Hide the table
      $("#stationDataTable").hide();

      // Show loading spinner
      self.isLoading = true;

      // Get station info if station changed
      if (this.lastStationId != this.model.stationId) {
        this.lastStationId = this.model.stationId;
        Core.getStationInfo(this, this.model.stationId);
      }

      axios.get('/data/StationData', {
        params: {
          id: self.model.stationId,
          dateTime: self.model.dateTime ? self.model.dateTime.format(Core.DATETIME_FORMAT) : null
        }
      })
        .then(function (response) {
          // Hide loading spinner
          self.isLoading = false;

          const stationData = response.data;

          if (stationData.lastDateTimeEntry) {
            const momentDate = self.model.dateTime ? moment(self.model.dateTime, Core.DATE_FORMAT) : null;
            const firstDateTimeEntry = moment(stationData.firstDateTimeEntry, Core.DATETIME_FORMAT);
            const lastDateTimeEntry = moment(stationData.lastDateTimeEntry, Core.DATETIME_FORMAT);

            if (momentDate != null && momentDate.isBefore(firstDateTimeEntry, 'day')) {
              self.error = "There is no earlier data available for this station";
            }
            else if (momentDate != null && momentDate.isAfter(lastDateTimeEntry, 'day')) {
              self.error = "There is no later data available for this station";
            }
            else if (stationData.stationDataRows == null) {
              self.error = "There is no data available for this date and time at this station";
            }

            if (momentDate == null || momentDate.isAfter(lastDateTimeEntry, 'day')) {
              self.model.dateTime = lastDateTimeEntry;
              $("#datePicker").data('datetimepicker').date(self.model.dateTime);
              $("#timePicker").data('datetimepicker').date(self.model.dateTime);
            }
            else if (momentDate != null && momentDate.isBefore(firstDateTimeEntry, 'day')) {
              self.model.dateTime = firstDateTimeEntry;
              $("#datePicker").data('datetimepicker').date(self.model.dateTime);
              $("#timePicker").data('datetimepicker').date(self.model.dateTime);
            }
          }
          else {
            self.error = "There is no data available for this station";
          }

          if (!self.error) {
            self.dateParam = self.model.dateTime.format(Core.DATE_FORMAT);
            self.dateTimeRecorded = self.model.dateTime.format(Core.DATETIME_FORMAT);

            // Show the table
            $("#stationDataTable").show();

            self.stationData = stationData.stationDataRows;

            Vue.nextTick(function () {
              $('[data-toggle="tooltip"]').tooltip({
                placement: 'bottom'
              });
            });
          }
        })
        .catch(function (error) {
          // Hide loading spinner
          self.isLoading = false;

          console.log('GetStationData failed', error);
        });
    },
  }
});
