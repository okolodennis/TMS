﻿
$(function () {
    $(".odd").remove();

    // Set  Customer Type
    var selectedMicroscopy = $("input[name='MicroscopyType']:checked").val();

    var radios = $(".MicroscopyTypeStatus");
    $.each(radios, function (i, radio) {
        if (selectedMicroscopy == "") {
            if (radio.id == "NA") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else {
                radio.checked = false;
            }
        } else {
            if (radio.id == selectedMicroscopy && selectedMicroscopy == "WET_MOUNT") {
                radio.checked = true;
                $("#WetAmountDiv").show();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "STAIN") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").show();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "SFA") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").show();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "KOH") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").show();
                $("#MicroOthersDiv").hide();
            }
            else if (radio.id == selectedMicroscopy && selectedMicroscopy == "OTHER_MICROSCOPY") {
                radio.checked = true;
                $("#WetAmountDiv").hide();
                $("#StainDiv").hide();
                $("#SFADiv").hide();
                $("#KOHDiv").hide();
                $("#MicroOthersDiv").show();
            }
            else {
                radio.checked = false;
            }
        }
   
    })


    var selectedStain = $("input[name='StainType']:checked").val();
    var radios = $(".StainTypeStatus");
    $.each(radios, function (i, radio) {
        if (selectedStain == "") {
            if (radio.id == "GRAIN_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").show();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else {
                radio.checked = false;
            }
        } else {
            if (radio.id == selectedStain && selectedStain == "GRAIN_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").show();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "GIEMSA_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").show();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "ZIEHL_NEELSON_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").show();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "INDIAN_INK_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").show();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "IODINE_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").show();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "METHYLENE_BLUE_STAIN") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").show();
                $("#OthersStainDiv").hide();
            }
            else if (radio.id == selectedStain && selectedStain == "OTHER_STAINS") {
                radio.checked = true;
                $("#GrainStainDiv").hide();
                $("#GiemsStainDiv").hide();
                $("#ZiehlStainDiv").hide();
                $("#IndiaStainDiv").hide();
                $("#IodineStainDiv").hide();
                $("#MethyleneStainDiv").hide();
                $("#OthersStainDiv").show();
            }
            else {
                radio.checked = false;
            }
        }
      
    })

    UpdateOrganismTbl();

})

function UpdateOrganismTbl() {
    var id = $("#Id").val();
    $.ajax({
        url: "GetComputedAntibioticsAndOrgansm/" + id,
        method: "GET",
        success: function (response) {
            let html = "";
            $.each(response, function (i, data) {
                html = `<tr>
                            <td>
                                <button class='btn btn-danger' type='button' onclick='DeleteOrganism(this)'>Remove</button>
                            </td>
                            <td data-orgranismid='${data.OrganismID}'>${data.Organism}</td>
                            <td data-antibioticsid='${data.AntiBioticID}'>
                             ${data.AntiBiotic}
                            </td>
                            <td>
                                <input type="checkbox" ${data.IsSensitive ? "checked" : ""} onchange="ToggleDegree(this)">
                                <input type="text" class="form-control" value="${data.SensitiveDegree == null ? "" : data.SensitiveDegree}" ${!data.IsSensitive ? "disabled" : ""} />
                            </td>
                            <td>
                                 <input type="checkbox" ${data.IsIntermediate ? "checked" : ""}>
                            </td>
                            <td>
                                <input type="checkbox" ${data.IsResistance ? "checked" : ""} onchange="ToggleDegree(this)">
                                <input type="text" class="form-control" value="${data.ResistanceDegree == null ? "" : data.ResistanceDegree}" ${!data.IsResistance ? "disabled" : ""}/>
                            </td>
                       </tr>`;
                $("#OrganismBody").append(html);
            })

            $("#organismField").val("");
            $("#OrganismTableLoader").hide();
            //    $("#OrganismTableDiv").show();
        },
        error: function (e) {
            $("#OrganismTableLoader").hide();
            toastr.error("Error", "An error occured!", { showDuration: 500 })
        }
    })
}
function CheckValidity() {
    let states = [];
    var inputs = $("input");
    var selects = $("select");
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

$(function () {
    $("#organismField").autoComplete({
        resolver: "custom",
        events: {
            search: function (qry, callback) {
                $.ajax({
                    url: "/Admin/Seed/GetOrganismAutoComplete",
                    type: "POST",
                    dataType: "json",
                    data: { term: qry },
                }).done(function (res) {
                    callback(res)
                });
            }
        },
        minLength: 1,
    });

})


function DeleteOrganism(e) {
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
            installmentCount--;
            toastr.success("Removed", "Organism removed", { showDuration: 500 })
            updateInstallmentNetAmount();
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



document.addEventListener("keyup", function (e) {
    if (e.target.value === "") {
        e.target.classList.add("is-invalid");
    } else {
        e.target.classList.remove("is-invalid");
    }
})

$("#step").steps({
    headerTag: "h3",
    bodyTag: "section",
    transitionEffect: "slideLeft",
    autoFocus: true,
    stepsOrientation: "vertical",
    onFinished: function () {
        Update();
    }
});

$("input[name='MicroscopyType']").change(function () {
    if ($("input[name='MicroscopyType']:checked").val() == "NA") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "WET_MOUNT") {
        $("#WetAmountDiv").show();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "STAIN") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").show();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "SFA") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").show();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "KOH") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").show();
        $("#MicroOthersDiv").hide();
    }
    else if ($("input[name='MicroscopyType']:checked").val() == "OTHER_MICROSCOPY") {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").show();
    }
    else {
        $("#WetAmountDiv").hide();
        $("#StainDiv").hide();
        $("#SFADiv").hide();
        $("#KOHDiv").hide();
        $("#MicroOthersDiv").hide();
    }

});

$("input[name='StainType']").change(function () {
    if ($("input[name='StainType']:checked").val() == "GRAIN_STAIN") {
        $("#GrainStainDiv").show();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "GIEMSA_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").show();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "ZIEHL_NEELSON_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").show();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "INDIAN_INK_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").show();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "IODINE_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").show();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "METHYLENE_BLUE_STAIN") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").show();
        $("#OthersStainDiv").hide();
    }
    else if ($("input[name='StainType']:checked").val() == "OTHER_STAINS") {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").show();
    }
    else {
        $("#GrainStainDiv").hide();
        $("#GiemsStainDiv").hide();
        $("#ZiehlStainDiv").hide();
        $("#IndiaStainDiv").hide();
        $("#IodineStainDiv").hide();
        $("#MethyleneStainDiv").hide();
        $("#OthersStainDiv").hide();
    }
});
function AddOrganism(organism) {
    $.ajax({
        url: "/Seed/GetAntiBioticByOrganismName?organismName=" + organism,
        method: "GET",
        success: function (response) {
            let html = "";
            $.each(response, function (i, data) {
                html = `<tr>
                            <td>
                                <button class='btn btn-danger' type='button' onclick='DeleteOrganism(this)'>Remove</button>
                            </td>
                            <td data-orgranismid='${data.OrganismID}'>${organism}</td>
                            <td data-antibioticsid='${data.Id}'>
                             ${data.Name}
                            </td>
                            <td>
                                <input type="checkbox" onchange="ToggleDegree(this)">
                                <input type="text" class="form-control" disabled />
                            </td>
                            <td>
                                 <input type="checkbox">
                            </td>
                            <td>
                                <input type="checkbox"  onchange="ToggleDegree(this)">
                                <input type="text" class="form-control" disabled/>
                            </td>
                       </tr>`;
                $("#OrganismBody").append(html);
            })

            $("#organismField").val("");
            $("#OrganismTableLoader").hide();
        //    $("#OrganismTableDiv").show();
        },
        error: function (e) {
            $("#OrganismTableLoader").hide();
            toastr.error("Error", "An error occured!", { showDuration: 500 })
        }
    })

}


function ToggleDegree(e) {
    var checkboxstate = e.checked;
    var field = e.parentElement.children[1];
    field.disabled = !checkboxstate;
}

$("#organismField").on("blur", function (e) {
    var organism = $("#organismField").val();

    if (organism === "") {
        $("#organismField").addClass("is-invalid");
    }
    else {
        $("#organismField").removeClass("is-invalid");
        $("#OrganismTableLoader").show();
        //$("#OrganismTableDiv").hide();
        setTimeout(function () {
            organism = $("#organismField").val();
            AddOrganism(organism);
        }, 200);

    }
});

function Update() {
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

            var data = {
                Appearance: $("#Appearance").val(),
                Color: $("#Color").val(),
                MacrosopyBlood: $("#MacrosopyBlood").val(),
                AdultWarm: $("#AdultWarm").val(),
                Mucus: $("#Mucus").val(),
                SpecificGravity: $("#SpecificGravity").val(),
                Blirubin: $("#Blirubin").val(),
                Acidity: $("#Acidity").val(),
                Urobilinogen: $("#Urobilinogen").val(),
                Glucose: $("#Glucose").val(),
                AscorbicAcid: $("#AscorbicAcid").val(),
                Protein: $("#Protein").val(),
                DipstickBlood: $("#DipstickBlood").val(),
                Niterite: $("#Niterite").val(),
                LeucocyteEsterase: $("#LeucocyteEsterase").val(),
                Ketones: $("#Ketones").val(),
                Temperature: $("#Temperature").val(),
                Duration: $("#Duration").val(),
                Atomsphere: $("#Atomsphere").val(),
                Plate: $("#Plate").val(),
                Incubatio: $("#Incubatio").val(),
                BillInvoiceNumber: $("#billnumber").val(),
                SpecimenCollectedID: $("#SpecimenCollectedID").val(),
                MicroscopyType: $("input[name='MicroscopyType']:checked").val(),
                PusCells: $("#PusCells").val(),
                Cystals: $("#Cystals").val(),
                WhiteBloodCells: $("#WhiteBloodCells").val(),
                Ova: $("#Ova").val(),
                RedBloodCells: $("#RedBloodCells").val(),
                WetMountParasite: $("#WetMountParasite").val(),
                WetMountBacteria: $("#WetMountBacteria").val(),
                EpithelialCells: $("#EpithelialCells").val(),
                WetMountYesats: $("#WetMountYesats").val(),
                Protozoa: $("#Protozoa").val(),
                Casts: $("#Casts").val(),
                WetMountOthers: $("#WetMountOthers").val(),
                StainType: $("input[name='StainType']:checked").val(),
                GramPositiveCocci: $("#GramPositiveCocci").val(),
                GramNegativeCocci: $("#GramNegativeCocci").val(),
                GramPositiveRods: $("#GramPositiveRods").val(),
                GramNegativeRods: $("#GramNegativeRods").val(),
                TrichomonasVaginalis: $("#TrichomonasVaginalis").val(),
                VincetsOrganisms: $("#VincetsOrganisms").val(),
                YeastCells: $("#YeastCells").val(),
                GiemsaStainParasite: $("#GiemsaStainParasite").val(),
                GiemsaOthers: $("#GiemsaOthers").val(),
                AcisFastBacilli: $("#AcisFastBacilli").val(),
                ZiehlOthers: $("#ZiehlOthers").val(),
                IndiaInkResult: $("#IndiaInkResult").val(),
                IodineResult: $("#IodineResult").val(),
                MethyleneResult: $("#MethyleneResult").val(),
                OtherStainResult: $("#OtherStainResult").val(),
                DurationOfAbstinence: $("#DurationOfAbstinence").val(),
                DateOfProduction: $("#DateOfProduction").val(),
                TimeRecieved: $("#TimeRecieved").val(),
                ModeOfProduction: $("#ModeOfProduction").val(),
                Viscosity: $("#Viscosity").val(),
                TimeExamined: $("#TimeExamined").val(),
                Morphology: $("#Morphology").val(),
                Motility: $("#Motility").val(),
                TotalSpermCount: $("#TotalSpermCount").val(),
                ImmatureCells: $("#ImmatureCells").val(),
                WBCS: $("#WBCS").val(),
                AnySpillage: $("#AnySpillage").val(),
                RBCS: $("#RBCS").val(),
                PH: $("#PH").val(),
                KOHPrepareation: $("#KOHPrepareation").val(),
                KOHResult: $("#KOHResult").val(),
                OthersResult: $("#OthersResult").val(),
                Labnote: $("#Labnote").val()
            }
            var specimencollectedID = $("#SpecimenCollectedID").val();
            let OrganismList = [];
            var table = $("#OrganismBody")[0].children;
            $.each(table, function (i, tr) {
                // Create installment
                let organism = {};
                organism.OrganismID = tr.children[1].dataset.orgranismid;
                organism.AntiBioticID = tr.children[2].dataset.antibioticsid;
                organism.IsSensitive = tr.children[3].children[0].checked;
                organism.SensitiveDegree = tr.children[3].children[1].value;
                organism.IsIntermediate = tr.children[4].children[0].checked;
                organism.IsResistance = tr.children[5].children[0].checked;
                organism.ResistanceDegree = tr.children[5].children[1].value;

                // Add to Installment list
                OrganismList.push(organism);
            });

            // Send ajax call to server
            $.ajax({
                url: 'UpdateNonTemplatedLabResults',
                method: 'Post',
                dataType: "json",
                data: { vmodel: data, organisms: OrganismList },
                success: function (response) {
                    Swal.fire({
                        title: 'Test computed successfully',
                        showCancelButton: false,
                        confirmButtonText: 'Ok',
                        showLoaderOnConfirm: true,
                    }).then((result) => {
                        if (result.value) {
                            location.href = "Prepare?ID=" + specimencollectedID + "&Saved=" + response;
                        } else if (
                            result.dismiss === Swal.DismissReason.cancel
                        ) {
                            location.href = "Prepare?ID=" + specimencollectedID + "&Saved=" + response;
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



