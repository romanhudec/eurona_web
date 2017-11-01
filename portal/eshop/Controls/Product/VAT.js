function calculatePriceWithVAT(vatCtrl, priveCtrl, priceWVATCtrl, decimalSeparator ) {
    var elmVAT = document.getElementById(vatCtrl);
    var elmPrice = document.getElementById(priveCtrl);
    var elmPriceWVAT = document.getElementById(priceWVATCtrl);

    if (elmVAT == null) return;
    var index = elmVAT.selectedIndex;
    if (index < 0) return;
    var vatValue = parseInt(elmVAT.options[index].text.replace(',', '.'));

    if (elmPrice == null) return;
    var priveValue = parseFloat(elmPrice.value.replace(',', '.'));

    var priveWVATValue = priveValue + (priveValue * (vatValue / 100));
    if (isNaN(priveWVATValue)) elmPriceWVAT.value = '';
    else elmPriceWVAT.value = roundNumber(priveWVATValue, 2, decimalSeparator);
}

function calculatePriceWithoutVAT(vatCtrl, priveCtrl, priceWVATCtrl, decimalSeparator) {
    var elmVAT = document.getElementById(vatCtrl);
    var elmPrice = document.getElementById(priveCtrl);
    var elmPriceWVAT = document.getElementById(priceWVATCtrl);

    if (elmVAT == null) return;
    var index = elmVAT.selectedIndex;
    if (index < 0) return;
    var vatValue = parseInt(elmVAT.options[index].text.replace(',', '.'));

    if (elmPriceWVAT == null) return;
    var priveWVATValue = parseFloat(elmPriceWVAT.value.replace(',', '.'));

    var priceValue = (priveWVATValue * 100) / (100 + vatValue);
    if (isNaN(priceValue)) elmPrice.value = '';
    else elmPrice.value = roundNumber(priceValue, 2, decimalSeparator);
}

function roundNumber(num, dec, decimalSeparator) {
    var result = Math.round(num * Math.pow(10, dec)) / Math.pow(10, dec);

    result = result.toString();
    result = result.replace('.', decimalSeparator);
    result = result.replace(',', decimalSeparator);
    return result;
}