using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace Coda {

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

	public class WaveformSerializer {

		public static string filePath = "Assets/Coda/Waveforms";

		public static void WriteWaveformData (Waveform waveform) {
			if (!Directory.Exists(filePath)) {
				Directory.CreateDirectory(filePath);
			}
			
			XmlSerializer serializer = new XmlSerializer(typeof(Waveform));
			FileStream stream = new FileStream(filePath + "/Waveform_" + waveform.fileName + ".xml", FileMode.Create);
			serializer.Serialize(stream, waveform);
			stream.Close();
		}

		public static Waveform ReadWaveformData (string fileName) {
			XmlSerializer serializer = new XmlSerializer(typeof(Waveform));
			FileStream stream = new FileStream(fileName, FileMode.Open);
			Waveform newWave = (Waveform)serializer.Deserialize(stream);
			stream.Close();
			return newWave;
		}

	}

}
