/*
 * DiceTokenizer.cs
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
     * <remarks>A character stream tokenizer.</remarks>
     */
    internal class DiceTokenizer : Tokenizer {

        /**
         * <summary>Creates a new tokenizer for the specified input
         * stream.</summary>
         * 
         * <param name='input'>the input stream to read</param>
         * 
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        public DiceTokenizer(TextReader input)
            : base(input) {

            CreatePatterns();
        }

        /**
         * <summary>Initializes the tokenizer by creating all the token
         * patterns.</summary>
         * 
         * <exception cref='ParserCreationException'>if the tokenizer
         * couldn't be initialized correctly</exception>
         */
        private void CreatePatterns() {
            TokenPattern  pattern;

            pattern = new TokenPattern((int) DiceConstants.ADD,
                                       "ADD",
                                       TokenPattern.PatternType.STRING,
                                       "+");
            AddPattern(pattern);

            pattern = new TokenPattern((int) DiceConstants.SUB,
                                       "SUB",
                                       TokenPattern.PatternType.STRING,
                                       "-");
            AddPattern(pattern);

            pattern = new TokenPattern((int) DiceConstants.DICE,
                                       "DICE",
                                       TokenPattern.PatternType.STRING,
                                       "d");
            AddPattern(pattern);

            pattern = new TokenPattern((int) DiceConstants.OPEN,
                                       "OPEN",
                                       TokenPattern.PatternType.STRING,
                                       "(");
            AddPattern(pattern);

            pattern = new TokenPattern((int) DiceConstants.CLOSE,
                                       "CLOSE",
                                       TokenPattern.PatternType.STRING,
                                       ")");
            AddPattern(pattern);

            pattern = new TokenPattern((int) DiceConstants.NUMBER,
                                       "NUMBER",
                                       TokenPattern.PatternType.REGEXP,
                                       "[0-9]+");
            AddPattern(pattern);

            pattern = new TokenPattern((int) DiceConstants.WHITESPACE,
                                       "WHITESPACE",
                                       TokenPattern.PatternType.REGEXP,
                                       "[ \\t\\n\\r]+");
            pattern.SetIgnore();
            AddPattern(pattern);
        }
    }
}
