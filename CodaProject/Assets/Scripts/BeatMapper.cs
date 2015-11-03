using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public struct Beat
{
    [XmlAttribute("time")]
    public double timeStamp;
    [XmlAttribute("freq")]
    public float frequency;
    [XmlAttribute("energy")]
    public float energy;
}

[XmlRoot("BeatMap")]
public class BeatMap
{
    [XmlArray("Beats")]
    [XmlArrayItem("Beat")]
    public List<Beat> beats;
    public string fileName;

    public BeatMap() { }

    public BeatMap(string name)
    {
        beats = new List<Beat>();
        fileName = name;
    }

    public void AddBeat(double timeStamp, float frequency, float energy)
    {
        Beat b;
        b.timeStamp = timeStamp;
        b.frequency = frequency;
        b.energy = energy;
        beats.Add(b);
    }
}

public class BeatMapWriter {

    public BeatMapWriter() { }

    public void WriteBeatMap(BeatMap map)
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
