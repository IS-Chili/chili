$(function () {
  StationMap();
});

const StationMap = function() {
  let stationMap = null;

  function init() {
    axios.get('/data/StationMap')
    .then(function (response) {
      createMap(response.data);
    })
    .catch(function (error) {
      console.log('StationMapInit failed', error);
    });
  }

  function createMap(stationList) {
    stationMap = L.map('station-map').setView([31.085943, -86.778946], 8);

    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
      attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      maxZoom: 18,
      id: 'mapbox.streets',
      accessToken: 'pk.eyJ1IjoidGhlanVraSIsImEiOiJjazBlbGN2ZmQwMzl4M2ltb2dqb2Rya2xzIn0.QLMCVnxQgsNKmBvtopyTPA'
    }).addTo(stationMap);

    stationList.forEach(function(station) {
      const stationLink = "<a class='font-weight-bold' href='/Data/Station?id=" + station.id + "'>" + station.displayName + "</a>";
      const stationData = "<br>High: 100 &deg;F<br>Low: 70 &deg;F";
      station.dataText = stationLink + stationData;
      addPoint(station);
    });

    stationMap.on('popupopen', onPopupOpen);
  }

  function addPoint(station) {
    const color = station.isActive ? '#1e76e3' : '#d60909';
    const circle = L.circle([station.latitude, station.longitude], {
      color: color,
      fillColor: color,
      fillOpacity: 1,
      radius: 1000
    }).addTo(stationMap);

    const popup = L.popup()
      .setContent(station.dataText);

    popup.stationId = station.id;

    circle.bindPopup(popup);
  }

  function onPopupOpen(e) {
    // TODO: Use this stationId to update the "Live Mesonet Observations" station
    console.log(e.popup.stationId);
  }

  init();
}
