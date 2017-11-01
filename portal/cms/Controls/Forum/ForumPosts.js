function showControl(elmId, elmPlusId, elmMinusId) {
    var elm = document.getElementById(elmId);
    elm.style.display = 'block';
    document.getElementById(elmPlusId).style.display = 'none';
    document.getElementById(elmMinusId).style.display = 'block';
}
function hideControl(elmId, elmPlusId, elmMinusId) {
    var elm = document.getElementById(elmId);
    elm.style.display = 'none';
    document.getElementById(elmPlusId).style.display = 'block';
    document.getElementById(elmMinusId).style.display = 'none';
}