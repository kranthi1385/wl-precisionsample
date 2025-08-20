//declare Name of the namespace
var LoadBasicQtns = function () {
    var that = {};
    Viewmodel = ko.observableArray();
    //Decalre variable here
    var save = '';
    var UserId = '';
    var rg = '';
    var ProfileId = '';
    var UserInvitationGuid = '';
    var UserGuid = '';
    var s = '';
    var subchildOptionsList = []
    var subcount = 1;
    var OSName = '';
    var OrgTypeId = '';
    var _ddlclick = 0;
    var _rdAnswers = '';
    // real answer api varables 
    var _rq1 = '';
    var _rq2 = '';
    var _rq3 = '';
    var _rq4 = '';
    var _realapiflag = 0;
    var orgInfo = [];
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
    //
    //Get query parmas value user guig
    ug = getUrlVars()["ug"];
    if (ug == undefined) {
        ug = '';
    }
    uig = getUrlVars()["uig"];
    if (uig == undefined) {
        uig = '';
    }
    usid = getUrlVars()["usid"];
    if (usid == undefined) {
        usid = '';
    }
    pid = getUrlVars()["pid"];
    if (pid == undefined) {
        pid = '';
    }
    s = getUrlVars()["s"]
    if (s == undefined) {
        s = '';
    }
    usg = getUrlVars()["usg"] //user status guid added 06/13/2016
    if (usg == undefined) {
        usg = '';
    }

    source = getUrlVars()["source"]
    if (source == undefined) {
        source = '';
    }


    // Get query params from login.aspx
    lpage = getUrlVars()["lpage"];
    if (lpage == undefined) {
        lpage = '';
    }
    confirm = getUrlVars()["confirm"];
    if (confirm == undefined) {
        confirm = '';
    }
    var cid = getUrlVars()["cid"];
    if (cid == undefined) {
        cid = '';
    }
    //Binding the questions
    var bindQuestionData = function (Pagedata) {
        if (ug == uig) {
            redirectUrls();
        }
        else {
            if(Pagedata == null){
                $.ajax({
                    url: '/prs/GetProfilePrescreener?uig=' + uig + '&ug=' + ug + '&cid=' + cid + '&pid=' + pid,
                    type: 'GET',
                    headers: {
                        '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
                    },
                    error: function error(XMLHttpRequest, textstatus, errorThrown) {
                        //  Global.Error(XMLHttpRequest, textstatus, errorThrown);
                    },
                    success: function (lstQuestions) {
                        bindQuestions(lstQuestions);
                    }
                });
            }else{
                bindQuestions(Pagedata);
            }
        }
        return false;
    }

    var bindQuestions = function (Pagedata){
        // if (Pagedata.length != 0 && (source != 'p' && source != 't')) { //remvoed this logic 06/13/2016. show top10 page daily ones after endpage.
        if (Pagedata.length != 0) {
            orgInfo = Pagedata[0].OrgInfo.split(';');//get the org information
            langId = Pagedata[0].LanguageID; // language id 
            countryID = Pagedata[0].CountryID; // country id
            questionId = Pagedata[0].QuestionId; // question id
            orgID = cid;
            if (Pagedata[0].IsFraud == false) {
                if (Pagedata[0].QuestionId > 0) { // if member answered all questions then redirect to next page
                    $('#dvMainContent').show();
                    $('#imgLogo').attr("src", orgInfo[0] + orgInfo[1]);
                    for (var _data in Pagedata) {
                        if (Pagedata[_data].QuestionId == 850 || Pagedata[_data].QuestionId == 905 || Pagedata[_data].QuestionId == 928 || Pagedata[_data].QuestionId == 948) {
                            var specialOptinLst = [];
                            Pagedata[_data].SpecialOptinLst = [];
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
                        if (Pagedata[_data].QuestionId == 1022) { //remove real api question and show static auestion
                            _realapiflag = 1;
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
                    if (_realapiflag == 1) {
                        $('#dvRealApiQuestion').show();
                        $('#dvGoogleCaptcha').hide();
                        $('#tmpTable').hide();
                    }
                    else {
                        $('#dvRealApiQuestion').hide();
                        $('#dvGoogleCaptcha').hide();
                        $('#tmpTable').show();
                    }
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
                        else if (ViewModel.Questions()[i].QuestionTypeId == 4) {
                            if (ViewModel.Questions()[i].OptionId != 0) {
                                for (var _rdopt in ViewModel.Questions()[i].OptionList) {
                                    if (ViewModel.Questions()[i].OptionId == ViewModel.Questions()[i].OptionList[_rdopt].OptionId) {
                                        $('#' + ViewModel.Questions()[i].QuestionId + '_' + ViewModel.Questions()[i].OptionId + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                        $('#' + ViewModel.Questions()[i].QuestionId + '_' + ViewModel.Questions()[i].OptionId + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                        $('#' + ViewModel.Questions()[i].OptionId + '_radio').attr("checked", 'checked');
                                        if (_rdAnswers != "") {
                                            _rdAnswers = _rdAnswers + ';' + ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionText;
                                        }
                                        else {
                                            _rdAnswers = ViewModel.Questions()[i].OptionList[s].QuestionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionId + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionText;
                                        }
                                    }
                                }
                            }
                        }
                        //if question is vehicle question then we are getting child questions from hardcoded questions and options javascript file
                        if (ViewModel.Questions()[i].QuestionId == 175) {
                            _ddlclick = 1
                            if (langId == 114282) {
                                qustsandopts = new questionsandoptions();
                            }
                            if (langId == 114283) {
                                qustsandopts = new frquestionsandoptions();
                            }
                            if (langId == 114284) {
                                qustsandopts = new esquestionsandoptions();
                            }
                            if (langId == 114285) {
                                qustsandopts = new dequestionsandoptions();
                            }
                            if (langId == 114286) {
                                qustsandopts = new ptquestionsandoptions();
                            }
                            if (langId == 114287) {
                                qustsandopts = new ruquestionsandoptions();
                            }
                            if (langId == 114288) {
                                qustsandopts = new itquestionsandoptions();
                            }
                            if (langId == 114289) {
                                qustsandopts = new nlquestionsandoptions();
                            }
                            if (langId == 114290) {
                                qustsandopts = new jpquestionsandoptions();
                            }
                            if (langId == 114291) {
                                qustsandopts = new chquestionsandoptions();
                            }
                            if (langId == 114292) {
                                qustsandopts = new koquestionsandoptions();
                            }
                            if (langId == 131616 || langId == 131617 || langId == 136675 || langId == 137223 || langId == 137224 || langId == 137225) {
                                qustsandopts = new questionsandoptions();
                            }
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
                                                if (ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt] == ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionId) {
                                                    var _soptid = ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].QuestionId + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId;
                                                    $('#' + _soptid + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                                                    $('#' + _soptid + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                                                    $('#' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionId + '_radio').attr("checked", 'checked');
                                                    if (_rdAnswers != "") {
                                                        _rdAnswers = _rdAnswers + ';' + _soptid + '_' + ViewModel.Questions()[i].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt].OptionText;
                                                    }
                                                    else {
                                                        _rdAnswers = _soptid + '_' + ViewModel.Questions()[i].OptionList[_rdopt].OptionText;
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
                            for (var l = 0; l < ViewModel.Questions()[i].ChildQuestionList[k].OptionList.length; l++) {
                                if (ViewModel.Questions()[i].ChildQuestionList[k].OptionId == ViewModel.Questions()[i].ChildQuestionList[k].OptionList[l].OptionId) {
                                    for (var r = 0; r < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList.length; r++) {
                                        for (var m = 0; m < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList.length; m++) {
                                            subChildQuestions.push(ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m])
                                            for (var p = 0; p < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList.length; p++) {
                                                if (ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].QuestionId == ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList[p].QuestionId) {
                                                    subChildOptions.push(ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList[p]);
                                                }
                                            }
                                        }

                                        ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList = subChildQuestions;
                                        for (var q = 0; q < ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList.length; q++) {
                                            ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[q].OptionList = [];
                                            for (var r = 0; r < subChildOptions.length; r++) {
                                                if (ViewModel.Questions()[i].ChildQuestionList[k].OptionId == subChildOptions[r].ParentOptionId) {

                                                    ViewModel.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[q].OptionList.push(subChildOptions[r]);
                                                }
                                            }
                                        }
                                        ko.applyBindings(ViewModel.Questions()[i].ChildQuestionList[k], document.getElementById(ViewModel.Questions()[i].ChildQuestionList[k].QuestionId + "_trChildQuestionId"));

                                    }
                                }
                            }
                        }

                    }
                    _ddlclick = 0;
                }
                else {
                    //redirect urls
                    if (Pagedata[0].IsShowCCPA == false && cid != 450 && cid != 541 && cid != 542 && cid != 162 && cid != 668 && cid != 645 && cid != 680 && cid != 707 && cid != 681 && cid != 501) {
                        window.location.href = "/prs/ccpa?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&cc=" + langId + "&conid=" + countryID;
                    }
                    else {
                        redirectUrls();
                    }
                }
            }
            else {
                window.location.href = "http://e.opinionetwork.com/e/psr?usg=EF75E92A-2A50-477F-9342-E6E597469B4A&uig=" + Pagedata[0].UserInvitationGuid + '&ug=' + ug + '&pid=' + pid + '&cid=' + cid; //redirect to endpage
            }

        }

        $('#dvImageLoading').hide();
        $('#buttonSaveandCancel').show();
        $('#FooterShowHide').show();
    }
    function showquestions(Questions, questionid) {
        for (var _qst in Questions) {
            if (Questions[_qst].QuestionId == questionid) {
                Questions[_qst].QuestionHide = false;
                $('#' + Questions[_qst].QuestionId + '_Show').css('display', 'inline');
            }
        }
    }
    var SurveyQuestion = function (questions) {
        var Self = this;
        var IsChaildMatrix = false;
        Self.Questions = new ko.observableArray(questions);
        UserId = Self.Questions()[0].UserId;
        //selection change event select parent question child Questions and options
        //        $(document).on('change', self.selectionChanged, function(event) {
        //------------------------------------------------------ Remove icheck properity for checkbox type questions user select none of the above options ---------------------
        self.rdClick = function (parent, event, i, questiontype) {
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
            if (parent.ChildQuestionList.length > 0) {
                var qstlst = Clone(event);
                qstlst.OptionList = Clone(parent.OptionList)
                qstlst.ChildQuestionList = Clone(parent.ChildQuestionList);
                selectChildQuestions(qstlst, parent.QuestionTypeId);
            }


        }
        self.chkClick = function (parent, event, i) {
            var _id = i.target.id;
            var _dupAnswers = '';
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
                        if ($('#' + _dvId).attr('type') != 'radio') {
                            $('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_checkbox').removeClass("input_on");   /* Remove  already applying css class if web click */

                            $('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */
                            $('#' + _dvId + '_checkbox').attr("checked", false);
                            $('#' + _optionid + '_checkbox').attr("checked", false);
                            event.IsChecked = false;
                            _ansArray.splice(i, 1);                                   /* Remove object from answers list */


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
        self.chkwithradioclick = function (parent, data, event) { //special question with chaeckbox with radio button
            //  var _surveyquestion = new SurveyQuestion();
            var _ansArray = [];
            var _duplicateAns = '';
            if (data.OptionText.toLowerCase() == 'none of the above' || data.OptionText.toLowerCase() == 'prefer not to answer') { //if selected option is radio button type
                self.rdClick(parent, data, event, 'specialqst'); //  special question insert extra param
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
                self.chkClick(parent, data, event); ///if selected option is checbox  type
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
        self.RemoveCheck = function (event) {
            if (event.IsChecked == true) {
                _eventOptId = event.OptionId;
                if (event.QuestionId == 328 || event.QuestionId == 329 || event.QuestionId == 330) {
                    for (var i = 0; i < ViewModel.Questions().length; i++) {
                        if (event.QuestionId == 328) {
                            if (ViewModel.Questions()[i].QuestionId == event.QuestionId) {
                                if (_eventOptId == 11668 || _eventOptId == 12128) {
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

                                        if (ViewModel.Questions()[i].OptionList[k].OptionId == 11668 || ViewModel.Questions()[i].OptionList[k].OptionId == 12128) {
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

        Self.Save = function () {
            //$("#btnSubmit").hide();
            $("#spnMessage").show();
            $('#dvImageLoading').show();

            myVar = setTimeout(SaveOptions(), 100);
            $('#dvImageLoading').hide();
            function Validation() {
                var _isvalid = true;
                if (_realapiflag == 1) {
                    var _realAnsText = document.getElementById('responseField').value;
                    if (_realAnsText == '' || _realAnsText == "") {
                        _isvalid = false;
                        $("#spnRealApiQstErrMsg").show();
                        $("#spnRealApiQstErrMsg").text('Please answer the question.');
                    }
                    else {
                        _isvalid = true;
                        $("#spnRealApiQstErrMsg").hide();
                    }
                }
                else {

                    //for all questions validation.
                    var _mySelf = [];
                    var _otherhoushld = [];
                    var _housldminor = [];
                    for (var i = 0; i < Self.Questions().length; i++) {
                        //parent dropdown validations
                        if (Self.Questions()[i].QuestionTypeId == 2) {
                            if (Self.Questions()[i].OptionId == null || Self.Questions()[i].OptionId == undefined) {
                                _isvalid = false;
                                $("#" + Self.Questions()[i].QuestionId + "_spn").show();
                                $("#" + Self.Questions()[i].QuestionId + "_spn").focus();
                                $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');

                            }
                            else {
                                $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                //child question dropdown validations
                                for (var j = 0; j < Self.Questions()[i].SelectedChildQuestionList.length; j++) {
                                    if (Self.Questions()[i].SelectedChildQuestionList.length > 0) {
                                        if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId == 3) {
                                            if (Self.Questions()[i].SelectedChildQuestionList[j].OptionId == null || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == 0 || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == undefined) {
                                                $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
                                                for (var k = 0; k < Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList.length; k++) {
                                                    var count = 0;
                                                    //Validtaions for subchildquestion matrix type
                                                    if (Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionTypeId == 5) {
                                                        $('.' + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + '_selectrdb').each(function (i, list) {
                                                            if ($(this).is(':checked')) {
                                                                count = 1;
                                                                return false;
                                                            }
                                                        });
                                                        if (count == 0) {
                                                            $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").show();
                                                            $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").text('Please answer the question.');
                                                            _isvalid = false;
                                                        }
                                                        else {
                                                            $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").hide();
                                                        }
                                                    }
                                                }
                                            }

                                        }

                                            //validations for selected questionlist
                                        else if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId != 3) {
                                            if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId != 4) {
                                                if (Self.Questions()[i].SelectedChildQuestionList[j].OptionId == null || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == 0 || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == undefined) {
                                                    _isvalid = false;
                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").show();
                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").text('Please answer the question.');
                                                }
                                                else {

                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
                                                }

                                            }
                                            else if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId == 4) {
                                                if (!$("input[name=" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_rd" + "]:checked").val()) {
                                                    _isvalid = false;
                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").show();
                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").text('Please answer the question.');
                                                }
                                                else {

                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
                                                }

                                            }
                                        }


                                    }
                                }

                            }
                        }

                        if (Self.Questions()[i].QuestionTypeId == 4 || Self.Questions()[i].QuestionTypeId == 14) {
                            //radio button validations
                            if (Self.Questions()[i].QuestionHide == false) { // if question in hide so skip the validation for particular question
                                if (!$("input[name=" + Self.Questions()[i].QuestionId + "_rd" + "]:checked").val()) {
                                    _isvalid = false;
                                    $("#" + Self.Questions()[i].QuestionId + "_spn").show();
                                    $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');
                                }
                                else {
                                    $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                }
                            }
                        }

                        if (Self.Questions()[i].QuestionTypeId == 3) {
                            //checkbox validation
                            if (Self.Questions()[i].ChildQuestionList.length == 0) {
                                if (Self.Questions()[i].QuestionId == 319 || Self.Questions()[i].QuestionId == 322 || Self.Questions()[i].QuestionId == 325 || Self.Questions()[i].QuestionId == 328) {
                                    //isValid = true;
                                    $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                }
                                else {
                                    if (!$("input[name=" + Self.Questions()[i].QuestionId + "_checkbox" + "]:checked").val()) {
                                        _isvalid = false;
                                        $("#" + Self.Questions()[i].QuestionId + "_spn").show();
                                        $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');

                                    }
                                    else {
                                        $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                    }
                                }

                            }
                            else {
                                if (Self.Questions()[i].SelectedChildQuestionList.length > 0) {
                                    if (Self.Questions()[i].QuestionTypeId == 3) {
                                        if (Self.Questions()[i].QuestionId == 319 || Self.Questions()[i].QuestionId == 322 || Self.Questions()[i].QuestionId == 325 || Self.Questions()[i].QuestionId == 328) {
                                            //_isvalid = true;
                                            $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                        }
                                        else {
                                            if (!$("input[name=" + Self.Questions()[i].QuestionId + "_checkbox" + "]:checked").val()) {
                                                _isvalid = false;
                                                $("#" + Self.Questions()[i].QuestionId + "_spn").show();
                                                $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');

                                            }
                                            else {
                                                $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                            }
                                        }
                                    }

                                }
                                for (var x = 0; x < Self.Questions()[i].SelectedChildQuestionList.length; x++) {
                                    if (Self.Questions()[i].SelectedChildQuestionList[x].QuestionTypeId == 3) {
                                        if (Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 320 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 321 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 323 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 324 ||
                                         Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 326 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 327 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 329 || Self.Questions()[i].SelectedChildQuestionList[x].QuestionId == 330) {
                                            //_isvalid = true;
                                            $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                        }
                                        else {
                                            if (!$("input[name=" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_checkbox" + "]:checked").val()) {
                                                _isvalid = false;
                                                $("#" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_spn").show();
                                                $("#" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_spn").text('Please answer the question.');
                                            }
                                            else {
                                                $("#" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_spn").hide();
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        if (Self.Questions()[i].ChildQuestionList.length != 0) {
                            if (Self.Questions()[i].QuestionHide == false) {
                                //matrix question validations
                                for (var k = 0; k < Self.Questions()[i].ChildQuestionList.length; k++) {
                                    //                        $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                    var count = 0;
                                    if (Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 5) {
                                        $('.' + Self.Questions()[i].ChildQuestionList[k].QuestionId + '_selectrdb').each(function (i, list) {
                                            if ($(this).is(':checked')) {
                                                count = 1;
                                                return false;
                                            }

                                        });

                                        if (count == 0) {
                                            $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").show();
                                            $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").text('Please answer the question.');
                                            _isvalid = false;
                                        }
                                        else {
                                            $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").hide();
                                        }


                                    }
                                        //validation for Subchild question dropdown type
                                    else if (Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 2 || Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 4) {
                                        for (var t = 0; t < Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList.length; t++) {
                                            if (Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].OptionId == null || Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].OptionId == undefined) {
                                                isValid = false;
                                                $("#" + Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].QuestionId + "_spn").show();
                                                $("#" + Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].QuestionId + "_spn").text('Please answer the question.');
                                            }
                                            else {
                                                $("#" + Self.Questions()[i].ChildQuestionList[k].SelectedChildQuestionList[t].QuestionId + "_spn").hide();
                                            }
                                        }
                                    }

                                }
                            }
                        }


                    }

                }

                return _isvalid;
            }

            function bindXml(ViewModel) {
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
                    //textbox type questions
                    if (ViewModel.Questions()[m].QuestionTypeId == 1) {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                        answerXML += "<question_id>" + ViewModel.Questions()[m].QuestionId + "</question_id>";
                        answerXML += "<option_id>" + 0 + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
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

                answerXML += "</profiles>";
                return answerXML;
            }

            function SaveOptions() {
                $('#dvImageLoading').show();
                $('.error').hide();
                if (Validation()) {
                    var _restext = ''; //
                    var RegStep = '';
                    var xml = bindXml(ViewModel)


                    if (_realapiflag == 1) {
                        _restext = document.getElementById('responseField').value;
                        _rq1 = document.getElementById("ra_1").value;
                        _rq2 = document.getElementById("ra_2").value;
                        _rq3 = document.getElementById("ra_3").value;
                        _rq4 = document.getElementById("ra_4").value;
                    }
                    if (uig == '' && uig == undefined)
                        var RegStep = 'B';

                    $("#spnErrorMessage").hide();
                    if (xml.indexOf("question_id") != -1) {
                        $("#lblResult").hide();
                        _realapiflag = 0;
                        $.ajax({
                            url: '/prs/SaveProfilePrescreener?uig=' + uig + '&ug=' + ug + '&cid=' + cid + '&rstext=' + _restext + '&rq1=' + _rq1 + '&rq2=' + _rq2 + '&rq3=' + _rq3 + '&rq4=' + _rq4 + '&rg=' + RegStep + '&pid=' + pid,
                            type: 'POST',
                            data: {
                                xml: xml
                            },
                            headers: {
                                '__RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value
                            },
                            success: function (pagedata) {
                                bindQuestionData(pagedata);
                            },
                            error: function () {
                                // window.location.href = '/mem/hm.aspx?ug=' + ug;
                            }
                        });

                    }
                    else {
                        redirectUrls();
                    }
                }

            }




        }
        Privacy = function () {
            $window.open('https://ps.opinionetwork.com/privacy.htm', 'SurveyDownline-Privacy', 'width=800,height=800')
        }

    }
    //added 02/17/2017.clone object
    function Clone(obj) {
        if (null == obj || "object" != typeof obj) return obj;
        var copy = obj.constructor();
        for (var attr in obj) {
            if (obj.hasOwnProperty(attr)) copy[attr] = obj[attr];
        }
        return copy;
    }

    //added 02/17/2017.Added child questions to Parent QuestionList.
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
                                if (ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionList[_rdopt] == ViewModel.Questions()[n].SelectedChildQuestionList[_chrdopt].OptionId) {
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
                                // Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList = [];
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

    //redirect url logic
    var redirectUrls = function () {
        window.location.href = "/prs/psl?uig=" + uig + "&ug=" + ug + "&cid=" + cid + "&pid=" + pid + "&conid=" + countryID;
    }

    that.init = function () {
        $('#dvImageLoading').show();
        $('#buttonSaveandCancel').hide();
        $('#FooterShowHide').hide();
        bindQuestionData(null);
    };

    $(document).ready(function () {
        that.init();
    });
}();


