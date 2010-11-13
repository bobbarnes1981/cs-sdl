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

import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.PrintWriter;
import java.io.StringWriter;

import javax.swing.JFrame;

import org.h2.jdbc.JdbcSQLException;

import arcane.Arcane;
import arcane.ui.util.MessageFrame;
import arcane.ui.util.ProgressDialog;

abstract public class Loader implements Runnable {
	protected ProgressDialog dialog = new ProgressDialog();

	private MessageFrame errorFrame;
	private boolean cancelled;
	private boolean success;

	public Loader (String title) {
		dialog.setMessage("Initializing...");
		dialog.setAlwaysOnTop(true);
		dialog.setValue(-1);
		dialog.setTitle(title);
		dialog.addWindowListener(new WindowAdapter() {
			public void windowClosed (WindowEvent evt) {
				if (!success) cancel();
			}
		});
	}

	protected synchronized MessageFrame getErrorFrame () {
		if (errorFrame == null) {
			errorFrame = new MessageFrame("Error - Arcane v" + Arcane.version);
			errorFrame.setDefaultCloseOperation(JFrame.DISPOSE_ON_CLOSE);
			errorFrame.editorPane.setContentType("text/plain");
			errorFrame.addButton("Close");
		}
		return errorFrame;
	}

	protected String getExceptionText (Exception ex) {
		if (ex instanceof JdbcSQLException && ex.getMessage().contains("Database may be already open"))
			return "Error: Arcane is already running.\n\n" + //
				"Only a single instance of Arcane may be ran at once.\n" + //
				"Try using the Tools menu or File->New Window to open multiple Arcane applications at the same time.";
		StringWriter writer = new StringWriter();
		ex.printStackTrace(new PrintWriter(writer));
		return writer.toString();
	}

	protected void handleError (Exception ex) {
		handleError(getExceptionText(ex));
	}

	protected void handleError (String error) {
		System.out.println(error);
		MessageFrame errorFrame = getErrorFrame();
		errorFrame.setTitle(dialog.getTitle());
		errorFrame.setVisible(true);
		errorFrame.appendText(error);
		errorFrame.scrollToEnd();
	}

	public void start (String threadName) {
		new Thread(this, threadName).start();
		dialog.setVisible(true);
		dialog.dispose();
	}

	public final void run () {
		try {
			if (isCancelled()) return;
			load();
			if (isCancelled()) return;
			success = true;
		} catch (Exception ex) {
			cancel();
			handleError(ex);
		} finally {
			dialog.setVisible(false);
		}
	}

	protected void cancel () {
		cancelled = true;
	}

	protected boolean isCancelled () {
		return cancelled;
	}

	public boolean failed () {
		return !success;
	}

	abstract public void load () throws Exception;
}
