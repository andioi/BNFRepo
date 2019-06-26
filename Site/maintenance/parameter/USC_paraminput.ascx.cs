using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using DevExpress.Web;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using My.Tools;
using My.Framework;

namespace MikroMnt.Parameter
{
    public partial class USC_paraminput : System.Web.UI.UserControl
    {
        DbConnection conn = null;
        int dbtimeout = 600;
        DataTable dtparam = null;
        string TableNm = "";
        const string _autogen = "[AUTOGENERATE]";
        DataTable dtparamschema;
        string sqldata, sqlpending1, sqlpending2, sqlsortby;

        #region static vars
        private static string Q_PARAMSYS = "SELECT * FROM PARAMETERSYSTEMFIELD WHERE TABLENM = @1 ORDER BY FIELDPOS";
        #endregion

        #region grid

        protected void creategridquery()
        {
            string fieldstr = "", pfieldstr = "", tempfieldstr = "", condstr = "";
            string Key = "''";
            SortedList sortby = new SortedList();
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString().ToUpper().Replace(":[", "[X].[");
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field 
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                int SortSeq = 0;
                if (dtparam.Rows[i]["SORTSEQ"].ToString() != "")
                    SortSeq = (int)dtparam.Rows[i]["SORTSEQ"];
                if (SortSeq > 0)
                {
                    if (dtparam.Rows[i]["GROUPBY"].ToString() == "" || (bool)dtparam.Rows[i]["GROUPBY"] == false)   //ignore grouped by columns
                        if (!sortby.ContainsKey(SortSeq))        //ignore repetitive order seq
                        {
                            if (dtparam.Rows[i]["SORTASC"].ToString() != "" && (bool)dtparam.Rows[i]["SORTASC"] == false)
                                sortby.Add(SortSeq, FieldNm + " DESC");
                            else
                                sortby.Add(SortSeq, FieldNm);
                        }
                }
                if (FieldKey)
                {
                    Key += "+' AND [" + FieldNm + "]='''+ CONVERT(VARCHAR(200),[" + FieldNm + "])+''''";
                }
                if (FieldReff != "")
                {
                    int idx = FieldReff.IndexOf(",");
                    int idx2 = FieldReff.LastIndexOf("SELECT ", idx) + 7;
                    int idx3 = FieldReff.IndexOf(" FROM", idx);

                    string FieldReffId = FieldReff.Substring(idx2, idx - idx2).Trim();
                    string FieldReffDesc = FieldReff.Substring(idx + 1, idx3 - (idx + 1)).Trim();
                    string FieldReffFrom = FieldReff.Substring(FieldReff.IndexOf("FROM"));
                    string FieldReffWhere = "";
                    if (FieldReff.IndexOf("WHERE") > 0)
                        FieldReffWhere += " AND ";
                    else
                        FieldReffWhere += " WHERE ";

                    fieldstr += ", " + "(SELECT " + FieldReffDesc + " " + FieldReffFrom + FieldReffWhere + FieldReffId + "=[X].[" + FieldNm + "]) AS [" + FieldNm + "_DESC]"; ;
                    pfieldstr += ", " + "(SELECT " + FieldReffDesc + " " + FieldReffFrom + FieldReffWhere + FieldReffId + "=[X].[" + FieldNm + "]) AS [" + FieldNm + "_DESC]"; ;
                }
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    int prelen, sulen;
                    prelen = 1 + AutoPrefix.Length;
                    sulen = AutoSufix.Length + AutoPrefix.Length;
                    fieldstr += ", substring([" + FieldNm + "], " + prelen.ToString() + ", len(" + FieldNm + ") - " + sulen.ToString() + ") as [" + FieldNm + "]";
                    pfieldstr += ", substring([" + FieldNm + "], " + prelen.ToString() + ", len(" + FieldNm + ") - " + sulen.ToString() + ") as [" + FieldNm + "]";
                }
                else
                {
                    fieldstr += ", [" + FieldNm + "]";
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                            dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                        pfieldstr += ", convert(float, [" + FieldNm + "]) as [" + FieldNm + "]";
                    else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                        pfieldstr += ", convert(datetime, [" + FieldNm + "]) as [" + FieldNm + "]";
                    else
                        pfieldstr += ", [" + FieldNm + "]";
                }
                tempfieldstr += ",(SELECT FIELDVALUE FROM PARAMETERSYSTEM_TEMPORARYDETAIL " +
                                "  WHERE FIELDNAME = '" + FieldNm + "' AND TEMPORARYID=Y.TEMPORARYID)[" + FieldNm + "] ";
                if (isAuto && FieldType != "auto")      //auto value but not autogenerated 
                {
                    string thisfieldauto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim();
                    if (thisfieldauto.StartsWith("q:"))
                    {
                        condstr += "and [X].[" + FieldNm + "] = '" + Request.QueryString[thisfieldauto.Substring(2)] + "' ";
                    }
                    else if (thisfieldauto.StartsWith("s:"))
                    {
                        condstr += "and [X].[" + FieldNm + "] = '" + Session[thisfieldauto.Substring(2)].ToString() + "' ";
                    }
                }
            }
            if (condstr != "")
                condstr = " WHERE " + condstr.Substring(4);
            sqldata = "SELECT " + Key + " AS __KEY " + fieldstr + " FROM " + TableNm + " [X] " + condstr;
            sqlpending1 = "SELECT Y.TEMPORARYID __KEY, Y.STATUS __STATUS, Y.CREATEDBY __CREATEDBY" + tempfieldstr + " FROM PARAMETERSYSTEM_TEMPORARY Y " +
                         "WHERE Y.TableName='" + TableNm + "'";
            sqlpending2 = "SELECT __KEY,__STATUS,__CREATEDBY" + pfieldstr + " FROM (" + sqlpending1 + ") [X] " + condstr;
            sqlsortby = "";
            for (int i = 0; i < sortby.Count; i++)
                sqlsortby += (string)sortby.GetByIndex(i) + ", ";
            sortby.Clear();
            if (sqlsortby != "")
                sqlsortby = sqlsortby.Substring(0, sqlsortby.Length - 2);
        }

        public void createGridColumns(DevExpress.Web.ASPxGridView grid, DevExpress.Web.ASPxGridView gridpending)
        {
            DevExpress.Web.GridViewDataTextColumn p;
            DevExpress.Web.GridViewDataTextColumn c;
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString().ToUpper().Replace(":[", "[X].[");
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                bool GroupBy = false;
                if (dtparam.Rows[i]["GROUPBY"].ToString() != "")
                    GroupBy = (bool)dtparam.Rows[i]["GROUPBY"];
                switch (FieldType)
                {
                    case "d":
                        break;
                }

                c = new DevExpress.Web.GridViewDataTextColumn();
                if (FieldReff != "")
                    c.FieldName = FieldNm + "_DESC";
                else
                    c.FieldName = FieldNm;
                c.Caption = FieldDesc;
                c.Settings.FilterMode = ColumnFilterMode.Value;
                c.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" || isAuto)
                    c.Visible = false;
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                    c.PropertiesTextEdit.DisplayFormatString = "###,##0.00";
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                    c.PropertiesTextEdit.DisplayFormatString = "dd/MM/yyyy";
                grid.Columns.Add(c);
                if (GroupBy)
                    grid.GroupBy(c);

                p = new DevExpress.Web.GridViewDataTextColumn();
                if (FieldReff != "")
                    p.FieldName = FieldNm + "_DESC";
                else
                    p.FieldName = FieldNm;
                p.Caption = FieldDesc;
                p.Settings.FilterMode = ColumnFilterMode.Value;
                p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" || isAuto)
                    p.Visible = false;
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                    p.PropertiesTextEdit.DisplayFormatString = "###,##0.00";
                else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                    p.PropertiesTextEdit.DisplayFormatString = "dd/MM/yyyy";
                gridpending.Columns.Add(p);
                if (GroupBy)
                    gridpending.GroupBy(p);
            }
            p = new DevExpress.Web.GridViewDataTextColumn();
            p.FieldName = "__STATUS";
            p.Caption = "Status";
            p.Settings.FilterMode = ColumnFilterMode.Value;
            p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            gridpending.Columns.Add(p);
            p = new DevExpress.Web.GridViewDataTextColumn();
            p.FieldName = "__CREATEDBY";
            p.Caption = "Created By";
            p.Settings.FilterMode = ColumnFilterMode.Value;
            p.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            gridpending.Columns.Add(p);

            grid.KeyFieldName = "__KEY";
            gridpending.KeyFieldName = "__KEY";
        }

        public void dtbinddata(DevExpress.Web.ASPxGridView grid)
        {
            DataTable dt = conn.GetDataTable(sqldata, null, dbtimeout);
            DataView dv = new DataView(dt);
            dv.Sort = sqlsortby;
            grid.DataSource = dv;
            grid.DataBind();
        }

        public void dtbindpending(DevExpress.Web.ASPxGridView gridpending)
        {
            DataTable dt = conn.GetDataTable(sqlpending2, null, dbtimeout);
            DataView dv = new DataView(dt);
            dv.Sort = sqlsortby;
            gridpending.DataSource = dv;
            gridpending.DataBind();
        }

        #endregion

        #region data
        public void savepending(string _keyfield)
        {
            dynamicFramework dyn = null;
            NameValueCollection Fields = new NameValueCollection();
            string fieldstr = "", keyfield = "", tempfieldstr = "T2.TEMPORARYID", tempkeyfield = "", fieldauto = "";
            bool insertnew = _keyfield == null;
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                string FormulaType = dtparam.Rows[i]["FormulaType"].ToString();
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field 
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];

                fieldstr += ",[" + FieldNm + "]";
                tempfieldstr += ",(SELECT FIELDVALUE FROM PARAMETERSYSTEM_TEMPORARYDETAIL " +
                                "  WHERE FIELDNAME = '" + FieldNm + "' AND TEMPORARYID=T2.TEMPORARYID)[" + FieldNm + "] ";

                if (_keyfield == null)
                {
                    Control oCtrl = this.FindControl(FieldNm);
                    if (locked)
                        oCtrl = this.FindControl("l_" + FieldNm);          //overwrite values with original locked field value from the hidden "l_" prefix controls
                    if (FieldKey)
                    {
                        HtmlInputHidden hCtrl = (HtmlInputHidden)this.FindControl("h_" + FieldNm);
                        if (hCtrl.Value.Trim() != "")       //hidden control set during retrieve 
                        {
                            //oCtrl = hCtrl;                  //always use key value from edited keys... oh well, maybe dont force edit if user want to change the key field value.. ease creation of large param with little differences on the param fields.. 
                            if (staticFramework.getvalue(oCtrl) == null || staticFramework.getvalue(oCtrl).ToString().Trim() == "")
                                oCtrl = hCtrl;              //in disabled mode, sometimes the control doesnt pass the value
                            if (staticFramework.getvalue(oCtrl).ToString() == staticFramework.getvalue(hCtrl).ToString())       //falsefy only if oCtrl value not changed.. meaning: dont do insertion cek..
                                insertnew = false;
                        }
                    }
                    Fields[FieldNm] = staticFramework.toSql(staticFramework.getvalue(oCtrl));
                    if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                    {
                        Fields[FieldNm] = staticFramework.toSql(AutoPrefix + staticFramework.getvalue(oCtrl) + AutoSufix);
                    }
                    if (FieldType == "auto" && Fields[FieldNm].IndexOf(_autogen) >= 0)
                    {
                        string thisfieldauto = dtparam.Rows[i]["FIELDAUTO"].ToString();
                        if (thisfieldauto.StartsWith("q:") ||
                            thisfieldauto.StartsWith("s:") ||
                            thisfieldauto.StartsWith("f:"))
                        {
                            string thisautovalue = "", thisautotype = "";

                            if (thisfieldauto.StartsWith("q:"))
                                thisautovalue = "'" + Request.QueryString[thisfieldauto.Substring(2)] + "'";
                            else if (thisfieldauto.StartsWith("s:"))
                                thisautovalue = "'" + Session[thisfieldauto.Substring(2)].ToString() + "'";
                            else if (thisfieldauto.StartsWith("f:"))
                                thisautovalue = thisfieldauto.Substring(2);

                            if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String")
                                thisautotype = "varchar(" + dtparamschema.Columns[FieldNm].MaxLength.ToString() + ")";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid")
                                thisautotype = "uniqueidentifier";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Boolean")
                                thisautotype = "bit";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                                thisautotype = "datetime";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int16" ||
                                dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int32" ||
                                dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int64")
                                thisautotype = "int";
                            else if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                                dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                                thisautotype = "float";

                            thisfieldauto = "DECLARE @" + FieldNm + " " + thisautotype + " SET @" + FieldNm + " = " + thisautovalue;
                        }

                        Fields[FieldNm] = "@" + FieldNm;
                        fieldauto += thisfieldauto + Environment.NewLine;
                    }
                    if (FieldReff.IndexOf(":[") > 0 && FieldReff.Trim().ToLower().IndexOf("select") == 0)
                    {   //special check for cascading refferencial FieldType
                        string qry = FieldReff.Trim();
                        qry = qry.Substring(qry.IndexOf(" ") + 1).Trim();
                        string fldkey = qry.Substring(0, qry.IndexOf(",")).Trim();
                        qry = qry.Substring(qry.IndexOf(",") + 1).Trim();
                        int cekpos = qry.IndexOf(",");
                        if (cekpos == -1 || cekpos > qry.IndexOf(" "))
                            cekpos = qry.IndexOf(" ");
                        string flddesc = qry.Substring(0, cekpos).Trim();
                        qry = qry.Substring(qry.ToLower().IndexOf("from") + 5).Trim();
                        string tblname = qry.Substring(0, qry.ToLower().IndexOf("where")).Trim();
                        qry = "select " + fldkey + " from " + tblname + " where " + fldkey + " = " + Fields[FieldNm];
                        object[] cekpar = new object[] { Fields[FieldNm] };
                        conn.ExecReader(qry, cekpar, dbtimeout);                //cek if Fields[FieldNm] contains correct key field
                        if (!conn.hasRow())
                        {                                                       //if not, cek if Fields[FieldNm] contains desc field
                            qry = "select " + fldkey + " from " + tblname + " where " + flddesc + " = " + Fields[FieldNm];
                            conn.ExecReader(qry, cekpar, dbtimeout);
                            if (conn.hasRow())
                            {                                                   //if so, renew Fields[FieldNm] to have value from key field 
                                Fields[FieldNm] = staticFramework.toSql(conn.GetNativeFieldValue(fldkey));
                            }
                        }
                    }
                    /* */
                    if (staticFramework.getvalue(oCtrl) != null && staticFramework.getvalue(oCtrl).ToString() != "")
                        switch (FormulaType)
                        {
                            case "1":
                                if (dyn == null)
                                    dyn = new dynamicFramework(conn);
                                try
                                {
                                    string fieldvalue = staticFramework.getvalue(oCtrl).ToString();
                                    string formula = dyn.retrvCondFW(fieldvalue);
                                    string formulaquery = dyn.Retrieve(formula, "").ToLower().Replace("select ", "select top 0 ");
                                    conn.ExecNonQuery(formulaquery, null, dbtimeout);
                                }
                                catch (Exception exd)
                                {
                                    string errmsg = exd.Message;
                                    if (errmsg.IndexOf("Last Query") > 0)
                                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                                    string msg = "Invalid formula in " + FieldDesc + "\nErrMsg: " + errmsg;
                                    throw new Exception(msg);
                                }
                                break;
                            case "2":
                                if (dyn == null)
                                    dyn = new dynamicFramework(conn);
                                try
                                {
                                    string[] strsep = new string[] { "&&", "||" };
                                    string[] fieldvalues = staticFramework.getvalue(oCtrl).ToString().Split(strsep, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string fieldvalue in fieldvalues)
                                    {
                                        string formulacond = " and " + dyn.retrvCondFW(fieldvalue);
                                        string formulaquery = dyn.Retrieve("1", formulacond).ToLower().Replace("select ", "select top 0 ").Replace("@value", "null");
                                        conn.ExecNonQuery(formulaquery, null, dbtimeout);
                                    }
                                }
                                catch (Exception exd)
                                {
                                    string errmsg = exd.Message;
                                    if (errmsg.IndexOf("Last Query") > 0)
                                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                                    string msg = "Invalid formula in " + FieldDesc + "\nErrMsg: " + errmsg;
                                    throw new Exception(msg);
                                }
                                break;
                            case "3":
                                if (dyn == null)
                                    dyn = new dynamicFramework(conn);
                                try
                                {
                                    string fieldvalue = staticFramework.getvalue(oCtrl).ToString();
                                    string formulacond = " and " + dyn.retrvCondFW(fieldvalue);
                                    string formulaquery = dyn.Retrieve("1", formulacond).ToLower().Replace("select ", "select top 0 ");
                                    conn.ExecNonQuery(formulaquery, null, dbtimeout);
                                }
                                catch (Exception exd)
                                {
                                    string errmsg = exd.Message;
                                    if (errmsg.IndexOf("Last Query") > 0)
                                        errmsg = errmsg.Substring(0, errmsg.IndexOf("Last Query"));
                                    string msg = "Invalid formula in " + FieldDesc + "\nErrMsg: " + errmsg;
                                    throw new Exception(msg);
                                }
                                break;
                            default:
                                break;
                        }
                    /* */

                    if (FieldKey)
                    {
                        keyfield += " AND [" + FieldNm + "]=" + Fields[FieldNm];
                        tempkeyfield += " AND [" + FieldNm + "]=" + Fields[FieldNm];
                        tempkeyfield += " AND [" + FieldNm + "]<>" + staticFramework.toSql(_autogen);
                    }
                }
            }
            fieldstr = fieldstr.Substring(1);

            if (_keyfield != null)
            {
                keyfield = _keyfield;
                tempkeyfield = _keyfield;
            }

            string tempparamsql =
                fieldauto +
                "SELECT " +
                "(SELECT TOP 1 TEMPORARYID FROM " +
                "	(SELECT " + tempfieldstr + " FROM PARAMETERSYSTEM_TEMPORARY T1 " +
                "	JOIN PARAMETERSYSTEM_TEMPORARYDETAIL T2 ON T2.TEMPORARYID=T1.TEMPORARYID " +
                "	GROUP BY T2.TEMPORARYID " +
                "	)X WHERE 1=1 " + keyfield +
                ") as col0, NEWID() as col1 ";
            conn.ExecReader(tempparamsql, null, dbtimeout);
            conn.hasRow();

            string TemporarID = conn.GetFieldValue(0);
            if (TemporarID == "")
                TemporarID = conn.GetFieldValue(1);
            else
                if (insertnew)
                    throw new Exception("Parameter with that key value had already been in the Pending Approval!");

            string SavingType = "Insert";
            DataTable dt = conn.GetDataTable(
                    fieldauto +
                    " SELECT " + fieldstr + " FROM " + TableNm +
                    " WHERE 1=1 " + keyfield
                    , null, dbtimeout);
            if (dt.Rows.Count > 0)
            {
                SavingType = "Update";
                if (insertnew)
                    throw new Exception("Parameter with that key value already exists!");
            }

            if (_keyfield != null)
                SavingType = "Delete";

            conn.ExecuteNonQuery(
                    "DELETE PARAMETERSYSTEM_TEMPORARY WHERE TEMPORARYID=@1"
                    , new object[] { TemporarID }, dbtimeout);
            conn.ExecuteNonQuery(
                "INSERT INTO PARAMETERSYSTEM_TEMPORARY " +
                "(TemporaryID,TableName,Status,CreatedBy) VALUES " +
                "(@1,@2,@3,@4)"
                , new object[] { TemporarID, TableNm, SavingType, Session["UserID"] }, dbtimeout);

            for (int col = 0; col < dt.Columns.Count; col++)
            {
                string valuebefore = staticFramework.toSql(null), value = staticFramework.toSql(null);
                if (dt.Rows.Count > 0)
                    valuebefore = staticFramework.toSql(dt.Rows[0][col]);
                if (Fields.Count > col)
                    value = Fields[col];
                else
                    value = valuebefore;

                conn.ExecuteNonQuery(
                    fieldauto +
                    "INSERT INTO PARAMETERSYSTEM_TEMPORARYDETAIL " +
                    "(TemporaryID,FieldName,FieldValue,FieldValueBefore) VALUES " +
                    "(" + staticFramework.toSql(TemporarID) + "," +
                         staticFramework.toSql(dt.Columns[col].ColumnName) + "," +
                         "CONVERT(VARCHAR(8000)," + value + ")," +
                         "CONVERT(VARCHAR(8000)," + valuebefore + ")" +
                    ")"
                    , null, dbtimeout);
            }
        }
        public void deletepending(string TemporaryID)
        {
            conn.ExecuteNonQuery(
                "DELETE PARAMETERSYSTEM_TEMPORARY WHERE TemporaryID=@1"
                , new object[] { TemporaryID }, dbtimeout);
        }
        public void retrieve(string keyvalue)
        {
            DataTable dtretrieve = conn.GetDataTable("SELECT * FROM " + TableNm + " WHERE 1=1 " + keyvalue, null, dbtimeout);
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field 
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];

                WebControl oCtrl = (WebControl)TableInput.FindControl(FieldNm);
                if (oCtrl is ASPxComboBox)
                    reffcascade((ASPxComboBox)oCtrl, FieldReff);
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    string value = null;
                    if (dtretrieve.Rows.Count > 0)
                        value = dtretrieve.Rows[0][FieldNm].ToString();
                    if (value.Length > AutoPrefix.Length + AutoSufix.Length)
                        value = value.Substring(AutoPrefix.Length, value.Length - AutoSufix.Length - AutoPrefix.Length);
                    staticFramework.retrieve(value, oCtrl);
                }
                else
                    staticFramework.retrieve(dtretrieve, FieldNm, oCtrl);

                if (FieldKey)
                {
                    HtmlInputHidden hCtrl = (HtmlInputHidden)TableInput.FindControl("h_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, hCtrl);
                }

                if (locked)
                {
                    HtmlInputHidden lCtrl = (HtmlInputHidden)TableInput.FindControl("l_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, lCtrl);
                }
            }
        }
        public void retrievepending(string TemporaryID)
        {
            DataTable dtretrieve = conn.GetDataTable(
                sqlpending1 + " AND Y.TEMPORARYID=@1"
                , new object[] { TemporaryID }, dbtimeout);
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string AutoPrefix = "", AutoSufix = "";
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.String"                   //autoprefix and autosuffix must not be used to a non string fieldtype
                        && isAuto == false && FieldReff == "")                                              //and non-auto and non refferential field 
                {
                    AutoPrefix = dtparam.Rows[i]["AUTOPREFIX"].ToString();
                    AutoSufix = dtparam.Rows[i]["AUTOSUFIX"].ToString();
                }
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];

                WebControl oCtrl = (WebControl)TableInput.FindControl(FieldNm);
                if (oCtrl is ASPxComboBox)
                    reffcascade((ASPxComboBox)oCtrl, FieldReff);
                if (AutoPrefix.Length > 0 || AutoSufix.Length > 0)
                {
                    string value = null;
                    if (dtretrieve.Rows.Count > 0)
                        value = dtretrieve.Rows[0][FieldNm].ToString();
                    if (value.Length > AutoPrefix.Length + AutoSufix.Length)
                        value = value.Substring(AutoPrefix.Length, value.Length - AutoSufix.Length - AutoPrefix.Length);
                    staticFramework.retrieve(value, oCtrl);
                }
                else
                    staticFramework.retrieve(dtretrieve, FieldNm, oCtrl);

                if (FieldKey)
                {
                    HtmlInputHidden hCtrl = (HtmlInputHidden)TableInput.FindControl("h_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, hCtrl);
                }

                if (locked)
                {
                    HtmlInputHidden lCtrl = (HtmlInputHidden)TableInput.FindControl("l_" + FieldNm);
                    staticFramework.retrieve(dtretrieve, FieldNm, lCtrl);
                }
            }
        }

        #endregion

        #region control
        protected void initcontrol()
        {
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                string FieldReff = dtparam.Rows[i]["FIELDREFF"].ToString();
                WebControl oCtrl = (WebControl)TableInput.FindControl(FieldNm);
                if (FieldType == "auto")
                    oCtrl.BorderStyle = BorderStyle.None;
                else
                {
                    staticFramework.retrieveschema(dtparamschema, FieldNm, oCtrl);
                }
                if (oCtrl is DevExpress.Web.ASPxComboBox)
                    reffcascade((DevExpress.Web.ASPxComboBox)oCtrl, FieldReff);
            }
        }

        protected string createcontrol()
        {
            TableRow TableFilterRow;
            TableCell TableFilterDesc, TableFilterControl, TableFilterSeprtr;
            string clearjs = "";
            for (int i = 0; i < dtparam.Rows.Count; i++)
            {
                string FieldNm = dtparam.Rows[i]["FIELDNM"].ToString();
                string FieldDesc = dtparam.Rows[i]["FIELDDESC"].ToString();
                string FieldType = dtparam.Rows[i]["FIELDTYPE"].ToString();
                bool FieldKey = false;
                if (dtparam.Rows[i]["FIELDKEY"].ToString() != "")
                    FieldKey = (bool)dtparam.Rows[i]["FIELDKEY"];
                bool FieldMand = false;
                if (dtparam.Rows[i]["FIELDMAN"].ToString() != "")
                    FieldMand = (bool)dtparam.Rows[i]["FIELDMAN"];
                bool isAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim() != "";
                string FieldAuto = dtparam.Rows[i]["FIELDAUTO"].ToString().Trim();
                bool locked = false;
                if (dtparam.Rows[i]["LOCKED"].ToString() != "")
                    locked = (bool)dtparam.Rows[i]["LOCKED"];
                locked = locked || isAuto;

                TableFilterRow = new TableRow();
                TableInput.Rows.Add(TableFilterRow);
                if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Guid" || isAuto)
                    TableFilterRow.Style["display"] = "none";
                TableFilterDesc = new TableCell();
                TableFilterSeprtr = new TableCell();
                TableFilterControl = new TableCell();
                TableFilterRow.Cells.Add(TableFilterDesc);
                TableFilterRow.Cells.Add(TableFilterSeprtr);
                TableFilterRow.Cells.Add(TableFilterControl);

                if (!IsPostBack && !Page.IsCallback)
                {
                    TableFilterDesc.Text = FieldDesc;
                    TableFilterDesc.CssClass = "B01";

                    TableFilterSeprtr.Text = ":";
                    TableFilterSeprtr.CssClass = "BS";

                    TableFilterControl.CssClass = "B11";
                }
                WebControl oCtrl;
                string devexpresseditorlib = this.Page.MapPath("~/bin") + "\\DevExpress.Web.v17.1.dll";
                string dmscontrollib = this.Page.MapPath("~/bin") + "\\DMSControls.dll";
                if (FieldType == "")
                {
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Boolean")
                        FieldType = "chk";
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.DateTime")
                        FieldType = "date";
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int16" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int32" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Int64")
                        FieldType = "int";
                    if (dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Double" ||
                        dtparamschema.Columns[FieldNm].DataType.ToString() == "System.Decimal")
                        FieldType = "float";
                }
                string initvalue = "";
                if (FieldAuto.StartsWith("q:"))
                    initvalue = Request.QueryString[FieldAuto.Substring(2)];
                else if (FieldAuto.StartsWith("s:"))
                    initvalue = Session[FieldAuto.Substring(2)].ToString();
                switch (FieldType.ToLower())
                {
                    case "auto":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, "System.Web.UI.WebControls", "System.Web.UI.WebControls.TextBox");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + _autogen + "';";
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        break;
                    case "float":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, dmscontrollib, "DMSControls.TXT_CURRENCY");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        break;
                    case "int":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, dmscontrollib, "DMSControls.TXT_NUMBER");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        break;
                    case "date":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, devexpresseditorlib, "DevExpress.Web.ASPxDateEdit");
                        (oCtrl as DevExpress.Web.ASPxDateEdit).ClientInstanceName = oCtrl.ClientID;
                        clearjs += oCtrl.ClientID + ".SetDate(null);";
                        break;
                    case "chk":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, "System.Web.UI.WebControls", "System.Web.UI.WebControls.CheckBox");
                        if (FieldNm == "ACTIVE")
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').checked = true;";
                        else
                            clearjs += "document.getElementById('" + oCtrl.ClientID + "').checked = false;";
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').disabled = false;";
                        break;
                    case "ddl":
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, devexpresseditorlib, "DevExpress.Web.ASPxComboBox");
                        (oCtrl as DevExpress.Web.ASPxComboBox).ClientInstanceName = oCtrl.ClientID;
                        (oCtrl as DevExpress.Web.ASPxComboBox).Callback += new DevExpress.Web.CallbackEventHandlerBase(dxComboBox_Callback);
                        (oCtrl as DevExpress.Web.ASPxComboBox).EnableIncrementalFiltering = true;
                        (oCtrl as DevExpress.Web.ASPxComboBox).CallbackPageSize = 20;
                        (oCtrl as DevExpress.Web.ASPxComboBox).EnableCallbackMode = true;
                        clearjs += oCtrl.ClientID + ".SetValue(null);";
                        clearjs += oCtrl.ClientID + ".PerformCallback('r');";
                        break;
                    default:
                        oCtrl = (WebControl)Reflection.CreateControl(TableFilterControl, FieldNm, "System.Web.UI.WebControls", "System.Web.UI.WebControls.TextBox");
                        clearjs += "document.getElementById('" + oCtrl.ClientID + "').value = '" + initvalue + "';";
                        break;
                }

                if (initvalue != "")
                    staticFramework.retrieve(initvalue, oCtrl);

                if ((FieldKey || FieldMand) && FieldType != "a")
                {
                    oCtrl.CssClass = "mandatory";
                }

                if (FieldKey)
                {
                    HtmlInputHidden hCtrl = (HtmlInputHidden)Reflection.CreateControl(TableFilterControl, "h_" + FieldNm, "System.Web.UI.WebControls", "System.Web.UI.HtmlControls.HtmlInputHidden");
                    clearjs += "document.getElementById('" + hCtrl.ClientID + "').value = '" + initvalue + "';";
                }

                if (locked)
                {
                    oCtrl.Enabled = false;
                    HtmlInputHidden lCtrl = (HtmlInputHidden)Reflection.CreateControl(TableFilterControl, "l_" + FieldNm, "System.Web.UI.WebControls", "System.Web.UI.HtmlControls.HtmlInputHidden");
                }
            }
            return clearjs;
        }

        protected void reffcascade(ASPxComboBox ctrl, string strreff)
        {
            string[] value = strreff.Split(new string[] { ":[" }, StringSplitOptions.None);
            string query = value[0];
            for (int j = 1; j < value.Length; j++)
            {
                string valuename = value[j].Substring(0, value[j].IndexOf("]"));
                string valuepad = value[j].Substring(value[j].IndexOf("]") + 1);
                WebControl oCtrl = (WebControl)this.FindControl(valuename);                 //callback invoker control
                if (!IsPostBack && !Page.IsCallback)
                {
                    if (oCtrl is ASPxComboBox)
                    {
                        if ((oCtrl as ASPxComboBox).ClientSideEvents.ValueChanged == "")
                            (oCtrl as ASPxComboBox).ClientSideEvents.ValueChanged = "function(s,e){}";
                        (oCtrl as ASPxComboBox).ClientSideEvents.ValueChanged =
                              (oCtrl as ASPxComboBox).ClientSideEvents.ValueChanged.Substring(0, (oCtrl as ASPxComboBox).ClientSideEvents.ValueChanged.Length - 1) +
                              ctrl.ClientID + ".PerformCallback('rc');}";
                        //(oCtrl as ASPxComboBox).ClientSideEvents.EndCallback = (oCtrl as ASPxComboBox).ClientSideEvents.ValueChanged;
                    }
                    else if (oCtrl is WebControl)
                    {
                        WebControl wc = (WebControl)oCtrl;
                        wc.Attributes["onchange"] += ";" + ctrl.ClientID + ".PerformCallback('rc')";
                    }
                }
                query += staticFramework.toSql(staticFramework.getvalue(oCtrl)) + valuepad;
            }
            staticFramework.reff(ctrl, query, null, conn);
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            string modid = "61";
            if (Request.QueryString["moduleid"] != null && Request.QueryString["moduleid"].Trim() != "")
                modid = Request.QueryString["moduleid"];
            conn = new DbConnection(MNTTools.GetConnString(modid));
            TableNm = Request.QueryString["TBLNM"];

            object[] param = new object[] { TableNm };

            dtparam = conn.GetDataTable(Q_PARAMSYS, param, dbtimeout);
            dtparamschema = conn.GetDataTable("SELECT TOP 0 * FROM " + TableNm, null, dbtimeout, true, true);

            clrButton.Attributes["onclick"] = createcontrol();

            creategridquery();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !Page.IsCallback)
                initcontrol();
        }
        protected void dxComboBox_Callback(object source, DevExpress.Web.CallbackEventArgsBase e)
        {
            ASPxComboBox oCtrl = (source as ASPxComboBox);
            DataView dv = new DataView(dtparam, "FIELDNM=" + staticFramework.toSql(oCtrl.ID), null, DataViewRowState.OriginalRows);
            if (e.Parameter == "rc")
                reffcascade(oCtrl, dv[0]["FIELDREFF"].ToString());
        }
    }
}