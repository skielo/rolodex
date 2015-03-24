var myApp = angular.module('RolodexApp', []);
myApp.controller('MainController', ['$scope', '$http', function ($scope, $http) {
    $http.get("/api/contacts")
        .then(function (response) {
            return response.data;
        });
}]);