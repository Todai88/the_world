!function(){"use strict";
i.$inject = ["i"];function i(i){var n=this;n.trips=[],n.errorMessage,n.isBusy=!0,i.get("/api/trips").then(function(i){angular.copy(i.data,n.trips)},function(i){n.errorMessage="Failed to load data:\n"+i}).finally(function(){n.isBusy=!1}),n.newTrip={},n.addTrip=function(){n.isBusy=!0,n.errorMessage="",i.post("/api/trips",n.newTrip).then(function(i){n.trips.push(i.data),n.newTrip={}},function(i){n.errorMessage="Failed to save new trip:\n\t"+i}).finally(function(){n.isBusy=!1})}}angular.module("app-trips").controller("tripsController",i)}();