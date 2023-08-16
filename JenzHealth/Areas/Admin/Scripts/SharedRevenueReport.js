$("#analytics-overview-date-range").datepicker({});
"use strict"; !function (t) { t(".transaction-history").DataTable({ responsive: !0 }) }(jQuery);

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