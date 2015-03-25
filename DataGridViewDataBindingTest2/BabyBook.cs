using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace DataGridViewDataBindingTest2
{
    [Serializable]
    public class BabyBook
    {
        public List<Record> Records { get; set; }
        public BabyBook()
        {
            Records = new List<Record>();
        }
        public void Save(string filename, bool xml = false)
        {
            if (xml)
            {
                // XML Serialization
                Stream TestFileStream = File.Create(filename);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Record>));
                serializer.Serialize(TestFileStream, Records);
                TestFileStream.Close();
            }
            else
            {
                // Binary serialization
                Stream TestFileStream = File.Create(filename);
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(TestFileStream, Records);
                TestFileStream.Close();
            }
        }
        public void Load(string filename, bool xml = false)
        {
            if (xml)
            {
                // XML Serialization
                if (File.Exists(filename))
                {
                    Stream TestFileStream = File.OpenRead(filename);
                    XmlSerializer deserializer = new XmlSerializer(typeof(List<Record>));
                    Records = (List<Record>)deserializer.Deserialize(TestFileStream);
                    TestFileStream.Close();
                }
            }
            else
            {
                // Binary serialization
                if (File.Exists(filename))
                {
                    Stream TestFileStream = File.OpenRead(filename);
                    BinaryFormatter deserializer = new BinaryFormatter();
                    Records = (List<Record>)deserializer.Deserialize(TestFileStream);
                    TestFileStream.Close();
                }
            }
        }
    }
    [Serializable]
    public class Record
    {
        public Record()
        {
            Name = string.Empty;
            Something = string.Empty;
            Image = null;
        }
        public string Name { get; set; }
        public string Something { get; set; }
        [XmlIgnore]
        public Bitmap Image { get; set; }
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [XmlElement("Image")]
        public byte[] ImageSerialized
        {
            get
            { // serialize
                if (Image == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    Image.Save(ms, ImageFormat.Bmp);
                    return ms.ToArray();
                }
            }
            set
            { // deserialize
                if (value == null)
                {
                    Image = null;
                }
                else
                {
                    using (MemoryStream ms = new MemoryStream(value))
                    {
                        Image = new Bitmap(ms);
                    }
                }
            }
        }
    }
}
