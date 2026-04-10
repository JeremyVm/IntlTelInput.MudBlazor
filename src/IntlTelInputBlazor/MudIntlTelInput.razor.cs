using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static MudBlazor.Colors;

namespace IntlTelInputBlazor;

public class Converter<T> : IReversibleConverter<T, string>
{
    public string Convert(T input)
    {
        return (input as IntlTel)?.Number;
    }

    public T ConvertBack(string input)
    {
        return (T)(object)new IntlTel { Number = input ?? string.Empty };
    }
}

public partial class MudIntlTelInput<T> : MudDebouncedInput<T>, IDisposable
{
    private DotNetObjectReference<MudIntlTelInput<IntlTel>> _dotNetHelper;

    private int _inputIndex = -1;

    public MudIntlTelInput()
    {
        Converter = new Converter<T>();
    }

    [Parameter] public bool AllowDropDown { get; set; } = true;

    [Parameter] public bool AutoHideDialCode { get; set; } = true;

    [Parameter] public string AutoPlaceholder { get; set; } = "polite";

    /// <summary>
    ///     Show clear button.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public bool Clearable { get; set; }

    [Parameter] public string CustomContainer { get; set; }

    [Parameter] public IEnumerable<string> ExcludeCountries { get; set; } = Enumerable.Empty<string>();

    [Parameter] public bool FormatOnDisplay { get; set; } = true;

    [Parameter] public string InitialCountry { get; set; }

    public MudInput<string> InputReference { get; private set; }

    /// <summary>
    ///     Type of the input element. It should be a valid HTML5 input type.
    /// </summary>
    [Parameter]
    [Category(CategoryTypes.FormComponent.Behavior)]
    public InputType InputType { get; set; } = InputType.Telephone;

    [Parameter] public Dictionary<string, string> LocalizedCountries { get; set; }

    [Parameter] public bool NationalMode { get; set; } = true;

    /// <summary>
    ///     Button click event for clear button. Called after text and value has been cleared.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClearButtonClick { get; set; }

    [Parameter] public IEnumerable<string> OnlyCountries { get; set; } = Enumerable.Empty<string>();

    [Parameter] public string PlaceholderNumberType { get; set; } = "MOBILE";

    [Parameter]
    public IEnumerable<string> PreferredCountries { get; set; } =
        new[] { RegionInfo.CurrentRegion.TwoLetterISORegionName };

    [Parameter] public bool SeparateDialCode { get; set; }

    [Parameter] public string UtilsScript { get; set; } = "/_content/IntlTelInputBlazor/js/utils.js";

    [CascadingParameter(Name = "SubscribeToParentForm")]
    internal bool SubscribeToParentFormEx { get; set; } = true;

    protected string Classname =>
        new CssBuilder("mud-input-input-control")
            .AddClass(Class)
            .AddClass("mud-input-iti")
            .Build();

    public override ValueTask BlurAsync()
    {
        return InputReference.BlurAsync();
    }

    /// <summary>
    ///     Clear the text field, set Value to default(T) and Text to null
    /// </summary>
    /// <returns></returns>
    public Task Clear()
    {
        return InputReference.SetText(null);
    }

    public void Dispose()
    {
        if (_inputIndex >= 0) _ = _intlTelInputJsInterop.Destroy(_inputIndex);
        _dotNetHelper?.Dispose();
    }

    public override ValueTask FocusAsync()
    {
        return InputReference.FocusAsync();
    }

    public override ValueTask SelectAsync()
    {
        return InputReference.SelectAsync();
    }

    public override ValueTask SelectRangeAsync(int pos1, int pos2)
    {
        return InputReference.SelectRangeAsync(pos1, pos2);
    }

    /// <summary>
    ///     Sets the input text from outside programmatically
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task SetText(string text)
    {
        if (InputReference != null)
            await InputReference.SetText(text);
    }

    [JSInvokable]
    public async Task Update(string input)
    {
        var value = (T)(object)await _intlTelInputJsInterop.GetData(_inputIndex);

        if (value is not null)
        {
            await _intlTelInputJsInterop.SetNumber(_inputIndex, value.ToString());
        }

        await base.SetValueAndUpdateTextAsync(value);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            if (typeof(T) != typeof(IntlTel))
                throw new InvalidOperationException(
                    $"MudIntlTelInput only supports IntlTel type parameter. Got: {typeof(T).Name}");
            _dotNetHelper = DotNetObjectReference.Create(this) as DotNetObjectReference<MudIntlTelInput<IntlTel>>;

            _inputIndex = await _intlTelInputJsInterop.Init(InputReference.ElementReference, _dotNetHelper, new
            {
                AllowDropDown,
                AutoHideDialCode,
                AutoPlaceholder,
                CustomContainer,
                ExcludeCountries,
                FormatOnDisplay,
                InitialCountry,
                LocalizedCountries,
                NationalMode,
                OnlyCountries,
                PlaceholderNumberType,
                PreferredCountries,
                SeparateDialCode,
                UtilsScript
            });

            if (Value?.ToString() is not null) await _intlTelInputJsInterop.SetNumber(_inputIndex, Value.ToString());
        }
    }

    protected override async Task ResetValueAsync()
    {
        await InputReference.ResetAsync();

        await base.ResetValueAsync();
    }

    protected override Task SetTextAndUpdateValueAsync(string text, bool updateValue = true)
    {
        Text = text;
        return Task.CompletedTask;
    }

    private string GetCounterText()
    {
        return Counter == null ? string.Empty :
            Counter == 0 ? string.IsNullOrEmpty(Text) ? "0" : $"{Text.Length}" :
            (string.IsNullOrEmpty(Text) ? "0" : $"{Text.Length}") + $" / {Counter}";
    }

    private async Task OnInput(string? input)
    {
        await Update(input);
    }
}