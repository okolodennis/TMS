﻿
$("#Search").click(function (e) {
    e.preventDefault();
    e.stopPropagation();

    let invoicenumber = $("#Searchby").val();

    if (invoicenumber === "") {
        $("#Searchby").addClass("is-invalid");
    }
    else {
        $("#Searchby").removeClass("is-invalid");
        e.target.innerHTML = "Searching...";
        $("#customerInfoLoader").show();
        $("#ServiceTableLoader").show();
        $("#InstallmentTableLoader").show();
        $("#serviceTableDiv").hide();
        $("#ServiceBody").empty();

        $.ajax({
            url: "CheckSpecimenCollection?invoicenumber=" + invoicenumber,
            method: "Get",
            dataType: "Json",
            contentType: "application/json;charset=utf-8",
            success: function (exist) {
                if (exist) {
                    $.ajax({
                        url: '/Payment/GetCustomerByInvoiceNumber?invoiceNumber=' + invoicenumber,
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


                            // Populate Service
                            $.ajax({
                                url: 'GetSpecimenCollected?invoiceNumber=' + invoicenumber,
                                method: "Get",
                                contentType: "application/json;charset=utf-8",
                                dataType: "json",
                                success: function (datas) {
                                    var date = new Date(+datas.RequestingDate.replace(/\D/g, '') + 3600 *1000 *24);
                                    $("#RequestingDate").val(date.toISOString().substr(0, 10))
                                    $("#RequestingPhysician").val(datas.RequestingPhysician)
                                    $("#ClinicalSummary").val(datas.ClinicalSummary)
                                    $("#ProvitionalDiagnosis").val(datas.ProvitionalDiagnosis)
                                    $("#OtherInformation").val(datas.OtherInformation)

                                    $("#ServiceBody").empty()

                                    $.each(datas.CheckList, function (i, data) {
                                        let html = "";
                                        if (data.IsCollected) {
                                            html = "<tr id='" + data.Id + "' ><td>" + data.Service + "</td><td>" + data.Specimen + "</td><td><input type='checkbox' class='chk' checked='" + data.IsCollected + "' value='" + data.Id + "' /></td></tr>"; $("#ServiceBody").append(html);
                                        }
                                        else {
                                            html = "<tr id='" + data.Id + "' ><td>" + data.Service + "</td><td>" + data.Specimen + "</td><td><input type='checkbox' class='chk' value='" + data.Id + "' /></td></tr>"; $("#ServiceBody").append(html);
                                        }
                                    });

                                    $("#ServiceTableLoader").hide();
                                    $("#serviceTableDiv").show();
                                },
                                error: function (err) {
                                    toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 })
                                }
                            });

                            e.target.innerHTML = "Search"
                        },
                        error: function (err) {
                            toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
                            e.target.innerHTML = "Search";
                            $("#customerInfoLoader").hide();
                            $("#customerinfoDiv").show();
                            $("#ServiceTableLoader").hide();
                            $("#serviceTableDiv").show();
                        }
                    })

                }
                else {
                    $.ajax({
                        url: '/Payment/GetCustomerByInvoiceNumber?invoiceNumber=' + invoicenumber,
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

                            // Populate Service
                            $.ajax({
                                url: 'GetServicesAndSpecimenByInvoiceNumber?invoiceNumber=' + invoicenumber,
                                method: "Get",
                                contentType: "application/json;charset=utf-8",
                                dataType: "json",
                                success: function (datas) {
                                    $("#ServiceBody").empty()
                                    $.each(datas, function (i, data) {
                                        let html = "";
                                        html = "<tr id='" + data.Id + "' ><td>" + data.Service + "</td><td>" + data.Specimen + "</td><td><input type='checkbox' class='chk' value='" + data.Id + "' /></td></tr>";
                                        $("#ServiceBody").append(html);
                                    });

                                    $("#ServiceTableLoader").hide();
                                    $("#serviceTableDiv").show();
                                },
                                error: function (err) {
                                    toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 })
                                }
                            });

                            e.target.innerHTML = "Search"
                        },
                        error: function (err) {
                            toastr.error(err.responseText, "Data not retrieved successfully", { showDuration: 500 })
                            e.target.innerHTML = "Search";
                            $("#customerInfoLoader").hide();
                            $("#customerinfoDiv").show();
                            $("#ServiceTableLoader").hide();
                            $("#serviceTableDiv").show();
                        }
                    })

                }
            },
            error: function (err) {
                toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 })
            }
        })


    }

})
$("#FinishBtn").click(function () {
    var requestDate = $("#RequestingDate").val();
    var requestPhysician = $("#RequestingPhysician").val();
    var clinicalSummary = $("#ClinicalSummary").val();
    var provitionalDiagnosis = $("#ProvitionalDiagnosis").val();
    var otherInformation = $("#OtherInformation").val();

    if (requestDate === "") {
        $("#RequestingDate").addClass("is-invalid");
    } else if (requestPhysician === "") {
        $("#RequestingPhysician").addClass("is-invalid");
    }
    else if (clinicalSummary === "") {
        $("#ClinicalSummary").addClass("is-invalid");
    }
    else if (provitionalDiagnosis === "") {
        $("#ProvitionalDiagnosis").addClass("is-invalid");
    }
    else if (otherInformation === "") {
        $("#OtherInformation").addClass("is-invalid");
    }
    else {
        $("#RequestingDate").removeClass("is-invalid");
        $("#RequestingPhysician").removeClass("is-invalid");
        $("#ClinicalSummary").removeClass("is-invalid");
        $("#ProvitionalDiagnosis").removeClass("is-invalid");
        $("#OtherInformation").removeClass("is-invalid");


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

                var SpecimentCollected = {
                    BillInvoiceNumber: $("#Searchby").val(),
                    RequestingDate: $("#RequestingDate").val(),
                    RequestingPhysician: $("#RequestingPhysician").val(),
                    ClinicalSummary: $("#ClinicalSummary").val(),
                    ProvitionalDiagnosis: $("#ProvitionalDiagnosis").val(),
                    OtherInformation: $("#OtherInformation").val()
                };

                let ServiceList = [];
                var table = $("#ServiceBody")[0].children;
                $.each(table, function (i, tr) {
                    i += 1;
                    // Create Parameter
                    let setup = {};
                    setup.Service = tr.children[0].innerText;
                    setup.Specimen = tr.children[1].innerText;
                    setup.IsCollected = tr.children[2].children[0].checked;

                    // Add to Parameter list
                    ServiceList.push(setup);
                });
                // Send ajax call to server
                $.ajax({
                    url: 'UpdateSpecimenCollection',
                    method: 'Post',
                    dataType: "json",
                    data: { specimenCollection: SpecimentCollected, checkList: ServiceList },
                    success: function (response) {
                        Swal.fire({
                            title: 'Specimen collected successfully',
                            showCancelButton: false,
                            confirmButtonText: 'Ok',
                            showLoaderOnConfirm: true,
                        }).then((result) => {
                            if (result.value) {
                                window.location.reload(true);
                            } else if (
                                result.dismiss === Swal.DismissReason.cancel
                            ) {
                                window.location.reload(true);
                            }
                        })
                    }
                })
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

document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})