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


package arcane.ui.table;

import java.awt.Color;
import java.awt.Component;
import java.awt.Graphics;
import java.util.regex.Pattern;

import javax.swing.JTable;
import javax.swing.table.DefaultTableCellRenderer;

import arcane.Card;

/**
 * Disables focus border and shifts cell contents down and/or right.
 */
public class PaddedRenderer extends DefaultTableCellRenderer {
	private final int down;
	private final int right;
	private Pattern replaceDashEntityPattern = Pattern.compile("&#821.;");
	private Color illegalColor = new Color(255, 208, 208);
	private Color illegalSelectedColor = new Color(238, 0, 0);

	public PaddedRenderer (int down, int right) {
		super();
		this.down = down;
		this.right = right;
	}

	protected void paintComponent (Graphics g) {
		if (ui != null) {
			Graphics scratchGraphics = g.create();
			g.setColor(getBackground());
			g.fillRect(0, 0, getWidth(), getHeight());
			scratchGraphics.translate(right, down);
			try {
				ui.update(scratchGraphics, this);
			} finally {
				scratchGraphics.dispose();
			}
		}
	}

	public Component getTableCellRendererComponent (JTable table, Object value, boolean isSelected, boolean hasFocus, int row,
		int column) {

		if (value != null) value = replaceDashEntityPattern.matcher(value.toString()).replaceAll("-");

		super.getTableCellRendererComponent(table, value, isSelected, false, row, column);

		CardTable cardTable = (CardTable)table;
		Card card = cardTable.model.viewCards.get(row);
		if (cardTable.illegalCards.contains(card)) {
			if (isSelected)
				setBackground(illegalSelectedColor);
			else
				setBackground(illegalColor);
		} else if (isSelected)
			super.setBackground(table.getSelectionBackground());
		else
			setBackground(table.getBackground());

		return this;
	}
}
