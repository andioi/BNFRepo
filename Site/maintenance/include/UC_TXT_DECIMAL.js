function SpecialKeyDec()
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
	if (event.keyCode == 46 || event.keyCode == 8) return false;

	if (
		(event.keyCode<28 && event.keyCode>8) 
	/*	|| (event.keyCode<46 && event.keyCode>32) 
		|| (event.keyCode==91)
		|| (event.keyCode<146 && event.keyCode>111)
	//	|| (event.keyCode<46 && event.keyCode>32)
		|| (event.keyCode<65 && event.keyCode>57)
		|| (event.keyCode<97 && event.keyCode>90)
		|| (event.keyCode<127 && event.keyCode>122)*/
	) return true;
}
function decimalformat(txt,dec)
{
	var length = txt.value.length,code;
	
	//if (SpecialKeyDec()) return true;
	
	//if user press backspace
	//if (event.keyCode==8) return;
	
	//if (event.keyCode>36 && event.keyCode<41) return;
	
	if (dec == '.')
		code = (event.keyCode!=110 && event.keyCode!=190) ;
	else if (dec == ',')
		code = (event.keyCode!=188) ;
	
	/*if ( (event.keyCode<48 || event.keyCode>57)
		&& (event.keyCode<96 || event.keyCode>105)
		&& code)
		txt.value = txt.value.substr(0,length-1);
	*/
	
	if (txt.value.indexOf(dec)!=txt.value.lastIndexOf(dec) 
	|| txt.value.indexOf(dec)==0 )
		txt.value = txt.value.substr(0,length-1);
	
	if (txt.value.indexOf(dec)!=-1)
		 if(length-txt.value.indexOf(dec) >3)
			 txt.value = txt.value.substr(0,length-1);
}
