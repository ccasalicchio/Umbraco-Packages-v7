angular.module("umbraco.resources").factory("TagManagerResource", function ($http) {
    return {
        getById: function (id) {
            if (isNaN(id)) {
                return false;
            } else {
                return $http.get("backoffice/TagManager/TagManagerAPI/GetTagById?tagId=" + id);
            }
        },

        getTagGroups: function () {
            return $http.get("backoffice/TagManager/TagManagerAPI/GetTagGroups");
        },

        getAllTagsInGroup: function (groupName) {
            return $http.get("backoffice/TagManager/TagManagerAPI/GetAllTagsInGroup?groupName=" + groupName);
        },
        save: function (cmsTags) {
            return $http.post("backoffice/TagManager/TagManagerAPI/Save", angular.toJson(cmsTags));
        },
        deleteTag: function (cmsTags) {
            return $http.post("backoffice/TagManager/TagManagerAPI/DeleteTag", angular.toJson(cmsTags));
        }
    };
});