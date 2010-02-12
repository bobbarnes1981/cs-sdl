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

import javax.swing.ButtonModel;
import javax.swing.DefaultButtonModel;

public class ButtonMenuItem extends MenuItem {
	private final Button button;
	private boolean isAlternate;

	public ButtonMenuItem (String text, Button button, boolean isAlternate) {
		this(text, button);
		this.isAlternate = isAlternate;
	}

	public ButtonMenuItem (String text, Button button) {
		super(text);

		this.button = button;

		setIcon(button.getIcon());
	}

	public void doClick (int pressTime) {
		if (isAlternate) button.setNextAlternate(true);
		button.doClick(0);
	}

	public void setModel (ButtonModel newModel) {
		super.setModel(new DefaultButtonModel() {
			public boolean isEnabled () {
				return ButtonMenuItem.this.isEnabled();
			}
		});
	}

	public boolean isEnabled () {
		if (button == null) return true;
		return button.isEnabled();
	}
}
