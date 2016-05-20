'use strict';
angular.module("umbraco")
    .controller("Social.Media.Channels",
    function ($scope, mediaResource, entityResource) {
        var engine = {
            themes: [],
            selectedIndex: null,
            selectedTheme: {},
            selectTheme: null
        }
        if ($scope.model.value) {
            engine.selectedTheme = $scope.model.value;
            engine.selectedIndex = $scope.model.value.index;
        }
        function selectTheme(index) {
            engine.selectedTheme = engine.themes[index];
            engine.selectedTheme.channels = populateChannels(engine.selectedTheme.Id)
            $scope.model.value = engine.selectedTheme;
        }
        function populateTheme(value, key) {
            return {
                index: key,
                Id: value.id,
                CreateDate: value.properties[0].value,
                Screenshot: value.properties[1].value.src,
                Url: value.properties[2].value,
                CreatedBy: value.properties[3].value,
                ThemeName: value.properties[8].value,
                channels: []
            };
        }

        function populateChannels(theme) {
            var channels = [];
            mediaResource.getChildren(theme)
                .then(function (contentArray) {
                    angular.forEach(contentArray.items, function (value, key) {
                        var fullPath = value.properties[0].value.src;
                        var filename = '';
                        if (fullPath) {
                            var startIndex = (fullPath.indexOf('\\') >= 0 ? fullPath.lastIndexOf('\\') : fullPath.lastIndexOf('/'));
                            filename = fullPath.substring(startIndex);
                            if (filename.indexOf('\\') === 0 || filename.indexOf('/') === 0) {
                                filename = filename.substring(1);
                            }
                        }
                        this.push({
                            name: filename.substr(0, filename.length - 4),
                            img: fullPath,
                            url: '',
                            order: key
                        });
                    }, channels);
                });
            return channels;
        }

        entityResource.search("Social Media Channels Themes", "Media")
            .then(function (mediaArray) {
                var mediaId = mediaArray[0].id;
                mediaResource.getChildren(mediaId)
                    .then(function (contentArray) {
                        angular.forEach(contentArray.items, function (value, key) {
                            this.push(populateTheme(value, key));
                        }, engine.themes);
                    });
            });
        $scope.sortableOptions = {
            axis: 'y',
            cursor: "move",
            handle: ".handle",
            update: function (ev, ui) {
            },
            stop: function (ev, ui) {
            }
        };
        engine.selectTheme = selectTheme;
        $scope.engine = engine;
    });