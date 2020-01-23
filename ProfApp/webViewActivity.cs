using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;

namespace ProfApp
{
    [Activity(Label = "webViewActivity")]
    public class webViewActivity : Activity
    {
        WebView webview;
        string url;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.WebView);
            webview = FindViewById<WebView>(Resource.Id.webview);
            url = Intent.GetStringExtra("url");
            webview.SetWebViewClient(new PDFViewClient());
           
            WebSettings websettings = webview.Settings;
            websettings.JavaScriptEnabled = true;

            if (!url.Contains("http://"))
            {
                string address = url;
                url = String.Format("http://(0)",address);
            }
            webview.LoadUrl(url);
        }
    }

    internal class PDFViewClient : WebViewClient
    {
        public override bool ShouldOverrideUrlLoading(WebView view, string url)
        {
            view.LoadUrl(url);
            return true;
        }
    }
}