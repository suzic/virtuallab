using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace virtuallab.Models
{
    /// <summary>
    /// 状态枚举值
    /// </summary>
    public enum EnvironmentState
    {
        NotReady = 0,
        InEditing,
        InCompiling,
        InUploading,
        InPlaying
    }

    /// <summary>
    /// 该代理用于声明状态机中变化的事件标准模型（暂时未启用）
    /// </summary>
    /// <param name="sender">事件发出对象</param>
    /// <param name="e">事件参数</param>
    public delegate void StateChanged(object sender, EventArgs e);

    /// <summary>
    /// 登录用户数据模型
    /// </summary>
    public class LoginUser
    {
        public int type;        // 用户类型，是学生还是管理员
        public int userId;      // 用户ID
        public string alias;    // 用户登录名
        public string name;     // 用户姓名
        public int gender;      // 用户性别
        public string grade;    // 用户级别信息
        public string belong;   // 用户来源信息
        public string phone;    // 用户联系电话
        public string email;    // 用户电子邮件
        public string password; // 用户密码

        // 当前的实验相关ID信息
        public string currentExperimentId;  // 当前执行的实验ID
        public string currentTaskId;        // 当前执行实验的具体任务ID
        public string currentSessionId;     // 当前执行实验任务的具体会话ID
        public string currentCompileId;     // 当前进行编译的标识ID
        public string currentUploadId;      // 当前进行上传操作的标识ID
        public string currentRunId;         // 当前进行运行程序的标识ID
        public string currentCodeUri;       // 当前代码存储的地址
        public string device_id;             // 设备ID
        public string app_name;             // 生成的程序名
        public string ssh_uuid;
        public int exp_type;             // 

        public bool compileSuccess;     // Flag信息：标记编译是否成功
        public bool uploadSuccess;      // Flag信息：标记上传是否成功
        public bool playSuccess;        // Flag信息：标记运行是否成功

        public int InError;
        public int InWaiting;

        /// <summary>
        /// 用户所处状态。在状态改变时，触发对应事件。
        /// </summary>
        public EnvironmentState _currentState;
        public EnvironmentState currentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                if(StateChangedEvent!=null)
                    StateChangedEvent(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 用户状态改变对应的事件
        /// </summary>
        public event StateChanged StateChangedEvent;
    }
}