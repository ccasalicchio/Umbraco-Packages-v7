(function ($) {
	setTimeout(function () {
		$('button[type="submit"]').on('click', function () {
			$(this).before("<div class='uil-ring-css' id='processing' style='transform:scale(0.6);'><div></div></div>");
		});
	}, 800);
	setInterval(function () {
		if ($('.alert').is(":visible")) {
			$('#processing').remove();
		}
	}, 1000);
})(jQuery);