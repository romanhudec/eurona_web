function DatePicker ( daysPickerName, monthsPickerName, yearsPickerName, moreYearsPickerName ) {
	this.daysPickerName = daysPickerName;
	this.monthsPickerName = monthsPickerName;
	this.yearsPickerName = yearsPickerName;
	this.moreYearsPickerName = moreYearsPickerName;
	this.dt = new Date ();
}

//===================================================================
// DAY CALENDAR
//===================================================================
//Zobrazenie kalendara dni v mesiaci.
DatePicker.prototype.showDaysPicker = function(dt, callback) {

    if (dt) {
        this.dt = dt;
    }

    this.callback = callback;

    //Ak este nie je kalendar vyrenderovany, vyrenderuj ho.
    if (!this.divDaysContent) {
        this.renderDaysPicker();
    }

    //Nastavenie pozicie kalendara.
    var offsetLeft = getElementOffsetLeft(oDatePicker.calendarButton, 0);
    var offsetTop = getElementOffsetTop(oDatePicker.calendarButton, 0);

    // IE
    if (Mothiva.isIE) {
        offsetLeft += document.documentElement.scrollLeft;
        offsetTop += document.documentElement.scrollTop;
    }
    else {
        offsetLeft += document.body.scrollLeft;
        offsetTop += document.body.scrollTop;
    }

    this.divDaysContent.style.left = (offsetLeft - this.divContentWidth + 16) + "px";
    this.divDaysContent.style.top = (offsetTop + 16) + "px";

    this.fillDaysPicker();

    this.divDaysContent.style.display = "block";
}

//Skryje kalendar
DatePicker.prototype.hideDaysPicker = function () {
	if (this.divDaysContent) {
		this.divDaysContent.style.display = "none";
	}
}

//Vtrenderuje kalendar.
DatePicker.prototype.renderDaysPicker = function () {
	var oTR1, oTD1, oTH1;
	var oTR2, oTD2;

    this.divContentWidth = 150;

	this.divDaysContent = document.getElementById (this.daysPickerName);
	this.divDaysContent.style.position = "absolute";
			
	this.oTable1 = document.createElement ("table");
	this.divDaysContent.appendChild (this.oTable1);
	this.oTable1.style.left = 0;
	this.oTable1.style.top = 0;
	this.oTable1.width = this.divContentWidth + "px";
	this.oTable1.border = 0;
	this.oTable1.cellSpacing = 0;
	this.oTable1.cellPadding = 0;
	oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
	oTD1 = oTR1.insertCell (oTR1.cells.length);
	oTD1.colSpan = 7;
	oTD1.className = 'header';
	
	this.oTable2 = document.createElement ("table");
	this.oTable2.cellSpacing = 0;
	this.oTable2.cellPadding = 0;
	this.oTable2.width = "100%";
	oTD1.appendChild (this.oTable2);
	this.oTable2.border = 0;
	
	// New row.
	oTR2 = this.oTable2.insertRow (this.oTable2.rows.length);
	oTR2.className = 'header';
	
	// Predachadzajuci mesuiac.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.prevMonth;
	oTD2.onclick = function () { this.oDatePicker.onPrevMonth (); }
	oTD2.oDatePicker = this;
	oTD2.align = "left";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "prev.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'prevButton';
	
	// Mesiac Rok.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.monthTitle;
	oTD2.width="100%";
	oTD2.align = "center";
	this.oDcMonthYear = document.createElement ("span");
	oTD2.appendChild (this.oDcMonthYear);
	this.oDcMonthYear.oDatePicker = this;
	this.oDcMonthYear.onclick = function () { this.oDatePicker.showMonthsPicker (); }
	this.oDcMonthYear.onmouseover = function () { this.oDatePicker.makeHeaderHover (this); }
	this.oDcMonthYear.onmouseout = function () { this.oDatePicker.makeHeaderDefault (this); }
	this.oDcMonthYear.className = "headerItem";
		
	// Nasledujuci mesiac
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.nextMonth;
	oTD2.onclick = function () { this.oDatePicker.onNextMonth (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "next.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'nextButton';
	
	// Tlacidlo zatvorit.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.onclick = function () { this.oDatePicker.hideDaysPicker (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "close.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'closeButton';
	oTD2.title = this.texts.close;
	
	// Dni v tyzdni.
	oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
	for (i = 0; i < 7; i++) {
		oTH1 = document.createElement ("th");
		oTR1.appendChild (oTH1);
		oTH1.align = "center";
		oTH1.innerHTML = this.texts.days[i];
		oTH1.className = 'daysInWeek';
		
	}
	
	this.dayCells = new Array;
	for (var j = 0; j < 6; j++) {
		this.dayCells.push (new Array);
		oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
		oTR1.className = "item";
		for (i = 0; i < 7; i++) {
			this.dayCells[j][i] = oTR1.insertCell (oTR1.cells.length);
			this.dayCells[j][i].align = "center";
			this.dayCells[j][i].oDatePicker = this;
			this.dayCells[j][i].onclick = function () { this.oDatePicker.onDay (this); }
			this.dayCells[j][i].onmouseover = function () { this.oDatePicker.makeHover (this); }
			this.dayCells[j][i].onmouseout = function () { this.oDatePicker.makeDefault (this); }
		}
	}
}

DatePicker.prototype.fillDaysPicker = function () {
	
	// Vycisti kalendar
	this.clearDaysPicker ();
	
	// nastav damumy do kalendara
	var nRow = 0;
	var d = new Date (this.dt.getTime ());
	var m = d.getMonth ();
	for (d.setDate (1); d.getMonth () == m; d.setTime (d.getTime () + 86400000)) {
		var nCol = d.getDay ();
		if (nCol == 0) {
			nCol = 7;
		}
		nCol = nCol - 1;
		
		this.dayCells[nRow][nCol].className = "item";
		
		if (nCol == 5 || nCol == 6) {
		    this.dayCells[nRow][nCol].className = "weekendItem";
		}
			
		this.dayCells[nRow][nCol].innerHTML = d.getDate ();
		if (d.getDate () == this.dt.getDate ()) {
			this.dayCells[nRow][nCol].className = 'selectedItem';
		}

		if (nCol == 6) {
			nRow++;
		}
	}
	
	//Nastavenie halicky kalendara vo fomrmate "mesiac rok".
	this.oDcMonthYear.innerHTML = this.texts.months[m] + ' ' + this.dt.getFullYear ();
		
}

DatePicker.prototype.clearDaysPicker = function () {
	for (var j = 0; j < 6; j++) {
		for (var i = 0; i < 7; i++) {
			this.dayCells[j][i].innerHTML = "&nbsp;"
			this.dayCells[j][i].className = "item";
		}
	}
}

DatePicker.prototype.onPrevMonth = function () {
	if (this.dt.getMonth () == 0) {
		this.dt.setFullYear (this.dt.getFullYear () - 1);
		this.dt.setMonth (11);
	} else {
		this.dt.setMonth (this.dt.getMonth () - 1);
	}
	
	this.fillDaysPicker ();
}

DatePicker.prototype.onNextMonth = function () {
	if (this.dt.getMonth () == 11) {
		this.dt.setFullYear (this.dt.getFullYear () + 1);
		this.dt.setMonth (0);
	} else {
		this.dt.setMonth (this.dt.getMonth () + 1);
	}
	
	this.fillDaysPicker ();
}

DatePicker.prototype.onDay = function (oCell) {
	var d = parseInt (oCell.innerHTML);
	
	if (d > 0) {
		this.dt.setDate (d);
		this.hideDaysPicker ();
		this.callback (this.dt);
	}
}
//===================================================================
// MONTH CALENDAR
//===================================================================
//Zobrazenie kalendara mesiacov v roku.
DatePicker.prototype.showMonthsPicker = function () {

	if (this.divDaysContent) {
		this.hideDaysPicker ();
	}		
	// Ak este nie je kalendar vyrenderovany, vyrenderuj ho.
	if (!this.divMonthsContent) {
		this.renderMonthsPicker ();
	}

	//Nastavenie pozicie kalendara	
	var offsetLeft = getElementOffsetLeft(oDatePicker.calendarButton, 0);
	var offsetTop = getElementOffsetTop(oDatePicker.calendarButton, 0);

	// IE
	if (Mothiva.isIE) {
	    offsetLeft += document.documentElement.scrollLeft;
	    offsetTop += document.documentElement.scrollTop;
	}
	else {
	    offsetLeft += document.body.scrollLeft;
	    offsetTop += document.body.scrollTop;
	}
	
	this.divMonthsContent.style.left = ( offsetLeft - this.divContentWidth + 16 ) + "px";
	this.divMonthsContent.style.top = (offsetTop + 16) + "px";
	
	this.fillMonthsPicker ();

	this.divMonthsContent.style.display = "block";
}

//Skryje kalendar mesiacov
DatePicker.prototype.hideMonthsPicker = function () {
	if (this.divMonthsContent) {
		this.divMonthsContent.style.display = "none";
	}
}

//Vyrenderuje kalendar mesiacov.
DatePicker.prototype.renderMonthsPicker = function () {
	var oTR1, oTD1, oTH1;
	var oTR2, oTD2;

	this.divMonthsContent = document.getElementById (this.monthsPickerName);
	this.divMonthsContent.style.position = "absolute";
			
	this.oTable1 = document.createElement ("table");
	this.divMonthsContent.appendChild (this.oTable1);
	this.oTable1.style.left = 0;
	this.oTable1.style.top = 0;
	this.oTable1.width = this.divContentWidth + "px";
	this.oTable1.border = 0;
	this.oTable1.cellSpacing = 0;
	this.oTable1.cellPadding = 0;
	oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
	oTD1 = oTR1.insertCell (oTR1.cells.length);
	oTD1.colSpan = 4;
	oTD1.className = 'header';
	
	this.oTable2 = document.createElement ("table");
	this.oTable2.cellSpacing = 0;
	this.oTable2.cellPadding = 0;
	this.oTable2.width = "100%";
	oTD1.appendChild (this.oTable2);
	this.oTable2.border = 0;
	
	// New row.
	oTR2 = this.oTable2.insertRow (this.oTable2.rows.length);
	oTR2.className = 'header';
	
	// Predachadzajuci rok.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.prevMonth;
	oTD2.onclick = function () { this.oDatePicker.onPrevYear (); }
	oTD2.oDatePicker = this;
	oTD2.align = "left";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "prev.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'prevButton';
		
	// Rok.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.yearTitle;
    oTD2.width="100%";
	oTD2.align = "center";	
	this.oMcYear = document.createElement ("span");
	oTD2.appendChild (this.oMcYear);
	this.oMcYear.onclick = function () { this.oDatePicker.showYearsPicker (); }
	this.oMcYear.oDatePicker = this;
	this.oMcYear.onmouseover = function () { this.oDatePicker.makeHeaderHover (this); }
	this.oMcYear.onmouseout = function () { this.oDatePicker.makeHeaderDefault (this); }
	this.oMcYear.className = "headerItem";
	
	// Nasledujuci rok.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.nextMonth;
	oTD2.onclick = function () { this.oDatePicker.onNextYear (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "next.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'nextButton';
	
	// Tlacidlo zatvorit.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.onclick = function () { this.oDatePicker.hideMonthsPicker (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "close.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'closeButton';
	oTD2.title = this.texts.close;
		
	this.monthCells = new Array;
	for (var j = 0; j < 3; j++) {
		this.monthCells.push (new Array);
		oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
		oTR1.className = "item";
		for (i = 0; i < 4; i++) {
			this.monthCells[j][i] = oTR1.insertCell (oTR1.cells.length);
			this.monthCells[j][i].align = "center";
			this.monthCells[j][i].oDatePicker = this;
			this.monthCells[j][i].onclick = function () { this.oDatePicker.onMonth (this); }
			this.monthCells[j][i].onmouseover = function () { this.oDatePicker.makeHover (this); }
			this.monthCells[j][i].onmouseout = function () { this.oDatePicker.makeDefault (this); }
		}
	}
}

DatePicker.prototype.fillMonthsPicker = function () {
	
	// Vycisti kalendar
	this.clearMonthsPicker ();
	
	// nastav damumy do kalendara
	var nRow = 0;
	var m=1;
	
    for ( m = 1; m <= this.texts.monthsRoman.length; m++ )
    {
        var nCol = ( m - 1 ) - ( 4 * nRow );
		this.monthCells[nRow][nCol].className = "item";
		this.monthCells[nRow][nCol].innerHTML = this.texts.monthsRoman[m-1];
		
		if ( (m-1) == this.dt.getMonth () ) {
			this.monthCells[nRow][nCol].className = 'selectedItem';
		}

		if (nCol == 3) {
			nRow++;
		}
	}
		
	// set the year text
	this.oMcYear.innerHTML = this.dt.getFullYear ();
}

DatePicker.prototype.clearMonthsPicker = function () {
	for (var j = 0; j < 3; j++) {
		for (var i = 0; i < 4; i++) {
			this.monthCells[j][i].innerHTML = "&nbsp;"
			this.monthCells[j][i].className = "item";
		}
	}
}

DatePicker.prototype.onPrevYear = function () {
	this.dt.setYear (this.dt.getFullYear () - 1);
	this.fillMonthsPicker ();
}

DatePicker.prototype.onNextYear = function () {
	this.dt.setYear (this.dt.getFullYear () + 1);
	this.fillMonthsPicker ();
}

DatePicker.prototype.onMonth = function ( oCell ) {
    var d = this.getMonthNumber( oCell.innerHTML );
	if (d > 0) {
		this.dt.setMonth (d-1);
		this.hideMonthsPicker ();
		this.showDaysPicker( this.dt, callback );
	}
}

//===================================================================
// YEAR CALENDAR
//===================================================================
//Zobrazenie kalendara rokov.
DatePicker.prototype.showYearsPicker = function() {

    if (this.divMonthsContent) {
        this.hideMonthsPicker();
    }
    // Ak este nie je kalendar vyrenderovany, vyrenderuj ho.
    if (!this.divYearsContent) {
        this.renderYearsPicker();
    }

    //Nastavenie pozicie kalendara	
    var offsetLeft = getElementOffsetLeft(oDatePicker.calendarButton, 0);
    var offsetTop = getElementOffsetTop(oDatePicker.calendarButton, 0);

    // IE
    if (Mothiva.isIE) {
        offsetLeft += document.documentElement.scrollLeft;
        offsetTop += document.documentElement.scrollTop;    
    }
    else {
        offsetLeft += document.body.scrollLeft;
        offsetTop += document.body.scrollTop;    
    }

    this.divYearsContent.style.left = (offsetLeft - this.divContentWidth + 16) + "px";
    this.divYearsContent.style.top = (offsetTop + 16) + "px";

    this.fillYearsPicker();

    this.divYearsContent.style.display = "block";
}

//Skryje kalendar mesiacov.
DatePicker.prototype.hideYearsPicker = function () {
	if (this.divYearsContent) {
		this.divYearsContent.style.display = "none";
	}
}

//Vyrenderuje kalendar rokov.
DatePicker.prototype.renderYearsPicker = function () {
	var oTR1, oTD1, oTH1;
	var oTR2, oTD2;

	this.divYearsContent = document.getElementById (this.yearsPickerName);
	this.divYearsContent.style.position = "absolute";
			
	this.oTable1 = document.createElement ("table");
	this.divYearsContent.appendChild (this.oTable1);
	this.oTable1.style.left = 0;
	this.oTable1.style.top = 0;
	this.oTable1.width = this.divContentWidth + "px";
	this.oTable1.border = 0;
	this.oTable1.cellSpacing = 0;
	this.oTable1.cellPadding = 0;
	oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
	oTD1 = oTR1.insertCell (oTR1.cells.length);
	oTD1.colSpan = 4;
	oTD1.className = 'header';
	
	this.oTable2 = document.createElement ("table");
	this.oTable2.cellSpacing = 0;
	this.oTable2.cellPadding = 0;
	this.oTable2.width = "100%";
	oTD1.appendChild (this.oTable2);
	this.oTable2.border = 0;
	
	// New row.
	oTR2 = this.oTable2.insertRow (this.oTable2.rows.length);
	oTR2.className = 'header';
	
	// Predachadzajucich 10 rokov.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.prevMonth;
	oTD2.onclick = function () { this.oDatePicker.onPrev10Year (); }
	oTD2.oDatePicker = this;
	oTD2.align = "left";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "prev.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'prevButton';
		
	// Rok.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.yearTitle;
	this.oYcYear = document.createElement ("span");
	oTD2.appendChild (this.oYcYear);
	oTD2.width="100%";
	oTD2.align = "center";
	this.oYcYear.oDatePicker = this;
	this.oYcYear.onclick = function () { this.oDatePicker.showMoreYearsPicker (); }
	this.oYcYear.onmouseover = function () { this.oDatePicker.makeHeaderHover (this); }
	this.oYcYear.onmouseout = function () { this.oDatePicker.makeHeaderDefault (this); }
	this.oYcYear.className = "headerItem";    
	
	
	// Nasledujucich 10 rokov
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.nextMonth;
	oTD2.onclick = function () { this.oDatePicker.onNext10Year (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "next.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'nextButton';
	
	// Tlacidlo zatvorit.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.onclick = function () { this.oDatePicker.hideYearsPicker (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "close.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'closeButton';
	oTD2.title = this.texts.close;
		
	this.yearCells = new Array;
	for (var j = 0; j < 3; j++) {
		this.yearCells.push (new Array);
		oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
		oTR1.className = "item";
		for (i = 0; i < 3; i++) {
			this.yearCells[j][i] = oTR1.insertCell (oTR1.cells.length);
			this.yearCells[j][i].align = "center";
			this.yearCells[j][i].oDatePicker = this;
			this.yearCells[j][i].onclick = function () { this.oDatePicker.onYear (this); }
			this.yearCells[j][i].onmouseover = function () { this.oDatePicker.makeHover (this); }
			this.yearCells[j][i].onmouseout = function () { this.oDatePicker.makeDefault (this); }
		}
	}
}

DatePicker.prototype.fillYearsPicker = function () {
	
	//Vycisti kalendar.
	this.clearMonthsPicker ();
	
	//Vastav damumy do kalendara.
	var nRow = 0;
	var nIndex = 0;
	
	var beginYear = (this.dt.getFullYear() - 4);
	var endYear = (this.dt.getFullYear() + 4);
	var yearsInterval = beginYear + ' - ' + endYear;
	
    for ( var year = beginYear; year <= endYear; year++ )
    {
        var nCol = nIndex - ( 3 * nRow );
		this.yearCells[nRow][nCol].className = "item";
		this.yearCells[nRow][nCol].innerHTML = year;
		
		if ( year == this.dt.getFullYear () ) {
			this.yearCells[nRow][nCol].className = 'selectedItem';
		}

		if (nCol == 2) {
			nRow++;
		}
		
		nIndex++;
	}
		
	//Nastavenie intervalu rokov.
	this.oYcYear.innerHTML = yearsInterval;
}

DatePicker.prototype.clearYearsPicker = function () {
	for (var j = 0; j < 3; j++) {
		for (var i = 0; i < 3; i++) {
			this.yearCells[j][i].innerHTML = "&nbsp;"
			this.yearCells[j][i].className = "item";
		}
	}
}

DatePicker.prototype.onPrev10Year = function () {
	this.dt.setYear (this.dt.getFullYear () - 9);
	this.fillYearsPicker ();
}

DatePicker.prototype.onNext10Year = function () {
	this.dt.setYear (this.dt.getFullYear () + 9);
	this.fillYearsPicker ();
}

DatePicker.prototype.onYear = function ( oCell ) {
	var d = parseInt (oCell.innerHTML);
	if (d > 0) {
		this.dt.setYear (d);
		this.hideYearsPicker ();
		this.showMonthsPicker( this.dt, callback );
	}
}

//===================================================================
// MORE YEARS CALENDAR
//===================================================================
//Zobrazenie kalendara rokov rokov.
DatePicker.prototype.showMoreYearsPicker = function () {

	if (this.divYearsContent) {
		this.hideYearsPicker ();
	}		
	// Ak este nie je kalendar vyrenderovany, vyrenderuj ho.
	if (!this.divMoreYearsContent) {
		this.renderMoreYearsPicker ();
	}

	//Nastavenie pozicie kalendara	
	var offsetLeft = getElementOffsetLeft(oDatePicker.calendarButton, 0);
	var offsetTop = getElementOffsetTop(oDatePicker.calendarButton, 0);

	// IE
	if (Mothiva.isIE) {
	    offsetLeft += document.documentElement.scrollLeft;
	    offsetTop += document.documentElement.scrollTop;
	}
	else {
	    offsetLeft += document.body.scrollLeft;
	    offsetTop += document.body.scrollTop;
	}		
	
	this.divMoreYearsContent.style.left = ( offsetLeft - this.divContentWidth + 16 ) + "px";
	this.divMoreYearsContent.style.top = (offsetTop + 16) + "px";
	
	this.fillMoreYearsPicker ();

	this.divMoreYearsContent.style.display = "block";
}

//Skryje kalendar mesiacov.
DatePicker.prototype.hideMoreYearsPicker = function () {
	if (this.divMoreYearsContent) {
		this.divMoreYearsContent.style.display = "none";
	}
}

//Vyrenderuje kalendar rokov-rokov.
DatePicker.prototype.renderMoreYearsPicker = function () {
	var oTR1, oTD1, oTH1;
	var oTR2, oTD2;

	this.divMoreYearsContent = document.getElementById (this.moreYearsPickerName);
	this.divMoreYearsContent.style.position = "absolute";
			
	this.oTable1 = document.createElement ("table");
	this.divMoreYearsContent.appendChild (this.oTable1);
	this.oTable1.style.left = 0;
	this.oTable1.style.top = 0;
	this.oTable1.width = this.divContentWidth + "px";
	this.oTable1.border = 0;
	this.oTable1.cellSpacing = 0;
	this.oTable1.cellPadding = 0;
	oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
	oTD1 = oTR1.insertCell (oTR1.cells.length);
	oTD1.colSpan = 4;
	oTD1.className = 'header';
	
	this.oTable2 = document.createElement ("table");
	this.oTable2.cellSpacing = 0;
	this.oTable2.cellPadding = 0;
	this.oTable2.width = "100%";
	oTD1.appendChild (this.oTable2);
	this.oTable2.border = 0;
	
	// New row.
	oTR2 = this.oTable2.insertRow (this.oTable2.rows.length);
	oTR2.className = 'header';
	
	// Predachadzajucich 10 rokov.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.prevMonth;
	oTD2.onclick = function () { this.oDatePicker.onPrev100Year (); }
	oTD2.oDatePicker = this;
	oTD2.align = "left";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "prev.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'prevButton';
		
	// Rok.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.yearTitle;
	this.oYYcYear = document.createElement ("span");
	oTD2.appendChild (this.oYYcYear);
	oTD2.width="100%";
	oTD2.align = "center";
	this.oYYcYear.oDatePicker = this;
	this.oYYcYear.className = "headerItem";    
	
	
	// Nasledujucich 10 rokov
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.title = this.texts.nextMonth;
	oTD2.onclick = function () { this.oDatePicker.onNext100Year (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "next.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'nextButton';
	
	// Tlacidlo zatvorit.
	oTD2 = oTR2.insertCell (oTR2.cells.length);
	oTD2.onclick = function () { this.oDatePicker.hideMoreYearsPicker (); }
	oTD2.oDatePicker = this;
	oTD2.align = "right";
	oTD2.innerHTML = "<img src=\"" + oDatePicker.relativePath + "close.gif\">";
	oTD2.style.cursor = "pointer";
	oTD2.className = 'closeButton';
	oTD2.title = this.texts.close;
		
	this.moreYearCells = new Array;
	for (var j = 0; j < 3; j++) {
		this.moreYearCells.push (new Array);
		oTR1 = this.oTable1.insertRow (this.oTable1.rows.length);
		oTR1.className = "item";
		for (i = 0; i < 3; i++) {
			this.moreYearCells[j][i] = oTR1.insertCell (oTR1.cells.length);
			this.moreYearCells[j][i].align = "center";
			this.moreYearCells[j][i].oDatePicker = this;
			this.moreYearCells[j][i].onclick = function () { this.oDatePicker.on10Year (this); }
			this.moreYearCells[j][i].onmouseover = function () { this.oDatePicker.makeHover (this); }
			this.moreYearCells[j][i].onmouseout = function () { this.oDatePicker.makeDefault (this); }
		}
	}
}

DatePicker.prototype.fillMoreYearsPicker = function () {
	
	//Vycisti kalendar.
	this.clearMonthsPicker ();
	
	//Vastav damumy do kalendara.
	var nRow = 0;
	var nIndex = 0;
	
	var centerYear = this.dt.getFullYear()-4;
	
	var beginYear = (centerYear - 36);
	var endYear = (centerYear + 36);
	var yearsInterval = beginYear + ' - ' + ( endYear + 8 );
	
    for ( var year = beginYear; year <= endYear; year=year+9 )
    {
        var yearInterval = year + '<br/>' + (year+8);
        var nCol = nIndex - ( 3 * nRow );
		this.moreYearCells[nRow][nCol].className = "moreYearsItem";
		this.moreYearCells[nRow][nCol].innerHTML = yearInterval;
		
		if ( year == centerYear ) {
			this.moreYearCells[nRow][nCol].className = 'selectedItem';
		}

		if (nCol == 2) {
			nRow++;
		}
		
		nIndex++;
	}
		
	//Nastavenie intervalu rokov.
	this.oYYcYear.innerHTML = yearsInterval;
}

DatePicker.prototype.clearMoreYearsPicker = function () {
	for (var j = 0; j < 3; j++) {
		for (var i = 0; i < 3; i++) {
			this.moreYearCells[j][i].innerHTML = "&nbsp;"
			this.moreYearCells[j][i].className = "item";
		}
	}
}

DatePicker.prototype.onPrev100Year = function () {
	this.dt.setYear (this.dt.getFullYear () - 81);//9 policok po 9 rokov.
	this.fillMoreYearsPicker ();
}

DatePicker.prototype.onNext100Year = function () {
	this.dt.setYear (this.dt.getFullYear () + 81);//9 policok po 9 rokov.
	this.fillMoreYearsPicker ();
}

DatePicker.prototype.on10Year = function ( oCell ) {
	var interval = oCell.innerHTML.split();
	var beginYear = parseInt(interval[0]);
	
	//dostanem strednu hodnotu z pola 3x3 ( 4 nalavo, 1 stred, 4napravo )
	//do kalendara nastavym strednu hodnotu z vybraneho intervalu rokov.
	var d = (beginYear + 4); 
	
	if (d > 0) {
		this.dt.setYear (d);
		this.hideMoreYearsPicker ();
		this.showYearsPicker( this.dt, callback );
	}
}
//===================================================================
// GLOBAL FUNCTIONS
//===================================================================
DatePicker.prototype.makeHeaderHover = function (oCell) {
    oCell.style.cursor='pointer';
	oCell.className='headerItemHover';
    
}
DatePicker.prototype.makeHeaderDefault = function (oCell) {
    oCell.style.cursor='pointer';
	oCell.className='headerItem';
    
}
DatePicker.prototype.makeHover = function (oCell) {
    oCell.style.cursor='pointer';
}

DatePicker.prototype.makeDefault = function (oCell) {
    oCell.style.cursor='default';
}

DatePicker.prototype.onToday = function () {
	this.dt = new Date ();
	this.fillDaysPicker ();
}

DatePicker.prototype.texts = {
	months: [ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" ],
	monthsRoman : [ "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" ],
	close: "Close",
	days: ["Su", "M", "Tu", "W", "Th", "F", "Sa"],
	monthTitle: "Month",
	prevMonth: "Prev Month",
	nextMonth: "Next Month",
	yearTitle: "Year",
	today: "Today"
}

DatePicker.prototype.getMonthNumber= function ( monthRoman ) {
    
    for ( m = 0; m < this.texts.monthsRoman.length; m++ )
    {
        if( this.texts.monthsRoman[m] == monthRoman )
            return m+1;
    }
    return 0;
}

function callback (dt) {
    oDatePicker.client.value = dateTimeToString(dt, oDatePicker.dateFormat, oDatePicker.dateSeparator);
}

function getElementOffsetTop(elem, value)
{
	if (elem==null) return value;
	if (elem.tagName.toUpperCase()=="TD" && elem.style.borderTopStyle !="none")
	{
		var shift=parseInt(elem.style.borderTopWidth);
		if (!isNaN(shift))
		{
			value+=shift;
		}
	}
	return getElementOffsetTop((elem.tagName.toUpperCase() == "BODY") ? elem.parentElement : elem.offsetParent, elem.offsetTop - elem.scrollTop + value);
}

function getElementOffsetLeft( elem, value )
{
	if (elem==null)return value;
	if (elem.tagName.toUpperCase()=="TD" && elem.style.borderLeftStyle !="none")
	{
		var shift=parseInt(elem.style.borderLeftWidth);
		if (!isNaN(shift)) {
			value+=shift;
		}
	}
	
	return getElementOffsetLeft((elem.tagName.toUpperCase()=="BODY") ? elem.parentElement : elem.offsetParent, elem.offsetLeft - elem.scrollLeft+value);
}

//Zobrazenie kalendara dni v mesiaci.
function DatePickerShow ( txtId, btnId, dateFormat, dateSeparator, relpath, strMonths,strClose,strDays,strMonthTitle,strPrevMonth,strNextMonth,strYearTitle,strToday,strYesterday,strTomorrow,strWeekAgo,strWeekAfter,strMonthAgo,strMonthAfter) {
	
	if (!document.getElementById) {
		return;
	}

	oDatePicker.texts.months = strMonths.split ("-");
	oDatePicker.texts.close = strClose;
	oDatePicker.texts.days = strDays.split ("-");
	oDatePicker.texts.monthTitle = strMonthTitle;
	oDatePicker.texts.prevMonth = strPrevMonth;
	oDatePicker.texts.nextMonth = strNextMonth;
	oDatePicker.texts.yearTitle = strYearTitle;
	oDatePicker.texts.today = strToday;

	oDatePicker.dateFormat = dateFormat;
	oDatePicker.dateSeparator = dateSeparator;
		
	// since we control the text format in callback(), getting the date is easy
    var value = document.getElementById(txtId).value;

    //Parse datetime from string
    var dt = stringToDateTime(value, oDatePicker.dateFormat, oDatePicker.dateSeparator);
   	
	oDatePicker.client = document.getElementById( txtId );
	oDatePicker.calendarButton = document.getElementById( btnId );
	oDatePicker.relativePath = relpath;
	
	oDatePicker.showDaysPicker ( dt, callback );
}

// Check if the user entered a valid date
// if not, set foreground color to red
// Att. this function checks the german date format DD.MM.YYYY (short format too)
function CheckDatePickerDate (ObjectID) {
	var value = document.getElementById (ObjectID).value;
	var rcode = true;
	var bYearSet = false, bMonthSet = false, bDaySet = false;
	var Day = "", Month = "", Year = "";

	for (i = 0; i < value.length; i++) {
	    if (value.charAt(i) != "dateSeparator") {
			if (bMonthSet) {
				if (Year.length < 4) {
					Year += value.charAt (i);
					bYearSet = true;
				} else {
					rcode = false;
				}
			} else {
				if (bDaySet) {
					Month += value.charAt (i);
				} else {
					Day += value.charAt (i);
				}
			}
		} else {
			if (bYearSet) {
				rcode = false;
			}
			if (bDaySet == false) {
				bDaySet = true;
			} else {
				if (bMonthSet == false) {
					bMonthSet = true;
				}
			}
		}
	}

	if (Year.length == 0) {
		rcode = false;
	}

	Month -= 1;
	var Datum = new Date (Year, Month, Day);

	if (Datum.getYear () == Year && Datum.getMonth () == Month && Datum.getDate () == Day) {
	} else {
		rcode = false;
	}

	if (rcode) {
		document.getElementById (ObjectID).style.color = document.fgColor;
	} else {
		document.getElementById (ObjectID).style.color = "#ff0000";
	}
		
	return rcode;
}

//Format datetime to string in correct format.
function dateTimeToString(dt, dateFormat, dateSeparator) {
    
    var dateValue = "";
    var dateFormatItems = dateFormat.split(dateSeparator);

    if (dateFormatItems && (dateFormatItems.length == 3)) {
        for (var i = 0; i < dateFormatItems.length; i++) {
            if (dateFormatItems[i].charAt(0).toUpperCase() == 'D') {
                dateValue += dt.getDate();
            }
            if (dateFormatItems[i].charAt(0).toUpperCase() == 'M') {
                dateValue += (dt.getMonth() + 1);
            }

            if (dateFormatItems[i].charAt(0).toUpperCase() == 'Y') {
                dateValue += dt.getFullYear();
            }

            if (i != 2)
                dateValue += dateSeparator;
        }
    }
    return dateValue;
}

//Format string to dateTime in correct format.
function stringToDateTime(value, dateFormat, dateSeparator) {

    var dt = new Date();
    if (value == null || value == '')
        return dt;
    
    var dateItems = value.split(dateSeparator); ;
    var dateFormatItems = dateFormat.split(dateSeparator);

    if (dateFormatItems && (dateFormatItems.length == 3)) {
        for (var i = 0; i < dateFormatItems.length; i++) {
            if (dateFormatItems[i].charAt(0).toUpperCase() == 'D') {
                dt.setDate(parseInt(dateItems[i]));
            }
            if (dateFormatItems[i].charAt(0).toUpperCase() == 'M') {
                dt.setMonth(parseInt(dateItems[i]) - 1);
            }

            if (dateFormatItems[i].charAt(0).toUpperCase() == 'Y') {
                dt.setFullYear(parseInt(dateItems[i]));
            }
        }
    }
    return dt;
}
