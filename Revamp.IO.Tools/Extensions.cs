using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace Revamp.IO.Tools
{
    public static class BoxExtensions
    {

        public static string GetPlainTextFromHtml(this string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\)|(\\<style(.+?)\\)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, string.Empty);
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            htmlString = htmlString.Replace(" ", string.Empty);

            return htmlString;
        }

        public static bool ContainsXHTML(this string input)
        {
            try
            {
                XElement x = XElement.Parse("<wrapper>" + input + "</wrapper>");
                return !(x.DescendantNodes().Count() == 1 && x.DescendantNodes().First().NodeType == XmlNodeType.Text);
            }
            catch (XmlException)
            {
                return true;
            }
        }

        public static string ConvertXHTMLEntities(this string input)
        {
            // Convert all ampersands to the ampersand entity.
            string output = input;
            output = output.Replace("&amp;", "amp_token");
            output = output.Replace("&", "&amp;");
            output = output.Replace("amp_token", "&amp;");

            // Convert less than to the less than entity (without messing up tags).
            output = output.Replace("< ", "&lt; ");
            return output;
        }
    }
}
