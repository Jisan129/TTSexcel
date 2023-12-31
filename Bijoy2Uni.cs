using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TTSexcel.Common;
namespace TTSexcel
{
    internal class Bijoy2Uni
    {


        private static readonly Dictionary<string, string> conversion_map = new Dictionary<string, string>()
        {
            {"i¨", "র‌্য"},
            {"ª¨", "্র্য"},
            {"«¨", "্র্য"},
            {"Ö¨", "্র্য"},
            {"°", "ক্ক"},
            {"±", "ক্ট"},
            {"²", "ক্ষ্ণ"},
            {"³", "ক্ত"},
            {"´", "ক্ন"},
            {"µ", "ক্র"},
            {"¶", "ক্ষ"},
            {"ÿ", "ক্ষ"},
            {"·", "ক্স"},
            {"¸", "গু"},
            {"¹", "গ্গ"},
            {"º", "গ্দ"},
            {"»", "গ্ধ"},
            {"¼", "ঙ্ক"},
            {"•¶", "ঙ্ক্ষ"},
            {"•ÿ", "ঙ্ক্ষ"},
            {"•L", "ঙ্খ"},
            {"½", "ঙ্গ"},
            {"•N", "ঙ্ঘ"},
            {"”P", "চ্চ"},
            {"”Q", "চ্ছ"},
            {"”T", "চ্ঞ"},
            {"¾", "জ্জ"},
            {"À", "জ্ঝ"},
            {"Á", "জ্ঞ"},
            {"Â", "ঞ্চ"},
            {"Ã", "ঞ্ছ"},
            {"Ä", "ঞ্জ"},
            {"Å", "ঞ্ঝ"},
            {"Æ", "ট্ট"},
            {"Ç", "ড্ড"},
            {"È", "ণ্ট"},
            {"É", "ণ্ঠ"},
            {"Ý", "ন্স"},
            {"Ð", "ণ্ড"},
            {"š‘", "ন্তু"},
            {"Ë", "ত্ত"},
            {"Ì", "ত্থ"},
            {"Î", "ত্র"},
            {"Ï", "দ্দ"},
            {"×", "দ্ধ"},
            {"Ø", "দ্ব"},
            {"Ù", "দ্ম"},
            {"Ú", "ন্ঠ"},
            {"Û", "ন্ড"},
            {"Ü", "ন্ধ"},
            {"Þ", "প্ট"},
            {"ß", "প্ত"},
            {"à", "প্প"},
            {"á", "প্স"},
            {"â", "ব্জ"},
            {"ã", "ব্দ"},
            {"ä", "ব্ধ"},
            {"å", "ভ্র"},
            {"ç", "ম্ফ"},
            {"é", "ল্ক"},
            {"ê", "ল্গ"},
            {"ë", "ল্ট"},
            {"ì", "ল্ড"},
            {"í", "ল্প"},
            {"î", "ল্ফ"},
            {"ï", "শু"},
            {"ð", "শ্চ"},
            {"ñ", "শ্ছ"},
            {"ó", "ষ্ট"},
            {"ô", "ষ্ঠ"},
            {"ò", "ষ্ণ"},
            {"õ", "ষ্ফ"},
            {"÷", "স্ট"},
            {"ö", "স্খ"},
            {"¯‘", "স্তু"},
            {"ù", "স্ফ"},
            {"û", "হু"},
            {"ý", "হ্ন"},
            {"þ", "হ্ম"},
            {"ü", "হৃ"},
            {"©", "র্"},
            {"¬", "্ল"},
            {"ø", "্ল"},
            {"ú", "্প"},
            {"Ÿ", "্ব"},
            {"¡", "্ব"},
            {"¦", "্ব"},
            {"^", "্ব"},
            {"¢", "্ভ"},
            {"£", "্ভ্র"},
            {"§", "্ম"},
            {"¥", "্ম"},
            {"¤", "ম্"},
            {"è", "্ণ"},
            {"œ", "্ন"},
            {"›", "ন্"},
            {"−", "ণ্ঢ"},
            {"’", "্থ"},
            {"¿", "্ত্র"},
            {"Í", "্ত"},
            {"˜", "দ্"},
            {"™", "দ্"},
            {"‹", "্ক"},
            {"Œ", "্ক্র"},
            {"”", "চ্"},
            {"š", "ন্"},
            {"®", "ষ্"},
            {"¯", "স্"},
            
            #region Vowel
            {"Av", "আ"},
            {"A", "অ"},
            {"B", "ই"},
            {"C", "ঈ"},
            {"D", "উ"},
            {"E", "ঊ"},
            {"F", "ঋ"},
            {"G", "এ"},
            {"H", "ঐ"},
            {"I", "ও"},
            {"J", "ঔ"}, 
            #endregion
            
            #region Consonant
            {"K", "ক"},
            {"L", "খ"},
            {"M", "গ"},
            {"N", "ঘ"},
            {"O", "ঙ"},
            {"P", "চ"},
            {"Q", "ছ"},
            {"R", "জ"},
            {"S", "ঝ"},
            {"T", "ঞ"},
            {"U", "ট"},
            {"V", "ঠ"},
            {"W", "ড"},
            {"X", "ঢ"},
            {"Y", "ণ"},
            {"Z", "ত"},
            {"_", "থ"},
            {"`", "দ"},
            {"a", "ধ"},
            {"b", "ন"},
            {"c", "প"},
            {"d", "ফ"},
            {"e", "ব"},
            {"f", "ভ"},
            {"g", "ম"},
            {"h", "য"},
            {"i", "র"},
            {"j", "ল"},
            {"k", "শ"},
            {"l", "ষ"},
            {"m", "স"},
            {"n", "হ"},
            {"o", "ড়"},
            {"p", "ঢ়"},
            {"q", "য়"},
            {"r", "ৎ"}, 
            #endregion
            
            #region Digit
            {"0", "০"},
            {"1", "১"},
            {"2", "২"},
            {"3", "৩"},
            {"4", "৪"},
            {"5", "৫"},
            {"6", "৬"},
            {"7", "৭"},
            {"8", "৮"},
            {"9", "৯"},
            #endregion

            #region VowelSign
            // U+09BE : BENGALI VOWEL SIGN AA
            {"v", "া"},
            // U+09BF : BENGALI VOWEL SIGN I
            {"w", "ি"},
            // U+09C0 : BENGALI VOWEL SIGN II
            {"x", "ী"},
            // U+09C1 : BENGALI VOWEL SIGN U
            {"y", "ু"},
            {"z", "ু"},
            {"æ", "ু"},  // side-wise
            // U+09C2 : BENGALI VOWEL SIGN UU
            {"~", "ূ"},
            {"ƒ", "ূ"},  // side-wise
            {"‚", "ূ"},  
            // U+09C3 : BENGALI VOWEL SIGN VOCALIC R
            {"„", "ৃ"},
            {"…", "ৃ"},
            // U+09C7 : BENGALI VOWEL SIGN E
            {"†", "ে"},  // no matra
            {"‡", "ে"},  
            // U+09C8 : BENGALI VOWEL SIGN AI
            {"‰", "ৈ"},
            {"ˆ", "ৈ"},
            // U+09D7 : BENGALI AU LENGTH MARK
            {"Š", "ৗ"}, 
            #endregion

            #region Punctuation
            {"Ñ", "–"},
            {"Ò", "“"},
            {"Ó", "”"},
            {"Ô", "‘"},
            {"Õ", "’"},
            {"|", "।"},
            #endregion
            
            #region Parasitic
            {"s", "ং"},
            {"t", "ঃ"},
            {"u", "ঁ"}, 
            #endregion

            #region Fola
            {"ª", "্র"},
            {"«", "্র"},
            {"Ö", "্র"},
            {"¨", "্য"}, 
            #endregion

            #region Hasant
            {"&", "্"},
            #endregion
            
            {"$", "৳"}
        };

        private static string Rearrange(string str)
        {
            for (var i = 0; i < str.Length; i++)
            {
                if (i > 0
                    && i < str.Length - 1
                    && IsBanglaHasant(str[i]))
                {
                    if (IsBanglaKar(str[i - 1]) || IsBanglaNukta(str[i - 1]))
                    {
                        var temp = str.substring(0, i - 1);
                        temp += str[i];
                        temp += str[i + 1];
                        temp += str[i - 1];
                        temp += str.substring(i + 2, str.Length);
                        str = temp;
                    }

                    if (str[i - 1] == (char)BanglaUni.LETTER_RA
                        && !IsBanglaHasant(str[i - 2])
                        && IsBanglaKar(str[i + 1]))
                    {
                        var temp = str.substring(0, i - 1);
                        temp += str[i + 1];
                        temp += str[i - 1];
                        temp += str[i];
                        temp += str.substring(i + 2, str.Length);
                        str = temp;
                    }
                }

                if (i > 0
                    && i < str.Length - 1
                    && IsBanglaHasant(str[i + 1])
                    && str[i] == (char)BanglaUni.LETTER_RA
                    && !IsBanglaHasant(str[i - 1]))
                {
                    var j = 1;
                    while (i >= j)
                    {
                        if (IsBanglaBanjonborno(str[i - j])
                            && IsBanglaHasant(str[i - j - 1]))
                        {
                            j += 2;
                        }
                        else if (j == 1
                            && IsBanglaKar(str[i - j]))
                        {
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    var temp = str.substring(0, i - j);
                    temp += str[i];
                    temp += str[i + 1];
                    temp += str.substring(i - j, i);
                    temp += str.substring(i + 2, str.Length);
                    str = temp;
                    i += 1;
                    continue;
                }

                if (i < str.Length - 1
                    && IsBanglaPreKar(str[i])
                    && !IsWhiteSpace(str[i + 1]))
                {
                    var temp = str.substring(0, i);
                    var j = 1;
                    while (i + j < str.Length - 2
                        && IsBanglaBanjonborno(str[i + j])
                        && IsBanglaHasant(str[i + j + 1]))
                    {
                        j += 2;
                    }
                    temp += str.substring(i + 1, i + j + 1);
                    var l = 0;

                    if (i + j < str.Length - 1
                        && str[i] == (char)BanglaUni.VOWEL_SIGN_E
                        && str[i + j + 1] == (char)BanglaUni.VOWEL_SIGN_AA)
                    {
                        temp += (char)BanglaUni.VOWEL_SIGN_O;
                        l = 1;
                    }
                    else if (i + j < str.Length - 1
                        && str[i] == (char)BanglaUni.VOWEL_SIGN_E
                        && str[i + j + 1] == (char)BanglaUni.LENGTH_MARK_AU)
                    {
                        temp += (char)BanglaUni.VOWEL_SIGN_AU;
                        l = 1;
                    }
                    else
                    {
                        temp += str[i];
                    }

                    temp += str.substring(i + j + l + 1, str.Length);
                    str = temp;
                    i += j;
                }

                if (i < str.Length - 1
                    && str[i] == (char)BanglaUni.SIGN_CANDRABINDU
                    && IsBanglaPostKar(str[i + 1]))
                {
                    var temp = str.substring(0, i);
                    temp += str[i + 1];
                    temp += str[i];
                    temp += str.substring(i + 2, str.Length);
                    str = temp;
                }
            }
            return str;
        }

        public static string Convert(string str, Conversion conversion = Conversion.Token)
        {
            if (conversion == Conversion.NonToken)
            {
                return str
                    .Replace('Ñ', '–')
                    .Replace('Ò', '“')
                    .Replace('Ó', '”')
                    .Replace('Ô', '‘')
                    .Replace('Õ', '’')
                    .Replace('|', '।')
                    .Replace('$', '৳');
            }
            else
            {
                var result = str;
                if (conversion == Conversion.Token)
                {
                    result = new Mapper(str).Output
                        .Replace("\u09CD\u09CD", "\u09CD");

                    //result = Regex.Replace(result, "[\u09CD]{2,}", "\u09CD");
                }
                else
                {
                    foreach (var key in conversion_map.Keys.OrderByDescending(x => x))
                    {
                        result = result.Replace(key, conversion_map[key]);
                    }
                }

                //Debug.WriteLine($"{str} ->b4-> {result}");

                try
                {
                    result = Rearrange(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                }

                return result.Replace("অা", "আ");
            }
        }

        public enum Conversion { All, Token, NonToken }

        class Mapper
        {
            private readonly int call;
            private readonly int[,] matrix;
            public string Output = string.Empty;

            public Mapper(string str)
            {
                var length = str.Length + 1;
                matrix = new int[length, length];
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        matrix[i, j] = -1; // initialize with -1
                    }
                }

                if ((call = Call(str)) < int.MaxValue)
                {
                    Path(str); // updates Output
                }
                else
                {
                    Output = str; // failed
                    Debug.Write($"Failed: {str}\n{GetMatrix()}");
                }
            }

            public string GetMatrix()
            {
                var stringBuilder = new StringBuilder();
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        stringBuilder.Append("\t" + matrix[i, j]);
                    }
                    stringBuilder.Append(Environment.NewLine);
                }
                return stringBuilder.ToString();
            }

            private int Call(string str, int index = 0, int total = 0)
            {
                if (index == str.Length)
                {
                    return matrix[index, total] = total;
                }

                if (matrix[index, total] != -1)
                {
                    return matrix[index, total];
                }

                var result = int.MaxValue;
                for (var i = index + 1; i <= str.Length; i++)
                {
                    var substring = str.substring(index, i);

                    if (conversion_map.ContainsKey(substring) || IsAllowed(substring))
                    {
                        result = Math.Min(result, Call(str, index + substring.Length, total + 1));
                    }
                }
                return matrix[index, total] = result;
            }

            private void Path(string str, int index = 0, int total = 0)
            {
                if (index == str.Length)
                {
                    return;
                }

                for (var i = index + 1; i <= str.Length; i++)
                {
                    var substring = str.substring(index, i);

                    if ((conversion_map.ContainsKey(substring) || IsAllowed(substring))
                        && call == matrix[index + substring.Length, total + 1])
                    {
                        conversion_map.TryGetValue(substring, out string replacement);
                        Output += replacement ?? substring[0].ToString();
                        Path(str, index + substring.Length, total + 1);
                    }
                }
            }

            private bool IsAllowed(string str)
            {
                return str.Length == 1
                    && (IsFullStop(str[0]) || IsHyphen(str[0]) || IsDash(str[0]));
            }
        }


    }
}
