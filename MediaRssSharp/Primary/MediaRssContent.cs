﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MediaRss.Primary
{
	[XmlRoot(ElementName = "content", Namespace = "http://search.yahoo.com/mrss/")]
	public class MediaRssContent : MediaRssBase, IMrssPrimaryElement, IXmlSerializable
	{
		private double _fileSize;
		private int _bitRate;
		private int _frameRate;
		private bool? _isDefault;
		private double _samplingRate;
		private int _channels;
		private TimeSpan _duration;
		private int _height;
		private int _width;
		private string _language;

		internal const string ELEMENT_NAME = "content";

		#region Optional elements

		public MrssOptionalElements OptionalElements { get; set; }

		#endregion 

		#region Attributes

		public Uri Url { get; set; }

		public Double FileSize
		{
			get { return _fileSize; }
			set { _fileSize = value; }
		}

		public String Type { get; set; }
		public MediaType Medium { get; set; }

		public bool? IsDefault
		{
			get { return _isDefault; }
			set { _isDefault = value; }
		}

		public String Expression { get; set; }

		public Int32 BitRate
		{
			get { return _bitRate; }
			set { _bitRate = value; }
		}

		public Int32 FrameRate
		{
			get { return _frameRate; }
			set { _frameRate = value; }
		}

		public Double SamplingRate
		{
			get { return _samplingRate; }
			set { _samplingRate = value; }
		}

		public Int32 Channels
		{
			get { return _channels; }
			set { _channels = value; }
		}

		public TimeSpan Duration
		{
			get { return _duration; }
			set { _duration = value; }
		}

		public Int32 Height
		{
			get { return _height; }
			set { _height = value; }
		}

		public Int32 Width
		{
			get { return _width; }
			set { _width = value; }
		}

		public string Language
		{
			get { return _language; }
			set { _language = value; }
		}

		#endregion 

		#region Constructors

		public MediaRssContent()
		{
			OptionalElements = new MrssOptionalElements();
		}

		#endregion

		#region IXmlSerializable Members

		public void ReadXml(XmlReader reader)
		{
			bool isEmpty = reader.IsEmptyElement;

			if (reader.HasAttributes)
			{
				for (int i = 0; i < reader.AttributeCount; i++)
				{
					reader.MoveToNextAttribute();

					if (reader.NamespaceURI == "")
					{
						switch (reader.LocalName)
						{
							case "url":
								Url = new Uri(reader.Value);
								break;
							case "fileSize":
								Double.TryParse(reader.Value, out _fileSize);
								break;
							case "type":
								Type = reader.Value;
								break;
							case "medium":
								Medium = (MediaType)Enum.Parse(typeof(MediaType), reader.Value);
								break;
							case "isDefault":
								bool tmpBool;
								if (bool.TryParse(reader.Value, out tmpBool))
									_isDefault = tmpBool;
								break;
							case "expression":
								Expression = reader.Value;
								break;
							case "bitrate":
								Int32.TryParse(reader.Value, out _bitRate);
								break;
							case "framerate":
								Int32.TryParse(reader.Value, out _frameRate);
								break;
							case "samplingrate":
								Double.TryParse(reader.Value, out _samplingRate);
								break;
							case "channels":
								Int32.TryParse(reader.Value, out _channels);
								break;
							case "duration":
								int duration;
								Int32.TryParse(reader.Value, out duration);
								_duration = TimeSpan.FromSeconds(duration);
								break;
							case "height":
								Int32.TryParse(reader.Value, out _height);
								break;
							case "width":
								Int32.TryParse(reader.Value, out _width);
								break;
							case "lang":
								Language = reader.Value;
								break;
							default:
								AttributeExtensions.Add(new XmlQualifiedName(reader.LocalName, reader.NamespaceURI), reader.Value);
								break;
						}
					}
				}
			}

			reader.ReadStartElement();

			if (!isEmpty)
			{
				while (reader.IsStartElement())
				{
					if (MediaRssHelper.TryParseOptionalElement(OptionalElements, reader) == false)
						ElementExtensions.Add((XElement)XNode.ReadFrom(reader));
				}
				reader.ReadEndElement();
			}
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.AppendFormat("Url: {0}\n", Url);
			builder.AppendFormat("FileSize: {0}\n", FileSize);
			builder.AppendFormat("Type: {0}\n", Type);
			builder.AppendFormat("Medium: {0}\n", Medium.ToString("G"));
			builder.AppendFormat("IsDefault: {0}\n", IsDefault);
			builder.AppendFormat("Expression: {0}\n", Expression);
			builder.AppendFormat("BitRate: {0}\n", BitRate);
			builder.AppendFormat("FrameRate: {0}\n", FrameRate);
			builder.AppendFormat("SamplingRate: {0}\n", SamplingRate);
			builder.AppendFormat("Channels: {0}\n", Channels);
			builder.AppendFormat("Duration: {0}\n", Duration);
			builder.AppendFormat("Height: {0}\n", Height);
			builder.AppendFormat("Width: {0}\n", Width);
			builder.AppendFormat("Language: {0}\n", Language);


			if (ElementExtensions.Count > 0)
			{
				foreach (var elt in ElementExtensions)
				{
					builder.AppendLine(elt.ToString());
				}
			}
			return builder.ToString();
		}


		public void WriteXml(XmlWriter writer)
		{
			if (Url != null) writer.WriteAttributeString("url", null, Url.ToString());
			if (FileSize > 0) writer.WriteAttributeString("filesize", null, FileSize.ToString());
			if (string.IsNullOrEmpty(Type) == false) writer.WriteAttributeString("type", null, Type);
			if (Medium != MediaType.none) writer.WriteAttributeString("medium", null, Medium.ToString("G"));
			if (IsDefault.HasValue) writer.WriteAttributeString("isDefault", null, IsDefault.Value.ToString());
			if (Expression != null) writer.WriteAttributeString("expression", null, Expression);
			if (BitRate > 0) writer.WriteAttributeString("bitrate", null, BitRate.ToString());
			if (FrameRate > 0) writer.WriteAttributeString("framerate", null, FrameRate.ToString());
			if (SamplingRate > 0) writer.WriteAttributeString("samplingrate", null, SamplingRate.ToString());
			if (Channels > 0) writer.WriteAttributeString("channels", null, Channels.ToString());
			if (Duration > TimeSpan.Zero) writer.WriteAttributeString("duration", null, ((int)Duration.TotalSeconds).ToString());
			if (Height > 0) writer.WriteAttributeString("height", null, Height.ToString());
			if (Width > 0) writer.WriteAttributeString("width", null, Width.ToString());
			if (string.IsNullOrEmpty(Language)) writer.WriteAttributeString("lang", null, Language);

			foreach (var kvp in AttributeExtensions)
			{
				writer.WriteAttributeString(kvp.Key.Name, kvp.Key.Namespace, kvp.Value);
			}

			foreach (var element in ElementExtensions)
			{
				element.WriteTo(writer);
			}
		}

		
		#endregion
	}
}