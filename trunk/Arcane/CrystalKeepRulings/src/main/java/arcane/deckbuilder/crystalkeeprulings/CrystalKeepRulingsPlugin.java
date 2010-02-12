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

package arcane.deckbuilder.crystalkeeprulings;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
//import java.io.BufferedReader;
//import java.io.File;
import java.io.FileOutputStream;
//import java.io.FileReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.URL;
//import java.sql.SQLException;
//import java.util.HashMap;
//import java.util.List;
//import java.util.Map;
//import java.util.Map.Entry;
import java.util.zip.ZipEntry;
//import java.util.zip.ZipFile;
import java.util.zip.ZipInputStream;

import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;

//import arcane.Arcane;
import arcane.Arcane;
import arcane.ArcaneException;
//import arcane.Card;
//import arcane.CardDataStore.CardDataStoreConnection;
import arcane.deckbuilder.DeckBuilderPlugin;
import arcane.deckbuilder.ui.DeckBuilder;
//import arcane.ui.ArcaneFrame;
import arcane.ui.util.ProgressDialog;

public class CrystalKeepRulingsPlugin extends DeckBuilderPlugin {
	public void install(final DeckBuilder deckBuilder) {
		JMenu menu = new JMenu(getName());
		{
			JMenuItem loadMenuItem = new JMenuItem("Update rulings...");
			menu.add(loadMenuItem);
			loadMenuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					updateRules(deckBuilder);
				}
			});
		}
		deckBuilder.addPluginMenu(menu);
	}

	public void install(ProgressDialog dialog) {

	}

	private void updateRules(DeckBuilder deckBuilder) {
		final ProgressDialog dialog = new ProgressDialog(deckBuilder,
				"Crystal Keep");
		dialog.setMessage("Downloading rulings summaries...");
		dialog.setAlwaysOnTop(true);
		dialog.setValue(-1);

		new Thread(new Runnable() {
			public void run() {
				try {
					InputStream input = new URL(
							"http://crystalkeep.com/magic/rules/summaries/rule-txt.zip")
							.openStream();
					ZipInputStream zip = new ZipInputStream(
							new BufferedInputStream(input));
					OutputStream out;
					BufferedOutputStream dest = null;
					try {
						ZipEntry entry;
						while ((entry = zip.getNextEntry()) != null) {
							out = new FileOutputStream(Arcane.getHomeDirectory() + "data/"
									+ entry.getName());
							dest = new BufferedOutputStream(out, 2048);
							int count;
							byte data[] = new byte[2048];
							while ((count = zip.read(data, 0, 2048)) != -1) {
								dest.write(data, 0, count);
							}
						}
					} finally {
						dest.flush();
						dest.close();
						zip.close();
					}
					JOptionPane
							.showMessageDialog(
									dialog,
									"Changes will take affect when the application is restarted.",
									"Crystal Keep Rulings Summaries",
									JOptionPane.WARNING_MESSAGE);
				} catch (IOException ex) {
					throw new ArcaneException(
							"Error downloading rulings summaries.", ex);
				} finally {
					dialog.setVisible(false);
				}
			}
		}, "DownloadRulings").start();

		dialog.setVisible(true);
		dialog.dispose();
	}

	public String getName() {
		return "Crystal Keep Rulings";
	}
}
