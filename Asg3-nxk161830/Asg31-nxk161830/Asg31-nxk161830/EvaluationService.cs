using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asg31_nxk161830
{
    class EvaluationService
    {
        public EvaluationService()
        {

        }

        /**
         * This method checks if the
         * File to be evaluated either exists
         **/
        public Boolean isFileExists(String fileName)
        {
            if (!System.IO.File.Exists(@fileName))
            {
                Console.WriteLine("FileNot Found!!!");
                return false;
            }

            return true;
        }

        /**
       * Everytime the Evaluation is run
       * this method reads the data that is saved in the file into the dictionary
       * so as to access the Customer data - Evaluate it
       **/
        public List<Customer> readCustomerDataFromFile(String fileName)
        {
            List<Customer> lstCustomer = new List<Customer>();
            String line;

            // If file does not exists 
            // return null
            if (!System.IO.File.Exists(@fileName))
            {
                Console.WriteLine("FileNot Found!!!");
                return null;
            }

            // If file exists 
            // Process the file to evaluate 
            System.IO.StreamReader file = new System.IO.StreamReader(@fileName);
            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    String[] word = line.Split('\t');

                    int index = 0;
                    Customer objCustomer = new Customer();
                    objCustomer.FirstName = word[index++];
                    objCustomer.LastName = word[index++];
                    objCustomer.MiddleInitial = word[index++];
                    objCustomer.AddressLine1 = word[index++];
                    objCustomer.AddressLine2 = word[index++];
                    objCustomer.City = word[index++];
                    objCustomer.State = word[index++];
                    objCustomer.Zipcode = word[index++];
                    objCustomer.Gender = word[index++];
                    objCustomer.PhoneNumber = word[index++];
                    objCustomer.Email = word[index++];
                    objCustomer.ProofOfPurchased = word[index++];
                    objCustomer.DateReceived = word[index++];
                    objCustomer.DateStarted = word[index++];
                    objCustomer.DateSaved = word[index++];
                    objCustomer.CountBackSpaceKey = Convert.ToInt32(word[index++]);

                    String uniqueId = generateCustomerUniqueId(objCustomer);
                    objCustomer.UniqueId = uniqueId;

                    lstCustomer.Add(objCustomer);
                }
            }
            catch(Exception)
            {
                return null;
            }

            file.Close();

            return lstCustomer;
        }

        /**
         *  Format the difference in time as MM:SS
         *  Minutes : Seconds
         **/
        private String formatTimeMinutesSeconds(double seconds)
        {
            if(seconds < 0)
            {
                return "00 : 00";
            }

            double dblMM = seconds / 60;
            int MM = (seconds > 59) ? ((int)Math.Truncate((dblMM))) : 0;

            double dblSS = MM * 60;
            int SS = (int)Math.Abs(seconds - dblSS);

            String time = (MM < 10 ? "0" + MM : "" + MM) + " : " + (SS < 10 ? "0" + SS : "" + SS);

            return time;
        }

        /**
         * UniqueId Combination of (FirstName + " " + LastName + "#" + Phone Number)
         **/
        public String generateCustomerUniqueId(Customer objCustomer)
        {
            String uniqueId = objCustomer.FirstName + " " + objCustomer.LastName + "#" + objCustomer.PhoneNumber;
            return uniqueId;
        }

        /**
         * Evaluation of fields
         **/
        public Dictionary<String, String> evaluateCustomerDataFile(List<Customer> lstCustomer)
        {
            Dictionary<String, String> dictEval = new Dictionary<String, String>();
            int numRecords = 0;

            double minEntryTime = 0;
            double maxEntryTime = 0;
            double avgEntryTime = 0;

            double minInterRecTime = 0;
            double maxInterRecTime = 0;
            double avgInterRecTime = 0;

            double totalTime = 0;
            double countBackspace = 0;

            if(lstCustomer == null || lstCustomer.Count == 0)
            {
                return null;
            }

            // Number of Records
            numRecords = lstCustomer.Count;

            // Entry Time
            double minEntryTimeSeconds = Double.MaxValue;
            double maxEntryTimeSeconds = Double.MinValue;
            double totalEntryTimeSeconds = 0;
            double avgEntryTimeSeconds = 0;

            // Inter Record Time
            double minInterRecTimeSeconds = Double.MaxValue;
            double maxInterRecTimeSeconds = Double.MinValue;
            double totalInterRecTimeSeconds = 0;
            double avgInterRecTimeSeconds = 0;


            double totalTimeSeconds = 0;

            DateTime firstRecordTime = DateTime.Now;
            DateTime lastRecordTime = DateTime.Now;

            Boolean isFirstRecord = false;

            
            foreach (var objCustomer in lstCustomer)
            {
               
                String dateStarted = objCustomer.DateStarted;
                String dateSaved = objCustomer.DateSaved;

                DateTime dtStarted = Convert.ToDateTime(dateStarted);
                DateTime dtSaved = Convert.ToDateTime(dateSaved);

                double diff = (dtSaved - dtStarted).TotalSeconds;
                totalEntryTimeSeconds += diff;
                

                if (diff < minEntryTimeSeconds)
                {
                    minEntryTimeSeconds = diff;
                }

                if(diff > maxEntryTimeSeconds)
                {
                    maxEntryTimeSeconds = diff;
                }

                if (isFirstRecord == false)
                {
                    isFirstRecord = true;
                    firstRecordTime = dtStarted;
                }
                else
                {
                    double diff2 = (dtStarted - lastRecordTime).TotalSeconds;
                    totalInterRecTimeSeconds += diff2;

                    if(diff2 < minInterRecTimeSeconds)
                    {
                        minInterRecTimeSeconds = diff2;
                    }

                    if(diff2 > maxInterRecTimeSeconds)
                    {
                        maxInterRecTimeSeconds = diff2;
                    }

                }
                lastRecordTime = dtSaved;

                // Count Backspace
                countBackspace += objCustomer.CountBackSpaceKey;
            }

            totalTimeSeconds = (lastRecordTime - firstRecordTime).TotalSeconds;

            // Average time Entry 
            avgEntryTimeSeconds = totalEntryTimeSeconds / numRecords;
            avgInterRecTimeSeconds = totalInterRecTimeSeconds / numRecords;

            minEntryTime = minEntryTimeSeconds;
            maxEntryTime = maxEntryTimeSeconds;
            avgEntryTime = avgEntryTimeSeconds;

            minInterRecTime = minInterRecTimeSeconds;
            maxInterRecTime = maxInterRecTimeSeconds;
            avgInterRecTime = avgInterRecTimeSeconds;

            totalTime = totalTimeSeconds;

            dictEval.Add("numRecords", Convert.ToString(numRecords));

            dictEval.Add("minEntryTime", this.formatTimeMinutesSeconds(minEntryTime));
            dictEval.Add("maxEntryTime", this.formatTimeMinutesSeconds(maxEntryTime));
            dictEval.Add("avgEntryTime", this.formatTimeMinutesSeconds(avgEntryTime));

            dictEval.Add("minInterRecTime", this.formatTimeMinutesSeconds(minInterRecTime));
            dictEval.Add("maxInterRecTime", this.formatTimeMinutesSeconds(maxInterRecTime));
            dictEval.Add("avgInterRecTime", this.formatTimeMinutesSeconds(avgInterRecTime));

            dictEval.Add("totalTime", this.formatTimeMinutesSeconds(totalTime));
            dictEval.Add("countBackspace", Convert.ToString(countBackspace));
            return dictEval;
        }

        /**
         * Writes the customer data file evaluation in the file
         * with file separation as \n next line
         **/
        public void writeCustomerDataToFile(Dictionary<String, String> dictEval, String fileName)
        {
            ICollection<String> uniqueIds = dictEval.Keys;
            Dictionary<String, String> evalName = new Dictionary<String, String>();

            evalName.Add("numRecords", "Number of records");

            evalName.Add("minEntryTime", "Minimum entry time");
            evalName.Add("maxEntryTime", "Maximum entry time");
            evalName.Add("avgEntryTime", "Average entry time");

            evalName.Add("minInterRecTime", "Minimum inter-record time");
            evalName.Add("maxInterRecTime", "Maximum inter-record time");
            evalName.Add("avgInterRecTime", "Average inter-record time");

            evalName.Add("totalTime", "Total time");
            evalName.Add("countBackspace", "Backspace count");

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(System.IO.Path.GetFullPath(@fileName)))
            {
                foreach (var key in uniqueIds)
                {
                    String line = evalName[key] + " = " + dictEval[key];

                    file.WriteLine(line);
                }
            }
        }
    }
}
