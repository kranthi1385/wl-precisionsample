// JavaScript Document

/*
###########################################################*/
function validateZip()
{
	if(isEmpty($('zip')) && lengthRestriction($('zip'), 5, 5) && isAlphNum($('zip'), /^[0-9]+$/))
	{
		return true;
	}
	else
	{
		alert('Please Enter a Valid Zip Code');
		return false;	
	}
}

/*
###########################################################*/
function validateQualifier()
{
	if(!isEmpty($('email_address')) || !emailValidator($('email_address')))
	{
		alert('Please Enter a Valid Email Address');
		return false;	
	}
	if(!isEmpty($('first_name')))
	{
		alert('Please Enter a Valid First Name');
		return false;	
	}
	if(!isEmpty($('last_name')))
	{
		alert('Please Enter a Valid Last Name');
		return false;	
	}
	if(!isEmpty($('address')))
	{
		alert('Please Enter a Valid Address');
		return false;	
	}
	if(!isEmpty($('city')))
	{
		alert('Please Enter a Valid City');
		return false;	
	}
	if(!isEmpty($('state')))
	{
		alert('Please Select a State');
		return false;	
	}
	if(!isEmpty($('zip')) || !lengthRestriction($('zip'), 5, 5) || !isAlphNum($('zip'), /^[0-9]+$/))
	{
		alert('Please Enter a Valid Zip Code');
		return false;
	}
	if(!isEmpty($('gender')))
	{
		alert('Please Select Your Gender.');
		return false;	
	}
	if(!isEmpty($('bday_year')) || !isEmpty($('bday_day')) || !isEmpty($('bday_month')))
	{
		alert('Please Enter a Valid Date of Birth');
		return false;
	}
	return true;
}

function isprefix(elem){
	var checkone = /^[0]{3}$/;
	var checktwo = /^[1]{3}$/;
	var checkthree = /^[5]{3}$/;
	var checkfour = /^[9]{3}$/;
	var finalcheck = /^[0-9]+$/;
	var onedigit = /^[0-9]{1}$/;
	var twodigit = /^[0-9]{2}$/;
	if(elem.value.match(checkone)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(checktwo)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(checkthree)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(checkfour)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(onedigit)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(twodigit)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value == "123"){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(finalcheck)){
		return true;
	}else{
		//alert(helperMsg);
		elem.focus();
		return false;
	}
}

function issuffix(elem){
	var numericExpression = /^[0-9]+$/;
	var onedigit = /^[0-9]{1}$/;
	var twodigit = /^[0-9]{2}$/;
	var threedigit = /^[0-9]{3}$/;
	if(elem.value.match(onedigit)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(twodigit)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(threedigit)){	
			//alert(helperMsg);
		elem.focus();
			return false;
	}else if(elem.value.match(numericExpression)){
		return true;
	}else{
		//alert(helperMsg);
		elem.focus();
		return false;
	}
}

/*
###########################################################*/
function validateContact()
{
	if(!isEmpty($('email_address')) || !emailValidator($('email_address')))
	{
		alert('Please Enter a Valid Email Address');
		return false;	
	}	
	if(!isEmpty($('sender_name')))
	{
		alert('Please Enter a Valid Name');
		return false;	
	}
	if(!isEmpty($('message')))
	{
		alert('Please Type a Message');
		return false;	
	}
	return true;
}

/*
###########################################################*/
function isEmpty(elem)
{
	if(elem.value.length == 0)
	{		
		elem.focus(); // set the focus to this input
		return false;
	}
	return true;
}

/*
###########################################################*/
function lengthRestriction(elem, min, max)
{
	var uInput = elem.value;
	if(uInput.length >= min && uInput.length <= max)
	{
		return true;
	}
	else
	{
		elem.focus();
		return false;
	}
}

/*
###########################################################*/
function isAlphNum(elem, useExp)
{
	var alphaExp = useExp;
	if(elem.value.match(alphaExp))
	{
		return true;
	}
	else
	{
		elem.focus();
		return false;
	}
}

/*
###########################################################*/
function emailValidator(elem)
{
	var emailExp = /^[\w\-\.\+]+\@[a-zA-Z0-9\.\-]+\.[a-zA-z0-9]{2,4}$/;
	if(elem.value.match(emailExp))
	{
		return true;
	}
	else
	{
		elem.focus();
		return false;
	}
}

/*
###########################################################*/
function redirectBrowser()
{
	window.location = 'survey.php';	
}

/*
###########################################################*/
function launch_offer(user_key, href, iter)
{
	var url = 'rc_inc/ajax/make_progress.php';
	var params = "user_key=" + user_key + "&iter=" + iter;
	var ajax = new Ajax.Request(url,{
	 method: 'post',
	 parameters: params
	});
	window.open(href);
}

/*
###########################################################*/
function launch_survey(user_key, href, action, id)
{
	var url = 'rc_inc/ajax/make_progress.php';
	var params = "user_key=" + user_key + "&type=survey&action=" + action + "&id=" + id;
	var ajax = new Ajax.Request(url,{
	 method: 'post',
	 parameters: params
	});
	if(action=='accept')
	{
		mywindow = window.open (href);
  		mywindow.moveTo(0, 0);
	}
}

/*
###########################################################*/
function check_progress(type, href, iter)
{
	var url = 'rc_inc/ajax/check_progress.php';
	var params = "type=" + type + '&iter=' + iter;
	var ajax = new Ajax.Request(url,{
	 method: 'post',
	 parameters: params,
	 onSuccess : function(transport) {
		 if(transport.responseText == 'PASS')
		 {
			window.location = href; 
		 }
		 else
		 {
			alert(transport.responseText); 
		 }
	 }
	//onLoading: function(){$('client_display').innerHTML = '<tr><td>Loading...</td></tr>'}
	 //onLoaded: function(){$('workingMsg').hide()}
	});		
}

/*
###########################################################*/
function check_survey_progress(type, href, count, foid, user_key)
{
	var url = 'rc_inc/ajax/check_progress.php';
	var params = "type=" + type + "&iter=" + count;
	var ajax = new Ajax.Request(url,{
	 method: 'post',
	 parameters: params,
	 onSuccess : function(transport) {
		 if(transport.responseText == 'PASS')
		 {
			/*var popunder = 'silver_level.php?foid=' + foid + '&u=' + user_key;
			var windowprops = "location=1,menubar=1,toolbar=1,scrollbars=1,resizable=1";
			newWindow = window.open(popunder,'PopupName',windowprops);
			newWindow.blur();
			setTimeout("window.focus();", 2000);*/
			
			window.location = href;
			
		 }
		 else
		 {
			alert(transport.responseText); 
		 }
	 }
	//onLoading: function(){$('client_display').innerHTML = '<tr><td>Loading...</td></tr>'}
	 //onLoaded: function(){$('workingMsg').hide()}
	});		
}

/*
###########################################################*/
function reload_status(id)
{
	window.location = 'status_report.php?offer_key=' + id;	
}


