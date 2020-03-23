using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Pocoor.Internal;

namespace Pocoor
{
    #region IDatabase
    //public interface IDatabase
    //{
    //    bool KeepConnectionAlive { get; set; }
    //    IDbConnection Connection { get; }

    //    void AdoCommand(Action<IDbCommand> action, Sql sql);
    //    void AdoCommand(Action<IDbCommand> action, string sql, params object[] args);
    //    T AdoCommand<T>(Func<IDbCommand, T> func, Sql sql);
    //    T AdoCommand<T>(Func<IDbCommand, T> func, string sql, params object[] args);
    //    void AdoReader(Action<IDataReader> action, Sql sql);
    //    void AdoReader(Action<IDataReader> action, string sql, params object[] args);
    //    IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, Sql sql);
    //    IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, string sql, params object[] args);
    //    void AdoReader(Action<IDataReader> action, long skip, long take, Sql sql);
    //    void AdoReader(Action<IDataReader> action, long skip, long take, string sql, params object[] args);
    //    IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, long skip, long take, Sql sql);
    //    IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, long skip, long take, string sql, params object[] args);

    //    int Execute(string sql, params object[] args);
    //    int Execute(Sql sql);

    //    object ExecuteScalar(string sql, params object[] args);
    //    object ExecuteScalar(Sql sql);
    //    T ExecuteScalar<T>(string sql, params object[] args);
    //    T ExecuteScalar<T>(Sql sql);

    //    IEnumerable<T> Query<T>(string sql, params object[] args);
    //    IEnumerable<T> Query<T>(Sql sql);
    //    IEnumerable<T> Query<T>(object poco, string columns = null);
    //    IEnumerable<T> Query<T>(long pageIndex, long pageSize, string sql, params object[] args);
    //    IEnumerable<T> Query<T>(long pageIndex, long pageSize, Sql sql);

    //    T Single<T>(string sql, params object[] args);
    //    T Single<T>(Sql sql);
    //    T Single<T>(object poco, string columns = null);
    //    T SingleOrDefault<T>(string sql, params object[] args);
    //    T SingleOrDefault<T>(Sql sql);
    //    T SingleOrDefault<T>(object poco, string columns = null);

    //    T First<T>(string sql, params object[] args);
    //    T First<T>(Sql sql);
    //    T First<T>(object poco, string columns = null);
    //    T FirstOrDefault<T>(string sql, params object[] args);
    //    T FirstOrDefault<T>(Sql sql);
    //    T FirstOrDefault<T>(object poco, string columns = null);

    //    bool Exists(string srcSql, params object[] srcArgs);
    //    bool Exists(Sql srcSql);
    //    bool Exists(object poco, string columns = null);
    //    bool Exists<T>(object poco, string columns = null);

    //    List<T> Fetch<T>(string sql, params object[] args);
    //    List<T> Fetch<T>(Sql sql);
    //    List<T> Fetch<T>(object poco, string columns = null);
    //    List<T> Fetch<T>(long pageIndex, long pageSize, string sql, params object[] args);
    //    List<T> Fetch<T>(long pageIndex, long pageSize, Sql sql);
    //    DataTable Fetch(string sql, params object[] args);
    //    DataTable Fetch(Sql sql);
    //    DataTable Fetch(object poco, string columns = null);
    //    DataTable Fetch(long pageIndex, long pageSize, string sql, params object[] args);
    //    DataTable Fetch(long pageIndex, long pageSize, Sql sql);
    //    DataSet Fetchs(string sql, params object[] args);
    //    DataSet Fetchs(Sql sql);

    //    Page<T> Page<T>(long pageIndex, long pageSize, string sql, params object[] args);
    //    Page<T> Page<T>(long pageIndex, long pageSize, Sql sql);
    //    DataSet Page(long pageIndex, long pageSize, string sql, params object[] args);
    //    DataSet Page(long pageIndex, long pageSize, Sql sql);

    //    int Insert(object poco, string excludeColumns = null, string includeColumns = null, string tableName = null);
    //    int Insert(IEnumerable pocoList, string excludeColumns = null, string includeColumns = null, string tableName = null);
    //    T Insert<T>(object poco, string excludeColumns = null, string autoIncrementColumn = null, string includeColumns = null, string tableName = null);

    //    int Update(object poco, string setColumns = null, string whereCondition = null, string notsetColumns = null, string tableName = null, params object[] whereConditionArgs);
    //    int Update(IEnumerable pocoList, string setColumns = null, string whereCondition = null, string notsetColumns = null, string tableName = null, params object[] whereConditionArgs);

    //    int Save(object poco, string autoIncrementColumn = null, string existsColumns = null, string excludeColumns = null, string includeColumns = null, string tableName = null);
    //    int Save(IEnumerable pocoList, string autoIncrementColumn = null, string existsColumns = null, string excludeColumns = null, string includeColumns = null, string tableName = null);
    //    T Save<T>(object poco, string autoIncrementColumn = null, string excludeColumns = null, string includeColumns = null, string tableName = null);

    //    int Delete<T>(object poco, string columns = null);
    //    int Delete(object poco, string columns = null);
    //    int Delete<T>(IEnumerable pocos, string columns = null);
    //    int Delete(IEnumerable pocos, string columns = null);
    //    int Delete<T>(string whereCondition, params object[] whereConditionArgs);



    //    #region Procedure
    //    void AdoCommandSp(Action<IDbCommand> action, string sp, params IDbDataParameter[] paras);
    //    T AdoCommandSp<T>(Func<IDbCommand, T> func, string sp, params IDbDataParameter[] paras);
    //    void AdoReaderSp(Action<IDataReader> action, string sp, params IDbDataParameter[] paras);
    //    IEnumerable<T> AdoReaderSp<T>(Func<IDataReader, T> func, string sp, params IDbDataParameter[] paras);

    //    int ExecuteSp(string sp, params IDbDataParameter[] paras);

    //    object ExecuteScalarSp(string sp, params IDbDataParameter[] paras);
    //    T ExecuteScalarSp<T>(string sp, params IDbDataParameter[] paras);

    //    IEnumerable<T> QuerySp<T>(string sp, params IDbDataParameter[] paras);

    //    T SingleSp<T>(string sp, params IDbDataParameter[] paras);
    //    T SingleOrDefaultSp<T>(string sp, params IDbDataParameter[] paras);

    //    T FirstSp<T>(string sp, params IDbDataParameter[] paras);
    //    T FirstOrDefaultSp<T>(string sp, params IDbDataParameter[] paras);

    //    List<T> FetchSp<T>(string sp, params IDbDataParameter[] paras);
    //    DataTable FetchSp(string sp, params IDbDataParameter[] paras);
    //    DataSet FetchsSp(string sp, params IDbDataParameter[] paras);

    //    Page<T> PageSp<T>(string sp, params IDbDataParameter[] paras);
    //    DataSet PageSp(string sp, params IDbDataParameter[] paras);
    //    #endregion
    //}
    #endregion
    #region Database
    public class Database : IDisposable
    {
        #region 字段
        //连接
        DatabaseType _Type;
        string _connectionString;
        IDbConnection _sharedConnection;
        int _sharedConnectionDepth;

        //事务
        IDbTransaction _transaction;
        int _transactionDepth;
        bool _transactionCancelled;

        //执行语句
        string _lastSql;
        object[] _lastArgs;
        #endregion
        #region CommandTimeout设置
        /// <summary>
        /// 设置所有 SQL 语句的超时值。
        /// </summary>
        public int CommandTimeout { get; set; }
        /// <summary>
        /// 设置下一个SQL 语句的超时值。（仅一次）
        /// </summary>
        public int OneTimeCommandTimeout { get; set; }
        #endregion
        #region 构造
        /// <summary>
        /// 连接字符串名称。当为null/Empty时，取系统第一个连接字符串。
        /// read from app/web.config.
        /// </summary>
        /// <param name="connectionStringName">连接字符串名称</param>
        public Database(string connectionStringName)
        {
            //获取连接字符串
            ConnectionStringSettings cs = null;
            if (string.IsNullOrWhiteSpace(connectionStringName))
            {
                if (ConfigurationManager.ConnectionStrings.Count < 1)
                    throw new InvalidOperationException("当前系统无默认连接字符串配置");
                else
                    cs = ConfigurationManager.ConnectionStrings[0];
            }
            else
            {
                cs = ConfigurationManager.ConnectionStrings[connectionStringName];
            }
            if (cs == null)
                throw new InvalidOperationException(string.Format("连接字符串 '{0}' 不存在", connectionStringName));


            _connectionString = cs.ConnectionString;
            _Type = new DatabaseType(cs.ConnectionString, cs.ProviderName);
        }
        /// <summary>
        /// 连接字符串和提供程序枚举。
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="providerEnum">提供程序枚举</param>
        public Database(string connectionString, DatabaseProviderEnum providerEnum)
        {
            _connectionString = connectionString;
            _Type = new DatabaseType(connectionString, providerEnum);
        }

        /// <summary>
        /// 连接字符串和提供程序名。提供程序名缺省为System.Data.SqlClient。
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="providerName">提供程序名</param>
        public Database(string connectionString, string providerName)
        {
            _connectionString = connectionString;
            _Type = new DatabaseType(connectionString, providerName);
        }
        /// <summary>
        /// 连接字符串和提供程序。
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="provider">提供程序</param>
        public Database(string connectionString, DbProviderFactory provider)
        {
            _connectionString = connectionString;
            _Type = new DatabaseType(connectionString, provider);
        }
        /// <summary>
        /// 连接对象。将不会closed/disposed，这将是调用方的责任。
        /// </summary>
        /// <param name="connection">连接对象</param>
        public Database(IDbConnection connection)
        {
            _sharedConnection = connection;
            _connectionString = connection.ConnectionString;
            _Type = new DatabaseType(connection);
            _sharedConnectionDepth = 2;		// 防止关闭外部连接
        }
        #endregion

        #region 数据库类型
        /// <summary>
        /// 获取 DbType
        /// </summary>
        public DatabaseType Type
        {
            get
            {
                return _Type;
            }
        }
        #endregion
        #region IDisposable
        /// <summary>
        /// 自动关闭一个打开的共享的连接
        /// </summary>
        public void Dispose()
        {
            // 自动关闭一个打开的共享的连接（当工作在KeepConnectionAlive状态和手动打开共享连接的情况下）
            CloseSharedConnection();
        }
        #endregion
        #region Connection管理
        /// <summary>
        /// 当设置为 true，第一次打开连接保持活动状态直到释放此对象
        /// </summary>
        public bool KeepConnectionAlive { get; set; }
        /// <summary>
        /// 打开将用于所有后续查询的连接。（对Open/CloseSharedConnection 的调用是引用计数的，必须保持平衡）
        /// </summary>
        public void OpenSharedConnection()
        {
            if (_sharedConnectionDepth == 0)
            {
                _sharedConnection = _Type.CreateConnection();
                _sharedConnection.ConnectionString = _connectionString;

                if (_sharedConnection.State == ConnectionState.Broken)
                    _sharedConnection.Close();

                if (_sharedConnection.State == ConnectionState.Closed)
                    _sharedConnection.Open();

                _sharedConnection = OnConnectionOpened(_sharedConnection);

                if (KeepConnectionAlive)
                    _sharedConnectionDepth++;		// 确保你调用Dispose
            }
            _sharedConnectionDepth++;
        }
        /// <summary>
        /// 释放共享连接
        /// </summary>
        public void CloseSharedConnection()
        {
            if (_sharedConnectionDepth > 0)
            {
                _sharedConnectionDepth--;
                if (_sharedConnectionDepth == 0)
                {
                    OnConnectionClosing(_sharedConnection);
                    _sharedConnection.Dispose();
                    _sharedConnection = null;
                }
            }
        }
        /// <summary>
        /// 提供对当前打开的共享连接的访问（或如果没有则为 null） 
        /// </summary>
        public IDbConnection Connection
        {
            get { return _sharedConnection; }
        }
        #endregion
        #region Transaction管理
        /// <summary>
        /// 开始或继续一个事务，用来创建一个事务范围。
        /// 事务可以嵌套，但是他们必须全部完成否则整个事务被中止。
        /// </summary>
        /// <returns>一个 ITransaction 引用， 该对象必须调用Complete或Dispose方法。</returns>
        /// <remarks>
        /// 使用方式:
        /// using (var tx = db.GetTransaction())
        /// {
        ///		// 数据库操作
        ///		db.Update(...);
        ///		
        ///     // 标记事务完成
        ///     tx.Complete();
        /// }
        /// </remarks>
        public ITransaction GetTransaction()
        {
            return new Transaction(this);
        }
        /// <summary>
        /// 开始一个事务范围。
        /// </summary>
        public void BeginTransaction()
        {
            _transactionDepth++;

            if (_transactionDepth == 1)
            {
                OpenSharedConnection();
                _transaction = _sharedConnection.BeginTransaction();
                _transactionCancelled = false;
                OnBeginTransaction();
            }
        }
        /// <summary>
        /// 标记当前事务为完成状态。
        /// </summary>
        public void CompleteTransaction()
        {
            if ((--_transactionDepth) == 0)
                CleanupTransaction();
        }
        /// <summary>
        /// 中止整个最外层事务范围。 
        /// 如果事务没有完成则由Transaction.Dispose()自动调用。
        /// </summary>
        public void AbortTransaction()
        {
            _transactionCancelled = true;
            if ((--_transactionDepth) == 0)
                CleanupTransaction();
        }
        /// <summary>
        /// 清除事务
        /// </summary>
        void CleanupTransaction()
        {
            OnEndTransaction();

            if (_transactionCancelled)
                _transaction.Rollback();
            else
                _transaction.Commit();

            _transaction.Dispose();
            _transaction = null;

            CloseSharedConnection();
        }

        /// <summary>
        /// 当事务开始时调用。
        /// </summary>
        public virtual void OnBeginTransaction()
        {
        }
        /// <summary>
        /// 当事务结束时调用
        /// </summary>
        public virtual void OnEndTransaction()
        {
        }
        #endregion
        #region 异常报告和日志
        /// <summary>
        /// 当发生异常时调用。重写以提供自定义的日志记录处理。
        /// </summary>
        /// <param name="ex">异常实例</param>
        /// <returns>True 重新引发异常, false 不引发</returns>
        public virtual bool OnException(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
            System.Diagnostics.Debug.WriteLine(LastCommand);
            return true;
        }
        /// <summary>
        /// 当打开连接时调用。重写以提供自定义日志记录，或者提供一个代理IDbConnection。
        /// </summary>
        /// <param name="conn">新打开的IDbConnection</param>
        /// <returns>同一个或替换后的IDbConnection</returns>
        public virtual IDbConnection OnConnectionOpened(IDbConnection conn)
        {
            return conn;
        }
        /// <summary>
        /// 当关闭连接时调用。重写以提供自定义日志记录。
        /// </summary>
        /// <param name="cnn">即将关闭的IDBConnection</param>
        public virtual void OnConnectionClosing(IDbConnection cnn)
        {
        }
        /// <summary>
        /// 当命令执行前调用。重写以提供自定义日志记录，或执行前的修改。
        /// </summary>
        /// <param name="cmd">即将执行的IDbCommand</param>
        public virtual void OnExecutingCommand(IDbCommand cmd)
        {
        }
        /// <summary>
        /// 当命令执行完成后调用。重写以提供自定义日志记录。
        /// </summary>
        /// <param name="cmd">执行完成的IDbCommand</param>
        public virtual void OnExecutedCommand(IDbCommand cmd)
        {
        }
        #endregion
        #region Last Command
        /// <summary>
        /// 检索上次执行的语句的 SQL。
        /// </summary>
        public string LastSQL
        {
            get { return _lastSql; }
        }
        /// <summary>
        /// 检索上次执行的语句的 参数。
        /// </summary>
        public object[] LastArgs
        {
            get { return _lastArgs; }
        }
        /// <summary>
        /// 格式化的上次执行语句的SQL和参数。
        /// </summary>
        public string LastCommand
        {
            get { return FormatCommand(_lastSql, _lastArgs); }
        }
        /// <summary>
        /// 格式化 DB 命令的SQL和其参数
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string FormatCommand(IDbCommand cmd)
        {
            return FormatCommand(cmd.CommandText, (from IDataParameter parameter in cmd.Parameters select parameter.Value).ToArray());
        }
        /// <summary>
        /// 格式化SQL和其参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string FormatCommand(string sql, object[] args)
        {
            var sb = new StringBuilder();
            if (sql == null)
                return "";
            sb.Append(sql);
            if (args != null && args.Length > 0)
            {
                sb.Append("\n");
                for (int i = 0; i < args.Length; i++)
                {
                    sb.AppendFormat("\t -> {0}{1} [{2}] = \"{3}\"\n", _Type.ParameterPrefix, i, args[i].GetType().Name, args[i]);
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
        #endregion
        #region CreateParameter
        /// <summary>
        /// 创建数据库参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value)
        {
            return _Type.CreateParameter(name, value);
        }
        /// <summary>
        /// 创建数据库返回参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParameter(string name)
        {
            return _Type.CreateReturnParameter(name);
        }
        /// <summary>
        /// 创建数据库输出参数 可变长类型需要指定长度
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType, int size)
        {
            return _Type.CreateOutputParameter(name, dbType, size);
        }
        /// <summary>
        /// 创建数据库输出参数 固定长值类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType)
        {
            return _Type.CreateOutputParameter(name, dbType);
        }
        #endregion

        #region Ado
        #region 基础
        IDbCommand CreateCommand(IDbConnection connection, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("要创建 DbCommand 对象的 sql 为空");

            IDbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.Transaction = _transaction;
            if (args != null && args.Length > 0)
            {
                //将@替换为当前前缀，并将两个@@替换为一个@。
                sql = _Type.ParameterPrefixReplace(sql);
                foreach (var item in args)
                {
                    cmd.Parameters.Add(CreateParameter(cmd, item));
                }
            }
            cmd.CommandText = sql;

            //命令个性设置
            _Type.SetCommand(cmd);

            //命令超时设置
            if (OneTimeCommandTimeout != 0)
            {
                cmd.CommandTimeout = OneTimeCommandTimeout;
                OneTimeCommandTimeout = 0;
            }
            else if (CommandTimeout != 0)
            {
                cmd.CommandTimeout = CommandTimeout;
            }

            // 虚方法供命令执行前的日志输出或修改
            OnExecutingCommand(cmd);

            // 最终SQL语句跟踪
            _lastSql = cmd.CommandText;
            _lastArgs = (from IDataParameter parameter in cmd.Parameters select parameter.Value).ToArray();

            return cmd;
        }
        IDbDataParameter CreateParameter(IDbCommand cmd, object value)
        {
            // 如果value已经是参数，则改变参数名后直接返回。
            IDbDataParameter p = value as IDbDataParameter;
            if (p != null)
            {
                p.ParameterName = _Type.ParameterIndexName(cmd);
                return p;
            }

            p = cmd.CreateParameter();
            p.ParameterName = _Type.ParameterIndexName(cmd);
            _Type.SetParameter(p, value);

            return p;
        }

        /// <summary>
        /// 执行SQ语句，提供用户 IDbCommand 操作。
        /// </summary>
        /// <param name="action">提供给用户操作IDbCommand的委托</param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        public void AdoCommand(Action<IDbCommand> action, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoCommand的sql不能为空");
            try
            {
                OpenSharedConnection();
                try
                {
                    using (IDbCommand cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        action(cmd);
                        OnExecutedCommand(cmd);
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
            }
        }
        /// <summary>
        /// 执行SQ语句，返回 T。要求用户提IDbCommand到T操作。
        /// </summary>
        /// <typeparam name="T">表示IDbCommand执行结果的类型</typeparam>
        /// <param name="func">提供给用户实现从IDbCommand到T操作的委托</param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>IDbCommand执行结果</returns>
        public T AdoCommand<T>(Func<IDbCommand, T> func, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoCommand<T>的sql不能为空");
            try
            {
                OpenSharedConnection();
                try
                {
                    using (IDbCommand cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        T t = func(cmd);
                        OnExecutedCommand(cmd);
                        return t;
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return default(T);
            }
        }

        /// <summary>
        /// 执行SQ语句，遍历整个结果集，要求用户处理每行数据的IDataReader。
        /// </summary>
        /// <param name="action">提供给用户操作一行数据IDataReader的委托</param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        public void AdoReader(Action<IDataReader> action, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoReader的sql不能为空");
            try
            {
                OpenSharedConnection();
                try
                {
                    using (IDbCommand cmd = CreateCommand(_sharedConnection, sql, args))
                    {
                        IDataReader r = null;
                        try
                        {
                            r = cmd.ExecuteReader();
                            OnExecutedCommand(cmd);
                        }
                        catch (Exception x)
                        {
                            if (OnException(x))
                                throw;
                            return;
                        }
                        using (r)
                        {
                            while (true)
                            {
                                try
                                {
                                    if (!r.Read())
                                        return;
                                    action(r);
                                }
                                catch (Exception x)
                                {
                                    if (OnException(x))
                                        throw;
                                    return;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return;
            }
        }
        /// <summary>
        /// 执行SQ语句，返回 IEnumerable&lt;T&gt;。遍历整个结果集，要求用户提供一行数据IDataReader到T的转换。（使用延迟加载技术，但需要Db提供程序支持）
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func">提供给用户实现从一行数据IDataReader转换到T操作的委托</param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoReader<T>的sql不能为空");

            //打开连接
            try
            {
                OpenSharedConnection();
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                yield break;
            }

            try
            {
                //创建命令
                IDbCommand cmd;
                try
                {
                    cmd = CreateCommand(_sharedConnection, sql, args);
                }
                catch (Exception x)
                {
                    if (OnException(x))
                        throw;
                    yield break;
                }

                using (cmd)
                {
                    //创建Reader
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        if (OnException(x))
                            throw;
                        yield break;
                    }

                    using (r)
                    {
                        //读取所有行
                        while (true)
                        {
                            T t;
                            try
                            {
                                //读取一行
                                if (!r.Read())
                                    yield break;
                                t = func(r);
                            }
                            catch (Exception x)
                            {
                                if (OnException(x))
                                    throw;
                                yield break;
                            }
                            yield return t;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        /// <summary>
        /// sql为两个查询，第一个返回总行数，第二个返回分页结果。执行Reader，遍历两个结果集，返回Page对象。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func"></param>
        /// <param name="sql">sql为两个查询，第一个返回总行数，第二个返回分页结果。</param>
        /// <param name="args"></param>
        /// <returns>包含总行数和分页结果的Page对象</returns>
        Page<T> AdoPage<T>(Func<IDataReader, T> func, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoPage<T>的sql不能为空");

            var page = new Page<T>();
            //打开连接
            try
            {
                OpenSharedConnection();
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return page;
            }

            try
            {
                //创建命令
                IDbCommand cmd;
                try
                {
                    cmd = CreateCommand(_sharedConnection, sql, args);
                }
                catch (Exception x)
                {
                    if (OnException(x))
                        throw;
                    return page;
                }

                using (cmd)
                {
                    //创建Reader
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        if (OnException(x))
                            throw;
                        return page;
                    }

                    using (r)
                    {
                        //读取总行数
                        bool b = false;
                        try
                        {
                            b = r.Read();
                        }
                        catch (Exception x)
                        {
                            if (OnException(x))
                                throw;
                        }

                        if (b)
                        {
                            page.TotalRows = (long)r[0];
                            page.Rows = new List<T>();

                            //读取分页数据
                            if (r.NextResult())
                            {
                                while (true)
                                {
                                    try
                                    {
                                        //读取一行
                                        if (!r.Read())
                                            break;
                                        page.Rows.Add(func(r));
                                    }
                                    catch (Exception x)
                                    {
                                        if (OnException(x))
                                            throw;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
            return page;
        }
        #endregion
        #region 重载
        /// <summary>
        /// 执行SQ语句，提供用户 IDbCommand 操作。
        /// </summary>
        /// <param name="action">提供给用户操作IDbCommand的委托</param>
        /// <param name="sql"></param>
        public void AdoCommand(Action<IDbCommand> action, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行AdoCommand的Sql对象不能为空");
            AdoCommand(action, sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 执行SQ语句，返回 T。要求用户提IDbCommand到T操作。
        /// </summary>
        /// <typeparam name="T">表示IDbCommand执行结果的类型</typeparam>
        /// <param name="func">提供给用户实现从IDbCommand到T操作的委托</param>
        /// <param name="sql"></param>
        /// <returns>IDbCommand执行结果</returns>
        public T AdoCommand<T>(Func<IDbCommand, T> func, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行AdoCommand<T>的Sql对象不能为空");
            return AdoCommand<T>(func, sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 执行SQ语句，遍历整个结果集，要求用户处理每行数据的IDataReader。
        /// </summary>
        /// <param name="action">提供给用户操作一行数据IDataReader的委托</param>
        /// <param name="sql"></param>
        public void AdoReader(Action<IDataReader> action, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行AdoReader的Sql对象不能为空");
            AdoReader(action, sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 执行SQ语句，返回 IEnumerable&lt;T&gt;。遍历整个结果集，要求用户提供一行数据IDataReader到T的转换。（使用延迟加载技术，但需要Db提供程序支持）
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func">提供给用户实现从一行数据IDataReader转换到T操作的委托</param>
        /// <param name="sql"></param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行AdoReader<T>的Sql对象不能为空");
            return AdoReader<T>(func, sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 将sql生成skiptakesql语句执行，遍历返回的一页结果集，要求用户处理每行数据的IDataReader。
        /// </summary>
        /// <param name="action">提供给用户操作IDbCommand的委托</param>
        /// <param name="skip">要跳过的行数</param>
        /// <param name="take">要获取的行数</param>
        /// <param name="sql">原查询语句</param>
        /// <param name="args"></param>
        public void AdoReader(Action<IDataReader> action, long skip, long take, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoReader分页的sql不能为空");
            string sqlPage = _Type.BuildSkipTake(skip, take, sql);
            AdoReader(action, sqlPage, args);
        }
        /// <summary>
        /// 将sql生成skiptakesql语句执行，返回 IEnumerable&lt;T&gt;。遍历整个结果集，要求用户提供一行数据IDataReader到T的转换。（使用延迟加载技术，但需要Db提供程序支持）
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func">提供给用户实现从一行数据IDataReader转换到T操作的委托</param>
        /// <param name="skip">要跳过的行数</param>
        /// <param name="take">要获取的行数</param>
        /// <param name="sql">原查询语句</param>
        /// <param name="args"></param>
        /// <returns>一页结果集的枚举集合</returns>
        public IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, long skip, long take, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行AdoReader<T>分页的sql不能为空");
            string sqlPage = _Type.BuildSkipTake(skip, take, sql);
            return AdoReader<T>(func, sqlPage, args);
        }

        /// <summary>
        /// 将sql生成skiptakesql语句执行，遍历返回的一页结果集，要求用户处理每行数据的IDataReader。
        /// </summary>
        /// <param name="action">提供给用户操作IDbCommand的委托</param>
        /// <param name="skip">要跳过的行数</param>
        /// <param name="take">要获取的行数</param>
        /// <param name="sql">原查询语句</param>
        public void AdoReader(Action<IDataReader> action, long skip, long take, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行AdoReader分页的Sql对象不能为空");
            string sqlPage = _Type.BuildSkipTake(skip, take, sql.SQL);
            AdoReader(action, sqlPage, sql.Arguments);
        }
        /// <summary>
        /// 将sql生成skiptakesql语句执行，返回 IEnumerable&lt;T&gt;。遍历整个结果集，要求用户提供一行数据IDataReader到T的转换。（使用延迟加载技术，但需要Db提供程序支持）
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func">提供给用户实现从一行数据IDataReader转换到T操作的委托</param>
        /// <param name="skip">要跳过的行数</param>
        /// <param name="take">要获取的行数</param>
        /// <param name="sql">原查询语句</param>
        /// <returns>一页结果集的枚举集合</returns>
        public IEnumerable<T> AdoReader<T>(Func<IDataReader, T> func, long skip, long take, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行AdoReader分页的Sql对象不能为空");
            string sqlPage = _Type.BuildSkipTake(skip, take, sql.SQL);
            return AdoReader<T>(func, sqlPage, sql.Arguments);
        }
        #endregion
        #endregion
        #region operation
        #region SQL_基础
        /// <summary>
        /// 执行 non-query 命令，返回受影响行数。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>受影响行数</returns>
        public int Execute(string sql, params object[] args)
        {
            //空字符串不执行任何操作返回
            if (string.IsNullOrWhiteSpace(sql))
                return 0;
            return AdoCommand<int>(cmd => cmd.ExecuteNonQuery(), sql, args);
        }

        /// <summary>
        /// 返回结果集的首行首列。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>结果集的首行首列</returns>
        public object ExecuteScalar(string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行ExecuteScalar的sql不能为空");
            return AdoCommand<object>(cmd => cmd.ExecuteScalar(), sql, args);
        }
        /// <summary>
        /// 返回结果集的首行首列。
        /// </summary>
        /// <typeparam name="T">将结果值转换为的类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>结果集的首行首列</returns>
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行ExecuteScalar<T>的sql不能为空");
            return AdoCommand<T>(cmd =>
            {
                object scalar = cmd.ExecuteScalar();
                return (T)new Meta(typeof(T)).Convert(scalar);

            }, sql, args);
        }

        /// <summary>
        /// 执行Reader，延迟加载返回结果集的枚举集合。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的全部数据。
            if (string.IsNullOrWhiteSpace(sql))
                return Query<T>();
            Meta meta = new Meta(typeof(T));
            return AdoReader<T>(r => (T)meta.Convert(r), sql, args);
        }

        /// <summary>
        /// 对Query的Linq Single方法调用。结果集必须有且仅有一行，零行或多行均抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>唯一一行数据的对象</returns>
        public T Single<T>(string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的唯一一行，零行或多行均抛异常。
            if (string.IsNullOrWhiteSpace(sql))
                return Single<T>();
            return Query<T>(sql, args).Single();
        }
        /// <summary>
        /// 对Query的Linq SingleOrDefault方法调用。结果集必须仅有一行或为空，多行抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql">The SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>一行数据的对象或default(T)</returns>
        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的仅有一行或为空，多行抛异常。
            if (string.IsNullOrWhiteSpace(sql))
                return SingleOrDefault<T>();
            return Query<T>(sql, args).SingleOrDefault();
        }

        /// <summary>
        /// 对Query的Linq First方法调用。结果集必须至少包含一行，返回首行，空行抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>结果集第一行数据的对象</returns>
        public T First<T>(string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的首行，空行抛异常。
            if (string.IsNullOrWhiteSpace(sql))
                return First<T>();
            return Query<T>(sql, args).First();
        }
        /// <summary>
        /// 对Query的Linq FirstOrDefault方法调用。返回结果集的第一行或default(T)。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>结果集的第一行或default(T)</returns>
        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的首行或default(T)。
            if (string.IsNullOrWhiteSpace(sql))
                return FirstOrDefault<T>();
            return Query<T>(sql, args).FirstOrDefault();
        }

        /// <summary>
        /// 判断指定的查询语句结果是否存在。
        /// </summary>
        /// <param name="srcSql">原查询语句</param>
        /// <param name="srcArgs">参数</param>
        /// <returns>存在 True,不存在 False。</returns>
        public bool Exists(string srcSql, params object[] srcArgs)
        {
            if (string.IsNullOrWhiteSpace(srcSql))
                throw new Exception("执行Exists的sql不能为空");
            string sql = string.Format(_Type.ExistsTemplate, srcSql);
            return ExecuteScalar<int>(sql, srcArgs) != 0;
        }

        /// <summary>
        /// 对Query的 ToList方法调用。返回整个结果集作为一个类型列表。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>表示整个结果集的类型列表</returns>
        public List<T> Fetch<T>(string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的全部数据。
            if (string.IsNullOrWhiteSpace(sql))
                return Fetch<T>();
            return Query<T>(sql, args).ToList();
        }
        /// <summary>
        /// 执行查询，返回DataTable。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>DataTable</returns>
        public DataTable Fetch(string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行Fetch的sql不能为空");
            return AdoCommand<DataTable>(cmd =>
            {
                var ada = _Type.CreateDataAdapter();
                ada.SelectCommand = (DbCommand)cmd;

                DataTable dt = new DataTable("DataTable");
                ada.Fill(dt);
                dt.RemotingFormat = SerializationFormat.Binary;
                return dt;

            }, sql, args);
        }
        /// <summary>
        /// 执行多结果集查询，返回DataSet。
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>包含多结果集的DataSet</returns>
        public DataSet Fetchs(string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行Fetchs的sql不能为空");
            return AdoCommand<DataSet>(cmd =>
            {
                var ada = _Type.CreateDataAdapter();
                ada.SelectCommand = (DbCommand)cmd;

                DataSet ds = new DataSet("DataSet");
                ada.Fill(ds);
                ds.RemotingFormat = SerializationFormat.Binary;
                return ds;

            }, sql, args);
        }

        /// <summary>
        /// 将sql构造出计算总行数的语句和取第几页数据的语句执行，并返回Page对象。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询</param>
        /// <param name="args"></param>
        /// <returns>包含总行数和分页数据的对象</returns>
        public Page<T> Page<T>(long pageIndex, long pageSize, string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (string.IsNullOrWhiteSpace(sql))
                return Page<T>(pageIndex, pageSize);

            //返回全部
            if (pageSize < 1 || pageIndex < 1)
            {
                var listPage = Fetch<T>(sql, args);
                return new Page<T>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRows = listPage == null ? 0 : listPage.Count,
                    TotalPages = 1,
                    Rows = listPage
                };
            }

            string sqlCount = SqlHelper.BuildCount(sql);
            var saveTimeout = OneTimeCommandTimeout;// 因为OneTimeCommandTimeout在一次执行后会清零，这里想让OneTimeCommandTimeout在两次查询中都有效。
            long totalRows = ExecuteScalar<long>(sqlCount, args);

            //计算总页数，总页数最小为1
            long totalPages = totalRows % pageSize == 0 ? totalRows / pageSize : totalRows / pageSize + 1;
            if (totalPages < 1)
                totalPages = 1;
            //当前页最大为总页数
            if (pageIndex > totalPages)
                pageIndex = totalPages;

            //无查询结果
            if (totalRows == 0)
            {
                return new Page<T>
                {
                    PageIndex = 1,
                    PageSize = pageSize,
                    TotalRows = 0,
                    TotalPages = 1,
                    Rows = new List<T>()
                };
            }

            string sqlPage = _Type.BuildSkipTake((pageIndex - 1) * pageSize, pageSize, sql);
            OneTimeCommandTimeout = saveTimeout;
            var rows = Fetch<T>(sqlPage, args);

            return new Page<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRows = totalRows,
                TotalPages = totalPages,
                Rows = rows
            };
        }
        public Page<T> Page<T>(long pageIndex, long pageSize, string sqlCount, string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (string.IsNullOrWhiteSpace(sql))
                return Page<T>(pageIndex, pageSize);

            //返回全部
            if (pageSize < 1 || pageIndex < 1)
            {
                var listPage = Fetch<T>(sql, args);
                return new Page<T>
                {
                    PageIndex = pageIndex,
                    PageSize = pageSize,
                    TotalRows = listPage == null ? 0 : listPage.Count,
                    TotalPages = 1,
                    Rows = listPage
                };
            }

            //string sqlCount = SqlHelper.BuildCount(sql);
            var saveTimeout = OneTimeCommandTimeout;// 因为OneTimeCommandTimeout在一次执行后会清零，这里想让OneTimeCommandTimeout在两次查询中都有效。
            long totalRows = ExecuteScalar<long>(sqlCount, args);

            //计算总页数，总页数最小为1
            long totalPages = totalRows % pageSize == 0 ? totalRows / pageSize : totalRows / pageSize + 1;
            if (totalPages < 1)
                totalPages = 1;
            //当前页最大为总页数
            if (pageIndex > totalPages)
                pageIndex = totalPages;

            //无查询结果
            if (totalRows == 0)
            {
                return new Page<T>
                {
                    PageIndex = 1,
                    PageSize = pageSize,
                    TotalRows = 0,
                    TotalPages = 1,
                    Rows = new List<T>()
                };
            }

            string sqlPage = _Type.BuildSkipTake((pageIndex - 1) * pageSize, pageSize, sql);
            OneTimeCommandTimeout = saveTimeout;
            var rows = Fetch<T>(sqlPage, args);

            return new Page<T>
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRows = totalRows,
                TotalPages = totalPages,
                Rows = rows
            };
        }

        /// <summary>
        /// 将sql构造出计算总行数的语句和取第几页数据的语句执行，并返回DataSet(第一个Table是总行数结果，第二个Table是分页数据)。
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询语句</param>
        /// <param name="args"></param>
        /// <returns>DataSet(第一个Table是总行数结果，第二个Table是分页数据)</returns>
        public DataSet Page(long pageIndex, long pageSize, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行Page的sql不能为空");
            //返回全部
            if (pageSize < 1 || pageIndex < 1)
            {
                DataTable dt = Fetch(sql, args);
                return SqlHelper.BuildDataSet(dt);
            }

            string sqlCount = SqlHelper.BuildCount(sql);
            var saveTimeout = OneTimeCommandTimeout;// 因为OneTimeCommandTimeout在一次执行后会清零，这里想让OneTimeCommandTimeout在两次查询中都有效。
            DataTable dtCount = Fetch(sqlCount, args);
            dtCount.TableName = "dtCount";
            long totalRows = Convert.ToInt64(dtCount.Rows[0][0]);

            //计算总页数，总页数最小为1
            long totalPages = totalRows % pageSize == 0 ? totalRows / pageSize : totalRows / pageSize + 1;
            if (totalPages < 1)
                totalPages = 1;
            //当前页最大为总页数
            if (pageIndex > totalPages)
                pageIndex = totalPages;

            string sqlPage = _Type.BuildSkipTake((pageIndex - 1) * pageSize, pageSize, sql);
            OneTimeCommandTimeout = saveTimeout;
            DataTable dtPage = Fetch(sqlPage, args);
            dtPage.TableName = "dtPage";

            DataSet ds = new DataSet("DataSet");
            ds.Tables.Add(dtCount);
            ds.Tables.Add(dtPage);
            return ds;
        }
        #endregion
        #region SQL_重载
        /// <summary>
        /// 执行 non-query 命令，返回受影响行数。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>受影响行数</returns>
        public int Execute(Sql sql)
        {
            //空字符串不执行任何操作返回
            if (sql == null)
                return 0;
            return Execute(sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 返回结果集的首行首列。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>结果集的首行首列</returns>
        public object ExecuteScalar(Sql sql)
        {
            if (sql == null)
                throw new Exception("执行ExecuteScalar的Sql对象不能为空");
            return ExecuteScalar(sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 返回结果集的首行首列。
        /// </summary>
        /// <typeparam name="T">将结果值转换为的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>结果集的首行首列</returns>
        public T ExecuteScalar<T>(Sql sql)
        {
            if (sql == null)
                throw new Exception("执行ExecuteScalar的Sql对象不能为空");
            return ExecuteScalar<T>(sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 执行Reader，延迟加载返回结果集的枚举集合。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> Query<T>(Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的全部数据。
            if (sql == null)
                return Query<T>();
            return Query<T>(sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 构造sql的分页查询语句延迟执行，并返回可枚举集合。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询</param>
        /// <param name="args"></param>
        /// <returns>一页结果集的可枚举集合</returns>
        public IEnumerable<T> Query<T>(long pageIndex, long pageSize, string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (string.IsNullOrWhiteSpace(sql))
                return Query<T>(pageIndex, pageSize);

            //返回全部
            if (pageSize < 1 || pageIndex < 1)
                return Query<T>(sql, args);

            string sqlPage = _Type.BuildSkipTake((pageIndex - 1) * pageSize, pageSize, sql);
            return Query<T>(sqlPage, args);
        }
        /// <summary>
        /// 构造sql的分页查询语句延迟执行，并返回可枚举集合。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询</param>
        /// <returns>一页结果集的可枚举集合</returns>
        public IEnumerable<T> Query<T>(long pageIndex, long pageSize, Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (sql == null)
                return Query<T>(pageIndex, pageSize);

            return Query<T>(pageIndex, pageSize, sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 对Query的Linq Single方法调用。结果集必须有且仅有一行，零行或多行均抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>唯一一行数据的对象</returns>
        public T Single<T>(Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的唯一一行，零行或多行均抛异常。
            if (sql == null)
                return Single<T>();
            return Single<T>(sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 对Query的Linq SingleOrDefault方法调用。结果集必须仅有一行或为空，多行抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>一行数据的对象或default(T)</returns>
        public T SingleOrDefault<T>(Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的仅有一行或为空，多行抛异常。
            if (sql == null)
                return SingleOrDefault<T>();
            return SingleOrDefault<T>(sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 对Query的Linq First方法调用。结果集必须至少包含一行，返回首行，空行抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>结果集第一行数据的对象</returns>
        public T First<T>(Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的首行，空行抛异常。
            if (sql == null)
                return First<T>();
            return First<T>(sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 对Query的Linq FirstOrDefault方法调用。返回结果集的第一行或default(T)。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>结果集的第一行或default(T)</returns>
        public T FirstOrDefault<T>(Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的首行或default(T)。
            if (sql == null)
                return FirstOrDefault<T>();
            return FirstOrDefault<T>(sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 判断指定的查询语句结果是否存在。
        /// </summary>
        /// <param name="srcSql">原查询语句</param>
        /// <returns>存在 True,不存在 False。</returns>
        public bool Exists(Sql srcSql)
        {
            if (srcSql == null)
                throw new Exception("执行Exists的sql不能为空");
            return Exists(srcSql.SQL, srcSql.Arguments);
        }

        /// <summary>
        /// 对Query的 ToList方法调用。返回整个结果集作为一个类型列表。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sql"></param>
        /// <returns>表示整个结果集的类型列表</returns>
        public List<T> Fetch<T>(Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的全部数据。
            if (sql == null)
                return Fetch<T>();
            return Fetch<T>(sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 构造sql的分页查询语句执行，并返回一页结果集。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询</param>
        /// <param name="args"></param>
        /// <returns>一页结果集</returns>
        public List<T> Fetch<T>(long pageIndex, long pageSize, string sql, params object[] args)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (string.IsNullOrWhiteSpace(sql))
                return Fetch<T>(pageIndex, pageSize);

            //返回全部
            if (pageSize < 1 || pageIndex < 1)
                return Fetch<T>(sql, args);

            string sqlPage = _Type.BuildSkipTake((pageIndex - 1) * pageSize, pageSize, sql);
            return Fetch<T>(sqlPage, args);
        }
        /// <summary>
        /// 构造sql的分页查询语句执行，并返回一页结果集。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询</param>
        /// <returns>一页结果集</returns>
        public List<T> Fetch<T>(long pageIndex, long pageSize, Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (sql == null)
                return Fetch<T>(pageIndex, pageSize);
            return Fetch<T>(pageIndex, pageSize, sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 执行查询，返回DataTable。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>DataTable</returns>
        public DataTable Fetch(Sql sql)
        {
            if (sql == null)
                throw new Exception("执行Fetch的sql对象不能为空");
            return Fetch(sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 将sql构造成分页查询执行，以DataTable返回。
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询语句</param>
        /// <param name="args"></param>
        /// <returns>包含一页数据的DataTable</returns>
        public DataTable Fetch(long pageIndex, long pageSize, string sql, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(sql))
                throw new Exception("执行分页Fetch的sql不能为空");

            //返回全部
            if (pageSize < 1 || pageIndex < 1)
                return Fetch(sql, args);

            string sqlPage = _Type.BuildSkipTake((pageIndex - 1) * pageSize, pageSize, sql);
            return Fetch(sqlPage, args);
        }
        /// <summary>
        /// 将sql构造成分页查询执行，以DataTable返回。
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询语句</param>
        /// <returns>包含一页数据的DataTable</returns>
        public DataTable Fetch(long pageIndex, long pageSize, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行分页Fetch的sql对象不能为空");
            return Fetch(pageIndex, pageSize, sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 执行多结果集查询，返回DataSet。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>包含多结果集的DataSet</returns>
        public DataSet Fetchs(Sql sql)
        {
            if (sql == null)
                throw new Exception("执行Fetchs的sql对象不能为空");
            return Fetchs(sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// 将sql构造出计算总行数的语句和取第几页数据的语句执行，并返回Page对象。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询</param>
        /// <returns>包含总行数和分页数据的对象</returns>
        public Page<T> Page<T>(long pageIndex, long pageSize, Sql sql)
        {
            //无查询条件返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
            if (sql == null)
                return Page<T>(pageIndex, pageSize);
            return Page<T>(pageIndex, pageSize, sql.SQL, sql.Arguments);
        }
        /// <summary>
        /// 将sql构造出计算总行数的语句和取第几页数据的语句执行，并返回DataSet(第一个Table是总行数结果，第二个Table是分页数据)。
        /// </summary>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="sql">原始查询语句</param>
        /// <returns>DataSet(第一个Table是总行数结果，第二个Table是分页数据)</returns>
        public DataSet Page(long pageIndex, long pageSize, Sql sql)
        {
            if (sql == null)
                throw new Exception("执行Page的sql对象不能为空");
            return Page(pageIndex, pageSize, sql.SQL, sql.Arguments);
        }
        #endregion
        #region 生成语句
        //T 首先表示返回类型，某些重载中用于指定表名。
        //生成表名：有tableName时直接指定，有泛型参数时由泛型类型名指定，最后由poco类型名指定。
        #region No Where
        //借用返回类型T参数或新增泛型参数来表示表名。
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的首行首列。
        /// </summary>
        /// <typeparam name="T">T仅用来表示表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的首行首列</returns>
        public object ExecuteScalar<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return ExecuteScalar(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的全部数据。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <returns>整个表(T的类型名或T上TableAttribute指定的表名)</returns>
        public IEnumerable<T> Query<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Query<T>(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的唯一一行，零行或多行均抛异常。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的唯一一行</returns>
        public T Single<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Single<T>(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的仅有一行或为空，多行抛异常。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的仅有一行或default(T)</returns>
        public T SingleOrDefault<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return SingleOrDefault<T>(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的首行，空行抛异常。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的首行</returns>
        public T First<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return First<T>(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的首行或default(T)。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的首行或default(T)</returns>
        public T FirstOrDefault<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return FirstOrDefault<T>(sql);
        }
        /// <summary>
        /// 判断表(T的类型名或T上TableAttribute指定的表名)是否包含数据。
        /// </summary>
        /// <typeparam name="T">T仅用来表示表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)包含数据为True,不包含为False。</returns>
        public bool Exists<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Exists<T>(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的全部数据。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的全部数据</returns>
        public List<T> Fetch<T>()
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Fetch<T>(sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的第n页数据</returns>
        public IEnumerable<T> Query<T>(long pageIndex, long pageSize)
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Query<T>(pageIndex, pageSize, sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的第n页数据</returns>
        public List<T> Fetch<T>(long pageIndex, long pageSize)
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Fetch<T>(pageIndex, pageSize, sql);
        }
        /// <summary>
        /// 返回表(T的类型名或T上TableAttribute指定的表名)的第n页数据。
        /// </summary>
        /// <typeparam name="T">T表示返回类型和表名</typeparam>
        /// <param name="pageIndex">第几页</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>表(T的类型名或T上TableAttribute指定的表名)的第n页数据</returns>
        public Page<T> Page<T>(long pageIndex, long pageSize)
        {
            Sql sql = PocoToSql.Where(typeof(T), null, null, null, _Type.AddBoundary);
            return Page<T>(pageIndex, pageSize, sql);
        }
        /// <summary>
        /// 删除表(T的类型名或T上TableAttribute指定的表名)全部数据
        /// </summary>
        /// <typeparam name="T">T仅用来表示表名</typeparam>
        /// <returns>受影响行数</returns>
        public int Delete<T>()
        {
            Sql sql = PocoToSql.Delete(typeof(T), null, null, null, _Type.AddBoundary);
            return Execute(sql);
        }
        #endregion
        #region Where
        /// <summary>
        /// 执行Reader，延迟加载返回结果集的枚举集合。
        /// </summary>
        /// <typeparam name="T">T表示返回类型，当未指定tableName时T还表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取T的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> Query<T>(object poco, string tableName = null, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, tableName, columns, _Type.AddBoundary);
            return Query<T>(sql);
        }
        /// <summary>
        /// 对Query的Linq Single方法调用。结果集必须有且仅有一行，零行或多行均抛异常。
        /// SELECT * FROM typeof(T).Name WHERE columns=poco.columns
        /// </summary>
        /// <typeparam name="T">T表示返回类型，当未指定tableName时T还表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取T的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>唯一一行数据的对象</returns>
        public T Single<T>(object poco, string tableName = null, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, tableName, columns, _Type.AddBoundary);
            return Single<T>(sql);
        }
        /// <summary>
        /// 对Query的Linq SingleOrDefault方法调用。结果集必须仅有一行或为空，多行抛异常。
        /// SELECT * FROM typeof(T).Name WHERE columns=poco.columns
        /// </summary>
        /// <typeparam name="T">T表示返回类型，当未指定tableName时T还表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取T的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>一行数据的对象</returns>
        public T SingleOrDefault<T>(object poco, string tableName = null, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, tableName, columns, _Type.AddBoundary);
            return SingleOrDefault<T>(sql);
        }
        /// <summary>
        /// 对Query的Linq First方法调用。结果集必须至少包含一行，返回首行，空行抛异常。
        /// </summary>
        /// <typeparam name="T">T表示返回类型，当未指定tableName时T还表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取T的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>结果集第一行数据的对象</returns>
        public T First<T>(object poco, string tableName = null, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, tableName, columns, _Type.AddBoundary);
            return First<T>(sql);
        }
        /// <summary>
        /// 对Query的Linq FirstOrDefault方法调用。返回结果集的第一行或default(T)。
        /// </summary>
        /// <typeparam name="T">T表示返回类型，当未指定tableName时T还表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取T的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>结果集的第一行或default(T)</returns>
        public T FirstOrDefault<T>(object poco, string tableName = null, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, tableName, columns, _Type.AddBoundary);
            return FirstOrDefault<T>(sql);
        }

        /// <summary>
        /// 判断 SELECT 1 FROM typeof(T).Name WHERE columns=poco.columns 是否存在。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>存在 True,不存在 False。</returns>
        public bool Exists<T>(object poco, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, null, columns, _Type.AddBoundary);
            return Exists(sql);
        }
        /// <summary>
        /// 判断 SELECT * FROM poco.GetType().Name WHERE columns=poco.columns 是否存在。
        /// </summary>
        /// <param name="poco">类型名为表名，其中columns字段设置为了指定值。</param>
        /// <param name="tableName">表名，为空取poco的类型名</param>
        /// <param name="columns">要查询的列名</param>
        /// <returns>存在 True,不存在 False。</returns>
        public bool Exists(object poco, string tableName = null, string columns = null)
        {
            if (poco == null)
                throw new Exception("执行Exists的poco对象不能为空");
            Sql sql = PocoToSql.Where(null, poco, tableName, columns, _Type.AddBoundary);
            return Exists(sql);
        }

        /// <summary>
        /// 对Query的 ToList方法调用。返回整个结果集作为一个类型列表。
        /// </summary>
        /// <typeparam name="T">T表示返回类型，当未指定tableName时T还表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取T的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>表示整个结果集的类型列表</returns>
        public List<T> Fetch<T>(object poco, string tableName = null, string columns = null)
        {
            Sql sql = PocoToSql.Where(typeof(T), poco, tableName, columns, _Type.AddBoundary);
            return Fetch<T>(sql);
        }
        /// <summary>
        /// 执行查询，返回DataTable。
        /// </summary>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="tableName">表名，为空取poco的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>DataTable</returns>
        public DataTable Fetch(object poco, string tableName = null, string columns = null)
        {
            if (poco == null)
                throw new Exception("执行Fetch的poco对象不能为空");
            Sql sql = PocoToSql.Where(null, poco, tableName, columns, _Type.AddBoundary);
            return Fetch(sql);
        }

        /// <summary>
        /// 执行多结果集查询，返回DataSet。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="pocos">简单类型值或Poco的指定columns属性值</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>包含多结果集的DataSet</returns>
        public DataSet Fetchs<T>(IEnumerable pocos, string columns = null)
        {
            Sql sql = PocoToSql.Wheres(typeof(T), pocos, null, columns, _Type.AddBoundary);
            return Fetchs(sql);
        }
        /// <summary>
        /// 执行多结果集查询，返回DataSet。
        /// </summary>
        /// <param name="pocos">类型名为表名，其中columns字段设置为了指定值。</param>
        /// <param name="tableName">表名，为空取pocos集合元素的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>包含多结果集的DataSet</returns>
        public DataSet Fetchs(IEnumerable pocos, string tableName = null, string columns = null)
        {
            if (pocos == null)
                throw new Exception("执行Fetchs的pocos集合不能为空");
            Sql sql = PocoToSql.Wheres(null, pocos, tableName, columns, _Type.AddBoundary);
            return Fetchs(sql);
        }
        #endregion
        #region Delete
        //原始SQL语句
        /// <summary>
        /// 同Execute(sql, args)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>受影响行数</returns>
        public int Delete(string sql, params object[] args)
        {
            return Execute(sql, args);
        }
        /// <summary>
        /// 同Execute(sql)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>受影响行数</returns>
        public int Delete(Sql sql)
        {
            return Execute(sql);
        }
        /// <summary>
        /// 生成 DELETE typeof(T).Name WHERE whereCondition 执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="whereCondition">条件语句</param>
        /// <param name="whereConditionArgs">条件语句的参数</param>
        /// <returns>受影响的行数</returns>
        public int Delete<T>(string whereCondition, params object[] whereConditionArgs)
        {
            Sql sql = PocoToSql.DeleteCondition(typeof(T), whereCondition, whereConditionArgs, _Type.AddBoundary);
            return Execute(sql);
        }

        //生成单条语句
        /// <summary>
        /// 生成 DELETE typeof(T).Name WHERE keyname=keyvalue 执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco">简单类型值或Poco的指定columns属性值</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>受影响的行数</returns>
        public int Delete<T>(object poco, string columns = null)
        {
            Sql sql = PocoToSql.Delete(typeof(T), poco, null, columns, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 生成 DELETE FROM poco.GetType().Name WHERE columns=poco.columns 执行并返回受影响的行数。
        /// </summary>
        /// <param name="poco">类型名为表名，其中columns字段设置为了指定值。</param>
        /// <param name="tableName">表名，为空取poco的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>受影响的行数</returns>
        public int Delete(object poco, string tableName = null, string columns = null)
        {
            if (poco == null)
                throw new Exception("执行Delete的poco对象不能为空");
            Sql sql = PocoToSql.Delete(null, poco, tableName, columns, _Type.AddBoundary);
            return Execute(sql);
        }

        //生成多条语句
        /// <summary>
        /// 生成多条 DELETE typeof(T).Name WHERE columns=poco.columns 批量执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="pocos">简单类型值或Poco的指定columns属性值</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>受影响的行数</returns>
        public int Deletes<T>(IEnumerable pocos, string columns = null)
        {
            Sql sql = PocoToSql.Deletes(typeof(T), pocos, null, columns, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 生成多条 DELETE typeof(T).Name WHERE columns=poco.columns 批量执行并返回受影响的行数。
        /// </summary>
        /// <param name="pocos">类型名为表名，其中columns字段设置为了指定值。</param>
        /// <param name="tableName">表名，为空取pocos集合元素的类型名</param>
        /// <param name="columns">指定poco的属性值</param>
        /// <returns>受影响的行数</returns>
        public int Deletes(IEnumerable pocos, string tableName = null, string columns = null)
        {
            if (pocos == null)
                throw new Exception("执行Deletes的pocos集合不能为空");
            Sql sql = PocoToSql.Deletes(null, pocos, tableName, columns, _Type.AddBoundary);
            return Execute(sql);
        }
        #endregion
        #region Insert
        //原始SQL语句
        /// <summary>
        /// 同Execute(sql, args)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>受影响行数</returns>
        public int Insert(string sql, params object[] args)
        {
            return Execute(sql, args);
        }
        /// <summary>
        /// 同Execute(sql)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>受影响行数</returns>
        public int Insert(Sql sql)
        {
            return Execute(sql);
        }

        //生成单条语句
        /// <summary>
        /// 将 poco 生成一条 INSERT 语句执行并返回受影响的行数。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要插入的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <returns>受影响的行数</returns>
        public int Insert(object poco, string tableName = null, string includeColumns = null, string excludeColumns = null)
        {
            Sql sql = PocoToSql.Insert(null, poco, tableName, includeColumns, excludeColumns, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 将 poco 生成一条 INSERT 语句执行并返回自增列的值。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要插入的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="autoIncrementColumn">自增列</param>
        /// <returns>自增列的值</returns>
        public object Insert(object poco, string tableName, string includeColumns, string excludeColumns, string autoIncrementColumn)
        {
            var tup = PocoToSql.InsertAutoIncrement(null, poco, tableName, includeColumns, excludeColumns, autoIncrementColumn, _Type);
            switch (_Type.ProviderEnum)
            {
                case DatabaseProviderEnum.OracleClient:
                case DatabaseProviderEnum.OracleDataAccessClient:
                    Execute(tup.Item1);
                    return tup.Item2.Value;
                case DatabaseProviderEnum.SqlServerCe35:
                case DatabaseProviderEnum.SqlServerCe40:
                    Execute(tup.Item1);
                    return ExecuteScalar(tup.Item3);
                default:
                    return ExecuteScalar(tup.Item1);
            }
        }
        //生成多条语句
        /// <summary>
        /// 将 pocoList 生成多条 INSERT 语句批量执行并返回受影响的行数。
        /// </summary>
        /// <param name="pocos"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要插入的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <returns>受影响的行数</returns>
        public int Inserts(IEnumerable pocos, string tableName = null, string includeColumns = null, string excludeColumns = null)
        {
            Sql sql = PocoToSql.Inserts(null, pocos, tableName, includeColumns, excludeColumns, _Type.AddBoundary);
            return Execute(sql);
        }

        //生成单条语句
        /// <summary>
        /// 将 poco 生成一条 INSERT 语句执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco"></param>
        /// <param name="includeColumns">逗号分隔的要插入的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <returns>受影响的行数</returns>
        public int Insert<T>(object poco, string includeColumns = null, string excludeColumns = null)
        {
            Sql sql = PocoToSql.Insert(typeof(T), poco, null, includeColumns, excludeColumns, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 将 poco 生成一条 INSERT 语句执行并返回自增列的值。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco"></param>
        /// <param name="includeColumns">逗号分隔的要插入的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="autoIncrementColumn">自增列</param>
        /// <returns>自增列的值</returns>
        public object Insert<T>(object poco, string includeColumns, string excludeColumns, string autoIncrementColumn)
        {
            var tup = PocoToSql.InsertAutoIncrement(typeof(T), poco, null, includeColumns, excludeColumns, autoIncrementColumn, _Type);
            switch (_Type.ProviderEnum)
            {
                case DatabaseProviderEnum.OracleClient:
                case DatabaseProviderEnum.OracleDataAccessClient:
                    Execute(tup.Item1);
                    return tup.Item2.Value;
                case DatabaseProviderEnum.SqlServerCe35:
                case DatabaseProviderEnum.SqlServerCe40:
                    Execute(tup.Item1);
                    return ExecuteScalar(tup.Item3);
                default:
                    return ExecuteScalar(tup.Item1);
            }
        }
        //生成多条语句
        /// <summary>
        /// 将 pocoList 生成多条 INSERT 语句批量执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="pocos"></param>
        /// <param name="includeColumns">逗号分隔的要插入的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <returns>受影响的行数</returns>
        public int Inserts<T>(IEnumerable pocos, string includeColumns = null, string excludeColumns = null)
        {
            Sql sql = PocoToSql.Inserts(typeof(T), pocos, null, includeColumns, excludeColumns, _Type.AddBoundary);
            return Execute(sql);
        }
        #endregion
        #region Update
        //原始SQL语句
        /// <summary>
        /// 同Execute(sql, args)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns>受影响行数</returns>
        public int Update(string sql, params object[] args)
        {
            return Execute(sql, args);
        }
        /// <summary>
        /// 同Execute(sql)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>受影响行数</returns>
        public int Update(Sql sql)
        {
            return Execute(sql);
        }

        //生成单条语句
        /// <summary>
        /// 将 poco 生成一条 UPDATE 语句执行并返回受影响的行数。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="setColumns">逗号分隔的要更新的列</param>
        /// <param name="notsetColumns">逗号分隔的要排除的列</param>
        /// <param name="whereCondition">WHERE条件语句</param>
        /// <param name="whereConditionArgs">WHERE条件语句的参数</param>
        /// <returns>受影响的行数</returns>
        public int UpdateCondition(object poco, string tableName = null, string setColumns = null, string notsetColumns = null, string whereCondition = null, params object[] whereConditionArgs)
        {
            Sql sql = PocoToSql.UpdateCondition(null, poco, tableName, setColumns, notsetColumns, whereCondition, whereConditionArgs, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 将 poco 生成一条 UPDATE 语句执行并返回受影响的行数。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="setColumns">逗号分隔的要更新的列</param>
        /// <param name="notsetColumns">逗号分隔的要排除的列</param>
        /// <param name="whereColumns">逗号分隔的WHERE条件列</param>
        /// <returns>受影响的行数</returns>
        public int Update(object poco, string tableName = null, string setColumns = null, string notsetColumns = null, string whereColumns = null)
        {
            Sql sql = PocoToSql.Update(null, poco, tableName, setColumns, notsetColumns, whereColumns, _Type.AddBoundary);
            return Execute(sql);
        }
        //生成多条语句
        /// <summary>
        /// 将 pocoList 生成多条 UPDATE 语句批量执行并返回受影响的行数。
        /// </summary>
        /// <param name="pocos"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="setColumns">逗号分隔的要更新的列</param>
        /// <param name="notsetColumns">逗号分隔的要排除的列</param>
        /// <param name="whereColumns">逗号分隔的WHERE条件列或WHERE条件语句</param>
        /// <returns>受影响的行数</returns>
        public int Updates(IEnumerable pocos, string tableName = null, string setColumns = null, string notsetColumns = null, string whereColumns = null)
        {
            Sql sql = PocoToSql.Updates(null, pocos, tableName, setColumns, notsetColumns, whereColumns, _Type.AddBoundary);
            return Execute(sql);
        }

        //生成单条语句
        /// <summary>
        /// 将 poco 生成一条 UPDATE 语句执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco"></param>
        /// <param name="setColumns">逗号分隔的要更新的列</param>
        /// <param name="notsetColumns">逗号分隔的要排除的列</param>
        /// <param name="whereCondition">WHERE条件语句</param>
        /// <param name="whereConditionArgs">WHERE条件语句的参数</param>
        /// <returns>受影响的行数</returns>
        public int UpdateCondition<T>(object poco, string setColumns = null, string notsetColumns = null, string whereCondition = null, params object[] whereConditionArgs)
        {
            Sql sql = PocoToSql.UpdateCondition(typeof(T), poco, null, setColumns, notsetColumns, whereCondition, whereConditionArgs, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 将 poco 生成一条 UPDATE 语句执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco"></param>
        /// <param name="setColumns">逗号分隔的要更新的列</param>
        /// <param name="notsetColumns">逗号分隔的要排除的列</param>
        /// <param name="whereColumns">逗号分隔的WHERE条件列</param>
        /// <returns>受影响的行数</returns>
        public int Update<T>(object poco, string setColumns = null, string notsetColumns = null, string whereColumns = null)
        {
            Sql sql = PocoToSql.Update(typeof(T), poco, null, setColumns, notsetColumns, whereColumns, _Type.AddBoundary);
            return Execute(sql);
        }
        //生成多条语句
        /// <summary>
        /// 将 pocoList 生成多条 UPDATE 语句批量执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="pocos"></param>
        /// <param name="setColumns">逗号分隔的要更新的列</param>
        /// <param name="notsetColumns">逗号分隔的要排除的列</param>
        /// <param name="whereColumns">逗号分隔的WHERE条件列或WHERE条件语句</param>
        /// <returns>受影响的行数</returns>
        public int Updates<T>(IEnumerable pocos, string setColumns = null, string notsetColumns = null, string whereColumns = null)
        {
            Sql sql = PocoToSql.Updates(typeof(T), pocos, null, setColumns, notsetColumns, whereColumns, _Type.AddBoundary);
            return Execute(sql);
        }
        #endregion
        #region Save
        /// <summary>
        /// 若autoIncrementColumn对应的属性未赋值则Insert，否则Update.
        /// 将 poco 生成一条 INSERT或UPDATE 语句执行并以T类型返回autoIncrementColumn列的值。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="autoIncrementColumn">自增列，当不为默认值时则执行更新，否则由existsColumns判断是插入还是更新</param>
        /// <returns>自增列的值</returns>
        public object Save(object poco, string tableName = null, string includeColumns = null, string excludeColumns = null, string autoIncrementColumn = null)
        {
            object autoIncrementValue = PocoToSql.GetAutoIncrementValue(poco, ref autoIncrementColumn);
            if (!autoIncrementValue.IsNull())
            {
                Update(poco, tableName, includeColumns, excludeColumns, autoIncrementColumn);
                return autoIncrementValue;
            }
            else
                return Insert(poco, tableName, includeColumns, excludeColumns, autoIncrementColumn);
        }
        /// <summary>
        /// 根据existsColumns列先执行查询判断数据库里是否存在该记录，来调用Inert方法还是Update方法。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="existsColumns">逗号分隔的要判断存在的列</param>
        /// <param name="avoid">无意义，仅作为重载占位符</param>
        /// <returns>受影响的行数</returns>
        public int Save(object poco, string tableName, string includeColumns, string excludeColumns, string existsColumns, object avoid)
        {
            if (poco == null)
                throw new Exception("执行Save方法的poco参数不能为空");
            if (string.IsNullOrWhiteSpace(existsColumns))
                throw new Exception("执行Save方法的existsColumns参数不能为空");
            Sql existsSql = PocoToSql.Exists(null, poco, tableName, existsColumns, _Type.AddBoundary);
            if (Exists(existsSql))
                return Update(poco, tableName, includeColumns, excludeColumns, existsColumns);
            else
                return Insert(poco, tableName, includeColumns, excludeColumns);
        }
        /// <summary>
        /// 根据pocos集合的每个对象的autoIncrementColumn属性是否赋值来生成Insert或Update批量执行并返回受影响的行数。
        /// </summary>
        /// <param name="pocos"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="autoIncrementColumn">自增列，当不为默认值时则执行更新，否则由existsColumns判断是插入还是更新</param>
        /// <returns>受影响的行数</returns>
        public int Saves(IEnumerable pocos, string tableName = null, string includeColumns = null, string excludeColumns = null, string autoIncrementColumn = null)
        {
            Sql sql = PocoToSql.SavesByAutoIncrement(null, pocos, tableName, includeColumns, excludeColumns, autoIncrementColumn, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 根据pocos集合的每个对象的existsColumns列先执行查询判断数据库里是否存在该记录，来生成多条 INERT 或 UPDATE Sql语句，批量执行并返回受影响的行数。。
        /// </summary>
        /// <param name="pocos"></param>
        /// <param name="tableName">表名，若为空则使用poco的类名</param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="existsColumns">逗号分隔的要判断存在的列</param>
        /// <param name="avoid">无意义，仅作为重载占位符</param>
        /// <returns>受影响的行数</returns>
        public int Saves(IEnumerable pocos, string tableName, string includeColumns, string excludeColumns, string existsColumns, object avoid)
        {
            Sql sql = PocoToSql.SavesByExists(null, pocos, tableName, includeColumns, excludeColumns, existsColumns, _Type.AddBoundary, Exists);
            return Execute(sql);
        }

        /// <summary>
        /// 若autoIncrementColumn对应的属性未赋值则Insert，否则Update.
        /// 将 poco 生成一条 INSERT或UPDATE 语句执行并以T类型返回autoIncrementColumn列的值。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco"></param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="autoIncrementColumn">自增列，当不为默认值时则执行更新，否则由existsColumns判断是插入还是更新</param>
        /// <returns>自增列的值</returns>
        public object Save<T>(object poco, string includeColumns = null, string excludeColumns = null, string autoIncrementColumn = null)
        {
            object autoIncrementValue = PocoToSql.GetAutoIncrementValue(poco, ref autoIncrementColumn);
            if (!autoIncrementValue.IsNull())
            {
                Update<T>(poco, includeColumns, excludeColumns, autoIncrementColumn);
                return autoIncrementValue;
            }
            else
                return Insert<T>(poco, includeColumns, excludeColumns, autoIncrementColumn);
        }
        /// <summary>
        /// 根据existsColumns列先执行查询判断数据库里是否存在该记录，来调用Inert方法还是Update方法。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="poco"></param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="existsColumns">逗号分隔的要判断存在的列</param>
        /// <param name="avoid">无意义，仅作为重载占位符</param>
        /// <returns>受影响的行数</returns>
        public int Save<T>(object poco, string includeColumns, string excludeColumns, string existsColumns, object avoid)
        {
            if (poco == null)
                throw new Exception("执行Save方法的poco参数不能为空");
            if (string.IsNullOrWhiteSpace(existsColumns))
                throw new Exception("执行Save方法的existsColumns参数不能为空");
            Sql existsSql = PocoToSql.Exists(typeof(T), poco, null, existsColumns, _Type.AddBoundary);
            if (Exists(existsSql))
                return Update<T>(poco, includeColumns, excludeColumns, existsColumns);
            else
                return Insert<T>(poco, includeColumns, excludeColumns);
        }
        /// <summary>
        /// 根据pocos集合的每个对象的autoIncrementColumn属性是否赋值来生成Insert或Update批量执行并返回受影响的行数。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="pocos"></param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="autoIncrementColumn">自增列，当不为默认值时则执行更新，否则由existsColumns判断是插入还是更新</param>
        /// <returns>受影响的行数</returns>
        public int Saves<T>(IEnumerable pocos, string includeColumns = null, string excludeColumns = null, string autoIncrementColumn = null)
        {
            Sql sql = PocoToSql.SavesByAutoIncrement(typeof(T), pocos, null, includeColumns, excludeColumns, autoIncrementColumn, _Type.AddBoundary);
            return Execute(sql);
        }
        /// <summary>
        /// 根据pocos集合的每个对象的existsColumns列先执行查询判断数据库里是否存在该记录，来生成多条 INERT 或 UPDATE Sql语句，批量执行并返回受影响的行数。。
        /// </summary>
        /// <typeparam name="T">T仅用于表示表名</typeparam>
        /// <param name="pocos"></param>
        /// <param name="includeColumns">逗号分隔的要保存的列</param>
        /// <param name="excludeColumns">逗号分隔的要排除的列</param>
        /// <param name="existsColumns">逗号分隔的要判断存在的列</param>
        /// <param name="avoid">无意义，仅作为重载占位符</param>
        /// <returns>受影响的行数</returns>
        public int Saves<T>(IEnumerable pocos, string includeColumns, string excludeColumns, string existsColumns, object avoid)
        {
            Sql sql = PocoToSql.SavesByExists(typeof(T), pocos, null, includeColumns, excludeColumns, existsColumns, _Type.AddBoundary, Exists);
            return Execute(sql);
        }
        #endregion
        #endregion
        #endregion
        #region Procedure
        #region Ado
        IDbCommand CreateCommandSp(IDbConnection connection, string sp, params IDbDataParameter[] paras)
        {
            IDbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = sp;
            cmd.Transaction = _transaction;
            if (paras != null && paras.Length > 0)
                foreach (var p in paras)
                {
                    cmd.Parameters.Add(p);
                }

            //命令个性设置
            _Type.SetCommand(cmd);

            //命令超时设置
            if (OneTimeCommandTimeout != 0)
            {
                cmd.CommandTimeout = OneTimeCommandTimeout;
                OneTimeCommandTimeout = 0;
            }
            else if (CommandTimeout != 0)
            {
                cmd.CommandTimeout = CommandTimeout;
            }

            // 虚方法供命令执行前的日志输出或修改
            OnExecutingCommand(cmd);

            // 最终SQL语句跟踪
            _lastSql = cmd.CommandText;
            _lastArgs = (from IDataParameter parameter in cmd.Parameters select parameter.Value).ToArray();

            return cmd;
        }

        /// <summary>
        /// 执行存储过程，提供用户 IDbCommand 操作。
        /// </summary>
        /// <param name="action">提供给用户操作IDbCommand的委托</param>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        public void AdoCommandSp(Action<IDbCommand> action, string sp, params IDbDataParameter[] paras)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (IDbCommand cmd = CreateCommandSp(_sharedConnection, sp, paras))
                    {
                        action(cmd);
                        OnExecutedCommand(cmd);
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
            }
        }
        /// <summary>
        /// 执行存储过程，返回 T。要求用户提IDbCommand到T操作。
        /// </summary>
        /// <typeparam name="T">表示IDbCommand执行结果的类型</typeparam>
        /// <param name="func">提供给用户实现从IDbCommand到T操作的委托</param>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>IDbCommand执行结果</returns>
        public T AdoCommandSp<T>(Func<IDbCommand, T> func, string sp, params IDbDataParameter[] paras)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (IDbCommand cmd = CreateCommandSp(_sharedConnection, sp, paras))
                    {
                        T t = func(cmd);
                        OnExecutedCommand(cmd);
                        return t;
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return default(T);
            }
        }

        /// <summary>
        /// 执行存储过程，遍历整个结果集，要求用户处理每行数据的IDataReader。
        /// </summary>
        /// <param name="action">提供给用户操作一行数据IDataReader的委托</param>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        public void AdoReaderSp(Action<IDataReader> action, string sp, params IDbDataParameter[] paras)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (IDbCommand cmd = CreateCommandSp(_sharedConnection, sp, paras))
                    {
                        IDataReader r = null;
                        try
                        {
                            r = cmd.ExecuteReader();
                            OnExecutedCommand(cmd);
                        }
                        catch (Exception x)
                        {
                            if (OnException(x))
                                throw;
                            return;
                        }
                        using (r)
                        {
                            while (true)
                            {
                                try
                                {
                                    if (!r.Read())
                                        return;
                                    action(r);
                                }
                                catch (Exception x)
                                {
                                    if (OnException(x))
                                        throw;
                                    return;
                                }
                            }
                        }
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return;
            }
        }
        /// <summary>
        /// 执行存储过程，返回 IEnumerable&lt;T&gt;。遍历整个结果集，要求用户提供一行数据IDataReader到T的转换。（使用延迟加载技术，但需要Db提供程序支持）
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func">提供给用户实现从一行数据IDataReader转换到T操作的委托</param>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> AdoReaderSp<T>(Func<IDataReader, T> func, string sp, params IDbDataParameter[] paras)
        {
            //打开连接
            try
            {
                OpenSharedConnection();
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                yield break;
            }

            try
            {
                //创建命令
                IDbCommand cmd;
                try
                {
                    cmd = CreateCommandSp(_sharedConnection, sp, paras);
                }
                catch (Exception x)
                {
                    if (OnException(x))
                        throw;
                    yield break;
                }

                using (cmd)
                {
                    //创建Reader
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        if (OnException(x))
                            throw;
                        yield break;
                    }

                    using (r)
                    {
                        //读取所有行
                        while (true)
                        {
                            T t;
                            try
                            {
                                //读取一行
                                if (!r.Read())
                                    yield break;
                                t = func(r);
                            }
                            catch (Exception x)
                            {
                                if (OnException(x))
                                    throw;
                                yield break;
                            }
                            yield return t;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }
        /// <summary>
        /// 执行存储过程获得Reader，遍历两个结果集，返回Page对象。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="func"></param>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>包含总行数和分页结果的Page对象</returns>
        Page<T> AdoPageSp<T>(Func<IDataReader, T> func, string sp, params IDbDataParameter[] paras)
        {
            var page = new Page<T>();
            //打开连接
            try
            {
                OpenSharedConnection();
            }
            catch (Exception x)
            {
                if (OnException(x))
                    throw;
                return page;
            }

            try
            {
                //创建命令
                IDbCommand cmd;
                try
                {
                    cmd = CreateCommandSp(_sharedConnection, sp, paras);
                }
                catch (Exception x)
                {
                    if (OnException(x))
                        throw;
                    return page;
                }

                using (cmd)
                {
                    //创建Reader
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        if (OnException(x))
                            throw;
                        return page;
                    }

                    using (r)
                    {
                        //读取总行数
                        bool b = false;
                        try
                        {
                            b = r.Read();
                        }
                        catch (Exception x)
                        {
                            if (OnException(x))
                                throw;
                        }

                        if (b)
                        {
                            page.TotalRows = (long)r[0];
                            page.Rows = new List<T>();

                            //读取分页数据
                            if (r.NextResult())
                            {
                                while (true)
                                {
                                    try
                                    {
                                        //读取一行
                                        if (!r.Read())
                                            break;
                                        page.Rows.Add(func(r));
                                    }
                                    catch (Exception x)
                                    {
                                        if (OnException(x))
                                            throw;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
            return page;
        }
        #endregion
        #region operation
        /// <summary>
        /// 执行存储过程，返回受影响行数。
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>受影响行数</returns>
        public int ExecuteSp(string sp, params IDbDataParameter[] paras)
        {
            return AdoCommandSp<int>(cmd => cmd.ExecuteNonQuery(), sp, paras);
        }

        /// <summary>
        /// 执行存储过程，返回结果集的首行首列。
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>结果集的首行首列</returns>
        public object ExecuteScalarSp(string sp, params IDbDataParameter[] paras)
        {
            return AdoCommandSp<object>(cmd => cmd.ExecuteScalar(), sp, paras);
        }
        /// <summary>
        /// 执行存储过程，返回结果集的首行首列。
        /// </summary>
        /// <typeparam name="T">将结果值转换为的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>结果集的首行首列</returns>
        public T ExecuteScalarSp<T>(string sp, params IDbDataParameter[] paras)
        {
            return AdoCommandSp<T>(cmd =>
            {
                object scalar = cmd.ExecuteScalar();
                return (T)new Meta(typeof(T)).Convert(scalar);

            }, sp, paras);
        }

        /// <summary>
        /// 执行存储过程，遍历Reader，延迟加载返回结果集的枚举集合。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>结果集的枚举集合</returns>
        public IEnumerable<T> QuerySp<T>(string sp, params IDbDataParameter[] paras)
        {
            Meta meta = new Meta(typeof(T));
            return AdoReaderSp<T>(r => (T)meta.Convert(r), sp, paras);
        }

        /// <summary>
        /// 执行存储过程，结果集必须有且仅有一行，零行或多行均抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>唯一一行数据的对象</returns>
        public T SingleSp<T>(string sp, params IDbDataParameter[] paras)
        {
            return QuerySp<T>(sp, paras).Single();
        }
        /// <summary>
        /// 执行存储过程，结果集必须仅有一行或为空，多行抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>一行数据的对象或default(T)</returns>
        public T SingleOrDefaultSp<T>(string sp, params IDbDataParameter[] paras)
        {
            return QuerySp<T>(sp, paras).SingleOrDefault();
        }

        /// <summary>
        /// 执行存储过程，结果集必须至少包含一行，返回首行，空行抛异常。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>结果集第一行数据的对象</returns>
        public T FirstSp<T>(string sp, params IDbDataParameter[] paras)
        {
            return QuerySp<T>(sp, paras).First();
        }
        /// <summary>
        /// 执行存储过程，返回结果集的第一行或default(T)。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>结果集的第一行或default(T)</returns>
        public T FirstOrDefaultSp<T>(string sp, params IDbDataParameter[] paras)
        {
            return QuerySp<T>(sp, paras).FirstOrDefault();
        }

        /// <summary>
        /// 执行存储过程，返回整个结果集作为一个类型列表。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>表示整个结果集的类型列表</returns>
        public List<T> FetchSp<T>(string sp, params IDbDataParameter[] paras)
        {
            return QuerySp<T>(sp, paras).ToList();
        }
        /// <summary>
        /// 执行存储过程，返回DataTable。
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>DataTable</returns>
        public DataTable FetchSp(string sp, params IDbDataParameter[] paras)
        {
            return AdoCommandSp<DataTable>(cmd =>
            {
                var ada = _Type.CreateDataAdapter();
                ada.SelectCommand = (DbCommand)cmd;

                DataTable dt = new DataTable("DataTable");
                ada.Fill(dt);
                dt.RemotingFormat = SerializationFormat.Binary;
                return dt;

            }, sp, paras);
        }
        /// <summary>
        /// 执行存储过程，返回DataSet。
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>包含多结果集的DataSet</returns>
        public DataSet FetchsSp(string sp, params IDbDataParameter[] paras)
        {
            return AdoCommandSp<DataSet>(cmd =>
            {
                var ada = _Type.CreateDataAdapter();
                ada.SelectCommand = (DbCommand)cmd;

                DataSet ds = new DataSet("DataSet");
                ada.Fill(ds);
                ds.RemotingFormat = SerializationFormat.Binary;
                return ds;

            }, sp, paras);
        }

        /// <summary>
        /// 执行存储过程，返回Page对象。
        /// </summary>
        /// <typeparam name="T">表示结果集一行数据的类型</typeparam>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>包含总行数和分页数据的对象</returns>
        public Page<T> PageSp<T>(string sp, params IDbDataParameter[] paras)
        {
            Meta meta = new Meta(typeof(T));
            var page = AdoPageSp<T>(r => (T)meta.Convert(r), sp, paras);

            //其它值
            page.PageIndex = SqlHelper.GetParamValue<long>(paras, "pageIndex");
            page.PageSize = SqlHelper.GetParamValue<long>(paras, "pageSize");
            page.TotalPages = page.TotalRows % page.PageSize == 0 ? page.TotalRows / page.PageSize : page.TotalRows / page.PageSize + 1;

            return page;
        }
        /// <summary>
        /// 执行存储过程，返回DataSet(第一个Table是总行数结果，第二个Table是分页数据)。
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="paras"></param>
        /// <returns>DataSet(第一个Table是总行数结果，第二个Table是分页数据)</returns>
        public DataSet PageSp(string sp, params IDbDataParameter[] paras)
        {
            return FetchsSp(sp, paras);
        }
        #endregion
        #endregion
    }
    #endregion
    #region DatabaseType
    /// <summary>
    /// 封装用于确定数据库类型的相关方法
    /// </summary>
    public class DatabaseType
    {
        #region 定义数据提供程序名称
        public const string SQLCLIENT = "System.Data.SqlClient";
        public const string ORACLECLIENT = "System.Data.OracleClient";
        public const string ORACLEDATAACCESSCLIENT = "Oracle.DataAccess.Client";
        public const string MYSQLCLIENT = "MySql.Data.MySqlClient";
        public const string OLEDB = "System.Data.OleDb";
        public const string ODBC = "System.Data.Odbc";
        public const string SQLSERVERCE35 = "System.Data.SqlServerCe.3.5";
        public const string SQLSERVERCE40 = "System.Data.SqlServerCe.4.0";
        public const string ENTITYCLIENT = "System.Data.EntityClient";
        public const string SQLITE = "System.Data.SQLite";
        public const string FIREBIRDCLIENT = "FirebirdSql.Data.FirebirdClient";
        public const string NPGSQL = "Npgsql";
        public const string PGSQL = "PgSql";
        public const string INFORMIX = "IBM.Data.Informix";
        public const string DB2 = "IBM.Data.DB2.iSeries";
        #endregion

        #region 构造
        string _connectionString;
        DatabaseProviderEnum _providerEnum;
        string _providerName;
        DbProviderFactory _provider;
        IDbConnection _connection;

        public DatabaseType(string connectionString, DatabaseProviderEnum providerEnum)
        {
            _connectionString = connectionString;
            if (providerEnum == DatabaseProviderEnum.Unknown)
            {
                _providerEnum = DatabaseProviderEnum.SqlClient;
                _providerName = SQLCLIENT;
            }
            else
            {
                _providerEnum = providerEnum;
                _providerName = providerEnum.ToProviderName();
            }

            _provider = OverrideGetFactory(_providerName);
        }
        public DatabaseType(string connectionString, string providerName)
        {
            _connectionString = connectionString;
            if (string.IsNullOrEmpty(providerName))
                _providerName = SQLCLIENT;
            else
                _providerName = providerName;

            _provider = OverrideGetFactory(_providerName);
        }
        public DatabaseType(string connectionString, DbProviderFactory provider)
        {
            _connectionString = connectionString;
            if (provider == null)
            {
                _providerName = SQLCLIENT;
                provider = OverrideGetFactory(_providerName);
            }
            else
            {
                _provider = provider;
            }
        }
        public DatabaseType(IDbConnection connection)
        {
            _connection = connection;
            if (connection != null)
                _connectionString = connection.ConnectionString;
        }

        //新手动创建Provider
        DbProviderFactory OverrideGetFactory(string providerName)
        {
            //return DbProviderFactories.GetFactory(_providerName);
            //if (providerName == MYSQLCLIENT)
            //    return MySql.Data.MySqlClient.MySqlClientFactory.Instance;
            return DbProviderFactories.GetFactory(_providerName);
        }
        #endregion

        #region CreateConnection
        public IDbConnection CreateConnection()
        {
            if (_connection != null)
                return _connection;
            else
                return _provider.CreateConnection();
        }
        #endregion

        #region CreateDataAdapter
        public DbDataAdapter CreateDataAdapter()
        {
            return Provider.CreateDataAdapter();
        }
        #endregion

        #region CreateParameter
        /// <summary>
        /// 创建数据库参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value)
        {
            var p = Provider.CreateParameter();
            if (!string.IsNullOrWhiteSpace(name))
                p.ParameterName = ParameterPrefix + name;

            SetParameter(p, value);
            return p;
        }
        /// <summary>
        /// 创建数据库返回参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DbParameter CreateReturnParameter(string name)
        {
            var p = Provider.CreateParameter();
            if (!string.IsNullOrWhiteSpace(name))
                p.ParameterName = ParameterPrefix + name;

            p.Direction = ParameterDirection.ReturnValue;
            p.DbType = DbType.Int64;
            p.Value = DBNull.Value;
            return p;
        }
        /// <summary>
        /// 创建数据库输出参数 可变长类型需要指定长度
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType, int size)
        {
            var p = CreateOutputParameter(name, dbType);
            p.Size = size;
            return p;
        }
        /// <summary>
        /// 创建数据库输出参数 固定长值类型
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DbParameter CreateOutputParameter(string name, DbType dbType)
        {
            var p = Provider.CreateParameter();
            if (!string.IsNullOrWhiteSpace(name))
                p.ParameterName = ParameterPrefix + name;

            p.Direction = ParameterDirection.Output;
            p.DbType = dbType;
            return p;
        }
        #endregion

        #region 类型判断
        /// <summary>
        /// 提供程序枚举
        /// </summary>
        public DatabaseProviderEnum ProviderEnum
        {
            get
            {
                if (_providerEnum != DatabaseProviderEnum.Unknown)
                    return _providerEnum;

                if (!string.IsNullOrEmpty(_providerName))
                {
                    _providerEnum = _providerName.ToProviderEnum();
                    return _providerEnum;
                }

                if (_provider != null)
                {
                    _providerEnum = _provider.ToProviderEnum();
                    return _providerEnum;
                }

                if (_connection != null)
                {
                    _providerEnum = _connection.ToProviderEnum();
                    return _providerEnum;
                }

                return _providerEnum;
            }
        }
        /// <summary>
        /// 提供程序名
        /// </summary>
        public string ProviderName
        {
            get
            {
                if (!string.IsNullOrEmpty(_providerName))
                    return _providerName;

                _providerName = ProviderEnum.ToProviderName();
                return _providerName;
            }
        }
        /// <summary>
        /// 数据提供程序对象
        /// </summary>
        public DbProviderFactory Provider
        {
            get
            {
                if (_provider == null)
                {
                    _provider = OverrideGetFactory(ProviderName);
                }
                return _provider;
            }
        }

        public bool IsSqlServer
        {
            get
            {
                return ProviderEnum == DatabaseProviderEnum.SqlClient;
            }
        }
        public bool IsMySql
        {
            get
            {
                return ProviderEnum == DatabaseProviderEnum.MySqlClient;
            }
        }
        public bool IsOracle
        {
            get
            {
                return ProviderEnum == DatabaseProviderEnum.OracleClient || ProviderEnum == DatabaseProviderEnum.OracleDataAccessClient;
            }
        }
        public bool SQLite
        {
            get
            {
                return ProviderEnum == DatabaseProviderEnum.SQLite;
            }
        }
        #endregion

        #region 数据库差异
        /// <summary>
        /// MySql的连接字符串配置选项，当为True时，@为变量前缀，?为参数前缀。
        /// </summary>
        public bool AllowUserVariables
        {
            get
            {
                if (_connectionString != null && _connectionString.IndexOf("Allow User Variables=true", StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
                else
                    return false;
            }
        }
        string _ParameterPrefix;
        /// <summary>
        /// 获取参数前缀
        /// </summary>
        public string ParameterPrefix
        {
            get
            {
                if (_ParameterPrefix == null)
                {
                    switch (ProviderEnum)
                    {
                        case DatabaseProviderEnum.OracleClient:
                        case DatabaseProviderEnum.OracleDataAccessClient:
                            _ParameterPrefix = ":"; break;
                        case DatabaseProviderEnum.MySqlClient:
                            _ParameterPrefix = AllowUserVariables ? "?" : "@"; break;
                        default:
                            _ParameterPrefix = "@"; break;
                    }
                }
                return _ParameterPrefix;
            }
        }
        string _ExistsTemplate;
        /// <summary>
        /// 获取查询结果是否存在的SQL语句模板
        /// </summary>
        public string ExistsTemplate
        {
            get
            {
                if (_ExistsTemplate == null)
                {
                    switch (ProviderEnum)
                    {
                        case DatabaseProviderEnum.SqlClient:
                            _ExistsTemplate = "IF EXISTS ({0}) SELECT 1 ELSE SELECT 0"; break;
                        case DatabaseProviderEnum.MySqlClient:
                        case DatabaseProviderEnum.FirebirdClient:
                        case DatabaseProviderEnum.SQLite:
                            _ExistsTemplate = "SELECT EXISTS ({0})"; break;
                        default:
                            _ExistsTemplate = "SELECT COUNT(*) FROM ({0}) AS ttttt"; break;
                    }
                }
                return _ExistsTemplate;
            }
        }
        #region AddBoundary
        bool IsCreateBoundary;
        Func<string, string> _AddBoundary;
        /// <summary>
        /// 获取给名称添加数据库分隔符的转换函数
        /// </summary>
        public Func<string, string> AddBoundary
        {
            get
            {
                if (!IsCreateBoundary)
                {
                    switch (ProviderEnum)
                    {
                        case DatabaseProviderEnum.MySqlClient:
                            _AddBoundary = name => string.Format("`{0}`", name);
                            break;
                        case DatabaseProviderEnum.OracleClient:
                        case DatabaseProviderEnum.OracleDataAccessClient:
                        case DatabaseProviderEnum.FirebirdClient:
                        case DatabaseProviderEnum.PgSql:
                        case DatabaseProviderEnum.Npgsql:
                            _AddBoundary = name => string.Format("\"{0}\"", name);
                            break;
                        default:
                            _AddBoundary = name => string.Format("[{0}]", name);
                            break;
                    }
                    IsCreateBoundary = true;
                }
                return _AddBoundary;
            }
        }
        #endregion

        /// <summary>
        /// 将@替换为当前前缀，并将两个@@替换为一个@。
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>参数替换后的sql语句</returns>
        public string ParameterPrefixReplace(string sql)
        {
            //如果前缀不是@，则将@替换为该ParameterPrefix
            if (ParameterPrefix != "@")
                sql = SqlHelper.ParameterPrefixReplace(sql, ParameterPrefix);

            //将两个@@替换为一个@
            sql = sql.Replace("@@", "@");

            return sql;
        }
        /// <summary>
        /// 以参数的索引作为参数的名称
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public string ParameterIndexName(IDbCommand cmd)
        {
            return string.Format("{0}{1}", ParameterPrefix, cmd.Parameters.Count);
        }
        /// <summary>
        /// 参数值传递给DB前的转换
        /// </summary>
        /// <param name="param"></param>
        /// <param name="value"></param>
        public void SetParameter(IDbDataParameter param, object value)
        {
            if (value == null)
            {
                param.Value = DBNull.Value;
                return;
            }

            var t = value.GetType();
            if (t.IsEnum)
                // PostgreSQL .NET 驱动程序不能转换 enum 到 int
                param.Value = (int)value;
            else if (MetaHelper.IsBool(t))
                switch (ProviderEnum)
                {
                    case DatabaseProviderEnum.PgSql:
                    case DatabaseProviderEnum.Npgsql:
                        param.Value = value; break;
                    default:
                        param.Value = ((bool)value) ? 1 : 0; break;
                }
            else if (MetaHelper.IsUint(t))
                switch (ProviderEnum)
                {
                    case DatabaseProviderEnum.SQLite:
                        param.Value = (long)((uint)value); break;
                    default:
                        param.Value = value; break;
                }
            else if (MetaHelper.IsGuid(t))
            {
                param.Value = value.ToString();
                param.DbType = DbType.String;
                param.Size = 40;
            }
            else if (MetaHelper.IsString(t))
            {
                string strvalue = value as string;
                if (strvalue.Length + 1 > 4000 && param.GetType().Name == "SqlCeParameter")
                    param.SetValue("SqlDbType", SqlDbType.NText);

                //通过使用共同的大小帮助查询计划缓存
                param.Size = Math.Max(strvalue.Length + 1, 4000);
                param.Value = value;
            }
            else if (MetaHelper.IsAnsiString(t))
            {
                // Thanks @DataChomp for pointing out the SQL Server indexing performance hit of using wrong string type on varchar
                string strvalue = (value as AnsiString).Value;
                param.Size = Math.Max(strvalue.Length + 1, 4000);
                param.Value = strvalue;
                param.DbType = DbType.AnsiString;
            }
            else if (t.Name == "SqlGeography")
            {
                //SqlGeography 是 CLR 类型，geography 是等价的  SQL Server 类型
                param.SetValue("UdtTypeName", "geography");
                param.Value = value;
            }
            else if (t.Name == "SqlGeometry")
            {
                //SqlGeometry 是 CLR 类型，geometry 是等价的  SQL Server 类型
                param.SetValue("UdtTypeName", "geometry");
                param.Value = value;
            }
            else
                param.Value = value;
        }
        /// <summary>
        /// 不同提供程序对 IDbCommand 的特性设置。
        /// </summary>
        /// <param name="cmd"></param>
        public void SetCommand(IDbCommand cmd)
        {
            switch (ProviderEnum)
            {
                case DatabaseProviderEnum.OracleClient:
                case DatabaseProviderEnum.OracleDataAccessClient:
                    cmd.SetValue("BindByName", true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 由sql构造sqlPage语句。
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string BuildSkipTake(long skip, long take, string sql)
        {
            //返回全部
            if (take < 1 || skip < 0)
                return sql;

            switch (ProviderEnum)
            {
                case DatabaseProviderEnum.SqlClient:
                case DatabaseProviderEnum.OracleClient:
                case DatabaseProviderEnum.OracleDataAccessClient:
                    return SqlHelper.BuildSkipTake(ProviderEnum == DatabaseProviderEnum.OracleClient || ProviderEnum == DatabaseProviderEnum.OracleDataAccessClient, skip, take, sql);
                case DatabaseProviderEnum.SqlServerCe35:
                case DatabaseProviderEnum.SqlServerCe40:
                    return string.Format("{0} OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY",sql, take, skip);
                default:
                    return string.Format("{0} LIMIT {1} OFFSET {2}", sql, take, skip);
            }
        }
        /// <summary>
        /// 由sql构造sqlCount语句和sqlPage语句。
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        //public Tuple<Sql, Sql> BuildPage(long skip, long take, string sql, params object[] args)
        //{
        //    //返回全部
        //    if (take < 1 || skip < 0)
        //        return new Tuple<Sql, Sql>(null, new Sql(sql, args));

        //    switch (ProviderEnum)
        //    {
        //        case DatabaseProviderEnum.SqlClient:
        //        case DatabaseProviderEnum.OracleClient:
        //        case DatabaseProviderEnum.OracleDataAccessClient:
        //            return SqlHelper.BuildPage(ProviderEnum == DatabaseProviderEnum.OracleClient || ProviderEnum == DatabaseProviderEnum.OracleDataAccessClient, skip, take, sql, args);
        //        case DatabaseProviderEnum.SqlServerCe35:
        //        case DatabaseProviderEnum.SqlServerCe40:
        //            return new Tuple<Sql, Sql>(
        //                new Sql(SqlHelper.BuildCount(sql), args),
        //                new Sql(sql, args).Append("OFFSET @0 ROWS FETCH NEXT @1 ROWS ONLY", take, skip)
        //                );
        //        default:
        //            return new Tuple<Sql, Sql>(
        //                new Sql(SqlHelper.BuildCount(sql), args),
        //                new Sql(sql, args).Append("LIMIT @0 OFFSET @1", take, skip)
        //                );
        //    }
        //}
        #endregion
    }
    #endregion
    #region DatabaseProviderEnum
    /// <summary>
    /// 定义数据提供程序的枚举
    /// </summary>
    public enum DatabaseProviderEnum
    {
        /// <summary>
        /// 未初始化或未知
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// System.Data.SqlClient
        /// </summary>
        SqlClient,
        /// <summary>
        /// System.Data.OracleClient
        /// </summary>
        OracleClient,
        /// <summary>
        /// Oracle.DataAccess.Client
        /// </summary>
        OracleDataAccessClient,
        /// <summary>
        /// MySql.Data.MySqlClient
        /// </summary>
        MySqlClient,
        /// <summary>
        /// System.Data.OleDb
        /// </summary>
        OleDb,
        /// <summary>
        /// System.Data.Odbc
        /// </summary>
        Odbc,
        /// <summary>
        /// System.Data.SqlServerCe.3.5
        /// </summary>
        SqlServerCe35,
        /// <summary>
        /// System.Data.SqlServerCe.4.0
        /// </summary>
        SqlServerCe40,
        /// <summary>
        /// System.Data.EntityClient
        /// </summary>
        EntityClient,

        /// <summary>
        /// System.Data.SQLite
        /// </summary>
        SQLite,
        /// <summary>
        /// FirebirdSql.Data.FirebirdClient
        /// </summary>
        FirebirdClient,
        /// <summary>
        /// Npgsql
        /// </summary>
        Npgsql,
        /// <summary>
        /// PgSql
        /// </summary>
        PgSql,
        /// <summary>
        /// IBM.Data.Informix
        /// </summary>
        Informix,
        /// <summary>
        /// IBM.Data.DB2.iSeries
        /// </summary>
        DB2,
    }
    #endregion
    #region DatabaseExtension
    /// <summary>
    /// 提供用户使用的相关扩展函数
    /// </summary>
    public static class DatabaseExtension
    {
        /// <summary>
        /// Pocoor扩展方法。判断对象是否为默认值。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNull(this object val)
        {
            if (val == null)
                return true;

            string s = val as string;
            if (s != null)
                return string.IsNullOrWhiteSpace(s);

            Type t = val.GetType();
            if (t.IsValueType)
                return object.Equals(val, Activator.CreateInstance(t));
            else
                return val == null;
        }
        /// <summary>
        /// Pocoor扩展方法。返回该类型的默认值。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object DefaultValue(this Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            else
                return null;
        }
        /// <summary>
        /// Pocoor扩展方法。将对象转成指定类型。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="src"></param>
        /// <returns></returns>
        public static T ToType<T>(this object src)
        {
            return (T)new Meta(typeof(T)).Convert(src);
        }
        /// <summary>
        /// Pocoor扩展方法。将本对象的属性值复制到指定对象
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public static void Copy(this object src, object dst)
        {
            if (src == null)
                throw new ArgumentNullException("src");
            if (dst == null)
                throw new ArgumentNullException("dst");

            var m = new Meta(src.GetType());
            m.Copy(src, dst);
        }
        /// <summary>
        /// Pocoor扩展方法。获取对象的指定属性值。若对象为基础类型或属性名为空则直接返回该对象。
        /// </summary>
        /// <param name="poco"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object poco, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName) || poco == null || Convert.IsDBNull(poco))
                return poco;
            return new Meta(poco.GetType()).GetValue(poco, propertyName);
        }
        /// <summary>
        /// Pocoor扩展方法。利用反射获取属性值。
        /// </summary>
        /// <param name="obj">要操作的对象</param>
        /// <param name="propertyName">要获取值的属性名</param>
        /// <returns>对象obj的属性propertyName值</returns>
        public static object GetValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
        /// <summary>
        /// Pocoor扩展方法。利用反射设置属性值。
        /// </summary>
        /// <param name="obj">要操作的对象</param>
        /// <param name="propertyName">要设置值的属性名</param>
        /// <param name="value">要设置的值</param>
        public static void SetValue(this object obj, string propertyName, object value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
        }
        /// <summary>
        /// Pocoor扩展方法。判断对象是否为集合（不包括string和byte[]）。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsCollection(this object obj)
        {
            if (obj == null)
                return false;
            return (obj as System.Collections.IEnumerable) != null && (obj as string) == null && (obj as byte[]) == null;
        }
        /// <summary>
        /// Pocoor扩展方法。枚举类型转int。
        /// </summary>
        /// <param name="enumvalue"></param>
        /// <returns></returns>
        public static int ToInt(this Enum enumvalue)
        {
            Type type = Enum.GetUnderlyingType(enumvalue.GetType());
            return Convert.ToInt32(Convert.ChangeType(enumvalue, type));
        }




        /// <summary>
        /// 转SQL字符串参数：null转NULL，替换单引号为双单引号，加单引号。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSqlVarchar(this string s)
        {
            if (s == null)
                return "NULL";

            s = s.Replace("'", "''");
            return string.Format("'{0}'", s);
        }
        /// <summary>
        /// 转SQL值类型参数：空白转NULL，其它原样输出。
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSqlValueType(this string s)
        {
            if (string.IsNullOrWhiteSpace(s))
                return "NULL";

            return s;
        }
        public static string ToSqlTimespan(this DateTime d)
        {
            if (d == DateTime.MinValue)
                return "NULL";

            return string.Format("'{0}'", d.ToString("yyyy-MM-dd HH:mm:ss.fff")); ;
        }
        public static string ToSqlSencond(this DateTime d)
        {
            if (d == DateTime.MinValue)
                return "NULL";

            return string.Format("'{0}'", d.ToString("yyyy-MM-dd HH:mm:ss")); ;
        }
        public static string ToSqlDate(this DateTime d)
        {
            if (d == DateTime.MinValue)
                return "NULL";

            return string.Format("'{0}'", d.ToString("yyyy-MM-dd")); ;
        }

    }
    #endregion
    #region Sql
    /// <summary>
    /// 将 @参数 统一转成 @索引 参数。
    /// </summary>
    public class Sql
    {
        #region 字段
        //当前语句
        string _sql;
        object[] _args;
        /// <summary>
        /// right-hand-side 右手边指针
        /// </summary>
        Sql _rhs;

        //最终的Sql及参数
        string _sqlFinal;
        object[] _argsFinal;
        #endregion
        #region 构造
        /// <summary>
        /// 空构造
        /// </summary>
        public Sql()
        {
        }
        /// <summary>
        /// 用 SQL 及其参数 构造
        /// </summary>
        /// <param name="sql">SQL</param>
        /// <param name="args">参数L</param>
        public Sql(string sql, params object[] args)
        {
            _sql = sql;
            _args = args;
        }
        #endregion
        #region NewSql
        /// <summary>
        /// 构建一个空实例
        /// </summary>
        public static Sql NewSql
        {
            get { return new Sql(); }
        }
        #endregion
        #region 输出
        /// <summary>
        /// 最终SQL
        /// </summary>
        public string SQL
        {
            get
            {
                Build();
                return _sqlFinal;
            }
        }
        /// <summary>
        /// 最终参数
        /// </summary>
        public object[] Arguments
        {
            get
            {
                Build();
                return _argsFinal;
            }
        }
        #endregion
        #region 构建
        /// <summary>
        /// 生成最终的SQL语句和参数列表
        /// </summary>
        void Build()
        {
            // 已经构建
            if (_sqlFinal != null)
                return;

            // 构建后的 SQL
            var allsql = new StringBuilder();
            // 构建后的 参数列表
            var allargs = new List<object>();

            Build(allsql, allargs, null);

            // 构建后的 SQL
            _sqlFinal = allsql.ToString();
            // 构建后的 参数列表
            _argsFinal = allargs.ToArray();
        }
        /// <summary>
        /// 递归处理当前语句得到总语句和总参数列表
        /// </summary>
        /// <param name="allsql">总语句</param>
        /// <param name="allargs">总参数列表</param>
        /// <param name="lhs">上一条 Sql对象</param>
        void Build(StringBuilder allsql, List<object> allargs, Sql lhs)
        {
            // 添加当前SQL
            if (!String.IsNullOrEmpty(_sql))
            {
                if (allsql.Length > 0)
                {
                    allsql.Append("\n");
                }

                //处理当前语句及参数，将参数添加到总参数列表args，语句输出到sql。
                var sql = SqlHelper.ProcessParams(_sql, _args, allargs);

                //如果当前语句是WHERE语句，且上一条语句也是WHERE语句，则当前语句删除WHERE关键字并用AND连接。
                if (Is(lhs, "WHERE ") && Is(this, "WHERE "))
                    sql = "AND " + sql.Substring(6);

                //如果当前语句是ORDER BY语句，且上一条语句也是ORDER BY语句，则当前语句删除ORDER BY关键字并用逗号连接。
                if (Is(lhs, "ORDER BY ") && Is(this, "ORDER BY "))
                    sql = ", " + sql.Substring(9);

                //将处理后的当前语句加到总语句上
                allsql.Append(sql);
            }

            // 递归右手边
            if (_rhs != null)
                _rhs.Build(allsql, allargs, this);
        }
        /// <summary>
        /// 判断sql是否是指定子句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqltype"></param>
        /// <returns></returns>
        static bool Is(Sql sql, string sqltype)
        {
            return sql != null && sql._sql != null && sql._sql.StartsWith(sqltype, StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion
        #region 构建方法
        /// <summary>
        /// 所有构建方法最终都将调用该方法。
        /// 将另一个 SQL 生成器实例追加到此 SQL 生成器的右手边
        /// </summary>
        /// <param name="sql">对另一个 SQL 生成器实例的引用</param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql Append(Sql sql)
        {
            //重置构建
            _sqlFinal = null;

            //添加到右边或右边的右边
            if (_rhs != null)
                _rhs.Append(sql);
            else
                _rhs = sql;

            //返回自己，添加的永远在这个的后边。
            return this;
        }
        /// <summary>
        /// 将 SQL 片段追加到此 SQL 生成器右右手边
        /// </summary>
        /// <param name="sql">SQL 语句或片段</param>
        /// <param name="args">嵌入 SQL 的参数</param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql Append(string sql, params object[] args)
        {
            return Append(new Sql(sql, args));
        }

        /// <summary>
        /// 将 SELECT 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="columns">SELECT 子句的列名称集合<param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql Select(params object[] columns)
        {
            return Append(new Sql("SELECT " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }
        /// <summary>
        /// 将 FROM 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="tables">FROM 子句的表名称集合</param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql From(params object[] tables)
        {
            return Append(new Sql("FROM " + String.Join(", ", (from x in tables select x.ToString()).ToArray())));
        }
        /// <summary>
        /// 将 WHERE 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="sql">WHERE 子句的条件</param>
        /// <param name="args">嵌入 SQL 的参数</param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql Where(string sql, params object[] args)
        {
            return Append(new Sql("WHERE (" + sql + ")", args));
        }
        /// <summary>
        /// 将 GROUP BY 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="columns">GROUP BY 子句的列名称集合</param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql GroupBy(params object[] columns)
        {
            return Append(new Sql("GROUP BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }
        /// <summary>
        /// 将 ORDER BY 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="columns">ORDER BY 子句的列名称集合</param>
        /// <returns>当前生成器串联后引用</returns>
        public Sql OrderBy(params object[] columns)
        {
            return Append(new Sql("ORDER BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }
        /// <summary>
        /// InnerJoin 的简写
        /// </summary>
        /// <param name="table">INNER JOIN 子句的表名</param>
        /// <returns>一个可以指定联接条件的 SqlJoinClause 引用</returns>
        public SqlJoinClause Join(string table)
        {
            return InnerJoin(table);
        }
        /// <summary>
        /// 将 INNER JOIN 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="table">INNER JOIN 子句的表名</param>
        /// <returns>一个可以指定联接条件的 SqlJoinClause 引用</returns>
        public SqlJoinClause InnerJoin(string table)
        {
            return new SqlJoinClause(Append(new Sql("INNER JOIN " + table)));
        }
        /// <summary>
        /// 将 LEFT JOIN 子句追加到此 SQL 生成器
        /// </summary>
        /// <param name="table">LEFT JOIN 子句的表名</param>
        /// <returns>一个可以指定联接条件的 SqlJoinClause 引用</returns>
        public SqlJoinClause LeftJoin(string table)
        {
            return new SqlJoinClause(Append(new Sql("LEFT JOIN " + table)));
        }
        #endregion
        #region SqlJoinClause
        /// <summary>
        /// SqlJoinClause 是一个简单的帮助器类，用在构建 JOIN 语句中使用 SQL 生成器
        /// </summary>
        public class SqlJoinClause
        {
            private readonly Sql _sql;

            public SqlJoinClause(Sql sql)
            {
                _sql = sql;
            }

            /// <summary>
            /// 在 JOIN 语句后追加 ON 子句
            /// </summary>
            /// <param name="onClause">要追加的 ON 子句</param>
            /// <param name="args">嵌入 SQL 的参数</param>
            /// <returns>串联后的父生成器的引用</returns>
            public Sql On(string onClause, params object[] args)
            {
                return _sql.Append("ON " + onClause, args);
            }
        }
        #endregion
    }
    #endregion
    #region Page
    /// <summary>
    /// 分页结果
    /// </summary>
    public class Page
    {
        /// <summary>
        /// 第几页 
        /// </summary>
        public long PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public long PageSize { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        public long TotalRows { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public long TotalPages { get; set; }

        /// <summary>
        /// 扩展属性供用户使用
        /// </summary>
        public object Tag { get; set; }
    }

    /// <summary>
    /// 分页结果
    /// </summary>
    /// <typeparam name="T">返回结果集的POCO类型</typeparam>
    public class Page<T> : Page
    {
        /// <summary>
        /// 一页的包含的实际行
        /// </summary>
        public List<T> Rows { get; set; }
    }
    #endregion
    #region Attribute
    /// <summary>
    /// 重命名表名或指定PrimaryKey列、AndutoIncrement列。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableAttribute : Attribute
    {
        public TableAttribute()
        {
        }

        public string Name { get; set; }
        public string PrimaryKeyColumn { get; set; }
        public string AutoIncrementColumn { get; set; }
    }
    /// <summary>
    /// 为列指定不同的名称
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; private set; }
    }
    /// <summary>
    /// 指定某列不进行反射
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
    #endregion
    #region Transaction
    public interface ITransaction : IDisposable
    {
        void Complete();
    }
    /// <summary>
    /// 事务对象用于帮助事务深度计数。
    /// </summary>
    public class Transaction : ITransaction
    {
        Database _db;
        public Transaction(Database db)
        {
            _db = db;
            _db.BeginTransaction();
        }
        public void Complete()
        {
            _db.CompleteTransaction();
            _db = null;
        }
        public void Dispose()
        {
            if (_db != null)
                _db.AbortTransaction();
        }
    }
    #endregion
    #region AnsiString
    /// <summary>
    /// string的包装，在传递给DB时强制使用 DBType.AnsiString
    /// </summary>
    public class AnsiString
    {
        /// <summary>
        /// 构造一个AnsiString
        /// </summary>
        /// <param name="str">在传递给DB前将要转换为ANSI的C# string</param>
        public AnsiString(string str)
        {
            Value = str;
        }
        /// <summary>
        /// 在传递给DB前将要转换为ANSI的C# string
        /// </summary>
        public string Value { get; private set; }
    }
    #endregion

    #region Internal
    namespace Internal
    {
        #region DatabaseInnerExtension
        /// <summary>
        /// 内部使用的相关扩展函数
        /// </summary>
        static class DatabaseInnerExtension
        {
            /// <summary>
            /// 提供程序枚举 转 提供程序名
            /// </summary>
            /// <param name="providerEnum">提供程序枚举</param>
            /// <returns>提供程序名</returns>
            public static string ToProviderName(this DatabaseProviderEnum providerEnum)
            {
                switch (providerEnum)
                {
                    case DatabaseProviderEnum.Unknown:
                        return string.Empty;

                    case DatabaseProviderEnum.SqlClient:
                        return DatabaseType.SQLCLIENT;
                    case DatabaseProviderEnum.OracleClient:
                        return DatabaseType.ORACLECLIENT;
                    case DatabaseProviderEnum.OracleDataAccessClient:
                        return DatabaseType.ORACLEDATAACCESSCLIENT;
                    case DatabaseProviderEnum.MySqlClient:
                        return DatabaseType.MYSQLCLIENT;
                    case DatabaseProviderEnum.OleDb:
                        return DatabaseType.OLEDB;
                    case DatabaseProviderEnum.Odbc:
                        return DatabaseType.ODBC;
                    case DatabaseProviderEnum.SqlServerCe35:
                        return DatabaseType.SQLSERVERCE35;
                    case DatabaseProviderEnum.SqlServerCe40:
                        return DatabaseType.SQLSERVERCE40;
                    case DatabaseProviderEnum.EntityClient:
                        return DatabaseType.ENTITYCLIENT;
                    case DatabaseProviderEnum.SQLite:
                        return DatabaseType.SQLITE;
                    case DatabaseProviderEnum.FirebirdClient:
                        return DatabaseType.FIREBIRDCLIENT;
                    case DatabaseProviderEnum.Npgsql:
                        return DatabaseType.NPGSQL;
                    case DatabaseProviderEnum.PgSql:
                        return DatabaseType.PGSQL;
                    case DatabaseProviderEnum.Informix:
                        return DatabaseType.INFORMIX;
                    case DatabaseProviderEnum.DB2:
                        return DatabaseType.DB2;
                }
                return string.Empty;
            }
            /// <summary>
            /// 提供程序名 转 提供程序枚举
            /// </summary>
            /// <param name="providerName">提供程序名</param>
            /// <returns>提供程序枚举</returns>
            public static DatabaseProviderEnum ToProviderEnum(this string providerName)
            {
                switch (providerName)
                {
                    case DatabaseType.SQLCLIENT:
                        return DatabaseProviderEnum.SqlClient;
                    case DatabaseType.ORACLECLIENT:
                        return DatabaseProviderEnum.OracleClient;
                    case DatabaseType.ORACLEDATAACCESSCLIENT:
                        return DatabaseProviderEnum.OracleDataAccessClient;
                    case DatabaseType.MYSQLCLIENT:
                        return DatabaseProviderEnum.MySqlClient;
                    case DatabaseType.OLEDB:
                        return DatabaseProviderEnum.OleDb;
                    case DatabaseType.ODBC:
                        return DatabaseProviderEnum.Odbc;
                    case DatabaseType.SQLSERVERCE35:
                        return DatabaseProviderEnum.SqlServerCe35;
                    case DatabaseType.SQLSERVERCE40:
                        return DatabaseProviderEnum.SqlServerCe40;
                    case DatabaseType.ENTITYCLIENT:
                        return DatabaseProviderEnum.EntityClient;
                    case DatabaseType.SQLITE:
                        return DatabaseProviderEnum.SQLite;
                    case DatabaseType.FIREBIRDCLIENT:
                        return DatabaseProviderEnum.FirebirdClient;
                    case DatabaseType.NPGSQL:
                        return DatabaseProviderEnum.Npgsql;
                    case DatabaseType.PGSQL:
                        return DatabaseProviderEnum.PgSql;
                    case DatabaseType.INFORMIX:
                        return DatabaseProviderEnum.Informix;
                    case DatabaseType.DB2:
                        return DatabaseProviderEnum.DB2;
                }
                return DatabaseProviderEnum.Unknown;
            }

            /// <summary>
            /// 由 DbProviderFactory 得到提供程序枚举
            /// </summary>
            /// <param name="provider">提供程序</param>
            /// <returns>提供程序枚举</returns>
            public static DatabaseProviderEnum ToProviderEnum(this DbProviderFactory provider)
            {
                if (provider == null)
                    return DatabaseProviderEnum.Unknown;

                Type t = provider.GetType();
                Version v = t.Assembly.GetName().Version;
                int major = v.Major;
                switch (t.FullName)
                {
                    case "System.Data.SqlClient.SqlClientFactory":
                        return DatabaseProviderEnum.SqlClient;
                    case "System.Data.OracleClient.OracleClientFactory":
                        return DatabaseProviderEnum.OracleClient;
                    case "Oracle.DataAccess.Client.OracleClientFactory":
                        return DatabaseProviderEnum.OracleDataAccessClient;
                    case "MySql.Data.MySqlClient.MySqlClientFactory":
                        return DatabaseProviderEnum.MySqlClient;
                    case "System.Data.OleDb.OleDbFactory":
                        return DatabaseProviderEnum.OleDb;
                    case "System.Data.Odbc.OdbcFactory":
                        return DatabaseProviderEnum.Odbc;
                    case "System.Data.SqlServerCe.SqlCeProviderFactory":
                        return major > 3 ? DatabaseProviderEnum.SqlServerCe40 : DatabaseProviderEnum.SqlServerCe35;
                    case "System.Data.EntityClient.EntityProviderFactory":
                        return DatabaseProviderEnum.EntityClient;
                    case "System.Data.SQLite.SQLiteFactory":
                        return DatabaseProviderEnum.SQLite;
                    case "FirebirdSql.Data.FirebirdClient.FirebirdClientFactory":
                        return DatabaseProviderEnum.FirebirdClient;
                    case "Npgsql.NpgsqlFactory":
                        return DatabaseProviderEnum.Npgsql;
                    case "PgSql.PgSqlFactory":
                        return DatabaseProviderEnum.PgSql;
                    case "IBM.Data.Informix.IfxFactory":
                        return DatabaseProviderEnum.Informix;
                    case "IBM.Data.DB2.iSeries.DB2Factory":
                        return DatabaseProviderEnum.DB2;
                }
                return DatabaseProviderEnum.Unknown;
            }
            /// <summary>
            /// 由 IDbConnection 得到提供程序枚举
            /// </summary>
            /// <param name="connection">连接对象</param>
            /// <returns>提供程序枚举</returns>
            public static DatabaseProviderEnum ToProviderEnum(this IDbConnection connection)
            {
                if (connection == null)
                    return DatabaseProviderEnum.Unknown;

                Type t = connection.GetType();
                Version v = t.Assembly.GetName().Version;
                int major = v.Major;
                switch (t.FullName)
                {
                    case "System.Data.SqlClient.SqlConnection":
                        return DatabaseProviderEnum.SqlClient;
                    case "System.Data.OracleClient.OracleConnection":
                        return DatabaseProviderEnum.OracleClient;
                    case "Oracle.DataAccess.Client.OracleConnection":
                        return DatabaseProviderEnum.OracleDataAccessClient;
                    case "MySql.Data.MySqlClient.MySqlConnection":
                        return DatabaseProviderEnum.MySqlClient;
                    case "System.Data.OleDb.OleDbConnection":
                        return DatabaseProviderEnum.OleDb;
                    case "System.Data.Odbc.OdbcConnection":
                        return DatabaseProviderEnum.Odbc;
                    case "System.Data.SqlServerCe.SqlCeConnection":
                        return major > 3 ? DatabaseProviderEnum.SqlServerCe40 : DatabaseProviderEnum.SqlServerCe35;
                    case "System.Data.EntityClient.EntityConnection":
                        return DatabaseProviderEnum.EntityClient;
                    case "System.Data.SQLite.SQLiteConnection":
                        return DatabaseProviderEnum.SQLite;
                    case "FirebirdSql.Data.FirebirdClient.FirebirdConnection":
                        return DatabaseProviderEnum.FirebirdClient;
                    case "Npgsql.NpgsqlConnection":
                        return DatabaseProviderEnum.Npgsql;
                    case "PgSql.PgSqlConnection":
                        return DatabaseProviderEnum.PgSql;
                    case "IBM.Data.Informix.IfxConnection":
                        return DatabaseProviderEnum.Informix;
                    case "IBM.Data.DB2.iSeries.DB2Connection":
                        return DatabaseProviderEnum.DB2;
                }
                return DatabaseProviderEnum.Unknown;
            }

        }
        #endregion
        #region SqlHelper
        static class SqlHelper
        {
            #region 正则表达式定义
            /// <summary>
            /// 参数前缀正则表达式 @name 或 @0
            /// </summary>
            public static Regex regexParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);
            /// <summary>
            /// 匹配 SELECT ... FROM 的正则
            /// </summary>
            public static Regex regexColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            /// <summary>
            /// 匹配 ORDER BY ... ASC|DESC 的正则
            /// </summary>
            public static Regex regexOrderBy = new Regex(@"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            /// <summary>
            /// 匹配 DISTINCT 的正则
            /// </summary>
            public static Regex regexDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
            #endregion

            /// <summary>
            /// 将 @参数 替换为 newPrefix参数。
            /// </summary>
            /// <param name="sql"></param>
            /// <param name="newPrefix">要将@替换为的前缀</param>
            /// <returns>替换后的字符串</returns>
            public static string ParameterPrefixReplace(string sql, string newPrefix)
            {
                return regexParamsPrefix.Replace(sql, m => newPrefix + m.Value.Substring(1));
            }

            /// <summary>
            /// 处理@name参数：
            /// 将语句中的数字参数、命名参数替换为最终的数字索引参数，集合参数替换成多个最终的数字索引参数。
            /// 将参数列表添加到最终的参数列表，命名参数取原参数的属性，集合参数转成多个参数添加到最终的参数列表。
            /// </summary>
            /// <param name="sql">原 SQL 语句</param>
            /// <param name="args_src">原 参数列表</param>
            /// <param name="args_dest">处理后的 参数列表(最终的参数列表，命名参数取原参数的属性，原集合参数转成多个参数)</param>
            /// <returns>处理后的 SQL 语句(原数字参数、命名参数替换为最终的数字索引参数，集合参数替换成多个最终的数字索引参数)</returns>
            public static string ProcessParams(string sql, object[] args_src, List<object> args_dest)
            {
                //处理@name参数
                return regexParamsPrefix.Replace(sql, new MatchEvaluator(m =>
                {
                    //得到参数名
                    string param = m.Value.Substring(1);

                    //判断args_src是否为空
                    if (args_src == null)
                        throw new ArgumentOutOfRangeException(string.Format("语句 `{0}` 中指定了参数 '@{1}'，但没有传递参数。", sql, param));

                    //参数值
                    object arg_val;

                    //数字参数
                    int paramIndex;
                    if (int.TryParse(param, out paramIndex))
                    {
                        //参数索引超出范围
                        if (paramIndex < 0 || paramIndex >= args_src.Length)
                            throw new ArgumentOutOfRangeException(string.Format("语句 `{0}` 中指定了参数 '@{1}'，但传递的参数只有 {2} 个。", sql, paramIndex, args_src.Length));
                        //取得参数值
                        arg_val = args_src[paramIndex];
                    }
                    //命名参数
                    else
                    {
                        // 即一定是一个参数的属性
                        bool found = false;
                        arg_val = null;
                        foreach (var o in args_src)
                        {
                            var pi = o.GetType().GetProperty(param);
                            if (pi != null)
                            {
                                //取得参数值
                                arg_val = pi.GetValue(o, null);
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            throw new ArgumentException(string.Format("语句 `{0}` 中指定了参数 '@{1}'，但传递的参数里没有这样的一个属性。", sql, param));
                    }

                    // 如果参数值是一个集合
                    if (arg_val.IsCollection())
                    {
                        var sb = new StringBuilder();
                        //将集合里的每一个值当作一个单独的参数
                        foreach (var i in arg_val as System.Collections.IEnumerable)
                        {
                            sb.Append((sb.Length == 0 ? "@" : ",@") + args_dest.Count.ToString());
                            args_dest.Add(i);
                        }
                        //用多个最终的数字索引参数替换原语句里的参数
                        return sb.ToString();
                    }
                    else
                    {
                        //将参数添加到最终输出集合
                        args_dest.Add(arg_val);
                        //用最终的数字索引参数替换原语句里的参数
                        return "@" + (args_dest.Count - 1).ToString();
                    }
                }
                ));
            }

            /// <summary>
            /// 从paras里找出名称为name的参数值。
            /// </summary>
            /// <typeparam name="T">参数类型</typeparam>
            /// <param name="paras"></param>
            /// <param name="name"></param>
            /// <returns>参数值</returns>
            public static T GetParamValue<T>(IDbDataParameter[] paras, string name)
            {
                if (paras == null || paras.Length < 1)
                    return default(T);
                foreach (var p in paras)
                {
                    if (string.Compare(name, p.ParameterName, true) == 0)
                        return (T)p.Value;
                }
                return default(T);
            }


            #region Page
            /// <summary>
            /// 由sql语句生成 COUNT 语句。
            /// </summary>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static string BuildCount(string sql)
            {
                // 匹配 "SELECT <whatever> FROM" 中的 columns
                Match m = regexColumns.Match(sql);
                if (!m.Success)
                    throw new Exception(string.Format("`{0}` 中匹配 regexColumns 失败。", sql));
                Group g = m.Groups[1];
                //SELECT 之后的部分
                string sqlCount = sql.Substring(g.Index);

                //替换列部分得到如下形式 SELECT COUNT(*)或COUNT(DISTINCT columns) FROM ...
                if (regexDistinct.IsMatch(sqlCount))
                    sqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " + sql.Substring(g.Index + g.Length);
                else
                    sqlCount = sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);

                // 去除排序子句
                m = regexOrderBy.Match(sqlCount);
                if (m.Success)
                {
                    g = m.Groups[0];
                    sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
                }
                return sqlCount;
            }
            /// <summary>
            /// SqlServer和Oracle中构造sqlPage语句。
            /// </summary>
            /// <param name="mustAliasStar">验证Oracle子查询中*必须带别名。例如 select t.* from table t order by t.id</param>
            /// <param name="skip"></param>
            /// <param name="take"></param>
            /// <param name="sql"></param>
            /// <returns></returns>
            public static string BuildSkipTake(bool mustAliasStar, long skip, long take, string sql)
            {
                // 匹配 "SELECT <whatever> FROM" 中的 columns
                Match m = regexColumns.Match(sql);
                if (!m.Success)
                    throw new Exception(string.Format("`{0}` 中匹配 regexColumns 失败。", sql));
                Group g = m.Groups[1];
                //SELECT 之后的部分
                string sqlPart = sql.Substring(g.Index);

                //验证Oracle子查询中*必须带别名。例如 select t.* from table t order by t.id
                if (mustAliasStar && sqlPart.StartsWith("*"))
                    throw new Exception(string.Format("`{0}` 构造分页查询时*必须带别名。例如 select t.* from table t order by t.id", sql));

                //去除排序子句
                string sqlPartNoOrderBy = regexOrderBy.Replace(sqlPart, "", 1);

                //当包含 DISTINCT 时，转成子查询
                if (regexDistinct.IsMatch(sqlPartNoOrderBy))
                    sqlPartNoOrderBy = "pocoor_inner.* FROM (SELECT " + sqlPart + ") pocoor_inner";

                // 排序子句
                string sqlOrderby = "ORDER BY (SELECT NULL)";
                m = regexOrderBy.Match(sqlPart);
                if (m.Success)
                    sqlOrderby = m.Groups[0].ToString();

                return string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) pocoor_rn, {1}) pocoor_paged WHERE pocoor_rn>{2} AND pocoor_rn<={3}", sqlOrderby, sqlPartNoOrderBy, skip, skip + take);
            }
            /// <summary>
            /// SqlServer和Oracle中构造sqlCount语句和sqlPage语句。
            /// </summary>
            /// <param name="mustAliasStar"></param>
            /// <param name="skip"></param>
            /// <param name="take"></param>
            /// <param name="sql"></param>
            /// <param name="args"></param>
            /// <returns></returns>
            //public static Tuple<Sql, Sql> BuildPage(bool mustAliasStar, long skip, long take, string sql, params object[] args)
            //{
            //    // 匹配 "SELECT <whatever> FROM" 中的 columns
            //    Match m = regexColumns.Match(sql);
            //    if (!m.Success)
            //        throw new Exception(string.Format("`{0}` 中匹配 regexColumns 失败。", sql));
            //    Group g = m.Groups[1];
            //    //SELECT 之后的部分
            //    string sqlPart = sql.Substring(g.Index);

            //    //验证Oracle子查询中*必须带别名。例如 select t.* from table t order by t.id
            //    if (mustAliasStar && sqlPart.StartsWith("*"))
            //        throw new Exception(string.Format("`{0}` 构造分页查询时*必须带别名。例如 select t.* from table t order by t.id", sql));

            //    //替换列部分得到如下形式 SELECT COUNT(*)或COUNT(DISTINCT columns) FROM ...
            //    string sqlCount = regexDistinct.IsMatch(sqlPart) ?
            //        sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " + sql.Substring(g.Index + g.Length) :
            //        sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);

            //    // 排序子句
            //    string sqlOrderby = "ORDER BY (SELECT NULL)";
            //    m = regexOrderBy.Match(sqlCount);
            //    if (m.Success)
            //    {
            //        g = m.Groups[0];
            //        sqlOrderby = g.ToString();
            //        //sqlCount去除排序子句
            //        sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            //    }

            //    //sqlPart去除排序子句
            //    sqlPart = regexOrderBy.Replace(sqlPart, "", 1);

            //    //当包含 DISTINCT 时，转成子查询
            //    if (regexDistinct.IsMatch(sqlPart))
            //        sqlPart = "pocoor_inner.* FROM (SELECT " + sqlPart + ") pocoor_inner";

            //    return new Tuple<Sql, Sql>(
            //        new Sql(sqlCount, args),
            //        new Sql(string.Format("SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) pocoor_rn, {1}) pocoor_paged WHERE pocoor_rn>@{2} AND pocoor_rn<=@{3}", sqlOrderby, sqlPart, args.Length, args.Length + 1), args.Concat(new object[] { skip, skip + take }).ToArray())
            //        );
            //}
            /// <summary>
            /// 当返回全部结果时，构造分页样式的DataSet。
            /// </summary>
            /// <param name="dtPage"></param>
            /// <returns></returns>
            public static DataSet BuildDataSet(DataTable dtPage)
            {
                dtPage.TableName = "dtPage";

                DataTable dtCount = new DataTable("dtCount");
                dtCount.Columns.Add("totalRows", typeof(long));
                DataRow row = dtCount.NewRow();
                row[0] = dtPage.Rows.Count;
                dtCount.Rows.Add(row);

                DataSet ds = new DataSet("DataSet");
                ds.Tables.Add(dtCount);
                ds.Tables.Add(dtPage);
                return ds;
            }
            #endregion
        }
        #endregion
        #region PocoToSql
        static class PocoToSql
        {
            #region Where
            /// <summary>
            /// select * from type类名或TableAttribute指定的表名 where 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="sqlTemplate"></param>
            /// <param name="tableType"></param>
            /// <param name="poco"></param>
            /// <param name="tableName"></param>
            /// <param name="columns">当未指定时取TableAttribute指定主键列或自增列，当仍未指定时，无查询条件返回全部结果。</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            static Sql WhereTemplate(string sqlTemplate, Type tableType, object poco, string tableName, string columns, Func<string, string> addBoundary)
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    if (tableType == null)
                    {
                        if (poco == null)
                            throw new Exception("poco对象和元数据类型不能同时为空");
                        else
                            tableType = poco.GetType();
                    }
                    var att = MetaHelper.GetTableAttribute(tableType);
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                    if (string.IsNullOrWhiteSpace(columns))
                        columns = att.PrimaryKeyColumn;
                    if (string.IsNullOrWhiteSpace(columns))
                        columns = att.AutoIncrementColumn;
                }

                Sql sql = new Sql(string.Format(sqlTemplate, addBoundary == null ? tableName : addBoundary(tableName)));
                if (poco == null || string.IsNullOrWhiteSpace(columns))
                    return sql;

                Type pocoType = poco.GetType();
                Meta pocoMeta = new Meta(pocoType);
                string[] listColumns = columns.Split(',');
                foreach (var column in listColumns)
                {
                    sql.Where(addBoundary == null ? column : addBoundary(column) + "=@0", pocoMeta.GetValue(poco, column));
                }
                return sql;
            }
            /// <summary>
            /// SELECT * FROM type类名或TableAttribute指定的表名 WHERE 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="tableType"></param>
            /// <param name="poco"></param>
            /// <param name="tableName"></param>
            /// <param name="columns">当未指定时取TableAttribute指定主键列或自增列，当仍未指定时，无查询条件返回全部结果。</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            public static Sql Where(Type tableType, object poco, string tableName, string columns, Func<string, string> addBoundary)
            {
                return WhereTemplate("SELECT * FROM {0}", tableType, poco, tableName, columns, addBoundary);
            }
            /// <summary>
            /// DELETE FROM type类名或TableAttribute指定的表名 WHERE 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="tableType"></param>
            /// <param name="poco"></param>
            /// <param name="tableName"></param>
            /// <param name="columns">当未指定时取TableAttribute指定主键列或自增列，当仍未指定时，无查询条件返回全部结果。</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            public static Sql Delete(Type tableType, object poco, string tableName, string columns, Func<string, string> addBoundary)
            {
                return WhereTemplate("DELETE FROM {0}", tableType, poco, tableName, columns, addBoundary);
            }
            #endregion
            #region WhereCondition
            /// <summary>
            /// select * from type类名或TableAttribute指定的表名 where 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="sqlTemplate"></param>
            /// <param name="tableType"></param>
            /// <param name="whereCondition">key=@0</param>
            /// <param name="whereConditionArgs">@0对应的Value</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            static Sql WhereConditionTemplate(string sqlTemplate, Type tableType, string whereCondition, object[] whereConditionArgs, Func<string, string> addBoundary)
            {
                if (tableType == null)
                    throw new Exception("WhereConditionTemplate元数据类型不能为空");

                var att = MetaHelper.GetTableAttribute(tableType);
                string tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                Sql sql = new Sql(string.Format(sqlTemplate, addBoundary == null ? tableName : addBoundary(tableName)));
                if (string.IsNullOrWhiteSpace(whereCondition))
                    return sql;
                sql.Where(whereCondition, whereConditionArgs);
                return sql;
            }
            /// <summary>
            /// SELECT * FROM type类名或TableAttribute指定的表名 WHERE 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="tableType"></param>
            /// <param name="whereCondition">key=@0</param>
            /// <param name="whereConditionArgs">@0对应的Value</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            public static Sql WhereCondition(Type tableType, string whereCondition, object[] whereConditionArgs, Func<string, string> addBoundary)
            {
                return WhereConditionTemplate("SELECT * FROM {0}", tableType, whereCondition, whereConditionArgs, addBoundary);
            }
            /// <summary>
            /// DELETE FROM type类名或TableAttribute指定的表名 WHERE 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="tableType"></param>
            /// <param name="whereCondition">key=@0</param>
            /// <param name="whereConditionArgs">@0对应的Value</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            public static Sql DeleteCondition(Type tableType, string whereCondition, object[] whereConditionArgs, Func<string, string> addBoundary)
            {
                return WhereConditionTemplate("DELETE FROM {0}", tableType, whereCondition, whereConditionArgs, addBoundary);
            }
            #endregion
            #region Wheres
            /// <summary>
            /// select * from type类名或TableAttribute指定的表名 where 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="sqlTemplate"></param>
            /// <param name="tableType"></param>
            /// <param name="pocos"></param>
            /// <param name="tableName"></param>
            /// <param name="columns">当未指定时取TableAttribute指定主键列或自增列，当仍未指定时，无查询条件返回全部结果。</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            static Sql WheresTemplate(string sqlTemplate, Type tableType, IEnumerable pocos, string tableName, string columns, Func<string, string> addBoundary)
            {
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    if (tableType == null)
                    {
                        if (pocos == null)
                            throw new Exception("pocos集合和元数据类型不能同时为空");
                        else
                            tableType = pocos.AsQueryable().ElementType;
                    }
                    var att = MetaHelper.GetTableAttribute(tableType);
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                    if (string.IsNullOrWhiteSpace(columns))
                        columns = att.PrimaryKeyColumn;
                    if (string.IsNullOrWhiteSpace(columns))
                        columns = att.AutoIncrementColumn;
                }
                string sqlBase = string.Format(sqlTemplate, addBoundary == null ? tableName : addBoundary(tableName));
                if (pocos == null || string.IsNullOrWhiteSpace(columns))
                    return new Sql(sqlBase);

                Type pocoType = pocos.AsQueryable().ElementType;
                Meta pocoMeta = new Meta(pocoType);
                string[] listColumns = columns.Split(',');
                Sql sqls = new Sql();
                bool isnull = true;
                foreach (var poco in pocos)
                {
                    if (poco != null)
                    {
                        isnull = false;
                        Sql onesql = new Sql(sqlBase);
                        foreach (var column in listColumns)
                        {
                            onesql.Where(addBoundary == null ? column : addBoundary(column) + "=@0", pocoMeta.GetValue(poco, column));
                        }
                        sqls.Append(onesql);
                    }
                }
                if (isnull)
                    return new Sql(sqlBase);
                return sqls;
            }
            /// <summary>
            /// SELECT * FROM type类名或TableAttribute指定的表名 WHERE 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="tableType"></param>
            /// <param name="pocos"></param>
            /// <param name="tableName"></param>
            /// <param name="columns">当未指定时取TableAttribute指定主键列或自增列，当仍未指定时，无查询条件返回全部结果。</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            public static Sql Wheres(Type tableType, IEnumerable pocos, string tableName, string columns, Func<string, string> addBoundary)
            {
                return WheresTemplate("SELECT * FROM {0}", tableType, pocos, tableName, columns, addBoundary);
            }
            /// <summary>
            /// DELETE FROM type类名或TableAttribute指定的表名 WHERE 指定列或主键列或自增列=poco或poco.属性
            /// </summary>
            /// <param name="tableType"></param>
            /// <param name="pocos"></param>
            /// <param name="tableName"></param>
            /// <param name="columns">当未指定时取TableAttribute指定主键列或自增列，当仍未指定时，无查询条件返回全部结果。</param>
            /// <param name="addBoundary"></param>
            /// <returns></returns>
            public static Sql Deletes(Type tableType, IEnumerable pocos, string tableName, string columns, Func<string, string> addBoundary)
            {
                return WheresTemplate("DELETE FROM {0}", tableType, pocos, tableName, columns, addBoundary);
            }
            #endregion
            #region Insert
            public static Sql Insert(Type tableType, object poco, string tableName, string includeColumns, string excludeColumns, Func<string, string> addBoundary)
            {
                if (poco == null)
                    throw new Exception("Insert的poco对象不能为空");
                if (tableType == null)
                    tableType = poco.GetType();

                var att = MetaHelper.GetTableAttribute(tableType);
                if (string.IsNullOrWhiteSpace(tableName))
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                string insertsql = string.Format("INSERT INTO {0} ({{0}}) VALUES ({{1}});", addBoundary == null ? tableName : addBoundary(tableName));

                Meta pocoMeta = new Meta(poco.GetType());
                List<string> listNames = new List<string>();
                List<string> listParams = new List<string>();
                List<object> listValues = new List<object>();
                Foreach(includeColumns, tableType, excludeColumns, att.AutoIncrementColumn, (column, i) =>
                {
                    listNames.Add(addBoundary == null ? column : addBoundary(column));
                    listParams.Add(string.Format("@{0}", i));
                    listValues.Add(pocoMeta.GetValue(poco, column));
                });

                insertsql = string.Format(insertsql, string.Join(",", listNames), string.Join(",", listParams));
                return new Sql(insertsql, listValues.ToArray());
            }
            static void Foreach(string includeColumns, Type tableType, string excludeColumns, string autoIncrementColumn, Action<string, int> action)
            {
                int i = 0;
                if (!string.IsNullOrWhiteSpace(includeColumns))
                {
                    string[] insertColumns = includeColumns.Split(',');
                    foreach (string column in insertColumns)
                    {
                        action(column, i);
                        i++;
                    }
                }
                else
                {
                    Dictionary<string, PropertyInfo> allColumns = MetaHelper.CreatePropertiesDict(tableType);
                    string[] excludes = string.IsNullOrWhiteSpace(excludeColumns) ? null : excludeColumns.Split(',');
                    foreach (string column in allColumns.Keys)
                    {
                        if (string.Compare(column, autoIncrementColumn, true) == 0)
                            continue;

                        bool b = true;
                        if (excludes != null)
                            foreach (string exclude in excludes)
                                if (string.Compare(column, exclude, true) == 0)
                                {
                                    b = false;
                                    break;
                                }

                        if (b)
                        {
                            action(column, i);
                            i++;
                        }
                    }
                }
            }
            public static Tuple<Sql, DbParameter, string> InsertAutoIncrement(Type tableType, object poco, string tableName, string includeColumns, string excludeColumns, string autoIncrementColumn, DatabaseType databaseType)
            {
                if (poco == null)
                    throw new Exception("InsertAutoIncrement的poco对象不能为空");
                if (tableType == null)
                    tableType = poco.GetType();

                var att = MetaHelper.GetTableAttribute(tableType);
                if (string.IsNullOrWhiteSpace(autoIncrementColumn))
                    autoIncrementColumn = att.AutoIncrementColumn;
                if (string.IsNullOrWhiteSpace(autoIncrementColumn))
                    throw new Exception("InsertAutoIncrement的autoIncrementColumn列不能为空");
                string dbAutoIncrementColumn = databaseType.AddBoundary == null ? autoIncrementColumn : databaseType.AddBoundary(autoIncrementColumn);

                Sql insertsql = Insert(tableType, poco, tableName, includeColumns, string.IsNullOrWhiteSpace(excludeColumns) ? autoIncrementColumn : excludeColumns + "," + autoIncrementColumn, databaseType.AddBoundary);
                Sql sql = null;
                DbParameter dbParameter = null;
                string nextsql = null;
                string sql1 = insertsql.SQL;
                object[] args = insertsql.Arguments;
                string newsql, sql2;
                switch (databaseType.ProviderEnum)
                {
                    case DatabaseProviderEnum.SqlClient:
                        newsql = sql1;
                        sql2 = string.Format("OUTPUT INSERTED.{0} VALUES", dbAutoIncrementColumn);
                        newsql = newsql.Replace("VALUES", sql2);
                        sql = new Sql(newsql, args);
                        break;
                    case DatabaseProviderEnum.OracleClient:
                    case DatabaseProviderEnum.OracleDataAccessClient:
                        newsql = sql1.Trim().TrimEnd(';');
                        sql = new Sql(newsql, args);
                        sql2 = string.Format("returning {0} into @0;", dbAutoIncrementColumn);
                        dbParameter = databaseType.CreateReturnParameter("newid");// :newid
                        sql.Append(sql2, dbParameter);
                        break;
                    case DatabaseProviderEnum.SqlServerCe35:
                    case DatabaseProviderEnum.SqlServerCe40:
                        sql = insertsql;
                        nextsql = "SELECT @@@IDENTITY AS NewID;";
                        break;
                    case DatabaseProviderEnum.SQLite:
                        sql = insertsql;
                        sql2 = "SELECT last_insert_rowid();";
                        sql.Append(sql2);
                        break;
                    case DatabaseProviderEnum.PgSql:
                    case DatabaseProviderEnum.Npgsql:
                        newsql = sql1.Trim().TrimEnd(';');
                        sql = new Sql(newsql, args);
                        sql2 = string.Format("returning {3} as NewID;", dbAutoIncrementColumn);
                        sql.Append(sql2);
                        break;
                    default:
                        sql = insertsql;
                        sql2 = "SELECT @@@IDENTITY AS NewID;";
                        sql.Append(sql2);
                        break;

                }
                return new Tuple<Sql, DbParameter, string>(sql, dbParameter, nextsql);
            }
            public static Sql Inserts(Type tableType, IEnumerable pocos, string tableName, string includeColumns, string excludeColumns, Func<string, string> addBoundary)
            {
                if (pocos == null)
                    throw new Exception("Inserts的pocos集合不能为空");
                if (tableType == null)
                    tableType = pocos.AsQueryable().ElementType;

                var att = MetaHelper.GetTableAttribute(tableType);
                if (string.IsNullOrWhiteSpace(tableName))
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                string insertsql = string.Format("INSERT INTO {0} ({{0}}) VALUES ({{1}});", addBoundary == null ? tableName : addBoundary(tableName));
                Meta pocoMeta = new Meta(pocos.AsQueryable().ElementType);

                Sql sql = Sql.NewSql;
                foreach (object poco in pocos)
                {
                    List<string> listNames = new List<string>();
                    List<string> listParams = new List<string>();
                    List<object> listValues = new List<object>();
                    Foreach(includeColumns, tableType, excludeColumns, att.AutoIncrementColumn, (column, i) =>
                    {
                        listNames.Add(addBoundary == null ? column : addBoundary(column));
                        listParams.Add(string.Format("@{0}", i));
                        listValues.Add(pocoMeta.GetValue(poco, column));
                    });

                    string onesql = string.Format(insertsql, string.Join(",", listNames), string.Join(",", listParams));
                    sql.Append(onesql, listValues.ToArray());
                }
                return sql;
            }
            #endregion
            #region Update
            public static Sql UpdateCondition(Type tableType, object poco, string tableName, string setColumns, string notsetColumns, string whereCondition, object[] whereConditionArgs, Func<string, string> addBoundary)
            {
                if (poco == null)
                    throw new Exception("Update的poco对象不能为空");
                if (tableType == null)
                    tableType = poco.GetType();

                var att = MetaHelper.GetTableAttribute(tableType);
                if (string.IsNullOrWhiteSpace(tableName))
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                string updatesql = string.Format("UPDATE {0} SET ", addBoundary == null ? tableName : addBoundary(tableName));

                Meta pocoMeta = new Meta(poco.GetType());
                List<object> listValues = new List<object>();
                Foreach(setColumns, tableType, notsetColumns, att.AutoIncrementColumn, (column, i) =>
                {
                    updatesql += string.Format("{0}=@{1},", addBoundary == null ? column : addBoundary(column), i);
                    listValues.Add(pocoMeta.GetValue(poco, column));
                });

                Sql sql = new Sql(updatesql.TrimEnd(','), listValues.ToArray());
                if (!string.IsNullOrWhiteSpace(whereCondition))
                    sql.Where(whereCondition, whereConditionArgs);
                return sql;
            }
            public static Sql Update(Type tableType, object poco, string tableName, string setColumns, string notsetColumns, string whereColumns, Func<string, string> addBoundary)
            {
                if (poco == null)
                    throw new Exception("Update的poco对象不能为空");
                if (tableType == null)
                    tableType = poco.GetType();

                var att = MetaHelper.GetTableAttribute(tableType);
                if (string.IsNullOrWhiteSpace(tableName))
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                string updatesql = string.Format("UPDATE {0} SET ", addBoundary == null ? tableName : addBoundary(tableName));

                Meta pocoMeta = new Meta(poco.GetType());
                List<object> listValues = new List<object>();
                Foreach(setColumns, tableType, notsetColumns, att.AutoIncrementColumn, (column, i) =>
                {
                    updatesql += string.Format("{0}=@{1},", addBoundary == null ? column : addBoundary(column), i);
                    listValues.Add(pocoMeta.GetValue(poco, column));
                });

                Sql sql = new Sql(updatesql.TrimEnd(','), listValues.ToArray());
                if (!string.IsNullOrWhiteSpace(whereColumns))
                {
                    string[] listColumns = whereColumns.Split(',');
                    foreach (var column in listColumns)
                    {
                        sql.Where(addBoundary == null ? column : addBoundary(column) + "=@0", pocoMeta.GetValue(poco, column));
                    }
                }

                return sql;
            }
            public static Sql Updates(Type tableType, IEnumerable pocos, string tableName, string setColumns, string notsetColumns, string whereColumns, Func<string, string> addBoundary)
            {
                if (pocos == null)
                    throw new Exception("Updates的pocos集合不能为空");
                if (tableType == null)
                    tableType = pocos.AsQueryable().ElementType;

                var att = MetaHelper.GetTableAttribute(tableType);
                if (string.IsNullOrWhiteSpace(tableName))
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                string updatesql = string.Format("UPDATE {0} SET ", addBoundary == null ? tableName : addBoundary(tableName));

                Meta pocoMeta = new Meta(pocos.AsQueryable().ElementType);

                Sql sql = Sql.NewSql;
                foreach (object poco in pocos)
                {
                    string onesqlstr = updatesql;
                    List<object> listValues = new List<object>();
                    Foreach(setColumns, tableType, notsetColumns, att.AutoIncrementColumn, (column, i) =>
                    {
                        onesqlstr += string.Format("{0}=@{1},", addBoundary == null ? column : addBoundary(column), i);
                        listValues.Add(pocoMeta.GetValue(poco, column));
                    });

                    Sql onesql = new Sql(onesqlstr.TrimEnd(','), listValues.ToArray());
                    if (!string.IsNullOrWhiteSpace(whereColumns))
                    {
                        string[] listColumns = whereColumns.Split(',');
                        foreach (var column in listColumns)
                        {
                            onesql.Where(addBoundary == null ? column : addBoundary(column) + "=@0", pocoMeta.GetValue(poco, column));
                        }
                    }
                    sql.Append(onesql);
                }
                return sql;
            }
            #endregion
            #region Save
            public static object GetAutoIncrementValue(object poco, ref string autoIncrementColumn)
            {
                if (poco == null)
                    throw new Exception("SaveByAutoIncrement的poco对象不能为空");

                Type pocoType = poco.GetType();
                if (string.IsNullOrWhiteSpace(autoIncrementColumn))
                {
                    var att = MetaHelper.GetTableAttribute(pocoType);
                    if (!string.IsNullOrWhiteSpace(att.AutoIncrementColumn))
                        autoIncrementColumn = att.AutoIncrementColumn;
                }
                if (string.IsNullOrWhiteSpace(autoIncrementColumn))
                    throw new Exception("SaveByAutoIncrement方法必须指定autoIncrementColumn列");

                PropertyInfo p = pocoType.GetProperty(autoIncrementColumn);
                if (p == null)
                    throw new Exception(string.Format("{0} 类型不存在这样的属性 {1}", pocoType.Name, autoIncrementColumn));
                return p.GetValue(poco, null);
            }
            public static Sql SavesByAutoIncrement(Type tableType, IEnumerable pocos, string tableName, string includeColumns, string excludeColumns, string autoIncrementColumn, Func<string, string> addBoundary)
            {
                if (pocos == null)
                    throw new Exception("SavesByAutoIncrement的pocos集合不能为空");

                if (tableType == null)
                    tableType = pocos.AsQueryable().ElementType;
                if (string.IsNullOrWhiteSpace(autoIncrementColumn))
                {
                    var att = MetaHelper.GetTableAttribute(tableType);
                    if (!string.IsNullOrWhiteSpace(att.AutoIncrementColumn))
                        autoIncrementColumn = att.AutoIncrementColumn;
                }
                if (string.IsNullOrWhiteSpace(autoIncrementColumn))
                    throw new Exception("SaveByAutoIncrement方法必须指定autoIncrementColumn列");

                Type pocoType = pocos.AsQueryable().ElementType;
                PropertyInfo p = pocoType.GetProperty(autoIncrementColumn);
                if (p == null)
                    throw new Exception(string.Format("{0} 类型不存在这样的属性 {1}", pocoType.Name, autoIncrementColumn));

                excludeColumns = string.IsNullOrWhiteSpace(excludeColumns) ? autoIncrementColumn : excludeColumns + "," + autoIncrementColumn;
                Sql sql = Sql.NewSql;
                foreach (object poco in pocos)
                {
                    if (p.GetValue(poco, null).IsNull())
                        sql.Append(Insert(tableType, poco, tableName, includeColumns, excludeColumns, addBoundary));
                    else
                        sql.Append(Update(tableType, poco, tableName, includeColumns, excludeColumns, autoIncrementColumn, addBoundary));
                }
                return sql;
            }
            public static Sql Exists(Type tableType, object poco, string tableName, string existsColumns, Func<string, string> addBoundary)
            {
                if (poco == null)
                    throw new Exception("SaveByExists方法的poco参数不能为空");
                if (string.IsNullOrWhiteSpace(existsColumns))
                    throw new Exception("SaveByExists方法的existsColumns参数不能为空");
                if (tableType == null)
                    tableType = poco.GetType();
                if (string.IsNullOrWhiteSpace(tableName))
                {
                    var att = MetaHelper.GetTableAttribute(tableType);
                    tableName = string.IsNullOrWhiteSpace(att.Name) ? tableType.Name : att.Name;
                }
                string existssql = string.Format("SELECT 1 FROM {0}", addBoundary == null ? tableName : addBoundary(tableName));
                Sql sql = new Sql(existssql);

                Type pocoType = poco.GetType();
                Meta pocoMeta = new Meta(pocoType);
                string[] listColumns = existsColumns.Split(',');
                foreach (var column in listColumns)
                {
                    sql.Where(addBoundary == null ? column : addBoundary(column) + "=@0", pocoMeta.GetValue(poco, column));
                }
                return sql;
            }
            public static Sql SavesByExists(Type tableType, IEnumerable pocos, string tableName, string includeColumns, string excludeColumns, string existsColumns, Func<string, string> addBoundary, Func<Sql, bool> existsFunc)
            {
                if (pocos == null)
                    throw new Exception("SavesByExists的pocos集合不能为空");
                if (string.IsNullOrWhiteSpace(existsColumns))
                    throw new Exception("SavesByExists方法的existsColumns参数不能为空");

                Sql sql = Sql.NewSql;
                foreach (object poco in pocos)
                {
                    if (existsFunc(Exists(tableType, poco, tableName, existsColumns, addBoundary)))
                        sql.Append(Update(tableType, poco, tableName, includeColumns, excludeColumns, existsColumns, addBoundary));
                    else
                        sql.Append(Insert(tableType, poco, tableName, includeColumns, excludeColumns, addBoundary));
                }
                return sql;
            }
            #endregion
        }
        #endregion
        #region MetaHelper
        static class MetaHelper
        {
            static Type EXPANDOTYPE = typeof(System.Dynamic.ExpandoObject);
            static Type IGNORETYPE = typeof(IgnoreAttribute);
            static Type COLUMNTYPE = typeof(ColumnAttribute);
            static Type TABLETYPE = typeof(TableAttribute);

            static Type BOOLTYPE = typeof(bool);
            static Type UINTTYPE = typeof(uint);
            static Type STRINGTYPE = typeof(string);
            static Type GUIDTYPE = typeof(Guid);
            static Type ANSISTRINGTYPE = typeof(AnsiString);
            #region RemoveBoundary
            /// <summary>
            /// 数据库名称分界符（[],``,""）
            /// </summary>
            static Regex regexBoundary = new Regex(@"[\[\]`""]", RegexOptions.Compiled);
            /// <summary>
            /// 去除数据库分界符，并且空格替换为_（[],``,"",空格）
            /// </summary>
            /// <param name="dbname"></param>
            /// <returns></returns>
            public static string RemoveBoundary(string dbname)
            {
                return regexBoundary.Replace(dbname.Replace(' ', '_'), string.Empty);
            }
            #endregion

            public static bool IsBool(Type type)
            {
                return type == BOOLTYPE;
            }
            public static bool IsUint(Type type)
            {
                return type == UINTTYPE;
            }
            public static bool IsString(Type type)
            {
                return type == STRINGTYPE;
            }
            public static bool IsGuid(Type type)
            {
                return type == GUIDTYPE;
            }
            public static bool IsAnsiString(Type type)
            {
                return type == ANSISTRINGTYPE;
            }

            /// <summary>
            /// 分析Type的类型，若是枚举或可空类型，则返回UnderlyingType。
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static Tuple<Type, MetaType> GetMetaType(Type type)
            {
                Type t = type;
                MetaType mt;
                if (type.IsEnum)
                {
                    mt = MetaType.Enum;
                    t = type.GetEnumUnderlyingType();
                }
                else if (type == EXPANDOTYPE)
                {
                    mt = MetaType.Expando;
                }
                else if (type.IsArray)
                {
                    mt = MetaType.Array;
                }
                else if (type.Name.StartsWith("<>f__AnonymousType"))
                {
                    mt = MetaType.Anonymous;
                }
                else
                {
                    var undertype = Nullable.GetUnderlyingType(type);
                    if (undertype != null)
                    {
                        mt = MetaType.Nullable;
                        t = undertype;
                    }
                    else
                    {
                        var code = Type.GetTypeCode(type);
                        switch (code)
                        {
                            case TypeCode.Object:
                                mt = MetaType.Poco;
                                break;
                            default:
                                mt = MetaType.BaseType;
                                break;
                        }
                    }
                }
                return new Tuple<Type, MetaType>(t, mt);
            }
            /// <summary>
            /// 获取指定类型的所有未标记IgnoreAttribute的属性，键名使用ColumnAttribute标记的名称。
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static Dictionary<string, PropertyInfo> CreatePropertiesDict(Type type)
            {
                var properties = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);
                string pname;
                foreach (var p in type.GetProperties())
                {
                    if (p.GetCustomAttributes(IGNORETYPE, false).Length == 0)
                    {
                        var a = p.GetCustomAttributes(COLUMNTYPE, false);
                        pname = a.Length > 0 ? (a[0] as ColumnAttribute).Name : p.Name;
                        properties.Add(pname, p);
                    }
                }
                return properties;
            }
            /// <summary>
            /// 创建Reader的索引、列名字典
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public static Dictionary<int, string> CreateReaderColumns(IDataReader r)
            {
                var dict = new Dictionary<int, string>();
                for (int i = 0; i < r.FieldCount; i++)
                {
                    dict.Add(i, RemoveBoundary(r.GetName(i)));
                }
                return dict;
            }
            /// <summary>
            /// 创建Reader的索引到Type属性之间映射的字典
            /// </summary>
            /// <param name="r"></param>
            /// <param name="type"></param>
            /// <returns></returns>
            public static Dictionary<int, MetaColumn> CreateReaderPocoMapper(IDataReader r, Type type)
            {
                var dict = new Dictionary<int, MetaColumn>();
                var properties = CreatePropertiesDict(type);
                for (int i = 0; i < r.FieldCount; i++)
                {
                    string name = RemoveBoundary(r.GetName(i));
                    PropertyInfo p = null;
                    properties.TryGetValue(name, out p);
                    dict.Add(i, new MetaColumn(name, p));
                }
                return dict;
            }
            /// <summary>
            /// 获取指定类型的TableAttribute特性
            /// </summary>
            /// <param name="type"></param>
            /// <returns></returns>
            public static TableAttribute GetTableAttribute(Type type)
            {
                var atts = type.GetCustomAttributes(TABLETYPE, false);
                if (atts == null | atts.Length < 1)
                    return new TableAttribute();
                return atts[0] as TableAttribute;
            }
        }
        #endregion
        #region Meta
        class Meta
        {
            Type _type;
            Type _undertype;
            MetaType _metatype;
            Dictionary<int, MetaColumn> _ReaderPocoMapper;
            Dictionary<int, string> _ReaderColumns;

            public Type Type
            {
                get
                {
                    return _type;
                }
            }
            public Type UnderType
            {
                get
                {
                    return _undertype;
                }
            }
            public MetaType MetaType
            {
                get
                {
                    return _metatype;
                }
            }
            public Meta(Type type)
            {
                _type = type;
                var tup = MetaHelper.GetMetaType(type);
                _undertype = tup.Item1;
                _metatype = tup.Item2;
            }

            /// <summary>
            /// 将对象转成当前类型，若类型相同直接返回原对象，否则返回新对象。
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public object Convert(object other)
            {
                //处理空值
                if (other == null || System.Convert.IsDBNull(other))
                    return _type.DefaultValue();

                //如果类型相同直接返回原对象
                Type othertype = other.GetType();
                if (_type == othertype)
                    return other;

                Meta o = new Meta(othertype);
                switch (_metatype)
                {
                    case MetaType.BaseType:
                    case MetaType.Nullable:
                    case MetaType.Enum:
                        switch (o.MetaType)
                        {
                            case MetaType.BaseType:
                            case MetaType.Nullable:
                            case MetaType.Enum:
                                return System.Convert.ChangeType(other, _undertype);
                            default:
                                throw new Exception(string.Format("`{0}` 类型不能转换为 `{1}` 类型", othertype.FullName, _type.FullName));
                        }
                    case MetaType.Array:
                        //数组必须原和目标类型完全相同，不作转换。
                        throw new Exception(string.Format("`{0}` 类型不能转换为 `{1}` 类型", othertype.FullName, _type.FullName));
                    case MetaType.Expando:
                        switch (o.MetaType)
                        {
                            case MetaType.BaseType:
                            case MetaType.Nullable:
                            case MetaType.Enum:
                            case MetaType.Array:
                                throw new Exception(string.Format("`{0}` 类型不能转换为 `{1}` 类型", othertype.FullName, _type.FullName));
                            default:
                                var dstObject = Activator.CreateInstance(_type);
                                var dict = dstObject as IDictionary<string, Object>;
                                var srcPropties = othertype.GetProperties();
                                foreach (var srcPropty in srcPropties)
                                {
                                    dict[srcPropty.Name] = srcPropty.GetValue(other, null);
                                }
                                return dstObject;
                        }
                    case MetaType.Anonymous:
                        switch (o.MetaType)
                        {
                            case MetaType.BaseType:
                            case MetaType.Nullable:
                            case MetaType.Enum:
                            case MetaType.Array:
                                throw new Exception(string.Format("`{0}` 类型不能转换为 `{1}` 类型", othertype.FullName, _type.FullName));
                            default:
                                var dstPropties = _type.GetProperties();
                                var srcPropties = othertype.GetProperties();
                                List<object> list = new List<object>();
                                PropertyInfo p;
                                foreach (var dstPropty in dstPropties)
                                {
                                    p = null;
                                    foreach (var srcPropty in srcPropties)
                                    {
                                        if (string.Compare(dstPropty.Name, srcPropty.Name, true) == 0)
                                            p = srcPropty;
                                    }

                                    list.Add(p == null ? dstPropty.PropertyType.DefaultValue() : new Meta(dstPropty.PropertyType).Convert(p.GetValue(other, null)));
                                }
                                return Activator.CreateInstance(_type, list.ToArray()); ;
                        }
                    default:
                        //全部以Poco类型处理
                        switch (o.MetaType)
                        {
                            case MetaType.BaseType:
                            case MetaType.Nullable:
                            case MetaType.Enum:
                            case MetaType.Array:
                                throw new Exception(string.Format("`{0}` 类型不能转换为 `{1}` 类型", othertype.FullName, _type.FullName));
                            default:
                                var dstObject = Activator.CreateInstance(_type);
                                var dstPropties = _type.GetProperties();
                                var srcPropties = othertype.GetProperties();
                                foreach (var srcPropty in srcPropties)
                                {
                                    foreach (var dstPropty in dstPropties)
                                    {
                                        if (string.Compare(dstPropty.Name, srcPropty.Name, true) == 0)
                                            dstPropty.SetValue(dstObject, new Meta(dstPropty.PropertyType).Convert(srcPropty.GetValue(other, null)), null);//递归处理属性
                                    }
                                }
                                return dstObject;
                        }
                }
            }
            /// <summary>
            /// 将 IDataReader 转成 Poco 对象
            /// </summary>
            /// <param name="r"></param>
            /// <returns></returns>
            public object Convert(IDataReader r)
            {
                if (_metatype == MetaType.Poco)
                {
                    if (_ReaderPocoMapper == null)
                        _ReaderPocoMapper = MetaHelper.CreateReaderPocoMapper(r, _undertype);

                    object o = Activator.CreateInstance(_undertype);
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        _ReaderPocoMapper[i].SetValue(o, r[i]);
                    }
                    return o;
                }
                else if (_metatype == MetaType.Expando)
                {
                    if (_ReaderColumns == null)
                        _ReaderColumns = MetaHelper.CreateReaderColumns(r);

                    dynamic o = new System.Dynamic.ExpandoObject();
                    for (int i = 0; i < r.FieldCount; i++)
                    {
                        o[_ReaderColumns[i]] = r[i];
                    }
                    return o;
                }
                else
                {
                    return Convert(r[0]);
                }
            }
            /// <summary>
            /// 将src对象的属性复制给dst对象。
            /// </summary>
            /// <param name="src"></param>
            /// <param name="dst"></param>
            public void Copy(object src, object dst)
            {
                if (src == null)
                    throw new ArgumentNullException("src");
                if (dst == null)
                    throw new ArgumentNullException("dst");

                Type dsttype = dst.GetType();
                Meta o = new Meta(dsttype);
                switch (_metatype)
                {
                    case MetaType.BaseType:
                    case MetaType.Nullable:
                    case MetaType.Enum:
                    case MetaType.Array:
                        throw new Exception(string.Format("`{0}` 非对象类型不支持属性复制到 `{1}`", _type.FullName, dsttype.FullName));
                    default:
                        //原类型是对象类型
                        switch (o.MetaType)
                        {
                            case MetaType.BaseType:
                            case MetaType.Nullable:
                            case MetaType.Enum:
                            case MetaType.Array:
                                throw new Exception(string.Format("`{0}` 非对象类型不支持属性复制到 `{1}`", _type.FullName, dsttype.FullName));
                            case MetaType.Expando:
                                {
                                    //目标类型是Expando类型
                                    var srcPropties = _type.GetProperties();
                                    var dict = dst as IDictionary<string, Object>;
                                    foreach (var srcPropty in srcPropties)
                                    {
                                        dict[srcPropty.Name] = srcPropty.GetValue(src, null);
                                    }
                                    break;
                                }
                            default:
                                {
                                    //目标类型是Poco,Anonymous类型
                                    var srcPropties = _type.GetProperties();
                                    var dstPropties = dsttype.GetProperties();
                                    foreach (var srcPropty in srcPropties)
                                    {
                                        foreach (var dstPropty in dstPropties)
                                        {
                                            if (string.Compare(dstPropty.Name, srcPropty.Name, true) == 0)
                                                dstPropty.SetValue(dst, new Meta(dstPropty.PropertyType).Convert(srcPropty.GetValue(src, null)), null);//递归处理属性
                                        }
                                    }
                                    break;
                                }
                        }
                        break;
                }
            }
            /// <summary>
            /// 获取对象的指定属性值。若为基础类型则直接返回该对象。
            /// </summary>
            /// <param name="poco"></param>
            /// <param name="propertyName"></param>
            /// <returns></returns>
            public object GetValue(object poco, string propertyName)
            {
                if (string.IsNullOrWhiteSpace(propertyName) || poco == null || System.Convert.IsDBNull(poco))
                    return poco;

                switch (_metatype)
                {
                    case MetaType.BaseType:
                    case MetaType.Nullable:
                    case MetaType.Enum:
                    case MetaType.Array:
                        return poco;
                    default:
                        return _type.GetProperty(propertyName).GetValue(poco, null);
                }
            }
        }
        #endregion
        #region MetaColumn
        class MetaColumn
        {
            public string Name;
            public PropertyInfo PropertyInfo;
            Meta Meta;

            public MetaColumn(string name, PropertyInfo p)
            {
                Name = name;
                if (p != null)
                {
                    PropertyInfo = p;
                    Meta = new Meta(p.PropertyType);
                }
            }

            /// <summary>
            /// 设置对象poco的属性，对值提供类型转换功能。
            /// </summary>
            /// <param name="poco"></param>
            /// <param name="val"></param>
            public void SetValue(object poco, object val)
            {
                if (PropertyInfo != null)
                {
                    PropertyInfo.SetValue(poco, Meta.Convert(val), null);
                }
            }
        }
        #endregion
        #region MetaType
        enum MetaType
        {
            /// <summary>
            /// 属性对应字段
            /// </summary>
            Poco,
            /// <summary>
            /// 动态类型 IDictionary&lt;string, Object&gt;
            /// </summary>
            Expando,
            /// <summary>
            /// 匿名类型 &lt;&gt;f__AnonymousType{index}`{count}   
            /// index:当前第几个匿名类型；count:属性个数；
            /// </summary>
            Anonymous,
            /// <summary>
            /// 可空类型
            /// </summary>
            Nullable,
            /// <summary>
            /// 枚举类型
            /// </summary>
            Enum,
            /// <summary>
            /// 数组
            /// </summary>
            Array,
            /// <summary>
            /// 基本类型
            /// </summary>
            BaseType,
        }
        #endregion
        #region Cache
        /// <summary>
        /// 多线程支持的字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        //class Cache<TKey, TValue>
        //{
        //    Dictionary<TKey, TValue> _map = new Dictionary<TKey, TValue>();
        //    ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        //    /// <summary>
        //    /// 获取指定项，若存在返回，否则创建一个新项返回。
        //    /// </summary>
        //    /// <param name="key">键</param>
        //    /// <param name="factory">项的创建工厂</param>
        //    /// <returns></returns>
        //    public TValue Get(TKey key, Func<TValue> factory)
        //    {
        //        // 读锁
        //        _lock.EnterReadLock();
        //        TValue val;
        //        try
        //        {
        //            if (_map.TryGetValue(key, out val))
        //                return val;
        //        }
        //        finally
        //        {
        //            _lock.ExitReadLock();
        //        }


        //        // 写锁
        //        _lock.EnterWriteLock();
        //        try
        //        {
        //            if (_map.TryGetValue(key, out val))
        //                return val;

        //            val = factory();
        //            _map.Add(key, val);
        //            return val;
        //        }
        //        finally
        //        {
        //            _lock.ExitWriteLock();
        //        }
        //    }
        //    /// <summary>
        //    /// 当前项数
        //    /// </summary>
        //    public int Count
        //    {
        //        get
        //        {
        //            return _map.Count;
        //        }
        //    }
        //    /// <summary>
        //    /// 清除所有项
        //    /// </summary>
        //    public void Clear()
        //    {
        //        _lock.EnterWriteLock();
        //        try
        //        {
        //            _map.Clear();
        //        }
        //        finally
        //        {
        //            _lock.ExitWriteLock();
        //        }
        //    }
        //}
        #endregion
    }
    #endregion
}
