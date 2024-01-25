// Logo
function readURL(input) {
  if (input.files && input.files[0]) {
    var reader = new FileReader();
    
    reader.onload = function(e) {
      $('#Logo').attr('src', e.target.result);
    }
    
    reader.readAsDataURL(input.files[0]);
  }
}

// Watermark
function readWatermarkURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#Watermark').attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}




// logo
$("#LogoInp").change(function () {
  readURL(this);
});

//Watermark
$("#WatermarkInp").change(function () {
    readWatermarkURL(this);
});

$("#PaymentCompleted").click(function (e) {
    if ($("#PaymentCompleted").is(":checked")) {
        $("#ClothReady").prop("checked", false);
        $("#ClothReady").prop("value", true);
    }
  
});

$("#ClothReady").click(function (e) {
    if ($("#ClothReady").is(":checked")) {
        $("#PaymentCompleted").prop("checked", false);
        $("#PaymentCompleted").prop("value", true);
    }
  
});
