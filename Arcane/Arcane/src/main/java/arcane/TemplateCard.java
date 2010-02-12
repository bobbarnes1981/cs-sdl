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


package arcane;

/**
 * Provides extra getters solely for StringTemplate output.
 */
public class TemplateCard extends Card {
	public TemplateCard (Card card) {
		super(card);
	}

	public TemplateCard (Card card, int qty) {
		super(card, qty);
	}
	
	public String getQty () {
		return Integer.toString(qty);
	}
	
	public String getCsvName () {
		String csvName = getNameWithPictureNumber();
		if (csvName.contains("\"") || csvName.contains(",")) return '"' + csvName.replace("\"", "\"\"") + '"';
		return csvName;
	}

	public String getSet () {
		return set.toUpperCase();
	}

	public String getShortSet () {
		return Arcane.getInstance().getAlternateSets(set).iterator().next().toUpperCase();
	}

	public String getNameWithPictureNumber () {
		if (pictureNumber > 0)
			return englishName + " (" + pictureNumber + ")";
		else
			return englishName;
	}

	public String getType () {
		return typeSpecialCharacters;
	}

	public String getLegal () {
		return legalSpecialCharacters;
	}

	public String getLegalIndented () {
		return legalSpecialCharacters.replace("\r\n", "\r\n    ");
	}
}
