var myMap;

ymaps.ready(init);

function init() {
    myMap = new ymaps.Map('map', {
        center: [55.753994, 37.622093],
        zoom: 9
    });
}

function moveToAddress(address) {
    ymaps.geocode(address)
        .then(function (res) {
            var firstGeoObject = res.geoObjects.get(0);
            var coords = firstGeoObject.geometry.getCoordinates();
            myMap.setCenter(coords, 16);
            
            // Создаем метку на карте с указанием адреса
            var placemark = new ymaps.Placemark(coords, {
                hintContent: 'Метка',
                balloonContent: address
            });
            
            myMap.geoObjects.add(placemark);
        });
}