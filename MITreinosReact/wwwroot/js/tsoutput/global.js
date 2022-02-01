//bootbox.setLocale('br');
// @ts-ignore
$(".deleter").click(function (e) {
    var _this = this;
    e.preventDefault();
    // @ts-ignore
    bootbox.confirm("Voc� realmente deseja remover esse registro?", function (result) {
        if (result) {
            // @ts-ignore
            document.location = $(_this).attr('href');
        }
    });
});
//# sourceMappingURL=global.js.map