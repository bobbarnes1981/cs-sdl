#region LICENSE
/* 
 * (c) 2005 Simon Gillespie
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */
#endregion LICENSE

// vector mathematics functions
namespace SdlDotNetExamples.Isotope
{
    /// <summary>
    /// 
    /// </summary>
    static class Vector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int[] AddVector(int[] v1, int[] v2)
        {
            int[] vout = new int[3];
            for (int i = 0; i <= 2; i++)
            {
                vout[i] = v1[i] + v2[i];
            }
            return vout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int[] SubtractVector(int[] v1, int[] v2)
        {
            int[] vout = new int[3];
            for (int i = 0; i <= 2; i++)
            {
                vout[i] = v1[i] - v2[i];
            }
            return vout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int[] MultiplyVector(int[] v1, int[] v2)
        {
            int[] vout = new int[3];
            for (int i = 0; i <= 2; i++)
            {
                vout[i] = v1[i] * v2[i];
            }
            return vout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static int[] DivideVector(int[] v1, int[] v2)
        {
            // Divides the elements of vector v1 by the elements of vector v2.
            int[] vout = new int[3];
            for (int i = 0; i <= 2; i++)
            {
                vout[i] = v1[i] / v2[i];
            }
            return vout;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        public static void CopyVector(int[] v1, int[] v2)
        {
            for (int i = 0; i <= 2; i++)
            {
                v2[i] = v1[i];
            }
        }

        // Direction defines a facing vector (uses 1 for positive and -1 for negative or zero for same) of v1 to v2
        public static int[] Direction(int[] v1, int[] v2)
        {
            // defines a facing vector (uses 1 for positive and -1 for negative or zero for same) of v1 to v2 """
            int[] vout = new int[3];
            for (int i = 0; i <= 2; i++)
            {
                if (v1[i] - v2[i] < 0)
                {
                    vout[i] = 1;
                }
                else if (v1[i] - v2[i] > 0)
                {
                    vout[i] = -1;
                }
                else
                {
                    vout[i] = 0;
                }
            }
            return (vout);
        }

        // converts vector to a facing direction
        // assumes that the vector has only one non zero value
        public static int VectorToFace(int[] vector)
        {
            int face = 0;
            for (int i = 0; i <= 2; i++)
            {
                int element = vector[i];
                if (element == -1 || element == 1)
                {
                    if (element == -1)
                    {
                        element = 1;
                    }
                    else
                    {
                        element = 0;
                    }
                    face = element + (i * 2);
                }
            }
            return face;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="v"></param>
        ///// <returns></returns>
        //public static int SqrMagnitude(int[] v)
        //{
        //    // sqr of the magnitude of the vector
        //    int sqr_mag = 0;
        //    foreach (int i in v)
        //    {
        //        sqr_mag = sqr_mag + i * i;
        //    }
        //    return (sqr_mag);
        //}

    }

}