using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revamp.IO.Structs.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Revamp.IO.Helpers.Helpers
{
    public class CommonHelper
    {
        public static string[] UnAllowedWords()
        {
            string[] words = {
                "application",
                "stage",
                "grip",
                "objectset",
                "object set",
                "grid",
                "property",
                "properties",
                "options",
                "system",
                "csa",
                "revamp",
                "sys",
                "owner",
                "select",
                "delete",
                "create",
                "build",
                "purge",
                "log",
                "server",
                "update",
                "insert",
                "create",
                "database",
                "set",
                "dynamic",
                "data",
                "dat",
                "redirect",
                "function",
                "procedure",
                "method",
                "abstract",
                "as",
                "base",
                "bool",
                "break",
                "byte",
                "case",
                "catch",
                "char",
                "checked",
                "class",
                "const",
                "continue",
                "decimal",
                "default",
                "delegate",
                "do",
                "double",
                "else",
                "enum",
                "event",
                "explicit",
                "extern",
                "false",
                "finally",
                "fixed",
                "float",
                "for",
                "foreach",
                "goto",
                "if",
                "implicit",
                "in",
                "int",
                "interface",
                "internal",
                "is",
                "lock",
                "long",
                "namespace",
                "new",
                "null",
                "object",
                "operator",
                "out",
                "override",
                "params",
                "private",
                "protected",
                "public",
                "readonly",
                "ref",
                "return",
                "sbyte",
                "sealed",
                "short",
                "sizeof",
                "stackalloc",
                "static",
                "string",
                "struct",
                "switch",
                "this",
                "throw",
                "true",
                "try",
                "typeof",
                "uint",
                "ulong",
                "unchecked",
                "unsafe",
                "ushort",
                "using",
                "virtual",
                "void",
                "volatile",
                "while"
            };

            return words;
        }

        public static string ReturnViewToString(CommonModels.MVCGetPartial thisModel)
        {
            //TODO: Review .Net Core Port COnversion
            /*thisModel._ViewData = thisModel._ViewData ?? null;

            thisModel._TempData = thisModel._TempData ?? null;

            thisModel._ViewData.Model = thisModel.model;



            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(thisModel._thisController, thisModel.ViewName);

                ViewContext viewContext = new ViewContext(thisModel._thisController, viewResult.View, thisModel._ViewData, thisModel._TempData, sw);

                viewResult.View.Render(viewContext, sw);

                viewResult.ViewEngine.ReleaseView(thisModel._thisController, viewResult.View);

                return sw.GetStringBuilder().ToString();
            }*/
            return "Review this .Net Core Port";
        }

        public static string ReturnString(string HtmlJsCssJson)
        {
            CommonHelper CH = new CommonHelper();

            return CH._ReturnSyntax(HtmlJsCssJson);
        }

        internal string _ReturnSyntax(string HtmlJsCssJson)
        {
            string view = string.Empty;

            switch (HtmlJsCssJson.ToLower())
            {
                case "tab_generator":
                    view = "~/Views/shared/_ERWebPartial_TabGenerator.cshtml";
                    break;
                case "datatable":
                    view = "~/Views/Common/_DataTable.cshtml";
                    break;

            }

            return view;
        }
    }
}
