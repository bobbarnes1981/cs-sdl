<?
////////////////////////////////////////////////////////////////////////
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
////////////////////////////////////////////////////////////////////////
class MasterBase {
	public $cards = array();

	static private $totalCardsInSet;

	public function __construct ($mwsFileName) {
		$mwsFile = fopen_utf8($mwsFileName, 'rb');
		if (!$mwsFile) error('Unable to open masterbase CSV file: ' . $mwsFileName);
		
		echo "Processing MWS masterbase...";

		// windows-1252
		//$csv = new CSV(';', "\r\n", '"'); // Cell separator, row separator, value enclosure.

		//$csv->setContent(file_get_contents($mwsFileName, false , null)); // Parse the string content.

		//$rows = $csv->getArray();
		$i = 0;
		while (($row = fgetcsv($mwsFile, 6000, ';', '"')) !== FALSE) {
		//foreach($rows as $row){
			if($i++ == 0) continue; //skip first line
			if ($i++ % 50 == 0) echo '.';

			// Extract.
			$title = (string)trim($row[0]);
			$set = (string)trim($row[1]);
			$color = (string)trim($row[4]);
			$type = (string)trim($row[6]);
			$p = (string)trim($row[8]);
			$t = (string)trim($row[9]);
			$flavor = (string)trim($row[10]);
			$rarity = (string)trim($row[11]);
			$cost = (string)trim($row[5]);
			$legal = trim($row[7]);
			$pic = (string)trim($row[2]);
			$artist = (string)trim($row[12]);
			$collectorNumber = (string)trim($row[13]);

			// Title.
			if ($set == 'VG') $title = 'Avatar: ' . $title;

			// Casting cost.
			$cost = $this->replaceDualManaSymbols($cost);
			$cost = preg_replace('/([0-9]+)/', '{\\1}', $cost); //
			$cost = preg_replace('/([WUBRGXYZ])/', '{\\1}', $cost);
			$cost = preg_replace('/{{([0-9XYZWUBRG])}{([WUBRG])}}/', '{\\1\\2}', $cost);

			// Color.
			if ($color == 'Z/Z') {
				// Determine split card colors.
				$cost1 = substr($cost, 0, strpos($cost, '/'));
				$colors = Card::getCostColors($cost1);
				$color = strlen($colors) == 1 ? $colors : 'Gld';

				$color .= '/';

				$cost2 = substr($cost, strpos($cost, '/') + 1);
				$colors = Card::getCostColors($cost2);
				$color .= strlen($colors) == 1 ? $colors : 'Gld';
			}

			//php5 fixups
			$flavor = str_replace("\xA0", '', $flavor);
			$flavor = iconv('windows-1250', 'utf-8', $flavor);
			$legal = iconv('windows-1250', 'utf-8' ,$legal);
			$artist = iconv('windows-1250', 'utf-8' ,$artist);
			//convert title and type just in case
			$title = iconv('windows-1250', 'utf-8', $title);
			$type = iconv('windows-1250', 'utf-8' ,$type);

			// Type.
			$type = str_replace(' - ', ' ??? ', $type);

			// Legal.
			$legal = $this->replaceDualManaSymbols($legal);
			$legal = preg_replace('/\%([0-9]+)/', '{\\1}', $legal);
			$legal = preg_replace('/\%([WUBRGTXYZ])/', '{\\1}', $legal);
			$legal = preg_replace('/\%([C])/', '{Q}', $legal);
			$legal = preg_replace('/#([^#]+)# ??? /', '\\1 ??? ', $legal); // Remove italics from ability keywords.
			$legal = str_replace("\r\n----\r\n", "\n-----\n", $legal); // Flip card separator.
			$legal = str_replace('Creature - ', 'Creature ??? ', $legal);
			$legal = str_replace(' upkeep - ', ' upkeep???', $legal);
			$legal = str_replace(' - ', ' ??? ', $legal);
			$legal = str_replace('AE', '??', $legal);
			$legal = str_replace(".]", ".)", $legal);
			$legal = str_replace("\r\n", "\n", $legal);
			// Fix vanguard inconsistencies.
			if (preg_match('/Starting & Max[^\+\-]+([\+\-][0-9]+)[^\+\-]+([\+\-][0-9]+)/', $legal, $matches))
				$legal = 'Hand ' . $matches[1] . ', Life ' . $matches[2] . "\n" . substr($legal, 0, strpos($legal, ' Starting & Max'));
			if (preg_match('/Hand Size[^\+\-]+([\+\-][0-9]+)[^\+\-]+([\+\-][0-9]+)\.?/', $legal, $matches))
				$legal = 'Hand ' . $matches[1] . ', Life ' . $matches[2] . "\n" . substr($legal, 0, strpos($legal, 'Hand Size'));
			$legal = trim($legal);

			// Flavor.
			$flavor = str_replace("'", '???', $flavor); // ' to ???
			$flavor = preg_replace('/"([^"]*)"/', '???\\1???', $flavor); // "text" to ???text???
			$flavor = preg_replace("/(.*[^.]) '([^']*)'/", "\\1 ???\\2???", $flavor); // 'text' to ???text???
			$flavor = preg_replace('/(.*[^.]) ???(.*)???/', '\\1 ???\\2???', $flavor); // ???text??? to ???text???
			$flavor = str_replace('??????', '??????', $flavor); // ?????? to ??????
			$flavor = str_replace('??????', '??????', $flavor); // ?????? to ??????
			$flavor = str_replace('??????', '??????', $flavor); // ?????? to ??????
			$flavor = str_replace(',???', '???,', $flavor); // ,??? to ???,
			$flavor = preg_replace("/\r\n- ?/", "\n???", $flavor); // - to ???
			$flavor = preg_replace("/\r\n#- ?/", "\n#???", $flavor);
			$flavor = preg_replace("/ - /", "???", $flavor);
			$flavor = str_replace('AE', '??', $flavor);
			$flavor = str_replace("\r\n", "\n", $flavor);
			$flavor = str_replace('"', '???', $flavor); // " to ???

			// Store.
			$card = new Card();
			$card->title = $title;
			$card->set = $set;
			$card->color = $color;
			$card->type = $type;
			$card->pt = ($p != "" && $t !="") ? $p . '/' . $t : (preg_match('/%([0-9]+)#/', $p, $matches) ? "/$matches[1]" : '');
			$card->flavor = $flavor;
			$card->rarity = $rarity;
			$card->cost = $cost;
			$card->legal = $legal;
			$card->pic = $pic;
			$card->artist = $artist;
			$card->collectorNumber = $collectorNumber;
			$this->cards[] = $card;
		}

		// Compute total cards in each set.
		$setToCollectorNumbers = array();
		foreach ($this->cards as $card) {
			// Only count cards with collector numbers.
			if (!$card->collectorNumber) continue;
			// Don't count the same collector number twice.
			if (!@$setToCollectorNumbers[$card->set]) $setToCollectorNumbers[$card->set] = array();
			if (@$setToCollectorNumbers[$card->set][$card->collectorNumber]) continue;

			$setToCollectorNumbers[$card->set][$card->collectorNumber] = true;
		}
		foreach ($this->cards as $card) {
			if (!$card->collectorNumber) continue;
			// Try hardcoded value first.
			$cardsInSet = MasterBase::getTotalCardsInSet($card->set);
			// Then try computed vallue.
			if (!$cardsInSet && @$setToCollectorNumbers[$card->set]) $cardsInSet = count($setToCollectorNumbers[$card->set]);
			if (!$cardsInSet) continue;
			$card->collectorNumber .= '/' . $cardsInSet;
		}
	}

	private function replaceDualManaSymbols ($text) {
		$text = str_replace('%V', '{UB}', $text);
		$text = str_replace('%P', '{RW}', $text);
		$text = str_replace('%Q', '{BG}', $text);
		$text = str_replace('%A', '{GW}', $text);
		$text = str_replace('%I', '{UR}', $text);
		$text = str_replace('%L', '{RG}', $text);
		$text = str_replace('%O', '{WB}', $text);
		$text = str_replace('%S', '{GU}', $text);
		$text = str_replace('%K', '{BR}', $text);
		$text = str_replace('%D', '{WU}', $text);
		$text = str_replace('%N', '{S}', $text);
		
		$text = str_replace('%E', '{2W}', $text);
		$text = str_replace('%F', '{2U}', $text);
		$text = str_replace('%H', '{2B}', $text);
		$text = str_replace('%J', '{2R}', $text);
		$text = str_replace('%M', '{2G}', $text);
		return $text;
	}

	private function getTotalCardsInSet ($set) {
		if (!MasterBase::$totalCardsInSet) {
			// These totals are not computed correctly by counting cards in the masterbase. The masterbase is wrong?
			MasterBase::$totalCardsInSet = array();
			MasterBase::$totalCardsInSet["CS"] = 155;
			MasterBase::$totalCardsInSet["HL"] = 140;
			MasterBase::$totalCardsInSet["WL"] = 167;
		}
		return @MasterBase::$totalCardsInSet[$set];
	}
}

?>
