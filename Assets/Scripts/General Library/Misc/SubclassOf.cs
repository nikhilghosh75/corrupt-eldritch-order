using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;

/*
 * Enables designers to specify a subclass (i.e. an inherited class) to define behavior
 * Enables inheritance to work substantially better
 * Nikhil Ghosh '24
 */

namespace WSoft
{
    public interface ISubclassOf
    {
        public Type BaseType { get; }

        public void SetType(Type newType);
    }

    [Serializable]
    public class SubclassOf<T> : ISubclassOf
    {
        [SerializeField] Type _type;

        [SerializeField] public string typeName;

        public Type type
        {
            get { return _type; }
            set
            {
                if (typeof(T).IsAssignableFrom(value))
                {
                    _type = value;
                }
            }
        }

        public Type BaseType => typeof(T);

        public T Create()
        {
            if (type == null)
            {
                type = ReflectionFunctions.FindTypeOfName(typeName);
            }

            return (T)Activator.CreateInstance(_type);
        }

        public void SetType(Type newType)
        {
            type = newType;
            typeName = ReflectionFunctions.GetNameOfTypeWithAssembly(type);
        }
    }

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SubclassOf<>), true)]
    class SubclassOfDrawer : PropertyDrawer
    {
        ISubclassOf subclassOf;
        List<Type> validTypes = new List<Type>();
        List<string> validTypeNames = new List<string>();

        int selectedType = 0;
        bool isInitialized = false;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + 2;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!isInitialized)
                Init(property);

            EditorGUI.BeginProperty(position, label, property);

            Rect popupRect = new Rect(position.x, position.y, position.width, 16);
            selectedType = EditorGUI.Popup(popupRect, "Type", selectedType, validTypeNames.ToArray());
            subclassOf.SetType(validTypes[selectedType]);

            EditorGUI.EndProperty();
        }

        void Init(SerializedProperty property)
        {
            subclassOf = (ISubclassOf)fieldInfo.GetValue(property.serializedObject.targetObject);
            validTypes.Clear();
            validTypeNames.Clear();

            // Find all things inheriting from T
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (type != null && type != subclassOf.BaseType && subclassOf.BaseType.IsAssignableFrom(type))
                    {
                        validTypes.Add(type);
                        validTypeNames.Add(type.Name);
                    }
                }
            }

            isInitialized = true;
        }
    }
#endif
}