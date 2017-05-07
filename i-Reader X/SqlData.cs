using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace i_Reader_X
{
    class SqlData
    {
        public static ResourceManager Rm = new ResourceManager("i_Reader_X.Properties.Resources", Assembly.GetExecutingAssembly());
        public static readonly string ConStr = "server =" + Dns.GetHostName() + @"\sqlexpress;database=i-Reader_X;integrated security = true; min pool size=1;max pool size=100;Connection Lifetime = 30; Enlist=true"; //ConfigurationManager.ConnectionStrings["SQLConnString"].ConnectionString;
        public static readonly string ConStrMaster = ConStr.Replace("i-Reader_X", "master");

        public static DataTable SelectAllResult()
        //查询所有结果
        {
            var strsql = new StringBuilder();
            strsql.Append("select SampleNo from Resultlist");
            return
                ExecuteDataset(new SqlConnection(ConStr), CommandType.Text, strsql.ToString()).Tables[0
                    ];
        }
        public static DataTable SelectResultCount(string searchconditon)
        {
            var strsql = new StringBuilder();
            strsql.Append(
                "select count(*) from ResultList where");
            strsql.Append(searchconditon);
            return
                ExecuteDataset(new SqlConnection(ConStr), CommandType.Text, strsql.ToString()).Tables[0
                    ];
        }

        public static DataTable SelectResult(int page, int ResultNum, string searchcondition)
        //查询结果
        {
            var strsql = new StringBuilder();
            strsql.Append("select a.SampleNo, c.TestitemName, CONVERT(varchar(20),coNVERT(decimal(18,2),Result) )+ a.unit as result, a.CreateTime from resultlist a, TestItemInfo c,(select top ");
            strsql.Append(ResultNum);
            strsql.Append(" a.CreateTime from ResultList a,(select top ");
            strsql.Append(ResultNum * page);
            strsql.Append(" createtime from ResultList where");
            strsql.Append(searchcondition);
            strsql.Append("order by CreateTime asc) b where a.CreateTime = b.CreateTime order by CreateTime desc) b where a.CreateTime = b.CreateTime and c.TestItemID = a.TestItemID order by a.CreateTime asc");
            return
                ExecuteDataset(new SqlConnection(ConStr), CommandType.Text, strsql.ToString()).Tables[0
                    ];
        }

        public static void InsertNewResult(string SampleNo, string Createtime, string TestItem, string Result, string Unit, string CSVDataPath, string ImgDataPath, string TyFixStr, string CalibDataID, string ODdata, string flag)
            //向数据库中写入结果
        {
            StringBuilder str1 = new StringBuilder();
            str1.Append("INSERT INTO ResultList VALUES ('");
            str1.Append(SampleNo);
            str1.Append("', '");
            str1.Append(Createtime);
            str1.Append("','");
            str1.Append(TestItem);
            str1.Append("','");
            str1.Append(Result);
            str1.Append("','");
            str1.Append(Unit);
            str1.Append("','");
            str1.Append(CSVDataPath);
            str1.Append("','");
            str1.Append(ImgDataPath);
            str1.Append("','");
            str1.Append(TyFixStr);
            str1.Append("','");
            str1.Append(CalibDataID);
            str1.Append("','");
            str1.Append(ODdata);
            str1.Append("','");
            str1.Append(flag);
            str1.Append("')");
            ExecuteNonQuery(new SqlConnection(ConStr), CommandType.Text, str1.ToString());
        }

        //将定标信息插入到定标记录表
        public static int InsertIntoCalibdata(string cpmc, string lot, DateTime dtPdate, DateTime epdate, string funtype,
            double[] param)
        {
            //如果存在同样记录，则不插入
            var calibDataExist = new StringBuilder();
            calibDataExist.Append("select calibdataid from Calibdata where productid=");
            var insertCalibData = new StringBuilder();
            insertCalibData.Append("insert into calibdata values(");
            //产品名称 productid
            insertCalibData.Append(cpmc);
            calibDataExist.Append(cpmc);
            calibDataExist.Append(" and lotno=");
            insertCalibData.Append(",");
            //批号lot
            insertCalibData.Append(lot);
            insertCalibData.Append(",'");
            calibDataExist.Append(lot);
            calibDataExist.Append(" and productdate='");
            //生产日期
            insertCalibData.Append(dtPdate.ToString("yyyy-MM-dd"));
            insertCalibData.Append("','");
            calibDataExist.Append(dtPdate);
            calibDataExist.Append("' and expiredate='");
            //过期日期
            insertCalibData.Append(epdate.ToString("yyyy-MM-dd"));
            insertCalibData.Append("',");
            calibDataExist.Append(epdate);
            calibDataExist.Append("' and formulatypeid=");
            //公式类型
            insertCalibData.Append(funtype);
            insertCalibData.Append(",");
            calibDataExist.Append(funtype);
            var str = "ABCDEFG";
            for (var i = 0; i < 7; i++)
            {
                calibDataExist.Append(" and formulaparam");
                calibDataExist.Append(str[i]);
                calibDataExist.Append("=");
                calibDataExist.Append(param[i]);
                insertCalibData.Append(param[i].ToString(CultureInfo.InvariantCulture));
                insertCalibData.Append(i < 6 ? "," : ")");
            }
            var dt =
                ExecuteDataset(new SqlConnection(ConStr), CommandType.Text, calibDataExist.ToString())
                    .Tables[0];
            if (dt.Rows.Count > 0)
                return int.Parse(dt.Rows[0][0].ToString());
            return
                ExecuteNonQuery(new SqlConnection(ConStr), CommandType.Text, insertCalibData.ToString()) >
                0
                    ? 0
                    : -1;
        }
        public static void UpdateResult(string testitemname, string unit, string unitratio, string ratio,
            string accurancy, string productid, string testitemid)
        {
            var strsql = new StringBuilder();
            strsql.Append("update testiteminfo set testitemname='");
            strsql.Append(testitemname);
            strsql.Append("',unit='");
            strsql.Append(unit);
            strsql.Append("',unitratio=");
            strsql.Append(unitratio);
            strsql.Append(", ratio=");
            strsql.Append(ratio);
            strsql.Append(",accurancy=");
            strsql.Append(accurancy);
            strsql.Append(" where productid=");
            strsql.Append(productid);
            strsql.Append(" and testitemid=");
            strsql.Append(testitemid);
            ExecuteNonQuery(new SqlConnection(ConStr), CommandType.Text, strsql.ToString());
        }

        public static DataTable SelectLotNo (int ProductID)
        {
            var strsql = new StringBuilder();
            strsql.Append(
                "select LotNo from CalibData where productID = ");
            strsql.Append(ProductID);
            return
                ExecuteDataset(new SqlConnection(ConStr), CommandType.Text, strsql.ToString()).Tables[0
                    ];
        }

        //数据库使用必须函数
        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        private static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            using (var da = new SqlDataAdapter(cmd))
            {
                var ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                if (mustCloseConnection)
                    connection.Close();
                return ds;
            }
        }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            command.Connection = connection;
            command.CommandText = commandText;
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
                command.Transaction = transaction;
            }
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandParameters != null)
            {
                foreach (var p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        private static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            var cmd = new SqlCommand();
            bool mustCloseConnection;
            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, out mustCloseConnection);
            var retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }
    }
}
