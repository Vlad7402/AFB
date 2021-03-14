using System.Collections.Generic;

namespace AFB.logic
{
    static class Table
    {
        public static string[] GetTable(double[] velumesOfX, double[] velumesOfY)
        {
            List<string> tableStrings = new List<string>();
            int longestX = GetLongestNum(velumesOfX);
            int longestY = GetLongestNum(velumesOfY);
            tableStrings.Add(HeadPrint(longestX, "┌", "┐") + HeadPrint(longestY, "┌", "┐"));
            tableStrings.Add(StringWithNum(longestX, "X") + StringWithNum(longestY, "Y"));
            tableStrings.Add(HeadPrint(longestX, "├", "┤") + HeadPrint(longestY, "├", "┤"));
            for (int i = 0; i < velumesOfX.Length; i++)
            {
                tableStrings.Add(StringWithNum(longestX, velumesOfX[i].ToString()) + StringWithNum(longestY, velumesOfY[i].ToString()));
                tableStrings.Add(HeadPrint(longestX, "├", "┤") + HeadPrint(longestY, "├", "┤"));
            }
            tableStrings.Add(HeadPrint(longestX, "└", "┘") + HeadPrint(longestY, "└", "┘"));
            return tableStrings.ToArray();
        }
        private static string HeadPrint(int longestNumLenght, string LineStart, string LineEnd)
        {
            string result = string.Empty;
            result += LineStart;
            for (int i = 0; i < longestNumLenght; i++) result += "─";
            result += LineEnd;
            return result;
        }
        private static string FillTableEmptyness(int emptynessLenght)
        {
            string result = string.Empty;
            for (int i = 0; i < emptynessLenght; i++) result += ' ';
            return result;
        }
        private static string StringWithNum(int longestNumLenght, string num)
        {
            string result = string.Empty;
            result += "│";
            result += num;
            result += FillTableEmptyness(longestNumLenght - num.Length);
            result += "│";
            return result;
        }
        private static int GetLongestNum(double[] input)
        {
            int result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                string inputVelInString = input[i].ToString();
                if (inputVelInString.Length > result) result = inputVelInString.Length;
            }
            return result;
        }
    }
}
