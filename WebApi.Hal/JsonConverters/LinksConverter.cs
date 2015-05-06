using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json;

namespace WebApi.Hal.JsonConverters
{
    public class LinksConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var links = new HashSet<Link>((IList<Link>)value, new LinkEqualityComparer());
            var lookup = links.ToLookup(l => l.Rel);
            if (lookup.Count == 0) return;

            writer.WriteStartObject();

            foreach (var rel in lookup)
            {
                var name = rel.Key;
                // If there's 1 curie link have rel as 'curie', if multiple have 'curies'
                if (name == ReservedProperties.Links.Curie && rel.Count() > 1)
                {
                    name = ReservedProperties.Links.Curies;
                }

                writer.WritePropertyName(name);
                if (rel.Count() > 1)
                    writer.WriteStartArray();
                foreach (var link in rel)
                {
                    writer.WriteStartObject();
                    writer.WritePropertyName("href");
                    writer.WriteValue(ResolveUri(link.Href));

                    if (link.IsTemplated)
                    {
                        writer.WritePropertyName("templated");
                        writer.WriteValue(true);
                    }
                    if (!string.IsNullOrEmpty(link.Title))
                    {
                        writer.WritePropertyName("title");
                        writer.WriteValue(link.Title);
                    }

                    foreach (var info in GetLinkProperties())
                    {
                        var text = info.GetValue(link) as string;

                        if (string.IsNullOrEmpty(text))
                            continue; // no value set, so don't serialize this ...

                        writer.WritePropertyName(info.Name.ToLowerInvariant());
                        writer.WriteValue(text);
                    }

                    writer.WriteEndObject();
                }
                if (rel.Count() > 1)
                    writer.WriteEndArray();
            }
            writer.WriteEndObject();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return reader.Value;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IList<Link>).IsAssignableFrom(objectType);
        }

        public string ResolveUri(string href)
        {
            if (!string.IsNullOrEmpty(href) && VirtualPathUtility.IsAppRelative(href))
                return HttpContext.Current != null ? VirtualPathUtility.ToAbsolute(href) : href.Replace("~/", "/");
            return href;
        }

        static IList<PropertyInfo> _linkProperties;

        static IEnumerable<PropertyInfo> GetLinkProperties()
        {
            if (_linkProperties == null)
            {
                Type stringType = typeof(string);
                Type linkType = typeof (Link);
                _linkProperties =
                    linkType.GetProperties()
                        .Where(
                            pi =>
                                !pi.Name.Equals("Href") && !pi.Name.Equals("IsTemplated") && !pi.Name.Equals("Rel") &&
                                pi.PropertyType == stringType)
                        .ToList();
            }

            return _linkProperties;
        }

    }
}