declare var _DATA_dbg: boolean;

//bootbox.setLocale('br');

// @ts-ignore
$(".deleter").click(function(e) {
	e.preventDefault();
	// @ts-ignore
	bootbox.confirm("Voc� realmente deseja remover esse registro?", (result) => {
		if(result)
		{
			// @ts-ignore
			document.location = $(this).attr('href');
		}
	});
});
