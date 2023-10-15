using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace tools.advanceddropdowns
{
    class GenericClassDropdown : AdvancedDropdown
    {
        const string ROOT_NAME = "Selector";
        const string BASE_ASSEMBLY_NAME = "myproject";
        readonly char[] CLASS_SEPARATOR = new char[] { '.' };

        private Dictionary<string, AdvancedDropdownItem> _subClassesCache;

        protected Type filter;
        protected Action<GenericClassDropdownItem> actionItemSelected;

        private GameObject _selectFromGameObject = null;
        private List<string> _containingComponents = new List<string>();

        public GenericClassDropdown(AdvancedDropdownState state, Type filter, Action<GenericClassDropdownItem> itemSelected, GameObject selectFromGameObject)
            : this(state, filter, itemSelected)
        {
            if (selectFromGameObject == null) return;

            _selectFromGameObject = selectFromGameObject;
            var list = selectFromGameObject.GetComponents(filter);
            if (list.Length == 0) return;

            foreach (var item in list)
            {
                _containingComponents.Add(item.GetType().FullName);
            }
        }

        public GenericClassDropdown(AdvancedDropdownState state, Type filter, Action<GenericClassDropdownItem> itemSelected)
            : base(state)
        {
            this.filter = filter;
            this.actionItemSelected = itemSelected;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            base.ItemSelected(item);
            actionItemSelected?.Invoke((GenericClassDropdownItem)item);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(ROOT_NAME);

            _subClassesCache = new Dictionary<string, AdvancedDropdownItem>();

            MonoScript[] list = Resources.FindObjectsOfTypeAll(typeof(MonoScript)) as MonoScript[];
            list = list.ToList()
                .FindAll(i => i.GetClass() != null
                && !i.GetClass().IsAbstract
                && (i.GetClass() == filter || i.GetClass().IsSubclassOf(filter))
                ).ToArray();

            if (_selectFromGameObject != null)
                list = list.ToList()
                .FindAll(i => _containingComponents.Contains(i.GetClass().FullName)).ToArray();

            foreach (var item in list)
            {
                AddClassEntry(root, item.GetClass().FullName, item.GetClass());
            }

            return root;
        }

        private void AddClassEntry(AdvancedDropdownItem root, string className, Type classType)
        {
            // Remove base assembly name
            className = className.Replace(BASE_ASSEMBLY_NAME + CLASS_SEPARATOR[0], "");

            string[] subClassNames = className.Split(CLASS_SEPARATOR);

            if (subClassNames.Length > 1)
            {
                // Do not create duplicated entries
                if (!_subClassesCache.ContainsKey(subClassNames[0]))
                {
                    _subClassesCache[subClassNames[0]] = new AdvancedDropdownItem(subClassNames[0]);
                    root.AddChild(_subClassesCache[subClassNames[0]]);
                }

                // Remove key class from fullname
                string subClasses = className.Replace(subClassNames[0] + CLASS_SEPARATOR[0], "");
                AddClassEntry(_subClassesCache[subClassNames[0]], subClasses, classType);
            }
            else
            {
                root.AddChild(new GenericClassDropdownItem(className, classType));
            }
        }
    }
    public class GenericClassDropdownItem : AdvancedDropdownItem
    {
        public readonly Type ClassType;

        public GenericClassDropdownItem(string name, Type classType) : base(name)
        {
            this.ClassType = classType;
        }
    }
}
