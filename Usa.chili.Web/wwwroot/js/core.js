﻿// Core constants and functions
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
Core.populateStationDropdown = function (self) {
  axios.get('/data/StationList')
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
        data: this.optionsData,
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
  computed: {
    optionsData: function() {
      return $.map(this.options, function (obj) {
        obj.text = obj.text || obj.displayName;
        return obj;
      });
    }
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
        data: this.optionsData,
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
