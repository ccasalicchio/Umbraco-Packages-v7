angular.module("umbraco")
    .controller("splatDev.SimpleAnalytics.Controller", ['$http', '$routeParams', '$filter', controller]);

function controller($http, $routeParams, $filter) {
    'use strict';
    const vm = this;
    const apiUrl = '/umbraco/Analytics/AnalyticsApi/';
    vm.nodeId = $routeParams.id;
    vm.stats = {
        collections: [],
        log: null,
        total: 0,
        recurring: 0,
        realTime: 0,
        days: null,
        daysCount: 7,
        entryUrls: [],
        exitUrls: [],
        graph: {
            labels: [],
            data: []
        }
    }
    vm.filter = {
        query: null,
        page: 1,
        pageSize: 10,
        pages: 0
    }
    vm.loading = true;

    function getVisitsEntryUrls() {
        $http.get(`${apiUrl}GetVisitsByEntryUrl`).then(function (response) {
            vm.stats.entryUrls = response.data;
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });
    }

    function getVisitsExityUrls() {
        $http.get(`${apiUrl}GetVisitsByExitUrl`).then(function (response) {
            vm.stats.exitUrls = response.data;
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });
    }

    function getTotalVisits() {
        $http.get(`${apiUrl}GetTotalVisits`).then(function (response) {
            vm.stats.total = +response.data;
            vm.filter.pages = Math.ceil(vm.stats.total / vm.filter.pageSize);
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });
    }

    function getRealTimeVisits() {
        $http.get(`${apiUrl}GetRealTimeVisits`).then(function (response) {
            vm.stats.realTime = +response.data;
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });

    }

    function getRecurringVisits() {
        $http.get(`${apiUrl}GetRecurringVisits`).then(function (response) {
            vm.stats.recurring = +response.data;
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });

    }

    function getByFilter(filter) {
        filter ??= 'IPAddress';
        $http.get(`${apiUrl}GetResultsBy?filter=${filter}`).then(function (response) {
            vm.stats.collections.push(response.data);
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });

    }

    function getXDays(days) {
        days ??= vm.stats.daysCount;
        $http.get(`${apiUrl}GetResultsXDays?days=${days}`).then(function (response) {
            vm.stats.days = response.data;
            vm.stats.graph.labels = vm.stats.days.map(x => $filter('date')(x.Key, "dd/MM/yyyy"));
            vm.stats.graph.data = vm.stats.days.map(x => x.Value);
        },
            function (data, status) {
                console.error('Api Error', status, data);
            });
    }

    vm.getPagedResults = function () {
        let endpoint = `${apiUrl}GetPagedResults`;
        let page = vm.filter.page;
        let pageSize = vm.filter.pageSize;
        let filter = vm.filter.query;

        if (page || pageSize || filter) endpoint += '?';

        if (page) endpoint += `page=${page}`;
        if (pageSize) endpoint += `&pageSize=${pageSize}`;
        if (filter) endpoint += `&ipAddress=${filter}`;

        $http.get(endpoint).then(function (response) {
            vm.stats.log = response.data;
            vm.loading = false;
        },
            function (data, status) {
                console.error('Api Error', status, data);
                vm.loading = false;
            });
    }

    vm.nextPage = function (pageNumber) {
        vm.filter.page = pageNumber;
        vm.getPagedResults()
    }

    vm.prevPage = function (pageNumber) {
        vm.filter.page = pageNumber;
        vm.getPagedResults()
    }

    vm.changePage = function (pageNumber) {
        vm.filter.page = pageNumber;
        vm.getPagedResults()
    }

    vm.goToPage = function (pageNumber) {
        vm.filter.page = pageNumber;
        vm.getPagedResults()
    }

    function init() {
        getByFilter();
        getXDays(7);
        getRealTimeVisits();
        getRecurringVisits();
        getTotalVisits();
        getVisitsEntryUrls();
        getVisitsExityUrls();
        vm.getPagedResults();
        vm.loading = false;
    }

    init();
};