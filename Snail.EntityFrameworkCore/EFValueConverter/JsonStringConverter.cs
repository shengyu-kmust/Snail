using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Snail.EntityFrameworkCore.EFValueConverter
{
    public class JsonStringConverter : ValueConverter<Dictionary<string,string>, string>
    {
        public JsonStringConverter(Expression<Func<Dictionary<string, string>, string>> convertToProviderExpression, Expression<Func<string, Dictionary<string, string>>> convertFromProviderExpression,ConverterMappingHints mappingHints = null) : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        {
        }

        public static JsonStringConverter DefaultConverter => new JsonStringConverter(a => a == null ? "" : JsonConvert.SerializeObject(a), a => string.IsNullOrEmpty(a) ? new Dictionary<string, string>() :JsonConvert.DeserializeObject<Dictionary<string, string>>(a));
    }
}
