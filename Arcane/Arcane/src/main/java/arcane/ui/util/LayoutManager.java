/**
 *     Copyright (C) 2010  snacko
 *
 *     This program is free software: you can redistribute it and/or modify
 *     it under the terms of the GNU General Public License as published by
 *     the Free Software Foundation, either version 3 of the License, or
 *     (at your option) any later version.
 *
 *     This program is distributed in the hope that it will be useful,
 *     but WITHOUT ANY WARRANTY; without even the implied warranty of
 *     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *     GNU General Public License for more details.
 *
 *     You should have received a copy of the GNU General Public License
 *     along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */


package arcane.ui.util;

import java.awt.Component;
import java.awt.Container;
import java.awt.Dimension;

public abstract class LayoutManager implements java.awt.LayoutManager {
	public void addLayoutComponent (String name, Component comp) {
	}

	public void layoutContainer (Container parent) {
		layout();
	}

	public Dimension minimumLayoutSize (Container parent) {
		return null;
	}

	public Dimension preferredLayoutSize (Container parent) {
		return null;
	}

	public void removeLayoutComponent (Component comp) {
	}

	abstract public void layout ();
}
