using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace virtuallab.Common.po
{
    public partial class bh_view_manager_reports
    {
        public int id_experiment { get; set; }
        public string title { get; set; }
        public int id_task { get; set; }
        public int score { get; set; }
        public int id_student { get; set; }
        public string name { get; set; }
        public short complete { get; set; }
        public string id_record { get; set; }
        public DateTime finish_date { get; set; }
        public string result_json_uri { get; set; }
        public string final_code_uri { get; set; }
    }
    public partial class bh_view_student_tasks
    {
        public int id_task { get; set; }
        public int fid_experiment { get; set; }
        public int fid_student { get; set; }
        public short complete { get; set; }
        public int score { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string memo { get; set; }
        public string template_uri { get; set; }
        public string rjson_uri { get; set; }
    }

    public partial class bhExperiment
    {
        public int id_experiment { get; set; }
        public string title { get; set; }
        public string template_uri { get; set; }
        public string rjson_uri { get; set; }
        public string memo { get; set; }
        public DateTime create_date { get; set; }
        public DateTime update_date { get; set; }
        public DateTime delete_date { get; set; }
        public short record_status { get; set; }
    }
    public partial class bhManager
    {
        public int id_manager { get; set; }
        public string alias { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
    public partial class bhRecord
    {
        public string id_record { get; set; }
        public int fid_task { get; set; }
        public int submit_times { get; set; }
        public string final_code_uri { get; set; }
        public string result_json_uri { get; set; }
        public DateTime finish_date { get; set; }
        public string score { get; set; }
        public short is_result { get; set; }
    }
    public class bhCode
    {
        public int id_code { get; set; }
        public string filename { get; set; }
        public string filecontent { get; set; }

        public string active { get; set; }
    }
    public partial class bhStudent
    {
        public int id_student { get; set; }
        public string alias { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public short gender { get; set; }
        public string grade { get; set; }
        public string belong { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public short record_status { get; set; }
    }
    public partial class bhTask
    {
        public int id_task { get; set; }
        public int fid_experiment { get; set; }
        public int fid_student { get; set; }
        public int fid_manager { get; set; }
        public int score { get; set; }
        public short complete { get; set; }
        public string complete_code { get; set; }
    }
    public partial class sysdiagrams
    {
        public string name { get; set; }
        public int principal_id { get; set; }
        public int diagram_id { get; set; }
        public int version { get; set; }
        public byte[] definition { get; set; }
    }

}