function cek_mandatory(frm, alamat)
{
	max_elm = (frm.elements.length) - 2;
	lanjut = true;
	for (var i=1; i<=max_elm; i++)
	{
		elm = frm.elements[i];
		nm_kolom = "kotak";
		if (elm.className == "mandatory" && (elm.value == "") && (elm.type == "text" || elm.type == "select-one" || elm.type == "textarea" || elm.type == "file"))
		{
			/* edited by wandy
			r = elm.parentElement.parentElement;
			d = r.cells(0).innerText;
			alert(d + " tidak boleh kosong...");
			*/
			
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
			
			lanjut = false;
			elm.focus();
			return false;
		}
	}
	if (lanjut)
	{

		if (alamat != undefined && alamat != "" )
			frm.action = alamat;

		return true;
	}
}
