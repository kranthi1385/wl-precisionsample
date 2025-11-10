version = 8;
require.config({
    //base url 
    baseUrl: "/Scripts/",
    waitSeconds: 0,
    urlArgs: "v=" + version,
    //provide all reuired script paths to to js
    paths: {
        angular: 'https://ajax.googleapis.com/ajax/libs/angularjs/1.5.6/angular.min',
        jquery: 'jquery-1.12.4',
        bootstrap: 'bootstrap',
        angularSanitize: 'angular-sanitize.min',
        angularTranslate: 'angular-translate.min',
        angualrTranslatePluggable: 'translate-pluggable-loader',
        angularTranslareLoaderPartial: 'angular-translate-loader-partial',
        customTranslateService: 'services/translationService',
        customService: 'services/customServices',
        app: 'services/app',
    },
    //shim
    // load all dependency script  file based on  order
    shim: {
        angular: { exports: 'angular' },
        jquery: { exports: 'jquery' },
        'bootstrap': { deps: ['angular', 'jquery'] },
        'angularSanitize': { deps: ['angular'] },
        'angularTranslate': { deps: ['angular'] },
        'angualrTranslatePluggable': { deps: ['angular', 'angularTranslate'] },
        'angularTranslareLoaderPartial': { deps: ['angular', 'angularTranslate'] },
        'customService': { deps: ['angular', 'angularTranslareLoaderPartial'] },
        'customTranslateService': { deps: ['angular', 'angualrTranslatePluggable', 'customService'] },
        app: {
            deps: [
              'angular',
              'jquery',
              'bootstrap',
              'angularSanitize',
             'angularTranslate',
             'angualrTranslatePluggable',
             //'angularTranslateHandlerLog',
             'angularTranslareLoaderPartial',
             'customService',
              'customTranslateService',
            ]
        }
    },

    deps: ['app'], //start app

});
require(['angular', 'app'], function (angular, mediaApp) {
    angular.bootstrap(document, ['app']);

});

//require(["angular"], function (react) {
//    console.log("React loaded OK.");
//});

//require(["angularRoute"], function (jsx) {
//    console.log("angularRouter loaded OK." + jsx);
//});

