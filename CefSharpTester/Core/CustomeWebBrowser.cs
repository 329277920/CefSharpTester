using CefSharp;
using CefSharp.WinForms;
using CefSharpTester.Core.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefSharpTester.Core
{
    public class CustomeWebBrowser : ChromiumWebBrowser
    {
        public CustomeWebBrowser(IRequestContext requestContext = null) : base("", requestContext)
        {
            var requestHandler = new CustomeRequestHandler(this);
            requestHandler.InterceptBeforeResourceLoad += RequestHandler_InterceptBeforeResourceLoad;
            requestHandler.InterceptResourceLoadComplete += RequestHandler_InterceptResourceLoadComplete;
            this.RequestHandler = requestHandler;
        }
      
        private CefReturnValue RequestHandler_InterceptBeforeResourceLoad(IWebBrowser arg1, IBrowser arg2, IFrame arg3, IRequest arg4, IRequestCallback arg5)
        {
            if (OnBeforeResourceLoad == null)
            {
                return CefReturnValue.Continue;
            }
            var args = new BeforeResourceLoadEventArgs(arg4.Url, arg4.ResourceType);
            OnBeforeResourceLoad.Invoke(this, args);
            if (args.CacheToQueue)
            {
                ResourceCacheManager.CreateResource(arg4.Identifier);
            }
            return !args.Cancel ? CefReturnValue.Continue : CefReturnValue.Cancel;
        }

        private void RequestHandler_InterceptResourceLoadComplete(IWebBrowser arg1, IBrowser arg2, IFrame arg3, IRequest arg4, IResponse arg5, UrlRequestStatus arg6, long arg7)
        {
            var stream = ResourceCacheManager.GetResource(arg4.Identifier);
            if(stream != null)
            {
                stream.Seek(0, System.IO.SeekOrigin.Begin);
            }          
            try
            {
                this.OnResourceLoadComplete?.Invoke(this, new ResourceLoadCompleteEventArgs(arg4.Url, arg4.ResourceType, arg7, stream, arg6));
            }
            finally
            {
                ResourceCacheManager.DeleteResource(arg4.Identifier);
            }
        }

        /// <summary>
        /// 在开始加载资源前触发
        /// </summary>
        public event EventHandler<BeforeResourceLoadEventArgs> OnBeforeResourceLoad;

        /// <summary>
        /// 在资源加载完成后触发
        /// </summary>
        public event EventHandler<ResourceLoadCompleteEventArgs> OnResourceLoadComplete;
    }
}
