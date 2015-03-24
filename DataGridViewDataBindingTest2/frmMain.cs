using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        const string FileName = @"BabyBook";
        private void frmMain_Load(object sender, EventArgs e)
        {
            if (xml)
            {
                Book.Load(FileName + ".xml", xml);
            }
            else
            {
                Book.Load(FileName + ".bin", xml);
            }
            bs.DataSource = Book.Records;
            dgData.DataSource = bs;
        }
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (xml)
            {
                Book.Save(FileName + ".xml", xml);
            }
            else
            {
                Book.Save(FileName + ".bin", xml);
            }
        }
    }
}
