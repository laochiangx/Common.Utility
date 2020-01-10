@using System.Web.Http
@using $rootnamespace$.Areas.HelpPage.Models
@model HelpPageApiModel

@{ 
    var description = Model.ApiDescription;
    bool hasParameters = description.ParameterDescriptions.Count > 0;
    bool hasRequestSamples = Model.SampleRequests.Count > 0;
    bool hasResponseSamples = Model.SampleResponses.Count > 0;
}
<h1>@description.HttpMethod.Method @description.RelativePath</h1>
<div>
    @if (description.Documentation != null)
    {
        <p>@description.Documentation</p>
    }
    else
    {
        <p>No documentation available.</p>
    }

    @if (hasParameters || hasRequestSamples)
    {
        <h2>Request Information</h2>
        if (hasParameters)
        {
            <h3>Parameters</h3>
            @Html.DisplayFor(apiModel => apiModel.ApiDescription.ParameterDescriptions, "Parameters")
        }
        if (hasRequestSamples)
        {
            <h3>Request body formats</h3>
            @Html.DisplayFor(apiModel => apiModel.SampleRequests, "Samples")
        }
    } 

    @if (hasResponseSamples)
    {
        <h2>Response Information</h2> 
        <h3>Response body formats</h3>
        @Html.DisplayFor(apiModel => apiModel.SampleResponses, "Samples")
    }
</div>