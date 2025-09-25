$(function () {
    $('#txtClientNumber').autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: "frmClientReview.aspx/GetClients",
                data: JSON.stringify({ prefix: request.term }), // prefix = typed value
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    response(data.d); // bind list to dropdown
                },
                error: function (xhr, status, error) {
                    console.error(error);
                }
            });
        },
        minLength: 2,  // start suggesting after 2 chars
        select: function (event, ui) {
            console.log("Selected: " + ui.item.value);
        }
    });
});
//function runScript(e) {
//    //See notes about 'which' and 'key'
//    if (e.keyCode == 13) {
//        if ($('#txtClientNumber').val() != '') {
//           // LoadPEDetails();
//        }
//        return false;
//    }
//}
//function clientChanged(input) {
//    alert("Client Number changed: " + input.value);
//    // you can call AJAX or any other function here
//}
function runScript(event) {
    if (event.key === "Enter") {
        alert("You pressed Enter. Value: " + event.target.value);
        getClientID(event.target.value);
        return false; // prevent postback if needed
    }
    // You can also handle every key press here
    return true;
}
function getClientID(value) {

        $.ajax({
            type: "POST",
            url: "frmClientReview.aspx/GetClients",
            data: JSON.stringify({ prefix: request.term }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                response(data.d); // "d" contains the returned List<string>
            }
        });

    minLength: 2 // start search after 2 characters
}



//$(function () {
//    $("#<%= txtClientNumber.ClientID %>").autocomplete({
//        source: function (request, response) {
//            $.ajax({
//                type: "POST",
//                url: "frmClientReview.aspx/GetClients",
//                data: JSON.stringify({ prefix: request.term }), // prefix = typed value
//                contentType: "application/json; charset=utf-8",
//                dataType: "json",
//                success: function (data) {
//                    response(data.d); // bind list to dropdown
//                },
//                error: function (xhr, status, error) {
//                    console.error(error);
//                }
//            });
//        },
//        minLength: 2,  // start suggesting after 2 chars
//        select: function (event, ui) {
//            console.log("Selected: " + ui.item.value);
//        }
//    });
//});