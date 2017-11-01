//if (typeof ("window.Mothiva") == undefined) window.alert("'mothiva.ajax.js' required 'mothiva.js'!");

Mothiva.Ajax = function() {
}

Mothiva.Ajax.prototype.getXHR = function() {
    var x = null;
    if (window.XMLHttpRequest) {
        // if IE7, Mozilla, Safari, etc: Use native object
        x = new XMLHttpRequest()
    }
    else
        if (window.ActiveXObject) {
        // ...otherwise, use the ActiveX control for IE5.x and IE6
        try {
            x = new ActiveXObject("Msxml2.XMLHTTP");
        }
        catch (e) {
        }
        if (x == null)
            try {
            x = new ActiveXObject("Microsoft.XMLHTTP");
        }
        catch (e) {
        }
    }
    return (x);
}

Mothiva.Ajax.prototype.postData = function(url, data, func, arg) {
    if (func == null) return;
    var x = this.getXHR();
    function callback() {
        func(x, arg);
    }
    var cb = callback;
    // enable cookieless sessions
    var cs = document.location.href.match(/\/\(.*\)\//);
    if (cs != null) {
        url = url.split('/');
        url[3] += cs[0].substr(0, cs[0].length - 1);
        url = url.join('/');
    }
    x.open("POST", url, (func != null));
    x.onreadystatechange = cb;
    x.send(data);
    return x;
}

Mothiva.Ajax.prototype.getData = function(url, data, func, arg) {
    if (func == null) return;
    var x = this.getXHR();
    function callback() {
        func(x, arg);
    }
    var cb = callback;
    // enable cookieless sessions
    var cs = document.location.href.match(/\/\(.*\)\//);
    if (cs != null) {
        url = url.split('/');
        url[3] += cs[0].substr(0, cs[0].length - 1);
        url = url.join('/');
    }
    x.open("GET", url, (func != null));
    x.onreadystatechange = cb;
    x.send(data);
    return x;
}

// config.succeed(data)
// config.failed(status, text)
// conffig.processed(step)
//	step = 0 // unintialized
//	step = 1 // open
//	step = 2 // sent
//	step = 3 // receiving
//	step = 4 // finished
//	step = 
Mothiva.Ajax.defaultCallback = function(xhr, config) {
    if (xhr != null && config) {
        switch (xhr.readyState) {
            default: // 0, 1, 2 & 3
                if (config.processed) config.processed(config, xhr.readyState);
                break;
            case 4: // finished
                if (config.processed) config.processed(config, xhr.readyState);
                if (xhr.status == 200) {
                    if (config.succeed) config.succeed(config, xhr.responseText);
                }
                else {
                    if (config.failed) config.failed(config, xhr.status, xhr.statusText);
                }
                break;
        }
    }
}
