(function (root) {
    /**
    * @ngdoc controller
    * @name Shield.BackofficeAccess.Edit
    * @function
    *
    * @description
    * Edit Controller for the Backoffice Access Edit view
    */
    angular.module('umbraco').controller('Shield.Editors.BackofficeAccess.Edit',
        ['$scope', 'localizationService',
        function ($scope, localizationService) {

            var vm = this;

            angular.extend(vm, {
                loading: true,
                configuration: $scope.$parent.configuration,
                init: function () {
                    vm.loading = false;
                }
            });
        }]
    );
}(window));