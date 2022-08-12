angular.module("umbraco")
 .factory('PMGScope', [function () {
     var service = {
         safeApply: function (fn) {
             var phase = this.$root.$$phase;
             if (phase == '$apply' || phase == '$digest') {
                 if (fn && (typeof (fn) === 'function')) {
                     fn();
                 }
             } else {
                 this.$apply(fn);
             }
         }
     };
     return service;

 } ]).factory('PmgVideoHelper', [function () {
     // the service types should match the techOrder items.
     var servicetype = {
         "vimeo": "vimeo",
         "youtube": "youtube",
         "dailymotion": "dailymotion"
     };
     var service = {
         getService: function (url) {
             url = url.toLowerCase();
             for (var key in servicetype) {
                 if (url.indexOf(key) >= 0) {
                     return servicetype[key];
                 }
             }
             return null;
         }
     }
     return service;
 } ]).directive('pmgPaste', ['PMGScope', '$timeout', function (PMGScope, $timeout) {
     return {
         restrict: 'A',
         link: function (scope, elem, attrs) {
             var el = elem;
             scope.safeApply = PMGScope.safeApply;

             $(elem).on('paste', function (e) {
                 // we have to put a timeout so that ngmodel updates after the event.
                 $timeout(function () {
                     scope.safeApply(function () {
                         // scope[attrs.ngModel] = e.val;
                         scope.pasteTracker++;
                     });
                 }, 100);

             })
         }
     }
 } ]).directive('pmgVideoplayer', ['angularHelper', function (angularHelper) {
        return {
            restrict: 'AE',
            template: '<div></div>',
            scope: {
                loaded: '=',
                pmgVideoplayer: '=',
                config: '='
            },
            link: function (scope, element, attr) {
                var init = false;
                var el; //= $('video', element).get(0);
                var template = $('<video class="vjs-default-skin video-js hideplayer" width="640" height="480"> </video>');

                var videoUrl;
                var vidPlayer;

                scope.safeApply = function (fn) {
                    var phase = this.$root.$$phase;
                    if (phase == '$apply' || phase == '$digest') {
                        if (fn && (typeof (fn) === 'function')) {
                            fn();
                        }
                    } else {
                        this.$apply(fn);
                    }
                };

                function setupPlayer(doDisposal) {

                    if (doDisposal) {
                        vidPlayer.dispose();
                        $(element).empty();
                    }
                    template.clone().appendTo(element);
                    el = $('video', element).get(0);
                    $(el).removeClass('hideplayer');
                    vidPlayer = videojs(el, scope.pmgVideoplayer).ready(function () {

                    });
                }
                scope.$watch('loaded', function (newVal) {

                    if (!init && newVal) {

                        scope.$watch('pmgVideoplayer.src', function (newVal) {
                            if (!newVal) return;
                            if (newVal != videoUrl) {
                                videoUrl = newVal;

                                if (!init) {
                                    setupPlayer(false);
                                    init = true;

                                } else {
                                    setupPlayer(true);

                                }
                            }

                        })

                    }

                });

            }
        }
    } ])
    .controller("pmg.VideoPlayerCtrl",
    function ($scope, PmgVideoHelper, contentResource, angularHelper, editorState, $q,
        notificationsService, navigationService, entityResource, assetsService, $location, PMGScope) {
        $scope.safeApply = PMGScope.safeApply;

        $scope.pasteTracker = 0;

        $scope.$watch('pasteTracker', function (newVal) {
            if (newVal) {
                $scope.safeApply(function () {
                    $scope.setVideo();
                });
            }
        })

        $scope.setVideo = function () {
            $scope.safeApply();
            $scope.config = { "controls": false, "quality": "480p", "width": "480", "height": "360", "autoplay": false, "preload": "auto", "techOrder": [PmgVideoHelper.getService($scope.videoUrl)], src: $scope.videoUrl };
            $scope.model.value = $scope.videoUrl;
        }

        $scope.loaded = false;
        assetsService.load(['/App_Plugins/PMGVideoPlayer/videojs.js', '/App_Plugins/PMGVideoPlayer/Players/youtube.js', '/App_Plugins/PMGVideoPlayer/Players/vimeo.js', '/App_Plugins/PMGVideoPlayer/Players/dailymotion.js']).then(function () {
            var vidid = $scope.model.config.testVideo;
            $scope.loaded = true;
            if ($scope.model.value) {
                $scope.videoUrl = $scope.model.value;
                $scope.setVideo();
            }
        });

    })
   