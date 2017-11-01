/*########################################################
Name : ASPxMultipleFileUpload
Autor: Ing.Roman Hudec
########################################################*/

function MultipleFileUpload(multipleFileUploadId, ddlSelectPositionId, divFiles, divDescription, tableFiles, btnAdd, maxFiles, deleteButtonText, editButtonText) {
    this.ID = 0;
    this.MaxFileToUpload = maxFiles;

    this.MultipleFileUploadId = multipleFileUploadId; //server control ID
    this.ddlSelectPosition = document.getElementById(ddlSelectPositionId);
    this.DivFiles = document.getElementById(divFiles);
    this.DivDescription = document.getElementById(divDescription);
    this.TableFiles = document.getElementById(tableFiles);
    this.BtnAdd = document.getElementById(btnAdd);

    this.DeleteButtonText = deleteButtonText;
    this.EditButtonText = editButtonText;
}

MultipleFileUpload.prototype.Add = function(errorMessage) {

    var inputFileAndDescription = this.GetTopFileAndDescription();
    var selectedPosition = this.ddlSelectPosition.options[this.ddlSelectPosition.selectedIndex].value;

    if (inputFileAndDescription.inputFile == null ||
        inputFileAndDescription.inputFile.value == null ||
        inputFileAndDescription.inputFile.value.length == 0) {
        alert(errorMessage);
        return;
    }
    var newFileAndDescription = this.CreateFileAndDescription();

    this.DivFiles.insertBefore(newFileAndDescription.newInputFile, inputFileAndDescription.inputFile);

    if (this.DivDescription != null)
        this.DivDescription.insertBefore(newFileAndDescription.newInputDescription, inputFileAndDescription.inputDescription);

    if (this.MaxFileToUpload != 0 && this.GetTotalFiles() - 1 == this.MaxFileToUpload) {
        newFileAndDescription.newInputFile.disabled = true;
        if (this.DivDescription != null)
            newFileAndDescription.newInputDescription.disabled = true;
        this.ddlSelectPosition.disabled = true;
        this.BtnAdd.disabled = true;
    }

    //nastavenie ID podla nazvu a POZICIE suboru.
    inputFileAndDescription.inputFile.id = inputFileAndDescription.inputFile.name = this.MultipleFileUploadId + '$inputFile_' + selectedPosition;
    inputFileAndDescription.inputFile.style.display = 'none';
    if (this.DivDescription != null) {
        inputFileAndDescription.inputDescription.id = inputFileAndDescription.inputDescription.name = this.MultipleFileUploadId + '$inputDescription_' + selectedPosition;
        inputFileAndDescription.inputDescription.style.display = 'none';
    }

    this.CreateFileItemRow(inputFileAndDescription);

    //Enable table files
    this.DisableTableFiles(false);
}

MultipleFileUpload.prototype.CreateFileAndDescription = function() {

    var inputDescription = null;
    if (this.DivDescription != null) {
        inputDescription = document.createElement('textarea');
        inputDescription.id = inputDescription.name = this.MultipleFileUploadId + '$inputDescription_';
        inputDescription.className = "description";
    }

    var inputFile = document.createElement('input');
    inputFile.id = inputFile.name = this.MultipleFileUploadId + '$inputFile_';
    inputFile.type = 'file';
    inputFile.className = 'inpuFile';

    var fileAndDescription = new Object();
    fileAndDescription.newInputFile = inputFile;
    fileAndDescription.newInputDescription = inputDescription;


    return fileAndDescription;
}

MultipleFileUpload.prototype.CreateFileItemRow = function(inputFileAndDescription) {

    var oMfu = this;

    //Vytvorenie noveho table row pre udaje.
    var newRow = this.TableFiles.insertRow(this.TableFiles.rows.length);

    //Zistim Poziciu, Nazov suboru.
    var selectedPosition = this.ddlSelectPosition.options[this.ddlSelectPosition.selectedIndex].value;
    this.ddlSelectPosition.remove(this.ddlSelectPosition.selectedIndex)

    var Splits = inputFileAndDescription.inputFile.value.split('\\');
    var fileName = Splits[Splits.length - 1];

    //Vytvorenie linku na editaciu table row.
    var editLink = document.createElement('a');
    editLink.innerHTML = this.EditButtonText;
    editLink.innerText = this.EditButtonText;
    editLink.id = this.MultipleFileUploadId + '$editLink_' + this.ID;
    editLink.name = editLink.id;
    editLink.className = "editLink";
    editLink.href = '#';
    editLink.onclick = function() { oMfu.OnButtonEdit(this, inputFileAndDescription) }

    //Vytvorenie linku na vymazanie table row.
    var deleteLink = document.createElement('a');
    deleteLink.innerHTML = this.DeleteButtonText;
    deleteLink.innerText = this.DeleteButtonText;
    deleteLink.id = this.MultipleFileUploadId + '$deleteLink_' + this.ID;
    deleteLink.name = deleteLink.id;
    deleteLink.className = "deleteLink";
    deleteLink.href = '#';
    deleteLink.onclick = function() { oMfu.OnButtonDelete(this, inputFileAndDescription) }

    //Vytvorenie table cells.
    var td1, td2, td3, td4
    td1 = newRow.insertCell(newRow.cells.length);
    td1.innerHTML = selectedPosition;
    td1.className = "cellPosition";

    td2 = newRow.insertCell(newRow.cells.length);
    td2.innerHTML = fileName;
    td2.className = "cellFile";
    if (this.DivDescription == null)
        td2.style.width = '100%';

    if (this.DivDescription != null) {
        td3 = newRow.insertCell(newRow.cells.length);
        td3.innerHTML = inputFileAndDescription.inputDescription.value;
        td3.className = "cellDescription";
    }

    td4 = newRow.insertCell(newRow.cells.length);
    if (this.DivDescription != null) {
        td4.appendChild(editLink);
    }
    td4.appendChild(deleteLink);
    td4.className = "cellAction";

    this.ID++;
    //Nastavenie pozicie obrazku do value riadku. 
    //Pri mazani sa spetne doplni tato hodnota do DDL.
    newRow.value = selectedPosition;
    return newRow;
}

//Metóda odstráni povodný záznam z tabulky a taktiež controls-i, ktoré niesli informácie 
//a do aktuálne pridavaných controls volží pôvodné hodnoty.
MultipleFileUpload.prototype.OnButtonEdit = function(editLink, inputFileAndDescription) {
    if (editLink.disabled == true) {
        return false;
    }

    var inputFileAndDescriptionTop = this.GetTopFileAndDescription();

    //Povodnu file input vratim spat do topControls.
    var inputFileToEdit = document.getElementById(inputFileAndDescription.inputFile.id);
    inputFileToEdit.style.display = 'inline';
    //Odstranim aktualny visible fileInput
    this.DivFiles.removeChild(inputFileAndDescriptionTop.inputFile)

    if (inputFileAndDescription.inputDescription != null) {
        //Povodny text area vratim do topControls.
        var inputDescriptionToEdit = document.getElementById(inputFileAndDescription.inputDescription.id);
        inputDescriptionToEdit.style.display = 'block';
        //Ostranim aktualny visible teztarea
        this.DivDescription.removeChild(inputFileAndDescriptionTop.inputDescription)
    }

    //Odstranim info z tabulky (odstranim cely riadok tabulky)
    var row = editLink.parentNode.parentNode;
    this.TableFiles.deleteRow(row.rowIndex);

    if (this.MaxFileToUpload != 0 && this.GetTotalFiles() - 1 < this.MaxFileToUpload) {
        inputFileAndDescription.inputFile.disabled = false;
        if (inputFileAndDescription.inputDescription != null)
            inputFileAndDescription.inputDescription.disabled = false;
        this.BtnAdd.disabled = false;
        this.ddlSelectPosition.disabled = false;

        this.AppendDropDownList(row.value);
        this.SortDropDownList(this.ddlSelectPosition.id);
        this.SelectDropDownListItemByValue(this.ddlSelectPosition.id, row.value);
    }

    //Disable table files
    this.DisableTableFiles(true);

}

//Metóda odstráni povodný záznam z tabulky a taktiež controls-i, ktoré niesli informácie.
MultipleFileUpload.prototype.OnButtonDelete = function(deleteLink, inputFileAndDescription) {
    if (deleteLink.disabled == true) {
        return false;
    }

    var inputFileToRemove = document.getElementById(inputFileAndDescription.inputFile.id);
    this.DivFiles.removeChild(inputFileToRemove)

    if (inputFileAndDescription.inputDescription != null) {
        var inputDescriptionToRemove = document.getElementById(inputFileAndDescription.inputDescription.id);
        this.DivDescription.removeChild(inputDescriptionToRemove)
    }

    //Odstranim info z tabulky
    var row = deleteLink.parentNode.parentNode;
    this.TableFiles.deleteRow(row.rowIndex);
    if (this.MaxFileToUpload != 0 && this.GetTotalFiles() - 1 < this.MaxFileToUpload) {
        var inputFileAndDescription = this.GetTopFileAndDescription();
        inputFileAndDescription.inputFile.disabled = false;
        if (inputFileAndDescription.inputDescription != null)
            inputFileAndDescription.inputDescription.disabled = false;
        this.BtnAdd.disabled = false;
        this.ddlSelectPosition.disabled = false;

        this.AppendDropDownList(row.value);
        this.SortDropDownList(this.ddlSelectPosition.id);
    }
}

MultipleFileUpload.prototype.GetTopFileAndDescription = function() {

    var inputFile = document.getElementById(this.MultipleFileUploadId + '$inputFile_');
    var Inputs = this.DivFiles.getElementsByTagName('input');

    for (var n = 0; n < Inputs.length && Inputs[n].type == 'file'; ++n) {
        if (Inputs[n].style.display == 'none')
            continue;
        inputFile = Inputs[n];
        break;
    }

    var inputDescription = null;
    if (this.DivDescription != null) {
        Inputs = this.DivDescription.getElementsByTagName('textarea');
        for (var n = 0; n < Inputs.length; ++n) {
            if (Inputs[n].style.display == 'none')
                continue;
            inputDescription = Inputs[n];
            break;
        }
    }

    var fileAndDescription = new Object();
    fileAndDescription.inputFile = inputFile;
    fileAndDescription.inputDescription = inputDescription;

    return fileAndDescription;
}

MultipleFileUpload.prototype.GetTotalFiles = function() {
    var Inputs = this.DivFiles.getElementsByTagName('input');
    var counter = 0;
    for (var n = 0; n < Inputs.length && Inputs[n].type == 'file'; ++n)
        counter++;
    return counter;
}

MultipleFileUpload.prototype.DisableTop = function() {
    if (this.TableFiles.rows.length == 0)
        return false;

    var inputFileAndDescription = this.GetTopFileAndDescription();
    inputFileAndDescription.inputFile.disabled = true;
    if (inputFileAndDescription.inputDescription != null)
        inputFileAndDescription.inputDescription.disabled = true;
    this.ddlSelectPosition.disabled = true;
    return true;
}

MultipleFileUpload.prototype.AppendDropDownList = function(position) {
    var elOptNew = document.createElement('option');
    elOptNew.text = position;
    elOptNew.value = position;

    try {
        this.ddlSelectPosition.add(elOptNew, null); // standards compliant; nefunguje v IE
    }
    catch (ex) {
        this.ddlSelectPosition.add(elOptNew); // ibe pre IE
    }
}

MultipleFileUpload.prototype.SortDropDownList = function(elementId) {
    var dropDownList = document.getElementById(elementId);
    listValues = new Array();

    for (i = 0; i < dropDownList.length; i++) {
        listValues[i] = parseInt(dropDownList.options[i].text);
    }

    listValues.sort(function(a, b) { return (a - b); });
    for (i = 0; i < listValues.length; i++) {
        dropDownList.options[i].text = listValues[i];
        dropDownList.options[i].value = listValues[i];
    }
}

MultipleFileUpload.prototype.SelectDropDownListItemByValue = function(elementId, value) {
    var dropDownList = document.getElementById(elementId);

    var index = 0;
    for (i = 0; i < dropDownList.length; i++) {
        if (value != dropDownList.options[i].value)
            continue;

        index = i;
        break;
    }
    dropDownList.selectedIndex = index;
}

MultipleFileUpload.prototype.DisableElement = function(elm, bDisable) {

    if (bDisable == true) {
        elm.style.opacity = .5;
        elm.style.filter = 'alpha(opacity=50);';
        elm.disabled = bDisable;
    } else {
        elm.style.opacity = 1;
        elm.style.filter = 'alpha(opacity=100);';
        elm.disabled = bDisable;
    }
}

MultipleFileUpload.prototype.DisableTableFiles = function(bDisable) {

    if (bDisable == true) {
        this.TableFiles.style.opacity = .3;
        this.TableFiles.style.filter = 'alpha(opacity=30);';
    } else {
        this.TableFiles.style.opacity = 1;
        this.TableFiles.style.filter = 'alpha(opacity=100);';
    }

    for (var r = 0; r < this.TableFiles.rows.length; r++) {
        var row = this.TableFiles.rows[r];
        for (var c = 0; c < row.cells.length; c++) {
            var cell = row.cells[c];
            if (cell.tagName == "TH") {
                cell.className = bDisable ? 'header-disabled' : 'header';
                continue;
            }
            for (var ch = 0; ch < cell.childNodes.length && cell.childNodes[ch].tagName == "A"; ch++) {
                var link = cell.childNodes[ch];
                link.disabled = bDisable;
            }
        }
    }
}

