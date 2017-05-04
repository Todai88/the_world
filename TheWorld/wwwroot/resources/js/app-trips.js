// app-trips.js

(function () {

    angular.module("app-trips", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) {

            $routeProvider.when("/", {
                controller: "tripsController",
                controllerAs: "vm",
                templateUrl: "/resources/views/tripsView.html"
            });

            $routeProvider.when("/editor/:tripName", {
                controller: "tripEditorController",
                controllerAs: "vm",
                templateUrl: "/resources/views/tripEditorView.html"
            });

            $routeProvider.otherwise({ redirectTo: "/" }); // redirect to root if route is not matched.
        });

})();