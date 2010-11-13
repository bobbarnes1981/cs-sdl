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

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.PushbackInputStream;
import java.io.Reader;

/**
 * Generic unicode textreader, which will use BOM mark to identify the encoding to be used.
 */
public class UnicodeReader extends Reader {
	PushbackInputStream internalIn;
	InputStreamReader internalIn2 = null;
	String defaultEnc;

	private static final int BOM_SIZE = 4;

	/**
	 * Default encoding is used only if BOM is not found. If defaultEncoding is NULL then systemdefault is used.
	 */
	public UnicodeReader (InputStream in, String defaultEnc) {
		internalIn = new PushbackInputStream(in, BOM_SIZE);
		this.defaultEnc = defaultEnc;
	}

	public String getDefaultEncoding () {
		return defaultEnc;
	}

	public String getEncoding () {
		if (internalIn2 == null) return null;
		return internalIn2.getEncoding();
	}

	/**
	 * Read-ahead four bytes and check for BOM marks. Extra bytes are unread back to the stream, only BOM bytes are skipped.
	 */
	protected void init () throws IOException {
		if (internalIn2 != null) return;

		String encoding;
		byte bom[] = new byte[BOM_SIZE];
		int n, unread;
		n = internalIn.read(bom, 0, bom.length);

		if ((bom[0] == (byte)0xEF) && (bom[1] == (byte)0xBB) && (bom[2] == (byte)0xBF)) {
			encoding = "UTF-8";
			unread = n - 3;
		} else if ((bom[0] == (byte)0xFE) && (bom[1] == (byte)0xFF)) {
			encoding = "UTF-16BE";
			unread = n - 2;
		} else if ((bom[0] == (byte)0xFF) && (bom[1] == (byte)0xFE)) {
			encoding = "UTF-16LE";
			unread = n - 2;
		} else if ((bom[0] == (byte)0x00) && (bom[1] == (byte)0x00) && (bom[2] == (byte)0xFE) && (bom[3] == (byte)0xFF)) {
			encoding = "UTF-32BE";
			unread = n - 4;
		} else if ((bom[0] == (byte)0xFF) && (bom[1] == (byte)0xFE) && (bom[2] == (byte)0x00) && (bom[3] == (byte)0x00)) {
			encoding = "UTF-32LE";
			unread = n - 4;
		} else {
			// Unicode BOM mark not found, unread all bytes
			encoding = defaultEnc;
			unread = n;
		}
		// System.out.println("read=" + n + ", unread=" + unread);

		if (unread > 0)
			internalIn.unread(bom, (n - unread), unread);
		else if (unread < -1) internalIn.unread(bom, 0, 0);

		// Use given encoding
		if (encoding == null) {
			internalIn2 = new InputStreamReader(internalIn);
		} else {
			internalIn2 = new InputStreamReader(internalIn, encoding);
		}
	}

	public void close () throws IOException {
		init();
		internalIn2.close();
	}

	public int read (char[] cbuf, int off, int len) throws IOException {
		init();
		return internalIn2.read(cbuf, off, len);
	}

}
