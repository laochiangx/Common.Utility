@using System.Web.Http
@using System.Web.Http.Description
@using $rootnamespace$.Areas.HelpPage
@using $rootnamespace$.Areas.HelpPage.Models
@model IGrouping<string, ApiDescription>

<h2 id="@Model.Key">@Model.Key</h2>
<table class="help-page-table">
    <thead>
        <tr><th>API</th><th>Description</th></tr>
    </thead>
    <tbody>
    @foreach (var api in Model)
    {
        <tr>
            <td class="api-name"><a href="@Url.Action("Api", "Help", new { apiId = api.GetFriendlyId() })">@api.HttpMethod.Method @api.RelativePath</a></td>
            <td class="api-documentation">
            @if (api.Documentation != null)
            {
                <p>@api.Documentation</p>
            }
            else
            {
                <p>No documentation available.</p>
            }
            </td>
        </tr>
    }
    </tbody>
</table>