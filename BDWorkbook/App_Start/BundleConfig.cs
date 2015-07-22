using System;
using System.Web.Optimization;

namespace BDWorkbook
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            AddDefaultIgnorePatterns(bundles.IgnoreList);

            //jQuery & non-Angualr libs
            var jqlibs=new ScriptBundle("~/bundles/Jqlibs")
                .Include("~/Scripts/vendor/jquery/dist/jquery.js")
                .Include("~/Scripts/vendor/toastr/toastr.js")
                .Include("~/Scripts/vendor-others/jquery.bootstrap.wizard.js")
                .Include("~/Scripts/vendor/bootstrap/dist/js/bootstrap.js")
                .Include("~/Scripts/vendor/sweetalert/dist/sweetalert-dev.js")
                .Include("~/Scripts/vendor/screenfull/dist/screenfull.js")
                .Include("~/Scripts/vendor/moment/moment.js")
                .Include("~/Scripts/vendor/bootstrap-daterangepicker/daterangepicker.js");

            var jqUnminify = new ScriptBundle("~/bundles/jqUnminifylibs")
                .Include("~/Scripts/vendor/footable/dist/footable.all.min.js")
                .Include("~/Scripts/vendor/lodash/lodash.min.js");
            jqUnminify.Transforms.Clear();


             //Angular & angular libs
            var ngUnminify = new ScriptBundle("~/bundles/ngUnminifylibs")
                .Include("~/Scripts/vendor/angular/angular.min.js")
                .Include("~/Scripts/vendor/angular-animate/angular-animate.min.js");
            ngUnminify.Transforms.Clear();

            var nglibs=new ScriptBundle("~/bundles/nglibs")
                .Include("~/Scripts/vendor/angular-cookies/angular-cookies.js")
                .Include("~/Scripts/vendor/angular-resource/angular-resource.js")
                .Include("~/Scripts/vendor/angular-sanitize/angular-sanitize.js")
                .Include("~/Scripts/vendor/angular-ui-router/release/angular-ui-router.js")
                .Include("~/Scripts/vendor/angular-filter/dist/angular-filter.js")
                .Include("~/Scripts/vendor/angular-ui-utils/ui-utils.js")
                .Include("~/Scripts/vendor/angular-bootstrap/ui-bootstrap-tpls.js")
                .Include("~/Scripts/vendor/angular-loading-bar/build/loading-bar.js")
                .Include("~/Scripts/vendor/ngstorage/ngStorage.js")
                .Include("~/Scripts/vendor/angular-http-auth/src/http-auth-interceptor.js")
                .Include("~/Scripts/vendor/angular-ui-select/dist/select.js")
                .Include("~/Scripts/vendor/angular-xeditable/dist/js/xeditable.js")
                .Include("~/Scripts/vendor/ng-table/dist/ng-table.js")
                .Include("~/Scripts/vendor/ng-file-upload/ng-file-upload-all.js")
                .Include("~/Scripts/vendor-others/angularjs-dropdown-multiselect.js")
                .Include("~/Scripts/vendor-others/ng-table-resizable-columns.src.js")
                .Include("~/Scripts/vendor-others/ng-bs-daterangepicker.js")
                .Include("~/Scripts/vendor-others/ui-load.js")
                .Include("~/Scripts/vendor-others/SweetAlert.js");
            
            

            //Main Application
            var app=new ScriptBundle("~/bundles/BDWorkBook")
                //Common
                .Include("~/Scripts/app/config.js")
                .Include("~/Scripts/app/app.js")
                .Include("~/Scripts/app/app.config.js")
                .Include("~/Scripts/app/app.lazyload.js")
                .Include("~/Scripts/app/app.router.js")
                .Include("~/Scripts/app/app.run.js")
                //Modules
                .Include("~/Scripts/app/services/services.js")
                .Include("~/Scripts/app/filters/filters.js")
                .Include("~/Scripts/app/directives/directives.js")
                .Include("~/Scripts/app/controllers/Admin/Admin.js")
                .Include("~/Scripts/app/controllers/CallAdmin/callAdmin.js")
                .Include("~/Scripts/app/controllers/DashBoard/DashBoard.js")
                .Include("~/Scripts/app/controllers/Home/Home.js")
                .Include("~/Scripts/app/controllers/Quote/quote.js")
                .Include("~/Scripts/app/controllers/Shared/Shared.js")
                .Include("~/Scripts/app/controllers/TeleSale/Telesale.js")
                .Include("~/Scripts/app/controllers/Lead/Lead.js")
                //App
                .IncludeDirectory("~/Scripts/app", "*.js", true);


            
            bundles.Add(jqlibs);
            bundles.Add(jqUnminify);

            bundles.Add(ngUnminify);
            bundles.Add(nglibs);

            bundles.Add(app);

            BundleTable.EnableOptimizations = false;
        }

        public static void AddDefaultIgnorePatterns(IgnoreList ignoreList)
        {
            if (ignoreList == null)
                throw new ArgumentNullException("ignoreList");
            ignoreList.Ignore("*.test.js");
        }
    }
}