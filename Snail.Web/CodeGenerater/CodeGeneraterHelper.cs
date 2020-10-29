using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Snail.Common.Extenssions;
using System.Xml;
using System;

namespace Snail.Web.CodeGenerater
{


    public static class CodeGeneraterHelper
    {
        public static CodeGenerateDto GenerateDtoFromPdm(string pdmString)
        {
            var xml = new XmlDocument();
            xml.LoadXml(pdmString);
            var tables = xml.GetElementsByTagName("o:Table");
            var codeGenerateDto = new CodeGenerateDto();
            Func<string, string> tableNameToEntityName = tableName =>
            {
                if (tableName.Contains("_"))
                {
                    return tableName.Substring(tableName.LastIndexOf('_') + 1);
                }
                return tableName;
            };
            foreach (XmlElement table in tables)
            {
                if (table.HasAttribute("Id"))
                {
                    var columns = table.GetElementsByTagName("o:Column");
                    var tableCode = table.GetElementsByTagName("a:Code")?[0]?.InnerText;
                    var tableName = table.GetElementsByTagName("a:Name")?[0]?.InnerText;
                    var tableComment = table.GetElementsByTagName("a:Comment")?[0]?.InnerText;
                    var entity = new EntityModel()
                    {
                        Comment = tableName + tableComment,
                        Fields = new List<EntityFieldModel>(),
                        Name = tableNameToEntityName(tableCode),
                        TableName = tableCode
                    };
                    if (string.IsNullOrEmpty(tableCode) || string.IsNullOrEmpty(tableName))
                    {
                        continue;
                    }
                    foreach (XmlElement column in columns)
                    {
                        var columnCode = column.GetElementsByTagName("a:Code")?[0]?.InnerText;
                        var columnName = column.GetElementsByTagName("a:Name")?[0]?.InnerText;
                        var columnDataType = column.GetElementsByTagName("a:DataType")?[0]?.InnerText;
                        var columnComment = column.GetElementsByTagName("a:Comment")?[0]?.InnerText ?? "";
                        var columnLength = column.GetElementsByTagName("a:Length")?[0]?.InnerText ?? "";
                        if (string.IsNullOrEmpty(columnCode) || string.IsNullOrEmpty(columnName) || string.IsNullOrEmpty(columnDataType))
                        {
                            continue;
                        }
                        entity.Fields.Add(GetFieldModelByPdmCfg(columnCode,columnName,columnDataType, columnLength,columnComment));
                    }
                    if (entity.Fields.Count > 0)
                    {
                        codeGenerateDto.Entities.Add(entity);
                    }
                }
            }
            return codeGenerateDto;
        }
        private static EntityFieldModel GetFieldModelByPdmCfg(string code,string name,string dataType,string columnLength,string comment)
        {
            Func<string, string> getType = dt =>
             {
                 switch (dt)
                 {
                     case string v when v.Contains("uniqueidentifier"):
                         return "string";
                     case string v when v.Contains("datetime"):
                         return "DateTime";
                     case string v when v.Contains("bit"):
                         return "bool";
                     case string v when v.Contains("varchar"):
                         return "string";
                     default:
                         return "";
                 }
             };
            Func<string,string, List<string>> getAttrs = (dt,len) =>
            {
                if (dt.Contains("varchar") && len.HasValue())
                {
                    return new List<string> { $"MaxLength({len})" };
                }
                return new List<string>();
            };
            var field = new EntityFieldModel
            {
                Comment = name + comment,
                Name = name,
                Type= getType(dataType),
                Attributes= getAttrs(dataType,columnLength)
            };
            return field;
        }
        public static CodeGenerateDto GenerateDtoFromConfig(string val, out List<string> errors)
        {
            var result = new CodeGenerateDto();
            errors = new List<string>();
            var configDto = JsonConvert.DeserializeObject<CodeGenerateConfig>(val);
            result.BasePath = configDto.BasePath.Trim('\\');
            result.Entities = CodeGeneraterHelper.GenerateEntitiesModelFromTableModels(configDto, ref errors);
            result.Enums = GenerateEnumModelFromConfig(configDto, ref errors);
            result.ExceptApis = configDto.ExceptApis;
            result.ExceptServices = configDto.ExceptServices;
            return result;

        }
        /// <summary>
        /// 从json配置里的entity配置节点，生成template.tt使用的model
        /// </summary>
        /// <param name="tableModels"></param>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static List<EntityModel> GenerateEntitiesModelFromTableModels(CodeGenerateConfig config, ref List<string> errors)
        {
            var result = new List<EntityModel>();
            if (errors == null) { errors = new List<string>(); }

            foreach (var item in config.Entities)
            {
                var columns = new List<EntityFieldModel>();
                foreach (var column in item.Columns)
                {
                    columns.Add(GetFieldModel(column, ref errors));
                }
                result.Add(new EntityModel
                {
                    Name = item.Name.Trim(),
                    TableName = item.TableName.Trim(),
                    Comment = item.Comment.Trim(),
                    Fields = columns
                });
            }
            return result;
        }
        public static List<EnumModel> GenerateEnumModelFromConfig(CodeGenerateConfig config, ref List<string> errors)
        {
            var result = new List<EnumModel>();
            if (errors == null)
            {
                errors = new List<string>();
            }
            foreach (var item in config.Enums)
            {
                result.Add(GetEnumModel(item, ref errors));
            }
            return result.Where(a => a.Name.HasValue()).ToList();
        }

        private static EntityFieldModel GetFieldModel(string val, ref List<string> error)
        {
            var result = new EntityFieldModel();
            if (error == null)
            {
                error = new List<string>();
            }
            var items = val.Split(',', '，');
            if (items.Length < 3)
            {
                error.Add($"{val}少于3段");
                return null;
            }
            result.Name = items[0].Trim();
            result.Type = items[1].Trim();
            result.Comment = items[2].Trim();
            if (items.Length > 3)
            {
                var len = items[3];
                if (result.Attributes == null)
                {
                    result.Attributes = new List<string>();
                }
                result.Attributes.Add($"[MaxLength({len})]");
            }
            return result;
        }

        private static EnumModel GetEnumModel(string val, ref List<string> error)
        {
            var result = new EnumModel();
            if (error == null)
            {
                error = new List<string>();
            }
            var items = val.Split(',', '，');
            if (items.Length == 0 || items.Length % 2 == 1)
            {
                error.Add("枚举的配置参数必需大于0且为偶数");
            }
            for (int i = 0; i < items.Length; i = i + 2)
            {
                if (i == 0)
                {
                    // 第一对为枚举名和描述
                    result.Name = items[i];
                    result.Comment = items[i + 1];
                }
                else
                {
                    result.Items.Add(new EnumFieldModel
                    {
                        Name = items[i],
                        Comment = items[i + 1]
                    });
                }

            }
            return result;
        }

        public static List<VueModel> GenerateVueModelFromEntityModels(List<EntityModel> entityModels)
        {
            return entityModels.Select(a => new VueModel
            {
                Name = ToCamel(a.Name),
                Fields = a.Fields.Select(i => GenerateVueModelFromEntity(i)).ToList()
            }).ToList();
        }

        public static string ToCamel(string val)
        {
            return val.First().ToString().ToLower() + val.Substring(1, val.Length - 1);
        }

        public static VueFieldModel GenerateVueModelFromEntity(EntityFieldModel entityFieldModel)
        {
            var result = new VueFieldModel
            {
                Name = ToCamel(entityFieldModel.Name),
                Comment = entityFieldModel.Comment,
            };
            if (CodeGeneraterHelper.IsEnumType(entityFieldModel))
            {
                result.Type = "select";
                result.KeyValues = entityFieldModel.Type == "bool" ? "this.$enum.ETrueFalse" : $"this.$enum.{entityFieldModel.Type}s";
            }
            else
            {
                //支持：string,int,datetime,date,select,multiSelect,time
                switch (entityFieldModel.Type)
                {
                    case "string":
                        result.Type = "string";
                        break;
                    case "DateTime":
                        result.Type = "datetime";
                        break;
                    case "int":
                        result.Type = "int";
                        break;
                    default:
                        result.Type = "string";
                        break;
                }
            }
            return result;
        }

        private static bool IsEnumType(EntityFieldModel entityFieldModel)
        {
            return entityFieldModel.Type == "bool" || entityFieldModel.Type.StartsWith("E");
        }

    }


}
