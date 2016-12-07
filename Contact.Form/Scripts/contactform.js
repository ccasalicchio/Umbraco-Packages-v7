$(function () {
	"use strict";
	$('#document_cpf').mask('000.000.000-00', {reverse: true});
	
	var maskBehavior = function (val) {
			return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
		},
		options = {onKeyPress: function(val, e, field, options) {
			field.mask(maskBehavior.apply({}, arguments), options);
		}
	};
	
	$('#phone').mask(maskBehavior, options);
	
	$("#contactform_send").submit(function(event) {
		event.preventDefault();
		$('.loader').show();
		$(this).children().each(function(){
			$(this).css("border","");
			$("#note").html("");
			
			if($(this).attr("required")){
				if($(this).val()!==''){	
					$(this).css("border", "2px solid #FF0000");
					return;
				}
			}
		});
		var userData = $(this).serialize();
		return $.ajax({
			type: "POST",
			url: "/ContactForm_Send.aspx",
			data: userData,
			success: function(t) {
				var result = '';
				"OK" == t.trim() ? (result = '<div class="notification_ok">Mensagem Enviada com sucesso!</div>', $("#fields").hide()) : result = '<div class="notification_error">' + t + '</div>', $("#note").html(result);
				$('.loader').hide();
			},
			error: function(t, i) {
				$("#note").html('<div class="notification_error">' + i + '</div>');
				$('.loader').hide();
			}
		});
	});
});