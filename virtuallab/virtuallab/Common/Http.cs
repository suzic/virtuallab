using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Collections.Specialized;

namespace Common
{
    public static class Http
    {
        //Content-Type
        //text/plain 空格转换为 "+" 加号，但不对特殊字符编码
        //application/x-www-form-urlencoded;charset=utf-8  在发送前编码所有字符（默认）
        //multipart/form-data 不对字符编码。
        //application/json;charset=utf-8
        //text/xml (text/xml采用的是us-ascii编码) application/xml (使用的编码是<?xml version="1.0" encoding="utf-8"?>)
        //raw
        //binary
        //application/octet-stream
        //HttpWebResponse响应处理：Header["set-cookie"]，Header["location"]重定向绝对或相对路径，response.ResponseUri获取最后访问的URl，返回内容的编码(先找响应流的<meta[^<]*charset=([^<]*)[\"']，iso-8859-1替换为gbk，否则response.CharacterSet)


        #region Core
        const string GET = "GET";
        const string POST = "POST";
        const string APPLICATION_OCTET_STREAM = "application/octet-stream";
        const string TEXT_PLAIN = "text/plain;charset=utf-8";
        const string APPLICATION_X_WWW_FORM_URLENCODED = "application/x-www-form-urlencoded;charset=utf-8";
        const string MULTIPART_FORM_DATA = "multipart/form-data;boundary=";
        const string APPLICATION_JSON = "application/json;charset=utf-8";
        const string APPLICATION_XML = "application/xml";

        static HttpWebRequest CreateRequest(RequestParam p)
        {
            //CerPath
            if (!string.IsNullOrWhiteSpace(p.CerPath))
                ServicePointManager.ServerCertificateValidationCallback = p.GetRemoteCertificateValidation();

            //会对Url进行编码
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(UrlEncodePath(p.Url));

            //ClentCertificates
            if (p.ClentCertificates != null && p.ClentCertificates.Count > 0)
                foreach (X509Certificate c in p.ClentCertificates)
                {
                    request.ClientCertificates.Add(c);
                }
            //CerPath
            if (!string.IsNullOrWhiteSpace(p.CerPath))
                request.ClientCertificates.Add(new X509Certificate(p.CerPath));

            //Header
            if (p.Header != null)
                foreach (string key in p.Header.AllKeys)
                    request.Headers.Add(key, p.Header[key]);

            //LocalIPEndPoint
            if (p.LocalIPEndPoint != null)
                request.ServicePoint.BindIPEndPointDelegate = p.GetLocalIPEndPointCallback();

            if (p.Expect100Continue.HasValue)
                request.ServicePoint.Expect100Continue = p.Expect100Continue.Value;

            if (p.ConnectionLimit.HasValue)
                request.ServicePoint.ConnectionLimit = p.ConnectionLimit.Value;


            //Proxy
            if (!string.IsNullOrWhiteSpace(p.ProxyIp))
            {
                string[] ipport = p.ProxyIp.Split(':');
                WebProxy proxy = ipport.Length > 1 ? new WebProxy(ipport[0], Convert.ToInt32(ipport[1])) : new WebProxy(ipport[0]);
                if (!string.IsNullOrWhiteSpace(p.ProxyUserName))
                    proxy.Credentials = new NetworkCredential(p.ProxyUserName, p.ProxyPwd);
                request.Proxy = proxy;
            }
            if (p.Proxy != null)
                request.Proxy = p.Proxy;

            //Credentials
            if (!string.IsNullOrWhiteSpace(p.CredentialsUserName))
                request.Credentials = new NetworkCredential(p.CredentialsUserName, p.CredentialsPwd);
            if (p.Credentials != null)
                request.Credentials = p.Credentials;


            if (p.ProtocolVersion != null)
                request.ProtocolVersion = p.ProtocolVersion;


            request.Method = p.Method;

            if (!string.IsNullOrWhiteSpace(p.ContentType))
                request.ContentType = p.ContentType;

            if (!string.IsNullOrWhiteSpace(p.Accept))
                request.Accept = p.Accept;

            if (!string.IsNullOrWhiteSpace(p.UserAgent))
                request.UserAgent = p.UserAgent;

            if (!string.IsNullOrWhiteSpace(p.Host))
                request.Host = p.Host;

            if (!string.IsNullOrWhiteSpace(p.Referer))
                request.Referer = p.Referer;



            if (p.Timeout.HasValue)
                request.Timeout = p.Timeout.Value;

            if (p.KeepAlive.HasValue)
                request.KeepAlive = p.KeepAlive.Value;

            if (p.ReadWriteTimeout.HasValue)
                request.ReadWriteTimeout = p.ReadWriteTimeout.Value;

            if (p.AllowAutoRedirect.HasValue)
                request.AllowAutoRedirect = p.AllowAutoRedirect.Value;

            if (p.MaximumAutomaticRedirections.HasValue)
                request.MaximumAutomaticRedirections = p.MaximumAutomaticRedirections.Value;

            if (p.IfModifiedSince.HasValue)
                request.IfModifiedSince = p.IfModifiedSince.Value;


            //Cookie
            if (!string.IsNullOrEmpty(p.Cookie))
                request.Headers[HttpRequestHeader.Cookie] = p.Cookie;

            //设置CookieCollection
            if (p.CookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                if (p.CookieCollection.Count > 0)
                    request.CookieContainer.Add(p.CookieCollection);
            }


            return request;
        }

        static void PostData(HttpWebRequest request, string data)
        {
            if (string.IsNullOrEmpty(data))
                return;
            PostData(request, System.Text.Encoding.UTF8.GetBytes(data));
        }
        static void PostData(HttpWebRequest request, byte[] data)
        {
            if (data == null || data.Length < 1)
            {
                request.ContentLength = 0;
                return;
            }

            request.ContentLength = data.Length;
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(data, 0, data.Length);
            }
        }
        static void PostData(HttpWebRequest request, int contentLength, Stream contentStream)
        {
            if (contentLength < 1 || contentStream == null)
            {
                request.ContentLength = 0;
                return;
            }

            request.ContentLength = contentLength;
            try
            {
                using (Stream requestStream = request.GetRequestStream())
                {
                    contentStream.CopyTo(requestStream);
                }
            }
            finally
            {
                CloseStream(contentStream);
            }
        }
        static void PostData(HttpWebRequest request, List<NameValueItem> paras)
        {
            if (paras == null || paras.Count < 1)
                return;
            PostData(request, ToEncodeQueryString(paras));
        }
        static void PostData(HttpWebRequest request, MultiPartFormData formData)
        {
            if (formData == null)
                return;
            PostData(request, formData.ToBytes());
        }

        static byte[] Send(HttpWebRequest request)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Send(request, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        static void Send(HttpWebRequest request, Stream outStream, bool closeOutStream = true)
        {
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    ReadResponse(response, outStream, closeOutStream);
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (HttpWebResponse response = (HttpWebResponse)ex.Response)
                    {
                        ReadResponse(response, outStream, closeOutStream);
                    }
                }
                else
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(ex.Message))
                        {
                            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ex.Message);
                            outStream.Write(bytes, 0, bytes.Length);
                        }
                    }
                    finally
                    {
                        if (closeOutStream)
                            CloseStream(outStream);
                    }
                }
            }
        }
        static void ReadResponse(HttpWebResponse response, Stream outStream, bool closeOutStream = true)
        {
            if (response == null)
                return;

            try
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                        return;

                    if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                    {
                        using (GZipStream gZipStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            gZipStream.CopyTo(outStream);
                        }
                    }
                    else
                    {
                        responseStream.CopyTo(outStream);
                    }
                }
            }
            finally
            {
                if (closeOutStream)
                    CloseStream(outStream);
            }
        }
        static void CloseStream(Stream stream)
        {
            try
            {
                if (stream != null)
                    stream.Close();
            }
            catch (Exception ex)
            {
            }
        }
        #endregion


        #region outStream
        public static void GetStream(string url, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url);
            HttpWebRequest request = CreateRequest(requestParam);
            Send(request, outStream, closeOutStream);
        }

        public static void PostOctetStream(string url, int contentLength, Stream contentStream, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_OCTET_STREAM);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, contentLength, contentStream);
            Send(request, outStream, closeOutStream);
        }
        public static void PostOctetStream(string url, byte[] data, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_OCTET_STREAM);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, data);
            Send(request, outStream, closeOutStream);
        }

        public static void PostPlainTextStream(string url, int contentLength, Stream contentStream, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, TEXT_PLAIN);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, contentLength, contentStream);
            Send(request, outStream, closeOutStream);
        }
        public static void PostPlainTextStream(string url, string plainText, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, TEXT_PLAIN);
            HttpWebRequest request = CreateRequest(requestParam);
            if (!string.IsNullOrEmpty(plainText))
                plainText = plainText.Replace(' ', '+');//空格转换为 "+" 加号
            PostData(request, plainText);
            Send(request, outStream, closeOutStream);
        }


        public static void PostFormUrlencodeStream(string url, List<NameValueItem> paras, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_X_WWW_FORM_URLENCODED);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, paras);
            Send(request, outStream, closeOutStream);
        }
        public static void PostFormUrlencodeStream(string url, string queryString, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_X_WWW_FORM_URLENCODED);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, FromQueryString(queryString));
            Send(request, outStream, closeOutStream);
        }

        public static void PostFormDataStream(string url, MultiPartFormData formData, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, MULTIPART_FORM_DATA+ formData.Boundary);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, formData);
            Send(request, outStream, closeOutStream);
        }
        public static void PostFormDataStream(string url, List<NameValueItem> paras, Stream outStream, bool closeOutStream = true)
        {
            MultiPartFormData formData = new MultiPartFormData(paras);
            RequestParam requestParam = new RequestParam(url, MULTIPART_FORM_DATA + formData.Boundary);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, formData);
            Send(request, outStream, closeOutStream);
        }
        public static void PostFormDataStream(string url, string queryString, Stream outStream, bool closeOutStream = true)
        {
            MultiPartFormData formData = new MultiPartFormData(queryString);
            RequestParam requestParam = new RequestParam(url, MULTIPART_FORM_DATA + formData.Boundary);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, formData);
            Send(request, outStream, closeOutStream);
        }

        public static void PostJsonStream(string url, int contentLength, Stream contentStream, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_JSON);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, contentLength, contentStream);
            Send(request, outStream, closeOutStream);
        }
        public static void PostJsonStream(string url, string json, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_JSON);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, json);
            Send(request, outStream, closeOutStream);
        }

        public static void PostXmlStream(string url, int contentLength, Stream contentStream, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_XML);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, contentLength, contentStream);
            Send(request, outStream, closeOutStream);
        }
        public static void PostXmlStream(string url, string xml, Stream outStream, bool closeOutStream = true)
        {
            RequestParam requestParam = new RequestParam(url, APPLICATION_XML);
            HttpWebRequest request = CreateRequest(requestParam);
            PostData(request, xml);
            Send(request, outStream, closeOutStream);
        }
        #endregion

        #region Bytes
        public static byte[] GetBytes(string url)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                GetStream(url, memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public static byte[] PostOctetBytes(string url, int contentLength, Stream contentStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostOctetStream(url,contentLength,contentStream, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostOctetBytes(string url, byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostOctetStream(url, data, memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public static byte[] PostPlainTextBytes(string url, int contentLength, Stream contentStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostPlainTextStream(url, contentLength,contentStream, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostPlainTextBytes(string url, string plainText)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostPlainTextStream(url, plainText, memoryStream, false);
                return memoryStream.ToArray();
            }
        }


        public static byte[] PostFormUrlencodeBytes(string url, List<NameValueItem> paras)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostFormUrlencodeStream(url, paras, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostFormUrlencodeBytes(string url, string queryString)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostFormUrlencodeStream(url, queryString, memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public static byte[] PostFormDataBytes(string url, MultiPartFormData formData)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostFormDataStream(url, formData, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostFormDataBytes(string url, List<NameValueItem> paras)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostFormDataStream(url, paras, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostFormDataBytes(string url, string queryString)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostFormDataStream(url, queryString, memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public static byte[] PostJsonBytes(string url, int contentLength, Stream contentStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostJsonStream(url, contentLength,contentStream, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostJsonBytes(string url, string json)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostJsonStream(url, json, memoryStream, false);
                return memoryStream.ToArray();
            }
        }

        public static byte[] PostXmlBytes(string url, int contentLength, Stream contentStream)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostXmlStream(url, contentLength, contentStream, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        public static byte[] PostXmlBytes(string url, string xml)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                PostXmlStream(url, xml, memoryStream, false);
                return memoryStream.ToArray();
            }
        }
        #endregion

        #region String
        public static string Get(string url)
        {
            return ToString(GetBytes(url));
        }

        public static string PostOctet(string url, int contentLength, Stream contentStream)
        {
            return ToString(PostOctetBytes(url, contentLength, contentStream));
        }
        public static string PostOctet(string url, byte[] data)
        {
            return ToString(PostOctetBytes(url, data));
        }

        public static string PostPlainText(string url, int contentLength, Stream contentStream)
        {
            return ToString(PostPlainTextBytes(url, contentLength, contentStream));
        }
        public static string PostPlainText(string url, string plainText)
        {
            return ToString(PostPlainTextBytes(url, plainText));
        }


        public static string PostFormUrlencode(string url, List<NameValueItem> paras)
        {
            return ToString(PostFormUrlencodeBytes(url, paras));
        }
        public static string PostFormUrlencode(string url, string queryString)
        {
            return ToString(PostFormUrlencodeBytes(url, queryString));
        }

        public static string PostFormData(string url, MultiPartFormData formData)
        {
            return ToString(PostFormDataBytes(url, formData));
        }
        public static string PostFormData(string url, List<NameValueItem> paras)
        {
            return ToString(PostFormDataBytes(url, paras));
        }
        public static string PostFormData(string url, string queryString)
        {
            return ToString(PostFormDataBytes(url, queryString));
        }

        public static string PostJson(string url, int contentLength, Stream contentStream)
        {
            return ToString(PostJsonBytes(url, contentLength, contentStream));
        }
        public static string PostJson(string url, string json)
        {
            return ToString(PostJsonBytes(url, json));
        }

        public static string PostXml(string url, int contentLength, Stream contentStream)
        {
            return ToString(PostXmlBytes(url, contentLength,contentStream));
        }
        public static string PostXml(string url, string xml)
        {
            return ToString(PostXmlBytes(url,xml));
        }
        static string ToString(byte[] bytes)
        {
            if (bytes == null)
                return null;
            if (bytes.Length == 0)
                return string.Empty;
            return System.Text.Encoding.UTF8.GetString(bytes);
        }
        #endregion


        #region QueryString
        public static string ToQueryString(List<NameValueItem> list)
        {
            if (list == null || list.Count < 1)
                return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i].ToString());
                if (i < list.Count - 1)
                    sb.Append("&");
            }
            return sb.ToString();
        }
        public static string ToEncodeQueryString(List<NameValueItem> list)
        {
            if (list == null || list.Count < 1)
                return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(list[i].ToEncodeString());
                if (i < list.Count - 1)
                    sb.Append("&");
            }
            return sb.ToString();
        }
        public static List<NameValueItem> FromQueryString(string queryString)
        {
            if (string.IsNullOrWhiteSpace(queryString))
                return null;
            List<NameValueItem> list = new List<NameValueItem>();
            string[] items = queryString.Split('&');
            string[] kv;
            foreach (string item in items)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    kv = item.Split('=');
                    list.Add(new NameValueItem(UrlDecode(kv[0].Trim()), kv.Length > 1 ? UrlDecode(kv[1].Trim()) : string.Empty));
                }
            }
            return list;
        }

        /// <summary>
        /// +转空格，Uri.UnescapeDataString(+不转空格)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;
            return System.Web.HttpUtility.UrlDecode(str);
        }
        /// <summary>
        /// 全转义，:/?&=+ ，空格转%20
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UrlEncodeData(string data)
        {
            if (string.IsNullOrEmpty(data))
                return data;
            return Uri.EscapeDataString(data);
        }
        /// <summary>
        /// 不转义:/?&=+ ，空格转%20
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string UrlEncodePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;
            return Uri.EscapeUriString(path);
        }
        #endregion

        #region RequestParam
        public class RequestParam
        {
            /// <summary>
            /// 请求地址
            /// </summary>
            public string Url;
            /// <summary>
            /// 请求方法
            /// </summary>
            public string Method;
            /// <summary>
            /// POST 类型
            /// </summary>
            public string ContentType;

            #region Cert
            /// <summary>
            /// 证书绝对路径
            /// </summary>
            public string CerPath;
            /// <summary>
            /// CerPath 回调
            /// </summary>
            public System.Net.Security.RemoteCertificateValidationCallback RemoteCertificateValidation;
            /// <summary>
            /// 回调验证证书问题
            /// </summary>
            /// <param name="sender">流对象</param>
            /// <param name="certificate">证书</param>
            /// <param name="chain">X509Chain</param>
            /// <param name="errors">SslPolicyErrors</param>
            /// <returns>bool</returns>
            static bool DefaultRemoteCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            }
            public System.Net.Security.RemoteCertificateValidationCallback GetRemoteCertificateValidation()
            {
                if (RemoteCertificateValidation == null)
                    return new RemoteCertificateValidationCallback(DefaultRemoteCertificateValidation);
                return RemoteCertificateValidation;
            }

            /// <summary>
            /// 设置509证书集合
            /// </summary>
            public X509CertificateCollection ClentCertificates;
            #endregion

            #region LocalIPEndPoint
            /// <summary>
            /// 设置本地的出口ip和端口
            /// </summary>
            public IPEndPoint LocalIPEndPoint;
            public BindIPEndPoint GetLocalIPEndPointCallback()
            {
                return new BindIPEndPoint(LocalIPEndPoint_Callback);
            }
            /// <summary>
            /// 通过设置这个属性，可以在发出连接的时候绑定客户端发出连接所使用的IP地址。 
            /// </summary>
            /// <param name="servicePoint"></param>
            /// <param name="remoteEndPoint"></param>
            /// <param name="retryCount"></param>
            /// <returns></returns>
            IPEndPoint LocalIPEndPoint_Callback(ServicePoint servicePoint, IPEndPoint remoteEndPoint, int retryCount)
            {
                return LocalIPEndPoint;//端口号
            }
            #endregion

            public WebHeaderCollection Header;

            #region Proxy
            public string ProxyIp;
            public string ProxyUserName;
            public string ProxyPwd;
            public WebProxy Proxy;
            #endregion

            #region NetCredentials
            public string CredentialsUserName;
            public string CredentialsPwd;
            /// <summary>
            /// 获取或设置请求的身份验证信息。
            /// </summary>
            public ICredentials Credentials;
            #endregion

            /// <summary>
            /// HTTP协议版本
            /// </summary>
            public Version ProtocolVersion;
            /// <summary>
            ///  POST 请求是否需要 100-Continue 响应
            /// </summary>
            public bool? Expect100Continue;

            /// <summary>
            /// 默认请求超时时间
            /// </summary>
            public int? Timeout;

            /// <summary>
            /// 默认写入Post数据超时间
            /// </summary>
            public int? ReadWriteTimeout;

            /// <summary>
            ///  获取或设置一个值，该值指示是否与 Internet 资源建立持久性连接默认为true。
            /// </summary>
            public bool? KeepAlive;

            /// <summary>
            /// 最大连接数
            /// </summary>
            public int? ConnectionLimit;

            /// <summary>
            /// 支持跳转页面，查询结果将是跳转后的页面，默认是不跳转
            /// </summary>
            public bool? AllowAutoRedirect;

            /// <summary>
            /// 设置请求将跟随的重定向的最大数目
            /// </summary>
            public int? MaximumAutomaticRedirections;

            /// <summary>
            /// 获取和设置IfModifiedSince，默认为当前日期和时间
            /// </summary>
            public DateTime? IfModifiedSince;



            /// <summary>
            /// 请求标头值 默认为text/html, application/xhtml+xml, */*
            /// </summary>
            public string Accept;

            /// <summary>
            /// 客户端访问信息默认Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)
            /// </summary>
            public string UserAgent;

            /// <summary>
            /// 设置Host的标头信息
            /// </summary>
            public string Host;

            /// <summary>
            /// 来源地址，上次访问地址
            /// </summary>
            public string Referer;



            /// <summary>
            /// 设置请求头的Cookie串
            /// </summary>
            public string Cookie;

            /// <summary>
            /// 启用请求的CookieContainer容器
            /// </summary>
            public CookieCollection CookieCollection;



            #region 构造
            public RequestParam(string url)
            {
                this.Url = url;
                this.Method = GET;
            }
            public RequestParam(string url, string contentType)
            {
                this.Url = url;
                this.Method = POST;
                this.ContentType = contentType;
            }
            #endregion
        }
        #endregion

        #region NameValueItem
        public class NameValueItem
        {
            public string Name;
            public string Value;

            public NameValueItem(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public override string ToString()
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return string.Empty;
                return string.Format("{0}={1}", Name, Value);
            }
            public string ToEncodeString()
            {
                if (string.IsNullOrWhiteSpace(Name))
                    return string.Empty;
                return string.Format("{0}={1}", Http.UrlEncodeData(Name), Http.UrlEncodeData(Value));
            }

        }
        #endregion

        #region MultiPartFormData
        public class MultiPartFormData
        {
            public const string DEFAULTBOUNDARY = "8ceb5aa6524347a089b125e7419bc04bd25bfecd884a4c23b99d74ca0978f520";
            const string NAME = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n";
            const string FILE = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: application/octet-stream\r\n\r\n";

            public string Boundary;
            MemoryStream MemoryStream;
            bool End;

            #region 构造
            public MultiPartFormData()
            {
                MemoryStream = new MemoryStream();
                Boundary = DEFAULTBOUNDARY;
            }
            public MultiPartFormData(List<NameValueItem> paras) : this()
            {
                AddeNameValueList(paras);
            }
            public MultiPartFormData(string queryString) : this()
            {
                AddeQueryString(queryString);
            }
            #endregion


            #region 基元
            void WriteBeginBoundary()
            {
                byte[] bytes = Encoding.ASCII.GetBytes("--" + Boundary + "\r\n");
                MemoryStream.Write(bytes, 0, bytes.Length);
            }
            void WriteEndBoundary()
            {
                byte[] bytes = Encoding.ASCII.GetBytes("--" + Boundary + "--\r\n");
                MemoryStream.Write(bytes, 0, bytes.Length);
            }
            void WriteNewline()
            {
                byte[] bytes = Encoding.ASCII.GetBytes("\r\n");
                MemoryStream.Write(bytes, 0, bytes.Length);
            }

            void WriteName(string name, string value)
            {
                string str = string.Format(NAME, name, value);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
                MemoryStream.Write(bytes, 0, bytes.Length);
            }
            void WriteFile(string name, string filename)
            {
                string str = string.Format(FILE, name, filename);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
                MemoryStream.Write(bytes, 0, bytes.Length);
            }
            void WriteFileBytes(byte[] fileBytes)
            {
                MemoryStream.Write(fileBytes, 0, fileBytes.Length);
                WriteNewline();
            }
            void WriteFileStream(Stream fileStream, bool closeFileStream = true)
            {
                try
                {
                    if (fileStream != null)
                        fileStream.CopyTo(MemoryStream);
                }
                finally
                {
                    if (closeFileStream)
                        fileStream.Close();
                }
                WriteNewline();
            }
            #endregion

            #region 添加
            public void Add(string name, string value)
            {
                if (End)
                    throw new Exception("已写入EndBoundary，不能再添加.");
                WriteBeginBoundary();
                WriteName(name, value);
            }
            public void AddFile(string name, string filename, byte[] fileBytes)
            {
                if (End)
                    throw new Exception("已写入EndBoundary，不能再添加.");
                WriteBeginBoundary();
                WriteFile(name, filename);
                WriteFileBytes(fileBytes);
            }
            public void AddFile(string name, string filename, Stream fileStream, bool closeFileStream = true)
            {
                if (End)
                    throw new Exception("已写入EndBoundary，不能再添加.");
                WriteBeginBoundary();
                WriteFile(name, filename);
                WriteFileStream(fileStream, closeFileStream);
            }
            #endregion

            #region 输出
            public byte[] ToBytes()
            {
                if (!End)
                {
                    WriteEndBoundary();
                    End = true;
                }
                return MemoryStream.ToArray();
            }
            #endregion

            #region 转换
            void AddeNameValueList(List<NameValueItem> paras)
            {
                if (paras == null)
                    return;
                foreach (NameValueItem item in paras)
                {
                    WriteName(item.Name, item.Value);
                }
            }
            void AddeQueryString(string queryString)
            {
                if (string.IsNullOrWhiteSpace(queryString))
                    return;
                string[] items = queryString.Split('&');
                string[] kv;
                foreach (string item in items)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                    {
                        kv = item.Split('=');
                        WriteName(UrlDecode(kv[0].Trim()), kv.Length > 1 ? UrlDecode(kv[1].Trim()) : string.Empty);
                    }
                }
            }
            #endregion
        }
        #endregion
    }
}
