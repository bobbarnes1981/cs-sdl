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

import java.awt.BorderLayout;
import java.awt.Component;
import java.awt.Container;
import java.awt.Cursor;
import java.awt.Dimension;
import java.awt.Graphics;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.util.LinkedList;
import java.util.StringTokenizer;

import javax.swing.BorderFactory;
import javax.swing.JButton;
import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.JSplitPane;
import javax.swing.plaf.basic.BasicSplitPaneDivider;
import javax.swing.plaf.basic.BasicSplitPaneUI;

public class SplitPane extends JSplitPane {
	private SplitPaneType type = SplitPaneType.pixel;
	private BasicSplitPaneDivider divider;
	private double location;

	public SplitPane (int orientation) {
		super(orientation, true);
		setBorder(BorderFactory.createEmptyBorder());
		setUI(new BasicSplitPaneUI() {
			public BasicSplitPaneDivider createDefaultDivider () {
				divider = new BasicSplitPaneDivider(this) {
					public void paint (Graphics g) {
					}
				};
				return divider;
			}
		});
	}

	public void setEnabled (boolean enabled) {
		if (enabled != isEnabled()) {
			if (enabled) {
				switch (getType()) {
				case pixel:
					setDividerLocation((int)location);
					break;
				case percentage:
					setResizeWeight(location);
					break;
				}
			} else {
				switch (getType()) {
				case pixel:
					location = getDividerLocation();
					break;
				case percentage:
					location = getResizeWeight();
					break;
				}
			}
		}
		super.setEnabled(enabled);
		if (divider == null) return;
		if (enabled) {
			divider.setCursor((orientation == JSplitPane.HORIZONTAL_SPLIT) ? Cursor.getPredefinedCursor(Cursor.E_RESIZE_CURSOR)
				: Cursor.getPredefinedCursor(Cursor.S_RESIZE_CURSOR));
		} else
			divider.setCursor(Cursor.getDefaultCursor());
	}

	public Dimension getPreferredSize () {
		if (isEnabled())
			return super.getPreferredSize();
		else
			return new Dimension();
	}

	public SplitPaneType getType () {
		return type;
	}

	public void setType (SplitPaneType type) {
		this.type = type;
	}

	static public enum SplitPaneType {
		pixel, percentage
	}

	static public void setScrollPaneInfo (Component component, String infoString) {
		LinkedList<Double> infos = new LinkedList();
		StringTokenizer infoTokenizer = new StringTokenizer(infoString, ",");
		while (infoTokenizer.hasMoreTokens()) {
			infos.add(Double.parseDouble(infoTokenizer.nextToken()));
		}
		setScrollPaneInfo(component, infos);
	}

	static private void setScrollPaneInfo (Component component, LinkedList<Double> infos) {
		if (infos.isEmpty()) return;
		if (component instanceof SplitPane) {
			SplitPane splitPane = (SplitPane)component;
			Double location = infos.removeFirst();
			if (location != null) {
				switch (splitPane.getType()) {
				case pixel:
					splitPane.setDividerLocation(location.intValue());
					break;
				case percentage:
					if (location >= 0 && location <= 1) splitPane.setResizeWeight(location);
					break;
				}
			}
		}
		if (component instanceof Container) {
			Container container = (Container)component;
			for (int i = 0, n = container.getComponentCount(); i < n; i++) {
				component = container.getComponent(i);
				setScrollPaneInfo(component, infos);
			}
		}
	}

	static public String getScrollPaneInfo (Component component) {
		String info = "";
		if (component instanceof SplitPane) {
			SplitPane splitPane = (SplitPane)component;
			switch (splitPane.getType()) {
			case pixel:
				info += splitPane.getDividerLocation();
				break;
			case percentage:
				info += (splitPane.getDividerLocation() - splitPane.getMinimumDividerLocation())
					/ (double)(splitPane.getMaximumDividerLocation() - splitPane.getMinimumDividerLocation());
				break;
			}
			info += ",";
		}
		if (component instanceof Container) {
			Container container = (Container)component;
			for (int i = 0, n = container.getComponentCount(); i < n; i++) {
				component = container.getComponent(i);
				info += getScrollPaneInfo((Container)component);
			}
		}
		return info;
	}

	public static void main (String[] args) {
		final JFrame frame = new JFrame();
		frame.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
		frame.setSize(640, 480);

		JButton button = new JButton("Dump");
		button.addActionListener(new ActionListener() {
			public void actionPerformed (ActionEvent evt) {
				System.out.println(getScrollPaneInfo(frame));
			}
		});

		SplitPane splitPane = new SplitPane(JSplitPane.VERTICAL_SPLIT);
		splitPane.setType(SplitPaneType.percentage);
		frame.add(splitPane);
		{
			JPanel panel = new JPanel();
			panel.add(button);
			splitPane.add(panel, JSplitPane.TOP);
		}
		{
			JPanel panel = new JPanel();
			panel.setLayout(new BorderLayout());
			splitPane.add(panel, JSplitPane.BOTTOM);
			{
				SplitPane splitPane2 = new SplitPane(JSplitPane.HORIZONTAL_SPLIT);
				splitPane2.setType(SplitPaneType.pixel);
				panel.add(splitPane2);
				{
					JPanel panel2 = new JPanel();
					panel2.add(button);
					splitPane2.add(panel2, JSplitPane.LEFT);
				}
				{
					JPanel panel2 = new JPanel();
					splitPane2.add(panel2, JSplitPane.RIGHT);
				}
			}
		}
		setScrollPaneInfo(splitPane, "0.20187794,465,");

		frame.setVisible(true);
	}
}
