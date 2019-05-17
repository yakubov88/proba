function Validate(event) {
    var regex = new RegExp("^[0-9-.,/]");
    var key = String.fromCharCode(event.charCode ? event.which : event.charCode);
    if (!regex.test(key)) {
        event.preventDefault();
        return false;
    }
}

<input ID="txtcheck" onkeypress="return Validate(event);" />
    
    
    
    $("#part_qty").keyup(function () {
        var filter = $(this).val();
        console.log(filter);
        $(".part_qty").each(function () {
            if ($(this).text().search(new RegExp(filter, "i")) < 0) {
                $(this).parent().fadeOut();
            } else {
                $(this).parent().show();
            }
        });
    });
