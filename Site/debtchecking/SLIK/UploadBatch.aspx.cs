using DMS.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DebtChecking.SLIK
{
    public partial class UploadBatch : MasterPage
    {
        private string batchId = "";
        private DataTable dtGrid = new DataTable();
        private string execSP = "";
        private string sheetName = "";
        private string tempTable = "";
        private int rowStart;

        protected void Page_Load(object sender, EventArgs e)
        {
            DataTable dtUpload = new DataTable();
            dtUpload = conn.GetDataTable("select * from CBAS_UPLOAD_DATA where tablename = '" + Request.QueryString["tablename"] + "'", null, dbtimeout);
            if (dtUpload.Rows.Count > 0)
            {
                //deskripsi.Text = dtUpload.Rows[0]["upload_data_desc"].ToString();
                execSP = dtUpload.Rows[0]["executeQuery"].ToString();
                sheetName = dtUpload.Rows[0]["sheetName"].ToString();
                rowStart = int.Parse(dtUpload.Rows[0]["rowStart"].ToString());
                tempTable = dtUpload.Rows[0]["tablenametemp"].ToString();
            }
        }

        protected void ASPxUploadControl2_FileUploadComplete(object sender, DevExpress.Web.FileUploadCompleteEventArgs e)
        {
            TXT_PROGRESS.Text = "Please wait until this process has done, dont refresh / move from this page";
            TXT_PROGRESS.ForeColor = System.Drawing.Color.Black;
            TXT_PROGRESS.Visible = true;
            DataTable dtStrukur = new DataTable();
            batchId = DateTime.Now.ToString("yyMMddHHmmssfff");
            string filename = MapPath("~/upload/Batch/") + batchId + "_" + e.UploadedFile.FileName;
            e.UploadedFile.SaveAs(filename, true);
            //string nameFile = new FileInfo(filename).Name;
            e.CallbackData = batchId + "#;" + filename + "#;" + Request.QueryString["tablename"];
        }

        protected void panelUploadFile_Callback(object sender, DevExpress.Web.CallbackEventArgsBase e)
        {
            txtlink.Visible = false;
            try
            {
                string[] myParams = e.Parameter.ToString().Split(new string[] { "#;" }, StringSplitOptions.None);
                string batchId = myParams[0];
                string filename = myParams[1];
                string tableName = myParams[2];

                conn.ExecNonQuery("insert into CBAS_UPLOAD_BATCH(batchId,filename,upload_date,upload_by,tableName,current_track, current_track_by) " +
                    " values (@1, @2, getdate(), @3, @4, '1', @3)",
                    new object[] { batchId, new FileInfo(filename).Name, Session["UserId"], tempTable }, dbtimeout);

                DataTable dtStrukur = new DataTable();
                dtStrukur = conn.GetDataTable(@"select * from CBAS_UPLOAD_DATA_DETAIL where tableName = '"
                                            + tempTable + "'  order by fieldNumber", null, dbtimeout);
                using (var package = new ExcelPackage(new FileInfo(filename)))
                {
                    ExcelWorksheet workSheet = package.Workbook.Worksheets[sheetName];
                    if (workSheet == null)
                        throw new Exception("Invalid Sheet Name.");
                    int? lastRow = GetLastUsedRow(workSheet);
                    if (lastRow == null || lastRow <= 1)
                        throw new Exception("Upload file failed, file has no contents.");
                    h_batchid.Value = batchId;
                    NameValueCollection keys = new NameValueCollection();
                    staticFramework.saveNVC(keys, "batchId", batchId);

                    

                    for (int i = rowStart;
                                i <= lastRow;
                                i++)
                    {
                        NameValueCollection fields = new NameValueCollection();
                        string msg = "";
                        for (int j = workSheet.Dimension.Start.Column;
                                    j <= workSheet.Dimension.End.Column;
                                    j++)
                        {
                            int fieldIndex = j - 1;
                            //string nilai = workSheet.Cells[i, j].ToString();
                            //object cellValue = workSheet.Cells[i, j].Value;
                            object cellValue = null;
                            //nilai = workSheet.Cells[i, j].Value.ToString();                            
                            string fieldName = "";
                            string fieldType = "";
                            bool isKey = false;
                            int fieldMaxChar = 0;
                            bool isMandatory = false;
                            string paramReffSQL = "";
                            #region validate type,length

                            if (dtStrukur.Rows.Count >= j)
                            {
                                fieldName = dtStrukur.Rows[fieldIndex]["fieldName"].ToString();
                                fieldType = dtStrukur.Rows[fieldIndex]["fieldType"].ToString();
                                isKey = dtStrukur.Rows[fieldIndex]["iskey"] == DBNull.Value ? false : Convert.ToBoolean(dtStrukur.Rows[fieldIndex]["iskey"]);
                                if (isKey == true)
                                {
                                    cellValue = int.Parse(i.ToString());
                                }
                                else
                                {
                                    cellValue = workSheet.Cells[i, j - 1].Value;
                                }
                                                          
                                try
                                {
                                    fieldMaxChar = int.Parse(dtStrukur.Rows[fieldIndex]["fieldMaxChar"].ToString());
                                }
                                catch { }

                                bool.TryParse(dtStrukur.Rows[fieldIndex]["IsMandatory"].ToString(), out isMandatory);
                                paramReffSQL = dtStrukur.Rows[fieldIndex]["ParamReffSQL"].ToString();

                                if (isMandatory && (cellValue == null || (cellValue != null && cellValue.ToString().Trim() == "")))
                                {
                                    if (fieldName == "NoKTP")
                                    {
                                        msg += string.Format("{0} is mandatory field", "Identity Number") + ";";
                                    }
                                    else
                                    {
                                        msg += string.Format("{0} is mandatory field", fieldName) + ";";
                                    }                                   
                                }
                                else
                                {
                                    if (cellValue == null)
                                        continue;
                                    if (fieldType.ToLower() == "numeric"
                                        || fieldType.ToLower() == "int"
                                        || fieldType.ToLower() == "date")
                                    {
                                        //Regex regex = new Regex(@" ^ -*[0 - 9,\.] + $");//termasuk negative dan decimal
                                        Regex regex = new Regex("^[0-9]+$");
                                        if (!regex.IsMatch(cellValue.ToString()))
                                        {                                            
                                            msg += string.Format("{0}({1}) invalid format , must be numeric", fieldName, cellValue.ToString()) + ";";
                                        }
                                        else
                                        {
                                            if (fieldType.ToLower() == "date")
                                            {
                                                try
                                                {
                                                    DateTime date = DateTime.ParseExact(cellValue.ToString(), "ddMMyyyy", null);
                                                }
                                                catch (Exception exp)
                                                {
                                                    msg += string.Format("{0}({1}) invalid format, must be date",fieldName,  cellValue.ToString()) + ";";
                                                }
                                            }
                                        }
                                    }

                                    if(paramReffSQL.Trim()!="")
                                    {
                                        DataTable dtCheck = conn.GetDataTable(paramReffSQL, new object[] { cellValue.ToString() }, dbtimeout);
                                        if(dtCheck.Rows.Count==0)
                                        {
                                            msg += string.Format("invalid field value {0}({1}) not linked to parameter ", fieldName, cellValue.ToString());
                                        }

                                    }

                                    if (cellValue.ToString().Length > fieldMaxChar)
                                    {
                                        int maxCharTempTable = 0;int charDiff = 0; int cellLength = cellValue.ToString().Length;
                                        conn.ExecReader("select CHARACTER_MAXIMUM_LENGTH from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @1 and COLUMN_NAME = @2",
                                            new object[] { tempTable, fieldName}, dbtimeout);
                                        if (conn.hasRow())
                                        {
                                            maxCharTempTable = int.Parse(conn.GetFieldValue(0));
                                        }
                                        if (cellLength <= maxCharTempTable)
                                        {
                                            charDiff = cellLength;
                                        }
                                        else
                                        {
                                            charDiff = maxCharTempTable;
                                        }
                                        msg += string.Format("{0}-lenght is greather then {1}", fieldName, fieldMaxChar ) + ";";
                                        cellValue = cellValue.ToString().Substring(0, charDiff);
                                        //cellValue = "";
                                    }
                                }
                            }

                            #endregion validate type,length

                            if (cellValue != null && cellValue.ToString() != "" /*&& msg == ""*/)
                            {
                                if (isKey == false)
                                {
                                    staticFramework.saveNVC(fields, fieldName, cellValue);
                                }
                                else
                                {
                                    staticFramework.saveNVC(keys, fieldName, cellValue);
                                }
                            }
                            cellValue = null;
                        }
                        
                        if (msg.Trim() != "")
                        {
                            staticFramework.saveNVC(fields, "message", msg);
                        }
                        else
                        {
                            staticFramework.saveNVC(fields, "message", "Success");
                        }
                        staticFramework.save(fields, keys, tempTable, conn);
                    }
                }

                object[] param = new object[] { batchId };
                conn.ExecNonQuery(execSP, param, dbtimeout);
                TXT_PROGRESS.Text = "File successfully uploaded. ";
                txtlink.Visible = true;
                TXT_PROGRESS.ForeColor = System.Drawing.Color.Black;
            }
            catch (Exception ex)
            {
                TXT_PROGRESS.Text = ex.Message.Substring(0, (ex.Message.IndexOf("Last Query") > 0 ? ex.Message.IndexOf("Last Query") : ex.Message.Length));
                TXT_PROGRESS.ForeColor = System.Drawing.Color.Red;
            }
            TXT_PROGRESS.Visible = true;
        }

        protected void btnTemplate_click(object sender, System.EventArgs e)
        {
            Response.ContentType = "Application/xlsx";
            Response.AppendHeader("Content-Disposition", "attachment; filename=Template_upload_web.xlsx");
            Response.TransmitFile(Server.MapPath("~/Templates/Master/Template_upload_web.xlsx"));
            Response.End();
        }

        protected int GetLastUsedRow(ExcelWorksheet sheet)
        {
            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 1, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrEmpty(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }
    }
}