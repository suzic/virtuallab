using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;

namespace virtuallab
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 配置和服务
            config.Services.Replace(typeof(IContentNegotiator), new JsonContentNegotiator(new JsonMediaTypeFormatter()));

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }

    /// <summary>
    ///  在全局设置中，使用自定义的只返回Json Result。只让api接口中替换xml，返回json。这种方法的性能是最高的！
    /// </summary>
    public class JsonContentNegotiator : IContentNegotiator
    {
        private readonly JsonMediaTypeFormatter _jsonFormatter;
        public JsonContentNegotiator(JsonMediaTypeFormatter formatter)
        {
            _jsonFormatter = formatter;
        }
        public ContentNegotiationResult Negotiate(Type type, HttpRequestMessage request, IEnumerable<MediaTypeFormatter> formatters)
        {
            // 对 JSON 数据使用混合大小写。驼峰式,但是是javascript 首字母小写形式.小驼峰命名法。
            //config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new  CamelCasePropertyNamesContractResolver();
            // 对 JSON 数据使用混合大小写。跟属性名同样的大小.输出
            //_jsonFormatter.SerializerSettings.ContractResolver = new UnderlineSplitContractResolver(); //小写命名法。
            _jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//解决json时间带T的问题
            _jsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;//解决json格式化缩进问题
            _jsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;//解决json序列化时的循环引用问题
            var result = new ContentNegotiationResult(_jsonFormatter, new MediaTypeHeaderValue("application/json"));
            return result;
        }
    }

    /// <summary>
    /// Json.NET 利用ContractResolver解决命名不一致问题
    /// 解决问题：通过无论是序列化还是反序列化都达到了效果，即：ProjectName -> project_name 和 project_name -> ProjectName
    /// </summary>
    public class UnderlineSplitContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return CamelCaseToUnderlineSplit(propertyName);//下划线分割命名法
            //return propertyName.ToLower();//小写命名法
        }

        private string CamelCaseToUnderlineSplit(string name)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                var ch = name[i];
                if (char.IsUpper(ch) && i > 0)
                {
                    var prev = name[i - 1];
                    if (prev != '_')
                    {
                        if (char.IsUpper(prev))
                        {
                            if (i < name.Length - 1)
                            {
                                var next = name[i + 1];
                                if (char.IsLower(next))
                                {
                                    builder.Append('_');
                                }
                            }
                        }
                        else
                        {
                            builder.Append('_');
                        }
                    }
                }

                builder.Append(char.ToLower(ch));
            }

            return builder.ToString();
        }


    }



}
