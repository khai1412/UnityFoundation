namespace UnityFoundation.Scripts.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    public static class ReflectionExtensions
    {
        public static List<Type> GetDerivedTypes(this Type baseType, bool sameAssembly = false)
        {
            var baseAsm = Assembly.GetAssembly(baseType);
            return AppDomain.CurrentDomain.GetAssemblies()
                            .Where(asm => !asm.IsDynamic && (!sameAssembly || asm == baseAsm))
                            .SelectMany(asm => asm.GetTypes())
                            .Where(type => type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type)).ToList();
        }
        
        /// <summary>Get all types dives from T or Implement interface T that are not abstract. Note: only same assembly</summary>
        [Obsolete("Use GetAllDerivedTypes instead")]
        public static IEnumerable<Type> GetAllDriveType<T>()
        {
            return Assembly.GetAssembly(typeof(T)).GetTypes().Where(type => type.IsClass && !type.IsAbstract && typeof(T).IsAssignableFrom(type));
        }

        /// <summary>
        /// Get all type that derive from <typeparamref name="T"/>
        /// </summary>
        public static void CopyTo(this object from, object to)
        {
            var fromFieldInfos = from.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            var toFieldInfos   = to.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var fromField in fromFieldInfos)
            {
                var toField = toFieldInfos.FirstOrDefault(toField => toField.Name == fromField.Name && toField.FieldType.IsAssignableFrom(fromField.FieldType));
                if (toField != null)
                {
                    toField.SetValue(to, fromField.GetValue(from));
                }
            }
        }

        public static List<FieldInfo> GetFieldInfoWithAttribute<T>(this object instance) where T : Attribute
        {
            var fieldInfos = instance.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            
            return fieldInfos.Where(field => field.CustomAttributes.Any()).Where(field => field.CustomAttributes.FirstOrDefault()?.AttributeType == typeof(T)).ToList();
        }

        public static T GetCustomAttribute<T>(this object instance) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(instance.GetType(), typeof(T));
        }
        
    }
}