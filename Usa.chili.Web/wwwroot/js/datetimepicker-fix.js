jQuery.fn.datetimepicker.Constructor.prototype._origSetValue = jQuery.fn.datetimepicker.Constructor.prototype._setValue;
jQuery.fn.datetimepicker.Constructor.prototype._setValue = function _setValue(targetMoment, index) {
  var oldDate = this.unset ? null : this._dates[index];
  var outpValue = '';
  if (!targetMoment && (!this._options.allowMultidate || this._dates.length === 1)) {
    this.unset = true;
    this._dates = [this.getMoment()];
    this._datesFormatted = [];
    this._viewDate = this.getMoment().locale(this._options.locale).clone();
    if (this.input !== undefined) {
      this.input.val('');
      this.input.trigger('input');
    }
    this._element.data('date', outpValue);
    this._notifyEvent({
      type: jQuery.fn.datetimepicker.Constructor.Event.CHANGE,
      date: false,
      oldDate: oldDate
    });
    this._update();
  }
  else {
    this._origSetValue(targetMoment, index);
  }
};
