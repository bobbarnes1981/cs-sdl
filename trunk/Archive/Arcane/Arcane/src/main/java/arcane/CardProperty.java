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

// Missing: artist
public enum CardProperty {
	qty("Qty"), //
	name("Name"), //
	castingCost("Cost"), //
	convertedCost("Converted Cost"), //
	type("Type"), //
	creatureType("Creature Type"), //
	color("Color"), //
	manaProduced("Mana Produced"), //
	pt("P/T"), //
	power("Power"), //
	touhgness("Toughness"), //
	rarity("Rarity"), //
	rating("Rating"), //
	flags("Flags"), //
	set("Set"), //
	setName("Set Name"), //
	pictureNumber("Art #"), //
	collectorNumber("Collector #"), //
	legal("Legal Text"), //
	ownedQty("Owned Qty"), //
	price("Price");

	public final String text;

	CardProperty (String text) {
		this.text = text;
	}
}
