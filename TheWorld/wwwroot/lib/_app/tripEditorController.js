!function(){"use strict";
t.$inject = ["t", "o"];function t(t,o){function n(t){if(t&&t.length>0){var o=_.map(t,function(t){return{lat:t.latitude,long:t.longitude,info:t.name}});travelMap.createMap({stops:o,selector:"#map",currentStop:0,initialZoom:3})}}var s=this;s.tripName=t.tripName,s.stops=[],s.errorMessage="",s.isBusy=!0,s.newStop={};var a="/api/trips/"+s.tripName+"/stops";o.get(a).then(function(t){angular.copy(t.data,s.stops),n(s.stops)},function(t){s.errorMessage="Failed to load stops.."}).finally(function(){s.isBusy=!1}),s.addStop=function(){s.isBusy=!0,o.post(a,s.newStop).then(function(t){s.stops.push(t.data),n(s.stops),s.newStops={}},function(t){s.errorMessage="Failed to push new stop to database."}).finally(function(){s.isBusy=!1})}}angular.module("app-trips").controller("tripEditorController",t)}();