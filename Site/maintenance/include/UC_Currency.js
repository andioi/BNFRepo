function isDec()
{
	if (SpecialKey()) return true;
	if ((event.keyCode<48||event.keyCode>57) && (event.keyCode<96 || event.keyCode>105) && (event.keyCode!=110 && event.keyCode!=190 && event.keyCode!=189))        //change 190 '.' to 188 ',' for id-ID
	{
		return false;
	}
	else 
	{
		return true;
	}
}

function decFormat(val, digi, dec, len)
{
    var iNum = parseInt(val);
    var decstr = '';
    if (len > 0)
    {
        var fDec = val - iNum;
        var iDec = parseInt(fDec * Math.pow(10, len));
        if (iDec > 0)
        {
            var decstr = iDec.toString();
            if (decstr.length > len) decstr = decstr.substr(0, len);
            //while (decstr.length < len)
            //    decstr = decstr + '0';
            decstr = dec + decstr;
        }
    }
    var num = iNum.toString();
    var numstr = '';
    while (num.length > 3)
    {
        numstr = digi + num.substr (num.length - 3, 3) + numstr;
        num = num.substr (0, num.length - 3);
    }
    numstr = num + numstr;
    return numstr + decstr;
}

function currzd(sign,txt,ribuan,cent,segment)
{
	if (SpecialKey() && event.keyCode != 8) return;
	
    if (txt.value == '-')
        return;
    var Sign = '';
    if(txt.value.substr(0,1) == '-')
    {
        Sign = '-';
        txt.value = txt.value.substr(1);
    }
    if(txt.value == '')
    {
        txt.value = Sign;
        return;
    }
    var fValue = parseFloat(clearValue(txt.value, ribuan, cent));
    var iStr = decFormat(fValue, ribuan, cent, 0);                 // formated integer 
    var decstr = '';
    var idx = txt.value.indexOf(cent);
    if (idx > 0)
    {
        decstr = txt.value.substr(idx);
        if (decstr.length > segment)
            decstr = decstr.substr(0, segment);
    }
    txt.value = Sign + iStr + decstr;
}

