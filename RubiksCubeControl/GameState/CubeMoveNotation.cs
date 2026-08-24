using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RubiksCubeControl.GameState
{
    public static class CubeMoveNotation
    {
        public static string ValidCharacters = "UEDRSLFMBXYZxyzfbudlr2 '";

        public static string[] InvalidStrings = new string[] { " 2 ", " ' ", " 2' " };

        public static Dictionary<char, RubiksCubeMoves> CharToMoveEnumDictionary = new Dictionary<char, RubiksCubeMoves>()
        {
            {'U', RubiksCubeMoves.Up},
            {'E', RubiksCubeMoves.Equator},
            {'D', RubiksCubeMoves.Down},

            {'F', RubiksCubeMoves.Front},
            {'S', RubiksCubeMoves.Slice},
            {'B', RubiksCubeMoves.Back},

            {'L', RubiksCubeMoves.Left},
            {'M', RubiksCubeMoves.Middle},
            {'R', RubiksCubeMoves.Right},

            {'X', RubiksCubeMoves.X},
            {'Y', RubiksCubeMoves.Y},
            {'Z', RubiksCubeMoves.Z},
        };

        /// <summary>
        /// Parses Half Turn Metric Singmaster Notation. 
        /// Supports single-character wide move notation only (no 'w' suffix).
        /// Each move must be delimiterd by 1 (or more) whitespace characters.
        /// </summary>        
        /// <remarks>
        /// Exhaustive list of supported commands, seperated by spaces:
        /// U E D R S L F M B x y z X Y Z 
        /// U' E' D' R' S' L' F' M' B' x' y' z' 
        /// U2 E2 D2 R2 S2 L2 F2 M2 B2 x2 y2 z2 
        /// U2' E2' D2' R2' S2' L2' F2' M2' B2' x2' y2' z2' 
        /// f b u d l r 
        /// f' b' u' d' l' r' 
        /// f2 b2 u2 d2 l2 r2 
        /// f2' b2' u2' d2' l2' r2'
        /// </remarks>
        public static List<(RubiksCubeMoves move, bool isPrime)> ParseCubeMoveNotation(string moveNotation)
        {
            List<(RubiksCubeMoves move, bool isPrime)> results = new List<(RubiksCubeMoves move, bool isPrime)>();

            if (string.IsNullOrWhiteSpace(moveNotation))
            {
                return results;
            }

            var invalidChars = moveNotation.Where(c => !(ValidCharacters.Contains(c) || char.IsWhiteSpace(c))).ToArray();
            if (invalidChars.Any())
            {
                int index = moveNotation.IndexOfAny(invalidChars);
                if (index != -1)
                {
                    throw new FormatException($"Input string contains unsupported characters at index position {index}: The character '{moveNotation[index]}' is not a recognized symbol in Half-Turn Metric (HTM) Singmaster notation.");
                }
            }

            string toNormalize = moveNotation.Trim();

            //Regex regex = new Regex(, RegexOptions.Singleline | RegexOptions.NonBacktracking);

            string pattern = "\\b([FBUDLRfbudlrESMxyz]{1})2([']{0,1})[\\s]{0,}";
            string replacement = "$1$2 $1$2 ";

            string normalized = Regex.Replace(toNormalize, pattern, replacement, RegexOptions.NonBacktracking);

            normalized = normalized.Replace("\t", " ");
            normalized = normalized.Replace("\r", " ");
            normalized = normalized.Replace("\n", " ");
            normalized = normalized.Replace("  ", " ");

            normalized = normalized.Replace("f' ", "F' S' ");
            normalized = normalized.Replace("b' ", "B' S' ");
            normalized = normalized.Replace("u' ", "U' E' ");
            normalized = normalized.Replace("d' ", "D' E' ");
            normalized = normalized.Replace("l' ", "L' M' ");
            normalized = normalized.Replace("r' ", "R' M' ");
            normalized = normalized.Replace("f ", "F S ");
            normalized = normalized.Replace("b ", "B S ");
            normalized = normalized.Replace("u ", "U E ");
            normalized = normalized.Replace("d ", "D E ");
            normalized = normalized.Replace("l ", "L M ");
            normalized = normalized.Replace("r ", "R M ");
            normalized = normalized.Replace("x", "X");
            normalized = normalized.Replace("y", "Y");
            normalized = normalized.Replace("z", "Z");

            string toSplit = Regex.Replace(normalized, "([\\s]{1,})", " ", RegexOptions.NonBacktracking);

            toSplit = toSplit.Trim();

            var invalidCombinations = InvalidStrings.Where(s => toSplit.Contains(s)).ToArray();
            if (invalidCombinations.Any())
            {
                string first = invalidCombinations[0];
                int index2 = toSplit.IndexOf(first);
                if (index2 != -1)
                {
                    throw new FormatException($"Input string was found to contain an orphaned/invalid notation construct at index position {index2} after normalization: The construct \"{first}\" is missing the corresponding move to be applied to. Please check your input for this orphaned character(s). This may be a result of a bug of the normalization routine; Multiple moves without any whitespace in between, followed by a 2 or ' (prime) may result in this, i.g. \"UF2\" or \"RL'\".");
                }
            }

            string[] moveStrings = toSplit.Split(' ');

            foreach (string toParse in moveStrings)
            {
                bool prime = toParse.Contains("'");
                string moveString = toParse.Replace("'", "").Trim();

                RubiksCubeMoves move = CharToMoveEnumDictionary[moveString[0]];

                results.Add(new(move, prime));
            }

            return results;
        }
    }
}
