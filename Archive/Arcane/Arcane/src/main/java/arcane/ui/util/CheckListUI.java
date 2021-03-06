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

import javax.swing.JComponent;
import javax.swing.JList;
import javax.swing.SwingUtilities;
import javax.swing.event.MouseInputListener;
import javax.swing.plaf.basic.BasicListUI;

/**
 * This class modifies the basic list UI so that items in the list can be toggle on/off when the user clicks on them. The user
 * does not hold down the CTRL or SHIFT key.
 * @author Gilles Paul
 */
public class CheckListUI extends BasicListUI {
	protected JList m_checklist = null;

	protected MouseInputListener createMouseInputListener () {
		return new CheckListMouseInputHandler();
	}

	public void installUI (JComponent c) {
		super.installUI(c);
		m_checklist = (JList)c;
	}

	protected int convertYToRow (int y) {
		return super.convertYToRow(y);
	}

	public class CheckListMouseInputHandler implements MouseInputListener {
		public void mouseClicked (MouseEvent e) {
		}

		public void mouseEntered (MouseEvent e) {
		}

		public void mouseExited (MouseEvent e) {
		}

		public void mousePressed (MouseEvent e) {
			if (!SwingUtilities.isLeftMouseButton(e)) {
				return;
			}

			if (!m_checklist.isEnabled()) {
				return;
			}

			/*
			 * Request focus before updating the list selection. This implies that the current focus owner will see a focusLost()
			 * event before the lists selection is updated IF requestFocus() is synchronous (it is on Windows). See bug 4122345
			 */
			if (!m_checklist.hasFocus()) {
				m_checklist.requestFocus();
			}

			int row = convertYToRow(e.getY());
			if (row != -1) {
				m_checklist.setValueIsAdjusting(true);
				int anchorIndex = m_checklist.getAnchorSelectionIndex();

				// This part is modified from original.
				if (m_checklist.isSelectedIndex(row)) {
					m_checklist.removeSelectionInterval(row, row);
				} else {
					m_checklist.addSelectionInterval(row, row);
				}
			}
		}

		public void mouseDragged (MouseEvent e) {
		}

		public void mouseMoved (MouseEvent e) {
		}

		public void mouseReleased (MouseEvent e) {
			if (!SwingUtilities.isLeftMouseButton(e)) {
				return;
			}

			m_checklist.setValueIsAdjusting(false);
		}
	}
}
