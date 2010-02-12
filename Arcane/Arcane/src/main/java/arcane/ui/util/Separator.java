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

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Graphics;

import javax.swing.JComponent;

public class Separator extends JComponent {
	private Dimension size = new Dimension(6, 20);

	protected void paintComponent (Graphics g) {
		int x = 2;
		g.setColor(Color.gray);
		g.drawLine(x, 2, x, getHeight() - 4);
		x++;
		g.setColor(Color.white);
		g.drawLine(x, 2, x, getHeight() - 4);
		super.paintComponent(g);
	}

	public int getWidth () {
		return (int)size.getWidth();
	}

	public int getHeight () {
		return (int)size.getHeight();
	}

	public Dimension getSize () {
		return size;
	}

	public Dimension getPreferredSize () {
		return size;
	}

	public Dimension getMinimumSize () {
		return size;
	}

	public Dimension getMaximumSize () {
		return size;
	}
}
