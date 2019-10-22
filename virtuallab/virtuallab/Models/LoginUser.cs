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

    public delegate void StateChanged(object sender, EventArgs e);

    public class LoginUser
    {
        public int type;
        public int userId;
        public string alias;
        public string name;
        public int gender;
        public string grade;
        public string belong;
        public string phone;
        public string email;
        public string password;

        public string currentExperimentId;
        public string currentTaskId;
        public string currentSessionId;
        public string currentCompileId;
        public string currentUploadId;
        public string currentRunId;
        public string currentCodeUri;

        public bool compileSuccess;
        public bool uploadSuccess;
        public bool playSuccess;

        public EnvironmentState _currentState;
        public EnvironmentState currentState
        {
            get { return _currentState; }
            set
            {
                _currentState = value;
                StateChangedEvent(this, EventArgs.Empty);
            }
        }
        public event StateChanged StateChangedEvent;
    }
}