function ValidateEmail(inputText, msg) {
    if (inputText.value != "") {
        var mailformat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
        if (!inputText.value.match(mailformat)) {
            if (msg != "") {
                alert(msg);
            }
            inputText.focus();
            return false;
        }
    }
    return true;
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function ValidatePhoneFaxNumber(inputText, msg) {
    var strPhoneNumber = inputText.value.trim();
    // For button click event start
    if (strPhoneNumber.length == 8) {
        strPhoneNumber = strPhoneNumber.substring(0, 3) + strPhoneNumber.substring(4, 8);
    }
    else if (strPhoneNumber.length == 14) {
        strPhoneNumber = strPhoneNumber.substring(1, 4) + strPhoneNumber.substring(6, 9) + strPhoneNumber.substring(10, 14);
    }
    //End

    if (strPhoneNumber != "") {
        if (!strPhoneNumber.match(/^\d+$/)) {
            alert(msg);
            inputText.focus();
            return false;
        }
        else {
            if (strPhoneNumber.length == 7) {
                strPhoneNumber = strPhoneNumber.substring(0, 3) + "-" + strPhoneNumber.substring(3, 7);
                inputText.value = strPhoneNumber;
                return true;
            }
            else if (strPhoneNumber.length == 10) {
                strPhoneNumber = "(" + strPhoneNumber.substring(0, 3) + ") " + strPhoneNumber.substring(3, 6) + "-" + strPhoneNumber.substring(6, 10);
                inputText.value = strPhoneNumber;
                return true;
            }
            else {
                alert(msg);
                inputText.focus();
                return false;
            }
        }
    }
    return true;
}

function mngPhoneFaxNumber(inputText) {
    var strPhoneNumber = inputText.value.trim();
    if (strPhoneNumber != "") {
        if (strPhoneNumber.length == 8) {
            strPhoneNumber = strPhoneNumber.substring(0, 3) + strPhoneNumber.substring(4, 8);
            inputText.value = strPhoneNumber;
        }
        else if (strPhoneNumber.length == 14) {
            strPhoneNumber = strPhoneNumber.substring(1, 4) + strPhoneNumber.substring(6, 9) + strPhoneNumber.substring(10, 14);
            inputText.value = strPhoneNumber;
        }
    }
}


