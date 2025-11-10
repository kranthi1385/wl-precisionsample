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
    var OrgId = '';
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
    //
    //Get query parmas value user guid
    ug = getUrlVars()["ug"];
    if (ug == undefined) {
        uid = '';
    }
    uid = getUrlVars()["uid"];
    if (uid == undefined) {
        uid = '';
    }

    s = getUrlVars()["s"]
    if (s == undefined) {
        s = '';
    }

    //Error Function
    var Global = (function () {
        var _error = function (XMLHttpRequest, textStatus, errorThrown) {
            jQuery('#dvLoadingImg').hide();
        }
        return {
            Error: _error
        };
    })();

    //Binding the questions
    var bindQuestions = function () {
        $.ajax({
            url: '/Services/RiverProfileService.aspx?Mode=Top10Questions&ug=' + ug,
            error: function error(XMLHttpRequest, textstatus, errorThrown) {
                Global.Error(XMLHttpRequest, textstatus, errorThrown);
            },
            success: function (Pagedata) {
                if (Pagedata.length != 0) {
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
                    //                    //                    if (!jQuery.browser.mobile) {
                    //                    $('.icheck').on('ifChecked ifUnchecked', function(event) {
                    //                        $(this).triggerHandler('click');
                    //                    }).iCheck({
                    //                        checkboxClass: 'icheckbox_square-grey',
                    //                        radioClass: 'iradio_square-grey',
                    //                        increaseArea: '20%' // optional
                    //                    });
                    //                    }
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
                            //                            if (!(jQuery.browser.mobile || OSName)) {
                            //                            $("#" + ViewModel.Questions()[i].QuestionId + '_trChildQuestionId .icheck').on('ifChecked ifUnchecked', function(event) {
                            //                                $(this).triggerHandler('click');
                            //                            }).iCheck({
                            //                                checkboxClass: 'icheckbox_square-grey',
                            //                                radioClass: 'iradio_square-grey',
                            //                                increaseArea: '20%' // optional
                            //                            });
                            //                            }

                            for (var s = 0; s < ViewModel.Questions()[i].OptionList.length; s++) {
                                for (var t = 0; t < ViewModel.Questions()[i].ResponseOptionList.length; t++) {
                                    if (ViewModel.Questions()[i].OptionList[s].OptionId == ViewModel.Questions()[i].ResponseOptionList[t].OptionId) {
                                        $("#" + ViewModel.Questions()[i].OptionList[s].OptionId + '_checkbox').iCheck('check');
                                    }
                                }
                            }

                            for (var x = 0; x < ViewModel.Questions()[i].ChildQuestionList.length; x++) {
                                for (var y = 0; y < ViewModel.Questions()[i].ChildQuestionList[x].OptionList.length; y++) {
                                    for (var z = 0; z < ViewModel.Questions()[i].ChildQuestionList[x].ResponseOptionList.length; z++) {
                                        if (ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId == ViewModel.Questions()[i].ChildQuestionList[x].ResponseOptionList[z].OptionId) {
                                            $("#" + ViewModel.Questions()[i].ChildQuestionList[x].OptionList[y].OptionId + '_checkbox').iCheck('check');
                                        }
                                    }
                                }
                            }
                        }
                        for (j = 0; j < ViewModel.Questions()[i].OptionList.length; j++) {
                            if (ViewModel.Questions()[i].OptionId == ViewModel.Questions()[i].OptionList[j].OptionId) {
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
                                //                                if (!(jQuery.browser.mobile || OSName)) {
                                //                                $("#" + ViewModel.Questions()[i].QuestionId + '_trChildQuestionId .icheck').on('ifChecked ifUnchecked', function(event) {
                                //                                    $(this).triggerHandler('click');
                                //                                }).iCheck({
                                //                                    checkboxClass: 'icheckbox_square-grey',
                                //                                    radioClass: 'iradio_square-grey',
                                //                                    increaseArea: '20%' // optional
                                //                                });
                                //                                }
                            }
                        }
                        //Bind the save data for child Questions and options
                        for (var k = 0; k < ViewModel.Questions()[i].ChildQuestionList.length; k++) {
                            var subChildQuestions = [];
                            var subChildOptions = [];
                            for (var l = 0; l < ViewModel.Questions()[i].ChildQuestionList[k].OptionList.length; l++) {
                                if (ViewModel.Questions()[i].ChildQuestionList[k].OptionId == ViewModel.Questions()[i].ChildQuestionList[k].OptionList[l].OptionId) {
                                    if (ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList.length != 0) {
                                        for (var m = 0; m < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList.length; m++) {
                                            subChildQuestions.push(ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m])
                                            for (var p = 0; p < ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList.length; p++) {
                                                subChildOptions.push(ViewModel.Questions()[i].ChildQuestionList[k].ChildQuestionList[m].OptionList[p]);
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
                }
                else {
                    window.location.href = "/River/entry.htm?ug=" + ug;
                }
                $('#dvImageLoading').hide();
            }

        });
    }

    var SurveyQuestion = function (questions) {
        var Self = this;
        var IsChaildMatrix = false;
        Self.Questions = new ko.observableArray(questions);
        UserId = Self.Questions()[0].UserId;
        //radio button click event
        self.rdClick = function (parent, event, i) {
            var _id = i.target.id;
            var _dupAnswers = '';
            var _dvId = _id.split('_')[0] + '_' + _id.split('_')[1];
            var _rowOptionID = _id.split('_')[0];
            var _optId = _id.split('_')[1];

            if (_rdAnswers == '') {
                _rdAnswers = _dvId + '_' + event.OptionText;
                $('#' + _dvId + '_radio').addClass("input_on");   /*  if Web click then add class to radio div id */
                $('#' + _dvId + '_dvmain').addClass("input_onm");            /*  if Mobile click then add class to radio div id */
                $('#' + _optId + '_radio').attr("checked", 'checked');
                parent.OptionId = _optId          /*  if div click then checked  radio button  */
            }
            else {
                var _ansArray = _rdAnswers.split(';');

                for (var i = 0; i < _ansArray.length; i++) {        /*---------------------------- Entry For loop 2 ----------------------------------------*/

                    if (_ansArray[i].split('_')[0] == _rowOptionID) {  /* Remove duplicate answer when already exists same Question answer  when array */


                        $('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_radio').removeClass("input_on");   /* Remove  already applying css class if web click */

                        jQuery('#' + _ansArray[i].split('_')[0] + '_' + _ansArray[i].split('_')[1] + '_dvmain').removeClass("input_onm");   /* Remove already applying css class if mobile click */

                        _ansArray.splice(i, 1);                                    /* Remove object from answers list */
                    }
                    if (i < _ansArray.length) {                                    /* when array item is removed then array size will reduce */
                        if (i == 0) {
                            _dupAnswers = _ansArray[i];
                        }
                        else {
                            _dupAnswers = _dupAnswers + ';' + _ansArray[i];
                        }
                    }
                } /*------------------------------------------------------------------------------- End For loop 2 ----------------------------------------*/
                _dupAnswers = _dupAnswers + ';' + _dvId + '_' + event.OptionText;
                $('#' + _optId + '_radio').attr("checked", 'checked');
                parent.OptionId = _optId          /*  if div click then checked  radio button  */;

                $('#' + _dvId + '_radio').addClass("input_on");
                $('#' + _dvId + '_dvmain').addClass("input_onm");
                _rdAnswers = _dupAnswers;
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
        //selection change event select parent question child Questions and options
        //        $(document).on('change', self.selectionChanged, function(event) {
        selectionChanged = function (event) {
            $('#dvImageLoading').show();
            if (event.OptionId != null && event.OptionId != '' && event.OptionId != undefined) {
                $("#" + event.QuestionId + '_trChildQuestionId').css('display', 'inline');
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
                for (var n = 0; n < Self.Questions().length; n++) {
                    if (Self.Questions()[n].QuestionId == event.QuestionId) {
                        Self.Questions()[n].SelectedChildQuestionList = childQuestions;
                        for (var p = 0; p < Self.Questions()[n].SelectedChildQuestionList.length; p++) {
                            Self.Questions()[n].SelectedChildQuestionList[p].OptionList = [];
                            for (var q = 0; q < childOptions.length; q++) {
                                if (Self.Questions()[n].SelectedChildQuestionList[p].QuestionId == childOptions[q].QuestionId) {
                                    Self.Questions()[n].SelectedChildQuestionList[p].OptionList.push(childOptions[q]);
                                }
                            }
                        }
                        ko.applyBindings(Self.Questions()[n], document.getElementById(event.QuestionId + "_trChildQuestionId"));
                        //                        if (!(jQuery.browser.mobile || OSName)) {
                        //                        $("#" + event.QuestionId + '_trChildQuestionId .icheck').on('ifChecked ifUnchecked', function(event) {
                        //                            $(this).triggerHandler('click');
                        //                        }).iCheck({
                        //                            checkboxClass: 'icheckbox_square-grey',
                        //                            radioClass: 'iradio_square-grey',
                        //                            increaseArea: '20%' // optional
                        //                        });
                        //                        //                        }
                    }
                        //Bind the childquestion selected subchildquestionlist
                    else {
                        for (var k = 0; k < Self.Questions()[n].ChildQuestionList.length; k++) {
                            if (Self.Questions()[n].ChildQuestionList[k].QuestionId == event.QuestionId) {
                                Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList = childQuestions;
                                if (Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList != 0) {
                                    for (var p = 0; p < Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList.length; p++) {
                                        Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList = [];
                                        // Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList = [];
                                        for (var q = 0; q < event.SubChildOptions.length; q++) {
                                            if (event.OptionId == event.SubChildOptions[q].ParentOptionId) {
                                                Self.Questions()[n].ChildQuestionList[k].SelectedChildQuestionList[p].OptionList.push(event.SubChildOptions[q]);
                                            }
                                        }
                                    }
                                    // $("#" + Self.Questions()[n].SelectedChildQuestionList[k].QuestionId + "_trChildQuestionId").html('');
                                    ko.cleanNode(document.getElementById(Self.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId"));
                                    $('#' + Self.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId").html('');
                                    ko.applyBindings(Self.Questions()[n].ChildQuestionList[k], $("#" + Self.Questions()[n].ChildQuestionList[k].QuestionId + "_trChildQuestionId").get(0));
                                    //                                    if (!(jQuery.browser.mobile || OSName)) {
                                    //                                    $("#" + event.QuestionId + '_trChildQuestionId .icheck').on('ifChecked ifUnchecked', function(event) {
                                    //                                        $(this).triggerHandler('click');
                                    //                                    }).iCheck({
                                    //                                        checkboxClass: 'icheckbox_square-grey',
                                    //                                        radioClass: 'iradio_square-grey',
                                    //                                        increaseArea: '20%' // optional
                                    //                                    });
                                    //                                    }
                                }
                            }
                        }
                    }
                }
            }
                //here click select option display data none
            else {
                $("#" + event.QuestionId + '_trChildQuestionId').css('display', 'none');
            }
            $('#dvImageLoading').hide();
        }
        //        //click function check all validations
        //        Self.Cancel = function () {
        //            window.location.href = '/mem/hm.aspx?ug=' + uid;
        //        }
        Self.Save = function () {
            $("#btnSubmit").hide();
            $("#spnMessage").show();
            $('#dvImageLoading').show();
            myVar = setTimeout(SaveOptions(), 100);
        }
        Self.Skip = function () {
            $.ajax({
                url: '/services/RiverProfileService.aspx?Mode=Inserttop10skiplog&ug=' + ug,
                success: function (data) {
                    $('#dvImageLoading').hide();
                    if (parseInt(data) > 0) { //added 5/30/2015 member contains vertiy challange question
                        $('#dvImageLoading').hide();
                        $("#spnMessage").hide();
                        window.location.href = "/River/entry.htm?ug=" + ug;
                    }
                    else {
                        $('#dvImageLoading').hide();
                        $("#spnMessage").hide();
                        window.location.href = "/River/entry.htm?ug=" + ug;
                    }
                }
            });
        }
        function SaveOptions() {
            var isValid = true;
            for (var i = 0; i < Self.Questions().length; i++) {
                //parent dropdown validations
                if (Self.Questions()[i].QuestionTypeId == 2) {
                    if (Self.Questions()[i].OptionId == null || Self.Questions()[i].OptionId == undefined) {
                        isValid = false;
                        $("#" + Self.Questions()[i].QuestionId + "_spn").show();
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
                                                    isValid = false;
                                                }
                                                else {
                                                    $("#" + Self.Questions()[i].SelectedChildQuestionList[j].ChildQuestionList[k].QuestionId + "_spn").hide();
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId == 4) {
                                    if (!$("input[name=" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_rd" + "]:checked").val()) {
                                        isValid = false;
                                        $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").show();
                                        $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").text('Please answer the question.');
                                    }
                                    else {

                                        $("#" + Self.Questions()[i].SelectedChildQuestionList[j].QuestionId + "_spn").hide();
                                    }

                                }
                                    //validations for selected questionlist 
                                else if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId != 3) {
                                    if (Self.Questions()[i].SelectedChildQuestionList[j].QuestionTypeId != 4) {
                                        if (Self.Questions()[i].SelectedChildQuestionList[j].OptionId == null || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == 0 || Self.Questions()[i].SelectedChildQuestionList[j].OptionId == undefined) {
                                            isValid = false;
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
                if (Self.Questions()[i].QuestionTypeId == 4) {
                    //radio button validations
                    if (!$("input[name=" + Self.Questions()[i].QuestionId + "_rd" + "]:checked").val()) {
                        isValid = false;
                        $("#" + Self.Questions()[i].QuestionId + "_spn").show();
                        $("#" + Self.Questions()[i].QuestionId + "_spn").text('Please answer the question.');
                    }
                    else {
                        $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                    }
                }
                if (Self.Questions()[i].QuestionTypeId == 3) {
                    //checkbox validation
                    if (Self.Questions()[i].ChildQuestionList.length == 0) {
                        if (Self.Questions()[i].QuestionId == 319 || Self.Questions()[i].QuestionId == 322 || Self.Questions()[i].QuestionId == 325 || Self.Questions()[i].QuestionId == 328) {
                            isValid = true;
                            $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                        }
                        else {
                            if (!$("input[name=" + Self.Questions()[i].QuestionId + "_checkbox" + "]:checked").val()) {
                                isValid = false;
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
                                    isValid = true;
                                    $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                }
                                else {
                                    if (!$("input[name=" + Self.Questions()[i].QuestionId + "_checkbox" + "]:checked").val()) {
                                        isValid = false;
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
                                    isValid = true;
                                    $("#" + Self.Questions()[i].QuestionId + "_spn").hide();
                                }
                                else {
                                    if (!$("input[name=" + Self.Questions()[i].SelectedChildQuestionList[x].QuestionId + "_checkbox" + "]:checked").val()) {
                                        isValid = false;
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
                                isValid = false;
                            }
                            else {
                                $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").hide();
                            }
                        }
                            //                        else if (Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 3) {
                            //                            if (!$("input[name=" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_checkbox" + "]:checked").val()) {
                            //                                isValid = false;
                            //                                $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").show();
                            //                                $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").text('Please answer the question.');
                            //                            }
                            //                            else {
                            //                                $("#" + Self.Questions()[i].ChildQuestionList[k].QuestionId + "_spn").hide();
                            //                            }

                            //                        }
                            //validation for Subchild question dropdown type
                        else if (Self.Questions()[i].ChildQuestionList[k].QuestionTypeId == 2) {
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
                                                if (ViewModel.Questions()[m].SelectedChildQuestionList[p].OptionLis[q].IsChecked) {
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

                        // Radio Question type with other
                    else {
                        answerXML += "<profile>";
                        answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                        answerXML += "<question_id>" + ViewModel.Questions()[m].QuestionId + "</question_id>";
                        answerXML += "<option_id>" + ViewModel.Questions()[m].OptionId + "</option_id>";
                        //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                        answerXML += "</profile>";
                        if (ViewModel.Questions()[m].SelectedChildQuestionList.length > 0) {
                            for (var n = 0; n < ViewModel.Questions()[m].SelectedChildQuestionList.length; n++) {
                                if (ViewModel.Questions()[m].SelectedChildQuestionList[n].OptionId != 0) {
                                    answerXML += "<profile>";
                                    answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                    answerXML += "<question_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].QuestionId + "</question_id>";
                                    answerXML += "<option_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].OptionId + "</option_id>";
                                    //answerXML += "<option_text><![CDATA[" + objSelectedChildOptions.OptionText + "]]></option_text>";
                                    answerXML += "</profile>";
                                    for (var p = 0; p < ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList.length; p++) {
                                        if (ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionTypeId == 5) {
                                            answerXML += "<profile>";
                                            answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                            answerXML += "<question_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionId + "</question_id>";
                                            answerXML += "<option_id>" + ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].OptionId + "</option_id>";
                                            //answerXML += "<option_text><![CDATA[" + objSubChildOptions.OptionText + "]]></option_text>";
                                            answerXML += "</profile>";
                                        }
                                    }
                                }
                                else {
                                    for (var p = 0; p < ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList.length; p++) {
                                        if (ViewModel.Questions()[m].SelectedChildQuestionList[n].ChildQuestionList[p].QuestionTypeId == 5) {
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
                        //DropdownQuestionType 
                    }
                    //matrix questions
                    if (ViewModel.Questions()[m].ChildQuestionList.length != 0) {
                        for (var p = 0; p < ViewModel.Questions()[m].ChildQuestionList.length; p++) {
                            if (ViewModel.Questions()[m].ChildQuestionList[p].QuestionTypeId == 5) {
                                answerXML += "<profile>";
                                answerXML += "<user_id>" + ViewModel.Questions()[m].UserId + "</user_id>";
                                answerXML += "<question_id>" + ViewModel.Questions()[m].ChildQuestionList[p].QuestionId + "</question_id>";
                                answerXML += "<option_id>" + ViewModel.Questions()[m].ChildQuestionList[p].OptionId + "</option_id>";
                                //answerXML += "<option_text><![CDATA[" + objAnswersSave.OptionText + "]]></option_text>";
                                answerXML += "</profile>";
                            }
                            if (ViewModel.Questions()[m].ChildQuestionList[p].QuestionTypeId == 2) {
                                for (var q = 0; q < ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList.length; q++) {
                                    if (ViewModel.Questions()[m].ChildQuestionList[p].SelectedChildQuestionList[q].OptionId != 0) {
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
                answerXML += "</profiles>";
                return answerXML;
            }
            //saving data
            if (isValid == true) {
                var xml = bindXml(ViewModel)
                $("#spnErrorMessage").hide();
                $.ajax({
                    dataType: "text",
                    type: 'POST',
                    data: xml,
                    url: '/services/RiverProfileService.aspx?Mode=OptionsSaveTop10&ug=' + ug,
                    success: function (pagedata) {
                        $('#dvImageLoading').hide();
                        $("#spnMessage").hide();
                        window.location.href = "/River/entry.htm?ug=" + ug;
                    }
                });
            }
            else {
                $('#dvImageLoading').hide();
                $("#spnMessage").hide();
                $("#btnSubmit").show();
                $("#spnErrorMessage").show();
                $("#spnErrorMessage").text('Please fill out all required fields');
            }
        }
    }

    //declare init function

    that.init = function () {
        if (ug != 0) {
            $('#dvImageLoading').show();
            bindQuestions();
        }
        else {
            $('#dvSurveyQuestions').css('height', '100px')
            $('#spnSurveyMessage').hide();
            $('#spnFraudMessage').show();
            $('#dvButtons').hide();
            $('#dvImageLoading').hide();
        }
    };

    $(document).ready(function () {
        var imgArr = new Array( // relative paths of images
    '/Images/background-1.jpg',
    '/Images/background-2.jpg',
    '/Images/background-3.jpg',
    '/Images/background-4.jpg'
    );
        var preloadArr = new Array();
        var i; /* preload images */
        for (i = 0; i < imgArr.length; i++) {
            preloadArr[i] = new Image();
            preloadArr[i].src = imgArr[i];
        }
        var currImg = 1;
        var intID = setInterval(changeImg, 6000); /* image rotator */function changeImg() {
            $('.cp-bg').animate(1000, function () {
                $(this).css('background-image', 'url(' + preloadArr[currImg++ % preloadArr.length].src + ' )');
            }).animate(1000);
        }
        if ($.browser.msie) {
            if ($.browser.version <= 8.0) {
                $('#lnkMainCss').attr("href", "/css/river-mainIE.css");
                $("#lnkTop10").attr("href", "/css/top10-ie.css");
            }
        }

        that.init();
    });
}();


