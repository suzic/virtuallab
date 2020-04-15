using Pocoor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using virtuallab.API.po;
using virtuallab.API.Service.po;
using virtuallab.Common.po;

namespace virtuallab.Common
{
    public class DB
    {
        static Database db
        {
            get
            {
                Database _db = new Database("ConnectionString");
                //db.CommandTimeout = 300;
                return _db;
            }
        }

        public static string GetTemplateIds(int id_experiment)
        {
            string sql = "SELECT template_uri FROM bhExperiment where id_experiment=@0";
            return db.FirstOrDefault<string>(sql, (object)id_experiment);
        }
        public static string GetHelpUrl(int id_experiment)
        {
            string sql = "SELECT rjson_uri FROM bhExperiment where id_experiment=@0";
            return db.FirstOrDefault<string>(sql, (object)id_experiment);
        }

        public static int GetExpType(int id_experiment)
        {
            string sql = "SELECT [type] FROM bhExperiment where id_experiment=@0";
            return db.FirstOrDefault<int>(sql, (object)id_experiment);
        }

        public static List<bhCode> GetCodes(int id_experiment)
        {
            string ids= GetTemplateIds(id_experiment);
            string sql = "select * from bhCode where id_code in ({0});";
            sql = string.Format(sql, ids);
            return db.Fetch<bhCode>(sql);
        }
        public static List<bhCode> GetStudentCodes(int id_task)
        {
            string ids = GetTaskCodeIds(id_task);
            string sql = "select * from bhCode where id_code in ({0});";
            sql = string.Format(sql, ids);
            return db.Fetch<bhCode>(sql);
        }
        public static string GetTaskCodeIds(int id_task)
        {
            string sql = "select final_code_uri from bhRecord where fid_task=@0 and is_result=1";
            return db.FirstOrDefault<string>(sql, (object)id_task);
        }

        #region SaveRecordInfo
        public static void SaveRecordInfo(ControllerCodeSubmitReq req)
        {
            //重置过去实验为未完成
            ResetdRecordResult0(req.fid_task);

            //插入或更新bhRecord表
            bhRecord r = GetRecord(req.session_id);
            if (r == null || string.IsNullOrWhiteSpace(r.id_record))
            {
                //插入bhRecord表
                r = new bhRecord();
                r.id_record = req.session_id;
                r.fid_task = req.fid_task;
                r.submit_times = 1;
                //插入bhCode表
                r.final_code_uri = InsertCodes(req.code);
                r.result_json_uri = "result_json_uri";
                r.finish_date = DateTime.Now;
                r.score = "0";
                r.is_result = 1;
                db.Insert(r, "bhRecord");
            }
            else {
                //更新bhCode表
                UpdateCodes(req.code, r.final_code_uri);

                //更新bhRecord表
                r.submit_times += 1;
                r.finish_date = DateTime.Now;
                db.Update(r, "bhRecord", "submit_times,finish_date", null, "id_record");
            }

            //设置任务为完成
            SetTaskComplete(req.fid_task);
        }
        static bhRecord GetRecord(string session_id)
        {
            string sql = "select * from bhRecord where id_record=@0";
            return db.FirstOrDefault<bhRecord>(sql, (object)session_id);
        }
        static void ResetdRecordResult0(int fid_task)
        {
            string sql = "UPDATE bhRecord SET is_result = 0 WHERE fid_task=@0 and is_result=1";
            db.Execute(sql, (object)fid_task);
        }
        static string GetSessionId(int fid_task)
        {
            string sql = "SELECT id_record FROM [bhRecord] WHERE (fid_task = @0 and is_result = 1)";
            return db.FirstOrDefault<string>(sql, fid_task);
        }
        static void SetTaskComplete(int fid_task)
        {
            string sql = "UPDATE bhTask SET complete = 1 WHERE id_task = @0";
            db.Execute(sql, fid_task);
        }


        static string InsertCodes(List<CodeFile> codes)
        {
            if (codes == null || codes.Count < 1)
                return null;

            StringBuilder sb = new StringBuilder();
            foreach (CodeFile code in codes)
            {
                sb.Append(InsertCode(code.filename, code.content));
                sb.Append(",");
            }
            return sb.ToString().TrimEnd(',');
        }
        static int InsertCode(string filename, string filecontent)
        {
            bhCode e = new bhCode();
            e.filename = filename;
            e.filecontent = filecontent;
            int id = Convert.ToInt32(db.Insert(e, "bhCode", "filename,filecontent", null, "id_code"));
            return id;
        }
        static void UpdateCodes(List<CodeFile> codes,string ids)
        {
            string sql = "select id_code,[filename] from bhCode where id_code in ({0});";
            sql = string.Format(sql, ids);
            List<bhCode> list = db.Fetch<bhCode>(sql);
            foreach (bhCode bh in list)
            {
                UpdateCode(bh.id_code, GetCodeContent(codes,bh.filename));
            }
        }
        static string GetCodeContent(List<CodeFile> codes, string filename)
        {
            foreach (CodeFile f in codes)
            {
                if (filename.Equals(f.filename, StringComparison.OrdinalIgnoreCase))
                    return f.content;
            }
            return null;
        }
        static void UpdateCode(int id_code, string content)
        {
            string sql = "update bhCode set filecontent=@1 where id_code=@0";
            db.Execute(sql, id_code, content);
        }
        #endregion


    }
}