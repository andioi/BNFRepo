
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
	if (event.keyCode == 46 || event.keyCode == 8 || event.keyCode == 9 || (event.keyCode<46 && event.keyCode>32) || event.keyCode == 109  || event.keyCode == 110 ) return true;

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

function decimalonly()
{
	if (SpecialKey()) return true;
	if ((event.keyCode<48||event.keyCode>57) && (event.keyCode<96 || event.keyCode>105) &&  (event.keyCode!=110 && event.keyCode!=190))
	{
		return false;
	}
	else 
	{
		return true;
	}
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


// Fungsi ketika tombol back diclick
// Created by		: Tri Maryanto
function back()
{	
	var hist = document.getElementById('history');
	history.go(parseInt(hist.value));	
}
