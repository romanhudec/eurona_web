function getElementOffsetTop(elem, value) {
    if (elem == null) return value;
    if (elem.tagName.toUpperCase() == "TD" && elem.style.borderTopStyle != "none") {
        var shift = parseInt(elem.style.borderTopWidth);
        if (!isNaN(shift)) {
            value += shift;
        }
    }
    return getElementOffsetTop((elem.tagName.toUpperCase() == "BODY") ? elem.parentElement : elem.offsetParent, elem.offsetTop - elem.scrollTop + value);
}

function getElementOffsetLeft(elem, value) {
    if (elem == null) return value;
    if (elem.tagName.toUpperCase() == "TD" && elem.style.borderLeftStyle != "none") {
        var shift = parseInt(elem.style.borderLeftWidth);
        if (!isNaN(shift)) {
            value += shift;
        }
    }

    return getElementOffsetLeft((elem.tagName.toUpperCase() == "BODY") ? elem.parentElement : elem.offsetParent, elem.offsetLeft - elem.scrollLeft + value);
}