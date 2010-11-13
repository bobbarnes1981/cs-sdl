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


package arcane;

public enum Format {
	all("All"), //
	vintage("Vintage (T1)"), //
	legacy("Legacy (T1.5)"), //
	extended("Extended (T1.x)"), //
	standard("Standard (T2)"), //
	custom("Custom");

	private final String text;

	private Format (String text) {
		this.text = text;
	}

	public String toString () {
		return text;
	}

	static Format getByText (String text) {
		if (text == null || text.length() == 0) return null;
		for (Format format : values())
			if (format.text.equals(text)) return format;
		throw new ArcaneException("Format not found: " + text);
	}
}
