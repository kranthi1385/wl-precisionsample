define(['psApp'], function (psApp) {
    psApp.register.controller("rdMatrixController", function ($scope, $timeout) {
        console.log("Controller instantiated (after bootstrap).");
        $scope.rdMatrixClick = function (Parent, question, optid, index) {
            $scope.radiobtnvalidation = index;
            question.OptionId = optid;
            question.ChqOptionId = optid;
            question.RdMatrixIndex = index;
        }

        // Wait for the ng-repeat to finish rendering
        $timeout(function () {
            applyEqualHeight();
            window.addEventListener('resize', applyEqualHeight); // Optional: for responsive design
        }, 0);

        function applyEqualHeight() {
            const boxes = document.querySelectorAll('.matrix_column');
            //if (window.innerWidth <= 760) {
            //	boxes.forEach(box => {
            //		box.style.height = 'auto'; // Reset height on small screens
            //	});
            //	return;
            //}
            let maxHeight = 0;
            if (boxes.length > 48) {
                maxHeight = 80;
                if (window.innerWidth <= 760) {
                    maxHeight = 0;
                } else {
                    boxes.forEach(box => {
                        box.style.width = '105px !important';
                    });
                }
            }
            else {
                maxWidth = 165;
                boxes.forEach(box => {
                    if (window.innerWidth <= 511) {
                        box.style.width = '100%';
                    } else {
                        box.style.width = `${maxWidth}px`;
                    }
                });
            }

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