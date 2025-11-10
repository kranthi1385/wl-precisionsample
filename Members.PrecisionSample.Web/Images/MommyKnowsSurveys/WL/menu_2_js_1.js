Config={
	isIE:     		!!(window.attachEvent && !window.opera),
	isIE6:			(window.attachEvent && !window.XMLHttpRequest && !window.opera),
	isIE7:			(window.attachEvent && window.XMLHttpRequest && !window.opera),
	isGecko:  		(navigator.userAgent.indexOf('Gecko') > -1 && navigator.userAgent.indexOf('KHTML') == -1),
	isChrome:		!!(navigator.userAgent.match('Chrome')),
	isWebKit: 		(navigator.userAgent.indexOf('AppleWebKit/') > -1),
	isOpera:  		!!(window.opera),
	isMobileSafari: !!(navigator.userAgent.match(/Apple.*Mobile.*Safari/))
};

myUtils={
	getBrowser:function(){
		for(b in Config){
			if(b == "isIE") continue;
			if(Config[b]) return b.substr(2,b.length-2);
		}
	}
};

if(myUtils.getBrowser() == "IE6"){
	window.onload = function(){
		document.body.className=(document.body.className.toString().length==0)?"IE6":" IE6";
	}
}

function buildsubmenus_horizontal(){
	var menuids=["treemenu1"] //Enter id(s) of SuckerTree UL menus, separated by commas
	for (var i=0; i<menuids.length; i++){
		
		if(document.getElementById(menuids[i])){
			var ultags = document.getElementById(menuids[i]).getElementsByTagName("ul");
			var isVertical = document.getElementById(menuids[i]).getAttribute("isVertical");
			
			var initialHeight = parseInt(document.getElementById(menuids[i]).parentNode.offsetHeight);
			var initialWidth = parseInt(document.getElementById(menuids[i]).parentNode.offsetWidth);
			var menuStyle = document.getElementById('treemenu1').parentNode.parentNode.style.cssText;
			var opacityExist = 0;

			if (menuStyle.match(/FILTER:/ig))
				opacityExist = 1;
			else
				opacityExist = 0;

			initialHeight = (initialHeight < 25)?25:initialHeight;
			
			for (var t=0; t<ultags.length; t++){

				if (ultags[t].parentNode.parentNode.id==menuids[i]){ //if this is a first level submenu
					
					if(isVertical == 1) {
						ultags[t].style.top="0px" //dynamically position first level submenus to be height of main menu item
						ultags[t].style.left="138px"; //dynamically position first level submenus to be height of main menu item
					} else {
						ultags[t].style.top=ultags[t].parentNode.offsetHeight+"px" //dynamically position first level submenus to be height of main menu item
					}
				}

				ultags[t].parentNode.onmouseover=function(){

					this.getElementsByTagName("ul")[0].style.visibility="visible";
                                        this.getElementsByTagName("ul")[0].style.display="block";

					if(Config.isIE && isVertical==0 && opacityExist == 1){

						this.style.zIndex="9999999";
						h = parseInt(this.getElementsByTagName("ul")[0].offsetHeight);
						this.parentNode.parentNode.parentNode.style.height = h + initialHeight+"px";
						this.getElementsByTagName("ul")[0].parentNode.parentNode.style.height = 'auto';
						
					
					} else if(Config.isIE && isVertical==null && opacityExist == 1){
						this.style.zIndex="9999999";
						h = parseInt(this.getElementsByTagName("ul")[0].offsetHeight);
						this.parentNode.parentNode.parentNode.style.height = h + initialHeight+"px";
						//this.getElementsByTagName("ul")[0].parentNode.parentNode.style.height = 'auto';

					} else if(Config.isIE && isVertical== 1 && opacityExist == 1){
						this.style.zIndex="9999999";
						w = parseInt(this.getElementsByTagName("ul")[0].offsetWidth);
						this.parentNode.parentNode.parentNode.style.width = w + initialWidth+"px";
						this.getElementsByTagName("ul")[0].parentNode.parentNode.style.width = 'auto';

						h = parseInt(this.getElementsByTagName("ul")[0].offsetHeight);
						//this.parentNode.parentNode.parentNode.style.height = h + initialHeight+"px";
					}
				}
				
				ultags[t].parentNode.onmouseout=function(){
					this.getElementsByTagName("ul")[0].style.visibility="hidden";
                                        this.getElementsByTagName("ul")[0].style.display="none";
					
					if(Config.isIE && isVertical==0 && opacityExist == 1){
						this.parentNode.parentNode.parentNode.style.height = initialHeight+"px";
						this.getElementsByTagName("ul")[0].parentNode.parentNode.style.height = initialHeight+"px";
					
					} else if(Config.isIE && isVertical==null && opacityExist == 1){
						this.parentNode.parentNode.parentNode.style.height = initialHeight+"px";
						this.getElementsByTagName("ul")[0].parentNode.parentNode.style.height = initialHeight+"px";
					
					} else if(Config.isIE && isVertical==1 && opacityExist == 1){
						this.parentNode.parentNode.parentNode.style.width = initialWidth+"px";
						this.parentNode.parentNode.parentNode.style.height = initialHeight+"px";
					}
					
				}
			}
		}	
	}
}

if (window.addEventListener)
	window.addEventListener("load", buildsubmenus_horizontal, false);
else if (window.attachEvent)
	window.attachEvent("onload", buildsubmenus_horizontal);