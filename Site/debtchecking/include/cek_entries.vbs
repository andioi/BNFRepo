function FormatCurrency(obj_curr)
	SetLocale("in")
	v_curr = obj_curr.value
	v_curr = replace(v_curr, ".", "")
	if isnumeric(v_curr) then
		v_curr = formatnumber(v_curr, 2)
	else
		v_curr = 0
	end if
	obj_curr.value = v_curr
end function

function curr(str)
    set obj = eval("document." & str)
    nilai = obj.value
    nilai = replace(nilai, ".", "")
    nilai = replace(nilai, ",", "")
    if not isnumeric(nilai) then
	  obj.value = 0
	else obj.value = formatnumber(nilai, 0)
	  obj.value = replace(obj.value, ",", ".")
	end if
	curr = obj.value
end function

' Nama					: characteronly
' Fungsi				: Mengecek field pada textbox agar hanya bisa diisi karakter
' Dibuat oleh			: Tri Maryanto
' Tanggal dibuat		: 29 Agustus 2006
' Dimodifikasi oleh		: 
' Tanggal modifikasi	:
		
function characteronly(txtValue)
		set obj=eval("document." & "form1." &  txtValue)
		objReplaced = replace(obj.value,"-","")
	
		' KeyCode 48 = 0, 57 = 9, 9 = tab, 37 = left, 39 = right, 8 = backspace, 96 = 0 pada numpad, 105 = 9 pada numpad
		' Keycode 32 = -, 16 = shift, 17 = tombol ctrl, 188 = tanda koma, 190 = tanda titik
		if ((window.event.keyCode>=65 and window.event.keyCode<=90) or window.event.keyCode=32 _
		or window.event.keyCode = 8 or window.event.keyCode = 16 or window.event.keyCode = 17) then
			' Jika user menekan backspace
			if(window.event.keyCode = 8) then
				obj.value = left(obj.value,len(obj.value))
			else	
				obj.value = obj.value
			end if
		' Jika tombol keyboard yang ditekan selain huruf, spasi, tanda "-" , shift dan backspace
		else
			' Jika tombol keyboard yang ditekan adalah angka
			if((window.event.keyCode >= 48 and window.event.keyCode <=57) or (window.event.keyCode >= 96 and window.event.keyCode <= 105) _
			or window.event.keyCode = 188 or window.event.keyCode = 190 or window.event.keyCode = 219 or window.event.keyCode = 221 _
			or window.event.keyCode = 186 or window.event.keyCode = 222 or window.event.keyCode = 191) then
				obj.value = left(obj.value,len(obj.value)-1)
			else
				obj.value = obj.value
			end if
		end if
end function

' Nama					: charnumber
' Fungsi				: Mengecek field pada textbox agar hanya bisa diisi karakter dan number 
' Dibuat oleh			: Tri Maryanto
' Tanggal dibuat		: 29 Agustus 2006
' Dimodifikasi oleh		: 
' Tanggal modifikasi	:
		
function charnumber(txtValue)
		set obj=eval("document." & "form1." &  txtValue)
		objReplaced = replace(obj.value,"-","")
	
		' KeyCode 48 = 0, 57 = 9, 9 = tab, 37 = left, 39 = right, 8 = backspace, 96 = 0 pada numpad, 105 = 9 pada numpad
		' Keycode 32 = -, 16 = shift, 17 = tombol ctrl, 188 = tanda koma, 190 = tanda titik
		if ((window.event.keyCode>=65 and window.event.keyCode<=90) or (window.event.keyCode>=48 and window.event.keyCode<=57) or _
		window.event.keyCode=32 _
		or window.event.keyCode = 8 or window.event.keyCode = 16 or window.event.keyCode = 17) then
			' Jika user menekan backspace
			if(window.event.keyCode = 8) then
				obj.value = left(obj.value,len(obj.value))
			else	
				obj.value = obj.value
			end if
		
		else
			if((window.event.keyCode >=49 and window.event.keyCode <= 56) _ 
			or window.event.keyCode = 188 or window.event.keyCode = 219 or window.event.keyCode = 221 _
			or window.event.keyCode = 186 or window.event.keyCode = 222 or window.event.keyCode = 191) then
				obj.value = left(obj.value,len(obj.value)-1)
			else
				obj.value = obj.value
			end if
		end if
end function