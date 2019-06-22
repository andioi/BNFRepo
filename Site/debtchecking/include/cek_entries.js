

function SpecialKey()
{
	/* keydown / keyup
	8-backspace; 9-tab; 13-Enter; 16-Shift; 17-Ctrl; 18-Alt; 19-Pause; 20-CapsLock; 27-Esc;

	33-PageUp; 34-PageDown; 35-End;	36-Home; 37-Left; 38-Up; 39-Right; 40-Down; 45-Insert; 46-Delete; 91-Win;

	F1-F12 - 112-123; 144-NumLock; ScrollLock - 145;	  
	*/

/*keypress
! 33; " 34; # 35; $ 36; % 37; & 38; ' 39;
( 40; ) 41; * 42; + 43; - 45;
: 58; ; 59; < 60; = 61; > 62; ? 63; @ 64;
[ 91; ] 93; ^ 94; _ 95; ` 96;
{ 123; | 124; } 125; ~ 126;
*/	
	// Allow Key
	if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || (event.keyCode<46 && event.keyCode>32) || event.keyCode == 109 || event.keyCode == 110 ) return true;

	// Deny Key
	if (
		(event.keyCode<28 && event.keyCode>8) 
		//|| (event.keyCode<46 && event.keyCode>32) 
		|| (event.keyCode==91)
		|| (event.keyCode<146 && event.keyCode>111)
	//	|| (event.keyCode<46 && event.keyCode>32)
		|| (event.keyCode<65 && event.keyCode>57)
		|| (event.keyCode<96 && event.keyCode>90) //
		|| (event.keyCode<127 && event.keyCode>122)
	) return false;
}
function numbersonly()
{
	if (SpecialKey()) return true;
	if ((event.keyCode<48||event.keyCode>57) && (event.keyCode<96 || event.keyCode>105))
	{
		return false;
	} else
	{
		return true;
	}	
}
function decimalonly()
{
	if (SpecialKey()) return true;
	if ((event.keyCode<48||event.keyCode>57) && (event.keyCode<96 || event.keyCode>105) &&  (event.keyCode!=110 && event.keyCode!=188))
	{
		return false;
	}
	else 
	{
		return true;
	}
}

function charsonly()
{	
	//A : 65; Z : 90; a:97; z:122; 0:48; 9:57; spasi : 32; - : 45
	if ((event.keyCode>=65 && event.keyCode<=90) || (event.keyCode>=97 && event.keyCode<=122) || event.keyCode==32)
	{
		return true;
	} else
	{
		return false;
	}	
}

function digitsonly()
{
    if ((event.keyCode > 47 && event.keyCode < 58) || (event.keyCode == 44) || (event.keyCode == 45) || (event.keyCode == 46))
	{
		return true;
	} else
	{
		return false;
	}	
}

function digitsonly2()
{
	if ((event.keyCode>47&&event.keyCode<58 ) || (event.keyCode == 46 ))
	{
		return true;
	} else
	{
		return false;
	}	
}

function digitsonly3()
{
	if ((event.keyCode == 46 ) || (event.keyCode == 35) || (event.keyCode == 39))
	{
		return false;
	} else
	{
		return true;
	}	
}

function kutip_satu()
{
	if ((event.keyCode == 35) || (event.keyCode == 39))
	{
		return false;
	} else
	{
		return true;
	}	
}

function date_isfuture(d, m, y)
{
    if (d.substring(0,1) == '0')
        d = d.substring(1,2);
    
    var today = new Date();
    var now = today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate();     //Date::getMonth starts at 0 
    var day = 0;

    if (d != '' && m != '' && y != '')                                                          // blank means present
    {
        day = parseInt(y) * 10000 + parseInt(m) * 100 + parseInt(d);
        if (day > now)
            return true;
    }
    return false;
}

function date_ispresent(d, m, y)
{
    if (d.substring(0,1) == '0')
        d = d.substring(1,2);
    
    var today = new Date();
    var now = today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate();     //Date::getMonth starts at 0 
    var day = 0;

    if (d != '' && m != '' && y != '')                                                          // blank means present
    {
        day = parseInt(y) * 10000 + parseInt(m) * 100 + parseInt(d);
        if (day == now)
            return true;
    }
    else
        return true;                                                                            // blank means present
    
    return false;
}

function date_ispast(d, m, y)
{
    if (d.substring(0,1) == '0')
        d = d.substring(1,2);
    
    var today = new Date();
    var now = today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate();     //Date::getMonth starts at 0 
    var day = 0;

    if (d != '' && m != '' && y != '')                                                          // blank means present
    {
        day = parseInt(y) * 10000 + parseInt(m) * 100 + parseInt(d);
        if (day < now)
            return true;
    }
    return false;
}

function date_compare(d1, m1, y1, d2, m2, y2)
{
    if (d1.substring(0,1) == '0')
        d1 = d1.substring(1,2);
    if (d2.substring(0,1) == '0')
        d2 = d2.substring(1,2);
    
    var today = new Date();
    var date1 = today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate();     //Date::getMonth starts at 0 
    var date2 = today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate();     //Date::getMonth starts at 0 

    if (d1 != '' && m1 != '' && y1 != '')                                                          // blank means present
        date1 = parseInt(y1) * 10000 + parseInt(m1) * 100 + parseInt(d1);
    if (d2 != '' && m2 != '' && y2 != '')                                                          // blank means present
        date2 = parseInt(y2) * 10000 + parseInt(m2) * 100 + parseInt(d2);
        
    if (date1 > date2)
        return 1;
    else if (date1 < date2)
        return -1;
    
    return 0;
}

function date_add(datepart, number, mydate){
  if (!datepart || number == 0) return mydate;
  switch (datepart.toLowerCase()){
    case "millisecond": case "ms":
      mydate.setMilliseconds(mydate.getMilliseconds() + number);
      break;
    case "second": case "ss": case "s":
      mydate.setSeconds(mydate.getSeconds() + number);
      break;
    case "minute": case "mi": case "n":
      mydate.setMinutes(mydate.getMinutes() + number);
      break;
    case "hour": case "hh":
      mydate.setHours(mydate.getHours() + number);
      break;
    case "week": case "wk": case "ww":
      number = number * 7;
      mydate.setDate(mydate.getDate() + number);
      break;
    case "day": case "dd": case "d":
      mydate.setDate(mydate.getDate() + number);
      break;
    case "month": case "mm": case "m":
      mydate.setMonth(mydate.getMonth() + number);
      break;
    case "year": case "yy": case "yyyy":
      mydate.setFullYear(mydate.getFullYear() + number);
      break;
  }
  return mydate;
}

function confirmupdate(msg)
{
    if (msg == null)
        msg = "Are you sure you want to Submit?";
	conf = confirm(msg);
	if (conf)
		return true;
	else
		return false;
}
