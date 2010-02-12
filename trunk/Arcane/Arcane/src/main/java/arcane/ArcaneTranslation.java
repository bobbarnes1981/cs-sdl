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

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import arcane.util.Preferences;

public class ArcaneTranslation extends Preferences{
	private final String fileName;
	
	public ArcaneTranslation (String fileName) {
		this.fileName = fileName;
		try {
			FileInputStream stream = new FileInputStream(fileName);
			load(stream);
			stream.close();
		} catch (FileNotFoundException ignored) {
		} catch (IOException ex) {
			throw new ArcaneException("Error reading \"" + fileName + "\".", ex);
		}
	}
	
	public void load (FileInputStream stream) throws IOException {
		props.loadFromXML(stream);
	}

	public void store (FileOutputStream stream, String comments) throws IOException {
		props.storeToXML(stream, comments);
	}
}
