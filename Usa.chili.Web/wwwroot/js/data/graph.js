$(function () {
  $('#datePicker').datetimepicker(Core.dateTimePickerDateOptions);
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      variables: [],
      model: {
        stationId: 201,
        variableId: 1,
        date: null,
        isMetricUnits: false
      }
    }
  },
  created: function () {
    Core.populateStationDropdown(this, false);
    Core.populateVariableDropdown(this);

    // Set model from URL params
    const params = new URLSearchParams(window.location.search.substring(1));
    this.model.id = Number(params.get("id"));
    this.model.variableId = Number(params.get("varId"));
    this.model.date = moment(params.get("date")).format('MM/DD/YYYY');

    // Create initial graph
    this.createGraph();
  },
  methods: {
    search: function () {
      this.createGraph();
    },

    // Create the graph using the Meteorological Data for
    // the station and variable and draw it using Highcharts
    createGraph: function () {
      const self = this;

      axios.get('/data/StationGraph', {
        params: {
          stationId: self.model.stationId,
          variableId: self.model.variableId,
          date: self.model.date,
          isMetricUnits: self.model.isMetricUnits
        }
      })
      .then(function (response) {
        const graphData = response.data;

        if(self.model.date == null) {
          self.model.date = moment(graphData.lastDateTimeEntry).format('MM/DD/YYYY');
        }

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
      })
      .catch(function (error) {
        console.log('StationGraphInit failed', error);
      });
    },
  }
});

