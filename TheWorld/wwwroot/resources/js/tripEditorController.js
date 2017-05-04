//  tripEditorController.js

(function () {
    "use strict";

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController);

    function tripEditorController($routeParams, $http) {
        // TODO
        let vm = this;

        /*
        * Setting up variables
        */
        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips/" + vm.tripName + "/stops")
            .then(function (response) {
                //SUCCESS
                angular.copy(response.data, vm.stops);
                _showMap(vm.stops);
            },
            function (error) {
                //FAILURE
                vm.errorMessage = "Failed to load stops..";
            }
            )
            .finally(function () {
                vm.isBusy = false;
            });
    }

    function _showMap(stops) {

        if (stops && stops.length > 0) {

            var mapStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name
                };
            });
            // Show map
            travelMap.createMap({
                stops: mapStops, 
                selector: "#map",
                currentStop: 9,
                initialZoom: 3
            });
        }
    }
})();