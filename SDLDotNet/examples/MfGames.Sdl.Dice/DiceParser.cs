/*
 * $RCSfile$
 * Copyright (C) 2004 D. R. E. Moonfire (d.moonfire@mfgames.com)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 * 
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

/*
 * DiceParser.cs
 * 
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 * 
 * Do not transmit.
 * 
 * Copyright (c) 2004 Moonfire Games. All rights reserved.
 */

using System.IO;

using PerCederberg.Grammatica.Parser;

namespace MfGames.Utility.Dice {

    /**
     * <remarks>A token stream parser.</remarks>
     */
    internal class DiceParser : RecursiveDescentParser {

        /**
         * <summary>An enumeration with the generated production node
         * identity constants.</summary>
         */
        private enum SynteticPatterns {
        }

        /**
         * <summary>Creates a new parser.</summary>
         * 
         * <param name='input'>the input stream to read from</param>
         * 
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        public DiceParser(TextReader input)
            : base(new DiceTokenizer(input)) {

            CreatePatterns();
        }

        /**
         * <summary>Creates a new parser.</summary>
         * 
         * <param name='input'>the input stream to read from</param>
         * 
         * <param name='analyzer'>the analyzer to parse with</param>
         * 
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        public DiceParser(TextReader input, Analyzer analyzer)
            : base(new DiceTokenizer(input), analyzer) {

            CreatePatterns();
        }

        /**
         * <summary>Initializes the parser by creating all the production
         * patterns.</summary>
         * 
         * <exception cref='ParserCreationException'>if the parser
         * couldn't be initialized correctly</exception>
         */
        private void CreatePatterns() {
            ProductionPattern             pattern;
            ProductionPatternAlternative  alt;

            pattern = new ProductionPattern((int) DiceConstants.EXPRESSION,
                                            "Expression");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) DiceConstants.ADDITION_FRAGMENT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) DiceConstants.ADDITION_FRAGMENT,
                                            "AdditionFragment");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) DiceConstants.NUMERIC_FRAGMENT, 1, 1);
            alt.AddToken((int) DiceConstants.ADD, 1, 1);
            alt.AddProduction((int) DiceConstants.ADDITION_FRAGMENT, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) DiceConstants.NUMERIC_FRAGMENT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) DiceConstants.NUMERIC_FRAGMENT,
                                            "NumericFragment");
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) DiceConstants.RANDOM_FRAGMENT, 1, 1);
            pattern.AddAlternative(alt);
            alt = new ProductionPatternAlternative();
            alt.AddProduction((int) DiceConstants.CONSTANT_FRAGMENT, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) DiceConstants.CONSTANT_FRAGMENT,
                                            "ConstantFragment");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) DiceConstants.NUMBER, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);

            pattern = new ProductionPattern((int) DiceConstants.RANDOM_FRAGMENT,
                                            "RandomFragment");
            alt = new ProductionPatternAlternative();
            alt.AddToken((int) DiceConstants.NUMBER, 0, 1);
            alt.AddToken((int) DiceConstants.DICE, 1, 1);
            alt.AddToken((int) DiceConstants.NUMBER, 1, 1);
            pattern.AddAlternative(alt);
            AddPattern(pattern);
        }
    }
}
