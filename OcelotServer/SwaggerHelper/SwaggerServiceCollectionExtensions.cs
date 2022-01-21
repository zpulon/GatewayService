using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace OcelotServer.SwaggerHelper
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static List<VersionItem> versions = new List<VersionItem>
        {
              new VersionItem
            {
                Version = "v1",
                Name = "OnlineSchool"
            },
             new VersionItem
            {
                Version = "LearningMethod",
                Name = "LearningMethod"
            },
             new VersionItem
            {
                Version = "KnowledgeMaterial",
                Name = "KnowledgeMaterial"
            },
            new VersionItem
             {
                Version = "Examine",
                Name = "Examine"
            },
            new VersionItem
             {
                Version = "Knowledge",
                Name = "Knowledge"
            },
             new VersionItem
             {
                Version = "English",
                Name = "English"
            },
              new VersionItem
            {
                Version = "v2",
                Name = "OnlineSchool"
            },
        };
        /// <summary>
        /// 配置添加 Swagger 服务
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
#if DEBUG
                versions.ForEach(apiVersion =>
                {
                    c.SwaggerDoc(apiVersion.Version, new OpenApiInfo
                    {
                        Version = apiVersion.Version,
                        Title = $"{apiVersion.Version} 接口文档",
                        Description = $" HTTP API : {apiVersion.Version}",
                    });
                    //按Http类型排序
                    c.OrderActionsBy(o => o.GroupName);
                });
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;
                    var versions = methodInfo.DeclaringType.GetCustomAttributes(true).OfType<ApiExplorerSettingsAttribute>().Select(attr => attr.GroupName);
                    if (docName.ToLower() == "v1" && versions.FirstOrDefault() == null)
                        return true;
                    return versions.Any(v => v.ToString() == docName);
                });
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //参数添加在头部
                    In = ParameterLocation.Header,
                    //使用Authorize头部
                    Type = SecuritySchemeType.Http,
                    //内容为以 bearer开头
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                //把所有方法配置为增加bearer头部信息
                var securityRequirement = new OpenApiSecurityRequirement
               {
                    {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "bearerAuth"
                                }
                            },
                            new string[] {}
                    }
               };
                //注册到swagger中
                c.AddSecurityDefinition("bearerAuth", securityScheme);
                c.AddSecurityRequirement(securityRequirement);
#else
               c.SwaggerDoc("v1",
                   new OpenApiInfo
                   {
                       Title = "OnlineSchool API 1.0",
                       Version = "v1",
                       Description = $" 智慧教室Api-Vserion:1.0"
                   });
               c.SwaggerDoc("v2",
                  new OpenApiInfo
                  {
                      Title = "OnlineSchool API 2.0",
                      Version = "v2",
                      Description = $" 智慧教室Api-Vserion:2.0"
                  });
               c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
#endif
                DirectoryInfo dirs = new DirectoryInfo(AppContext.BaseDirectory);
                FileInfo[] files = dirs.GetFiles("*.xml");
                foreach (var path in files)
                {
                    c.IncludeXmlComments(path.FullName);
                }

            });
            return services;
        }
    }
}
