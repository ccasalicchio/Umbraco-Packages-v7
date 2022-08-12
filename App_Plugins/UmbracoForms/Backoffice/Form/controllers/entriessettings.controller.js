angular.module("umbraco").controller("UmbracoForms.Editors.Form.EntriesSettingsController",
    function($scope, $log, $timeout, exportResource){

       //The Form ID is found in the filter object we pass into the dialog
       var formId = $scope.dialogOptions.filter.form;
        
        exportResource.getExportTypes(formId).then(function(response){
            $scope.exportTypes = response.data;
        });

        $scope.export = function(type, filter){
            filter.exportType = type.id;
            
            exportResource.generateExport(filter).then(function(response){

                var url = exportResource.getExportUrl(response.data.formId, response.data.fileName);
                
                var iframe = document.createElement('iframe');
                iframe.id = "hiddenDownloadframe";
                iframe.style.display = 'none';
                document.body.appendChild(iframe);
                iframe.src= url;

                //remove all traces
                $timeout(function(){
                    document.body.removeChild(iframe);
                }, 1000);
                
            });
        };

    });
