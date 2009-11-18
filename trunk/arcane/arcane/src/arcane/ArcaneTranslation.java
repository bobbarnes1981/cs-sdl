package arcane;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;

import arcane.util.Preferences;

public class ArcaneTranslation extends Preferences{
	private final String fileName;
	
	public ArcaneTranslation (String fileName) {
		this.fileName = fileName;
		try {
			FileInputStream stream = new FileInputStream(fileName);
			load(stream);
			stream.close();
		} catch (FileNotFoundException ignored) {
		} catch (IOException ex) {
			throw new ArcaneException("Error reading \"" + fileName + "\".", ex);
		}
	}
	
	public void load (FileInputStream stream) throws IOException {
		props.loadFromXML(stream);
	}

	public void store (FileOutputStream stream, String comments) throws IOException {
		props.storeToXML(stream, comments);
	}
}
