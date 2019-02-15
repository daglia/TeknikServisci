using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TeknikServisci.Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundle/js").Include(
                "~/assets/js/jquery-2.1.1.min.js",
                "~/assets/js/bootstrap.min.js",
                "~/assets/plugins/nanoscrollerjs/jquery.nanoscroller.min.js",
                "~/assets/plugins/metismenu/metismenu.min.js",
                "~/assets/js/scripts.js",
                "~/assets/js/demo/wizard.js",
                "~/assets/js/demo/form-wizard.js",
                "~/assets/js/demo/dashboard-v2.js",
                "~/assets/plugins/screenfull/screenfull.js"
            ));

            bundles.Add(new StyleBundle("~/bundle/css").Include(
                "~/assets/css/bootstrap.min.css",
                "~/assets/css/style.css",
               "~/assets/plugins/switchery/switchery.min.css",
                "~/assets/plugins/bootstrap-select/bootstrap-select.min.css",
                "~/assets/plugins/jquery-ricksaw-chart/css/rickshaw.css",
                "~/assets/plugins/bootstrap-validator/bootstrapValidator.min.css",
                "~/assets/css/demo/jquery-steps.min.css",
                "~/assets/plugins/summernote/summernote.min.css",
                "~/assets/css/demo/jasmine.css",
                "~/assets/plugins/pace/pace.min.css"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}