userBrowser = (function(){
    var ua = navigator.userAgent;
    var isOpera = Object.prototype.toString.call(window.opera) == '[object Opera]';
    return {
      IE:             !!window.attachEvent && !isOpera,
      Opera:          isOpera,
      WebKit:         ua.indexOf('AppleWebKit/') > -1,
      Gecko:          ua.indexOf('Gecko') > -1 && ua.indexOf('KHTML') === -1,
      MobileSafari:   /Apple.*Mobile.*Safari/.test(ua)
    }
})();

if(userBrowser.WebKit){
	document.write('<style type="text/css">\n<!--\nhtml, body, div, span, table {-webkit-text-size-adjust:none;}\n-->\n</style>');
}
includeFile(getCurrentPath()+"ws-common.css");

function includeFile(files){
	
	var arr = mixedReturn(files);

	for(i=0;i<arr.length;i++){

		var filename = arr[i];
		var filetype = getExt(filename);

		if (filetype=="js"){ //if filename is a external JavaScript file
			var fileref=document.createElement('script');
			fileref.setAttribute("type","text/javascript");
			fileref.setAttribute("src", filename);
		}
		else if (filetype=="css"){ //if filename is an external CSS file
			var fileref=document.createElement("link");
			fileref.setAttribute("rel", "stylesheet");
			fileref.setAttribute("type", "text/css");
			fileref.setAttribute("href", filename);
		}
		if (typeof fileref!="undefined")
			document.getElementsByTagName("head")[0].appendChild(fileref);
	}
}

function mixedReturn(Obj){
	if(typeof Obj == "string"){
		var arr = new Array();
		arr[0] = Obj;
	}else{
		arr = Obj;
	}
	return arr;
}

function getExt(FileName){
	var ext = "";
	if(FileName.indexOf(".")!=-1){
		var tmp = FileName.lastIndexOf(".");
		ext = FileName.substr(tmp+1, (FileName.length-tmp)-1);
		ext = ext.toLowerCase();
	}
	return ext;
}

function getCurrentPath(){
	var arr = document.getElementsByTagName("SCRIPT");
	for(var i=0;i<arr.length;i++)
	{
		if(arr[i].src && arr[i].src.indexOf("ws-common.js") != -1){
			return arr[i].src.replace("ws-common.js", "");
		}
	}
	return "";
}