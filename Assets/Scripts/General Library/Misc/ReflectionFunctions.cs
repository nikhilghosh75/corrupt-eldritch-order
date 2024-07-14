using System;
using System.Reflection;
using System.Text;

/*
 * Various functions that allow you to get information about types through reflection
 * Nikhil Ghosh '24
 */
namespace WSoft
{
    public class ReflectionFunctions
    {
        /// <summary>Finds a type with a specific name</summary>
        /// <param name="typeName">Note that assembly;typename denotes that we should look in a specific assembly. If there is no ;, we look in the current one</param>
        /// <returns></returns>
        public static Type FindTypeOfName(string typeName)
        {
            Assembly assembly;
            if (typeName.Contains(";"))
            {
                string assemblyName = typeName.Substring(0, typeName.IndexOf(";"));
                string realTypeName = typeName.Substring(typeName.IndexOf(";") + 1);

                assembly = FindAssemblyOfName(assemblyName);

                return Array.Find(assembly.GetTypes(), t => t.FullName == realTypeName);
            }

            assembly = Assembly.GetAssembly(typeof(ReflectionFunctions));

            return Array.Find(assembly.GetTypes(), t => t.FullName == typeName);
        }

        /// <summary>
        /// Returns an assmebly with a specific name
        /// </summary>
        public static Assembly FindAssemblyOfName(string assemblyName)
            => Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name == assemblyName);

        /// <summary>
        /// Returns assembly;typename 
        /// </summary>
        public static string GetNameOfTypeWithAssembly(Type type)
        {
            return type.Assembly.GetName().Name + ";" + type.FullName;
        }
    }
}
