version = 5;
require.config({
    //base url 
    baseUrl: "/Scripts/",
    waitSeconds: 100,
    urlArgs: "v=" + version,
    //provide all reuired script paths to to js
    paths: {
        angular: 'https://ajax.googleapis.com/ajax/libs/angularjs/1.5.6/angular.min',
        jquery: 'jquery-1.12.4',
        bootstrap: 'bootstrap',
        angularCookies: 'angular-cookies.min',
        angularSanitize: 'angular-sanitize.min',
        angularTranslate: 'angular-translate.min',
        angualrTranslatePluggable: 'translate-pluggable-loader',
        angularTranslareLoaderPartial: 'angular-translate-loader-partial',
        slider: 'https://cdnjs.cloudflare.com/ajax/libs/angularjs-slider/5.4.0/rzslider.min',
        specialQst: 'lead/splqst',
        customService: 'services/customServices',
        customTranslateService: 'services/translationService',
        vrCaptcha: 'https://cdnjs.cloudflare.com/ajax/libs/angular-recaptcha/4.1.1/angular-recaptcha.min',
        app: 'services/app',
    },
    //shim
    // load all dependency script  file based on  order
    shim: {
        angular: { exports: 'angular' },
        jquery: { exports: 'jquery' },
        'bootstrap': { deps: ['angular', 'jquery'] },
        'angularCookies': { deps: ['angular'] },
        'angularSanitize': { deps: ['angular'] },
        'angularTranslate': { deps: ['angular'] },
        'angualrTranslatePluggable': { deps: ['angular', 'angularTranslate'] },
        'angularTranslareLoaderPartial': { deps: ['angular', 'angularTranslate'] },
        'slider': { deps: ['angular'] },
        'specialQst': { deps: ['angular'] },
        'customService': { deps: ['angular', 'angularTranslareLoaderPartial', 'angularCookies'] },
        'vrCaptcha': { deps: ['angular'] },
        'customTranslateService': { deps: ['angular', 'angualrTranslatePluggable', 'customService'] },
        app: {
            deps: [
              'angular',
              'jquery',
              'bootstrap',
              'angularCookies',
              'angularSanitize',
             'angularTranslate',
             'angualrTranslatePluggable',
             //'angularTranslateHandlerLog',
             'angularTranslareLoaderPartial',
             'slider',
              'specialQst',
             'customService',
            'vrCaptcha',
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

