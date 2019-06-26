using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Xml;
using System.Collections.Specialized;
using My.Tools;


namespace MikroMnt
{
    public class MNTTools
    {
        private static string Q_MODULECONNALL = "select db_ip, db_nama, db_loginid, db_loginpwd, moduleid from rfmodule ";
        private static string Q_MODULEDB = "select db_ip, db_nama, db_loginid, db_loginpwd " +
            "from rfmodule where moduleid = @1 ";

        #region group query
        private static string Q_PENDINGGROUPDATA = "select GROUPID, SG_GRPNAME, SG_GRPUPLINER, CH_STA,LOAD_UNIT,APPROVAL_GROUP " +
            "from PENDING_SCALLGROUP WHERE GROUPID = @1";
        private static string SP_APPRGRPDELETE = "exec SU_SCALLGROUP_DELETE @1, NULL, NULL, '0', @2 ";
        private static string SP_APPRALLGROUP = "exec SU_SCALLGROUP_APPROVE @1, @2 ";
        private static string SP_APPRLOCGROUP = "exec SU_SCGROUP @1, @2, @3, @4, @5, @6 ";
        #endregion

        #region menu query
        private static string Q_GRPMENUHEADING = "select menucode, menudisplay from vw_grpaccessmenuall " +
            "where moduleid = @1 and groupid = @2 and menulevel = '0' order by menucode ";
        private static string Q_GRPMENUDETAIL = "select menucode, menudisplay, tm_linkname, tm_parsingparam from vw_grpaccessmenuall " +
            "where moduleid = @1 and groupid = @2 and menuparentid = @3 order by menucode ";
        #endregion

        #region user query
        private static string SP_USP_MODULE_USER = "EXEC USP_MODULE_USER @1, @2, @3, @4, @5, @6, @7, @8, @9, @10, @11";

        private static string Q_GRPMODULE = "select MODULEID from GRPACCESSMODULE where GROUPID = @1 ";
        private static string Q_PENDINGUSERDATA = "SELECT PSU.USERID, PSU.GROUPID, PSU.SU_FULLNAME, PSU.SU_PWD, " +
            "PSU.SU_HPNUM, PSU.SU_EMAIL, PSU.SU_UPLINER, PSU.SU_NIP, PSU.SU_REGISTERDATE, PSU.SU_REGISTERBY, " +
            "PSU.SU_APPROVEBY, PSU.SU_PWDEXPDATE, PSU.SU_ACTIVE, PSU.SU_REVOKE, PSU.CH_STA, " +
            "MU.BRANCH_CODE, MU.RO_CODE, MU.UNIT_CODE, MU.LOADQUOTA, MU.MANUALFWD " +
            "FROM PENDING_SCALLUSER PSU LEFT JOIN PENDING_MODULEUSER MU ON PSU.USERID = MU.USERID " +
            "WHERE PSU.USERID = @1 AND MU.MODULEID = @2 ";
        private static string SP_APPRUSRDELETE = "exec SU_SCALLUSER_DELETE @1, NULL, '0', NULL, NULL, @2 ";
        private static string SP_APPRUSRUNDELETE = "exec SU_SCALLUSER_UNDELETE @1, NULL, '0', NULL, NULL, @2 ";
        private static string SP_APPRALLUSER = "exec SU_SCALLUSER_APPROVE @1, @2 ";
        private static string SP_APPRLOCUSER = "exec SU_SCUSER @1";
        #endregion

        #region General Function

        public static string getConnStringLogin()
        {

            //Encryptor enc = new Encryptor();
            //ApplicationRegistryHandler reg = new ApplicationRegistryHandler();
            //string appName = ConfigurationSettings.AppSettings["appName"].ToString();
            //appName = appName.Replace(" ", "");
            //string uobikey = ConfigurationSettings.AppSettings["UOBIKey"].ToString();
            //string DBConfig = ConfigurationSettings.AppSettings["DBConfig"].ToString();
            //string keyReg = reg.ReadFromRegistry("Software\\" + appName, "Key");

            //string connDetail = enc.Decrypt(DBConfig, keyReg, uobikey);
            //string[] connDetails = connDetail.Split(';');
            //string dbhost = connDetails[1].Substring(connDetails[1].IndexOf("=") + 1);
            //string dbname = connDetails[2].Substring(connDetails[2].IndexOf("=") + 1);
            //string dbuser = connDetails[3].Substring(connDetails[3].IndexOf("=") + 1);
            //string dbpwd = connDetails[4].Substring(connDetails[4].IndexOf("=") + 1);
            //string connstring = "Data Source=" + dbhost + ";Initial Catalog=" + dbname + ";uid=" + dbuser + ";pwd=" + dbpwd + ";Pooling=true";

            string connstring = ConfigurationSettings.AppSettings["connString"].ToString();
            return connstring;

        }

        public static string getConnStringModule()
        {

            //Encryptor enc = new Encryptor();
            //ApplicationRegistryHandler reg = new ApplicationRegistryHandler();
            //string appName = ConfigurationSettings.AppSettings["appName"].ToString();
            //appName = appName.Replace(" ", "");
            //string uobikey = ConfigurationSettings.AppSettings["UOBIKeyModule"].ToString();
            //string DBConfig = ConfigurationSettings.AppSettings["DBConfigModule"].ToString();
            //string keyReg = reg.ReadFromRegistry("Software\\" + appName, "Key");

            //string connDetail = enc.Decrypt(DBConfig, keyReg, uobikey);
            //string[] connDetails = connDetail.Split(';');
            //string dbhost = connDetails[1].Substring(connDetails[1].IndexOf("=") + 1);
            //string dbname = connDetails[2].Substring(connDetails[2].IndexOf("=") + 1);
            //string dbuser = connDetails[3].Substring(connDetails[3].IndexOf("=") + 1);
            //string dbpwd = connDetails[4].Substring(connDetails[4].IndexOf("=") + 1);
            //string connstring = "Data Source=" + dbhost + ";Initial Catalog=" + dbname + ";uid=" + dbuser + ";pwd=" + dbpwd + ";Pooling=true";

            string connstring = ConfigurationSettings.AppSettings["connStringModule"].ToString();
            return connstring;

        }

        public static string GetConnString(string moduleid)
        {
            string modconnstr = null;
                //, connstr = getConnStringLogin();
            //int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            //using (DbConnection conn = new DbConnection(connstr))
            //{
            //    object[] parmod = new object[1] { moduleid };
            //    conn.ExecReader(Q_MODULEDB, parmod, timeout);
            //    if (conn.hasRow())
            //    {
            //        modconnstr = "Data Source=" + conn.GetFieldValue(0) + ";Initial Catalog=" + conn.GetFieldValue(1) +
            //            ";uid=" + conn.GetFieldValue(2) + ";pwd=" + conn.GetFieldValue(3) + ";Pooling=true";
            //    }
            //}
            if (moduleid=="login")
                modconnstr = getConnStringLogin();
            else
                modconnstr = getConnStringModule();

            return modconnstr;
        }

        public static void LogError(System.Web.UI.Page source, string user, Exception ex)
        {
        }

        public static void LogError(string source, string user, Exception ex)
        {
        }
        #endregion

        #region group methods
        public static void ApproveGroup(string groupid, string moduleIDs, string aprby)
        {
            string dbconnstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            DbConnection dbdata = new DbConnection(dbconnstr);
            object[] pardata = new object[1] { groupid };
            dbdata.ExecReader(Q_PENDINGGROUPDATA, pardata, timeout);
            if (!dbdata.hasRow())
            {
                dbdata.Dispose();
                return;
            }

            #region each module

            using (DbConnection dball = new DbConnection(dbconnstr))
            {
                dball.ExecReader(Q_MODULECONNALL, null, timeout);

                // Update all modules for the approved group 
                //while (dball.hasRow())
                if (dball.hasRow())
                {
                    string moduleid = dball.GetFieldValue(4)
                        //,locconstr = "Data Source=" + dball.GetFieldValue(0) + ";Initial Catalog=" + dball.GetFieldValue(1) +
                        //";uid=" + dball.GetFieldValue(2) + ";pwd=" + dball.GetFieldValue(3) + ";Pooling=true";
                    , locconstr = getConnStringModule();

                    if (moduleid != "" && locconstr != "")
                    {
                        using (DbConnection locconn = new DbConnection(locconstr))
                        {
                            object[] parmoddata = new object[6]
                            {
                                dbdata.GetFieldValue("groupid"), 
                                dbdata.GetFieldValue("sg_grpname"), 
                                dbdata.GetFieldValue("sg_grpupliner"), 
                                dbdata.GetFieldValue("ch_sta"), 
                                dbdata.GetFieldValue("approval_group"), 
                                moduleid
                            };
                            if (moduleIDs.IndexOf(moduleid + ";") >= 0)			//new group has access to current module 
                            {
                                locconn.ExecuteNonQuery(SP_APPRLOCGROUP, parmoddata, timeout);
                            }
                            else												//new group doesn't has access to current module.. remove group from module.. 
                            {
                                parmoddata[3] = "2";		//set ch_sta delete 
                                locconn.ExecuteNonQuery(SP_APPRLOCGROUP, parmoddata, timeout);
                            }
                        }
                    }
                }
            }
            #endregion

            // Update scallgroup table
            pardata = new object[2] { groupid, aprby };
            dbdata.ExecuteNonQuery(SP_APPRALLGROUP, pardata, timeout);
            dbdata.Dispose();
        }

        public static void RemoveGroup(string groupid, string aprby)
        {
            string dbconnstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            DbConnection dball = new DbConnection(dbconnstr);

            #region each module
            dball.ExecReader(Q_MODULECONNALL, null, timeout);

            // Update all modules for the approved to remove group 
            //while (dball.hasRow())
            if (dball.hasRow())
            {
                string moduleid = dball.GetFieldValue(4)
                    //,locconstr = "Data Source=" + dball.GetFieldValue(0) + ";Initial Catalog=" + dball.GetFieldValue(1) +
                    //";uid=" + dball.GetFieldValue(2) + ";pwd=" + dball.GetFieldValue(3) + ";Pooling=true";
                , locconstr = getConnStringModule();

                if (moduleid != "" && locconstr != "")
                {
                    using (DbConnection locconn = new DbConnection(locconstr))
                    {
                        object[] parmoddata = new object[6] { groupid, null, null, "2", null, moduleid };
                        locconn.ExecuteNonQuery(SP_APPRLOCGROUP, parmoddata, timeout);
                    }
                }
            }
            #endregion

            // Update scallgroup table
            object[] pardel = new object[2] { groupid, aprby };
            dball.ExecuteNonQuery(SP_APPRGRPDELETE, pardel, timeout);
            dball.Dispose();
        }

        #endregion

        #region menu methods
        public static string GenMenuData(string moduleid, string groupid)
        {
            string connstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            string xmlresult;
            using (StringWriter result = new StringWriter())
            {
                XmlTextWriter writer = new XmlTextWriter(result);
                writer.Formatting = Formatting.Indented;

                writer.WriteStartElement("MenuData");
                writer.WriteAttributeString("ImagesBaseURL", "image/menu/");
                writer.WriteAttributeString("DefaultGroupCssClass", "MenuGroup");
                writer.WriteAttributeString("DefaultItemCssClass", "MenuItem");
                writer.WriteAttributeString("DefaultItemCssClassOver", "MenuItemOver");
                writer.WriteAttributeString("DefaultItemCssClassDown", "MenuItemDown");

                //submodule menu
                using (DbConnection connhead = new DbConnection(connstr))
                {
                    object[] parhead = new object[2] { moduleid, groupid };
                    connhead.ExecReader(Q_GRPMENUHEADING, parhead, timeout);
                    writer.WriteStartElement("MenuGroup");		//start MenuGroup head
                    while (connhead.hasRow())
                    {
                        string submodid = connhead.GetFieldValue(0);
                        writer.WriteStartElement("MenuItem");	//start MenuItem sub
                        writer.WriteAttributeString("Label", connhead.GetFieldValue(1));
                        writer.WriteAttributeString("LeftIcon", "bullet.gif");
                        writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                        writer.WriteAttributeString("LeftIconWidth", "20");
                        writer.WriteAttributeString("LeftIconHeight", "12");

                        writer.WriteStartElement("MenuGroup");		//start MenuGroup sub
                        using (DbConnection connparent = new DbConnection(connstr))
                        {
                            object[] parparent = new object[3] { moduleid, groupid, submodid };
                            connparent.ExecReader(Q_GRPMENUDETAIL, parparent, timeout);
                            while (connparent.hasRow())
                            {
                                string mncode = connparent.GetFieldValue(0),
                                    mndisp = connparent.GetFieldValue(1),
                                    mnlink = connparent.GetFieldValue(2),
                                    mnlinkparam = connparent.GetFieldValue(3);
                                writer.WriteStartElement("MenuItem");
                                writer.WriteAttributeString("Label", mndisp);
                                writer.WriteAttributeString("LeftIcon", "bullet.gif");
                                writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                                writer.WriteAttributeString("LeftIconWidth", "20");
                                writer.WriteAttributeString("LeftIconHeight", "12");
                                if (mnlink != "")
                                    writer.WriteAttributeString("URL", mnlink + mnlinkparam);

                                GenMenuChild(writer, moduleid, groupid, mncode, connstr, timeout);	//gen child if any

                                writer.WriteEndElement();
                            }
                        }
                        writer.WriteEndElement();		//end MenuGroup sub
                        writer.WriteEndElement();		//end MenuItem sub
                    }
                    // always add logout menu 
                    writer.WriteStartElement("MenuItem");
                    writer.WriteAttributeString("Label", "Logout");
                    writer.WriteAttributeString("LeftIcon", "bullet.gif");
                    writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                    writer.WriteAttributeString("LeftIconWidth", "20");
                    writer.WriteAttributeString("LeftIconHeight", "12");
                    writer.WriteAttributeString("URL", "Logout.aspx");
                    writer.WriteAttributeString("URLTarget", "_top");
                    writer.WriteEndElement();
                    writer.WriteEndElement();		//end MenuGroup head
                }

                writer.WriteEndElement();		//end MenuData

                xmlresult = result.ToString();
            }

            return xmlresult;
        }

        private static void GenMenuChild(XmlTextWriter writer, string moduleid, string groupid, string menuparent, string connstr, int timeout)
        {
            bool haschild = false, firstloop = true;
            using (DbConnection connchild = new DbConnection(connstr))
            {
                object[] parchild = new object[3] { moduleid, groupid, menuparent };
                connchild.ExecReader(Q_GRPMENUDETAIL, parchild, timeout);
                while (connchild.hasRow())
                {
                    if (firstloop)
                    {
                        writer.WriteAttributeString("RightIcon", "arrow.gif");
                        writer.WriteAttributeString("RightconWidth", "17");
                        writer.WriteStartElement("MenuGroup");		//start MenuGroup child
                        firstloop = false;
                        haschild = true;
                    }
                    string mncode = connchild.GetFieldValue(0),
                        mndisp = connchild.GetFieldValue(1),
                        mnlink = connchild.GetFieldValue(2),
                        mnlinkparam = connchild.GetFieldValue(3);
                    writer.WriteStartElement("MenuItem");
                    writer.WriteAttributeString("Label", mndisp);
                    writer.WriteAttributeString("LeftIcon", "bullet.gif");
                    writer.WriteAttributeString("LeftIconOver", "bullet_over.gif");
                    writer.WriteAttributeString("LeftIconWidth", "20");
                    writer.WriteAttributeString("LeftIconHeight", "12");
                    if (mnlink != "")
                        writer.WriteAttributeString("URL", mnlink + mnlinkparam);

                    GenMenuChild(writer, moduleid, groupid, mncode, connstr, timeout);		//gen child if any

                    writer.WriteEndElement();
                }
                if (haschild)
                    writer.WriteEndElement();		//end MenuGroup child
            }
        }

        #endregion

        #region user methods

        public static void InsertModuleUser(string userid, string moduleid)
        {
            string dbconnstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            using (DbConnection conn = new DbConnection(dbconnstr))
            {
                object[] pardata = new object[11]
					{
						userid,
						moduleid,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
                conn.ExecuteNonQuery(SP_USP_MODULE_USER, pardata, timeout);
            }
        }

        public static void ApproveUser(string userid, string groupid, string aprby)
        {
            string dbconnstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            string moduleIDs = "";

            //Get all module id form particular group id
            DbConnection dbdata = new DbConnection(dbconnstr);
            object[] pardata = new object[1] { groupid };
            dbdata.ExecReader(Q_GRPMODULE, pardata, timeout);
            while (dbdata.hasRow())
            {
                if (dbdata.GetFieldValue(0) != "")
                    moduleIDs += dbdata.GetFieldValue(0) + ";";
            }

            #region each module
            using (DbConnection dball = new DbConnection(dbconnstr))
            {
                //get all module
                dball.ExecReader(Q_MODULECONNALL, null, timeout);

                // Update all modules for the approved user 
                //while (dball.hasRow())
                if (dball.hasRow())
                {
                    //Create connection string for certain module
                    string moduleid = dball.GetFieldValue(4)
                        //,locconstr = "Data Source=" + dball.GetFieldValue(0) + ";Initial Catalog=" + dball.GetFieldValue(1) +
                        //";uid=" + dball.GetFieldValue(2) + ";pwd=" + dball.GetFieldValue(3) + ";Pooling=true";
                    , locconstr = getConnStringModule();

                    if (moduleid != "" && locconstr != "")
                    {
                        using (DbConnection locconn = new DbConnection(locconstr))
                        {
                            object[] parmoddata = new object[] { userid };
                            locconn.ExecuteNonQuery(SP_APPRLOCUSER, parmoddata, timeout);
                        }
                    }
                }
            }
            #endregion

            pardata = new object[2] { userid, aprby };
            dbdata.ExecuteNonQuery(SP_APPRALLUSER, pardata, timeout);
            dbdata.Dispose();
        }

        public static void RemoveUser(string userid, string groupid, string aprby)
        {
            string dbconnstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            string moduleIDs = "";
            DbConnection dbdata = new DbConnection(dbconnstr);
            object[] pardata = new object[1] { groupid };
            dbdata.ExecReader(Q_GRPMODULE, pardata, timeout);
            while (dbdata.hasRow())
            {
                if (dbdata.GetFieldValue(0) != "")
                    moduleIDs += dbdata.GetFieldValue(0) + ";";
            }
            #region each module

            //using (DbConnection dball = new DbConnection(dbconnstr))
            //{
            //    dball.ExecReader(Q_MODULECONNALL, null, timeout);

            //    // Update all modules for the approved user 
            //    while (dball.hasRow())
            //    {
            //        string moduleid = dball.GetFieldValue(4),
            //            locconstr = "Data Source=" + dball.GetFieldValue(0) + ";Initial Catalog=" + dball.GetFieldValue(1) +
            //            ";uid=" + dball.GetFieldValue(2) + ";pwd=" + dball.GetFieldValue(3) + ";Pooling=true";

            //        if (moduleid != "" && locconstr != "")
            //        {
            //            using (DbConnection locconn = new DbConnection(locconstr))
            //            {
            //                if (moduleIDs.IndexOf(moduleid + ";") >= 0)			//new user has access to current module 
            //                {
            //                    object[] parmoddata = new object[8]
            //                    {
            //                        userid, null, null, null, null, null, "2", moduleid
            //                    };
            //                    locconn.ExecuteNonQuery(SP_APPRLOCUSER, parmoddata, timeout);
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            // Update scalluser table
            pardata = new object[2] { userid, aprby };
            dbdata.ExecuteNonQuery(SP_APPRUSRDELETE, pardata, timeout);
            dbdata.Dispose();
        }

        public static void RestoreUser(string userid, string groupid, string aprby)
        {
            string dbconnstr = getConnStringLogin();
            int timeout = int.Parse(ConfigurationSettings.AppSettings["dbTimeOut"]);
            string moduleIDs = "";
            DbConnection dbdata = new DbConnection(dbconnstr);
            object[] pardata = new object[1] { groupid };
            dbdata.ExecReader(Q_GRPMODULE, pardata, timeout);
            while (dbdata.hasRow())
            {
                if (dbdata.GetFieldValue(0) != "")
                    moduleIDs += dbdata.GetFieldValue(0) + ";";
            }
            #region each module

            //using (DbConnection dball = new DbConnection(dbconnstr))
            //{
            //    dball.ExecReader(Q_MODULECONNALL, null, timeout);

            //    // Update all modules for the approved user 
            //    while (dball.hasRow())
            //    {
            //        string moduleid = dball.GetFieldValue(4),
            //            locconstr = "Data Source=" + dball.GetFieldValue(0) + ";Initial Catalog=" + dball.GetFieldValue(1) +
            //            ";uid=" + dball.GetFieldValue(2) + ";pwd=" + dball.GetFieldValue(3) + ";Pooling=true";

            //        if (moduleid != "" && locconstr != "")
            //        {
            //            using (DbConnection locconn = new DbConnection(locconstr))
            //            {
            //                if (moduleIDs.IndexOf(moduleid + ";") >= 0)			//user has access to current module 
            //                {
            //                    object[] parmoddata = new object[8]
            //                    {
            //                        userid, null, null, null, null, null, "3", moduleid
            //                    };
            //                    locconn.ExecuteNonQuery(SP_APPRLOCUSER, parmoddata, timeout);
            //                }
            //            }
            //        }
            //    }
            //}
            #endregion

            // Update scalluser table
            pardata = new object[2] { userid, aprby };
            dbdata.ExecuteNonQuery(SP_APPRUSRUNDELETE, pardata, timeout);
            dbdata.Dispose();
        }

        #endregion
    }
}
