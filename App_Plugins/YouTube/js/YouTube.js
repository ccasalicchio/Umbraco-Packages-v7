/*! YouTube - v1.0.4 - 2016-04-21
 * Copyright (c) 2016 Warren Buckley
 * Licensed MIT
 */

angular.module("umbraco").controller("YouTube.channel.controller", function ($scope, YouTubeResource, angularHelper) {

    function debug(message, object){
        //Check we have the console object
        //Some older browsers don't
        if (typeof console === "object") {
            //Now let's check if user set the debug flag on property editor to true
            //In this case a string not a real number of 1 or 0
            var isDebug = $scope.model.config.debug;

            if(isDebug === "1"){
                console.log(message, object);
            }
        }
    }

    //Set Has Videos to false - until we get some back from API call
    $scope.hasVideos        = false;
    $scope.notFoundVideos   = false;

    //Debug message
    debug("Scope Model on init", $scope.model);

    debug("Scope Model Config minmax", $scope.model.config.minmax);

    //Set to be default empty array or value saved
    $scope.model.value = $scope.model.value ? $scope.model.value : [];


    //Try & get videos for grid on Page Load
    YouTubeResource.getChannelVideos($scope.model.config.channel.youtube.channelId, $scope.model.config.orderBy, null, null).then(function(response) {

        //Debug message
        debug("Response Data on init", response.data);

        //Check we have items back from YouTube
        if (response.data.items.length > 0) {

            //Videos
            $scope.videos = response.data;

            //Now we can show the grid of videos
            $scope.hasVideos        = true;
            $scope.notFoundVideos   = false;
        }
        else {
            //No videos - may be searching & found no results
            $scope.notFoundVideos   = true;
        }

    });

    $scope.toggleVideo = function(video) {

        //Create new JSON object as we don't need full object passed in here
        var newVideoObject = {
            "id": video.id.videoId,
            "title": video.snippet.title
        };

        //See if we can find the item or not in the array
        var tryFindItem = $scope.model.value.map(function (e) { return e.id; }).indexOf(newVideoObject.id);

        //Check validity of min & max items
        var minValid = isMinValid();
        var maxValid = isMaxValid();

        //Check to add or remove item
        if (tryFindItem !== -1) {
            
            //Found the item in the array

            //Lets remove it at the index we found it at & remove the single item only
            $scope.model.value.splice(tryFindItem, 1);

        }
        else {

            //Adding item to the collection
            //Item does not exist in the array, let's add it & all OK with validation :)
            $scope.model.value.push(newVideoObject);
        }
    };

    $scope.getPagedVideos = function(pagedToken) {

        //Check we have a paged token
        //May be at beginning or end of list
        //If so don't do anything
        if (pagedToken == null) {
            return;
        }

        //Call getVideos() with our page token
        this.getVideos(pagedToken);
    };

    $scope.getVideos = function (pagedToken) {

        //Set Has Videos to false - until we get some back from API call
        $scope.hasVideos = false;

        //Do new request to API
        YouTubeResource.getChannelVideos($scope.model.config.channel.youtube.channelId, $scope.model.config.orderBy, $scope.searchQuery, pagedToken).then(function (response) {

            //Debug message
            debug("Response Data from GetVideos()", response.data);

            //Check we have items back from YouTube
            if (response.data.items.length > 0) {

                //Videos
                $scope.videos = response.data;

                //Now we can show the grid of videos
                $scope.hasVideos        = true;
                $scope.notFoundVideos   = false;
            }
            else {
                //No videos - may be searching & found no results
                $scope.notFoundVideos   = true;
            }

        });
    };


    $scope.removeVideo = function(videoIndex) {
        debug("Remove video at index",videoIndex);

        //Lets remove it at the index we pass in & remove the single item only
        $scope.model.value.splice(videoIndex, 1);
    };

    $scope.isInArray = function (videoId) {
        //See if we can find the item or not in the array
        var tryFindItem = $scope.model.value.map(function (e) { return e.id; }).indexOf(videoId);

        if (tryFindItem !== -1) {
            //Found it in the array
            return true;
        } else {
            //Could not find it in the array - was -1
            return false;
        }
    };


    //Watch our $scope.model.value of items
    //When items get added or removed - validate...
    $scope.$watch(function() {
        return $scope.model.value;
    }, function(newVal, oldVal) {

        debug("Old Value", oldVal);
        debug("New Value", newVal);

        //Call our validation methods
        isMinValid();
        isMaxValid();

    }, true);

    function isMaxValid() {
        var isMaxEnabled = $scope.model.config.minmax.enableMax;

        if(isMaxEnabled){
            //If it's enabled let's check to see if we reached total items
            var maxItems = parseInt($scope.model.config.minmax.maxValue);

            //Get the current form
            var currentForm = angularHelper.getCurrentForm($scope);
            
            if($scope.model.value.length > maxItems){                    

                //The hidden field in the view set its validity
                currentForm.maxerror.$setValidity('youtubemax', false);

                //Not Valid - False
                return false;
            }
            else {
                //The hidden field in the view set its validity
                //It's valid & OK
                currentForm.maxerror.$setValidity('youtubemax', true);

                //It is valid - True
                return true;
            }
        }

        //Flag not enabled to check for Max items, so always valid
        return true;
    }

    function isMinValid() {
        //Is Min Enabled?
        var isMinEnabled = $scope.model.config.minmax.enableMin;

        if(isMinEnabled){
            //If it's enabled let's check to see if we reached total items
            var minItems = parseInt($scope.model.config.minmax.minValue);

            //Get the current form
            var currentForm = angularHelper.getCurrentForm($scope);

            //If number of items we have is less than minItems we want
            if($scope.model.value.length < minItems){

                //The hidden field in the view set its validity
                currentForm.minerror.$setValidity('youtubemin', false);

                //Not Valid - False
                return false;

            }
            else {
                //The hidden field in the view set its validity
                //All is OK (Have more or equal to minimum number)
                currentForm.minerror.$setValidity('youtubemin', true);

                //It is valid - True
                return true;
            }
        }

        //Flag not enabled to check for Min items, so always valid
        return true;
    }

});

angular.module("umbraco").controller("YouTube.prevalue.channel.controller", function ($scope, YouTubeResource, angularHelper, serverValidationManager) {


    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : null;

    if($scope.model.value){
        //Have a value - so lets assume our JSON object is all good
        //Debug message
        console.log("Scope Model Value on init", $scope.model.value);
    }


    //This is run when the button is clicked
    $scope.queryChannel = function(username) {

        //Default flag for validity
        var isThisValid = false;

        //Query this via our resource
        YouTubeResource.queryUsernameForChannel(username).then(function(response) {

            //Only do this is we have a result back from the API
            if(response.data.items.length > 0){
                //Data we are interested is in
                //response.data.items[0]
                var channel = response.data.items[0];


                //Create new JSON object as we don't need full object from Google's API response
                var newChannelObject = {
                    "username": username,
                    "channelId": channel.id,
                    "title": channel.snippet.title,
                    "description": channel.snippet.description,
                    "thumbnails": channel.snippet.thumbnails,
                    "statistics": channel.statistics
                };

                //Set the value to be our new JSON object
                $scope.model.value.youtube = newChannelObject;

                //Set our flag to true
                isThisValid = true;
            }
            else {
                
                //Set the value to be empty
                $scope.model.value.youtube = null;

                //Ensure flag is set to false
                isThisValid = false;
            }


            //Call our isUsernameValid function
            //With our bool if form is valid or not
            //Will show or hide valdiation if needed
            isUsernameValid(isThisValid);
        });

    };

    //Watch our $scope.model.value
    //When data changes - revalidate...
    $scope.$watch(function() {
        return $scope.model.value;
    }, function(newVal, oldVal) {

        console.log("Old Value", oldVal);
        console.log("New Value", newVal);

        //Call our validation method
        isThisValid();

    }, true);

    function isThisValid(){
        console.log("Is this valid?");

        //Always default to is valid & set to false when we know not valid
        var isValid = true;

        //Check we have a username object
        //May be that a username was querried that does not exist
        if($scope.model.value.youtube !== null && $scope.model.value.youtube.username !== null){
            
            if($scope.model.value.querriedUsername !== $scope.model.value.youtube.username){
                //The username in the textbox is not the same as we have saved in YouTube
                //So means user has typed in a new name but not pressed the query button

                //User has not rechecked new name in textbox
                isValid = false;
            }
        }

        //Run validation to show or remove existing errors
        hasUserRechecked(isValid);

    }

    function isUsernameValid(isValid) {
        //Get the form with Umbraco's helper of this $scope
        //The form is wrapped just around this single prevalue editor
        var form = angularHelper.getCurrentForm($scope);

        //Inside the form we have our input field with the name/id of username
        //Set this field to be valid or invalid based on our flag
        form.username.$setValidity('YouTubeChannel', isValid);


        if(!isValid){
            //Property Alias, Field name (ID/name of text box), Error Message
            serverValidationManager.addPropertyError($scope.model.alias, "username", "The channel/user could not be found on YouTube");
        }
        else {
            //Property Alias, Field name (ID/name of text box)
            serverValidationManager.removePropertyError($scope.model.alias, "username");
        }
    }

    function hasUserRechecked(isValid) {
        //Get the form with Umbraco's helper of this $scope
        //The form is wrapped just around this single prevalue editor
        var form = angularHelper.getCurrentForm($scope);

        //Inside the form we have our input field with the name/id of username
        //Set this field to be valid or invalid based on our flag
        form.username.$setValidity('notchecked', isValid);
    }

});

angular.module("umbraco").controller("YouTube.prevalue.minmax.controller", function ($scope) {

    //Default object if no value saved
    var defaultObject = {
        "enableMin": false,
        "minValue": null,
        "enableMax": false,
        "maxValue": null
    };

    //Set to be default empty object or value saved if we have it
    $scope.model.value = $scope.model.value ? $scope.model.value : defaultObject;

    //This fires when a checkbox is clicked/toggled
    $scope.clearMinValue = function() {

        //If the enabled min is not checked/true
        if(!$scope.model.value.enableMin){
            //Then clear out the value in the textbox for min
            $scope.model.value.minValue = null;
        }
    };

    //This fires when a checkbox is clicked/toggled
    $scope.clearMaxValue = function() {

        //If the enabled max is not checked/true
        if(!$scope.model.value.enableMax){
            //Then clear out the value in the textbox for max
            $scope.model.value.maxValue = null;
        }
    };

});
angular.module('umbraco.resources')
.factory('YouTubeResource',
['$http',
function ($http) {

    //Base API URL
    var apiUrl = Umbraco.Sys.ServerVariables["YouTube"]["ApiUrl"];

    //the factory object returned
    return {

        getChannelVideos: function (channelId, orderBy, searchQuery, pageToken) {
            return $http.post(apiUrl + "VideosForChannel", { pageToken: pageToken, channelId: channelId, searchQuery: searchQuery, orderBy: orderBy });
        },

        queryUsernameForChannel: function(usernameToQuery) {
        	return $http.get(apiUrl + "ChannelFromUsername?username=" + usernameToQuery);
        }

    };
}]);
