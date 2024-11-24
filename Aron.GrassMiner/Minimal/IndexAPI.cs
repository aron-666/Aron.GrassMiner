using Aron.GrassMiner.Models;
using Aron.GrassMiner.Services;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using System.Xml;

namespace Aron.GrassMiner.Minimal
{
    public static class IndexAPI
    {
        public static WebApplication UseIndex(this WebApplication app)
        {

            app.MapGet("/", (HttpContext context) =>
            {
                var html = System.IO.File.ReadAllText(Path.Combine("wwwroot", "index.html"));
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var nodes = doc.DocumentNode.SelectNodes("/html/head/base");
                string _base = $"{context.Request.PathBase + (context.Request.PathBase.HasValue ? "/" : "")}";
                if (!_base.StartsWith('/'))
                {
                    _base = "/" + _base;
                }

                if (nodes != null)
                {
                    foreach (var node in nodes)
                    {
                        node.SetAttributeValue("href", _base);
                    }
                }
                else
                {
                    var head = doc.DocumentNode.SelectSingleNode("/html/head");
                    if (head != null)
                    {
                        var baseNode = HtmlNode.CreateNode($"<base href=\"{_base}\" />");
                        head.AppendChild(baseNode);
                    }
                }

                return Results.Content(doc.DocumentNode.OuterHtml, "text/html", Encoding.UTF8);
            });


            return app;
        }
    }
}
