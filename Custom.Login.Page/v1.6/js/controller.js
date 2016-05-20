'use strict';
angular.module("umbraco")
    .controller("Custom.Login.Page",
    function ($scope, mediaResource) {
        var engine = {
            backgrounds: [],
            selectBackground: null,
            customization: {
                background: {},
                css: '',
                color: ''
            }
        }
        if ($scope.model.value) {
            engine.customization = $scope.model.value;
        }
        function populateBackground(value) {
            return {
                name: value.name,
                url: value.properties[0].value.src
            };
        }
        function selectBackground(index) {
            engine.customization.background = engine.backgrounds[index];
        }
        function renderCss() {
            var bg = ".login-overlay {background:url('{0}') no-repeat center !important;background-size: 100% 100% !important;}",
                ring = ".uil-ring-css>div {box-shadow: 0 6px 0 0 {0} !important;position: absolute;display: block;width: 160px;height: 160px;top: 20px;left: 20px;border-radius: 80px;-ms-animation: uil-ring-anim 1s linear infinite;-moz-animation: uil-ring-anim 1s linear infinite;-webkit-animation: uil-ring-anim 1s linear infinite;-o-animation: uil-ring-anim 1s linear infinite;animation: uil-ring-anim 1s linear infinite}",
                input = ".login-overlay input[type='text']:focus, input[type='password']:focus {border: 3px solid {0} !important;}",
                tag = "<style>{0}</style>",
                fullstr = "";

            if (engine.customization.color) {
                ring = ring.replace('{0}', engine.customization.color);
                input = input.replace('{0}', engine.customization.color);
                fullstr += ring;
                fullstr += input;
            }
            if (engine.customization.background) {
                bg = bg.replace('{0}', engine.customization.background.url);
                fullstr += bg;
            }
            if (engine.customization.css) {
                fullstr += engine.customization.css;
            }
            tag = tag.replace('{0}', fullstr);
            return tag;
        }

        mediaResource.getChildren(1738)
            .then(function (contentArray) {
                angular.forEach(contentArray.items, function (value, key) {
                    this.push(populateBackground(value));
                }, engine.backgrounds);
            });
        angular.element('button[type="submit"]').on('click', function () {
            angular.element(this).before("<div class='uil-ring-css' id='processing' style='transform:scale(0.6);'><div></div></div>");
        });
        angular.element('head').append(renderCss());
        engine.selectBackground = selectBackground;
        $scope.model.value = engine.customization;
        $scope.engine = engine;
    });