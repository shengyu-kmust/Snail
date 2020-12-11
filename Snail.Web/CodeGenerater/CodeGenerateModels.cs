﻿using System.Collections.Generic;

namespace Snail.Web.CodeGenerater
{
    #region 配置文件dto
    /// <summary>
    /// 代码生成的配置文件，结构dto
    /// </summary>
    public class CodeGenerateConfig
    {
        public string BasePath { get; set; }
        public List<EntityConfigModel> Entities { get; set; }
        public List<string> Enums { get; set; }
        /// <summary>
        /// 哪些表不生成service
        /// </summary>
        public List<string> ExceptServices { get; set; }
        /// <summary>
        /// 哪些表不生成api
        /// </summary>
        public List<string> ExceptApis { get; set; }
    }

    /// <summary>
    /// 基于pdm生成代码的配置参数dto
    /// </summary>
    public class CodeGenerateConfigForPdm
    {
        public string PdmFilePath { get; set; }
        /// <summary>
        /// 生成文件的路径
        /// </summary>
        public string BasePath { get; set; }
        /// <summary>
        /// 枚举
        /// </summary>
        public List<string> Enums { get; set; }
        /// <summary>
        /// 哪些表不生成service
        /// </summary>
        public List<string> ExceptServices { get; set; }
        /// <summary>
        /// 哪些表不生成api
        /// </summary>
        public List<string> ExceptApis { get; set; }
    }

    /// <summary>
    /// 配置里的entity节点
    /// </summary>
    public class EntityConfigModel
    {
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Comment { get; set; }
        public List<string> Columns { get; set; }
    }
    #endregion
    #region 配置文件解析dto
    public class CodeGenerateDto
    {
        public CodeGenerateDto()
        {
            Entities = new List<EntityModel>();
            Enums = new List<EnumModel>();
            ExceptServices = new List<string>();
            ExceptApis = new List<string>();
            BasePath = "";
        }
        public string BasePath { get; set; }
        public List<EntityModel> Entities { get; set; }
        public List<EnumModel> Enums { get; set; }
        /// <summary>
        /// 哪些表不生成service
        /// </summary>
        public List<string> ExceptServices { get; set; }
        /// <summary>
        /// 哪些表不生成api
        /// </summary>
        public List<string> ExceptApis { get; set; }
    }
    #endregion

    #region 用于在template.tt里使用的model
    #region 生成entity的model
    public class EntityModel
    {
        public EntityModel()
        {
            Fields = new List<EntityFieldModel>();
        }
        public List<EntityFieldModel> Fields { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 实体的命名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 实体对应的表名
        /// </summary>
        public string TableName { get; set; }
    }
    public class EntityFieldModel
    {
        public EntityFieldModel()
        {
            Attributes = new List<string>();
        }
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Comment { get; set; }
        public int Length { get; set; }
        /// <summary>
        /// 特性
        /// </summary>
        public List<string> Attributes { get; set; }
    }
    #endregion

    #region 生成vue的model
    public class VueModel
    {
        public List<VueFieldModel> Fields { get; set; }
        public string Name { get; set; }
    }
    public class VueFieldModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 当type为select时，keyValues值
        /// </summary>
        public string KeyValues { get; set; }
    }
    #endregion
    #region 生成dto的model

    public class DtoModel
    {
        public List<EntityFieldModel> Fields { get; set; }
        public string Name { get; set; }
        public string Prefix { get; set; }
        public string BaseClass { get; set; }
    }

    #endregion

    #region 生成vue route
    public class VueRouteModel
    {
        public string Name { get; set; }
        public string Comment { get; set; }
    }
    #endregion

    #region 生成enum
    public class EnumModel
    {
        public EnumModel()
        {
            Items = new List<EnumFieldModel>();
        }
        public string Comment { get; set; }
        public string Name { get; set; }
        public List<EnumFieldModel> Items { get; set; }
    }

    public class EnumFieldModel
    {
        /// <summary>
        /// 字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Comment { get; set; }
    }
    #endregion
    #endregion


}
