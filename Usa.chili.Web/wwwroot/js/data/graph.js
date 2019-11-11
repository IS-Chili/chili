$(function () {
  $('#datePicker').datetimepicker(Core.dateTimePickerDateOptions);
  $('#variableSelection').select2(Core.select2Options);
  DataGraph();
});

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: null
      }
    }
  },
  created: function () {
    Core.populateStationDropdown(this, false);
  },
  computed: {
    currentDate: function () {
      return moment().format('MM/DD/YYYY');
    }
  }
});

// Data graph generation
const DataGraph = function () {
  // Initialize the station map by getting the station data first
  function init() {
    axios.get('/data/StationGraph', {
        params: {
          stationId: 1,
          variableId: 1,
          date: '11/01/2019',
          isMetricUnits: false
        }
      })
      .then(function (response) {
        createGraph(response.data);
      })
      .catch(function (error) {
        console.log('StationGraphInit failed', error);
      });
  }

  // Create the graph using the Meteorological Data for
  // the station and variable and draw it using Highcharts
  function createGraph(graphData) {
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

  init();
}

