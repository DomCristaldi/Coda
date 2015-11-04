using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Coda {

	public struct Beat
	{
	    [XmlAttribute("time")]
	    public double timeStamp;
	    [XmlAttribute("freq")]
	    public float frequency;
	    [XmlAttribute("energy")]
	    public double energy;
	}

	[XmlRoot("BeatMap")]
	public class BeatMap
	{
	    [XmlArray("Beats")]
	    [XmlArrayItem("Beat")]
	    public List<Beat> beats;
	    public string fileName;
	    public float songLength;

	    public BeatMap() { }

	    public BeatMap(string name, float length)
	    {
	        beats = new List<Beat>();
	        fileName = name;
	        songLength = length;
	    }

	    public void AddBeat(double timeStamp, float frequency, double energy)
	    {
	        Beat b;
	        b.timeStamp = timeStamp;
	        b.frequency = frequency;
	        b.energy = energy;
	        beats.Add(b);
	    }
	}

	public static class BeatMapWriter {

	    public static void WriteBeatMap(BeatMap map)
	    {
	        // Check if beatmap folder exists
	        if (!Directory.Exists("Assets/Beatmaps"))
	        {
	            Directory.CreateDirectory("Assets/Beatmaps");
	        }

	        XmlSerializer serializer = new XmlSerializer(typeof(BeatMap));
	        FileStream stream = new FileStream("Assets/Beatmaps/BeatMap_" + map.fileName + ".xml", FileMode.Create);
	        serializer.Serialize(stream, map);
	        stream.Close();
	        Debug.Log("Wrote beatmap to file");   
	    }
	}

}
