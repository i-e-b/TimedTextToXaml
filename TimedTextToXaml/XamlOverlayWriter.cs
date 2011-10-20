using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimedTextToXaml {
	public class XamlOverlayWriter {
		private readonly StringBuilder sb;

		public XamlOverlayWriter() {
			sb = new StringBuilder();
		}

		public string BuildXamlFor(IEnumerable<TimedString> timedStrings) {
			sb.Clear();
			var strings = timedStrings.ToArray();

			WriteHeaders();
			WriteTimeline(strings);
			WriteBinding();
			WriteBlocks(strings);
			WriteFooters();

			return sb.ToString();
		}


		private void WriteBlocks(TimedString[] strings) {
			Action<string> a = s => sb.Append(s);
			for (int i = 0; i < strings.Length; i++) {
				a("<TextBlock x:Name=\"a");
				a(i.ToString());
				a("\" TextWrapping=\"Wrap\" FontSize=\"32\" Foreground=\"White\" VerticalAlignment=\"Bottom\" HorizontalAlignment=\"Center\" Visibility=\"Collapsed\" Text=\"");
				a(strings[i].Text.Replace("\"", "&quot;"));
				a("\"/>");
				sb.AppendLine();
			}
		}

		private void WriteTimeline (TimedString[] strings) {
			Action<string> a = s => sb.Append(s);
			for (int i = 0; i < strings.Length; i++) {
				a("<ObjectAnimationUsingKeyFrames Storyboard.TargetProperty=\"(UIElement.Visibility)\" Storyboard.TargetName=\"a");
				a(i.ToString());
				a("\">");
				sb.AppendLine();
				a("<DiscreteObjectKeyFrame KeyTime=\"");
				a(strings[i].Start.ToString());
				a("\" Value=\"{x:Static Visibility.Visible}\"/>");
				sb.AppendLine();
				a("<DiscreteObjectKeyFrame KeyTime=\"");
				a(strings[i].End.ToString());
				a("\" Value=\"{x:Static Visibility.Collapsed}\"/>");
				sb.AppendLine();
				a("</ObjectAnimationUsingKeyFrames>");
				sb.AppendLine();
			}
		}

		#region static bits
		private void WriteHeaders() {
			Action<string> l = s => sb.AppendLine(s);
			l("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			l("<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" x:Name=\"BurnInSubtitles\" Width=\"480\" Height=\"270\" Clip=\"F1 M 0,0L 480,0L 480,270L 0,270L 0,0\">");
			l("<Canvas.Resources>");
			l("<Storyboard x:Key=\"Subtitles\">");
		}
		private void WriteBinding() {
			Action<string> l = s => sb.AppendLine(s);
			l("</Storyboard>");
			l("</Canvas.Resources>");
			l("<Canvas.Triggers>");
			l("<EventTrigger RoutedEvent=\"FrameworkElement.Loaded\">");
			l("<BeginStoryboard Storyboard=\"{StaticResource Subtitles}\"/>");
			l("</EventTrigger>");
			l("</Canvas.Triggers>");
			l("<Grid x:Name=\"Layer_1\" Height=\"270\" Canvas.Left=\"0\" Canvas.Top=\"0\" Width=\"480\">");
		}
		private void WriteFooters () {
			Action<string> l = s => sb.AppendLine(s);
			l("</Grid>");
			l("</Canvas>");
		}
		#endregion
	}
}