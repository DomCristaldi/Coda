using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Coda {

	/// <summary>
	/// Waveform class for data encapsulation.
	/// </summary>
	[XmlRoot("Waveform")]
	public class Waveform {
		[XmlArray("Data")]
		[XmlArrayItem("value")]
		public double[] data;
		public string fileName;

		public Waveform() {

		}

		public Waveform(double[] d, string name) {
			data = d;
			fileName = name;
		}
	}

	/// <summary>
	/// Waveform xml serializer class.
	/// </summary>
	public class WaveformSerializer {

		public static string filePath = "Assets/Coda/Waveforms";

		/// <summary>
		/// Writes the waveform data to xml file.
		/// </summary>
		/// <param name="waveform">Waveform to write to file.</param>
		public static void WriteWaveformData (Waveform waveform) {
			if (!Directory.Exists(filePath)) {
				Directory.CreateDirectory(filePath);
			}
			
			XmlSerializer serializer = new XmlSerializer(typeof(Waveform));
			FileStream stream = new FileStream(filePath + "/Waveform_" + waveform.fileName + ".xml", FileMode.Create);
			serializer.Serialize(stream, waveform);
			stream.Close();
		}

		/// <summary>
		/// Reads the waveform data from file.
		/// </summary>
		/// <returns>An instance of the waveform class read from file.</returns>
		/// <param name="fileName">Xml file name.</param>
		public static Waveform ReadWaveformData (string fileName) {
			XmlSerializer serializer = new XmlSerializer(typeof(Waveform));
			FileStream stream = new FileStream(fileName, FileMode.Open);
			Waveform newWave = (Waveform)serializer.Deserialize(stream);
			stream.Close();
			return newWave;
		}

	}

}
