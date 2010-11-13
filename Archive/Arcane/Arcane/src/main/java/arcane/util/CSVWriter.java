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

import java.io.PrintWriter;
import java.io.Writer;

/**
 * Writes CSV files. This format is used by Microsoft Word and Excel. Fields are separated by commas, and enclosed in quotes if
 * they contain commas or quotes. Embedded quotes are doubled. Embedded spaces do not normally require surrounding quotes. The
 * last field on the line is not followed by a comma. Null fields are represented by two commas in a row.
 * @author copyright (c) 2002-2005 Roedy Green Canadian Mind Products
 */
public final class CSVWriter {
	static public final int QUOTES_MINIMAL = 0; // As few quotes as possible.
	static public final int QUOTES_AROUND_SPACES = 1; // Quotes around fields containing spaces.
	static public final int QUOTES_ALWAYS = 2; // Quotes around all fields.

	private PrintWriter writer;
	private int quoteLevel;
	private char separator;
	private char quote;
	private final boolean trim;

	// True: a field was previously written to this line, meaning there is a comma pending to be written.
	private boolean hasPreviousField = false;

	/**
	 * @param fieldSeparator Field separator character. Usually ',' in North America, ';' and sometimes '\t' in Europe.
	 * @param textQualifier Character used to enclose fields containing a separator, usually '"'.
	 * @param trim True if reader should trim leading/trailing whitespace (eg, blanks, Cr, Lf, tab) from fields.
	 * @param quoteLevel QUOTES_MINIMAL, QUOTES_AROUND_SPACES, or QUOTES_ALWAYS.
	 */
	public CSVWriter (Writer writer, char fieldSeparator, char textQualifier, boolean trim, int quoteLevel) {
		if (writer == null) throw new IllegalArgumentException("writer cannot be null.");
		if (quoteLevel != QUOTES_MINIMAL && quoteLevel != QUOTES_AROUND_SPACES && quoteLevel != QUOTES_ALWAYS)
			throw new IllegalArgumentException("quoteLevel is invalid.");

		if (writer instanceof PrintWriter)
			this.writer = (PrintWriter)writer;
		else
			this.writer = new PrintWriter(writer);

		this.separator = fieldSeparator;
		this.quote = textQualifier;
		this.trim = trim;
		this.quoteLevel = quoteLevel;
	}

	/**
	 * Convenience constructor defaulting to: comma separator, double quote (") for a text qualifier, with trimming,
	 * QUOTES_MINIMAL.
	 */
	public CSVWriter (Writer writer) {
		this(writer, ',', '\"', true, QUOTES_MINIMAL);
	}

	/**
	 * Writes one blank CSV field.
	 */
	public void writeField () {
		writeField("");
	}

	public void writeField (int field) {
		writeField(String.valueOf(field));
	}

	/**
	 * Writes one CSV field.
	 * @param field String to write, without any surrounding quotes or escaped embedded quotes. Passing null writes a blank field.
	 */
	public void writeField (String field) {
		if (writer == null) throw new IllegalArgumentException("This CSVWriter has been closed.");

		if (field == null) field = "";

		if (trim) field = field.trim();

		if (hasPreviousField) writer.print(separator);

		if (field.indexOf(quote) >= 0) {
			// Worst case, needs surrounding quotes and internal quotes doubled.
			writer.print(quote);
			for (int i = 0; i < field.length(); i++) {
				char c = field.charAt(i);
				if (c == quote) {
					writer.print(quote);
					writer.print(quote);
				} else {
					writer.print(c);
				}
			}
			writer.print(quote);
		} else if (quoteLevel == QUOTES_ALWAYS || quoteLevel == QUOTES_AROUND_SPACES && field.indexOf(' ') >= 0
			|| field.indexOf(separator) >= 0) {
			// Needs surrounding quotes.
			writer.print(quote);
			writer.print(field);
			writer.print(quote);
		} else {
			// Ordinary case, no surrounding quotes needed.
			writer.print(field);
		}
		// Make a note to print trailing comma later.
		hasPreviousField = true;
	}

	/**
	 * Write a new line in the CVS output file to demark the end of record.
	 */
	public void newLine () {
		if (writer == null) throw new IllegalArgumentException("This CSVWriter has been closed.");

		// Don't write last comma on the line.
		writer.print("\r\n"); // Windows convention.

		hasPreviousField = false;
	}

	/**
	 * Close the CSVWriter.
	 */
	public void close () {
		if (writer != null) writer.close();
		writer = null;
	}
}
