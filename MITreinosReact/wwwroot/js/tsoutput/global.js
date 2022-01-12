//bootbox.setLocale('br');
$(".deleter").click(function (e) {
    var _this = this;
    e.preventDefault();
    bootbox.confirm("Você realmente deseja remover esse registro?", function (result) {
        if (result) {
            // @ts-ignore
            document.location = $(_this).attr('href');
        }
    });
});
// @ts-ignore
$('[data-toggle="tooltip"]').tooltip();
// @ts-ignore
$('[tooltip]').tooltip({
    title: function () {
        return $(this).attr('tooltip');
    }
});
//# sourceMappingURL=global.js.map