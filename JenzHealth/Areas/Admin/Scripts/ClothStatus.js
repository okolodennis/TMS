
$(".Search").on("change", function () {
    // e.preventDefault();
    // e.stopPropagation();

    setTimeout(function () {

        let username = $("#Tailor").val();

        if (username === "") {
            $("#Tailor").addClass("is-invalid");
        }
        else {
            $("#Tailor").removeClass("is-invalid");
            // e.target.innerHTML = "Searching...";
            $("#ServiceTableLoader").show();
            $("#serviceTableDiv").hide();
            $("#ServiceBody").empty();

            var counter = 0;
            // Populate Billed Clothes
            $.ajax({
                url: 'GetClothesFromAssignTailor?username=' + username,
                method: "Get",
                contentType: "application/json;charset=utf-8",
                dataType: "json",
                success: function (datas) {
                    /* $("#ServiceBody").empty()*/
                    $.each(datas, function (i, data) {
                        counter++;
                        let html = "";
                        if (data.IsReady)
                            html = "<tr id='" + data.Id + "' ><td>" + counter + "</td><td>" + data.BillNumber + "</td><td>" + data.CustomerName + "</td><td>" + data.ClothType + "</td><td>" + data.Quantity + "</td><td>" + data.CollectionDateString + "</td><td>" + data.Status + "</td><td><input type='checkbox' class='chk' disabled checked /></td></tr>";
                        else
                            html = "<tr id='" + data.Id + "' ><td>" + counter + "</td><td>" + data.BillNumber + "</td><td>" + data.CustomerName + "</td><td>" + data.ClothType + "</td><td>" + data.Quantity + "</td><td>" + data.CollectionDateString + "</td><td>" + data.Status + "</td><td><input type='checkbox' class='chk' /></td></tr>";
                        $("#ServiceBody").append(html);
                        $("#FinishBtn").addClass("d-block");

                    });

                    $("#ServiceTableLoader").hide();
                    $("#serviceTableDiv").show();
                },
                error: function (err) {
                    toastr.error(err.fail, "Data not retrieved successfully", { showDuration: 500 })
                }
            });

            //  e.target.innerHTML = "Search"

        }
    }, 1000);
})
$("#FinishBtn").click(function () {
   
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
                const data = []
                const tbody = $("#ServiceBody")[0].children;

                $.each(tbody, (i, tr) => {
                    const id = tr.id;
                    const IsReady = tr.children[7].getElementsByClassName("chk")[0].checked;
                   // const IsReady = tr.getElememtsByClassName("chk").checked;

                    data.push({ Id: id, IsReady: IsReady})
                })
                console.log(data);
                // Send ajax call to server
                $.ajax({
                    url: 'UpdateClothesFromAssignTailor',
                    method: 'Post',
                    dataType: "json",
                    data: { data: data },
                    success: function (response) {
                        Swal.fire({
                            title: 'Saved successfully',
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
    


})

$(function () {
    $(".Tailor").autoComplete({
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

})
document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})