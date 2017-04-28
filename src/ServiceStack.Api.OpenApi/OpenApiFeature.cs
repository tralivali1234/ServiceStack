﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ServiceStack.Host.Handlers;
using ServiceStack.IO;
using ServiceStack.Api.OpenApi.Specification;

namespace ServiceStack.Api.OpenApi
{
    public class OpenApiFeature : IPlugin, IPreInitPlugin
    {
        /// <summary>
        /// Gets or sets <see cref="Regex"/> pattern to filter available resources. 
        /// </summary>
        public string ResourceFilterPattern { get; set; }

        public bool UseCamelCaseSchemaPropertyNames { get; set; }

        public bool UseLowercaseUnderscoreSchemaPropertyNames { get; set; }

        public bool DisableAutoDtoInBodyParam { get; set; }

        public string LogoUrl { get; set; }

        public Action<OpenApiDeclaration> ApiDeclarationFilter { get; set; }

        /// <summary>
        /// Operation filter. Action takes a verb and operation as parameters
        /// </summary>
        public Action<string, OpenApiOperation> OperationFilter { get; set; }

        public Action<OpenApiSchema> SchemaFilter { get; set; }

        public Action<OpenApiProperty> SchemaPropertyFilter { get; set; }
        
        public Dictionary<string, string> RouteSummary { get; set; }

        public List<string> AnyRouteVerbs { get; set; }

        public OpenApiFeature()
        {
            LogoUrl = "//raw.githubusercontent.com/ServiceStack/Assets/master/img/artwork/logo-24.png";
            RouteSummary = new Dictionary<string, string>();
            AnyRouteVerbs = new List<string> { HttpMethods.Get, HttpMethods.Post, HttpMethods.Put, HttpMethods.Delete };
        }

        public void Configure(IAppHost appHost)
        {
            appHost.Config.EmbeddedResourceSources.Add(typeof(OpenApiFeature).GetAssembly());
        }

        public void Register(IAppHost appHost)
        {
            if (ResourceFilterPattern != null)
                OpenApiService.resourceFilterRegex = new Regex(ResourceFilterPattern, RegexOptions.Compiled);

            OpenApiService.UseCamelCaseSchemaPropertyNames = UseCamelCaseSchemaPropertyNames;
            OpenApiService.UseLowercaseUnderscoreSchemaPropertyNames = UseLowercaseUnderscoreSchemaPropertyNames;
            OpenApiService.DisableAutoDtoInBodyParam = DisableAutoDtoInBodyParam;
            OpenApiService.ApiDeclarationFilter = ApiDeclarationFilter;
            OpenApiService.OperationFilter = OperationFilter;
            OpenApiService.SchemaFilter = SchemaFilter;
            OpenApiService.SchemaPropertyFilter = SchemaPropertyFilter;
            OpenApiService.AnyRouteVerbs = AnyRouteVerbs.ToArray();

            appHost.RegisterService(typeof(OpenApiService), "/openapi");

            var swaggerUrl = "swagger-ui/";

            appHost.GetPlugin<MetadataFeature>()
                .AddPluginLink(swaggerUrl, "Swagger UI");

            appHost.CatchAllHandlers.Add((httpMethod, pathInfo, filePath) =>
            {
                IVirtualFile indexFile;
                IVirtualFile patchFile = null;
                switch (pathInfo)
                {
                    case "/swagger-ui":
                    case "/swagger-ui/":
                    case "/swagger-ui/default.html":
                        indexFile = appHost.VirtualFileSources.GetFile("/swagger-ui/index.html");
                        patchFile = appHost.VirtualFileSources.GetFile("/swagger-ui/patch.js");
                        break;
                    default:
                        indexFile = null;
                        break;
                }
                if (indexFile != null)
                {
                    var html = indexFile.ReadAllText();
                    var injectJs = patchFile?.ReadAllText();

                    return new CustomResponseHandler((req, res) =>
                    {
                        res.ContentType = MimeTypes.Html;
                        var resourcesUrl = req.ResolveAbsoluteUrl("~/openapi");
                        html = html.Replace("http://petstore.swagger.io/v2/swagger.json", resourcesUrl)
                            .Replace("ApiDocs", HostContext.ServiceName)
                            .Replace("{LogoUrl}", LogoUrl);

                        if (injectJs != null)
                        {
                            html = html.Replace("</body>",
                                "<script type='text/javascript'>" + injectJs + "</script></body>");
                        }

                        return html;
                    });
                }
                return pathInfo.StartsWith("/swagger-ui") ? new StaticFileHandler() : null;
            });
        }

        public static bool IsEnabled => HostContext.HasPlugin<OpenApiFeature>();
    }
}