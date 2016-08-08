using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sesion2_Lab01.com.isil.utils {
    public class NCommon {

        public static char Char_Parse_Win8(string inParameter) {
            bool wasSuccesfull = false;
            return Char_Parse_Win8(inParameter, out wasSuccesfull);
        }

        public static char Char_Parse_Win8(string inParameter, out bool wasSuccesfull) {
            char output = char.MinValue;
            wasSuccesfull = char.TryParse(inParameter, out output);
            return output;
        }

        public static string eraseCharsInString(string baseString, int[] asciiChars) {
            string newStrings = "";
            int charsLength = asciiChars.Length;
            int stringLength = baseString.Length;

            for (int i = 0; i < stringLength; i++) {
                int unicodeChar = char.ConvertToUtf32(baseString, i);
                bool comparisionIsTrue = false;

                for (int j = 0; j < charsLength; j++) {
                    if (asciiChars[j] == unicodeChar) {
                        comparisionIsTrue = true;
                        break;
                    }
                }

                if (!comparisionIsTrue) {
                    newStrings += baseString[i];
                }
            }

            return newStrings;
        }
    }
}
