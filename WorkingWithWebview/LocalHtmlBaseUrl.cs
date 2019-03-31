using Xamarin.Forms;

namespace WorkingWithWebview
{
    public interface IBaseUrl { string Get(); }

    public class LocalHtmlBaseUrl : ContentPage
    {
        public LocalHtmlBaseUrl()
        {
            var browser = new WebView();
            var htmlSource = new HtmlWebViewSource();

            htmlSource.Html = @"<html>
<head>
<link rel=""stylesheet"" href=""default.css"">
</head>
<body>
<h1>Xamarin.Forms</h1>
<p>The CSS and image are loaded from local files!</p>
<img src='XamarinLogo.png'/>
<p><a href=""local.html"">next page</a></p>
<p><a href=""http://localhost:8080/roger.pdf"">PDF</a></p>
<p><a href=""http://localhost:8080/slt01.JPG"">JPG</a></p>
<p><a href=""http://localhost:8080/keeweb.html"">KeeWeb</a></p>
</body>
</html>";

            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            browser.Source = htmlSource;
            Content = browser;
        }
    }
}
