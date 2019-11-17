// Core constants and functions
const Core = {};

// Global Select2 Options
Core.select2Options = {
  theme: 'bootstrap4',
  width: '100%'
};

// Global Date Options
Core.dateTimePickerDateOptions = {
  format: 'MM/DD/YYYY',
  allowInputToggle: true
};

// Global Time Options
Core.dateTimePickerTimeOptions = {
  format: 'HH:mm:ss',
  allowInputToggle: true
};

// Get Stations
Core.populateStationDropdown = function (self, activeOnly) {
  axios.get(activeOnly === true ? '/data/ActiveStationList' : '/data/StationList')
    .then(function (response) {
      self.stations = response.data;
    })
    .catch(function (error) {
      console.log('populateStationDropdown failed', error);
    });
};

// select2 Vue component
Vue.component('select2', {
  props: ['options', 'value'],
  template: '#select2-template',
  mounted: function () {
    const vm = this;

    $(this.$el)
      // init select2
      .select2({
        data: this.options,
        theme: Core.select2Options.theme,
        width: Core.select2Options.width
      })
      .val(this.value)
      .trigger('change')
      // emit event on change.
      .on('change', function () {
        vm.$emit('input', this.value)
      });
  },
  watch: {
    value: function (value) {
      // update value
      $(this.$el)
      	.val(value)
      	.trigger('change');
    },
    options: function (options) {
      // update options
      $(this.$el).empty().select2({
        data: options,
        theme: Core.select2Options.theme,
        width: Core.select2Options.width
      })
      .val(this.value)
      .trigger('change');
    }
  },
  destroyed: function () {
    $(this.$el).off().select2('destroy');
  }
});

// Prevent changes to Core object
Object.freeze(Core);

// Handle navbar stickiness
$(function () {
  const header = document.getElementById("navbar");
  const sticky = header.offsetTop;

  window.onscroll = function() {
    if (window.pageYOffset > sticky) {
      header.classList.add("fixed-top");
    } else {
      header.classList.remove("fixed-top");
    }
  };
});
$(document).ready(function() {
  $('tbody').scroll(function(e) { //detect a scroll event on the tbody
  	/*
    Setting the thead left value to the negative valule of tbody.scrollLeft will make it track the movement
    of the tbody element. Setting an elements left value to that of the tbody.scrollLeft left makes it maintain 			it's relative position at the left of the table.    
    */
    $('thead').css("left", -$("tbody").scrollLeft()); //fix the thead relative to the body scrolling
    $('thead th:nth-child(1)').css("left", $("tbody").scrollLeft()); //fix the first cell of the header
    $('tbody td:nth-child(1)').css("left", $("tbody").scrollLeft()); //fix the first column of tdbody
  });
});