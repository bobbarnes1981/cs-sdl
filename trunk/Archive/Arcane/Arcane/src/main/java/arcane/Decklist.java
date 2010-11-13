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

import java.io.IOException;
import java.util.List;
import java.util.Map;

public interface Decklist {
	public String getName ();

	public boolean exists ();

	public boolean isOpenable ();

	public void open () throws IOException;

	public List<DecklistCard> getDecklistCards ();

	public void save (List<Card> deckCards, Map<Card, Integer> deckCardToQty, List<Card> sideCards, Map<Card, Integer> sideCardToQty) throws IOException;

	public String getData ();
}
