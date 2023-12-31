using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTSexcel
{
   public static class Common
    {
        public static bool IsBanglaDigit(char chUni)
        {
            switch ((BanglaUni)chUni)
            {
                case BanglaUni.DIGIT_ZERO:
                case BanglaUni.DIGIT_ONE:
                case BanglaUni.DIGIT_TWO:
                case BanglaUni.DIGIT_THREE:
                case BanglaUni.DIGIT_FOUR:
                case BanglaUni.DIGIT_FIVE:
                case BanglaUni.DIGIT_SIX:
                case BanglaUni.DIGIT_SEVEN:
                case BanglaUni.DIGIT_EIGHT:
                case BanglaUni.DIGIT_NINE:
                    return true;
                default:
                    return false;
            }
        }


        public static bool IsBanglaPreKar(char chUni)
        {
            switch ((BanglaUni)chUni)
            {
                case BanglaUni.VOWEL_SIGN_I:
                case BanglaUni.VOWEL_SIGN_E:
                case BanglaUni.VOWEL_SIGN_AI:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBanglaPostKar(char chUni)
        {
            switch ((BanglaUni)chUni)
            {
                case BanglaUni.VOWEL_SIGN_AA:
                case BanglaUni.VOWEL_SIGN_II:
                case BanglaUni.VOWEL_SIGN_U:
                case BanglaUni.VOWEL_SIGN_UU:
                case BanglaUni.VOWEL_SIGN_VOCALIC_R:
                //case BanglaUni.VOWEL_SIGN_VOCALIC_RR:
                case BanglaUni.VOWEL_SIGN_O:
                case BanglaUni.VOWEL_SIGN_AU:
                case BanglaUni.LENGTH_MARK_AU:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBanglaKar(char chUni)
        {
            return IsBanglaPreKar(chUni) || IsBanglaPostKar(chUni);
        }

        public static bool IsBanglaBanjonborno(char chUni)
        {
            switch ((BanglaUni)chUni)
            {
                case BanglaUni.SIGN_CANDRABINDU:
                case BanglaUni.SIGN_ANUSVARA:
                case BanglaUni.SIGN_VISARGA:
                case BanglaUni.LETTER_KA:
                case BanglaUni.LETTER_KHA:
                case BanglaUni.LETTER_GA:
                case BanglaUni.LETTER_GHA:
                case BanglaUni.LETTER_NGA:
                case BanglaUni.LETTER_CA:
                case BanglaUni.LETTER_CHA:
                case BanglaUni.LETTER_JA:
                case BanglaUni.LETTER_JHA:
                case BanglaUni.LETTER_NYA:
                case BanglaUni.LETTER_TTA:
                case BanglaUni.LETTER_TTHA:
                case BanglaUni.LETTER_DDA:
                case BanglaUni.LETTER_DDHA:
                case BanglaUni.LETTER_NNA:
                case BanglaUni.LETTER_TA:
                case BanglaUni.LETTER_THA:
                case BanglaUni.LETTER_DA:
                case BanglaUni.LETTER_DHA:
                case BanglaUni.LETTER_NA:
                case BanglaUni.LETTER_PA:
                case BanglaUni.LETTER_PHA:
                case BanglaUni.LETTER_BA:
                case BanglaUni.LETTER_BHA:
                case BanglaUni.LETTER_MA:
                case BanglaUni.LETTER_YA:
                case BanglaUni.LETTER_RA:
                case BanglaUni.LETTER_LA:
                case BanglaUni.LETTER_SHA:
                case BanglaUni.LETTER_SSA:
                case BanglaUni.LETTER_SA:
                case BanglaUni.LETTER_HA:
                case BanglaUni.LETTER_KHANDA_TA:
                case BanglaUni.LETTER_RRA:
                case BanglaUni.LETTER_RHA:
                case BanglaUni.LETTER_YYA:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBanglaSorborno(char chUni)
        {
            switch ((BanglaUni)chUni)
            {
                case BanglaUni.LETTER_A:
                case BanglaUni.LETTER_AA:
                case BanglaUni.LETTER_I:
                case BanglaUni.LETTER_II:
                case BanglaUni.LETTER_U:
                case BanglaUni.LETTER_UU:
                case BanglaUni.LETTER_VOCALIC_R:
                case BanglaUni.LETTER_VOCALIC_L:
                case BanglaUni.LETTER_E:
                case BanglaUni.LETTER_AI:
                case BanglaUni.LETTER_O:
                case BanglaUni.LETTER_AU:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBanglaNukta(char chUni)
        {
            switch ((BanglaUni)chUni)
            {
                case BanglaUni.SIGN_CANDRABINDU:
                case BanglaUni.SIGN_ANUSVARA:
                case BanglaUni.SIGN_VISARGA:
                case BanglaUni.SIGN_NUKTA:
                    return true;
                default:
                    return false;
            }
        }

        private const string FOLA_YA = "\u09CD\u09AF";
        private const string FOLA_RA = "\u09CD\u09B0";

        public static bool IsBanglaFola(string strUni)
        {
            switch (strUni)
            {
                case FOLA_YA:
                case FOLA_RA:
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsBanglaHasant(char chUni)
        {
            return chUni == (char)BanglaUni.SIGN_VIRAMA;
        }

        public static bool IsWhiteSpace(char ch)
        {
            switch (ch)
            {
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsFullStop(char chUni)
        {
            return chUni == '\u002E'; // FULL STOP
        }

        public static bool IsHyphen(char ch)
        {
            switch (ch)
            {
                case '\u002D': // HYPHEN-MINUS
                case '\u2010': // HYPHEN
                case '\uFE63': // SMALL HYPHEN-MINUS
                case '\uFF0D': // FULLWIDTH HYPHEN-MINUS
                    return true;
                default:
                    return false;
            }
        }

        public static bool IsDash(char ch)
        {
            switch (ch)
            {
                case '\u2012': // FIGURE DASH
                case '\u2013': // EN DASH
                case '\u2014': // EM DASH
                case '\u2E3A': // TWO EM DASH
                case '\u2E3B': // THREE EM DASH
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Extracts string between Start and End indices, from a string
        /// </summary>
        /// <param name="str">String</param>
        /// <param name="startIndex">From index</param>
        /// <param name="endIndex">To index (up to, but not including)</param>
        /// <returns>A string containing the extracted characters</returns>
        internal static string substring(this string str,
                                         int startIndex,
                                         int endIndex)
        {
            return str.Substring(startIndex, endIndex - startIndex);
        }

    }
}
