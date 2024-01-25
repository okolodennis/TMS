////$("#analytics-overview-date-range").datepicker({});
"use strict"; !function (t) { t(".transaction-history").DataTable({ responsive: !0 }) }(jQuery);


$("#pdf[data-form-method='post']").click(function (event) {
    event.preventDefault();
    var element = $(this);
    var action = element.attr("href");
    document.getElementById("exportfiletype").value = 'PDF';
    element.closest("form").each(function () {
        var form = $(this);
        form.attr("EarnedRevenueReport", "Report");
        form.submit();
    });
});