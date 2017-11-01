//==========================================================//
// CountDownManager
//==========================================================//
function CountDownManager() {
    this.globalTickArray = new Array();
}

CountDownManager.prototype.add = function (countDown) {
    this.globalTickArray.push(countDown)
}

CountDownManager.prototype.start = function () {
    for (var i = 0; i < this.globalTickArray.length; i++) {
        var countDown = this.globalTickArray[i]
        if (countDown == null) continue;

        if (countDown.currentSeconds <= 0) {
            this.globalTickArray[i] = null;
            continue;
        }
        countDown.setCountdownText(countDown.currentSeconds - 1, countDown.onEndFunction);
    }

    var pThis = this;
    window.setTimeout(function () { pThis.start(); }, 1000);
}

//==========================================================//
// CountDown - item
//==========================================================//
//function CountDown(strContainerID, seconds, locale) {
//    this.locale = locale;
//    this.containerElm = document.getElementById(strContainerID);

//    this.containerElm = document.getElementById(strContainerID);
//    if (!this.containerElm) {
//        alert("count down error: container does not exist: " + strContainerID +
//            "\nmake sure html element with this ID exists");
//        return;
//    }

//    this.setCountdownText(seconds);
//}

//==========================================================//
// CountDown - item
//==========================================================//
function CountDown(strContainerID, seconds, locale, onEndFunction) {
    this.locale = locale;
    this.onEndFunction = onEndFunction;
    this.containerElm = document.getElementById(strContainerID);

    this.containerElm = document.getElementById(strContainerID);
    if (!this.containerElm) {
        alert("count down error: container does not exist: " + strContainerID +
            "\nmake sure html element with this ID exists");
        return;
    }

    this.setCountdownText(seconds, onEndFunction);
}

CountDown.prototype.setCountdownText = function (seconds, onEndFunction) {

    if (seconds <= 0 && onEndFunction != null) {
        onEndFunction();
    }

    this.currentSeconds = seconds;
    var minutes = parseInt(seconds / 60);
    seconds = (seconds % 60);
    var hours = parseInt(minutes / 60);
    minutes = (minutes % 60);
    var days = parseInt(hours / 24);
    if (days > 0) {
        hours = hours - (days * 24);
    }

    var strText = (days > 0 ? days.toString() + this.getDaysText(days) : "") + " " +
        (hours > 0 ? hours.toString() + this.getHoursText(hours) : "") + " " +
        (minutes > 0 ? minutes.toString() + this.getMinutesText(minutes) : "") + " " +
        seconds.toString() + this.getSecondsText(seconds);

    this.containerElm.innerHTML = strText;

    if (hours == 0 && minutes == 0 && seconds <= 60){
        this.containerElm.style.textDecoration = 'blink';
    }
}

CountDown.prototype.addZero = function (num) {
    return ((num >= 0) && (num < 10)) ? "0" + num : num + "";
}

CountDown.prototype.getDaysText = function (num) {
    if (this.locale == 'cs') {
        if (num > 4) return ' dnů';
        else return ' dny';
    }
    else if (this.locale == 'sk') {
        if (num > 4) return ' dní';
        else return ' dni';
    }
    else if (this.locale == 'pl') {
        return ' dni';
    }
    else return ' dnů';
}
CountDown.prototype.getHoursText = function (num) {
    return ' hod';
}
CountDown.prototype.getMinutesText = function (num) {
    return ' min';
}
CountDown.prototype.getSecondsText = function (num) {
    return ' sec';
}  