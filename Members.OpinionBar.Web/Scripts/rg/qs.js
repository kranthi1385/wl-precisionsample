//declare Name of the namespace

var LoadQtns = function () {
    var that = {};
    ViewModel = ko.observableArray();
    //Decalre variable here
    var save = '';
    var _ddlclick = 0;
    var _rdAnswers = '';
    //var pfid = '49FAA29F-3C4C-4E45-AA8F-7A5F79729F96'; 
    //var pfid = '8BB9309A-8692-4BCF-B3BE-EFEDD5D6F5D5'; // matrix questions
    // to get URL params
    function getUrlVars() {
        var Url = window.location.href.toLowerCase();
        var vars = {};
        var parts = Url.replace(/[?&]+([^=&]+)=([^&]*)/gi, function (m, key, value) {
            vars[key] = value;
        });
        return vars;
    }
    //Get query parmas value profile id
    pid = getUrlVars()["pid"];
    if (pid == undefined) {
        pid = '';
    }
    ug = getUrlVars()["ug"];
    if (ug == undefined) {
        ug = '';
    }
    var cid = getUrlVars()["cid"];
    if (cid == undefined) {
        cid = '';
    }
    var scid = getUrlVars()["scid"];
    if(scid == undefined){
        scid = '';
    }
    //Binding the questions
    var bindQuestions = function () {
        $.ajax({
            url: '/Mem/ProfileGet?pid=' + pid + '&ug=' + ug + '&cid=' + cid + '&scid=' + scid,
            type: 'GET',
            headers: {
                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
            },
            success: function (Pagedata) {
               
                if (Pagedata != null) {
                    orgInfo = Pagedata[0].OrgInfo.split(';');//get the org information
                    langId = Pagedata[0].LanguageID;
                    if (langId == 44) {
                        translatedSelectText = ko.observable('-- Sélectionnez --'); // French
                    } else if (langId == 140) {
                        translatedSelectText = ko.observable('-- Seleccione --'); // Spanish
                    } else if (langId == 51) {
                        translatedSelectText = ko.observable('-- Wählen --'); // German
                    } else if (langId == 111) {
                        translatedSelectText = ko.observable('-- Selecione --'); // Portuguese
                    } else if (langId == 120) {
                        translatedSelectText = ko.observable('-- Выбрать --'); // Russian
                    } else if (langId == 69) {
                        translatedSelectText = ko.observable('-- Seleziona --'); // Italian
                    } else if (langId == 36) {
                        translatedSelectText = ko.observable('-- Selecteer --'); // Dutch
                    } else if (langId == 70) {
                        translatedSelectText = ko.observable('-- 選択 --'); // Japanese
                    } else if (langId == 27) {
                        translatedSelectText = ko.observable('-- 选择 --'); // Chinese (Simplified)
                    } else if (langId == 80) {
                        translatedSelectText = ko.observable('-- 선택 --'); // Korean
                    } else if (langId == 7) {
                        translatedSelectText = ko.observable('-- اختر --'); // Arabic
                    } else if (langId == 144) {
                        translatedSelectText = ko.observable('-- Välj --'); // Swedish
                    } else if (langId == 110) {
                        translatedSelectText = ko.observable('-- Wybierz --'); // Polish
                    } else if (langId == 152) {
                        translatedSelectText = ko.observable('-- เลือก --'); // Thai
                    } else if (langId == 61) {
                        translatedSelectText = ko.observable('-- Pilih --'); // Indonesian
                    } else if (langId == 168) {
                        translatedSelectText = ko.observable('-- Chọn --'); // Vietnamese
                    } else if (langId == 34) {
                        translatedSelectText = ko.observable('-- Vælg --'); // Danish
                    } else if (langId == 43) {
                        translatedSelectText = ko.observable('-- Valitse --'); // Finnish
                    } else if (langId == 103) {
                        translatedSelectText = ko.observable('-- Velg --'); // Norwegian
                    } else {
                        // Default to English (langId == 35)
                        translatedSelectText = ko.observable('-- Select --');
                    }


                    $('#imgLogo').attr("src", orgInfo[1]);
                    for (var _data in Pagedata) {
                        if (Pagedata[_data].QuestionId == 850 || Pagedata[_data].QuestionId == 905 || Pagedata[_data].QuestionId == 928 || Pagedata[_data].QuestionId == 948 || Pagedata[_data].QuestionId == 5262) {
                            Pagedata[_data].SpecialOptinLst = [];
                            var specialOptinLst = [];
                            var _sqpqstcount = 0;
                            for (var _ch in Pagedata[_data].ChildQuestionList) {
                                var _indoptlist = [];
                                // _maxgroupingid = Math.max.apply(Math, Pagedata[_data].ChildQuestionList[_ch].OptionList);
                                for (var i = 0; i < Pagedata[_data].ChildQuestionList[_ch].OptionList.length; i++) {
                                    _indoptlist.push(Pagedata[_data].ChildQuestionList[_ch].OptionList[i]);
                                    _indoptlist[i].QuesitonText = Pagedata[_data].ChildQuestionList[_ch].QuestionText;
                                    _indoptlist[i].ParentQuestionId = Pagedata[_data].ChildQuestionList[_ch].ParentQuestionId;
                                }
                                if (_ch == 0) {
                                    _sqpqstcount = _indoptlist.length - 1;
                                }
                                for (var _opt in _indoptlist) {
                                    specialOptinLst.push(_indoptlist[_opt]);
                                }
                            }
                            for (var j = 0; j <= _sqpqstcount; j++) {
                                var _qstopt = [];
                                for (var _op in specialOptinLst) {
                                    if (specialOptinLst[_op].SpecialGroupingId == j + 1) {
                                        _qstopt.push(specialOptinLst[_op]);
                                    }
                                }
                                //for(var _shtlst in _qstopt){
                                // Pagedata[_data].SpecialOptinLst.push.apply();
                                Pagedata[_data].SpecialOptinLst.push(_qstopt);
                                //}

                            }
                        }
                    }
                    ViewModel = new SurveyQuestion(Pagedata);
                    ko.utils.arrayForEach(ViewModel.Questions, function (item) {
                        item = ko.observable(item);
                        item.OptionList = ko.observableArray(item.OptionList);
                        item.ChildQuestionList = ko.observableArray(item.ChildQuestionList);
                        item.ChildQuestionList.OptionList = ko.observableArray(item.ChildQuestionList.OptionList);
                        item.SelectedChildQuestionList == ko.observableArray(item.SelectedChildQuestionList);
                        item.SelectedChildQuestionList.OptionList = ko.observableArray(item.SelectedChildQuestionList.OptionList);
                        ko.utils.arrayForEach(item.OptionList, function (option) {
                            option = ko.observable(option);
                        });
                    });

                    ko.applyBindings(ViewModel, document.getElementById("dvQuestions"));
                    //Binding the save questions options displayed using mapping data for Parent Questions
                    for (var i = 0; i < ViewModel.Questions().length; i++) {
                        var childQuestions = [];
                        var childOptions = [];
                        var subChildQuestions = [];
                        var subChildOptions = [];
                        if (ViewModel.Questions()[i].QuestionTypeId == 3) {
                            for (var p = 0; p < ViewModel.Questions()[i].ChildQuestionList.length; p++) {
                                if (ViewModel.Questions()[i].ChildQuestionList[p].QuestionTypeId != 5) {
                                    childQuestions.push(ViewModel.Questions()[i].ChildQuestionList[p]);
                                    for (var q = 0; q < ViewModel.Questions()[i].ChildQuestionList[p].OptionList.length; q++) {
                                        childOptions.push(ViewModel.Questions()[i].ChildQuestionList[p].OptionList[q]);
                                    }
                                    ViewModel.Questions()[i].SelectedChildQuestionList = childQuestions;
                                    for (var r = 0; r < ViewModel.Questions()[i].SelectedChildQuestionList.length; r++) {
                                        ViewModel.Questions()[i].SelectedChildQuestionList[r].OptionList = []
                                        for (var s = 0; s < childOptions.length; s++) {
                                            if (ViewModel.Questions()[i].SelectedChildQuestionList[r].QuestionId == childOptions[s].QuestionId) {
                                                ViewModel.Questions()[i].SelectedChildQuestionList[r].OptionList.push(childOptions[s]);
                                            }
                                        }

                                    }
                                    ko.applyBindings(ViewModel.Questions()[i], document.getElementById(ViewModel.Questions()[i].QuestionId + "_trChildQuestionId"));

                                }
                                else {
                                    for (var _op in ViewModel.Questions()[i].ChildQuestionList[p].OptionList) {
                                        if (ViewModel.Questions()[i].OptionId != 0 || ViewModel.Questions()[i].OptionId != undefined) {
                                            if (ViewModel.Questions()[i].ChildQuestionList[p].OptionId == ViewModel.Questions()[i].ChildQuestionList[p].OptionList[_op].OptionId) {
                                                if (ViewModel.Questions()[i].ChildQuestionList[p].OptionId == 13095 || ViewModel.Questions()[i].ChildQuestionList[p].OptionId == 13096) {
                                                    showquestions(ViewModel.Questions(), 898);
                                                }
                                                var _matrixopt = ViewModel.Questions()[i].ChildQuestionList[p].QuestionId + '_' + ViewModel.Questions()[i].ChildQuestionList[p].OptionId;
                                                $('#' + _matrixopt + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                                $('#' + _matrixopt + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                                $('#' + ViewModel.Questions()[i].ChildQuestionList[p].OptionId + '_radio').attr("checked", 'checked');
                                                if (_rdAnswers != "") {
                                                    _rdAnswers = _rdAnswers + ';' + _matrixopt + '_' + ViewModel.Questions()[i].ChildQuestionList[p].OptionList[_op].OptionText;
                                                }
                                                else {
                                                    _rdAnswers = _matrixopt + '_' + ViewModel.Questions()[i].ChildQuestionList[p].OptionList[_op].OptionText;
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                            for (var s = 0; s < ViewModel.Questions()[i].OptionList.length; s++) {
                                for (var t = 0; t < ViewModel.Questions()[i].ResponseOptionList.length; t++) {
                                    if (ViewModel.Questions()[i].OptionList[s].OptionId == ViewModel.Questions()[i].ResponseOptionList[t].OptionId) {
                                        ViewModel.Questions()[i].OptionList[s].IsChecked = true;
                                        $('#' + ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionId + '_checkbox').addClass("input_on");
                                        $('#' + ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionId + '_dvmain').addClass("input_onm");
                                        $('#' + ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionId + '_checkbox').attr("checked", 'checked');
                                        $('#' + ViewModel.Questions()[i].OptionList[s].OptionId + '_checkbox').attr("checked", 'checked');
                                        if (_rdAnswers != "") {
                                            _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionText;
                                        }
                                        else {
                                            _rdAnswers = ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionText; + '_' + ViewModel.Questions()[i].OptionList[s].OptionText;
                                        }

                                    }
                                }
                            }

                            for (var x = 0; x < ViewModel.Questions()[i].ChildQuestionList.length; x++) {
                                for (var y = 0; y < ViewModel.Questions()[i].ChildQuestionList[x].OptionList.length; y++) {
                                    for (var z = 0; z < ViewModel.Questions()[i].ChildQuestionList[x].ResponseOptionList.length; z++) {
                                        if (ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId == ViewModel.Questions()[i].ChildQuestionList[x].ResponseOptionList[z].OptionId) {
                                            ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].IsChecked = true;
                                            $('#' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].QuestionId + '_' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId + '_checkbox').addClass("input_on");
                                            $('#' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].QuestionId + '_' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId + '_dvmain').addClass("input_onm");
                                            $('#' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].QuestionId + '_' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId + '_checkbox').attr("checked", 'checked');
                                            $('#' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId + '_checkbox').attr("checked", 'checked');
                                            if (_rdAnswers != "") {
                                                _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].QuestionId + '_' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId + '_' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionText;
                                            }
                                            else {
                                                _rdAnswers = ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[s].OptionId + '_' + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionText;
                                            }


                                        }
                                    }
                                }
                            }
                        }
                        else if (ViewModel.Questions()[i].QuestionTypeId == 13) {
                            for (var x = 0; x < ViewModel.Questions()[i].ChildQuestionList.length; x++) {

                                for (var z = 0; z < ViewModel.Questions()[i].ChildQuestionList[x].ResponseOptionList.length; z++) {
                                    for (var _spq in ViewModel.Questions()[i].SpecialOptinLst) {
                                        for (var _spqopt in ViewModel.Questions()[i].SpecialOptinLst[_spq]) {
                                            if (ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId == ViewModel.Questions()[i].ChildQuestionList[x].ResponseOptionList[z].OptionId) {
                                                if (ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionText == 'None of the above' || ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionText == 'Prefer not to answer') {
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_radio').attr("checked", 'checked');
                                                    ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].SpeicalOptionId = ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId;
                                                    if (_rdAnswers != "") {
                                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionText;
                                                    }
                                                    else {
                                                        _rdAnswers = ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionText;
                                                    }
                                                }
                                                else {
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_checkbox').addClass("input_on");
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_dvmain').addClass("input_onm");
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_checkbox').attr("checked", 'checked');
                                                    $('#' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_checkbox').attr("checked", 'checked');
                                                    ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].IsChecked = true;
                                                    if (_rdAnswers != "") {

                                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionText;
                                                    }
                                                    else {
                                                        _rdAnswers = ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].QuestionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionId + '_' + ViewModel.Questions()[i].SpecialOptinLst[_spq][_spqopt].OptionText;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (ViewModel.Questions()[i].QuestionTypeId == 4 || ViewModel.Questions()[i].QuestionTypeId == 14) {
                            if (ViewModel.Questions()[i].OptionId != 0 || ViewModel.Questions()[i].OptionId != undefined) {
                                for (var _rdopt in ViewModel.Questions()[i].OptionList) {
                                    if (ViewModel.Questions()[i].OptionId == ViewModel.Questions()[i].OptionList[_rdopt].OptionId) {
                                        $('#' + ViewModel.Questions()[i].QuestionId + '_' + ViewModel.Questions()[i].OptionId + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                        $('#' + ViewModel.Questions()[i].QuestionId + '_' + ViewModel.Questions()[i].OptionId + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                        $('#' + ViewModel.Questions()[i].OptionId + '_radio').attr("checked", 'checked');
                                        if (_rdAnswers != "") {
                                            _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[i].OptionList[_rdopt].QuestionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionText;
                                        }
                                        else {
                                            _rdAnswers = ViewModel.Questions()[i].OptionList[_rdopt].QuestionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionText;
                                        }
                                        if (ViewModel.Questions()[i].QuestionId == 927 || ViewModel.Questions()[i].QuestionId == 957) {
                                            for (var _qst in ViewModel.Questions()) {
                                                if (ViewModel.Questions()[i].QuestionId == 927) {
                                                    if (ViewModel.Questions()[_qst].QuestionId == 947) {
                                                        if (ViewModel.Questions()[i].OptionId == 13004) {
                                                            ViewModel.Questions()[_qst].QuestionHide = false;
                                                            $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'inline');
                                                        }
                                                        else {
                                                            ViewModel.Questions()[_qst].QuestionHide = true;
                                                            $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'none');
                                                        }
                                                    }
                                                }
                                                else if (ViewModel.Questions()[i].QuestionId == 957) {
                                                    if (ViewModel.Questions()[_qst].QuestionId == 898) {
                                                        if (parseInt(ViewModel.Questions()[i].OptionId) != 13097) {
                                                            ViewModel.Questions()[_qst].QuestionHide = false;
                                                            $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'inline');
                                                        }
                                                        else {
                                                            ViewModel.Questions()[_qst].QuestionHide = true;
                                                            $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'none');
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        //if question is vehicle question then we are getting child questions from hardcoded questions and options javascript file
                        if (ViewModel.Questions()[i].QuestionId == 175) {
                            _ddlclick = 1
                            qustsandopts = new questionsandoptions();
                            var _childOptions = [];
                            var _subchildqs = [];
                            var _subchildops = [];
                            var _childQuestions = qustsandopts.getquestions(ViewModel.Questions()[i].QuestionId);//based on parent questionId get child questions
                            for (question in _childQuestions) {
                                childQuestions.push(_childQuestions[question]);
                            }
                            for (_qus in childQuestions) {
                                _childOptions = [];
                                ViewModel.Questions()[i].ChildQuestionList.push(childQuestions[_qus]);
                                _childOptions.push(qustsandopts.getoptions(childQuestions[_qus].QuestionId));//get options based on questionId
                                for (options in _childOptions) {
                                    for (op in _childOptions[options]) {
                                        _childOptions[options][op].ListChildQuestionId = [];
                                        ViewModel.Questions()[i].ChildQuestionList[_qus].OptionList.push(_childOptions[options][op]);

                                    }

                                }
                                _subchildqs = [];
                                _subchildops = [];
                                _subchildqs = qustsandopts.getquestions(childQuestions[_qus].QuestionId);
                                for (_subqs in _subchildqs) {
                                    ViewModel.Questions()[i].ChildQuestionList[_qus].ChildQuestionList.push(_subchildqs[_subqs]);
                                    _subchildops.push(qustsandopts.getoptions(_subchildqs[_subqs].QuestionId)); // get child question based on the parent questionId
                                    for (_subops in _subchildops) {
                                        for (_subop in _subchildops[_subops]) {
                                            _subchildops[_subops].ListChildQuestionId = [];
                                            ViewModel.Questions()[i].ChildQuestionList[_qus].ChildQuestionList[_subqs].OptionList.push(_subchildops[_subops][_subop]);
                                            if (ViewModel.Questions()[i].ChildQuestionList[_qus].ChildQuestionList[_subqs].QuestionId == 190
                                                || ViewModel.Questions()[i].ChildQuestionList[_qus].ChildQuestionList[_subqs].QuestionId == 200
                                                || ViewModel.Questions()[i].ChildQuestionList[_qus].ChildQuestionList[_subqs].QuestionId == 208
                                                || ViewModel.Questions()[i].ChildQuestionList[_qus].ChildQuestionList[_subqs].QuestionId == 216) {
                                                ViewModel.Questions()[i].ChildQuestionList[_qus].SubChildOptions.push(_subchildops[_subops][_subop]);
                                            }
                                        }

                                    }
                                }
                            }
                            for (var _chqsres = 0; _chqsres < ViewModel.Questions()[i].ChildQuestionList.length; _chqsres++) {
                                for (var _resp = 0; _resp < ViewModel.Questions()[i].ResponseOptionList.length; _resp++) {
                                    if (ViewModel.Questions()[i].ChildQuestionList[_chqsres].QuestionId == ViewModel.Questions()[i].ResponseOptionList[_resp].QuestionId) {
                                        ViewModel.Questions()[i].ChildQuestionList[_chqsres].OptionId = ViewModel.Questions()[i].ResponseOptionList[_resp].OptionId;
                                    }
                                    else {
                                        for (var _subchildresp = 0; _subchildresp < ViewModel.Questions()[i].ChildQuestionList[_chqsres].ChildQuestionList.length; _subchildresp++) {
                                            if (ViewModel.Questions()[i].ChildQuestionList[_chqsres].ChildQuestionList[_subchildresp].QuestionId == ViewModel.Questions()[i].ResponseOptionList[_resp].QuestionId) {
                                                ViewModel.Questions()[i].ChildQuestionList[_chqsres].ChildQuestionList[_subchildresp].OptionId = ViewModel.Questions()[i].ResponseOptionList[_resp].OptionId;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        for (j = 0; j < ViewModel.Questions()[i].OptionList.length; j++) {
                            if (ViewModel.Questions()[i].OptionId == ViewModel.Questions()[i].OptionList[j].OptionId) {
                                if (ViewModel.Questions()[i].QuestionId == 160) {
                                    if (ViewModel.Questions()[i].OptionId != 3448) {
                                        showquestions(ViewModel.Questions(), 927);
                                    }
                                }
                                childQuestions = [];
                                childOptions = [];
                                for (var l = 0; l < ViewModel.Questions()[i].ChildQuestionList.length; l++) {
                                    var Count = 0;
                                    var QuestionId = 0;
                                    var ChQuestionId = 0;
                                    for (var k = 0; k < ViewModel.Questions()[i].OptionList[j].ListChildQuestionId.length; k++) {
                                        ChQuestionId = ViewModel.Questions()[i].OptionList[j].ListChildQuestionId[k];
                                        QuestionId = ViewModel.Questions()[i].ChildQuestionList[l].QuestionId;
                                        if (QuestionId == ChQuestionId) {
                                            Count = 1;
                                        }
                                    }
                                    if (Count == 0) {
                                        childQuestions.push(ViewModel.Questions()[i].ChildQuestionList[l]);
                                        for (var m = 0; m < ViewModel.Questions()[i].ChildQuestionList[l].OptionList.length; m++) {
                                            if (ViewModel.Questions()[i].ChildQuestionList[l].QuestionId == ViewModel.Questions()[i].ChildQuestionList[l].OptionList[m].QuestionId) {
                                                childOptions.push(ViewModel.Questions()[i].ChildQuestionList[l].OptionList[m]);
                                            }
                                        }
                                    }
                                }

                                ViewModel.Questions()[i].SelectedChildQuestionList = childQuestions;
                                for (var p = 0; p < ViewModel.Questions()[i].SelectedChildQuestionList.length; p++) {
                                    ViewModel.Questions()[i].SelectedChildQuestionList[p].OptionList = [];
                                    for (var q = 0; q < childOptions.length; q++) {
                                        if (ViewModel.Questions()[i].SelectedChildQuestionList[p].QuestionId == childOptions[q].QuestionId) {
                                            ViewModel.Questions()[i].SelectedChildQuestionList[p].OptionList.push(childOptions[q]);
                                        }
                                    }
                                    //                            if (ViewModel.Questions()[i].SelectedChildQuestionList[p].QuestionTypeId == 3) {
                                    //                                for (var s = 0; s < ViewModel.Questions()[i].SelectedChildQuestionList[p].OptionList.length; s++) {
                                    //                                    for (var t = 0; t < ViewModel.Questions()[i].SelectedChildQuestionList[p].ResponseOptionList.length; t++) {
                                    //                                        if (ViewModel.Questions()[i].SelectedChildQuestionList[p].OptionList[s].OptionId == ViewModel.Questions()[i].SelectedChildQuestionList[p].ResponseOptionList[t].OptionId) {
                                    //                                            $("#" + ViewModel.Questions()[i].SelectedChildQuestionList[p].OptionList[s].OptionId + '_checkbox').iCheck('check');
                                    //                                        }
                                    //                                    }
                                    //                                }
                                    //                            }

                                }

                                ko.applyBindings(ViewModel.Questions()[i], document.getElementById(ViewModel.Questions()[i].QuestionId + "_trChildQuestionId"));
                                for (var _chrdopt in ViewModel.Questions()[i].SelectedChildQuestionList) {
                                    if (ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].QuestionTypeId == 4) {
                                        if (ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionId != 0 && ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionId != null) {
                                            for (var _rdopt in ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList) {
                                                if (ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId == ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionId) {
                                                    var _soptid = ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].QuestionId + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId;
                                                    $('#' + _soptid + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                                    $('#' + _soptid + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                                    $('#' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId + '_radio').attr("checked", 'checked');
                                                    if (_rdAnswers != "") {
                                                        _rdAnswers = _rdAnswers + ';' + _soptid + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionText;
                                                    }
                                                    else {
                                                        _rdAnswers = _soptid + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionText;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else if (ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].QuestionTypeId == 3) {
                                        for (var _selopt in ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList) {
                                            var _selchldopt = ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionId;
                                            if (_selchldopt != 0 && _selchldopt != null) {
                                                for (var _selrdopt in ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList) {
                                                    if (ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList[_selrdopt].OptionId == ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionId) {
                                                        var _soptid = ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList[_selrdopt].QuestionId + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList[_selrdopt].OptionId;
                                                        $('#' + _soptid + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                                        $('#' + _soptid + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                                        $('#' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList[_selrdopt].OptionId + '_radio').attr("checked", 'checked');
                                                        if (_rdAnswers != "") {
                                                            _rdAnswers = _rdAnswers + ';' + _soptid + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList[_selrdopt].OptionText;
                                                        }
                                                        else {
                                                            _rdAnswers = _soptid + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].ChildQuestionList[_selopt].OptionList[_selrdopt].OptionText;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        //Bind the save data for child Questions and options

                        for (var k = 0; k < ViewModel.Questions()[i].ChildQuestionList.length; k++) {
                            subChildQuestions = [];
                            subChildOptions = [];
                            //if (ViewModel.Questions()[i].ChildQuestionList[k].QuestionId != 1418) {
                            for (var l = 0; l < ViewModel.Questions()[i].ChildQuestionList[k].OptionList.length; l++) {
                                if (ViewModel.Questions()[i].ChildQuestionList[k].OptionId == ViewModel.Questions()[i].ChildQuestionList[k].OptionList[l].OptionId) {
                                    for (var m = 0; m < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList.length; m++) {
                                        subChildQuestions.push(ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m])
                                        for (var p = 0; p < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList.length; p++) {
                                            if (ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].QuestionId == ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList[p].QuestionId) {
                                                subChildOptions.push(ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList[p]);
                                            }
                                        }
                                    }


                                    if (ViewModel.Questions()[i].ChildQuestionList[k].QuestionId == 1418 && ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList.length == 0) {
                                        ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList = subChildQuestions;
                                    }
                                    else {
                                        ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList = subChildQuestions;
                                        for (var q = 0; q < ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList.length; q++) {
                                            ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[q].OptionList = [];
                                            for (var r = 0; r < subChildOptions.length; r++) {
                                                if (ViewModel.Questions()[i].ChildQuestionList[k].OptionId == subChildOptions[r].ParentOptionId) {

                                                    ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[q].OptionList.push(subChildOptions[r]);
                                                }
                                            }
                                        }
                                    }
                                    ko.applyBindings(ViewModel.Questions()[i].ChildQuestionList[k], document.getElementById(ViewModel.Questions()[i].ChildQuestionList[k].QuestionId + "_trChildQuestionId"));
                                }
                            }
                            //}
                            //else {
                            //   ko.applyBindings(ViewModel.Questions()[i].ChildQuestionList[k], document.getElementById(ViewModel.Questions()[i].ChildQuestionList[k].QuestionId + "_trChildQuestionId"));
                            //}
                        }

                    }
                    if (ViewModel.Questions()[9] != null) {
                        if (ViewModel.Questions()[9].QuestionId == 5110) {
                            if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList.length == 1) {
                                if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList[0].OptionId == 133157) {
                                    chkClick(ViewModel.Questions()[9].ChildQuestionList[0], ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0], 0)
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionText;
                                    }
                                    else {
                                        _rdAnswers = ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionText;
                                    }
                                    ko.applyBindings(ViewModel.Questions()[9].ChildQuestionList[0], document.getElementById(ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].QuestionId + "_trChildQuestionId"));
                                }
                                else if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList[0].OptionId == 133156) {
                                    chkClick(ViewModel.Questions()[9].ChildQuestionList[0], ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1], 0)
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionText;
                                    }
                                    else {
                                        _rdAnswers = ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionText;
                                    }
                                    ko.applyBindings(ViewModel.Questions()[9].ChildQuestionList[0], document.getElementById(ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].QuestionId + "_trChildQuestionId"));

                                }
                                else if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList[0].OptionId == 137221) {
                                    chkClick(ViewModel.Questions()[9].ChildQuestionList[0], ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2], 0)
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionText;
                                    }
                                    else {
                                        _rdAnswers = ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionText;
                                    }
                                    ko.applyBindings(ViewModel.Questions()[9].ChildQuestionList[0], document.getElementById(ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].QuestionId + "_trChildQuestionId"));

                                }
                            }
                            if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList.length > 1) {
                                if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList[0].OptionId == 133157) {
                                    chkClick(ViewModel.Questions()[9].ChildQuestionList[0], ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0], 0)
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionText;
                                    }
                                    else {
                                        _rdAnswers = ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].OptionText;
                                    }
                                    ko.applyBindings(ViewModel.Questions()[9].ChildQuestionList[0], document.getElementById(ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].QuestionId + "_trChildQuestionId"));
                                }
                                if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList[1].OptionId == 133156) {
                                    chkClick(ViewModel.Questions()[9].ChildQuestionList[0], ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1], 0)
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionText;
                                    }
                                    else {
                                        _rdAnswers = ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].OptionText;
                                    }
                                    ko.applyBindings(ViewModel.Questions()[9].ChildQuestionList[0], document.getElementById(ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].QuestionId + "_trChildQuestionId"));

                                }
                                if (ViewModel.Questions()[9].ChildQuestionList[0].ResponseOptionList[0].OptionId == 137221) {
                                    chkClick(ViewModel.Questions()[9].ChildQuestionList[0], ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2], 0)
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionText;
                                    }
                                    else {
                                        _rdAnswers = ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].QuestionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionId + '_' + ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].OptionText;
                                    }
                                    ko.applyBindings(ViewModel.Questions()[9].ChildQuestionList[0], document.getElementById(ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].QuestionId + "_trChildQuestionId"));

                                }
                            }
                        }
                    }

                    _ddlclick = 0;
                    $('#dvImageLoading').hide();
                    $('#buttonSaveandCancel').show();
                    $('#gdprtermsandconditions').show();
                }

            }

        });
    }
    function showquestions(Questions, questionid) {
        for (var _qst in Questions) {
            if (Questions[_qst].QuestionId == questionid) {
                Questions[_qst].QuestionHide = false;
                $('#' + Questions[_qst].QuestionId + '_Show').css('display', 'inline');
            }
        }
    }
    function insertvechilequestion(question, childQuestions, childOptions) {
        question.ChildQuestionList = childQuestions;
        for (var p = 0; p < question.ChildQuestionList.length; p++) {
            question.ChildQuestionList[p].OptionList = [];
            for (var q = 0; q < childOptions.length; q++) {
                if (question.ChildQuestionList[p].QuestionId == childOptions[q].QuestionId) {
                    question.ChildQuestionList[p].OptionList.push(childOptions[q]);
                }
            }

        }
    }


    var SurveyQuestion = function (questions) {
        var Self = this;
        var IsChaildMatrix = false;
        Self.Questions = new ko.observableArray(questions);
        //selection change event select parent question child Questions and options
        //        $(document).on('change', Self.selectionChanged, function(event) {

        rdClick = function (parent, event, i, questiontype) {
            var _id = i.target.id;
            var _dupAnswers = '';
            var _dvId = _id.split('_')[0] + '_' + _id.split('_')[1];
            var _rowOptionID = _id.split('_')[0];
            var _optId = _id.split('_')[1];
            //Based on Option selection show questions
            if (event.QuestionId == 927 || event.QuestionId == 957) {
                for (var _qst in ViewModel.Questions()) {
                    if (event.QuestionId == 927) {
                        if (ViewModel.Questions()[_qst].QuestionId == 947) {
                            if (event.OptionId == 13004) {
                                ViewModel.Questions()[_qst].QuestionHide = false;
                                $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'inline');
                            }
                            else {
                                ViewModel.Questions()[_qst].QuestionHide = true;
                                $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'none');
                            }
                        }
                    }
                    else if (event.QuestionId == 957) {
                        if (ViewModel.Questions()[_qst].QuestionId == 898) {
                            if (parseInt(event.OptionId) != 13097) {
                                ViewModel.Questions()[_qst].QuestionHide = false;
                                $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'inline');
                            }
                            else {
                                ViewModel.Questions()[_qst].QuestionHide = true;
                                $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'none');
                            }
                        }
                    }
                }

            }

            if (_rdAnswers == '') {
                _rdAnswers = _dvId + '_' + event.OptionText;
                $('#' + _dvId + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                $('#' + _dvId + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                $('#' + _optId + '_radio').attr("checked", 'checked');
                if (questiontype == 'specialqst') {
                    event.SpeicalOptionId = event.OptionId;         /*  if div click then checked  radio button  */
                }
                else {
                    parent.OptionId = _optId
                }
            }
            else {
                if (questiontype != 'specialqst') {
                    var _ansArray = _rdAnswers.split(';');
                    for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 2 ----------------------------------------*/

                        if (_ansArray[i].split('_')[0] == _rowOptionID) {  /* Remove duplicate answer when already exists same Question answer  when array */

                            $('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_radio').removeClass("input_on");   /* Remove  already applying css class if web click */

                            jQuery('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */

                            _ansArray.splice(i, 1);                                    /* Remove object from answers list */
                        }
                        /* when array item is removed then array size will reduce */
                        if (i < _ansArray.length) {
                            if (i == 0) {
                                _dupAnswers = _ansArray[i];
                            }
                            else {
                                _dupAnswers = _dupAnswers + ';' + _ansArray[i];
                            }
                        }
                    }
                }
                else {
                    _dupAnswers = _rdAnswers;
                }

                /*------------------------------------------------------------------------------- End For loop 2 ----------------------------------------*/

                if (_dupAnswers != "") {
                    _dupAnswers = _dupAnswers + ';' + _dvId + '_' + event.OptionText;
                }
                else {
                    _dupAnswers = _dvId + '_' + event.OptionText;
                }

                $('#' + _optId + '_radio').attr("checked", 'checked');
                if (questiontype == 'specialqst') {
                    event.SpeicalOptionId = event.OptionId;         /*  if div click then checked  radio button  */
                }
                else {
                    parent.OptionId = _optId
                }

                $('#' + _dvId + '_radio').addClass("input_on");
                $('#' + _dvId + '_dvmain').addClass("input_onm");
                _rdAnswers = _dupAnswers;

            }
            if (parent.ChildQuestionList != undefined) {
                if (parent.ChildQuestionList.length > 0) {
                    var qstlst = Clone(event);
                    qstlst.OptionList = Clone(parent.OptionList)
                    qstlst.ChildQuestionList = Clone(parent.ChildQuestionList);
                    selectChildQuestions(qstlst, parent.QuestionTypeId);
                }
            }

        }
        chkClick = function (parent, event, i) {
            var _dupAnswers = '';
            if (i != 0) {
                var _id = i.target.id;
            }
            else {
                if (event.OptionId == 133157) {
                    var _id = "5111_133157";
                }
                else if (event.OptionId == 133156) {
                    var _id = "5111_133156";
                }
                else {
                    var _id = "5111_137221";
                }
            }

            var _dvId = _id.split('_')[0] + '_' + _id.split('_')[1];
            var _rowOptionID = _id.split('_')[0];
            var _optionid = _id.split('_')[1];
            var _isUncheck = false;
            //            parent.OptionId = 0;
            if (_rdAnswers == '') {
                _rdAnswers = _dvId + '_' + event.OptionText;
                $('#' + _dvId + '_checkbox').addClass("input_on");   /*  if Web click then add class to radio div id */
                $('#' + _dvId + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                $('#' + _dvId + '_checkbox').attr("checked", 'checked');         /*  if div click then checked  radio button  */
                $('#' + _optionid + '_checkbox').attr("checked", 'checked');
                event.IsChecked = true;
            }
            else {

                var _ansArray = _rdAnswers.split(';');

                for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 2 ----------------------------------------*/

                    /* Multi select with none question if readio button click then remove all checkboxes check vise versa */
                    if ((_ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1]) == _dvId) {  /* Remove duplicate answer when already exists same Question answer  when array */
                        _isUncheck = true
                        removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid)
                        _ansArray.splice(i, 1); /* Remove object from answers list */
                        event.IsChecked = false;
                        i = i + _ansArray.length;
                    }
                        //Added new logic for 557 question. memeber select none option remove previously checked all option to that question.
                    else if (_ansArray[i].split('_')[0] == 557 && _rowOptionID == 557) {
                        if (_optionid == 9675 && parseInt(_ansArray[i].split('_')[1]) != 9675) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 9675 && parseInt(_ansArray[i].split('_')[1]) == 9675) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }

                    else if (_ansArray[i].split('_')[0] == 5130 && _rowOptionID == 5130) {
                        if (_optionid == 133856 && parseInt(_ansArray[i].split('_')[1]) != 133856) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 133856 && parseInt(_ansArray[i].split('_')[1]) == 133856) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 153 && _rowOptionID == 153) {
                        if (_optionid == 3150 && parseInt(_ansArray[i].split('_')[1]) != 3150) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 3150 && parseInt(_ansArray[i].split('_')[1]) == 3150) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 317 && _rowOptionID == 317) {
                        if (_optionid == 9898 && parseInt(_ansArray[i].split('_')[1]) != 9898) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 9898 && parseInt(_ansArray[i].split('_')[1]) == 9898) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 284 && _rowOptionID == 284) {
                        if (_optionid == 9899 && parseInt(_ansArray[i].split('_')[1]) != 9899) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 9899 && parseInt(_ansArray[i].split('_')[1]) == 9899) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 295 && _rowOptionID == 295) {
                        if (_optionid == 9634 && parseInt(_ansArray[i].split('_')[1]) != 9634) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 9634 && parseInt(_ansArray[i].split('_')[1]) == 9634) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 647 && _rowOptionID == 647) {
                        if (_optionid == 10646 && parseInt(_ansArray[i].split('_')[1]) != 10646) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 10646 && parseInt(_ansArray[i].split('_')[1]) == 10646) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 308 && _rowOptionID == 308) {
                        if (_optionid == 8503 && parseInt(_ansArray[i].split('_')[1]) != 8503) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 8503 && parseInt(_ansArray[i].split('_')[1]) == 8503) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5100 && _rowOptionID == 5100) {
                        if (_optionid == 133204 && parseInt(_ansArray[i].split('_')[1]) != 133204) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else if (_optionid != 133204 && parseInt(_ansArray[i].split('_')[1]) == 133204) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                _ansArray.splice(i, 1);
                            }
                        }
                        else if (_optionid == 133205 && parseInt(_ansArray[i].split('_')[1]) != 133205) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 133205 && parseInt(_ansArray[i].split('_')[1]) == 133205) {
                                if (event.IsChecked == false) {
                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5023 && _rowOptionID == 5023) {
                        if (_optionid == 137203 && parseInt(_ansArray[i].split('_')[1]) != 137203) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137203 && parseInt(_ansArray[i].split('_')[1]) == 137203) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5024 && _rowOptionID == 5024) {
                        if (_optionid == 137204 && parseInt(_ansArray[i].split('_')[1]) != 137204) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137204 && parseInt(_ansArray[i].split('_')[1]) == 137204) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5026 && _rowOptionID == 5026) {
                        if (_optionid == 137205 && parseInt(_ansArray[i].split('_')[1]) != 137205) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137205 && parseInt(_ansArray[i].split('_')[1]) == 137205) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5027 && _rowOptionID == 5027) {
                        if (_optionid == 137206 && parseInt(_ansArray[i].split('_')[1]) != 137206) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137206 && parseInt(_ansArray[i].split('_')[1]) == 137206) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5028 && _rowOptionID == 5028) {
                        if (_optionid == 137207 && parseInt(_ansArray[i].split('_')[1]) != 137207) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137207 && parseInt(_ansArray[i].split('_')[1]) == 137207) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5029 && _rowOptionID == 5029) {
                        if (_optionid == 137208 && parseInt(_ansArray[i].split('_')[1]) != 137208) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137208 && parseInt(_ansArray[i].split('_')[1]) == 137208) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5030 && _rowOptionID == 5030) {
                        if (_optionid == 137209 && parseInt(_ansArray[i].split('_')[1]) != 137209) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137209 && parseInt(_ansArray[i].split('_')[1]) == 137209) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5031 && _rowOptionID == 5031) {
                        if (_optionid == 137210 && parseInt(_ansArray[i].split('_')[1]) != 137210) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137210 && parseInt(_ansArray[i].split('_')[1]) == 137210) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5034 && _rowOptionID == 5034) {
                        if (_optionid == 137211 && parseInt(_ansArray[i].split('_')[1]) != 137211) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137211 && parseInt(_ansArray[i].split('_')[1]) == 137211) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5072 && _rowOptionID == 5072) {
                        if (_optionid == 137212 && parseInt(_ansArray[i].split('_')[1]) != 137212) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137212 && parseInt(_ansArray[i].split('_')[1]) == 137212) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5073 && _rowOptionID == 5073) {
                        if (_optionid == 137213 && parseInt(_ansArray[i].split('_')[1]) != 137213) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137213 && parseInt(_ansArray[i].split('_')[1]) == 137213) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5074 && _rowOptionID == 5074) {
                        if (_optionid == 137214 && parseInt(_ansArray[i].split('_')[1]) != 137214) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137214 && parseInt(_ansArray[i].split('_')[1]) == 137214) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5092 && _rowOptionID == 5092) {
                        if (_optionid == 137215 && parseInt(_ansArray[i].split('_')[1]) != 137215) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137215 && parseInt(_ansArray[i].split('_')[1]) == 137215) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5181 && _rowOptionID == 5181) {
                        if (_optionid == 137216 && parseInt(_ansArray[i].split('_')[1]) != 137216) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137216 && parseInt(_ansArray[i].split('_')[1]) == 137216) {
                                if (event.IsChecked == false) {
                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5101 && _rowOptionID == 5101) {
                        if (_optionid == 137217 && parseInt(_ansArray[i].split('_')[1]) != 137217) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137217 && parseInt(_ansArray[i].split('_')[1]) == 137217) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5103 && _rowOptionID == 5103) {
                        if (_optionid == 137218 && parseInt(_ansArray[i].split('_')[1]) != 137218) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137218 && parseInt(_ansArray[i].split('_')[1]) == 137218) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5106 && _rowOptionID == 5106) {
                        if (_optionid == 137219 && parseInt(_ansArray[i].split('_')[1]) != 137219) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137219 && parseInt(_ansArray[i].split('_')[1]) == 137219) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5109 && _rowOptionID == 5109) {
                        if (_optionid == 137220 && parseInt(_ansArray[i].split('_')[1]) != 137220) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137220 && parseInt(_ansArray[i].split('_')[1]) == 137220) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 5111 && _rowOptionID == 5111) {
                        if (_optionid == 137221 && parseInt(_ansArray[i].split('_')[1]) != 137221) {
                            event.IsChecked = false;
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137221 && parseInt(_ansArray[i].split('_')[1]) == 137221) {
                                event.IsChecked = false;
                                if (event.IsChecked == false) {
                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }
                    else if (_ansArray[i].split('_')[0] == 198 && _rowOptionID == 198) {
                        if (_optionid == 137222 && parseInt(_ansArray[i].split('_')[1]) != 137222) {
                            if (event.IsChecked == false) {
                                removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox');
                                _ansArray.splice(i, 1);                                   /* Remove object from answers list */
                                if (i != _ansArray.length) {
                                    i = i - 1;
                                }
                            }
                        }
                        else {
                            if (_optionid != 137222 && parseInt(_ansArray[i].split('_')[1]) == 137222) {
                                if (event.IsChecked == false) {

                                    removeAllOptionNoneSelect(_ansArray[i], _dvId, _optionid, 'checkBox')
                                    _ansArray.splice(i, 1);
                                }
                            }
                        }
                    }

                } /*------------------------------------------------------------------------------- End For loop 2 ----------------------------------------*/

                for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 3 ----------------------------------------*/

                    if (i == 0) {
                        _dupAnswers = _ansArray[i];
                    }
                    else {
                        _dupAnswers = _dupAnswers + ';' + _ansArray[i];
                    }

                }
                if (_isUncheck == false) {

                    if (_dupAnswers == '') {
                        _dupAnswers = _dvId + '_' + event.OptionText;
                    }
                    else {
                        _dupAnswers = _dupAnswers + ';' + _dvId + '_' + event.OptionText;
                    }
                    $('#' + _dvId + '_checkbox').addClass("input_on");
                    $('#' + _dvId + '_dvmain').addClass("input_onm");
                    $('#' + _dvId + '_checkbox').attr("checked", 'checked');
                    $('#' + _optionid + '_checkbox').attr("checked", 'checked');
                    event.IsChecked = true;
                }

                _rdAnswers = _dupAnswers;
            }

            //            alert(jQuery('#' + _dvId).is(":checked"));

        }
        chkwithradioclick = function (parent, data, event) { //special question with chaeckbox with radio button
            //  var _surveyquestion = new SurveyQuestion();
            var _ansArray = [];
            var _duplicateAns = '';
            if (data.OptionText.toLowerCase() == 'none of the above' || data.OptionText.toLowerCase() == 'prefer not to answer') { //if selected option is radio button type
                rdClick(parent, data, event, 'specialqst'); //  special question insert extra param
                _ansArray = _rdAnswers.split(';'); // previous save answers split
                for (var _qs in ViewModel.Questions()) {
                    if (ViewModel.Questions()[_qs].QuestionId == data.ParentQuestionId) {
                        for (var _sp in ViewModel.Questions()[_qs].SpecialOptinLst) {
                            for (var _sop in ViewModel.Questions()[_qs].SpecialOptinLst[_sp]) {
                                if (ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].QuestionId == data.QuestionId && ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].OptionId != data.OptionId) { //check all question optons except current clicked options
                                    var _qstoptid = ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].QuestionId + '_' + ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].OptionId;
                                    if (ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].OptionText.toLowerCase() == 'none of the above' ||
                                        ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].OptionText.toLowerCase() == 'prefer not to answer') {
                                        for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 2 ----------------------------------------*/
                                            if (_ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] == _qstoptid) {  /* Remove duplicate answer when already exists same Question answer  when array */
                                                ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].SpeicalOptionId = 0;
                                                $('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_radio').removeClass("input_on");   /* Remove  already applying css class if web click */

                                                jQuery('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */

                                                _ansArray.splice(i, 1);                                    /* Remove object from answers list */
                                            }

                                        } /*------------------------------------------------------------------------------- End For loop 2 ----------------------------------------*/


                                    }
                                    else {
                                        if (ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].IsChecked == true) { //if  radio button checked remove current question prevoius selected options
                                            ViewModel.Questions()[_qs].SpecialOptinLst[_sp][_sop].IsChecked = false;
                                            //var _qstoptid = ViewModel.Questions()[_qs].ChildQuestionList[_ch].OptionList[_op].QuestionId + '_' + ViewModel.Questions()[_qs].ChildQuestionList[_ch].OptionList[_op].OptionId;
                                            for (var j = 0; j < _ansArray.length; j++) {
                                                if ((_ansArray[j].split('_')[0] + '_' + _ansArray[j].split('_')[1]) == _qstoptid) {
                                                    $('#' + _ansArray[j].split('_')[0] + '_' + _ansArray[j].split('_')[1] + '_checkbox').removeClass("input_on");   /* Remove  already applying css class if web click */
                                                    $('#' + _ansArray[j].split('_')[0] + '_' + _ansArray[j].split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */
                                                    _ansArray.splice(j, 1);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 3 ----------------------------------------*/

                            if (i == 0) {
                                if (_ansArray[i] != "") {
                                    _duplicateAns = _ansArray[i];
                                }
                            }
                            else {
                                _duplicateAns = _duplicateAns + ';' + _ansArray[i];
                            }

                        }
                    }
                }

            }
            else {
                chkClick(parent, data, event); ///if selected option is checbox  type
                _ansArray = _rdAnswers.split(';'); // spilt all selected options
                for (var _qs in ViewModel.Questions()) {
                    if (ViewModel.Questions()[_qs].QuestionId == data.ParentQuestionId) {
                        for (var _spq in ViewModel.Questions()[_qs].SpecialOptinLst) {
                            for (var _sop in ViewModel.Questions()[_qs].SpecialOptinLst[_spq]) {
                                if (ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].QuestionId == data.QuestionId && ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].OptionId != data.OptionId) { //check all question optons except current clicked options
                                    if (ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].OptionText.toLowerCase() == 'none of the above' ||
                                        ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].OptionText.toLowerCase() == 'prefer not to answer') {
                                        if (ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].SpeicalOptionId != 0) { //if option type checkbox remove current question previous selected radio button options
                                            var _qstoptid = ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].QuestionId + '_' + ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].OptionId;
                                            for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 2 ----------------------------------------*/
                                                if (_ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] == _qstoptid) {  /* Remove duplicate answer when already exists same Question answer  when array */
                                                    ViewModel.Questions()[_qs].SpecialOptinLst[_spq][_sop].SpeicalOptionId = 0;
                                                    $('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_radio').removeClass("input_on");   /* Remove  already applying css class if web click */

                                                    jQuery('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */

                                                    _ansArray.splice(i, 1);                                    /* Remove object from answers list */
                                                }

                                            } /*------------------------------------------------------------------------------- End For loop 2 ----------------------------------------*/

                                        }
                                    }
                                }
                            }
                        }
                        for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 3 ----------------------------------------*/
                            if (i == 0) {
                                if (_ansArray[i] != "") {
                                    _duplicateAns = _ansArray[i];
                                }
                            }
                            else {
                                _duplicateAns = _duplicateAns + ';' + _ansArray[i];
                            }

                        }

                    }
                }
            }
            if (_duplicateAns != '') {
                _rdAnswers = _duplicateAns;
            }
        }
        RemoveCheck = function (event) {
            if (event.IsChecked == true) {
                _eventOptId = event.OptionId;
                if (event.QuestionId == 328 || event.QuestionId == 329 || event.QuestionId == 330 || event.QuestionId == 5263 || event.QuestionId == 5264 || event.QuestionId == 5265) {
                    for (var i = 0; i < ViewModel.Questions().length; i++) {
                        if (event.QuestionId == 328 || event.QuestionId == 5265) {
                            if (ViewModel.Questions()[i].QuestionId == event.QuestionId) {
                                if (_eventOptId == 11668 || _eventOptId == 12128 || _eventOptId == 135579 || _eventOptId == 135580) {
                                    for (var j = 0; j < ViewModel.Questions()[i].OptionList.length; j++) {

                                        if (ViewModel.Questions()[i].OptionList[j].IsChecked == true) {
                                            ViewModel.Questions()[i].OptionList[j].IsChecked = false;

                                            $('#' + ViewModel.Questions()[i].OptionList[j].OptionId + '_checkbox').iCheck('uncheck');



                                        }
                                        if (ViewModel.Questions()[i].OptionList[j].OptionId == _eventOptId) {
                                            ViewModel.Questions()[i].OptionList[j].IsChecked = true;
                                        }

                                    }
                                }
                                else {
                                    for (var k = 0; k < ViewModel.Questions()[i].OptionList.length; k++) {

                                        if (ViewModel.Questions()[i].OptionList[k].OptionId == 11668 || ViewModel.Questions()[i].OptionList[k].OptionId == 12128 ||
                                            ViewModel.Questions()[i].OptionList[k].OptionId == 135579 || ViewModel.Questions()[i].OptionList[k].OptionId == 135580) {
                                            if (ViewModel.Questions()[i].OptionList[k].IsChecked == true) {
                                                ViewModel.Questions()[i].OptionList[k].IsChecked = false;

                                                $('#' + ViewModel.Questions()[i].OptionList[k].OptionId + '_checkbox').iCheck('uncheck');

                                            }
                                        }

                                        // $('#' + ViewModel.Questions()[i].OptionList[k].OptionId + '_checkbox').prop("checked", false);


                                    }
                                }
                            }

                        }
                        else {
                            for (var m = 0; m < ViewModel.Questions()[i].SelectedChildQuestionList.length; m++) {
                                if (ViewModel.Questions()[i].SelectedChildQuestionList[m].QuestionId == event.QuestionId) {
                                    if (event.OptionText.toLowerCase() == 'prefer not to answer' || event.OptionText.toLowerCase() == 'none of the above') {
                                        for (var j = 0; j < ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList.length; j++) {

                                            if (ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[j].IsChecked == true) {
                                                ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[j].IsChecked = false;

                                                $('#' + ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[j].OptionId + '_checkbox').iCheck('uncheck');


                                            }
                                            if (ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[j].OptionId == _eventOptId) {
                                                ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[j].IsChecked = true;
                                            }

                                        }
                                    }
                                    else {
                                        for (var k = 0; k < ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList.length; k++) {

                                            if (ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[k].OptionText.toLowerCase() == 'none of the above' || ViewModel.Questions()[i].ChildQuestionList[m].OptionList[k].OptionText.toLowerCase() == 'prefer not to answer') {
                                                if (ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[k].IsChecked == true) {
                                                    ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[k].IsChecked = false;

                                                    $('#' + ViewModel.Questions()[i].SelectedChildQuestionList[m].OptionList[k].OptionId + '_checkbox').iCheck('uncheck');

                                                }
                                            }

                                            // $('#' + ViewModel.Questions()[i].OptionList[k].OptionId + '_checkbox').prop("checked", false);


                                        }
                                    }
                                }

                            }
                        }
                    }
                }


            }
            return true;
        }
        selectionChanged = function (event) {
            $('#dvImageLoading').show();
            if (_ddlclick == 0) {
                if (event.OptionId != null && event.OptionId != '' && event.OptionId != undefined) {
                    if (event.QuestionId == 160) {
                        for (var _qst in ViewModel.Questions()) {
                            if (ViewModel.Questions()[_qst].QuestionId == 927) {
                                if (parseInt(event.OptionId) != 3448) {
                                    ViewModel.Questions()[_qst].QuestionHide = false;
                                    $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'inline');
                                }
                                else {
                                    ViewModel.Questions()[_qst].QuestionHide = true;
                                    $('#' + ViewModel.Questions()[_qst].QuestionId + '_Show').css('display', 'none');
                                }
                            }
                        }
                    }
                    $("#" + event.QuestionId + '_trChildQuestionId').css('display', 'inline-block');
                    selectChildQuestions(event, event.QuestionTypeId)
                }
                    //here click select option display data none
                else {
                    $("#" + event.QuestionId + '_trChildQuestionId').css('display', 'none');
                }
            }

            $('#dvImageLoading').hide();

        }
        //click function check all validations
        Cancel = function () {
            if (cid == 11) {//added by Giri for Fusioncash to redirect their urls on survey completion
                window.location.href = 'http://www.fusioncash.net/fcsurveys.php';
            }
            else {
                window.close();
            }
        }

        Save = function () {
            $('#dvImageLoading').show();
            var isValid = false;
            var _mySelf = [];
            var _otherhoushld = [];
            var _housldminor = [];
            var gdprCheckbox = $('#gdprterms').is(":checked");
            if (pid == 'cb963824-983a-41fa-a2ac-508bd5f85b95') {
                for (var i = 0; i < Self.Questions().length; i++) {
                    if (Self.Questions()[i].QuestionId == 573 || Self.Questions()[i].QuestionId == 580) {
                        if ($("input[name=" + Self.Questions()[i].QuestionId + "_rd" + "]:checked").val()) {
                            isValid = true;
                        }
                        if (Self.Questions()[i].OptionId != null && Self.Questions()[i].OptionId != undefined && Self.Questions()[i].OptionId != 0) {
                            isValid = true;
                        }
                    }
                }
            }
            if (pid == '3dc7c62c-9112-47c7-be37-229f7657976b') {
                for (var i = 0; i < Self.Questions().length; i++) {
                    if (Self.Questions()[i].QuestionId == 850 || Self.Questions()[i].QuestionId == 5099) {
                        if ($("input[name=" + Self.Questions()[i].QuestionId + "_rd" + "]:checked").val()) {
                            isValid = true;
                        }
                        for (var j = 0; j < Self.Questions()[i].ChildQuestionList.length; j++) {
                            for (var k = 0; k < Self.Questions()[i].ChildQuestionList[j].OptionList.length; k++) {
                                if (Self.Questions()[i].ChildQuestionList[j].OptionList[k].IsChecked) {
                                    isValid = true;
                                }
                            }
                        }
                    }
                }
            }
            //if (pid == 'cb963824-983a-41fa-a2ac-508bd5f85b95' || pid == '3dc7c62c-9112-47c7-be37-229f7657976b') {

            //    //parent dropdown validations
            //    if (Self.Questions()[i].QuestionTypeId == 2) {
            //        if (Self.Questions()[i].OptionId == null || Self.Questions()[i].OptionId == undefined) {
            //            isValid = false;
            //            $("#" + Self.Questions()[i].QuestionId + "_spn").show();
            //            $("#" + Self.Questions()[i].QuestionId + "_spn").focus();
            //            $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');

            //        }
            //        else {
            //            $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //            //child question dropdown validations
            //            for (var j = 0; j < Self.Questions()[i].SelectedChildQuestionList.length; j++) {
            //                if (Self.Questions()[i].SelectedChildQuestionList.length > 0) {
            //                    if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId == 3) {
            //                        if (Self.Questions()[i].SelectedChildQuestionList[j].OptionId == null || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == 0 || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == undefined) {
            //                            $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
            //                            for (var k = 0; k < Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList.length; k++) {
            //                                var count = 0;
            //                                //Validtaions for subchildquestion matrix type
            //                                if (Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionTypeId == 5) {
            //                                    $('.' + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + '_selectrdb').each(function (i, list) {
            //                                        if ($(this).is(':checked')) {
            //                                            count = 1;
            //                                            return false;
            //                                        }
            //                                    });
            //                                    if (count == 0) {
            //                                        $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").show();
            //                                        $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").text('Please answer the question.');
            //                                        isValid = false;
            //                                    }
            //                                    else {
            //                                        $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").hide();
            //                                    }
            //                                }
            //                            }
            //                        }

            //                    }

            //                        //validations for selected questionlist
            //                    else if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId != 3) {
            //                        if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId != 4) {
            //                            if (Self.Questions()[i].SelectedChildQuestionList[j].OptionId == null || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == 0 || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == undefined) {
            //                                isValid = false;
            //                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").show();
            //                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").text('Please answer the question.');
            //                            }
            //                            else {

            //                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
            //                            }

            //                        }
            //                        else if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId == 4) {
            //                            if (!$("input[name=" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_rd" + "]:checked").val()) {
            //                                isValid = false;
            //                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").show();
            //                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").text('Please answer the question.');
            //                            }
            //                            else {

            //                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
            //                            }

            //                        }
            //                    }


            //                }
            //            }

            //        }
            //    }

            //    if (Self.Questions()[i].QuestionTypeId == 4 || Self.Questions()[i].QuestionTypeId == 14) {
            //        //radio button validations
            //        if (Self.Questions()[i].QuestionHide == false) { // if question in hide so skip the validation for particular question
            //            if (!$("input[name=" + Self.Questions()[i].QuestionId + "_rd" + "]:checked").val()) {
            //                isValid = false;
            //                $("#" + Self.Questions()[i].QuestionId + "_spn").show();
            //                $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');
            //            }
            //            else {
            //                $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //            }
            //        }
            //    }

            //    if (Self.Questions()[i].QuestionTypeId == 3) {
            //        //checkbox validation
            //        if (Self.Questions()[i].ChildQuestionList.length == 0) {
            //            if (Self.Questions()[i].QuestionId == 319 || Self.Questions()[i].QuestionId == 322 || Self.Questions()[i].QuestionId == 325 || Self.Questions()[i].QuestionId == 328 || Self.Questions()[i].QuestionId == 5265) {
            //                //isValid = true;
            //                $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //            }
            //            else {
            //                if (!$("input[name=" + Self.Questions()[i].QuestionId + "_checkbox" + "]:checked").val()) {
            //                    isValid = false;
            //                    $("#" + Self.Questions()[i].QuestionId + "_spn").show();
            //                    $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');

            //                }
            //                else {
            //                    $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //                }
            //            }

            //        }
            //        else {
            //            if (Self.Questions()[i].SelectedChildQuestionList.length > 0) {
            //                if (Self.Questions()[i].QuestionTypeId == 3) {
            //                    if (Self.Questions()[i].QuestionId == 319 || Self.Questions()[i].QuestionId == 322 || Self.Questions()[i].QuestionId == 325 || Self.Questions()[i].QuestionId == 328 || Self.Questions()[i].QuestionId == 5265) {
            //                        //isValid = true;
            //                        $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //                    }
            //                    else {
            //                        if (!$("input[name=" + Self.Questions()[i].QuestionId + "_checkbox" + "]:checked").val()) {
            //                            isValid = false;
            //                            $("#" + Self.Questions()[i].QuestionId + "_spn").show();
            //                            $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');

            //                        }
            //                        else {
            //                            $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //                        }
            //                    }
            //                }

            //            }
            //            for (var x = 0; x < Self.Questions()[i].SelectedChildQuestionList.length; x++) {
            //                if (Self.Questions()[i].SelectedChildQuestionList[x].QuestionTypeId == 3) {
            //                    if (Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 320 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 321 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 323 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 324 ||
            //                     Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 326 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 327 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 329 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 330 ||
            //                        Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 5263 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 5264) {
            //                        //isValid = true;
            //                        $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //                    }
            //                    else {
            //                        if (!$("input[name=" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_checkbox" + "]:checked").val()) {
            //                            isValid = false;
            //                            $("#" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_spn").show();
            //                            $("#" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_spn").text('Please answer the question.');
            //                        }
            //                        else {
            //                            $("#" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_spn").hide();
            //                        }
            //                    }

            //                }
            //            }
            //        }
            //    }
            //    if (Self.Questions()[i].ChildQuestionList.length != 0) {
            //        if (Self.Questions()[i].QuestionHide == false) {
            //            //matrix question validations
            //            for (var k = 0; k < Self.Questions()[i].ChildQuestionList.length; k++) {
            //                //                        $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
            //                var count = 0;
            //                if (Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 5) {
            //                    if (Self.Questions()[i].QuestionId != 5131 && Self.Questions()[i].QuestionId != 5156) {
            //                        $('.' + Self.Questions()[i].ChildQuestionList[k].QuestionId + '_selectrdb').each(function (i, list) {
            //                            if ($(this).is(':checked')) {
            //                                count = 1;
            //                                return false;
            //                            }

            //                        });

            //                        if (count == 0) {
            //                            $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").show();
            //                            $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").text('Please answer the question.');
            //                            isValid = false;
            //                        }
            //                        else {
            //                            $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").hide();
            //                        }
            //                    }

            //                }
            //                    //validation for Subchild question dropdown type
            //                else if (Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 2 || Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 4) {
            //                    for (var t = 0; t < Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList.length; t++) {
            //                        if (Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].OptionId == null || Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].OptionId == undefined) {
            //                            isValid = false;
            //                            $("#" + Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].QuestionId + "_spn").show();
            //                            $("#" + Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].QuestionId + "_spn").text('Please answer the question.');
            //                        }
            //                        else {
            //                            $("#" + Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].QuestionId + "_spn").hide();
            //                        }
            //                    }
            //                }

            //            }
            //        }
            //    }
            //    if (ViewModel.Questions()[9] != null) {
            //        if (ViewModel.Questions()[9].QuestionId == 5110 && ViewModel.Questions()[9].OptionId == 133154) {
            //            if (Self.Questions()[9].ChildQuestionList[0].OptionList[0].IsChecked == false && Self.Questions()[9].ChildQuestionList[0].OptionList[1].IsChecked == false && Self.Questions()[9].ChildQuestionList[0].OptionList[2].IsChecked == false) {
            //                isValid = false;
            //            }

            //        }
            //    }
            //}
            if (isValid == true && gdprCheckbox == false && (pid == 'cb963824-983a-41fa-a2ac-508bd5f85b95' || pid == '3dc7c62c-9112-47c7-be37-229f7657976b')) {
                $("#gdprErrorMsg").show();
                $('#dvImageLoading').hide();
            }
            else {
                //saving data
                // if (isValid == true) {
                var xml = bindXml(ViewModel)
                $("#spnErrorMessage").hide();
                $("#gdprErrorMsg").hide();
                $.ajax({
                    dataType: "text",
                    type: 'POST',
                    data: { xml: xml },
                    url: '/Mem/ProfileSave?ug=' + ug + '&cid=' + cid + '&pfId=' + pid,
                    headers: {
                        '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
                    },
                    success: function (pagedata) {
                        $('#dvImageLoading').hide();
                        pagedata = pagedata.replace('"', '');
                        var isPopUp = pagedata.split(';')[1];
                        isPopUp = isPopUp.replace('"', '');
                        if (cid == 11) {//added by Giri for Fusioncash to redirect their urls on survey completion
                            window.location.href = 'http://www.fusioncash.net/fcsurveys.php';
                        }
                        if (isPopUp.toLowerCase() == "true" || cid != -1) {
                            refreshParent();
                        }
                        else {
                            window.close();
                        }
                    }
                });
                //}
                //else {
                //    $('#dvImageLoading').hide();
                //    $("#spnErrorMessage").show();
                //    $("#spnErrorMessage").text('Please fill out all required fields');

                //}
            }
        }
    }
    //refresh parent windoe
    function refreshParent() {
        window.close();
        if (window.opener != null && !window.opener.closed) {
            window.opener.location.reload();
        }
    }

    function removeAllOptionNoneSelect(_ansArray, _dvId, _optionid, _optionType) {
        if ($('#' + _dvId).attr('type') != 'radio') {
            $('#' + _ansArray.split('_')[0] + '_' + _ansArray.split('_')[1] + '_checkbox').removeClass("input_on");   /* Remove  already applying css class if web click */
            $('#' + _ansArray.split('_')[0] + '_' + _ansArray.split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */
            $('#' + _dvId + '_checkbox').attr("checked", false);
            $('#' + _optionid + '_checkbox').attr("checked", false);
        }
        if (_optionType == 'checkBox') {
            for (var _qst in ViewModel.Questions()) {
                if (ViewModel.Questions()[_qst].QuestionId == parseInt(_ansArray.split('_')[0])) {
                    for (var _opt in ViewModel.Questions()[_qst].OptionList) {
                        if (ViewModel.Questions()[_qst].OptionList[_opt].OptionId == parseInt(_ansArray.split('_')[1])) {
                            ViewModel.Questions()[_qst].OptionList[_opt].IsChecked = false;
                        }
                    }
                }
            }
            //if (ViewModel.Questions()[9].QuestionId == 5110) {
            //    if (ViewModel.Questions()[9].ChildQuestionList[0].QuestionId == 5111) {
            //        if (parseInt(_ansArray.split('_')[1]) == 133157) {
            //            ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].IsChecked = false;
            //        }
            //        if (parseInt(_ansArray.split('_')[1]) == 133156) {
            //            ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].IsChecked = false;
            //        }
            //        if (parseInt(_ansArray.split('_')[1]) == 137221) {
            //            ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].IsChecked = false;
            //        }
            //    }
            //}

        }
    }
    function Clone(obj) {
        if (null == obj || "object" != typeof obj) return obj;
        var copy = obj.constructor();
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
        }
        return copy;
    }

    function selectChildQuestions(event, qid) {
        var OptionId = event.OptionId;
        var childQuestions = [];
        var childOptions = [];
        $("#" + event.QuestionId + "_spn").hide();
        for (var i = 0; i < event.OptionList.length; i++) {
            if (event.OptionList[i].OptionId == OptionId) {
                for (var l = 0; l < event.ChildQuestionList.length; l++) {
                    var Count = 0;
                    var QuestionId = 0;
                    var ChQuestionId = 0;
                    for (var k = 0; k < event.OptionList[i].ListChildQuestionId.length; k++) {
                        ChQuestionId = event.OptionList[i].ListChildQuestionId[k];
                        QuestionId = event.ChildQuestionList[l].QuestionId;
                        if (QuestionId == ChQuestionId) {
                            Count = 1;
                        }
                    }
                    //this is for mapped data
                    if (Count == 0) {
                        childQuestions.push(event.ChildQuestionList[l]);
                        for (var m = 0; m < event.ChildQuestionList[l].OptionList.length; m++) {
                            if (event.ChildQuestionList[l].QuestionId == event.ChildQuestionList[l].OptionList[m].QuestionId) {
                                childOptions.push(event.ChildQuestionList[l].OptionList[m]);
                            }
                        }
                    }

                }
            }
        }
        //Bind the Parentquestions selected child questionlist
        for (var n = 0; n < ViewModel.Questions().length; n++) {
            if (ViewModel.Questions()[n].QuestionId == event.QuestionId) {
                ViewModel.Questions()[n].SelectedChildQuestionList = childQuestions;
                for (var p = 0; p < ViewModel.Questions()[n].SelectedChildQuestionList.length; p++) {
                    ViewModel.Questions()[n].SelectedChildQuestionList[p].OptionList = [];
                    for (var q = 0; q < childOptions.length; q++) {
                        if (ViewModel.Questions()[n].SelectedChildQuestionList[p].QuestionId == childOptions[q].QuestionId) {
                            ViewModel.Questions()[n].SelectedChildQuestionList[p].OptionList.push(childOptions[q]);
                        }
                    }

                }
                ko.applyBindings(ViewModel.Questions()[n], document.getElementById(event.QuestionId + "_trChildQuestionId"));
                for (var _chrdopt in ViewModel.Questions()[n].SelectedChildQuestionList) {
                    if (ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].QuestionTypeId == 4) {
                        if (ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionId != 0 && ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionId != null) {
                            for (var _rdopt in ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList) {
                                if (ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId == ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionId) {
                                    var _soptid = ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].QuestionId + '_' + ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId;
                                    $('#' + _soptid + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                    $('#' + _soptid + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                    $('#' + ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId + '_radio').attr("checked", 'checked');
                                    if (_rdAnswers != "") {
                                        _rdAnswers = _rdAnswers + ';' + _soptid + '_' + ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionText;
                                    }
                                    else {
                                        _rdAnswers = _soptid + '_' + ViewModel.Questions()[n].OptionList[_rdopt].OptionText;
                                    }
                                }
                            }
                        }
                    }
                }

            }
                //Bind the childquestion selected subchildquestionlist
            else {
                for (var k = 0; k < ViewModel.Questions()[n].ChildQuestionList.length; k++) {
                    if (ViewModel.Questions()[n].ChildQuestionList[k].QuestionId == event.QuestionId) {
                        ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList = childQuestions;
                        if (ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList != 0) {
                            for (var p = 0; p < ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList.length; p++) {
                                ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList = [];
                                //added 02/16/2017. append options to radio button child questions list.
                                if (qid == 4) {
                                    for (var t = 0; t < childOptions.length; t++) {
                                        if (ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].QuestionId == childOptions[t].QuestionId) {
                                            ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList.push(childOptions[t]);
                                        }
                                    }
                                }
                                else {
                                    // dropdown questions.
                                    for (var q = 0; q < event.SubChildOptions.length; q++) {
                                        if (event.OptionId == event.SubChildOptions[q].ParentOptionId) {
                                            ViewModel.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList.push(event.SubChildOptions[q]);
                                        }
                                    }
                                }

                            }
                            // $("#" + Self.Questions()[n].SelectedChildQuestionList[k].QuestionId + "_trChildQuestionId").html('');
                            ko.cleanNode(document.getElementById(ViewModel.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId"));
                            $('#' + ViewModel.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId").html('');
                            ko.applyBindings(ViewModel.Questions()[n].ChildQuestionList[k], $("#" + ViewModel.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId").get(0));

                        }
                        else {
                            //added 02/16/2017. newly added radio button questions not having questions 
                            if (qid == 4) {
                                // $("#" + Self.Questions()[n].SelectedChildQuestionList[k].QuestionId + "_trChildQuestionId").html('');
                                ko.cleanNode(document.getElementById(ViewModel.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId"));
                                $('#' + ViewModel.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId").html('');
                                ko.applyBindings(ViewModel.Questions()[n].ChildQuestionList[k], $("#" + ViewModel.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId").get(0));

                            }
                        }
                    }
                }

            }
        }
    }
    var bindXml = function (ViewModel) {
        var count = 0;
        var answerXML = '';
        answerXML += "<profiles>";
        for (var m = 0; m < ViewModel.Questions().length; m++) {
            // Checkbox Question type - More than one option may select
            if (ViewModel.Questions()[m].QuestionTypeId == 3) {
                count = 0;
                for (var n = 0; n < ViewModel.Questions()[m].OptionList.length; n++) {
                    if (ViewModel.Questions()[m].OptionList[n].IsChecked) {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                        answerXML += "<question_id>" + ViewModel.Questions()[m].OptionList[n].QuestionId + "</question_id>";
                        answerXML += "<option_id>" + ViewModel.Questions()[m].OptionList[n].OptionId + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
                        if (ViewModel.Questions()[m].SelectedChildQuestionList.length > 0) {
                            if (count == 0) {
                                count = 1;
                                for (var p = 0; p < ViewModel.Questions()[m].SelectedChildQuestionList.length; p++) {
                                    for (var q = 0; q < ViewModel.Questions()[m].SelectedChildQuestionList[p].OptionList.length; q++) {
                                        if (ViewModel.Questions()[m].SelectedChildQuestionList[p].OptionList[q].IsChecked) {

                                            answerXML += "<profile>";
                                            answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                            answerXML += "<question_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[p].OptionList[q].QuestionId + "</question_id>";
                                            answerXML += "<option_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[p].OptionList[q].OptionId + "</option_id>";
                                            //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                                            answerXML += "</profile>";
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

            }
            else if (ViewModel.Questions()[m].QuestionTypeId == 13) {
                for (var _opt in ViewModel.Questions()[m].SpecialOptinLst) {
                    for (var _mdop in ViewModel.Questions()[m].SpecialOptinLst[_opt]) {
                        if (ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].OptionText.toLowerCase() == 'none of the above' ||
                               ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].OptionText.toLowerCase() == 'prefer not to answer') {
                            if (ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId != 0 && ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId != "" &&
                                ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId != undefined) {
                                answerXML += "<profile>";
                                answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                answerXML += "<question_id>" + ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].QuestionId + "</question_id>";
                                answerXML += "<option_id>" + ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].SpeicalOptionId + "</option_id>";
                                //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                                answerXML += "</profile>";
                            }

                        }
                        else {
                            if (ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].IsChecked) {
                                answerXML += "<profile>";
                                answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                answerXML += "<question_id>" + ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].QuestionId + "</question_id>";
                                answerXML += "<option_id>" + ViewModel.Questions()[m].SpecialOptinLst[_opt][_mdop].OptionId + "</option_id>";
                                //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                                answerXML += "</profile>";
                            }
                        }
                    }
                }

            }

                // Radio Question type with other
            else {
                if (ViewModel.Questions()[m].QuestionHide == false) {
                    if (ViewModel.Questions()[m].OptionId != 0 && ViewModel.Questions()[m].OptionId != undefined) {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                        answerXML += "<question_id>" + ViewModel.Questions()[m].QuestionId + "</question_id>";
                        answerXML += "<option_id>" + ViewModel.Questions()[m].OptionId + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
                    }

                    if (ViewModel.Questions()[m].SelectedChildQuestionList.length > 0) {
                        for (var n = 0; n < ViewModel.Questions()[m].SelectedChildQuestionList.length; n++) {
                            if (ViewModel.Questions()[m].SelectedChildQuestionList[n].OptionId != 0 &&
                                ViewModel.Questions()[m].SelectedChildQuestionList[n].OptionId != undefined) {
                                answerXML += "<profile>";
                                answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                answerXML += "<question_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].QuestionId + "</question_id>";
                                answerXML += "<option_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].OptionId + "</option_id>";
                                //answerXML += "<option_text><![CDATA[" + objSelectedChildOptions.OptionText + "]]></option_text>";
                                answerXML += "</profile>";
                                for (var p = 0; p < ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList.length; p++) {
                                    if (ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionTypeId == 5) {
                                        if (ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId != 0 &&
                                            ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId != undefined) {
                                            answerXML += "<profile>";
                                            answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                            answerXML += "<question_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionId + "</question_id>";
                                            answerXML += "<option_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId + "</option_id>";
                                            //answerXML += "<option_text><![CDATA[" + objSubChildOptions.OptionText + "]]></option_text>";
                                            answerXML += "</profile>";
                                        }
                                    }
                                }
                            }
                            else {
                                for (var p = 0; p < ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList.length; p++) {
                                    if (ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionTypeId == 5) {
                                        if (ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId != 0 &&
                                            ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId != undefined) {
                                            answerXML += "<profile>";
                                            answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                            answerXML += "<question_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionId + "</question_id>";
                                            answerXML += "<option_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId + "</option_id>";
                                            //answerXML += "<option_text><![CDATA[" + objSubChildOptions.OptionText + "]]></option_text>";
                                            answerXML += "</profile>";
                                        }
                                    }
                                }
                            }

                        }
                    }

                    //DropdownQuestionType             

                }
            }
            //matrix questions
            if (ViewModel.Questions()[m].ChildQuestionList.length != 0) {
                if (ViewModel.Questions()[m].QuestionHide == false) {
                    for (var p = 0; p < ViewModel.Questions()[m].ChildQuestionList.length; p++) {
                        if (ViewModel.Questions()[m].ChildQuestionList[p].QuestionTypeId == 5) {
                            if (ViewModel.Questions()[m].ChildQuestionList[p].OptionId != 0 && ViewModel.Questions()[m].ChildQuestionList[p].OptionId != undefined) {
                                answerXML += "<profile>";
                                answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                answerXML += "<question_id>" + ViewModel.Questions()[m].ChildQuestionList[p].QuestionId + "</question_id>";
                                answerXML += "<option_id>" + ViewModel.Questions()[m].ChildQuestionList[p].OptionId + "</option_id>";
                                //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                                answerXML += "</profile>";
                            }

                        }

                        if (ViewModel.Questions()[m].ChildQuestionList[p].QuestionTypeId == 2 || ViewModel.Questions()[m].ChildQuestionList[p].QuestionTypeId == 4) {
                            for (var q = 0; q < ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList.length; q++) {
                                if (ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList[q].OptionId != 0 &&
                                    ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList[q].OptionId != undefined) {
                                    answerXML += "<profile>";
                                    answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                    answerXML += "<question_id>" + ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList[q].QuestionId + "</question_id>";
                                    answerXML += "<option_id>" + ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList[q].OptionId + "</option_id>";
                                    //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                                    answerXML += "</profile>";
                                }
                            }
                        }
                    }
                }
            }

        }
        if (ViewModel.Questions()[9] != null) {
            if (ViewModel.Questions()[9].QuestionId == 5110 && ViewModel.Questions()[9].OptionId == 133154) {
                if (ViewModel.Questions()[9].ChildQuestionList[0].QuestionId == 5111) {
                    if (ViewModel.Questions()[9].ChildQuestionList[0].OptionList[0].IsChecked == true) {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[9].UserId + "</user_id>";
                        answerXML += "<question_id>" + 5111 + "</question_id>";
                        answerXML += "<option_id>" + 133157 + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
                    }
                }
                if (ViewModel.Questions()[9].ChildQuestionList[0].QuestionId == 5111) {
                    if (ViewModel.Questions()[9].ChildQuestionList[0].OptionList[1].IsChecked == true) {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[9].UserId + "</user_id>";
                        answerXML += "<question_id>" + 5111 + "</question_id>";
                        answerXML += "<option_id>" + 133156 + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
                    }
                }
                if (ViewModel.Questions()[9].ChildQuestionList[0].QuestionId == 5111) {
                    if (ViewModel.Questions()[9].ChildQuestionList[0].OptionList[2].IsChecked == true) {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[9].UserId + "</user_id>";
                        answerXML += "<question_id>" + 5111 + "</question_id>";
                        answerXML += "<option_id>" + 137221 + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
                    }
                }
            }
        }
        answerXML += "</profiles>";
        return answerXML;
    }

    function specialquestions(spqoptions, Question) {
        var _ischecked = true
        var _isspqst = false;
        var _chkrdb = 0;
        var _qstid = "";
        for (var _ms in spqoptions) {
            if (spqoptions[_ms].OptionText == 'None of the above' ||
                 spqoptions[_ms].OptionText == 'Prefer not to answer') {
                if (_isspqst == false) {
                    if (spqoptions[_ms].SpeicalOptionId != 0 && spqoptions[_ms].SpeicalOptionId != null) {
                        _chkrdb = 1;
                        _qstid += ';';
                    }
                    else {
                        if (_ms == spqoptions.length - 1) {
                            if (_qstid != "") {
                                _qstid += ';' + spqoptions[_ms].QuestionId;
                            }
                            else {
                                _qstid += spqoptions[_ms].QuestionId;
                            }
                        }
                        else {
                            _qstid += spqoptions[_ms].QuestionId;
                        }

                    }
                }
                if (_ms == spqoptions.length - 1) {
                    var _spqids = _qstid.split(';');
                    if (_chkrdb == 0) {
                        //var _spqids = _qstid.split(';');
                        for (var _qid in _spqids) {
                            if (_spqids[_qid] != "") {
                                $("#" + _spqids[_qid] + "_spn").show();
                                $("#" + _spqids[_qid] + "_spn").text('Please answer the question.');
                                _ischecked = false;
                            }
                        }
                    }
                    else {
                        for (var _qid in _spqids) {
                            if (_spqids[_qid] != "") {
                                $("#" + _spqids[_qid] + "_spn").hide();
                                // _ischecked = true;
                            }
                        }
                    }
                }
            }
            else {
                if (_isspqst == false) {
                    if (spqoptions[_ms].IsChecked) {
                        _isspqst = true;
                    }
                }
            }


        }
        return _ischecked;
    }
    //declare init function

    that.init = function () {
        $('#dvImageLoading').show();
        $('#buttonSaveandCancel').hide();
        $('#gdprtermsandconditions').hide();
        bindQuestions();
    };

    $(document).ready(function () {

        that.init();
    });
}();


