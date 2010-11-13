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
import java.io.OutputStream;

/**
 * An OutputStream that writes to multiple other OutputStreams.
 */
public class MultiplexOutputStream extends OutputStream {
	private final OutputStream[] streams;

	public MultiplexOutputStream (OutputStream... streams) {
		super();
		if (streams == null) throw new IllegalArgumentException("streams cannot be null.");
		this.streams = streams;
	}

	public void write (int b) throws IOException {
		for (int i = 0; i < streams.length; i++)
			streams[i].write(b);
	}

	public void write (byte[] b, int off, int len) throws IOException {
		for (int i = 0; i < streams.length; i++)
			streams[i].write(b, off, len);
	}
}
