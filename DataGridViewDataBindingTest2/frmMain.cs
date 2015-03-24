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
        List<TestClass> TestClasses = new List<TestClass>();
        BindingSource bs = new BindingSource();
        const string BinFileName = @"C:\Users\mbond\Downloads\TestClasses.bin";
        const string XmlFileName = @"C:\Users\mbond\Downloads\TestClasses.xml";
        private void frmMain_Load(object sender, EventArgs e)
        {
            //// Binary serialization
            //if (File.Exists(BinFileName))
            //{
            //    Stream TestFileStream = File.OpenRead(BinFileName);
            //    BinaryFormatter deserializer = new BinaryFormatter();
            //    TestClasses = (List<TestClass>)deserializer.Deserialize(TestFileStream);
            //    TestFileStream.Close();
            //}

            // XML Serialization
            if (File.Exists(XmlFileName))
            {
                Stream TestFileStream = File.OpenRead(XmlFileName);
                XmlSerializer deserializer = new XmlSerializer(typeof(List<TestClass>));
                TestClasses = (List<TestClass>)deserializer.Deserialize(TestFileStream);
                TestFileStream.Close();
            }

            bs.DataSource = TestClasses;
            dgData.DataSource = bs;

            if (TestClasses.Count > 0)
            {
                TestClasses[0].Image = new Bitmap(@"C:\Users\mbond\Downloads\Image.png");
            }

        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //// Binary serialization
            //Stream TestFileStream = File.Create(BinFileName);
            //BinaryFormatter serializer = new BinaryFormatter();
            //serializer.Serialize(TestFileStream, TestClasses);
            //TestFileStream.Close();

            // XML Serialization
            Stream TestFileStream = File.Create(XmlFileName);
            XmlSerializer serializer = new XmlSerializer(typeof(List<TestClass>));
            serializer.Serialize(TestFileStream, TestClasses);
            TestFileStream.Close();
        }
    }
    [Serializable]
    public class TestClass
    {
        public TestClass()
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
