using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_template
{
    public class Schedule
    {
        public string education_group_name { get; set; }
        public string education_group_id { get; set; }
        public string day_number { get; set; }
        public string lesson_number { get; set; }
        public string place { get; set; }
        public string subgroup { get; set; }
        public string teacher_id { get; set; }
        public string subject { get; set; }
        public string type { get; set; }
        public string week_type { get; set; }
    }

    public class SSchedule
    {
        public Schedule schedule { get; set; }
    }

    public class MainSchedule
    {
        public string id_schedule { get; set; }
        public string education_group_name { get; set; }
        public string education_group_id { get; set; }
        public string day_number { get; set; }
        public string lesson_number { get; set; }
        public string place { get; set; }
        public string subgroup { get; set; }
        public string teacher_id { get; set; }
       // public teacher teacher { get; set; }
        public string subject { get; set; }
        public string type { get; set; }
        public string week_type { get; set; }
    }

    public class MainScheduleL
    {
        public IEnumerable<MainSchedule> schedules { get; set; }
    }

    //public class teacher
    //{
    //    public string teacher_id { get; set; }
    //    public string surname { get; set; }
    //    public string first_name { get; set; }
    //    public string middle_name { get; set; }
    //    public string photo { get; set; }
    //    public string work_place_number { get; set; }
    //    public string work_phone_number { get; set; }
    //    public string work_phone_city_number { get; set; }
    //}
}
