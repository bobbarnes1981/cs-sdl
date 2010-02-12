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

import java.io.IOException;
import java.io.OutputStream;

import javax.swing.text.BadLocationException;
import javax.swing.text.Document;
import javax.swing.text.JTextComponent;

public class TextComponentOutputStream extends OutputStream {
	StringBuilder buffer = new StringBuilder(512);
	private JTextComponent textComponent;

	public TextComponentOutputStream (JTextComponent textComponent) {
		this.textComponent = textComponent;
	}

	public void write (int b) throws IOException {
		char c = (char)b;
		buffer.append(c);
		if (c != '\n') return;
		Document document = textComponent.getDocument();
		try {
			document.insertString(document.getEndPosition().getOffset(), buffer.toString(), null);
		} catch (BadLocationException ignored) {
		}
		textComponent.setCaretPosition(textComponent.getDocument().getLength());
		buffer.setLength(0);
	}
}
