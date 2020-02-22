using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Revamp.IO.Foundation
{
    public static class ER_ObjectExtensions
    {      
        /// <summary>
        /// !!!Limited case!!!<para />
        /// because generics aren't supported in attributes (where this was needed), we're extending object, but this will not behave the way you'd like it to for every object<para />
        /// this will return the [Display(Name)] of the given enum, if one exists<para />
        ///     if none exists, it just returns the object's value (in the case of an enum like BabyBlue=3, it will return "BabyBlue")
        /// </summary>
        ///     <example>
        ///     This shows the type of object that should be used with this extension method, such as in <see cref="IRISModels.Enums.CSAPrivilege"/>
        ///     <code>
        ///     public enum color {
        ///     red = 1,
        ///     [Display(Name="Shades of Green")]
        ///     shadesofgreen = 2
        ///     }
        ///     </code>
        /// </example>
        /// <param name="value">the enum value</param>
        /// <returns>string representation of the object, hopefully the DisplayName!</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string GetEnumDisplayName(this object value)
        {
            if (value == null) { return string.Empty; }

            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null
                    && fi.CustomAttributes.Where(a => a.AttributeType.Name == "DisplayAttribute").Count() == 1
                    && fi.CustomAttributes.First(a => a.AttributeType.Name == "DisplayAttribute").NamedArguments.Any(a => a.MemberName == "Name"))

            {

                return fi.CustomAttributes.First(a => a.AttributeType.Name == "DisplayAttribute").NamedArguments.First(a => a.MemberName == "Name").TypedValue.ToString().Replace('"', ' ').Trim();
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
