
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

        this.trace = function (msg, pagetype) {
            JL('clickstart').trace(msg);
        }
        this.debug = function (msg, pagetype) {
            if (pagetype == 1) {
                JL('clickstart').debug(msg);
            }
            else if (pagetype == 2) {
                JL('piicollectionlog').debug(msg);
            }
            else if (pagetype == 3 && pagetype == 4) {
                JL('veritylog').debug(msg);
            }
            else {
                // JL('Angular').error(msg);
            }
        }
        this.info = function (msg) {
            JL('clickstart').info(msg);
        }
        this.warn = function (msg) {
            JL('clickstart').warn(msg);
        }
        this.error = function (msg, pagetype) {
            JL('clickstarterror').error(msg);
            if (pagetype == 1) {
                JL('clickstarterror').error(msg);
            }
            else if (pagetype == 2) {
                JL('piicollectionerrorlog').error(msg);
            }
            else if (pagetype == 3 && pagetype == 4) {
                JL('verityerrorlog').error(msg);
            }
            else {
                //JL('Angular').error(msg);
            }
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
//Load translate joson file
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
}])

        //----------------- Lazy script loading services ------------------------
.service('lazyscriptLoader', ['$rootScope', '$q', '$compile', function ($rootScope, $q, $compile) {
    // download the javascript file
    var _load = function (dependencies) {
        var deferred = $q.defer();
        //var dependencies = [
        //names
        //];
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
.service('httpService', ['$http', '$q', '$location', 'clickLoggerService', function ($http, $q, $location, clickLoggerService) {
    this.getData = function (url, pageType) {
        var deferred = $q.defer();
        $http({
            method: 'GET',
            url: url,
            headers: {
                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
            }
        }).success(function (response) {
            clickLoggerService.debug(document.referrer + '|' + document.URL + '|' + url + '|' + JSON.stringify(response), pageType)
            //   loggerService.debug(document.referrer + '|' + document.URL + '|' + url + '|' + JSON.stringify(response))
            deferred.resolve(response);
        }).error(function (err, status) {
            // logOut();
            // clickLoggerService.error(document.referrer + '|' + document.URL + '|' + url + '|' + JSON.stringify(err), pageType)
            deferred.reject(err);
        });
        return deferred.promise;
    }
    this.postData = function (url, data, pageType) {
        var ref = document.referrer;
        var deferred = $q.defer();
        $http({
            method: 'POST',
            url: url,
            data: data,
            headers: {
                'Content-Type': 'application/json',
                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
            }
        }).success(function (response) {
            clickLoggerService.debug(document.referrer + '|' + document.URL + '|' + url + '|' + JSON.stringify(response), pageType)
            // loggerService.debug(document.referrer + '|' + document.URL + '|' + url + '|' + JSON.stringify(data) + '|' + JSON.stringify(response))
            deferred.resolve(response);
        }).error(function (err, status) {
            //  clickLoggerService.error(document.referrer + '|' + document.URL + '|' + url + '|' + JSON.stringify(err), pageType)
            // logOut();
            //loggerService.error(document.referrer + '|' + document.URL + '|' + url + '|' + data + '|' + err)
            deferred.reject(err);
            window.location.href = '/cr/error';
        });
        return deferred.promise;
    }
}])

    //each page click get the current page records.
.service('getCurrentPageList', ['$http', '$q', '$location', function () {
    this.getCurrentPageRecords = function (totlaPages, stIndex, edIndex) {
        var currentItems = [];
        var keepGoing = true;
        angular.forEach(totlaPages, function (value, index) {
            //current index must be in b/w starting and endind index
            if (index >= stIndex && index <= edIndex) {
                currentItems.push(value);
            }
            else {
                return false;
            }
        });
        return currentItems;
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

//loader show/hide directive
.directive("loader", function ($rootScope) {
    return function ($scope, element, attrs) {
        ;
        //loder interceptor fire the brodcast event at the time of request
        $scope.$on("loader_show", function () {
            ;
            return element.css('display', 'block');
        });
        //loder interceptor fire the brodcast event at the time of responce
        return $scope.$on("loader_hide", function () {
            return element.css('display', 'none');
        });
    };
})
//loading content directive
.directive("loadingDirective", ['$compile', '$q', '$http', function ($compile, $q, $http) {
    return {
        restrict: 'E',
        replace: true,
        link: function (scope, element, attrs) {
            //loader html tags
            var loader = '<div class="lds-css ng-scope"><div class=lds-ellipsis style=width:100%;height:100%><div><div></div></div><div><div></div></div><div><div></div></div><div><div></div></div><div><div></div></div></div></div>'
            element.html(loader);
            return $compile(element.contents())(scope);
        }
    }
}])
