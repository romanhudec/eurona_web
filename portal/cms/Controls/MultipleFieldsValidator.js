/* 
Author: Adam Tibi
Date: 04 April 2006
Email: aztibi@gmail.com
*/

function MultipleFieldsValidatorEvaluateIsValid(val) {
 
    controltovalidateIDs = val.controlstovalidate.split(',');
    switch (val.condition) {
        case 'OR':
            for (var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                if (document.getElementById(controlID) != null) {
                    if (ValidatorTrim(ValidatorGetValue(controlID)) != '') {
                        return true;
                    }
                }
            }
            return false;
            break;
        case 'XOR':
            for(var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                if (controltovalidateIDIndex == '0') {
                    var previousResult = !(ValidatorTrim(ValidatorGetValue(controlID)) == '');
                    continue;
                }
                var currentResult = !(ValidatorTrim(ValidatorGetValue(controlID)) == '');
                if (currentResult != previousResult) {
                    return true;
                }
                previousResult != currentResult;
            }
            return false;
        break;
        case 'AND':
            for(var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                if (ValidatorTrim(ValidatorGetValue(controlID)) == '') {
                    return false;
                } 
            }
            return true;
        break;
    }
    return false;
}
