//Validation Functions
function addValidator(observable, options) {
    if (!observable.validators && !observable.isValid) {
        observable.validators = ko.observableArray();
        observable.isValid = ko.dependentObservable(function () {
            for (var i = 0; i < observable.validators().length; i++) {
                var validator = observable.validators()[i];
                if (!validator.func(observable(), validator.value))
                    return validator.message;
            }
            return null;
        });
    }
    observable.validators.push(options);
    return observable;
}

function requiredValidator(val) {
    return val && val.length > 0;
}

function maxLengthValidator(val, maxLength) {
    return !val || val.length <= maxLength;
}

ko.extenders.required = function (observable) {
    return addValidator(observable, {
        func: requiredValidator,
        message: 'Required'
    });
};

ko.extenders.maxLength = function (observable, maxLength) {
    return addValidator(observable, {
        func: maxLengthValidator,
        value: maxLength,
        message: 'Too long'
    });
};

ko.bindingHandlers.validator = {
    update: function (element, valueAccessor) {
        var value = valueAccessor().isValid;
        ko.bindingHandlers.text.update(element, function () { return value });
    }
};

function validationSet(observables) {
    return ko.dependentObservable(function () {
        var errors = [];
        for (var i = 0; i < observables.length; i++) {
            var error = observables[i].isValid();
            if (error)
                errors.push(error);
        }
        return errors;
    });
}

function validateEmail(txtEmail) {
    var validEmails = false;
    var str = txtEmail.split(',');
    for (var i = 0; i < str.length; i++) {
        var filter = /^((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*?)\s*;?\s*)+/;
        if (filter.test(str[i])) {
            validEmails = true;
        }
        else {
            validEmails = false;
        }
    }
    return validEmails;
}

function validatePhone(txtPhone) {
    var validPhones = false;
    var str = txtPhone.split(',');
    for (var i = 0; i < str.length; i++) {
        var intRegex = /^\d+$/;
        if (intRegex.test(str[i])) {
            validPhones = true;
        }
        else {
            validPhones = false;
        }
    }
    return validPhones;
}

