using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.Drawing.Imaging;
namespace DataGridViewDataBindingTest2
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        BabyBook Book = new BabyBook();
        BindingSource bs = new BindingSource();
        bool xml = true;
        const string BinFileName = @"BabyBook.bin";
        const string XmlFileName = @"BabyBook.xml";
        private void frmMain_Load(object sender, EventArgs e)
        {
            Book.Load(XmlFileName, xml);
            bs.DataSource = Book.Records;
            dgData.DataSource = bs;
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Book.Save(XmlFileName, xml);
        }
    }
    [Serializable]
    public class BabyBook
    {
        public IList<Record> Records { get; set; }
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
