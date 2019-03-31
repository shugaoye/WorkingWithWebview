using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Reflection;

using XamStorage;

using Unosquare.Labs.EmbedIO;
using Unosquare.Labs.EmbedIO.Modules;

namespace WorkingWithWebview
{
	public class App : Application // superclass new in 1.3
	{
		public App ()
		{
#if DEBUG
            System.Diagnostics.Debug.WriteLine("====== resource debug info =========");
            var assembly = typeof(App).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            System.Diagnostics.Debug.WriteLine("====================================");

#endif
            Task.Run(() =>
            {
                string fileName = FileSystem.Current.LocalStorage.Path + "/keeweb.html";

                // var assembly = IntrospectionExtensions.GetTypeInfo(typeof(Byte)).Assembly;
                Stream stream = assembly.GetManifestResourceStream("WorkingWithWebview.html.keeweb.html");

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // 设置当前流的位置为流的开始   
                stream.Seek(0, SeekOrigin.Begin);

                // 把 byte[] 写入文件   
                FileStream fs = new FileStream(fileName, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();
            });


                // Server must be started, before WebView is initialized,
                // because we have no reload implemented in this sample.
                Task.Factory.StartNew(async () =>
            {
                using (var server = new WebServer("http://*:8080"))
                {
                    // Assembly assembly = typeof(App).Assembly;
                    // server.RegisterModule(new ResourceFilesModule(assembly, "EmbedIO.Forms.Sample.html"));
                    var url = FileSystem.Current.LocalStorage.Path;
                    Debug.WriteLine($"URL={url}");
                    server.RegisterModule(new StaticFilesModule(url));

                    await server.RunAsync();
                }
            });

            var tabs = new TabbedPage ();

			tabs.Children.Add (new LocalHtml {Title = "Local" });
			tabs.Children.Add (new LocalHtmlBaseUrl {Title = "BaseUrl" });
			tabs.Children.Add (new WebPage { Title = "Web Page"});
			tabs.Children.Add (new WebAppPage {Title ="External"});

			MainPage = tabs;
		}
	}
}

