
package arcane.network;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.HashMap;
import java.util.LinkedHashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.Map.Entry;

import arcane.Arcane;
import arcane.Card;

import com.captiveimagination.jgn.MessageClient;
import com.captiveimagination.jgn.convert.ArrayConverter;
import com.captiveimagination.jgn.convert.ConversionException;
import com.captiveimagination.jgn.convert.type.FieldExternalizable;
import com.captiveimagination.jgn.convert.type.FieldSerializable;
import com.captiveimagination.jgn.message.Message;
import com.captiveimagination.jgn.message.type.PlayerMessage;

public final class LobbyGame implements FieldSerializable {
	static private Arcane arcane = Arcane.getInstance();

	private short gameID;
	private Player[] players;

	public short getId () {
		return gameID;
	}

	public void setGameID (short gameID) {
		this.gameID = gameID;
	}

	public short getGameID () {
		return gameID;
	}

	public void setPlayers (Player[] players) {
		this.players = players;
	}

	public Player[] getPlayers () {
		return players;
	}

	public Player getPlayer (short playerID) {
		for (Player player : players)
			if (player.getId() == playerID) return player;
		return null;
	}

	public <T extends Message & PlayerMessage> Player getPlayer (T message) {
		return getPlayer(message.getPlayerId());
	}

	public String toString () {
		return String.valueOf(gameID);
	}
}
