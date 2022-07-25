using System.Web;
using System.Web.Optimization;

namespace EvaBatch
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                          "~/Scripts/bootstrap.js",
                          "~/Scripts/bootstrap.min.js",
                          "~/Scripts/bootstrap.bundle.js",
                          "~/Scripts/bootstrap.bundle.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.css.map",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap-theme.css.map",
                      "~/Content/bootstrap-theme.min.css",
                      "~/Content/bootstrap-theme.min.css.map",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css",
                        "~/Content/DataTables/media/css/dataTables.bootstrap.css",
                        "~/Content/buttons.dataTables.css",
                        "~/Content/buttons.dataTables.min.css",
                        "~/Content/jquery.dataTables.css",
                        "~/Content/jquery.dataTables.min.css",
                        "~/Content/DataTables/extensions/FixedColumns/css/fixedColumns.bootstrap.css"
                        ));

            //Select2 4.0.3
            bundles.Add(new StyleBundle("~/Content/select2").Include(
                "~/Content/css/select2.css",
                "~/Content/css/select2.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/select2").Include(
                "~/Scripts/select2.js",
                "~/Scripts/select2.min.js"));

            // jTable
            bundles.Add(new StyleBundle("~/Content/jTable").Include(
                        "~/Scripts/jtable/themes/metro/darkgray/jtable.css"));
            bundles.Add(new ScriptBundle("~/bundles/jTable").Include(
                        "~/Scripts/jtable/jquery.jtable.js"));

            //Moment.js
            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                        "~/Scripts/moment.js"));


            // form-validator
            bundles.Add(new StyleBundle("~/Content/formvalidator").Include(
                        "~/Scripts/form-validator/theme-default.css"));

            bundles.Add(new ScriptBundle("~/bundles/formvalidator").Include(
                         "~/Scripts/form-validator/jquery.validator.js",
                        "~/Scripts/form-validator/jquery.validator.min.js",
                        "~/Scripts/form-validator/jquery.validator.unobtrusive.js",
                        "~/Scripts/form-validator/jquery.validator.unobtrusive.min.js",
                        "~/Scripts/form-validator/jquery.form-validator.js"));

            // Noty notifications
            bundles.Add(new ScriptBundle("~/bundles/noty").Include(
                        "~/Scripts/noty/jquery.noty.js",
                        "~/Scripts/noty/themes/default.js",
                        "~/Scripts/noty/layouts/center.js",
                        "~/Scripts/noty/layouts/centerRight.js",
                        "~/Scripts/noty/layouts/topCenter.js",
                        "~/Scripts/noty/layouts/topRight.js"));

            // Sweet Alert
            bundles.Add(new StyleBundle("~/Content/sweetalert").Include(
                        "~/Content/sweetalert/sweetalert.css",
                        "~/Content/toastr.css"));

            // datatables
            bundles.Add(new StyleBundle("~/Content/datatables").Include(
                         "~/Scripts/jquery.dataTables.js",
                        "~/Scripts/jquery.dataTables.min.js"));



            // ********************** LOGIN ************************ //

            bundles.Add(new ScriptBundle("~/bundles/loginn").Include(
                      "~/Scripts/login/bootstrap.min.js",
                      "~/Scripts/login/bootstrap-colorpalette.js",
                      "~/Scripts/login/bootstrap-hover-dropdown.min.js",
                      "~/Scripts/login/jquery.blockUI.js",
                      "~/Scripts/login/jquery.cookie.js",
                      "~/Scripts/login/jquery.icheck.min.js",
                      "~/Scripts/login/jquery.mousewheel.js",
                      "~/Scripts/login/jquery-ui-1.10.2.custom.min.js",
                      "~/Scripts/login/LayoutPage.js",
                      "~/Scripts/login/less-1.5.0.min.js",
                      "~/Scripts/login/perfect-scrollbar.js",
                      "~/Scripts/login/sweetalert.min.js.js"));

            bundles.Add(new StyleBundle("~/Content/loginn").Include(
                      //"~/Content/login/iCheck/skins/all.css",
                      "~/Content/login/bootstrap.min.css",
                      "~/Content/login/bootstrap-colorpalette.css",
                      "~/Content/login/font-awesome/css/font-awesome.min.css",
                      "~/Content/login/main.css",
                      "~/Content/login/main-responsive.css",
                      "~/Content/login/perfect-scrollbar.css",
                      "~/Content/login/print.css",
                      "~/Content/login/style.css",
                      "~/Content/login/theme_navy.css"));
        }
    }
}
