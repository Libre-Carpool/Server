function isValidPhoneNumber(str) {
    return /^05\d(-| )?\d{7}$/.test(str);
}

function isValidPassword(str) {
    return /^[a-zA-Z0-9 ]{8,24}$/.test(pass);
}

function isValidPlacesID(str) {
    return /^[a-zA-Z0-9_-]{27}$/.test(str);
}

function isValidDateString(str) {
    return /^([1-9]|(0\d)|([12]\d)|(3[01]))\/(((0\d)|([1-9]))|(1[012]))(\/\d\d\d\d)?$/.test(str);
}

function isValidTimeString(str) {
    return /^((0)|([01]\d)|(2[0-3])):([0-5]\d)$/.test(str);
}

function isValidComment(str) {
    return /^[a-zA-Z0-9\u{05D0}/u-\u{05EA}/u \-\:\,\.\(\)\!\@\#\$\%\&\*]*$/.test(str);
}
