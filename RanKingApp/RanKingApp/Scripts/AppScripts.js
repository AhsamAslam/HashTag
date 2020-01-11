var url1 = window.location.href.split('/');
//var baseUrl = url1[0] + '//' + url1[2] + '/' + url1[3];
var baseUrl = url1[0] + '//' + url1[2];
function DeleteBonus(obj) {
    let c = confirm("Are you sure you want to delete it?");
    if (c) {

        var id = $(obj).parent().find("input").val().split('!')[0];
        var image = $(obj).parent().find("input").val().split('!')[1];

        //$("loader").show();
        $.ajax({
            data: { id: id, image: image },
            url: baseUrl + "/Bonuses/DeleteBonus",
            type: "POST",
            success: function (r) {
                if (r == "success") {
                    $(obj).parent().parent().remove();
                    toastr.success("Bonus-Delete-Success");
                }
                else {
                    toastr.error("some error occcured!!");
                }
                //$("loader").hide();
            },
            error: function () {
                // $("loader").hide();
                toastr.error("Bonus-Delete-Failed");
            }

        });
    }
}