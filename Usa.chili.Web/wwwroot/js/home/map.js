$(function () {
  StationMap();
});

const StationMap = function() {
  let stationMap = null;

  function init() {
    stationMap = L.map('station-map').setView([31.085943, -86.778946], 8);

    L.tileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
      attribution: 'Map data &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors, <a href="https://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="https://www.mapbox.com/">Mapbox</a>',
      maxZoom: 18,
      id: 'mapbox.streets',
      accessToken: 'pk.eyJ1IjoidGhlanVraSIsImEiOiJjazBlbGN2ZmQwMzl4M2ltb2dqb2Rya2xzIn0.QLMCVnxQgsNKmBvtopyTPA'
    }).addTo(stationMap);

    //const marker = L.marker([31.085943, -86.778946]).addTo(stationMap);

    let stationList = [
      {
        id: 1,
        displayName: "Test",
        latLng: [31.085943, -86.778946]
      }
    ];

    stationList.forEach(function(station) {
      const stationLink = "<a class='font-weight-bold' href='/Data/Station?id='" + station.id + "'>" + station.displayName + "</a>";
      const stationData = "<br>High: 100 &deg;F<br>Low: 70 &deg;F";
      addPoint(station.latLng, stationLink + stationData);
    });

    //marker.bindPopup("<b>Some station</b>").openPopup();
    circle.bindPopup("<b>Some station</b><br>High: 100 &deg;F<br>Low: 70 &deg;F").openPopup();

    stationMap.on('click', onMapClick);
  }

  function addPoint(latLng, dataText) {
    const circle = L.circle(latLng, {
      color: '#1e76e3',
      fillColor: '#1e76e3',
      fillOpacity: 1,
      radius: 1000
    }).addTo(stationMap);

    circle.bindPopup(dataText).openPopup();
  }

  function onMapClick(e) {
    L.popup()
      .setLatLng(e.latlng)
      .setContent("You clicked the map at " + e.latlng.toString())
      .openOn(stationMap);
  }

  init();
}