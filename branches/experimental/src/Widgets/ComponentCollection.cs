#region Header

/*
 * Copyright (C) 2010 Pikablu
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

#endregion Header

namespace SdlDotNet.Widgets
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ComponentCollection
    {
        #region Fields

        List<string> componentNames;
        List<Component> components;

        #endregion Fields

        #region Constructors

        public ComponentCollection()
        {
            componentNames = new List<string>();
            components = new List<Component>();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Gets the number of components in this collection.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return componentNames.Count; }
        }

        #endregion Properties

        #region Indexers

        /// <summary>
        /// Gets the <see cref="SdlDotNet.Widgets.Component"/> with the specified name.
        /// </summary>
        /// <value></value>
        public Component this[string name]
        {
            get {
                return components[componentNames.IndexOf(name)];
            }
        }

        /// <summary>
        /// Gets the <see cref="SdlDotNet.Widgets.Component"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Component this[int index]
        {
            get {
                return components[index];
            }
        }

        #endregion Indexers

        #region Methods

        /// <summary>
        /// Adds the component.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="component">The component.</param>
        public void AddComponent(string name, Component component)
        {
            if (!componentNames.Contains(name)) {
                componentNames.Add(name);
                components.Add(component);
            }
        }

        /// <summary>
        /// Finds the component.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public int FindComponent(string name)
        {
            return componentNames.IndexOf(name);
        }

        /// <summary>
        /// Removes the component at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public void RemoveComponent(int index)
        {
            if (index > -1) {
                componentNames.RemoveAt(index);
                components.RemoveAt(index);
            }
        }

        #endregion Methods
    }
}