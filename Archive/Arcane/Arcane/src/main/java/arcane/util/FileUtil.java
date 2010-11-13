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

import java.io.File;
import java.io.FileFilter;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.FilenameFilter;
import java.io.IOException;
import java.nio.channels.FileChannel;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Provides file utility methods.
 */
public class FileUtil {
	private FileUtil () {
	}

	/**
	 * A {@link FileFilter} that accepts only directories.
	 */
	static public FileFilter directoryFilter = new FileFilter() {
		public boolean accept (File file) {
			return file.isDirectory();
		}
	};

	/**
	 * Returns a {@link FilenameFilter} that accepts only filenames that end with the specified string.
	 */
	static public FilenameFilter filenameEndsWith (final String endsWith) {
		return new FilenameFilter() {
			public boolean accept (File dir, String filename) {
				return filename.toLowerCase().endsWith(endsWith);
			}
		};
	}

	static public javax.swing.filechooser.FileFilter chooserFileEndsWith (final String description, final String endsWith) {
		return new javax.swing.filechooser.FileFilter() {
			public boolean accept (File file) {
				if (file.isDirectory()) return true;
				return file.getName().toLowerCase().endsWith(endsWith);
			}

			public String getDescription () {
				return description;
			}
		};
	}

	/**
	 * Copies one file to another.
	 */
	static public void copyFile (File in, File out) throws IOException {
		FileChannel sourceChannel = new FileInputStream(in).getChannel();
		FileChannel destinationChannel = new FileOutputStream(out).getChannel();
		sourceChannel.transferTo(0, sourceChannel.size(), destinationChannel);
		sourceChannel.close();
		destinationChannel.close();
	}

	/**
	 * Deletes a directory and all files and directories it contains.
	 */
	static public boolean deleteDirectory (File file) {
		if (file.exists()) {
			File[] files = file.listFiles();
			for (int i = 0, n = files.length; i < n; i++) {
				if (files[i].isDirectory())
					deleteDirectory(files[i]);
				else
					files[i].delete();
			}
		}
		return file.delete();
	}

	static public String getExtension (String fileName) {
		Pattern pattern = Pattern.compile(".+\\.([^\\.\\/]+)$");
		Matcher matcher = pattern.matcher(new File(fileName).getName());
		if (!matcher.matches()) return null;
		return matcher.group(1);
	}
}
