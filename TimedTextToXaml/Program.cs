using System;
using System.IO;

namespace TimedTextToXaml {
	class Program {
		static void Main (string[] args) {
			if (args.Length < 2) {
				ShowUsage();
				return;
			}

			string dest = args[1];
			string source = args[0];
			if (!File.Exists(source)) {
				ShowUsage();
				return;
			}

			var input = File.ReadAllText(source);
			var timedStrings = new TimedTextMapper()
				.MapToTimedStrings(input)
				.TimedStrings;

			var output = new XamlOverlayWriter()
				.BuildXamlFor(timedStrings);

			File.WriteAllText(dest, output);
		}

		private static void ShowUsage() {
			Console.WriteLine("Usage:");
			Console.WriteLine("  TimedTextToXaml src dest");
			Console.WriteLine("     src: Source Timed Text XML file");
			Console.WriteLine("     dest: destination path and filename for XAML output");
		}
	}
}
