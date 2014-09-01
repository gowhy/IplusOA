using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Common
{
    public class SecurityManager
    {
        public static readonly SecurityManager Instance = new SecurityManager();

        private readonly IDictionary<string, Node> _nodes = new Dictionary<string, Node>();

        //private ISecurityService _securityService;

        private SecurityManager()
        {
        }

        public static void Configure(Action<SecurityManager> action)
        {
            Check.Argument.IsNotNull(action, "action");

            action(Instance);

            Instance.Init();
        }

        public static void Configure(Assembly assembly, Action<SecurityManager> action)
        {
            Check.Argument.IsNotNull(action, "action");

            Instance.LoadAssembly(assembly);

            action(Instance);

            Instance.Init();
        }

        public void Init()
        {
            GetSystemNodes();
        }

        private Type[] Controllers { get; set; }

        //public ISecurityService SecurityService
        //{
        //    get
        //    {
        //        var service = _securityService ?? IoC.Resolve<ISecurityService>();
        //        Check.Argument.IsNotNull(service, "SecurityService");
        //        return service;
        //    }
        //    set { _securityService = value; }
        //}

        public IDictionary<string, Node> Nodes
        {
            get { return _nodes; }
        }

        //public SecurityManager RegisterService<T>() where T : ISecurityService
        //{
        //    IoC.Current.RegisterAsSingleton<ISecurityService, T>();
        //    return this;
        //}

        /// <summary>
        /// 加载包含Controller的程序集
        /// </summary>
        /// <param name="assembly"></param>
        public void LoadAssembly(Assembly assembly)
        {
            Check.Argument.IsNotNull(assembly, "assembly");

            Func<Type, bool> filter =
               type => type.BaseType != null && (type.IsPublic && type.IsSubclassOf(typeof(Controller)));

            Controllers = assembly.GetTypes().Where(filter).ToArray();
        }


        private void GetSystemNodes()
        {
            _nodes.Clear();

            foreach (var controller in Controllers)
            {
                GetControllerNodes(controller);
            }
        }

        private void GetControllerNodes(Type controller)
        {
            foreach (var method in controller.GetMethods())
            {
                var node = CreateNode(method);

                if (node != null) _nodes.Add(node.NodeKey, node);
            }
        }

        private static string GetModuleName(Type controller)
        {
            var attr = controller.GetCustomAttributes(typeof(SecurityModuleAttribute), false).FirstOrDefault();
            var controllerName = ControllerName(controller);

            if (attr == null) return controllerName;
            var moduleAttr = (SecurityModuleAttribute)attr;
            return moduleAttr.Name ?? controllerName;
        }

        private static string ControllerName(Type controller)
        {
            return controller.Name.ReplaceEnd("Controller", "");
        }

        /// <summary>
        /// 通过Controller的Action方法获取Node节点，需要被安全控制的节点必须要有<see cref="SecurityNodeAttribute">SecurityNodeAttribute</see>
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        private static Node CreateNode(MethodInfo method)
        {
            var attr = method.GetCustomAttributes(typeof(SecurityNodeAttribute), false).FirstOrDefault();

            if (attr == null) return null;

            var nodeAttr = (SecurityNodeAttribute)attr;

            var actionName = method.Name;

            var node = new Node
            {
                Module = GetModuleName(method.DeclaringType),
                Controller = ControllerName(method.DeclaringType),
                Action = actionName,
                Name = nodeAttr.Name ?? actionName,
                IsEffect = nodeAttr.IsEffect,
                Description = nodeAttr.Description
            };

            return node;
        }

        internal static Node CreateNode(ActionDescriptor method)
        {
            var attr = method.GetCustomAttributes(typeof(SecurityNodeAttribute), false).FirstOrDefault();

            if (attr == null) return null;

            var nodeAttr = (SecurityNodeAttribute)attr;

            var actionName = method.ActionName;

            var node = new Node
            {
                Module = GetModuleName(method.ControllerDescriptor.ControllerType),
                Controller = ControllerName(method.ControllerDescriptor.ControllerType),
                Action = actionName,
                Name = nodeAttr.Name ?? actionName,
                IsEffect = nodeAttr.IsEffect,
                Description = nodeAttr.Description
            };

            return node;
        }



    }
}
