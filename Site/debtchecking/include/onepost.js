// include this script to prevent form being submitted twice
// (i.e when user click the button twice)
// form must be named Form1 in the aspx page for this to work
var processing = false;
function cekmandatorypanel(panel, excludelist)
{
    if(panel == null)
    {
        max_elm = (document.forms[0].elements.length) - 2;
		elmlst = document.forms[0].elements;
    }
    else
    {
		max_elm = panel.GetMainElement().all.length-1;
        elmlst = panel.GetMainElement().all;
    }
    dxmandatory = false;
	for (var i=0; i<max_elm; i++)
	{
	    elm = elmlst[i];
		if(excludelist != null && elm.name!="framex" && (elm.className == "mandatory" || elm.className == "dxeButtonEdit mandatory"))
	    {
	        exclude = false;
	        for(var j=0;j<excludelist.length;j++)
	        {
	            if(elm.name.indexOf(excludelist[j]) != -1)
                {
                    exclude = true;
                    break;
                }
            }
            if(exclude) continue;
	    }
        if(elm.className=="dxeButtonEdit mandatory")
        { 
            dxelm = elm;
	        dxmandatory = true;
	    }
		try
		{
		    if (elm.type == "radio") {
		        var idradio = elm.id.substring(0, elm.id.length - 2);
		        var radioname = elm.name;
		        var objparent = document.getElementById(idradio);
		        if (objparent.className == "mandatory") {
		            var radiolist = document.getElementsByName(radioname);
		            var pass = false;
		            dxmandatory = false;
		            for (var j = 0; j <= radiolist.length - 1; j++) {
		                if (radiolist[j].checked) { pass = true; break; }
		            }
		            
		            if (!pass) {
		                var r = objparent.parentElement;
		                var d;
		                var counter = 0;
		                while (d == null && counter < 8) {
		                    r = r.parentElement;
		                    try {d = r.cells(0).innerText;}
		                    catch (ex) { d = null; counter = counter + 1; }
		                }
		                if (d == null || d == "") {
		                    alert("lengkapi dulu data yang kosong...");
		                } else {
		                    alert(d + " tidak boleh kosong...");
		                }
		                return false;
		            }
		        }
		    }

		    if(dxmandatory && elm.type=="text" && elm.value!="")
		        dxmandatory=false;
		    if ((elm.className == "mandatory"||(dxmandatory && elm.type=="text")) && (elm.value == "") )
		    {
			    try{
			    
			    if(dxmandatory)elm=dxelm;
			    
			    var r = elm.parentElement;
			    var d;
			    var counter = 0;
			    
			    while (d == null && counter < 8)
			    {
				    r = r.parentElement;
				    try{d = r.cells(0).innerText;}
				    catch(ex){d = null;counter=counter+1;}
			    }
			    if (d == null || d == "") {
			        alert("lengkapi dulu data yang kosong...");
			    } else {
			        alert(d + " tidak boleh kosong...");
			    }
    			
    			}
    			catch(e){}
    			dxmandatory=false;
			    elm.focus();
			    return false;
		    }
		 } catch(e){};
	}
	return true;
}

function endcallback(s, e) {
    if(s.hasOwnProperty("cp_alert")&&s.cp_alert!="")
    {
        s.cp_alert = replaceAll(s.cp_alert, "\\n", "\n");
        alert(s.cp_alert);

        s.cp_alert = "";
    }
    if(s.hasOwnProperty("cp_redirect"))
    {
        if(s.hasOwnProperty("cp_target")&&s.cp_target=="_parent")
            parent.window.location = s.cp_redirect;
        if(s.hasOwnProperty("cp_target")&&s.cp_target=="_blank")
            PopupPage(s.cp_redirect,800,600,'yes');
        else
            window.location =  s.cp_redirect;
    }   
    processing=false;
    try
    {
        window.parent.resizeFrame();
    }
    catch(e)
    {
    };
}

function replaceAll(str, oldchr, newchr)
{
	while (str.indexOf(oldchr) >= 0)
		str = str.replace(oldchr, newchr);
	return str;
}

function callback(objcallback,parameter,cekmandatory, excludelist)
{
    if(cekmandatory==null)cekmandatory=true;
    if(!processing)
    if(!cekmandatory || cekmandatorypanel(objcallback, excludelist))
    {
        processing=true;
        objcallback.EndCallback.RemoveHandler(endcallback);
        objcallback.EndCallback.AddHandler(endcallback);
        objcallback.PerformCallback(parameter)
    }
}

function callbackpopup(popup,objcallback,parameter,objrefcallback,refparameter,cekmandatory)
{
    if (cekmandatory == null) cekmandatory = false;
    objcallback.EndCallback.ClearHandlers();
    if(!processing)
    {
        if(objrefcallback==null)
        {
            popup.Show();
            callback(objcallback, parameter, cekmandatory);
        }
        else
        {
            objcallback.EndCallback.AddHandler(
                function customcallback(s,e)
                {
                    popup.Hide();
                    processing = false;
                    callback(objrefcallback, refparameter, cekmandatory);
                }
            );
            callback(objcallback, parameter, cekmandatory);
        }
    }
}

var popupWindow = null;
function PopupPage(href,width,height,scroll,onepop)
{
	if (popupWindow != null){popupWindow.close();}
	if (width==null) width = screen.availWidth * 0.7;
	if (height==null) height = screen.availHeight * 0.7;
	var X = (screen.availWidth-width)/2;
	var Y = (screen.availHeight-height)/2;
	if (scroll==null) scroll = "no";
	popupWindow = window.open(href,"","height="+height+"px,width="+width+"px,left="+X+",top="+Y+
			",status=no,toolbar=no,scrollbars="+scroll+",resizable=yes,titlebar=no,menubar=no,location=no,dependent=yes");
			
	var resize = false;
	var counter = 0;
	if (width==0)
	{
	    counter = 0;
		while (popupWindow.document.readyState != 'complete' && counter < 1000000)
		    counter++;
		
		if (popupWindow.document.readyState == 'interactive' || popupWindow.document.readyState == 'complete')
		{
	        if (popupWindow.document.body.scrollWidth < screen.availWidth * 0.9)
	            width = popupWindow.document.body.scrollWidth ;
	        else
	            width = screen.availWidth * 0.9;
    	    resize = true;
    	}
	}
	if (height==0)
	{
	    counter = 0;
		while (popupWindow.document.readyState != 'complete' && counter < 1000000)
		    counter++;
		
		if (popupWindow.document.readyState == 'interactive' || popupWindow.document.readyState == 'complete')
		{
	        if (popupWindow.document.body.scrollHeight < screen.availHeight * 0.9)
	            height = popupWindow.document.body.scrollHeight ;
	        else
	            height = screen.availHeight * 0.9;
    	    resize = true;
    	}
	}
	
	if (resize)
	{
	    X = (screen.availWidth-width)/2;
	    Y = (screen.availHeight-height)/2;
	    popupWindow.resizeTo(width, height);
	    popupWindow.moveTo(X,Y);
	}
    
	if (onepop == "no")
	    popupWindow = null;
}


function confirmupdate(msg)
{
    if (msg == null)
        msg = "Proses Submit?";
	conf = confirm(msg);
	if (conf)
		return true;
	else
		return false;
}

