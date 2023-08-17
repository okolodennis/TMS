
$("#FinishBtn").click(function () {
    if (CheckValidity()) {

        Swal.fire({
            title: 'Confirmation',
            text: "Are you sure, you want to proceed with this operation?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, proceed!'
        }).then((result) => {
            if (result.value) {
                const payloads = [];
                let getTotalqty;
                let BilledQty;
                let ShouldSave = true;

                var tbodys = $("#resultBody").find("tbody");
                $.each(tbodys, function (i, tbody) {
                    const paramters = [];
                    const tailorAssignments = [];
                    let clothTypeId;

                    $.each(tbody.children, function (i, tr) {
                        // Prepare result

                        clothTypeId = tr.children[0].dataset.parent;
                        let result = {
                            ParameterID: tr.children[0].dataset.child,
                            Value: tr.children[1] == null ? 0 : tr.children[1].children[0] == null ? 0 : tr.children[1].children[0].value
                        };

                        // Add to result to list
                        paramters.push(result);
                    });

                    let totalQtyID = ".totalQty-" + clothTypeId;
                    getTotalqty = parseInt($(totalQtyID).html());
                    let BilledQtyClass = ".BilledQuantity-" + clothTypeId;
                    BilledQty = parseInt($(BilledQtyClass).html());

                    if (BilledQty > getTotalqty) {
                        toastr.info("Kindly complete the cloth assignment", { showDuration: 500 });
                        ShouldSave = false;
                    }

                    let AssignTable = ".AssignTable-" + clothTypeId + " tr";
                    $(AssignTable).each(function () {
                        if (this.rowIndex > 1) {
                            let Tailor = $(this).find("td").eq(1).html();
                            let Quantity = $(this).find("td").eq(2).html();
                            let CollectionDate = $(this).find("td").eq(3).html();
                            let result = { Tailor, Quantity, CollectionDate };
                            tailorAssignments.push(result);
                        }
                    });


                    const billingid = tbody.parentElement.parentElement.parentElement.getElementsByClassName("BillingID")[0]?.value;

                    const payload = {
                        BillNumber: $("#billnumber").val(),
                        ClothTypeId: clothTypeId,
                        Parameters: paramters,
                        BillingID: billingid,
                        TailorAssignments: tailorAssignments
                    }
                    
                    if (payload.ClothTypeId != undefined) {
                        payloads.push(payload);
                    }
                });

                if (ShouldSave) {
                    //Send ajax call to server
                    $.ajax({
                        url: 'UpdateComputedMeausurement',
                        method: 'Post',
                        dataType: "json",
                        data: { results: payloads },
                        success: function (response) {
                            Swal.fire({
                                title: 'Measurement computed successfully',
                                showCancelButton: false,
                                confirmButtonText: 'Ok',
                                showLoaderOnConfirm: true,
                            }).then((result) => {
                                if (result.value) {
                                    location.href = "TakeMeasurement?Saved=true";
                                } else if (
                                    result.dismiss === Swal.DismissReason.cancel
                                ) {
                                    location.href = "TakeMeasurement?Saved=true";
                                }
                            })
                        }
                    })
                }
            }
            else if (
                result.dismiss === Swal.DismissReason.cancel
            ) {
                swalWithBootstrapButtons.fire(
                    'Cancelled',
                    'Cancelled :)',
                    'error'
                )
            }
        })
    }
})

function CheckValidity() {
    let states = [];
    var inputs = $("input");
    $.each(inputs, function (i, input) {
        if (input.value === "" && input.required) {
            input.classList.add("is-invalid");
            states.push(false);
        }
        else {
            input.classList.remove("is-invalid");
            states.push(true);
        }
    });

    return states.every(hasAllPassed);
}

function hasAllPassed(status) {
    return status ? true : false;
}
document.addEventListener("keyup change", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})

$(function () {
    $(".tailorAutoComplete").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/User/GetTailorAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: qry },
                }).done(function (res) {
                    callback(res)
                });
            }
        },
        minLength: 1
    });

});


function AssignTailor(clothTypeId) {
    let Tailor = ".Tailor-" + clothTypeId;
    let Quantity = ".Quantity-" + clothTypeId;
    let CollectionDate = ".CollectionDate-" + clothTypeId;
    let BilledQtyClass = ".BilledQuantity-" + clothTypeId;
    let totalQty = ".totalQty-" + clothTypeId;

    let tailor = $(Tailor).val();
    let collectionDate = $(CollectionDate).val();
    let quantity = parseInt($(Quantity).val());
    let totalQuantity = parseInt($(totalQty).html());
    let BilledQty = parseInt($(BilledQtyClass).html());
    let AssignBodyId = "#AssignBody-" + clothTypeId;

    if (tailor != "" && quantity != 0) {
        totalQuantity = quantity + totalQuantity;

        if (totalQuantity > BilledQty) {
            toastr.info("Cannot excceed billed quantity", { showDuration: 500 });
        }
        else {
            let html = "";
            html = "<tr id='" + clothTypeId + "' ><td><button class='btn btn-danger' onclick='Delete(this)'>Remove</button></td><td>" + tailor + "</td><td>" + quantity + "</td><td>" + collectionDate + "</td><td class='d-none'>" + clothTypeId + "</td></tr > ";
            $(AssignBodyId).append(html);
            $(totalQty).html(totalQuantity);
            $(Tailor).val("");
            $(Quantity).val(0)
        }
        if (totalQuantity == 0) {
            $(".dataTables_empty").addClass("d-block");

        }
        else {
            $(".dataTables_empty").addClass("d-none");

        }
    }
  
};

function Delete(e) {
    var qty = parseInt(e.parentElement.parentElement.children[2].innerHTML);
    var clothTypeId = e.parentElement.parentElement.children[4].innerHTML;
    let totalQtyID = ".totalQty-" + clothTypeId;
    var getTotalqty = parseInt($(totalQtyID).html());
    $(totalQtyID).html(getTotalqty - qty);

    e.parentElement.parentElement.remove();
    toastr.success("Record removed", { showDuration: 500 });
};

$(function () {
    $(".BillNumberAutoComplete").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Payment/GetBillNumberAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: qry },
                }).done(function (res) {
                    callback(res)
                });
            }
        },
        minLength: 1
    });
});