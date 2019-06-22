function getTot(id,idx,out,digi,dec)
{
    if (digi == null)
        digi = '.';
    if (dec == null)
        dec = ',';              //default indonesian setting
	var obj,i=0,jum=0;
	
	try
	{
		for(i=0;i<=parseInt(idx);i++)
		{	
			obj = document.getElementById(id+'_'+i).value.replace('','0');
			while(obj.indexOf(digi) != -1)
			{
				obj = obj.replace(digi,'');
			}
			jum = jum + parseFloat(obj); 
		}

		document.getElementById(out).value = jum;
		var txt = document.getElementById(out);
		currzd('', txt, digi, dec, 3);
	}catch(ex){alert(ex);}
		
}
