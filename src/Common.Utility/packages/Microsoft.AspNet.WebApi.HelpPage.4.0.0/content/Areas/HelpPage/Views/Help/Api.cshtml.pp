@using System.Web.Http
@using $rootnamespace$.Areas.HelpPage.Models
@model HelpPageApiModel

@{
    var description = Model.ApiDescription;
    ViewBag.Title = description.HttpMethod.Method + " " + description.RelativePath;
}

<div id="body">
    <section class="featured">
        <div class="content-wrapper">
            <p>
                @Html.ActionLink("Help Page Home", "Index")
            </p>
        </div>
    </section>
    <section class="content-wrapper main-content clear-fix">
        @Html.DisplayFor(m => Model)
    </section>
</div>

@section Scripts {
    <link type="text/css" href="~/Areas/HelpPage/HelpPage.css" rel="stylesheet" />
}