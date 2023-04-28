using System.IO;
using System.Net.Http;
using System.Threading;
using Xamarin.Forms;

namespace HandOver
{
    public class WebPage : ContentPage
    {

        public WebPage()
        {
            WebView browser = new WebView
            {
                Source = App.Config.path
            };

            //browser.Eval("setTimeout(function() {document.getElementById('w3review').style.display = 'none';}, 5000); ");
            Content = browser;
            Content.Focus();
        }

        HtmlWebViewSource LoadHTMLFileFromResource()
        {
            var source = new HtmlWebViewSource();
            source.Html = new HttpClient().GetStringAsync(App.Config.path).GetAwaiter().GetResult();
            return source;
        }
    }
}

