define(['app'], function (app) {
    app.register.controller('homeController', ['$rootScope', '$scope', 'translationsLoadingService', 'httpService', 'loadQuestionFilter', 'questionService',
        function ($rootScope, $scope, translationsLoadingService, httpService, loadQuestionFilter, questionService) {
            translationsLoadingService.loadTranslatePagePath("hm");
            //expand flag used for expand or collapse text in moblies
            var selectedLanguage = this;
            $scope.isexpand = true;
            $scope.showSurveys = true;
            $scope.totalRewCount = 0;
            $scope.avaliableReward = 0;
            $scope.referralsReward = 0;
            $scope.isShowQst = true;
            //get user details
            httpService.getData('/hm/GetUserDetails').then(function (response) {
                $scope.user = response;
                //get all user avaliable surveys
                httpService.getData('/hm/GetAllAvaliableSurveys').then(function (response1) {
                    if (response1 != '') {
                        if (response1[0].ProjectId == 0) {
                            $scope.showSurveys = false;
                        }
                        else {
                            $scope.surveys = response1;
                        }
                    }
                    else {
                        $scope.showSurveys = false;
                        $scope.surveys = '';
                    }
                    $scope.totalRewCount = $scope.surveys.length;
                    angular.forEach($scope.surveys, function (survey, key) {
                        $scope.avaliableReward += survey.MemberReward;
                    });
                }, function (err) {
                });

                //get all referrer details
                httpService.getData('/hm/GetUserReferrerDetails?UsId=' + $scope.user.UserId).then(function (response2) {
                    $scope.referrals = response2;
                    $scope.referralsReward = $scope.referrals.RewardsEarned;
                }, function (err) {
                });
                httpService.getData('/hm/GetPollQuestions?UsId=' + $scope.user.UserId).then(function (res) {
                    getQstResponse(res);
                });
                //get all avaliables profiles
                httpService.getData('/hm/GetAllProfiles').then(function (response3) {
                    $scope.profiles = response3;
                }, function (err) {

                });
            }, function (err) {
            });

            $scope.popup = function (url) {
                var width = 990;
                var height = 600;
                var left = (screen.width - width) / 2;
                var top = (screen.height - height) / 2;
                var params = 'width=' + width + ', height=' + height;
                params += ', top=' + top + ', left=' + left;
                params += ', directories=no';
                params += ', location=no';
                params += ', menubar=no';
                params += ', resizable=no';
                params += ', scrollbars=yes';
                params += ', status=no';
                params += ', toolbar=no';
                newwin = window.open(url, 'windowname5', params);
                if (window.focus) { newwin.focus() }
                return false;
            }


            //expand click. . isexpand is true show all data otherwise hide data.
            $scope.expand = function (isExpand) {
                $scope.isexpand = isExpand
            }

            $scope.submitValidations = false;
            var getQstResponse = function (res) {
                if (res.length >= 0) {
                    $rootScope.isShowFooter = true;
                    $scope.isShowQst = true;
                    $scope.questions = specialQuestionsService.questions(res);
                }
            }
            $scope.getQuestion = function () {

                httpService.getData('/hm/GetPollQuestions?UsId=' + $scope.user.UserId).then(function (res) {
                    getQstResponse(res);
                })
                document.getElementById("optionPoll").style.display = 'none';
                document.getElementById("polldv").innerHTML = "";
            }
            $scope.save = function (validate) {
                // $scope.plotBarChar(["hello", "hi", "bye"], [15, 20, 30])
                if (validate) {
                    var sortOrder = 0;
                    var xml = questionService.buildXml($scope.questions);
                    var qId = $scope.questions[0].QuestionId;
                    if ($scope.questions[0].CurrentSortOrder != 0) {
                        sortOrder = $scope.questions[0].CurrentSortOrder;
                    }
                    var data = { 'xml': xml };
                    httpService.postData('/hm/SavePollOptions?UsId=' + $scope.user.UserId + '&qId=' + qId + '&sr=' + sortOrder, data).then(function (res) {
                        $scope.submitValidations = false;
                        $scope.isShowQst = false;
                        $scope.radiobtnvalidation = -1;
                        $rootScope.psQstLoad = false;
                        if (res != "") {
                            var polldata = JSON.parse(res);
                            var y = []
                            var x = []
                            for (i = 0; i < polldata.length; i++) {
                                y.push(polldata[i].option_text)
                                x.push(polldata[i].user_count)
                            }
                            // $scope.questions = [];
                            $scope.plotBarChar(x, y);
                                 setTimeout($scope.getQuestion, 10000);
                        }
                    });
                }
                else {
                    $scope.submitValidations = true;
                }
            }
            Array.prototype.max = function () {
                var r = this[0];
                this.forEach(function (v, i, a) { if (v > r) r = v; });
                return r;
            };
            function getRandomColor() {
                var letters = '0123456789ABCDEF'.split('');
                var color = '#';
                for (var i = 0; i < 6; i++) {
                    color += letters[Math.floor(Math.random() * 16)];
                }
                return color;
            }

            function getRandomColorEachEmployee(count) {
                var data = [];
                for (var i = 0; i < count; i++) {
                    data.push(getRandomColor());
                }
                return data;
            }
            $scope.plotBarChar = function (yaxis, xaxis) {
                document.getElementById("polldv").innerHTML = '<canvas id="optionPoll"></canvas>'
                var chart = document.getElementById("optionPoll").getContext("2d");
                chart.canvas.parentNode.style.height = "300px";
                chart.canvas.parentNode.style.width = "600px";
                ymax = yaxis.max();
                new Chart(chart, {
                    type: 'horizontalBar',
                    data: {
                        labels: xaxis.length == 0 ? [] : xaxis,
                        datasets: [
                          {
                              label: "Users Poll",
                              data: yaxis,
                              backgroundColor: getRandomColorEachEmployee(yaxis.length)
                          }
                        ]
                    },

                    options: {
                        legend: { display: false },
                        title: {
                            display: true,
                            text: 'User Count'
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    autoSkip: false
                                },

                                gridLines: {
                                    display: false
                                }
                            }],
                            xAxes: [{
                                ticks: {
                                    beginAtZero: true,
                                    steps: 10,
                                    stepValue: 5,
                                    max: 100,
                                    callback: function (value) {
                                        return value + '%'
                                    }
                                },
                                gridLines: {
                                    display: false
                                }
                            }]
                        },
                        tooltips: {
                            callbacks: {
                                label: function (tooltipItem) {
                                    return Number(tooltipItem.xLabel) + "%";
                                }
                            }
                        }

                    }
                });

            }
        }]);
});