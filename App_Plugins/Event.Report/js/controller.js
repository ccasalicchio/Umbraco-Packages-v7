'use strict';
angular.module("umbraco")
    .controller("Event.Report",
    function ($scope, $http, $filter, $window, editorState, contentEditingHelper) {
        $scope.eventId = editorState.current.id;
        $scope.Edit = false;
        $scope.posY = 0;
        $scope.loading = false;

        $scope.onLoad = function () {
            $scope.loading = true;
            $http({
                method: 'GET',
                url: '/umbraco/surface/eventregistrationreportsurface/getall/' + $scope.eventId
            }).then(function successCallback(response) {
                var content = editorState.current;
                var properties = contentEditingHelper.getAllProps(content);
                for (var i = 0; i < response.data.length; i++) {
                    //response.data[i].REFCode = _.findWhere(properties, { alias: "EventReferenceCode" }).value + response.data[i].RegistrationNumber;
                    response.data[i].selected = false;
                }

                $scope.registrations = response.data;

                $scope.loading = false;

            }, function errorCallback(response) {
                 $scope.dialog = {
                    Title: response.data, Ok: 'Ok', Cancel: null, Confirm: function () {
                        $scope.dialog = null;
                    }
                }; return false;

            });
        };

        $scope.Export = function () {
            $window.open('/umbraco/surface/eventregistrationreportsurface/ExportToCSV?eventId=' + $scope.eventId, "_blank");
        }

        $scope.PrintAll = function() {
            $window.open('/umbraco/surface/eventregistrationreportsurface/PrintAll?eventId=' + $scope.eventId, "_blank");

        }

        $scope.openForm = function (index) {
            if (index !== null) {
                $scope.selection = $scope.registrations[index];
                $scope.Edit = true;
            }
            else {
                $scope.selection = {};
                $scope.Edit = false;
            }
            $scope.LoadAddons();
        }

        $scope.CancelForm = function () {
            $scope.selection = null;
        }

        $scope.IncludeAddOn = function () {
            var ids = '';
            angular.forEach($scope.AllAddOns, function (addon) {
                ids += addon.Checked ? addon.Id + ',' : '';
            });

                if (ids === '') {
                    $scope.dialog = {
                    Title: 'Nenhum Item Selecionado', Ok: 'Ok', Cancel: null, Confirm: function () {
                        $scope.dialog = null;
                    }
                }; return false;
                }
            return ids;
        }

        $scope.LoadAddons = function () {
            $http({
                method: 'GET',
                url: '/umbraco/surface/eventregistrationreportsurface/GetAvailableAddOns/' + $scope.eventId
            }).then(function successCallback(response) {
                $scope.AllAddOns = response.data;
                if ($scope.selection.AddOnListNames !== undefined) {
                    angular.forEach($scope.AllAddOns,
                        function (addon, index) {
                            if ($scope.AllAddOns[index].Name === $scope.selection.AddOnListNames[index])
                                $scope.AllAddOns[index].Checked = true;
                        });
                }
            }, function errorCallback(response) {
                 $scope.dialog = {
                    Title: response.data, Ok: 'Ok', Cancel: null, Confirm: function () {
                        $scope.dialog = null;
                    }
                }; return false;
            });
        }

        $scope.ConfirmDelete = function () {
            var ids = '';
            angular.forEach($scope.registrations, function (reg) {
                ids += reg.selected ? "ids=" + reg.Id + '&' : '';
            });
            if (ids === '') {
                $scope.dialog = {
                    Title: 'Nenhum Registro Selecionado', Ok: 'Ok', Cancel: null, Confirm: function () {
                        $scope.dialog = null;
                    }
                }; return;
            }
            $scope.dialog = {
                Title: 'Remover Registros?', Ok: 'Sim', Cancel: 'Cancelar',
                Confirm: function () {
                    deleteSelected(ids);
                    $scope.dialog = null;
                }
            };
        }

        $scope.ConfirmDeleteOne = function (id) {
            $scope.dialog = {
                Title: 'Remover Registro?',
                Ok: 'Sim',
                Cancel: 'Cancelar',
                Confirm: function () {
                    deleteOne(id);
                    $scope.dialog = null;
                }
            };
        }

        $scope.Save = function () {
            $scope.selection.AddOns = $scope.IncludeAddOn();
            $scope.selection.EventId = $scope.eventId;
            var fullname = '';
            var url = '';
            var title = '';

            switch ($scope.Edit) {
                case true:
                    url = '/umbraco/surface/eventregistrationreportsurface/update/';
                    title = 'Inscrição atualizada para'
                    break;
                case false:
                    url = '/umbraco/surface/eventregistrationreportsurface/manualregistration/';
                    title = 'Nova Inscrição Inserida com Sucesso para';
                    break;
            }
            if ($scope.selection.Address === undefined ||
                $scope.selection.Postal === undefined ||
                $scope.selection.Document === undefined ||
                $scope.selection.Email1 === undefined ||
                $scope.selection.Phone1 === undefined ||
                $scope.selection.Fullname === undefined) {
                 $scope.dialog = {
                    Title: 'Formulário não preenchido corretamente', Ok: 'Ok', Cancel: null, Confirm: function () {
                        $scope.dialog = null;
                    }
                }; return false;
            }
            if ($scope.selection !== null) {
                $http({
                    method: 'GET',
                    params: $scope.selection,
                    paramSerializer: '$httpParamSerializerJQLike',
                    headers: {
                        'Content-Type': 'application/x-www-form-urlencoded'
                    },
                    url: url
                }).then(function successCallback(response) {
                    fullname = response.data.Fullname;
                    $scope.selection = null;
                    $scope.dialog = null;
                    $scope.onLoad();

                    $scope.dialog = {
                        Title: title,
                        Ok: 'Ok',
                        Cancel: null,
                        Info: fullname,
                        Confirm: function () {
                            $scope.dialog = null;
                        }
                    };
                });
            }
        }
        function deleteSelected(ids) {

            $http({
                method: 'DELETE',
                url: '/umbraco/surface/eventregistrationreportsurface/delete?' + ids.substring(0, ids.length - 1)
            }).then(function successCallback(response) {
                $scope.onLoad();
            });
        }

        function deleteOne(id) {
            $http({
                method: 'DELETE',
                url: '/umbraco/surface/eventregistrationreportsurface/deleteone/' + id
            }).then(function successCallback(response) {

                $scope.onLoad();
            });
        }
        $scope.Reload = function () {
            $scope.onLoad();
        };
        $scope.CheckAll = function () {
            angular.forEach($scope.registrations, function (reg) {
                reg.selected = $scope.selectAll;
            });
        };

        $scope.GetPostal = function () {
            var cep = $scope.selection.Postal.replace(/\D/g, '');
            if ($.trim(cep) !== '') {
                $.getJSON('//viacep.com.br/ws/' + cep + '/json/?callback=?', function (dados) {
                    if (dados.erro === undefined) {
                        $scope.selection.Address = dados.logradouro;
                        $scope.selection.Neighborhood = dados.bairro;
                        $scope.selection.City = dados.localidade;
                        $scope.selection.State = dados.uf;
                        $('.report .status').html('');
                    } else {
                        $('.report .status').html('Não foi possivel encontrar o endereço');
                    }
                });
            }
        };

        $('[data-ctrl=phone1]').mask('(00) 0000-0000');
        $('[data-ctrl=postal]').mask('00000-000');
        $('[data-ctrl=document]').mask('000.000.000-00', { reverse: true });

        var SPMaskBehavior = function (val) {
            return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
        },
            spOptions = {
                onKeyPress: function (val, e, field, options) {
                    field.mask(SPMaskBehavior.apply({}, arguments), options);
                }
            };

        $('[data-ctrl=phone2]').mask(SPMaskBehavior, spOptions);

        $scope.CalSubTotal =
            function (index) {
                var total = 0;
                $scope.AllAddOns[index].Checked = $scope.AllAddOns[index].Checked ? false : true;
                angular.forEach($scope.AllAddOns,
                    function (addon) {
                        if (addon.Checked)
                            total += parseInt(addon.Price);

                    });
                $scope.selection.Total = total;
            }
        $scope.onLoad();
    });