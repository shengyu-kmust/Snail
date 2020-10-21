using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Web.CodeGenerater
{
    public partial class ControllerTemplate
    {
        public string Comment { get; set; }
        public string Name { get; set; }
    }
    public partial class DtoTemplate
    {
        public DtoModel Dto { get; set; }
    }
    public partial class EntityConfigTemplate
    {
        public EntityModel Entity { get; set; }
    }
    public partial class EntityTemplate
    {
        public EntityModel Entity { get; set; }
    }
    public partial class ServiceTemplate
    {
        public string Name { get; set; }
    }
    public partial class AppDbContextTemplate
    {
        public List<string> EntityNames { get; set; }
    }
    public partial class VueTemplate
    {
        public VueModel Vue { get; set; }
    }
    public partial class VueApiTemplate
    {
        public List<string> EntityNames { get; set; }
    }
    public partial class VueRouterTemplate
    {
        public List<VueRouteModel> VueRouteModels { get; set; }
    }
    public partial class EnumTemplate
    {
        public EnumModel Model { get; set; }
    }

    public partial class EnumJsTemplate
    {
        public List<EnumModel> Model { get; set; }
    }
}
