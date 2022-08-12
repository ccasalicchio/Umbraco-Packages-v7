angular.module('umbraco.resources').factory('arknuRelationsResource',
    function ($q, $http) {
    	//the factory object returned
    	return {
    		//this cals the Api Controller we setup earlier
    		getRelations: function (id, type) {
    			var baseurl = Umbraco.Sys.ServerVariables.arknuRelations.relationsApiBase;
    			return $http.get(baseurl + "GetRelations/" + id + "?type=" + type);
    		},
    		getRelationTypes: function (id) {
    			var baseurl = Umbraco.Sys.ServerVariables.arknuRelations.relationsApiBase;
    			return $http.get(baseurl + "GetRelationTypes");
    		},
    		saveRelation: function (sourceid, targetid, type) {
    		    var baseurl = Umbraco.Sys.ServerVariables.arknuRelations.relationsApiBase;
    		    return $http.post(baseurl + "SaveRelation?sourceid=" + sourceid + "&targetid=" + targetid + "&type=" + type);
    		},
    		deleteRelation: function (relationid) {
    		    var baseurl = Umbraco.Sys.ServerVariables.arknuRelations.relationsApiBase;
    		    return $http.delete(baseurl + "DeleteRelation?relationid=" + relationid);
    		}
    	};
    }
);