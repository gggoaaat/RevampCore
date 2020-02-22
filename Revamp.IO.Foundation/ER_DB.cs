using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Threading;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Configuration;
using Revamp.IO.Foundation;
using Revamp.IO.DB.Bridge;
using Revamp.IO.Structs;

namespace Revamp.IO.Foundation
{
    //public class ConnectToDB
    //{
    //    public string Platform { get; set; }
    //    public string DBConnString { get; set; }
    //    public string SourceDBOwner { get; set; }
    //    public string ConnServer { get; set; }
    //    public string Password { get; set; }
    //}

    public class ER_DB
    {

        public List<string> DROP_ALL(DropDBStruct ERStruct)
        {
            List<string> Logger = new List<string>();

            DROP_ALL((IConnectToDB)new ConnectToDB { 
                Platform = ERStruct.DB_PLATFORM, 
                DBConnString = ERStruct.connAuth, 
                SourceDBOwner = ERStruct.SystemName });

            return Logger;
        }

        public List<CommandResult> DROP_ALL(IConnectToDB _Connect)
        {
            List<CommandResult> Logger = new List<CommandResult>();


            switch (_Connect.Platform.ToUpper())
            { 
                case "MICROSOFT":
                    Logger.Add(DROP_MSSQL_DB(_Connect, _Connect.SourceDBOwner));
                    Logger.Add(DROP_MSSQL_USER(_Connect, _Connect.SourceDBOwner));
                    string sqlIn = "BEGIN " +
                        "DECLARE @SYSTEM_NM varchar(max) = '{0}' " +
                        "DELETE FROM [ReVampNucleus].[CSA].[ER_OWNER_SEEDS] " +
                        "WHERE SYSTEM_NAME = @SYSTEM_NM " +
                        "END";

                    if (!Logger.Exists(x => x._Successful == false))
                    {
                        sqlIn = String.Format(sqlIn, _Connect.SourceDBOwner);

                        ER_Query er_query = new ER_Query();
                        CommandResult _result = new CommandResult();
                        _Connect.SourceDBOwner = "RevampNucleus";
                        string SuccessMessage = "Dropped DB Successfully issued";
                        _result._Response = er_query.RUN_NON_QUERY(_Connect, sqlIn, SuccessMessage);
                        _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
                        _result._EndTime = DateTime.Now;
                        Logger.Add(_result); 
                    }

                    break;
            }
            return Logger;
        }

        //MICROSOFT STRUCTURE METHODS

        //   

        public CommandResult ADD_MSSQL_DB(IConnectToDB _Connect, string Name)
        {
            CommandResult _result = new CommandResult();

            //Create Database in SqlServer
            try
            {
                String connectionString = _Connect.DBConnString;

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                ServerConnection conn = new ServerConnection(sqlConnection);
                Server srv = new Server(conn);


                //Connect to the local, default instance of SQL Server. 
                // Server srv = new Server);
                //Define a Database object variable by supplying the server and the database name arguments in the constructor. 
                Database db;
                db = new Database(srv, Name);
                //Create the database on the instance of SQL Server. 
                db.Create();
                //db.SetOwner(Name);
                db.Alter();
                //Reference the database and display the date when it was created. 
                //db = srv.Databases["Test_SMO_Database"];
                //Console.WriteLine(db.CreateDate);
                //Remove the database. 
                //db.Drop();

                _result._Response = "SQL Server Database Created";
                _result._Successful = true;
                
            }
            catch (Exception e)
            {
                _result._Response = e.ToString();
                _result._Successful = false;
                
            }

            _result._EndTime = DateTime.Now;
            return _result;

        }

        public CommandResult ADD_MSSQL_LOGIN(IConnectToDB _Connect, string LoginName, string Password, string DefaultDatabase)
        {
            CommandResult _result = new CommandResult();

            try
            {
                String connectionString = _Connect.DBConnString;

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                ServerConnection conn = new ServerConnection(sqlConnection);
                Server srv = new Server(conn);

                Login newLogin = new Login(srv, LoginName);
                newLogin.LoginType = Microsoft.SqlServer.Management.Smo.LoginType.SqlLogin;

                //newLogin.AddToRole("sysadmin");

                newLogin.DefaultDatabase = DefaultDatabase;

                newLogin.Create(Password);

                _result._Response = "SQL Server Login " + LoginName + " Created. The defaultdb is " + newLogin.DefaultDatabase.ToString();

                _result._Successful = true;
            }
            catch (Exception e)
            {
                _result._Response = e.ToString();
            }


            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult ADD_MSSQL_ROLE(IConnectToDB _Connect, string Name)
        {
            CommandResult _result = new CommandResult();

            ER_Tools er_tools = new ER_Tools();
            ER_Query er_query = new ER_Query();

            string tempstringNAME = er_tools.MaxNameLength(Name, 125) + "_RL";

            string SuccessMessage = "Role " + Name + " Created";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, "Create Role " + tempstringNAME, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;

            return _result;
        }

        public CommandResult ADD_MSSQL_USER(IConnectToDB _Connect, string UserName, string LoginName, string DatabaseName, string DefaultSchema)
        {

            CommandResult _result = new CommandResult();

            try
            {
                String connectionString = _Connect.DBConnString;

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                ServerConnection conn = new ServerConnection(sqlConnection);
                Server srv = new Server(conn);
                Database db = srv.Databases[DatabaseName];

                User newUser = new User(db, UserName);

                newUser.DefaultSchema = DefaultSchema;
                newUser.Login = LoginName;

                Thread.Sleep(5000);

                newUser.Create();

                DatabasePermissionSet perms = new DatabasePermissionSet();

                perms.Connect = true;
                perms.CreateTable = true;

                perms.Select = true;
                perms.Insert = true;
                perms.Delete = true;
                perms.Execute = true;
                perms.CreateSchema = true;
                perms.CreateRole = true;
                perms.CreateTable = true;
                perms.CreateProcedure = true;
                perms.CreateFunction = true;
                perms.Control = true;
                perms.TakeOwnership = true;


                db.Grant(perms, UserName);
               
                _result._Response = "SQL Server UserName " + UserName + " Created for Login " + LoginName + ". The defaultdb is " + DatabaseName;
                _result._Successful = true;
            }
            catch (Exception e)
            {
                _result._Response = e.ToString();
            }

            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult ADD_MSSQL_SCHEMA(IConnectToDB _Connect, string SchemaName, string SchemaOwner)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = "Schema Successfully created";
            //_result._Response = er_query.RUN_NON_QUERY(_Connect, "CREATE SCHEMA " + SchemaName + " AUTHORIZATION " + SchemaOwner, SuccessMessage);
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "CREATE SCHEMA " + SchemaName, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult GRANT_MSSQL_PRIVILEGE(IConnectToDB _Connect, string Privilege, string Grantee, string SourceDB)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = "Grant Successfully issued";
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "GRANT " + Privilege + " TO " + Grantee, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult ADD_MEMBER_TO_ROLE(IConnectToDB _Connect, string Role, string MemberName)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = MemberName + " added to role " + Role + " successfully";
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "EXEC sp_addrolemember '" + Role + "','" + MemberName + "'", SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult UPDATE_MSSQL_USER_DEFAULT_SCHEMA(IConnectToDB _Connect, string UserName, string DefaultSchema)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = UserName + " default schema changed to " + DefaultSchema + " successfully";
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "ALTER USER " + UserName + " with DEFAULT_SCHEMA =" + DefaultSchema + "", SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult UPDATE_MSSQL_LOGIN_DEFAULT_DB(IConnectToDB _Connect, string LoginName, string DefaultDatabase)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = LoginName + " default db changed to " + DefaultDatabase + " successfully" ;
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "ALTER LOGIN " + LoginName + " with DEFAULT_DATABASE  =" + DefaultDatabase + "", SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;

        }

        public CommandResult ALTER_AUTH_ON_MSSQL_SCHEMA(IConnectToDB _Connect, string Assignee, string SchemaName)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = Assignee + " has been authorized for Schema " + SchemaName + " successfully";
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "ALTER AUTHORIZATION ON SCHEMA::" + _Connect.Schema + "." + SchemaName + " TO " + Assignee, SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;
        }

        public CommandResult DROP_MSSQL_USER(IConnectToDB _Connect, string Name)
        {
            CommandResult _result = new CommandResult();
            try
            {
                String connectionString = _Connect.DBConnString;

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                ServerConnection conn = new ServerConnection(sqlConnection);
                Server svr = new Server(conn);

                Database db = svr.Databases["TESTDB1"];

                //Login login = new Login(svr, Name);
                //login.LoginType = LoginType.SqlLogin;
                //login.Create("password@1");

                //User user1 = new User(db, "User1");
                //user1.Login = "login1";
                //user1.Create(); 

                //User user1 = db.IDENTITIES["User1"];
                //user1.Drop();

                Login Login1 = svr.Logins[Name];
                Login1.Drop();


                _result._Response = "MSSQL User " + Name + " has been succesfully dropped";
                _result._Successful = true;
            }
            catch (Exception e)
            {
                _result._Response = e.ToString();
                _result._Successful = _result._Response.IndexOf("Object reference not set to an instance of an object") > -1 ? true : false;
            }

            return _result;
        }

        public CommandResult DROP_MSSQL_DB(IConnectToDB _Connect, string Name)
        {
            CommandResult _result = new CommandResult();
            //Drop Database in SqlServer
            try
            {
                String connectionString = _Connect.DBConnString;

                SqlConnection sqlConnection = new SqlConnection(connectionString);
                ServerConnection conn = new ServerConnection(sqlConnection);
                Server srv = new Server(conn);

                //Connect to the local, default instance of SQL Server. 
                // Server srv = new Server);
                //Define a Database object variable by supplying the server and the database name arguments in the constructor. 
                Database db;
                //db = new Database(srv, Name);
                srv.KillAllProcesses(Name);

                db = srv.Databases[Name];

                //Drop the database on the instance of SQL Server. 
                db.Drop();
                //Reference the database and display the date when it was created. 
                //db = srv.Databases["Test_SMO_Database"];
                //Console.WriteLine(db.CreateDate);
                //Remove the database. 
                //db.Drop();

                _result._Response = "SQL Server Database Dropped";
                _result._Successful = true;
            }
            catch (Exception e)
            {
                _result._Response = e.ToString();
                _result._Successful = _result._Response.IndexOf( "Object reference not set to an instance of an object") > -1 ? true : false;
            }

            return _result;
        }

        public CommandResult DROP_SCHEMA(IConnectToDB _Connect, string SchemaName)
        {
            CommandResult _result = new CommandResult();
            ER_Tools er_tools = new ER_Tools();
            ER_Query er_query = new ER_Query();
            ER_DML er_dml = new ER_DML();

            StringBuilder SQLBuffer = new StringBuilder();

            string _Schema = ER_DB.GetSchema(_Connect);

            SQLBuffer.Append("DROP SCHEMA " + SchemaName);

            string SuccessMessage = "Success " + SchemaName + " has been dropped.";

            _result._Response = er_query.RUN_NON_QUERY(_Connect, SQLBuffer.ToString(), SuccessMessage).ToString();
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;

            if (_result._Successful)
            {
                er_dml.DROP_Dictionary_View(_Connect, SchemaName);
            }
            
            return _result;
        }

        public CommandResult CREATE_CATALOG(IConnectToDB _Connect, string Name)
        {
            ER_Query er_query = new ER_Query();
            CommandResult _result = new CommandResult();
            string SuccessMessage = "Catalog " + Name + "_CATALOG Created";
            _result._Response = er_query.RUN_NON_QUERY(_Connect, "Create FullTEXT CATALOG " + Name + "_CATALOG", SuccessMessage);
            _result._Successful = _result._Response.IndexOf(SuccessMessage) > -1 ? true : false;
            _result._EndTime = DateTime.Now;
            return _result;
        }

        public static string GetSchema(IConnectToDB _Connect)
        {
            string _Schema = (_Connect.Schema == "" || _Connect.Schema == null ? (_Connect.SourceDBOwner == "" || _Connect.SourceDBOwner == null ? _Connect.RevampSystemName : _Connect.SourceDBOwner) : _Connect.Schema);
            return _Schema;
        }

    }
}