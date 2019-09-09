$(function () {
  $('#datePicker').datetimepicker(Core.dateTimePickerDateOptions);
  $('#variableSelection').select2(Core.select2Options);
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
  created: function() {
    Core.populateStationDropdown(this);
  },
  computed: {
    currentDate: function() {
      return moment().format('MM/DD/YYYY');
    }
  }
});
