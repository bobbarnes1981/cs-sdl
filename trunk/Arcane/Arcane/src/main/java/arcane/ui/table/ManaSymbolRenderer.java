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

import java.awt.Component;
import java.awt.Graphics;

import javax.swing.JTable;

import arcane.ui.ManaSymbols;

public class ManaSymbolRenderer extends PaddedRenderer {
	private CardTable table;
	private int row;
	private String manaCost;

	public ManaSymbolRenderer () {
		super(0, 0);
	}

	public void paintComponent (Graphics g) {
		super.paintComponent(g);
		if (manaCost == null || manaCost.length() == 0) return;
		ManaSymbols.draw(g, manaCost, 3, 1);
	};

	public Component getTableCellRendererComponent (JTable table, Object value, boolean isSelected, boolean hasFocus, int row,
		int column) {

		this.table = ((CardTable)table);
		this.row = row;
		manaCost = (String)value;

		String text = "";
		if (manaCost.equals("{C}")) {
			text = "Colorless";
			manaCost = null;
		}

		return super.getTableCellRendererComponent(table, text, isSelected, false, row, column);
	}
}
