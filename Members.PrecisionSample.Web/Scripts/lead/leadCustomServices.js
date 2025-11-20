
//all custom services are defined in this module.
angular.module('customSerivces', [])


//.service('loggerService', ['$log', function ($log) {
//    this.trace = function (msg) {
//        JL('Angular').trace(msg);
//    }
//    this.debug = function (msg) {
//        JL('Angular').debug(msg);
//    }
//    this.info = function (msg) {
//        JL('Angular').info(msg);
//    }
//    this.warn = function (msg) {
//        JL('Angular').warn(msg);
//    }
//    this.error = function (msg) {
//        JL('Angular').error(msg);
//    }
//}])
////.factory('$exceptionHandler', function () {
////    return function (exception, cause) {
////        JL('Angular').fatalException(cause, exception);
////        throw exception;
////    };
////})

//.factory('logToServerInterceptor', ['$q', function ($q) {
//    var myInterceptor = {
//        'request': function (config) {
//            config.msBeforeAjaxCall = new Date().getTime();
//            return config;
//        },
//        'response': function (response) {
//            if (response.config.warningAfter) {
//                var msAfterAjaxCall = new Date().getTime();
//                var timeTakenInMs =
//                      msAfterAjaxCall - response.config.msBeforeAjaxCall;
//                if (timeTakenInMs > response.config.warningAfter) {
//                    JL('Angular.Ajax').warn({
//                        timeTakenInMs: timeTakenInMs,
//                        config: response.config,
//                        data: response.data
//                    });
//                }
//            }
//            return response;
//        },
//        'responseError': function (rejection) {
//            var errorMessage = "timeout";
//            if (rejection && rejection.status && rejection.data) {
//                errorMessage = rejection.data.ExceptionMessage;
//            }
//            JL('Angular.Ajax').fatalException({
//                errorMessage: errorMessage,
//                status: rejection.status,
//                config: rejection.config
//            }, rejection.data);
//            return $q.reject(rejection);
//        }
//    };
//    return myInterceptor;
//}])
//Load translate joson file
.service('translationsLoadingService', ['$location', '$translate', '$translatePartialLoader', function ($location, $translate, $translatePartialLoader) {
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
    //this.writeNlogService = function () {
    //    //nlog formate is {AbsoluteUrl | RequestUrl | PostData | ResponseData }
    //    var msg = document.URL + '|' + '' + '|' + '' + '|' + '';
    //    loggerService.debug(msg)
    //}
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

    //load html pages question wise
.filter("loadQuestion", function () {
    return function (questionTypeId) {
        // do some bounds checking here to ensure it has that index
        switch (questionTypeId) {
            case "1":
                return '/QstTemp/TxtboxTmp.html;/Scripts/qstcntrl/txtb.js';
            case "2":
                return '/QstTemp/DropDownTmp.html;/Scripts/qstcntrl/ddl.js';
            case "3":
                return '/QstTemp/ChkBoxTmp.html;/Scripts/qstcntrl/chkbox.js';
            case "4":
                return '/QstTemp/RdTemp.html;/Scripts/qstcntrl/rd.js';
            case "5":
                return '/QstTemp/RdmatrixTmp.html;/Scripts/qstcntrl/rdmatrix.js';
            case "7":
                return '/QstTemp/Checkboxmatrix.html;/Scripts/qstcntrl/cbmatrix.js';
            case "8":
                return '/QstTemp/LikertTmp.html;/Scripts/qstcntrl/slidebar.js';
            case "9":
                return '/QstTemp/TxtboxTmp.html;/Scripts/qstcntrl/txtb.js';
            case "10":
                return '/QstTemp/TxtboxTmp.html;/Scripts/qstcntrl/txtb.js';
            case "12":
                return '/QstTemp/CheckboxwithnoneTmp.html;/Scripts/qstcntrl/cbkwrd.js'
            case "13":
                return '/QstTemp/ChkboxwithrdmatrixTmp.html;/Scripts/qstcntrl/checkboxwithrdmatrix.js'
            case "14":
                return '/QstTemp/RdVeritcalTmp.html?v=' + version + ';/Scripts/qstcntrl/rd.js'
        }

    }

})
      //load vechile question make and models
.service("specialQuestionsService", ['vechileqst', function (vechileqst) {
    this.questions = function (data) {
        angular.forEach(data, function (qst) {
            if (qst.QuestionId == 573) {
                qst.OptionDisplay = 'H';
            }
            //heath pfofile questions display in vertical manner so implementing custom logic
            //---------- Logic Start ----------------------//
            if (qst.QuestionId == 850 || qst.QuestionId == 905 || qst.QuestionId == 928 || qst.QuestionId == 948) {
                var specialOptinLst = [];
                qst.SpecialOptinLst = [];
                var sqpQstCount = 0;
                angular.forEach(qst.ChildQuestionList, function (chq, chindx) {
                    var indOptLst = [];
                    //------------- insert  all child question optons into temp array ---------//
                    angular.forEach(chq.OptionList, function (op, index) {
                        indOptLst.push(op);
                        indOptLst[index].QuesitonText = chq.QuestionText;
                        indOptLst[index].ParentQuestionId = chq.ParentQuestionId;
                    });
                    if (chindx == 0) {
                        sqpQstCount = indOptLst.length - 1; // --- get length of child questions ---------//
                    }
                    angular.forEach(indOptLst, function (opt) {
                        specialOptinLst.push(opt);
                    });
                });
                for (var j = 0; j <= sqpQstCount; j++) {
                    var qstOpt = [];
                    angular.forEach(specialOptinLst, function (sqpop) {
                        if (sqpop.SpecialGroupingId == j + 1) { // -- check special groupingid with current value --//
                            qstOpt.push(sqpop);
                        }
                    });
                    qst.SpecialOptinLst.push(qstOpt); // ---- insert options list into parent level SpecialOptinLst arry -- //
                    qst.SpecialOptinLst[j].OptidsLst = [];
                }
            }
            if (qst.QuestionId == 175) {
                var qustsandopts = vechileqst;
                var childQuestions = [];
                var childOptions = [];
                var subChildQuestions = [];
                var subChildOptions = [];
                var childOptions = [];
                var subChildqs = [];
                var subChildops = [];
                childQuestions = qustsandopts.getquestions(qst.QuestionId); //based on parent questionId get child questions
                //angular.forEach(childQuestions, function (chq) {
                //    childQuestions.push(chq);
                //});
                qst.ChildQuestionList = [];
                angular.forEach(childQuestions, function (chqlst, index) {
                    childOptions = [];
                    qst.ChildQuestionList.push(chqlst);
                    childOptions.push(qustsandopts.getoptions(chqlst.QuestionId)); //get options based on questionId
                    qst.ChildQuestionList[index].OptionList = [];
                    qst.ChildQuestionList[index].SubChildOptions = [];
                    angular.forEach(childOptions, function (chqoptlst, chqindex) {
                        angular.forEach(chqoptlst, function (chop, opindex) {
                            childOptions[chqindex][opindex].ListChildQuestionId = [];
                            qst.ChildQuestionList[index].OptionList.push(childOptions[chqindex][opindex]);
                        });

                    });
                    subChildqs = [];
                    subChildops = [];
                    subChildqs = qustsandopts.getquestions(chqlst.QuestionId);
                    angular.forEach(subChildqs, function (subchqlst, subqsindex) {
                        qst.ChildQuestionList[index].ChildQuestionList.push(subchqlst);
                        qst.ChildQuestionList[index].ChildQuestionList[subqsindex].OptionList = [];
                        subChildops.push(qustsandopts.getoptions(subchqlst.QuestionId)); // get child question based on the parent questionId
                        angular.forEach(subChildops, function (subchoplst, subopsndex) {
                            angular.forEach(subchoplst, function (subchop, subopindex) {
                                subChildops[subopsndex][subopindex].ListChildQuestionId = [];
                                qst.ChildQuestionList[index].ChildQuestionList[subqsindex].OptionList.push(subChildops[subopsndex][subopindex]);
                                if (qst.ChildQuestionList[index].ChildQuestionList[subqsindex].QuestionId == 190
                                    || qst.ChildQuestionList[index].ChildQuestionList[subqsindex].QuestionId == 200
                                    || qst.ChildQuestionList[index].ChildQuestionList[subqsindex].QuestionId == 208
                                    || qst.ChildQuestionList[index].ChildQuestionList[subqsindex].QuestionId == 216) {
                                    qst.ChildQuestionList[index].SubChildOptions.push(subChildops[subopsndex][subopindex]);
                                }
                            });

                        });
                    });
                });


            }
        });
        return data;
    }

}])
    //load vechile question make and models
.service('questionService', ['$http', '$q', function ($http, $q) {
    this.buildXml = function (questions) {
        var count = 0;
        var answerXML = '';
        answerXML += "<profiles>";
        for (var m = 0; m < questions.length; m++) {
            var _qtypeid = questions[m].QuestionTypeId;
            // var _opttypeid = questions[m].OptionTypeId;
            // Checkbox Question type - More than one option may select

            if (_qtypeid == 3) {
                if (questions[m].OptionList != null) {
                    for (var n = 0; n < questions[m].OptionList.length; n++) {
                        if (questions[m].OptionList[n].IsChecked) {
                            answerXML += "<profile>";
                            //answerXML += "<user_id>" + uid + "</user_id>";
                            answerXML += "<question_id>" + questions[m].OptionList[n].QuestionId + "</question_id>";
                            answerXML += "<option_id>" + questions[m].OptionList[n].OptionId + "</option_id>";
                            answerXML += "<option_text><![CDATA[" + questions[m].OptionList[n].OptionText + " ]]></option_text>";
                            answerXML += "</profile>";
                            if (questions[m].SelectedChildQuestionList != null) {
                                if (questions[m].SelectedChildQuestionList.length > 0) {
                                    if (count == 0) {
                                        count = 1;
                                        for (var p = 0; p < questions[m].SelectedChildQuestionList.length; p++) {
                                            for (var q = 0; q < questions[m].SelectedChildQuestionList[p].OptionList.length; q++) {
                                                if (questions[m].SelectedChildQuestionList[p].OptionLis[q].IsChecked) {

                                                    answerXML += "<profile>";
                                                    //answerXML += "<user_id>" + uid + "</user_id>";
                                                    answerXML += "<question_id>" + questions[m].SelectedChildQuestionList[p].OptionList[q].QuestionId + "</question_id>";
                                                    answerXML += "<option_id>" + questions[m].SelectedChildQuestionList[p].OptionList[q].OptionId + "</option_id>";
                                                    answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                                                    answerXML += "</profile>";
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

            }
            else if (_qtypeid == 12) { // multi select  with none option
                if (questions[m].OptionList != null) {
                    for (var n = 0; n < questions[m].OptionList.length; n++) {
                        if (questions[m].OptionList[n].IsChecked) {
                            answerXML += "<profile>";
                            //answerXML += "<user_id>" + uid + "</user_id>";
                            answerXML += "<question_id>" + questions[m].OptionList[n].QuestionId + "</question_id>";
                            answerXML += "<option_id>" + questions[m].OptionList[n].OptionId + "</option_id>";
                            answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                            answerXML += "</profile>";
                        }


                    }
                    if (questions[m].OptionId != 0 && questions[m].OptionId != null) {
                        // Radio Question type with other
                        answerXML += "<profile>";
                        //answerXML += "<user_id>" + uid + "</user_id>";
                        answerXML += "<question_id>" + questions[m].QuestionId + "</question_id>";
                        answerXML += "<option_id>" + questions[m].OptionId + "</option_id>";
                        answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                        answerXML += "</profile>";
                    }
                }
            }
            else if (_qtypeid == 1 || _qtypeid == 9 || _qtypeid == 10) {
                answerXML += "<profile>";
                answerXML += "<question_id>" + questions[m].QuestionId + "</question_id>";
                answerXML += "<option_id>" + questions[m].OptionId + "</option_id>";
                answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                answerXML += "</profile>";

            }
                //likert scale
            else if (questions[m].QuestionTypeId == 8) {
                for (var j = 0; j < questions[m].OptionList.length; j++) {
                    if (parseInt(questions[m].OptionId) == questions[m].OptionList[j].OptionId) {
                        answerXML += "<profile>";
                        //answerXML += "<user_id>" + uid + "</user_id>";
                        answerXML += "<question_id>" + questions[m].QuestionId + "</question_id>";
                        answerXML += "<option_id>" + questions[m].OptionList[j].OptionId + "</option_id>";
                        answerXML += "<option_text>" + questions[m].OptionList[j].OptionText + "</option_text>";
                        answerXML += "</profile>";
                    }
                }
            }
            else if (questions[m].QuestionTypeId == 13) {
                for (var _opt in questions[m].SpecialOptinLst) {
                    for (var _mdop in questions[m].SpecialOptinLst[_opt]) {
                        //if (questions[m].SpecialOptinLst[_opt][_mdop].OptionText.toLowerCase() == 'none of the above' ||
                        //       questions[m].SpecialOptinLst[_opt][_mdop].OptionText.toLowerCase() == 'prefer not to answer') {
                        //    if (questions[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId != 0 && questions[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId != "") {
                        //        answerXML += "<profile>";
                        //        answerXML += "<user_id>" + questions[m].UserId + "</user_id>";
                        //        answerXML += "<question_id>" + questions[m].SpecialOptinLst[_opt][_mdop].QuestionId + "</question_id>";
                        //        answerXML += "<option_id>" + questions[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId + "</option_id>";
                        //        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        //        answerXML += "</profile>";
                        //    }

                        //}
                        //else {
                        if (questions[m].SpecialOptinLst[_opt][_mdop].IsChecked) {
                            answerXML += "<profile>";
                            //answerXML += "<user_id>" + questions[m].UserId + "</user_id>";
                            answerXML += "<question_id>" + questions[m].SpecialOptinLst[_opt][_mdop].QuestionId + "</question_id>";
                            answerXML += "<option_id>" + questions[m].SpecialOptinLst[_opt][_mdop].OptionId + "</option_id>";
                            //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                            answerXML += "</profile>";
                        }
                        //}
                    }
                }

            }
            else {
                // Radio Question type with other
                answerXML += "<profile>";
                //answerXML += "<user_id>" + uid + "</user_id>";
                answerXML += "<question_id>" + questions[m].QuestionId + "</question_id>";
                answerXML += "<option_id>" + questions[m].OptionId + "</option_id>";
               // questions[m].OptionText
                answerXML += "<option_text>" + "" + "</option_text>";
                answerXML += "</profile>";
                if (questions[m].SelectedChildQuestionList != null) {
                    for (var n = 0; n < questions[m].SelectedChildQuestionList.length; n++) {
                        if (questions[m].SelectedChildQuestionList[n].OptionId != 0) {
                            answerXML += "<profile>";
                            // answerXML += "<user_id>" + uid + "</user_id>";
                            answerXML += "<question_id>" + questions[m].SelectedChildQuestionList[n].QuestionId + "</question_id>";
                            answerXML += "<option_id>" + questions[m].SelectedChildQuestionList[n].OptionId + "</option_id>";
                            answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                            answerXML += "</profile>";
                            if (questions[m].SelectedChildQuestionList[n].ChildQuestionList != null) {
                                for (var p = 0; p < questions[m].SelectedChildQuestionList[n].ChildQuestionList.length; p++) {
                                    if (questions[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionTypeId == 5) {
                                        answerXML += "<profile>";
                                        // answerXML += "<user_id>" + uid + "</user_id>";
                                        answerXML += "<question_id>" + questions[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionId + "</question_id>";
                                        answerXML += "<option_id>" + questions[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId + "</option_id>";
                                        answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                                        answerXML += "</profile>";
                                    }
                                }
                            }
                        }
                        else {
                            if (questions[m].SelectedChildQuestionList[n].ChildQuestionList != null) {
                                for (var p = 0; p < questions[m].SelectedChildQuestionList[n].ChildQuestionList.length; p++) {
                                    if (questions[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionTypeId == 5) {
                                        answerXML += "<profile>";
                                        // answerXML += "<user_id>" + uid + "</user_id>";
                                        answerXML += "<question_id>" + questions[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionId + "</question_id>";
                                        answerXML += "<option_id>" + questions[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId + "</option_id>";
                                        answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                                        answerXML += "</profile>";
                                    }
                                }
                            }
                        }

                    }
                }

            }


            //DropdownQuestionType             

            //matrix questions
            if (questions[m].ChildQuestionList != null) {
                if (questions[m].ChildQuestionList.length != 0) {
                    for (var p = 0; p < questions[m].ChildQuestionList.length; p++) {
                        if (questions[m].ChildQuestionList[p].QuestionTypeId == 5) {
                            answerXML += "<profile>";
                            //answerXML += "<user_id>" + uid + "</user_id>";
                            answerXML += "<question_id>" + questions[m].ChildQuestionList[p].QuestionId + "</question_id>";
                            answerXML += "<option_id>" + questions[m].ChildQuestionList[p].OptionId + "</option_id>";
                            answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                            answerXML += "</profile>";

                        }
                        else if (questions[m].ChildQuestionList[p].QuestionTypeId == 7) {
                            for (var s = 0; s < questions[m].ChildQuestionList[p].OptionList.length; s++) {
                                if (questions[m].ChildQuestionList[p].OptionList[s].IsChecked) {
                                    answerXML += "<profile>";
                                    //answerXML += "<user_id>" + uid + "</user_id>";
                                    answerXML += "<question_id>" + questions[m].ChildQuestionList[p].QuestionId + "</question_id>";
                                    answerXML += "<option_id>" + questions[m].ChildQuestionList[p].OptionList[s].OptionId + "</option_id>";
                                    answerXML += "<option_text>" + questions[m].ChildQuestionList[p].OptionList[s].OptionText + "</option_text>";
                                    answerXML += "</profile>";
                                }
                            }
                        }
                        else if (questions[m].ChildQuestionList[p].QuestionTypeId == 2) {
                            if (questions[m].ChildQuestionList[p].SelectedChildQuestionList != null) {
                                for (var q = 0; q < questions[m].ChildQuestionList[p].SelectedChildQuestionList.length; q++) {
                                    if (questions[m].ChildQuestionList[p].SelectedChildQuestionList[q].OptionId != 0) {
                                        answerXML += "<profile>";
                                        //answerXML += "<user_id>" + uid + "</user_id>";
                                        answerXML += "<question_id>" + questions[m].ChildQuestionList[p].SelectedChildQuestionList[q].QuestionId + "</question_id>";
                                        answerXML += "<option_id>" + questions[m].ChildQuestionList[p].SelectedChildQuestionList[q].OptionId + "</option_id>";
                                        answerXML += "<option_text>" + questions[m].OptionText + "</option_text>";
                                        answerXML += "</profile>";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        answerXML += "</profiles>";
        return answerXML;
    }
}])
.directive('templateDirective', ['$compile', '$rootScope', '$http', 'lazyscriptLoader', 'loadQuestionFilter',
          function ($compile, $rootScope, $http, lazyscriptLoader, loadQuestionFilter) {
              return {
                  restrict: 'E',
                  replace: false,
                  transclude: true,
                  scope: false,
                  link: function (scope, element, attrs) {
                      var url = loadQuestionFilter(attrs.questionTypeId);
                      lazyscriptLoader.load(url.split(';')[1]).then(function () {
                          $rootScope.psQstLoad = true;
                          //var url = '/Templates/QuestionTmp.html'
                          $http.get('http://dev.affiliate.sdl.com' + url.split(';')[0]).then(function (html) {
                              // Convert the html to an actual DOM node
                              var template = angular.element(html.data);
                              // Append it to the directive element
                              element.append(template);
                              return $compile(element.contents())(scope);
                          });
                      });


                  }
              }

          }])
.filter('unsafe', function ($sce) {
    return function (val) {
        return $sce.trustAsHtml(val);
    };

})
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
            //loggerService.debug(document.URL + '|' + url + '|' + " " + '|' + JSON.stringify(response))
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
                "Content-Type": "application/json",
                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
            }
        }).success(function (response) {
            //loggerService.debug(document.URL + '|' + url + '|' + JSON.stringify(data) + '|' + JSON.stringify(response))
            deferred.resolve(response);
        }).error(function (err, status) {
            // logOut();
            //loggerService.error(document.URL + '|' + url + '|' + data + '|' + err)
            deferred.reject(err);
        });
        return deferred.promise;
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
        //loder interceptor fire the brodcast event at the time of request
        $scope.$on("loader_show", function () {
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