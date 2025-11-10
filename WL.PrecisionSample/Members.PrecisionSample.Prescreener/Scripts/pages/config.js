version = 1;
require.config({
    //base url 
    baseUrl: "/Scripts/",
    waitSeconds: 100,
    urlArgs: "v=" + version,
    //provide all reuired script paths to to js
    paths: {
        angular: 'angular.min',
        jquery: 'jquery-1.12.4',
        bootstrap: 'bootstrap',
        angularSanitize: 'angular-sanitize.min',
        angularTranslate: 'angular-translate.min',
        angularTranslareLoaderPartial: 'angular-translate-loader-partial',
        slider: 'https://cdnjs.cloudflare.com/ajax/libs/angularjs-slider/5.4.0/rzslider.min',
        specialQst: 'pages/ps/splqst',
        customService: 'pages/customServices',
        psApp: 'pages/app',
    },
    //shim
    // load all dependency script  file based on  order
    shim: {
        angular: { exports: 'angular' },
        jquery: { exports: 'jquery' },
        'bootstrap': { deps: ['angular', 'jquery'] },
        'angularSanitize': { deps: ['angular'] },
        'angularTranslate': { deps: ['angular'] },
        'angularTranslareLoaderPartial': { deps: ['angular', 'angularTranslate'] },
        'slider': { deps: ['angular'] },
        'specialQst': { deps: ['angular'] },
        'customService': { deps: ['angular'] },
        psApp: {
            deps: [
              'angular',
              'jquery',
              'bootstrap',
              'angularSanitize',
             'angularTranslate',
             'angularTranslareLoaderPartial',
             'slider',
             'specialQst',
             'customService'
            ]
        }
    },

    deps: ['psApp'], //start app

});
require(['angular', 'psApp'], function (angular, mediaApp) {
    angular.bootstrap(document, ['psApp']);

});


