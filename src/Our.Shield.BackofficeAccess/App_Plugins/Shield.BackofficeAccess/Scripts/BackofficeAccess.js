(function(root){
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
            configuration: $scope.configuration,
            contentPickerProperty: {
                label: localizationService.localize('Shield.BackofficeAccess.Properties_UnauthorisedUrlLabel'),
                description: localizationService.localize('Shield.BackofficeAccess.Properties_UnauthorisedUrlContentPickerDescription'),
                view: 'contentpicker',
                alias: 'unauthorisedUrlContentPicker',
                config: {
                    multiPicker: "0",
                    entityType: "Document",
                    startNode: {
                        query: "",
                        type: "content",
                        id: -1
                    },
                    filter: "",
                    minNumber: 0,
                    maxNumber: 1
                }
            },
            init: function () {
                vm.contentPickerProperty.value = vm.configuration.unauthorisedUrlContentPicker;

                $scope.$watch('vm.contentPickerProperty.value', function (newVal, oldVal) {
                    vm.configuration.unauthorisedUrlContentPicker = newVal;
                });

                vm.loading = false;
            }
        });
    }]
);
/**
* @ngdoc controller
* @name Shield.Properties.IpAddress
* @function
*
* @description
* Controller to handle the custom IP Address Property Editor
*/
angular.module('umbraco').controller('Shield.Properties.IpAddress',
    ['$scope', 'localizationService',
    function ($scope, localizationService) {

        var vm = this;

        angular.extend(vm, {
            configuration: $scope.configuration,
            init: function () {
                if (vm.configuration.ipAddresses.length === 0) {
                    vm.configuration.ipAddresses.push({
                        ipAddress: '',
                        description: ''
                    });
                }
            },
            add: function () {
                vm.configuration.ipAddresses.push({
                    ipAddress: '',
                    description: ''
                });
            },
            remove: function ($index) {
                var ip = vm.configuration.ipAddresses[$index];

                localizationService.localize('Shield.BackofficeAccess.AlertMessages_ConfirmRemoveIp').then(function (warningMsg) {
                    if (confirm(warningMsg + ip.ipAddress + ' - ' + ip.description)) {
                        vm.configuration.ipAddresses.splice($index, 1);
                    }
                });
            }
        });
    }]
);
/**
   * @ngdoc directive
   * @name ipaddress-valid
   * @function
   *
   * @description
   * Custom angular directive for validating an IP Address
   * as IPv4 or IPv6 with optional cidr
*/
angular.module('umbraco.directives').directive('ipaddressvalid', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elm, attr, ctrl) {
            ctrl.$parsers.push(function (modelValue) {
                if (modelValue === '' || modelValue === undefined) {
                    ctrl.$setValidity('ipaddressvalid', true);
                    return modelValue;
                }

                //Check if IPv4 with optional cidr
                var pattern = /^(?=\d+\.\d+\.\d+\.\d+($|\/))(([1-9]?\d|1\d\d|2[0-4]\d|25[0-5])\.?){4}(\/([0-9]|[1-2][0-9]|3[0-2]))?$/g;

                if (pattern.test(modelValue)) {
                    ctrl.$setValidity('ipaddressvalid', true);
                } else {
                    //Check if IPv6 with optional cidr
                    pattern = /^s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]d|1dd|[1-9]?d)(.(25[0-5]|2[0-4]d|1dd|[1-9]?d)){3}))|:)))(%.+)?s*(\/([0-9]|[1-9][0-9]|1[0-1][0-9]|12[0-8]))?$/g;
                    if (pattern.test(modelValue)) {
                        ctrl.$setValidity('ipaddressvalid', true);
                    } else {
                        ctrl.$setValidity('ipaddressvalid', false);
                    }
                }

                return modelValue
            });
        }
    };
});

/**
   * @ngdoc directive
   * @name ipaddress-duplicate
   * @function
   *
   * @description
   * Checks to make sure an IP address isn't being added more than
   * once to the IP address White-List
*/
angular.module('umbraco.directives').directive('ipaddressduplicate', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, elm, attr, ctrl) {
            ctrl.$parsers.push(function (modelValue) {
                if (modelValue === '' || modelValue === undefined) {
                    ctrl.$setValidity('ipaddressduplicate', true);
                    return modelValue;
                }

                var ipAddresses = angular.fromJson(attr.ipaddressduplicate);

                if (ipAddresses.filter((x) => x.ipAddress === modelValue)[0] !== undefined) {
                    ctrl.$setValidity('ipaddressduplicate', false);
                    return modelValue;
                }

                ctrl.$setValidity('ipaddressduplicate', true);
                return modelValue
            })
        }
    };
});
}(window));