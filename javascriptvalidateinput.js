function Validate(event) {
    var regex = new RegExp("^[0-9-.,/]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

<input ID="txtcheck" onkeypress="return Validate(event);" />
