var tailor = 0;
var cloth = 0;
function Edit(Id) {
    $.ajax({
        url: "/ApplicationSettings/GetSettlementSetup/" + Id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#Id').val(result.Id);
            $('#Tailor').val(result.Tailor);
            $('#ClothType').val(result.ClothType);
            $('#OwnerPercent').val(result.OwnerPercent);
            $('#PartnerPercent').val(result.PartnerPercent);
        },
        error: function (errormessage) {
            var message = errormessage.responseText;
            toastr.error(message, "Data not retrieved successfully", { showDuration: 500 })
        }
    });
}
const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
        confirmButton: 'btn btn-success',
        cancelButton: 'btn btn-danger'
    },
    buttonsStyling: false
})
function Prompt(ID) {
    Swal.fire({
        title: 'Confirmation',
        text: "Are you sure, you want to delete this?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/ApplicationSettings/DeleteSettlementSetup/" + ID,
                type: "Post",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'success',
                        title: 'Deleted successfully',
                        showConfirmButton: false,
                        timer: 1500
                    })
                    window.location.reload(true);
                },
                error: function () {
                    Swal.fire({
                        position: 'top-end',
                        icon: 'error',
                        title: 'Delete Failed',
                        showConfirmButton: false,
                        timer: 1500
                    })
                }
            })
        }
        else if (
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'Deactivation cancelled :)',
                'error'
            )
        }
    })
}
function CheckIfSettlementSetupExist() {
    var clothtype = $("#ClothTypeIDCreate").val();
    var tailor = $("#TailorIDCreate").val();
   
    $.ajax({
        url: "/ApplicationSettings/CheckIfSettlementSetupExist?clothtype=" + clothtype + "&tailor=" + tailor,
        type: "Get",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (data) {
            if (data) {

                $("#Message").html("Entry already exist !").css("color", "red");
                $("#btnSubmit").attr("disabled", true);
            }
            else {
                $("#Message").html("").css("color", "green");
                $("#btnSubmit").attr("disabled", false);
            }
        },
        error: function () {

        }
    })
};