function $import(path) {
    var i, base, src = "default.js", scripts = document.getElementsByTagName("script");
    for (i = 0; i < scripts.length; i++) {
        if (scripts[i].src.match(src)) {
            base = scripts[i].src.replace(src, "");
            break;
        }
    }
    document.write("<" + "script src=\"" + base + path + "\"></" + "script>");
}

if (window.Mothiva == null || window.Mothiva == undefined) {

	var Mothiva = {
	
		version: "1.0",
		
		version_monor: 1,
		
		version_major: 0,
		
		user_agent: navigator.userAgent.toLowerCase(),
		
		isStrict: document.compatMode == "CSS1Compat",
		
		isOpera: navigator.userAgent.toLowerCase().indexOf("opera") > -1,
		
		isSafari: (/webkit|khtml/).test(navigator.userAgent.toLowerCase()),
		
		isIE: navigator.userAgent.toLowerCase().indexOf("opera") == -1 && navigator.userAgent.toLowerCase().indexOf("msie") > -1,
		
		isIE7: navigator.userAgent.toLowerCase().indexOf("opera") == -1 && navigator.userAgent.toLowerCase().indexOf("msie 7") > -1,
		
		isGecko: !((/webkit|khtml/).test(navigator.userAgent.toLowerCase())) && navigator.userAgent.toLowerCase().indexOf("gecko") > -1,
		
		isWindows: (navigator.userAgent.toLowerCase().indexOf("windows") != -1 || navigator.userAgent.toLowerCase().indexOf("win32") != -1),
		
		isMac: (navigator.userAgent.toLowerCase().indexOf("macintosh") != -1 || navigator.userAgent.toLowerCase().indexOf("mac os x") != -1),
		
		isLinux: (navigator.userAgent.toLowerCase().indexOf("linux") != -1),
		
		isSecure: window.location.href.toLowerCase().indexOf("https") === 0,
		
		el: function(id)
		{
			return document.getElementById(id);
		},
		
		wr: function(t)
		{
		
			document.write(t);
			if (arguments.length > 1) 
				for (var i = 1; i < arguments.length; i++) 
					document.write(arguments[i]);
		},
		
		execute: function(code)
		{
			if (window.execScript) 
				window.execScript(code);
			else 
				if (Mothiva.isSafari) 
					// safari doesn't provide a synchronous global eval
					window.setTimeout(code, 0);
				else 
					eval(code);
		},
		
		update: function(node, data)
		{
			node.innerHTML = data;
			if (arguments.length > 2 && arguments[2]) {
				var st = node.getElementsByTagName('SCRIPT');
				for (var i = 0; i < st.length; i++) {
					var code = "";
					if (Mothiva.isSafari) {
						code = st[i].innerHTML;
					}
					else if (Mothiva.isOpera) {
						code = st[i].text;
					}
					else if (Mothiva.isGecko) {
						code = st[i].textContent;
					} else {
						code = st[i].text;
					}
					Mothiva.execute(code);
				}
			}
			
		}
	};
	
	String.prototype.trim = function()
	{
		return this.replace(/^\s+|\s+$/g, "");
	}
	String.prototype.ltrim = function()
	{
		return this.replace(/^\s+/, "");
	}
	String.prototype.rtrim = function()
	{
		return this.replace(/\s+$/, "");
	}
	
	/*
	 String.prototype.trim = function()
	 {
	 var re = /^\s+|\s+$/g;
	 return this.replace(re, "");
	 //return function() { this.replace(re, ""); }
	 }
	 */
}


// MOVED FROM default.master //

function startLotosAnimation() {
    runClock();
    setInterval('runClock()', 600000); /*Kazdych 10minut*/
    //setInterval('runAnimation()', 200); /*100 ms*/
}

function runClock() {
    var cDate = new Date();
    var elm = document.getElementById('lotosImage');

    var imageIndex = 0;
    if (cDate.getHours() < 7) imageIndex = 0;
    else if (cDate.getHours() >= 19 && cDate.getMinutes() > 0) imageIndex = 0;
    else {
        //+624 obrazkov, Interval 07:00 - 13:00
        //-624 obrazkov, Interval 13:00 - 18:59

        var startMinute = 420; //7 hodina rano
        var endMinute = 779//12:59 hodina vecer
        var delta = endMinute - startMinute; //359 minut

        var current = cDate.getHours() * 60 + cDate.getMinutes();

        imageIndex = parseInt(((current - startMinute) * 624) / delta);
        if (cDate.getHours() >= 13 && cDate.getMinutes() > 0)
            imageIndex = 624 - (imageIndex - 624);
    }
    currentImageIndex = imageIndex;
    setLotosImage(elm, currentImageIndex.toString());
}

function runAnimation() {
    var maximum = 20;
    if (currentImageIndex < currentAnimateIndex) return;
    var elm = document.getElementById('lotosImage');
    var index = currentImageIndex - currentAnimateIndex;
    setLotosImage(elm, index.toString());

    if (animationType == 1) {
        currentAnimateIndex = currentAnimateIndex + 2;
    } else if (animationType == 2) {
        currentAnimateIndex = currentAnimateIndex - 2;
    }
    var max = currentImageIndex;
    if (max > maximum) max = maximum;

    if (currentAnimateIndex >= max || index >= 623) animationType = 2;
    if (currentAnimateIndex <= 0 || index <= 0) animationType = 1;
}

function setLotosImage(el, index) {
    var src = '/images/lotos/lotos_000000'; //el.src;
    var pos = src.indexOf("lotos_") + 6/*lenght("lotos_")*/;
    src = src.substring(0, pos + (5 - index.length)) + index + ".png";
    el.src = src;
    el.style.display = 'block';
}

function ExpandCollapseGridViewDetail(image, index, colspan)
 {
    // get the source of the image
    var src = image.getAttribute("src");
    // if src is currently "icon-expand.png", then toggle it to "icon-collapse.png"
    if(src.indexOf("plus")>0)
    {
         //  toggle the  image
        image.src = src.replace("plus","minus");
        // Get a reference to the current row where the image is
        var tr = image.parentNode.parentNode;
        // Get a reference to the next row from where the image is
        var next_tr = tr.nextSibling;
        // Get a refernece to the <tbody> tag of the grid
        var tbody = tr.parentNode;
         // Get a reference to the image's column
        var td = image.parentNode;
        var detailnode
         //loop through the content of the image's column. if hidden div is found, get a reference
        for(var j =0; j<td.childNodes.length; j++)
        {     
            if(td.childNodes[j].nodeType == 1)
            {
                if(td.childNodes[j].nodeName.toLowerCase() == 'div')
                {
                     detailnode = td.childNodes[j].cloneNode(true);
                     detailnode.setAttribute('style','');
                }
            }
        }

        // Create a new table row for "Expand"  
        var newtr = document.createElement('tr');
        var newtd = document.createElement('td');
        var newfirsttd = newtd.cloneNode(true);

        newfirsttd.innerHTML='&nbsp;';
        newtr.appendChild(newfirsttd);

        newtd.colSpan = colspan;
        // insert the  hidden div's content  into the new row
        newtd.innerHTML = detailnode.innerHTML;
        newtr.appendChild(newtd);
        tbody.insertBefore(newtr, next_tr);
        //tr.className = "selected";
    }
    else{
        image.src = src.replace("minus","plus");
        var row = image.parentNode.parentNode;
        var rowsibiling = row.nextSibling;
        var rowparent = row.parentNode;
        rowparent.removeChild(rowsibiling);       
        if(index%2==0)
        {
            row.className = "dataGrid_rowStyle";
        }
        else{
            row.className = "dataGrid_alternatingRowStyle";
        }
    }
}

function blockUIProcessing(message) {
    $.blockUI({
        message: "<h3>" + message + "</h3><p>",
        overlayCSS: { backgroundColor: '#333' },
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#fff',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            color: '#EA008A'
        }
    });
}
function blockUIAlert(title, message) {
    document.body.style.cursor = 'default';
    $.blockUI({
        message: "<h2 style='color:black;'>" + title + "</h2><h3>" + message + "</h3><p> <input  onclick='$.unblockUI();' type='button' value='OK' class='button' id='block-ui-dialog-btn' style='padding:5px;width:80px;height:30px;'></p>",
        overlayCSS: { backgroundColor: '#333' },
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#fff',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: 1,
            color: '#EA008A'
        }
    });
}