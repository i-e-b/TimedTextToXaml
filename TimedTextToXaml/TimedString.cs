using System;

namespace TimedTextToXaml {

	public class TimedString {
		public TimeSpan Start { get; private set; }
		public TimeSpan End { get; private set; }
		public string Text { get; private set; }

		public TimedString (TimeSpan start, TimeSpan end, string text) {
			Start = start;
			End = end;
			Text = text;
		}
	}
}