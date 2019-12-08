// Core constants and functions
const Core = {};

Core.DEFAULT_WIDGET_STATION = 23; // 307
Core.DEFAULT_STATION = 1; // 201

Core.DATE_FORMAT = "MM/DD/YYYY";
Core.DATE_PARAM_FORMAT = "MM-DD-YYYY";
Core.DATETIME_FORMAT = "MM/DD/YYYY HH:mm:ss";
Core.TIME_FORMAT = "HH:mm:ss";

// Global Select2 Options
Core.select2Options = {
  theme: 'bootstrap4',
  width: '100%'
};

// Global Date Options
Core.dateTimePickerDateOptions = {
  format: Core.DATE_FORMAT,
  allowInputToggle: true
};

// Global Time Options
Core.dateTimePickerTimeOptions = {
  format: Core.TIME_FORMAT,
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

// Get Variables
Core.populateVariableDropdown = function (self) {
  axios.get('/data/VariableList')
    .then(function (response) {
      self.variables = response.data;
    })
    .catch(function (error) {
      console.log('populateVariableDropdown failed', error);
    });
};

// Get Station Info
Core.getStationInfo = function (self, id) {
  axios.get('/data/StationInfo?id=' + id)
    .then(function (response) {
      const stationInfo = response.data;
      stationInfo.beginDate = moment(stationInfo.beginDate, Core.DATETIME_FORMAT).format(Core.DATE_FORMAT);
      stationInfo.endDate = stationInfo.endDate ? moment(stationInfo.endDate, Core.DATETIME_FORMAT).format(Core.DATE_FORMAT) : null;
      self.stationInfo = stationInfo;
    })
    .catch(function (error) {
      console.log('getStationInfo failed', error);
    });
};

// Set all null values to N/A in the array
Core.setNullsInArrayToNA = function (array) {
  array.forEach(function(object) {
    Core.setNullsInObjectToNA(object);
  });
};

// Set all null values to N/A in the object
Core.setNullsInObjectToNA = function (object) {
  for(key in object) {
    if(typeof object[key] === 'object') {
      for(objectKey in object[key]) {
        if(object[key][objectKey] == null) {
          object[key][objectKey] = 'N/A';
        }
      }
    }
    if(object[key] == null) {
      object[key] = 'N/A';
    }
  }
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

// datetimepicker Vue component
Vue.component('datetimepicker', {
  props: {
    value: {
      default: null,
      required: true
    },
    id: {
      type: String,
      default: ''
    },
    time: {
      type: Boolean,
      default: false
    },
    disabled: {
      type: Boolean,
      default: false
    }
  },
  template: '#datetimepicker-template',
  data: function () {
    return {
      dp: null
    }
  },
  mounted: function () {
    const vm = this;

    $(this.$el).datetimepicker(vm.time ? Core.dateTimePickerTimeOptions : Core.dateTimePickerDateOptions);

    this.dp = $(this.$el).data('datetimepicker');

    $(this.$el).on('change.datetimepicker', function(e) {
      vm.$emit('input', e.date ? e.date.format(vm.dp.format()) : null);
    });

    this.dp.date(this.value);
  },
  destroyed: function () {
    $(this.$el).off().datetimepicker('destroy');
  }
});

// Prevent changes to Core object
Object.freeze(Core);

// Handle navbar and footer stickiness
$(function () {
  const header = document.getElementsByTagName("header")[0];
  const footer = document.getElementsByTagName("footer")[0];
  const navbar = document.getElementById("navbar");
  const sticky = navbar.offsetTop;

  if ($(document).height() <= $(window).height()) {
    footer.classList.add("footer-bottom");
  }
  else {
    footer.classList.remove("footer-bottom");
  }

  window.onscroll = function() {
    if (window.pageYOffset > sticky) {
      navbar.classList.add("fixed-top");
      header.classList.add("header-fixed");
    } else {
      navbar.classList.remove("fixed-top");
      header.classList.remove("header-fixed");
    }

    if ($(document).height() <= $(window).height()) {
      footer.classList.add("footer-bottom");
    }
    else {
      footer.classList.remove("footer-bottom");
    }
  };
});
