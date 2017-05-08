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
        vm.newStop = {};
        let url = "/api/trips/" + vm.tripName + "/stops";

        $http.get(url)
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

        vm.addStop = function () {

            vm.isBusy = true;

            $http.post(url, vm.newStop)
                .then(function (response) {
                    vm.stops.push(response.data);
                    _showMap(vm.stops);
                    vm.newStops = {};

                }, function (err) {
                    vm.errorMessage = "Failed to push new stop to database.";
                })
                .finally(function () {
                    vm.isBusy = false;
                });
        };

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
                    currentStop: 0,
                    initialZoom: 3
                });
            }
        }
    }
})();