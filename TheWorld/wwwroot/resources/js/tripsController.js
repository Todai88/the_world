// tripsController.js

(function () {
    "use strict";

    angular.module("app-trips")
        .controller("tripsController", tripsController);

    
    function tripsController($http) {

        var vm = this;

        vm.trips = [];
        vm.errorMessage;
        vm.isBusy = true;

        /*
        * $http.get(url, [config])
        * returns a HTTP-response based on the request sent to the url.
        */
        $http.get("/api/trips")
            .then(function (response) {

                //SUCCESS
                angular.copy(response.data, vm.trips);
            }, function (error) {

                //FAILURE
                vm.errorMessage = "Failed to load data:\n" + error;

            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.newTrip = {};

        /*
        * $http.post(url, data, [config])
        * data = body / content of request.
        */
        vm.addTrip = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip).
                then(function (response) {
                    // SUCCESS
                    vm.trips.push(response.data);
                    vm.newTrip = {};
                },
                function (error) {
                    // FAILURE
                    vm.errorMessage = "Failed to save new trip:\n\t" + error;
                }).
                finally(

                function () {
                    vm.isBusy = false;
                });
        };

    }
})();