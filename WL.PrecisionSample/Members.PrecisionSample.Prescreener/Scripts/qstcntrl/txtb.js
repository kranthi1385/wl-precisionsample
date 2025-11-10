define(['psApp'], function (psApp) {
	psApp.register.controller("textBoxController", function ($scope, $compile) {

		$scope.invalidEmail = false;
		$scope.alreadyChecked = false;
		$scope.validateEmail = function () {
			var fieldName = $scope.question.QuestionId + '_txtbox';
			var field = $scope.formValidation[fieldName];
			if (field.$valid || $scope.invalidEmail || $scope.alreadyChecked) {
				var regex = /^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/;
				$scope.invalidEmail = !regex.test($scope.question.OptionText);
				if ($scope.invalidEmail) {
					field.$setValidity('emailapi', false);
					$scope.alreadyChecked = true;
				}
				else {
					field.$setValidity('emailapi', true);
					$scope.alreadyChecked = false
				}
			}
		}
	});
});