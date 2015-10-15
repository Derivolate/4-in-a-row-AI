﻿using System;

namespace GUI
{
    public enum player { Empty, Alice, Bob }
    public class Game
    {
        private static byte height;
        private static byte width;
        private readonly byte[][] field;

        //0 for noone, 1 for alice and 2 for bob
        public player next_player { get; private set; }
        private player winning { get; set; }
        /// <summary>
        /// A check if the specified person has won
        /// </summary>
        /// <param name="player">1 for alice, 2 for bob</param>
        public bool has_won(player player)
        {
            if (player == winning)
            {
                return true;
            }
            return false;

        }
        /// <summary>
        /// Initializes the field on 0;
        /// </summary>
        public Game(byte _width, byte _height)
        {
            height = _height;
            width = _width;
            field = new byte[_width][];
            for (int i = 0; i < field.Length; i++)
            {
                field[i] = new byte[height];
                for (int j = 0; j < field[i].Length; j++)
                {
                    //Console.WriteLine("X: " + i + ", Y: " + j);
                    field[i][j] = (byte)0;

                }
            }
            next_player = player.Alice;
            winning = 0;
        }

        public byte[][] get_field()
        {
            return field;
        }
        /// <summary>
        /// Adds a stone on top of the specified column
        /// </summary>
        /// <param name="column">The column that the stone must be added to. Minimum of 0 and Maximum of the width of the field</param>
        /// <param name="player">1 for Alice, 2 for Bob</param>
        /// <returns>If the stone could be placed in that column</returns>
        public bool add_stone(byte column, player player, ref string info)
        {
            if (next_player != player)
            {
                info = "It's not this players your turn";
                return false;
            }
            if (column >= field.Length)
            {
                info = "specified invalid (" + column + ") column";
                return false;
            }
            for (byte i = 0; i < height; i++)
            {

                if (field[column][i] == 0)
                {
                    field[column][i] = (byte)player;
                    Console.WriteLine("Dropped a stone for {0} at {1}, {2}", ((int)player == 1 ? "alice" : "bob"), column, i);
                    check_for_win(column, i, player);
                    if (player == player.Alice)
                    {
                        next_player = player.Bob;
                    }
                    else
                    {
                        next_player = player.Alice;
                    }
                    return true;
                }
            }
            info = "column " + column + " is already full";
            return false;
        }
        #region check_for_win
        /// <summary>
        /// Checks if someone has won
        /// </summary>
        /// <p>
        /// Checks in each direction from the given stone if it can make a whole row. If so, the variable winning will be changed
        /// </p>
        /// <param name="x">The x of the given stone</param>
        /// <param name="y">The y of the given stone</param>
        /// <param name="player">1 for alice, 2 for bob</param>
        private void check_for_win(byte x, byte y, player player)
        {
            //Checks from botleft to topright
            byte counter = 1;
            counter += count_for_win_direction(x, y, -1, 1, player);
            counter += count_for_win_direction(x, y, 1, -1, player);
            if (counter >= 4)
            {
                winning = player;
            }

            //checks from topleft to botright
            counter = 1;
            counter += count_for_win_direction(x, y, 1, 1, player);
            counter += count_for_win_direction(x, y, -1, -1, player);
            if (counter >= 4)
            {
                winning = player;
            }

            //checks horizontal
            counter = 1;
            counter += count_for_win_direction(x, y, 0, 1, player);
            counter += count_for_win_direction(x, y, 0, -1, player);
            if (counter >= 4)
            {
                winning = player;
            }

            //checks vertical
            counter = 1;
            counter += count_for_win_direction(x, y, -1, 0, player);
            counter += count_for_win_direction(x, y, 1, 0, player);
            if (counter >= 4)
            {
                winning = player;
            }
        }
        /// <summary>
        /// count the amount of consecutive stones of one player in the given direction
        /// </summary>
        /// <param name="x">the x-coordinate of the start</param>
        /// <param name="y">the y-cooordinate of the start</param>
        /// <param name="dx">the direction in x (can only be -1, 0, or -1)</param>
        /// <param name="dy">the direction in y (can only be -1, 0, or -1)</param>
        /// <param name="ab">the player of which the stones should be counted (1 for alice, 2 for bob)</param>
        /// <returns>The amount of stones from player ab found, not counting the starting stone</returns>
        private byte count_for_win_direction(byte x, byte y, sbyte dx, sbyte dy, player player)
        {
            byte counter = 0;
            sbyte _x = (sbyte)x;
            sbyte _y = (sbyte)y;
            while (true)
            {
                _x += dx;
                _y += dy;
                if (_x < 0 || _x >= field.Length || _y < 0 || _y >= field[0].Length)
                {
                    break;
                }
                if (field[_x][_y] != (byte)player)
                {
                    break;
                }
                counter++;
            }
            return counter;
        }
        #endregion
    }
}