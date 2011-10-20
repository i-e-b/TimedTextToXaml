using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace TimedTextToXaml {
	public class TimedTextMapper {
		private readonly List<TimedString> timedStrings;

		public IEnumerable<TimedString> TimedStrings {
			get { return timedStrings; }
		}

		public TimedTextMapper () {
			timedStrings = new List<TimedString>();
		}

		public TimedTextMapper MapToTimedStrings (string timedText) {
			var xml = XmlReader.Create(new StringReader(timedText));

			while (xml.Read()) {
				if (xml.Name != "p") continue;
				try {
					TimeSpan start = TimeSpan.Zero, end = TimeSpan.Zero;

					if (xml.HasAttributes) {
						while (xml.MoveToNextAttribute()) {
							if (xml.Name.EndsWith("begin")) start = TimeSpan.Parse(xml.Value);
							if (xml.Name.EndsWith("end")) end = TimeSpan.Parse(xml.Value);
						}
						xml.MoveToElement();

						timedStrings.Add(new TimedString(start, end, FixText(xml.ReadInnerXml())));
					}
				} catch {
					drop();
				}
			}
			return this;
		}

		private string FixText (string src) {
			var clean = src.Replace("\r", "").Replace("\n", "").Replace("<br", "\r\n<");
			var outp = new StringBuilder();
			bool on = true;
			foreach (char c in clean) {
				if (c == '<') on = false;
				if (on) outp.Append(c);
				if (c == '>') on = true;
			}
			return outp.ToString();
		}

		private void drop () { }

	}
}
