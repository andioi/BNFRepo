function menupick(objmenu,objmenuitem)
{
	document.getElementById('framex').src='';
    setenabledmenu(objmenu);
	frameurl(objmenuitem.GetNavigateUrl());

    objmenu.SetSelectedItem(objmenuitem);
	objmenuitem.SetEnabled(false);
}

function setenabledmenu(objmenu)
{
    for(i=0;i<objmenu.GetItemCount();i++)
        objmenu.GetItem(i).SetEnabled(true);
}

function frameurl(url)
{
    if(document.getElementById('framex').src=='')
        document.getElementById('framex').src=url;    
}

function resizeFrame() 
{
    try{
        var oBody = document.framex.document.body;
        var oFrame = document.getElementById('framex');
        oFrame.style.width = "100%";
        var h = oBody.scrollHeight + (oBody.offsetHeight - oBody.clientHeight) + 20;
        if (h < 450)
            h = 520;
        oFrame.style.height = h;
        oFrame.style.width = oBody.scrollWidth + (oBody.offsetWidth - oBody.clientWidth);
    }
    //An error is raised if the IFrame domain != its container's domain
    catch(e)
    {
        window.status =      'Error: ' + e.number + '; ' + e.description;
    }
}
