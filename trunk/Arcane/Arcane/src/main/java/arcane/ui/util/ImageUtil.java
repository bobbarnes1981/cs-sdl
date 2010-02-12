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


package arcane.ui.util;

import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.image.BufferedImage;
import java.awt.image.ConvolveOp;
import java.awt.image.Kernel;
import java.io.IOException;
import java.io.InputStream;

import javax.imageio.ImageIO;

public class ImageUtil {
	static public BufferedImage getImage (InputStream stream) throws IOException {
		Image tempImage = ImageIO.read(stream);
		BufferedImage image = new BufferedImage(tempImage.getWidth(null), tempImage.getHeight(null), BufferedImage.TYPE_INT_RGB);
		Graphics2D g2 = image.createGraphics();
		g2.drawImage(tempImage, 0, 0, null);
		g2.dispose();
		return image;
	}

	static public BufferedImage getBlurredImage (BufferedImage image, int radius, float intensity) {
		float weight = intensity / (radius * radius);
		float[] elements = new float[radius * radius];
		for (int i = 0, n = radius * radius; i < n; i++)
			elements[i] = weight;
		ConvolveOp blurOp = new ConvolveOp(new Kernel(radius, radius, elements));
		return blurOp.filter(image, null);
	}
}
