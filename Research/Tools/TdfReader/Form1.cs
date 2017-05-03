using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace TdfReader
{
    public partial class Form1 : Form
    {
        private TDFFile _openFile;

        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "TDF File (*.tdf)|*.tdf"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                listView1.Items.Clear();

                _openFile = new TDFFile();
                _openFile.Read(ofd.FileName);

                listView1.BeginUpdate();
                for (int col = 0; col < _openFile.header.Col; col++)
                {
                    listView1.Columns.Add(TDFFile.GetColumnName(col, ofd.SafeFileName));
                }

                // TODO: Async loading :)?
                using (BinaryReaderExt reader = new BinaryReaderExt(new MemoryStream(_openFile.resTable)))
                {
                    for (int row = 0; row < _openFile.header.Row; row++)
                    {
                        ListViewItem lvi = new ListViewItem();
                        for (int col = 0; col < _openFile.header.Col; col++)
                        {
                            if(col != 0)
                                lvi.SubItems.Add(reader.ReadUnicode());
                            else
                            {
                                lvi.Text = reader.ReadUnicode();
                            }
                        }
                        listView1.Items.Add(lvi);
                    }
                }
                listView1.EndUpdate();
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO: More export options :)
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Comma seperated list (*.csv)|*.csv"
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                using (TextWriter writer = File.CreateText(sfd.FileName))
                {
                    StringBuilder sb = new StringBuilder();

                    //Making columns!
                    foreach (ColumnHeader ch in listView1.Columns)
                    {
                        sb.Append(ch.Text + ",");
                    }

                    sb.AppendLine();


                    //Looping through items and subitems
                    foreach (ListViewItem lvi in listView1.Items)
                    {
                        foreach (ListViewItem.ListViewSubItem lvs in lvi.SubItems)
                        {
                            if (lvs.Text.Trim() == string.Empty)
                                sb.Append(" ,");
                            else
                                sb.Append(lvs.Text + ",");
                        }
                        sb.AppendLine();
                    }
                    writer.Write(sb.ToString());
                }
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listView1.BeginUpdate();
            listView1.Clear();
            listView1.EndUpdate();
            _openFile = null;
        }
    }
}
