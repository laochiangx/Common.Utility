using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using $rootnamespace$.Areas.HelpPage.Models;

namespace $rootnamespace$.Areas.HelpPage
{
    public static class HelpPageConfigurationExtensions
    {
        private const string ApiModelPrefix = "MS_HelpPageApiModel_";

        /// <summary>
        /// Sets the documentation provider for help page.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="documentationProvider">The documentation provider.</param>
        public static void SetDocumentationProvider(this HttpConfiguration config, IDocumentationProvider documentationProvider)
        {
            config.Services.Replace(typeof(IDocumentationProvider), documentationProvider);
        }

        /// <summary>
        /// Sets the objects that will be used by the formatters to produce sample requests/responses.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sampleObjects">The sample objects.</param>
        public static void SetSampleObjects(this HttpConfiguration config, IDictionary<Type, object> sampleObjects)
        {
            config.GetHelpPageSampleGenerator().SampleObjects = sampleObjects;
        }

        /// <summary>
        /// Sets the sample request directly for the specified media type and action.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sample">The sample request.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetSampleRequest(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Request, controllerName, actionName, new[] { "*" }), sample);
        }

        /// <summary>
        /// Sets the sample request directly for the specified media type and action with parameters.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sample">The sample request.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetSampleRequest(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Request, controllerName, actionName, parameterNames), sample);
        }

        /// <summary>
        /// Sets the sample request directly for the specified media type of the action.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sample">The sample response.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetSampleResponse(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Response, controllerName, actionName, new[] { "*" }), sample);
        }

        /// <summary>
        /// Sets the sample response directly for the specified media type of the action with specific parameters.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sample">The sample response.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetSampleResponse(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, SampleDirection.Response, controllerName, actionName, parameterNames), sample);
        }

        /// <summary>
        /// Sets the sample directly for all actions with the specified type and media type.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sample">The sample.</param>
        /// <param name="mediaType">The media type.</param>
        /// <param name="type">The parameter type or return type of an action.</param>
        public static void SetSampleForType(this HttpConfiguration config, object sample, MediaTypeHeaderValue mediaType, Type type)
        {
            config.GetHelpPageSampleGenerator().ActionSamples.Add(new HelpPageSampleKey(mediaType, type), sample);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="System.Net.Http.ObjectContent{T}"/> passed to the <see cref="System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate request samples.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetActualRequestType(this HttpConfiguration config, Type type, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Request, controllerName, actionName, new[] { "*" }), type);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="System.Net.Http.ObjectContent{T}"/> passed to the <see cref="System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate request samples.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetActualRequestType(this HttpConfiguration config, Type type, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Request, controllerName, actionName, parameterNames), type);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="System.Net.Http.ObjectContent{T}"/> returned as part of the <see cref="System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate response samples.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        public static void SetActualResponseType(this HttpConfiguration config, Type type, string controllerName, string actionName)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Response, controllerName, actionName, new[] { "*" }), type);
        }

        /// <summary>
        /// Specifies the actual type of <see cref="System.Net.Http.ObjectContent{T}"/> returned as part of the <see cref="System.Net.Http.HttpRequestMessage"/> in an action. 
        /// The help page will use this information to produce more accurate response samples.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="type">The type.</param>
        /// <param name="controllerName">Name of the controller.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="parameterNames">The parameter names.</param>
        public static void SetActualResponseType(this HttpConfiguration config, Type type, string controllerName, string actionName, params string[] parameterNames)
        {
            config.GetHelpPageSampleGenerator().ActualHttpMessageTypes.Add(new HelpPageSampleKey(SampleDirection.Response, controllerName, actionName, parameterNames), type);
        }

        /// <summary>
        /// Gets the help page sample generator.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <returns>The help page sample generator.</returns>
        public static HelpPageSampleGenerator GetHelpPageSampleGenerator(this HttpConfiguration config)
        {
            return (HelpPageSampleGenerator)config.Properties.GetOrAdd(
                typeof(HelpPageSampleGenerator),
                k => new HelpPageSampleGenerator());
        }

        /// <summary>
        /// Sets the help page sample generator.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="sampleGenerator">The help page sample generator.</param>
        public static void SetHelpPageSampleGenerator(this HttpConfiguration config, HelpPageSampleGenerator sampleGenerator)
        {
            config.Properties.AddOrUpdate(
                typeof(HelpPageSampleGenerator),
                k => sampleGenerator,
                (k, o) => sampleGenerator);
        }

        /// <summary>
        /// Gets the model that represents an API displayed on the help page. The model is initialized on the first call and cached for subsequent calls.
        /// </summary>
        /// <param name="config">The <see cref="HttpConfiguration"/>.</param>
        /// <param name="apiDescriptionId">The <see cref="ApiDescription"/> ID.</param>
        /// <returns>
        /// An <see cref="HelpPageApiModel"/>
        /// </returns>
        public static HelpPageApiModel GetHelpPageApiModel(this HttpConfiguration config, string apiDescriptionId)
        {
            object model;
            string modelId = ApiModelPrefix + apiDescriptionId;
            if (!config.Properties.TryGetValue(modelId, out model))
            {
                Collection<ApiDescription> apiDescriptions = config.Services.GetApiExplorer().ApiDescriptions;
                ApiDescription apiDescription = apiDescriptions.FirstOrDefault(api => String.Equals(api.GetFriendlyId(), apiDescriptionId, StringComparison.OrdinalIgnoreCase));
                if (apiDescription != null)
                {
                    HelpPageSampleGenerator sampleGenerator = config.GetHelpPageSampleGenerator();
                    model = GenerateApiModel(apiDescription, sampleGenerator);
                    config.Properties.TryAdd(modelId, model);
                }
            }

            return (HelpPageApiModel)model;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "The exception is recorded as ErrorMessages.")]
        private static HelpPageApiModel GenerateApiModel(ApiDescription apiDescription, HelpPageSampleGenerator sampleGenerator)
        {
            HelpPageApiModel apiModel = new HelpPageApiModel();
            apiModel.ApiDescription = apiDescription;

            try
            {
                foreach (var item in sampleGenerator.GetSampleRequests(apiDescription))
                {
                    apiModel.SampleRequests.Add(item.Key, item.Value);
                    LogInvalidSampleAsError(apiModel, item.Value);
                }

                foreach (var item in sampleGenerator.GetSampleResponses(apiDescription))
                {
                    apiModel.SampleResponses.Add(item.Key, item.Value);
                    LogInvalidSampleAsError(apiModel, item.Value);
                }
            }
            catch (Exception e)
            {
                apiModel.ErrorMessages.Add(String.Format(CultureInfo.CurrentCulture, "An exception has occurred while generating the sample. Exception Message: {0}", e.Message));
            }

            return apiModel;
        }

        private static void LogInvalidSampleAsError(HelpPageApiModel apiModel, object sample)
        {
            InvalidSample invalidSample = sample as InvalidSample;
            if (invalidSample != null)
            {
                apiModel.ErrorMessages.Add(invalidSample.ErrorMessage);
            }
        }
    }
}
