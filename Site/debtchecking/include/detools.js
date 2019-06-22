        function alphanumericonly()
        {	
	        //A : 65; Z : 90; a:97; z:122; 0:48; 9:57; spasi : 32; - : 45
	        if ((event.keyCode>=65 && event.keyCode<=90) || (event.keyCode>=97 && event.keyCode<=122) || event.keyCode>=48 && event.keyCode<=57)
	        {
		        return true;
	        } 
	        else
	        {
		        return false;
	        }	
       }
       function phonenumber()
       {	
	        //A : 65; Z : 90; a:97; z:122; 0:48; 9:57; spasi : 32; - : 45
	        if (event.keyCode>=48 && event.keyCode<=57)
	        {
		        return true;
	        } 
	        else
	        {
		        return false;
	        }	
       }
       function setFullName(first, mid, last, fullnm)
       {
            var full = Trim(Trim(Trim(first.value) + ' ' + Trim(mid.value)) + ' ' + Trim(last.value));
            if(full.length > 30)
            	full = full.substring(0, 30);
            
            fullnm.value = full;
       }
       function Trim(str)
       {
            while('' + str.charAt(0) == ' ')
                str = str.substring(1, str.length);
            while('' + str.charAt(str.length-1) == ' ')
        	    str = str.substring(0, str.length-1);
        	return str;
       }
