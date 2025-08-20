define(['psApp'], function (psApp) {
    psApp.register.controller("rdController", function ($scope, $timeout) {
        console.log("Controller instantiated (after bootstrap).");

        $scope.radiobtnvalidation = -1;

        $scope.rdClick = function ($event, question, optid, index) {
            $scope.radiobtnvalidation = index;
            question.OptionId = optid;
        };

        // Wait for the ng-repeat to finish rendering
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
    });
});
