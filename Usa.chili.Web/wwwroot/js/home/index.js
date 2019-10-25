const USACampusWestStationId = 23;
var currentStation = '23';

$(function () {
  //populateStationDropdown_Jquery();
  //getStationDataAndDraw(USACampusWestStationId);

  /*
  $('#stationSelection').on('change', function() {
    getStationDataAndDraw($(this).val());
  });
  */

  $('#dataSetSelection').select2(Core.select2Options);

  $('#dataSetSelection').on('change', function() {
    changeRegional($(this).val());
  });
});

function populateStationDropdown_Jquery() {
  axios.get(ajaxUrls.stationList)
    .then(function (response) {
      response.data.forEach(function(station) {
        const option = $('<option>');
        option.text(station.displayName);
        option.val(station.id);
        if(station.id === 1) {
          option.prop('selected', true);
        }
        $('#stationSelection').append(option);
      });

      $('#stationSelection').select2(select2Options);

      $('#stationSelection').on('change', function() {
        getStationDataAndDraw($(this).val());
      });
    })
    .catch(function (error) {
      console.log('populateStationDropdown failed', error);
    });
}

function goToFullData(paramName, paramValue){
  url = document.getElementById("widgetFullDataLink").href;
  if (paramValue === null) {
      paramValue = '';
  }
  var pattern = new RegExp('\\b('+paramName+'=).*?(&|#|$)');
  if (url.search(pattern)>=0) {
      return url.replace(pattern,'$1' + paramValue + '$2');
  }
  url = url.replace(/[?#]$/,'');
  url = url + (url.indexOf('?')>0 ? '&' : '?') + paramName + '=' + paramValue;
  document.getElementById("widgetFullDataLink").href = url;
};

function getStationDataAndDraw(stationId) {
  currentStation = stationId;
  axios.get('/data/StationObservation?id=' + stationId)
    .then(function (response) {
      if(response.data != null) {
        drawStation(response.data);
      }
    })
    .catch(function (error) {
      console.log('getStationDataAndDraw failed', error);
    });
}

function drawStation(stationData) {
  const stationTemp = Math.round(Number(stationData.airTemperature));
  const stationDew = Math.round(Number(stationData.dewPoint));
  const stationRH = Math.round(Number(stationData.realHumidity));
  const stationWinD = Math.round(Number(stationData.windDirection));
  const stationWinS = Math.round(Number(stationData.windSpeed));
  const stationPrec = Number(stationData.precipitation);
  const stationPres = Number(stationData.pressure);
  const stationTS = stationData.stationTimestamp;

  const canvas = $("#stationObservationsCanvas");
  const context = canvas.get(0).getContext('2d');
  canvas.attr("width", canvas.width());
  canvas.attr("height", canvas.height());

  const canvasDIV = $("#stationObservationsCanvasDiv");
  divWidth = canvasDIV.width() - 10;
  canvas.attr("width", divWidth);
  canvas.attr("height", divWidth);

  context.clearRect(0, 0, canvas.width(), canvas.height());
  context.closePath();

  //==============================================================
  // Set up variables for customization later
  const totHeight = canvas.height();
  const totWidth = canvas.width();

  const tempTop = totHeight * 0.20;
  const tempLeft = totWidth * 0.08;
  const tempBottom = totHeight * 0.70;
  const tempRight = totWidth * 0.13;
  const tempBulb = tempBottom + ((tempRight - tempLeft) * 0.4);
  const lowTemp = -30.0;
  const highTemp = 120.0;

  const humidityBot = totHeight * 0.95;
  const humidityDesc = totHeight * 0.87;
  const humidityRHX = totWidth * 0.15;
  const humidityDewX = totWidth * 0.45;

  const precipDesc = humidityDesc;
  const precipBot = humidityBot;
  const precipDayX = totWidth * 0.75;

  const slope = (tempTop - tempBottom) / (highTemp - lowTemp);
  const inter = tempBottom - (slope * lowTemp);

  const windCentX = totWidth * 0.65;
  const windCentY = (tempTop + tempBottom) / 2.0;
  const windLength = totHeight * 0.25;
  const lengthFrac = 5.0;
  const barbLength = windLength / lengthFrac;

  const largeFontSize = (totHeight * 0.06).toFixed(0);
  const medFontSize = (totHeight * 0.04).toFixed(0);
  const smallFontSize = (totHeight * 0.03).toFixed(0);

  const thickLine = (totHeight * 0.01).toFixed(0);
  const thinLine = (thickLine / 4.0).toFixed(0);
  const medLine = ((Number(thickLine) + Number(thinLine)) / 2.0).toFixed(0);

  //==============================================================
  // Make the title of the canvas
  context.beginPath();
  context.font = largeFontSize.toString() + 'pt Calibri';
  context.fillStyle = 'blue';
  context.textAlign = "center";
  context.fillText(stationData.stationName, totWidth * 0.50, totHeight * 0.065);
  context.closePath();

  context.beginPath();
  context.font = medFontSize.toString() + 'pt Calibri';
  context.fillStyle = 'purple';
  context.textAlign = "center";
  context.fillText("Valid at: " + stationTS + " CST", totWidth * 0.50, totHeight * 0.125);
  context.closePath();

  //==============================================================
  // Plot the base rectangle / bulb around the whole temperature thing
  context.beginPath();
  context.rect(tempLeft, tempTop, tempRight - tempLeft, tempBottom - tempTop);
  context.lineWidth = thinLine;
  context.strokeStyle = "black";
  context.stroke();
  context.closePath();

  context.beginPath();
  context.arc((tempLeft + tempRight) / 2.0, tempBulb, (tempBulb - tempBottom) * 2.0, 2 * Math.PI, false);
  context.fillStyle = "red";
  context.fill();
  context.lineWidth = thinLine;
  context.strokeStyle = "black";
  context.stroke();
  context.closePath();


  //==============================================================
  // Plot the current temperature
  context.beginPath();
  let currSpot = stationTemp * slope + inter;
  context.rect(tempLeft, currSpot, tempRight - tempLeft, tempBottom - currSpot);
  context.fillStyle = "red";
  context.fill();
  context.closePath();
  context.font = smallFontSize.toString() + 'pt Calibri';
  context.textAlign = "right";
  context.fillStyle = "red";
  context.fillText(stationTemp, tempLeft - (smallFontSize / 2.0), currSpot + (smallFontSize / 2.0));

  //==============================================================
  // Create the temperature scale
  currSpot = 0;
  for (let i = lowTemp + 10; i <= highTemp; i += 20) {
    context.beginPath();

    currSpot = slope * i + inter;

    context.moveTo(tempLeft, currSpot);
    context.lineTo(tempRight, currSpot);
    context.lineWidth = thinLine;
    context.strokeStyle = "LightGray";
    context.stroke();
    context.closePath();

    context.font = smallFontSize.toString() + 'pt Calibri';
    context.fillStyle = "Gray";
    context.textAlign = "left";
    context.fillText(i, tempRight + (smallFontSize / 2.0), currSpot + (smallFontSize / 2.0));
  }


  //================================================================
  // Output the humidity and rainfall dataspots
  context.font = medFontSize.toString() + 'pt Calibri';
  context.textAlign = "center";
  context.fillStyle = "brown";
  context.fillText("Rel. Hum.", humidityRHX, humidityDesc);
  context.font = largeFontSize.toString() + 'pt Calibri';
  context.fillText(stationRH.toString() + "%", humidityRHX, humidityBot);

  context.font = medFontSize.toString() + 'pt Calibri';
  context.fillStyle = "green";
  context.fillText("Dew Pt.", humidityDewX, humidityDesc);
  context.font = largeFontSize.toString() + 'pt Calibri';
  context.fillText(stationDew.toString(), humidityDewX, humidityBot);


  context.font = medFontSize.toString() + 'pt Calibri';
  context.fillStyle = "blue";
  context.fillText("Today's Rain", precipDayX, precipDesc);
  context.font = largeFontSize.toString() + 'pt Calibri';
  context.fillText(stationPrec.toString() + " in.", precipDayX, precipBot);


  //==============================================================
  // Set up wind rose plot
  context.beginPath();
  context.moveTo(windCentX - windLength, windCentY);
  context.lineTo(windCentX + windLength, windCentY);
  context.lineWidth = thinLine;
  context.strokeStyle = "gray";
  context.stroke();
  context.closePath();

  context.font = smallFontSize.toString() + "pt Calibri";
  context.textAlign = "center";
  context.fillStyle = "gray";
  context.fillText("W", windCentX - windLength - 2 * thickLine, windCentY + 1.5 * thickLine);
  context.fillText("E", windCentX + windLength + 2 * thickLine, windCentY + 1.5 * thickLine);
  context.fillText("N", windCentX, windCentY - windLength - thickLine);
  context.fillText("S", windCentX, windCentY + windLength + 3 * thickLine);

  context.beginPath();
  context.moveTo(windCentX, windCentY - windLength);
  context.lineTo(windCentX, windCentY + windLength);
  context.lineWidth = thinLine;
  context.strokeStyle = "gray";
  context.stroke();
  context.closePath();

  context.beginPath();
  const xShift = windLength * Math.sin(stationWinD * Math.PI / 180.0);
  const yShift = -1.0 * windLength * Math.cos(stationWinD * Math.PI / 180.0);
  context.moveTo(windCentX, windCentY);
  context.lineTo(windCentX + xShift, windCentY + yShift);
  context.lineWidth = thickLine;
  context.strokeStyle = "black";
  context.stroke();
  context.closePath();

  context.beginPath();
  context.arc(windCentX, windCentY, (totHeight * 0.02), 0, 2 * Math.PI, false);
  context.fillStyle = "white";
  context.fill();
  context.lineWidth = thickLine;
  context.strokeStyle = "black";
  context.stroke();
  context.closePath();

  let numHalf = Math.round(stationWinS / 5);
  const numFlag = Math.floor(numHalf / 10);
  numHalf = numHalf - 10 * numFlag;
  const numFull = Math.floor(numHalf / 2);
  numHalf = numHalf - numFull * 2;

  let fracShift = 1.0;
  const standardFrac = 1.0 / 15.0;

  const xBarb = -1.0 * yShift / lengthFrac;
  const yBarb = xShift / lengthFrac;

  if (numFlag > 0) {
    for (let i = 0; i < numFlag; i++) {
      context.fillStyle = "black";
      context.beginPath();
      context.moveTo(windCentX + xShift * fracShift, windCentY + yShift * fracShift);
      context.lineTo(windCentX + (xShift * (fracShift - 0.5 * standardFrac)) + xBarb, windCentY + (yShift * (fracShift - 0.5 * standardFrac)) + yBarb);
      context.lineTo(windCentX + xShift * (fracShift - standardFrac), windCentY + yShift * (fracShift - standardFrac));
      context.lineWidth = medLine;
      context.strokeStyle = "black";
      context.stroke();
      context.closePath();
      context.fill();
      fracShift -= standardFrac * 2.0;
    }
  }

  if (numFull > 0) {
    for (let i = 0; i < numFull; i++) {
      context.beginPath();
      context.moveTo(windCentX + xShift * fracShift, windCentY + yShift * fracShift);
      context.lineTo(windCentX + xShift * fracShift + xBarb, windCentY + yShift * fracShift + yBarb);
      context.lineWidth = medLine;
      context.strokeStyle = "black";
      context.stroke();
      context.closePath();
      fracShift -= standardFrac;
    }
  }

  if (numHalf > 0) {
    context.beginPath();
    context.moveTo(windCentX + xShift * fracShift, windCentY + yShift * fracShift);
    context.lineTo(windCentX + xShift * fracShift + (xBarb * 0.5), windCentY + yShift * fracShift + (yBarb * 0.5));
    context.lineWidth = medLine;
    context.strokeStyle = "black";
    context.stroke();
    context.closePath();
    fracShift -= standardFrac;
  }

}

function changeRegional(index) {
	const image = $("#regionalImage");
	if (index == 1) {
		image.attr("src", "http://weather.southalabama.edu/images/surface/GulfCoast_CHILI_Temp.png");
	} else if (index == 2) {
		image.attr("src", "http://weather.southalabama.edu/images/surface/GulfCoast_CHILI_Dewpoint.png");
	} else if (index == 3) {
		image.attr("src", "http://weather.southalabama.edu/images/surface/GulfCoast_CHILI_Pressure.png");
	}
}

const App = new Vue({
  el: '#app',
  data: function () {
    return {
      stations: [],
      model: {
        stationId: 23
      }
    }
  },
  created: function() {
    Core.populateStationDropdown(this, true);
  },
  watch: {
    "model.stationId": function(val) {
      getStationDataAndDraw(val);
    }
  }
});
