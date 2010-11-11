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

    public abstract class Component
    {
        #region Fields

        string name;

        #endregion Fields

        #region Constructors

        public Component(string name)
        {
            this.Name = name;
        }

        #endregion Constructors

        #region Events

        /// <summary>
        /// Fired every tick.
        /// </summary>
        public event EventHandler<SdlDotNet.Core.TickEventArgs> Tick;

        #endregion Events

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Frees all resources used.
        /// </summary>
        public abstract void FreeResources();

        public virtual void OnTick(SdlDotNet.Core.TickEventArgs e)
        {
            if (Tick != null)
                Tick(this, e);
        }

        #endregion Methods
    }
}