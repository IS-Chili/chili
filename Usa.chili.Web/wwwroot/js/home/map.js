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

    //const marker = L.marker([31.085943, -86.778946]).addTo(stationMap)

    stationList.forEach(function(station) {
      const stationLink = "<a class='font-weight-bold' href='/Data/Station?id=" + station.id + "'>" + station.displayName + "</a>";
      const stationData = "<br>High: 100 &deg;F<br>Low: 70 &deg;F";
      addPoint([station.latitude, station.longitude], station.isActive, stationLink + stationData);
    });

    //marker.bindPopup("<b>Some station</b>").openPopup();

    stationMap.on('click', onMapClick);
  }

  function addPoint(latLng, isActive, dataText) {
    const color = isActive ? '#1e76e3' : '#d60909';
    const circle = L.circle(latLng, {
      color: color,
      fillColor: color,
      fillOpacity: 1,
      radius: 1000
    }).addTo(stationMap);

    circle.bindPopup(dataText);
  }

  function onMapClick(e) {
    L.popup()
      .setLatLng(e.latlng)
      .setContent("You clicked the map at " + e.latlng.toString())
      .openOn(stationMap);
  }

  init();
}
