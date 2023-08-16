﻿$("input[name='CollectionType']").change(function () {
    if ($("input[name='CollectionType']:checked").val() == "BILLED") {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").show();
    }
    else if ($("input[name='CollectionType']:checked").val() == "UNBILLED") {
        $("#CustomerIDDiv").show();
        $("#InvoiceDiv").hide();
    } else {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").hide();
    }
});
$("#PaymentType").change(function () {
    var selected = $(this).val();

    if (selected == 1) {
        $("#ReferenceNumber").removeAttr("required");
        $("#refDiv").hide();
    } else {
        $("#ReferenceNumber").attr("required", true);
        $("#refDiv").show();
    }
})
$("input[name='CollectionType']").change(function () {

    if ($("input[name='CollectionType']:checked").val() == "WALK_IN") {
        let CustomerNameField = "<input type='text' class='form-control' name='CustomerName'  placeholder='Enter name'/>";
        let CustomerAgeField = "<input type='number' class='form-control' name='CustomerAge'  placeholder='Enter age' />";
        let CustomerPhoneNumberField = "<input type='text' class='form-control' name='CustomerPhoneNumber' placeholder='Enter phone number' />";
        let CustomerGender = "<select class='form-control' name='CustomerGender' id='genderFll'><option value='Male'>Male</option><option value='Female'>Female</option></select>"
        $("#Customername").html(CustomerNameField);
        $("#Customerage").html(CustomerAgeField);
        $("#Customerphonenumber").html(CustomerPhoneNumberField);
        $("#Customergender").html(CustomerGender);
        $("#searchform").hide();
        $("#ServiceBody").empty();
        $("#WaiveAmount").html("₦0.00");
        $("#BalanceAmount").html("₦0.00");
        $("#NetAmount").html("₦0.00");
        $("#InstallmentDiv").hide();

        updateNetAmount();
    }
    else {
        $("#Customername").empty();
        $("#Customerage").empty();
        $("#Customerphonenumber").empty();
        $("#Customergender").empty();
        $("#searchform").show();
        $("#ServiceBody").empty();
        $("#WaiveAmount").html("₦0.00");
        $("#BalanceAmount").html("₦0.00");
        $("#NetAmount").html("₦0.00");
        $("#InstallmentDiv").hide();

        updateNetAmount();
    }
})
$(window).ready(function () {
    // Set  Customer Type
   
    var radios = $(".Status");
    $.each(radios, function (i, radio) {
        if (radio.id == "BILLED") {
            radio.checked = true;
        }
        else {
            radio.checked = false;
        }
    })

   

    if ($("input[name='CollectionType']:checked").val() == "BILLED") {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").show();
        $("#PaymentModeDiv").show();
    }
    else if ($("input[name='CollectionType']:checked").val() == "UNBILLED") {
        $("#CustomerIDDiv").show();
        $("#InvoiceDiv").hide();
        $("#PaymentModeDiv").hide();
    } else {
        $("#CustomerIDDiv").hide();
        $("#InvoiceDiv").hide();
        $("#PaymentModeDiv").hide();
    }
});


$("#FinalPayment").click(function (e) {
    if ($("#FinalPayment").is(":checked")) {
        $("#Deposit").prop("checked", false);
    }
    $("#AmountToDepositDiv").hide();
    $("#FinishBtn").html("Pay Now")
});

$("#Deposit").click(function (e) {
    if ($("#Deposit").is(":checked")) {
        $("#FinalPayment").prop("checked", false);
    }
    $("#AmountToDepositDiv").show();
    $("#FinishBtn").html("Deposit")
});

$("#SearchCustomer").click(function (e) {
    e.preventDefault();
    e.stopPropagation();

    $("#FinishBtn").prop("disabled", false);

    let searchby = $("input[name='CollectionType']:checked").val();
    if (searchby == "UNBILLED") {
        var username = $("#CustomerUniqueID").val();
        if (username === "") {
            $("#CustomerUniqueID").addClass("is-invalid");
        } else {
            $("#CustomerUniqueID").removeClass("is-invalid");
            e.target.innerHTML = "Searching..."
            $("#ServiceBody").empty();
            $("#WaiveAmount").html("₦0.00");
            $("#BalanceAmount").html("₦0.00");
            $("#NetAmount").html("₦0.00");
            $("#customerInfoLoader").show();
            $("#ServiceTableLoader").show();
            $("#serviceTableDiv").hide();
            $("#customerinfoDiv").hide();
            $("#Customername").empty();
            $("#Customergender").empty();
            $("#Customerphonenumber").empty();
            $("#Customerage").empty();
            $("#ServiceBody").empty();
            $("#installmentdrp").empty();
            $("#InstallmentDiv").hide();


            $.ajax({
                url: 'GetCustomerByUsername?username=' + username,
                method: "Get",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#Customername").html(response.Firstname + " " + response.Lastname);
                    $("#Customergender").html(response.Gender);
                    $("#Customerphonenumber").html(response.PhoneNumber);
                    $("#CustomerUniqueID").val(response.CustomerUniqueID);

                    // Calcualte age
                    let customerDOBYear = new Date(+response.DOB.replace(/\D/g, '')).getFullYear();
                    let currentYear = new Date().getFullYear();
                    let customerAge = parseInt(currentYear - customerDOBYear);
                    $("#Customerage").html(customerAge);

                    $("#customerInfoLoader").hide();
                    $("#customerinfoDiv").show();
                    $("#ServiceTableLoader").hide();
                    $("#serviceTableDiv").show();
                    e.target.innerHTML = "Search"
                },
                error: function (err) {
                    toastr.error(err.responseText, "An Error Occurred", { showDuration: 500 })
                    e.target.innerHTML = "Search";
                    $("#customerInfoLoader").hide();
                    $("#customerinfoDiv").show();
                    $("#ServiceTableLoader").hide();
                    $("#serviceTableDiv").show();
                }
            })

        }
    } else {
        var invoiceNumber = $("#BillInvoiceNumber").val();
        if (invoiceNumber === "") {
            $("#BillInvoiceNumber").addClass("is-invalid");
        }
        else {
            $("#BillInvoiceNumber").removeClass("is-invalid");
            e.target.innerHTML = "Searching..."

            $("#ServiceBody").empty();
            $("#WaiveAmount").html("₦0.00");
            $("#BalanceAmount").html("₦0.00");
            $("#NetAmount").html("₦0.00");
            $("#customerInfoLoader").show();
            $("#ServiceTableLoader").show();
            $("#serviceTableDiv").hide();
            $("#customerinfoDiv").hide();
            $("#Customername").empty();
            $("#Customergender").empty();
            $("#Customerphonenumber").empty();
            $("#Customerage").empty();
            $("#ServiceBody").empty();
            $("#installmentdrp").empty();
            $("#InstallmentDiv").hide();

            $.ajax({
                url: 'GetCustomerByInvoiceNumber?invoiceNumber=' + invoiceNumber,
                method: "Get",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (response) {
                    $("#Customername").html(response.CustomerName);
                    $("#Customergender").html(response.CustomerGender);
                    $("#Customerphonenumber").html(response.CustomerPhoneNumber);
                    $("#Customerage").html(response.CustomerAge);

                    $("#customerInfoLoader").hide();
                    $("#customerinfoDiv").show();

                    // Get Services
                    $.ajax({
                        url: 'GetServicesByInvoiceNumber?invoiceNumber=' + invoiceNumber,
                        method: "Get",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (datas) {
                            $("#ServiceBody").empty()
                            $.each(datas, function (i, data) {
                                let html = "";
                                html = "<tr id='" + data.Id + "' ><td><button class='btn btn-danger' disabled onclick='Delete(this)'>Remove</button></td><td>" + data.ServiceName + "</td><td><input type='number' disabled value=" + data.Quantity + " class='form-control quantity-" + data.Id + "' onchange='UpdateAmount(this)' onkeyup='UpdateAmount(this)' /></td><td class='sellingprice-" + data.Id + "' data-id='" + data.SellingPrice + "'>" + data.SellingPriceString + "</td><td><strong class='gross-" + data.Id + " gross'>₦00.00</strong></td></tr>";
                                $("#ServiceBody").append(html);
                                $("#ServiceName").val("");
                                CalculateGrossAmount(data.Quantity, data.SellingPrice, data.Id);
                            });
                            updateNetAmount();
                            var netamount = $("#NetAmount").html();
                            var waiveamount = $("#WaiveAmount").html();
                            var paidamount = $("#PaidAmount").html();
                            var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - ConvertToDecimal(paidamount);


                            $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
                        },
                        error: function (err) {
                            toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 });
                        }
                    });

                    // Get Waived Amount
                    $.ajax({
                        url: 'GetWaivedAmountsForInvoiceNumber?invoiceNumber=' + invoiceNumber,
                        method: "Get",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (data) {
                            if (data.Id != 0) {
                                $("#WaiveAmount").html("₦" + numberWithCommas(data.WaiveAmount) + ".00");
                            }
                            var netamount = $("#NetAmount").html();
                            var waiveamount = $("#WaiveAmount").html();
                            var paidamount = $("#PaidAmount").html();
                            var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - ConvertToDecimal(paidamount);

                            $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
                        },
                        error: function (err) {
                            toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 });
                            $("#ServiceTableLoader").hide();
                            $("#serviceTableDiv").show();
                        }
                    });

                    // Get Total Amount Paid
                    $.ajax({
                        url: 'GetTotalPaidBillAmount?invoiceNumber=' + invoiceNumber,
                        method: "Get",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (totalamountpaid) {
                            updateNetAmount();
                            if (totalamountpaid != null) {
                                $("#PaidAmount").html("₦" + numberWithCommas(totalamountpaid) + ".00");
                                var netamount = $("#NetAmount").html();
                                var waiveamount = $("#WaiveAmount").html();
                                var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - totalamountpaid;

                                $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");
                            } else {
                                totalamount = "₦0.00";
                                $("#PaidAmount").html(totalamount);
                                var netamount = $("#NetAmount").html();
                                var balanceAmount = (ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount)) - totalamountpaid;

                                $("#BalanceAmount").html("₦" + numberWithCommas(balanceAmount) + ".00");                            }

                            $("#ServiceTableLoader").hide();
                            $("#serviceTableDiv").show();
                        },
                        error: function (err) {
                            toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 });
                            $("#ServiceTableLoader").hide();
                            $("#serviceTableDiv").show();
                        }
                    });

                    // Populate installment
                    $.ajax({
                        url: 'GetInstallmentsByInvoiceNumber?invoiceNumber=' + invoiceNumber,
                        method: "Get",
                        contentType: "application/json;charset=utf-8",
                        dataType: "json",
                        success: function (datas) {
                            if (datas.length > 0) {
                                installmentCount = 0;
                                $.each(datas, function (i, data) {
                                    installmentCount++;
                                    let html = "";
                                    if (data.HasPaid) {
                                        html = "<option value='" + data.Id + "' style='color: green'>" + data.InstallmentName + " (₦" + numberWithCommas(data.PartPaymentAmount) + ".00) <span class='badge badge-pill badge-success'>PAID</span></option>";
                                    } else {
                                        html = "<option value='" + data.Id + "'>" + data.InstallmentName + " (₦" + numberWithCommas(data.PartPaymentAmount) + ".00)</option>";
                                    }
                                    $("#installmentdrp").append(html);
                                });
                                $("#InstallmentDiv").show();
                            }

                        },
                        error: function (err) {
                            toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 });
                        }
                    })


                    e.target.innerHTML = "Search"
                   

                },
                error: function (err) {
                    toastr.error(err.responseText, "An Error Occurred", { showDuration: 500 })
                    e.target.innerHTML = "Search";
                    $("#customerInfoLoader").hide();
                    $("#customerinfoDiv").show();
                    $("#ServiceTableLoader").hide();
                    $("#serviceTableDiv").show();
                    $("#InstallmentTableLoader").hide();
                }
            })

        }
    }

})

function AddService(servicename) {
    $.ajax({
        url: 'GetServiceByName?servicename=' + servicename,
        method: "Get",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (response) {
            let html = "";
            html = "<tr id='" + response.Id + "' ><td><button class='btn btn-danger' onclick='Delete(this)'>Remove</button></td><td class='clothType'>" + response.Name + "</td><td><input type='number' value='1' class='form-control quantity-" + response.Id + "' onchange='UpdateAmount(this)' onkeyup='UpdateAmount(this)' /></td><td class='sellingprice-" + response.Id + "' data-id='" + response.CostPrice + "'>" + response.CostPriceString + "</td><td><strong class='gross-" + response.Id + " gross'>₦00.00</strong></td></tr>";
            $("#ServiceBody").append(html);
            $("#ServiceName").val("");
            CalculateGrossAmount(1, response.CostPrice, response.Id);
            updateNetAmount();
            UpdateBalanceAmount();
            //e.target.innerHTML = "Add";
        },
        error: function (err) {
           // toastr.error(err.responseText, "An Error Occurred", { showDuration: 500 })
           // e.target.innerHTML = "Add";
        }
    });

}


$("#ServiceName").on("blur", function () {
    var servicename = $("#ServiceName").val();
    if (servicename === "") {
        $("#ServiceName").addClass("is-invalid");
    }
    else {
        $(".dataTables_empty").addClass("d-none");
        $("#ServiceName").removeClass("is-invalid");

        setTimeout(function () {
            var isClothTypeExist = false;
            $('#ServiceBody .clothType').each(function () {
                var clothtype = $(this).html();
                servicename = $("#ServiceName").val();

                if (clothtype == servicename)
                    isClothTypeExist = true;
            });
            if (!isClothTypeExist) {
                servicename = $("#ServiceName").val();
                AddService(servicename);
            }
            else {
                toastr.success(servicename + " has already been added", { showDuration: 500 });
            }
        }, 200);
    }
});

$("#FinishBtn").click(function () {
    var valName = $("input[name='CustomerName']").val();
    var valAge = $("input[name='CustomerAge']").val();
    var valPhoneNumber = $("input[name='CustomerPhoneNumber']").val();
    if (valName === "" || valAge == "" || valPhoneNumber == "") {
        $("input[name='CustomerName']").addClass("is-invalid");
        $("input[name='CustomerAge']").addClass("is-invalid");
        $("#genderFll").addClass("is-invalid");
        $("input[name='CustomerPhoneNumber']").addClass("is-invalid");
        toastr.error("Please, fill the form properly", "Validation error", { showDuration: 500 })

    }
    else {
        var amountToPay = ConvertToDecimal($("#BalanceAmount").html());
        var amountToDeposit = ConvertToDecimal($("#AmountToDepositString").val());
        if (amountToPay == 0) {

            Swal.fire({
                title: 'Payment Completed',
                showCancelButton: false,
                confirmButtonText: 'Payment has already been completed',
                showLoaderOnConfirm: true,
            }).then((result) => {
           
            })
        }
        else if ($("#Deposit").is(":checked") && amountToDeposit == 0) {
            Swal.fire({
                title: 'Empty Deposit Amount',
                showCancelButton: false,
                confirmButtonText: 'Please enter amount to deposit',
                showLoaderOnConfirm: true,
            }).then((result) => {

            })
        }
        else if (amountToDeposit > amountToPay)
        {
            Swal.fire({
                title: 'Deposit Amount Exceeded',
                showCancelButton: false,
                confirmButtonText: 'Deposit amount is more than the amount to pay',
                showLoaderOnConfirm: true,
            }).then((result) => {

            })
        }
        else {
            $("input[name='CustomerName']").removeClass("is-invalid");
            $("input[name='CustomerAge']").removeClass("is-invalid");
            $("#genderFll").removeClass("is-invalid");
            $("input[name='CustomerPhoneNumber']").removeClass("is-invalid");
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
                    let serviceArr = [];

                    let data = {
                        BillInvoiceNumber: $("#BillInvoiceNumber").val(),
                        CollectionType: $("input[name='CollectionType']:checked").val(),
                        CustomerName: $("input[name='CustomerName']").val() == undefined ? $("#Customername")[0].innerText : $("input[name='CustomerName']").val(),
                        CustomerUniqueID: $("#CustomerUniqueID").val(),
                        CustomerAge: $("input[name='CustomerAge']").val() == undefined ? $("#Customerage")[0].innerText : $("input[name='CustomerAge']").val(),
                        CustomerGender: $("#genderFll").val() == undefined ? $("#Customergender")[0].innerText : $("#genderFll").val(),
                        CustomerPhoneNumber: $("input[name='CustomerPhoneNumber']").val() == undefined ? $("#Customerphonenumber")[0].innerText : $("input[name='CustomerPhoneNumber']").val(),
                        AmountPaid:  ConvertToDecimal($("#BalanceAmount").html()),
                       // AmountPaid: $("input[name='CollectionType']:checked").val() != "BILLED" ? ConvertToDecimal($("#BalanceAmount").html()) : ConvertToDecimal($("#BalanceAmount").html(),
                        WaivedAmount: ConvertToDecimal($("#WaiveAmount").html()),
                        NetAmount: ConvertToDecimal($("#NetAmount").html()),
                        PaymentType: $("#PaymentType").val(),
                        PartPaymentID: $("#installmentdrp").val(),
                        TransactionReferenceNumber: $("#TransactionReferenceNumber").val(),
                        AmountToDeposit: ConvertToDecimal($("#AmountToDepositString").val())
                    };

                    var table = $("#ServiceBody")[0].children;
                    $.each(table, function (i, tr) {
                        let serviceObj = {};
                        serviceObj.ServiceID = tr.id;
                        serviceObj.Quantity = $(".quantity-" + tr.id).val();
                        serviceObj.GrossAmount = ConvertToDecimal(tr.children[3].innerText);
                        serviceArr.push(serviceObj);
                    });
                    // Send ajax call to server
                    $.ajax({
                        url: 'CashCollections',
                        method: 'Post',
                        dataType: "json",
                        data: { vmodel: data, serviceList: serviceArr },
                        success: function (response) {
                            if (response.PaymentReciept != null) {
                                Swal.fire({
                                    title: 'Cash collected successfully',
                                    showCancelButton: false,
                                    confirmButtonText: 'Print payment receipt',
                                    showLoaderOnConfirm: true,
                                }).then((result) => {
                                    if (result.value) {
                                        let reportUrl = '/Admin/Report/PaymentReciept?recieptnumber=' + response.PaymentReciept + '&billnumber=' + response.BillInvoiceNumber;
                                        window.open(reportUrl, 'blank');
                                        location.href = "CashCollections?Saved=true";
                                    } else if (
                                        result.dismiss === Swal.DismissReason.cancel
                                    ) {
                                        location.href = "CashCollections?Saved=true";
                                    }
                                })
                            }
                        }
                    })
                }
                else if (
                    result.dismiss === Swal.DismissReason.cancel
                ) {
                    Swal.fire(
                        'Cancelled',
                        'Cancelled :)',
                        'error'
                    )
                }
            })
        }
    }
})

function CalculateGrossAmount(quantity, price, RowID) {
    let grossAmount = quantity * price;
    $(".gross-" + RowID).html("₦" + numberWithCommas(grossAmount) + ".00");
}
function Delete(e) {
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
            e.parentElement.parentElement.remove();
            toastr.success("Removed", "Service removed", { showDuration: 500 })
            updateNetAmount();
            UpdateBalanceAmount();
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
function UpdateAmount(e) {
    let RowID = e.parentElement.parentElement.id;

    let quantity = e.value;
    let price = $(".sellingprice-" + RowID)[0].dataset.id;

    let grossAmount = quantity * price;
    $(".gross-" + RowID).html("₦" + numberWithCommas(grossAmount) + ".00");

    updateNetAmount();
    UpdateBalanceAmount();

}
function updateNetAmount() {
    let grosses = $(".gross");
    let total = 0;
    $.each(grosses, function (i, gross) {
        var amount = ConvertToDecimal(gross.innerText);
        total += amount;
    });
    $("#NetAmount").empty();
    $("#NetAmount").html("₦" + numberWithCommas(total) + ".00")
}
function UpdateBalanceAmount() {
    var netamount = $("#NetAmount").html();
    var waiveamount = $("#WaiveAmount").html();

    var balance = ConvertToDecimal(netamount) - ConvertToDecimal(waiveamount);
    $("#BalanceAmount").html("₦" + numberWithCommas(balance) + ".00")
}
document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})
$(function () {
    $(".CustomerUnique").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Customer/GetCustomerOrPhoneAutoComplete",
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
