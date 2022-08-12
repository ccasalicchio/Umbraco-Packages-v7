angular.module("umbraco").controller("UmbracoForms.Dashboards.YourFormsController", function ($scope,$location, formResource, recordResource, userService, securityResource, utilityService) {

    var vm = this;

    vm.entriesUrl = 'entries';
		
    //Get the current umbraco version we are using
    var umbracoVersion = Umbraco.Sys.ServerVariables.application.version;
    
    var compareOptions = {
        zeroExtend: true
    };
    
    //Check what version of Umbraco we have is greater than 7.4 or not 
    //So we can load old or new editor UI
    var versionCompare = utilityService.compareVersions(umbracoVersion, "7.4", compareOptions);
    
    //If value is 0 then versions are an exact match
    //If 1 then we are greater than 7.4.x
    //If it's -1 then we are less than 7.4.x
    if(versionCompare < 0) {
        //I am less than 7.4 - load the legacy editor
        vm.entriesUrl = 'entries-legacy';
    }

   vm.formsLimit = 4;

    vm.showAll = function(){
        vm.formsLimit = 50;
    };

    formResource.getOverView().then(function(response){
        vm.forms = response.data;

        _.each(vm.forms, function(form){
            var filter = { form: form.id };

            recordResource.getRecordsCount(filter).then(function(response){
                    form.entries = response.data.count;
            });
        });
    });
});
