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


package arcane.util;

import java.awt.Dimension;
import java.awt.Toolkit;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Collections;
import java.util.Enumeration;
import java.util.Properties;
import java.util.Vector;

import javax.swing.JFrame;

/**
 * A collection of name/value pairs with sorted keys and utility methods.
 */
public class Preferences {
	protected Properties props;

	public Preferences () {
		props = new Properties();
	}

	public Preferences (Preferences prefs) {
		props = prefs.props;
	}

	public synchronized Enumeration keys () {
		Enumeration keysEnum = props.keys();
		Vector keyList = new Vector();
		while (keysEnum.hasMoreElements()) {
			keyList.add(keysEnum.nextElement());
		}
		Collections.sort(keyList);
		return keyList.elements();
	}

	public int getInt (String name, int defaultValue) {
		String value = props.getProperty(name);
		if (value == null) return defaultValue;
		try {
			return Integer.parseInt(value);
		} catch (NumberFormatException ex) {
			return defaultValue;
		}
	}

	public boolean getBoolean (String name, boolean defaultValue) {
		String value = props.getProperty(name);
		if (value == null) return defaultValue;
		return Boolean.parseBoolean(value);
	}

	public long getLong (String name, long defaultValue) {
		String value = props.getProperty(name);
		if (value == null) return defaultValue;
		return Long.parseLong(value);
	}

	public void set (String key, Object value) {
		props.setProperty(key, String.valueOf(value));
	}

	public String get (String key, Object value) {
		String string = null;
		if (value != null) string = String.valueOf(value);
		return props.getProperty(key, string);
	}

	public void load (FileInputStream stream) throws IOException {
		props.load(stream);
	}

	public void store (FileOutputStream stream, String comments) throws IOException {
		props.store(stream, comments);
	}

	public void loadFrameState (JFrame frame, String prefix, int defaultWidth, int defaultHeight) {
		frame.setSize(getInt(prefix + ".width", defaultWidth), getInt(prefix + ".height", defaultHeight));
		Dimension size = frame.getSize();
		Dimension screenSize = Toolkit.getDefaultToolkit().getScreenSize();
		int defaultX = screenSize.width / 2 - size.width / 2;
		int defaultY = screenSize.height / 2 - size.height / 2;
		frame.setLocation(getInt(prefix + ".left", defaultX), getInt(prefix + ".top", defaultY));
		frame.setExtendedState(getInt(prefix + ".extended.state", JFrame.NORMAL));
	}

	public void saveFrameState (JFrame frame, String prefix) {
		int extendedState = frame.getExtendedState();
		if (extendedState == JFrame.ICONIFIED) extendedState = JFrame.NORMAL;
		set(prefix + ".extended.state", String.valueOf(extendedState));

		if (extendedState != JFrame.MAXIMIZED_BOTH) {
			if (extendedState == JFrame.MAXIMIZED_BOTH) frame.setExtendedState(JFrame.NORMAL);
			Dimension size = frame.getSize();
			if (size.width > 0 && size.height > 0) {
				set(prefix + ".width", String.valueOf(size.width));
				set(prefix + ".height", String.valueOf(size.height));
				set(prefix + ".top", String.valueOf(frame.getLocation().y));
				set(prefix + ".left", String.valueOf(frame.getLocation().x));
			}
		}
	}
}
