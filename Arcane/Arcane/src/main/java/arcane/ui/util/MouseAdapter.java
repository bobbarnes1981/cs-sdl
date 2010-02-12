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

import java.awt.event.MouseEvent;

public class MouseAdapter extends java.awt.event.MouseAdapter {
	private boolean mouseDown;

	public final void mouseClicked (MouseEvent evt) {
	}

	public void mouseActuallyClicked (MouseEvent evt) {
	}

	public void mouseExited (MouseEvent evt) {
		mouseDown = false;
	}

	public void mousePressed (MouseEvent evt) {
		mouseDown = true;
	}

	public void mouseReleased (MouseEvent evt) {
		if (mouseDown) {
			mouseActuallyClicked(evt);
			mouseDown = false;
		}
	}
}
