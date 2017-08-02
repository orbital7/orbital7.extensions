// RequiredIf, RegularExpressionIf, RangeIf attribute client-side validation.
$(['requiredif', 'regularexpressionif', 'rangeif']).each(function (index, validationName) {
	$.validator.addMethod(validationName,
            function (value, element, parameters) {
            	// Get the name prefix for the target control, depending on the validated control name
            	var prefix = "";
            	var lastDot = element.name.lastIndexOf('.');
            	if (lastDot != -1) {
            		prefix = element.name.substring(0, lastDot + 1).replace('.', '_');
            	}
            	var id = '#' + prefix + parameters['dependentproperty'];
            	// get the target value
            	var targetvalue = parameters['targetvalue'];
            	targetvalue = (targetvalue == null ? '' : targetvalue).toString();
            	// get the actual value of the target control
            	var control = $(id);
            	if (control.length == 0 && prefix.length > 0) {
            		// Target control not found, try without the prefix
            		control = $('#' + parameters['dependentproperty']);
            	}
            	if (control.length > 0) {
            		var controltype = control.attr('type');
            		var actualvalue = "";
            		switch (controltype) {
            			case 'checkbox':
            				actualvalue = control.attr('checked').toString(); break;
            			case 'select':
            				actualvalue = $('option:selected', control).text(); break;
            			default:
            				actualvalue = control.val(); break;
            		}
            		// if the condition is true, reuse the existing validator functionality
            		if (targetvalue.toLowerCase() === actualvalue.toLowerCase()) {
            			var rule = parameters['rule'];
            			var ruleparam = parameters['ruleparam'];
            			return $.validator.methods[rule].call(this, value, element, ruleparam);
            		}
            	}
            	return true;
            }
        );

	$.validator.unobtrusive.adapters.add(validationName, ['dependentproperty', 'targetvalue', 'rule', 'ruleparam'], function (options) {
		var rp = options.params['ruleparam'];
		options.rules[validationName] = {
			dependentproperty: options.params['dependentproperty'],
			targetvalue: options.params['targetvalue'],
			rule: options.params['rule']
		};
		if (rp) {
			options.rules[validationName].ruleparam = rp.charAt(0) == '[' ? eval(rp) : rp;
		}
		options.messages[validationName] = options.message;
	});
});


// GenericCompare attribute client-side validation.
$.validator.addMethod("genericcompare", function (value, element, params) {
    // debugger;
    var propelename = params.split(",")[0];
    var operName = params.split(",")[1];
    if (params == undefined || params == null || params.length == 0 ||
    value == undefined || value == null || value.length == 0 ||
    propelename == undefined || propelename == null || propelename.length == 0 ||
    operName == undefined || operName == null || operName.length == 0)
        return true;
    var valueOther = $(propelename).val();
    var val1 = (isNaN(value) ? Date.parse(value) : eval(value));
    var val2 = (isNaN(valueOther) ? Date.parse(valueOther) : eval(valueOther));

    if (operName == "GreaterThan")
        return val1 > val2;
    if (operName == "LessThan")
        return val1 < val2;
    if (operName == "GreaterThanOrEqual")
        return val1 >= val2;
    if (operName == "LessThanOrEqual")
        return val1 <= val2;
});
$.validator.unobtrusive.adapters.add("genericcompare",
["comparetopropertyname", "operatorname"], function (options) {
    options.rules["genericcompare"] = "#" +
    options.params.comparetopropertyname + "," + options.params.operatorname;
    options.messages["genericcompare"] = options.message;
});

// NotEqualTo attribute client-side validation.
$.validator.unobtrusive.adapters.add(
        'notequalto', ['other'], function (options) {
            options.rules['notEqualTo'] = '#' + options.params.other;
            if (options.message) {
                options.messages['notEqualTo'] = options.message;
            }
        });
$.validator.addMethod('notEqualTo', function (value, element, param) {
    return this.optional(element) || value != $(param).val();
}, '');

if ($.validator && $.validator.unobtrusive) {
    $.validator.addMethod('valuegreaterthan', function (value, element, params) {
        value = parseFloat(value);
        var otherValue = parseFloat($(params.compareTo).val());
        if (isNaN(value) || isNaN(otherValue))
            return true;
        return value > otherValue || (value == otherValue && params.allowEqualValues);
    });
    $.validator.unobtrusive.adapters.add('valuegreaterthan', ['propertyname', 'allowequalvalues'], function (options) {
        options.rules['valuegreaterthan'] = {
            'allowEqualValues': options.params['allowequalvalues'],
            'compareTo': '#' + options.params['propertyname']
        };
        options.messages['valuegreaterthan'] = options.message;
    });
}

if ($.validator && $.validator.unobtrusive) {
    $.validator.addMethod('valuelessthan', function (value, element, params) {
        value = parseFloat(value);
        var otherValue = parseFloat($(params.compareTo).val());
        if (isNaN(value) || isNaN(otherValue))
            return true;
        return value < otherValue || (value == otherValue && params.allowEqualValues);
    });
    $.validator.unobtrusive.adapters.add('valuelessthan', ['propertyname', 'allowequalvalues'], function (options) {
        options.rules['valuelessthan'] = {
            'allowEqualValues': options.params['allowequalvalues'],
            'compareTo': '#' + options.params['propertyname']
        };
        options.messages['valuelessthan'] = options.message;
    });
}