function Vocabulary(id, baseCss, texts, requestUrl) {
    this.id = id;
    this.css = baseCss;
    if (texts != null) this.texts.popUpControl = texts.split(';');
    this.requestUrl = requestUrl;
    this.editControlId = id + "_editControl";
    this.popUpControlId = id + "_popUpControl";
    this.canHideEditControl = true;
}

Vocabulary.prototype.texts = {
    popUpControl: ["Title", "Update"]
}
//===================================================================
// EDIT CONTROL
//===================================================================
Vocabulary.prototype.showEditControl = function() {

    //Ak este nie je edit control vyrenderovany, vyrenderuj ho.
    if (!this.divEditContent)
        this.renderEditControl();

    var parentElm = document.getElementById(this.id);
    if (parentElm == null) return;

    this.term = parentElm.innerHTML.replace(/^\s+|\s+$/, '');
    document.body.appendChild(this.divEditContent);

    var offsetTop = getElementOffsetTop(parentElm, 0);
    var offsetLeft = getElementOffsetLeft(parentElm, 0);

    // IE
    if (Mothiva.isIE) {
        offsetLeft += document.documentElement.scrollLeft;
        offsetTop += document.documentElement.scrollTop;
    }
    else {
        offsetLeft += document.body.scrollLeft;
        offsetTop += document.body.scrollTop;
    }

    this.divEditContent.style.top = offsetTop - this.divEditContent.scrollHeight / 2 + "px";
    this.divEditContent.style.left = offsetLeft + parentElm.offsetWidth - (this.divEditContent.scrollWidth / 2) + "px";
    this.divEditContent.style.display = "block";
}

//Skryje popup control
Vocabulary.prototype.hideEditControl = function() {
    if (this.divEditContent != null) {
        var othis = this;
        window.setTimeout(function() {
            if (othis.canHideEditControl == true && othis.divEditContent != null) {
                document.body.removeChild(othis.divEditContent);
                othis.divEditContent = null;
            }
        }, 500);
    }
}

//Vtrenderuje editControl.
Vocabulary.prototype.renderEditControl = function() {
    var othis = this;
    this.divEditContent = document.createElement("div");
    this.divEditContent.setAttribute("id", this.editControlId);
    this.divEditContent.style.position = "absolute";
    this.divEditContent.className = this.css + '_editControl';

    this.divEditContent.onmouseover = function() { othis.canHideEditControl = false; }
    this.divEditContent.onmouseout = function() { othis.canHideEditControl = true; othis.hideEditControl(); }
    this.divEditContent.onclick = function() { othis.showPopUpControl(); }
}

//===================================================================
// POPUP CONTROL
//===================================================================
//Zobrazenie popup okna.
Vocabulary.prototype.showPopUpControl = function() {

    //Ak este nie je edit control vyrenderovany, vyrenderuj ho.
    if (!this.mainPopUpContent)
        this.renderPopUpControl();

    var parentElm = document.getElementById(this.id);
    if (parentElm == null) return;

    document.body.appendChild(this.mainPopUpContent);

    var offsetTop = getElementOffsetTop(parentElm, 0);
    var offsetLeft = getElementOffsetLeft(parentElm, 0);

    // IE
    if (Mothiva.isIE) {
        offsetLeft += document.documentElement.scrollLeft;
        offsetTop += document.documentElement.scrollTop;
    }
    else {
        offsetLeft += document.body.scrollLeft;
        offsetTop += document.body.scrollTop;
    }

    if (this.divPopUpFormContent.offsetHeight + offsetTop > document.body.offsetHeight)
        this.divPopUpFormContent.style.top = offsetTop - this.divPopUpFormContent.offsetHeight + "px";
    else
        this.divPopUpFormContent.style.top = offsetTop + parentElm.offsetHeight + "px";

    if (this.divPopUpFormContent.offsetWidth + offsetLeft > document.body.offsetWidth)
        this.divPopUpFormContent.style.left = offsetLeft - (this.divPopUpFormContent.offsetWidth - parentElm.offsetWidth) + "px";
    else
        this.divPopUpFormContent.style.left = offsetLeft + "px";

    this.divPopUpFormContent.style.display = "block";

}

//Skryje popup control
Vocabulary.prototype.hidePopUpControl = function() {
    if (this.mainPopUpContent != null) {
        document.body.removeChild(this.mainPopUpContent);
        this.mainPopUpContent = null;
        this.divPopUpFormContent = null;
    }
}

//Vyrenderuje popupControl.
Vocabulary.prototype.renderPopUpControl = function() {
    var othis = this;
    this.mainPopUpContent = document.createElement("div");
    this.mainPopUpContent.style.top = 0;
    this.mainPopUpContent.style.left = 0;
    // IE
    if (Mothiva.isIE) {
        this.mainPopUpContent.style.width = document.documentElement.scrollWidth + "px";
        this.mainPopUpContent.style.height = document.documentElement.scrollHeight + "px";
    }
    else {
        this.mainPopUpContent.style.width = document.body.scrollWidth + "px";
        this.mainPopUpContent.style.height = document.body.scrollHeight + "px";
    }
    this.mainPopUpContent.className = this.css + '_popUpContent';
    this.mainPopUpContent.style.position = "absolute";
    this.mainPopUpContent.onclick = function(e) {
        if (!e) var e = window.event;
        var targ = null;
        if (e.target) targ = e.target;
        else if (e.srcElement) targ = e.srcElement;
        if (targ.nodeType == 3) // defeat Safari bug
            targ = targ.parentNode;

        if (targ != null && targ == othis.mainPopUpContent)
            othis.hidePopUpControl();
    }

    this.divPopUpFormContent = document.createElement("div");
    this.divPopUpFormContent.setAttribute("id", this.popUpControlId);
    this.divPopUpFormContent.style.position = "absolute";
    this.divPopUpFormContent.className = this.css + '_popUpControl';

    var tbl = document.createElement("table");
    this.divPopUpFormContent.appendChild(tbl);
    tbl.width = "100%";
    tbl.height = "100%";
    tbl.border = "0";
    tbl.cellSpacing = 0;
    tbl.cellPadding = 0;

    //Header
    var th = tbl.insertRow(tbl.rows.length);
    var tdTitle = th.insertCell(th.cells.length);
    th.className = this.divPopUpFormContent.className + "_header"
    tdTitle.innerHTML = this.texts.popUpControl[0];
    tdTitle.className = th.className + "_title"

    var closeButton = document.createElement("div");
    closeButton.className = th.className + "_closeButton"
    closeButton.onclick = function() { othis.hidePopUpControl() }

    var tdClose = th.insertCell(th.cells.length);
    tdClose.align = "right";
    tdClose.appendChild(closeButton);

    //Content
    var tbody = tbl.insertRow(tbl.rows.length);
    tbody.className = this.divPopUpFormContent.className + "_content"

    var tdContent = tbody.insertCell(tbody.cells.length);
    tdContent.colSpan = 2;
    tdContent.className = tbody.className + "_inputArea";

    var inputText = document.createElement("textArea");
    inputText.value = this.term;
    inputText.className = tdContent.className + "_textBox";
    tdContent.appendChild(inputText);

    //Footer
    var tfooter = tbl.insertRow(tbl.rows.length);
    tfooter.className = this.divPopUpFormContent.className + "_footer"

    var tdButton = tfooter.insertCell(tfooter.cells.length);
    tdButton.colSpan = 2;
    tdButton.className = tfooter.className + "_inputArea";

    var inputButton = document.createElement("input");
    inputButton.type = "button";
    inputButton.value = this.texts.popUpControl[1];
    inputButton.className = tdButton.className + "_button";
    inputButton.onclick = function() { othis.commitTerm(inputText) }
    tdButton.appendChild(inputButton);

    this.mainPopUpContent.appendChild(this.divPopUpFormContent);
}

//Vtrenderuje popupControl.
Vocabulary.prototype.commitTerm = function(elm) {
    var othis = this;
    var parentElm = document.getElementById(this.id);
    if (parentElm == null) return;

    var value = elm.value.replace(/^\s+|\s+$/, '');
    var id = parentElm.id;

    var vocRequest = new VocabularyRequest(function() {
        parentElm.innerHTML = value;
        othis.hidePopUpControl();
    });

    vocRequest.sendRequest(this.requestUrl, value);
}

//===================================================================
// VOCABULARY REQUEST
//===================================================================
VocabularyRequest = function(func) {
    this.completeFunc = func;
    this.ajax = new Mothiva.Ajax();
}

VocabularyRequest.prototype.sendRequest = function(reqUrl, value) {
    if (reqUrl.length == 0) return;
    var url = reqUrl;
    var data = value;
    var senderThis = this;
    var on_succeed = this.succeed;
    var on_failed = this.failed;
    var on_processed = this.processed;
    var config = {
        sender: senderThis,
        succeed: VocabularyRequest.succeed,
        failed: VocabularyRequest.failed,
        processed: VocabularyRequest.processed
    };

    this.ajax.postData(url, data, Mothiva.Ajax.defaultCallback, config);
};

VocabularyRequest.processed = function(config, step) {
    var sender = config.sender;
    switch (step) {
        case 1: // open
            break;
        case 4: // finished
            break;
    }
}

VocabularyRequest.succeed = function(config, data) {
    var sender = config.sender;
    var data = data.trim();
    if (data == "#OK")
        sender.completeFunc(data);
}

VocabularyRequest.failed = function(config, status, text) {
    var sender = config.sender;
} 