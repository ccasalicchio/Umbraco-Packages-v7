(function($) {
	setTimeout(function(){
		$('button[type="submit"]').on('click',function(){
			$(this).before("<div class='uil-ring-css' id='processing' style='transform:scale(0.6);'><div></div></div>");
			//$(this).attr('disabled','disabled');
		});
	},800);
	setInterval(function(){
		if($('.alert').is(":visible")){
			//$(this).removeAttr('disabled');
			$('#processing').remove();
		}
	},1000);
})(jQuery);