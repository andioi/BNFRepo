
function jsgetInstallment(inttype, limit, ratetahun, tenor, coeff)
{
    var installment = 0; 
    var rate = ratetahun / 1200; 
    
    if (inttype == '01' || inttype == '02')		// Add On Advance & Arrears  (Flat) 
    	installment = (limit + (limit * rate * tenor)) / tenor;
    else if (inttype == '03')				// Annuity in Advance 
    	installment = limit / (1 + ((1 - Math.pow((1 + rate),(1 - tenor))) / rate));
    else if (inttype == '04')				// Annuity in Arrears  
    	installment = limit / ((1 - Math.pow((1 + rate),(0 - tenor))) / rate);
    else if (inttype == '05')				// Simple Interest   (Flat) 
    	installment = (limit / tenor) + (limit * rate);
    else if (inttype == '06')				// Annual Rest 
    {
    	installment = limit /((1 - Math.pow((1 + (rate*12)),(0 - (tenor / 12)))) / (rate*12));
    	installment = installment / 12; 
    }
    else if (inttype == '07')				// Angsuran Berjenjang 
    	installment = (limit * rate) + (coeff/100) * (limit * rate);
    
    return installment;
}
