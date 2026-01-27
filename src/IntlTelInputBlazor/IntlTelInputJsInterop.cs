using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace IntlTelInputBlazor;

public class IntlTelInputJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private IJSObjectReference _module;

    public IntlTelInputJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => jsRuntime.InvokeAsync<IJSObjectReference>(
            "import", "/_content/IntlTelInputBlazor/js/intlTelInputInterop.js").AsTask());
    }

    public async ValueTask Destroy(int inputIndex)
    {
        if (_moduleTask.IsValueCreated)
        {
            _module ??= await _moduleTask.Value;
            await _module.InvokeVoidAsync("destroy", inputIndex);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            _module ??= await _moduleTask.Value;
            await _module.DisposeAsync();
        }
    }

    public async ValueTask<IntlTel> GetData(int inputIndex)
    {
        _module ??= await _moduleTask.Value;
        return await _module.InvokeAsync<IntlTel>("get", inputIndex);
    }

    public async ValueTask<int> Init<T>(ElementReference reference, DotNetObjectReference<T> dotNetHelper,
        object options)
        where T : class
    {
        _module = await _moduleTask.Value;
        return await _module.InvokeAsync<int>("init", reference, dotNetHelper, options);
    }

    public async ValueTask SetNumber(int id, string number)
    {
        _module ??= await _moduleTask.Value;
        await _module.InvokeVoidAsync("setNumber", id, number);
    }
}