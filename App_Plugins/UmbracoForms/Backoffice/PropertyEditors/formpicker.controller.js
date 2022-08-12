angular.module("umbraco").controller("UmbracoForms.FormPickerController", function ($scope, $http, formResource) {

    //Used to minipulate the Form Object we get back into the simpler form object needed here
    function massageFormDataObject(form) {

        var fields = [];
        var fieldSummary = '';

        //Not sure how to get these fields without this ugly nested loop?
        //For each page in the object
        for (var pageIndex = 0; pageIndex < form.pages.length; pageIndex++) {

            //For each page it will have one or more fieldsets nested
            for (var fieldsetIndex = 0; fieldsetIndex < form.pages[pageIndex].fieldSets.length; fieldsetIndex++)
            {

                //for each fieldset it will have a container
                for (var containerIndex = 0; containerIndex < form.pages[pageIndex].fieldSets[fieldsetIndex].containers.length; containerIndex++)
                {

                    //For each container will have one or more fields
                    for (var fieldIndex = 0; fieldIndex < form.pages[pageIndex].fieldSets[fieldsetIndex].containers[containerIndex].fields.length; fieldIndex++)
                    {
                        var field = form.pages[pageIndex].fieldSets[fieldsetIndex].containers[containerIndex].fields[fieldIndex];

                        //Push the field we find into our new array of just fields only
                        fields.push(field);
                    }
                }
            }
        }

        //Build up field summary
        //Example: Name, Age, Location, Left column and one additional fields
        //Example: Name, Age Location and Left column

        var currentFieldItem = null;

        //Check that we have 4 fields or less
        if (fields.length <= 4) {

            //Loop over first 4 items in fields array
            for (var i = 0; i < fields.length; i++) {

                //Get the current field object in the loop out of array
                currentFieldItem = fields[i];

                //Set the string fieldSummary with the name of the field aka caption
                //If we are not the last item in the array prefix with a comma
                //Otherwise it's an 'and
                if (i !== fields.length - 1) {
                    fieldSummary += currentFieldItem.caption;

                    if (i !== fields.length - 2) {
                        fieldSummary += ', ';
                    }

                }
                else {
                    fieldSummary += ' and ' + currentFieldItem.caption;
                }
            }
        }
        else {
            //Loop over first four items & then append a count of the remaining fields we have
            for (var x = 0; x < 4; x++) {

                //Get the current field object in the loop out of array
                currentFieldItem = fields[x];

                //Set the string fieldSummary with the name of the field aka caption
                fieldSummary += currentFieldItem.caption;

                //If we are NOT the last item use a comma
                //Otherwise it's a comma
                if (x !== fields.length - 1) {
                    fieldSummary += ', ';
                }
            }

            //
            //More than 4 records use - Name, Age, Location, Left column and one additional fiels
            //TODO: Need to use word numbers :'(
            var countExtraFields = fields.length - 4;
            fieldSummary += 'and ' + countExtraFields + ' additional fields';
        }

        var pages = form.pages.length;
        var summary = pages + ' page form with ' + fields.length + ' fields';

        return {
            id: form.id,
            name: form.name,
            fields: fieldSummary,
            summary: summary,
            workflows: form.workflows.length
        };
    };

    $scope.loading = true;
    var selectedForm = null;

    formResource.getOverView().then(function (response) {
        $scope.forms  = response.data;
        $scope.loading = false;
        
        //Only do this is we have a value saved
        if ($scope.model.value) {

            //Try & find picked form from 'model.value' that we save as a 
            //simple GUID out of the collection in forms with _underscore
            selectedForm = _.where(response.data, { id: $scope.model.value });

            if (selectedForm.length === 1) {
                //Found the form from the API (means we currently have access to it)
                $scope.pickedFormName = selectedForm[0].name;
            } else {
                //We have a GUID in model.value saved but it did not come back from the overview API response
                //So this means we do not have access to it, but need to show the form name in the UI

                //Go fetch that specific form by the GUID we have saved
                formResource.getByGuid($scope.model.value).then(function (response) {

                    var form = response.data;

                    //Add the form to the collection of forms - change to same format as API response
                    //Push the item into the array/collection of forms so it can still be selected as a radio button option
                    var formToPush = massageFormDataObject(form);
                    $scope.forms.push(formToPush);
                });
            }
        }

    },
    function (err) {
        $scope.error = "An Error has occured while loading!";
        $scope.loading = false;
    });

    $scope.clear = function () {
        $scope.model.value = null;
    }
});
