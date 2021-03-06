
This utility can generate any MTG card. To generate cards, you need a decklist. This utility will read CSV files, "mwDeck" files (which are created by Magic Workstation), and "dec" files (which are created by MTGO). To make a CSV decklist file, create a CSV file with two columns. In the first column put the quantity, in the second column put the name of the card. Run the generateCards.bat and specify the CSV decklist file. It will generate the cards. For example CSV decklists, see the files in the directory "decks". You can also include a third column in the CSV which will be the set abbreviation for the card. The fourth column is for promo cards (eg, try "FNM").

------------

Each of the ".bat" files will prompt you for a CSV decklist file or a directory of CSV decklists. One or more decklist files or directories can be dropped directly on the ".bat" files.

generateCards.bat: Generates individual card images.
generateCards-decklists.bat: Generates individual decklist card images.
generatePages.bat: Generates pages of card images.
generatePages-decklists.bat: Generates pages of decklist card images.
misc\diffDecklists.bat: Shows the difference between two decklists.
misc\createCard.bat: Prompts for the information to generate a card.
misc\buildFontSizeCache.bat: Creates or appends to the data\fontSizes.csv file for faster card generation.
misc\generateAll.bat: Generates all versions of all cards in the database.
misc\installContextMenuItems.bat: Adds context menu items to the Windows menu when a decklist is right clicked while shift is down.
misc\prices.bat: Gives the latest pricing information for decklists.
misc\import\mwsToCards.bat: Rebuilds the data\cards.csv file from an MWS masterbase.
misc\import\mwsToLand.bat: Rebuilds a data\cards-language.csv file from an MWS masterbase.

------------

The card database for the utility exists in "data/cards.csv". This CSV file is not a decklist, rather it holds all the data about every MTG card. This file can be modified or replaced, allowing you to generate cards with your own content.

------------

Various settings for the utility can be configured in the "config" text files. If you are having problem with it finding your art, set "art.debug=1".

The font settings in the config files are defined as follows:

name=size, fontFiles[/fontFiles][, options]

The parts in brackets are optional.

size: A numeric value that is the point size of the font. Fractional values are supported, eg "24.2"

fontFiles: A greater than (">") delimited list of font filenames. The generator looks in the "fonts" directory for these files. It looks first for the leftmost font, then moves down the list. Eg, "MatrixB.ttf>GaramondB.ttf" uses MatrixB if it exists in "fonts", otherwise it uses GaramondB. If there is a slash and another set of filenames, then these are used when this text is italic.

options: A space (" ") delimited list of font options. Options are specified using "name:value". Available options:

leading: This is the percent of the font height to use between lines. Eg, "leading:45" means 45% of the font's height will be used as empty space between lines of text. Text that will never span more than one line will never use this value.

centerX: true/false. When true, the text will be centered horizontally.

centerY: true/false. When true, the text will be centered vertically.

shadow: true/false. When true, the text will have a black shadow.

color: Comma separated values for red, green, and blue ranging from 0 to 255. Eg, "color:255,255,255" is white, "color:0,0,0" is black, "color:0,255,0" is green, "color:0,90,0" is dark green, etc.

------------

More info in the MWS Pictures forum...
http://www.mwsdata.com/forum/
http://www.mwsdata.com/forum/viewtopic.php?t=424
