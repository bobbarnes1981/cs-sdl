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
 * DiceAnalyzer.cs
 * 
 * THIS FILE HAS BEEN GENERATED AUTOMATICALLY. DO NOT EDIT!
 * 
 * Do not transmit.
 * 
 * Copyright (c) 2004 Moonfire Games. All rights reserved.
 */

using PerCederberg.Grammatica.Parser;

namespace MfGames.Utility.Dice {

    /**
     * <remarks>A class providing callback methods for the
     * parser.</remarks>
     */
    internal abstract class DiceAnalyzer : Analyzer {

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public override void Enter(Node node) {
            switch (node.GetId()) {
            case (int) DiceConstants.ADD:
                EnterAdd((Token) node);
                break;
            case (int) DiceConstants.SUB:
                EnterSub((Token) node);
                break;
            case (int) DiceConstants.DICE:
                EnterDice((Token) node);
                break;
            case (int) DiceConstants.OPEN:
                EnterOpen((Token) node);
                break;
            case (int) DiceConstants.CLOSE:
                EnterClose((Token) node);
                break;
            case (int) DiceConstants.NUMBER:
                EnterNumber((Token) node);
                break;
            case (int) DiceConstants.EXPRESSION:
                EnterExpression((Production) node);
                break;
            case (int) DiceConstants.ADDITION_FRAGMENT:
                EnterAdditionFragment((Production) node);
                break;
            case (int) DiceConstants.NUMERIC_FRAGMENT:
                EnterNumericFragment((Production) node);
                break;
            case (int) DiceConstants.CONSTANT_FRAGMENT:
                EnterConstantFragment((Production) node);
                break;
            case (int) DiceConstants.RANDOM_FRAGMENT:
                EnterRandomFragment((Production) node);
                break;
            }
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public override Node Exit(Node node) {
            switch (node.GetId()) {
            case (int) DiceConstants.ADD:
                return ExitAdd((Token) node);
            case (int) DiceConstants.SUB:
                return ExitSub((Token) node);
            case (int) DiceConstants.DICE:
                return ExitDice((Token) node);
            case (int) DiceConstants.OPEN:
                return ExitOpen((Token) node);
            case (int) DiceConstants.CLOSE:
                return ExitClose((Token) node);
            case (int) DiceConstants.NUMBER:
                return ExitNumber((Token) node);
            case (int) DiceConstants.EXPRESSION:
                return ExitExpression((Production) node);
            case (int) DiceConstants.ADDITION_FRAGMENT:
                return ExitAdditionFragment((Production) node);
            case (int) DiceConstants.NUMERIC_FRAGMENT:
                return ExitNumericFragment((Production) node);
            case (int) DiceConstants.CONSTANT_FRAGMENT:
                return ExitConstantFragment((Production) node);
            case (int) DiceConstants.RANDOM_FRAGMENT:
                return ExitRandomFragment((Production) node);
            }
            return node;
        }

        /**
         * <summary>Called when adding a child to a parse tree
         * node.</summary>
         * 
         * <param name='node'>the parent node</param>
         * <param name='child'>the child node, or null</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public override void Child(Production node, Node child) {
            switch (node.GetId()) {
            case (int) DiceConstants.EXPRESSION:
                ChildExpression(node, child);
                break;
            case (int) DiceConstants.ADDITION_FRAGMENT:
                ChildAdditionFragment(node, child);
                break;
            case (int) DiceConstants.NUMERIC_FRAGMENT:
                ChildNumericFragment(node, child);
                break;
            case (int) DiceConstants.CONSTANT_FRAGMENT:
                ChildConstantFragment(node, child);
                break;
            case (int) DiceConstants.RANDOM_FRAGMENT:
                ChildRandomFragment(node, child);
                break;
            }
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterAdd(Token node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitAdd(Token node) {
            return node;
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterSub(Token node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitSub(Token node) {
            return node;
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterDice(Token node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitDice(Token node) {
            return node;
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterOpen(Token node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitOpen(Token node) {
            return node;
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterClose(Token node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitClose(Token node) {
            return node;
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterNumber(Token node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitNumber(Token node) {
            return node;
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterExpression(Production node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitExpression(Production node) {
            return node;
        }

        /**
         * <summary>Called when adding a child to a parse tree
         * node.</summary>
         * 
         * <param name='node'>the parent node</param>
         * <param name='child'>the child node, or null</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void ChildExpression(Production node, Node child) {
            node.AddChild(child);
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterAdditionFragment(Production node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitAdditionFragment(Production node) {
            return node;
        }

        /**
         * <summary>Called when adding a child to a parse tree
         * node.</summary>
         * 
         * <param name='node'>the parent node</param>
         * <param name='child'>the child node, or null</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void ChildAdditionFragment(Production node, Node child) {
            node.AddChild(child);
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterNumericFragment(Production node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitNumericFragment(Production node) {
            return node;
        }

        /**
         * <summary>Called when adding a child to a parse tree
         * node.</summary>
         * 
         * <param name='node'>the parent node</param>
         * <param name='child'>the child node, or null</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void ChildNumericFragment(Production node, Node child) {
            node.AddChild(child);
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterConstantFragment(Production node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitConstantFragment(Production node) {
            return node;
        }

        /**
         * <summary>Called when adding a child to a parse tree
         * node.</summary>
         * 
         * <param name='node'>the parent node</param>
         * <param name='child'>the child node, or null</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void ChildConstantFragment(Production node, Node child) {
            node.AddChild(child);
        }

        /**
         * <summary>Called when entering a parse tree node.</summary>
         * 
         * <param name='node'>the node being entered</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void EnterRandomFragment(Production node) {
        }

        /**
         * <summary>Called when exiting a parse tree node.</summary>
         * 
         * <param name='node'>the node being exited</param>
         * 
         * <returns>the node to add to the parse tree, or
         *          null if no parse tree should be created</returns>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual Node ExitRandomFragment(Production node) {
            return node;
        }

        /**
         * <summary>Called when adding a child to a parse tree
         * node.</summary>
         * 
         * <param name='node'>the parent node</param>
         * <param name='child'>the child node, or null</param>
         * 
         * <exception cref='ParseException'>if the node analysis
         * discovered errors</exception>
         */
        public virtual void ChildRandomFragment(Production node, Node child) {
            node.AddChild(child);
        }
    }
}
