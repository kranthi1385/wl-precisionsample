define(['app'], function (app) {
    app.register.controller("textBoxController", function ($scope, $compile) {
        console.log("Controller instantiated (after bootstrap).");
    });
});