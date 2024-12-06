//compruebo que mi referencia esté ien hecha
console.log("Hola desde google.js")

$(document).ready(function () {
    obtenerUbicacion();
});

//Función para obtener la geolocalización
function obtenerUbicacion() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(mostrarUbicacion, mostrarError);
    } else {
        alert("La geolocalización no es soportada por este navegador");
    }
}

//Función para mostrar la ubicación (si es que podemos recuerarla)
function mostrarUbicacion(position) {
    console.log(position);
    const lat = position.coords.latitude;
    const lon = position.coords.longitude;

    //muestro la latitud y longitud
    console.log("Latitud: " + lat + ",  Longitud: " + lon);

    // Llamar a la función para obtener la dirección
    obtenerDireccion(lat, lon);

    // Inicializar el mapa y el Street View
    initMapComponents(lat, lon);

}

//Función que se ejecuta en caso de algún error al recuperar la geolocalización
function mostrarError(error) {
    switch (error.code) {
        case error.PERMISSION_DENIED:
            alert("El usuario negó el permiso de ubicación.");
            break;
        case error.POSITION_UNAVAILABLE:
            alert("La ubicación no está disponible.");
            break;
        case error.TIMEOUT:
            alert("Se agotó el tiempo de espera.");
            break;
        default:
            alert("Error desconocido: " + error.message);
            break;
    }
}

// Función para obtener la dirección de las coordenadas
function obtenerDireccion(lat, lon) {
    const latLng = { lat: lat, lng: lon };
    const geocoder = new google.maps.Geocoder();

    geocoder.geocode({ location: latLng }, function (results, status) {
        if (status === 'OK') {
            console.log(results)
            if (results[0]) {
                const direccion = results[0].formatted_address;
                // Mostrar la dirección obtenida en el HTML
                document.getElementById('direccion').innerText = "Dirección: " + direccion;
            } else {
                alert("No se encontraron resultados de dirección.");
            }
        } else {
            alert("Geocodificación fallida: " + status);
        }
    });
}

// Función para inicializar el mapa y el Street View
function initMapComponents(lat, lon) {
    const ubicacion = { lat: lat, lng: lon };

    // Inicializar el mapa
    const mapa = new google.maps.Map(document.getElementById("map"), {
        zoom: 15,
        center: ubicacion
    });

    // Añadir un marcador en el mapa
    new google.maps.Marker({
        position: ubicacion,
        map: mapa,
        title: "Ubicación Actual"
    });

    // Configurar Street View
    const panorama = new google.maps.StreetViewPanorama(
        document.getElementById("street"), {
        position: ubicacion,
        pov: { heading: 90, pitch: 5 }
    }
    );

    // Vincular el Street View al mapa
    mapa.setStreetView(panorama);
}