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

package arcane.deckbuilder.cardgenerator;

import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.IOException;

import javax.swing.JFileChooser;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;

import arcane.Arcane;
import arcane.ArcaneException;
import arcane.DecklistFile;
import arcane.deckbuilder.DeckBuilderPlugin;
import arcane.deckbuilder.ui.DeckBuilder; //import arcane.ui.util.ProgressDialog;
import arcane.util.Preferences;
import arcane.util.Util;

public class CardGeneratorPlugin extends DeckBuilderPlugin {
	static private File directory;
	static private JFileChooser dirFileChooser;

	private DeckBuilder deckBuilder;
	
	private String generateCardsWindows = "generateCards.bat";
	private String generateCardsDecklistsWindows = "generateCards-decklists.bat";
	private String generatePagesWindows = "generatePages.bat";
	private String generatePagesDecklistsWindows = "generatePages-decklists.bat";
	private String createCardWindows = "createCard.bat";
	
	private String generateCardsUnix = "generateCards.sh";
	private String generateCardsDecklistsUnix = "generateCards-decklists.sh";
	private String generatePagesUnix = "generatePages.sh";
	private String generatePagesDecklistsUnix = "generatePages-decklists.sh";
	private String createCardUnix = "createCard.sh";
	
	private String generateCards;
	private String generateCardsDecklists;
	private String generatePages;
	private String generatePagesDecklists;
	private String createCard;
	
	private String[] commandString;


	public void install(DeckBuilder deckBuilder) {
		this.deckBuilder = deckBuilder;
		if (Util.isWindows) {
			generateCards = generateCardsWindows;
			generateCardsDecklists = generateCardsDecklistsWindows;
			generatePages = generatePagesWindows;
			generatePagesDecklists = generatePagesDecklistsWindows;
			createCard = createCardWindows;
		}
		else
		{
			generateCards = generateCardsUnix;
			generateCardsDecklists = generateCardsDecklistsUnix;
			generatePages = generatePagesUnix;
			generatePagesDecklists = generatePagesDecklistsUnix;
			createCard = createCardUnix;
		}
		JMenu menu = new JMenu(getName());
		{
			JMenuItem menuItem = new JMenuItem("Generate cards...");
			menu.add(menuItem);
			menuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					launchGenerator(generateCards);
				}
			});
		}
		{
			JMenuItem menuItem = new JMenuItem("Generate decklist card...");
			menu.add(menuItem);
			menuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					launchGenerator(generateCardsDecklists);
				}
			});
		}
		{
			JMenuItem menuItem = new JMenuItem("Generate pages...");
			menu.add(menuItem);
			menuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					launchGenerator(generatePages);
				}
			});
		}
		{
			JMenuItem menuItem = new JMenuItem(
					"Generate pages (decklist card)...");
			menu.add(menuItem);
			menuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					launchGenerator(generatePagesDecklists);
				}
			});
		}
		menu.addSeparator();
		{
			JMenuItem menuItem = new JMenuItem("Create card...");
			menu.add(menuItem);
			menuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					try {
						String[] commandCreateCard;
						if (Util.isWindows) {
							commandCreateCard = new String[] {
									"cmd",
									"/C",
									"start",
									"Card Generator",
									"cmd",
									"/C",
									directory.getAbsolutePath()
											+ "\\misc\\" + createCard };
						} else {
							commandCreateCard = new String[] {
									directory.getAbsolutePath() + "/misc/" + createCard };
						}
						Runtime.getRuntime().exec(commandCreateCard);
					} catch (IOException ex) {
						throw new ArcaneException(
								"Error launching card generator.", ex);
					}
				}
			});
		}
		menu.addSeparator();
		{
			JMenuItem setDirectoryMenuItem = new JMenuItem(
					"Set card generator directory...");
			menu.add(setDirectoryMenuItem);
			setDirectoryMenuItem.addActionListener(new ActionListener() {
				public void actionPerformed(ActionEvent evt) {
					promptDirectory();
				}
			});
		}
		deckBuilder.addPluginMenu(menu);
	}

	private void promptDirectory() {
//		if (!checkWindows())
//			return;

		if (dirFileChooser == null) {
			dirFileChooser = new JFileChooser((directory == null || !directory
					.exists()) ? "/" : directory.getAbsolutePath());
			dirFileChooser.setFileSelectionMode(JFileChooser.DIRECTORIES_ONLY);
			dirFileChooser.setDialogTitle("Set card generator directory");
		}
		int result = dirFileChooser.showOpenDialog(deckBuilder);
		if (result != JFileChooser.APPROVE_OPTION)
			return;
		File file = dirFileChooser.getSelectedFile();
		if (!file.exists())
			return;
		directory = file;
	}

//	private boolean checkWindows() {
//		if (Util.isWindows) {
//			JOptionPane.showMessageDialog(deckBuilder,
//					"Sorry, the card generator plugin only works on Windows.",
//					"Windows Only", JOptionPane.WARNING_MESSAGE);
//			return false;
//		}
//		return true;
//	}

	private void launchGenerator(String batchFile) {
//		if (!checkWindows())
//			return;

		if (directory == null || !directory.exists()) {
			promptDirectory();
			if (directory == null || !directory.exists())
				return;
		}

		try {
			File tempFile = File.createTempFile("deckbuilder", "cardgenerator");
			if (deckBuilder.getCurrentDecklist() != null)
				tempFile = new File(tempFile.getParent(), deckBuilder
						.getCurrentDecklist().getName()
						+ ".csv");
			DecklistFile decklistFile = new DecklistFile(tempFile
					.getAbsolutePath(), "CSV (csv)");
			decklistFile.save(deckBuilder.getDeckCards(), deckBuilder
					.getDeckCardToQty(), deckBuilder.getSideCards(),
					deckBuilder.getSideCardToQty());
			if (Util.isWindows) {
				commandString = new String[] {
						"cmd",
						"/C",
						"start",
						"Card Generator",
						"cmd /C \"\"" + directory.getAbsolutePath() + "\\"
								+ batchFile + "\" \""
								+ tempFile.getAbsolutePath() + "\"\"" };
			} else {
				commandString = new String[] {
						directory.getAbsolutePath() + "/" + batchFile,
						tempFile.getAbsolutePath()};
			}
			Runtime.getRuntime().exec(commandString);					
		} catch (IOException ex) {
			throw new ArcaneException("Error launching card generator.", ex);
		}
	}

	public void savePreferences() {
		Preferences props = Arcane.getInstance().getPrefs();
		if (directory != null)
			props.set("cardgenerator.directory", directory.getAbsolutePath());
	}

	public void loadPreferences() {
		Preferences props = Arcane.getInstance().getPrefs();
		String directoryString = props.get("cardgenerator.directory", null);
		if (directoryString != null) {
			directory = new File(directoryString);
			if (!directory.exists())
				directory = null;
		}
	}

	public String getName() {
		return "Card Generator";
	}
}
