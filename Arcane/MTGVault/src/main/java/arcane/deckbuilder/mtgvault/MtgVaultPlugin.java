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

package arcane.deckbuilder.mtgvault;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.BufferedInputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;

import arcane.Arcane;
import arcane.ArcaneException;
import arcane.deckbuilder.DeckBuilderPlugin;
import arcane.deckbuilder.ui.DeckBuilder;
import arcane.ui.util.ProgressDialog;
import arcane.util.CSVReader;
import arcane.util.Preferences;

public class MtgVaultPlugin extends DeckBuilderPlugin {
	private static MtgVaultPlugin instance;
	DeckBuilder deckBuilder;
	private String username;
	private String password;
	private SaveToVaultDialog saveDialog;
	private LoadFromVaultDialog loadDialog;

	public MtgVaultPlugin() {
		instance = this;
	}

	public void install(final DeckBuilder deckBuilder) {
		this.deckBuilder = deckBuilder;
		JMenu menu = new JMenu(getName());
		{
			JMenuItem loadMenuItem = new JMenuItem("Load from vault...");
			menu.add(loadMenuItem);
			loadMenuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					if (getUsername() == null)
						return;
					if (getPassword() == null)
						return;
					if (loadDialog == null)
						loadDialog = new LoadFromVaultDialog(
								MtgVaultPlugin.this);
					loadDialog.refresh();
					loadDialog.setVisible(true);
				}
			});
		}
		{
			JMenuItem saveMenuItem = new JMenuItem("Save to vault...");
			menu.add(saveMenuItem);
			saveMenuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					if (getUsername() == null)
						return;
					if (getPassword() == null)
						return;
					if (saveDialog == null)
						saveDialog = new SaveToVaultDialog(MtgVaultPlugin.this);
					saveDialog.refresh();
					saveDialog.setVisible(true);
				}
			});
		}
		menu.addSeparator();
		{
			JMenuItem setUserMenuItem = new JMenuItem(
					"Set username/password...");
			menu.add(setUserMenuItem);
			setUserMenuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					promptUsernameAndPassword();
				}
			});
		}
		deckBuilder.addPluginMenu(menu);
	}

	private void promptUsernameAndPassword() {
		String usernameResult = JOptionPane.showInputDialog(deckBuilder,
				"Enter your MTG Vault username:", "MTG Vault - Set username",
				JOptionPane.QUESTION_MESSAGE);
		if (usernameResult == null || usernameResult.trim().length() == 0)
			return;
		String passwordResult = JOptionPane.showInputDialog(deckBuilder,
				"Enter your MTG Vault password:", "MTG Vault - Set password",
				JOptionPane.QUESTION_MESSAGE);
		if (passwordResult == null || passwordResult.trim().length() == 0)
			return;
		username = usernameResult.trim();
		password = passwordResult.trim();
	}

	public List<MtgVaultDecklist> getVaultDecklists() {
		final String username = getUsername();
		if (username == null)
			return new ArrayList(0);
		final String password = getPassword();
		if (password == null)
			return new ArrayList(0);

		final ProgressDialog dialog = new ProgressDialog(deckBuilder,
				"MTG Vault");
		dialog.setMessage("Loading decks from vault...");
		dialog.setAlwaysOnTop(true);
		dialog.setValue(-1);

		class LoadVaultDecks implements Runnable {
			public List<MtgVaultDecklist> decklists = new ArrayList();

			public void run() {
				try {
					CSVReader reader = getCsvReader("getDecks", "");
					while (true) {
						List<String> fields = reader.getFields();
						if (fields == null)
							break;
						if (fields.size() != 2)
							throw new ArcaneException(
									"MTG Vault returned invalid data: "
											+ fields);
						decklists.add(new MtgVaultDecklist(fields.get(1),
								fields.get(0)));
					}
				} catch (IOException ex) {
					throw new ArcaneException(
							"Error communicating with MTG Vault.", ex);
				} finally {
					dialog.setVisible(false);
				}
			}
		}
		;
		LoadVaultDecks loadVaultDecks = new LoadVaultDecks();
		new Thread(loadVaultDecks, "LoadVaultDecks").start();

		dialog.setVisible(true);
		dialog.dispose();

		return loadVaultDecks.decklists;
	}

	String getUsername() {
		if (username == null)
			promptUsernameAndPassword();
		return username;
	}

	String getPassword() {
		if (password == null)
			promptUsernameAndPassword();
		return password;
	}

	public String getName() {
		return "MTG Vault";
	}

	public void savePreferences() {
		Preferences props = Arcane.getInstance().getPrefs();
		if (username != null)
			props.set("mtgvault.username", username);
		if (password != null)
			props.set("mtgvault.password", password);
	}

	public void loadPreferences() {
		Preferences props = Arcane.getInstance().getPrefs();
		username = props.get("mtgvault.username", null);
		password = props.get("mtgvault.password", null);
	}

	CSVReader getCsvReader(String action, String extraParams)
			throws IOException {
		URL url = new URL("http://www.mtgvault.com/api/" + action
				+ ".php?username=" + getUsername() + "&password="
				+ getPassword() + "&" + extraParams);
		return new CSVReader(new InputStreamReader(new BufferedInputStream(url
				.openStream())), ",", "'", true, false);
	}

	static public MtgVaultPlugin getInstance() {
		return instance;
	}
}
