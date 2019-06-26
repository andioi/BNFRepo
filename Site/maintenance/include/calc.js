function clearValue(txt, digi, dec)
{
    while(txt.indexOf(' ') != -1)
        txt = txt.replace(' ','');
    while(txt.indexOf(digi) != -1)
        txt = txt.replace(digi,'');
    txt = txt.replace(dec,'.');
    if (txt == '')
        txt = '0';
    return txt;
}

function jsmoneyformat(val, digi, dec, len)
{
    var Sign = '';
    if(val < 0)
    {
        Sign = '-';
        val = val * -1;
    }
    var iNum = parseInt(val);
    var decstr = '';
    if (len > 0)
    {
        var fDec = val - iNum;
        var iDec = parseInt(fDec * Math.pow(10, len));
        var decstr = iDec.toString();
        if (decstr.length > len) decstr = decstr.substr(0, len);
        //while (decstr.length < len)
        //    decstr = decstr + '0';
        decstr = dec + decstr;
    }
    var num = iNum.toString();
    var numstr = '';
    while (num.length > 3)
    {
        numstr = digi + num.substr (num.length - 3, 3) + numstr;
        num = num.substr (0, num.length - 3);
    }
    numstr = num + numstr;
    return Sign + numstr + decstr;
}

