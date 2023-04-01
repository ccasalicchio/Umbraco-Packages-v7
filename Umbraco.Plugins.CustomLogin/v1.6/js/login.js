(function ($) {
	$('body').append("<div class='uil-ring-css' id='processing' style='transform:scale(0.6);'><div></div></div>");
	setTimeout(function () {
		$('button[type="submit"]').on('click', function () {
			if ($('.uil-ring-css').length > 0) {
				$('#processing').find('div').attr("opacity", 1);
			}
		});
	}, 800);
	setInterval(function () {
		if ($('.alert').is(":visible")) {
			$('#processing').attr("opacity", 0);
		}
	}, 1000);
})(jQuery);