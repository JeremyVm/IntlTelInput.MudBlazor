const inputs = [];

// Map v17 options to v25 options for backward compatibility
function mapOptions(options) {
    const v25Options = {
        allowDropdown: options.AllowDropDown !== undefined ? options.AllowDropDown : true,
        autoPlaceholder: options.AutoPlaceholder || "polite",
        containerClass: options.CustomContainer || "",
        countryOrder: options.PreferredCountries || null,
        countrySearch: true,
        excludeCountries: options.ExcludeCountries || [],
        fixDropdownWidth: true,
        formatAsYouType: true,
        formatOnDisplay: options.FormatOnDisplay !== undefined ? options.FormatOnDisplay : true,
        initialCountry: options.InitialCountry || "",
        i18n: options.LocalizedCountries || {},
        nationalMode: options.NationalMode !== undefined ? options.NationalMode : true,
        onlyCountries: options.OnlyCountries || [],
        placeholderNumberType: options.PlaceholderNumberType || "MOBILE",
        showFlags: true,
        separateDialCode: options.SeparateDialCode || false,
    };

    // Map UtilsScript to loadUtils function
    if (options.UtilsScript) {
        v25Options.loadUtils = () => import(options.UtilsScript);
    }

    return v25Options;
}

export function init(element, helper, options) {
    const v25Options = mapOptions(options);
    const iti = window.intlTelInput(element, v25Options);
    iti.dotNetHelper = helper;
    inputs.push(iti);
    return inputs.indexOf(iti);
}

export function get(id) {
    const input = inputs[id];
    
    const number = input.getNumber();
    const isValid = input.isValidNumber();
    const validationError = input.getValidationError();
    const countryData = input.getSelectedCountryData();
    const extension = input.getExtension();
    const numberType = input.getNumberType();
    
    return {isValid, number, validationError, countryData, extension, numberType};
}

export function setNumber(id, number) {
    const input = inputs[id];
    input.setNumber(number);
}

export function destroy(index) {
    if (inputs[index]) {
        inputs[index].destroy();
        inputs[index] = null;
    }
}