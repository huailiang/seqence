using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;


namespace UnityEditor.Timeline
{
    class DirectorNamedColor 
    {
        public readonly static Color colorDuration = new Color(0.66f, 0.66f, 0.66f, 1.0f);
        
        public  readonly static Color colorRecordingClipOutline = new Color(1, 0, 0, 0.9f);
        
        public  readonly static Color colorAnimEditorBinding = new Color(54.0f / 255.0f, 54.0f / 255.0f, 54.0f / 255.0f);
        
        public  readonly static Color colorTimelineBackground = new Color(0.2f, 0.2f, 0.2f, 1.0f);
        
        public  readonly static Color colorLockTextBG = Color.red;
        
        public  readonly static Color colorInlineCurveVerticalLines = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        
        public  readonly static Color colorInlineCurveOutOfRangeOverlay = new Color(0.0f, 0.0f, 0.0f, 0.5f);
        
        public  readonly static Color colorInlineCurvesBackground;
        
        public  readonly static Color markerDrawerBackgroundColor = new Color(0.4f, 0.4f, 0.4f , 1.0f);
        
        public  readonly static Color markerHeaderDrawerBackgroundColor = new Color(0.5f, 0.5f, 0.5f , 1.0f);
        
        public  readonly static Color colorControl = new Color(0.2313f, 0.6353f, 0.5843f, 1.0f);
        
        public  readonly static Color colorSubSequenceBackground = new Color(0.1294118f, 0.1764706f, 0.1764706f, 1.0f);
        
        public  readonly static Color colorTrackSubSequenceBackground = new Color(0.1607843f, 0.2156863f, 0.2156863f, 1.0f);
        
        public  readonly static Color colorTrackSubSequenceBackgroundSelected = new Color(0.0726923f, 0.252f, 0.252f, 1.0f);

        public  readonly static Color colorSubSequenceOverlay = new Color(0.02f, 0.025f, 0.025f, 0.30f);
        
        public  readonly static Color colorSubSequenceDurationLine = new Color(0.0f, 1.0f, 0.88f, 0.46f);

       
        public void ToText(string path)
        {
            StringBuilder builder = new StringBuilder();

            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var f in fields)
            {
                if (f.FieldType != typeof(Color))
                    continue;

                Color c = (Color)f.GetValue(this);
                builder.AppendLine(f.Name + "," + c);
            }

            string filePath = Application.dataPath + "/Editor Default Resources/" + path;
            File.WriteAllText(filePath, builder.ToString());
        }

        public void FromText(string text)
        {
            // parse to a map
            string[] lines = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            var map = new Dictionary<string, Color>();
            foreach (var line in lines)
            {
                var pieces = line.Replace("RGBA(", "").Replace(")", "").Split(',');
                if (pieces.Length == 5)
                {
                    string name = pieces[0].Trim();
                    Color c = Color.black;
                    bool b = ParseFloat(pieces[1], out c.r) &&
                        ParseFloat(pieces[2], out c.g) &&
                        ParseFloat(pieces[3], out c.b) &&
                        ParseFloat(pieces[4], out c.a);

                    if (b)
                    {
                        map[name] = c;
                    }
                }
            }

            var fields = typeof(DirectorNamedColor).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
            foreach (var f in fields)
            {
                if (f.FieldType != typeof(Color))
                    continue;

                Color c = Color.black;
                if (map.TryGetValue(f.Name, out c))
                {
                    f.SetValue(this, c);
                }
            }
        }

        // Case 938534 - Timeline window has white background when running on .NET 4.6 depending on the set system language
        // Make sure we're using an invariant culture so "0.35" is parsed as 0.35 and not 35
        static bool ParseFloat(string str, out float f)
        {
            return float.TryParse(str, NumberStyles.Any, CultureInfo.InvariantCulture, out f);
        }

    }
}
