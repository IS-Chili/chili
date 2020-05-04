$(function () {
  StationMap();
});

// Station map generation
const StationMap = function() {
  let stationMap = null;

  // Initialize the station map by getting the station data first
  function init() {
    axios.get('/data/StationMap')
    .then(function (response) {
      createMap(response.data);
    })
    .catch(function (error) {
      console.log('StationMapInit failed', error);
    });
  }

  // Create the station map using the station data and draw it using Leaflet
  function createMap(stationList) {
    stationMap = L.map('station-map').setView([30.80, -86.93], 8);

    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
      attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      maxZoom: 12,
      minZoom: 7,
      id: 'mapbox.streets',
      accessToken: 'pk.eyJ1IjoidGhlanVraSIsImEiOiJjazBlbGN2ZmQwMzl4M2ltb2dqb2Rya2xzIn0.QLMCVnxQgsNKmBvtopyTPA'
    }).addTo(stationMap);

    // Add the station points to the map
    stationList.forEach(function(station) {
      const stationLink = "<a class='font-weight-bold' href='/Data/Station?id=" + station.id + "'>" + station.displayName + "</a>";
      const stationData = "<br>High: " + (station.airTemperatureHigh || 'N/A') + "&deg;F<br>Low: " + (station.airTemperatureLow || 'N/A') + "&deg;F";
      station.dataText = stationLink + (station.isActive ? stationData : '');
      addPoint(station);
    });

    // Handle the popup open event
    stationMap.on('popupopen', onPopupOpen);

    // Restrict Map zoom
    stationMap.setMaxZoom(12);
    stationMap.setMinZoom(7);
  }

  // Create a Leaflet circle with a popup and tooltip
  function addPoint(station) {
    // Active stations are blue and inactive stations are red
    const color = station.isActive ? '#1e76e3' : '#d60909';

    // Create the circleMarker object at the station coordinates on the map
    const circleMarker = L.circleMarker([station.latitude, station.longitude], {
      color: color,
      fillColor: color,
      clickable: true,
      fillOpacity: 1,
      radius: 6
    })
    .bindTooltip(station.displayName,
      {
        permanent: station.isActive,
        className: 'station-label',
        direction: 'top',
        opacity: 1
      }
    )
    .addTo(stationMap);

    // Create the popup object with the station data text
    const popup = L.popup()
      .setContent(station.dataText);

    // Add the station Id to the popup object
    popup.stationId = station.id;

    // Add the station isActive to the popup object
    popup.isActive = station.isActive;

    // Bind the popup object to the circle object
    circleMarker.bindPopup(popup);
  }

  // Set the App stationId from the popup object
  // This will change the station in the "Live Mesonet Observations" widget
  function onPopupOpen(e){
    App.model.stationId = e.popup.stationId;
    if(e.popup.isActive) {
      
      //this will draw the canvas on the bootstarp popup when the staion is selected on the map
      $('#stationPopup').on('shown.bs.modal', function () {
        getStationDataAndDraw(App.model.stationId);
      }).modal('show');
      

    }
    else {
      App.model.stationId = null;
      $('#stationPopup').modal('dispose');
    }
  }

  init();
}
