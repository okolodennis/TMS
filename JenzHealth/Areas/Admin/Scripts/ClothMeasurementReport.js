////$("#analytics-overview-date-range").datepicker({});
"use strict"; !function (t) { t(".transaction-history").DataTable({ responsive: !0 }) }(jQuery);


$("#pdf[data-form-method='post']").click(function (event) {
    event.preventDefault();
    var element = $(this);
    var action = element.attr("href");
    document.getElementById("exportfiletype").value = 'PDF';
    element.closest("form").each(function () {
        var form = $(this);
        form.attr("ClothMeasurementReport", "Report");
        form.submit();
    });
});

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
