
//all custom services are defined in this module.
angular.module('customSerivces', ['ngCookies'])

//Load translate joson file
.service('translationsLoadingService', ['$location', '$translate', '$translatePartialLoader', '$cookies', function ($location, $translate, $translatePartialLoader, $cookies) {
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
                return '/DailyQstTemp/TxtboxTmp.html;/Scripts/dailyqstcntrl/txtb.js';
            case "2":
                return '/DailyQstTemp/DropDownTmp.html;/Scripts/dailyqstcntrl/ddl.js';
            case "3":
                return '/DailyQstTemp/ChkBoxTmp.html;/Scripts/dailyqstcntrl/chkbox.js';
            case "4":
                return '/DailyQstTemp/RdTemp.html;/Scripts/dailyqstcntrl/rd.js';
            case "5":
                return '/DailyQstTemp/RdmatrixTmp.html;/Scripts/dailyqstcntrl/rdmatrix.js';
            case "7":
                return '/DailyQstTemp/Checkboxmatrix.html;/Scripts/dailyqstcntrl/cbmatrix.js';
            case "8":
                return '/DailyQstTemp/LikertTmp.html;/Scripts/dailyqstcntrl/slidebar.js';
            case "9":
                return '/DailyQstTemp/TxtboxTmp.html;/Scripts/dailyqstcntrl/txtb.js';
            case "10":
                return '/DailyQstTemp/TxtboxTmp.html;/Scripts/dailyqstcntrl/txtb.js';
            case "12":
                return '/DailyQstTemp/CheckboxwithnoneTmp.html;/Scripts/dailyqstcntrl/cbkwrd.js'
            case "13":
                return '/DailyQstTemp/ChkboxwithrdmatrixTmp.html;/Scripts/dailyqstcntrl/checkboxwithrdmatrix.js'
            case "14":
                return '/DailyQstTemp/RdVeritcalTmp.html?v=' + version + ';/Scripts/dailyqstcntrl/rd.js'
        }

    }

})
      //load vechile question make and models
.service("specialQuestionsService", ['vechileqst', function (vechileqst) {
    this.questions = function (data) {
        debugger;
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

    //pagging service
.service('pagerService', ['$q', function ($q) {
    // service definition
    var service = {};
    // service implementation
    service.getPager = function (totalItems, currentPage, stPage, edPage, pageSize) {
        // default to first page
        currentPage = currentPage || 1;

        // default page size is 10
        pageSize = pageSize || 10;

        // calculate total pages
        var totalPages = Math.ceil(totalItems / pageSize);
        var startPage, endPage;
        //total pages less tthan 5 then set starting and ending page 
        if (totalPages <= 5) {
            // less than 10 total pages so show all
            startPage = 1;
            endPage = totalPages;
        } else {
            // more than 10 total pages so calculate start and end pages
            if (currentPage <= 4) {
                startPage = 1;
                endPage = 5;
            }
            else {
                //escap lat 2 pages logic
                if (currentPage < totalPages - 1) {
                    startPage = currentPage - 2;
                    endPage = currentPage + 2;
                }
                else {
                    //check current page equal to 2nd record from last to first.
                    if (currentPage == totalPages - 1) {
                        startPage = stPage;
                        endPage = totalPages;
                    }
                    else {
                        //current page is last page
                        startPage = stPage;
                        endPage = edPage;
                    }
                }

            }
        }

        // calculate start and end item indexes
        // var startIndex = (currentPage - 1) * pageSize;
        // var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        var startIndex = (currentPage * pageSize) - 10;
        var endIndex = Math.min(startIndex + pageSize - 1, totalItems - 1);

        // create an array of pages to ng-repeat in the pager control

        var pages = [];
        // var pages = _.range(startPage, endPage + 1);
        for (var i = startPage; i < endPage + 1; i++) {
            pages.push(i)
        }

        // return object with all pager properties required by the view
        return {
            totalItems: totalItems,
            currentPage: currentPage,
            pageSize: pageSize,
            totalPages: totalPages,
            startPage: startPage,
            endPage: endPage,
            startIndex: startIndex,
            endIndex: endIndex,
            pages: pages
        };
    }
    return service;
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
    //get all avaliable organization list
.service('organizationService', ['$http', '$q', '$location', function () {
    this.getAllOrganization = function () {
        var orgLst = [
            { orgId: -1, bgcolor: '#005D7B' },
            { orgId: 16, bgcolor: '#005D7B' },
            { orgId: 20, bgcolor: '#005D7B' },
            { orgId: 33, bgcolor: '#005D7B' },
            { orgId: 38, bgcolor: '#005D7B' },
            { orgId: 57, bgcolor: '#005D7B' },
            { orgId: 73, bgcolor: '#005D7B' },
            { orgId: 111, bgcolor: '#D83B22' },
            { orgId: 211, bgcolor: '#88BB49' }]
        return orgLst
    }

}])

    //convert dropdown select model value to number
.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                return parseInt(val, 10);
            });
            ngModel.$formatters.push(function (val) {
                return '' + val;
            });
        }
    };
})

    //loading spinner interceptor for each request
.factory('httpInterceptor', function ($q, $rootScope, $log) {
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

    //google recaptcha directive
    .directive('reCaptcha', function () {
        var ddo = {
            restrict: 'AE',
            scope: {},
            require: 'ngModel',
            link: link,
        };
        return ddo;

        function link(scope, elm, attrs, ngModel) {
            var id;
            ngModel.$validators.captcha = function (modelValue, ViewValue) {
                // if the viewvalue is empty, there is no response yet,
                // so we need to raise an error.
                return !!ViewValue;
            };

            function update(response) {
                ngModel.$setViewValue(response);
                ngModel.$render();
            }

            function expired() {
                grecaptcha.reset(id);
                ngModel.$setViewValue('');
                ngModel.$render();
                // do an apply to make sure the  empty response is 
                // proaganded into your models/view.
                // not really needed in most cases tough! so commented by default
                // scope.$apply();
            }

            function iscaptchaReady() {
                if (typeof grecaptcha !== "object") {
                    // api not yet ready, retry in a while
                    return setTimeout(iscaptchaReady, 250);
                }
                id = grecaptcha.render(
                    elm[0], {
                        // put your own sitekey in here, otherwise it will not
                        // function.
                        //6Lct0x8TAAAAANYh_X7Vu_6lPGYd6uUaXzFgMQ7z
                        "sitekey": "6LfGAyMUAAAAAIomckDPmiquU_frFKS5Qe1rRDjz",
                        callback: update,
                        "expired-callback": expired
                    }
                );
            }
            iscaptchaReady();
        }
    })

  //pagging directive
.directive("paggingDirective", ['$compile', '$q', '$http', function ($compile, $q, $http) {
    return {
        restrict: 'E',
        replace: false,
        transclude: true,
        scope: false,
        link: function (scope, element, attrs) {
            var pagging = '<ul ng-if="pager.pages.length > 1" class="pagination"> <li ng-hide="pager.currentPage==1"> <a ng-click="setPage(pager.currentPage - 1)"><i class="fa fa-chevron-circle-left" aria-hidden="true"></i></a> </li><li ng-repeat="page in pager.pages"> <a ng-click="setPage(page)" ng-class="{active:pager.currentPage==page}">{{page}}</a> </li><li ng-hide="pager.currentPage==pager.totalPages"> <a ng-click="setPage(pager.currentPage + 1)"><i class="fa fa-chevron-circle-right" aria-hidden="true"></i></a> </li></ul>'
            // Convert the html to an actual DOM node
            var template = angular.element(pagging);
            // Append it to the directive element
            element.append(template);
            return $compile(element.contents())(scope);
        }
    }
}])

.filter('translateFilter', function ($translate) {
    return function (input, param) {
        if (!param) {
            return input;
        }
        var searchVal = param.key.toLowerCase();
        var result = [];
        angular.forEach(input, function (value) {
            var translated = $translate(value.key);
            if (translated.toLowerCase().indexOf(searchVal) !== -1) {
                result.push(value);
            }
        });
        return result;
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
