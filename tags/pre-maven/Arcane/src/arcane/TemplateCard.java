
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
