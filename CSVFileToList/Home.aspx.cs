using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

namespace CSVFileToList
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void importButton_Click(object sender, EventArgs e)
        {
            string fileName = @"~/App_Data/" + fileNameTextBox.Text;

            if (File.Exists(Server.MapPath(fileName)))
            {
                DataTable csvDataTable = new DataTable();
                using (TextFieldParser csvReader = new TextFieldParser(Server.MapPath(fileName)))
                {



                    csvReader.SetDelimiters(new string[] { "," });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                  //  string[] colFields = csvReader.ReadFields();
                    //foreach (string column in colFields)
                    //{
                    //    DataColumn datacolumn = new DataColumn(column);
                    //    datacolumn.AllowDBNull = true;
                    //    csvDataTable.Columns.Add(datacolumn);
                    //}

                    csvDataTable.Columns.Add("Code", typeof(string));
                    csvDataTable.Columns.Add("Description", typeof(string));
                    csvDataTable.Columns.Add("Value", typeof(string));
                    

                    while (!csvReader.EndOfData)
                    {
                        string[] fieldData = csvReader.ReadFields();
                        //Making empty value as null
                        string code = null;
                        string description = null;
                        string value = null;
                      //  int count = 0;
                        for (int i = 0; i < fieldData.Length; i++)
                        {
                            code = fieldData[0];
                            if (fieldData[i]==String.Empty)
                            {
                                break;
                            }
                            else
                            {
                                
                                if (i != 0 && i%2 == 1)
                                {
                                    description = fieldData[i];
                                }
                                else if (i != 0 && i%2 == 0)
                                {
                                    value = fieldData[i];
                                    csvDataTable.Rows.Add(code, description, value); 
                                }                                                              
                            }
                        }                      
                    }
                }               
                importStatusLabel.Text = "Done";
                //GridView1.DataSource = csvDataTable;
                //GridView1.DataBind();
            }
            else
            {
                importStatusLabel.Text = "File not found.";
            }
        }


        public int SaveMainCSVFileData(DataTable tableCSV)
        {
            int rowsInserted = 0;

            for (int i = 0; i < tableCSV.Rows.Count; i++)
            {
                string code = tableCSV.Rows[i].ItemArray.GetValue(0).ToString();
                string type = tableCSV.Rows[i].ItemArray.GetValue(1).ToString();
                string year = tableCSV.Rows[i].ItemArray.GetValue(2).ToString();
                string make = tableCSV.Rows[i].ItemArray.GetValue(3).ToString();
                string model = tableCSV.Rows[i].ItemArray.GetValue(4).ToString();
                string trim = tableCSV.Rows[i].ItemArray.GetValue(5).ToString();
                string drive = tableCSV.Rows[i].ItemArray.GetValue(6).ToString();
                string doors = tableCSV.Rows[i].ItemArray.GetValue(7).ToString();
                string body = tableCSV.Rows[i].ItemArray.GetValue(8).ToString();
                decimal wholesale = Convert.ToDecimal(tableCSV.Rows[i].ItemArray.GetValue(9));
                decimal retail = Convert.ToDecimal(tableCSV.Rows[i].ItemArray.GetValue(10));

                string query = String.Format(@"Insert into tblMain values(@code,@type,@year,@make,@model,@trim,@drive,@doors,@body,@wholesale,@retail)");



                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@code", code);
                        command.Parameters.AddWithValue("@type", type);
                        command.Parameters.AddWithValue("@year", year);
                        command.Parameters.AddWithValue("@make", model);
                        command.Parameters.AddWithValue("@model", model);
                        command.Parameters.AddWithValue("@trim", trim);
                        command.Parameters.AddWithValue("@drive", drive);
                        command.Parameters.AddWithValue("@doors", doors);
                        command.Parameters.AddWithValue("@body", body);
                        command.Parameters.AddWithValue("@wholesale", wholesale);
                        command.Parameters.AddWithValue("@retail", retail);
                        connection.Open();
                        rowsInserted = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }

            return rowsInserted;

        }


        public int SaveAddCode(string code)
        {
            int rowsInserted = 0;
            string query = String.Format("insert into tblCode values(@code)");
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@code",code);
                    connection.Open();
                    rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return rowsInserted;
        }

        public int GetLastIdentityOfAddCode()
        {
            int lastIdentityValue = 0;
            string query = String.Format("select IDENT_CURRENT('tblCode')");

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    connection.Open();
                    lastIdentityValue = Convert.ToInt32(command.ExecuteScalar());
                    connection.Close();
                }
            }
            return lastIdentityValue;
        }

        public int SaveAddInformation(string description,string value, int lastIdentity)
        {
            int rowsInserted = 0;
            string query = String.Format("Insert into tblAdd values(@description,@vale,@codeId)");
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[1].ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@vale",value);
                    command.Parameters.AddWithValue("@codeId",lastIdentity);
                    connection.Open();
                    rowsInserted = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return rowsInserted;
        }


        


    }
}