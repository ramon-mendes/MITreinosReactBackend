declare var _DATA_dbg: boolean;

//bootbox.setLocale('br');

$(".deleter").click(function(e) {
	e.preventDefault();
	bootbox.confirm("Você realmente deseja remover esse registro?", (result) => {
		if(result)
		{
			// @ts-ignore
			document.location = $(this).attr('href');
		}
	});
});

// @ts-ignore
$('[data-toggle="tooltip"]').tooltip();

// @ts-ignore
$('[tooltip]').tooltip({
	title: function() {
		return $(this).attr('tooltip');
	}
});
