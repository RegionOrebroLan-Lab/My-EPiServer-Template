import $ from "jquery";

export default function recaptcha(resolver: any, siteKey: string, tokenParameterName: string) {
	resolver.ready(() => {
		$(".grecaptcha-badge").hide();

		var recaptchaLogo = $(".grecaptcha-logo");
		var recaptchaLogoHtml = "<div class=\"form-group recaptcha-information\">" + recaptchaLogo.html() + "</div>";

		$("form[data-recaptcha-enabled=\"true\"]").each(function () {
			var formComponentsSelector = "button, input, textarea";
			var form = $(this);

			var elementAfterRecaptchaInformation = form.find("[data-element-after-recaptcha-information]");

			if (elementAfterRecaptchaInformation.length === 0)
				elementAfterRecaptchaInformation = form.find("button[type=\"submit\"]");

			elementAfterRecaptchaInformation.before(recaptchaLogoHtml);

			var anchorCharacter = "#";
			var action = form.attr("action");
			//console.log("Form-action: " + action);
			var actionParts = action.split(anchorCharacter);

			if (actionParts.length > 1) {
				actionParts.pop();
				action = actionParts.join(anchorCharacter);
				//console.log("Form-action without anchor: " + action);
			}

			var recaptchaAction = action.replace(/[^a-zA-Z_]/g, "_").substring(0, 100); // Only A-Z, a-z and _ are supported and a maximum length of 100.
			//console.log("ReCaptcha-action: " + recaptchaAction);

			form.find(formComponentsSelector).each(function () {
				$(this).prop("disabled", true);
			});

			resolver.execute(siteKey, { action: recaptchaAction }).then(token => {
				form.prepend("<input name=\"" + tokenParameterName + "\" type=\"hidden\" value=\"" + token + "\" />");

				form.find(formComponentsSelector).each(function () {
					$(this).prop("disabled", false);
				});
			});
		});
	});
}