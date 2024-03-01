using System.Web;
using System.Web.Optimization;

public class BundleConfig
{
    // Для получения информации о бандлинге и минификации посетите https://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {
        bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));

        bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            "~/Scripts/bootstrap.js",
            "~/Scripts/respond.js"));

        // Добавьте новые бандлы для дополнительных файлов
        bundles.Add(new StyleBundle("~/Content/styles").Include(
            "~/Content/bootstrap.css",
            "~/Content/font-awesome.min.css",
            "~/Content/style.css",
            "~/Content/responsive.css"
            ));

        // Установите EnableOptimizations в false для отладки. Для получения дополнительной информации,
        // посетите http://go.microsoft.com/fwlink/?LinkId=301862
        BundleTable.EnableOptimizations = true;
    }
}