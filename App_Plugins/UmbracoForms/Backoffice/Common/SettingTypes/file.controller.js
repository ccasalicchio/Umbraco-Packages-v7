angular.module("umbraco").controller("UmbracoForms.SettingTypes.File",
	function ($scope, dialogService, utilityService) {

		var umbracoVersion = Umbraco.Sys.ServerVariables.application.version;

	    $scope.openMediaPicker = function() {

			var compareOptions = {
				zeroExtend: true
			};

			var versionCompare = utilityService.compareVersions(umbracoVersion, "7.4", compareOptions);

			if(versionCompare === 0 || versionCompare === 1) {

				$scope.mediaPickerOverlay = {
					view: "mediapicker",
					show: true,
					submit: function(model) {

						var selectedImage = model.selectedImages[0];
						populateFile(selectedImage);

						$scope.mediaPickerOverlay.show = false;
						$scope.mediaPickerOverlay = null;
					}
				};

			} else {

				dialogService.mediaPicker({ callback: populateFile });

			}

	    };

	    function populateFile(item) {

	        //From the picked media item - get the 'umbracoFile' property
	        //Previously we was assuming the first property was umbracoFile but if user adds custom propeties then it may not be the first
            //Rather than a for loop, use underscore.js
	        var umbracoFileProp = _.findWhere(item.properties, {alias: "umbracoFile"});

            $scope.setting.value = umbracoFileProp.value;
        }
	});
