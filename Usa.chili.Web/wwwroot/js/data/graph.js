const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      variables: [],
      model: {
        stationId: Core.DEFAULT_STATION,
        variableId: 1,
        date: null,
        isMetricUnits: false
      },
      lastStationId: null,
      stationInfo: {},
      error: null
    }
  },
  created: function () {
    Core.populateStationDropdown(this, false);
    Core.populateVariableDropdown(this);
    Core.getStationInfo(this, this.model.stationId);

    // Set model from URL params
    const params = new URLSearchParams(window.location.search.substring(1));
    this.model.stationId = (params.get("id") && !isNaN(params.get("id"))) ? Number(params.get("id")) : Core.DEFAULT_STATION;
    this.model.variableId = (params.get("varId") && !isNaN(params.get("varId"))) ? Number(params.get("varId")) : 1;
    this.model.date = params.get("date") ? moment(params.get("date")).format(Core.DATE_FORMAT) : null;
    this.model.isMetricUnits = localStorage.getItem('isMetricUnits') === 'true';

    this.lastStationId = this.model.stationId;

    // Create initial graph
    this.createGraph();
  },
  watch: {
    "model.isMetricUnits": function (value) {
      localStorage.setItem('isMetricUnits', value.toString());
      this.createGraph();
    }
  },
  methods: {
    go: function () {
      this.createGraph();
    },
    next: function () {
      const momentDate = this.model.date ? moment(this.model.date, Core.DATE_FORMAT) : null;
      if(momentDate) {
        momentDate.add(1, 'days');
        $("#datePicker").data('datetimepicker').date(momentDate);
      }
      this.createGraph();
    },
    previous: function () {
      const momentDate = this.model.date ? moment(this.model.date, Core.DATE_FORMAT) : null;
      if(momentDate) {
        momentDate.add(-1, 'days');
        $("#datePicker").data('datetimepicker').date(momentDate);
      }
      this.createGraph();
    },
    lastDateTime: function () {
      $("#datePicker").data('datetimepicker').date(null);
      this.createGraph();
    },

    // Create the graph using the Meteorological Data for
    // the station and variable and draw it using Highcharts
    createGraph: function () {
      const self = this;

      // Get station info if station changed
      if(this.lastStationId != this.model.stationId) {
        this.lastStationId = this.model.stationId;
        Core.getStationInfo(this, this.model.stationId);
      }

      axios.get('/data/StationGraph', {
        params: {
          stationId: self.model.stationId,
          variableId: self.model.variableId,
          date: self.model.date,
          isMetricUnits: self.model.isMetricUnits
        }
      })
      .then(function (response) {
        self.error = null;

        const graphData = response.data;

        if(graphData.lastDateTimeEntry) {
          const momentDate = self.model.date ? moment(self.model.date, Core.DATE_FORMAT) : null;
          const firstDateTimeEntry = moment(graphData.firstDateTimeEntry, Core.DATETIME_FORMAT);
          const lastDateTimeEntry = moment(graphData.lastDateTimeEntry, Core.DATETIME_FORMAT);

          if(momentDate != null && momentDate.isBefore(firstDateTimeEntry, 'day')) {
            self.error = "There is no earlier data available for this station";
          }
          else if(momentDate != null && momentDate.isAfter(lastDateTimeEntry, 'day')) {
            self.error = "There is no later data available for this station";
          }
          else if(graphData.series[0].data.length == 0) {
            self.error = "There is no data available for this date at this station";
          }

          if(momentDate == null || momentDate.isAfter(lastDateTimeEntry, 'day')) {
            $("#datePicker").data('datetimepicker').date(lastDateTimeEntry);
          }
          else if(momentDate != null && momentDate.isBefore(firstDateTimeEntry, 'day')) {
            $("#datePicker").data('datetimepicker').date(firstDateTimeEntry);
          }
        }
        else {
          self.error = "There is no data available for this station";
        }

        if(!self.error) {
          $("#graph").show();

          Highcharts.chart('graph', {
            chart: {
              zoomType: 'x',
              type: 'line'
            },
            title: {
              text: graphData.title
            },
            tooltip: {
              valueDecimals: 2
            },
            xAxis: {
              type: 'datetime',
              title: {
                text: graphData.xAxisTitle
              }
            },
            yAxis: {
              type: 'linear',
              title: {
                text: graphData.yAxisTitle
              }
            },
            series: graphData.series,
            exporting: {
              buttons: {
                contextButton: {
                  menuItems: ['viewFullscreen', 'separator', 'downloadPNG', 'downloadJPEG', 'downloadSVG']
                }
              }
            }
          });
        }
        else {
          $("#graph").hide();
        }

      })
      .catch(function (error) {
        console.log('StationGraphInit failed', error);
      });
    },
  }
});
