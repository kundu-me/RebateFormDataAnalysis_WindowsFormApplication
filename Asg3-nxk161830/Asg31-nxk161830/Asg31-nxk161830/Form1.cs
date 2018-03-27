using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Asg31_nxk161830
{
    public partial class Form1 : Form
    {
        Dictionary<String, String> dictEvalGlobal = new Dictionary<String, String>();
        public Form1()
        {
            InitializeComponent();
        }

        public void resetEvalLabels()
        { 
            labelNRecords.Text = "...";
            labelMinEntryTime.Text = "...";
            labelMaxEntryTime.Text = "...";
            labelAvgEntryTime.Text = "...";
            labelMinInterRecTime.Text = "...";
            labelMaxInterRecTime.Text = "...";
            labelAvgInterRecTime.Text = "...";
            labelTotalTime.Text = "...";
            labelBackspaceCount.Text = "...";
            labelErrorText.Text = "";

            btnExport.Enabled = false;
            dictEvalGlobal = null;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            //resetEvalLabels();
            labelErrorText.Text = "";

            // Open the Browse File Dialog Box
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                String fileName = openFileDialog1.FileName;
                txtBxFile.Text = fileName;
            }
        }

        private void btnEvaluate_Click(object sender, EventArgs e)
        {
            resetEvalLabels();

            if(txtBxFile.Text == "")
            {
                labelErrorText.Text = "File name field is empty";
                return;
            }

            EvaluationService service = new EvaluationService();
            String fileName = txtBxFile.Text;

            Boolean isFileExists = service.isFileExists(fileName);
            if(!isFileExists)
            {
                labelErrorText.Text = "Error: File not found";
                return;
            }

            List<Customer> lstCustomer = service.readCustomerDataFromFile(fileName);

            if(lstCustomer == null || lstCustomer.Count == 0)
            {
                labelErrorText.Text = "Error: Incorrect file selected ";
                return;
            }

            Dictionary<String, String> dictEval = service.evaluateCustomerDataFile(lstCustomer);

            dictEvalGlobal = dictEval;
            btnExport.Enabled = true;

            labelNRecords.Text = dictEval["numRecords"];

            labelMinEntryTime.Text = dictEval["minEntryTime"];
            labelMaxEntryTime.Text = dictEval["maxEntryTime"];
            labelAvgEntryTime.Text = dictEval["avgEntryTime"];

            labelMinInterRecTime.Text = dictEval["minInterRecTime"];
            labelMaxInterRecTime.Text = dictEval["maxInterRecTime"];
            labelAvgInterRecTime.Text = dictEval["avgInterRecTime"];

            labelTotalTime.Text = dictEval["totalTime"];
            labelBackspaceCount.Text = dictEval["countBackspace"];

            service.writeCustomerDataToFile(dictEval, "test-eval.txt");
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            resetEvalLabels();
            txtBxFile.Text = "";
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save text Files";
            saveFileDialog1.CheckPathExists = true;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String exportFileName = saveFileDialog1.FileName;

                if(exportFileName == "")
                {
                    return;
                }

                if(dictEvalGlobal == null || dictEvalGlobal.Count == 0)
                {
                    return;
                }

                EvaluationService service = new EvaluationService();
                service.writeCustomerDataToFile(dictEvalGlobal, exportFileName);
                
            }
        }
    }
}
