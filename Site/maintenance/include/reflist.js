function refupd(cid, cde, vid, vde)
{
	id = window.parent.document.getElementById(cid);
	id.value = vid;
	de = window.parent.document.getElementById(cde);
	de.value = vde;
	if (vde == '__dopostback__')
		window.parent.document.Form1.submit();
}

