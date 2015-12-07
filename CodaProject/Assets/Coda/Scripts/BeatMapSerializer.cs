using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Coda {

	/// <summary>
	/// Contains the filePath for the Beatmaps directory, as well as helper classes for reading and writing beatmaps to file.
	/// </summary>
	public class BeatMapSerializer {

		public static string filePath = "Assets/Coda/Beatmaps";

		public static class BeatMapWriter {
			
			public static void WriteBeatMap(BeatMap map) {
				// Check if beatmap folder exists
				if (!Directory.Exists(BeatMapSerializer.filePath)) {
					Directory.CreateDirectory(BeatMapSerializer.filePath);
				}
				
				XmlSerializer serializer = new XmlSerializer(typeof(BeatMap));
				StreamWriter writer = new StreamWriter(BeatMapSerializer.filePath + "/BeatMap_" + map.fileName + ".xml", false, System.Text.Encoding.UTF8);
				serializer.Serialize(writer, map);
				writer.Close();
				Debug.Log("Wrote beatmap to file");   
			}
		}
		
		public static class BeatMapReader {
			
			public static BeatMap ReadBeatMap(TextAsset xmlFile) {
				XmlSerializer serializer = new XmlSerializer(typeof(BeatMap));
                StringReader xml = new StringReader(xmlFile.text);
                BeatMap newMap = serializer.Deserialize(xml) as BeatMap;
				return newMap;
			}
			
			public static BeatMap ReadBeatMap(string filePath) {
				XmlSerializer serializer = new XmlSerializer(typeof(BeatMap));
				FileStream stream = new FileStream(filePath, FileMode.Open);
				BeatMap newMap = (BeatMap)serializer.Deserialize(stream);
				stream.Close();
				return newMap;
			}
			
		}

	}

	/// <summary>
	/// Class to contain beat information.
	/// </summary>
	[System.Serializable]
	public struct Beat {
	    [XmlAttribute("time")]
	    public double timeStamp;
	    [XmlAttribute("freq")]
	    public float frequency;
	    [XmlAttribute("energy")]
	    public double energy;

        public static bool operator == (Beat beat1, Beat beat2) {
            return (beat1.timeStamp == beat2.timeStamp && beat1.frequency == beat2.frequency && beat1.energy == beat2.energy);
        }

        public static bool operator != (Beat beat1, Beat beat2) {
            return !(beat1 == beat2);
        }

	}

	/// <summary>
	/// Class to contain the beatmap information obtained by the FFT.
	/// </summary>
	[System.Serializable]
	[XmlRoot("BeatMap")]
	public class BeatMap {
	    [XmlArray("Beats")]
	    [XmlArrayItem("Beat")]
	    public List<Beat> beats;
	    public string fileName;
	    public float songLength;

	    public BeatMap() { }

	    public BeatMap(string name, float length) {
	        beats = new List<Beat>();
			fileName = name;
	        songLength = length;
	    }

	    public void AddBeat(double timeStamp, float frequency, double energy) {
	        Beat b;
	        b.timeStamp = timeStamp;
	        b.frequency = frequency;
	        b.energy = energy;
	        beats.Add(b);
	    }
	}

}
