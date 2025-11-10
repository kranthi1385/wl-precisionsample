//all custom services are defined in this module.
angular.module('customSerivces', [])
.service('loggerService', ['$log', function ($log) {
    //this.trace = function (msg) {
    //    JL('Angular').trace(msg);
    //}
    this.debug = function (msg) {
        JL('Angular').debug(msg);
    }
    //this.info = function (msg) {
    //    JL('Angular').info(msg);
    //}
    //this.warn = function (msg) {
    //    JL('Angular').warn(msg);
    //}
    this.error = function (msg) {
        JL('Angular').error(msg);
    }
}])
    .service('clickLoggerService', ['$log', function ($log) {
        this.trace = function (msg) {
            JL('recaptchaInfo').trace(msg);
        }
        this.debug = function (msg) {
            JL('recaptchaInfo').debug(msg);
        }
        this.info = function (msg) {
            JL('recaptchaInfo').info(msg);
        }
        this.error = function (msg) {
            JL('jsErrorLoggFile').error(msg);
        }
    }])

.factory('$exceptionHandler', function () {
    return function (exception, cause) {
        JL('jsErrorLoggFile').fatalException(cause, exception);
        throw exception;
    };
})

.factory('logToServerInterceptor', ['$q', function ($q) {
    var myInterceptor = {
        'request': function (config) {
            config.msBeforeAjaxCall = new Date().getTime();
            return config;
        },
        'response': function (response) {
            if (response.config.warningAfter) {
                var msAfterAjaxCall = new Date().getTime();
                var timeTakenInMs =
                      msAfterAjaxCall - response.config.msBeforeAjaxCall;
                if (timeTakenInMs > response.config.warningAfter) {
                    JL('Angular.Ajax').warn({
                        timeTakenInMs: timeTakenInMs,
                        config: response.config,
                        data: response.data
                    });
                }
            }
            return response;
        },
        'responseError': function (rejection) {
            var errorMessage = "timeout";
            if (rejection && rejection.status && rejection.data) {
                errorMessage = rejection.data.ExceptionMessage;
            }
            //JL('Angular.Ajax').fatalException({
            //    errorMessage: errorMessage,
            //    status: rejection.status,
            //    config: rejection.config
            //}, rejection.data);
            return $q.reject(rejection);
        }
    };
    return myInterceptor;
}])

.service('translationsLoadingService', ['$location', '$translate', '$translatePartialLoader', 'loggerService', function ($location, $translate, $translatePartialLoader, loggerService) {
    this.loadTranslatePagePath = function (pgName) {
        // load Current Language Trnanslation Page.
        if (pgName != '') {
            $translatePartialLoader.addPart(pgName);
        }
        $translate.refresh();
    }
    this.loadTranslationHeader = function () {
        // $translatePartialLoader.addPart('header');
    }
    this.getCurrentLanguagePath = function () {
        return '/app/json/' + $translate.use()
    }
    this.setCurrentUserLanguage = function (langName) {
        $translate.use(langName);
        $translate.refresh();
    }
    this.writeNlogService = function () {
        //nlog formate is {AbsoluteUrl | RequestUrl | PostData | ResponseData }
        var msg = document.URL + '|' + '' + '|' + '' + '|' + '';
        loggerService.debug(msg)
    }
    this.clickLoggerService = function () {
        //nlog formate is {AbsoluteUrl | RequestUrl | PostData | ResponseData }
        var msg = document.URL + '|' + '' + '|' + '' + '|' + '';
        loggerService.debug(msg)
    }
}])

    //----------------- Lazy script loading services ------------------------
.service('lazyscriptLoader', ['$rootScope', '$q', '$compile', function ($rootScope, $q, $compile) {
    // download the javascript file
    var _load = function (names) {
        var deferred = $q.defer();
        var dependencies = [
		names
        ];
        require(dependencies, function () {
            $rootScope.$apply(function () {
                deferred.resolve();
            });
        });
        return deferred.promise;
    }

    return {
        load: _load
    };

}])


  //all http services.
.service('httpService', ['$http', '$q', '$location', function ($http, $q, $location) {
    this.getData = function (url) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: url,
            headers: {
                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
            }
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            // logOut();
            deferred.reject(err);
        });
        return deferred.promise;
    }
    this.postData = function (url, data, antiForgeryToken) {
        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: url,
            data: data,
            headers: {
                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
            }
        }).success(function (response) {
            deferred.resolve(response);
        }).error(function (err, status) {
            // logOut();
            deferred.reject(err);
        });
        return deferred.promise;
    }
}])

 //reqd query params function
.service('getQueryParams', ['$http', '$q', '$location', function () {
    this.getUrlVars = function () {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }

}])


//loading spinner interceptor for each request
.factory('loadingInterceptorService', function ($q, $rootScope, $log) {
    var numLoadings = 0;
    return {
        request: function (config) {
            numLoadings++;
            // Show loader
            $rootScope.$broadcast("loader_show");
            return config || $q.when(config)

        },
        response: function (response) {
            if ((--numLoadings) === 0) {
                // Hide loader
                $rootScope.$broadcast("loader_hide");
            }
            return response || $q.when(response);

        },
        responseError: function (response) {

            if (!(--numLoadings)) {
                // Hide loader
                $rootScope.$broadcast("loader_hide");
            }

            return $q.reject(response);
        }
    };
})