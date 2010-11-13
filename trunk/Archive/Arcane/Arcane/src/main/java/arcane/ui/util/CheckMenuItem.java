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

import javax.swing.JCheckBoxMenuItem;
import javax.swing.MenuSelectionManager;

import com.sun.java.swing.plaf.windows.WindowsCheckBoxMenuItemUI;

/**
 * JCheckBoxMenuItem that doesn't close when checked or unchecked.
 */
public class CheckMenuItem extends JCheckBoxMenuItem {
	static private final MenuSelectionManager dontCloseSelectionManager = new MenuSelectionManager() {
		public void clearSelectedPath () {
			super.clearSelectedPath();
		}
	};

	public CheckMenuItem (String text) {
		super(text);
		if (!(getUI() instanceof WindowsCheckBoxMenuItemUI)) return;
		setUI(new WindowsCheckBoxMenuItemUI() {
			protected void doClick (MenuSelectionManager msm) {
				super.doClick(CheckMenuItem.dontCloseSelectionManager);
				setArmed(true);
			}
		});
	}
}
