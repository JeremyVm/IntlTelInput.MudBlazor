const inputs = [];

// Map v17 options to v25 options for backward compatibility
// Note: C# serializes property names as camelCase (e.g., UtilsScript -> utilsScript)
function mapOptions(options) {
    const v25Options = {
        allowDropdown: options.allowDropDown !== undefined ? options.allowDropDown : true,
        autoPlaceholder: options.autoPlaceholder || "polite",
        containerClass: options.customContainer || "",
        countryOrder: options.preferredCountries || null,
        countrySearch: true,
        excludeCountries: options.excludeCountries || [],
        fixDropdownWidth: true,
        formatAsYouType: true,
        formatOnDisplay: options.formatOnDisplay !== undefined ? options.formatOnDisplay : true,
        initialCountry: options.initialCountry || "",
        i18n: options.localizedCountries || {},
        nationalMode: options.nationalMode !== undefined ? options.nationalMode : true,
        onlyCountries: options.onlyCountries || [],
        placeholderNumberType: options.placeholderNumberType || "MOBILE",
        showFlags: true,
        separateDialCode: options.separateDialCode || false,
        // Accept all number types for validation (mobile, fixed-line, toll-free, etc.)
        validationNumberTypes: ["FIXED_LINE", "MOBILE", "FIXED_LINE_OR_MOBILE"],
    };

    // Add geoIpLookup when initialCountry is "auto"
    if (options.initialCountry === "auto") {
        v25Options.geoIpLookup = (success, failure) => {
            fetch("https://ipapi.co/json/")
                .then(res => res.json())
                .then(data => success(data.country_code))
                .catch(() => failure());
        };
    }

    // Load utils via dynamic import - it's an ES module with default export
    if (options.utilsScript) {
        v25Options.loadUtils = () => {
            return import(options.utilsScript);
        };
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

export async function get(id) {
    const input = inputs[id];

    // Wait for async init to complete
    await input.promise;

    // Get raw input value for debugging
    const rawValue = input.telInput?.value;
    const number = input.getNumber();
    const isValid = input.isValidNumber();
    const validationError = input.getValidationError();
    const countryData = input.getSelectedCountryData();
    const extension = input.getExtension();
    const numberType = input.getNumberType();

    return {isValid: isValid ?? false, number, validationError, countryData, extension, numberType};
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