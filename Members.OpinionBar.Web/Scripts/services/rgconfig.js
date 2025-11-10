version = 3;
debugger;
require.config({
    //base url 
    baseUrl: "/Scripts/",
    waitSeconds: 100,
    //provide all reuired script paths to to js
    paths: {
        angular: 'https://ajax.googleapis.com/ajax/libs/angularjs/1.5.6/angular.min',
        jquery: 'jquery-1.12.4',
        bootstrap: 'bootstrap',
        angularSanitize: 'angular-sanitize.min',
        angularTranslate: 'angular-translate.min',
        angualrTranslatePluggable: 'translate-pluggable-loader',
        angularTranslareLoaderPartial: 'angular-translate-loader-partial',
        customService: 'services/customServices',
        customTranslateService: 'services/translationService',
        rgApp: 'services/rgapp',
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
        rgApp: {
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

    deps: ['rgApp'], //start app

});
require(['angular', 'rgApp'], function (angular, mediaApp) {
    angular.bootstrap(document, ['rgApp']);

});

