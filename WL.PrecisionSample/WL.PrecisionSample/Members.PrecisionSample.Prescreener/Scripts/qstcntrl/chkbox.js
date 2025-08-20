define(['psApp'], function (psApp) {
	psApp.register.controller("chkBoxController", function ($scope, $timeout) {
		console.log("Controller instantiated (after bootstrap).");
		$scope.chekboxValidation = $scope.chekboxValidation || [];
		$scope.chkboxClick = function (event, option, index) {
			if (option.IsChecked) {
				option.IsChecked = false;
				if ($scope.chekboxValidation.indexOf(option.OptionId) != -1) {
					var currentIndex = $scope.chekboxValidation.indexOf(option.OptionId);
					$scope.chekboxValidation.splice(currentIndex, 1);
				}

			}
			else {
				option.IsChecked = true;
				if ($scope.chekboxValidation.indexOf(option.OptionId) == -1) {
					$scope.chekboxValidation.push(option.OptionId);
				}
			}
			event.stopPropagation()
		}
		$timeout(function () {
			applyEqualHeight();
			window.addEventListener('resize', applyEqualHeight); // Optional: for responsive design
		}, 0);

		function applyEqualHeight() {
			const boxes = document.querySelectorAll('.dvChkbox.dvstyle');
			if (window.innerWidth <= 760) {
				boxes.forEach(box => {
					box.style.height = 'auto'; // Reset height on small screens
				});
				return;
			}
			let maxHeight = 0;

			boxes.forEach(box => {
				box.style.height = 'auto'; // Reset to natural height
				if (box.offsetHeight > maxHeight) {
					maxHeight = box.offsetHeight;
				}
			});

			boxes.forEach(box => {
				box.style.height = `${maxHeight}px`;
			});
		}
	})
});