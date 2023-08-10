using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SC_M4
{
    public partial class Main
    {

        private CancellationTokenSource cts_manual = new CancellationTokenSource();
        private Task task_manual;

        private void InitializeManualTest()
        {

        }

        private void StartManualTest()
        {
            if (task_manual != null && task_manual.Status == TaskStatus.Running)
            {
                return;
            }

            // Reset the cancellation token.
            cts_manual?.Dispose();
            cts_manual = new CancellationTokenSource();
        }

        private string CleanAndReplaceText1(string input)
        {
            int index731 = input.IndexOf("-731");
            input = input.Substring(index731 + 1);
            index731 = input.IndexOf("|731");
            input = input.Substring(index731 + 1);

            var replacements = new Dictionary<string, string>()
            {
                { "T31TM", "731TM" },
                { "731THC", "731TMC" },
                { "7731TMC", "731TMC" },
                { "731TMCO", "731TMC6" },
                { "-I", "-1" },
                { "-S-", "-5-" },
                { "G.22", "G:22" },
                { " ", "" },
                { "\r", "" },
                { "\t", "" },
                { "\n", "" },
                { "\\", "" },
                { "|", "" },
                { @"\", "" },
            };

            foreach (var pair in replacements)
            {
                input = input.Replace(pair.Key, pair.Value);
            }

            return input.Trim();
        }
        private string CleanAndReplaceText2(string input)
        {
            // Initial cleanup: Remove white spaces, newlines, carriage returns and tabs
            input = input.Trim().Replace(" ", "").Replace("\r", "").Replace("\t", "").Replace("\n", "");
            // Retain only alphanumerics and some symbols
            input = Regex.Replace(input, "[^a-zA-Z0-9(),:,-]", "");
            // Special replacements
            input = input.Replace("91J7", "9U7");
            input = input.Replace("-OO", "-00");
            input = input.Replace(")9U7", "9U7");
            // Define dictionary for pattern replacements
            var replacements = new Dictionary<string, string>()
            {
                {"731OTM", "7310TM"},
                {"3O1731", "3-01731"},
                {"4O1731", "4-01731"},
                {"5O1731", "5-01731"},
                {"6O3731", "6-03731"},
                {"2O1731", "2-01731"},
                {"7O1731", "7-01731"},
                {"5O2731", "5-02731"},
                {"4O4731", "4-04731"},
                {"8OO731", "8-00731"},
                {"8OA731", "8-0A731"},
                {"9OA731", "9-0A731"},
                {"6O1731", "6-01731"},
                {"7OA731", "7-0A731"},
                {"7OO731", "7-00731"},
                {"2OA731", "2-0A731"},
            };
            // Perform pattern replacements
            foreach (var pair in replacements)
            {
                input = input.Replace(pair.Key, pair.Value);
                input = input.Replace(pair.Key.Insert(1, "-"), pair.Value); // insert "-" after first character
            }
            return input;
        }

    }
}
